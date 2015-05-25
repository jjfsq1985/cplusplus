namespace CodeTable
{
    partial class CompanyCode
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.DataViewPanel = new System.Windows.Forms.Panel();
            this.SuperiorView = new System.Windows.Forms.DataGridView();
            this.SuperiorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuperiorCodeValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDel = new System.Windows.Forms.Button();
            this.DataViewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SuperiorView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(262, 90);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(31, 23);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // DataViewPanel
            // 
            this.DataViewPanel.Controls.Add(this.SuperiorView);
            this.DataViewPanel.Location = new System.Drawing.Point(0, 0);
            this.DataViewPanel.Name = "DataViewPanel";
            this.DataViewPanel.Size = new System.Drawing.Size(256, 311);
            this.DataViewPanel.TabIndex = 7;
            // 
            // SuperiorView
            // 
            this.SuperiorView.AllowUserToAddRows = false;
            this.SuperiorView.AllowUserToDeleteRows = false;
            this.SuperiorView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.SuperiorView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SuperiorView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SuperiorName,
            this.SuperiorCodeValue});
            this.SuperiorView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SuperiorView.Location = new System.Drawing.Point(0, 0);
            this.SuperiorView.MultiSelect = false;
            this.SuperiorView.Name = "SuperiorView";
            this.SuperiorView.RowHeadersVisible = false;
            this.SuperiorView.RowTemplate.Height = 23;
            this.SuperiorView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.SuperiorView.Size = new System.Drawing.Size(256, 311);
            this.SuperiorView.TabIndex = 0;
            this.SuperiorView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.SuperiorView_CellValidated);
            // 
            // SuperiorName
            // 
            this.SuperiorName.HeaderText = "公司名称";
            this.SuperiorName.MaxInputLength = 50;
            this.SuperiorName.Name = "SuperiorName";
            this.SuperiorName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SuperiorName.Width = 150;
            // 
            // SuperiorCodeValue
            // 
            this.SuperiorCodeValue.HeaderText = "公司代码";
            this.SuperiorCodeValue.MaxInputLength = 4;
            this.SuperiorCodeValue.Name = "SuperiorCodeValue";
            this.SuperiorCodeValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(262, 154);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(31, 23);
            this.btnDel.TabIndex = 9;
            this.btnDel.Text = "-";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // CompanyCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 311);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.DataViewPanel);
            this.Controls.Add(this.btnDel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CompanyCode";
            this.Text = "公司代码";
            this.Load += new System.EventHandler(this.CompanyCode_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CompanyCode_FormClosed);
            this.DataViewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SuperiorView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel DataViewPanel;
        private System.Windows.Forms.DataGridView SuperiorView;
        private System.Windows.Forms.DataGridViewTextBoxColumn SuperiorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SuperiorCodeValue;
        private System.Windows.Forms.Button btnDel;
    }
}

