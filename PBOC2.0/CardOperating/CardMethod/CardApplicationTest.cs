using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;
using ApduParam;
using ApduCtrl;
using ApduInterface;

namespace CardOperating
{
    public partial class CardApplicationTest : Form
    {
        private const Char Backspace = (Char)8;
        public event MessageOutput TextOutput = null;

        private ISamCardControl m_SamCardCtrl = null;
        private IUserCardControl m_UserCardCtrl = null;

        private readonly byte[] m_FixedTermialId = new byte[] { 0x20, 0x15, 0x01, 0x01, 0x00, 0x01 };  //固定的终端机设备编号
        private static byte[] m_TermialId = new byte[6];      //终端机设备编号
        private static byte[] m_ASN = new byte[] { 0x06, 0x71, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01 };//用户卡卡号

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        private ApduController m_DevControl = null;
        private bool m_bContactCard = false;

        private int m_nAppIndex = 1;

        public CardApplicationTest()
        {
            InitializeComponent();

            Page1Init();
            Page2Init();

            tabApp.TabPages.Remove(tabPageApp2);
        }

        public void SetDbInfo(SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
        }

        public void SetDeviceHandler(ApduController ApduCtrlObj,bool bContactCard)
        {
            m_DevControl = ApduCtrlObj;
            m_bContactCard = bContactCard;

            //接触式用户卡只能使用SAM卡槽消费
            if (m_bContactCard)
            {
                SamSlot.Checked = true;
                SamSlot.Enabled = false;

                LySamSlot.Checked = true;
                LySamSlot.Enabled = false;


            }
            else
            {
                SamSlot.Checked = false;
                SamSlot.Enabled = true;

                LySamSlot.Checked = false;
                LySamSlot.Enabled = true;
            }

            ContactCard.Checked = m_bContactCard;
        }

        private bool OpenSAMCard(bool bSamSlot, int nAppIndex)
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            m_SamCardCtrl = m_DevControl.SamCardConstructor(m_DBInfo);
            m_SamCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
            int nResult = m_SamCardCtrl.ReadKeyValueFromSource();
            if (nResult == 1)
            {
                MessageBox.Show("从数据库读取PSAM卡密钥失败，请检查。");
                return false;
            }
            else if(nResult == 2)
            {
                MessageBox.Show("从XML文件读取PSAM卡密钥失败，请检查。");
                return false;
            }
                        
            string strCardInfo = "";
            bool bRet = m_DevControl.SAMPowerOn(bSamSlot, ref strCardInfo);
            if (!bRet)
            {
                OnMessageOutput(new MsgOutEvent(0, "SAM卡复位失败"));                
                return false;
            }
            else
            {
                OnMessageOutput(new MsgOutEvent(0, "SAM卡复位成功"));
                OnMessageOutput(new MsgOutEvent(0, "复位信息：" + strCardInfo));
            }

            byte[] TermialId = m_SamCardCtrl.GetTerminalId(bSamSlot);
            if(TermialId != null)
                Buffer.BlockCopy(TermialId, 0, m_TermialId, 0, 6);
            return true;
        }

        private bool CloseSAMCard(bool bSamSlot)
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            m_DevControl.SAMPowerOff(bSamSlot);
            OnMessageOutput(new MsgOutEvent(0, "关闭卡片成功"));
            m_SamCardCtrl = null;
            return true;
        }

        private bool OpenUserCard(int nAppIndex)
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            m_UserCardCtrl = m_DevControl.UserCardConstructor(m_bContactCard, m_DBInfo);
            m_UserCardCtrl.TextOutput += new MessageOutput(OnMessageOutput);
            int nResult = m_UserCardCtrl.ReadKeyValueFromSource();
            if (nResult == 1)
            {
                MessageBox.Show("从数据库读取用户卡密钥失败，请检查。");
                return false;
            }
            else if (nResult == 2)
            {
                MessageBox.Show("从XML文件读取用户卡密钥失败，请检查。");
                return false;
            }

            string cardInfo = "";
            if (m_bContactCard)
            {
                bool bRet = m_DevControl.OpenContactCard(ref cardInfo);
                if (!bRet)
                {
                    OnMessageOutput(new MsgOutEvent(0, "接触式用户卡打开失败"));                    
                    return false;
                }
            }
            else
            {
                bool bRet = m_DevControl.OpenCard(ref cardInfo);
                if (!bRet)
                {
                    OnMessageOutput(new MsgOutEvent(0, "非接触式卡打开失败"));
                    return false;
                }
            }

            OnMessageOutput(new MsgOutEvent(0, "用户卡打开成功"));
            OnMessageOutput(new MsgOutEvent(0, "卡信息：" + cardInfo));
            return true;
        }

        private bool CloseUserCard()
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            if (m_bContactCard)
                m_DevControl.CloseContactCard();
            else
                m_DevControl.CloseCard();
            OnMessageOutput(new MsgOutEvent(0, "关闭卡片成功"));
            m_UserCardCtrl = null;
            return true;
       }

        private void OnMessageOutput(MsgOutEvent args)
        {
            if (this.TextOutput != null)
                this.TextOutput(args);
        }

        private bool ReadUserCardAsn(int nAppIndex)
        {
            if (!m_UserCardCtrl.SelectCardApp(nAppIndex))
                return false;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(true, ref cardStart, ref cardEnd);
            if (ASN == null)
                return false;
            Buffer.BlockCopy(ASN, 0, m_ASN, 0, 8);
            return true;
        }

        private string RecordType(byte RecordType)
        {
            string strT = RecordType.ToString();
            switch (RecordType)
            {
                case 0x01:
                    strT = "圈存存折";
                    break;
                case 0x02:
                    strT = "圈存钱包";
                    break;
                case 0x03:
                    strT = "圈提存折";
                    break;
                case 0x93:
                    strT = "加气交易";
                    break;
                case 0x95:
                    strT = "联机交易解扣";
                    break;
                case 0xB1:
                    strT = "积分圈存";
                    break;
                case 0xA3:
                    strT = "积分消费";
                    break;
                case 0xA5:
                    strT = "联机积分解扣";
                    break;
            }
            return strT;
        }

        private void ContactCard_CheckedChanged(object sender, EventArgs e)
        {
            m_bContactCard = ContactCard.Checked;
            //接触式用户卡只能使用SAM卡槽消费
            if (m_bContactCard)
            {
                SamSlot.Checked = true;
                SamSlot.Enabled = false;

                LySamSlot.Checked = true;
                LySamSlot.Enabled = false;

            }
            else
            {
                SamSlot.Checked = false;
                SamSlot.Enabled = true;

                LySamSlot.Checked = false;
                LySamSlot.Enabled = true;

            }
        }

        private void tabApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabApp.SelectedIndex == 1)
                m_nAppIndex = 2;
            else
                m_nAppIndex = 1;
        }

    }
}