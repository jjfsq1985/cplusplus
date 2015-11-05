namespace CardOperating
{
    partial class CardOperating
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CardOprQuit = new System.Windows.Forms.Button();
            this.btnInitCard = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.OutputText = new System.Windows.Forms.RichTextBox();
            this.Card = new System.Windows.Forms.GroupBox();
            this.btnLoyalty = new System.Windows.Forms.Button();
            this.ContactCard = new System.Windows.Forms.CheckBox();
            this.btnUserCardReset = new System.Windows.Forms.Button();
            this.UserCardSetting = new System.Windows.Forms.Button();
            this.btnApplication = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCloseCard = new System.Windows.Forms.Button();
            this.btnOpenCard = new System.Windows.Forms.Button();
            this.ICC_Card = new System.Windows.Forms.GroupBox();
            this.btnIccCardReset = new System.Windows.Forms.Button();
            this.IccCardSetting = new System.Windows.Forms.Button();
            this.btnIccAppKey = new System.Windows.Forms.Button();
            this.btnIccCreate = new System.Windows.Forms.Button();
            this.btnCloseIccCard = new System.Windows.Forms.Button();
            this.btnOpenIccCard = new System.Windows.Forms.Button();
            this.btnInitIccCard = new System.Windows.Forms.Button();
            this.btnCleanInfo = new System.Windows.Forms.Button();
            this.CardInfoPanel = new System.Windows.Forms.Panel();
            this.cmbDevType = new System.Windows.Forms.ComboBox();
            this.Card.SuspendLayout();
            this.ICC_Card.SuspendLayout();
            this.SuspendLayout();
            // 
            // CardOprQuit
            // 
            this.CardOprQuit.Location = new System.Drawing.Point(431, 607);
            this.CardOprQuit.Name = "CardOprQuit";
            this.CardOprQuit.Size = new System.Drawing.Size(75, 23);
            this.CardOprQuit.TabIndex = 8;
            this.CardOprQuit.Text = "退出";
            this.CardOprQuit.UseVisualStyleBackColor = true;
            this.CardOprQuit.Click += new System.EventHandler(this.CardOprQuit_Click);
            // 
            // btnInitCard
            // 
            this.btnInitCard.Location = new System.Drawing.Point(17, 71);
            this.btnInitCard.Name = "btnInitCard";
            this.btnInitCard.Size = new System.Drawing.Size(75, 23);
            this.btnInitCard.TabIndex = 3;
            this.btnInitCard.Text = "初始化";
            this.btnInitCard.UseVisualStyleBackColor = true;
            this.btnInitCard.Click += new System.EventHandler(this.btnInitCard_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(184, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "建立连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(265, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "断开连接";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // OutputText
            // 
            this.OutputText.Location = new System.Drawing.Point(125, 45);
            this.OutputText.Name = "OutputText";
            this.OutputText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.OutputText.Size = new System.Drawing.Size(381, 555);
            this.OutputText.TabIndex = 6;
            this.OutputText.Text = "";
            // 
            // Card
            // 
            this.Card.BackColor = System.Drawing.SystemColors.Control;
            this.Card.Controls.Add(this.btnLoyalty);
            this.Card.Controls.Add(this.ContactCard);
            this.Card.Controls.Add(this.btnUserCardReset);
            this.Card.Controls.Add(this.UserCardSetting);
            this.Card.Controls.Add(this.btnApplication);
            this.Card.Controls.Add(this.btnCreate);
            this.Card.Controls.Add(this.btnCloseCard);
            this.Card.Controls.Add(this.btnOpenCard);
            this.Card.Controls.Add(this.btnInitCard);
            this.Card.Location = new System.Drawing.Point(4, 45);
            this.Card.Name = "Card";
            this.Card.Size = new System.Drawing.Size(115, 291);
            this.Card.TabIndex = 4;
            this.Card.TabStop = false;
            this.Card.Text = "用户卡";
            // 
            // btnLoyalty
            // 
            this.btnLoyalty.Location = new System.Drawing.Point(17, 233);
            this.btnLoyalty.Name = "btnLoyalty";
            this.btnLoyalty.Size = new System.Drawing.Size(75, 23);
            this.btnLoyalty.TabIndex = 10;
            this.btnLoyalty.Text = "积分应用";
            this.btnLoyalty.UseVisualStyleBackColor = true;
            this.btnLoyalty.Click += new System.EventHandler(this.btnLoyalty_Click);
            // 
            // ContactCard
            // 
            this.ContactCard.AutoSize = true;
            this.ContactCard.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ContactCard.Location = new System.Drawing.Point(24, 51);
            this.ContactCard.Name = "ContactCard";
            this.ContactCard.Size = new System.Drawing.Size(60, 16);
            this.ContactCard.TabIndex = 9;
            this.ContactCard.Text = "接触式";
            this.ContactCard.UseVisualStyleBackColor = true;
            // 
            // btnUserCardReset
            // 
            this.btnUserCardReset.Location = new System.Drawing.Point(17, 104);
            this.btnUserCardReset.Name = "btnUserCardReset";
            this.btnUserCardReset.Size = new System.Drawing.Size(75, 23);
            this.btnUserCardReset.TabIndex = 4;
            this.btnUserCardReset.Text = "清空重置";
            this.btnUserCardReset.UseVisualStyleBackColor = true;
            this.btnUserCardReset.Click += new System.EventHandler(this.btnUserCardReset_Click);
            // 
            // UserCardSetting
            // 
            this.UserCardSetting.Location = new System.Drawing.Point(17, 170);
            this.UserCardSetting.Name = "UserCardSetting";
            this.UserCardSetting.Size = new System.Drawing.Size(75, 23);
            this.UserCardSetting.TabIndex = 6;
            this.UserCardSetting.Text = "信息设置";
            this.UserCardSetting.UseVisualStyleBackColor = true;
            this.UserCardSetting.Click += new System.EventHandler(this.UserCardSetting_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.Location = new System.Drawing.Point(17, 203);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(75, 23);
            this.btnApplication.TabIndex = 7;
            this.btnApplication.Text = "加气应用";
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(17, 137);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 5;
            this.btnCreate.Text = "安装密钥";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCloseCard
            // 
            this.btnCloseCard.Location = new System.Drawing.Point(17, 262);
            this.btnCloseCard.Name = "btnCloseCard";
            this.btnCloseCard.Size = new System.Drawing.Size(75, 23);
            this.btnCloseCard.TabIndex = 8;
            this.btnCloseCard.Text = "关闭";
            this.btnCloseCard.UseVisualStyleBackColor = true;
            this.btnCloseCard.Click += new System.EventHandler(this.btnCloseCard_Click);
            // 
            // btnOpenCard
            // 
            this.btnOpenCard.Location = new System.Drawing.Point(17, 22);
            this.btnOpenCard.Name = "btnOpenCard";
            this.btnOpenCard.Size = new System.Drawing.Size(75, 23);
            this.btnOpenCard.TabIndex = 0;
            this.btnOpenCard.Text = "打开";
            this.btnOpenCard.UseVisualStyleBackColor = true;
            this.btnOpenCard.Click += new System.EventHandler(this.btnOpenCard_Click);
            // 
            // ICC_Card
            // 
            this.ICC_Card.Controls.Add(this.btnIccCardReset);
            this.ICC_Card.Controls.Add(this.IccCardSetting);
            this.ICC_Card.Controls.Add(this.btnIccAppKey);
            this.ICC_Card.Controls.Add(this.btnIccCreate);
            this.ICC_Card.Controls.Add(this.btnCloseIccCard);
            this.ICC_Card.Controls.Add(this.btnOpenIccCard);
            this.ICC_Card.Controls.Add(this.btnInitIccCard);
            this.ICC_Card.Location = new System.Drawing.Point(4, 342);
            this.ICC_Card.Name = "ICC_Card";
            this.ICC_Card.Size = new System.Drawing.Size(115, 258);
            this.ICC_Card.TabIndex = 5;
            this.ICC_Card.TabStop = false;
            this.ICC_Card.Text = "SAM卡";
            // 
            // btnIccCardReset
            // 
            this.btnIccCardReset.Location = new System.Drawing.Point(12, 126);
            this.btnIccCardReset.Name = "btnIccCardReset";
            this.btnIccCardReset.Size = new System.Drawing.Size(75, 23);
            this.btnIccCardReset.TabIndex = 2;
            this.btnIccCardReset.Text = "清空重置";
            this.btnIccCardReset.UseVisualStyleBackColor = true;
            this.btnIccCardReset.Click += new System.EventHandler(this.btnIccCardReset_Click);
            // 
            // IccCardSetting
            // 
            this.IccCardSetting.Location = new System.Drawing.Point(12, 58);
            this.IccCardSetting.Name = "IccCardSetting";
            this.IccCardSetting.Size = new System.Drawing.Size(75, 23);
            this.IccCardSetting.TabIndex = 3;
            this.IccCardSetting.Text = "信息设置";
            this.IccCardSetting.UseVisualStyleBackColor = true;
            this.IccCardSetting.Click += new System.EventHandler(this.IccCardSetting_Click);
            // 
            // btnIccAppKey
            // 
            this.btnIccAppKey.Location = new System.Drawing.Point(12, 194);
            this.btnIccAppKey.Name = "btnIccAppKey";
            this.btnIccAppKey.Size = new System.Drawing.Size(75, 23);
            this.btnIccAppKey.TabIndex = 5;
            this.btnIccAppKey.Text = "应用密钥";
            this.btnIccAppKey.UseVisualStyleBackColor = true;
            this.btnIccAppKey.Click += new System.EventHandler(this.btnIccAppKey_Click);
            // 
            // btnIccCreate
            // 
            this.btnIccCreate.Location = new System.Drawing.Point(12, 160);
            this.btnIccCreate.Name = "btnIccCreate";
            this.btnIccCreate.Size = new System.Drawing.Size(75, 23);
            this.btnIccCreate.TabIndex = 4;
            this.btnIccCreate.Text = "业务应用";
            this.btnIccCreate.UseVisualStyleBackColor = true;
            this.btnIccCreate.Click += new System.EventHandler(this.btnIccCreate_Click);
            // 
            // btnCloseIccCard
            // 
            this.btnCloseIccCard.Location = new System.Drawing.Point(12, 228);
            this.btnCloseIccCard.Name = "btnCloseIccCard";
            this.btnCloseIccCard.Size = new System.Drawing.Size(75, 23);
            this.btnCloseIccCard.TabIndex = 6;
            this.btnCloseIccCard.Text = "关闭";
            this.btnCloseIccCard.UseVisualStyleBackColor = true;
            this.btnCloseIccCard.Click += new System.EventHandler(this.btnCloseIccCard_Click);
            // 
            // btnOpenIccCard
            // 
            this.btnOpenIccCard.Location = new System.Drawing.Point(12, 24);
            this.btnOpenIccCard.Name = "btnOpenIccCard";
            this.btnOpenIccCard.Size = new System.Drawing.Size(75, 23);
            this.btnOpenIccCard.TabIndex = 0;
            this.btnOpenIccCard.Text = "打开";
            this.btnOpenIccCard.UseVisualStyleBackColor = true;
            this.btnOpenIccCard.Click += new System.EventHandler(this.btnOpenIccCard_Click);
            // 
            // btnInitIccCard
            // 
            this.btnInitIccCard.Location = new System.Drawing.Point(12, 92);
            this.btnInitIccCard.Name = "btnInitIccCard";
            this.btnInitIccCard.Size = new System.Drawing.Size(75, 23);
            this.btnInitIccCard.TabIndex = 1;
            this.btnInitIccCard.Text = "初始化";
            this.btnInitIccCard.UseVisualStyleBackColor = true;
            this.btnInitIccCard.Click += new System.EventHandler(this.btnInitIccCard_Click);
            // 
            // btnCleanInfo
            // 
            this.btnCleanInfo.Location = new System.Drawing.Point(334, 607);
            this.btnCleanInfo.Name = "btnCleanInfo";
            this.btnCleanInfo.Size = new System.Drawing.Size(91, 23);
            this.btnCleanInfo.TabIndex = 7;
            this.btnCleanInfo.Text = "清除信息";
            this.btnCleanInfo.UseVisualStyleBackColor = true;
            this.btnCleanInfo.Click += new System.EventHandler(this.btnCleanInfo_Click);
            // 
            // CardInfoPanel
            // 
            this.CardInfoPanel.Location = new System.Drawing.Point(518, 10);
            this.CardInfoPanel.Name = "CardInfoPanel";
            this.CardInfoPanel.Size = new System.Drawing.Size(560, 610);
            this.CardInfoPanel.TabIndex = 9;
            this.CardInfoPanel.Visible = false;
            // 
            // cmbDevType
            // 
            this.cmbDevType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevType.FormattingEnabled = true;
            this.cmbDevType.Items.AddRange(new object[] {
            "达华-明泰 MT3",
            "龙寰-Duali DE-620",
            "龙寰-明泰 MT3"});
            this.cmbDevType.Location = new System.Drawing.Point(12, 13);
            this.cmbDevType.Name = "cmbDevType";
            this.cmbDevType.Size = new System.Drawing.Size(166, 20);
            this.cmbDevType.TabIndex = 0;
            // 
            // CardOperating
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(512, 632);
            this.Controls.Add(this.cmbDevType);
            this.Controls.Add(this.CardInfoPanel);
            this.Controls.Add(this.btnCleanInfo);
            this.Controls.Add(this.ICC_Card);
            this.Controls.Add(this.Card);
            this.Controls.Add(this.OutputText);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.CardOprQuit);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CardOperating";
            this.Text = "制卡操作";
            this.Load += new System.EventHandler(this.CardOperating_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CardOperating_FormClosing);
            this.Card.ResumeLayout(false);
            this.Card.PerformLayout();
            this.ICC_Card.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CardOprQuit;
        private System.Windows.Forms.Button btnInitCard;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.RichTextBox OutputText;
        private System.Windows.Forms.GroupBox Card;
        private System.Windows.Forms.GroupBox ICC_Card;
        private System.Windows.Forms.Button btnInitIccCard;
        private System.Windows.Forms.Button btnOpenCard;
        private System.Windows.Forms.Button btnOpenIccCard;
        private System.Windows.Forms.Button btnCleanInfo;
        private System.Windows.Forms.Button btnCloseCard;
        private System.Windows.Forms.Button btnCloseIccCard;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnIccCreate;
        private System.Windows.Forms.Button btnApplication;
        private System.Windows.Forms.Button btnIccAppKey;
        private System.Windows.Forms.Button UserCardSetting;
        private System.Windows.Forms.Button IccCardSetting;
        private System.Windows.Forms.Panel CardInfoPanel;
        private System.Windows.Forms.Button btnUserCardReset;
        private System.Windows.Forms.Button btnIccCardReset;
        private System.Windows.Forms.ComboBox cmbDevType;
        private System.Windows.Forms.CheckBox ContactCard;
        private System.Windows.Forms.Button btnLoyalty;
    }
}
