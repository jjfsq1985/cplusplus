namespace AsyncServer
{
    partial class AsyncForm
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
            this.textPort = new System.Windows.Forms.TextBox();
            this.btnListen = new System.Windows.Forms.Button();
            this.textAsyncRecv = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(59, 26);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(58, 21);
            this.textPort.TabIndex = 0;
            this.textPort.Text = "4500";
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(178, 29);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 23);
            this.btnListen.TabIndex = 1;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // textAsyncRecv
            // 
            this.textAsyncRecv.Location = new System.Drawing.Point(18, 154);
            this.textAsyncRecv.Multiline = true;
            this.textAsyncRecv.Name = "textBox1";
            this.textAsyncRecv.Size = new System.Drawing.Size(277, 157);
            this.textAsyncRecv.TabIndex = 2;
            // 
            // AsyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 313);
            this.Controls.Add(this.textAsyncRecv);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.textPort);
            this.Name = "AsyncForm";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.TextBox textAsyncRecv;
    }
}

