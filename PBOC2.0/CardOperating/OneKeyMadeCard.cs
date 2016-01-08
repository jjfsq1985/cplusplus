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
using ApduParam;
using ApduCtrl;
using ApduInterface;
using System.Diagnostics;

namespace CardOperating
{
    public partial class OneKeyMadeCard : BaseMadeCard, IPlugin
    {
        private string m_CompanyID = "0001";

        public OneKeyMadeCard()
        {
            InitializeComponent();
            cmbFactory.SelectedIndex = 0;
            cmbCardType.SelectedIndex = 0;
        }

        public MenuType GetMenuType()
        {
            return MenuType.eOneKeyMadeCard;
        }

        public string PluginName()
        {
            return "OneKeyMadeCard";
        }

        public Guid PluginGuid()
        {
            return new Guid("467991AA-6FD8-4bcb-93F5-3931434469B6");
        }

        public string PluginMenu()
        {
            return "一键制卡";
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
            Trace.Assert(nAuthority == GrobalVariable.CardOp_KeyManage_Authority);//必然有制卡的权限，密钥从数据库读取
        }

        private void CardOperating_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            m_DevControl.Close_Device();
        }      

        private void OneKeyMadeCard_Load(object sender, EventArgs e)
        {
            m_DevControl = new ApduController(ApduDomain.DaHua);

            CompanyId.Text = m_CompanyID;

            ReadInfoFromDb();

            cmbClientName.Items.Clear();
            foreach(ClientInfo info in m_ListClientInfo)
                cmbClientName.Items.Add(info.strClientName);
            if (cmbClientName.Items.Count > 0)
                cmbClientName.SelectedIndex = GetClientIdIndex(m_IccCardInfoPar.ClientID);
   
            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(m_CompanyID, nCardType);//从数据库读出
            UserCardId.Text = m_CompanyID + UserCardInfoParam.CardGroup.ToString("X2") + nCardType.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            m_CardInfoPar.SetCardId(m_CompanyID);

            TermialID.Text = GetTerminalIdFromDb(m_CompanyID);
            PsamId.Text = GetPsamCardIdFromDb(m_CompanyID);
            m_IccCardInfoPar.PSAMCardID = PsamId.Text;
            m_IccCardInfoPar.TermialID = TermialID.Text;

            cmbFactory.SelectedIndexChanged += new System.EventHandler(this.cmbFactory_SelectedIndexChanged);
            this.CompanyId.Validated += new System.EventHandler(this.CompanyId_Validated);
            this.cmbCardType.SelectedIndexChanged += new System.EventHandler(this.cmbCardType_SelectedIndexChanged);
            this.cmbClientName.SelectedIndexChanged += new System.EventHandler(this.cmbClientName_SelectedIndexChanged);
        }

        private CardType GetCardType(int nSelectIndex)
        {
            CardType eType = CardType.PersonalCard;

            switch (nSelectIndex)
            {
                case 0:
                    eType = CardType.PersonalCard;
                    break;
                case 1:
                    eType = CardType.ManagerCard;
                    break;
                case 2:
                    eType = CardType.EmployeeCard;
                    break;
                case 3:
                    eType = CardType.ServiceCard;
                    break;
                case 4:
                    eType = CardType.CompanySubCard;
                    break;
                case 5:
                    eType = CardType.CompanyMotherCard;
                    break;
            }
            return eType;
        }

        private string GetTerminalIdFromDb(string companyId)
        {
            int nOrderNo = 1;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return companyId + "88" + nOrderNo.ToString().PadLeft(6, '0');
            }

            string CardNoMin = companyId + "88000000";//按公司代码计算卡流水号
            string CardNoMax = companyId + "88999999"; //最大卡号
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam("CardNoMin", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMin);
            sqlparams[1] = ObjSql.MakeParam("CardNoMax", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMax);

            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select Max(TerminalId) as OrderNo from Psam_Card where TerminalId > @CardNoMin and TerminalId < @CardNoMax", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("OrderNo")))
                    {
                        string strValue = (string)dataReader["OrderNo"];
                        nOrderNo = Convert.ToInt32(strValue.Substring(6, 6)) + 1;
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return companyId + "88" + nOrderNo.ToString().PadLeft(6, '0');
        }

