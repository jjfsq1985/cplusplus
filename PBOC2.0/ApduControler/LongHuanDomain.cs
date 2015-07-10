using System;
using System.Collections.Generic;
using System.Text;
using ApduLoh;
using ApduInterface;
using System.Diagnostics;
using LohApduCtrl;
using IFuncPlugin;

namespace ApduCtrl
{
    class LongHuanDomain
    {
        private string m_ReaderNameContactless = "Duali DE-620 Contactless Reader 0";
        private string m_ReaderNameContact = "Duali DE-620 Contact Reader 0";
        private string m_ReaderNameSam = "Duali DE-620 SAM Reader 0";   //该读卡器在内部，标准SAM卡槽，一般不用于制卡

        private PcscSmardCard m_PcscReader = new PcscSmardCard();

        private LohPsamApduProvider m_ctrlPsamApdu = new LohPsamApduProvider();
        private LohUserApduProvider m_ctrlUserCardApdu = new LohUserApduProvider();

        public bool Open_Device()
        {
            List<string> Readers = new List<string>();
            bool bRet = m_PcscReader.LH_Open(ref Readers);
            if (!bRet || Readers.Count == 0)
                return false;
            else
                return true;
        }

        public void Close_Device()
        {
            m_PcscReader.LH_Close();
        }

        public ISamApduProvider GetPsamApduProvider()
        {
            return m_ctrlPsamApdu;
        }

        public IUserApduProvider GetUserApduProvider()
        {
            return m_ctrlUserCardApdu;
        }

        public bool OpenCard(ref string CardAtr)
        {
            byte[] byteCardAtr = null;
            if (m_PcscReader.LH_ConnectReader(m_ReaderNameContactless, out byteCardAtr))
            {
                CardAtr = BitConverter.ToString(byteCardAtr).Replace("-", "");
                return true;
            }
            else
            {                
                return false;
            }
            
        }

        public bool OpenContactCard(ref string CardAtr)
        {
            byte[] byteCardAtr = null;
            if (m_PcscReader.LH_ConnectReader(m_ReaderNameContact, out byteCardAtr))
            {
                CardAtr = BitConverter.ToString(byteCardAtr).Replace("-", "");
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            return m_PcscReader.LH_DataTransmit(m_ReaderNameContactless, data, datalen, outdata, ref outdatalen);
        }

        public int ContactCmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            return m_PcscReader.LH_DataTransmit(m_ReaderNameContact, data, datalen, outdata, ref outdatalen);
        }

        public void CloseCard()
        {
            m_PcscReader.LH_DisconnectReader(m_ReaderNameContactless);
        }

        public void CloseContactCard()
        {
            m_PcscReader.LH_DisconnectReader(m_ReaderNameContact);
        }

        public bool IccPowerOn(ref string CardAtr)
        {
            byte[] byteCardAtr = null;
            if (m_PcscReader.LH_ConnectReader(m_ReaderNameContact, out byteCardAtr))
            {
                CardAtr = BitConverter.ToString(byteCardAtr).Replace("-", "");
                return true;
            }
            return false;
        }

        public int IccCmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            return m_PcscReader.LH_DataTransmit(m_ReaderNameContact, data, datalen, outdata, ref outdatalen);

        }

        public void IccPowerOff()
        {
            m_PcscReader.LH_DisconnectReader(m_ReaderNameContact);
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

        public bool IsDevicePcscMode(ref int nMode)
        {
            List<string> Readers = new List<string>();
            bool bRet = m_PcscReader.LH_Open(ref Readers);
            if (!bRet || Readers.Count == 0)
                return false;
            else
            {
                nMode = Readers.Count;
                return true;
            }
        }

        public bool ChangeDevice(int nMode)
        {
            List<string> Readers = new List<string>();
            bool bRet = m_PcscReader.LH_Open(ref Readers);
            if (!bRet || Readers.Count == 0)
            {
                return m_PcscReader.LH_ChangeMode_Dll((byte)nMode);                    
            }
            else
            {
                byte[] CardAtr = null;
                if (!m_PcscReader.LH_ConnectReader(m_ReaderNameContactless, out CardAtr))
                {
                    System.Diagnostics.Trace.WriteLine("插入非接触式卡片才能修改读卡器Mode.");
                    return false;
                }
                byte ChangeMode = (byte)nMode;
                bool bChange = m_PcscReader.LH_ChangeMode(false,ref ChangeMode);
                m_PcscReader.LH_DisconnectReader(m_ReaderNameContactless);
                return bChange;
            }

        }

        public ISamCardControl SamCardConstructor(ApduController ctrlApdu, SqlConnectInfo DbInfo)
        {
            return new LohPsamCardControl(ctrlApdu, DbInfo);
        }

        public IUserCardControl UserCardConstructor(ApduController ctrlApdu, bool bContact, SqlConnectInfo DbInfo)
        {
            return new LohUserCardControl(ctrlApdu, bContact, DbInfo);
        }

    }
}
