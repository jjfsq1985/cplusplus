using System;
using System.Collections.Generic;
using System.Text;

namespace ApduParam
{
    public class StorageKeyParam
    {
        private string m_strStorageName; //信息        
        private byte m_StorageIndex; //密钥编号
        private byte m_KeyPar1; //密钥类型
        private byte m_KeyPar2; //密钥索引号
        private byte m_KeyPar3; //错误计数器
        private byte m_KeyPar4;  //正确应用后的后继权限
        private byte[] m_ASN = null;  //卡号
        private byte[] m_XorASN = null; //异或后的卡号
        private byte[] m_StorageKey = null; //安装的Key,位于数据区
        private byte[] m_EncryptKey = null;  //用于加密的Key

        public string PromptInfo
        {
            get { return m_strStorageName; }
        }
        public byte P2
        {
            get { return m_StorageIndex; }
        }
        public byte KeyPar1
        {
            get { return m_KeyPar1; }
        }
        public byte KeyPar2
        {
            get { return m_KeyPar2; }
        }
        public byte KeyPar3
        {
            get { return m_KeyPar3; }
        }
        public byte KeyPar4
        {
            get { return m_KeyPar4; }
        }
        public byte[] ASN
        {
            get { return m_ASN; }
        }
        public byte[] XorASN
        {
            get { return m_XorASN; }
        }
        public byte[] StorageKey
        {
            get { return m_StorageKey; }
        }
        public byte[] EncryptKey
        {
            get { return m_EncryptKey; }
        }

        public StorageKeyParam(string strStorageName, byte StorageIndex, byte KeyPar1, byte KeyPar2, byte KeyPar3, byte KeyPar4)
        {
            m_strStorageName = strStorageName;
            m_StorageIndex = StorageIndex;
            m_KeyPar1 = KeyPar1;
            m_KeyPar2 = KeyPar2;
            m_KeyPar3 = KeyPar3;
            m_KeyPar4 = KeyPar4;
        }

        public void SetParam(byte[] ASN, byte[] StorageKey, byte[] EncryptKey)
        {
            if (ASN.Length < 8 || ASN.Length > 16)
                return;
            m_ASN = new byte[8];
            m_XorASN = new byte[8];
            int nOffset = ASN.Length - 8;
            Buffer.BlockCopy(ASN, nOffset, m_ASN, 0, 8);
            for (int i = 0; i < 8; i++)
                m_XorASN[i] = (byte)(m_ASN[i] ^ 0xFF);
            m_StorageKey = StorageKey;            
            m_EncryptKey = new byte[16];
            Buffer.BlockCopy(EncryptKey, 0, m_EncryptKey, 0, 16);//初始值为厂商密钥
        }

        public void SetDiversify(byte[] byteKey)
        {
            if (byteKey.Length != 16)
                return;
            m_EncryptKey = new byte[16];
            Buffer.BlockCopy(byteKey, 0, m_EncryptKey, 0, 16);//修改成分散密钥
        }

        public static byte[] GetDiversify(byte[] byteASN, byte[] KeyApp)
        {
            if (byteASN.Length < 8 || byteASN.Length > 16)
                return null;
            byte[] asn = new byte[8];
            byte[] xorasn = new byte[8];
            int nOffset = byteASN.Length - 8;
            Buffer.BlockCopy(byteASN, nOffset, asn, 0, 8);
            for (int i = 0; i < 8; i++)
                xorasn[i] = (byte)(asn[i] ^ 0xFF);
            //生成分散密钥
            byte[] keyDiversify = new byte[16];
            byte[] KeyLeft = DesCryptography.TripleEncryptData(asn, KeyApp);
            byte[] KeyRight = DesCryptography.TripleEncryptData(xorasn, KeyApp);
            Buffer.BlockCopy(KeyLeft, 0, keyDiversify, 0, 8);
            Buffer.BlockCopy(KeyRight, 0, keyDiversify, 8, 8);
            return keyDiversify;
        }

        public static byte[] GetUpdateEFKey(byte[] keyMAMK, byte[] byteASN)
        {
            if (byteASN.Length < 8 || byteASN.Length > 16)
                return null;
            byte[] asn = new byte[8];
            byte[] xorasn = new byte[8];
            int nOffset = byteASN.Length - 8;
            Buffer.BlockCopy(byteASN, nOffset, asn, 0, 8);
            for (int i = 0; i < 8; i++)
                xorasn[i] = (byte)(asn[i] ^ 0xFF);
            //生成分散密钥
            byte[] keyUpdate = new byte[16];
            byte[] KeyLeft = DesCryptography.TripleEncryptData(asn, keyMAMK);
            byte[] KeyRight = DesCryptography.TripleEncryptData(xorasn, keyMAMK);
            Buffer.BlockCopy(KeyLeft, 0, keyUpdate, 0, 8);
            Buffer.BlockCopy(KeyRight, 0, keyUpdate, 8, 8);
            return keyUpdate;
        }

    }    
}
