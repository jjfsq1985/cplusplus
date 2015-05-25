namespace PublishCardOperator
{
    partial class PSAMKeyManage
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
            this.PsamKeyLabel = new System.Windows.Forms.Label();
            this.PsamKeyView = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEditKey = new System.Windows.Forms.Button();
            this.btnSaveEdit = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PsamKeyView)).BeginInit();
            this.SuspendLayout();
            // 
            // PsamKeyLabel
            // 
            this.PsamKeyLabel.AutoSize = true;
            this.PsamKeyLabel.Location = new System.Drawing.Point(15, 9);
            this.PsamKeyLabel.Name = "PsamKeyLabel";
            this.PsamKeyLabel.Size = new System.Drawing.Size(89, 12);
            this.PsamKeyLabel.TabIndex = 1;
            this.PsamKeyLabel.Text = "PSAM卡密钥列表";
            // 
            // PsamKeyView
            // 
            this.PsamKeyView.AllowUserToAddRows = false;
            this.PsamKeyView.AllowUserToDeleteRows = false;
            this.PsamKeyView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.PsamKeyView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PsamKeyView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.PsamKeyView.Location = new System.Drawing.Point(17, 24);
            this.PsamKeyView.MultiSelect = false;
            this.PsamKeyView.Name = "PsamKeyView";
            this.PsamKeyView.RowHeadersVisible = false;
            this.PsamKeyView.RowTemplate.Height = 23;
            this.PsamKeyView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.PsamKeyView.Size = new System.Drawing.Size(715, 253);
            this.PsamKeyView.TabIndex = 0;
            this.PsamKeyView.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.PsamKeyView_CellLeave);
            this.PsamKeyView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.PsamKeyView_CellEnter);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(26, 287);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "增加 ";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(124, 287);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEditKey
            // 
            this.btnEditKey.Location = new System.Drawing.Point(222, 287);
            this.btnEditKey.Name = "btnEditKey";
            this.btnEditKey.Size = new System.Drawing.Size(75, 23);
            this.btnEditKey.TabIndex = 4;
            this.btnEditKey.Text = "编辑列表";
            this.btnEditKey.UseVisualStyleBackColor = true;
            this.btnEditKey.Click += new System.EventHandler(this.btnEditKey_Click);
            // 
            // btnSaveEdit
            // 
            this.btnSaveEdit.Location = new System.Drawing.Point(320, 287);
            this.btnSaveEdit.Name = "btnSaveEdit";
            this.btnSaveEdit.Size = new System.Drawing.Size(75, 23);
            this.btnSaveEdit.TabIndex = 5;
            this.btnSaveEdit.Text = "保存编辑";
            this.btnSaveEdit.UseVisualStyleBackColor = true;
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(657, 316);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(636, 287);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(75, 23);
            this.btnNextPage.TabIndex = 12;
            this.btnNextPage.Text = "下一页";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Location = new System.Drawing.Point(477, 287);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(75, 23);
            this.btnPrevPage.TabIndex = 11;
            this.btnPrevPage.Text = "上一页";
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // PSAMKeyManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 351);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrevPage);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveEdit);
            this.Controls.Add(this.btnEditKey);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.PsamKeyView);
            this.Controls.Add(this.PsamKeyLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PSAMKeyManage";
            this.Text = "PSAM卡密钥管理";
            this.Load += new System.EventHandler(this.PSAMKeyManage_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PSAMKeyManage_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.PsamKeyView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PsamKeyLabel;
        private System.Windows.Forms.DataGridView PsamKeyView;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEditKey;
        private System.Windows.Forms.Button btnSaveEdit;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrevPage;
    }
}