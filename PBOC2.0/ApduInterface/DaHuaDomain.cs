using System;
using System.Collections.Generic;
using System.Text;
using ApduDaHua;

namespace ApduInterface
{
    class DaHuaDomain
    {
        private enum ICC_Status
        {
            ICC_PowerOff = 0, //未上电
            ICC_PowerOn     //已上电
        }

        private int m_hDevHandler = 0;//读卡器句柄
        private ICC_Status m_curIccStatus = ICC_Status.ICC_PowerOff;

        public bool Open_Device(int nReader)
        {
            m_hDevHandler = DllExportMT.open_device(0, 9600);
            if (m_hDevHandler <= 0)
                return false;
            else
                return true;            
        }

        public void Close_Device()
        {
            if (m_hDevHandler <= 0)
                return;
            DllExportMT.close_device(m_hDevHandler);
            m_hDevHandler = 0;
        }

        public bool OpenCard(ref string CardAtr)
        {
            if (m_hDevHandler <= 0)
                return false;
            byte[] cardUid = new byte[4];
            byte[] cardInfo = new byte[64];
            byte[] cardInfolen = new byte[4];
            short nRet = DllExportMT.OpenCard(m_hDevHandler, 1, cardUid, cardInfo, cardInfolen);
            if (nRet != 0)
                return false;
            uint infoLen = BitConverter.ToUInt32(cardInfolen, 0);
            byte[] cardInfoAsc = new byte[infoLen * 2];
            DllExportMT.hex_asc(cardInfo, cardInfoAsc, infoLen);
            CardAtr = Encoding.ASCII.GetString(cardInfoAsc);
            return true;   
        }

        public bool CmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            if (m_hDevHandler <= 0)
                return false;
            byte[] recvBuffer = new byte[255];
            byte[] recvLen = new byte[4];
            short nRet = DllExportMT.ExchangePro(m_hDevHandler, data, (short)datalen, recvBuffer, recvLen);
            if (nRet != 0)
                return false;            
            uint outLen = BitConverter.ToUInt32(recvLen, 0);
            outdatalen = (int)outLen;
            Buffer.BlockCopy(recvBuffer, 0, outdata, 0, outdatalen);
            return true;
        }

        public void CloseCard()
        {
            if (m_hDevHandler <= 0)
                return;
            DllExportMT.CloseCard(m_hDevHandler);
            m_hDevHandler = 0;
        }

        public bool IccPowerOn(ref string CardAtr)
        {
            if (m_hDevHandler <= 0)
                return false;
            short nRet = 0;
            byte[] cardInfo = new byte[64];
            byte[] cardInfolen = new byte[4];
            if (m_curIccStatus == ICC_Status.ICC_PowerOn)
            {
                nRet = DllExportMT.ICC_Reset(m_hDevHandler, 0x00, cardInfo, cardInfolen);
            }
            else
            {
                nRet = DllExportMT.ICC_PowerOn(m_hDevHandler, 0x00, cardInfo, cardInfolen);
            }
            if (nRet != 0)
                return false;
            uint infoLen = BitConverter.ToUInt32(cardInfolen, 0);
            byte[] cardInfoAsc = new byte[infoLen * 2];
            DllExportMT.hex_asc(cardInfo, cardInfoAsc, infoLen);
            CardAtr = Encoding.ASCII.GetString(cardInfoAsc);
            return true;
        }

        public bool IccCmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            if (m_hDevHandler <= 0)
                return false;
            byte[] recvBuffer = new byte[255];
            byte[] recvLen = new byte[4];
            short nRet = DllExportMT.ICC_CommandExchange(m_hDevHandler, 0x00, data, (short)datalen, recvBuffer, recvLen);
            if (nRet != 0)
                return false;
            uint outLen = BitConverter.ToUInt32(recvLen, 0);
            outdatalen = (int)outLen;
            Buffer.BlockCopy(recvBuffer, 0, outdata, 0, outdatalen);
            return true;
        }

        public void IccPowerOff()
        {
            if (m_hDevHandler <= 0)
                return;
            DllExportMT.ICC_PowerOff(m_hDevHandler, 0x00);
            m_curIccStatus = ICC_Status.ICC_PowerOff;  
        }

        public string hex2asc(byte[] dataSrc, int nSrcLen)
        {
            byte[] dataAsc = new byte[nSrcLen * 2];
            DllExportMT.hex_asc(dataSrc, dataAsc, (uint)nSrcLen);
            return Encoding.ASCII.GetString(dataAsc);
        }
    }
}
