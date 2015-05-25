namespace AccountManage
{
    partial class Account
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
            this.AccountQuit = new System.Windows.Forms.Button();
            this.UserGridView = new System.Windows.Forms.DataGridView();
            this.labelUser = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.PrevPage = new System.Windows.Forms.Button();
            this.NextPage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UserGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // AccountQuit
            // 
            this.AccountQuit.Location = new System.Drawing.Point(549, 295);
            this.AccountQuit.Name = "AccountQuit";
            this.AccountQuit.Size = new System.Drawing.Size(73, 26);
            this.AccountQuit.TabIndex = 0;
            this.AccountQuit.Text = "退出";
            this.AccountQuit.UseVisualStyleBackColor = true;
            this.AccountQuit.Click += new System.EventHandler(this.AccountQuit_Click);
            // 
            // UserGridView
            // 
            this.UserGridView.AllowUserToAddRows = false;
            this.UserGridView.AllowUserToDeleteRows = false;
            this.UserGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.UserGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UserGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.UserGridView.Location = new System.Drawing.Point(12, 31);
            this.UserGridView.MultiSelect = false;
            this.UserGridView.Name = "UserGridView";
            this.UserGridView.RowHeadersVisible = false;
            this.UserGridView.RowTemplate.Height = 23;
            this.UserGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UserGridView.Size = new System.Drawing.Size(607, 229);
            this.UserGridView.TabIndex = 1;
            this.UserGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.UserGridView_CellDoubleClick);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(12, 15);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(53, 12);
            this.labelUser.TabIndex = 2;
            this.labelUser.Text = "账户列表";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 266);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(53, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "增加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(115, 266);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(53, 23);
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(218, 266);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(53, 23);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(321, 266);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // PrevPage
            // 
            this.PrevPage.Location = new System.Drawing.Point(424, 266);
            this.PrevPage.Name = "PrevPage";
            this.PrevPage.Size = new System.Drawing.Size(53, 23);
            this.PrevPage.TabIndex = 7;
            this.PrevPage.Text = "上一页";
            this.PrevPage.UseVisualStyleBackColor = true;
            this.PrevPage.Click += new System.EventHandler(this.PrevPage_Click);
            // 
            // NextPage
            // 
            this.NextPage.Location = new System.Drawing.Point(527, 266);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(53, 23);
            this.NextPage.TabIndex = 8;
            this.NextPage.Text = "下一页";
            this.NextPage.UseVisualStyleBackColor = true;
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // Account
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(629, 326);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrevPage);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.UserGridView);
            this.Controls.Add(this.AccountQuit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Account";
            this.Text = "账户管理";
            this.Load += new System.EventHandler(this.Account_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Account_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.UserGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AccountQuit;
        private System.Windows.Forms.DataGridView UserGridView;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button PrevPage;
        private System.Windows.Forms.Button NextPage;
    }
}