using System;
using System.Collections.Generic;
using System.Text;
using ApduParam;
using ApduInterface;

namespace ApduDaHua
{
    public class ApduDaHuaBase : IApduBase
    {
        private static byte[] DEFAULT_MF_NAME = new byte[] { 0x31, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46, 0x30, 0x31 };//"1PAY.SYS.DDF01"        
        public ApduDaHuaBase()
        {

        }
        protected int m_nTotalLen = 0;
        /// <summary>
        /// APDU命令
        /// </summary>
        protected byte m_CLA = 0;
        protected byte m_INS = 0;
        protected byte m_P1 = 0;
        protected byte m_P2 = 0;
        protected byte m_Lc = 0;
        protected byte[] m_Data = null;
        protected byte m_le = 0;

        public byte[] GetOutputCmd()
        {
            byte[] outByte = new byte[m_nTotalLen];
            outByte[0] = m_CLA;
            outByte[1] = m_INS;
            outByte[2] = m_P1;
            outByte[3] = m_P2;
            int nOffset = 4;
            if (m_Lc > 0)
            {
                outByte[nOffset] = m_Lc;//m_Data不存在时，m_Lc也不存在
                nOffset++;
                Buffer.BlockCopy(m_Data, 0, outByte, nOffset, m_Lc);
                nOffset += m_Lc;
            }
            if (m_nTotalLen > nOffset)
                outByte[nOffset] = m_le;
            return outByte;
        }

        //选择
        public bool createSelectCmd(byte[] byteName, byte[] prefixData)
        {
            if (byteName == null || byteName.Length < 5 || byteName.Length > 16)
                return false;
            bool bMF = false;
            if (prefixData == null && ByteDataEquals(byteName, DEFAULT_MF_NAME))
                bMF = true;
            m_CLA = 0x00;
            m_INS = 0xA4;
            m_P1 = 0x04;
            m_P2 = 0x00;
            int nLen = byteName.Length;
            if (!bMF)
            {
                nLen += prefixData.Length;
            }
            m_Lc = (byte)nLen; //1. MF 0x0e   / 2. ADF name.length + prefixData.length        
            m_Data = new byte[nLen];
            if (bMF)
            {
                Buffer.BlockCopy(byteName, 0, m_Data, 0, byteName.Length);
            }
            else
            {
                for (int i = 0; i < prefixData.Length; i++)
                {
                    m_Data[i] = prefixData[i];
                }
                Buffer.BlockCopy(byteName, 0, m_Data, prefixData.Length, byteName.Length);
            }
            m_le = 0;
            m_nTotalLen = 5 + nLen;
            return true;
        }

        //取随机数
        public bool createGetChallengeCmd(int nRandLen)
        {
            if (nRandLen != 4 && nRandLen != 8)
                return false;
            m_CLA = 0x00;
            m_INS = 0x84;
            m_P1 = 0x00;
            m_P2 = 0x00;
            m_Lc = 0x00;  //不存在
            m_Data = null; //不存在
            m_le = (byte)nRandLen;   //指定随机数字节
            m_nTotalLen = 5;
            return true;
        }

        //COS版本
        public bool createCosVersionCmd()
        {
            m_CLA = 0x00;
            m_INS = 0xCA;
            m_P1 = 0x9F;
            m_P2 = 0x80;
            m_Lc = 0x00;  //不存在
            m_Data = null; //不存在
            m_le = 3;
            m_nTotalLen = 5;
            return true;
        }

        //外部认证
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteRandom">随机数,每个命令随机数只使用1次</param>        
        /// <param name="KeyValue">使用的密钥:初始密钥/主控密钥</param>
        /// <returns></returns>
        public bool createExternalAuthenticationCmd(byte[] byteRandom, byte[] KeyValue)
        {
            if (byteRandom.Length != 4 && byteRandom.Length != 8)
                return false;

            m_CLA = 0x00;
            m_INS = 0x82;
            m_P1 = 0x00;   //无安全信息
            m_P2 = 0x00;
            m_Lc = 0x08;

            //byteRandom.Length==4时后4字节为0x00，加密后得到认证数据
            byte[] baseData = new byte[8];
            Buffer.BlockCopy(byteRandom, 0, baseData, 0, byteRandom.Length);
            byte[] cryptData = DesCryptography.TripleEncryptData(baseData, KeyValue);

            m_Data = new byte[8];//密文
            Buffer.BlockCopy(cryptData, 0, m_Data, 0, 8);
            m_le = 0;
            m_nTotalLen = 13;
            return true;
        }

        /// <summary>
        /// 删除MF
        /// </summary>
        /// <param name="byteRandom">随机数,每个命令随机数只使用1次</param>
        /// <param name="KeyValue">使用的密钥:初始密钥/主控密钥</param>
        /// <returns></returns>
        public bool createClearMFcmd(byte[] byteRandom, byte[] KeyValue)
        {
            if (byteRandom.Length != 4 && byteRandom.Length != 8)
                return false;

            m_CLA = 0x80;
            m_INS = 0xE4;
            m_P1 = 0x00;
            m_P2 = 0x00;
            m_Lc = 0x08;

            //byteRandom.Length==4时后4字节为0x00，加密后得到认证数据
            byte[] baseData = new byte[8];
            Buffer.BlockCopy(byteRandom, 0, baseData, 0, byteRandom.Length);
            byte[] cryptData = DesCryptography.TripleEncryptData(baseData, KeyValue);

            m_Data = new byte[8];//密文
            Buffer.BlockCopy(cryptData, 0, m_Data, 0, 8);
            m_le = 0;
            m_nTotalLen = 13;
            return true;
        }

