namespace AccountManage
{
    partial class Account
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
            this.AccountQuit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AccountQuit
            // 
            this.AccountQuit.Location = new System.Drawing.Point(221, 208);
            this.AccountQuit.Name = "AccountQuit";
            this.AccountQuit.Size = new System.Drawing.Size(73, 26);
            this.AccountQuit.TabIndex = 0;
            this.AccountQuit.Text = "退出";
            this.AccountQuit.UseVisualStyleBackColor = true;
            this.AccountQuit.Click += new System.EventHandler(this.AccountQuit_Click);
            // 
            // Account
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(315, 249);
            this.Controls.Add(this.AccountQuit);
            this.Name = "Account";
            this.Text = "账户管理";
            this.Load += new System.EventHandler(this.Account_Load);
            this.Resize += new System.EventHandler(this.Account_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AccountQuit;
    }
}