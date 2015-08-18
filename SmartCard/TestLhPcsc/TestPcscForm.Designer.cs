namespace TestLhPcsc
{
    partial class TestPcscForm
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
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnEstablish = new System.Windows.Forms.Button();
            this.btnRelease = new System.Windows.Forms.Button();
            this.cmbReaderName = new System.Windows.Forms.ComboBox();
            this.textAtr = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(318, 68);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "打开卡片";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(415, 68);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭卡片";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEstablish
            // 
            this.btnEstablish.Location = new System.Drawing.Point(22, 22);
            this.btnEstablish.Name = "btnEstablish";
            this.btnEstablish.Size = new System.Drawing.Size(75, 23);
            this.btnEstablish.TabIndex = 2;
            this.btnEstablish.Text = "连接";
            this.btnEstablish.UseVisualStyleBackColor = true;
            this.btnEstablish.Click += new System.EventHandler(this.btnEstablish_Click);
            // 
            // btnRelease
            // 
            this.btnRelease.Location = new System.Drawing.Point(103, 23);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(75, 23);
            this.btnRelease.TabIndex = 3;
            this.btnRelease.Text = "断开";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // cmbReaderName
            // 
            this.cmbReaderName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReaderName.FormattingEnabled = true;
            this.cmbReaderName.Location = new System.Drawing.Point(24, 71);
            this.cmbReaderName.Name = "cmbReaderName";
            this.cmbReaderName.Size = new System.Drawing.Size(259, 20);
            this.cmbReaderName.TabIndex = 4;
            // 
            // textAtr
            // 
            this.textAtr.Location = new System.Drawing.Point(74, 150);
            this.textAtr.Name = "textAtr";
            this.textAtr.Size = new System.Drawing.Size(319, 21);
            this.textAtr.TabIndex = 5;
            // 
            // TestPcscForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 364);
            this.Controls.Add(this.textAtr);
            this.Controls.Add(this.cmbReaderName);
            this.Controls.Add(this.btnRelease);
            this.Controls.Add(this.btnEstablish);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOpen);
            this.Name = "TestPcscForm";
            this.Text = "Smart Card";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnEstablish;
        private System.Windows.Forms.Button btnRelease;
        private System.Windows.Forms.ComboBox cmbReaderName;
        private System.Windows.Forms.TextBox textAtr;
    }
}

