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
            this.groupAppTest = new System.Windows.Forms.GroupBox();
            this.btnLockCard = new System.Windows.Forms.Button();
            this.btnUnlockCard = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textPurchase = new System.Windows.Forms.TextBox();
            this.Purchase = new System.Windows.Forms.Label();
            this.Balance = new System.Windows.Forms.Label();
            this.btnUnlockGrayCard = new System.Windows.Forms.Button();
            this.GrayFlag = new System.Windows.Forms.CheckBox();
            this.Unit1 = new System.Windows.Forms.Label();
            this.textBalance = new System.Windows.Forms.TextBox();
            this.btnInfo = new System.Windows.Forms.Button();
            this.Unit2 = new System.Windows.Forms.Label();
            this.Money = new System.Windows.Forms.Label();
            this.textMoney = new System.Windows.Forms.TextBox();
            this.btnCardLoad = new System.Windows.Forms.Button();
            this.groupAppTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupAppTest
            // 
            this.groupAppTest.Controls.Add(this.btnLockCard);
            this.groupAppTest.Controls.Add(this.btnUnlockCard);
            this.groupAppTest.Controls.Add(this.label2);
            this.groupAppTest.Controls.Add(this.textPurchase);
            this.groupAppTest.Controls.Add(this.Purchase);
            this.groupAppTest.Controls.Add(this.Balance);
            this.groupAppTest.Controls.Add(this.btnUnlockGrayCard);
            this.groupAppTest.Controls.Add(this.GrayFlag);
            this.groupAppTest.Controls.Add(this.Unit1);
            this.groupAppTest.Controls.Add(this.textBalance);
            this.groupAppTest.Controls.Add(this.btnInfo);
            this.groupAppTest.Controls.Add(this.Unit2);
            this.groupAppTest.Controls.Add(this.Money);
            this.groupAppTest.Controls.Add(this.textMoney);
            this.groupAppTest.Controls.Add(this.btnCardLoad);
            this.groupAppTest.Location = new System.Drawing.Point(2, 1);
            this.groupAppTest.Name = "groupAppTest";
            this.groupAppTest.Size = new System.Drawing.Size(395, 495);
            this.groupAppTest.TabIndex = 0;
            this.groupAppTest.TabStop = false;
            this.groupAppTest.Text = "卡应用测试";
            // 
            // btnLockCard
            // 
            this.btnLockCard.Location = new System.Drawing.Point(23, 195);
            this.btnLockCard.Name = "btnLockCard";
            this.btnLockCard.Size = new System.Drawing.Size(57, 23);
            this.btnLockCard.TabIndex = 31;
            this.btnLockCard.Text = "卡锁定";
            this.btnLockCard.UseVisualStyleBackColor = true;
            this.btnLockCard.Click += new System.EventHandler(this.btnLockCard_Click);
            // 
            // btnUnlockCard
            // 
            this.btnUnlockCard.Location = new System.Drawing.Point(271, 195);
            this.btnUnlockCard.Name = "btnUnlockCard";
            this.btnUnlockCard.Size = new System.Drawing.Size(64, 23);
            this.btnUnlockCard.TabIndex = 30;
            this.btnUnlockCard.Text = "扣款解锁";
            this.btnUnlockCard.UseVisualStyleBackColor = true;
            this.btnUnlockCard.Click += new System.EventHandler(this.btnUnlockCard_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "元";
            // 
            // textPurchase
            // 
            this.textPurchase.Location = new System.Drawing.Point(139, 197);
            this.textPurchase.Name = "textPurchase";
            this.textPurchase.Size = new System.Drawing.Size(97, 21);
            this.textPurchase.TabIndex = 28;
            this.textPurchase.Text = "200.00";
            // 
            // Purchase
            // 
            this.Purchase.AutoSize = true;
            this.Purchase.Location = new System.Drawing.Point(97, 200);
            this.Purchase.Name = "Purchase";
            this.Purchase.Size = new System.Drawing.Size(41, 12);
            this.Purchase.TabIndex = 27;
            this.Purchase.Text = "消费：";
            // 
            // Balance
            // 
            this.Balance.AutoSize = true;
            this.Balance.Location = new System.Drawing.Point(21, 38);
            this.Balance.Name = "Balance";
            this.Balance.Size = new System.Drawing.Size(41, 12);
            this.Balance.TabIndex = 26;
            this.Balance.Text = "余额：";
            // 
            // btnUnlockGrayCard
            // 
            this.btnUnlockGrayCard.Location = new System.Drawing.Point(271, 61);
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
            this.GrayFlag.Location = new System.Drawing.Point(204, 37);
            this.GrayFlag.Name = "GrayFlag";
            this.GrayFlag.Size = new System.Drawing.Size(45, 16);
            this.GrayFlag.TabIndex = 24;
            this.GrayFlag.Text = "灰锁";
            this.GrayFlag.UseVisualStyleBackColor = true;
            // 
            // Unit1
            // 
            this.Unit1.AutoSize = true;
            this.Unit1.Location = new System.Drawing.Point(165, 39);
            this.Unit1.Name = "Unit1";
            this.Unit1.Size = new System.Drawing.Size(17, 12);
            this.Unit1.TabIndex = 23;
            this.Unit1.Text = "元";
            // 
            // textBalance
            // 
            this.textBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBalance.Location = new System.Drawing.Point(63, 34);
            this.textBalance.Name = "textBalance";
            this.textBalance.ReadOnly = true;
            this.textBalance.Size = new System.Drawing.Size(97, 21);
            this.textBalance.TabIndex = 22;
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(271, 31);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(64, 23);
            this.btnInfo.TabIndex = 21;
            this.btnInfo.Text = "读取信息";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // Unit2
            // 
            this.Unit2.AutoSize = true;
            this.Unit2.Location = new System.Drawing.Point(165, 128);
            this.Unit2.Name = "Unit2";
            this.Unit2.Size = new System.Drawing.Size(17, 12);
            this.Unit2.TabIndex = 20;
            this.Unit2.Text = "元";
            // 
            // Money
            // 
            this.Money.AutoSize = true;
            this.Money.Location = new System.Drawing.Point(21, 127);
            this.Money.Name = "Money";
            this.Money.Size = new System.Drawing.Size(41, 12);
            this.Money.TabIndex = 19;
            this.Money.Text = "金额：";
            // 
            // textMoney
            // 
            this.textMoney.Location = new System.Drawing.Point(63, 123);
            this.textMoney.Name = "textMoney";
            this.textMoney.Size = new System.Drawing.Size(97, 21);
            this.textMoney.TabIndex = 18;
            this.textMoney.Text = "10,000.00";
            // 
            // btnCardLoad
            // 
            this.btnCardLoad.Location = new System.Drawing.Point(204, 123);
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
            this.ClientSize = new System.Drawing.Size(400, 500);
            this.Controls.Add(this.groupAppTest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CardApplicationTest";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "卡应用测试";
            this.groupAppTest.ResumeLayout(false);
            this.groupAppTest.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupAppTest;
        private System.Windows.Forms.Label Unit2;
        private System.Windows.Forms.Label Money;
        private System.Windows.Forms.TextBox textMoney;
        private System.Windows.Forms.Button btnCardLoad;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Label Unit1;
        private System.Windows.Forms.TextBox textBalance;
        private System.Windows.Forms.CheckBox GrayFlag;
        private System.Windows.Forms.Button btnUnlockGrayCard;
        private System.Windows.Forms.Label Balance;
        private System.Windows.Forms.Button btnUnlockCard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textPurchase;
        private System.Windows.Forms.Label Purchase;
        private System.Windows.Forms.Button btnLockCard;
    }
}