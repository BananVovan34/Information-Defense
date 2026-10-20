using System.Security.Cryptography;
using System.Text;
using Lab2Encryption;

string root = Path.GetFullPath(args[0]);
Directory.CreateDirectory(root);
string run = Path.Combine(root, DateTime.UtcNow.ToString("yyyyMMdd-HHmmss") + "-" + Guid.NewGuid().ToString("N")[..6]);
Directory.CreateDirectory(run);
string password = "Учебный пароль AES-192!";
var results = new List<string>();
string NewPath(string name) => Path.Combine(run, name);

void Check(bool condition, string description)
{
    if (!condition) throw new Exception("FAIL: " + description);
    results.Add("PASS: " + description);
    Console.WriteLine(results[^1]);
}

void Reject<T>(Action action, string output, string description) where T : Exception
{
    bool rejected = false;
    try { action(); }
    catch (T) { rejected = true; }
    Check(rejected && !File.Exists(output), description);
}

foreach (int size in new[] { 32, 33, 47, 48, 49, 4 * 1024 * 1024 })
{
    string input = NewPath($"size-{size}.txt");
    string encrypted = input + ".aes192";
    string restored = input + ".restored";
    byte[] content = RandomNumberGenerator.GetBytes(size);
    File.WriteAllBytes(input, content);
    var encryption = FileEncryptionService.EncryptFile(input, encrypted, password);
    var decryption = FileEncryptionService.DecryptFile(encrypted, restored, password);
    Check(content.SequenceEqual(File.ReadAllBytes(restored)) && encryption.InputBytes == size &&
        decryption.OutputBytes == size, $"Exact round trip for {size} bytes");
}

string sample = NewPath("unicode.txt");
byte[] text = Encoding.UTF8.GetBytes("Кириллица, emoji 🔐, 0123456789.\r\nВторая строка.\nТретья строка.\r\n");
File.WriteAllBytes(sample, text);
string cipher = NewPath("unicode.aes192");
string restoredSample = NewPath("unicode-restored.txt");
FileEncryptionService.EncryptFile(sample, cipher, password);
FileEncryptionService.DecryptFile(cipher, restoredSample, password);
Check(text.SequenceEqual(File.ReadAllBytes(restoredSample)), "Unicode, emoji and mixed line endings preserved");

string cipher2 = NewPath("unicode-2.aes192");
FileEncryptionService.EncryptFile(sample, cipher2, password);
byte[] encryptedBytes = File.ReadAllBytes(cipher);
byte[] encryptedBytes2 = File.ReadAllBytes(cipher2);
Check(!encryptedBytes.AsSpan(9, 16).SequenceEqual(encryptedBytes2.AsSpan(9, 16)) &&
    !encryptedBytes.AsSpan(25, 16).SequenceEqual(encryptedBytes2.AsSpan(25, 16)),
    "New salt and IV for repeated encryption");

// Read the format independently and verify the actual key size and authentication tag.
byte[] keys = Rfc2898DeriveBytes.Pbkdf2(password, encryptedBytes.AsSpan(9, 16), 210_000,
    HashAlgorithmName.SHA256, 56);
byte[] tag = HMACSHA256.HashData(keys.AsSpan(24), encryptedBytes.AsSpan(0, encryptedBytes.Length - 32));
using (Aes aes = Aes.Create())
{
    aes.Key = keys[..24];
    byte[] independentPlaintext = aes.DecryptCbc(encryptedBytes.AsSpan(41, encryptedBytes.Length - 73),
        encryptedBytes.AsSpan(25, 16), PaddingMode.PKCS7);
    Check(aes.KeySize == 192 && text.SequenceEqual(independentPlaintext) &&
        CryptographicOperations.FixedTimeEquals(tag, encryptedBytes.AsSpan(encryptedBytes.Length - 32)),
        "Container uses AES-192-CBC, PBKDF2-SHA256 and valid HMAC-SHA256");
}
CryptographicOperations.ZeroMemory(keys);

string wrong = NewPath("wrong-password.txt");
Reject<CryptographicException>(() => FileEncryptionService.DecryptFile(cipher, wrong, "Другой пароль"),
    wrong, "Wrong password rejected without output");

foreach (int offset in new[] { 0, 8, 9, 25, 41, encryptedBytes.Length - 1 })
{
    byte[] damaged = (byte[])encryptedBytes.Clone();
    damaged[offset] ^= 1;
    string damagedPath = NewPath($"damaged-{offset}.aes192");
    File.WriteAllBytes(damagedPath, damaged);
    string damagedOutput = damagedPath + ".restored";
    Reject<Exception>(() => FileEncryptionService.DecryptFile(damagedPath, damagedOutput, password),
        damagedOutput, $"Tampered byte {offset} rejected without output");
}

foreach (int size in new[] { 0, 31, 4 * 1024 * 1024 + 1 })
{
    string invalid = NewPath($"invalid-{size}.txt");
    File.WriteAllBytes(invalid, new byte[size]);
    string invalidOutput = invalid + ".aes192";
    Reject<InvalidDataException>(() => FileEncryptionService.EncryptFile(invalid, invalidOutput, password),
        invalidOutput, $"Out-of-range plaintext {size} bytes rejected");
}

string truncated = NewPath("truncated.aes192");
File.WriteAllBytes(truncated, encryptedBytes[..^1]);
string truncatedOutput = truncated + ".restored";
Reject<InvalidDataException>(() => FileEncryptionService.DecryptFile(truncated, truncatedOutput, password),
    truncatedOutput, "Truncated ciphertext rejected");
string emptyPasswordOutput = NewPath("empty-password.aes192");
Reject<ArgumentException>(() => FileEncryptionService.EncryptFile(sample, emptyPasswordOutput, ""),
    emptyPasswordOutput, "Empty password rejected");

bool samePathRejected = false;
try { FileEncryptionService.EncryptFile(sample, sample, password); }
catch (ArgumentException) { samePathRejected = true; }
Check(samePathRejected && text.SequenceEqual(File.ReadAllBytes(sample)), "Source cannot be overwritten");
string existing = NewPath("existing.txt");
byte[] sentinel = "Existing content must survive"u8.ToArray();
File.WriteAllBytes(existing, sentinel);
bool overwriteRejected = false;
try { FileEncryptionService.DecryptFile(cipher, existing, password); }
catch (IOException) { overwriteRejected = true; }
Check(overwriteRejected && sentinel.SequenceEqual(File.ReadAllBytes(existing)), "Existing output preserved");
Check(!Directory.EnumerateFiles(run, ".lab2-*.tmp").Any(), "No temporary files remain");
int count = results.Count;
results.Add($"SHA256 original/restored: {Convert.ToHexString(SHA256.HashData(text))}");
results.Add($"All {count} checks passed.");
File.WriteAllLines(Path.Combine(root, "latest-results.txt"), results);
Console.WriteLine($"Results: {root}");