        private string GetPsamCardIdFromDb(string companyId)
        {
            int nOrderNo = 1;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return companyId + "029900" + nOrderNo.ToString().PadLeft(6, '0');
            }
            
            string CardNoMin = companyId + "0299" + "00000000";//按公司代码计算卡流水号
            string CardNoMax = companyId + "0299" + "00999999"; //最大卡号
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam("CardNoMin", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMin);
            sqlparams[1] = ObjSql.MakeParam("CardNoMax", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMax);

            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select Max(PsamId) as OrderNo from Psam_Card where PsamId > @CardNoMin and PsamId < @CardNoMax", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("OrderNo")))
                    {
                        string strValue = (string)dataReader["OrderNo"];
                        nOrderNo = Convert.ToInt32(strValue.Substring(10, 6)) + 1;
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return companyId + "029900" + nOrderNo.ToString().PadLeft(6, '0');
        }

        private string GetCardOrderNoFromDb(string companyId, byte cardType)
        {
            int nOrderNo = 1;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return nOrderNo.ToString().PadLeft(6, '0');
            }
            string strType = cardType.ToString("X2");
            string CardNoMin = companyId + "02" + strType + "00000000";//按公司代码计算卡流水号
            string CardNoMax = companyId + "02" + strType + "00999999"; //最大卡号
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam("CardNoMin", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMin);
            sqlparams[1] = ObjSql.MakeParam("CardNoMax", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMax);

            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select Max(CardNum) as OrderNo from Base_Card where CardNum > @CardNoMin and CardNum < @CardNoMax", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("OrderNo")))
                    {
                        string strValue = (string)dataReader["OrderNo"];
                        nOrderNo = Convert.ToInt32(strValue.Substring(10, 6)) + 1;
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return nOrderNo.ToString().PadLeft(6, '0');
        }

        private void CompanyId_Validated(object sender, EventArgs e)
        {
            m_CompanyID = CompanyId.Text.PadLeft(4, '0');

            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(m_CompanyID, nCardType);//从数据库读出
            UserCardId.Text = m_CompanyID + UserCardInfoParam.CardGroup.ToString("X2") + nCardType.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            m_CardInfoPar.SetCardId(m_CompanyID);

            TermialID.Text = GetTerminalIdFromDb(m_CompanyID);
            PsamId.Text = GetPsamCardIdFromDb(m_CompanyID);
            m_IccCardInfoPar.PSAMCardID = PsamId.Text;
            m_IccCardInfoPar.TermialID = TermialID.Text;
        }

