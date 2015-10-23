namespace PublishSN
{
    partial class Generate
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelAuth = new System.Windows.Forms.Label();
            this.LabelLicense = new System.Windows.Forms.Label();
            this.textCode = new System.Windows.Forms.TextBox();
            this.textLicense = new System.Windows.Forms.TextBox();
            this.btnCalc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelAuth
            // 
            this.LabelAuth.AutoSize = true;
            this.LabelAuth.Location = new System.Drawing.Point(61, 29);
            this.LabelAuth.Name = "LabelAuth";
            this.LabelAuth.Size = new System.Drawing.Size(41, 12);
            this.LabelAuth.TabIndex = 0;
            this.LabelAuth.Text = "申请码";
            // 
            // LabelLicense
            // 
            this.LabelLicense.AutoSize = true;
            this.LabelLicense.Location = new System.Drawing.Point(61, 96);
            this.LabelLicense.Name = "LabelLicense";
            this.LabelLicense.Size = new System.Drawing.Size(41, 12);
            this.LabelLicense.TabIndex = 1;
            this.LabelLicense.Text = "注册码";
            // 
            // textCode
            // 
            this.textCode.Location = new System.Drawing.Point(120, 26);
            this.textCode.Name = "textCode";
            this.textCode.Size = new System.Drawing.Size(260, 21);
            this.textCode.TabIndex = 2;
            // 
            // textLicense
            // 
            this.textLicense.Location = new System.Drawing.Point(120, 93);
            this.textLicense.Name = "textLicense";
            this.textLicense.ReadOnly = true;
            this.textLicense.Size = new System.Drawing.Size(260, 21);
            this.textLicense.TabIndex = 3;
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(191, 59);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(75, 23);
            this.btnCalc.TabIndex = 4;
            this.btnCalc.Text = "计算注册码";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // Generate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 152);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.textLicense);
            this.Controls.Add(this.textCode);
            this.Controls.Add(this.LabelLicense);
            this.Controls.Add(this.LabelAuth);
            this.Name = "Generate";
            this.Text = "软件注册码生成器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelAuth;
        private System.Windows.Forms.Label LabelLicense;
        private System.Windows.Forms.TextBox textCode;
        private System.Windows.Forms.TextBox textLicense;
        private System.Windows.Forms.Button btnCalc;
    }
}

