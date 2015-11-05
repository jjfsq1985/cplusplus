namespace PublishCardOperator
{
    partial class ImportKey
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
            this.btnXmlPath = new System.Windows.Forms.Button();
            this.textXmlPath = new System.Windows.Forms.TextBox();
            this.LabelPath = new System.Windows.Forms.Label();
            this.ReadXml = new System.Windows.Forms.CheckBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnXmlPath
            // 
            this.btnXmlPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnXmlPath.Location = new System.Drawing.Point(457, 25);
            this.btnXmlPath.Name = "btnXmlPath";
            this.btnXmlPath.Size = new System.Drawing.Size(39, 21);
            this.btnXmlPath.TabIndex = 15;
            this.btnXmlPath.Text = "...";
            this.btnXmlPath.UseVisualStyleBackColor = true;
            this.btnXmlPath.Click += new System.EventHandler(this.btnXmlPath_Click);
            // 
            // textXmlPath
            // 
            this.textXmlPath.Location = new System.Drawing.Point(97, 25);
            this.textXmlPath.Name = "textXmlPath";
            this.textXmlPath.ReadOnly = true;
            this.textXmlPath.Size = new System.Drawing.Size(354, 21);
            this.textXmlPath.TabIndex = 14;
            // 
            // LabelPath
            // 
            this.LabelPath.AutoSize = true;
            this.LabelPath.Location = new System.Drawing.Point(20, 28);
            this.LabelPath.Name = "LabelPath";
            this.LabelPath.Size = new System.Drawing.Size(71, 12);
            this.LabelPath.TabIndex = 13;
            this.LabelPath.Text = "制卡密钥XML";
            // 
            // ReadXml
            // 
            this.ReadXml.AutoSize = true;
            this.ReadXml.Location = new System.Drawing.Point(97, 60);
            this.ReadXml.Name = "ReadXml";
            this.ReadXml.Size = new System.Drawing.Size(126, 16);
            this.ReadXml.TabIndex = 12;
            this.ReadXml.Text = "从XML文件读取密钥";
            this.ReadXml.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(376, 56);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 11;
            this.BtnSave.Text = "保存配置";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // ImportKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 98);
            this.Controls.Add(this.btnXmlPath);
            this.Controls.Add(this.textXmlPath);
            this.Controls.Add(this.LabelPath);
            this.Controls.Add(this.ReadXml);
            this.Controls.Add(this.BtnSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportKey";
            this.Text = "制卡密钥配置";
            this.Load += new System.EventHandler(this.ImportKey_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnXmlPath;
        private System.Windows.Forms.TextBox textXmlPath;
        private System.Windows.Forms.Label LabelPath;
        private System.Windows.Forms.CheckBox ReadXml;
        private System.Windows.Forms.Button BtnSave;
    }
}