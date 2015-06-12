using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;

namespace CardOperating
{
    public partial class CardApplicationTest : Form
    {
        private const Char Backspace = (Char)8;
        public event MessageOutput TextOutput = null;
        private int m_hDevHandler = 0 ;//读卡器句柄
        private IccCardControl m_IccCardCtrl = null;
        private UserCardControl m_UserCardCtrl = null;

        private ICC_Status m_curIccStatus = ICC_Status.ICC_PowerOff;

        private readonly byte[] m_FixedTermialId = new byte[] { 0x20, 0x10, 0x01, 0x01, 0x00, 0x01 };  //固定的终端机设备编号

        private static byte[] m_TermialId = new byte[6];      //终端机设备编号
        private static byte[] m_ASN = new byte[] { 0x06, 0x71, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01 };//用户卡卡号
        private static string m_strPIN = "999999";//由用户输入
                
        private bool m_bGray = false;   //卡已灰，不能扣款解锁        

        private int m_nBusinessSn;  //脱机交易序号
        private int m_nTermialSn;  //终端交易序号 

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        public CardApplicationTest()
        {
            InitializeComponent();
            textPIN.Text = m_strPIN;
        }

        public void SetDbInfo(SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
        }

        public void SetDeviceHandler(int hDevHandler)
        {
            m_hDevHandler = hDevHandler;
        }

        private bool OpenIccCard()
        {
            if (m_hDevHandler <= 0)
                return false;
            m_IccCardCtrl = new IccCardControl(m_hDevHandler,m_DBInfo);
            m_IccCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
 
            byte[] sInfo = new byte[64];
            byte[] sInfolen = new byte[4];
            uint infoLen = 0;
            short nRetValue = 0;
            if (m_curIccStatus == ICC_Status.ICC_PowerOn)
            {
                nRetValue = DllExportMT.ICC_Reset(m_hDevHandler, 0x00, sInfo, sInfolen);
            }
            else
            {
                nRetValue = DllExportMT.ICC_PowerOn(m_hDevHandler, 0x00, sInfo, sInfolen);
            }

            if (nRetValue != 0)
            {
                OnMessageOutput(new MsgOutEvent(nRetValue, "接触式卡复位失败"));
                m_curIccStatus = ICC_Status.ICC_PowerOff;
                return false;
            }
            else
            {
                if (m_curIccStatus == ICC_Status.ICC_PowerOff)
                    OnMessageOutput(new MsgOutEvent(0, "接触式卡上电复位成功"));
                else
                    OnMessageOutput(new MsgOutEvent(0, "接触式卡重新复位成功"));
                m_curIccStatus = ICC_Status.ICC_PowerOn;
                infoLen = BitConverter.ToUInt32(sInfolen, 0);
                byte[] infoAsc = new byte[infoLen * 2];
                DllExportMT.hex_asc(sInfo, infoAsc, infoLen);
                OnMessageOutput(new MsgOutEvent(0, "复位信息：" + Encoding.ASCII.GetString(infoAsc)));
            }
            byte[] TermialId = m_IccCardCtrl.GetTerminalId();
            if(TermialId != null)
                Buffer.BlockCopy(TermialId, 0, m_TermialId, 0, 6);
            return true;
        }

        private bool CloseIccCard()
        {
            if (m_hDevHandler <= 0)
                return false;
            short nRetValue = DllExportMT.ICC_PowerOff(m_hDevHandler, 0x00);
            if (nRetValue != 0)
                OnMessageOutput(new MsgOutEvent(nRetValue, "关闭卡片失败"));   
            else
                OnMessageOutput(new MsgOutEvent(0, "关闭卡片成功"));
            m_curIccStatus = ICC_Status.ICC_PowerOff;
            m_IccCardCtrl = null;
            return true;
        }

