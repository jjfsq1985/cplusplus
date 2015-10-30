using System;
using System.Collections.Generic;
using System.Text;
using ApduParam;
using ApduInterface;

namespace ApduLoh
{
    public class LohPsamApduProvider : APDULohBase, ISamApduProvider
    {
        public LohPsamApduProvider()
        {

        }

        public bool createGenerateKeyCmd(ushort uFileId, ushort RecordCount, byte RecordLength)
        {
            m_CLA = 0x80;
            m_INS = 0xE0;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 7;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = (byte)((uFileId >> 8) & 0xff);
            m_Data[1] = (byte)(uFileId & 0xff);
            m_Data[2] = 0x48;
            m_Data[3] = 0x01;
            m_Data[4] = 0xF0;
            m_Data[5] = 0xFF;
            m_Data[6] = 0xFF;
            m_le = 0;
            m_nTotalLen = 12;
            return true;
        }
        
        //卡片主控和维护密钥 的明文导入
        public bool createStorageKeyCmd(byte[] keyVal, byte[] param1, byte[] param2)
        {
            if (param1.Length != 2 || param2.Length != 5)
                return false;
            m_CLA = 0x80;
            m_INS = 0xD4;
            m_P1 = param1[0];
            m_P2 = param1[1];
            int nLen = 21;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(param2, 0, m_Data, 0, 5);
            Buffer.BlockCopy(keyVal, 0, m_Data, 5, 16);
            m_le = 0;
            m_nTotalLen = 5 + nLen;
            return true;
        }

        public bool createStorageFCICmd(byte[] byteName, byte[] prefix)
        {
            return false;//80e0
        }

        public bool createStorageCardInfoCmd(byte[] byteASN)
        {
            if (byteASN.Length != 8)
                return false;
            m_CLA = 0x00;
            m_INS = 0xD6;
            m_P1 = 0x95;
            m_P2 = 0x00;
            int nLen  = 14;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            //序列号，前面补2字节
            m_Data[0] = 0x01;
            m_Data[1] = 0x00;
            Buffer.BlockCopy(byteASN, 0, m_Data, 2, 8);
            m_Data[10] = 0x01;//PSAM版本号
            m_Data[11] = 0x01;//密钥卡类型
            //发卡方FCI自定义数据
            m_Data[12] = 0x00; 
            m_Data[13] = 0x00;
            m_le = 0;
            m_nTotalLen = 19;
            return true;
        }

        /// <summary>
        /// 创建EF文件
        /// </summary>
        /// <param name="bGenerateFlag">P1</param>
        /// <param name="FileId">ID</param>
        /// <param name="FileType">Type</param>
        /// <param name="FileSize">Len</param>
        /// <returns>true</returns>
        public bool createGenerateEFCmd(byte GenerateFlag, ushort FileId, byte FileType, ushort FileSize)
        {
            m_CLA = 0x80;
            m_INS = 0xE0;
            m_P1 = (byte)((FileId >> 8) & 0xff);
            m_P2 = (byte)(FileId & 0xff);
            int nLen = 7;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = FileType;
            m_Data[1] = (byte)((FileSize >> 8) & 0xff);
            m_Data[2] = (byte)(FileSize & 0xff); 
            m_Data[3] = 0xF0;
            m_Data[4] = 0xF0;
            m_Data[5] = 0xFF;
            m_Data[6] = 0xFF;
            m_le = 0;
            m_nTotalLen = 12;
            return true;
        }

        public bool createSelectEFCmd(ushort FileId)
        {
            m_CLA = 0x00;
            m_INS = 0xA4;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 2;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            //FileId
            m_Data[0] = (byte)((FileId >> 8) & 0xff);
            m_Data[1] = (byte)(FileId & 0xff);
            m_le = 0;
            m_nTotalLen = 7;
            return true;
        }

