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
            this.OptionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SystemStatus = new System.Windows.Forms.StatusStrip();
            this.CompanyNameLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Split1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.UserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.Split2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.DbName = new System.Windows.Forms.ToolStripStatusLabel();
            this.FuzzySearch = new System.Windows.Forms.GroupBox();
            this.ChkSearchPsam = new System.Windows.Forms.CheckBox();
            this.listSearchResult = new System.Windows.Forms.ListView();
            this.btnSearch = new System.Windows.Forms.Button();
            this.textSearchContent = new System.Windows.Forms.TextBox();
            this.LabelTextLike = new System.Windows.Forms.Label();
            this.LabelText = new System.Windows.Forms.Label();
            this.cmbCondition = new System.Windows.Forms.ComboBox();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.MainMenu.SuspendLayout();
            this.SystemStatus.SuspendLayout();
            this.FuzzySearch.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SystemMenuItem,
            this.RechargeMenuItem,
            this.CardOperatingMenuItem,
            this.OptionMenuItem,
            this.HelpMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1072, 25);
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
            this.Split1,
            this.UserName,
            this.Split2,
            this.DbName});
            this.SystemStatus.Location = new System.Drawing.Point(0, 681);
            this.SystemStatus.Name = "SystemStatus";
            this.SystemStatus.Size = new System.Drawing.Size(1072, 22);
            this.SystemStatus.TabIndex = 2;
            // 
            // CompanyNameLabel
            // 
            this.CompanyNameLabel.Name = "CompanyNameLabel";
            this.CompanyNameLabel.Size = new System.Drawing.Size(212, 17);
            this.CompanyNameLabel.Text = "张家港富耐特新能源智能系统有限公司";
            // 
            // Split1
            // 
            this.Split1.Name = "Split1";
            this.Split1.Size = new System.Drawing.Size(24, 17);
            this.Split1.Text = "    ";
            // 
            // UserName
            // 
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(44, 17);
            this.UserName.Text = "登录名";
            // 
            // Split2
            // 
            this.Split2.Name = "Split2";
            this.Split2.Size = new System.Drawing.Size(24, 17);
            this.Split2.Text = "    ";
            // 
            // DbName
            // 
            this.DbName.Name = "DbName";
            this.DbName.Size = new System.Drawing.Size(68, 17);
            this.DbName.Text = "数据库信息";
            // 
            // FuzzySearch
            // 
            this.FuzzySearch.Controls.Add(this.ChkSearchPsam);
            this.FuzzySearch.Controls.Add(this.listSearchResult);
            this.FuzzySearch.Controls.Add(this.btnSearch);
            this.FuzzySearch.Controls.Add(this.textSearchContent);
            this.FuzzySearch.Controls.Add(this.LabelTextLike);
            this.FuzzySearch.Controls.Add(this.LabelText);
            this.FuzzySearch.Controls.Add(this.cmbCondition);
            this.FuzzySearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FuzzySearch.Location = new System.Drawing.Point(0, 0);
            this.FuzzySearch.Name = "FuzzySearch";
            this.FuzzySearch.Size = new System.Drawing.Size(1072, 656);
            this.FuzzySearch.TabIndex = 6;
            this.FuzzySearch.TabStop = false;
            // 
            // ChkSearchPsam
            // 
            this.ChkSearchPsam.AutoSize = true;
            this.ChkSearchPsam.Location = new System.Drawing.Point(456, 68);
            this.ChkSearchPsam.Name = "ChkSearchPsam";
            this.ChkSearchPsam.Size = new System.Drawing.Size(66, 16);
            this.ChkSearchPsam.TabIndex = 6;
            this.ChkSearchPsam.Text = "查SAM卡";
            this.ChkSearchPsam.UseVisualStyleBackColor = true;
            this.ChkSearchPsam.CheckedChanged += new System.EventHandler(this.ChkSearchPsam_CheckedChanged);
            // 
            // listSearchResult
            // 
            this.listSearchResult.Location = new System.Drawing.Point(20, 99);
            this.listSearchResult.Name = "listSearchResult";
            this.listSearchResult.Size = new System.Drawing.Size(1041, 546);
            this.listSearchResult.TabIndex = 5;
            this.listSearchResult.UseCompatibleStateImageBehavior = false;
            this.listSearchResult.View = System.Windows.Forms.View.Details;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(575, 24);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 32);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询卡信息";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // textSearchContent
            // 
            this.textSearchContent.Location = new System.Drawing.Point(319, 31);
            this.textSearchContent.Name = "textSearchContent";
            this.textSearchContent.Size = new System.Drawing.Size(209, 21);
            this.textSearchContent.TabIndex = 2;
            // 
            // LabelTextLike
            // 
            this.LabelTextLike.AutoSize = true;
            this.LabelTextLike.Location = new System.Drawing.Point(260, 34);
            this.LabelTextLike.Name = "LabelTextLike";
            this.LabelTextLike.Size = new System.Drawing.Size(53, 12);
            this.LabelTextLike.TabIndex = 3;
            this.LabelTextLike.Text = "查询内容";
            // 
            // LabelText
            // 
            this.LabelText.AutoSize = true;
            this.LabelText.Location = new System.Drawing.Point(49, 34);
            this.LabelText.Name = "LabelText";
            this.LabelText.Size = new System.Drawing.Size(53, 12);
            this.LabelText.TabIndex = 0;
            this.LabelText.Text = "查询条件";
            // 
            // cmbCondition
            // 
            this.cmbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCondition.FormattingEnabled = true;
            this.cmbCondition.Location = new System.Drawing.Point(108, 31);
            this.cmbCondition.Name = "cmbCondition";
            this.cmbCondition.Size = new System.Drawing.Size(121, 20);
            this.cmbCondition.TabIndex = 1;
            this.cmbCondition.SelectedIndexChanged += new System.EventHandler(this.cmbCondition_SelectedIndexChanged);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.FuzzySearch);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 25);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1072, 656);
            this.MainPanel.TabIndex = 3;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 703);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.SystemStatus);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.Name = "Main";
            this.Text = "FNT Manage System";
            this.Load += new System.EventHandler(this.Main_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.SystemStatus.ResumeLayout(false);
            this.SystemStatus.PerformLayout();
            this.FuzzySearch.ResumeLayout(false);
            this.FuzzySearch.PerformLayout();
            this.MainPanel.ResumeLayout(false);
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
        private System.Windows.Forms.StatusStrip SystemStatus;
        private System.Windows.Forms.ToolStripStatusLabel CompanyNameLabel;
        private System.Windows.Forms.ToolStripStatusLabel UserName;
        private System.Windows.Forms.ToolStripStatusLabel Split1;
        private System.Windows.Forms.ToolStripStatusLabel DbName;
        private System.Windows.Forms.ToolStripStatusLabel Split2;
        private System.Windows.Forms.Label LabelText;
        private System.Windows.Forms.ComboBox cmbCondition;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label LabelTextLike;
        private System.Windows.Forms.TextBox textSearchContent;
        private System.Windows.Forms.ListView listSearchResult;
        private System.Windows.Forms.GroupBox FuzzySearch;
        private System.Windows.Forms.CheckBox ChkSearchPsam;
        private System.Windows.Forms.Panel MainPanel;
    }
}