        private bool OpenUserCard()
        {
            if (m_hDevHandler <= 0)
                return false;
            m_UserCardCtrl = new UserCardControl(m_hDevHandler, m_DBInfo);
            m_UserCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
            if (!m_UserCardCtrl.ReadKeyValueFormDb())
            {
                MessageBox.Show("未读到默认密钥，请检查数据库是否正常。");
                return false;
            }
  
            byte[] cardUid = new byte[4];
            byte[] cardInfo = new byte[64];
            byte[] cardInfolen = new byte[4];
            uint infoLen = 0;
            short nRetValue = DllExportMT.OpenCard(m_hDevHandler, 1, cardUid, cardInfo, cardInfolen);
            if (nRetValue != 0)
            {
                OnMessageOutput(new MsgOutEvent(nRetValue, "非接触式卡打开失败"));
                return false;
            }
            else
            {
                OnMessageOutput(new MsgOutEvent(0, "非接触式卡打开成功"));                
                byte[] cardUidAsc = new byte[8];
                DllExportMT.hex_asc(cardUid, cardUidAsc, 4);
                OnMessageOutput( new MsgOutEvent(0, "Uid：" + Encoding.ASCII.GetString(cardUidAsc)) );
                infoLen = BitConverter.ToUInt32(cardInfolen, 0);
                byte[] cardInfoAsc = new byte[infoLen * 2];
                DllExportMT.hex_asc(cardInfo, cardInfoAsc, infoLen);
                OnMessageOutput( new MsgOutEvent(0, "卡信息：" + Encoding.ASCII.GetString(cardInfoAsc)) ) ;
            }
            return true;
        }

        private bool CloseUserCard()
        {
            if (m_hDevHandler <= 0)
                return false;
            short nRetValue = DllExportMT.CloseCard(m_hDevHandler);
            if (nRetValue != 0)
                OnMessageOutput( new MsgOutEvent(nRetValue, "关闭卡片失败") ) ;                
            else
                OnMessageOutput(new MsgOutEvent(0, "关闭卡片成功"));
            m_UserCardCtrl = null;
            return true;
        }

        private void OnMessageOutput(MsgOutEvent args)
        {
            if (this.TextOutput != null)
                this.TextOutput(args);
        }

        //圈存
        private void btnCardLoad_Click(object sender, EventArgs e)
        {
            if (m_hDevHandler <= 0)
                return;
            if (!OpenUserCard() || !ReadUserCardAsn())
                return;
            decimal MoneyLoad = decimal.Parse(textMoney.Text, System.Globalization.NumberStyles.Number);
            double dbMoneyLoad = decimal.ToDouble(MoneyLoad);
            //圈存
            string strInfo = string.Format("对卡号{0}圈存{1}元", BitConverter.ToString(m_ASN), dbMoneyLoad.ToString("F2"));
            OnMessageOutput( new MsgOutEvent(0, strInfo) );
            if(m_UserCardCtrl.VerifyUserPin(m_strPIN) == 1)
            {
                byte[] TerminalId = new byte[6];
                if (APDUBase.ByteDataEquals(TerminalId, m_TermialId))//未读到终端机编号，使用固定编号
                    Buffer.BlockCopy(m_FixedTermialId, 0, TerminalId, 0, 6);
                else
                    Buffer.BlockCopy(m_TermialId, 0, TerminalId, 0, 6);
                m_UserCardCtrl.UserCardLoad(m_ASN, TerminalId, (int)(dbMoneyLoad * 100.0),false);
            }
            CloseUserCard();
            CloseIccCard();
        }

