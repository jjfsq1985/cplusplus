namespace StationManage
{
    partial class StationManage
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
            this.StationView = new System.Windows.Forms.DataGridView();
            this.StationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Province = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Superior = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.StationPanel = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.StationView)).BeginInit();
            this.StationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // StationView
            // 
            this.StationView.AllowUserToAddRows = false;
            this.StationView.AllowUserToDeleteRows = false;
            this.StationView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.StationView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StationView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StationId,
            this.StationName,
            this.Province,
            this.City,
            this.Superior,
            this.ClientName});
            this.StationView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StationView.Location = new System.Drawing.Point(0, 0);
            this.StationView.MultiSelect = false;
            this.StationView.Name = "StationView";
            this.StationView.RowHeadersVisible = false;
            this.StationView.RowTemplate.Height = 23;
            this.StationView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.StationView.Size = new System.Drawing.Size(746, 302);
            this.StationView.TabIndex = 0;
            this.StationView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.StationView_CellBeginEdit);
            this.StationView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.StationView_CellValidated);
            this.StationView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.StationView_EditingControlShowing);
            // 
            // StationId
            // 
            this.StationId.HeaderText = "站点编号";
            this.StationId.MaxInputLength = 4;
            this.StationId.Name = "StationId";
            this.StationId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StationId.Width = 80;
            // 
            // StationName
            // 
            this.StationName.HeaderText = "站点名称";
            this.StationName.Name = "StationName";
            this.StationName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StationName.Width = 160;
            // 
            // Province
            // 
            this.Province.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.Province.HeaderText = "省名称";
            this.Province.Name = "Province";
            // 
            // City
            // 
            this.City.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.City.HeaderText = "地市名称";
            this.City.Name = "City";
            // 
            // Superior
            // 
            this.Superior.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.Superior.HeaderText = "公司名称";
            this.Superior.Name = "Superior";
            // 
            // ClientName
            // 
            this.ClientName.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.ClientName.HeaderText = "所属单位";
            this.ClientName.Name = "ClientName";
            this.ClientName.Width = 200;
            // 
            // StationPanel
            // 
            this.StationPanel.Controls.Add(this.StationView);
            this.StationPanel.Location = new System.Drawing.Point(1, 3);
            this.StationPanel.Name = "StationPanel";
            this.StationPanel.Size = new System.Drawing.Size(746, 302);
            this.StationPanel.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(774, 74);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(28, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(774, 179);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(28, 23);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "-";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // StationManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 307);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.StationPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StationManage";
            this.Text = "站点管理";
            this.Load += new System.EventHandler(this.StationManage_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StationManage_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.StationView)).EndInit();
            this.StationPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView StationView;
        private System.Windows.Forms.Panel StationPanel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn StationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn StationName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Province;
        private System.Windows.Forms.DataGridViewComboBoxColumn City;
        private System.Windows.Forms.DataGridViewComboBoxColumn Superior;
        private System.Windows.Forms.DataGridViewComboBoxColumn ClientName;
    }
}

