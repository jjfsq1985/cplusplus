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
    public partial class SinopecCard : BaseMadeCard, IPlugin
    {
        private byte m_ProvCode = 0x10;

        public SinopecCard()
        {
            InitializeComponent();
            cmbCardType.SelectedIndex = 0;
            cmbProvCode.SelectedIndex = 0;
        }

        public MenuType GetMenuType()
        {
            return MenuType.eSinopecCard;
        }

        public string PluginName()
        {
            return "SinopecCard";
        }

        public Guid PluginGuid()
        {
            return new Guid("59CF2101-1747-44e4-B095-B3D37CF26DE8");
        }

        public string PluginMenu()
        {
            return "制中石化卡";
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

        private void SinopecCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            m_DevControl.Close_Device();
        }

        private string GetCardOrderNoFromDb(byte cardType, byte provCode)
        {
            int nOrderNo = 1;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return nOrderNo.ToString().PadLeft(9, '0');
            }
            string CardNoMin = "";
            string CardNoMax = "";
            if (cardType == 0x01 || cardType == 0x02)
            {
                CardNoMin = "011" + provCode.ToString("X2") +"00" +"000000000";//计算卡流水号
                CardNoMax = "021" + provCode.ToString("X2") + "00" + "999999999";
            }
            else if (cardType == 0x04 || cardType == 0x06)
            {
                CardNoMin = "041" + provCode.ToString("X2") + "00" + "000000000";//计算卡流水号
                CardNoMax = "061" + provCode.ToString("X2") + "00" + "999999999";
            }

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
                        nOrderNo = Convert.ToInt32(strValue.Substring(7, 9)) + 1;
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return nOrderNo.ToString().PadLeft(9, '0');
        }

        private void SinopecCard_Load(object sender, EventArgs e)
        {
            m_DevControl = new ApduController(ApduDomain.LoH_at_MT);
                  
            ReadInfoFromDb();

            cmbClientName.Items.Clear();
            foreach (ClientInfo info in m_ListClientInfo)
                cmbClientName.Items.Add(info.strClientName);
            if (cmbClientName.Items.Count > 0)
                cmbClientName.SelectedIndex = GetClientIdIndex(m_IccCardInfoPar.ClientID);

            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(nCardType, m_ProvCode);//从数据库读出
            UserCardId.Text = "0100" + nCardType.ToString("X2") + "1" + m_ProvCode.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            m_CardInfoPar.SetCardId_Sinopec(UserCardId.Text);

            TermialID.Text = GetTerminalIdFromDb(m_ProvCode);
            PsamId.Text = GetPsamCardIdFromDb(m_ProvCode);
            m_IccCardInfoPar.PSAMCardID = PsamId.Text.Substring(PsamId.Text.Length - 16, 16);
            m_IccCardInfoPar.TermialID = TermialID.Text;

            cmbProvCode.SelectedIndexChanged += new System.EventHandler(this.cmbProvCode_SelectedIndexChanged);            
            this.cmbCardType.SelectedIndexChanged += new System.EventHandler(this.cmbCardType_SelectedIndexChanged);
            this.cmbClientName.SelectedIndexChanged += new System.EventHandler(this.cmbClientName_SelectedIndexChanged);
        }

        private void cmbProvCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_ProvCode = GetProvCode(cmbProvCode.SelectedIndex);
            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(nCardType, m_ProvCode);//从数据库读出
            UserCardId.Text = "0100" + nCardType.ToString("X2") + "1" + m_ProvCode.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            m_CardInfoPar.SetCardId_Sinopec(UserCardId.Text);
        }

        private byte GetProvCode(int nSelectIndex)
        {
            byte provCode = 0x10;
            switch (nSelectIndex)
            {
                case 0:
                    provCode = 0x10;//中石化
                    break;
                case 1:
                    provCode = 0x11;//北京
                    break;
                case 2:
                    provCode = 0x12;//天津
                    break;
                case 3:
                    provCode = 0x13;//河北
                    break;
                case 4:
                    provCode = 0x14;//山西
                    break;
                case 5:
                    provCode = 0x31;//上海
                    break;
                case 6:
                    provCode = 0x32;//江苏
                    break;
                case 7:
                    provCode = 0x33;//浙江
                    break;
                case 8:
                    provCode = 0x34;//安徽
                    break;
                case 9:
                    provCode = 0x35;//福建
                    break;
                case 10:
                    provCode = 0x36;//江西
                    break;
                case 11:
                    provCode = 0x37;//山东
                    break;
                case 12:
                    provCode = 0x41;//河南
                    break;
                case 13:
                    provCode = 0x42;//湖北
                    break;
                case 14:
                    provCode = 0x43;//湖南
                    break;
                case 15:
                    provCode = 0x44;//广东
                    break;
                case 16:
                    provCode = 0x45;//广西
                    break;
                case 17:
                    provCode = 0x46;//海南
                    break;
                case 18:
                    provCode = 0x52;//贵州
                    break;
                case 19:
                    provCode = 0x53;//云南
                    break;
                case 20:
                    provCode = 0x90;//深圳
                    break;
            }
            return provCode;
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
            }
            return eType;
        }

        private void cmbCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_CardInfoPar.UserCardType = GetCardType(cmbCardType.SelectedIndex);

            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(nCardType, m_ProvCode);//从数据库读出
            UserCardId.Text = "0100" + nCardType.ToString("X2") + "1" + m_ProvCode.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            m_CardInfoPar.SetCardId_Sinopec(UserCardId.Text);
        }

        private void cmbClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClientName.SelectedIndex >= 0 && cmbClientName.SelectedIndex < m_ListClientInfo.Count)
                m_IccCardInfoPar.ClientID = m_ListClientInfo[cmbClientName.SelectedIndex].ClientId;
        }

        private string GetTerminalIdFromDb(byte provCode)
        {
            int nOrderNo = 1;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return "13" + provCode.ToString("X2") + nOrderNo.ToString().PadLeft(8, '0');
            }

            string CardNoMin = "13" + provCode.ToString("X2") + "00000000";//计算卡流水号
            string CardNoMax = "13" + provCode.ToString("X2") + "99999999"; //最大卡号
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
                        nOrderNo = Convert.ToInt32(strValue.Substring(4, 8)) + 1;
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return "13" + provCode.ToString("X2") + nOrderNo.ToString().PadLeft(8, '0');
        }

        private string GetPsamCardIdFromDb(byte provCode)
        {
            int nOrderNo = 1;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return "0100101" + provCode.ToString("X2") + "00" + nOrderNo.ToString().PadLeft(9, '0');
            }

            string CardNoMin = "0100101" + provCode.ToString("X2") + "00" + "000000000";//计算卡流水号
            string CardNoMax = "0100101" + provCode.ToString("X2") + "00" + "999999999"; //最大卡号
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
                        nOrderNo = Convert.ToInt32(strValue.Substring(11, 9)) + 1;
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return "0100101" + provCode.ToString("X2") + "00" + nOrderNo.ToString().PadLeft(9, '0');
        }

        private void CardIdRefresh_Click(object sender, EventArgs e)
        {
            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(nCardType,m_ProvCode);//从数据库读出
            UserCardId.Text = "0100" + nCardType.ToString("X2") + "1" + m_ProvCode.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            m_CardInfoPar.SetCardId_Sinopec(UserCardId.Text);
        }

        private void btnPsamRefresh_Click(object sender, EventArgs e)
        {
            TermialID.Text = GetTerminalIdFromDb(m_ProvCode);
            PsamId.Text = GetPsamCardIdFromDb(m_ProvCode);
            m_IccCardInfoPar.PSAMCardID = PsamId.Text.Substring(PsamId.Text.Length - 16, 16);
            m_IccCardInfoPar.TermialID = TermialID.Text;
        }

        protected override void WriteMsg(int nErrColor, string strMsg)
        {
            string strTextOut = "";
            if (nErrColor != 0)
            {
                int nTextLen = OutputText.TextLength;
                strTextOut = strMsg + "\n";
                OutputText.AppendText(strTextOut);
                OutputText.Select(nTextLen, strMsg.Length + 1);
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

        private void OnMessageOutput(MsgOutEvent args)
        {
            WriteMsg(args.ErrColor, args.Message);
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
                WriteMsg(0, "建立连接成功");
                btnDisconnect.Enabled = true;
            }
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
        }

        private void btnUserCard_Click(object sender, EventArgs e)
        {
            if (!OpenUserCard(1))
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
            if (!OpenUserCard(1))
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
            if (!OpenIccCard(1))
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
            if (!OpenIccCard(1))
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

    }
}
