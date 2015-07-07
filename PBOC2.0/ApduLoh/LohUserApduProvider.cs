using System;
using System.Collections.Generic;
using System.Text;
using ApduParam;
using ApduInterface;

namespace ApduLoh
{
    public class LohUserApduProvider : APDULohBase , IUserApduProvider
    {
        public LohUserApduProvider()
        {

        }

        public bool createGenerateFCICmd()
        {
            m_CLA = 0x80;
            m_INS = 0xE0;
            m_P1 = 0x00;
            m_P2 = 0x01;
            int nLen = 7;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];            
            m_Data[0] = 0x2C;
            m_Data[1] = 0x00;            
            m_Data[2] = 0x30;            
            m_Data[3] = 0xF0;
            m_Data[4] = 0xAA;            
            m_Data[5] = 0xFF;            
            m_Data[6] = 0xFF;
            m_le = 0;
            m_nTotalLen = 12;
            return true;
        }

        public bool createStorageFCICmd(byte[] AidName, byte[] param, byte[] prefix)
        {
            return false;
        }

        //创建3F01文件(nFileIndex = 1, AidName = "86980701")
        //创建3F02文件(nFileIndex = 2, AidName = "86980702")
        public bool createUpdateEF01Cmd(byte nFileIndex, byte[] AidName)
        {
            m_CLA = 0x80;
            m_INS = 0xE0;
            m_P1 = 0x3F;
            m_P2 = nFileIndex;
            int nLen = 17;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = 0x38;
            m_Data[1] = 0x03;
            m_Data[2] = 0x86;
            m_Data[3] = 0xEF;
            m_Data[4] = 0xEF;
            m_Data[5] = 0xFF;
            m_Data[6] = 0xFF;
            m_Data[7] = 0xFF;
            //
            m_Data[8] = 0xA0;
            m_Data[9] = 0x00;
            m_Data[10] = 0x00;
            m_Data[11] = 0x00;
            m_Data[12] = 0x03;            
            Buffer.BlockCopy(AidName, 0, m_Data, 13, AidName.Length);
            m_le = 0;
            m_nTotalLen = 5 + nLen;
            return true;
        }

        public bool createGenerateKeyCmd()
        {
            m_CLA = 0x80;
            m_INS = 0xE0;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 7;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = 0x3F;
            m_Data[1] = 0x00;
            m_Data[2] = 0x48;
            m_Data[3] = 0x01;
            m_Data[4] = 0xF0;
            m_Data[5] = 0xFF;
            m_Data[6] = 0xFF;
            m_le = 0;
            m_nTotalLen = 12;
            return true;
        }

        //安装主控密钥
        public bool createStorageKeyCmd(byte[] StorageKey, byte[] Param1, byte[] Param2)
        {
            if (Param1.Length != 2 || Param2.Length != 5)
                return false;
            m_CLA = 0x80;
            m_INS = 0xD4;
            m_P1 = Param1[0];
            m_P2 = Param1[1];
            int nLen = 21;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(Param2, 0, m_Data, 0, 5);
            Buffer.BlockCopy(StorageKey, 0, m_Data, 5, 16);
            m_le = 0;
            m_nTotalLen = 5 + nLen;
            return true;
        }

        public bool createGenerateADFCmd(byte[] ADFName)
        {
            return false;
        }        

