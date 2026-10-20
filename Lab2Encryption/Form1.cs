using System.Diagnostics;
using System.Security.Cryptography;

namespace Lab2Encryption;

public partial class Form1 : Form
{
    private bool operationInProgress;

    public Form1()
    {
        InitializeComponent();
    }

    private void SourceBrowseButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Выберите исходный файл",
            Filter = "Текстовые и зашифрованные файлы|*.txt;*.aes192|Все файлы|*.*",
            CheckFileExists = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;
        sourceTextBox.Text = dialog.FileName;
        string suggested = dialog.FileName.EndsWith(".aes192", StringComparison.OrdinalIgnoreCase)
            ? Path.Combine(Path.GetDirectoryName(dialog.FileName)!,
                Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(dialog.FileName)) + "_restored.txt")
            : dialog.FileName + ".aes192";
        outputTextBox.Text = GetUnusedPath(suggested);
    }

    private void OutputBrowseButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Укажите новый файл для результата",
            Filter = "Все файлы|*.*",
            FileName = outputTextBox.Text,
            OverwritePrompt = false,
            AddExtension = false
        };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            outputTextBox.Text = dialog.FileName;
    }

    private async void EncryptButton_Click(object? sender, EventArgs e) => await RunOperationAsync(encrypt: true);
    private async void DecryptButton_Click(object? sender, EventArgs e) => await RunOperationAsync(encrypt: false);

    private async Task RunOperationAsync(bool encrypt)
    {
        if (operationInProgress)
            return;
        string source = sourceTextBox.Text.Trim();
        string output = outputTextBox.Text.Trim();
        string password = passwordTextBox.Text;
        SetBusy(true);
        statusTextBox.ForeColor = Color.FromArgb(45, 55, 72);
        statusTextBox.Text = encrypt ? "Шифрование…" : "Расшифрование и проверка целостности…";
        var stopwatch = Stopwatch.StartNew();
        try
        {
            FileOperationResult result = await Task.Run(() => encrypt
                ? FileEncryptionService.EncryptFile(source, output, password)
                : FileEncryptionService.DecryptFile(source, output, password));
            stopwatch.Stop();
            statusTextBox.ForeColor = Color.FromArgb(28, 100, 65);
            statusTextBox.Text = $"{(encrypt ? "Шифрование" : "Расшифрование")} завершено.\r\n" +
                $"Файл: {Path.GetFileName(output)}\r\n" +
                $"Размер: {result.InputBytes:N0} → {result.OutputBytes:N0} байт\r\n" +
                $"Время: {stopwatch.Elapsed.TotalSeconds:F2} с";
        }
        catch (Exception ex) when (ex is IOException or InvalidDataException or UnauthorizedAccessException or
            ArgumentException or CryptographicException or NotSupportedException or System.Security.SecurityException)
        {
            statusTextBox.ForeColor = Color.FromArgb(165, 38, 38);
            statusTextBox.Text = $"Операция не выполнена.\r\n{ex.Message}";
        }
        finally
        {
            passwordTextBox.Clear();
            password = string.Empty;
            SetBusy(false);
        }
    }

    private void SetBusy(bool busy)
    {
        operationInProgress = busy;
        sourceTextBox.Enabled = outputTextBox.Enabled = passwordTextBox.Enabled = !busy;
        sourceBrowseButton.Enabled = outputBrowseButton.Enabled = !busy;
        encryptButton.Enabled = decryptButton.Enabled = !busy;
        UseWaitCursor = busy;
    }

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (operationInProgress)
            e.Cancel = true;
    }

    private static string GetUnusedPath(string path)
    {
        string candidate = path;
        for (int suffix = 2; File.Exists(candidate) || Directory.Exists(candidate); suffix++)
            candidate = Path.Combine(Path.GetDirectoryName(path)!,
                $"{Path.GetFileNameWithoutExtension(path)}_{suffix}{Path.GetExtension(path)}");
        return candidate;
    }
}
