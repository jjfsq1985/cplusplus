namespace PublishCardOperator
{
    partial class ExportKey
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelData = new System.Windows.Forms.Label();
            this.btnGenerateEncryptKey = new System.Windows.Forms.Button();
            this.btnExportCardKey = new System.Windows.Forms.Button();
            this.LabelKey = new System.Windows.Forms.Label();
            this.textData = new System.Windows.Forms.TextBox();
            this.textKey = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LabelData
            // 
            this.LabelData.AutoSize = true;
            this.LabelData.Location = new System.Drawing.Point(38, 28);
            this.LabelData.Name = "LabelData";
            this.LabelData.Size = new System.Drawing.Size(53, 12);
            this.LabelData.TabIndex = 0;
            this.LabelData.Text = "初始数据";
            // 
            // btnGenerateEncryptKey
            // 
            this.btnGenerateEncryptKey.Location = new System.Drawing.Point(102, 96);
            this.btnGenerateEncryptKey.Name = "btnGenerateEncryptKey";
            this.btnGenerateEncryptKey.Size = new System.Drawing.Size(88, 23);
            this.btnGenerateEncryptKey.TabIndex = 1;
            this.btnGenerateEncryptKey.Text = "生成加密密钥";
            this.btnGenerateEncryptKey.UseVisualStyleBackColor = true;
            this.btnGenerateEncryptKey.Click += new System.EventHandler(this.btnGenerateEncryptKey_Click);
            // 
            // btnExportCardKey
            // 
            this.btnExportCardKey.Location = new System.Drawing.Point(196, 96);
            this.btnExportCardKey.Name = "btnExportCardKey";
            this.btnExportCardKey.Size = new System.Drawing.Size(92, 23);
            this.btnExportCardKey.TabIndex = 2;
            this.btnExportCardKey.Text = "导出卡片密钥";
            this.btnExportCardKey.UseVisualStyleBackColor = true;
            this.btnExportCardKey.Click += new System.EventHandler(this.btnExportCardKey_Click);
            // 
            // LabelKey
            // 
            this.LabelKey.AutoSize = true;
            this.LabelKey.Location = new System.Drawing.Point(38, 61);
            this.LabelKey.Name = "LabelKey";
            this.LabelKey.Size = new System.Drawing.Size(53, 12);
            this.LabelKey.TabIndex = 3;
            this.LabelKey.Text = "初始密钥";
            // 
            // textData
            // 
            this.textData.Location = new System.Drawing.Point(102, 25);
            this.textData.MaxLength = 16;
            this.textData.Name = "textData";
            this.textData.Size = new System.Drawing.Size(340, 21);
            this.textData.TabIndex = 4;
            // 
            // textKey
            // 
            this.textKey.Location = new System.Drawing.Point(102, 58);
            this.textKey.MaxLength = 32;
            this.textKey.Name = "textKey";
            this.textKey.Size = new System.Drawing.Size(340, 21);
            this.textKey.TabIndex = 5;
            // 
            // ExportKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 133);
            this.Controls.Add(this.textKey);
            this.Controls.Add(this.textData);
            this.Controls.Add(this.LabelKey);
            this.Controls.Add(this.btnExportCardKey);
            this.Controls.Add(this.LabelData);
            this.Controls.Add(this.btnGenerateEncryptKey);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportKey";
            this.Text = "导出密钥";
            this.Load += new System.EventHandler(this.ExportKey_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExportKey_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelData;
        private System.Windows.Forms.Button btnGenerateEncryptKey;
        private System.Windows.Forms.Button btnExportCardKey;
        private System.Windows.Forms.Label LabelKey;
        private System.Windows.Forms.TextBox textData;
        private System.Windows.Forms.TextBox textKey;
    }
}