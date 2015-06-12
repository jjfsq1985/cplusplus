namespace AccountManage
{
    partial class AccountEdit
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.labelAuthority = new System.Windows.Forms.Label();
            this.ChkLBAuthority = new System.Windows.Forms.CheckedListBox();
            this.labelRePwd = new System.Windows.Forms.Label();
            this.textPwdAgain = new System.Windows.Forms.TextBox();
            this.textPwd = new System.Windows.Forms.TextBox();
            this.textName = new System.Windows.Forms.TextBox();
            this.labelPwd = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelOldPwd = new System.Windows.Forms.Label();
            this.textPwdOld = new System.Windows.Forms.TextBox();
            this.AccountStop = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(234, 282);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(63, 282);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelAuthority
            // 
            this.labelAuthority.AutoSize = true;
            this.labelAuthority.Location = new System.Drawing.Point(73, 164);
            this.labelAuthority.Name = "labelAuthority";
            this.labelAuthority.Size = new System.Drawing.Size(29, 12);
            this.labelAuthority.TabIndex = 17;
            this.labelAuthority.Text = "权限";
            // 
            // ChkLBAuthority
            // 
            this.ChkLBAuthority.FormattingEnabled = true;
            this.ChkLBAuthority.Location = new System.Drawing.Point(114, 164);
            this.ChkLBAuthority.Name = "ChkLBAuthority";
            this.ChkLBAuthority.Size = new System.Drawing.Size(136, 68);
            this.ChkLBAuthority.TabIndex = 16;
            // 
            // labelRePwd
            // 
            this.labelRePwd.AutoSize = true;
            this.labelRePwd.Location = new System.Drawing.Point(25, 128);
            this.labelRePwd.Name = "labelRePwd";
            this.labelRePwd.Size = new System.Drawing.Size(89, 12);
            this.labelRePwd.TabIndex = 15;
            this.labelRePwd.Text = "再输一次新密码";
            // 
            // textPwdAgain
            // 
            this.textPwdAgain.Location = new System.Drawing.Point(118, 125);
            this.textPwdAgain.MaxLength = 32;
            this.textPwdAgain.Name = "textPwdAgain";
            this.textPwdAgain.PasswordChar = '#';
            this.textPwdAgain.Size = new System.Drawing.Size(164, 21);
            this.textPwdAgain.TabIndex = 14;
            // 
            // textPwd
            // 
            this.textPwd.Location = new System.Drawing.Point(118, 89);
            this.textPwd.MaxLength = 32;
            this.textPwd.Name = "textPwd";
            this.textPwd.PasswordChar = '#';
            this.textPwd.Size = new System.Drawing.Size(164, 21);
            this.textPwd.TabIndex = 13;
            // 
            // textName
            // 
            this.textName.Enabled = false;
            this.textName.Location = new System.Drawing.Point(118, 17);
            this.textName.MaxLength = 32;
            this.textName.Name = "textName";
            this.textName.ReadOnly = true;
            this.textName.Size = new System.Drawing.Size(164, 21);
            this.textName.TabIndex = 12;
            // 
            // labelPwd
            // 
            this.labelPwd.AutoSize = true;
            this.labelPwd.Location = new System.Drawing.Point(49, 92);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.Size = new System.Drawing.Size(65, 12);
            this.labelPwd.TabIndex = 11;
            this.labelPwd.Text = "输入新密码";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(61, 20);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 12);
            this.labelName.TabIndex = 10;
            this.labelName.Text = "用户名";
            // 
            // labelOldPwd
            // 
            this.labelOldPwd.AutoSize = true;
            this.labelOldPwd.Location = new System.Drawing.Point(61, 56);
            this.labelOldPwd.Name = "labelOldPwd";
            this.labelOldPwd.Size = new System.Drawing.Size(41, 12);
            this.labelOldPwd.TabIndex = 20;
            this.labelOldPwd.Text = "原密码";
            // 
            // textPwdOld
            // 
            this.textPwdOld.Location = new System.Drawing.Point(118, 53);
            this.textPwdOld.MaxLength = 32;
            this.textPwdOld.Name = "textPwdOld";
            this.textPwdOld.PasswordChar = '#';
            this.textPwdOld.Size = new System.Drawing.Size(164, 21);
            this.textPwdOld.TabIndex = 21;
            // 
            // AccountStop
            // 
            this.AccountStop.AutoSize = true;
            this.AccountStop.Location = new System.Drawing.Point(75, 250);
            this.AccountStop.Name = "AccountStop";
            this.AccountStop.Size = new System.Drawing.Size(72, 16);
            this.AccountStop.TabIndex = 22;
            this.AccountStop.Text = "账户停用";
            this.AccountStop.UseVisualStyleBackColor = true;
            // 
            // AccountEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 316);
            this.Controls.Add(this.AccountStop);
            this.Controls.Add(this.textPwdOld);
            this.Controls.Add(this.labelOldPwd);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelAuthority);
            this.Controls.Add(this.ChkLBAuthority);
            this.Controls.Add(this.labelRePwd);
            this.Controls.Add(this.textPwdAgain);
            this.Controls.Add(this.textPwd);
            this.Controls.Add(this.textName);
            this.Controls.Add(this.labelPwd);
            this.Controls.Add(this.labelName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AccountEdit";
            this.Text = "编辑账户";
            this.Load += new System.EventHandler(this.AccountEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labelAuthority;
        private System.Windows.Forms.CheckedListBox ChkLBAuthority;
        private System.Windows.Forms.Label labelRePwd;
        private System.Windows.Forms.TextBox textPwdAgain;
        private System.Windows.Forms.TextBox textPwd;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Label labelPwd;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelOldPwd;
        private System.Windows.Forms.TextBox textPwdOld;
        private System.Windows.Forms.CheckBox AccountStop;
    }
}