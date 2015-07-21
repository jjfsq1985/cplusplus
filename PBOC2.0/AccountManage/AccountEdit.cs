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
        private int m_nCurUserAuthority = 0;

        public AccountEdit()
        {
            InitializeComponent();
        }

        public void SetAccountInfo(AccountInfo info, int nCurUserAuthority)
        {
            m_AccountInfo = info;
            m_nCurUserAuthority = nCurUserAuthority;
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
                int nIndex = AuthorityIndexof(itemChecked);
                nRet |= (1 << nIndex);
            }
            return nRet;
        }

        private int AuthorityIndexof(string strVal)
        {
            int nRet = 0;
            int i=0;
            foreach (string strAuthority in GrobalVariable.strAuthority)
            {
                if (strVal == strAuthority)
                {
                    nRet = i;
                    break;
                }
                i++;
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
                int nAuthority = (1<<index);
                index++;
                if ((m_nCurUserAuthority & nAuthority) != nAuthority)
                    continue;
                ChkLBAuthority.Items.Add(strAuthority);
            }
        }

        private void AccountEdit_Load(object sender, EventArgs e)
        {
            FillListAuthority();
            textName.Text = m_AccountInfo.strUserName;
            AccountStop.Checked = m_AccountInfo.UserStatus == 2 ? true : false;
            int nShowIndex = 0;
            for (int i = 0; i < GrobalVariable.Authority_Config_Count; i++)
            {
                int nAuthority = (1 << i);
                if ((m_nCurUserAuthority & nAuthority) != nAuthority)
                    continue;
                if ((m_AccountInfo.UserAuthority & nAuthority) == nAuthority)
                {
                    ChkLBAuthority.SetItemChecked(nShowIndex, true);
                }
                nShowIndex++;
            }
        }

    }
}
