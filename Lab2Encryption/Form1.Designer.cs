namespace Lab2Encryption
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            sourceLabel = new Label();
            sourceTextBox = new TextBox();
            sourceBrowseButton = new Button();
            outputLabel = new Label();
            outputTextBox = new TextBox();
            outputBrowseButton = new Button();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            encryptButton = new Button();
            decryptButton = new Button();
            resultLabel = new Label();
            statusTextBox = new TextBox();
            SuspendLayout();
            // sourceLabel
            sourceLabel.AutoSize = true;
            sourceLabel.Location = new Point(12, 17);
            sourceLabel.Text = "Исходный файл:";
            // sourceTextBox
            sourceTextBox.Location = new Point(120, 13);
            sourceTextBox.Name = "sourceTextBox";
            sourceTextBox.AccessibleName = "Исходный файл";
            sourceTextBox.Size = new Size(558, 23);
            sourceTextBox.TabIndex = 0;
            // sourceBrowseButton
            sourceBrowseButton.Location = new Point(688, 12);
            sourceBrowseButton.Size = new Size(40, 25);
            sourceBrowseButton.AccessibleName = "Выбрать исходный файл";
            sourceBrowseButton.TabIndex = 1;
            sourceBrowseButton.Text = "...";
            sourceBrowseButton.UseVisualStyleBackColor = true;
            sourceBrowseButton.Click += SourceBrowseButton_Click;
            // outputLabel
            outputLabel.AutoSize = true;
            outputLabel.Location = new Point(12, 55);
            outputLabel.Text = "Выходной файл:";
            // outputTextBox
            outputTextBox.Location = new Point(120, 51);
            outputTextBox.Name = "outputTextBox";
            outputTextBox.AccessibleName = "Выходной файл";
            outputTextBox.Size = new Size(558, 23);
            outputTextBox.TabIndex = 2;
            // outputBrowseButton
            outputBrowseButton.Location = new Point(688, 50);
            outputBrowseButton.Size = new Size(40, 25);
            outputBrowseButton.AccessibleName = "Выбрать выходной файл";
            outputBrowseButton.TabIndex = 3;
            outputBrowseButton.Text = "...";
            outputBrowseButton.UseVisualStyleBackColor = true;
            outputBrowseButton.Click += OutputBrowseButton_Click;
            // passwordLabel
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(12, 93);
            passwordLabel.Text = "Пароль:";
            // passwordTextBox
            passwordTextBox.Location = new Point(120, 89);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.AccessibleName = "Пароль";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(558, 23);
            passwordTextBox.TabIndex = 4;
            // encryptButton
            encryptButton.Location = new Point(120, 128);
            encryptButton.Name = "encryptButton";
            encryptButton.Size = new Size(145, 30);
            encryptButton.TabIndex = 5;
            encryptButton.Text = "Зашифровать";
            encryptButton.UseVisualStyleBackColor = true;
            encryptButton.Click += EncryptButton_Click;
            // decryptButton
            decryptButton.Location = new Point(277, 128);
            decryptButton.Name = "decryptButton";
            decryptButton.Size = new Size(145, 30);
            decryptButton.TabIndex = 6;
            decryptButton.Text = "Расшифровать";
            decryptButton.UseVisualStyleBackColor = true;
            decryptButton.Click += DecryptButton_Click;
            // resultLabel
            resultLabel.AutoSize = true;
            resultLabel.Location = new Point(12, 181);
            resultLabel.Text = "Результат:";
            // statusTextBox
            statusTextBox.Location = new Point(12, 202);
            statusTextBox.Name = "statusTextBox";
            statusTextBox.AccessibleName = "Результат операции";
            statusTextBox.Size = new Size(716, 116);
            statusTextBox.Multiline = true;
            statusTextBox.ReadOnly = true;
            statusTextBox.ScrollBars = ScrollBars.Vertical;
            statusTextBox.TabStop = false;
            statusTextBox.Text = "Выберите файл и введите пароль.";
            // Form1
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(740, 330);
            Controls.Add(sourceLabel);
            Controls.Add(sourceTextBox);
            Controls.Add(sourceBrowseButton);
            Controls.Add(outputLabel);
            Controls.Add(outputTextBox);
            Controls.Add(outputBrowseButton);
            Controls.Add(passwordLabel);
            Controls.Add(passwordTextBox);
            Controls.Add(encryptButton);
            Controls.Add(decryptButton);
            Controls.Add(resultLabel);
            Controls.Add(statusTextBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Лабораторная 2 - AES-192";
            FormClosing += Form1_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label sourceLabel;
        private TextBox sourceTextBox;
        private Button sourceBrowseButton;
        private Label outputLabel;
        private TextBox outputTextBox;
        private Button outputBrowseButton;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private Button encryptButton;
        private Button decryptButton;
        private Label resultLabel;
        private TextBox statusTextBox;
    }
}
