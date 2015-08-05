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
            this.label1 = new System.Windows.Forms.Label();
            this.textData = new System.Windows.Forms.TextBox();
            this.textKey = new System.Windows.Forms.TextBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.ReadXml = new System.Windows.Forms.CheckBox();
            this.LabelPath = new System.Windows.Forms.Label();
            this.textXmlPath = new System.Windows.Forms.TextBox();
            this.btnXmlPath = new System.Windows.Forms.Button();
            this.groupExport = new System.Windows.Forms.GroupBox();
            this.groupImport = new System.Windows.Forms.GroupBox();
            this.groupExport.SuspendLayout();
            this.groupImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelData
            // 
            this.LabelData.AutoSize = true;
            this.LabelData.Location = new System.Drawing.Point(15, 35);
            this.LabelData.Name = "LabelData";
            this.LabelData.Size = new System.Drawing.Size(53, 12);
            this.LabelData.TabIndex = 0;
            this.LabelData.Text = "初始数据";
            // 
            // btnGenerateEncryptKey
            // 
            this.btnGenerateEncryptKey.Location = new System.Drawing.Point(50, 129);
            this.btnGenerateEncryptKey.Name = "btnGenerateEncryptKey";
            this.btnGenerateEncryptKey.Size = new System.Drawing.Size(88, 23);
            this.btnGenerateEncryptKey.TabIndex = 1;
            this.btnGenerateEncryptKey.Text = "生成加密密钥";
            this.btnGenerateEncryptKey.UseVisualStyleBackColor = true;
            this.btnGenerateEncryptKey.Click += new System.EventHandler(this.btnGenerateEncryptKey_Click);
            // 
            // btnExportCardKey
            // 
            this.btnExportCardKey.Location = new System.Drawing.Point(235, 129);
            this.btnExportCardKey.Name = "btnExportCardKey";
            this.btnExportCardKey.Size = new System.Drawing.Size(92, 23);
            this.btnExportCardKey.TabIndex = 2;
            this.btnExportCardKey.Text = "导出卡片密钥";
            this.btnExportCardKey.UseVisualStyleBackColor = true;
            this.btnExportCardKey.Click += new System.EventHandler(this.btnExportCardKey_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "初始密钥";
            // 
            // textData
            // 
            this.textData.Location = new System.Drawing.Point(92, 32);
            this.textData.MaxLength = 16;
            this.textData.Name = "textData";
            this.textData.Size = new System.Drawing.Size(174, 21);
            this.textData.TabIndex = 4;
            // 
            // textKey
            // 
            this.textKey.Location = new System.Drawing.Point(92, 81);
            this.textKey.MaxLength = 32;
            this.textKey.Name = "textKey";
            this.textKey.Size = new System.Drawing.Size(340, 21);
            this.textKey.TabIndex = 5;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(260, 59);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 6;
            this.BtnSave.Text = "保存配置";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // ReadXml
            // 
            this.ReadXml.AutoSize = true;
            this.ReadXml.Location = new System.Drawing.Point(42, 59);
            this.ReadXml.Name = "ReadXml";
            this.ReadXml.Size = new System.Drawing.Size(126, 16);
            this.ReadXml.TabIndex = 7;
            this.ReadXml.Text = "从XML文件读取密钥";
            this.ReadXml.UseVisualStyleBackColor = true;
            // 
            // LabelPath
            // 
            this.LabelPath.AutoSize = true;
            this.LabelPath.Location = new System.Drawing.Point(21, 30);
            this.LabelPath.Name = "LabelPath";
            this.LabelPath.Size = new System.Drawing.Size(71, 12);
            this.LabelPath.TabIndex = 8;
            this.LabelPath.Text = "制卡密钥XML";
            // 
            // textXmlPath
            // 
            this.textXmlPath.Location = new System.Drawing.Point(98, 27);
            this.textXmlPath.Name = "textXmlPath";
            this.textXmlPath.ReadOnly = true;
            this.textXmlPath.Size = new System.Drawing.Size(300, 21);
            this.textXmlPath.TabIndex = 9;
            // 
            // btnXmlPath
            // 
            this.btnXmlPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnXmlPath.Location = new System.Drawing.Point(407, 27);
            this.btnXmlPath.Name = "btnXmlPath";
            this.btnXmlPath.Size = new System.Drawing.Size(39, 21);
            this.btnXmlPath.TabIndex = 10;
            this.btnXmlPath.Text = "...";
            this.btnXmlPath.UseVisualStyleBackColor = true;
            this.btnXmlPath.Click += new System.EventHandler(this.btnXmlPath_Click);
            // 
            // groupExport
            // 
            this.groupExport.Controls.Add(this.textKey);
            this.groupExport.Controls.Add(this.textData);
            this.groupExport.Controls.Add(this.label1);
            this.groupExport.Controls.Add(this.btnExportCardKey);
            this.groupExport.Controls.Add(this.btnGenerateEncryptKey);
            this.groupExport.Controls.Add(this.LabelData);
            this.groupExport.Location = new System.Drawing.Point(18, 18);
            this.groupExport.Name = "groupExport";
            this.groupExport.Size = new System.Drawing.Size(453, 164);
            this.groupExport.TabIndex = 11;
            this.groupExport.TabStop = false;
            this.groupExport.Text = "导出";
            // 
            // groupImport
            // 
            this.groupImport.Controls.Add(this.btnXmlPath);
            this.groupImport.Controls.Add(this.textXmlPath);
            this.groupImport.Controls.Add(this.LabelPath);
            this.groupImport.Controls.Add(this.ReadXml);
            this.groupImport.Controls.Add(this.BtnSave);
            this.groupImport.Location = new System.Drawing.Point(18, 204);
            this.groupImport.Name = "groupImport";
            this.groupImport.Size = new System.Drawing.Size(453, 92);
            this.groupImport.TabIndex = 12;
            this.groupImport.TabStop = false;
            this.groupImport.Text = "导入";
            // 
            // ExportKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 311);
            this.Controls.Add(this.groupImport);
            this.Controls.Add(this.groupExport);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportKey";
            this.Text = "导入导出密钥";
            this.Load += new System.EventHandler(this.ExportKey_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExportKey_FormClosed);
            this.groupExport.ResumeLayout(false);
            this.groupExport.PerformLayout();
            this.groupImport.ResumeLayout(false);
            this.groupImport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LabelData;
        private System.Windows.Forms.Button btnGenerateEncryptKey;
        private System.Windows.Forms.Button btnExportCardKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textData;
        private System.Windows.Forms.TextBox textKey;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.CheckBox ReadXml;
        private System.Windows.Forms.Label LabelPath;
        private System.Windows.Forms.TextBox textXmlPath;
        private System.Windows.Forms.Button btnXmlPath;
        private System.Windows.Forms.GroupBox groupExport;
        private System.Windows.Forms.GroupBox groupImport;
    }
}