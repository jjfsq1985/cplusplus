namespace AutoUpdate
{
    partial class MainForm
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
            this.ServerOnOff = new System.Windows.Forms.Button();
            this.TcpPort = new System.Windows.Forms.Label();
            this.textTcpPort = new System.Windows.Forms.TextBox();
            this.UdpPort = new System.Windows.Forms.Label();
            this.textUdpPort = new System.Windows.Forms.TextBox();
            this.LabelPrompt = new System.Windows.Forms.Label();
            this.lstClient = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ServerOnOff
            // 
            this.ServerOnOff.Location = new System.Drawing.Point(56, 103);
            this.ServerOnOff.Name = "ServerOnOff";
            this.ServerOnOff.Size = new System.Drawing.Size(75, 23);
            this.ServerOnOff.TabIndex = 0;
            this.ServerOnOff.Text = "启动";
            this.ServerOnOff.UseVisualStyleBackColor = true;
            this.ServerOnOff.Click += new System.EventHandler(this.StartServer_Click);
            // 
            // TcpPort
            // 
            this.TcpPort.AutoSize = true;
            this.TcpPort.Location = new System.Drawing.Point(54, 70);
            this.TcpPort.Name = "TcpPort";
            this.TcpPort.Size = new System.Drawing.Size(53, 12);
            this.TcpPort.TabIndex = 1;
            this.TcpPort.Text = "升级端口";
            // 
            // textTcpPort
            // 
            this.textTcpPort.Location = new System.Drawing.Point(112, 66);
            this.textTcpPort.Name = "textTcpPort";
            this.textTcpPort.Size = new System.Drawing.Size(100, 21);
            this.textTcpPort.TabIndex = 2;
            this.textTcpPort.Text = "4747";
            // 
            // UdpPort
            // 
            this.UdpPort.AutoSize = true;
            this.UdpPort.Location = new System.Drawing.Point(54, 33);
            this.UdpPort.Name = "UdpPort";
            this.UdpPort.Size = new System.Drawing.Size(53, 12);
            this.UdpPort.TabIndex = 3;
            this.UdpPort.Text = "广播端口";
            // 
            // textUdpPort
            // 
            this.textUdpPort.Location = new System.Drawing.Point(112, 29);
            this.textUdpPort.Name = "textUdpPort";
            this.textUdpPort.ReadOnly = true;
            this.textUdpPort.Size = new System.Drawing.Size(100, 21);
            this.textUdpPort.TabIndex = 4;
            this.textUdpPort.Text = "4646";
            // 
            // LabelPrompt
            // 
            this.LabelPrompt.AutoSize = true;
            this.LabelPrompt.Location = new System.Drawing.Point(41, 153);
            this.LabelPrompt.Name = "LabelPrompt";
            this.LabelPrompt.Size = new System.Drawing.Size(0, 12);
            this.LabelPrompt.TabIndex = 5;
            // 
            // lstClient
            // 
            this.lstClient.FormattingEnabled = true;
            this.lstClient.ItemHeight = 12;
            this.lstClient.Location = new System.Drawing.Point(25, 142);
            this.lstClient.Name = "lstClient";
            this.lstClient.Size = new System.Drawing.Size(298, 208);
            this.lstClient.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 368);
            this.Controls.Add(this.lstClient);
            this.Controls.Add(this.LabelPrompt);
            this.Controls.Add(this.textUdpPort);
            this.Controls.Add(this.UdpPort);
            this.Controls.Add(this.textTcpPort);
            this.Controls.Add(this.TcpPort);
            this.Controls.Add(this.ServerOnOff);
            this.Name = "MainForm";
            this.Text = "远程升级";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ServerOnOff;
        private System.Windows.Forms.Label TcpPort;
        private System.Windows.Forms.TextBox textTcpPort;
        private System.Windows.Forms.Label UdpPort;
        private System.Windows.Forms.TextBox textUdpPort;
        private System.Windows.Forms.Label LabelPrompt;
        private System.Windows.Forms.ListBox lstClient;
    }
}

