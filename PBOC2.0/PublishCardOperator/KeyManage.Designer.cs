namespace PublishCardOperator
{
    partial class KeyManage
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
            this.CpuKeyLabel = new System.Windows.Forms.Label();
            this.CpuKeyGridView = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveEdit = new System.Windows.Forms.Button();
            this.btnEditKey = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.AppKeyGridView = new System.Windows.Forms.DataGridView();
            this.AppKeyLabel = new System.Windows.Forms.Label();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CpuKeyGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AppKeyGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CpuKeyLabel
            // 
            this.CpuKeyLabel.AutoSize = true;
            this.CpuKeyLabel.Location = new System.Drawing.Point(24, 9);
            this.CpuKeyLabel.Name = "CpuKeyLabel";
            this.CpuKeyLabel.Size = new System.Drawing.Size(89, 12);
            this.CpuKeyLabel.TabIndex = 0;
            this.CpuKeyLabel.Text = "用户卡密钥列表";
            // 
            // CpuKeyGridView
            // 
            this.CpuKeyGridView.AllowUserToAddRows = false;
            this.CpuKeyGridView.AllowUserToDeleteRows = false;
            this.CpuKeyGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.CpuKeyGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CpuKeyGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.CpuKeyGridView.Location = new System.Drawing.Point(13, 26);
            this.CpuKeyGridView.MultiSelect = false;
            this.CpuKeyGridView.Name = "CpuKeyGridView";
            this.CpuKeyGridView.RowHeadersVisible = false;
            this.CpuKeyGridView.RowTemplate.Height = 23;
            this.CpuKeyGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.CpuKeyGridView.Size = new System.Drawing.Size(694, 207);
            this.CpuKeyGridView.TabIndex = 1;
            this.CpuKeyGridView.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.CpuKeyGridView_CellLeave);
            this.CpuKeyGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.CpuKeyGridView_EditingControlShowing);
            this.CpuKeyGridView.CurrentCellChanged += new System.EventHandler(this.CpuKeyGridView_CurrentCellChanged);
            this.CpuKeyGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.CpuKeyGridView_CellEnter);
            this.CpuKeyGridView.Click += new System.EventHandler(this.CpuKeyGridView_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(749, 410);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveEdit
            // 
            this.btnSaveEdit.Location = new System.Drawing.Point(727, 202);
            this.btnSaveEdit.Name = "btnSaveEdit";
            this.btnSaveEdit.Size = new System.Drawing.Size(75, 23);
            this.btnSaveEdit.TabIndex = 7;
            this.btnSaveEdit.Text = "保存编辑";
            this.btnSaveEdit.UseVisualStyleBackColor = true;
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);
            // 
            // btnEditKey
            // 
            this.btnEditKey.Location = new System.Drawing.Point(727, 151);
            this.btnEditKey.Name = "btnEditKey";
            this.btnEditKey.Size = new System.Drawing.Size(75, 23);
            this.btnEditKey.TabIndex = 6;
            this.btnEditKey.Text = "编辑列表";
            this.btnEditKey.UseVisualStyleBackColor = true;
            this.btnEditKey.Click += new System.EventHandler(this.btnEditKey_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(727, 100);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(727, 49);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "增加 ";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // AppKeyGridView
            // 
            this.AppKeyGridView.AllowUserToAddRows = false;
            this.AppKeyGridView.AllowUserToDeleteRows = false;
            this.AppKeyGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.AppKeyGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AppKeyGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.AppKeyGridView.Location = new System.Drawing.Point(13, 278);
            this.AppKeyGridView.MultiSelect = false;
            this.AppKeyGridView.Name = "AppKeyGridView";
            this.AppKeyGridView.RowHeadersVisible = false;
            this.AppKeyGridView.RowTemplate.Height = 23;
            this.AppKeyGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.AppKeyGridView.Size = new System.Drawing.Size(811, 126);
            this.AppKeyGridView.TabIndex = 3;
            this.AppKeyGridView.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.AppKeyGridView_CellLeave);
            this.AppKeyGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.AppKeyGridView_EditingControlShowing);
            this.AppKeyGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.AppKeyGridView_CellEnter);
            this.AppKeyGridView.Click += new System.EventHandler(this.AppKeyGridView_Click);
            // 
            // AppKeyLabel
            // 
            this.AppKeyLabel.AutoSize = true;
            this.AppKeyLabel.Location = new System.Drawing.Point(24, 257);
            this.AppKeyLabel.Name = "AppKeyLabel";
            this.AppKeyLabel.Size = new System.Drawing.Size(77, 12);
            this.AppKeyLabel.TabIndex = 2;
            this.AppKeyLabel.Text = "应用密钥列表";
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Location = new System.Drawing.Point(180, 239);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(75, 23);
            this.btnPrevPage.TabIndex = 9;
            this.btnPrevPage.Text = "上一页";
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(454, 239);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(75, 23);
            this.btnNextPage.TabIndex = 10;
            this.btnNextPage.Text = "下一页";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // KeyManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 441);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrevPage);
            this.Controls.Add(this.AppKeyLabel);
            this.Controls.Add(this.AppKeyGridView);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveEdit);
            this.Controls.Add(this.btnEditKey);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.CpuKeyGridView);
            this.Controls.Add(this.CpuKeyLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyManage";
            this.Text = "用户卡密钥管理";
            this.Load += new System.EventHandler(this.KeyManage_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.KeyManage_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.CpuKeyGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AppKeyGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CpuKeyLabel;
        private System.Windows.Forms.DataGridView CpuKeyGridView;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveEdit;
        private System.Windows.Forms.Button btnEditKey;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView AppKeyGridView;
        private System.Windows.Forms.Label AppKeyLabel;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
    }
}