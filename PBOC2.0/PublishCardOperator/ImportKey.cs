using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;
using System.Xml;
using ApduParam;
using System.Diagnostics;
using System.Data.SqlClient;
using SqlServerHelper;
using CardControl;

namespace PublishCardOperator
{
    public partial class ImportKey : Form, IPlugin
    {
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        private int m_nImportAuthority = 0;

        public ImportKey()
        {
            InitializeComponent();
        }


        public MenuType GetMenuType()
        {
            return MenuType.eImportKeyXml;
        }

        public string PluginName()
        {
            return "ImportKeyXml";
        }

        public Guid PluginGuid()
        {
            return new Guid("04CD1292-9AC4-437f-BDD1-918E78846EFD");
        }

        public string PluginMenu()
        {
            return "制卡密钥配置";
        }

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
            if ((m_nImportAuthority & GrobalVariable.CardPublish_Authority) != GrobalVariable.CardPublish_Authority)
            {
                btnXmlPath.Enabled = false;
                ReadXml.Enabled = false;
                BtnSave.Enabled = false;
            }
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            m_nImportAuthority = nAuthority;
        }

        private void ImportKey_Load(object sender, EventArgs e)
        {
            try
            {
                XmlNode node = null;
                XmlDocument xml = new XmlDocument();
                string strXmlPath = Application.StartupPath + @"\plugins\KeyValueCfg.xml";
                xml.Load(strXmlPath);//按路径读xml文件
                XmlNode root = xml.DocumentElement;//指向根节点
                if (root.Name != "Config")
                    return;
                node = root.SelectSingleNode("Source");
                if (node.InnerText == "2")
                    ReadXml.Checked = true;
                else
                    ReadXml.Checked = false;
                node = root.SelectSingleNode("xmlPath");
                textXmlPath.Text = node.InnerText;
            }
            catch
            {

            }
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
            if (ReadXml.Checked && string.IsNullOrEmpty(textXmlPath.Text))
            {
                MessageBox.Show("请设置制卡密钥的XML文件路径");
                return;
            }
            XmlNode node = null;
            XmlDocument xml = new XmlDocument();
            string strXmlPath = Application.StartupPath + @"\plugins\KeyValueCfg.xml";
            XmlElement Root = xml.CreateElement("Config");
            xml.AppendChild(Root);
            XmlDeclaration xmldecl = xml.CreateXmlDeclaration("1.0", "utf-8", null);
            xml.InsertBefore(xmldecl, Root);

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

            if (ReadXml.Checked)
                MessageBox.Show("配置成功，以后制卡从XML文件读取密钥。");
            else
                MessageBox.Show("配置成功，以后制卡从数据库读取密钥。");

            SaveKeyToDB(strXmlPath);
        }

        //xml配置写入DB数据库
        private void SaveKeyToDB(string strXmlPath)
        {
            //有密钥管理权限则将xml的密钥写入数据库
            if ((m_nImportAuthority & GrobalVariable.CardOp_KeyManage_Authority) != GrobalVariable.CardOp_KeyManage_Authority)
                return;
            try
            {
                CpuKeyData XmlCpuKey = new CpuKeyData();
                XmlCpuKey.nAppIndex = 1;
                PsamKeyData XmlPsamKey = new PsamKeyData();
                if (!GlobalControl.GetXmlCpuKeyVal(strXmlPath, XmlCpuKey) || !GlobalControl.GetXmlPsamKeyVal(strXmlPath, XmlPsamKey))
                    return;
                UpdateDbOrgKey(XmlCpuKey.OrgKeyVal, XmlPsamKey.OrgKeyVal, "从XML导入");
                UpdateDbPsamKey(XmlPsamKey);

                CpuKeyData XmlCpuKey_Ly = new CpuKeyData();
                XmlCpuKey_Ly.nAppIndex = 2;
                if (!GlobalControl.GetXmlCpuKeyVal(strXmlPath, XmlCpuKey_Ly))
                    XmlCpuKey_Ly = null;
                UpdateDbCpuKey(XmlCpuKey,XmlCpuKey_Ly);
            }
            catch
            {
                MessageBox.Show("XML中的密钥保存至数据库失败。");
            }
        }

