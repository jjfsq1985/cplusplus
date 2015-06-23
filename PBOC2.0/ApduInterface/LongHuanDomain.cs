using System;
using System.Collections.Generic;
using System.Text;
using ApduLoh;

namespace ApduInterface
{
    class LongHuanDomain
    {
        private string m_ReadName0 = "Duali DE-620 Contact Reader 0";
        private string m_ReadName1 = "Duali DE-620 Contactless Reader 0";
        private string m_ReadName2 = "Duali DE-620 SAM Reader 0";

        private PcscSmardCard m_PcscReader = new PcscSmardCard();
        private string m_ReaderName = "";

        public bool Open_Device(int nReader)
        {
            List<string> Readers = new List<string>();
            m_PcscReader.LH_Open(ref Readers);
            if (nReader == 0 && Readers.Contains(m_ReadName0))
            {
                m_ReaderName = m_ReadName0;
                return true;
            }
            else if (nReader == 1 && Readers.Contains(m_ReadName1))
            {
                m_ReaderName = m_ReadName1;
                return true;
            }
            else if (nReader == 2 && Readers.Contains(m_ReadName2))
            {
                m_ReaderName = m_ReadName2;
                return true;
            }
            else
            {
                m_ReaderName = "";
                return false;
            }
        }

        public void Close_Device()
        {
            m_PcscReader.LH_Close();
        }

        public bool OpenCard(ref string CardAtr)
        {
            if(string.IsNullOrEmpty(m_ReaderName))
                return false;       
            byte[] byteCardAtr = null;
            if (m_PcscReader.LH_ConnectReader(m_ReaderName, out byteCardAtr))
            {
                CardAtr = BitConverter.ToString(byteCardAtr).Replace("-", "");
                return true;
            }
            return false;
        }

        public bool CmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            if (string.IsNullOrEmpty(m_ReaderName))
                return false;  
            return m_PcscReader.LH_DataTransmit(data, datalen, out outdata, out outdatalen);
        }

        public void CloseCard()
        {
            m_PcscReader.LH_DisconnectReader();
        }

        public bool IccPowerOn(ref string CardAtr)
        {
            if (string.IsNullOrEmpty(m_ReaderName))
                return false;
            byte[] byteCardAtr = null;
            if (m_PcscReader.LH_ConnectReader(m_ReaderName, out byteCardAtr))
            {
                CardAtr = BitConverter.ToString(byteCardAtr).Replace("-", "");
                return true;
            }
            return false;
        }

        public bool IccCmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            if (string.IsNullOrEmpty(m_ReaderName))
                return false;
            return m_PcscReader.LH_DataTransmit(data, datalen, out outdata, out outdatalen);

        }

        public void IccPowerOff()
        {
            m_PcscReader.LH_DisconnectReader();
        }

        private byte ToAsc(byte Hex)
        {
            if (Hex >= 0 && Hex <= 9)
                return (byte)(Hex + 48);//'0'--'9'
            else if (Hex >= 10 && Hex <= 15)
                return (byte)(Hex + 55);//'A' -- 'F'
            else
                return (byte)(48);//'0'
        }

        public string hex2asc(byte[] dataSrc, int nSrcLen)
        {
            byte[] AsciiData = new byte[nSrcLen*2];            
            for (int i = 0; i < nSrcLen; i++)
            {
                AsciiData[i * 2] = ToAsc((byte)((dataSrc[i] >> 4) & 0x0F));
                AsciiData[i * 2 + 1] = ToAsc((byte)(dataSrc[i] & 0x0F));
            }
            return Encoding.ASCII.GetString(AsciiData);
        }
    }
}
