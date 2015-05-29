namespace ClientManage
{
    partial class ClientInfoManage
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
            this.ClientLabel = new System.Windows.Forms.Label();
            this.DetailInfo = new System.Windows.Forms.GroupBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.textRemark = new System.Windows.Forms.TextBox();
            this.Remark = new System.Windows.Forms.Label();
            this.textBankAccount = new System.Windows.Forms.TextBox();
            this.BankAccount = new System.Windows.Forms.Label();
            this.ClientName = new System.Windows.Forms.Label();
            this.textBank = new System.Windows.Forms.TextBox();
            this.textClientName = new System.Windows.Forms.TextBox();
            this.Bank = new System.Windows.Forms.Label();
            this.textAddress = new System.Windows.Forms.TextBox();
            this.Address = new System.Windows.Forms.Label();
            this.textZipcode = new System.Windows.Forms.TextBox();
            this.Zipcode = new System.Windows.Forms.Label();
            this.textEmail = new System.Windows.Forms.TextBox();
            this.EMail = new System.Windows.Forms.Label();
            this.textFaxNum = new System.Windows.Forms.TextBox();
            this.FaxNum = new System.Windows.Forms.Label();
            this.textTelephone = new System.Windows.Forms.TextBox();
            this.Telephone = new System.Windows.Forms.Label();
            this.textLinkMan = new System.Windows.Forms.TextBox();
            this.LinkMan = new System.Windows.Forms.Label();
            this.ClientId = new System.Windows.Forms.Label();
            this.textClientId = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.treeClient = new System.Windows.Forms.TreeView();
            this.DetailInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClientLabel
            // 
            this.ClientLabel.AutoSize = true;
            this.ClientLabel.Location = new System.Drawing.Point(25, 13);
            this.ClientLabel.Name = "ClientLabel";
            this.ClientLabel.Size = new System.Drawing.Size(53, 12);
            this.ClientLabel.TabIndex = 1;
            this.ClientLabel.Text = "单位信息";
            // 
            // DetailInfo
            // 
            this.DetailInfo.Controls.Add(this.btnModify);
            this.DetailInfo.Controls.Add(this.textRemark);
            this.DetailInfo.Controls.Add(this.Remark);
            this.DetailInfo.Controls.Add(this.textBankAccount);
            this.DetailInfo.Controls.Add(this.BankAccount);
            this.DetailInfo.Controls.Add(this.ClientName);
            this.DetailInfo.Controls.Add(this.textBank);
            this.DetailInfo.Controls.Add(this.textClientName);
            this.DetailInfo.Controls.Add(this.Bank);
            this.DetailInfo.Controls.Add(this.textAddress);
            this.DetailInfo.Controls.Add(this.Address);
            this.DetailInfo.Controls.Add(this.textZipcode);
            this.DetailInfo.Controls.Add(this.Zipcode);
            this.DetailInfo.Controls.Add(this.textEmail);
            this.DetailInfo.Controls.Add(this.EMail);
            this.DetailInfo.Controls.Add(this.textFaxNum);
            this.DetailInfo.Controls.Add(this.FaxNum);
            this.DetailInfo.Controls.Add(this.textTelephone);
            this.DetailInfo.Controls.Add(this.Telephone);
            this.DetailInfo.Controls.Add(this.textLinkMan);
            this.DetailInfo.Controls.Add(this.LinkMan);
            this.DetailInfo.Location = new System.Drawing.Point(317, 75);
            this.DetailInfo.Name = "DetailInfo";
            this.DetailInfo.Size = new System.Drawing.Size(352, 470);
            this.DetailInfo.TabIndex = 2;
            this.DetailInfo.TabStop = false;
            this.DetailInfo.Text = "详细信息";
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(271, 20);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 4;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // textRemark
            // 
            this.textRemark.Location = new System.Drawing.Point(87, 396);
            this.textRemark.Name = "textRemark";
            this.textRemark.Size = new System.Drawing.Size(126, 21);
            this.textRemark.TabIndex = 22;
            // 
            // Remark
            // 
            this.Remark.AutoSize = true;
            this.Remark.Location = new System.Drawing.Point(48, 399);
            this.Remark.Name = "Remark";
            this.Remark.Size = new System.Drawing.Size(29, 12);
            this.Remark.TabIndex = 21;
            this.Remark.Text = "备注";
            // 
            // textBankAccount
            // 
            this.textBankAccount.Location = new System.Drawing.Point(87, 356);
            this.textBankAccount.Name = "textBankAccount";
            this.textBankAccount.Size = new System.Drawing.Size(126, 21);
            this.textBankAccount.TabIndex = 20;
            // 
            // BankAccount
            // 
            this.BankAccount.AutoSize = true;
            this.BankAccount.Location = new System.Drawing.Point(24, 359);
            this.BankAccount.Name = "BankAccount";
            this.BankAccount.Size = new System.Drawing.Size(53, 12);
            this.BankAccount.TabIndex = 19;
            this.BankAccount.Text = "银行账号";
            // 
            // ClientName
            // 
            this.ClientName.AutoSize = true;
            this.ClientName.Location = new System.Drawing.Point(24, 39);
            this.ClientName.Name = "ClientName";
            this.ClientName.Size = new System.Drawing.Size(53, 12);
            this.ClientName.TabIndex = 2;
            this.ClientName.Text = "单位名称";
            // 
            // textBank
            // 
            this.textBank.Location = new System.Drawing.Point(87, 316);
            this.textBank.Name = "textBank";
            this.textBank.Size = new System.Drawing.Size(126, 21);
            this.textBank.TabIndex = 18;
            // 
            // textClientName
            // 
            this.textClientName.Location = new System.Drawing.Point(87, 36);
            this.textClientName.MaxLength = 32;
            this.textClientName.Name = "textClientName";
            this.textClientName.Size = new System.Drawing.Size(126, 21);
            this.textClientName.TabIndex = 3;
            // 
            // Bank
            // 
            this.Bank.AutoSize = true;
            this.Bank.Location = new System.Drawing.Point(24, 319);
            this.Bank.Name = "Bank";
            this.Bank.Size = new System.Drawing.Size(53, 12);
            this.Bank.TabIndex = 17;
            this.Bank.Text = "开户银行";
            // 
            // textAddress
            // 
            this.textAddress.Location = new System.Drawing.Point(87, 156);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(126, 21);
            this.textAddress.TabIndex = 16;
            // 
            // Address
            // 
            this.Address.AutoSize = true;
            this.Address.Location = new System.Drawing.Point(24, 159);
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(53, 12);
            this.Address.TabIndex = 15;
            this.Address.Text = "单位地址";
            // 
            // textZipcode
            // 
            this.textZipcode.Location = new System.Drawing.Point(87, 276);
            this.textZipcode.Name = "textZipcode";
            this.textZipcode.Size = new System.Drawing.Size(126, 21);
            this.textZipcode.TabIndex = 14;
            // 
            // Zipcode
            // 
            this.Zipcode.AutoSize = true;
            this.Zipcode.Location = new System.Drawing.Point(24, 279);
            this.Zipcode.Name = "Zipcode";
            this.Zipcode.Size = new System.Drawing.Size(53, 12);
            this.Zipcode.TabIndex = 13;
            this.Zipcode.Text = "邮政编码";
            // 
            // textEmail
            // 
            this.textEmail.Location = new System.Drawing.Point(87, 236);
            this.textEmail.Name = "textEmail";
            this.textEmail.Size = new System.Drawing.Size(126, 21);
            this.textEmail.TabIndex = 12;
            // 
            // EMail
            // 
            this.EMail.AutoSize = true;
            this.EMail.Location = new System.Drawing.Point(24, 239);
            this.EMail.Name = "EMail";
            this.EMail.Size = new System.Drawing.Size(53, 12);
            this.EMail.TabIndex = 11;
            this.EMail.Text = "电子邮箱";
            // 
            // textFaxNum
            // 
            this.textFaxNum.Location = new System.Drawing.Point(87, 196);
            this.textFaxNum.Name = "textFaxNum";
            this.textFaxNum.Size = new System.Drawing.Size(126, 21);
            this.textFaxNum.TabIndex = 10;
            // 
            // FaxNum
            // 
            this.FaxNum.AutoSize = true;
            this.FaxNum.Location = new System.Drawing.Point(24, 199);
            this.FaxNum.Name = "FaxNum";
            this.FaxNum.Size = new System.Drawing.Size(53, 12);
            this.FaxNum.TabIndex = 9;
            this.FaxNum.Text = "传真号码";
            // 
            // textTelephone
            // 
            this.textTelephone.Location = new System.Drawing.Point(87, 116);
            this.textTelephone.Name = "textTelephone";
            this.textTelephone.Size = new System.Drawing.Size(126, 21);
            this.textTelephone.TabIndex = 8;
            // 
            // Telephone
            // 
            this.Telephone.AutoSize = true;
            this.Telephone.Location = new System.Drawing.Point(24, 119);
            this.Telephone.Name = "Telephone";
            this.Telephone.Size = new System.Drawing.Size(53, 12);
            this.Telephone.TabIndex = 7;
            this.Telephone.Text = "联系电话";
            // 
            // textLinkMan
            // 
            this.textLinkMan.Location = new System.Drawing.Point(87, 76);
            this.textLinkMan.Name = "textLinkMan";
            this.textLinkMan.Size = new System.Drawing.Size(126, 21);
            this.textLinkMan.TabIndex = 6;
            // 
            // LinkMan
            // 
            this.LinkMan.AutoSize = true;
            this.LinkMan.Location = new System.Drawing.Point(36, 79);
            this.LinkMan.Name = "LinkMan";
            this.LinkMan.Size = new System.Drawing.Size(41, 12);
            this.LinkMan.TabIndex = 5;
            this.LinkMan.Text = "联系人";
            // 
            // ClientId
            // 
            this.ClientId.AutoSize = true;
            this.ClientId.Location = new System.Drawing.Point(341, 31);
            this.ClientId.Name = "ClientId";
            this.ClientId.Size = new System.Drawing.Size(53, 12);
            this.ClientId.TabIndex = 0;
            this.ClientId.Text = "单位编号";
            // 
            // textClientId
            // 
            this.textClientId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textClientId.Location = new System.Drawing.Point(404, 28);
            this.textClientId.MaxLength = 16;
            this.textClientId.Name = "textClientId";
            this.textClientId.ReadOnly = true;
            this.textClientId.Size = new System.Drawing.Size(126, 21);
            this.textClientId.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Location = new System.Drawing.Point(268, 192);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(26, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDel.Location = new System.Drawing.Point(268, 342);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(26, 23);
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "-";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // treeClient
            // 
            this.treeClient.HideSelection = false;
            this.treeClient.LabelEdit = true;
            this.treeClient.Location = new System.Drawing.Point(25, 31);
            this.treeClient.Name = "treeClient";
            this.treeClient.Size = new System.Drawing.Size(237, 514);
            this.treeClient.TabIndex = 0;
            this.treeClient.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeClient_AfterLabelEdit);
            this.treeClient.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeClient_AfterSelect);
            this.treeClient.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeClient_NodeMouseClick);
            this.treeClient.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeClient_BeforeLabelEdit);
            // 
            // ClientInfoManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 557);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.DetailInfo);
            this.Controls.Add(this.ClientLabel);
            this.Controls.Add(this.treeClient);
            this.Controls.Add(this.ClientId);
            this.Controls.Add(this.textClientId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientInfoManage";
            this.Text = "单位信息管理";
            this.Load += new System.EventHandler(this.ClientInfoManage_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientInfoManage_FormClosed);
            this.DetailInfo.ResumeLayout(false);
            this.DetailInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ClientLabel;
        private System.Windows.Forms.GroupBox DetailInfo;
        private System.Windows.Forms.Label ClientId;
        private System.Windows.Forms.TextBox textClientId;
        private System.Windows.Forms.Label ClientName;
        private System.Windows.Forms.TextBox textClientName;
        private System.Windows.Forms.Label LinkMan;
        private System.Windows.Forms.TextBox textLinkMan;
        private System.Windows.Forms.TextBox textTelephone;
        private System.Windows.Forms.Label Telephone;
        private System.Windows.Forms.TextBox textZipcode;
        private System.Windows.Forms.Label Zipcode;
        private System.Windows.Forms.TextBox textEmail;
        private System.Windows.Forms.Label EMail;
        private System.Windows.Forms.TextBox textFaxNum;
        private System.Windows.Forms.Label FaxNum;
        private System.Windows.Forms.TextBox textBank;
        private System.Windows.Forms.Label Bank;
        private System.Windows.Forms.TextBox textAddress;
        private System.Windows.Forms.Label Address;
        private System.Windows.Forms.TextBox textRemark;
        private System.Windows.Forms.Label Remark;
        private System.Windows.Forms.TextBox textBankAccount;
        private System.Windows.Forms.Label BankAccount;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.TreeView treeClient;
    }
}

