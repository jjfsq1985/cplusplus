namespace PublishCardOperator.Dialog
{
    partial class AddOrgKey
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
            this.labelValue = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.IsValid = new System.Windows.Forms.CheckBox();
            this.textOrgKey = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.textKeyDetail = new System.Windows.Forms.TextBox();
            this.KeyDetail = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(19, 54);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(41, 12);
            this.labelValue.TabIndex = 0;
            this.labelValue.Text = "密钥值";
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(19, 22);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(77, 12);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "初始密钥类型";
            // 
            // IsValid
            // 
            this.IsValid.AutoSize = true;
            this.IsValid.Location = new System.Drawing.Point(226, 23);
            this.IsValid.Name = "IsValid";
            this.IsValid.Size = new System.Drawing.Size(72, 16);
            this.IsValid.TabIndex = 3;
            this.IsValid.Text = "立即生效";
            this.IsValid.UseVisualStyleBackColor = true;
            // 
            // textOrgKey
            // 
            this.textOrgKey.Location = new System.Drawing.Point(76, 51);
            this.textOrgKey.MaxLength = 32;
            this.textOrgKey.Name = "textOrgKey";
            this.textOrgKey.Size = new System.Drawing.Size(222, 21);
            this.textOrgKey.TabIndex = 4;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "CPU卡密钥",
            "PSAM卡密钥",
            "公共初始密钥"});
            this.cmbType.Location = new System.Drawing.Point(103, 19);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(83, 20);
            this.cmbType.TabIndex = 5;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(53, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(195, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // textKeyDetail
            // 
            this.textKeyDetail.Location = new System.Drawing.Point(76, 78);
            this.textKeyDetail.MaxLength = 32;
            this.textKeyDetail.Multiline = true;
            this.textKeyDetail.Name = "textKeyDetail";
            this.textKeyDetail.Size = new System.Drawing.Size(222, 43);
            this.textKeyDetail.TabIndex = 36;
            // 
            // KeyDetail
            // 
            this.KeyDetail.AutoSize = true;
            this.KeyDetail.Location = new System.Drawing.Point(19, 93);
            this.KeyDetail.Name = "KeyDetail";
            this.KeyDetail.Size = new System.Drawing.Size(53, 12);
            this.KeyDetail.TabIndex = 35;
            this.KeyDetail.Text = "密钥描述";
            // 
            // AddOrgKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 176);
            this.Controls.Add(this.textKeyDetail);
            this.Controls.Add(this.KeyDetail);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.textOrgKey);
            this.Controls.Add(this.IsValid);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddOrgKey";
            this.Text = "厂商初始卡片密钥";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.CheckBox IsValid;
        private System.Windows.Forms.TextBox textOrgKey;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox textKeyDetail;
        private System.Windows.Forms.Label KeyDetail;
    }
}