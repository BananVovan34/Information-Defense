using System.Security.Cryptography;

namespace Lab2Encryption;

/// <summary>AES-192-CBC with an authenticated, versioned file header.</summary>
public static class FileEncryptionService
{
    public const int MinPlaintextSize = 32;
    public const int MaxPlaintextSize = 4 * 1024 * 1024;

    private static ReadOnlySpan<byte> Magic => "L2AES192"u8;
    private const byte FormatVersion = 1;
    private const int Iterations = 210_000;
    private const int SaltSize = 16;
    private const int IvSize = 16;
    private const int EncryptionKeySize = 24;
    private const int AuthenticationKeySize = 32;
    private const int TagSize = 32;
    private const int HeaderSize = 8 + 1 + SaltSize + IvSize;
    private const int MaxEncryptedSize = HeaderSize + MaxPlaintextSize + 16 + TagSize;

    public static FileOperationResult EncryptFile(string sourcePath, string outputPath, string password)
    {
        var paths = ValidatePaths(sourcePath, outputPath, password);
        byte[] plaintext = ReadBoundedFile(paths.Source, MinPlaintextSize, MaxPlaintextSize,
            "Размер исходного файла должен быть от 32 байт до 4 МБ (4 194 304 байт).");
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] iv = RandomNumberGenerator.GetBytes(IvSize);
        byte[] keyMaterial = DeriveKeys(password, salt);
        byte[] encryptionKey = keyMaterial.AsSpan(0, EncryptionKeySize).ToArray();

        try
        {
            using Aes aes = CreateAes(encryptionKey);
            byte[] ciphertext = aes.EncryptCbc(plaintext, iv, PaddingMode.PKCS7);
            byte[] result = new byte[HeaderSize + ciphertext.Length + TagSize];
            Magic.CopyTo(result);
            result[8] = FormatVersion;
            salt.CopyTo(result, 9);
            iv.CopyTo(result, 9 + SaltSize);
            ciphertext.CopyTo(result, HeaderSize);
            // Authenticate the salt and IV along with the ciphertext.
            HMACSHA256.HashData(keyMaterial.AsSpan(EncryptionKeySize, AuthenticationKeySize),
                result.AsSpan(0, result.Length - TagSize), result.AsSpan(result.Length - TagSize));
            WriteNewFile(paths.Output, result);
            return new FileOperationResult(plaintext.Length, result.Length);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(plaintext);
            CryptographicOperations.ZeroMemory(keyMaterial);
            CryptographicOperations.ZeroMemory(encryptionKey);
        }
    }

    public static FileOperationResult DecryptFile(string sourcePath, string outputPath, string password)
    {
        var paths = ValidatePaths(sourcePath, outputPath, password);
        byte[] encrypted = ReadBoundedFile(paths.Source, HeaderSize + 48 + TagSize, MaxEncryptedSize,
            "Размер зашифрованного файла не соответствует формату этой программы.");
        if (!encrypted.AsSpan(0, 8).SequenceEqual(Magic) || encrypted[8] != FormatVersion)
            throw new InvalidDataException("Файл не создан этой программой или его заголовок повреждён.");
        int ciphertextSize = encrypted.Length - HeaderSize - TagSize;
        if (ciphertextSize % IvSize != 0)
            throw new InvalidDataException("Зашифрованный файл повреждён: неверная длина данных.");
        byte[] keyMaterial = DeriveKeys(password, encrypted.AsSpan(9, SaltSize));
        byte[] encryptionKey = keyMaterial.AsSpan(0, EncryptionKeySize).ToArray();
        byte[]? plaintext = null;
        try
        {
            Span<byte> expectedTag = stackalloc byte[TagSize];
            HMACSHA256.HashData(keyMaterial.AsSpan(EncryptionKeySize, AuthenticationKeySize),
                encrypted.AsSpan(0, encrypted.Length - TagSize), expectedTag);
            // Verify before decrypting or writing anything to disk.
            if (!CryptographicOperations.FixedTimeEquals(expectedTag, encrypted.AsSpan(encrypted.Length - TagSize)))
                throw new CryptographicException("Неверный пароль или файл повреждён. Результат не создан.");
            using Aes aes = CreateAes(encryptionKey);
            plaintext = aes.DecryptCbc(encrypted.AsSpan(HeaderSize, ciphertextSize),
                encrypted.AsSpan(9 + SaltSize, IvSize), PaddingMode.PKCS7);
            if (plaintext.Length < MinPlaintextSize || plaintext.Length > MaxPlaintextSize)
                throw new InvalidDataException("Размер расшифрованного файла выходит за пределы задания.");
            WriteNewFile(paths.Output, plaintext);
            return new FileOperationResult(encrypted.Length, plaintext.Length);
        }
        finally
        {
            if (plaintext is not null)
                CryptographicOperations.ZeroMemory(plaintext);
            CryptographicOperations.ZeroMemory(keyMaterial);
            CryptographicOperations.ZeroMemory(encryptionKey);
        }
    }

    private static byte[] DeriveKeys(string password, ReadOnlySpan<byte> salt) =>
        Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256,
            EncryptionKeySize + AuthenticationKeySize);

    private static Aes CreateAes(byte[] key)
    {
        Aes aes = Aes.Create();
        aes.KeySize = 192;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;
        return aes;
    }

    private static (string Source, string Output) ValidatePaths(string sourcePath, string outputPath, string password)
    {
        if (string.IsNullOrWhiteSpace(sourcePath))
            throw new ArgumentException("Выберите исходный файл.");
        if (string.IsNullOrWhiteSpace(outputPath))
            throw new ArgumentException("Укажите имя выходного файла.");
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Введите пароль.");
        string source = Path.GetFullPath(sourcePath);
        string output = Path.GetFullPath(outputPath);
        if (string.Equals(source, output, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Исходный и выходной файлы должны иметь разные имена.");
        if (!File.Exists(source))
            throw new FileNotFoundException("Исходный файл не найден.");
        if (File.Exists(output) || Directory.Exists(output))
            throw new IOException("Выходной файл уже существует. Выберите другое имя, чтобы сохранить его содержимое.");
        if (!Directory.Exists(Path.GetDirectoryName(output)))
            throw new DirectoryNotFoundException("Папка для сохранения результата не найдена.");
        return (source, output);
    }

    private static byte[] ReadBoundedFile(string path, int minSize, int maxSize, string errorMessage)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (stream.Length < minSize || stream.Length > maxSize)
            throw new InvalidDataException(errorMessage);
        byte[] bytes = new byte[(int)stream.Length];
        stream.ReadExactly(bytes);
        return bytes;
    }

    private static void WriteNewFile(string outputPath, byte[] contents)
    {
        // Commit only a complete file and never overwrite an existing destination.
        string temporaryPath = Path.Combine(Path.GetDirectoryName(outputPath)!, $".lab2-{Guid.NewGuid():N}.tmp");
        try
        {
            using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                stream.Write(contents);
                stream.Flush(flushToDisk: true);
            }
            File.Move(temporaryPath, outputPath, overwrite: false);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }
}

public sealed record FileOperationResult(long InputBytes, long OutputBytes);
