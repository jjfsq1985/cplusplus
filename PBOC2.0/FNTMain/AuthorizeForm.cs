using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FNTMain
{
    public partial class AuthorizeForm : Form
    {
        public AuthorizeForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string strWriteAuthorize = textCode.Text;
            int nRet = LicenseCalc.AuthorizeVerify(strWriteAuthorize);
            if (nRet == 1)
            {
                LicenseCalc.SetAuthorize(strWriteAuthorize);
                MessageBox.Show("授权成功，请重新登录。提示：该授权有效期一个月。", "授权", MessageBoxButtons.OK);
                this.DialogResult = DialogResult.OK;
            }
            else if(nRet == 2)
            {
                MessageBox.Show("授权码已过期，请重新申请。", "授权", MessageBoxButtons.OK);
            }
            else            
            {
                MessageBox.Show("授权码不正确", "授权", MessageBoxButtons.OK);
            }
        }

        private void AuthorizeForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }
    }
}