        /// <summary>
        ///  创建EF文件
        /// </summary>
        /// <param name="fileID">文件ID</param>
        /// <param name="fileType">类型，透明文件0x01</param>
        /// <param name="fileLen">文件大小，透明文件用</param>
        /// <param name="keyIndex">Key Index</param>
        /// <param name="RecordNum">记录数，记录文件用</param>
        /// <param name="RecordLen">记录长度，记录文件用</param>
        /// <param name="ACr">读权限</param>
        /// <param name="ACw">写权限</param>
        /// <returns></returns>
        public bool createGenerateEFCmd(ushort fileID, byte fileType, ushort fileLen, byte keyIndex, byte RecordNum, byte RecordLen, ushort ACr, ushort ACw)
        {
            m_CLA = 0x80;
            m_INS = 0xE0;
            m_P1 = 0x00;
            m_P2 = (byte)(fileID & 0xff);
            int nCmdLen = 7;
            m_Lc = (byte)nCmdLen;
            m_Data = new byte[nCmdLen];
            m_Data[0] = fileType;
            if (RecordNum == 0 && RecordLen == 0)
            {
                //透明文件/其他EF文件
                m_Data[1] = (byte)((fileLen >> 8) & 0xff);
                m_Data[2] = (byte)(fileLen & 0xff);
            }
            else
            {
                //记录文件
                m_Data[1] = RecordNum;
                m_Data[2] = RecordLen;
            }
            m_Data[3] = (byte)((ACr >> 8) & 0xff);
            m_Data[4] = (byte)(ACr & 0xff);
            m_Data[5] = (byte)((ACw >> 8) & 0xff);
            m_Data[6] = (byte)(ACw & 0xff);
            m_le = 0;
            m_nTotalLen = 12;
            return true;
        }

        public bool createStorageApplicationCmd()
        {
            return false;
        }

        public bool createStoragePINFileCmd(bool bDefaultPwd, byte[] customPwd)
        {
            return false;
        }        

        private byte XorValue(byte[] data)
        {
            byte byteRetVal = 0;
            for (int i = 0; i < data.Length; i++)
            {
                byteRetVal ^= data[i];
            }
            return byteRetVal;
        }

        //气票交易密钥
        public bool createGenerateEFKeyCmd()
        {
            m_CLA = 0x80;
            m_INS = 0xE0;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nCmdLen = 7;
            m_Lc = (byte)nCmdLen;
            m_Data = new byte[nCmdLen];
            m_Data[0] = 0x3F;
            m_Data[1] = 0x01;
            m_Data[2] = 0x38;
            m_Data[3] = 0x94;
            m_Data[4] = 0xEF;
            m_Data[5] = 0xFF;
            m_Data[6] = 0xFF;
            m_le = 0;
            m_nTotalLen = 12;
            return true;
        }

        public bool createWriteUserKeyCmd(byte[] randVal, StorageKeyParam Param)
        {
            if (randVal != null)
                return false;
            m_CLA = 0x80;
            m_INS = 0xD4;
            m_P1 = 0x01;
            m_P2 = Param.P2;
            int nCmdLen = 21;
            m_Lc = (byte)nCmdLen;
            m_Data = new byte[nCmdLen];
            m_Data[0] = Param.KeyPar1;
            m_Data[1] = 0xF0;
            m_Data[2] = Param.KeyPar2;
            m_Data[3] = Param.KeyPar3;
            m_Data[4] = Param.KeyPar4;
            Buffer.BlockCopy(Param.StorageKey, 0, m_Data, 5, 16);
            m_le = 0;
            m_nTotalLen = 5 + nCmdLen;
            return true;
        }

        public bool createSetStatusCmd(byte[] RandomVal, byte[] keyCalc)
        {
            return false;
        }

