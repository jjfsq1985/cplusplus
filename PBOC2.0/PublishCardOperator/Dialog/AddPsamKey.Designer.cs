namespace PublishCardOperator.Dialog
{
    partial class AddPsamKey
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
            this.MasterLabel = new System.Windows.Forms.Label();
            this.MasterTendingLabel = new System.Windows.Forms.Label();
            this.AppMasterLabel = new System.Windows.Forms.Label();
            this.AppTendingLabel = new System.Windows.Forms.Label();
            this.textMasterKey = new System.Windows.Forms.TextBox();
            this.textMasterTendingKey = new System.Windows.Forms.TextBox();
            this.textAppMasterKey = new System.Windows.Forms.TextBox();
            this.textAppTendingKey = new System.Windows.Forms.TextBox();
            this.ConsumerMasterLabel = new System.Windows.Forms.Label();
            this.textConsumerMasterKey = new System.Windows.Forms.TextBox();
            this.GrayLockLabel = new System.Windows.Forms.Label();
            this.textGrayLockKey = new System.Windows.Forms.TextBox();
            this.MacEncryptLabel = new System.Windows.Forms.Label();
            this.textMACEncryptKey = new System.Windows.Forms.TextBox();
            this.IsValid = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.textKeyDetail = new System.Windows.Forms.TextBox();
            this.KeyDetail = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MasterLabel
            // 
            this.MasterLabel.AutoSize = true;
            this.MasterLabel.Location = new System.Drawing.Point(33, 36);
            this.MasterLabel.Name = "MasterLabel";
            this.MasterLabel.Size = new System.Drawing.Size(77, 12);
            this.MasterLabel.TabIndex = 0;
            this.MasterLabel.Text = "卡片主控密钥";
            // 
            // MasterTendingLabel
            // 
            this.MasterTendingLabel.AutoSize = true;
            this.MasterTendingLabel.Location = new System.Drawing.Point(33, 82);
            this.MasterTendingLabel.Name = "MasterTendingLabel";
            this.MasterTendingLabel.Size = new System.Drawing.Size(77, 12);
            this.MasterTendingLabel.TabIndex = 1;
            this.MasterTendingLabel.Text = "卡片维护密钥";
            // 
            // AppMasterLabel
            // 
            this.AppMasterLabel.AutoSize = true;
            this.AppMasterLabel.Location = new System.Drawing.Point(33, 128);
            this.AppMasterLabel.Name = "AppMasterLabel";
            this.AppMasterLabel.Size = new System.Drawing.Size(77, 12);
            this.AppMasterLabel.TabIndex = 2;
            this.AppMasterLabel.Text = "应用主控密钥";
            // 
            // AppTendingLabel
            // 
            this.AppTendingLabel.AutoSize = true;
            this.AppTendingLabel.Location = new System.Drawing.Point(33, 174);
            this.AppTendingLabel.Name = "AppTendingLabel";
            this.AppTendingLabel.Size = new System.Drawing.Size(77, 12);
            this.AppTendingLabel.TabIndex = 3;
            this.AppTendingLabel.Text = "应用维护密钥";
            // 
            // textMasterKey
            // 
            this.textMasterKey.Location = new System.Drawing.Point(124, 32);
            this.textMasterKey.MaxLength = 32;
            this.textMasterKey.Name = "textMasterKey";
            this.textMasterKey.Size = new System.Drawing.Size(229, 21);
            this.textMasterKey.TabIndex = 4;
            // 
            // textMasterTendingKey
            // 
            this.textMasterTendingKey.Location = new System.Drawing.Point(124, 78);
            this.textMasterTendingKey.MaxLength = 32;
            this.textMasterTendingKey.Name = "textMasterTendingKey";
            this.textMasterTendingKey.Size = new System.Drawing.Size(229, 21);
            this.textMasterTendingKey.TabIndex = 5;
            // 
            // textAppMasterKey
            // 
            this.textAppMasterKey.Location = new System.Drawing.Point(124, 124);
            this.textAppMasterKey.MaxLength = 32;
            this.textAppMasterKey.Name = "textAppMasterKey";
            this.textAppMasterKey.Size = new System.Drawing.Size(229, 21);
            this.textAppMasterKey.TabIndex = 6;
            // 
            // textAppTendingKey
            // 
            this.textAppTendingKey.Location = new System.Drawing.Point(124, 170);
            this.textAppTendingKey.MaxLength = 32;
            this.textAppTendingKey.Name = "textAppTendingKey";
            this.textAppTendingKey.Size = new System.Drawing.Size(229, 21);
            this.textAppTendingKey.TabIndex = 7;
            // 
            // ConsumerMasterLabel
            // 
            this.ConsumerMasterLabel.AutoSize = true;
            this.ConsumerMasterLabel.Location = new System.Drawing.Point(39, 220);
            this.ConsumerMasterLabel.Name = "ConsumerMasterLabel";
            this.ConsumerMasterLabel.Size = new System.Drawing.Size(65, 12);
            this.ConsumerMasterLabel.TabIndex = 8;
            this.ConsumerMasterLabel.Text = "消费主密钥";
            // 
            // textConsumerMasterKey
            // 
            this.textConsumerMasterKey.Location = new System.Drawing.Point(124, 216);
            this.textConsumerMasterKey.MaxLength = 32;
            this.textConsumerMasterKey.Name = "textConsumerMasterKey";
            this.textConsumerMasterKey.Size = new System.Drawing.Size(229, 21);
            this.textConsumerMasterKey.TabIndex = 9;
            // 
            // GrayLockLabel
            // 
            this.GrayLockLabel.AutoSize = true;
            this.GrayLockLabel.Location = new System.Drawing.Point(45, 266);
            this.GrayLockLabel.Name = "GrayLockLabel";
            this.GrayLockLabel.Size = new System.Drawing.Size(53, 12);
            this.GrayLockLabel.TabIndex = 10;
            this.GrayLockLabel.Text = "灰锁密钥";
            // 
            // textGrayLockKey
            // 
            this.textGrayLockKey.Location = new System.Drawing.Point(124, 262);
            this.textGrayLockKey.MaxLength = 32;
            this.textGrayLockKey.Name = "textGrayLockKey";
            this.textGrayLockKey.Size = new System.Drawing.Size(229, 21);
            this.textGrayLockKey.TabIndex = 11;
            // 
            // MacEncryptLabel
            // 
            this.MacEncryptLabel.AutoSize = true;
            this.MacEncryptLabel.Location = new System.Drawing.Point(36, 312);
            this.MacEncryptLabel.Name = "MacEncryptLabel";
            this.MacEncryptLabel.Size = new System.Drawing.Size(71, 12);
            this.MacEncryptLabel.TabIndex = 12;
            this.MacEncryptLabel.Text = "MAC加密密钥";
            // 
            // textMACEncryptKey
            // 
            this.textMACEncryptKey.Location = new System.Drawing.Point(124, 308);
            this.textMACEncryptKey.MaxLength = 32;
            this.textMACEncryptKey.Name = "textMACEncryptKey";
            this.textMACEncryptKey.Size = new System.Drawing.Size(229, 21);
            this.textMACEncryptKey.TabIndex = 13;
            // 
            // IsValid
            // 
            this.IsValid.AutoSize = true;
            this.IsValid.Location = new System.Drawing.Point(82, 414);
            this.IsValid.Name = "IsValid";
            this.IsValid.Size = new System.Drawing.Size(72, 16);
            this.IsValid.TabIndex = 14;
            this.IsValid.Text = "立即生效";
            this.IsValid.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(142, 447);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(249, 447);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // textKeyDetail
            // 
            this.textKeyDetail.Location = new System.Drawing.Point(124, 352);
            this.textKeyDetail.MaxLength = 32;
            this.textKeyDetail.Multiline = true;
            this.textKeyDetail.Name = "textKeyDetail";
            this.textKeyDetail.Size = new System.Drawing.Size(229, 43);
            this.textKeyDetail.TabIndex = 38;
            // 
            // KeyDetail
            // 
            this.KeyDetail.AutoSize = true;
            this.KeyDetail.Location = new System.Drawing.Point(47, 367);
            this.KeyDetail.Name = "KeyDetail";
            this.KeyDetail.Size = new System.Drawing.Size(53, 12);
            this.KeyDetail.TabIndex = 37;
            this.KeyDetail.Text = "密钥描述";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(35, 447);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 22);
            this.btnGenerate.TabIndex = 39;
            this.btnGenerate.Text = "生成";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // AddPsamKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 475);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.textKeyDetail);
            this.Controls.Add(this.KeyDetail);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.IsValid);
            this.Controls.Add(this.textMACEncryptKey);
            this.Controls.Add(this.MacEncryptLabel);
            this.Controls.Add(this.textGrayLockKey);
            this.Controls.Add(this.GrayLockLabel);
            this.Controls.Add(this.textConsumerMasterKey);
            this.Controls.Add(this.ConsumerMasterLabel);
            this.Controls.Add(this.textAppTendingKey);
            this.Controls.Add(this.textAppMasterKey);
            this.Controls.Add(this.textMasterTendingKey);
            this.Controls.Add(this.textMasterKey);
            this.Controls.Add(this.AppTendingLabel);
            this.Controls.Add(this.AppMasterLabel);
            this.Controls.Add(this.MasterTendingLabel);
            this.Controls.Add(this.MasterLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddPsamKey";
            this.Text = "SAM卡密钥";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MasterLabel;
        private System.Windows.Forms.Label MasterTendingLabel;
        private System.Windows.Forms.Label AppMasterLabel;
        private System.Windows.Forms.Label AppTendingLabel;
        private System.Windows.Forms.TextBox textMasterKey;
        private System.Windows.Forms.TextBox textMasterTendingKey;
        private System.Windows.Forms.TextBox textAppMasterKey;
        private System.Windows.Forms.TextBox textAppTendingKey;
        private System.Windows.Forms.Label ConsumerMasterLabel;
        private System.Windows.Forms.TextBox textConsumerMasterKey;
        private System.Windows.Forms.Label GrayLockLabel;
        private System.Windows.Forms.TextBox textGrayLockKey;
        private System.Windows.Forms.Label MacEncryptLabel;
        private System.Windows.Forms.TextBox textMACEncryptKey;
        private System.Windows.Forms.CheckBox IsValid;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox textKeyDetail;
        private System.Windows.Forms.Label KeyDetail;
        private System.Windows.Forms.Button btnGenerate;
    }
}