        public bool createStorageTermInfoCmd(byte[] TermialId)
        {
            if (TermialId.Length != 6)
                return false;
            m_CLA = 0x00;
            m_INS = 0xD6;
            m_P1 = 0x96;
            m_P2 = 0x00;
            int nLen = 6;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(TermialId, 0, m_Data, 0, 6);
            m_le = 0;
            m_nTotalLen = 11;
            return true;
        }

        public bool createGenerateADFCmd(byte[] byteName, ushort FileId)
        {
            if (byteName == null || byteName.Length < 5 || byteName.Length > 16)
                return false;
            int nNameLen = byteName.Length;
            m_CLA = 0x80;
            m_INS = 0xE0;
            m_P1 = (byte)((FileId >> 8) & 0xff);
            m_P2 = (byte)(FileId & 0xff);
            int nLen = 13 + nNameLen;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = 0x38;
            m_Data[1] = 0x05;                                    
            m_Data[2] = 0x00;
            m_Data[3] = 0xAA;
            m_Data[4] = 0xAA;
            m_Data[5] = 0xFF;
            m_Data[6] = 0xFF;
            m_Data[7] = 0xFF;

            m_Data[8] = 0xA0;
            m_Data[9] = 0x00;
            m_Data[10] = 0x00;
            m_Data[11] = 0x00;
            m_Data[12] = 0x03;            
            Buffer.BlockCopy(byteName, 0, m_Data, 13, nNameLen);
            m_le = 0;
            m_nTotalLen = 5 + nLen;
            return true;
        }

        public bool createStoragePsamInfoCmd(IccCardInfoParam PsamInfo)
        {
            m_CLA = 0x00;
            m_INS = 0xD6;
            m_P1 = 0x97;
            m_P2 = 0x00;
            int nLen = 25;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            //全国消费密钥索引
            m_Data[0] = 0x01;
            Buffer.BlockCopy(PsamInfo.GetByteCompanyIssue(), 0, m_Data, 1, 8);
            Buffer.BlockCopy(PsamInfo.GetByteCompanyRecv(), 0, m_Data, 9, 8);
            byte[] byteFrom = GetBCDDate(PsamInfo.ValidAppForm);
            Buffer.BlockCopy(byteFrom, 0, m_Data, 17, 4);
            byte[] byteTo = GetBCDDate(PsamInfo.ValidAppTo);
            Buffer.BlockCopy(byteTo, 0, m_Data, 21, 4);
            m_le = 0;
            m_nTotalLen = 30;
            return true;
        }

        public bool createWriteMAC2Cmd(byte maxCount, byte remainCount)
        {
            return false;
        }

        public bool createSetStatusCmd(byte[] RandomVal, byte[] keyCalc)
        {
            if (RandomVal.Length != 8)
                return false;
            m_CLA = 0x80;
            m_INS = 0xF1;
            m_P1 = 0x00;
            m_P2 = 0x00;
            m_Lc = 0x08;
            byte[] cryptData = DesCryptography.TripleEncryptData(RandomVal, keyCalc);
            m_Data = new byte[8];
            Buffer.BlockCopy(cryptData, 0, m_Data, 0, 8);
            m_le = 0;
            m_nTotalLen = 13;
            return true;
        }