        public bool createUpdateEF15FileCmd(byte[] key, byte[] RandomVal, byte[] ASN, DateTime dateBegin, DateTime dateEnd)
        {
            if (RandomVal.Length != 8 || ASN.Length != 8)
                return false;
            m_CLA = 0x00;
            int nLen = 30;
            if (key != null)
            {
                m_CLA = 0x04;
                nLen += 4;
            }

            m_INS = 0xD6;
            m_P1 = 0x95;
            m_P2 = 0x00;            
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            //发卡方标识
            m_Data[0] = 0x10;
            m_Data[1] = 0xFF;
            m_Data[2] = 0xFF;
            m_Data[3] = 0xFF;
            m_Data[4] = 0xFF;
            m_Data[5] = 0xFF;
            m_Data[6] = 0xFF;
            m_Data[7] = 0xFF;
            //应用类型标识
            m_Data[8] = 0x11;
            m_Data[9] = 0x01;
            //卡号前2字节
            m_Data[10] = 0x01;
            m_Data[11] = 0x00;
            Buffer.BlockCopy(ASN, 0, m_Data, 12, 8);
            byte[] bcdDateBegin = GetBCDDate(dateBegin);//启用日期
            byte[] bcdDateEnd = GetBCDDate(dateEnd);    //有效日期
            Buffer.BlockCopy(bcdDateBegin, 0, m_Data, 20, 4);
            Buffer.BlockCopy(bcdDateEnd, 0, m_Data, 24, 4);
            m_Data[28] = 0x01;//指令集版本
            m_Data[29] = 0x00;//备用
            m_le = 0;

            m_nTotalLen = 35;
            if (key != null)
            {
                byte[] srcMacData = new byte[35]; //头5 +Data30
                srcMacData[0] = m_CLA;//需要后半字节为4
                srcMacData[1] = m_INS;
                srcMacData[2] = m_P1;
                srcMacData[3] = m_P2;
                srcMacData[4] = m_Lc;
                Buffer.BlockCopy(m_Data, 0, srcMacData, 5, 30);
                byte[] byteMAC = CalcMACValue(srcMacData, key, RandomVal);//计算MAC
                Buffer.BlockCopy(byteMAC, 0, m_Data, 30, 4);
                m_nTotalLen += 4;
            }

            
            return true;
        }

        public bool createUpdateEF16FileCmd(byte[] key, byte[] RandomVal, UserCardInfoParam cardInfo)
        {
            if (RandomVal.Length != 8)
                return false;
            m_CLA = 0x00;
            int nLen = 41;
            if (key != null)
            {
                m_CLA = 0x04;
                nLen += 4;
            }
            m_INS = 0xD6;
            m_P1 = 0x96;
            m_P2 = 0x00;            
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            for (int i = 0; i < nLen; i++)
            {
                m_Data[i] = 0xFF;
            }

            m_Data[0] = (byte)cardInfo.UserCardType;//卡类型
            m_Data[1] = 0x00;//职工标识
            //持卡人姓名
            int nOffset = 2;
            if (!string.IsNullOrEmpty(cardInfo.UserName))
            {
                byte[] byteName = Encoding.Unicode.GetBytes(cardInfo.UserName);
                for (int i = 0; i < byteName.Length; i++)
                {
                    m_Data[nOffset + i] = byteName[i];
                }
            }
            nOffset += 20;
            //身份证号
            string strIdentity = "12345678901234567F";            
            if (!string.IsNullOrEmpty(cardInfo.UserIdentity))
               strIdentity = cardInfo.UserIdentity.Replace('X','F');
            byte[] byteIdentity = StringToBCD(strIdentity);
            if (byteIdentity != null)
            {
                for (int i = 0; i < byteIdentity.Length; i++)
                {
                    m_Data[nOffset + i] = byteIdentity[i];
                }
            }
            nOffset += 9;
            //nOffset = 31
            m_Data[nOffset] = (byte)(cardInfo.IdType);//证件类型
            nOffset += 1;
            //折扣率
            byte[] byteRate = StringToBCD(cardInfo.DiscountRate.ToString("D4"));
            Buffer.BlockCopy(byteRate, 0, m_Data, nOffset, 2);
            byte[] bcdDate = GetBCDDate(cardInfo.DiscountRateEnd);//启用日期
            //折扣有效期,BCD码
            Buffer.BlockCopy(bcdDate, 0, m_Data, 34, 4);
            nOffset += 6;

            m_Data[nOffset] = cardInfo.PriceLevel; //价格等级
            m_Data[39] = 0;//当前等级
            m_Data[40] = 0x00;  //备用
            m_le = 0;

            m_nTotalLen = 46;
            if (key != null)
            {
                byte[] srcMacData = new byte[46]; //头5 +Data41
                srcMacData[0] = m_CLA;
                srcMacData[1] = m_INS;
                srcMacData[2] = m_P1;
                srcMacData[3] = m_P2;
                srcMacData[4] = m_Lc;
                Buffer.BlockCopy(m_Data, 0, srcMacData, 5, 41);
                byte[] byteMAC = CalcMACValue(srcMacData, key, RandomVal);//计算MAC
                Buffer.BlockCopy(byteMAC, 0, m_Data, 41, 4);
                m_nTotalLen += 4;
            }
            
            return true;
        }

