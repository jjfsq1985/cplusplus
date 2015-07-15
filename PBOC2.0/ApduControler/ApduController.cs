﻿using System;
using System.Collections.Generic;
using System.Text;
using ApduInterface;
using IFuncPlugin;

namespace ApduCtrl
{
    public class ApduController
    {
        private ApduDomain m_Domain = ApduDomain.Unknown;
        private DaHuaDomain m_DahuaDomain = null;
        private LongHuanDomain m_LongHuanDomain = null;
        private bool m_bDeviceOpen = false;


        public ApduController(ApduDomain eDomain)
        {
            m_Domain = eDomain;
            if (m_Domain == ApduDomain.DaHua)
                m_DahuaDomain = new DaHuaDomain();
            else if (m_Domain == ApduDomain.LongHuan)
                m_LongHuanDomain = new LongHuanDomain();
        }

        public ISamApduProvider GetPsamApduProvider()
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.GetPsamApduProvider();
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.GetPsamApduProvider();
            else
                return null;
        }

        public IUserApduProvider GetUserApduProvider()
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.GetUserApduProvider();
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.GetUserApduProvider();
            else
                return null;
        }

        public bool IsDeviceOpen()
        {
            return m_bDeviceOpen;
        }

        public bool Open_Device()
        {
            if (m_Domain == ApduDomain.DaHua)
                m_bDeviceOpen = m_DahuaDomain.Open_Device();
            else if (m_Domain == ApduDomain.LongHuan)
                m_bDeviceOpen = m_LongHuanDomain.Open_Device();
            else
                m_bDeviceOpen = false;
            return m_bDeviceOpen;
        }

        public void Close_Device()
        {
            m_bDeviceOpen = false;
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

        public int CmdExchange(bool bContact,byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            if (m_Domain == ApduDomain.DaHua)
            {
                if (bContact)
                    return -1;
                else
                    return m_DahuaDomain.CmdExchange(data, datalen, outdata, ref outdatalen);
            }
            else if (m_Domain == ApduDomain.LongHuan)
            {
                if (bContact)
                    return m_LongHuanDomain.ContactCmdExchange(data, datalen, outdata, ref outdatalen);
                else
                    return m_LongHuanDomain.CmdExchange(data, datalen, outdata, ref outdatalen);
            }
            else
            {
                return -1;
            }
        }

        public void CloseCard()
        {
            if (m_Domain == ApduDomain.DaHua)
                m_DahuaDomain.CloseCard();
            else if (m_Domain == ApduDomain.LongHuan)
                m_LongHuanDomain.CloseCard();
        }

        public bool OpenContactCard(ref string CardAtr)
        {
            if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.OpenContactCard(ref CardAtr);
            else
                return false;
        }

        public void CloseContactCard()
        {
            if (m_Domain == ApduDomain.LongHuan)
                m_LongHuanDomain.CloseContactCard();
        }

        public bool IccPowerOn(ref string CardAtr)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.IccPowerOn(ref CardAtr);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.IccPowerOn(ref CardAtr);
            else
                return false;
        }

        public int IccCmdExchange(byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.IccCmdExchange(data, datalen, outdata, ref outdatalen);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.IccCmdExchange(data, datalen, outdata, ref outdatalen);
            else
                return -1;
        }

        public void IccPowerOff()
        {
            if (m_Domain == ApduDomain.DaHua)
                m_DahuaDomain.IccPowerOff();
            else if (m_Domain == ApduDomain.LongHuan)
                m_LongHuanDomain.IccPowerOff();
        }

        public bool SAMPowerOn(bool bSamSlot,ref string CardAtr)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.IccPowerOn(ref CardAtr);
            else if (m_Domain == ApduDomain.LongHuan)
            {
                if (bSamSlot)
                    return m_LongHuanDomain.OpenSAM(ref CardAtr);
                else
                    return m_LongHuanDomain.IccPowerOn(ref CardAtr);
            }
            else
                return false;
        }

        public int SAMCmdExchange(bool bSamSlot,byte[] data, int datalen, byte[] outdata, ref int outdatalen)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.IccCmdExchange(data, datalen, outdata, ref outdatalen);
            else if (m_Domain == ApduDomain.LongHuan)
            {
                if(bSamSlot)
                    return m_LongHuanDomain.CmdExchangeSam(data, datalen, outdata, ref outdatalen);
                else
                    return m_LongHuanDomain.IccCmdExchange(data, datalen, outdata, ref outdatalen);
            }
            else
                return -1;
        }

        public void SAMPowerOff(bool bSamSlot)
        {
            if (m_Domain == ApduDomain.DaHua)
                m_DahuaDomain.IccPowerOff();
            else if (m_Domain == ApduDomain.LongHuan)
            {
                if (bSamSlot)
                    m_LongHuanDomain.CloseSAM();
                else
                    m_LongHuanDomain.IccPowerOff();
            }
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

        public bool IsDevicePcscMode(ref int nMode)
        {
            if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.IsDevicePcscMode(ref nMode);
            else
                return true;
        }

        public bool ChangeDevice(int nMode)
        {
            if (nMode <= 0 || nMode > 3)
                return false;
            if (m_Domain == ApduDomain.LongHuan)
            {
                return m_LongHuanDomain.ChangeDevice(nMode);
            }
            else
            {
                return false;
            }            
        }

        public ISamCardControl SamCardConstructor(SqlConnectInfo DbInfo)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.SamCardConstructor(this,DbInfo);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.SamCardConstructor(this, DbInfo);
            else
                return null;
        }

        public IUserCardControl UserCardConstructor(bool bContact, SqlConnectInfo DbInfo)
        {
            if (m_Domain == ApduDomain.DaHua)
                return m_DahuaDomain.UserCardConstructor(this, bContact, DbInfo);
            else if (m_Domain == ApduDomain.LongHuan)
                return m_LongHuanDomain.UserCardConstructor(this, bContact, DbInfo);
            else
                return null;
        }

    }
}