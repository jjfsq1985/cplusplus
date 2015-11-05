namespace RechargeManage
{
    partial class RechargeRecord
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
            this.RechargeView = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RechargeView)).BeginInit();
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
            // RechargeView
            // 
            this.RechargeView.AllowUserToAddRows = false;
            this.RechargeView.AllowUserToDeleteRows = false;
            this.RechargeView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.RechargeView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RechargeView.Dock = System.Windows.Forms.DockStyle.Top;
            this.RechargeView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.RechargeView.Location = new System.Drawing.Point(0, 0);
            this.RechargeView.MultiSelect = false;
            this.RechargeView.Name = "RechargeView";
            this.RechargeView.ReadOnly = true;
            this.RechargeView.RowHeadersVisible = false;
            this.RechargeView.RowTemplate.Height = 23;
            this.RechargeView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.RechargeView.Size = new System.Drawing.Size(783, 405);
            this.RechargeView.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(708, 411);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Location = new System.Drawing.Point(546, 411);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(75, 23);
            this.btnPrevPage.TabIndex = 2;
            this.btnPrevPage.Text = "上一页";
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(627, 411);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(75, 23);
            this.btnNextPage.TabIndex = 3;
            this.btnNextPage.Text = "下一页";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // RechargeRecord
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(783, 438);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrevPage);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.RechargeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RechargeRecord";
            this.Text = "充值信息";
            this.Load += new System.EventHandler(this.RechargeRecord_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RechargeRecord_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.RechargeView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.Button GasInfoQuit;
        private System.Windows.Forms.DataGridView RechargeView;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
    }
}