        public bool createVerifyPINCmd(bool bDefaultPwd, byte[] customPwd)
        {
            if (!bDefaultPwd && customPwd.Length != 6)
                return false; 
            m_CLA = 0x00;
            m_INS = 0x20;
            m_P1 = 0x00;
            m_P2 = 0x00;
            m_Lc = (byte)3;
            m_Data = new byte[3];
            //PIN
            for (int i = 0; i < 3; i++)
            {
                    if (bDefaultPwd)
                        m_Data[i] = 0x99;
                    else
                        m_Data[i] = (byte)((customPwd[i * 2] << 4) | customPwd[i * 2 + 1]);
            }
            m_le = 0;
            m_nTotalLen = 8;
            return true;
        }

        public bool createUpdateEF0BFileCmd(bool bDefaultPwd)
        {
            m_CLA = 0x00;
            m_INS = 0xD6;
            m_P1 = 0x9B;
            m_P2 = 0x00;           
            m_Lc = (byte)32;
            m_Data = new byte[32];
            m_Data[0] = (byte)(bDefaultPwd ? 0x00 : 0x01);
            m_Data[1] = 0x01;//内部卡：员工号，其他：备用
            //内部卡：员工密码，其他：备用
            m_Data[2] = 0x12;
            m_Data[3] = 0x34;
            //4~31备用
            m_le = 0;
            m_nTotalLen = 37;
            return true;
        }

        public bool createUpdateEF1CFileCmd(byte[] key, byte[] RandomVal, UserCardInfoParam cardInfo)
        {
            if (RandomVal.Length != 8)
                return false;
            m_CLA = 0x00;
            int nLen = 96;
            if (key != null)
            {
                m_CLA = 0x04;
                nLen += 4;
            }
            m_INS = 0xD6;
            m_P1 = 0x9C;
            m_P2 = 0x00;            
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            for (int i = 0; i < nLen; i++)
                m_Data[i] = 0xFF;
            //限制气品
            byte[] byteGasType = BitConverter.GetBytes(cardInfo.LimitGasType);
            m_Data[0] = byteGasType[1];
            m_Data[1] = byteGasType[0];
            m_Data[2] = cardInfo.LimitArea;
            if (cardInfo.LimitArea != 0xFF)
            {
                byte[] byteLimitAreaCode = StringToBCD(cardInfo.LimitAreaCode);
                if (byteLimitAreaCode != null)
                    Buffer.BlockCopy(byteLimitAreaCode, 0, m_Data, 3, byteLimitAreaCode.Length);
            }
            if (cardInfo.LimitCarNo && !string.IsNullOrEmpty(cardInfo.CarNo))
            {
                byte[] carNo = Encoding.Unicode.GetBytes(cardInfo.CarNo);
                for (int i = 0; i < 16; i++)
                {
                    if (i < carNo.Length)
                        m_Data[43 + i] = carNo[i];
                    else
                        m_Data[43 + i] = 0x00;                    
                }
            }
                
            byte[] fixData = BitConverter.GetBytes(cardInfo.LimitFixDepartment);//定点单位标识
            m_Data[59] = fixData[3];
            m_Data[60] = fixData[2];
            m_Data[61] = fixData[1];
            m_Data[62] = fixData[0];
            
            m_Data[63] = cardInfo.LimitGasFillCount;
            byte[] byteAmount = BitConverter.GetBytes(cardInfo.LimitGasFillAmount);
            m_Data[64] = byteAmount[3];
            m_Data[65] = byteAmount[2];
            m_Data[66] = byteAmount[1];
            m_Data[67] = byteAmount[0];
            m_le = 0;

            m_nTotalLen = 101;
            if (key != null)
            {
                byte[] srcMacData = new byte[101]; //头5 +Data96
                srcMacData[0] = m_CLA;
                srcMacData[1] = m_INS;
                srcMacData[2] = m_P1;
                srcMacData[3] = m_P2;
                srcMacData[4] = m_Lc;
                Buffer.BlockCopy(m_Data, 0, srcMacData, 5, 96);
                byte[] byteMAC = CalcMACValue(srcMacData, key, RandomVal);//计算MAC
                Buffer.BlockCopy(byteMAC, 0, m_Data, 96, 4);
                m_nTotalLen += 4;
            }
            return true;
        }
        
