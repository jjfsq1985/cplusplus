using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Cryptography;
using ApduInterface;
using ApduCtrl;
using System.Threading;

namespace PublishCardX
{
    [Guid("8FA99275-798D-454b-AC7E-DEC997B99042")]
    [ProgId("PublishCardX.PublishOperator")]
    [ComVisible(true)]    
    public partial class PublishOperator : UserControl,IObjectSafety
    {
        private const int S_OK = 0;
        private const int S_FALSE = 1;
        #region IObjectSafety 成员

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;        

        [PreserveSig]
        public int GetInterfaceSafetyOptions(ref Guid riid, [MarshalAs(UnmanagedType.U4)] ref int pdwSupportedOptions, [MarshalAs(UnmanagedType.U4)] ref int pdwEnabledOptions)
        {
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            return S_OK;
        }

        [PreserveSig]
        public int SetInterfaceSafetyOptions(ref Guid riid, [MarshalAs(UnmanagedType.U4)] int dwOptionSetMask, [MarshalAs(UnmanagedType.U4)] int dwEnabledOptions)
        {
            return S_OK;
        }

        #endregion

        private IUserCardControl m_UserCardCtrl = null;
        private ApduController m_DevControl = null;

        private const string m_MutexName = "1644D21D-E795-4de6-937A-5AFCE0F26A5D";

        //同一时间只能一个页面使用该控件，另一页面使用前需要先释放（ReleaseCardCtrl）
        private Mutex m_SingleObj = null;

        public PublishOperator()
        {

        }

        ~PublishOperator()
        {
            CloseDevice();  
        }

        private bool CloseDevice()
        {
            if (m_DevControl != null)
            {
                if (m_DevControl.IsDeviceOpen())
                {
                    m_DevControl.Close_Device();
                }
                m_DevControl = null;
            }   
            if (m_SingleObj != null)
            {
                m_SingleObj.Close();
                m_SingleObj = null;
            }            
            return true;
        }

        private bool OpenDevice()
        {
            if (m_DevControl == null)
                return false;
            if (m_DevControl.IsDeviceOpen())
                return true;
            return m_DevControl.Open_Device();
        }

        /// <summary>
        /// 打开卡片
        /// </summary>
        /// <param name="bContact">是否接触式卡片</param>        
        private bool OpenUserCard(bool bContact)
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            m_UserCardCtrl = m_DevControl.UserCardConstructor(bContact);

            string cardInfo = "";
            if (bContact)
                return m_DevControl.OpenContactCard(ref cardInfo);
            else
                return m_DevControl.OpenCard(ref cardInfo);
        }

