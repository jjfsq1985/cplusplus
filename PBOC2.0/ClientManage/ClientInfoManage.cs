using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;
using SqlServerHelper;
using System.Data.SqlClient;

namespace ClientManage
{
    public partial class ClientInfoManage : Form, IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();

        private List<ClientInfo> m_lstClientInfo = new List<ClientInfo>();

        private ClientInfo m_SelectedClient = null;
        private ContextMenuStrip m_treeMenu = new ContextMenuStrip();
        private ContextMenuStrip m_treeChildMenu = new ContextMenuStrip();
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nClientInfoAuthority = 0;


        public ClientInfoManage()
        {
            InitializeComponent();
            m_treeMenu.Items.Add("增加节点", null, new EventHandler(AddClientNode));
            m_treeMenu.Items.Add("删除节点", null, new EventHandler(DelClientNode));
            m_treeChildMenu.Items.Add("删除节点", null, new EventHandler(DelClientNode));
            
        }

        public MenuType GetMenuType()
        {
            return MenuType.eClientInfo;
        }

        public string PluginName()
        {
            return "ClientInfoManage";
        }

        public Guid PluginGuid()
        {
            return new Guid("FFC0BC06-C24E-4067-A911-352673F74931");
        }

        public string PluginMenu()
        {
            return "单位信息管理";
        }

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
            if (m_nClientInfoAuthority != GrobalVariable.ClientInfo_Authority)
            {
                btnAdd.Enabled = false;
                btnDel.Enabled = false;
                btnModify.Enabled = false;
            }
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            m_nClientInfoAuthority = nAuthority;
        }

        //加载单位信息数据库
        private void ClientInfoManage_Load(object sender, EventArgs e)
        {
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
                return;
            }
            ReadClientInfoFromDb();

            FillTreeView();
        }

        private void ReadClientInfoFromDb()
        {
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select * from Base_Client", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ClientInfo info = new ClientInfo();
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("ClientId")))
                            info.ClientId = (int)dataReader["ClientId"];
                        else
                            info.ClientId = 0;
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("ParentId")))
                            info.ParentId = (int)dataReader["ParentId"];
                        else
                            info.ParentId = 0;
                        info.ClientName = (string)dataReader["ClientName"];
                        info.ParentName = (string)dataReader["ParentName"];
                        info.LinkMan = (string)dataReader["LinkMan"];
                        info.Telephone = (string)dataReader["Telephone"];
                        info.FaxNum = (string)dataReader["FaxNum"];
                        info.Address = (string)dataReader["Address"];
                        info.Email = (string)dataReader["Email"];
                        info.Zipcode = (string)dataReader["Zipcode"];
                        info.BankName = (string)dataReader["Bank"];
                        info.BankAccount = (string)dataReader["BankAccountNum"];
                        info.Remark = (string)dataReader["Remark"];
                        m_lstClientInfo.Add(info);
                    }
                }
                dataReader.Close();
            }
        }

        private void FillTreeNode(TreeNode CurrentNode, int nIndex,int nKeyVal,ref List<ClientInfo> lstClient)
        {
            List<ClientInfo> lstChildNode = new List<ClientInfo>();
            for (int i = nIndex + 1; i < lstClient.Count; i++)
            {
                if (lstClient[i].ParentId == nKeyVal)
                {
                    TreeNode ChindNode = new TreeNode(lstClient[i].ClientName);
                    ChindNode.Tag = lstClient[i].ClientId;
                    FillTreeNode(ChindNode, i, lstClient[i].ClientId, ref lstClient);
                    CurrentNode.Nodes.Add(ChindNode);
                    lstChildNode.Add(lstClient[i]);
                }
            }
            foreach (ClientInfo removeinfo in lstChildNode)
            {
                lstClient.Remove(removeinfo);
            }
        }

        private void FillTreeView()
        {
            if (m_lstClientInfo.Count == 0)
                return;
            treeClient.Nodes.Clear();
            List<ClientInfo> tempInfo = new List<ClientInfo>(m_lstClientInfo);
            int tempIndex = 0;
            while (tempIndex < tempInfo.Count)
            {
                ClientInfo info = tempInfo[tempIndex];
                TreeNode Node = new TreeNode(info.ClientName);
                Node.Tag = info.ClientId;
                FillTreeNode(Node, tempIndex,info.ClientId, ref tempInfo);
                treeClient.Nodes.Add(Node);                
                tempIndex++;
            }
            treeClient.TopNode.Expand();
        }

        private void ClientInfoManage_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        //显示详细信息
        private void treeClient_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClientInfo  selectedInfo = null;
            foreach (ClientInfo info in m_lstClientInfo)
            {
                if (info.ClientId == (int)e.Node.Tag)
                {
                    selectedInfo = info;
                    break;
                }
            }
            if(selectedInfo != null)
            {
                m_SelectedClient = selectedInfo;
                textClientId.Text = selectedInfo.ClientId.ToString();
                textClientName.Text = selectedInfo.ClientName;
                textLinkMan.Text = selectedInfo.LinkMan;
                textTelephone.Text = selectedInfo.Telephone;
                textAddress.Text = selectedInfo.Address;
                textFaxNum.Text = selectedInfo.FaxNum;
                textEmail.Text = selectedInfo.Email;
                textZipcode.Text = selectedInfo.Zipcode;
                textBank.Text = selectedInfo.BankName;
                textBankAccount.Text = selectedInfo.BankAccount;
                textRemark.Text = selectedInfo.Remark;
            }
        }

        //显示右键菜单
        private void treeClient_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (m_nClientInfoAuthority != GrobalVariable.ClientInfo_Authority || e.Button != MouseButtons.Right)
                return;
            if (e.Node == null)
                return;

            if(e.Node.Parent != null)
            {
                treeClient.SelectedNode = e.Node;
                m_treeChildMenu.Show(treeClient, e.X, e.Y);
            }
            else
            {
                treeClient.SelectedNode = e.Node;
                m_treeMenu.Show(treeClient, e.X, e.Y);
            }
        }

        private void treeClient_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label) || e.Node == null)
                return;            
            string strQuestion = string.Format("是否将单位名称修改为{0}?",e.Label);
            if (MessageBox.Show(strQuestion, "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.CancelEdit = true;
                return;
            }
            e.Node.Text = e.Label;
            textClientName.Text = e.Label;
            //修改m_lstClientInfo并存入数据库
            int ClientId = (int)e.Node.Tag;
            for (int i = 0; i < m_lstClientInfo.Count; i++)
            {
                if (m_lstClientInfo[i].ClientId == ClientId)
                {
                    m_lstClientInfo[i].ClientName = e.Label;                                      
                }
                else if (m_lstClientInfo[i].ParentId == ClientId)
                {
                    m_lstClientInfo[i].ParentName = e.Label;
                }
            }

            SaveToDb(m_SelectedClient,DbStateFlag.eDbUpdate);
            
        }

        //修改关联数据库
        private void SaveToDb(ClientInfo info, DbStateFlag eState)
        {
            if (info == null)
                return;

            SqlParameter[] sqlparams = new SqlParameter[14];
            sqlparams[0] = m_ObjSql.MakeParam("ClientId", SqlDbType.Int, 4, ParameterDirection.Input, info.ClientId);
            sqlparams[1] = m_ObjSql.MakeParam("ClientName", SqlDbType.NVarChar, 50, ParameterDirection.Input, info.ClientName);
            sqlparams[2] = m_ObjSql.MakeParam("ParentID", SqlDbType.Int, 4, ParameterDirection.Input, info.ParentId);
            sqlparams[3] = m_ObjSql.MakeParam("ParentName", SqlDbType.NVarChar, 50, ParameterDirection.Input, info.ParentName);
            sqlparams[4] = m_ObjSql.MakeParam("Linkman", SqlDbType.NVarChar, 12, ParameterDirection.Input, info.LinkMan);
            sqlparams[5] = m_ObjSql.MakeParam("Telephone", SqlDbType.VarChar, 15, ParameterDirection.Input, info.LinkMan);
            sqlparams[6] = m_ObjSql.MakeParam("FaxNum", SqlDbType.VarChar, 50, ParameterDirection.Input, info.FaxNum);
            sqlparams[7] = m_ObjSql.MakeParam("Email", SqlDbType.VarChar, 50, ParameterDirection.Input, info.Email);
            sqlparams[8] = m_ObjSql.MakeParam("ZipCode", SqlDbType.VarChar, 10, ParameterDirection.Input, info.Zipcode);
            sqlparams[9] = m_ObjSql.MakeParam("Address", SqlDbType.NVarChar, 50, ParameterDirection.Input, info.Address);
            sqlparams[10] = m_ObjSql.MakeParam("Bank", SqlDbType.NVarChar, 50, ParameterDirection.Input, info.BankName);
            sqlparams[11] = m_ObjSql.MakeParam("BankAccountNum", SqlDbType.VarChar, 25, ParameterDirection.Input, info.BankAccount);
            sqlparams[12] = m_ObjSql.MakeParam("Remark", SqlDbType.NVarChar, 50, ParameterDirection.Input, info.Remark);
            sqlparams[13] = m_ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, eState);
            m_ObjSql.ExecuteProc("PROC_UpdateClientInfo", sqlparams);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            string strNodeName = textClientName.Text;
            int ClientId = m_SelectedClient.ClientId;
            //修改选中的ClientInfo
            m_SelectedClient.ClientName = strNodeName;
            m_SelectedClient.LinkMan = textLinkMan.Text;
            m_SelectedClient.Telephone = textTelephone.Text;
            m_SelectedClient.Address = textAddress.Text;
            m_SelectedClient.FaxNum = textFaxNum.Text;
            m_SelectedClient.Email = textEmail.Text;
            m_SelectedClient.Zipcode = textZipcode.Text;
            m_SelectedClient.BankName = textBank.Text;
            m_SelectedClient.BankAccount = textBankAccount.Text;
            m_SelectedClient.Remark = textRemark.Text;
            //修改m_lstClientInfo
            for(int i=0; i< m_lstClientInfo.Count; i++)
            {
                if (m_lstClientInfo[i].ClientId == ClientId)
                {
                    m_lstClientInfo[i] = m_SelectedClient;
                }
                else if (m_lstClientInfo[i].ParentId == ClientId)
                {
                    m_lstClientInfo[i].ParentName = strNodeName;
                }
            }
            //修改TreeView
            treeClient.SelectedNode.Text = strNodeName;
            SaveToDb(m_SelectedClient, DbStateFlag.eDbUpdate);

        }

        private void DelClientNode(object sender, EventArgs e)
        {
            if (m_SelectedClient == null)
                return;
            SaveToDb(m_SelectedClient, DbStateFlag.eDbDel);
            TreeNode parentNode = treeClient.SelectedNode.Parent;
            treeClient.SelectedNode.Remove();
            //更新选中项
            m_lstClientInfo.Remove(m_SelectedClient);
            if (parentNode != null)
            {
                treeClient.SelectedNode = parentNode;
                int nClientId = (int)parentNode.Tag;
                foreach (ClientInfo info in m_lstClientInfo)
                {
                    if (info.ClientId == nClientId)
                    {
                        m_SelectedClient = info;
                        break;
                    }
                }
            }
            else if (treeClient.TopNode != null)
            {
                treeClient.SelectedNode = treeClient.TopNode;
                int nClientId = (int)treeClient.TopNode.Tag;
                foreach (ClientInfo info in m_lstClientInfo)
                {
                    if (info.ClientId == nClientId)
                    {
                        m_SelectedClient = info;
                        break;
                    }
                }
            }
            else
            {
                treeClient.SelectedNode = null;
                m_SelectedClient = null;
            }
        }

        private void AddNextClientNode()
        {
            ClientInfo NodeClient = new ClientInfo();
            NodeClient.ParentId = 0;
            NodeClient.ParentName = "";
            NodeClient.ClientId = m_lstClientInfo.Count+1;
            NodeClient.ClientName = "新单位";
            m_lstClientInfo.Add(NodeClient);
            m_SelectedClient = NodeClient;
            //TreeView更新
            TreeNode childNode = new TreeNode(NodeClient.ClientName);
            childNode.Tag = NodeClient.ClientId;
            treeClient.Nodes.Add(childNode);
            treeClient.SelectedNode = childNode;
            SaveToDb(NodeClient, DbStateFlag.eDbAdd);
        }

        private void AddClientNode(object sender, EventArgs e)
        {
            if (m_SelectedClient == null)
                return;
            ClientInfo NodeClient = new ClientInfo();
            NodeClient.ParentId = m_SelectedClient.ClientId;
            NodeClient.ParentName = m_SelectedClient.ClientName;
            int nClientId = 1;
            for (int i = 0; i < m_lstClientInfo.Count; i++)
            {
                if (m_lstClientInfo[i].ClientId >= nClientId)
                    nClientId = m_lstClientInfo[i].ClientId + 1;
            }
            NodeClient.ClientId = nClientId;
            NodeClient.ClientName = "新单位";
            m_lstClientInfo.Add(NodeClient);
            m_SelectedClient = NodeClient;
            //TreeView更新
            TreeNode childNode = new TreeNode(NodeClient.ClientName);
            childNode.Tag = NodeClient.ClientId;
            treeClient.SelectedNode.Nodes.Add(childNode);
            treeClient.SelectedNode = childNode;



            SaveToDb(NodeClient, DbStateFlag.eDbAdd);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DelClientNode(this, EventArgs.Empty);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddNextClientNode();
        }

        private void treeClient_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (m_nClientInfoAuthority != GrobalVariable.ClientInfo_Authority)
                e.CancelEdit = true;
        }

    }
}