        public bool createUpdateEF0DFileCmd(byte[] key, byte[] RandomVal, UserCardInfoParam cardInfo)
        {
            return false;
        }

        public bool createUpdateEF10FileCmd(byte[] key, byte[] RandomVal)
        {
            return false;
        }

        public bool createInitializeLoadCmd(int nMoney, byte[] TermialID)
        {
            if (TermialID.Length != 6)
                return false;
            m_CLA = 0x80;
            m_INS = 0x50;
            m_P1 = 0x00;
            m_P2 = 0x01; //ED电子存折0x01; EP电子钱包0x02
            int nLen = 11;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = 1; //密钥索引
            byte[] byteMoney = BitConverter.GetBytes(nMoney);
            m_Data[1] = byteMoney[3];
            m_Data[2] = byteMoney[2];
            m_Data[3] = byteMoney[1];
            m_Data[4] = byteMoney[0];
            Buffer.BlockCopy(TermialID, 0, m_Data, 5, 6);
            m_le = 16;
            m_nTotalLen = 17;
            return true;
        }

        public bool createInitializeUnLoadCmd(int nMoney, byte[] TermialID)
        {
            if (TermialID.Length != 6)
                return false;
            m_CLA = 0x80;
            m_INS = 0x50;
            m_P1 = 0x05;
            m_P2 = 0x01; //圈提
            int nLen = 11;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = 1; //密钥索引
            byte[] byteMoney = BitConverter.GetBytes(nMoney);
            m_Data[1] = byteMoney[3];
            m_Data[2] = byteMoney[2];
            m_Data[3] = byteMoney[1];
            m_Data[4] = byteMoney[0];
            Buffer.BlockCopy(TermialID, 0, m_Data, 5, 6);
            m_le = 16;
            m_nTotalLen = 17;
            return true;
        }

        public bool createCreditLoadCmd(byte[] byteMAC2, byte[] TimeBcd)
        {
            m_CLA = 0x80;
            m_INS = 0x52;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 11;  //Data Len
            m_Lc = (byte)nLen; 
            m_Data = new byte[nLen];
            Buffer.BlockCopy(TimeBcd, 0, m_Data, 0, 7);
            Buffer.BlockCopy(byteMAC2, 0, m_Data, 7, 4);
            m_le = 4;
            m_nTotalLen = 17;
            return true;
        }

        public bool createDebitUnLoadCmd(byte[] byteMAC2, byte[] TimeBcd)
        {
            m_CLA = 0x80;
            m_INS = 0x54;
            m_P1 = 0x03;
            m_P2 = 0x00;
            int nLen = 11;  //Data Len
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(TimeBcd, 0, m_Data, 0, 7);
            Buffer.BlockCopy(byteMAC2, 0, m_Data, 7, 4);
            m_le = 4;
            m_nTotalLen = 17;
            return true;
        }

