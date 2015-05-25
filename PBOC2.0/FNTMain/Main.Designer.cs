namespace FNTMain
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.SystemMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RechargeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CardOperatingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KeyManageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SystemStatus = new System.Windows.Forms.StatusStrip();
            this.CompanyNameLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Split = new System.Windows.Forms.ToolStripStatusLabel();
            this.UserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.BackGroundPic = new System.Windows.Forms.PictureBox();
            this.MainMenu.SuspendLayout();
            this.SystemStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BackGroundPic)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SystemMenuItem,
            this.RechargeMenuItem,
            this.CardOperatingMenuItem,
            this.KeyManageMenuItem,
            this.OptionMenuItem,
            this.HelpMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1071, 25);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "MainMenu";
            // 
            // SystemMenuItem
            // 
            this.SystemMenuItem.Name = "SystemMenuItem";
            this.SystemMenuItem.Size = new System.Drawing.Size(68, 21);
            this.SystemMenuItem.Text = "系统管理";
            // 
            // RechargeMenuItem
            // 
            this.RechargeMenuItem.Name = "RechargeMenuItem";
            this.RechargeMenuItem.Size = new System.Drawing.Size(68, 21);
            this.RechargeMenuItem.Text = "记录管理";
            // 
            // CardOperatingMenuItem
            // 
            this.CardOperatingMenuItem.Name = "CardOperatingMenuItem";
            this.CardOperatingMenuItem.Size = new System.Drawing.Size(68, 21);
            this.CardOperatingMenuItem.Text = "IC卡操作";
            // 
            // KeyManageMenuItem
            // 
            this.KeyManageMenuItem.Name = "KeyManageMenuItem";
            this.KeyManageMenuItem.Size = new System.Drawing.Size(68, 21);
            this.KeyManageMenuItem.Text = "密钥管理";
            // 
            // OptionMenuItem
            // 
            this.OptionMenuItem.Name = "OptionMenuItem";
            this.OptionMenuItem.Size = new System.Drawing.Size(44, 21);
            this.OptionMenuItem.Text = "选项";
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(44, 21);
            this.HelpMenuItem.Text = "帮助";
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(100, 22);
            this.AboutMenuItem.Text = "关于";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // SystemStatus
            // 
            this.SystemStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CompanyNameLabel,
            this.Split,
            this.UserName});
            this.SystemStatus.Location = new System.Drawing.Point(0, 666);
            this.SystemStatus.Name = "SystemStatus";
            this.SystemStatus.Size = new System.Drawing.Size(1071, 22);
            this.SystemStatus.TabIndex = 2;
            // 
            // CompanyName
            // 
            this.CompanyNameLabel.Name = "CompanyName";
            this.CompanyNameLabel.Size = new System.Drawing.Size(212, 17);
            this.CompanyNameLabel.Text = "张家港富耐特新能源智能系统有限公司";
            // 
            // Split
            // 
            this.Split.Name = "Split";
            this.Split.Size = new System.Drawing.Size(24, 17);
            this.Split.Text = "    ";
            // 
            // UserName
            // 
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(44, 17);
            this.UserName.Text = "登录名";
            // 
            // BackGroundPic
            // 
            this.BackGroundPic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BackGroundPic.Image = ((System.Drawing.Image)(resources.GetObject("BackGroundPic.Image")));
            this.BackGroundPic.Location = new System.Drawing.Point(812, 397);
            this.BackGroundPic.Name = "BackGroundPic";
            this.BackGroundPic.Size = new System.Drawing.Size(259, 266);
            this.BackGroundPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BackGroundPic.TabIndex = 4;
            this.BackGroundPic.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 688);
            this.Controls.Add(this.BackGroundPic);
            this.Controls.Add(this.SystemStatus);
            this.Controls.Add(this.MainMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "Main";
            this.Text = "FNT Manage System";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.SystemStatus.ResumeLayout(false);
            this.SystemStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BackGroundPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem SystemMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RechargeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OptionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CardOperatingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem KeyManageMenuItem;
        private System.Windows.Forms.StatusStrip SystemStatus;
        private System.Windows.Forms.ToolStripStatusLabel CompanyNameLabel;
        private System.Windows.Forms.ToolStripStatusLabel UserName;
        private System.Windows.Forms.ToolStripStatusLabel Split;
        private System.Windows.Forms.PictureBox BackGroundPic;
    }
}

