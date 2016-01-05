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
using System.IO;

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

        private string szUserName;
        private string szPassword;

        public LoginMain()
        {
            InitializeComponent();

            this.AcceptButton = btnIn;
            this.CancelButton = btnOut;

            ReadDbInfo();
            ReadLoginInfo();
            textUser.Text = szUserName;
            textPwd.Text = szPassword;
        }

        public SqlConnectInfo GetDbInfo()
        {
            return m_DbInfo;
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            if (!m_DbInfo.m_bConfig)
            {
                MessageBox.Show("数据库尚未配置，请先设置数据库。");
                return;
            }
            if (string.IsNullOrEmpty(textUser.Text) || textUser.Text.Length < 2 || textUser.Text.Length > 32)
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            if (string.IsNullOrEmpty(textPwd.Text) || textPwd.Text.Length < 4 || textPwd.Text.Length > 32)
            {
                Info.ForeColor = Color.Red;
                Info.Text = "(4-32个字符)";
                return;
            }
            szUserName = textUser.Text;
            szPassword = textPwd.Text;

            if (!CheckUserAndPwd(szUserName, szPassword))
            {
                MessageBox.Show("用户名或密码不正确，请重新输入");
            }
            else
            {                
                SaveLoginInfo();
                UpdateUserDb();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void CheckPwd()
        {
            Info.ForeColor = Color.Black;
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
            if (File.Exists(strXmlPath))
                xml.Load(strXmlPath);
            XmlElement Root = xml.DocumentElement;
            if (Root == null)
            {
                Root = xml.CreateElement("Root");
                xml.AppendChild(Root);
                XmlDeclaration xmldecl = xml.CreateXmlDeclaration("1.0", "utf-8", null);
                xml.InsertBefore(xmldecl, Root);
            }

            XmlNode dbRoot = null;            
            if(Root != null)
                dbRoot = Root.SelectSingleNode("DBConfig");
            if (dbRoot == null)
            {
                dbRoot = xml.CreateNode(XmlNodeType.Element, "DBConfig", "");

                XmlAttribute DbCfgAttr = xml.CreateAttribute("Connect");
                DbCfgAttr.Value = "true";
                dbRoot.Attributes.Append(DbCfgAttr);

                node = xml.CreateNode(XmlNodeType.Element, "Server", "");
                node.InnerText = m_DbInfo.strServerName;
                dbRoot.AppendChild(node);

                node = xml.CreateNode(XmlNodeType.Element, "DbName", "");
                node.InnerText = m_DbInfo.strDbName;
                dbRoot.AppendChild(node);

                node = xml.CreateNode(XmlNodeType.Element, "User", "");
                node.InnerText = m_DbInfo.strUser;
                dbRoot.AppendChild(node);

                node = xml.CreateNode(XmlNodeType.Element, "Pwd", "");
                if (!string.IsNullOrEmpty(m_DbInfo.strUserPwd))
                {
                    node.InnerText = Convert.ToBase64String(Encoding.ASCII.GetBytes(m_DbInfo.strUserPwd));
                }
                else
                {
                    node.InnerText = "";
                }
                dbRoot.AppendChild(node);

                Root.AppendChild(dbRoot);
            }
            else
            {
                dbRoot.Attributes["Connect"].InnerText = "true";
                node = dbRoot.SelectSingleNode("Server");
                node.InnerText = m_DbInfo.strServerName;
                node = dbRoot.SelectSingleNode("DbName");
                node.InnerText = m_DbInfo.strDbName;
                node = dbRoot.SelectSingleNode("User");
                node.InnerText = m_DbInfo.strUser;
                node = dbRoot.SelectSingleNode("Pwd");
                if (!string.IsNullOrEmpty(m_DbInfo.strUserPwd))
                {
                    node.InnerText = Convert.ToBase64String(Encoding.ASCII.GetBytes(m_DbInfo.strUserPwd));
                }
                else
                {
                    node.InnerText = "";
                }
            }
            
            xml.Save(strXmlPath);
        }

        private void ReadDbInfo()
        {
            try
            {
                XmlNode node = null;
                XmlDocument xml = new XmlDocument();
                string strXmlPath = Application.StartupPath + @"\DbConfig.xml";
                xml.Load(strXmlPath);//按路径读xml文件
                XmlNode root = xml.DocumentElement;
                XmlNode DbNode = root.SelectSingleNode("DBConfig");//指向根节点
                if (DbNode.Attributes["Connect"].InnerText == "true")
                    m_DbInfo.m_bConfig = true;
                node = DbNode.SelectSingleNode("Server");
                m_DbInfo.strServerName = node.InnerText;
                node = DbNode.SelectSingleNode("DbName");
                m_DbInfo.strDbName = node.InnerText;
                node = DbNode.SelectSingleNode("User");
                m_DbInfo.strUser = node.InnerText;
                node = DbNode.SelectSingleNode("Pwd");
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    m_DbInfo.strUserPwd = Encoding.ASCII.GetString(Convert.FromBase64String(node.InnerText));
                }
                else 
                {
                    m_DbInfo.strUserPwd = "";
                }
            }
            catch
            {

            }
        }

        private void SaveLoginInfo()
        {
            XmlNode node = null;
            XmlDocument xml = new XmlDocument();
            string strXmlPath = Application.StartupPath + @"\DbConfig.xml";
            if (File.Exists(strXmlPath))                
                xml.Load(strXmlPath);
            XmlElement Root = xml.DocumentElement;
            if (Root == null)
            {
                Root = xml.CreateElement("Root");
                xml.AppendChild(Root);
                XmlDeclaration xmldecl = xml.CreateXmlDeclaration("1.0", "utf-8", null);
                xml.InsertBefore(xmldecl, Root);
            }

            XmlNode loginRoot = null;            
            if(Root != null)
                loginRoot = Root.SelectSingleNode("LoginCfg");
            if (loginRoot == null)
            {
                loginRoot = xml.CreateNode(XmlNodeType.Element, "LoginCfg", "");
            
                node = xml.CreateNode(XmlNodeType.Element, "User", "");
                node.InnerText = szUserName;
                loginRoot.AppendChild(node);

                node = xml.CreateNode(XmlNodeType.Element, "Password", "");
                if(SaveLogin.Checked && !string.IsNullOrEmpty(szPassword))
                {
                    node.InnerText = Convert.ToBase64String(Encoding.ASCII.GetBytes(szPassword));
                }
                else
                {
                    node.InnerText = "";
                }
                loginRoot.AppendChild(node);
                Root.AppendChild(loginRoot);
            }
            else
            {
                node = loginRoot.SelectSingleNode("User");
                node.InnerText = szUserName;
                node = loginRoot.SelectSingleNode("Password");
                if (SaveLogin.Checked && !string.IsNullOrEmpty(szPassword))
                {
                    node.InnerText = Convert.ToBase64String(Encoding.ASCII.GetBytes(szPassword));
                }
                else
                {
                    node.InnerText = "";
                }
            }
            xml.Save(strXmlPath);            
        }


        private void ReadLoginInfo()
        {
            try
            {
                XmlNode node = null;
                XmlDocument xml = new XmlDocument();
                string strXmlPath = Application.StartupPath + @"\DbConfig.xml";
                xml.Load(strXmlPath);//按路径读xml文件
                XmlNode root = xml.DocumentElement;
                XmlNode loginNode = root.SelectSingleNode("LoginCfg");//指向根节点
                node = loginNode.SelectSingleNode("User");
                szUserName = node.InnerText;
                node = loginNode.SelectSingleNode("Password");
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    szPassword = Encoding.ASCII.GetString(Convert.FromBase64String(node.InnerText));
                    SaveLogin.Checked = true;
                }
                else
                {
                    szPassword = "";
                    SaveLogin.Checked = false;
                }
            }
            catch
            {

            }
        }

    }
}