        public bool createCardBalanceCmd()
        {
            m_CLA = 0x80;
            m_INS = 0x5C;
            m_P1 = 0x00;
            m_P2 = 0x01; //ED电子存折0x01; EP电子钱包0x02
            m_Lc = 0;
            m_Data = null;
            m_le = 4;
            m_nTotalLen = 5;
            return true;
        }

        public bool createrCardGrayCmd(bool bClearTAC)
        {
            m_CLA = 0xE0;
            m_INS = 0xCA;
            m_P1 = (byte)(bClearTAC ? 0x01 : 0x00); //0x00普通读取, 0x01清除TACUF
            m_P2 = 0x00;
            m_Lc = 0;
            m_Data = null;
            m_le = 30; //应答长度
            m_nTotalLen = 5;
            return true;
        }

        //灰锁初始化
        public bool createrInitForGrayCmd(byte[] TermialID)
        {
            if (TermialID.Length != 6)
                return false;
            m_CLA = 0xE0;
            m_INS = 0x7A;
            m_P1 = 0x08;
            m_P2 = 0x01;//ED电子存折0x01; EP电子钱包0x02
            int nLen = 7;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = 0x01;
            Buffer.BlockCopy(TermialID, 0, m_Data, 1, 6);
            m_le = 0x0F; //应答长度
            m_nTotalLen = 13;
            return true;
        }

        public bool createrGrayLockCmd(byte[] DataVal)
        {
            int nLen = DataVal.Length;
            if (nLen != 19)
                return false;
            m_CLA = 0xE0;
            m_INS = 0x7C;
            m_P1 = 0x08;
            m_P2 = 0x00;            
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(DataVal,0,m_Data,0,nLen);
            m_le = 0x08;
            m_nTotalLen = 25;
            return true;
        }

        //联机解扣初始化
        public bool createrInitForUnlockCardCmd(byte[] TermialID)
        {
            if (TermialID.Length != 6)
                return false;
            m_CLA = 0xE0;
            m_INS = 0x7A;
            m_P1 = 0x09;
            m_P2 = 0x01;
            int nLen = 7;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            m_Data[0] = 1; //密钥索引
            Buffer.BlockCopy(TermialID, 0, m_Data, 1, 6);
            m_le = 18;
            m_nTotalLen = 13;
            return true;
        }

        //联机解扣
        public bool createGreyCardUnLockCmd(int nMoney,byte[] byteMAC2, byte[] TimeBcd)
        {
            m_CLA = 0xE0;
            m_INS = 0x7E;
            m_P1 = 0x09;
            m_P2 = 0x00;
            int nLen = 15;  //Data Len
            m_Lc = (byte)nLen; 
            m_Data = new byte[nLen];
            byte[] byteMoney = BitConverter.GetBytes(nMoney);
            m_Data[0] = byteMoney[3];
            m_Data[1] = byteMoney[2];
            m_Data[2] = byteMoney[1];
            m_Data[3] = byteMoney[0];
            Buffer.BlockCopy(TimeBcd, 0, m_Data, 4, 7);
            Buffer.BlockCopy(byteMAC2, 0, m_Data, 11, 4);
            m_le = 4;
            m_nTotalLen = 21;
            return true;
        }

        public bool createDebitForUnlockCmd(byte[] DebitData)
        {
            int nLen = DebitData.Length;
            if (nLen != 27)
                return false;
            m_CLA = 0xE0;
            m_INS = 0x7E;
            m_P1 = 0x08;
            m_P2 = 0x01;//ED电子存折0x01; EP电子钱包0x02
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(DebitData, 0, m_Data, 0, 27);            
            m_le = 4;
            m_nTotalLen = 33;
            return true;
        }

        //加气交易明细记录文件
        public bool createReadRecordCmd(byte ResponseLen)
        {
            m_CLA = 0x00;
            m_INS = 0xB2;
            m_P1 = 0x01;
            m_P2 = 0xC4;
            m_Lc = 0x00;  //不存在
            m_Data = null; //不存在
            m_le = ResponseLen;   //公共应用基本数据文件EF15长度
            m_nTotalLen = 5;
            return true;
        }