        private void UpdateDbOrgKey(byte[] UserOrgKey, byte[] PsamOrgKey, string strDescirbe)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            bool bEqual = PublicFunc.ByteDataEquals(UserOrgKey, PsamOrgKey);

            string strBcd = "";
            SqlParameter[] sqlparams = new SqlParameter[7];
            sqlparams[0] = ObjSql.MakeParam("KeyId", SqlDbType.Int, 4, ParameterDirection.Input, 0);
            strBcd = BitConverter.ToString(UserOrgKey).Replace("-", "");
            sqlparams[1] = ObjSql.MakeParam("OrgKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);
            if (!bEqual)
                sqlparams[2] = ObjSql.MakeParam("KeyType", SqlDbType.Int, 4, ParameterDirection.Input, 0);
            else
                sqlparams[2] = ObjSql.MakeParam("KeyType", SqlDbType.Int, 4, ParameterDirection.Input, 2);

            sqlparams[3] = ObjSql.MakeParam("KeyDetail", SqlDbType.NVarChar, 50, ParameterDirection.Input, strDescirbe);
            sqlparams[4] = ObjSql.MakeParam("KeyState", SqlDbType.Bit, 1, ParameterDirection.Input, true);
            sqlparams[5] = ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, DbStateFlag.eDbAdd);
            sqlparams[6] = ObjSql.MakeParam("AddKeyId", SqlDbType.Int, 4, ParameterDirection.Output, null);
            ObjSql.ExecuteProc("PROC_UpdateOrgKeyRoot", sqlparams);

