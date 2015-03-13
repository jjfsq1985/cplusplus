namespace GasInfoManage
{
    partial class GasInfo
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
            this.Quit = new System.Windows.Forms.Button();
            this.GasInfoQuit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Quit
            // 
            this.Quit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Quit.Location = new System.Drawing.Point(0, 0);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(75, 23);
            this.Quit.TabIndex = 0;
            // 
            // GasInfoQuit
            // 
            this.GasInfoQuit.Location = new System.Drawing.Point(0, 0);
            this.GasInfoQuit.Name = "GasInfoQuit";
            this.GasInfoQuit.Size = new System.Drawing.Size(75, 23);
            this.GasInfoQuit.TabIndex = 0;
            // 
            // GasInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(380, 344);
            this.Name = "GasInfo";
            this.Text = "加气管理";
            this.Load += new System.EventHandler(this.GasInfo_Load);            
            this.Resize += new System.EventHandler(this.GasInfo_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.Button GasInfoQuit;
    }
}