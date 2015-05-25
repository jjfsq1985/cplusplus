using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;
using System.Security.Cryptography;

namespace AccountManage
{
    public partial class AccountEdit : Form
    {
        private AccountInfo m_AccountInfo = new AccountInfo();

        public AccountEdit()
        {
            InitializeComponent();
            FillListAuthority();
            textName.Enabled = false;
        }

        public void SetAccountInfo(AccountInfo info)
        {
            m_AccountInfo = info;
        }

        public AccountInfo GetAccountInfo()
        {
            return m_AccountInfo;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (textPwdOld.Text.Length < 4 || textPwdOld.Text.Length > 32)
            {
                MessageBox.Show("输入密码后才能修改账户信息");
                return;
            }
            MD5 md5Provider = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.ASCII.GetBytes(textPwdOld.Text);
            byte[] targetData = md5Provider.ComputeHash(fromData);
            if (string.Compare(BitConverter.ToString(targetData).Replace("-", ""), m_AccountInfo.strPassword, true) != 0)
            {
                MessageBox.Show("原密码不正确，请重新输入");
                return;
            }
            if (textPwd.Text.Length >= 4 && textPwd.Text.Length <= 32)
            {
                if (textPwd.Text != textPwdAgain.Text)
                {
                    MessageBox.Show("两次输入的密码不一致");
                    return;
                }
                byte[] fromNewData = Encoding.ASCII.GetBytes(textPwd.Text);
                byte[] targetNewData = md5Provider.ComputeHash(fromNewData);
                m_AccountInfo.strPassword = BitConverter.ToString(targetNewData).Replace("-", "");
            }
            m_AccountInfo.UserAuthority = GetAuthorityValue();
            m_AccountInfo.UserStatus = AccountStop.Checked ? 2 : 0;

            this.DialogResult = DialogResult.OK;
        }

        private int GetAuthorityValue()
        {
            int nRet = 0;
            foreach (string itemChecked in ChkLBAuthority.CheckedItems)
            {
                int nIndex = ChkLBAuthority.Items.IndexOf(itemChecked);
                nRet |= (1 << nIndex);
            }
            return nRet;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FillListAuthority()
        {
            ChkLBAuthority.Items.Clear();
            int index = 0;
            foreach (string strAuthority in GrobalVariable.strAuthority)
            {
                ChkLBAuthority.Items.Add(strAuthority);
                index++;
            }
        }

        private void AccountEdit_Load(object sender, EventArgs e)
        {
            textName.Text = m_AccountInfo.strUserName;
            AccountStop.Checked = m_AccountInfo.UserStatus == 2 ? true : false;
            for (int i = 0; i < GrobalVariable.Authority_Config_Count; i++)
            {
                if ((m_AccountInfo.UserAuthority & (1 << i)) == (1 << i))
                {
                    ChkLBAuthority.SetItemChecked(i, true);
                }
            }
        }

    }
}
