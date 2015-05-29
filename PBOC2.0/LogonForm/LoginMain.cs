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
using IFuncPlugin;
using System.Xml;

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

        SqlConnectInfo m_DbInfo = new SqlConnectInfo();

        public LoginMain()
        {
            InitializeComponent();

            ReadDbInfo();
        }

        public SqlConnectInfo GetDbInfo()
        {
            return m_DbInfo;
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void CheckEnterMainForm()
        {
            if (!m_DbInfo.m_bConfig)
            {
                MessageBox.Show("数据库尚未配置，请先设置数据库。");
                return;
            }

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
            if (!ObjSql.OpenSqlServerConnection(m_DbInfo.strServerName, m_DbInfo.strDbName, m_DbInfo.strUser, m_DbInfo.strUserPwd))
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
            if (!ObjSql.OpenSqlServerConnection(m_DbInfo.strServerName, m_DbInfo.strDbName, m_DbInfo.strUser, m_DbInfo.strUserPwd))
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

        private void btnDbSetting_Click(object sender, EventArgs e)
        {
            DbSetting setting = new DbSetting();
            setting.SetDbInfo(m_DbInfo);
            if (setting.ShowDialog(this) != DialogResult.OK)
                return;
            m_DbInfo = setting.GetDbInfo();
            SaveDbInfo();
        }

        private void SaveDbInfo()
        {
            XmlNode node = null;
            XmlDocument xml = new XmlDocument();
            string strXmlPath = Application.StartupPath + @"\DbConfig.xml";
            xml.Load(strXmlPath);//按路径读xml文件
            XmlNode root = xml.SelectSingleNode("DBConfig");//指向根节点
            root.Attributes["Connect"].InnerText = "true";
            node = root.SelectSingleNode("Server");
            node.InnerText = m_DbInfo.strServerName;             
            node = root.SelectSingleNode("DbName");
            node.InnerText = m_DbInfo.strDbName;
            node = root.SelectSingleNode("User");
            node.InnerText = m_DbInfo.strUser;
            node = root.SelectSingleNode("Pwd");
            if (!string.IsNullOrEmpty(m_DbInfo.strUserPwd))
            {
                node.InnerText = Convert.ToBase64String(Encoding.ASCII.GetBytes(m_DbInfo.strUserPwd));                
            }
            xml.Save(strXmlPath);
        }

        private void ReadDbInfo()
        {
            XmlNode node = null;
            XmlDocument xml = new XmlDocument();
            string strXmlPath = Application.StartupPath + @"\DbConfig.xml";
            xml.Load(strXmlPath);//按路径读xml文件
            XmlNode root = xml.SelectSingleNode("DBConfig");//指向根节点
            if (root.Attributes["Connect"].InnerText == "true")
                m_DbInfo.m_bConfig = true;
            node = root.SelectSingleNode("Server");
            m_DbInfo.strServerName = node.InnerText;
            node = root.SelectSingleNode("DbName");
            m_DbInfo.strDbName = node.InnerText;
            node = root.SelectSingleNode("User");
            m_DbInfo.strUser = node.InnerText;
            node = root.SelectSingleNode("Pwd");
            if (!string.IsNullOrEmpty(node.InnerText))
            {
                m_DbInfo.strUserPwd = Encoding.ASCII.GetString(Convert.FromBase64String(node.InnerText));
            }
        }

    }
}
