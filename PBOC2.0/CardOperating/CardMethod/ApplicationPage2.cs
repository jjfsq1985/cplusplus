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


namespace CardOperating
{
    public partial class CardApplicationTest
    {
        private static string m_strLyPin = "999999"; //积分应用PIN

        private bool m_bLyGray = false;   //卡已灰，不能扣积分解锁  

        private int m_nLyBusinessSn;  //脱机交易序号
        private int m_nLyTerminalSn;  //终端交易序号 

        public void Page2Init()
        {
            textLyPin.Text = m_strLyPin;
        }

        private void textLyPin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textLyPin_Validated(object sender, EventArgs e)
        {
            m_strLyPin = textLyPin.Text;
        }

        private void btnLoadLy_Click(object sender, EventArgs e)
        {
            int LoadLoyalty = 0;
            int.TryParse(textLyLoad.Text, System.Globalization.NumberStyles.AllowThousands, null, out LoadLoyalty);
            if (LoadLoyalty < 1)
                return;
            if (m_nAppIndex != 2 || !OpenUserCard() || !ReadUserCardAsn(2))
                return;
            //圈存
            string strInfo = string.Format("对卡号{0}圈存{1}积分", BitConverter.ToString(m_ASN), LoadLoyalty.ToString());
            OnMessageOutput(new MsgOutEvent(0, strInfo));
            if (m_UserCardCtrl.VerifyUserPin(m_strLyPin) == 1)
            {
                byte[] TerminalId = new byte[6];
                if (PublicFunc.ByteDataEquals(TerminalId, m_TermialId))//未读到终端机编号，使用固定编号
                    Buffer.BlockCopy(m_FixedTermialId, 0, TerminalId, 0, 6);
                else
                    Buffer.BlockCopy(m_TermialId, 0, TerminalId, 0, 6);
                m_UserCardCtrl.LoyaltyLoad(m_ASN, TerminalId, LoadLoyalty, false);
            }
            CloseUserCard();
        }

        private void btnReadLy_Click(object sender, EventArgs e)
        {
            if (m_nAppIndex != 2 || !OpenUserCard() || !ReadUserCardAsn(2))
                return;
            string strInfo = string.Format("读取卡号{0}的积分，并检查是否灰锁。", BitConverter.ToString(m_ASN));
            OnMessageOutput(new MsgOutEvent(0, strInfo));
            if (m_UserCardCtrl.VerifyUserPin(m_strLyPin) == 1)
            {
                m_bLyGray = false;
                //未灰锁时终端机编号输出为0
                int nCardStatus = 0;
                if (m_UserCardCtrl.UserCardGray(ref nCardStatus, m_TermialId, m_GTAC))
                {
                    if (nCardStatus == 2)
                    {
                        //当前TAC未读，需要清空后重读
                        m_UserCardCtrl.ClearTACUF();
                        nCardStatus = 0;
                        m_UserCardCtrl.UserCardGray(ref nCardStatus, m_TermialId, m_GTAC);
                        m_bLyGray = nCardStatus == 1 ? true : false;
                    }
                    else
                    {
                        m_bLyGray = nCardStatus == 1 ? true : false;
                    }
                    LyGrayFlag.CheckState = m_bLyGray ? CheckState.Checked : CheckState.Unchecked;
                    LyGrayFlag.Checked = m_bLyGray;
                }
                else
                {
                    LyGrayFlag.CheckState = CheckState.Indeterminate;
                    LyGrayFlag.Checked = false;
                }

                int nBalance = 0;
                if (m_UserCardCtrl.UserCardBalance(ref nBalance, BalanceType.Balance_EP))
                    textLoyalty.Text = nBalance.ToString();
                else
                    textLoyalty.Text = "0.00";
            }
            CloseUserCard();
        }

        private void btnUnGrayLy_Click(object sender, EventArgs e)
        {
            //未灰状态不可强制解灰
            if (m_nAppIndex != 2 || !m_bLyGray)
                return;
            if (!OpenUserCard() || !ReadUserCardAsn(2))
                return;
            if (m_UserCardCtrl.VerifyUserPin(m_strLyPin) == 1)
            {
                const float BusinessLoyalty = 0.0F;//强制联机解灰 0 扣款
                byte[] TerminalId = new byte[6];
                if (PublicFunc.ByteDataEquals(TerminalId, m_TermialId))//未读到终端机编号，使用固定编号
                    Buffer.BlockCopy(m_FixedTermialId, 0, TerminalId, 0, 6);
                else
                    Buffer.BlockCopy(m_TermialId, 0, TerminalId, 0, 6);
                if (m_UserCardCtrl.UnLockGrayCard(m_ASN, TerminalId, (int)(BusinessLoyalty * 100.0), false,2))
                    m_bLyGray = false;
            }
            CloseUserCard();
        }

