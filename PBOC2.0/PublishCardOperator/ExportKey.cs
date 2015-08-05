using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SqlServerHelper;
using IFuncPlugin;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Xml;
using ApduParam;

namespace PublishCardOperator
{
    public partial class ExportKey : Form, IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();


        public ExportKey()
        {
            InitializeComponent();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eExportKeyXml;
        }

        public string PluginName()
        {
            return "ExportKeyXml";
        }

        public Guid PluginGuid()
        {
            return new Guid("D122EE72-2338-456c-88BD-531F2D2415CD");
        }

        public string PluginMenu()
        {
            return "导入导出密钥";
        }

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            Trace.Assert(nAuthority == GrobalVariable.CardOp_KeyManage_Authority); //必然有密钥管理权限           
        }

        private void ReadOrgKey(XmlDocument xml, XmlElement rootNode, SqlHelper ObjSql, string strName, int nType, byte[] encryptkey)
        {
            SqlDataReader dataReader = null;
            SqlParameter[] KeyType = new SqlParameter[1];
            KeyType[0] = ObjSql.MakeParam("OrgKeyType", SqlDbType.Int, 4, ParameterDirection.Input, nType);
            ObjSql.ExecuteProc("PROC_GetOrgKey", KeyType, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    string strKey = (string)dataReader["OrgKey"];
                    byte[] byteKey = PublicFunc.StringToBCD(strKey);
                    byte[] OrgKey = DesCryptography.TripleEncryptData(byteKey, encryptkey);
                    XmlNode node = xml.CreateNode(XmlNodeType.Element, strName, "");
                    node.InnerText = BitConverter.ToString(OrgKey).Replace("-", "");
                    rootNode.AppendChild(node);
                }
            }
            dataReader.Close();
            dataReader = null;
        }

        private void KeyToXmlNode(SqlDataReader dataReader, XmlDocument xml, XmlNode parentNode, string strName, byte[] encryptkey)
        {
            string strKey = (string)dataReader[strName];
            byte[] byteKey = PublicFunc.StringToBCD(strKey);
            byte[] byteEncrypt = DesCryptography.TripleEncryptData(byteKey, encryptkey);
            XmlNode node = xml.CreateNode(XmlNodeType.Element, strName, "");
            node.InnerText = BitConverter.ToString(byteEncrypt).Replace("-", "");
            parentNode.AppendChild(node);
        }

        private void ExportKeyToXml(string strXmlFile, byte[] InitData, byte[] InitKey)
        {
            XmlNode node = null;
            XmlDocument xml = new XmlDocument();
            XmlElement Root = xml.CreateElement("Root");
            xml.AppendChild(Root);

            node = xml.CreateNode(XmlNodeType.Element, "Seed", "");
            node.InnerText = BitConverter.ToString(InitData).Replace("-", "");
            Root.AppendChild(node);
            node = xml.CreateNode(XmlNodeType.Element, "InitKey", "");
            node.InnerText = BitConverter.ToString(InitKey).Replace("-", "");
            Root.AppendChild(node);

            byte[] Left = DesCryptography.TripleEncryptData(InitData, InitKey);
            byte[] Right = DesCryptography.TripleDecryptData(InitData, InitKey);
            byte[] EncryptKey = new byte[16];
            Buffer.BlockCopy(Left, 0, EncryptKey, 0, 8);
            Buffer.BlockCopy(Right, 0, EncryptKey, 8, 8);

            ReadOrgKey(xml, Root, m_ObjSql, "UserOrgKey", 0, EncryptKey);
            ReadOrgKey(xml, Root, m_ObjSql, "PsamOrgKey", 1, EncryptKey);

            SqlDataReader dataReader = null;

            //用户卡密钥
            SqlParameter[] sqlparam = new SqlParameter[1];
            sqlparam[0] = m_ObjSql.MakeParam("ApplicationIndex", SqlDbType.Int, 4, ParameterDirection.Input, 1);
            m_ObjSql.ExecuteProc("PROC_GetCpuKey", sqlparam, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    //卡应用1
                    XmlNode CpuKeyRoot = xml.CreateNode(XmlNodeType.Element, "UserKeyValue_App1", "");

                    KeyToXmlNode(dataReader,xml, CpuKeyRoot, "MasterKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "MasterTendingKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "ApplicatonMasterKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "ApplicationTendingKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "AppInternalAuthKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "PINResetKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "PINUnlockKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "ConsumerMasterKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "LoadKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "TacMasterKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "UnGrayKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "UnLoadKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, CpuKeyRoot, "OverdraftKey", EncryptKey);

                    Root.AppendChild(CpuKeyRoot);
                }
                dataReader.Close();
                dataReader = null;
            }

            //PSAM卡密钥
            m_ObjSql.ExecuteProc("PROC_GetPsamKey", out dataReader);
            if (dataReader != null)                
            {
                if(dataReader.HasRows && dataReader.Read())
                {
                    XmlNode PsamKeyRoot = xml.CreateNode(XmlNodeType.Element, "PsamKeyValue", "");

                    KeyToXmlNode(dataReader, xml, PsamKeyRoot, "MasterKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, PsamKeyRoot, "MasterTendingKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, PsamKeyRoot, "ApplicatonMasterKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, PsamKeyRoot, "ApplicationTendingKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, PsamKeyRoot, "ConsumerMasterKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, PsamKeyRoot, "GrayCardKey", EncryptKey);
                    KeyToXmlNode(dataReader, xml, PsamKeyRoot, "MacEncryptKey", EncryptKey);

                    Root.AppendChild(PsamKeyRoot);
                }

                dataReader.Close();
                dataReader = null;
            }
            xml.Save(strXmlFile);
        }

        private void ExportKey_Load(object sender, EventArgs e)
        {
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
            }
            InitKey();

            try
            {
                XmlNode node = null;
                XmlDocument xml = new XmlDocument();
                string strXmlPath = Application.StartupPath + @"\plugins\KeyValueCfg.xml";
                xml.Load(strXmlPath);//按路径读xml文件
                XmlNode root = xml.DocumentElement;
                XmlNode DbNode = root.SelectSingleNode("Config");//指向根节点
                node = DbNode.SelectSingleNode("Source");
                if (node.InnerText == "2")
                    ReadXml.Checked = true;
                else
                    ReadXml.Checked = false;
                node = DbNode.SelectSingleNode("xmlPath");
                textXmlPath.Text = node.InnerText;
            }
            catch
            {

            }

        }

        private void ExportKey_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        private void btnGenerateEncryptKey_Click(object sender, EventArgs e)
        {
            InitKey();
        }

        private void InitKey()
        {
            string strData = "";
            Random dataRand = new Random();
            for (int i = 0; i < 16; i++)
            {
                int nVal = dataRand.Next(0, 16);
                strData += nVal.ToString("X");
            }
            textData.Text = strData;
            Guid temp = Guid.NewGuid();
            textKey.Text = temp.ToString().Replace("-", "");
        }

        private void btnExportCardKey_Click(object sender, EventArgs e)
        {
            if (textData.Text.Length != 16 || textKey.Text.Length != 32)
            {
                MessageBox.Show("请输入16位初始数据和32位初始密钥，用于加密卡片密钥");
                return;
            }

            SaveFileDialog ExportKey = new SaveFileDialog();
            ExportKey.Filter = "卡密钥文件（*.xml）|*.xml|所有文件（*.*）|*.*";
            ExportKey.FilterIndex = 1;
            ExportKey.RestoreDirectory = true;
            if (ExportKey.ShowDialog() != DialogResult.OK)
                return;
            string strFilePath = ExportKey.FileName;
            byte[] data = PublicFunc.StringToBCD(textData.Text);
            byte[] InitKey = PublicFunc.StringToBCD(textKey.Text);
            ExportKeyToXml(strFilePath,data, InitKey);
            MessageBox.Show("导出密钥XML文件完成");

        }

        private void btnXmlPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog XmlPath = new OpenFileDialog();
            XmlPath.InitialDirectory = "C:\\";
            XmlPath.Filter = "卡密钥文件（*.xml）|*.xml|所有文件（*.*）|*.*";
            XmlPath.FilterIndex = 1;
            XmlPath.RestoreDirectory = true;
            if (XmlPath.ShowDialog() == DialogResult.OK)
            {
                textXmlPath.Text = XmlPath.FileName;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            XmlNode node = null;
            XmlDocument xml = new XmlDocument();
            string strXmlPath = Application.StartupPath + @"\plugins\KeyValueCfg.xml";
            XmlElement Root = xml.CreateElement("Config");
            xml.AppendChild(Root);


            node = xml.CreateNode(XmlNodeType.Element, "Source", "");
            node.InnerText = ReadXml.Checked == true ? "2" : "1";
            Root.AppendChild(node);

            node = xml.CreateNode(XmlNodeType.Element, "xmlPath", "");
            node.InnerText = textXmlPath.Text;
            Root.AppendChild(node);

            node = xml.CreateNode(XmlNodeType.Element, "Describ", "");
            node.InnerText = "从数据库读取密钥：1; 从XML文件读取密钥：2.";
            Root.AppendChild(node);            
            
            xml.Save(strXmlPath);            
        }
    }
}