        public bool createGetEFFileCmd(byte fileFlag, byte ReponseLen)
        {
            m_CLA = 0x00;
            m_INS = 0xB0;
            m_P1 = fileFlag; //短文件标识符读文件
            m_P2 = 0x00;
            m_Lc = 0x00;  //不存在
            m_Data = null; //不存在
            m_le = ReponseLen;   //返回长度
            m_nTotalLen = 5;
            return true;
        }

        //计算命令数据的MAC值作为传输正确性验证
        protected byte[] CalcMACValue(byte[] srcData, byte[] KeyValue, byte[] InitVal)
        {
            byte[] MacResult = new byte[8];//EncryptData，DecryptData计算中会修改该值

            Buffer.BlockCopy(InitVal, 0, MacResult, 0, 8);

            int nAppendLen = 8 - (srcData.Length % 8);
            int nSrcLen = srcData.Length + nAppendLen;
            byte[] calcData = new byte[nSrcLen];
            Buffer.BlockCopy(srcData, 0, calcData, 0, srcData.Length);
            for (int i = 0; i < nAppendLen; i++)
            {
                if (i == 0)
                    calcData[srcData.Length + i] = 0x80;
                else
                    calcData[srcData.Length + i] = 0x00;
            }

            byte[] MacData = new byte[8];

            byte[] KeyLeft = new byte[8];
            byte[] KeyRight = new byte[8];
            Buffer.BlockCopy(KeyValue, 0, KeyLeft, 0, 8);
            Buffer.BlockCopy(KeyValue, 8, KeyRight, 0, 8);

            int nCalcOffset = 0;
            int nCalcCount = nSrcLen / 8;//必然能整除
            for (int nCalcIndex = 0; nCalcIndex < nCalcCount; nCalcIndex++)
            {
                Buffer.BlockCopy(calcData, nCalcOffset, MacData, 0, 8);
                for (int i = 0; i < 8; i++)
                {
                    MacResult[i] ^= MacData[i];//异或运算                
                }
                DesCryptography.EncryptData(MacResult, KeyLeft);//用密钥的前半部来进行加密操作
                nCalcOffset += 8;
            }
            //使用的是8字节密钥，最后一个数据块的处理
            DesCryptography.DecryptData(MacResult, KeyRight);//用密钥的后半部来进行解密操作
            DesCryptography.EncryptData(MacResult, KeyLeft);//用前半部加密

            byte[] MACData = new byte[4];
            Buffer.BlockCopy(MacResult, 0, MACData, 0, 4);
            return MACData;

        }

        //用过程密钥计算MAC1，MAC2值
        public byte[] CalcMacVal(byte[] srcData, byte[] keytoMac)
        {
            byte[] CryptData = SingleDesCalc(srcData, keytoMac);

            byte[] MACData = new byte[4];
            Buffer.BlockCopy(CryptData, 0, MACData, 0, 4);
            return MACData;
        }

        public byte[] CalcPrivateProcessKey(byte[] srcData, byte[] tmpck)
        {
            if (srcData.Length != 4)
                return null;
            //srcData是4字节，后面需要补“0x80 0x00 0x00 0x00”
            return SingleDesCalc(srcData, tmpck);
        }

        private byte[] SingleDesCalc(byte[] srcData, byte[] KeyValue)
        {
            byte[] MacResult = new byte[8];//初始值全0
            int nAppendLen = 8 - (srcData.Length % 8);
            int nSrcLen = srcData.Length + nAppendLen;
            byte[] calcData = new byte[nSrcLen];
            Buffer.BlockCopy(srcData, 0, calcData, 0, srcData.Length);
            for (int i = 0; i < nAppendLen; i++)
            {
                if (i == 0)
                    calcData[srcData.Length + i] = 0x80;
                else
                    calcData[srcData.Length + i] = 0x00;
            }

            byte[] MacData = new byte[8];

            int nCalcOffset = 0;
            int nCalcCount = nSrcLen / 8;//必然能整除
            for (int nCalcIndex = 0; nCalcIndex < nCalcCount; nCalcIndex++)
            {
                Buffer.BlockCopy(calcData, nCalcOffset, MacData, 0, 8);
                for (int i = 0; i < 8; i++)
                {
                    MacResult[i] ^= MacData[i];//异或运算                
                }
                DesCryptography.EncryptData(MacResult, KeyValue);//进行加密操作
                nCalcOffset += 8;
            }

            return MacResult;
        }

        protected byte[] GetBCDDate(DateTime dateData)
        {
            byte[] byteReturn = new byte[4];
            string strDate = dateData.ToString("yyyyMMdd");
            for (int i = 0; i < 4; i++)
            {
                byteReturn[i] = Convert.ToByte(strDate.Substring(i * 2, 2), 16);
            }
            return byteReturn;
        }

        protected byte[] StringToBCD(string strData)
        {
            if (string.IsNullOrEmpty(strData) || strData.Length % 2 != 0)
                return null;
            int nByteSize = strData.Length / 2;
            byte[] byteBCD = new byte[nByteSize];
            for (int i = 0; i < nByteSize; i++)
            {
                byte bcdbyte = 0;
                byte.TryParse(strData.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber, null, out bcdbyte);
                byteBCD[i] = bcdbyte;
            }
            return byteBCD;
        }

        protected bool ByteDataEquals(byte[] byteL, byte[] byteR)
        {
            if (byteL.Length != byteR.Length)
                return false;
            for (int i = 0; i < byteL.Length; i++)
            {
                if (byteL[i] != byteR[i])
                    return false;
            }
            return true;
        }

    }
}

