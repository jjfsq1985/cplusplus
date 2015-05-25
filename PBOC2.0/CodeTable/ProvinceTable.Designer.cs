namespace CodeTable
{
    partial class ProvinceCode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProvinceView = new System.Windows.Forms.DataGridView();
            this.DataViewPanel = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.ProvinceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProvinceCodeValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ProvinceView)).BeginInit();
            this.DataViewPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProvinceView
            // 
            this.ProvinceView.AllowUserToAddRows = false;
            this.ProvinceView.AllowUserToDeleteRows = false;
            this.ProvinceView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ProvinceView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProvinceView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProvinceName,
            this.ProvinceCodeValue});
            this.ProvinceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProvinceView.Location = new System.Drawing.Point(0, 0);
            this.ProvinceView.MultiSelect = false;
            this.ProvinceView.Name = "ProvinceView";
            this.ProvinceView.RowHeadersVisible = false;
            this.ProvinceView.RowTemplate.Height = 23;
            this.ProvinceView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ProvinceView.Size = new System.Drawing.Size(256, 311);
            this.ProvinceView.TabIndex = 0;
            this.ProvinceView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProvinceView_CellValidated);
            // 
            // DataViewPanel
            // 
            this.DataViewPanel.Controls.Add(this.ProvinceView);
            this.DataViewPanel.Location = new System.Drawing.Point(2, 1);
            this.DataViewPanel.Name = "DataViewPanel";
            this.DataViewPanel.Size = new System.Drawing.Size(256, 311);
            this.DataViewPanel.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(264, 91);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(31, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(264, 155);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(31, 23);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "-";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // ProvinceName
            // 
            this.ProvinceName.HeaderText = "省名称";
            this.ProvinceName.MaxInputLength = 50;
            this.ProvinceName.Name = "ProvinceName";
            this.ProvinceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProvinceName.Width = 150;
            // 
            // ProvinceCodeValue
            // 
            this.ProvinceCodeValue.HeaderText = "省代码";
            this.ProvinceCodeValue.MaxInputLength = 2;
            this.ProvinceCodeValue.Name = "ProvinceCodeValue";
            this.ProvinceCodeValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProvinceCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 313);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.DataViewPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProvinceCode";
            this.Text = "省代码";
            this.Load += new System.EventHandler(this.ProvinceCode_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProvinceCode_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.ProvinceView)).EndInit();
            this.DataViewPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ProvinceView;
        private System.Windows.Forms.Panel DataViewPanel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProvinceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProvinceCodeValue;
    }
}