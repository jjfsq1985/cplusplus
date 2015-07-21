namespace AccountManage
{
    partial class AddAccount
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
            this.labelName = new System.Windows.Forms.Label();
            this.labelPwd = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.textPwd = new System.Windows.Forms.TextBox();
            this.textPwdAgain = new System.Windows.Forms.TextBox();
            this.labelRePwd = new System.Windows.Forms.Label();
            this.ChkLBAuthority = new System.Windows.Forms.CheckedListBox();
            this.labelAuthority = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(67, 39);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 12);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "用户名";
            // 
            // labelPwd
            // 
            this.labelPwd.AutoSize = true;
            this.labelPwd.Location = new System.Drawing.Point(55, 72);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.Size = new System.Drawing.Size(53, 12);
            this.labelPwd.TabIndex = 1;
            this.labelPwd.Text = "输入密码";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(119, 36);
            this.textName.MaxLength = 32;
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(164, 21);
            this.textName.TabIndex = 2;
            this.textName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textName_KeyPress);
            // 
            // textPwd
            // 
            this.textPwd.Location = new System.Drawing.Point(119, 69);
            this.textPwd.MaxLength = 32;
            this.textPwd.Name = "textPwd";
            this.textPwd.PasswordChar = '#';
            this.textPwd.Size = new System.Drawing.Size(164, 21);
            this.textPwd.TabIndex = 3;
            // 
            // textPwdAgain
            // 
            this.textPwdAgain.Location = new System.Drawing.Point(119, 96);
            this.textPwdAgain.MaxLength = 32;
            this.textPwdAgain.Name = "textPwdAgain";
            this.textPwdAgain.PasswordChar = '#';
            this.textPwdAgain.Size = new System.Drawing.Size(164, 21);
            this.textPwdAgain.TabIndex = 4;
            // 
            // labelRePwd
            // 
            this.labelRePwd.AutoSize = true;
            this.labelRePwd.Location = new System.Drawing.Point(31, 99);
            this.labelRePwd.Name = "labelRePwd";
            this.labelRePwd.Size = new System.Drawing.Size(77, 12);
            this.labelRePwd.TabIndex = 5;
            this.labelRePwd.Text = "再输一次密码";
            // 
            // ChkLBAuthority
            // 
            this.ChkLBAuthority.FormattingEnabled = true;
            this.ChkLBAuthority.Location = new System.Drawing.Point(120, 136);
            this.ChkLBAuthority.Name = "ChkLBAuthority";
            this.ChkLBAuthority.Size = new System.Drawing.Size(136, 68);
            this.ChkLBAuthority.TabIndex = 6;
            // 
            // labelAuthority
            // 
            this.labelAuthority.AutoSize = true;
            this.labelAuthority.Location = new System.Drawing.Point(79, 136);
            this.labelAuthority.Name = "labelAuthority";
            this.labelAuthority.Size = new System.Drawing.Size(29, 12);
            this.labelAuthority.TabIndex = 7;
            this.labelAuthority.Text = "权限";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(69, 216);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(232, 216);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 251);
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
            this.Name = "AddAccount";
            this.Text = "添加账户";
            this.Load += new System.EventHandler(this.AddAccount_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelPwd;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.TextBox textPwd;
        private System.Windows.Forms.TextBox textPwdAgain;
        private System.Windows.Forms.Label labelRePwd;
        private System.Windows.Forms.CheckedListBox ChkLBAuthority;
        private System.Windows.Forms.Label labelAuthority;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}