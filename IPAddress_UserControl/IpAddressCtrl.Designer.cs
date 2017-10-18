namespace IPAddress
{
    partial class IpAddressCtrl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelIP = new System.Windows.Forms.Panel();
            this.textIP1 = new IPAddress.IpEditBox();
            this.textIP2 = new IPAddress.IpEditBox();
            this.textIP3 = new IPAddress.IpEditBox();
            this.textIP4 = new IPAddress.IpEditBox();
            this.dot1 = new System.Windows.Forms.Label();
            this.dot2 = new System.Windows.Forms.Label();
            this.dot3 = new System.Windows.Forms.Label();
            this.panelIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelIP
            // 
            this.panelIP.AutoSize = true;
            this.panelIP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelIP.BackColor = System.Drawing.SystemColors.Control;
            this.panelIP.Controls.Add(this.textIP1);
            this.panelIP.Controls.Add(this.textIP2);
            this.panelIP.Controls.Add(this.textIP3);
            this.panelIP.Controls.Add(this.textIP4);
            this.panelIP.Controls.Add(this.dot1);
            this.panelIP.Controls.Add(this.dot2);
            this.panelIP.Controls.Add(this.dot3);
            this.panelIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelIP.Location = new System.Drawing.Point(0, 0);
            this.panelIP.Margin = new System.Windows.Forms.Padding(0);
            this.panelIP.Name = "panelIP";
            this.panelIP.Size = new System.Drawing.Size(206, 19);
            this.panelIP.TabIndex = 0;
            // 
            // textIP1
            // 
            this.textIP1.BackColor = System.Drawing.Color.White;
            this.textIP1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textIP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textIP1.IpIndex = 0;
            this.textIP1.IsMask = false;
            this.textIP1.IsSendKey = false;
            this.textIP1.Location = new System.Drawing.Point(0, 0);
            this.textIP1.Margin = new System.Windows.Forms.Padding(0);
            this.textIP1.MaxLength = 3;
            this.textIP1.Name = "textIP1";
            this.textIP1.Size = new System.Drawing.Size(41, 19);
            this.textIP1.TabIndex = 0;
            this.textIP1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textIP2
            // 
            this.textIP2.BackColor = System.Drawing.Color.White;
            this.textIP2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textIP2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textIP2.IpIndex = 1;
            this.textIP2.IsMask = false;
            this.textIP2.IsSendKey = false;
            this.textIP2.Location = new System.Drawing.Point(55, 0);
            this.textIP2.Margin = new System.Windows.Forms.Padding(0);
            this.textIP2.MaxLength = 3;
            this.textIP2.Name = "textIP2";
            this.textIP2.Size = new System.Drawing.Size(41, 19);
            this.textIP2.TabIndex = 1;
            this.textIP2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textIP3
            // 
            this.textIP3.BackColor = System.Drawing.Color.White;
            this.textIP3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textIP3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textIP3.IpIndex = 2;
            this.textIP3.IsMask = false;
            this.textIP3.IsSendKey = false;
            this.textIP3.Location = new System.Drawing.Point(110, 0);
            this.textIP3.Margin = new System.Windows.Forms.Padding(0);
            this.textIP3.MaxLength = 3;
            this.textIP3.Name = "textIP3";
            this.textIP3.Size = new System.Drawing.Size(41, 19);
            this.textIP3.TabIndex = 2;
            this.textIP3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textIP4
            // 
            this.textIP4.BackColor = System.Drawing.Color.White;
            this.textIP4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textIP4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textIP4.IpIndex = 3;
            this.textIP4.IsMask = false;
            this.textIP4.IsSendKey = false;
            this.textIP4.Location = new System.Drawing.Point(165, 0);
            this.textIP4.Margin = new System.Windows.Forms.Padding(0);
            this.textIP4.MaxLength = 3;
            this.textIP4.Name = "textIP4";
            this.textIP4.Size = new System.Drawing.Size(41, 19);
            this.textIP4.TabIndex = 3;
            this.textIP4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dot1
            // 
            this.dot1.AutoSize = true;
            this.dot1.BackColor = System.Drawing.Color.White;
            this.dot1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dot1.Location = new System.Drawing.Point(41, -1);
            this.dot1.Margin = new System.Windows.Forms.Padding(0);
            this.dot1.Name = "dot1";
            this.dot1.Size = new System.Drawing.Size(14, 20);
            this.dot1.TabIndex = 4;
            this.dot1.Text = ".";
            // 
            // dot2
            // 
            this.dot2.AutoSize = true;
            this.dot2.BackColor = System.Drawing.Color.White;
            this.dot2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dot2.Location = new System.Drawing.Point(96, -1);
            this.dot2.Margin = new System.Windows.Forms.Padding(0);
            this.dot2.Name = "dot2";
            this.dot2.Size = new System.Drawing.Size(14, 20);
            this.dot2.TabIndex = 5;
            this.dot2.Text = ".";
            // 
            // dot3
            // 
            this.dot3.AutoSize = true;
            this.dot3.BackColor = System.Drawing.Color.White;
            this.dot3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dot3.Location = new System.Drawing.Point(151, -1);
            this.dot3.Margin = new System.Windows.Forms.Padding(0);
            this.dot3.Name = "dot3";
            this.dot3.Size = new System.Drawing.Size(14, 20);
            this.dot3.TabIndex = 6;
            this.dot3.Text = ".";
            // 
            // IpAddressCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.panelIP);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "IpAddressCtrl";
            this.Size = new System.Drawing.Size(206, 19);
            this.panelIP.ResumeLayout(false);
            this.panelIP.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelIP;
        private IpEditBox textIP1;
        private IpEditBox textIP2;
        private IpEditBox textIP3;
        private IpEditBox textIP4;
        private System.Windows.Forms.Label dot1;
        private System.Windows.Forms.Label dot2;
        private System.Windows.Forms.Label dot3;
    }
}