        public bool createPINResetCmd(byte[] key, byte[] bytePIN)
        {
            if (key.Length != 16 || bytePIN.Length != 6)
                return false;
            byte[] PinVal = new byte[3];
            for (int i = 0; i < 3; i++)
            {
                PinVal[i] = (byte)((bytePIN[i * 2] << 4) | bytePIN[i * 2 + 1]);
            }
            m_CLA = 0x80;
            m_INS = 0x5E;
            m_P1 = 0x00;
            m_P2 = 0x00;
            int nLen = 7;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];

            Buffer.BlockCopy(PinVal, 0, m_Data, 0, 3);
            byte[] macKey = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                macKey[i] = (byte)(key[i] ^ key[8 + i]);
            }
            byte[] mac = CalcMacVal(PinVal, macKey);
            Buffer.BlockCopy(mac, 0, m_Data, 3, 4);
            m_le = 0;
            m_nTotalLen = 12;
            return true;
        }

        public bool createChangePINCmd(byte[] oldPwd, byte[] newPwd)
        {
            if (oldPwd.Length != 6 || newPwd.Length != 6)
                return false;
            byte[] oldPinVal = new byte[3];
            byte[] newPinVal = new byte[3];
            for (int i = 0; i < 3; i++)
            {
                oldPinVal[i] = (byte)((oldPwd[i * 2] << 4) | oldPwd[i * 2 + 1]);
                newPinVal[i] = (byte)((newPwd[i * 2] << 4) | newPwd[i * 2 + 1]);
            }
            
            m_CLA = 0x80;
            m_INS = 0x5E;
            m_P1 = 0x01;
            m_P2 = 0x00;
            int nLen = 7;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            Buffer.BlockCopy(oldPinVal, 0, m_Data, 0, 3);            
            m_Data[3] = 0xFF;
            Buffer.BlockCopy(newPinVal, 0, m_Data, 4, 3);
            m_le = 0;
            m_nTotalLen = 12;
            return true;
        }

        public bool createPINUnLockCmd(byte[] randval, byte[] key, byte[] bytePIN)
        {
            if (key.Length != 16 || bytePIN.Length != 6)
                return false;
                
            byte[] PINVal = new byte[8];
            PINVal[0] = 0x03;
            PINVal[1] = (byte)((bytePIN[0] << 4) | bytePIN[1]);
            PINVal[2] = (byte)((bytePIN[2] << 4) | bytePIN[3]);
            PINVal[3] = (byte)((bytePIN[4] << 4) | bytePIN[5]);
            PINVal[4] = 0x80;
            PINVal[5] = 0x00;
            PINVal[6] = 0x00;
            PINVal[7] = 0x00;

            
            m_CLA = 0x84;
            m_INS = 0x24;
            m_P1 = 0x00;
            m_P2 = 0x00;            
            int nLen = 12;
            m_Lc = (byte)nLen;
            m_Data = new byte[nLen];
            byte[] EncryptPinVal = DesCryptography.TripleEncryptData(PINVal, key);
            byte[] srcData = new byte[13];//用于计算MAC的原始数据
            srcData[0] = m_CLA;
            srcData[1] = m_INS;
            srcData[2] = m_P1;
            srcData[3] = m_P2;
            srcData[4] = m_Lc;
            Buffer.BlockCopy(EncryptPinVal, 0, srcData, 5, 8);
            byte[] mac = CalcMACValue(srcData, key, randval);
            Buffer.BlockCopy(EncryptPinVal, 0, m_Data, 0, 8);
            Buffer.BlockCopy(mac, 0, m_Data, 8, 4);
            m_le = 0;
            m_nTotalLen = 17;
            return true;
        }
    }
}
