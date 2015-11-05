namespace RePublish
{
    partial class ToBlackCard
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
            this.LabelName = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.textPersonalID = new System.Windows.Forms.TextBox();
            this.LabelPersonalID = new System.Windows.Forms.Label();
            this.LabelTel = new System.Windows.Forms.Label();
            this.LabelCardID = new System.Windows.Forms.Label();
            this.textCardID = new System.Windows.Forms.TextBox();
            this.textTel = new System.Windows.Forms.TextBox();
            this.btnSetting = new System.Windows.Forms.Button();
            this.ContactCard = new System.Windows.Forms.CheckBox();
            this.LabelCardType = new System.Windows.Forms.Label();
            this.cmbDevType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // LabelName
            // 
            this.LabelName.AutoSize = true;
            this.LabelName.Location = new System.Drawing.Point(15, 51);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(65, 12);
            this.LabelName.TabIndex = 0;
            this.LabelName.Text = "申请者姓名";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(117, 48);
            this.textName.MaxLength = 10;
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(191, 21);
            this.textName.TabIndex = 1;
            // 
            // textPersonalID
            // 
            this.textPersonalID.Location = new System.Drawing.Point(117, 76);
            this.textPersonalID.MaxLength = 18;
            this.textPersonalID.Name = "textPersonalID";
            this.textPersonalID.Size = new System.Drawing.Size(191, 21);
            this.textPersonalID.TabIndex = 2;
            this.textPersonalID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPersonalID_KeyPress);
            // 
            // LabelPersonalID
            // 
            this.LabelPersonalID.AutoSize = true;
            this.LabelPersonalID.Location = new System.Drawing.Point(15, 79);
            this.LabelPersonalID.Name = "LabelPersonalID";
            this.LabelPersonalID.Size = new System.Drawing.Size(77, 12);
            this.LabelPersonalID.TabIndex = 3;
            this.LabelPersonalID.Text = "申请者证件号";
            // 
            // LabelTel
            // 
            this.LabelTel.AutoSize = true;
            this.LabelTel.Location = new System.Drawing.Point(15, 107);
            this.LabelTel.Name = "LabelTel";
            this.LabelTel.Size = new System.Drawing.Size(89, 12);
            this.LabelTel.TabIndex = 4;
            this.LabelTel.Text = "申请者联系电话";
            // 
            // LabelCardID
            // 
            this.LabelCardID.AutoSize = true;
            this.LabelCardID.Location = new System.Drawing.Point(15, 20);
            this.LabelCardID.Name = "LabelCardID";
            this.LabelCardID.Size = new System.Drawing.Size(53, 12);
            this.LabelCardID.TabIndex = 5;
            this.LabelCardID.Text = "设置卡号";
            // 
            // textCardID
            // 
            this.textCardID.Location = new System.Drawing.Point(117, 17);
            this.textCardID.Name = "textCardID";
            this.textCardID.ReadOnly = true;
            this.textCardID.Size = new System.Drawing.Size(191, 21);
            this.textCardID.TabIndex = 6;
            // 
            // textTel
            // 
            this.textTel.Location = new System.Drawing.Point(117, 104);
            this.textTel.MaxLength = 15;
            this.textTel.Name = "textTel";
            this.textTel.Size = new System.Drawing.Size(191, 21);
            this.textTel.TabIndex = 7;
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(117, 163);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(75, 23);
            this.btnSetting.TabIndex = 8;
            this.btnSetting.Text = "确定";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // ContactCard
            // 
            this.ContactCard.AutoSize = true;
            this.ContactCard.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ContactCard.Location = new System.Drawing.Point(320, 133);
            this.ContactCard.Name = "ContactCard";
            this.ContactCard.Size = new System.Drawing.Size(60, 16);
            this.ContactCard.TabIndex = 107;
            this.ContactCard.Text = "接触式";
            this.ContactCard.UseVisualStyleBackColor = true;
            // 
            // LabelCardType
            // 
            this.LabelCardType.AutoSize = true;
            this.LabelCardType.Location = new System.Drawing.Point(15, 135);
            this.LabelCardType.Name = "LabelCardType";
            this.LabelCardType.Size = new System.Drawing.Size(53, 12);
            this.LabelCardType.TabIndex = 108;
            this.LabelCardType.Text = "补卡设备";
            // 
            // cmbDevType
            // 
            this.cmbDevType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevType.FormattingEnabled = true;
            this.cmbDevType.Items.AddRange(new object[] {
            "达华-明泰 MT3",
            "龙寰-Duali DE-620",
            "龙寰-明泰 MT3"});
            this.cmbDevType.Location = new System.Drawing.Point(117, 131);
            this.cmbDevType.Name = "cmbDevType";
            this.cmbDevType.Size = new System.Drawing.Size(191, 20);
            this.cmbDevType.TabIndex = 106;
            this.cmbDevType.SelectedIndexChanged += new System.EventHandler(this.cmbDevType_SelectedIndexChanged);
            // 
            // ToBlackCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 197);
            this.Controls.Add(this.ContactCard);
            this.Controls.Add(this.LabelCardType);
            this.Controls.Add(this.cmbDevType);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.textTel);
            this.Controls.Add(this.textCardID);
            this.Controls.Add(this.LabelCardID);
            this.Controls.Add(this.LabelTel);
            this.Controls.Add(this.LabelPersonalID);
            this.Controls.Add(this.textPersonalID);
            this.Controls.Add(this.textName);
            this.Controls.Add(this.LabelName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToBlackCard";
            this.Text = "卡状态设置";
            this.Load += new System.EventHandler(this.ToBlackCard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelName;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.TextBox textPersonalID;
        private System.Windows.Forms.Label LabelPersonalID;
        private System.Windows.Forms.Label LabelTel;
        private System.Windows.Forms.Label LabelCardID;
        private System.Windows.Forms.TextBox textCardID;
        private System.Windows.Forms.TextBox textTel;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.CheckBox ContactCard;
        private System.Windows.Forms.Label LabelCardType;
        private System.Windows.Forms.ComboBox cmbDevType;
    }
}