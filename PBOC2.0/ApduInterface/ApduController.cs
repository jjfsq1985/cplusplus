using System;
using System.Collections.Generic;
using System.Text;

namespace ApduInterface
{
    public enum ApduDomain
    {
        Unknown = 0,
        DaHua = 1,
        LongHuan = 2
    }

    public class ApduController
    {
        ApduDomain m_Domain = ApduDomain.Unknown;
        DaHuaDomain m_DahuaDomain = null;
        LongHuanDomain m_LongHuanDomain = null;


        public ApduController(ApduDomain eDomain)
        {
            m_Domain = eDomain;
            if (m_Domain == ApduDomain.DaHua)
                m_DahuaDomain = new DaHuaDomain();
            else if (m_Domain == ApduDomain.LongHuan)
                m_LongHuanDomain = new LongHuanDomain();
        }


        public bool Open_Device(int nReader)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.Open_Device(nReader);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.Open_Device(nReader);
            else
                return false;
        }

        public void Close_Device()
        {
            if (m_Domain == ApduDomain.DaHua)
                m_DahuaDomain.Close_Device();
            else if (m_Domain == ApduDomain.LongHuan)
                m_LongHuanDomain.Close_Device();
        }

        public bool OpenCard(ref string CardAtr)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.OpenCard(ref CardAtr);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.OpenCard(ref CardAtr);
            else
                return false;
        }

        public bool CmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.CmdExchange(data, datalen, outdata, ref outdatalen);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.CmdExchange(data, datalen, outdata, ref outdatalen);
            else
                return false;
        }

        public void CloseCard()
        {
            if (m_Domain == ApduDomain.DaHua)
                m_DahuaDomain.CloseCard();
            else if (m_Domain == ApduDomain.LongHuan)
                m_LongHuanDomain.CloseCard();
        }

        public bool IccPowerOn(int nReader,ref string CardAtr)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.IccPowerOn(ref CardAtr);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.IccPowerOn(ref CardAtr);
            else
                return false;
        }

        public bool IccCmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.IccCmdExchange(data, datalen, outdata, ref outdatalen);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.IccCmdExchange(data, datalen, outdata, ref outdatalen);
            else
                return false;
        }

        public void IccPowerOff()
        {
            if (m_Domain == ApduDomain.DaHua)
                m_DahuaDomain.IccPowerOff();
            else if (m_Domain == ApduDomain.LongHuan)
                m_LongHuanDomain.IccPowerOff();
        }

        public string hex2asc(byte[] dataSrc, int nSrcLen)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.hex2asc(dataSrc, nSrcLen);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.hex2asc(dataSrc, nSrcLen);
            else
                return "";
        }
    }
}
