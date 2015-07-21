namespace DbManage
{
    partial class SqlSvr
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
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.textBackup = new System.Windows.Forms.TextBox();
            this.btnBackupPath = new System.Windows.Forms.Button();
            this.textRestore = new System.Windows.Forms.TextBox();
            this.btnRestorePath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(188, 82);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(75, 23);
            this.btnBackup.TabIndex = 0;
            this.btnBackup.Text = "备份";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(188, 202);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(75, 23);
            this.btnRestore.TabIndex = 1;
            this.btnRestore.Text = "还原";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // textBackup
            // 
            this.textBackup.Location = new System.Drawing.Point(12, 39);
            this.textBackup.Name = "textBackup";
            this.textBackup.ReadOnly = true;
            this.textBackup.Size = new System.Drawing.Size(425, 21);
            this.textBackup.TabIndex = 2;
            // 
            // btnBackupPath
            // 
            this.btnBackupPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBackupPath.Location = new System.Drawing.Point(443, 37);
            this.btnBackupPath.Name = "btnBackupPath";
            this.btnBackupPath.Size = new System.Drawing.Size(39, 21);
            this.btnBackupPath.TabIndex = 3;
            this.btnBackupPath.Text = "...";
            this.btnBackupPath.UseVisualStyleBackColor = true;
            this.btnBackupPath.Click += new System.EventHandler(this.btnBackupPath_Click);
            // 
            // textRestore
            // 
            this.textRestore.Location = new System.Drawing.Point(12, 161);
            this.textRestore.Name = "textRestore";
            this.textRestore.ReadOnly = true;
            this.textRestore.Size = new System.Drawing.Size(425, 21);
            this.textRestore.TabIndex = 4;
            // 
            // btnRestorePath
            // 
            this.btnRestorePath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRestorePath.Location = new System.Drawing.Point(443, 160);
            this.btnRestorePath.Name = "btnRestorePath";
            this.btnRestorePath.Size = new System.Drawing.Size(39, 21);
            this.btnRestorePath.TabIndex = 5;
            this.btnRestorePath.Text = "...";
            this.btnRestorePath.UseVisualStyleBackColor = true;
            this.btnRestorePath.Click += new System.EventHandler(this.btnRestorePath_Click);
            // 
            // SqlSvr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 262);
            this.Controls.Add(this.btnRestorePath);
            this.Controls.Add(this.textRestore);
            this.Controls.Add(this.btnBackupPath);
            this.Controls.Add(this.textBackup);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnBackup);
            this.Name = "SqlSvr";
            this.Text = "数据库备份与还原";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.TextBox textBackup;
        private System.Windows.Forms.Button btnBackupPath;
        private System.Windows.Forms.TextBox textRestore;
        private System.Windows.Forms.Button btnRestorePath;
    }
}