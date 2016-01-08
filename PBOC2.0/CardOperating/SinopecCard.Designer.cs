namespace CardOperating
{
    partial class SinopecCard
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
            this.UserCardId = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRePsamCard = new System.Windows.Forms.Button();
            this.cmbCardType = new System.Windows.Forms.ComboBox();
            this.OutputText = new System.Windows.Forms.RichTextBox();
            this.groupPsam = new System.Windows.Forms.GroupBox();
            this.PsamId = new System.Windows.Forms.TextBox();
            this.TermialID = new System.Windows.Forms.TextBox();
            this.cmbClientName = new System.Windows.Forms.ComboBox();
            this.btnPsamCard = new System.Windows.Forms.Button();
            this.LabelClient = new System.Windows.Forms.Label();
            this.LabelPsamId = new System.Windows.Forms.Label();
            this.LabelTerminalId = new System.Windows.Forms.Label();
            this.btnReUserCard = new System.Windows.Forms.Button();
            this.LabelCardId = new System.Windows.Forms.Label();
            this.LabelCardType = new System.Windows.Forms.Label();
            this.groupUser = new System.Windows.Forms.GroupBox();
            this.btnUserCard = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnPsamRefresh = new System.Windows.Forms.Button();
            this.CardIdRefresh = new System.Windows.Forms.Button();
            this.ProvCode = new System.Windows.Forms.Label();
            this.cmbProvCode = new System.Windows.Forms.ComboBox();
            this.groupPsam.SuspendLayout();
            this.groupUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // UserCardId
            // 
            this.UserCardId.Location = new System.Drawing.Point(100, 61);
            this.UserCardId.Name = "UserCardId";
            this.UserCardId.ReadOnly = true;
            this.UserCardId.Size = new System.Drawing.Size(199, 21);
            this.UserCardId.TabIndex = 3;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(65, 14);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(102, 23);
            this.btnConnect.TabIndex = 18;
            this.btnConnect.Text = "连接读卡器";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnRePsamCard
            // 
            this.btnRePsamCard.Location = new System.Drawing.Point(338, 78);
            this.btnRePsamCard.Name = "btnRePsamCard";
            this.btnRePsamCard.Size = new System.Drawing.Size(75, 23);
            this.btnRePsamCard.TabIndex = 8;
            this.btnRePsamCard.Text = "重制";
            this.btnRePsamCard.UseVisualStyleBackColor = true;
            this.btnRePsamCard.Click += new System.EventHandler(this.btnRePsamCard_Click);
            // 
            // cmbCardType
            // 
            this.cmbCardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCardType.FormattingEnabled = true;
            this.cmbCardType.Items.AddRange(new object[] {
            "用户卡",
            "管理卡",
            "员工卡",
            "维修卡"});
            this.cmbCardType.Location = new System.Drawing.Point(100, 25);
            this.cmbCardType.Name = "cmbCardType";
            this.cmbCardType.Size = new System.Drawing.Size(121, 20);
            this.cmbCardType.TabIndex = 4;
            // 
            // OutputText
            // 
            this.OutputText.Location = new System.Drawing.Point(15, 364);
            this.OutputText.Name = "OutputText";
            this.OutputText.Size = new System.Drawing.Size(421, 252);
            this.OutputText.TabIndex = 16;
            this.OutputText.Text = "";
            // 
            // groupPsam
            // 
            this.groupPsam.Controls.Add(this.btnRePsamCard);
            this.groupPsam.Controls.Add(this.btnPsamRefresh);
            this.groupPsam.Controls.Add(this.PsamId);
            this.groupPsam.Controls.Add(this.TermialID);
            this.groupPsam.Controls.Add(this.cmbClientName);
            this.groupPsam.Controls.Add(this.btnPsamCard);
            this.groupPsam.Controls.Add(this.LabelClient);
            this.groupPsam.Controls.Add(this.LabelPsamId);
            this.groupPsam.Controls.Add(this.LabelTerminalId);
            this.groupPsam.Location = new System.Drawing.Point(16, 212);
            this.groupPsam.Name = "groupPsam";
            this.groupPsam.Size = new System.Drawing.Size(420, 132);
            this.groupPsam.TabIndex = 15;
            this.groupPsam.TabStop = false;
            this.groupPsam.Text = "PSAM卡";
            // 
            // PsamId
            // 
            this.PsamId.Location = new System.Drawing.Point(99, 100);
            this.PsamId.Name = "PsamId";
            this.PsamId.ReadOnly = true;
            this.PsamId.Size = new System.Drawing.Size(200, 21);
            this.PsamId.TabIndex = 6;
            // 
            // TermialID
            // 
            this.TermialID.Location = new System.Drawing.Point(99, 62);
            this.TermialID.Name = "TermialID";
            this.TermialID.ReadOnly = true;
            this.TermialID.Size = new System.Drawing.Size(200, 21);
            this.TermialID.TabIndex = 5;
            // 
            // cmbClientName
            // 
            this.cmbClientName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClientName.FormattingEnabled = true;
            this.cmbClientName.Location = new System.Drawing.Point(100, 25);
            this.cmbClientName.Name = "cmbClientName";
            this.cmbClientName.Size = new System.Drawing.Size(151, 20);
            this.cmbClientName.TabIndex = 4;
            // 
            // btnPsamCard
            // 
            this.btnPsamCard.Location = new System.Drawing.Point(338, 41);
            this.btnPsamCard.Name = "btnPsamCard";
            this.btnPsamCard.Size = new System.Drawing.Size(75, 23);
            this.btnPsamCard.TabIndex = 3;
            this.btnPsamCard.Text = "制卡";
            this.btnPsamCard.UseVisualStyleBackColor = true;
            this.btnPsamCard.Click += new System.EventHandler(this.btnPsamCard_Click);
            // 
            // LabelClient
            // 
            this.LabelClient.AutoSize = true;
            this.LabelClient.Location = new System.Drawing.Point(29, 27);
            this.LabelClient.Name = "LabelClient";
            this.LabelClient.Size = new System.Drawing.Size(53, 12);
            this.LabelClient.TabIndex = 0;
            this.LabelClient.Text = "所属单位";
            // 
            // LabelPsamId
            // 
            this.LabelPsamId.AutoSize = true;
            this.LabelPsamId.Location = new System.Drawing.Point(28, 103);
            this.LabelPsamId.Name = "LabelPsamId";
            this.LabelPsamId.Size = new System.Drawing.Size(59, 12);
            this.LabelPsamId.TabIndex = 2;
            this.LabelPsamId.Text = "SAM序列号";
            // 
            // LabelTerminalId
            // 
            this.LabelTerminalId.AutoSize = true;
            this.LabelTerminalId.Location = new System.Drawing.Point(28, 65);
            this.LabelTerminalId.Name = "LabelTerminalId";
            this.LabelTerminalId.Size = new System.Drawing.Size(65, 12);
            this.LabelTerminalId.TabIndex = 1;
            this.LabelTerminalId.Text = "终端机编号";
            // 
            // btnReUserCard
            // 
            this.btnReUserCard.Location = new System.Drawing.Point(338, 59);
            this.btnReUserCard.Name = "btnReUserCard";
            this.btnReUserCard.Size = new System.Drawing.Size(75, 23);
            this.btnReUserCard.TabIndex = 5;
            this.btnReUserCard.Text = "重制";
            this.btnReUserCard.UseVisualStyleBackColor = true;
            this.btnReUserCard.Click += new System.EventHandler(this.btnReUserCard_Click);
            // 
            // LabelCardId
            // 
            this.LabelCardId.AutoSize = true;
            this.LabelCardId.Location = new System.Drawing.Point(29, 64);
            this.LabelCardId.Name = "LabelCardId";
            this.LabelCardId.Size = new System.Drawing.Size(29, 12);
            this.LabelCardId.TabIndex = 1;
            this.LabelCardId.Text = "卡号";
            // 
            // LabelCardType
            // 
            this.LabelCardType.AutoSize = true;
            this.LabelCardType.Location = new System.Drawing.Point(29, 28);
            this.LabelCardType.Name = "LabelCardType";
            this.LabelCardType.Size = new System.Drawing.Size(53, 12);
            this.LabelCardType.TabIndex = 0;
            this.LabelCardType.Text = "卡片类型";
            // 
            // groupUser
            // 
            this.groupUser.Controls.Add(this.CardIdRefresh);
            this.groupUser.Controls.Add(this.btnReUserCard);
            this.groupUser.Controls.Add(this.cmbCardType);
            this.groupUser.Controls.Add(this.UserCardId);
            this.groupUser.Controls.Add(this.btnUserCard);
            this.groupUser.Controls.Add(this.LabelCardType);
            this.groupUser.Controls.Add(this.LabelCardId);
            this.groupUser.Location = new System.Drawing.Point(16, 98);
            this.groupUser.Name = "groupUser";
            this.groupUser.Size = new System.Drawing.Size(421, 94);
            this.groupUser.TabIndex = 14;
            this.groupUser.TabStop = false;
            this.groupUser.Text = "用户卡";
            // 
            // btnUserCard
            // 
            this.btnUserCard.Location = new System.Drawing.Point(338, 23);
            this.btnUserCard.Name = "btnUserCard";
            this.btnUserCard.Size = new System.Drawing.Size(75, 23);
            this.btnUserCard.TabIndex = 2;
            this.btnUserCard.Text = "制卡";
            this.btnUserCard.UseVisualStyleBackColor = true;
            this.btnUserCard.Click += new System.EventHandler(this.btnUserCard_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(257, 14);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(79, 23);
            this.btnDisconnect.TabIndex = 19;
            this.btnDisconnect.Text = "断开读卡器";
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnPsamRefresh
            // 
            this.btnPsamRefresh.BackgroundImage = global::CardOperating.Properties.Resources.Refresh;
            this.btnPsamRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPsamRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPsamRefresh.Location = new System.Drawing.Point(305, 99);
            this.btnPsamRefresh.Name = "btnPsamRefresh";
            this.btnPsamRefresh.Size = new System.Drawing.Size(22, 21);
            this.btnPsamRefresh.TabIndex = 8;
            this.btnPsamRefresh.UseVisualStyleBackColor = true;
            this.btnPsamRefresh.Click += new System.EventHandler(this.btnPsamRefresh_Click);
            // 
            // CardIdRefresh
            // 
            this.CardIdRefresh.BackgroundImage = global::CardOperating.Properties.Resources.Refresh;
            this.CardIdRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CardIdRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CardIdRefresh.Location = new System.Drawing.Point(301, 61);
            this.CardIdRefresh.Name = "CardIdRefresh";
            this.CardIdRefresh.Size = new System.Drawing.Size(22, 21);
            this.CardIdRefresh.TabIndex = 7;
            this.CardIdRefresh.UseVisualStyleBackColor = true;
            this.CardIdRefresh.Click += new System.EventHandler(this.CardIdRefresh_Click);
            // 
            // ProvCode
            // 
            this.ProvCode.AutoSize = true;
            this.ProvCode.Location = new System.Drawing.Point(45, 58);
            this.ProvCode.Name = "ProvCode";
            this.ProvCode.Size = new System.Drawing.Size(41, 12);
            this.ProvCode.TabIndex = 20;
            this.ProvCode.Text = "地区号";
            // 
            // cmbProvCode
            // 
            this.cmbProvCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProvCode.FormattingEnabled = true;
            this.cmbProvCode.Items.AddRange(new object[] {
            "中石化",
            "北京",
            "天津",
            "河北",
            "山西",
            "上海",
            "江苏",
            "浙江",
            "安徽",
            "福建",
            "江西",
            "山东",
            "河南",
            "湖北",
            "湖南",
            "广东",
            "广西",
            "海南",
            "贵州",
            "云南",
            "深圳"});
            this.cmbProvCode.Location = new System.Drawing.Point(102, 55);
            this.cmbProvCode.Name = "cmbProvCode";
            this.cmbProvCode.Size = new System.Drawing.Size(90, 20);
            this.cmbProvCode.TabIndex = 21;
            // 
            // SinopecCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 630);
            this.Controls.Add(this.cmbProvCode);
            this.Controls.Add(this.ProvCode);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.OutputText);
            this.Controls.Add(this.groupPsam);
            this.Controls.Add(this.groupUser);
            this.Controls.Add(this.btnDisconnect);
            this.Name = "SinopecCard";
            this.Text = "制中石化卡";
            this.Load += new System.EventHandler(this.SinopecCard_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SinopecCard_FormClosing);
            this.groupPsam.ResumeLayout(false);
            this.groupPsam.PerformLayout();
            this.groupUser.ResumeLayout(false);
            this.groupUser.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox UserCardId;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRePsamCard;
        private System.Windows.Forms.ComboBox cmbCardType;
        private System.Windows.Forms.RichTextBox OutputText;
        private System.Windows.Forms.GroupBox groupPsam;
        private System.Windows.Forms.Button btnPsamRefresh;
        private System.Windows.Forms.TextBox PsamId;
        private System.Windows.Forms.TextBox TermialID;
        private System.Windows.Forms.ComboBox cmbClientName;
        private System.Windows.Forms.Button btnPsamCard;
        private System.Windows.Forms.Label LabelClient;
        private System.Windows.Forms.Label LabelPsamId;
        private System.Windows.Forms.Label LabelTerminalId;
        private System.Windows.Forms.Button btnReUserCard;
        private System.Windows.Forms.Button CardIdRefresh;
        private System.Windows.Forms.Label LabelCardId;
        private System.Windows.Forms.Label LabelCardType;
        private System.Windows.Forms.GroupBox groupUser;
        private System.Windows.Forms.Button btnUserCard;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label ProvCode;
        private System.Windows.Forms.ComboBox cmbProvCode;
    }
}