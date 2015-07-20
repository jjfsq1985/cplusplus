using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;
using System.Security.Cryptography;
using SqlServerHelper;
using System.Data.SqlClient;
using System.Diagnostics;

namespace AccountManage
{
    public partial class AddAccount : Form
    {
        private const Char Backspace = (Char)8;

        private AccountInfo m_AccountInfo = new AccountInfo();
        private int m_nCurUserAuthority = 0;

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        public AddAccount()
        {
            InitializeComponent();
            FillListAuthority();
        }

        public void SetInfo(SqlConnectInfo DbInfo, int nCurUserAuthority)
        {
            m_DBInfo = DbInfo;
            m_nCurUserAuthority = nCurUserAuthority;
        }

        public AccountInfo GetAccountInfo()
        {
            return m_AccountInfo;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (textName.Text.Length < 2 || textName.Text.Length > 32)
            {
                MessageBox.Show("请输入用户名");
                return;
            }

            if (IsExistUserName(textName.Text))
            {
                MessageBox.Show("用户名已存在，请重新输入用户名");
                return;
            }

            if (textPwd.Text.Length < 4 || textPwd.Text.Length > 32)
            {
                MessageBox.Show("请设置长度为4-32个字符的密码");
                return;
            }
            if (textPwd.Text != textPwdAgain.Text)
            {
                MessageBox.Show("两次输入的密码不一致");
                return;
            }
            MD5 md5Provider = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.ASCII.GetBytes( textPwd.Text);
            byte[] targetData = md5Provider.ComputeHash(fromData);
            m_AccountInfo.strUserName = textName.Text;
            m_AccountInfo.strPassword = BitConverter.ToString(targetData).Replace("-", "");
            m_AccountInfo.UserAuthority = GetAuthorityValue();
            m_AccountInfo.UserStatus = 0;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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
            int i = 0;
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

        private void FillListAuthority()
        {
            ChkLBAuthority.Items.Clear();
            int index = 0;
            int nShowIndex = 0;
            foreach (string strAuthority in GrobalVariable.strAuthority)
            {
                int nAuthority = (1 << index);
                index++;
                if ((m_nCurUserAuthority & nAuthority) != nAuthority)
                    continue;
                ChkLBAuthority.Items.Add(strAuthority);
                ChkLBAuthority.SetItemChecked(nShowIndex, true);
                nShowIndex++;
            }
        }

        private void textName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && !Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非字母值
        }

        private bool IsExistUserName(string strName)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return false;
            }

            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("UserName", SqlDbType.VarChar, 32, ParameterDirection.Input, strName);

            bool bExist = false;
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from UserDb where UserName = @UserName", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    Trace.Assert((string)dataReader["UserName"] == strName);
                    bExist = true;                    
                }
                dataReader.Close();
             }


            ObjSql.CloseConnection();
            ObjSql = null;
            return bExist;
        }
    }
}
