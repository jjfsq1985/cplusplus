namespace FNTMain
{
    partial class LoginMain
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
            this.labelUser = new System.Windows.Forms.Label();
            this.labelPwd = new System.Windows.Forms.Label();
            this.textUser = new System.Windows.Forms.TextBox();
            this.textPwd = new System.Windows.Forms.TextBox();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.Info = new System.Windows.Forms.Label();
            this.btnDbSetting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(54, 41);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(49, 14);
            this.labelUser.TabIndex = 2;
            this.labelUser.Text = "用户名";
            // 
            // labelPwd
            // 
            this.labelPwd.AutoSize = true;
            this.labelPwd.Location = new System.Drawing.Point(54, 81);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.Size = new System.Drawing.Size(35, 14);
            this.labelPwd.TabIndex = 3;
            this.labelPwd.Text = "密码";
            // 
            // textUser
            // 
            this.textUser.Location = new System.Drawing.Point(112, 37);
            this.textUser.Name = "textUser";
            this.textUser.Size = new System.Drawing.Size(131, 23);
            this.textUser.TabIndex = 0;
            this.textUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textUser_KeyPress);
            // 
            // textPwd
            // 
            this.textPwd.Location = new System.Drawing.Point(112, 78);
            this.textPwd.Name = "textPwd";
            this.textPwd.PasswordChar = '#';
            this.textPwd.Size = new System.Drawing.Size(131, 23);
            this.textPwd.TabIndex = 1;
            this.textPwd.Leave += new System.EventHandler(this.textPwd_Leave);            
            this.textPwd.Enter += new System.EventHandler(this.textPwd_Enter);
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(132, 124);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(91, 25);
            this.btnIn.TabIndex = 5;
            this.btnIn.Text = "登录";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(237, 124);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(91, 25);
            this.btnOut.TabIndex = 6;
            this.btnOut.Text = "取消";
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // Info
            // 
            this.Info.AutoSize = true;
            this.Info.Location = new System.Drawing.Point(251, 81);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(91, 14);
            this.Info.TabIndex = 4;
            this.Info.Text = "(4-32个字符)";
            // 
            // btnDbSetting
            // 
            this.btnDbSetting.Location = new System.Drawing.Point(27, 124);
            this.btnDbSetting.Name = "btnDbSetting";
            this.btnDbSetting.Size = new System.Drawing.Size(91, 25);
            this.btnDbSetting.TabIndex = 7;
            this.btnDbSetting.Text = "数据库配置";
            this.btnDbSetting.UseVisualStyleBackColor = true;
            this.btnDbSetting.Click += new System.EventHandler(this.btnDbSetting_Click);
            // 
            // LoginMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 170);
            this.Controls.Add(this.btnDbSetting);
            this.Controls.Add(this.Info);
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.textPwd);
            this.Controls.Add(this.textUser);
            this.Controls.Add(this.labelPwd);
            this.Controls.Add(this.labelUser);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelPwd;
        private System.Windows.Forms.TextBox textUser;
        private System.Windows.Forms.TextBox textPwd;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.Label Info;
        private System.Windows.Forms.Button btnDbSetting;
    }
}

