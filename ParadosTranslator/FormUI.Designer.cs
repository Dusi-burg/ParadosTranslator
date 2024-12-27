namespace ParadosTranslator
{
    partial class FormUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
		private FolderBrowserDialog folderBrowserDialogFra;
		private TextBox txtOutput;
		private Button btnSelectOriginal;
		private Button btnSelectWorking;
		private TextBox tbPathOrigin;
		private TextBox tbPathWorking;
		private FolderBrowserDialog folderBrowserDialogIta;
		private Button btnVerifyOriTra;
		private FolderBrowserDialog folderBrowserDialogOut;
	
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
            folderBrowserDialogFra = new FolderBrowserDialog();
            txtOutput = new TextBox();
            btnSelectOriginal = new Button();
            btnSelectWorking = new Button();
            tbPathOrigin = new TextBox();
            tbPathWorking = new TextBox();
            folderBrowserDialogIta = new FolderBrowserDialog();
            btnVerifyOriTra = new Button();
            folderBrowserDialogOut = new FolderBrowserDialog();
            tbPathTranslated = new TextBox();
            btnSelectTranslated = new Button();
            btnCreateDb = new Button();
            cbOriginLanguage = new ComboBox();
            cbTraducedLanguage = new ComboBox();
            cbWorkingLanguage = new ComboBox();
            btnCreateFilesFromDb = new Button();
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            label2 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // folderBrowserDialogFra
            // 
            folderBrowserDialogFra.RootFolder = Environment.SpecialFolder.History;
            // 
            // txtOutput
            // 
            txtOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtOutput.Location = new Point(14, 287);
            txtOutput.Margin = new Padding(3, 4, 3, 4);
            txtOutput.Multiline = true;
            txtOutput.Name = "txtOutput";
            txtOutput.ScrollBars = ScrollBars.Vertical;
            txtOutput.Size = new Size(1143, 478);
            txtOutput.TabIndex = 0;
            // 
            // btnSelectOriginal
            // 
            btnSelectOriginal.Location = new Point(14, 52);
            btnSelectOriginal.Margin = new Padding(3, 4, 3, 4);
            btnSelectOriginal.Name = "btnSelectOriginal";
            btnSelectOriginal.Size = new Size(167, 31);
            btnSelectOriginal.TabIndex = 1;
            btnSelectOriginal.Text = "Select Original";
            btnSelectOriginal.UseVisualStyleBackColor = true;
            btnSelectOriginal.Click += btnSelectOriginal_Click;
            // 
            // btnSelectWorking
            // 
            btnSelectWorking.Location = new Point(14, 145);
            btnSelectWorking.Margin = new Padding(3, 4, 3, 4);
            btnSelectWorking.Name = "btnSelectWorking";
            btnSelectWorking.Size = new Size(167, 31);
            btnSelectWorking.TabIndex = 2;
            btnSelectWorking.Text = "Select Working";
            btnSelectWorking.UseVisualStyleBackColor = true;
            btnSelectWorking.Click += btnSelectWorking_Click;
            // 
            // tbPathOrigin
            // 
            tbPathOrigin.Location = new Point(226, 52);
            tbPathOrigin.Margin = new Padding(3, 4, 3, 4);
            tbPathOrigin.Name = "tbPathOrigin";
            tbPathOrigin.Size = new Size(604, 27);
            tbPathOrigin.TabIndex = 3;
            // 
            // tbPathWorking
            // 
            tbPathWorking.Location = new Point(226, 146);
            tbPathWorking.Margin = new Padding(3, 4, 3, 4);
            tbPathWorking.Name = "tbPathWorking";
            tbPathWorking.Size = new Size(604, 27);
            tbPathWorking.TabIndex = 4;
            // 
            // folderBrowserDialogIta
            // 
            folderBrowserDialogIta.RootFolder = Environment.SpecialFolder.History;
            // 
            // btnVerifyOriTra
            // 
            btnVerifyOriTra.Location = new Point(13, 39);
            btnVerifyOriTra.Margin = new Padding(3, 4, 3, 4);
            btnVerifyOriTra.Name = "btnVerifyOriTra";
            btnVerifyOriTra.Size = new Size(135, 31);
            btnVerifyOriTra.TabIndex = 11;
            btnVerifyOriTra.Text = "Verify ORI-TRA";
            btnVerifyOriTra.UseVisualStyleBackColor = true;
            btnVerifyOriTra.Click += btnVerifyOriTra_Click;
            // 
            // folderBrowserDialogOut
            // 
            folderBrowserDialogOut.RootFolder = Environment.SpecialFolder.History;
            // 
            // tbPathTranslated
            // 
            tbPathTranslated.Location = new Point(226, 100);
            tbPathTranslated.Margin = new Padding(3, 4, 3, 4);
            tbPathTranslated.Name = "tbPathTranslated";
            tbPathTranslated.Size = new Size(604, 27);
            tbPathTranslated.TabIndex = 16;
            // 
            // btnSelectTranslated
            // 
            btnSelectTranslated.Location = new Point(14, 99);
            btnSelectTranslated.Margin = new Padding(3, 4, 3, 4);
            btnSelectTranslated.Name = "btnSelectTranslated";
            btnSelectTranslated.Size = new Size(167, 31);
            btnSelectTranslated.TabIndex = 15;
            btnSelectTranslated.Text = "Select Translated";
            btnSelectTranslated.UseVisualStyleBackColor = true;
            btnSelectTranslated.Click += btnSelectTranslated_Click;
            // 
            // btnCreateDb
            // 
            btnCreateDb.Location = new Point(164, 39);
            btnCreateDb.Margin = new Padding(3, 4, 3, 4);
            btnCreateDb.Name = "btnCreateDb";
            btnCreateDb.Size = new Size(135, 31);
            btnCreateDb.TabIndex = 20;
            btnCreateDb.Text = "Create Db";
            btnCreateDb.UseVisualStyleBackColor = true;
            btnCreateDb.Click += btnCreateDb_Click;
            // 
            // cbOriginLanguage
            // 
            cbOriginLanguage.FormattingEnabled = true;
            cbOriginLanguage.Location = new Point(865, 51);
            cbOriginLanguage.Name = "cbOriginLanguage";
            cbOriginLanguage.Size = new Size(151, 28);
            cbOriginLanguage.TabIndex = 21;
            // 
            // cbTraducedLanguage
            // 
            cbTraducedLanguage.FormattingEnabled = true;
            cbTraducedLanguage.Location = new Point(865, 99);
            cbTraducedLanguage.Name = "cbTraducedLanguage";
            cbTraducedLanguage.Size = new Size(151, 28);
            cbTraducedLanguage.TabIndex = 22;
            // 
            // cbWorkingLanguage
            // 
            cbWorkingLanguage.FormattingEnabled = true;
            cbWorkingLanguage.Location = new Point(865, 145);
            cbWorkingLanguage.Name = "cbWorkingLanguage";
            cbWorkingLanguage.Size = new Size(151, 28);
            cbWorkingLanguage.TabIndex = 23;
            // 
            // btnCreateFilesFromDb
            // 
            btnCreateFilesFromDb.Location = new Point(54, 40);
            btnCreateFilesFromDb.Margin = new Padding(3, 4, 3, 4);
            btnCreateFilesFromDb.Name = "btnCreateFilesFromDb";
            btnCreateFilesFromDb.Size = new Size(218, 31);
            btnCreateFilesFromDb.TabIndex = 24;
            btnCreateFilesFromDb.Text = "Create Files From Db";
            btnCreateFilesFromDb.UseVisualStyleBackColor = true;
            btnCreateFilesFromDb.Click += btnCreateFilesFromDb_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnCreateDb);
            panel1.Controls.Add(btnVerifyOriTra);
            panel1.Location = new Point(206, 195);
            panel1.Name = "panel1";
            panel1.Size = new Size(326, 85);
            panel1.TabIndex = 25;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(79, 11);
            label1.Name = "label1";
            label1.Size = new Size(164, 20);
            label1.TabIndex = 21;
            label1.Text = "From Files To Db (csv)";
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(label2);
            panel2.Controls.Add(btnCreateFilesFromDb);
            panel2.Location = new Point(548, 195);
            panel2.Name = "panel2";
            panel2.Size = new Size(326, 85);
            panel2.TabIndex = 26;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(80, 12);
            label2.Name = "label2";
            label2.Size = new Size(164, 20);
            label2.TabIndex = 22;
            label2.Text = "From Files To Db (csv)";
            // 
            // FormUI
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1171, 782);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(cbWorkingLanguage);
            Controls.Add(cbTraducedLanguage);
            Controls.Add(tbPathTranslated);
            Controls.Add(btnSelectTranslated);
            Controls.Add(tbPathWorking);
            Controls.Add(tbPathOrigin);
            Controls.Add(btnSelectWorking);
            Controls.Add(btnSelectOriginal);
            Controls.Add(txtOutput);
            Controls.Add(cbOriginLanguage);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormUI";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox tbPathTranslated;
        private Button btnSelectTranslated;
        private Button btnCreateDb;
        private ComboBox cbOriginLanguage;
        private ComboBox cbTraducedLanguage;
        private ComboBox cbWorkingLanguage;
        private Button btnCreateFilesFromDb;
        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private Label label2;
    }
}
