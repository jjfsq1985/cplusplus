using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SqlServerHelper;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace FNTMain
{
    public partial class LoginMain : Form
    {
        private const Char Backspace = (Char)8;
        private const Char EnterKey = (Char)13;

        private int m_UserSignInId = 0;
        public int UserLoginId
        {
            get { return m_UserSignInId; }
        }

        private string m_UserNameSignIn = "";
        public string UserLogin
        {
            get { return m_UserNameSignIn; }            
        }

        public LoginMain()
        {
            InitializeComponent();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void CheckEnterMainForm()
        {
            if (textUser.Text.Length < 2 || textUser.Text.Length > 32)
                return;
            if (textPwd.Text.Length < 4 || textPwd.Text.Length > 32)
                return;
            string strUser = textUser.Text;
            string strPwd = textPwd.Text;

            if (!CheckUserAndPwd(strUser, strPwd))
            {
               MessageBox.Show("用户名或密码不正确，请重新输入");
            }
            else
            {
                UpdateUserDb();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textUser.Text) || string.IsNullOrEmpty(textPwd.Text))
                return;
            CheckEnterMainForm();
        }

        private void CheckPwd()
        {
            if (textPwd.Text.Length < 4 || textPwd.Text.Length > 32)
                Info.Text = "(4-32个字符)";
            else
                Info.Text = "";
        }

        private void textUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && !Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非字母值
        }

        private void textPwd_Enter(object sender, EventArgs e)
        {
            CheckPwd();
        }

        private void textPwd_Leave(object sender, EventArgs e)
        {
            CheckPwd(); 
        }

        private void UpdateUserDb()
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection("(local)", "FunnettStation", "sa", "sasoft"))
            {
                ObjSql = null;
                return;
            }

            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("UserID", SqlDbType.Int, 4, ParameterDirection.Input, m_UserSignInId);
            ObjSql.ExecuteCommand("update UserDb set Status = 1 where UserId = @UserID", sqlparams);
            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private bool CheckUserAndPwd(string strUser, string strPwd)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection("(local)", "FunnettStation", "sa", "sasoft"))
            {
                ObjSql = null;
                return false;
            }

            bool bCheckPass = false; 
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("UserName", SqlDbType.VarChar, 32, ParameterDirection.Input, strUser);            

            SqlDataReader dataReader = null;
            //停用的账户不可登录
            ObjSql.ExecuteCommand("select * from UserDb where UserName = @UserName and Status <> 2", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Password")))
                    {
                        string strPwdInDb = (string)dataReader["Password"];

                        MD5   md5Provider = new MD5CryptoServiceProvider();  
                        byte[] fromData = Encoding.ASCII.GetBytes(strPwd);
                        byte[] targetData = md5Provider.ComputeHash(fromData);
                        if (string.Compare(BitConverter.ToString(targetData).Replace("-", ""), strPwdInDb, true) == 0)
                        {
                            bCheckPass = true;
                            m_UserSignInId = (int)dataReader["UserId"];
                            m_UserNameSignIn = (string)dataReader["UserName"];

                        }
                    }
                }
                dataReader.Close();
            }

            ObjSql.CloseConnection();
            ObjSql = null;
            return bCheckPass;
        }

        private void textPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == EnterKey && !string.IsNullOrEmpty(textUser.Text) && !string.IsNullOrEmpty(textPwd.Text))
            {
                CheckEnterMainForm();
            }
        }

    }
}
