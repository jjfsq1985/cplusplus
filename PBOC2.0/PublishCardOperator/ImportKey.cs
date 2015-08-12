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

namespace PublishCardOperator
{
    public partial class ImportKey : Form, IPlugin
    {
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
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
            if (m_nImportAuthority != GrobalVariable.CardPublish_Authority)
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
        }

    }
}
