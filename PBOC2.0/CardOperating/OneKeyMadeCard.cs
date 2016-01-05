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
    public partial class OneKeyMadeCard : Form, IPlugin
    {
        private ApduController m_DevControl = null;

        private ISamCardControl m_IccCardCtrl = null;
        private IUserCardControl m_UserCardCtrl = null;
        private string m_CompanyID = "0001";

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        private UserCardInfoParam m_CardInfoPar = new UserCardInfoParam();
        private IccCardInfoParam m_IccCardInfoPar = new IccCardInfoParam();
        private List<ClientInfo> m_ListClientInfo = new List<ClientInfo>();

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

        private void ReadInfoFromDb()
        {
            cmbClientName.Items.Clear();
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select ClientId,ClientName from Base_Client", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ClientInfo info = new ClientInfo();
                        info.ClientId = (int)dataReader["ClientId"];
                        info.strClientName = (string)dataReader["ClientName"];
                        m_ListClientInfo.Add(info);
                        cmbClientName.Items.Add(info.strClientName);
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
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


        private int GetClientIdIndex(int nClientID)
        {
            int nSel = -1;
            int nIndex = 0;
            foreach (ClientInfo info in m_ListClientInfo)
            {
                if (info.ClientId == nClientID)
                {
                    nSel = nIndex;
                    break;
                }
                nIndex++;
            }
            return nSel;
        }

        private void OneKeyMadeCard_Load(object sender, EventArgs e)
        {
            m_DevControl = new ApduController(ApduDomain.DaHua);

            CompanyId.Text = m_CompanyID;
            ReadInfoFromDb();
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
        
        private void WriteMsg(int nErrColor, string strMsg)
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

        /// <summary>
        /// 获取卡片厂商
        /// </summary>
        /// <param name="strHexInfo">Atr</param>
        /// <param name="nDevType">设备类型 0 达华+ 明泰; 1 龙寰+明泰</param>
        /// <param name="nCardType">0 非接cpu;  1 psam卡</param>
        /// <returns></returns>
        private string GetCardDescrib(string strHexInfo, int nDevType, int nCardType)
        {
            string strInfo = "";
            if (nDevType != 0)
            {
                //龙寰
                switch (nCardType)
                {
                    case 0:
                        {
                            if (strHexInfo.Contains("4C4F48434F53"))//非接cpu卡"LOHCOS"
                                strInfo = "龙寰-非接触CPU卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    case 1:
                        {
                            if (strHexInfo.Contains("53554E434F53"))//psam卡"SUNCOS"
                                strInfo = "龙寰-PSAM卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //达华
                switch (nCardType)
                {
                    case 0:
                        {
                            if (strHexInfo.Contains("7A6A"))//非接cpu卡"zj"
                                strInfo = "达华-非接触CPU卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    case 1:
                        {
                            if (strHexInfo.Contains("52434F53"))//psam卡"RCOS"
                                strInfo = "达华-PSAM卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    default:
                        break;
                }
            }
            return strInfo;
        }

        private void OnMessageOutput(MsgOutEvent args)
        {
            WriteMsg(args.ErrColor, args.Message);
        }

        private bool OpenUserCard()
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            if (m_UserCardCtrl != null)
                return true;
            string cardInfo = "";
            bool bRet = m_DevControl.OpenCard(ref cardInfo);
            if (!bRet)
            {
                WriteMsg(Color.Red.ToArgb(), "用户卡打开失败");
                return false;
            }
            WriteMsg(0, "用户卡打开成功");
            string strDescribe = GetCardDescrib(cardInfo, cmbFactory.SelectedIndex, 0);
            if (string.IsNullOrEmpty(strDescribe))
                WriteMsg(0, "卡信息：" + cardInfo);
            else
                WriteMsg(0, strDescribe);

            m_UserCardCtrl = m_DevControl.UserCardConstructor(false, m_DBInfo);
            m_UserCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
            int nResult = m_UserCardCtrl.ReadKeyValueFromSource();
            if (nResult == 1)
                MessageBox.Show("从数据库读取用户卡密钥失败，请检查。");
            else if (nResult == 2)
                MessageBox.Show("从XML文件读取用户卡密钥失败，请检查。");
            m_UserCardCtrl.GetCosVer();
            if (nResult != 0)
                return false;
            else
                return true;
        } 

        private bool CloseUserCard()
        {
            if (m_DevControl == null)
                return false;
            m_DevControl.CloseCard();
            m_UserCardCtrl = null;
            return true;
        }

        private bool OpenIccCard()
        {
            if (!m_DevControl.IsDeviceOpen())
                return false;
            string strCardInfo = "";
            bool bRet = m_DevControl.IccPowerOn(ref strCardInfo);
            if (!bRet)
            {
                WriteMsg(Color.Red.ToArgb(), "SAM卡复位失败");
                return false;
            }
            else
            {
                WriteMsg(0, "SAM卡复位成功");
                string strCardDescribe = GetCardDescrib(strCardInfo, cmbFactory.SelectedIndex, 1);
                if (string.IsNullOrEmpty(strCardDescribe))
                    WriteMsg(0, "复位信息：" + strCardInfo);
                else
                    WriteMsg(0, strCardDescribe);
            }

            m_IccCardCtrl = m_DevControl.SamCardConstructor(m_DBInfo);
            m_IccCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);

            int nResult = m_IccCardCtrl.ReadKeyValueFromSource();
            if (nResult == 1)
                MessageBox.Show("从数据库读取PSAM卡密钥失败，请检查。");
            else if (nResult == 2)
                MessageBox.Show("从XML文件读取PSAM卡密钥失败，请检查。");
            m_IccCardCtrl.GetCosVer();
            if (nResult != 0)
                return false;
            else
                return true;
        }

        private bool CloseIccCard()
        {
            if (!m_DevControl.IsDeviceOpen())
                return false;
            m_DevControl.IccPowerOff();
            WriteMsg(0, "卡片关闭成功");
            m_IccCardCtrl = null;
            return true;
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

        private bool InitCard()
        {
            bool bRet = false;
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return false;
            //动作
            int nResult = m_UserCardCtrl.InitCard(false);
            if (nResult != 0)
            {
                if (nResult == 1)
                    MessageBox.Show("初始化失败，请确认卡商。", "警告", MessageBoxButtons.OK);
                else if (nResult == 2)
                    MessageBox.Show("已存在卡号外部认证失败，请确认当前卡片的初始密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 3)
                    MessageBox.Show("已存在卡号初始化失败，请确认当前卡片的初始密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 4)
                    MessageBox.Show("外部认证失败，请确认制卡使用的初始密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 5)
                    MessageBox.Show("初始化失败，请确认制卡使用的初始密钥。", "警告", MessageBoxButtons.OK);
            }
            else
            {
                bRet = true;
            }
            return bRet;
        }

        private bool ResetCard()
        {
            bool bRet = false;
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return false;
            //动作
            int nResult = m_UserCardCtrl.InitCard(true);
            if (nResult != 0)
            {
                if (nResult == 1)
                    MessageBox.Show("重置失败，请确认卡商。", "警告", MessageBoxButtons.OK);
                else if (nResult == 2)
                    MessageBox.Show("已存在卡号外部认证失败，请确认当前卡片的主控密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 3)
                    MessageBox.Show("已存在卡号初始化失败，请确认当前卡片的主控密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 4)
                    MessageBox.Show("外部认证失败，请确认制卡使用的主控密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 5)
                    MessageBox.Show("初始化失败，请确认制卡使用的主控密钥。", "警告", MessageBoxButtons.OK);
            }
            else
            {
                bRet = true;
            }
            return bRet;
        }

        private void btnUserCard_Click(object sender, EventArgs e)
        {
            if (!OpenUserCard())
                return;
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
            if (!OpenUserCard())
                return;
            this.Cursor = Cursors.WaitCursor;
            if (ResetCard())
            {
                MadeCard();
            }
            CloseUserCard();
            this.Cursor = Cursors.Default;
        }

        private void MadeCard()
        {
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return;
            if (!m_UserCardCtrl.CreateEFInMF())
                return;
            m_UserCardCtrl.CreateKey();            
            byte[] UserCardId = m_CardInfoPar.GetUserCardID();
            if (UserCardId == null)
                return;
            WriteMsg(0, "用户卡号：" + BitConverter.ToString(UserCardId));

            //建立应用目录
            if (!m_UserCardCtrl.CreateADFApp(1))
                return;
            //生成加气数据文件
            if (!m_UserCardCtrl.CreateApplication(UserCardId, m_CardInfoPar.DefaultPwdFlag, m_CardInfoPar.CustomPassword))
                return;
            if (!m_UserCardCtrl.UpdateApplicationFile(m_CardInfoPar, null))
                return;
            //保存至数据库            
            string strSuccess = m_UserCardCtrl.SaveCpuCardInfoToDb(m_CardInfoPar, false) ? "成功" : "失败";
            WriteMsg(0, "卡信息写入数据库，结果：" + strSuccess);
        }

        private bool InitIccCard()
        {
            if (!m_DevControl.IsDeviceOpen() || m_IccCardCtrl == null)
                return false;
            if (!ReMakeIccCard())
                return false;
            //动作            
            if (m_IccCardCtrl.InitIccCard(false) != 0)
            {
                MessageBox.Show("当前SAM卡内主控密钥不匹配，初始化失败。\n请确认卡商然后重置。", "警告", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private bool ResetIccCard()
        {
            if (!m_DevControl.IsDeviceOpen() || m_IccCardCtrl == null)
                return false;
            if (!ReMakeIccCard())
                return false;
            //动作            
            if (m_IccCardCtrl.InitIccCard(true) != 0)
            {
                MessageBox.Show("当前SAM卡内主控密钥不匹配，重置失败。\n请确认卡商然后初始化。", "警告", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

         private bool IsExistPsamId(byte[] psamID, byte[] TermId)
        {            
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return false;
            }
            bool bExist = false;
            string strPsamId = BitConverter.ToString(psamID).Replace("-", "");
            string strTermId = BitConverter.ToString(TermId).Replace("-", "");


            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam("PsamId",SqlDbType.Char,16,ParameterDirection.Input,strPsamId);
            sqlparams[1] = ObjSql.MakeParam("TermId", SqlDbType.VarChar, 12, ParameterDirection.Input, strTermId);

            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Psam_Card where PsamId=@PsamId or TerminalId=@TermId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    bExist = true;
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return bExist;
        }


     private bool ReMakeIccCard()
        {            
            byte[] IccCardId = m_IccCardInfoPar.GetBytePsamId();
            byte[] TermialId = m_IccCardInfoPar.GetByteTermId();
            if (IsExistPsamId(IccCardId,TermialId))
            {
                if (MessageBox.Show("该卡的SAM序列号或终端机编号已存在，是否要重新制作？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;
            }
            return true;
        }

        private void btnPsamCard_Click(object sender, EventArgs e)
        {
            if (!OpenIccCard())
                return;
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
            if (!OpenIccCard())
                return;
            this.Cursor = Cursors.WaitCursor;
            if (ResetIccCard())
            {
                MadeIccCard();
            }
            CloseIccCard();
            this.Cursor = Cursors.Default;
        }

        private void MadeIccCard()
        {               
            byte[] IccCardId = m_IccCardInfoPar.GetBytePsamId();
            byte[] TermialId = m_IccCardInfoPar.GetByteTermId();          
            WriteMsg(0, "SAM卡号：" + BitConverter.ToString(IccCardId));
            WriteMsg(0, "终端机编号：" + BitConverter.ToString(TermialId));
            if (!m_IccCardCtrl.CreateIccInfo(IccCardId, TermialId))
                return;
            if(!m_IccCardCtrl.WriteApplicationInfo(m_IccCardInfoPar))
                return;
            //安装所有密钥
            if (!m_IccCardCtrl.SetupIccKey())
                return;
             if (!m_IccCardCtrl.SetupMainKey())
                 return;
            //保存至数据库
            string strSuccess = m_IccCardCtrl.SavePsamCardInfoToDb(m_IccCardInfoPar) ? "成功" : "失败";
            WriteMsg(0, "卡信息写入数据库，结果：" + strSuccess);
        }

        private void OneKeyMadeCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            m_DevControl.Close_Device();
        }

    }
}