        private void btnReadLyRecord_Click(object sender, EventArgs e)
        {
            if (m_nAppIndex != 2 || !OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp(2))
                return;
            if (m_UserCardCtrl.VerifyUserPin(m_strLyPin) == 1)
            {
                List<CardRecord> lstRecord = m_UserCardCtrl.ReadRecord();
                if (lstRecord.Count > 0)
                {
                    FillLyListView(lstRecord);
                }
            }
            CloseUserCard();
        }

        private void FillLyListView(List<CardRecord> lstRecord)
        {
            RecordInCard.Items.Clear();
            foreach (CardRecord record in lstRecord)
            {
                ListViewItem item = new ListViewItem();
                item.Text = record.BusinessSn.ToString();
                item.SubItems.Add(record.Amount.ToString("F0"));
                item.SubItems.Add(RecordType(record.BusinessType));
                item.SubItems.Add(record.TerminalID);
                item.SubItems.Add(record.BusinessTime);
                LyRecordInCard.Items.Add(item);
            }
        }

        private void textRate_MouseHover(object sender, EventArgs e)
        {
            ToolTip RateTip = new ToolTip();
            RateTip.SetToolTip(textRate, "例如：10 表示10个积分兑换金额1元");
        }

        private bool LoyaltyPurchase(int nLyAmount)
        {
            if(!ReadUserCardAsn(2))
                return false;
            if (m_UserCardCtrl.VerifyUserPin(m_strLyPin) != 1)
                return false;
            bool bSamSlot = LySamSlot.Checked;//使用SAM卡槽
            if (!OpenSAMCard(bSamSlot, 2))
                return false;
            //积分消费初始化
            byte[] outData = new byte[15];
            m_UserCardCtrl.InitForPurchase(m_TermialId, nLyAmount, outData);

            byte[] bytebalance = new byte[4];
            bytebalance[0] = outData[3];
            bytebalance[1] = outData[2];
            bytebalance[2] = outData[1];
            bytebalance[3] = outData[0];
            int nBalanceLy = BitConverter.ToInt32(bytebalance, 0);//余额
            if (nLyAmount > nBalanceLy)
            {
                MessageBox.Show("积分不足");
                return false;
            }

            byte[] byteAmount = new byte[4];
            byte[] value = BitConverter.GetBytes(nLyAmount);
            byteAmount[0] = value[3];
            byteAmount[1] = value[2];
            byteAmount[2] = value[1];
            byteAmount[3] = value[0];
            byte[] OfflineSn = new byte[2];//ET脱机交易序号
            Buffer.BlockCopy(outData, 4, OfflineSn, 0, 2);
            byte keyVer = outData[9];
            byte keyFlag = outData[10];
            byte[] rand = new byte[4];
            Buffer.BlockCopy(outData, 11, rand, 0, 4);
            //积分消费
            const byte BusinessType = 0x06;//交易类型
            byte[] PurchaseData = new byte[15]; //从PSAM卡获得顺序为终端交易序号，BCD时间，MAC1

            if (!m_SamCardCtrl.SamAppSelect(bSamSlot))
                return false;
            if (!m_SamCardCtrl.InitSamPurchase(bSamSlot, m_TermialId, rand, OfflineSn, byteAmount, BusinessType, m_ASN, PurchaseData))
                return false;
            byte[] TAC = new byte[4];
            byte[] MAC2 = new byte[4];
            if (!m_UserCardCtrl.LyPurchase(PurchaseData, TAC, MAC2))
                return false;
            if (!m_SamCardCtrl.VerifyMAC2(bSamSlot, MAC2, 2))//验证MAC2
                return false;
            m_nLyBusinessSn = (int)((OfflineSn[0] << 8) | OfflineSn[1]);
            m_nLyTerminalSn = (int)((PurchaseData[0] << 24) | (PurchaseData[1] << 16) | (PurchaseData[2] << 8) | PurchaseData[3]);
            return true;
        }

        private void btnLyPurchase_Click(object sender, EventArgs e)
        {
            int nLyAmount = 0;
            int.TryParse(textLyPurchase.Text, System.Globalization.NumberStyles.AllowThousands, null, out nLyAmount);//消费积分金额
            if (nLyAmount < 1)
                return;
            if (m_bLyGray || m_nAppIndex != 2)
                return;
            if (!OpenUserCard())
                return;
            LoyaltyPurchase(nLyAmount);
            CloseUserCard();
        }

    }
}   

