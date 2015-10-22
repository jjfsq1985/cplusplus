using System;
using System.Collections.Generic;
using System.Text;
using IFuncPlugin;
using System.Data.SqlClient;
using System.Data;
using SqlServerHelper;
using ApduParam;
using ApduCtrl;
using ApduInterface;

namespace DaHuaApduCtrl
{
    public class DaHuaCardCtrlBase
    {
        //MFœ¬ø®∆¨÷˜øÿ√‹‘ø
        protected static byte[] m_KeyMain = new byte[16];
        //MFœ¬ø®∆¨Œ¨ª§√‹‘ø
        protected static byte[] m_KeyMaintain = new byte[] { 0xF2, 0x1B, 0x12, 0x34, 0x04, 0x38, 0x30, 0xD4, 0x48, 0x29, 0x3E, 0x66, 0x36, 0x88, 0x33, 0x78 };
        //ø®∆¨÷–≥ı º√‹‘ø
        protected static byte[] m_KeyOrg = new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f };

        //////////////////////////////////////////////////////////////////////////
        //PSAMø®µƒMFœ¬ø®∆¨÷˜øÿ√‹‘ø
        protected static byte[] m_KeyPsamMain = new byte[16];
        //PSAMø®µƒMFœ¬ø®∆¨Œ¨ª§√‹‘ø
        protected static byte[] m_KeyPsamMaintain = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //PSAMø® ≥ı º√‹‘ø
        protected static byte[] m_PsamKeyOrg = new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f };

        protected SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        public DaHuaCardCtrlBase()
        {
        }

        public void SetOrgKeyValue(byte[] byteKey, CardCategory eCategory)
        {
            if (byteKey.Length != 16)
                return;
            if (eCategory == CardCategory.CpuCard)
                Buffer.BlockCopy(byteKey, 0, m_KeyOrg, 0, 16);
            else if (eCategory == CardCategory.PsamCard)
                Buffer.BlockCopy(byteKey, 0, m_PsamKeyOrg, 0, 16);
        }

        public void SetMainKeyValue(byte[] byteKey, CardCategory eCategory)
        {
            if (byteKey.Length != 16)
                return;
            if (eCategory == CardCategory.CpuCard)
                Buffer.BlockCopy(byteKey, 0, m_KeyMain, 0, 16);
            else if (eCategory == CardCategory.PsamCard)
                Buffer.BlockCopy(byteKey, 0, m_KeyPsamMain, 0, 16);
        }

        public void SetMaintainKeyValue(byte[] byteKey, CardCategory eCategory)
        {
            if (byteKey.Length != 16)
                return;
            if (eCategory == CardCategory.CpuCard)
                Buffer.BlockCopy(byteKey, 0, m_KeyMaintain, 0, 16);
            else if (eCategory == CardCategory.PsamCard)
                Buffer.BlockCopy(byteKey, 0, m_KeyPsamMaintain, 0, 16);
        }

        public byte[] GetKeyVal(bool bMainKey, CardCategory eCategory)
        {
            byte[] key = null;
            if (bMainKey)
            {
                if (eCategory == CardCategory.CpuCard)
                    key = m_KeyMain;
                else if (eCategory == CardCategory.PsamCard)
                    key = m_KeyPsamMain;
            }
            else
            {
                if (eCategory == CardCategory.CpuCard)
                    key = m_KeyOrg;
                else if (eCategory == CardCategory.PsamCard)
                    key = m_PsamKeyOrg;
            }
            return key;
        }

    }
}