        public bool createStorageAppKeyCmd(byte[] RandomVal, byte[] StorageKey, byte Usage, byte Ver, byte[] EncryptKey)
        {
            if (RandomVal.Length != 8 || StorageKey.Length != 16)
                return false;
            m_CLA = 0x84;
            m_INS = 0xD4;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 28;
            m_Lc = (byte)nLen;

            byte[] bufferData = new byte[24];
            bufferData[0] = 0x13; //明文长度
            bufferData[1] = Usage;
            bufferData[2] = Ver;
            bufferData[3] = 0x00;
            Buffer.BlockCopy(StorageKey, 0, bufferData, 4, 16);
            //补齐24字节计算密文
            int nAppendLen = 4;
            for (int i = 0; i < nAppendLen; i++)
            {
                if (i == 0)
                    bufferData[20 + i] = 0x80;
                else
                    bufferData[20 + i] = 0x00;
            }
            byte[] cryptData = DesCryptography.TripleEncryptData(bufferData, EncryptKey);//用原始密钥加密
            m_Data = new byte[nLen];//密文 + MAC
            Buffer.BlockCopy(cryptData, 0, m_Data, 0, 24);
            byte[] srcMacData = new byte[29]; //头5 +密文24
            srcMacData[0] = m_CLA;
            srcMacData[1] = m_INS;
            srcMacData[2] = m_P1;
            srcMacData[3] = m_P2;
            srcMacData[4] = m_Lc;
            Buffer.BlockCopy(cryptData, 0, srcMacData, 5, 24);
            byte[] byteMAC = CalcMACValue(srcMacData, EncryptKey, RandomVal);//原始密钥计算MAC
            Buffer.BlockCopy(byteMAC, 0, m_Data, 24, 4);
            m_le = 0;
            m_nTotalLen = 5 + nLen;
            return true;
        }

        public bool createInitSamGrayLockCmd(byte[] DataVal)
        {
            int nLen = DataVal.Length;
            if (nLen != 36)
                return false;
            m_CLA = 0xE0;
            m_INS = 0x40;
            m_P1 = 0x00;
            m_P2 = 0x00;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(DataVal, 0, m_Data, 0, 36);
            m_le = 0x0C;
            m_nTotalLen = 42;
            return true;
        }

        public bool createInitSamPurchaseCmd(byte[] DataVal)
        {
            int nLen = DataVal.Length;
            if (nLen != 36)
                return false;
            m_CLA = 0x80;
            m_INS = 0x70;
            m_P1 = 0x00;
            m_P2 = 0x00;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(DataVal, 0, m_Data, 0, 36);
            m_le = 0x0C;
            m_nTotalLen = 42;
            return true;
        }

        public bool createVerifyMAC2Cmd(byte[] MAC2)
        {
            m_CLA = 0xE0;
            m_INS = 0x42;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 4;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(MAC2, 0, m_Data, 0, 4);
            m_le = 0;
            m_nTotalLen = 9;
            return true;
        }

        public bool createVerifyPurchaseMAC2Cmd(byte[] MAC2)
        {
            m_CLA = 0x80;
            m_INS = 0x72;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 4;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(MAC2, 0, m_Data, 0, 4);
            m_le = 0;
            m_nTotalLen = 9;
            return true;
        }

        public bool createCalcGMACCmd(byte BusinessType,byte[] ASN, int nOfflineSn, int nAmount)
        {
            m_CLA = 0xE0;
            m_INS = 0x44;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 15;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = BusinessType;
            Buffer.BlockCopy(ASN, 0, m_Data, 1, 8);
            m_Data[9] = (byte)((nOfflineSn >> 8) & 0xFF);
            m_Data[10] = (byte)(nOfflineSn & 0xFF);
            byte[] byteMoney = BitConverter.GetBytes(nAmount); //气票消费金额
            m_Data[11] = byteMoney[3];
            m_Data[12] = byteMoney[2];
            m_Data[13] = byteMoney[1];
            m_Data[14] = byteMoney[0];            
            m_le = 8;
            m_nTotalLen = 21;
            return true;            
        }

        public bool createInitDesCalcCmd(byte[] PsamAsn)
        {
            m_CLA = 0x80;
            m_INS = 0x1A;
            m_P1 = 0x08; //MAC、加密密钥
            m_P2 = 0x01; //密钥版本            
            m_Lc = 0;
            m_Data = null;
            m_le = 0;
            m_nTotalLen = 5;
            return true;
        }

        public bool createPsamDesCalcCmd(byte[] srcData)
        {
            m_CLA = 0x80;
            m_INS = 0xFA;
            m_P1 = 0x00;  //无后续块加密
            m_P2 = 0x00;
            int nLen = srcData.Length;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(srcData, 0, m_Data, 0, nLen);
            m_le = 0;
            m_nTotalLen = nLen + 5;
            return true;
        }
    }
}
