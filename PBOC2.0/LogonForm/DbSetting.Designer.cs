namespace FNTMain
{
    partial class DbSetting
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
            this.labelServer = new System.Windows.Forms.Label();
            this.labelDbName = new System.Windows.Forms.Label();
            this.labelDbUser = new System.Windows.Forms.Label();
            this.labelDbPwd = new System.Windows.Forms.Label();
            this.textDbServer = new System.Windows.Forms.TextBox();
            this.textDbName = new System.Windows.Forms.TextBox();
            this.textDbUser = new System.Windows.Forms.TextBox();
            this.textDbPwd = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Location = new System.Drawing.Point(27, 27);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(65, 12);
            this.labelServer.TabIndex = 0;
            this.labelServer.Text = "服务器名称";
            // 
            // labelDbName
            // 
            this.labelDbName.AutoSize = true;
            this.labelDbName.Location = new System.Drawing.Point(27, 67);
            this.labelDbName.Name = "labelDbName";
            this.labelDbName.Size = new System.Drawing.Size(65, 12);
            this.labelDbName.TabIndex = 1;
            this.labelDbName.Text = "数据库名称";
            // 
            // labelDbUser
            // 
            this.labelDbUser.AutoSize = true;
            this.labelDbUser.Location = new System.Drawing.Point(39, 102);
            this.labelDbUser.Name = "labelDbUser";
            this.labelDbUser.Size = new System.Drawing.Size(41, 12);
            this.labelDbUser.TabIndex = 2;
            this.labelDbUser.Text = "用户名";
            // 
            // labelDbPwd
            // 
            this.labelDbPwd.AutoSize = true;
            this.labelDbPwd.Location = new System.Drawing.Point(45, 141);
            this.labelDbPwd.Name = "labelDbPwd";
            this.labelDbPwd.Size = new System.Drawing.Size(29, 12);
            this.labelDbPwd.TabIndex = 3;
            this.labelDbPwd.Text = "密码";
            // 
            // textDbServer
            // 
            this.textDbServer.Location = new System.Drawing.Point(108, 24);
            this.textDbServer.Name = "textDbServer";
            this.textDbServer.Size = new System.Drawing.Size(100, 21);
            this.textDbServer.TabIndex = 4;
            // 
            // textDbName
            // 
            this.textDbName.Location = new System.Drawing.Point(108, 67);
            this.textDbName.Name = "textDbName";
            this.textDbName.ReadOnly = true;
            this.textDbName.Size = new System.Drawing.Size(100, 21);
            this.textDbName.TabIndex = 5;
            // 
            // textDbUser
            // 
            this.textDbUser.Location = new System.Drawing.Point(108, 99);
            this.textDbUser.Name = "textDbUser";
            this.textDbUser.Size = new System.Drawing.Size(100, 21);
            this.textDbUser.TabIndex = 6;
            // 
            // textDbPwd
            // 
            this.textDbPwd.Location = new System.Drawing.Point(108, 138);
            this.textDbPwd.Name = "textDbPwd";
            this.textDbPwd.PasswordChar = '#';
            this.textDbPwd.Size = new System.Drawing.Size(100, 21);
            this.textDbPwd.TabIndex = 7;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(41, 177);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(170, 177);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DbSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 205);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.textDbPwd);
            this.Controls.Add(this.textDbUser);
            this.Controls.Add(this.textDbName);
            this.Controls.Add(this.textDbServer);
            this.Controls.Add(this.labelDbPwd);
            this.Controls.Add(this.labelDbUser);
            this.Controls.Add(this.labelDbName);
            this.Controls.Add(this.labelServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DbSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库配置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Label labelDbName;
        private System.Windows.Forms.Label labelDbUser;
        private System.Windows.Forms.Label labelDbPwd;
        private System.Windows.Forms.TextBox textDbServer;
        private System.Windows.Forms.TextBox textDbName;
        private System.Windows.Forms.TextBox textDbUser;
        private System.Windows.Forms.TextBox textDbPwd;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}