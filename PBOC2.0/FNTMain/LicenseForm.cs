using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FNTMain
{
    public partial class LicenseForm : Form
    {
        public LicenseForm(string strSrcCode)
        {
            InitializeComponent();
            
            textCode.Text = strSrcCode;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            string strWriteLicense = textLicense.Text;
            if (!LicenseCalc.LicenseVerify(textCode.Text, strWriteLicense))
            {
                MessageBox.Show("注册码不正确","软件注册", MessageBoxButtons.OK);
            }
            else
            {
                LicenseCalc.SetSN(strWriteLicense);
                MessageBox.Show("注册码验证成功", "软件注册", MessageBoxButtons.OK);
                this.DialogResult = DialogResult.OK;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }
    }
}
