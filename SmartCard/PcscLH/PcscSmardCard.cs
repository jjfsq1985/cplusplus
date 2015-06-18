using System;
using System.Collections.Generic;
using System.Text;

namespace PcscLH
{
    public class PcscSmardCard
    {        
        private UIntPtr hContext = UIntPtr.Zero;
        private UIntPtr hCard = UIntPtr.Zero;
        private uint ActiveProtocol = WinSCard_Dll.SCARD_PROTOCOL_UNDEFINED;

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

        public bool LH_Close()
        {
            if (hContext == UIntPtr.Zero)
                return true;
            LH_DisconnectReader();
            WinSCard_Dll.SCardReleaseContext(hContext);
            hContext = UIntPtr.Zero;
            return true;
        }

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
                return true;
            else
                return false;
        }

        public bool LH_DisconnectReader()
        {
            if (hCard == UIntPtr.Zero)
                return true;
            WinSCard_Dll.SCardDisconnect(hCard, WinSCard_Dll.SCARD_LEAVE_CARD);
            hCard = UIntPtr.Zero;
            return true;
        }

        public bool LH_DataTransmit(byte[] SendData, uint uSendLen, out byte[] RecvData, out uint uRecvLen)
        {
            RecvData = null;
            uRecvLen = 0;
            if (hCard == UIntPtr.Zero)
                return false;
            IntPtr SendPci = IntPtr.Zero;
            if (ActiveProtocol == WinSCard_Dll.SCARD_PROTOCOL_T0)
                SendPci = WinSCard_Dll.SCardT0Pci();
            else if(ActiveProtocol == WinSCard_Dll.SCARD_PROTOCOL_T1)
                SendPci = WinSCard_Dll.SCardT1Pci();

            uint cbRecvLength = 255;
            byte[] cRecvBuffer = new byte[cbRecvLength];
            int nResult = WinSCard_Dll.SCardTransmit(hCard, SendPci, SendData, uSendLen, IntPtr.Zero, cRecvBuffer, ref cbRecvLength);
            if (nResult != 0)
                return false;
            uRecvLen = cbRecvLength;
            RecvData = new byte[uRecvLen];
            Array.Copy(cRecvBuffer, RecvData, uRecvLen);
            return false;
        }

    }
}
