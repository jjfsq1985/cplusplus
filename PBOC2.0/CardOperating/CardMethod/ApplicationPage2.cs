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
            if (m_nAppIndex != 2 || !OpenUserCard(2) || !ReadUserCardAsn(2))
                return;
            decimal LoyaltyLoad = decimal.Parse(textLyLoad.Text, System.Globalization.NumberStyles.Number);
            double dbLyLoad = decimal.ToDouble(LoyaltyLoad);
            //圈存
            string strInfo = string.Format("对卡号{0}圈存{1}积分", BitConverter.ToString(m_ASN), dbLyLoad.ToString("F2"));
            OnMessageOutput(new MsgOutEvent(0, strInfo));
            if (m_UserCardCtrl.VerifyUserPin(m_strLyPin) == 1)
            {
                byte[] TerminalId = new byte[6];
                if (PublicFunc.ByteDataEquals(TerminalId, m_TermialId))//未读到终端机编号，使用固定编号
                    Buffer.BlockCopy(m_FixedTermialId, 0, TerminalId, 0, 6);
                else
                    Buffer.BlockCopy(m_TermialId, 0, TerminalId, 0, 6);
                m_UserCardCtrl.LoyaltyLoad(m_ASN, TerminalId, (int)(dbLyLoad * 100.0), false);
            }
            CloseUserCard();
        }

        private void btnReadLy_Click(object sender, EventArgs e)
        {
            if (m_nAppIndex != 2 || !OpenUserCard(2) || !ReadUserCardAsn(2))
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
            if (!OpenUserCard(2) || !ReadUserCardAsn(2))
                return;
            if (m_UserCardCtrl.VerifyUserPin(m_strLyPin) == 1)
            {
                const float BusinessLoyalty = 0.0F;//强制联机解灰 0 扣款
                byte[] TerminalId = new byte[6];
                if (PublicFunc.ByteDataEquals(TerminalId, m_TermialId))//未读到终端机编号，使用固定编号
                    Buffer.BlockCopy(m_FixedTermialId, 0, TerminalId, 0, 6);
                else
                    Buffer.BlockCopy(m_TermialId, 0, TerminalId, 0, 6);
                if (m_UserCardCtrl.UnLockGrayCard(m_ASN, TerminalId, (int)(BusinessLoyalty * 100.0), false))
                    m_bLyGray = false;
            }
            CloseUserCard();
        }

        private void btnLockLy_Click(object sender, EventArgs e)
        {
            if (m_bLyGray || m_nAppIndex != 2)
                return;
            if (!OpenUserCard(2) || !ReadUserCardAsn(2))
                return;
            if (m_UserCardCtrl.VerifyUserPin(m_strLyPin) != 1)
                return;

            bool bSamSlot = LySamSlot.Checked;//使用SAM卡槽
            if (!OpenSAMCard(bSamSlot, 2))
                return;
            //灰锁初始化
            byte[] outData = new byte[15];
            m_UserCardCtrl.InitForGray(m_TermialId, outData,BalanceType.Balance_EP);
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

            if (!m_SamCardCtrl.SamAppSelect(bSamSlot))
                return;
            if (!m_SamCardCtrl.InitSamGrayLock(bSamSlot, m_TermialId, rand, OfflineSn, byteBalance, BusinessType, m_ASN, GrayLockData))
                return;
            byte[] GTAC = new byte[4];
            byte[] MAC2 = new byte[4];
            if (!m_UserCardCtrl.GrayLock(GrayLockData, GTAC, MAC2))
                return;
            if (!m_SamCardCtrl.VerifyMAC2(bSamSlot, MAC2))//验证MAC2
                return;
            m_nLyBusinessSn = (int)((OfflineSn[0] << 8) | OfflineSn[1]);
            m_nLyTerminalSn = (int)((GrayLockData[0] << 24) | (GrayLockData[1] << 16) | (GrayLockData[2] << 8) | GrayLockData[3]);
            if (!m_bContactCard)
                LySamSlot.Enabled = false;
        }

        private byte[] GetLyDebitforUnlockData(bool bSamSlot)
        {
            byte[] DebitData = new byte[27];
            //计算GMAC
            const byte BusinessType = 0x93;//交易类型: 解0扣
            decimal Amount = decimal.Parse(textDebitLy.Text, System.Globalization.NumberStyles.Number);
            double dbAmount = decimal.ToDouble(Amount);
            int nMoneyAmount = (int)(dbAmount * 100.0); ////气票消费金额
            byte[] GMAC = new byte[4];
            if (!m_SamCardCtrl.CalcGMAC(bSamSlot, BusinessType, m_ASN, m_nLyBusinessSn, nMoneyAmount, GMAC))
                return null;
            byte[] byteMoney = BitConverter.GetBytes(nMoneyAmount); //气票消费金额
            DebitData[0] = byteMoney[3];
            DebitData[1] = byteMoney[2];
            DebitData[2] = byteMoney[1];
            DebitData[3] = byteMoney[0];
            m_nLyBusinessSn += 1;
            DebitData[4] = (byte)((m_nLyBusinessSn >> 8) & 0xFF);
            DebitData[5] = (byte)(m_nLyBusinessSn & 0xFF);
            Buffer.BlockCopy(m_TermialId, 0, DebitData, 6, 6);
            m_nLyTerminalSn += 1;
            DebitData[12] = (byte)((m_nLyTerminalSn >> 24) & 0xFF);//终端交易序号
            DebitData[13] = (byte)((m_nLyTerminalSn >> 16) & 0xFF);
            DebitData[14] = (byte)((m_nLyTerminalSn >> 8) & 0xFF);
            DebitData[15] = (byte)(m_nLyTerminalSn & 0xFF);
            byte[] SysTime = PublicFunc.GetBCDTime();
            Buffer.BlockCopy(SysTime, 0, DebitData, 16, 7);//BCD时间
            Buffer.BlockCopy(GMAC, 0, DebitData, 23, 4);//GMAC
            return DebitData;
        }

        private void btnUnlockLy_Click(object sender, EventArgs e)
        {
            if (m_bLyGray || m_UserCardCtrl == null)
                return;
            bool bSamSlot = LySamSlot.Checked;
            byte[] UnlockData = null;
            if (m_SamCardCtrl != null)
            {
                UnlockData = GetLyDebitforUnlockData(bSamSlot);
                CloseSAMCard(bSamSlot);
            }
            if (!m_bContactCard)
                LySamSlot.Enabled = true;

            if (UnlockData != null)
            {
                //解扣debit for unlock
                if (m_UserCardCtrl.DebitForUnlock(UnlockData, BalanceType.Balance_EP))
                {
                    //清TACUF （即 读灰锁状态，但其中P1 == 0x01）
                    m_UserCardCtrl.ClearTACUF();
                }
            }
            CloseUserCard();
        }

        private void btnReadLyRecord_Click(object sender, EventArgs e)
        {
            if (m_nAppIndex != 2 || !OpenUserCard(2))
                return;
            if (!m_UserCardCtrl.SelectCardApp(2))
                return;
            if (m_UserCardCtrl.VerifyUserPin(m_strLyPin) == 1)
            {
                List<CardRecord> lstRecord = m_UserCardCtrl.ReadRecord();
                if (lstRecord.Count > 0)
                {
                    FillListView(lstRecord);
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
                item.SubItems.Add(record.Amount.ToString("F2"));
                item.SubItems.Add(RecordType(record.BusinessType));
                item.SubItems.Add(record.TerminalID);
                item.SubItems.Add(record.BusinessTime);
                RecordInCard.Items.Add(item);
            }
        }
    }
}   

