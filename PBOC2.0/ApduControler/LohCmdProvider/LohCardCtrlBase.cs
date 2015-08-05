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

namespace LohApduCtrl
{
    public class LohCardCtrlBase : ICardCtrlBase
    {
        //MF下卡片主控密钥
        protected static byte[] m_KeyMain = new byte[] { 0x57, 0x41, 0x54, 0x43, 0x48, 0x44, 0x41, 0x54, 0x41, 0x54, 0x69, 0x6d, 0x65, 0x43, 0x4f, 0x53 };
        //MF下卡片维护密钥
        protected static byte[] m_KeyMaintain = new byte[] { 0x57, 0x41, 0x54, 0x43, 0x48, 0x44, 0x41, 0x54, 0x41, 0x54, 0x69, 0x6d, 0x65, 0x43, 0x4f, 0x53 };

        //////////////////////////////////////////////////////////////////////////
        //PSAM卡的MF下卡片主控密钥
        protected static byte[] m_KeyPsamMain = new byte[] { 0x57, 0x41, 0x54, 0x43, 0x48, 0x44, 0x41, 0x54, 0x41, 0x54, 0x69, 0x6d, 0x65, 0x43, 0x4f, 0x53 };
        //PSAM卡的MF下卡片维护密钥
        protected static byte[] m_KeyPsamMaintain = new byte[] { 0x57, 0x41, 0x54, 0x43, 0x48, 0x44, 0x41, 0x54, 0x41, 0x54, 0x69, 0x6d, 0x65, 0x43, 0x4f, 0x53 };

        protected SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        public LohCardCtrlBase()
        {

        }

        protected string GetErrString(byte SW1, byte SW2, string strErrCode)
        {
            if (SW1 == 0x63 && (byte)(SW2 & 0xF0) == 0xC0)
            {
                int nRetry = (int)(SW2&0x0F);
                return string.Format("认证失败，剩余{0}次机会",nRetry);
            }
            else if (SW1 == 0x69 && SW2 == 0x83)
            {
                return "认证方法已锁";
            }
            else if (SW1 == 0x93 && SW2 == 0x03)
            {
                return "应用永久锁定";
            }
            return "其他错误：" + strErrCode;
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

        //计算过程密钥
        protected byte[] GetProcessKey(byte[] ASN, byte[] MasterKey, byte[] RandVal, byte[] byteSn)
        {
            if (ASN.Length != 8)
                return null;
            //过程密钥
            byte[] SubKey = new byte[16];
            byte[] LeftDiversify = DesCryptography.TripleEncryptData(ASN, MasterKey);
            byte[] XorASN = new byte[8];
            for (int i = 0; i < 8; i++)
                XorASN[i] = (byte)(ASN[i] ^ 0xFF);
            byte[] RightDiversify = DesCryptography.TripleEncryptData(XorASN, MasterKey);
            Buffer.BlockCopy(LeftDiversify, 0, SubKey, 0, 8);
            Buffer.BlockCopy(RightDiversify, 0, SubKey, 8, 8);
            byte[] byteData = new byte[8];
            Buffer.BlockCopy(RandVal, 0, byteData, 0, 4);
            Buffer.BlockCopy(byteSn, 0, byteData, 4, 2);
            byteData[6] = 0x80;
            byteData[7] = 0x00;
            byte[] byteRetKey = DesCryptography.TripleEncryptData(byteData, SubKey);
            return byteRetKey;
        }

        protected void StrKeyToByte(string strKey, byte[] byteKey)
        {
            byte[] BcdKey = PublicFunc.StringToBCD(strKey);
            if (BcdKey.Length == 16)
                Buffer.BlockCopy(BcdKey, 0, byteKey, 0, 16);
        }        
    }
}
