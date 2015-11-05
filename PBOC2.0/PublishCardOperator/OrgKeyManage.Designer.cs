namespace PublishCardOperator
{
    partial class OrgKeyManage
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
            this.OrgKeyGridView = new System.Windows.Forms.DataGridView();
            this.OrgKeyLabel = new System.Windows.Forms.Label();
            this.btnAddOrgKey = new System.Windows.Forms.Button();
            this.btnDelOrgKey = new System.Windows.Forms.Button();
            this.btnModifyOrgKey = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveEdit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OrgKeyGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // OrgKeyGridView
            // 
            this.OrgKeyGridView.AllowUserToAddRows = false;
            this.OrgKeyGridView.AllowUserToDeleteRows = false;
            this.OrgKeyGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.OrgKeyGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OrgKeyGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.OrgKeyGridView.Location = new System.Drawing.Point(8, 30);
            this.OrgKeyGridView.MultiSelect = false;
            this.OrgKeyGridView.Name = "OrgKeyGridView";
            this.OrgKeyGridView.RowHeadersVisible = false;
            this.OrgKeyGridView.RowTemplate.Height = 23;
            this.OrgKeyGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.OrgKeyGridView.Size = new System.Drawing.Size(555, 224);
            this.OrgKeyGridView.TabIndex = 0;
            this.OrgKeyGridView.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OrgKeyGridView_CellLeave);
            this.OrgKeyGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OrgKeyGridView_CellEnter);
            // 
            // OrgKeyLabel
            // 
            this.OrgKeyLabel.AutoSize = true;
            this.OrgKeyLabel.Location = new System.Drawing.Point(6, 11);
            this.OrgKeyLabel.Name = "OrgKeyLabel";
            this.OrgKeyLabel.Size = new System.Drawing.Size(77, 12);
            this.OrgKeyLabel.TabIndex = 1;
            this.OrgKeyLabel.Text = "初始密钥列表";
            // 
            // btnAddOrgKey
            // 
            this.btnAddOrgKey.Location = new System.Drawing.Point(8, 262);
            this.btnAddOrgKey.Name = "btnAddOrgKey";
            this.btnAddOrgKey.Size = new System.Drawing.Size(75, 23);
            this.btnAddOrgKey.TabIndex = 3;
            this.btnAddOrgKey.Text = "增加";
            this.btnAddOrgKey.UseVisualStyleBackColor = true;
            this.btnAddOrgKey.Click += new System.EventHandler(this.btnAddOrgKey_Click);
            // 
            // btnDelOrgKey
            // 
            this.btnDelOrgKey.Location = new System.Drawing.Point(89, 262);
            this.btnDelOrgKey.Name = "btnDelOrgKey";
            this.btnDelOrgKey.Size = new System.Drawing.Size(75, 23);
            this.btnDelOrgKey.TabIndex = 4;
            this.btnDelOrgKey.Text = "删除";
            this.btnDelOrgKey.UseVisualStyleBackColor = true;
            this.btnDelOrgKey.Click += new System.EventHandler(this.btnDelOrgKey_Click);
            // 
            // btnModifyOrgKey
            // 
            this.btnModifyOrgKey.Location = new System.Drawing.Point(170, 262);
            this.btnModifyOrgKey.Name = "btnModifyOrgKey";
            this.btnModifyOrgKey.Size = new System.Drawing.Size(75, 23);
            this.btnModifyOrgKey.TabIndex = 5;
            this.btnModifyOrgKey.Text = "编辑列表";
            this.btnModifyOrgKey.UseVisualStyleBackColor = true;
            this.btnModifyOrgKey.Click += new System.EventHandler(this.btnModifyOrgKey_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(488, 262);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveEdit
            // 
            this.btnSaveEdit.Location = new System.Drawing.Point(251, 262);
            this.btnSaveEdit.Name = "btnSaveEdit";
            this.btnSaveEdit.Size = new System.Drawing.Size(75, 23);
            this.btnSaveEdit.TabIndex = 7;
            this.btnSaveEdit.Text = "保存编辑";
            this.btnSaveEdit.UseVisualStyleBackColor = true;
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);
            // 
            // OrgKeyManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 298);
            this.Controls.Add(this.btnSaveEdit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnModifyOrgKey);
            this.Controls.Add(this.btnDelOrgKey);
            this.Controls.Add(this.btnAddOrgKey);
            this.Controls.Add(this.OrgKeyLabel);
            this.Controls.Add(this.OrgKeyGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrgKeyManage";
            this.Text = "卡片初始密钥管理";
            this.Load += new System.EventHandler(this.OrgKeyManage_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OrgKeyManage_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.OrgKeyGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView OrgKeyGridView;
        private System.Windows.Forms.Label OrgKeyLabel;
        private System.Windows.Forms.Button btnAddOrgKey;
        private System.Windows.Forms.Button btnDelOrgKey;
        private System.Windows.Forms.Button btnModifyOrgKey;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveEdit;
    }
}