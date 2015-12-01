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
        //¥Ôª™ø®…Ã≥ı º√‹‘ø  -------{0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f}
        //MFœ¬ø®∆¨÷˜øÿ√‹‘ø
        protected byte[] m_KeyMain = new byte[16];
        //MFœ¬ø®∆¨Œ¨ª§√‹‘ø
        protected byte[] m_KeyMaintain = new byte[16];
        //ø®∆¨÷–≥ı º√‹‘ø
        protected byte[] m_KeyOrg = new byte[16];

        //////////////////////////////////////////////////////////////////////////
        //PSAMø®µƒMFœ¬ø®∆¨÷˜øÿ√‹‘ø
        protected byte[] m_KeyPsamMain = new byte[16];
        //PSAMø®µƒMFœ¬ø®∆¨Œ¨ª§√‹‘ø
        protected byte[] m_KeyPsamMaintain = new byte[16];
        //PSAMø® ≥ı º√‹‘ø
        protected byte[] m_PsamKeyOrg = new byte[16];

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
