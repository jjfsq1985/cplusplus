namespace PublishCardOperator.Dialog
{
    partial class AddCpuKey
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.Mlabel = new System.Windows.Forms.Label();
            this.TLabel = new System.Windows.Forms.Label();
            this.AuthLabel = new System.Windows.Forms.Label();
            this.textAppMasterKey = new System.Windows.Forms.TextBox();
            this.textTendingKey = new System.Windows.Forms.TextBox();
            this.textAuthKey = new System.Windows.Forms.TextBox();
            this.IsValid = new System.Windows.Forms.CheckBox();
            this.listAppKey = new System.Windows.Forms.ListView();
            this.AppIndex = new System.Windows.Forms.ColumnHeader();
            this.AppMasterKey = new System.Windows.Forms.ColumnHeader();
            this.AppTendingKey = new System.Windows.Forms.ColumnHeader();
            this.AppAuthKey = new System.Windows.Forms.ColumnHeader();
            this.PinResetKey = new System.Windows.Forms.ColumnHeader();
            this.PINUnlockKey = new System.Windows.Forms.ColumnHeader();
            this.AppConsumerKey = new System.Windows.Forms.ColumnHeader();
            this.AppLoadKey = new System.Windows.Forms.ColumnHeader();
            this.AppUnLoadKey = new System.Windows.Forms.ColumnHeader();
            this.AppTacKey = new System.Windows.Forms.ColumnHeader();
            this.UnGrayKey = new System.Windows.Forms.ColumnHeader();
            this.OverdraftKey = new System.Windows.Forms.ColumnHeader();
            this.groupAppKey = new System.Windows.Forms.GroupBox();
            this.btnDelKey = new System.Windows.Forms.Button();
            this.BtnAddKey = new System.Windows.Forms.Button();
            this.KeyDetail = new System.Windows.Forms.Label();
            this.textKeyDetail = new System.Windows.Forms.TextBox();
            this.KeyRefresh = new System.Windows.Forms.Button();
            this.groupAppKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(425, 392);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(102, 392);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 22;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Mlabel
            // 
            this.Mlabel.AutoSize = true;
            this.Mlabel.Location = new System.Drawing.Point(45, 25);
            this.Mlabel.Name = "Mlabel";
            this.Mlabel.Size = new System.Drawing.Size(77, 12);
            this.Mlabel.TabIndex = 24;
            this.Mlabel.Text = "卡片主控密钥";
            // 
            // TLabel
            // 
            this.TLabel.AutoSize = true;
            this.TLabel.Location = new System.Drawing.Point(45, 59);
            this.TLabel.Name = "TLabel";
            this.TLabel.Size = new System.Drawing.Size(77, 12);
            this.TLabel.TabIndex = 25;
            this.TLabel.Text = "卡片维护密钥";
            // 
            // AuthLabel
            // 
            this.AuthLabel.AutoSize = true;
            this.AuthLabel.Location = new System.Drawing.Point(45, 93);
            this.AuthLabel.Name = "AuthLabel";
            this.AuthLabel.Size = new System.Drawing.Size(77, 12);
            this.AuthLabel.TabIndex = 26;
            this.AuthLabel.Text = "内部认证密钥";
            // 
            // textAppMasterKey
            // 
            this.textAppMasterKey.Location = new System.Drawing.Point(143, 22);
            this.textAppMasterKey.MaxLength = 32;
            this.textAppMasterKey.Name = "textAppMasterKey";
            this.textAppMasterKey.Size = new System.Drawing.Size(223, 21);
            this.textAppMasterKey.TabIndex = 27;
            // 
            // textTendingKey
            // 
            this.textTendingKey.Location = new System.Drawing.Point(143, 56);
            this.textTendingKey.MaxLength = 32;
            this.textTendingKey.Name = "textTendingKey";
            this.textTendingKey.Size = new System.Drawing.Size(223, 21);
            this.textTendingKey.TabIndex = 28;
            // 
            // textAuthKey
            // 
            this.textAuthKey.Location = new System.Drawing.Point(143, 90);
            this.textAuthKey.MaxLength = 32;
            this.textAuthKey.Name = "textAuthKey";
            this.textAuthKey.Size = new System.Drawing.Size(223, 21);
            this.textAuthKey.TabIndex = 29;
            // 
            // IsValid
            // 
            this.IsValid.AutoSize = true;
            this.IsValid.Location = new System.Drawing.Point(508, 58);
            this.IsValid.Name = "IsValid";
            this.IsValid.Size = new System.Drawing.Size(72, 16);
            this.IsValid.TabIndex = 30;
            this.IsValid.Text = "立即生效";
            this.IsValid.UseVisualStyleBackColor = true;
            // 
            // listAppKey
            // 
            this.listAppKey.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AppIndex,
            this.AppMasterKey,
            this.AppTendingKey,
            this.AppAuthKey,
            this.PinResetKey,
            this.PINUnlockKey,
            this.AppConsumerKey,
            this.AppLoadKey,
            this.AppUnLoadKey,
            this.AppTacKey,
            this.UnGrayKey,
            this.OverdraftKey});
            this.listAppKey.FullRowSelect = true;
            this.listAppKey.GridLines = true;
            this.listAppKey.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listAppKey.Location = new System.Drawing.Point(51, 44);
            this.listAppKey.MultiSelect = false;
            this.listAppKey.Name = "listAppKey";
            this.listAppKey.Size = new System.Drawing.Size(607, 156);
            this.listAppKey.TabIndex = 31;
            this.listAppKey.UseCompatibleStateImageBehavior = false;
            this.listAppKey.View = System.Windows.Forms.View.Details;
            // 
            // AppIndex
            // 
            this.AppIndex.Text = "应用号";
            // 
            // AppMasterKey
            // 
            this.AppMasterKey.Text = "应用主控密钥";
            this.AppMasterKey.Width = 150;
            // 
            // AppTendingKey
            // 
            this.AppTendingKey.Text = "应用维护密钥";
            this.AppTendingKey.Width = 150;
            // 
            // AppAuthKey
            // 
            this.AppAuthKey.Text = "内部认证密钥";
            this.AppAuthKey.Width = 150;
            // 
            // PinResetKey
            // 
            this.PinResetKey.Text = "PIN重装密钥";
            this.PinResetKey.Width = 150;
            // 
            // PINUnlockKey
            // 
            this.PINUnlockKey.Text = "PIN解锁密钥";
            this.PINUnlockKey.Width = 150;
            // 
            // AppConsumerKey
            // 
            this.AppConsumerKey.Text = "消费主密钥";
            this.AppConsumerKey.Width = 150;
            // 
            // AppLoadKey
            // 
            this.AppLoadKey.Text = "圈存密钥";
            this.AppLoadKey.Width = 150;
            // 
            // AppUnLoadKey
            // 
            this.AppUnLoadKey.Text = "圈提密钥";
            this.AppUnLoadKey.Width = 150;
            // 
            // AppTacKey
            // 
            this.AppTacKey.Text = "TAC密钥";
            this.AppTacKey.Width = 150;
            // 
            // UnGrayKey
            // 
            this.UnGrayKey.Text = "联机解扣密钥";
            this.UnGrayKey.Width = 150;
            // 
            // OverdraftKey
            // 
            this.OverdraftKey.Text = "修改透支限额密钥";
            this.OverdraftKey.Width = 150;
            // 
            // groupAppKey
            // 
            this.groupAppKey.Controls.Add(this.btnDelKey);
            this.groupAppKey.Controls.Add(this.BtnAddKey);
            this.groupAppKey.Controls.Add(this.listAppKey);
            this.groupAppKey.Location = new System.Drawing.Point(11, 153);
            this.groupAppKey.Name = "groupAppKey";
            this.groupAppKey.Size = new System.Drawing.Size(658, 206);
            this.groupAppKey.TabIndex = 32;
            this.groupAppKey.TabStop = false;
            this.groupAppKey.Text = "应用密钥列表";
            // 
            // btnDelKey
            // 
            this.btnDelKey.Location = new System.Drawing.Point(10, 123);
            this.btnDelKey.Name = "btnDelKey";
            this.btnDelKey.Size = new System.Drawing.Size(29, 23);
            this.btnDelKey.TabIndex = 33;
            this.btnDelKey.Text = "-";
            this.btnDelKey.UseVisualStyleBackColor = true;
            this.btnDelKey.Click += new System.EventHandler(this.btnDelKey_Click);
            // 
            // BtnAddKey
            // 
            this.BtnAddKey.Location = new System.Drawing.Point(10, 71);
            this.BtnAddKey.Name = "BtnAddKey";
            this.BtnAddKey.Size = new System.Drawing.Size(29, 23);
            this.BtnAddKey.TabIndex = 32;
            this.BtnAddKey.Text = "+";
            this.BtnAddKey.UseVisualStyleBackColor = true;
            this.BtnAddKey.Click += new System.EventHandler(this.btnAddKey_Click);
            // 
            // KeyDetail
            // 
            this.KeyDetail.AutoSize = true;
            this.KeyDetail.Location = new System.Drawing.Point(45, 127);
            this.KeyDetail.Name = "KeyDetail";
            this.KeyDetail.Size = new System.Drawing.Size(53, 12);
            this.KeyDetail.TabIndex = 33;
            this.KeyDetail.Text = "密钥描述";
            // 
            // textKeyDetail
            // 
            this.textKeyDetail.Location = new System.Drawing.Point(143, 124);
            this.textKeyDetail.MaxLength = 32;
            this.textKeyDetail.Name = "textKeyDetail";
            this.textKeyDetail.Size = new System.Drawing.Size(410, 21);
            this.textKeyDetail.TabIndex = 34;
            // 
            // Refresh
            // 
            this.KeyRefresh.BackgroundImage = global::PublishCardOperator.Properties.Resources.Refresh;
            this.KeyRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.KeyRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.KeyRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KeyRefresh.Location = new System.Drawing.Point(508, 22);
            this.KeyRefresh.Name = "Refresh";
            this.KeyRefresh.Size = new System.Drawing.Size(22, 21);
            this.KeyRefresh.TabIndex = 54;
            this.KeyRefresh.UseVisualStyleBackColor = true;
            this.KeyRefresh.Click += new System.EventHandler(this.KeyRefresh_Click);
            // 
            // AddCpuKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 426);
            this.Controls.Add(this.KeyRefresh);
            this.Controls.Add(this.textKeyDetail);
            this.Controls.Add(this.KeyDetail);
            this.Controls.Add(this.groupAppKey);
            this.Controls.Add(this.IsValid);
            this.Controls.Add(this.textAuthKey);
            this.Controls.Add(this.textTendingKey);
            this.Controls.Add(this.textAppMasterKey);
            this.Controls.Add(this.AuthLabel);
            this.Controls.Add(this.TLabel);
            this.Controls.Add(this.Mlabel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCpuKey";
            this.Text = "用户卡密钥";
            this.groupAppKey.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label Mlabel;
        private System.Windows.Forms.Label TLabel;
        private System.Windows.Forms.Label AuthLabel;
        private System.Windows.Forms.TextBox textAppMasterKey;
        private System.Windows.Forms.TextBox textTendingKey;
        private System.Windows.Forms.TextBox textAuthKey;
        private System.Windows.Forms.CheckBox IsValid;
        private System.Windows.Forms.ListView listAppKey;
        private System.Windows.Forms.ColumnHeader AppIndex;
        private System.Windows.Forms.ColumnHeader AppMasterKey;
        private System.Windows.Forms.ColumnHeader AppTendingKey;
        private System.Windows.Forms.ColumnHeader AppAuthKey;
        private System.Windows.Forms.ColumnHeader PinResetKey;
        private System.Windows.Forms.ColumnHeader PINUnlockKey;
        private System.Windows.Forms.ColumnHeader AppConsumerKey;
        private System.Windows.Forms.ColumnHeader AppLoadKey;
        private System.Windows.Forms.ColumnHeader AppUnLoadKey;
        private System.Windows.Forms.ColumnHeader AppTacKey;
        private System.Windows.Forms.ColumnHeader UnGrayKey;
        private System.Windows.Forms.ColumnHeader OverdraftKey;
        private System.Windows.Forms.GroupBox groupAppKey;
        private System.Windows.Forms.Button btnDelKey;
        private System.Windows.Forms.Button BtnAddKey;
        private System.Windows.Forms.Label KeyDetail;
        private System.Windows.Forms.TextBox textKeyDetail;
        private System.Windows.Forms.Button KeyRefresh;
    }
}