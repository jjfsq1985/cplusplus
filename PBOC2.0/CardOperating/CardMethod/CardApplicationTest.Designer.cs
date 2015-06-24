namespace CardOperating
{
    partial class CardApplicationTest
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
            this.groupApp = new System.Windows.Forms.GroupBox();
            this.btnUnload = new System.Windows.Forms.Button();
            this.Infomation = new System.Windows.Forms.Label();
            this.RecordInCard = new System.Windows.Forms.ListView();
            this.BusinessSn = new System.Windows.Forms.ColumnHeader();
            this.OverdraftMoney = new System.Windows.Forms.ColumnHeader();
            this.Amount = new System.Windows.Forms.ColumnHeader();
            this.BusinessType = new System.Windows.Forms.ColumnHeader();
            this.TermID = new System.Windows.Forms.ColumnHeader();
            this.BusinessTime = new System.Windows.Forms.ColumnHeader();
            this.btnReadRecord = new System.Windows.Forms.Button();
            this.textPIN = new System.Windows.Forms.TextBox();
            this.LabelPIN = new System.Windows.Forms.Label();
            this.groupCardTest = new System.Windows.Forms.GroupBox();
            this.btnLockCard = new System.Windows.Forms.Button();
            this.Purchase = new System.Windows.Forms.Label();
            this.btnUnlockCard = new System.Windows.Forms.Button();
            this.textPurchase = new System.Windows.Forms.TextBox();
            this.Unit3 = new System.Windows.Forms.Label();
            this.Balance = new System.Windows.Forms.Label();
            this.btnUnlockGrayCard = new System.Windows.Forms.Button();
            this.GrayFlag = new System.Windows.Forms.CheckBox();
            this.Unit1 = new System.Windows.Forms.Label();
            this.textBalance = new System.Windows.Forms.TextBox();
            this.btnBalance = new System.Windows.Forms.Button();
            this.Unit2 = new System.Windows.Forms.Label();
            this.Money = new System.Windows.Forms.Label();
            this.textMoney = new System.Windows.Forms.TextBox();
            this.btnCardLoad = new System.Windows.Forms.Button();
            this.groupApp.SuspendLayout();
            this.groupCardTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupApp
            // 
            this.groupApp.Controls.Add(this.btnUnload);
            this.groupApp.Controls.Add(this.Infomation);
            this.groupApp.Controls.Add(this.RecordInCard);
            this.groupApp.Controls.Add(this.btnReadRecord);
            this.groupApp.Controls.Add(this.textPIN);
            this.groupApp.Controls.Add(this.LabelPIN);
            this.groupApp.Controls.Add(this.groupCardTest);
            this.groupApp.Controls.Add(this.Balance);
            this.groupApp.Controls.Add(this.btnUnlockGrayCard);
            this.groupApp.Controls.Add(this.GrayFlag);
            this.groupApp.Controls.Add(this.Unit1);
            this.groupApp.Controls.Add(this.textBalance);
            this.groupApp.Controls.Add(this.btnBalance);
            this.groupApp.Controls.Add(this.Unit2);
            this.groupApp.Controls.Add(this.Money);
            this.groupApp.Controls.Add(this.textMoney);
            this.groupApp.Controls.Add(this.btnCardLoad);
            this.groupApp.Location = new System.Drawing.Point(2, 1);
            this.groupApp.Name = "groupApp";
            this.groupApp.Size = new System.Drawing.Size(438, 604);
            this.groupApp.TabIndex = 0;
            this.groupApp.TabStop = false;
            this.groupApp.Text = "卡应用";
            // 
            // btnUnload
            // 
            this.btnUnload.Location = new System.Drawing.Point(250, 185);
            this.btnUnload.Name = "btnUnload";
            this.btnUnload.Size = new System.Drawing.Size(40, 23);
            this.btnUnload.TabIndex = 38;
            this.btnUnload.Text = "圈提";
            this.btnUnload.UseVisualStyleBackColor = true;
            this.btnUnload.Click += new System.EventHandler(this.btnUnload_Click);
            // 
            // Infomation
            // 
            this.Infomation.AutoSize = true;
            this.Infomation.Location = new System.Drawing.Point(110, 31);
            this.Infomation.Name = "Infomation";
            this.Infomation.Size = new System.Drawing.Size(161, 12);
            this.Infomation.TabIndex = 37;
            this.Infomation.Text = "！使用数据库默认密钥操作！";
            // 
            // RecordInCard
            // 
            this.RecordInCard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.BusinessSn,
            this.OverdraftMoney,
            this.Amount,
            this.BusinessType,
            this.TermID,
            this.BusinessTime});
            this.RecordInCard.FullRowSelect = true;
            this.RecordInCard.GridLines = true;
            this.RecordInCard.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.RecordInCard.Location = new System.Drawing.Point(10, 396);
            this.RecordInCard.MultiSelect = false;
            this.RecordInCard.Name = "RecordInCard";
            this.RecordInCard.Size = new System.Drawing.Size(422, 184);
            this.RecordInCard.TabIndex = 36;
            this.RecordInCard.UseCompatibleStateImageBehavior = false;
            this.RecordInCard.View = System.Windows.Forms.View.Details;
            // 
            // BusinessSn
            // 
            this.BusinessSn.Text = "交易序号";
            // 
            // OverdraftMoney
            // 
            this.OverdraftMoney.Text = "透支限额";
            // 
            // Amount
            // 
            this.Amount.Text = "交易金额";
            // 
            // BusinessType
            // 
            this.BusinessType.Text = "交易类型";
            // 
            // TermID
            // 
            this.TermID.Text = "终端机编号";
            this.TermID.Width = 80;
            // 
            // BusinessTime
            // 
            this.BusinessTime.Text = "交易时间";
            this.BusinessTime.Width = 100;
            // 
            // btnReadRecord
            // 
            this.btnReadRecord.Location = new System.Drawing.Point(34, 367);
            this.btnReadRecord.Name = "btnReadRecord";
            this.btnReadRecord.Size = new System.Drawing.Size(75, 23);
            this.btnReadRecord.TabIndex = 35;
            this.btnReadRecord.Text = "读交易记录";
            this.btnReadRecord.UseVisualStyleBackColor = true;
            this.btnReadRecord.Click += new System.EventHandler(this.btnReadRecord_Click);
            // 
            // textPIN
            // 
            this.textPIN.Location = new System.Drawing.Point(112, 66);
            this.textPIN.MaxLength = 6;
            this.textPIN.Name = "textPIN";
            this.textPIN.Size = new System.Drawing.Size(100, 21);
            this.textPIN.TabIndex = 34;
            this.textPIN.Validated += new System.EventHandler(this.textPIN_Validated);
            this.textPIN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPIN_KeyPress);
            // 
            // LabelPIN
            // 
            this.LabelPIN.AutoSize = true;
            this.LabelPIN.Location = new System.Drawing.Point(21, 69);
            this.LabelPIN.Name = "LabelPIN";
            this.LabelPIN.Size = new System.Drawing.Size(83, 12);
            this.LabelPIN.TabIndex = 33;
            this.LabelPIN.Text = "请输入PIN码：";
            // 
            // groupCardTest
            // 
            this.groupCardTest.Controls.Add(this.btnLockCard);
            this.groupCardTest.Controls.Add(this.Purchase);
            this.groupCardTest.Controls.Add(this.btnUnlockCard);
            this.groupCardTest.Controls.Add(this.textPurchase);
            this.groupCardTest.Controls.Add(this.Unit3);
            this.groupCardTest.Location = new System.Drawing.Point(23, 229);
            this.groupCardTest.Name = "groupCardTest";
            this.groupCardTest.Size = new System.Drawing.Size(363, 101);
            this.groupCardTest.TabIndex = 32;
            this.groupCardTest.TabStop = false;
            this.groupCardTest.Text = "消费测试";
            // 
            // btnLockCard
            // 
            this.btnLockCard.Location = new System.Drawing.Point(29, 40);
            this.btnLockCard.Name = "btnLockCard";
            this.btnLockCard.Size = new System.Drawing.Size(57, 23);
            this.btnLockCard.TabIndex = 31;
            this.btnLockCard.Text = "卡锁定";
            this.btnLockCard.UseVisualStyleBackColor = true;
            this.btnLockCard.Click += new System.EventHandler(this.btnLockCard_Click);
            // 
            // Purchase
            // 
            this.Purchase.AutoSize = true;
            this.Purchase.Location = new System.Drawing.Point(103, 45);
            this.Purchase.Name = "Purchase";
            this.Purchase.Size = new System.Drawing.Size(41, 12);
            this.Purchase.TabIndex = 27;
            this.Purchase.Text = "消费：";
            // 
            // btnUnlockCard
            // 
            this.btnUnlockCard.Location = new System.Drawing.Point(277, 40);
            this.btnUnlockCard.Name = "btnUnlockCard";
            this.btnUnlockCard.Size = new System.Drawing.Size(64, 23);
            this.btnUnlockCard.TabIndex = 30;
            this.btnUnlockCard.Text = "扣款解锁";
            this.btnUnlockCard.UseVisualStyleBackColor = true;
            this.btnUnlockCard.Click += new System.EventHandler(this.btnUnlockCard_Click);
            // 
            // textPurchase
            // 
            this.textPurchase.Location = new System.Drawing.Point(145, 42);
            this.textPurchase.Name = "textPurchase";
            this.textPurchase.Size = new System.Drawing.Size(97, 21);
            this.textPurchase.TabIndex = 28;
            this.textPurchase.Text = "200.00";
            // 
            // Unit3
            // 
            this.Unit3.AutoSize = true;
            this.Unit3.Location = new System.Drawing.Point(248, 45);
            this.Unit3.Name = "Unit3";
            this.Unit3.Size = new System.Drawing.Size(17, 12);
            this.Unit3.TabIndex = 29;
            this.Unit3.Text = "元";
            // 
            // Balance
            // 
            this.Balance.AutoSize = true;
            this.Balance.Location = new System.Drawing.Point(21, 130);
            this.Balance.Name = "Balance";
            this.Balance.Size = new System.Drawing.Size(41, 12);
            this.Balance.TabIndex = 26;
            this.Balance.Text = "余额：";
            // 
            // btnUnlockGrayCard
            // 
            this.btnUnlockGrayCard.Location = new System.Drawing.Point(271, 153);
            this.btnUnlockGrayCard.Name = "btnUnlockGrayCard";
            this.btnUnlockGrayCard.Size = new System.Drawing.Size(64, 23);
            this.btnUnlockGrayCard.TabIndex = 25;
            this.btnUnlockGrayCard.Text = "强制解灰";
            this.btnUnlockGrayCard.UseVisualStyleBackColor = true;
            this.btnUnlockGrayCard.Click += new System.EventHandler(this.btnUnlockGrayCard_Click);
            // 
            // GrayFlag
            // 
            this.GrayFlag.AutoCheck = false;
            this.GrayFlag.AutoSize = true;
            this.GrayFlag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GrayFlag.Location = new System.Drawing.Point(204, 129);
            this.GrayFlag.Name = "GrayFlag";
            this.GrayFlag.Size = new System.Drawing.Size(45, 16);
            this.GrayFlag.TabIndex = 24;
            this.GrayFlag.Text = "灰锁";
            this.GrayFlag.UseVisualStyleBackColor = true;
            // 
            // Unit1
            // 
            this.Unit1.AutoSize = true;
            this.Unit1.Location = new System.Drawing.Point(165, 131);
            this.Unit1.Name = "Unit1";
            this.Unit1.Size = new System.Drawing.Size(17, 12);
            this.Unit1.TabIndex = 23;
            this.Unit1.Text = "元";
            // 
            // textBalance
            // 
            this.textBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBalance.Location = new System.Drawing.Point(63, 126);
            this.textBalance.Name = "textBalance";
            this.textBalance.ReadOnly = true;
            this.textBalance.Size = new System.Drawing.Size(97, 21);
            this.textBalance.TabIndex = 22;
            // 
            // btnBalance
            // 
            this.btnBalance.Location = new System.Drawing.Point(271, 123);
            this.btnBalance.Name = "btnBalance";
            this.btnBalance.Size = new System.Drawing.Size(64, 23);
            this.btnBalance.TabIndex = 21;
            this.btnBalance.Text = "读取余额";
            this.btnBalance.UseVisualStyleBackColor = true;
            this.btnBalance.Click += new System.EventHandler(this.btnBalance_Click);
            // 
            // Unit2
            // 
            this.Unit2.AutoSize = true;
            this.Unit2.Location = new System.Drawing.Point(165, 190);
            this.Unit2.Name = "Unit2";
            this.Unit2.Size = new System.Drawing.Size(17, 12);
            this.Unit2.TabIndex = 20;
            this.Unit2.Text = "元";
            // 
            // Money
            // 
            this.Money.AutoSize = true;
            this.Money.Location = new System.Drawing.Point(21, 189);
            this.Money.Name = "Money";
            this.Money.Size = new System.Drawing.Size(41, 12);
            this.Money.TabIndex = 19;
            this.Money.Text = "金额：";
            // 
            // textMoney
            // 
            this.textMoney.Location = new System.Drawing.Point(63, 185);
            this.textMoney.Name = "textMoney";
            this.textMoney.Size = new System.Drawing.Size(97, 21);
            this.textMoney.TabIndex = 18;
            this.textMoney.Text = "10,000.00";
            // 
            // btnCardLoad
            // 
            this.btnCardLoad.Location = new System.Drawing.Point(204, 185);
            this.btnCardLoad.Name = "btnCardLoad";
            this.btnCardLoad.Size = new System.Drawing.Size(40, 23);
            this.btnCardLoad.TabIndex = 17;
            this.btnCardLoad.Text = "圈存";
            this.btnCardLoad.UseVisualStyleBackColor = true;
            this.btnCardLoad.Click += new System.EventHandler(this.btnCardLoad_Click);
            // 
            // CardApplicationTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 606);
            this.Controls.Add(this.groupApp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CardApplicationTest";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "卡应用测试";
            this.groupApp.ResumeLayout(false);
            this.groupApp.PerformLayout();
            this.groupCardTest.ResumeLayout(false);
            this.groupCardTest.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupApp;
        private System.Windows.Forms.Label Unit2;
        private System.Windows.Forms.Label Money;
        private System.Windows.Forms.TextBox textMoney;
        private System.Windows.Forms.Button btnCardLoad;
        private System.Windows.Forms.Button btnBalance;
        private System.Windows.Forms.Label Unit1;
        private System.Windows.Forms.TextBox textBalance;
        private System.Windows.Forms.CheckBox GrayFlag;
        private System.Windows.Forms.Button btnUnlockGrayCard;
        private System.Windows.Forms.Label Balance;
        private System.Windows.Forms.Button btnUnlockCard;
        private System.Windows.Forms.Label Unit3;
        private System.Windows.Forms.TextBox textPurchase;
        private System.Windows.Forms.Label Purchase;
        private System.Windows.Forms.Button btnLockCard;
        private System.Windows.Forms.TextBox textPIN;
        private System.Windows.Forms.Label LabelPIN;
        private System.Windows.Forms.GroupBox groupCardTest;
        private System.Windows.Forms.ListView RecordInCard;
        private System.Windows.Forms.Button btnReadRecord;
        private System.Windows.Forms.ColumnHeader BusinessSn;
        private System.Windows.Forms.ColumnHeader OverdraftMoney;
        private System.Windows.Forms.ColumnHeader Amount;
        private System.Windows.Forms.ColumnHeader BusinessType;
        private System.Windows.Forms.ColumnHeader TermID;
        private System.Windows.Forms.ColumnHeader BusinessTime;
        private System.Windows.Forms.Label Infomation;
        private System.Windows.Forms.Button btnUnload;
    }
}