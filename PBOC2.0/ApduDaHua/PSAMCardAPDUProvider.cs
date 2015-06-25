using System;
using System.Collections.Generic;
using System.Text;
using ApduParam;
using ApduInterface;

namespace ApduDaHua
{
    public class PSAMCardAPDUProvider : ApduDaHuaBase, ISamApduProvider
    {
        public PSAMCardAPDUProvider()
        {

        }

        public bool createGenerateKeyCmd(ushort RecordCount, byte RecordLength)
        {
            m_CLA = 0x80;
            m_INS = 0xF5;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 9;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            //File ID
            m_Data[0] = 0xFE;
            m_Data[1] = 0x01;
            //File Type
            m_Data[2] = 0x8C;
            //Record Number
            m_Data[3] = (byte)((RecordCount >> 8) & 0xff);
            m_Data[4] = (byte)(RecordCount & 0xff);
            //Record Length
            m_Data[5] = RecordLength;
            //ACr
            m_Data[6] = 0x00;
            //ACw
            m_Data[7] = 0x00;
            //ACu
            m_Data[8] = 0x40;//密文+MAC
            m_le = 0;
            m_nTotalLen = 14;
            return true;
        }

        public bool createStorageFCICmd(byte[] byteName, byte[] prefix)
        {
            if (byteName == null || byteName.Length < 5 || byteName.Length > 16)
                return false;
            int nNameLen = byteName.Length;
            m_CLA = 0x80;
            m_INS = 0xE2;
            m_P1 = 0x00;
            m_P2 = 0x01;
            int nLen = nNameLen + 22;
            if (prefix != null)
                nLen += prefix.Length;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = 0x1E;
            m_Data[1] = 0x01;
            m_Data[2] = 0x21;
            m_Data[3] = 0x20;
            //Tag
            m_Data[4] = 0x6F;
            //Len
            int nLen6F = nNameLen + 16; //后面所有长度
            if (prefix != null)
                nLen6F += prefix.Length;
            m_Data[5] = (byte)nLen6F;
            //Tag
            m_Data[6] = 0x84;
            //Len
            int nLen84 = nNameLen;
            if (prefix != null)
                nLen84 += prefix.Length;
            m_Data[7] = (byte)nLen84; //Name长度（包含A0 00 00 00 03）
            int nOffset = 8;
            if (prefix != null)
            {
                Buffer.BlockCopy(prefix, 0, m_Data, nOffset, prefix.Length);
                nOffset += prefix.Length;
            }            
            Buffer.BlockCopy(byteName, 0, m_Data, nOffset, nNameLen);
            nOffset += nNameLen;
            //Tag
            m_Data[nOffset] = 0xA5;
            //Length
            m_Data[nOffset+1] = 0x09;
            //tag
            m_Data[nOffset+2] = 0x9F;
            m_Data[nOffset+3] = 0x08;
            //应用版本号
            m_Data[nOffset+4] = 0x01;
            m_Data[nOffset+5] = 0x01;
            //tag
            m_Data[nOffset+6] = 0xBF;
            m_Data[nOffset+7] = 0x0C;
            //自定义数据
            m_Data[nOffset+8] = 0x02;
            m_Data[nOffset+9] = 0x55;
            m_Data[nOffset+10] = 0x66;
            //tag
            m_Data[nOffset+11] = 0x88;
            //目录基本文件的SFI
            m_Data[nOffset+12] = 0x01;
            m_Data[nOffset+13] = 0x01;            
            nOffset += 14;
            //}}
            m_le = 0;
            m_nTotalLen = 5 + nLen; //APDU Len
            return true;
        }

        public bool createStorageCardInfoCmd(byte[] byteASN)
        {
            if (byteASN.Length != 8)
                return false;
            m_CLA = 0x00;
            m_INS = 0xD6;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen  = 14;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            //序列号，前面补2字节0
            m_Data[0] = 0x00;
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
            m_INS = 0xF5;
            m_P1 = GenerateFlag;
            m_P2 = 0x00;
            int nLen = 10;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            //FileId
            m_Data[0] = (byte)((FileId >> 8) & 0xff);
            m_Data[1] = (byte)(FileId & 0xff);
            //file Type
            m_Data[2] = FileType; 
            //file Size
            m_Data[3] = (byte)((FileSize >> 8) & 0xff);
            m_Data[4] = (byte)(FileSize & 0xff);
            //Data
            m_Data[5] = 0x00;
            if (FileId == 0xFF01 || FileId == 0xFF03)
            {
                m_Data[6] = 0xFF;
                m_Data[7] = 0xFF;
            }
            else if (FileId == 0x0018)
            {
                m_Data[6] = 0x00;
                m_Data[7] = 0x01;
            }
            else
            {
                m_Data[6] = 0x00;
                m_Data[7] = 0x00;
            }
            m_Data[8] = 0x00;
            m_Data[9] = 0x00;
            m_le = 0;
            m_nTotalLen = 15;
            return true;
        }

        public bool createSelectEFCmd(ushort FileId)
        {
            m_CLA = 0x00;
            m_INS = 0xA4;
            m_P1 = 0x02;
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
            m_P1 = 0x00;
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
            if (byteName.Length < 5 || byteName.Length > 16)
                return false;
            int nNameLen = byteName.Length;
            m_CLA = 0x80;
            m_INS = 0xE6;
            m_P1 = 0x08;
            m_P2 = 0x00;
            int nLen = 13 + nNameLen;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            //File ID
            m_Data[0] = (byte)((FileId >> 8) & 0xff);
            m_Data[1] = (byte)(FileId & 0xff);
            //File Type
            m_Data[2] = 0x20;
            //AC
            m_Data[3] = 0x00;
            //应用类型
            m_Data[4] = 0x01;
            //FCI索引
            m_Data[5] = 0x00;
            //外部认证
            m_Data[6] = 0x00;
            //前3bit: SM+后6bit:Length
            m_Data[7] = 0x2E;  //SM：密文+MAC
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
            m_P1 = 0x00;
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
            m_CLA = 0x00;
            m_INS = 0xD6;
            m_P1 = 0x00;
            m_P2 = 0x00;            
            m_Lc = 2;
            m_Data = new byte[2];
            //全国消费密钥索引
            m_Data[0] = 0x0A;
            m_Data[1] = 0x0A;
            m_le = 0;
            m_nTotalLen = 7;
            return true;
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
            bufferData[0] = 0x13;
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
            srcMacData[0] = m_CLA;//不需要后半字节为4
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
            if (nLen != 28)
                return false;
            m_CLA = 0xE0;
            m_INS = 0x40;
            m_P1 = 0x00;
            m_P2 = 0x00;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(DataVal, 0, m_Data, 0, 28);
            m_le = 0x0C;
            m_nTotalLen = 34;
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
    }
}