        private void cmbClientName_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (cmbClientName.SelectedIndex >= 0 && cmbClientName.SelectedIndex < m_ListClientInfo.Count)
                m_IccCardInfoPar.ClientID = m_ListClientInfo[cmbClientName.SelectedIndex].ClientId;
        }

        private void cmbCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_CardInfoPar.UserCardType = GetCardType(cmbCardType.SelectedIndex);

            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(m_CardInfoPar.CompanyID, nCardType);//从数据库读出
            UserCardId.Text = m_CardInfoPar.CompanyID + UserCardInfoParam.CardGroup.ToString("X2") + nCardType.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            m_CardInfoPar.SetCardId(m_CardInfoPar.CompanyID);
        }

        private void CardIdRefresh_Click(object sender, EventArgs e)
        {
            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(m_CardInfoPar.CompanyID, nCardType);//从数据库读出
            UserCardId.Text = m_CardInfoPar.CompanyID + UserCardInfoParam.CardGroup.ToString("X2") + nCardType.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            m_CardInfoPar.SetCardId(m_CardInfoPar.CompanyID);
        }

        private void btnPsamRefresh_Click(object sender, EventArgs e)
        {
            TermialID.Text = GetTerminalIdFromDb(m_CompanyID);
            PsamId.Text = GetPsamCardIdFromDb(m_CompanyID);
            m_IccCardInfoPar.PSAMCardID = PsamId.Text;
            m_IccCardInfoPar.TermialID = TermialID.Text;
        }

        private void cmbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSel = cmbFactory.SelectedIndex;
            if (nSel == 0)
            {
                m_DevControl = new ApduController(ApduDomain.DaHua);
            }
            else if (nSel == 1)
            {
                m_DevControl = new ApduController(ApduDomain.LoH_at_MT);
            }
            else
            {
                m_DevControl = null;
            }
        }

        protected override void WriteMsg(int nErrColor, string strMsg)
        {
            string strTextOut = "";
            if (nErrColor != 0)
            {
                int nTextLen = OutputText.TextLength;
                strTextOut = strMsg + "\n";
                OutputText.AppendText(strTextOut);
                OutputText.Select(nTextLen,strMsg.Length+1);
                OutputText.SelectionColor = System.Drawing.Color.FromArgb(nErrColor);
            }
            else
            {
                int nTextLen = OutputText.TextLength;
                strTextOut = strMsg + "\n";
                OutputText.AppendText(strTextOut);
                OutputText.Select(nTextLen, strMsg.Length + 1);
                OutputText.SelectionColor = Color.Black;
            }
            OutputText.Refresh();
            OutputText.ScrollToCaret();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (m_DevControl == null)
                return;
            if (m_DevControl.IsDeviceOpen())
                return;
            if (!m_DevControl.Open_Device())
            {
                WriteMsg(Color.Red.ToArgb(), "建立连接失败");
                btnDisconnect.Enabled = false;
            }
            else
            {
                if (cmbFactory.SelectedIndex != 0)
                    WriteMsg(0, "建立连接成功，当前连接龙寰卡");
                else
                    WriteMsg(0, "建立连接成功，当前连接达华卡");
                cmbFactory.Enabled = false;
                btnDisconnect.Enabled = true;
            }
        }

        private void OnMessageOutput(MsgOutEvent args)
        {
            WriteMsg(args.ErrColor, args.Message);
        }
                
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (m_DevControl == null)
                return;
            if (!m_DevControl.IsDeviceOpen())
                return;
            m_DevControl.Close_Device();
            WriteMsg(0, "断开连接成功");
            btnDisconnect.Enabled = false;
            cmbFactory.Enabled = true;
        }
       
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        private void btnUserCard_Click(object sender, EventArgs e)
        {
            if (!OpenUserCard(cmbFactory.SelectedIndex))
                return;
            m_UserCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
            this.Cursor = Cursors.WaitCursor;
            if (InitCard())
            {
                MadeCard();
            }
            CloseUserCard();
            this.Cursor = Cursors.Default;
        }

        private void btnReUserCard_Click(object sender, EventArgs e)
        {
            if (!OpenUserCard(cmbFactory.SelectedIndex))
                return;
            m_UserCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
            this.Cursor = Cursors.WaitCursor;
            if (ResetCard())
            {
                MadeCard();
            }
            CloseUserCard();
            this.Cursor = Cursors.Default;
        }




        private void btnPsamCard_Click(object sender, EventArgs e)
        {
            if (!OpenIccCard(cmbFactory.SelectedIndex))
                return;
            m_IccCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
            this.Cursor = Cursors.WaitCursor;
            if (InitIccCard())
            {
                MadeIccCard();
            }
            CloseIccCard();
            this.Cursor = Cursors.Default;
        }

        private void btnRePsamCard_Click(object sender, EventArgs e)
        {
            if (!OpenIccCard(cmbFactory.SelectedIndex))
                return;
            m_IccCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
            this.Cursor = Cursors.WaitCursor;
            if (ResetIccCard())
            {
                MadeIccCard();
            }
            CloseIccCard();
            this.Cursor = Cursors.Default;
        }

        private void OneKeyMadeCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            m_DevControl.Close_Device();
        }

    }
}
