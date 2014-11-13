namespace TCPServer
{
    partial class MyServer
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
            this.btnListen = new System.Windows.Forms.Button();
            this.textPort = new System.Windows.Forms.TextBox();
            this.textSend = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.listClient = new System.Windows.Forms.ListBox();
            this.textRecv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(230, 22);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(93, 29);
            this.btnListen.TabIndex = 0;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(112, 28);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(100, 21);
            this.textPort.TabIndex = 1;
            this.textPort.Text = "4500";
            // 
            // textSend
            // 
            this.textSend.Location = new System.Drawing.Point(40, 66);
            this.textSend.Multiline = true;
            this.textSend.Name = "textSend";
            this.textSend.Size = new System.Drawing.Size(331, 108);
            this.textSend.TabIndex = 2;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(393, 151);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // listClient
            // 
            this.listClient.FormattingEnabled = true;
            this.listClient.ItemHeight = 12;
            this.listClient.Location = new System.Drawing.Point(530, 16);
            this.listClient.Name = "listClient";
            this.listClient.Size = new System.Drawing.Size(165, 448);
            this.listClient.TabIndex = 4;
            // 
            // textRecv
            // 
            this.textRecv.Location = new System.Drawing.Point(41, 198);
            this.textRecv.Multiline = true;
            this.textRecv.Name = "textRecv";
            this.textRecv.Size = new System.Drawing.Size(466, 260);
            this.textRecv.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "ListenPort";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(481, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Client";
            // 
            // MyServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 470);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textRecv);
            this.Controls.Add(this.listClient);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.textSend);
            this.Controls.Add(this.textPort);
            this.Controls.Add(this.btnListen);
            this.Name = "MyServer";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.TextBox textSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListBox listClient;
        private System.Windows.Forms.TextBox textRecv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