        private void btnBalance_Click(object sender, EventArgs e)
        {
            if (m_hDevHandler <= 0)
                return;
            if (!OpenUserCard() || !ReadUserCardAsn())
                return;
            string strInfo = string.Format("读取卡号{0}的余额，并检查是否灰锁。", BitConverter.ToString(m_ASN));
            OnMessageOutput(new MsgOutEvent(0, strInfo));
            if (m_UserCardCtrl.VerifyUserPin(m_strPIN) == 1)
            {
                m_bGray = false;                
                //未灰锁时终端机编号输出为0
                int nCardStatus = 0;
                if (m_UserCardCtrl.UserCardGray(ref nCardStatus, m_TermialId))
                {
                    if (nCardStatus == 2)
                    {
                        //当前TAC未读，需要清空后重读
                        m_UserCardCtrl.ClearTACUF();
                        nCardStatus = 0;
                        m_UserCardCtrl.UserCardGray(ref nCardStatus, m_TermialId);
                        m_bGray = nCardStatus == 1 ? true : false;
                    }
                    else
                    {
                        m_bGray = nCardStatus == 1 ? true : false;
                    }
                    GrayFlag.CheckState = m_bGray ? CheckState.Checked : CheckState.Unchecked;
                    GrayFlag.Checked = m_bGray;
                }
                else
                {
                    GrayFlag.CheckState = CheckState.Indeterminate;
                    GrayFlag.Checked = false;
                }

                double dbBalance = 0.0f;
                if (m_UserCardCtrl.UserCardBalance(ref dbBalance))
                    textBalance.Text = dbBalance.ToString("F2");
                else
                    textBalance.Text = "0.00";
            }            
            CloseUserCard();
        }

        //强制解灰
        private void btnUnlockGrayCard_Click(object sender, EventArgs e)
        {
            if (m_hDevHandler <= 0)
                return;
            //未灰状态不可强制解灰
            if (!m_bGray || !OpenUserCard() || !ReadUserCardAsn())
                return;
            if (m_UserCardCtrl.VerifyUserPin(m_strPIN) == 1)
            {
                const float BusinessMoney = 0.0F;//强制联机解灰 0 扣款
                byte[] TerminalId = new byte[6];
                if (APDUBase.ByteDataEquals(TerminalId, m_TermialId))//未读到终端机编号，使用固定编号
                    Buffer.BlockCopy(m_FixedTermialId, 0, TerminalId, 0, 6);
                else
                    Buffer.BlockCopy(m_TermialId, 0, TerminalId, 0, 6);
                m_UserCardCtrl.UnLockGrayCard(m_ASN, TerminalId, (int)(BusinessMoney * 100.0), false);
                m_bGray = false;
            }
            CloseUserCard();
            CloseIccCard();
        }

        private void btnLockCard_Click(object sender, EventArgs e)
        {
            if (m_bGray)
                return;
            if (!OpenUserCard() || !OpenIccCard())
                return;
            if (!ReadUserCardAsn() || !m_IccCardCtrl.SelectPsamApp())
                return;
            if (m_UserCardCtrl.VerifyUserPin(m_strPIN) != 1)
                return;            
            //灰锁初始化
            byte[] outData = new byte[15];
            m_UserCardCtrl.InitForGray(m_TermialId,outData);
            byte[] byteBalance = new byte[4];
            Buffer.BlockCopy(outData, 0, byteBalance, 0, 4);//ET余额
            byte[] OfflineSn = new byte[2];//ET脱机交易序号
            Buffer.BlockCopy(outData, 4, OfflineSn, 0, 2);
            byte keyVer = outData[9];
            byte keyFlag = outData[10];
            byte[] rand = new byte[4];
            Buffer.BlockCopy(outData, 11, rand, 0, 4);
            //灰锁
            const byte BusinessType = 0x91;//交易类型
            byte[] GrayLockData = new byte[19]; //从PSAM卡获得顺序为终端交易序号，终端随机数，BCD时间，MAC1
            if (!m_IccCardCtrl.InitSamGrayLock(m_TermialId, rand, OfflineSn, byteBalance, BusinessType, m_ASN, GrayLockData))
                return;
            byte[] GTAC = new byte[4];
            byte[] MAC2 =new byte[4];
            if (!m_UserCardCtrl.GrayLock(GrayLockData, GTAC, MAC2))
                return;
            if (!m_IccCardCtrl.VerifyMAC2(MAC2))//验证MAC2
                return;
            m_nBusinessSn = (int)((OfflineSn[0] << 8) | OfflineSn[1]);
            m_nTermialSn = (int)((GrayLockData[0] << 24) | (GrayLockData[1] << 16) | (GrayLockData[2] << 8) | GrayLockData[3]);
        }

