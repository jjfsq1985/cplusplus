using System;
using System.Collections.Generic;
using System.Text;
using IFuncPlugin;
using System.Data.SqlClient;
using System.Data;
using SqlServerHelper;
using ApduDaHua;

namespace CardOperating
{
    public class CardControlBase
    {
        public event MessageOutput TextOutput = null;

        //卡片种类
        public enum CardCategory
        {
            CpuCard,  //CPU卡
            PsamCard  //PSAM卡
        }

        //卡片中初始密钥
        protected static byte[] m_KeyOrg = new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f };

        //MF下卡片主控密钥
        protected static byte[] m_KeyMain = new byte[] { 0xF2, 0x1B, 0x12, 0x34, 0x04, 0x38, 0x30, 0xD4, 0x48, 0x29, 0x3E, 0x66, 0x36, 0x88, 0x33, 0x78 };

        //卡片应用主控密钥
        protected static byte[] m_KeyAppMain = new byte[] { 0xF2, 0x1B, 0x12, 0x34, 0x04, 0x38, 0x30, 0xD4, 0x48, 0x29, 0x3E, 0x66, 0x36, 0x88, 0x33, 0xCC };

        //////////////////////////////////////////////////////////////////////////
        //PSAM卡 初始密钥
        protected static byte[] m_PsamKeyOrg = new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f };

        //PSAM卡的MF下卡片主控密钥
        protected static byte[] m_KeyPsamMain = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };

        protected int m_MtDevHandler = 0;
        protected short m_RetVal = 0;  //返回值

        protected byte[] m_RecvData = new byte[128];
        protected byte[] m_RecvDataLen = new byte[4];

        //用于接收数值初始化
        protected readonly byte[] m_InitByte = new byte[128];
        protected SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        public CardControlBase()
        {

        }

        protected virtual void OnTextOutput(MsgOutEvent args)
        {
            if (this.TextOutput != null)
                this.TextOutput(args);
        }

        //获取当前BCD码格式的系统时间
        public static byte[] GetBCDTime()
        {
            string strTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            int nByteSize = strTime.Length / 2;
            byte[] byteBCD = new byte[nByteSize];
            for (int i = 0; i < nByteSize; i++)
            {
                byteBCD[i] = Convert.ToByte(strTime.Substring(i * 2, 2), 16);
            }
            return byteBCD;
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

        protected void SetOrgKeyValue(byte[] byteKey, CardCategory eCategory)
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

        public void SetUserAppKeyValue(byte[] byteKey)
        {
            if (byteKey.Length != 16)
                return;
            Buffer.BlockCopy(byteKey, 0, m_KeyAppMain, 0, 16);
        }

        public byte[] CardKeyToDb(bool bOrg, CardCategory eCategory)
        {
            if (bOrg)
            {
                if (eCategory == CardCategory.CpuCard)
                    return m_KeyOrg;
                else if (eCategory == CardCategory.PsamCard)
                    return m_PsamKeyOrg;
                else
                    return null;
            }
            else
            {
                if (eCategory == CardCategory.CpuCard)
                    return m_KeyMain;
                else if (eCategory == CardCategory.PsamCard)
                    return m_KeyPsamMain;
                else
                    return null;
            }
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

        //计算过程密钥
        protected byte[] GetProcessKey(byte[] ASN, byte[] MasterKey, byte[] RandVal, byte[] byteSn)
        {
            if (ASN.Length != 8)
                return null;
            //过程密钥
            byte[] SubKey = new byte[16];
            byte[] encryptAsn = APDUBase.TripleEncryptData(ASN, MasterKey);
            byte[] XorASN = new byte[8];
            for (int i = 0; i < 8; i++)
                XorASN[i] = (byte)(ASN[i] ^ 0xFF);
            byte[] encryptXorAsn = APDUBase.TripleEncryptData(XorASN, MasterKey);
            Buffer.BlockCopy(encryptAsn, 0, SubKey, 0, 8);
            Buffer.BlockCopy(encryptXorAsn, 0, SubKey, 8, 8);
            byte[] byteData = new byte[8];
            Buffer.BlockCopy(RandVal, 0, byteData, 0, 4);
            Buffer.BlockCopy(byteSn, 0, byteData, 4, 2);
            byteData[6] = 0x80;
            byteData[7] = 0x00;
            byte[] byteRetKey = APDUBase.TripleEncryptData(byteData, SubKey);
            return byteRetKey;
        }

        protected void StrKeyToByte(string strKey, byte[] byteKey)
        {
            byte[] BcdKey = APDUBase.StringToBCD(strKey);
            if (BcdKey.Length == 16)
                Buffer.BlockCopy(BcdKey, 0, byteKey, 0, 16);
        }

        protected byte[] GetRelatedKey(SqlHelper sqlHelp, CardCategory eCardType)
        {
            SqlDataReader dataReader = null;
            if (eCardType == CardCategory.PsamCard)
            {
                sqlHelp.ExecuteProc("PROC_GetPsamKey", out dataReader);
            }
            else
            {
                SqlParameter[] sqlparam = new SqlParameter[1];
                sqlparam[0] = sqlHelp.MakeParam("ApplicationIndex", SqlDbType.Int, 4, ParameterDirection.Input, 1);
                sqlHelp.ExecuteProc("PROC_GetCpuKey", sqlparam, out dataReader);
            }
            if (dataReader == null)
                return null;
            if (!dataReader.HasRows)
            {
                dataReader.Close();
                return null;
            }
            else
            {
                byte[] ConsumerKey = new byte[16];
                if (dataReader.Read())
                {
                    string strKey = (string)dataReader["ConsumerMasterKey"];
                    StrKeyToByte(strKey, ConsumerKey);
                }
                dataReader.Close();
                return ConsumerKey;
            }
        }
    }
}
