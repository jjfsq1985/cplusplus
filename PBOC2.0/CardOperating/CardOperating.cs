using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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
    public partial class CardOperating : Form, IPlugin
    {
        private ApduController m_DevControl = null;

        private ISamCardControl m_IccCardCtrl = null;
        private IUserCardControl m_UserCardCtrl = null;

        private byte[] m_IccCardId = null;
        private byte[] m_UserCardId = null;
        
        private bool m_bShowPanel = false;
        private IccCardInfo m_CardPSAM = new IccCardInfo();
        private UserCardInfo m_CardUser = new UserCardInfo();
        private CardApplicationTest m_CardMethod = new CardApplicationTest();

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        public CardOperating()
        {
            InitializeComponent();
            //CardOperatingCtrlPos();
            m_CardUser.TopLevel = false;
            m_CardUser.Parent = this;
            CardInfoPanel.Controls.Add(m_CardUser);
            m_CardPSAM.TopLevel = false;
            m_CardPSAM.Parent = this;
            CardInfoPanel.Controls.Add(m_CardPSAM);

            m_CardMethod.TextOutput += new MessageOutput(OnMessageOutput);
            m_CardMethod.TopLevel = false;
            m_CardMethod.Parent = this;
            CardInfoPanel.Controls.Add(m_CardMethod);

            cmbDevType.SelectedIndex = 0;
        }

        public MenuType GetMenuType()
        {
            return MenuType.eCardOperating;
        }

        public string PluginName()
        {
            return "CardOperating";
        }

        public Guid PluginGuid()
        {
            return new Guid("1AFEA8C6-5026-4bf7-9C77-573D8C10E4A8");
        }

        public string PluginMenu()
        {
            return "制卡操作";
        }

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            m_CardPSAM.SetDbInfo(m_DBInfo);
            m_CardUser.SetDbInfo(m_DBInfo);
            m_CardMethod.SetDbInfo(m_DBInfo);
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

        private void CardOprQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /*
        private void CardOperatingCtrlPos()
        {
            foreach (Control ctrl in Controls)
            {
                ControlPos pos = new ControlPos();
                pos.x = ctrl.Left;
                pos.dbRateH = (double)ctrl.Right / this.Width;
                pos.y = ctrl.Top;
                pos.dbRateV = (double)ctrl.Bottom / this.Height;
                ctrl.Tag = pos;
            }            
        }        

        private void CardOperating_Resize(object sender, EventArgs e)
        {           
            foreach (Control ctrl in Controls)
            {
                ControlPos pos = (ControlPos)ctrl.Tag;
                pos.x = (int)((this.Width * pos.dbRateH) - ctrl.Width);
                pos.y = (int)((this.Height * pos.dbRateV) - ctrl.Height);
                ctrl.Left = pos.x;
                ctrl.Top = pos.y;
                ctrl.Tag = pos;
            }            
        }
        */

        private void WriteMsg(int nErrColor, string strMsg)
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
                if (strMsg.Contains("应答") && !strMsg.EndsWith("9000"))
                    OutputText.SelectionColor = Color.RoyalBlue;
                else
                    OutputText.SelectionColor = Color.Black;
            }
            OutputText.Refresh();
            OutputText.ScrollToCaret();
        }

        private void btnCleanInfo_Click(object sender, EventArgs e)
        {
            OutputText.Text = "";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (m_DevControl == null)
                return;
            if (m_DevControl.IsDeviceOpen())
                return;
            int nMode = 0;
            if (!m_DevControl.IsDevicePcscMode(ref nMode))
            {
                DialogResult Result = MessageBox.Show("读卡器没有插入或者不是PC/SC模式，是否切换到PC/SC模式？", "连接读卡器", MessageBoxButtons.YesNo);
                if (Result == DialogResult.Yes)
                {
                    m_DevControl.ChangeDevice(3);//open Contactless、Contact and sam Reader
                    return;
                }
            }
            else
            {
                if (nMode == 1)
                {
                    DialogResult Result = MessageBox.Show("接触式读卡器未启用，是否启用以使用接触式卡片？", "提示", MessageBoxButtons.YesNo);
                    if (Result == DialogResult.Yes)
                    {
                        m_DevControl.ChangeDevice(3);//open Contactless、Contact and sam Reader
                        return;
                    }                    
                }
            }
            if(!m_DevControl.Open_Device())
            {
                WriteMsg(Color.Red.ToArgb(), "建立连接失败");
                btnDisconnect.Enabled = false;
            }
            else
            {                
                if(cmbDevType.SelectedIndex != 0)
                    WriteMsg(0, "建立连接成功，当前连接龙寰卡");
                else
                    WriteMsg(0, "建立连接成功，当前连接达华卡");
                cmbDevType.Enabled = false;
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
            cmbDevType.Enabled = true;
        }

        private void btnOpenCard_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            string cardInfo = "";
            if (ContactCard.Checked)
            {
                bool bRet = m_DevControl.OpenContactCard(ref cardInfo);                
                if(!bRet)
                {
                    WriteMsg(Color.Red.ToArgb(), "接触式用户卡打开失败");
                    return;
                }
            }
            else
            {
                bool bRet = m_DevControl.OpenCard(ref cardInfo);
                if (!bRet)
                {
                    WriteMsg(Color.Red.ToArgb(), "非接触式用户卡打开失败");
                    return;
                }
            }

            int nCardType = 0;
            if(ContactCard.Checked)
                nCardType = 1;
            string strCardDescribe = GetCardDescrib(cardInfo, cmbDevType.SelectedIndex, nCardType);
            WriteMsg(0, "用户卡打开成功" );
            if (string.IsNullOrEmpty(strCardDescribe))
                WriteMsg(0, "卡信息：" + cardInfo);
            else
                WriteMsg(0, strCardDescribe);           

            m_UserCardCtrl = m_DevControl.UserCardConstructor(ContactCard.Checked,m_DBInfo);
            m_UserCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
            int nResult = m_UserCardCtrl.ReadKeyValueFromSource();
            if (nResult == 1)
                MessageBox.Show("从数据库读取用户卡密钥失败，请检查。");
            else if (nResult == 2)
                MessageBox.Show("从XML文件读取用户卡密钥失败，请检查。");

            m_UserCardCtrl.GetCosVer();
        }

        private void btnCloseCard_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            m_DevControl.CloseCard();
            WriteMsg(0, "关闭卡片成功");            
            m_UserCardCtrl = null;
        }

        private void OnMessageOutput(MsgOutEvent args)
        {
            WriteMsg(args.ErrColor, args.Message);
        }

        //删除白卡MF中的文件，只保留MF
        private void btnInitCard_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return;
            //动作
            int nResult = m_UserCardCtrl.InitCard(false);
            if (nResult != 0)
            {
                if (nResult == 1)
                    MessageBox.Show("初始化失败，请确认卡商。", "警告", MessageBoxButtons.OK);
                else if(nResult == 2)
                    MessageBox.Show("已存在卡号外部认证失败，请确认当前卡片的初始密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 3)
                    MessageBox.Show("已存在卡号初始化失败，请确认当前卡片的初始密钥。", "警告", MessageBoxButtons.OK);
                else if(nResult == 4)
                    MessageBox.Show("外部认证失败，请确认制卡使用的初始密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 5)
                    MessageBox.Show("初始化失败，请确认制卡使用的初始密钥。", "警告", MessageBoxButtons.OK);     
            }
        }

        private void btnUserCardReset_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return;
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
        }

        //卡信息设置
        private void UserCardSetting_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            if (!m_CardUser.Visible && m_bShowPanel)
            {
                m_CardPSAM.Hide();
                m_CardMethod.Hide();
                m_CardUser.Show();
            }
            else
            {
                m_bShowPanel = !m_bShowPanel;
                if (m_bShowPanel)
                {
                    CardInfoPanel.Visible = true;
                    this.Width += CardInfoPanel.Width;
                    m_CardUser.Show();
                }
                else
                {
                    this.Width -= CardInfoPanel.Width;
                    CardInfoPanel.Visible = false;
                    m_CardUser.Hide();
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return;
            if (!m_UserCardCtrl.CreateEFInMF())
                return;
            m_UserCardCtrl.CreateKey();
        }


        private void btnApplication_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return;
            UserCardInfoParam cardInfo = m_CardUser.GetUserCardParam();
            m_UserCardId = cardInfo.GetUserCardID();
            if (m_UserCardId == null)
            {
                WriteMsg(0, "用户卡号为空，请先进行信息设置。");
                return;
            }
            WriteMsg(0, "用户卡号：" + BitConverter.ToString(m_UserCardId));

            //建立应用目录
            if (!m_UserCardCtrl.CreateADFApp(1))
                return;
            //生成加气数据文件
            if (!m_UserCardCtrl.CreateApplication(m_UserCardId, cardInfo.DefaultPwdFlag, cardInfo.CustomPassword))
                return;
            if (!m_UserCardCtrl.UpdateApplicationFile(cardInfo, null))
                return;

            //保存至数据库            
            string strSuccess = m_UserCardCtrl.SaveCpuCardInfoToDb(cardInfo,false) ? "成功" : "失败";
            WriteMsg(0, "卡信息写入数据库，结果：" + strSuccess);
        }

        /// <summary>
        /// 获取卡片厂商
        /// </summary>
        /// <param name="strHexInfo">Atr</param>
        /// <param name="nDevType">设备类型 0 达华+ 明泰; 1 龙寰+DE620; 2 龙寰+明泰</param>
        /// <param name="nCardType">0 非接cpu; 1 接触cpu; 2 psam卡</param>
        /// <returns></returns>
        private string GetCardDescrib(string strHexInfo, int nDevType, int nCardType)
        {
            string strInfo = "";
            if (nDevType != 0)
            {
                //龙寰
                switch(nCardType)
                {
                    case 0:
                        {
                            if(strHexInfo.Contains("4C4F48434F53"))//非接cpu卡"LOHCOS"
                                strInfo = "龙寰-非接触CPU卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    case 1:
                        {
                            if(strHexInfo.Contains("574454434844415441"))//接触cpu卡"WDTCHDATA"
                               strInfo = "龙寰-接触式CPU卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    case 2:
                        {
                            if(strHexInfo.Contains("53554E434F53"))//psam卡"SUNCOS"
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
                switch(nCardType)
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
                            strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    case 2:
                        {
                            if(strHexInfo.Contains("52434F53"))//psam卡"RCOS"
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

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////PSAM卡制卡///////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnOpenIccCard_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            string strCardInfo = "";
            bool bRet = m_DevControl.IccPowerOn(ref strCardInfo);
            if (!bRet)
            {
                WriteMsg(Color.Red.ToArgb(), "SAM卡复位失败");                
                return;
            }
            else
            {
                WriteMsg(0, "SAM卡复位成功");
                string strCardDescribe = GetCardDescrib(strCardInfo, cmbDevType.SelectedIndex, 2);
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
        }

        private void btnCloseIccCard_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            m_DevControl.IccPowerOff();
            WriteMsg(0, "卡片关闭成功");
            m_IccCardCtrl = null;
        }

        private bool ReMakeIccCard()
        {
            IccCardInfoParam PSAMInfo = m_CardPSAM.GetPSAMCardParam();
            m_IccCardId = PSAMInfo.GetBytePsamId();
            if (m_IccCardId == null)
            {
                WriteMsg(0, "SAM卡号为空，请先进行信息设置。");
                return false;
            }
            byte[] TermialId = PSAMInfo.GetByteTermId();
            if (TermialId == null)
            {
                WriteMsg(0, "终端机编号为空，请先进行信息设置。");
                return false;
            }            
            if (IsExistPsamId(m_IccCardId,TermialId))
            {
                if (MessageBox.Show("该卡的SAM序列号或终端机编号已存在，是否要重新制作？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;
            }
            return true;

        }

        //删除白卡MF中的文件，只保留MF
        private void btnInitIccCard_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen() || m_IccCardCtrl == null)
                return;
            if (!ReMakeIccCard())
                return;
            //动作            
            if (m_IccCardCtrl.InitIccCard(false) != 0)
                MessageBox.Show("当前SAM卡内主控密钥不匹配，初始化失败。\n请确认卡商然后重置。", "警告", MessageBoxButtons.OK);
        }

        private void btnIccCardReset_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen() || m_IccCardCtrl == null)
                return;
            if (!ReMakeIccCard())
                return;
            //动作            
            if (m_IccCardCtrl.InitIccCard(true) != 0)
                MessageBox.Show("当前SAM卡内主控密钥不匹配，重置失败。\n请确认卡商然后初始化。", "警告", MessageBoxButtons.OK);
        }

        private void IccCardSetting_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            if (!m_CardPSAM.Visible && m_bShowPanel)
            {
                m_CardUser.Hide();
                m_CardMethod.Hide();
                m_CardPSAM.Show();
            }
            else
            {
                m_bShowPanel = !m_bShowPanel;
                if (m_bShowPanel)
                {
                    CardInfoPanel.Visible = true;
                    this.Width += CardInfoPanel.Width;
                    m_CardPSAM.Show();
                }
                else
                {
                    this.Width -= CardInfoPanel.Width;
                    CardInfoPanel.Visible = false;
                    m_CardPSAM.Hide();
                }                
            }

        }

        private void btnIccCreate_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen() || m_IccCardCtrl == null)
                return;
            IccCardInfoParam PSAMInfo = m_CardPSAM.GetPSAMCardParam();
            m_IccCardId = PSAMInfo.GetBytePsamId();
            byte[] TermialId = PSAMInfo.GetByteTermId();          
            WriteMsg(0, "SAM卡号：" + BitConverter.ToString(m_IccCardId));
            WriteMsg(0, "终端机编号：" + BitConverter.ToString(TermialId));
            if (!m_IccCardCtrl.CreateIccInfo(m_IccCardId, TermialId))
                return;
            m_IccCardCtrl.WriteApplicationInfo(PSAMInfo);
        }

        private void btnIccAppKey_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen() || m_IccCardCtrl == null)
                return;
            //安装所有密钥
            if (!m_IccCardCtrl.SetupIccKey())
                return;
            m_IccCardCtrl.SetupMainKey();
            //保存至数据库
            string strSuccess = m_IccCardCtrl.SavePsamCardInfoToDb(m_CardPSAM.GetPSAMCardParam()) ? "成功" : "失败";
            WriteMsg(0, "卡信息写入数据库，结果：" + strSuccess);

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

        private void cmbDevType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSel = cmbDevType.SelectedIndex;
            if (nSel == 0)
            {
                m_DevControl = new ApduController(ApduDomain.DaHua);
                ContactCard.Checked = false;
                ContactCard.Enabled = false;
            }
            else if (nSel == 1)
            {
                m_DevControl = new ApduController(ApduDomain.LongHuan);
                ContactCard.Checked = false;
                ContactCard.Enabled = true;
            }
            else if (nSel == 2)
            {
                m_DevControl = new ApduController(ApduDomain.LoH_at_MT);
                ContactCard.Checked = false;
                ContactCard.Enabled = true;
            }
            else
            {
                m_DevControl = null;
                ContactCard.Checked = false;
                ContactCard.Enabled = false;
            }
        }

        private void CardOperating_Load(object sender, EventArgs e)
        {
            m_DevControl = new ApduController(ApduDomain.DaHua);
            cmbDevType.SelectedIndexChanged += new System.EventHandler(this.cmbDevType_SelectedIndexChanged);
        }

        private void btnLoyalty_Click(object sender, EventArgs e)
        {
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return;
            if (!m_UserCardCtrl.HasLyKey())
            {
                WriteMsg(Color.Red.ToArgb(), "没有积分密钥不能建立积分应用。");
                return;
            }
            UserCardInfoParam cardInfo = m_CardUser.GetUserCardParam();
            if (cardInfo.UserCardType != CardType.PersonalCard
                && cardInfo.UserCardType != CardType.CompanySubCard
                && cardInfo.UserCardType != CardType.CompanyMotherCard)
            {
                WriteMsg(0, "只有加气卡才有积分应用。");
                return;
            }
            m_UserCardId = cardInfo.GetUserCardID();
            if (m_UserCardId == null)
            {
                WriteMsg(0, "用户卡号为空，请先进行信息设置。");
                return;
            }
            WriteMsg(0, "用户卡号：" + BitConverter.ToString(m_UserCardId));

            //建立应用目录
            if (!m_UserCardCtrl.CreateADFApp(2))
                return;
            //生成积分数据文件
            if (!m_UserCardCtrl.CreateLoyaltyApp(m_UserCardId, cardInfo.DefaultPwdFlag, cardInfo.CustomPassword))
                return;
            if (!m_UserCardCtrl.UpdateLoyaltyApp(cardInfo, null))
                return;
            WriteMsg(0, "积分应用写入成功");
        }

        private void UserCardAppTest()
        {
            if (!m_DevControl.IsDeviceOpen())
                return;
            if (!m_CardMethod.Visible && m_bShowPanel)
            {
                m_CardPSAM.Hide();
                m_CardUser.Hide();
                m_CardMethod.Show();
                m_CardMethod.SetDeviceHandler(m_DevControl, ContactCard.Checked);
            }
            else
            {
                m_bShowPanel = !m_bShowPanel;
                if (m_bShowPanel)
                {
                    CardInfoPanel.Visible = true;
                    m_CardMethod.Show();
                    m_CardMethod.SetDeviceHandler(m_DevControl, ContactCard.Checked);
                    this.Width += CardInfoPanel.Width;
                }
                else
                {
                    CardInfoPanel.Visible = false;
                    m_CardMethod.Hide();
                    this.Width -= CardInfoPanel.Width;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Alt | Keys.T))
            {
                UserCardAppTest();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}