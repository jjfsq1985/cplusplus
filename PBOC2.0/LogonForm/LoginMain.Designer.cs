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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginMain));
            this.labelUser = new System.Windows.Forms.Label();
            this.labelPwd = new System.Windows.Forms.Label();
            this.textUser = new System.Windows.Forms.TextBox();
            this.textPwd = new System.Windows.Forms.TextBox();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.Info = new System.Windows.Forms.Label();
            this.btnDbSetting = new System.Windows.Forms.Button();
            this.SaveLogin = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(31, 21);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(41, 12);
            this.labelUser.TabIndex = 0;
            this.labelUser.Text = "用户名";
            // 
            // labelPwd
            // 
            this.labelPwd.AutoSize = true;
            this.labelPwd.Location = new System.Drawing.Point(31, 52);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.Size = new System.Drawing.Size(29, 12);
            this.labelPwd.TabIndex = 2;
            this.labelPwd.Text = "密码";
            // 
            // textUser
            // 
            this.textUser.Location = new System.Drawing.Point(81, 18);
            this.textUser.Name = "textUser";
            this.textUser.Size = new System.Drawing.Size(138, 21);
            this.textUser.TabIndex = 1;
            this.textUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textUser_KeyPress);
            // 
            // textPwd
            // 
            this.textPwd.Location = new System.Drawing.Point(81, 50);
            this.textPwd.Name = "textPwd";
            this.textPwd.PasswordChar = '#';
            this.textPwd.Size = new System.Drawing.Size(138, 21);
            this.textPwd.TabIndex = 3;
            this.textPwd.Leave += new System.EventHandler(this.textPwd_Leave);
            this.textPwd.Enter += new System.EventHandler(this.textPwd_Enter);
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(55, 112);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(56, 21);
            this.btnIn.TabIndex = 7;
            this.btnIn.Text = "登录";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(148, 112);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(56, 21);
            this.btnOut.TabIndex = 8;
            this.btnOut.Text = "取消";
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // Info
            // 
            this.Info.AutoSize = true;
            this.Info.Location = new System.Drawing.Point(225, 52);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(77, 12);
            this.Info.TabIndex = 4;
            this.Info.Text = "(4-32个字符)";
            // 
            // btnDbSetting
            // 
            this.btnDbSetting.Location = new System.Drawing.Point(223, 112);
            this.btnDbSetting.Name = "btnDbSetting";
            this.btnDbSetting.Size = new System.Drawing.Size(78, 21);
            this.btnDbSetting.TabIndex = 6;
            this.btnDbSetting.Text = "数据库配置";
            this.btnDbSetting.UseVisualStyleBackColor = true;
            this.btnDbSetting.Click += new System.EventHandler(this.btnDbSetting_Click);
            // 
            // SaveLogin
            // 
            this.SaveLogin.AutoSize = true;
            this.SaveLogin.Location = new System.Drawing.Point(81, 77);
            this.SaveLogin.Name = "SaveLogin";
            this.SaveLogin.Size = new System.Drawing.Size(72, 16);
            this.SaveLogin.TabIndex = 5;
            this.SaveLogin.Text = "记住密码";
            this.SaveLogin.UseVisualStyleBackColor = true;
            // 
            // LoginMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 150);
            this.Controls.Add(this.SaveLogin);
            this.Controls.Add(this.btnDbSetting);
            this.Controls.Add(this.Info);
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.textPwd);
            this.Controls.Add(this.textUser);
            this.Controls.Add(this.labelPwd);
            this.Controls.Add(this.labelUser);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.CheckBox SaveLogin;
    }
}

