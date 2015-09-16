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
            this.tabApp = new System.Windows.Forms.TabControl();
            this.tabPageApp1 = new System.Windows.Forms.TabPage();
            this.btnUnload = new System.Windows.Forms.Button();
            this.textPIN = new System.Windows.Forms.TextBox();
            this.RecordInCard = new System.Windows.Forms.ListView();
            this.BusinessSn = new System.Windows.Forms.ColumnHeader();
            this.Amount = new System.Windows.Forms.ColumnHeader();
            this.BusinessType = new System.Windows.Forms.ColumnHeader();
            this.TermID = new System.Windows.Forms.ColumnHeader();
            this.BusinessTime = new System.Windows.Forms.ColumnHeader();
            this.btnCardLoad = new System.Windows.Forms.Button();
            this.btnReadRecord = new System.Windows.Forms.Button();
            this.textMoney = new System.Windows.Forms.TextBox();
            this.Money = new System.Windows.Forms.Label();
            this.LabelPIN = new System.Windows.Forms.Label();
            this.Unit2 = new System.Windows.Forms.Label();
            this.groupCardTest = new System.Windows.Forms.GroupBox();
            this.SamSlot = new System.Windows.Forms.CheckBox();
            this.btnLockCard = new System.Windows.Forms.Button();
            this.Purchase = new System.Windows.Forms.Label();
            this.btnUnlockCard = new System.Windows.Forms.Button();
            this.textPurchase = new System.Windows.Forms.TextBox();
            this.Unit3 = new System.Windows.Forms.Label();
            this.btnBalance = new System.Windows.Forms.Button();
            this.Balance = new System.Windows.Forms.Label();
            this.textBalance = new System.Windows.Forms.TextBox();
            this.btnUnlockGrayCard = new System.Windows.Forms.Button();
            this.Unit1 = new System.Windows.Forms.Label();
            this.GrayFlag = new System.Windows.Forms.CheckBox();
            this.tabPageApp2 = new System.Windows.Forms.TabPage();
            this.textLyPin = new System.Windows.Forms.TextBox();
            this.LyRecordInCard = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.btnLoadLy = new System.Windows.Forms.Button();
            this.btnReadLyRecord = new System.Windows.Forms.Button();
            this.textLyLoad = new System.Windows.Forms.TextBox();
            this.LoadLoyalty = new System.Windows.Forms.Label();
            this.LabelLyPin = new System.Windows.Forms.Label();
            this.groupLyTest = new System.Windows.Forms.GroupBox();
            this.LySamSlot = new System.Windows.Forms.CheckBox();
            this.btnLockLy = new System.Windows.Forms.Button();
            this.DebitLy = new System.Windows.Forms.Label();
            this.btnUnlockLy = new System.Windows.Forms.Button();
            this.textDebitLy = new System.Windows.Forms.TextBox();
            this.btnReadLy = new System.Windows.Forms.Button();
            this.Loyalty = new System.Windows.Forms.Label();
            this.textLoyalty = new System.Windows.Forms.TextBox();
            this.btnUnGrayLy = new System.Windows.Forms.Button();
            this.LyGrayFlag = new System.Windows.Forms.CheckBox();
            this.ContactCard = new System.Windows.Forms.CheckBox();
            this.Infomation = new System.Windows.Forms.Label();
            this.tabApp.SuspendLayout();
            this.tabPageApp1.SuspendLayout();
            this.groupCardTest.SuspendLayout();
            this.tabPageApp2.SuspendLayout();
            this.groupLyTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabApp
            // 
            this.tabApp.Controls.Add(this.tabPageApp1);
            this.tabApp.Controls.Add(this.tabPageApp2);
            this.tabApp.Location = new System.Drawing.Point(2, 55);
            this.tabApp.Name = "tabApp";
            this.tabApp.SelectedIndex = 0;
            this.tabApp.Size = new System.Drawing.Size(528, 530);
            this.tabApp.TabIndex = 0;
            this.tabApp.SelectedIndexChanged += new System.EventHandler(this.tabApp_SelectedIndexChanged);
            // 
            // tabPageApp1
            // 
            this.tabPageApp1.Controls.Add(this.btnUnload);
            this.tabPageApp1.Controls.Add(this.textPIN);
            this.tabPageApp1.Controls.Add(this.RecordInCard);
            this.tabPageApp1.Controls.Add(this.btnCardLoad);
            this.tabPageApp1.Controls.Add(this.btnReadRecord);
            this.tabPageApp1.Controls.Add(this.textMoney);
            this.tabPageApp1.Controls.Add(this.Money);
            this.tabPageApp1.Controls.Add(this.LabelPIN);
            this.tabPageApp1.Controls.Add(this.Unit2);
            this.tabPageApp1.Controls.Add(this.groupCardTest);
            this.tabPageApp1.Controls.Add(this.btnBalance);
            this.tabPageApp1.Controls.Add(this.Balance);
            this.tabPageApp1.Controls.Add(this.textBalance);
            this.tabPageApp1.Controls.Add(this.btnUnlockGrayCard);
            this.tabPageApp1.Controls.Add(this.Unit1);
            this.tabPageApp1.Controls.Add(this.GrayFlag);
            this.tabPageApp1.Location = new System.Drawing.Point(4, 22);
            this.tabPageApp1.Name = "tabPageApp1";
            this.tabPageApp1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApp1.Size = new System.Drawing.Size(520, 504);
            this.tabPageApp1.TabIndex = 0;
            this.tabPageApp1.Text = "加气应用";
            this.tabPageApp1.UseVisualStyleBackColor = true;
            // 
            // btnUnload
            // 
            this.btnUnload.Location = new System.Drawing.Point(255, 136);
            this.btnUnload.Name = "btnUnload";
            this.btnUnload.Size = new System.Drawing.Size(40, 23);
            this.btnUnload.TabIndex = 14;
            this.btnUnload.Text = "圈提";
            this.btnUnload.UseVisualStyleBackColor = true;
            this.btnUnload.Click += new System.EventHandler(this.btnUnload_Click);
            // 
            // textPIN
            // 
            this.textPIN.Location = new System.Drawing.Point(117, 27);
            this.textPIN.MaxLength = 6;
            this.textPIN.Name = "textPIN";
            this.textPIN.Size = new System.Drawing.Size(100, 21);
            this.textPIN.TabIndex = 3;
            this.textPIN.Validated += new System.EventHandler(this.textPIN_Validated);
            this.textPIN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPIN_KeyPress);
            // 
            // RecordInCard
            // 
            this.RecordInCard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.BusinessSn,
            this.Amount,
            this.BusinessType,
            this.TermID,
            this.BusinessTime});
            this.RecordInCard.FullRowSelect = true;
            this.RecordInCard.GridLines = true;
            this.RecordInCard.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.RecordInCard.Location = new System.Drawing.Point(28, 314);
            this.RecordInCard.MultiSelect = false;
            this.RecordInCard.Name = "RecordInCard";
            this.RecordInCard.Size = new System.Drawing.Size(431, 184);
            this.RecordInCard.TabIndex = 17;
            this.RecordInCard.UseCompatibleStateImageBehavior = false;
            this.RecordInCard.View = System.Windows.Forms.View.Details;
            // 
            // BusinessSn
            // 
            this.BusinessSn.Text = "交易序号";
            // 
            // Amount
            // 
            this.Amount.Text = "交易金额";
            this.Amount.Width = 80;
            // 
            // BusinessType
            // 
            this.BusinessType.Text = "交易类型";
            this.BusinessType.Width = 65;
            // 
            // TermID
            // 
            this.TermID.Text = "终端机编号";
            this.TermID.Width = 80;
            // 
            // BusinessTime
            // 
            this.BusinessTime.Text = "交易时间";
            this.BusinessTime.Width = 120;
            // 
            // btnCardLoad
            // 
            this.btnCardLoad.Location = new System.Drawing.Point(209, 136);
            this.btnCardLoad.Name = "btnCardLoad";
            this.btnCardLoad.Size = new System.Drawing.Size(40, 23);
            this.btnCardLoad.TabIndex = 13;
            this.btnCardLoad.Text = "圈存";
            this.btnCardLoad.UseVisualStyleBackColor = true;
            this.btnCardLoad.Click += new System.EventHandler(this.btnCardLoad_Click);
            // 
            // btnReadRecord
            // 
            this.btnReadRecord.Location = new System.Drawing.Point(39, 286);
            this.btnReadRecord.Name = "btnReadRecord";
            this.btnReadRecord.Size = new System.Drawing.Size(75, 23);
            this.btnReadRecord.TabIndex = 16;
            this.btnReadRecord.Text = "读交易记录";
            this.btnReadRecord.UseVisualStyleBackColor = true;
            this.btnReadRecord.Click += new System.EventHandler(this.btnReadRecord_Click);
            // 
            // textMoney
            // 
            this.textMoney.Location = new System.Drawing.Point(68, 136);
            this.textMoney.Name = "textMoney";
            this.textMoney.Size = new System.Drawing.Size(97, 21);
            this.textMoney.TabIndex = 11;
            this.textMoney.Text = "10,000.00";
            // 
            // Money
            // 
            this.Money.AutoSize = true;
            this.Money.Location = new System.Drawing.Point(26, 140);
            this.Money.Name = "Money";
            this.Money.Size = new System.Drawing.Size(41, 12);
            this.Money.TabIndex = 10;
            this.Money.Text = "金额：";
            // 
            // LabelPIN
            // 
            this.LabelPIN.AutoSize = true;
            this.LabelPIN.Location = new System.Drawing.Point(26, 30);
            this.LabelPIN.Name = "LabelPIN";
            this.LabelPIN.Size = new System.Drawing.Size(83, 12);
            this.LabelPIN.TabIndex = 2;
            this.LabelPIN.Text = "请输入PIN码：";
            // 
            // Unit2
            // 
            this.Unit2.AutoSize = true;
            this.Unit2.Location = new System.Drawing.Point(170, 141);
            this.Unit2.Name = "Unit2";
            this.Unit2.Size = new System.Drawing.Size(17, 12);
            this.Unit2.TabIndex = 12;
            this.Unit2.Text = "元";
            // 
            // groupCardTest
            // 
            this.groupCardTest.Controls.Add(this.SamSlot);
            this.groupCardTest.Controls.Add(this.btnLockCard);
            this.groupCardTest.Controls.Add(this.Purchase);
            this.groupCardTest.Controls.Add(this.btnUnlockCard);
            this.groupCardTest.Controls.Add(this.textPurchase);
            this.groupCardTest.Controls.Add(this.Unit3);
            this.groupCardTest.Location = new System.Drawing.Point(28, 180);
            this.groupCardTest.Name = "groupCardTest";
            this.groupCardTest.Size = new System.Drawing.Size(363, 99);
            this.groupCardTest.TabIndex = 15;
            this.groupCardTest.TabStop = false;
            this.groupCardTest.Text = "消费测试";
            // 
            // SamSlot
            // 
            this.SamSlot.AutoSize = true;
            this.SamSlot.Location = new System.Drawing.Point(116, 76);
            this.SamSlot.Name = "SamSlot";
            this.SamSlot.Size = new System.Drawing.Size(84, 16);
            this.SamSlot.TabIndex = 5;
            this.SamSlot.Text = "PSAM卡内置";
            this.SamSlot.UseVisualStyleBackColor = true;
            // 
            // btnLockCard
            // 
            this.btnLockCard.Location = new System.Drawing.Point(29, 40);
            this.btnLockCard.Name = "btnLockCard";
            this.btnLockCard.Size = new System.Drawing.Size(57, 23);
            this.btnLockCard.TabIndex = 0;
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
            this.Purchase.TabIndex = 1;
            this.Purchase.Text = "消费：";
            // 
            // btnUnlockCard
            // 
            this.btnUnlockCard.Location = new System.Drawing.Point(277, 40);
            this.btnUnlockCard.Name = "btnUnlockCard";
            this.btnUnlockCard.Size = new System.Drawing.Size(64, 23);
            this.btnUnlockCard.TabIndex = 4;
            this.btnUnlockCard.Text = "扣款解锁";
            this.btnUnlockCard.UseVisualStyleBackColor = true;
            this.btnUnlockCard.Click += new System.EventHandler(this.btnUnlockCard_Click);
            // 
            // textPurchase
            // 
            this.textPurchase.Location = new System.Drawing.Point(145, 42);
            this.textPurchase.Name = "textPurchase";
            this.textPurchase.Size = new System.Drawing.Size(97, 21);
            this.textPurchase.TabIndex = 2;
            this.textPurchase.Text = "200.00";
            // 
            // Unit3
            // 
            this.Unit3.AutoSize = true;
            this.Unit3.Location = new System.Drawing.Point(248, 45);
            this.Unit3.Name = "Unit3";
            this.Unit3.Size = new System.Drawing.Size(17, 12);
            this.Unit3.TabIndex = 3;
            this.Unit3.Text = "元";
            // 
            // btnBalance
            // 
            this.btnBalance.Location = new System.Drawing.Point(276, 74);
            this.btnBalance.Name = "btnBalance";
            this.btnBalance.Size = new System.Drawing.Size(64, 23);
            this.btnBalance.TabIndex = 8;
            this.btnBalance.Text = "读取余额";
            this.btnBalance.UseVisualStyleBackColor = true;
            this.btnBalance.Click += new System.EventHandler(this.btnBalance_Click);
            // 
            // Balance
            // 
            this.Balance.AutoSize = true;
            this.Balance.Location = new System.Drawing.Point(26, 81);
            this.Balance.Name = "Balance";
            this.Balance.Size = new System.Drawing.Size(41, 12);
            this.Balance.TabIndex = 4;
            this.Balance.Text = "余额：";
            // 
            // textBalance
            // 
            this.textBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBalance.Location = new System.Drawing.Point(68, 77);
            this.textBalance.Name = "textBalance";
            this.textBalance.ReadOnly = true;
            this.textBalance.Size = new System.Drawing.Size(97, 21);
            this.textBalance.TabIndex = 5;
            // 
            // btnUnlockGrayCard
            // 
            this.btnUnlockGrayCard.Location = new System.Drawing.Point(276, 104);
            this.btnUnlockGrayCard.Name = "btnUnlockGrayCard";
            this.btnUnlockGrayCard.Size = new System.Drawing.Size(64, 23);
            this.btnUnlockGrayCard.TabIndex = 9;
            this.btnUnlockGrayCard.Text = "强制解灰";
            this.btnUnlockGrayCard.UseVisualStyleBackColor = true;
            this.btnUnlockGrayCard.Click += new System.EventHandler(this.btnUnlockGrayCard_Click);
            // 
            // Unit1
            // 
            this.Unit1.AutoSize = true;
            this.Unit1.Location = new System.Drawing.Point(170, 82);
            this.Unit1.Name = "Unit1";
            this.Unit1.Size = new System.Drawing.Size(17, 12);
            this.Unit1.TabIndex = 6;
            this.Unit1.Text = "元";
            // 
            // GrayFlag
            // 
            this.GrayFlag.AutoCheck = false;
            this.GrayFlag.AutoSize = true;
            this.GrayFlag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GrayFlag.Location = new System.Drawing.Point(209, 80);
            this.GrayFlag.Name = "GrayFlag";
            this.GrayFlag.Size = new System.Drawing.Size(45, 16);
            this.GrayFlag.TabIndex = 7;
            this.GrayFlag.Text = "灰锁";
            this.GrayFlag.UseVisualStyleBackColor = true;
            // 
            // tabPageApp2
            // 
            this.tabPageApp2.Controls.Add(this.textLyPin);
            this.tabPageApp2.Controls.Add(this.LyRecordInCard);
            this.tabPageApp2.Controls.Add(this.btnLoadLy);
            this.tabPageApp2.Controls.Add(this.btnReadLyRecord);
            this.tabPageApp2.Controls.Add(this.textLyLoad);
            this.tabPageApp2.Controls.Add(this.LoadLoyalty);
            this.tabPageApp2.Controls.Add(this.LabelLyPin);
            this.tabPageApp2.Controls.Add(this.groupLyTest);
            this.tabPageApp2.Controls.Add(this.btnReadLy);
            this.tabPageApp2.Controls.Add(this.Loyalty);
            this.tabPageApp2.Controls.Add(this.textLoyalty);
            this.tabPageApp2.Controls.Add(this.btnUnGrayLy);
            this.tabPageApp2.Controls.Add(this.LyGrayFlag);
            this.tabPageApp2.Location = new System.Drawing.Point(4, 22);
            this.tabPageApp2.Name = "tabPageApp2";
            this.tabPageApp2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApp2.Size = new System.Drawing.Size(520, 504);
            this.tabPageApp2.TabIndex = 1;
            this.tabPageApp2.Text = "积分应用";
            this.tabPageApp2.UseVisualStyleBackColor = true;
            // 
            // textLyPin
            // 
            this.textLyPin.Location = new System.Drawing.Point(132, 24);
            this.textLyPin.MaxLength = 6;
            this.textLyPin.Name = "textLyPin";
            this.textLyPin.Size = new System.Drawing.Size(100, 21);
            this.textLyPin.TabIndex = 19;
            this.textLyPin.Validated += new System.EventHandler(this.textLyPin_Validated);
            this.textLyPin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textLyPin_KeyPress);
            // 
            // LyRecordInCard
            // 
            this.LyRecordInCard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.LyRecordInCard.FullRowSelect = true;
            this.LyRecordInCard.GridLines = true;
            this.LyRecordInCard.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.LyRecordInCard.Location = new System.Drawing.Point(26, 309);
            this.LyRecordInCard.MultiSelect = false;
            this.LyRecordInCard.Name = "LyRecordInCard";
            this.LyRecordInCard.Size = new System.Drawing.Size(411, 184);
            this.LyRecordInCard.TabIndex = 33;
            this.LyRecordInCard.UseCompatibleStateImageBehavior = false;
            this.LyRecordInCard.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "交易序号";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "交易积分";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "交易类型";
            this.columnHeader3.Width = 65;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "终端机编号";
            this.columnHeader4.Width = 80;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "交易时间";
            this.columnHeader5.Width = 120;
            // 
            // btnLoadLy
            // 
            this.btnLoadLy.Location = new System.Drawing.Point(208, 133);
            this.btnLoadLy.Name = "btnLoadLy";
            this.btnLoadLy.Size = new System.Drawing.Size(61, 23);
            this.btnLoadLy.TabIndex = 29;
            this.btnLoadLy.Text = "圈存积分";
            this.btnLoadLy.UseVisualStyleBackColor = true;
            this.btnLoadLy.Click += new System.EventHandler(this.btnLoadLy_Click);
            // 
            // btnReadLyRecord
            // 
            this.btnReadLyRecord.Location = new System.Drawing.Point(38, 283);
            this.btnReadLyRecord.Name = "btnReadLyRecord";
            this.btnReadLyRecord.Size = new System.Drawing.Size(75, 23);
            this.btnReadLyRecord.TabIndex = 32;
            this.btnReadLyRecord.Text = "读积分交易记录";
            this.btnReadLyRecord.UseVisualStyleBackColor = true;
            this.btnReadLyRecord.Click += new System.EventHandler(this.btnReadLyRecord_Click);
            // 
            // textLyLoad
            // 
            this.textLyLoad.Location = new System.Drawing.Point(67, 133);
            this.textLyLoad.Name = "textLyLoad";
            this.textLyLoad.Size = new System.Drawing.Size(97, 21);
            this.textLyLoad.TabIndex = 27;
            this.textLyLoad.Text = "10,000";
            // 
            // LoadLoyalty
            // 
            this.LoadLoyalty.AutoSize = true;
            this.LoadLoyalty.Location = new System.Drawing.Point(24, 137);
            this.LoadLoyalty.Name = "LoadLoyalty";
            this.LoadLoyalty.Size = new System.Drawing.Size(41, 12);
            this.LoadLoyalty.TabIndex = 26;
            this.LoadLoyalty.Text = "积分：";
            // 
            // LabelLyPin
            // 
            this.LabelLyPin.AutoSize = true;
            this.LabelLyPin.Location = new System.Drawing.Point(23, 27);
            this.LabelLyPin.Name = "LabelLyPin";
            this.LabelLyPin.Size = new System.Drawing.Size(107, 12);
            this.LabelLyPin.TabIndex = 18;
            this.LabelLyPin.Text = "请输入积分PIN码：";
            // 
            // groupLyTest
            // 
            this.groupLyTest.Controls.Add(this.LySamSlot);
            this.groupLyTest.Controls.Add(this.btnLockLy);
            this.groupLyTest.Controls.Add(this.DebitLy);
            this.groupLyTest.Controls.Add(this.btnUnlockLy);
            this.groupLyTest.Controls.Add(this.textDebitLy);
            this.groupLyTest.Location = new System.Drawing.Point(27, 177);
            this.groupLyTest.Name = "groupLyTest";
            this.groupLyTest.Size = new System.Drawing.Size(363, 99);
            this.groupLyTest.TabIndex = 31;
            this.groupLyTest.TabStop = false;
            this.groupLyTest.Text = "积分消费测试";
            // 
            // LySamSlot
            // 
            this.LySamSlot.AutoSize = true;
            this.LySamSlot.Location = new System.Drawing.Point(116, 76);
            this.LySamSlot.Name = "LySamSlot";
            this.LySamSlot.Size = new System.Drawing.Size(84, 16);
            this.LySamSlot.TabIndex = 5;
            this.LySamSlot.Text = "PSAM卡内置";
            this.LySamSlot.UseVisualStyleBackColor = true;
            // 
            // btnLockLy
            // 
            this.btnLockLy.Location = new System.Drawing.Point(11, 40);
            this.btnLockLy.Name = "btnLockLy";
            this.btnLockLy.Size = new System.Drawing.Size(86, 23);
            this.btnLockLy.TabIndex = 0;
            this.btnLockLy.Text = "积分消费锁定";
            this.btnLockLy.UseVisualStyleBackColor = true;
            this.btnLockLy.Click += new System.EventHandler(this.btnLockLy_Click);
            // 
            // DebitLy
            // 
            this.DebitLy.AutoSize = true;
            this.DebitLy.Location = new System.Drawing.Point(103, 45);
            this.DebitLy.Name = "DebitLy";
            this.DebitLy.Size = new System.Drawing.Size(41, 12);
            this.DebitLy.TabIndex = 1;
            this.DebitLy.Text = "消费：";
            // 
            // btnUnlockLy
            // 
            this.btnUnlockLy.Location = new System.Drawing.Point(277, 40);
            this.btnUnlockLy.Name = "btnUnlockLy";
            this.btnUnlockLy.Size = new System.Drawing.Size(80, 23);
            this.btnUnlockLy.TabIndex = 4;
            this.btnUnlockLy.Text = "扣积分解锁";
            this.btnUnlockLy.UseVisualStyleBackColor = true;
            this.btnUnlockLy.Click += new System.EventHandler(this.btnUnlockLy_Click);
            // 
            // textDebitLy
            // 
            this.textDebitLy.Location = new System.Drawing.Point(145, 42);
            this.textDebitLy.Name = "textDebitLy";
            this.textDebitLy.Size = new System.Drawing.Size(97, 21);
            this.textDebitLy.TabIndex = 2;
            this.textDebitLy.Text = "200.00";
            // 
            // btnReadLy
            // 
            this.btnReadLy.Location = new System.Drawing.Point(275, 71);
            this.btnReadLy.Name = "btnReadLy";
            this.btnReadLy.Size = new System.Drawing.Size(64, 23);
            this.btnReadLy.TabIndex = 24;
            this.btnReadLy.Text = "读取积分";
            this.btnReadLy.UseVisualStyleBackColor = true;
            this.btnReadLy.Click += new System.EventHandler(this.btnReadLy_Click);
            // 
            // Loyalty
            // 
            this.Loyalty.AutoSize = true;
            this.Loyalty.Location = new System.Drawing.Point(23, 78);
            this.Loyalty.Name = "Loyalty";
            this.Loyalty.Size = new System.Drawing.Size(65, 12);
            this.Loyalty.TabIndex = 20;
            this.Loyalty.Text = "卡内积分：";
            // 
            // textLoyalty
            // 
            this.textLoyalty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textLoyalty.Location = new System.Drawing.Point(90, 74);
            this.textLoyalty.Name = "textLoyalty";
            this.textLoyalty.ReadOnly = true;
            this.textLoyalty.Size = new System.Drawing.Size(117, 21);
            this.textLoyalty.TabIndex = 21;
            // 
            // btnUnGrayLy
            // 
            this.btnUnGrayLy.Location = new System.Drawing.Point(275, 101);
            this.btnUnGrayLy.Name = "btnUnGrayLy";
            this.btnUnGrayLy.Size = new System.Drawing.Size(64, 23);
            this.btnUnGrayLy.TabIndex = 25;
            this.btnUnGrayLy.Text = "强制解灰";
            this.btnUnGrayLy.UseVisualStyleBackColor = true;
            this.btnUnGrayLy.Click += new System.EventHandler(this.btnUnGrayLy_Click);
            // 
            // LyGrayFlag
            // 
            this.LyGrayFlag.AutoCheck = false;
            this.LyGrayFlag.AutoSize = true;
            this.LyGrayFlag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LyGrayFlag.Location = new System.Drawing.Point(216, 77);
            this.LyGrayFlag.Name = "LyGrayFlag";
            this.LyGrayFlag.Size = new System.Drawing.Size(45, 16);
            this.LyGrayFlag.TabIndex = 23;
            this.LyGrayFlag.Text = "灰锁";
            this.LyGrayFlag.UseVisualStyleBackColor = true;
            // 
            // ContactCard
            // 
            this.ContactCard.AutoSize = true;
            this.ContactCard.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ContactCard.Location = new System.Drawing.Point(40, 22);
            this.ContactCard.Name = "ContactCard";
            this.ContactCard.Size = new System.Drawing.Size(102, 16);
            this.ContactCard.TabIndex = 0;
            this.ContactCard.Text = "接触式用户卡";
            this.ContactCard.UseVisualStyleBackColor = true;
            this.ContactCard.CheckedChanged += new System.EventHandler(this.ContactCard_CheckedChanged);
            // 
            // Infomation
            // 
            this.Infomation.AutoSize = true;
            this.Infomation.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Infomation.ForeColor = System.Drawing.Color.Red;
            this.Infomation.Location = new System.Drawing.Point(171, 23);
            this.Infomation.Name = "Infomation";
            this.Infomation.Size = new System.Drawing.Size(161, 12);
            this.Infomation.TabIndex = 1;
            this.Infomation.Text = "！使用当前制卡密钥操作！";
            // 
            // CardApplicationTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 606);
            this.Controls.Add(this.tabApp);
            this.Controls.Add(this.ContactCard);
            this.Controls.Add(this.Infomation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CardApplicationTest";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "卡应用测试";
            this.tabApp.ResumeLayout(false);
            this.tabPageApp1.ResumeLayout(false);
            this.tabPageApp1.PerformLayout();
            this.groupCardTest.ResumeLayout(false);
            this.groupCardTest.PerformLayout();
            this.tabPageApp2.ResumeLayout(false);
            this.tabPageApp2.PerformLayout();
            this.groupLyTest.ResumeLayout(false);
            this.groupLyTest.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabApp;
        private System.Windows.Forms.TabPage tabPageApp1;
        private System.Windows.Forms.CheckBox ContactCard;
        private System.Windows.Forms.Button btnUnload;
        private System.Windows.Forms.Label Infomation;
        private System.Windows.Forms.ListView RecordInCard;
        private System.Windows.Forms.ColumnHeader BusinessSn;
        private System.Windows.Forms.ColumnHeader Amount;
        private System.Windows.Forms.ColumnHeader BusinessType;
        private System.Windows.Forms.ColumnHeader TermID;
        private System.Windows.Forms.ColumnHeader BusinessTime;
        private System.Windows.Forms.Button btnReadRecord;
        private System.Windows.Forms.TextBox textPIN;
        private System.Windows.Forms.Label LabelPIN;
        private System.Windows.Forms.GroupBox groupCardTest;
        private System.Windows.Forms.CheckBox SamSlot;
        private System.Windows.Forms.Button btnLockCard;
        private System.Windows.Forms.Label Purchase;
        private System.Windows.Forms.Button btnUnlockCard;
        private System.Windows.Forms.TextBox textPurchase;
        private System.Windows.Forms.Label Unit3;
        private System.Windows.Forms.Label Balance;
        private System.Windows.Forms.Button btnUnlockGrayCard;
        private System.Windows.Forms.CheckBox GrayFlag;
        private System.Windows.Forms.Label Unit1;
        private System.Windows.Forms.TextBox textBalance;
        private System.Windows.Forms.Button btnBalance;
        private System.Windows.Forms.Label Unit2;
        private System.Windows.Forms.Label Money;
        private System.Windows.Forms.TextBox textMoney;
        private System.Windows.Forms.Button btnCardLoad;
        private System.Windows.Forms.TabPage tabPageApp2;
        private System.Windows.Forms.TextBox textLyPin;
        private System.Windows.Forms.ListView LyRecordInCard;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btnLoadLy;
        private System.Windows.Forms.Button btnReadLyRecord;
        private System.Windows.Forms.TextBox textLyLoad;
        private System.Windows.Forms.Label LoadLoyalty;
        private System.Windows.Forms.Label LabelLyPin;
        private System.Windows.Forms.GroupBox groupLyTest;
        private System.Windows.Forms.CheckBox LySamSlot;
        private System.Windows.Forms.Button btnLockLy;
        private System.Windows.Forms.Label DebitLy;
        private System.Windows.Forms.Button btnUnlockLy;
        private System.Windows.Forms.TextBox textDebitLy;
        private System.Windows.Forms.Button btnReadLy;
        private System.Windows.Forms.Label Loyalty;
        private System.Windows.Forms.TextBox textLoyalty;
        private System.Windows.Forms.Button btnUnGrayLy;
        private System.Windows.Forms.CheckBox LyGrayFlag;

    }
}