        private bool CloseUserCard(bool bContact)
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            if (bContact)
                m_DevControl.CloseContactCard();
            else
                m_DevControl.CloseCard();
            m_UserCardCtrl = null;
            return true;
        }

        private void ReadCardInfo(UserCardInfoParam CardInfo)
        {
            if (!m_UserCardCtrl.SelectCardApp(1))
                return;
            m_UserCardCtrl.GetUserCardInfo(CardInfo);
        }

        private bool HasRunOne()
        {
            bool bNew = false;
            m_SingleObj = new Mutex(true, m_MutexName, out bNew);
            if (bNew)
            {
                m_SingleObj.ReleaseMutex();
                return false;
            }
            else
            {
                Trace.WriteLine("PublishCardX can be called only once at the same time!!");
                return true;
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="nCardType">设备类型 0:达华卡-明泰读卡器 1：龙寰卡-DE-620 Reader 2:龙寰卡-明泰读卡器</param>
        public int InitCardControl(int nDeviceType)
        {
            if (HasRunOne())
                return S_FALSE;

            if (nDeviceType == 0)
            {
                CloseDevice();
                m_DevControl = new ApduController(ApduType.DaHua);
                OpenDevice();
            }
            else if (nDeviceType == 1)
            {
                CloseDevice();
                m_DevControl = new ApduController(ApduType.LongHuan);
                OpenDevice();
            }
            else if (nDeviceType == 2)
            {
                CloseDevice();
                m_DevControl = new ApduController(ApduType.LoH_at_MT);
                OpenDevice();
            }
            else
            {
                m_DevControl = null;
            }
            return S_OK;
        }

        public int ReleaseCardCtrl()
        {
            CloseDevice();
            return S_OK;
        }

        public int ReadUserCardInfo(bool bContact,UserCardInfoParam CardInfo)
        {
            if (m_DevControl == null)
                return S_FALSE;
            if (!OpenUserCard(bContact))
            {
                MessageBox.Show("打开卡片失败");
                return S_FALSE;
            }            
            ReadCardInfo(CardInfo);
            CloseUserCard(bContact);
            return S_OK;
        }

        public double ReadCardBalance(bool bContact, string strPIN)
        {
            if (m_DevControl == null)
                return 0.0;
            double dbBalance = 0.0;
            if (!OpenUserCard(bContact) || strPIN.Length != 6)
                return dbBalance;
            if (!m_UserCardCtrl.SelectCardApp(1))
                return dbBalance;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return dbBalance;
            }
            int nRet = m_UserCardCtrl.VerifyUserPin(strPIN);
            if (nRet == 1)
            {
                m_UserCardCtrl.UserCardBalance(ref dbBalance);
            }
            else if (nRet == 2)
            {
                MessageBox.Show("PIN码已锁!");
            }
            else
            {
                MessageBox.Show("PIN码验证失败!");
            }
            CloseUserCard(bContact);
            return dbBalance;
        }

        public int ReadCardGrayStatus(bool bContact, string strPIN,GrayStatus cardStatus)
        {
            if (m_DevControl == null)
                return S_FALSE;
            if (!OpenUserCard(bContact) || strPIN.Length != 6)
                return S_FALSE;
            if (!m_UserCardCtrl.SelectCardApp(1))
                return S_FALSE;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return S_FALSE;
            }
            int nResult = S_FALSE;
            int nRet = m_UserCardCtrl.VerifyUserPin(strPIN);
            if (nRet == 1)
            {
                bool bGray = false;
                byte[] byteTerminalID = new byte[6];
                byte[] byteGTAC = new byte[4];
                //未灰锁时终端机编号输出为0
                int nCardStatus = 0;
                if (m_UserCardCtrl.UserCardGray(ref nCardStatus, byteTerminalID, byteGTAC))
                {
                    if (nCardStatus == 2)
                    {
                        //当前TAC未读，需要清空后重读
                        m_UserCardCtrl.ClearTACUF();
                        nCardStatus = 0;
                        m_UserCardCtrl.UserCardGray(ref nCardStatus, byteTerminalID, byteGTAC);
                        bGray = nCardStatus == 1 ? true : false;
                    }
                    else
                    {
                        bGray = nCardStatus == 1 ? true : false;
                    }
                    cardStatus.bCardGray = bGray;
                    cardStatus.strTerminalId = BitConverter.ToString(byteTerminalID).Replace("-", "");
                    cardStatus.strGTAC = BitConverter.ToString(byteGTAC).Replace("-", "");

                    nResult = S_OK;
                }
            }
            else if (nRet == 2)
            {
                MessageBox.Show("PIN码已锁!");
            }
            else
            {
                MessageBox.Show("PIN码验证失败!");
            }
            CloseUserCard(bContact);
            return nResult;
        }

        /// <summary>
        /// 写卡信息
        /// </summary>
        /// <param name="CardInfo"></param>
        /// <param name="strProtectCode">MD5保护码</param>
        /// <param name="strTime">时间值(格式：“yyyyMMddHHmmss”)</param>
        /// <param name="strTendingKey">维护密钥</param>
        /// <returns></returns>
        public int WriteUserCardInfo(bool bContact,UserCardInfoParam CardInfo, EncryptKeyParam AppTendingKeyInfo, EncryptKeyParam MasterKeyInfo, VerifyParam VerifyInfo)
        {
            if (m_DevControl == null)
                return S_FALSE;

            if (!OpenUserCard(bContact))
            {
                MessageBox.Show("打开卡片失败");
                return S_FALSE;
            }
            UserCardInfoParam CurCard = new UserCardInfoParam();
            ReadCardInfo(CurCard);
            byte[] CurCardId = CurCard.GetUserCardID(); 
            byte[] WriteCardId = CardInfo.GetUserCardID();
            if(CurCardId == null || WriteCardId == null)
            {
                MessageBox.Show("卡号不能为空");
                CloseUserCard(bContact);
                return S_FALSE;
            }
            if (!PublicFunc.ByteDataEquals(CurCardId, WriteCardId))
            {
                MessageBox.Show("卡号不匹配");
                CloseUserCard(bContact);
                return S_FALSE;
            }

            if (!CheckFunctionCall(CurCard.GetStringUserCardID(), VerifyInfo))
            {
                CloseUserCard(bContact);
                return S_FALSE;
            }
            byte[] TendingKey = CalcDestKey(AppTendingKeyInfo);
            byte[] MasterKey = CalcDestKey(MasterKeyInfo);
            if (TendingKey != null)
            {
                if (!m_UserCardCtrl.UpdateCardInfo(CardInfo, TendingKey, MasterKey))
                {
                    MessageBox.Show("修改卡片信息失败");
                }
            }
            else
            {
                MessageBox.Show("提供的密钥格式不正确");
            }
            CloseUserCard(bContact);
            return S_OK;
        }

        public int UnlockGrayCard(bool bContact,string strPIN, string strTerminalID, double dbUnlockMoney, EncryptKeyParam UnlockKeyInfo, VerifyParam VerifyInfo)
        {
            if (m_DevControl == null)
                return S_FALSE;

            byte[] byteTerminalID = PublicFunc.StringToBCD(strTerminalID);
            if (byteTerminalID == null)
                return S_FALSE;
            if (!OpenUserCard(bContact) || strPIN.Length != 6 || byteTerminalID.Length != 6)
                return S_FALSE;
            if (!m_UserCardCtrl.SelectCardApp(1))
                return S_FALSE;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                CloseUserCard(bContact);
                return S_FALSE;
            }

            if (!CheckFunctionCall(BitConverter.ToString(ASN).Replace("-", ""), VerifyInfo))
            {
                CloseUserCard(bContact);
                return S_FALSE;
            }

            int nResult = S_FALSE;
            int nRet = m_UserCardCtrl.VerifyUserPin(strPIN);
            if (nRet == 1)
            {
                byte[] UnlockKey = CalcDestKey(UnlockKeyInfo);
                if (UnlockKey != null)
                {           
                    Trace.WriteLine("解灰信息：" +　BitConverter.ToString(ASN) + strTerminalID + dbUnlockMoney.ToString("F2"));
                    m_UserCardCtrl.UnLockGrayCard(ASN, byteTerminalID, (int)(dbUnlockMoney * 100.0), UnlockKey);
                    nResult = S_OK;
                }
            }
            else if (nRet == 2)
            {
                MessageBox.Show("PIN码已锁!");
            }
            else
            {
                MessageBox.Show("PIN码验证失败!");
            }
            CloseUserCard(bContact);
            return nResult;
        }

        //卡片圈存
        public int CardLoadMoney(bool bContact, string strPIN, string strTerminalID, double dbLoadMoney, EncryptKeyParam LoadKeyInfo, VerifyParam VerifyInfo)
        {
            if (m_DevControl == null)
                return S_FALSE;

            byte[] byteTerminalID = PublicFunc.StringToBCD(strTerminalID);
            if (byteTerminalID == null)
                return S_FALSE;
            if (!OpenUserCard(bContact) || strPIN.Length != 6 || byteTerminalID.Length != 6)
                return S_FALSE;
            if (!m_UserCardCtrl.SelectCardApp(1))
                return S_FALSE;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                CloseUserCard(bContact);
                return S_FALSE;
            }

            if (!CheckFunctionCall(BitConverter.ToString(ASN).Replace("-", ""), VerifyInfo))
            {
                CloseUserCard(bContact);
                return S_FALSE;
            }

            int nResult = S_FALSE;
            int nRet = m_UserCardCtrl.VerifyUserPin(strPIN);
            if (nRet == 1)
            {
                byte[] LoadKey = CalcDestKey(LoadKeyInfo);
                if (LoadKey != null)
                {
                    if (m_UserCardCtrl.UserCardLoad(ASN, byteTerminalID, (int)(dbLoadMoney * 100.0), LoadKey))
                    {
                        Trace.WriteLine("成功对卡号" + BitConverter.ToString(ASN) + "圈存" + dbLoadMoney.ToString("F2") + "元.");
                        nResult = S_OK;
                    }
                    else
                    {
                        MessageBox.Show("圈存失败");
                    }
                }
            }
            else if (nRet == 2)
            {
                MessageBox.Show("PIN码已锁!");
            }
            else
            {
                MessageBox.Show("PIN码验证失败!");
            }

            CloseUserCard(bContact);
            return nResult;
        }

        //PIN码解锁
        public int UnlockPin(bool bContact, string strPin, EncryptKeyParam UnlockPinKeyInfo, VerifyParam VerifyInfo)
        {
            if (m_DevControl == null)
                return S_FALSE;

            if (!OpenUserCard(bContact) || strPin.Length != 6)
                return S_FALSE;
            if (!m_UserCardCtrl.SelectCardApp(1))
                return S_FALSE;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                CloseUserCard(bContact);
                return S_FALSE;
            }

            if (!CheckFunctionCall(BitConverter.ToString(ASN).Replace("-", ""), VerifyInfo))
            {
                CloseUserCard(bContact);
                return S_FALSE;
            }
            int nResult = S_FALSE;
            byte[] PinUnlockKey = CalcDestKey(UnlockPinKeyInfo);
            if (PinUnlockKey != null)
            {
                if (m_UserCardCtrl.PINUnLock(ASN, strPin, PinUnlockKey))
                {
                    nResult = S_OK;
                    MessageBox.Show("PIN码已解锁");
                }
                else
                {
                    MessageBox.Show("PIN码解锁失败");
                }
            }
            CloseUserCard(bContact);
            return nResult;
        }

        //PIN码重装
        public int ResetPin(bool bContact, string strPinNew, EncryptKeyParam ResetPinKeyInfo, VerifyParam VerifyInfo)
        {
            if (m_DevControl == null)
                return S_FALSE;

            if (!OpenUserCard(bContact) || strPinNew.Length != 6)
                return S_FALSE;
            if (!m_UserCardCtrl.SelectCardApp(1))
                return S_FALSE;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                CloseUserCard(bContact);
                return S_FALSE;
            }

            if (!CheckFunctionCall(BitConverter.ToString(ASN).Replace("-", ""), VerifyInfo))
            {
                CloseUserCard(bContact);
                return S_FALSE;
            }
            int nResult = S_FALSE;
            byte[] PinReloadKey = CalcDestKey(ResetPinKeyInfo);
            if (PinReloadKey != null)
            {
                if (m_UserCardCtrl.PINReset(ASN, strPinNew, PinReloadKey))
                {
                    nResult = S_OK;
                    MessageBox.Show("新PIN码PIN码已装入");
                }
                else
                {
                    MessageBox.Show("PIN码重装失败");
                }
            }
            CloseUserCard(bContact);
            return nResult;
        }

        //PIN码修改
        public int ChangePin(bool bContact, string strPinOld, string strPinNew)
        {
            if (m_DevControl == null)
                return S_FALSE;

            if (strPinOld.Length != 6 || strPinNew.Length != 6)
                return S_FALSE;
            if (!OpenUserCard(bContact) || !m_UserCardCtrl.SelectCardApp(1))
                return S_FALSE;
            int nResult = S_FALSE;
            if (m_UserCardCtrl.ChangePIN(strPinOld, strPinNew))
            {
                if (m_UserCardCtrl.VerifyUserPin(strPinNew) == 1)
                {
                    nResult = S_OK;
                    MessageBox.Show("PIN码修改成功");
                }
                else
                {
                    MessageBox.Show("PIN码修改后验证失败，请重试");
                }
            }
            else
            {
                MessageBox.Show("PIN码修改失败");
            }
            CloseUserCard(bContact);
            return nResult;
        }

        private byte[] CalcDestKey(EncryptKeyParam EncryptObj)
        {
            if (EncryptObj.strEncryptSeed.Length != 16 || EncryptObj.strInitKey.Length != 32 || EncryptObj.strSrcKey.Length != 32)
                return null;
            byte[] InitData = PublicFunc.StringToBCD(EncryptObj.strEncryptSeed);
            byte[] InitKey = PublicFunc.StringToBCD(EncryptObj.strInitKey);

            byte[] Left = DesCryptography.TripleEncryptData(InitData, InitKey);
            byte[] Right = DesCryptography.TripleDecryptData(InitData, InitKey);
            byte[] EncryptKey = new byte[16];
            Buffer.BlockCopy(Left, 0, EncryptKey, 0, 8);
            Buffer.BlockCopy(Right, 0, EncryptKey, 8, 8);

            return DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(EncryptObj.strSrcKey), EncryptKey);            
        }

        private bool CheckFunctionCall(string strCardId, VerifyParam VerifyObj)
        {
            if (VerifyObj.strVerifyTime.Length != 14)
                return false;
            string strMD5Src = VerifyObj.strVerifyTime.Substring(0, 8) + strCardId + VerifyObj.strVerifyTime.Substring(8, 6);
            MD5 md5Provider = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.ASCII.GetBytes(strMD5Src);
            byte[] targetData = md5Provider.ComputeHash(fromData);
            if (string.Compare(BitConverter.ToString(targetData).Replace("-", ""), VerifyObj.strPrivateCode, true) == 0)
                return true;
            else
                return false;
        }

    }
}