            if (!bEqual)
            {
                strBcd = BitConverter.ToString(PsamOrgKey).Replace("-", "");
                sqlparams[1] = ObjSql.MakeParam("OrgKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);
                sqlparams[2] = ObjSql.MakeParam("KeyType", SqlDbType.Int, 4, ParameterDirection.Input, 1);
                ObjSql.ExecuteProc("PROC_UpdateOrgKeyRoot", sqlparams);
            }
            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void UpdateDbPsamKey(PsamKeyData XmlPsamKey)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            string strBcd = "";

            SqlParameter[] sqlparams = new SqlParameter[12];
            sqlparams[0] = ObjSql.MakeParam("KeyId", SqlDbType.Int, 4, ParameterDirection.Input, 0);

            strBcd = BitConverter.ToString(XmlPsamKey.MasterKeyVal).Replace("-", "");
            sqlparams[1] = ObjSql.MakeParam("MasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlPsamKey.MasterTendingKeyVal).Replace("-", "");
            sqlparams[2] = ObjSql.MakeParam("MasterTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlPsamKey.ApplicationMasterKey).Replace("-", "");
            sqlparams[3] = ObjSql.MakeParam("AppMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlPsamKey.ApplicationTendingKey).Replace("-", "");
            sqlparams[4] = ObjSql.MakeParam("AppTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlPsamKey.ConsumerMasterKey).Replace("-", "");
            sqlparams[5] = ObjSql.MakeParam("ConsumerMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlPsamKey.GrayCardKey).Replace("-", "");
            sqlparams[6] = ObjSql.MakeParam("GrayCardKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlPsamKey.MacEncryptKey).Replace("-", "");
            sqlparams[7] = ObjSql.MakeParam("MacEncryptKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            sqlparams[8] = ObjSql.MakeParam("KeyDetail", SqlDbType.NVarChar, 50, ParameterDirection.Input, XmlPsamKey.strDescribe);
            sqlparams[9] = ObjSql.MakeParam("KeyState", SqlDbType.Bit, 1, ParameterDirection.Input, true);
            sqlparams[10] = ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, DbStateFlag.eDbAdd);
            sqlparams[11] = ObjSql.MakeParam("AddKeyId", SqlDbType.Int, 4, ParameterDirection.Output, null);
            ObjSql.ExecuteProc("PROC_UpdatePsamKey", sqlparams);

            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void UpdateDbCpuKey(CpuKeyData XmlCpuKey, CpuKeyData XmlCpuKey_Ly)
        {            
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            string strBcd = "";

            SqlParameter[] sqlparams = new SqlParameter[8];
            sqlparams[0] = ObjSql.MakeParam("KeyId", SqlDbType.Int, 4, ParameterDirection.Input, 0);

            strBcd = BitConverter.ToString(XmlCpuKey.MasterKeyVal).Replace("-", "");
            sqlparams[1] = ObjSql.MakeParam("MasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlCpuKey.MasterTendingKeyVal).Replace("-", "");
            sqlparams[2] = ObjSql.MakeParam("MasterTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            //没有卡片内部认证密钥导入，用全0保存
            strBcd = "00000000000000000000000000000000";
            sqlparams[3] = ObjSql.MakeParam("InternalAuthKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);
            
            sqlparams[4] = ObjSql.MakeParam("KeyDetail", SqlDbType.NVarChar, 50, ParameterDirection.Input, XmlCpuKey.strDescribe);
            sqlparams[5] = ObjSql.MakeParam("KeyState", SqlDbType.Bit, 1, ParameterDirection.Input, true);
            sqlparams[6] = ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, DbStateFlag.eDbAdd);
            sqlparams[7] = ObjSql.MakeParam("AddKeyId", SqlDbType.Int, 4, ParameterDirection.Output, null);
            if (ObjSql.ExecuteProc("PROC_UpdateCpuKey", sqlparams) == 0)
            {
                int nRelatedKeyId = (int)sqlparams[7].Value;
                UpdateDbCpuAppKeyValue(ObjSql, XmlCpuKey, nRelatedKeyId);
                if(XmlCpuKey_Ly != null)
                    UpdateDbCpuAppKeyValue(ObjSql, XmlCpuKey_Ly, nRelatedKeyId);
            }

            ObjSql.CloseConnection();
            ObjSql = null;           
        }

        private void UpdateDbCpuAppKeyValue(SqlHelper ObjSql, CpuKeyData XmlCpuKey, int nRelatedKeyId)
        {
            string strBcd = "";
            SqlParameter[] sqlparams = new SqlParameter[14];
            sqlparams[0] = ObjSql.MakeParam("RelatedKeyId", SqlDbType.Int, 4, ParameterDirection.Input, nRelatedKeyId);
            sqlparams[1] = ObjSql.MakeParam("AppIndex", SqlDbType.Int, 4, ParameterDirection.Input, XmlCpuKey.nAppIndex);

            strBcd = BitConverter.ToString(XmlCpuKey.AppMasterKey).Replace("-", "");
            sqlparams[2] = ObjSql.MakeParam("AppMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlCpuKey.AppTendingKey).Replace("-", "");
            sqlparams[3] = ObjSql.MakeParam("AppTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlCpuKey.AppInternalAuthKey).Replace("-", "");
            sqlparams[4] = ObjSql.MakeParam("AppInternalAuthKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlCpuKey.AppPinResetKey).Replace("-", "");
            sqlparams[5] = ObjSql.MakeParam("PinResetKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlCpuKey.AppPinUnlockKey).Replace("-", "");
            sqlparams[6] = ObjSql.MakeParam("PinUnlockKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlCpuKey.AppConsumerKey).Replace("-", "");
            sqlparams[7] = ObjSql.MakeParam("ConsumerMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlCpuKey.AppLoadKey).Replace("-", "");
            sqlparams[8] = ObjSql.MakeParam("LoadKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlCpuKey.AppTacKey).Replace("-", "");
            sqlparams[9] = ObjSql.MakeParam("TacMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            strBcd = BitConverter.ToString(XmlCpuKey.AppUnGrayKey).Replace("-", "");
            sqlparams[10] = ObjSql.MakeParam("UnGrayKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            //不存在的密钥用0补满
            if (XmlCpuKey.nAppIndex == 1)
                strBcd = BitConverter.ToString(XmlCpuKey.AppUnLoadKey).Replace("-", "");
            else
                strBcd = "00000000000000000000000000000000";
            sqlparams[11] = ObjSql.MakeParam("UnLoadKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

            if (XmlCpuKey.nAppIndex == 1)
                strBcd = BitConverter.ToString(XmlCpuKey.AppOverdraftKey).Replace("-", "");
            else
                strBcd = "00000000000000000000000000000000";
            sqlparams[12] = ObjSql.MakeParam("OvertraftKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);            

            sqlparams[13] = ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, DbStateFlag.eDbAdd);

            ObjSql.ExecuteProc("PROC_UpdateCpuAppKey", sqlparams);
        }

    }
}
