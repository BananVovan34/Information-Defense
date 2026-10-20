namespace Lab2Encryption
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            layout = new TableLayoutPanel();
            headerPanel = new Panel();
            titleLabel = new Label();
            subtitleLabel = new Label();
            sourcePanel = new Panel();
            sourceLabel = new Label();
            sourceTextBox = new TextBox();
            sourceBrowseButton = new Button();
            outputPanel = new Panel();
            outputLabel = new Label();
            outputTextBox = new TextBox();
            outputBrowseButton = new Button();
            passwordPanel = new Panel();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            passwordHintLabel = new Label();
            buttonPanel = new FlowLayoutPanel();
            encryptButton = new Button();
            decryptButton = new Button();
            statusGroupBox = new GroupBox();
            statusTextBox = new TextBox();
            layout.SuspendLayout();
            headerPanel.SuspendLayout();
            sourcePanel.SuspendLayout();
            outputPanel.SuspendLayout();
            passwordPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            statusGroupBox.SuspendLayout();
            SuspendLayout();
            // layout
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.Controls.Add(headerPanel, 0, 0);
            layout.Controls.Add(sourcePanel, 0, 1);
            layout.Controls.Add(outputPanel, 0, 2);
            layout.Controls.Add(passwordPanel, 0, 3);
            layout.Controls.Add(buttonPanel, 0, 4);
            layout.Controls.Add(statusGroupBox, 0, 5);
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(24, 20, 24, 20);
            layout.RowCount = 6;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.Size = new Size(880, 650);
            // headerPanel
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(subtitleLabel);
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.Margin = new Padding(0);
            // titleLabel
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 21F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(30, 49, 76);
            titleLabel.Location = new Point(0, 0);
            titleLabel.Text = "Вариант 5 · AES-192";
            // subtitleLabel
            subtitleLabel.AutoSize = true;
            subtitleLabel.ForeColor = Color.FromArgb(85, 95, 110);
            subtitleLabel.Location = new Point(2, 47);
            subtitleLabel.Text = "Шифрование и расшифрование текстовых файлов от 32 байт до 4 МБ.";
            // sourcePanel
            sourcePanel.Controls.Add(sourceLabel);
            sourcePanel.Controls.Add(sourceTextBox);
            sourcePanel.Controls.Add(sourceBrowseButton);
            sourcePanel.Dock = DockStyle.Fill;
            sourcePanel.Margin = new Padding(0);
            sourcePanel.Size = new Size(832, 82);
            // sourceLabel
            sourceLabel.AutoSize = true;
            sourceLabel.Location = new Point(0, 0);
            sourceLabel.Text = "Исходный файл";
            // sourceTextBox
            sourceTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            sourceTextBox.Location = new Point(0, 29);
            sourceTextBox.Name = "sourceTextBox";
            sourceTextBox.AccessibleName = "Исходный файл";
            sourceTextBox.Size = new Size(700, 30);
            sourceTextBox.TabIndex = 0;
            // sourceBrowseButton
            sourceBrowseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            sourceBrowseButton.Location = new Point(712, 27);
            sourceBrowseButton.Size = new Size(120, 34);
            sourceBrowseButton.TabIndex = 1;
            sourceBrowseButton.Text = "Обзор…";
            sourceBrowseButton.UseVisualStyleBackColor = true;
            sourceBrowseButton.Click += SourceBrowseButton_Click;
            // outputPanel
            outputPanel.Controls.Add(outputLabel);
            outputPanel.Controls.Add(outputTextBox);
            outputPanel.Controls.Add(outputBrowseButton);
            outputPanel.Dock = DockStyle.Fill;
            outputPanel.Margin = new Padding(0);
            outputPanel.Size = new Size(832, 82);
            // outputLabel
            outputLabel.AutoSize = true;
            outputLabel.Location = new Point(0, 0);
            outputLabel.Text = "Выходной файл (новое имя)";
            // outputTextBox
            outputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            outputTextBox.Location = new Point(0, 29);
            outputTextBox.Name = "outputTextBox";
            outputTextBox.AccessibleName = "Выходной файл";
            outputTextBox.Size = new Size(700, 30);
            outputTextBox.TabIndex = 0;
            // outputBrowseButton
            outputBrowseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            outputBrowseButton.Location = new Point(712, 27);
            outputBrowseButton.Size = new Size(120, 34);
            outputBrowseButton.TabIndex = 1;
            outputBrowseButton.Text = "Сохранить…";
            outputBrowseButton.UseVisualStyleBackColor = true;
            outputBrowseButton.Click += OutputBrowseButton_Click;
            // passwordPanel
            passwordPanel.Controls.Add(passwordLabel);
            passwordPanel.Controls.Add(passwordTextBox);
            passwordPanel.Controls.Add(passwordHintLabel);
            passwordPanel.Dock = DockStyle.Fill;
            passwordPanel.Margin = new Padding(0);
            passwordPanel.Size = new Size(832, 96);
            // passwordLabel
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(0, 0);
            passwordLabel.Text = "Пароль";
            // passwordTextBox
            passwordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            passwordTextBox.Location = new Point(0, 29);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.AccessibleName = "Пароль";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(832, 30);
            passwordTextBox.TabIndex = 0;
            // passwordHintLabel
            passwordHintLabel.AutoSize = true;
            passwordHintLabel.ForeColor = Color.FromArgb(85, 95, 110);
            passwordHintLabel.Location = new Point(0, 65);
            passwordHintLabel.Text = "Для расшифрования нужен тот же пароль. После операции поле очищается.";
            // buttonPanel
            buttonPanel.Controls.Add(encryptButton);
            buttonPanel.Controls.Add(decryptButton);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.Margin = new Padding(0);
            // encryptButton
            encryptButton.BackColor = Color.FromArgb(36, 76, 126);
            encryptButton.FlatStyle = FlatStyle.Flat;
            encryptButton.FlatAppearance.BorderSize = 0;
            encryptButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            encryptButton.ForeColor = Color.White;
            encryptButton.Margin = new Padding(0, 0, 12, 0);
            encryptButton.Name = "encryptButton";
            encryptButton.Size = new Size(190, 42);
            encryptButton.Text = "Зашифровать";
            encryptButton.UseVisualStyleBackColor = false;
            encryptButton.Click += EncryptButton_Click;
            // decryptButton
            decryptButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            decryptButton.Margin = new Padding(0);
            decryptButton.Name = "decryptButton";
            decryptButton.Size = new Size(190, 42);
            decryptButton.Text = "Расшифровать";
            decryptButton.UseVisualStyleBackColor = true;
            decryptButton.Click += DecryptButton_Click;
            // statusGroupBox
            statusGroupBox.Controls.Add(statusTextBox);
            statusGroupBox.Dock = DockStyle.Fill;
            statusGroupBox.Margin = new Padding(0);
            statusGroupBox.Padding = new Padding(12, 8, 12, 12);
            statusGroupBox.Text = "Результат";
            // statusTextBox
            statusTextBox.BackColor = Color.FromArgb(246, 248, 251);
            statusTextBox.BorderStyle = BorderStyle.None;
            statusTextBox.Dock = DockStyle.Fill;
            statusTextBox.ForeColor = Color.FromArgb(45, 55, 72);
            statusTextBox.Multiline = true;
            statusTextBox.Name = "statusTextBox";
            statusTextBox.AccessibleName = "Результат операции";
            statusTextBox.ReadOnly = true;
            statusTextBox.ScrollBars = ScrollBars.Vertical;
            statusTextBox.TabStop = false;
            statusTextBox.Text = "Выберите файл, укажите новое имя для результата и введите пароль.";
            // Form1
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(880, 650);
            Controls.Add(layout);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(850, 665);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Лабораторная работа 2 — Симметричное шифрование";
            FormClosing += Form1_FormClosing;
            layout.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            sourcePanel.ResumeLayout(false);
            sourcePanel.PerformLayout();
            outputPanel.ResumeLayout(false);
            outputPanel.PerformLayout();
            passwordPanel.ResumeLayout(false);
            passwordPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
            statusGroupBox.ResumeLayout(false);
            statusGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layout;
        private Panel headerPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private Panel sourcePanel;
        private Label sourceLabel;
        private TextBox sourceTextBox;
        private Button sourceBrowseButton;
        private Panel outputPanel;
        private Label outputLabel;
        private TextBox outputTextBox;
        private Button outputBrowseButton;
        private Panel passwordPanel;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private Label passwordHintLabel;
        private FlowLayoutPanel buttonPanel;
        private Button encryptButton;
        private Button decryptButton;
        private GroupBox statusGroupBox;
        private TextBox statusTextBox;
    }
}
