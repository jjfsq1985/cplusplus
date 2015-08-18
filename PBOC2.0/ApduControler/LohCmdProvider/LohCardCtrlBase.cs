using System;
using System.Collections.Generic;
using System.Text;
using IFuncPlugin;
using System.Data.SqlClient;
using System.Data;
using SqlServerHelper;
using ApduParam;
using ApduCtrl;
using System.Xml;
using ApduInterface;

namespace LohApduCtrl
{
    public class LohCardCtrlBase
    {
        //MFÏÂ¿¨Æ¬Ö÷¿ØÃÜÔ¿
        protected static byte[] m_KeyMain = new byte[] { 0x57, 0x41, 0x54, 0x43, 0x48, 0x44, 0x41, 0x54, 0x41, 0x54, 0x69, 0x6d, 0x65, 0x43, 0x4f, 0x53 };
        //MFÏÂ¿¨Æ¬Î¬»¤ÃÜÔ¿
        protected static byte[] m_KeyMaintain = new byte[] { 0x57, 0x41, 0x54, 0x43, 0x48, 0x44, 0x41, 0x54, 0x41, 0x54, 0x69, 0x6d, 0x65, 0x43, 0x4f, 0x53 };

        //////////////////////////////////////////////////////////////////////////
        //PSAM¿¨µÄMFÏÂ¿¨Æ¬Ö÷¿ØÃÜÔ¿
        protected static byte[] m_KeyPsamMain = new byte[] { 0x57, 0x41, 0x54, 0x43, 0x48, 0x44, 0x41, 0x54, 0x41, 0x54, 0x69, 0x6d, 0x65, 0x43, 0x4f, 0x53 };
        //PSAM¿¨µÄMFÏÂ¿¨Æ¬Î¬»¤ÃÜÔ¿
        protected static byte[] m_KeyPsamMaintain = new byte[] { 0x57, 0x41, 0x54, 0x43, 0x48, 0x44, 0x41, 0x54, 0x41, 0x54, 0x69, 0x6d, 0x65, 0x43, 0x4f, 0x53 };

        protected SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        public LohCardCtrlBase()
        {

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

        public byte[] CardKeyToDb(bool bOrg, CardCategory eCategory)
        {
            if (eCategory == CardCategory.CpuCard)
                return m_KeyMain;
            else if (eCategory == CardCategory.PsamCard)
                return m_KeyPsamMain;
            else
                return null;
        }

        public byte[] GetKeyVal(bool bMainKey, CardCategory eCategory)
        {
            byte[] key = null;

            if (eCategory == CardCategory.CpuCard)
                key = m_KeyMain;
            else if (eCategory == CardCategory.PsamCard)
                key = m_KeyPsamMain;
     
            return key;
        }        
    }
}