        private byte[] GetDebitforUnlockData()
        {
            byte[] DebitData = new byte[27];
             //计算GMAC
            const byte BusinessType = 0x93;//交易类型: 解0扣
            decimal Amount = decimal.Parse(textPurchase.Text, System.Globalization.NumberStyles.Number);
            double dbAmount = decimal.ToDouble(Amount);
            int nMoneyAmount = (int)(dbAmount * 100.0); ////气票消费金额
            byte[] GMAC = new byte[4];
            if (!m_IccCardCtrl.CalcGMAC(BusinessType, m_ASN, m_nBusinessSn, nMoneyAmount, GMAC))
                return null;
            byte[] byteMoney = BitConverter.GetBytes(nMoneyAmount); //气票消费金额
            DebitData[0] = byteMoney[3];
            DebitData[1] = byteMoney[2];
            DebitData[2] = byteMoney[1];
            DebitData[3] = byteMoney[0];
            m_nBusinessSn += 1;
            DebitData[4] = (byte)((m_nBusinessSn >> 8) & 0xFF);
            DebitData[5] = (byte)(m_nBusinessSn & 0xFF);
            Buffer.BlockCopy(m_TermialId, 0, DebitData, 6, 6);
            m_nTermialSn += 1;
            DebitData[12] = (byte)((m_nTermialSn >> 24) & 0xFF);//终端交易序号
            DebitData[13] = (byte)((m_nTermialSn >> 16) & 0xFF);
            DebitData[14] = (byte)((m_nTermialSn >> 8) & 0xFF);
            DebitData[15] = (byte)(m_nTermialSn & 0xFF);
            byte[] SysTime = CardControlBase.GetBCDTime();
            Buffer.BlockCopy(SysTime, 0, DebitData, 16, 7);//BCD时间
            Buffer.BlockCopy(GMAC, 0, DebitData, 23, 4);//GMAC
            return DebitData;
        }

        private void btnUnlockCard_Click(object sender, EventArgs e)
        {
            if (m_bGray || m_IccCardCtrl == null || m_UserCardCtrl == null)
                return;
            byte[] UnlockData = GetDebitforUnlockData();
            if (UnlockData == null)
                return;
            //解扣debit for unlock
            if (!m_UserCardCtrl.DebitForUnlock(UnlockData))
                return;            
            //清TACUF （即 读灰锁状态，但其中P1 == 0x01）
            m_UserCardCtrl.ClearTACUF();            
            CloseUserCard();
            CloseIccCard();
        }

        private bool ReadUserCardAsn()
        {
            if (!m_UserCardCtrl.SelectCardApp())
                return false;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(true);
            if (ASN == null)
                return false;
            Buffer.BlockCopy(ASN, 0, m_ASN, 0, 8);
            return true;
        }

        private void textPIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textPIN_Validated(object sender, EventArgs e)
        {
            m_strPIN = textPIN.Text;
        }

        private void btnReadRecord_Click(object sender, EventArgs e)
        {
            if (m_hDevHandler <= 0)
                return;
            if (!OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp())
                return;
            if (m_UserCardCtrl.VerifyUserPin(m_strPIN) == 1)
            {                 
                List<CardRecord> lstRecord = m_UserCardCtrl.ReadRecord();
                if (lstRecord.Count > 0)
                {
                    FillListView(lstRecord);
                }
            }
            CloseUserCard();            
        }

        public void FillListView(List<CardRecord> lstRecord)
        {
            foreach (CardRecord record in lstRecord)
            {
                ListViewItem item = new ListViewItem();
                item.Text = record.BusinessSn.ToString();
                item.SubItems.Add(record.OverdraftMoney.ToString("F2"));
                item.SubItems.Add(record.Amount.ToString("F2"));
                item.SubItems.Add(record.BusinessType.ToString());
                item.SubItems.Add(record.TerminalID);
                item.SubItems.Add(record.BusinessTime);
                RecordInCard.Items.Add(item);
            }
        }
    }
}