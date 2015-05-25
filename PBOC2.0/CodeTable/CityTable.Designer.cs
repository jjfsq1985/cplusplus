namespace CodeTable
{
    partial class CityCode
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.DataViewPanel = new System.Windows.Forms.Panel();
            this.CityView = new System.Windows.Forms.DataGridView();
            this.btnDel = new System.Windows.Forms.Button();
            this.CityName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CityCodeValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataViewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CityView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(262, 90);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(31, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // DataViewPanel
            // 
            this.DataViewPanel.Controls.Add(this.CityView);
            this.DataViewPanel.Location = new System.Drawing.Point(0, 0);
            this.DataViewPanel.Name = "DataViewPanel";
            this.DataViewPanel.Size = new System.Drawing.Size(256, 311);
            this.DataViewPanel.TabIndex = 4;
            // 
            // CityView
            // 
            this.CityView.AllowUserToAddRows = false;
            this.CityView.AllowUserToDeleteRows = false;
            this.CityView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.CityView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CityView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CityName,
            this.CityCodeValue});
            this.CityView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CityView.Location = new System.Drawing.Point(0, 0);
            this.CityView.MultiSelect = false;
            this.CityView.Name = "CityView";
            this.CityView.RowHeadersVisible = false;
            this.CityView.RowTemplate.Height = 23;
            this.CityView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.CityView.Size = new System.Drawing.Size(256, 311);
            this.CityView.TabIndex = 0;
            this.CityView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.CityView_CellValidated);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(262, 154);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(31, 23);
            this.btnDel.TabIndex = 6;
            this.btnDel.Text = "-";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // CityName
            // 
            this.CityName.HeaderText = "地市名称";
            this.CityName.MaxInputLength = 50;
            this.CityName.Name = "CityName";
            this.CityName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CityName.Width = 150;
            // 
            // CityCodeValue
            // 
            this.CityCodeValue.HeaderText = "地市代码";
            this.CityCodeValue.MaxInputLength = 4;
            this.CityCodeValue.Name = "CityCodeValue";
            this.CityCodeValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CityCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 311);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.DataViewPanel);
            this.Controls.Add(this.btnDel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CityCode";
            this.Text = "地市代码";
            this.Load += new System.EventHandler(this.CityCode_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CityCode_FormClosed);
            this.DataViewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CityView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel DataViewPanel;
        private System.Windows.Forms.DataGridView CityView;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn CityName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CityCodeValue;
    }
}