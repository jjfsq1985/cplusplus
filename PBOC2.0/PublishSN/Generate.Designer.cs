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
            this.MadeCard = new System.Windows.Forms.Label();
            this.textAuthorize = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelAuth
            // 
            this.LabelAuth.AutoSize = true;
            this.LabelAuth.Location = new System.Drawing.Point(30, 29);
            this.LabelAuth.Name = "LabelAuth";
            this.LabelAuth.Size = new System.Drawing.Size(41, 12);
            this.LabelAuth.TabIndex = 0;
            this.LabelAuth.Text = "申请码";
            // 
            // LabelLicense
            // 
            this.LabelLicense.AutoSize = true;
            this.LabelLicense.Location = new System.Drawing.Point(30, 96);
            this.LabelLicense.Name = "LabelLicense";
            this.LabelLicense.Size = new System.Drawing.Size(41, 12);
            this.LabelLicense.TabIndex = 1;
            this.LabelLicense.Text = "注册码";
            // 
            // textCode
            // 
            this.textCode.Location = new System.Drawing.Point(89, 26);
            this.textCode.Name = "textCode";
            this.textCode.Size = new System.Drawing.Size(260, 21);
            this.textCode.TabIndex = 2;
            // 
            // textLicense
            // 
            this.textLicense.Location = new System.Drawing.Point(89, 93);
            this.textLicense.Name = "textLicense";
            this.textLicense.ReadOnly = true;
            this.textLicense.Size = new System.Drawing.Size(260, 21);
            this.textLicense.TabIndex = 3;
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(160, 59);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(75, 23);
            this.btnCalc.TabIndex = 4;
            this.btnCalc.Text = "计算注册码";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // MadeCard
            // 
            this.MadeCard.AutoSize = true;
            this.MadeCard.Location = new System.Drawing.Point(20, 30);
            this.MadeCard.Name = "MadeCard";
            this.MadeCard.Size = new System.Drawing.Size(41, 12);
            this.MadeCard.TabIndex = 5;
            this.MadeCard.Text = "授权码";
            // 
            // textAuthorize
            // 
            this.textAuthorize.Location = new System.Drawing.Point(79, 27);
            this.textAuthorize.Name = "textAuthorize";
            this.textAuthorize.ReadOnly = true;
            this.textAuthorize.Size = new System.Drawing.Size(130, 21);
            this.textAuthorize.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Controls.Add(this.textAuthorize);
            this.groupBox1.Controls.Add(this.MadeCard);
            this.groupBox1.Location = new System.Drawing.Point(19, 135);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 67);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "制卡功能授权";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(229, 25);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "生成";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // Generate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 220);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.textLicense);
            this.Controls.Add(this.textCode);
            this.Controls.Add(this.LabelLicense);
            this.Controls.Add(this.LabelAuth);
            this.Name = "Generate";
            this.Text = "软件注册码生成器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelAuth;
        private System.Windows.Forms.Label LabelLicense;
        private System.Windows.Forms.TextBox textCode;
        private System.Windows.Forms.TextBox textLicense;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Label MadeCard;
        private System.Windows.Forms.TextBox textAuthorize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGenerate;
    }
}

