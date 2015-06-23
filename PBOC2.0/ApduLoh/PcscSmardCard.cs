using System;
using System.Collections.Generic;
using System.Text;

namespace ApduLoh
{
    public class PcscSmardCard
    {        
        private UIntPtr hContext = UIntPtr.Zero;
        private string PcscReadName = "";
        private UIntPtr hCard = UIntPtr.Zero;
        private uint ActiveProtocol = WinSCard_Dll.SCARD_PROTOCOL_UNDEFINED;
        private uint IOCTL_BYPASS_COMMAND = 0x00312418;//直接写命令==SCARD_CTL_CODE(0x906)

        /// <summary>
        /// 连接PCSC读卡器
        /// </summary>
        /// <param name="lstReaders">PCSC读卡器列表</param>
        public bool LH_Open(ref List<string> lstReaders)
        {
            if (hContext != UIntPtr.Zero)
                return true;
            int nResult = WinSCard_Dll.SCardEstablishContext(WinSCard_Dll.SCARD_SCOPE_USER, IntPtr.Zero, IntPtr.Zero, ref hContext);
            if (nResult != 0)
                return false;            
            uint mszLen = 0;
            nResult = WinSCard_Dll.SCardListReaders(hContext, null, null, ref mszLen);
            if (nResult != 0)
                return false;
            Char[] mszReaders = new Char[mszLen];
            WinSCard_Dll.SCardListReaders(hContext, null, mszReaders, ref mszLen);

            lstReaders.Clear();
            int nSplitLen = 0;
            for (int i = 0; i < mszLen; i++)
            {
                if (mszReaders[i] != '\0')
                {
                    nSplitLen++;
                }
                else if (nSplitLen > 0)
                { 
                    lstReaders.Add(new string(mszReaders, i-nSplitLen,nSplitLen));
                    nSplitLen = 0;
                }
            }
            return true;            
        }

        /// <summary>
        /// 关闭读卡器
        /// </summary>        
        public bool LH_Close()
        {
            if (hContext == UIntPtr.Zero)
                return true;
            LH_DisconnectReader();
            WinSCard_Dll.SCardReleaseContext(hContext);
            hContext = UIntPtr.Zero;
            return true;
        }

        /// <summary>
        /// 打开PCSC读卡器
        /// </summary>
        /// <param name="strReadName">PCSC读卡器名称</param>
        /// <param name="CardAtr">卡片ATR</param>        
        public bool LH_ConnectReader(string strReadName, out byte[] CardAtr)
        {
            CardAtr = null;
            if (hContext == UIntPtr.Zero)
                return false;
            LH_DisconnectReader();
            //独占方式连接读卡器            
            int nResult = WinSCard_Dll.SCardConnect(hContext, strReadName, WinSCard_Dll.SCARD_SHARE_EXCLUSIVE, WinSCard_Dll.SCARD_PROTOCOL_T0 | WinSCard_Dll.SCARD_PROTOCOL_T1, ref hCard, ref ActiveProtocol);
            if (nResult != 0 || hCard == UIntPtr.Zero)
                return false;
            //检查卡状态
            uint Readerlen = 0;
            uint dwstat = 0;
            uint protocol = 0;
            uint atrlen = 0;
            nResult = WinSCard_Dll.SCardStatus(hCard, null, ref Readerlen, ref dwstat, ref protocol, null, ref atrlen);
            if (nResult != 0)
                return false;
            Char[] ReaderName = new Char[Readerlen];
            CardAtr = new byte[atrlen];
            WinSCard_Dll.SCardStatus(hCard, ReaderName, ref Readerlen, ref dwstat, ref protocol, CardAtr, ref atrlen);
            string strConvertReaderName = new string(ReaderName).TrimEnd('\0');
            if (strConvertReaderName == strReadName && ActiveProtocol == protocol)
            {
                PcscReadName = strConvertReaderName;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 关闭PCSC读卡器
        /// </summary>        
        public bool LH_DisconnectReader()
        {
            if (hCard == UIntPtr.Zero)
                return true;
            WinSCard_Dll.SCardDisconnect(hCard, WinSCard_Dll.SCARD_LEAVE_CARD);
            hCard = UIntPtr.Zero;
            return true;
        }

        /// <summary>
        /// 数据收发
        /// </summary>
        /// <param name="SendData">发送的数据</param>
        /// <param name="uSendLen">发送长度</param>
        /// <param name="RecvData">接收的数据</param>
        /// <param name="uRecvLen">接收长度</param>        
        public bool LH_DataTransmit(byte[] SendData, int SendLen, out byte[] RecvData, out int RecvLen)
        {
            RecvData = null;
            RecvLen = 0;
            if (hCard == UIntPtr.Zero)
                return false;
            IntPtr SendPci = IntPtr.Zero;
            if (ActiveProtocol == WinSCard_Dll.SCARD_PROTOCOL_T0)
                SendPci = WinSCard_Dll.SCardT0Pci();
            else if(ActiveProtocol == WinSCard_Dll.SCARD_PROTOCOL_T1)
                SendPci = WinSCard_Dll.SCardT1Pci();

            uint cbRecvLength = 255;
            byte[] cRecvBuffer = new byte[cbRecvLength];
            int nResult = WinSCard_Dll.SCardTransmit(hCard, SendPci, SendData, (uint)SendLen, IntPtr.Zero, cRecvBuffer, ref cbRecvLength);
            if (nResult != 0)
                return false;
            RecvLen = (int)cbRecvLength;
            RecvData = new byte[RecvLen];
            Array.Copy(cRecvBuffer, RecvData, RecvLen);
            return true;
        }

        /// <summary>
        /// 通过DualCardDll.dll修改读卡器Mode,只在smartcard模式下使用
        /// </summary>        
        /// <param name="nMode">1 RF(即Contactless),2 RF+contact, 3 RF+Contact+SAM</param>        
        public bool LH_ChangeMode_Dll(byte nMode)
        {
            LH_Close();
            const int nPort = 100;//USB  nPort：固定100
            int nRet = DllExportDuali.DE_InitPort(nPort, 115200); //nBaud：忽略
            if (nRet != 100)
                return false;
            //3个PC/SC读卡器(RF+Contact+SAM),rf 即Contactless
            DllExportDuali.DE_ChangeDevice(nPort, ref nMode, 1);//Inqflag固定为1
            return true;
        }


        /// <summary>
        /// PCSC读卡器Mode修改
        /// </summary>
        /// <param name="bInquire">是否查询，如果true,输出nMode</param>
        /// <param name="nMode">0 smartcard模式，pscs关闭; 1 RF(即Contactless),2 RF+contact, 3 RF+Contact+SAM</param>        
        public bool LH_ChangeMode(bool bInquire, ref byte nMode)
        {
            if (hCard == UIntPtr.Zero || !PcscReadName.Contains("Contactless"))
                return false;
            if (!bInquire && nMode > 3)
                return false;
            uint datalen = 0;
            byte[] data = null;
            if (bInquire)
            {
                datalen = 4;
                data = new byte[] { 0x00, 0x02, 0x15, 0x00};                
            }
            else
            {
                datalen = 5;
                data = new byte[] { 0x00, 0x03, 0x15, 0x01, 0x00 };
                data[4] = nMode;
            }
            uint uRet = 0;
            byte[] OutBuffer = new byte[8];
            WinSCard_Dll.SCardControl(hCard, IOCTL_BYPASS_COMMAND, data, datalen, OutBuffer, 8, ref uRet);
            if (uRet > 0)
            {
                if (bInquire && uRet == 4 && OutBuffer[1] == 0x02)
                    nMode = OutBuffer[3];
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
