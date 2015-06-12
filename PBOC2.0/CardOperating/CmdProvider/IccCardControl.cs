using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using SqlServerHelper;
using System.Data;
using IFuncPlugin;
using System.Windows.Forms;

namespace CardOperating
{
    public class IccCardControl : CardControlBase
    {
        private PSAMCardAPDUProvider m_ctrlApdu = new PSAMCardAPDUProvider();

        private static byte[] m_PSE = new byte[] { 0x31, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46, 0x30, 0x31 };//"1PAY.SYS.DDF01"
        private static byte[] m_ADF01 = new byte[] { 0x45, 0x4E, 0x45, 0x52, 0x47, 0x59, 0x2E, 0x30, 0x31 };//ENERGY.01
        private static byte[] m_ADF02 = new byte[] { 0x45, 0x4E, 0x45, 0x52, 0x47, 0x59, 0x2E, 0x30, 0x32 };//ENERGY.02
        private static byte[] m_ADF03 = new byte[] { 0x45, 0x4E, 0x45, 0x52, 0x47, 0x59, 0x2E, 0x30, 0x33 };//ENERGY.03


        //卡片主控密钥
        private static byte[] m_MCMK = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //卡片维护密钥
        private static byte[] m_CCMK = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //应用主控密钥
        private static byte[] m_MAMK = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //应用维护密钥
        private static byte[] m_MAMTK = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //消费主密钥1
        private static byte[] m_MPK1 = new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
        //消费主密钥2
        private static byte[] m_MPK2 = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //消费主密钥3
        private static byte[] m_MPK3 = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //灰锁主密钥1
        private static byte[] m_MDK1 = new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
        //灰锁主密钥2
        private static byte[] m_MDK2 = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //灰锁主密钥3
        private static byte[] m_MDK3 = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //DTK 
        private static byte[] m_DTK = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //密钥（MAC加密等）
        private static byte[] m_MADK = new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };


        public IccCardControl(int icdev, SqlConnectInfo DbInfo)
        {
            m_MtDevHandler = icdev;
            m_DBInfo = DbInfo;
        } 

        private bool SelectFile(byte[] byteArray, byte[] prefixData)
        {
            m_ctrlApdu.createSelectCmd(byteArray, prefixData);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "PSAM卡选择" + GetFileDescribe(byteArray) + "文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] selectAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, selectAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "选择" + GetFileDescribe(byteArray) + "文件应答：" + Encoding.ASCII.GetString(selectAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        public void GetCardCosVersion()
        {
            m_ctrlApdu.createCosVersionCmd();
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "读取COS版本失败"));
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] VerAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, VerAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "读取COS版本应答：" + Encoding.ASCII.GetString(VerAsc)));
            }
        }

        private byte[] GetRandomValue(APDUBase provider, int nRandomLen)
        {
            provider.createGetChallengeCmd(nRandomLen);
            byte[] data = provider.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                this.OnTextOutput(new MsgOutEvent(m_RetVal, "获取随机值失败"));
                return null;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                //uint nAscLen = nRecvLen * 2;
                //byte[] randAsc = new byte[nAscLen];
                //DllExportMT.hex_asc(m_RecvData, randAsc, nRecvLen);
                //this.OnTextOutput(new MsgOutEvent(0, "随机值应答：" + Encoding.ASCII.GetString(randAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return null;
            }
            byte[] RandomValue = new byte[nRandomLen];
            Buffer.BlockCopy(m_RecvData, 0, RandomValue, 0, nRandomLen);
            return RandomValue;
        }

        private bool ExternalAuthWithKey(byte[] KeyVal)
        {
            byte[] randByte = GetRandomValue(m_ctrlApdu, 8);
            if (randByte == null || randByte.Length != 8)
                return false;

            base.OnTextOutput(new MsgOutEvent(0, "使用密钥：" + BitConverter.ToString(KeyVal) + "进行外部认证"));

            return ExternalAuthenticate(randByte, KeyVal);
        }

        private bool ExternalAuthentication(bool bMainKey)
        {
            byte[] randByte = GetRandomValue(m_ctrlApdu, 8);
            if (randByte == null || randByte.Length != 8)
                return false;

            byte[] KeyVal = m_ctrlApdu.GetKeyVal(bMainKey, APDUBase.CardCategory.PsamCard);

            return ExternalAuthenticate(randByte, KeyVal);
        }

        private bool ExternalAuthenticate(byte[] randByte, byte[] KeyVal)
        {
            m_ctrlApdu.createExternalAuthenticationCmd(randByte, KeyVal);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "外部认证失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] ExAuthAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, ExAuthAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "外部认证应答：" + Encoding.ASCII.GetString(ExAuthAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool DeleteMF(bool bMainKey)
        {
            byte[] randByte = GetRandomValue(m_ctrlApdu, 8);
            if (randByte == null || randByte.Length != 8)
                return false;

            byte[] KeyVal = m_ctrlApdu.GetKeyVal(bMainKey, APDUBase.CardCategory.PsamCard);

            return ClearMF(randByte, KeyVal);
        }

        private bool DeleteMFWithKey(byte[] KeyVal)
        {
            byte[] randByte = GetRandomValue(m_ctrlApdu, 8);
            if (randByte == null || randByte.Length != 8)
                return false;

            base.OnTextOutput(new MsgOutEvent(0, "使用密钥：" + BitConverter.ToString(KeyVal) + "初始化"));

            return ClearMF(randByte, KeyVal);
        }

        private bool ClearMF(byte[] randByte, byte[] KeyVal)
        {
            m_ctrlApdu.createClearMFcmd(randByte, KeyVal);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "初始化失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] ClearAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, ClearAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "初始化应答：" + Encoding.ASCII.GetString(ClearAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private string GetFileDescribe(byte[] byteArray)
        {
            if (APDUBase.ByteDataEquals(byteArray, m_PSE))
                return "MF";
            else if (APDUBase.ByteDataEquals(byteArray, m_ADF01))
                return "ADF01";
            else if (APDUBase.ByteDataEquals(byteArray, m_ADF02))
                return "ADF02";
            else if (APDUBase.ByteDataEquals(byteArray, m_ADF03))
                return "ADF03";
            else
                return "";
        }

        public int InitIccCard(bool bMainKey)
        {
            if (!SelectFile(m_PSE, null))
                return 1;
            byte[] KeyInit = new byte[16];
            bool bPublished = CheckPublishedCard(bMainKey, KeyInit);
            if (bPublished)
            {
                if (!ExternalAuthWithKey(KeyInit))
                    return 2;
                if (!DeleteMFWithKey(KeyInit))
                    return 3;
            }
            else
            {
                if (!ExternalAuthentication(bMainKey))
                    return 2;
                if (!DeleteMF(bMainKey))
                    return 3;
            }
            return 0;
        }

        public bool SelectPsamApp()
        {
            if (!SelectFile(m_PSE, null))
                return false;
            byte[] prefix = new byte[] { 0xA0, 0x00, 0x00, 0x00, 0x03 };
            if (!SelectFile(m_ADF01, prefix))
                return false;            
            return true; 
        }

        private bool CreateKeyFile(ushort RecordCount, byte RecordLength)
        {
            m_ctrlApdu.createGenerateKeyCmd(RecordCount, RecordLength);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "创建Key文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] keyAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, keyAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "创建Key文件应答：" + Encoding.ASCII.GetString(keyAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool CreateFCI()
        {
            m_ctrlApdu.createGenerateEFCmd(0x00, 0xEF1E, 0xA4, 0x23);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "创建FCI文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] fciAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, fciAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "创建FCI文件应答：" + Encoding.ASCII.GetString(fciAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool StorageFCI(byte[] byteName, byte[] prefix)
        {
            m_ctrlApdu.createStorageFCICmd(byteName, prefix);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "安装FCI文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] fciAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, fciAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "安装FCI文件应答：" + Encoding.ASCII.GetString(fciAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool CreateCardInfo(byte GenerateFlag, ushort FileId, byte FileType, ushort FileLen)
        {
            m_ctrlApdu.createGenerateEFCmd(GenerateFlag, FileId, FileType, FileLen);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                string strMsg = string.Format("创建文件{0}失败", FileId.ToString("X"));
                base.OnTextOutput(new MsgOutEvent(m_RetVal, strMsg));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] infoAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, infoAsc, nRecvLen);
                string strMsg = string.Format("创建文件{0}应答：{1}", FileId.ToString("X"), Encoding.ASCII.GetString(infoAsc));
                base.OnTextOutput(new MsgOutEvent(0, strMsg));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private string GetFileName(ushort FileId,bool bMainKey)
        {
            string strRet = "";
            switch (FileId)
            {
                case 0x0015:
                    strRet = "卡片公共信息文件";
                    break;
                case 0x0016:
                    strRet = "卡片终端信息文件";
                    break;
                case 0x0017:
                    strRet = "应用公共信息文件";
                    break;
                case 0x0018:
                    strRet = "终端应用交易文件";
                    break;
                case 0xFF01:
                    strRet = "终端脱机交易1文件";
                    break;
                case 0xFF02:
                    strRet = "MAC2文件";
                    break;
                case 0xFF03:
                    strRet = "终端脱机交易3文件";
                    break;
                case 0xFE01:
                    if (bMainKey)
                        strRet = "卡片密钥文件";
                    else
                        strRet = "应用密钥文件";
                    break;
            }
            return strRet;
        }

        private bool SelectCardInfo(ushort FileId,bool bMainKey)
        {
            m_ctrlApdu.createSelectEFCmd(FileId);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                string strMessage = string.Format("选择{0}失败", GetFileName(FileId, bMainKey));
                base.OnTextOutput(new MsgOutEvent(m_RetVal, strMessage));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] selectAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, selectAsc, nRecvLen);
                string strMessage = string.Format("选择{0}应答：{1}", GetFileName(FileId, bMainKey), Encoding.ASCII.GetString(selectAsc));
                base.OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool StorageCardInfo(byte[] PsamId)
        {
            m_ctrlApdu.createStorageCardInfoCmd(PsamId);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "写入卡片公共信息失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] storageAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, storageAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "写入卡片公共信息应答：" + Encoding.ASCII.GetString(storageAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool StorageTermialInfo(byte[] TermialId)
        {
            m_ctrlApdu.createStorageTermInfoCmd(TermialId);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "写入终端信息失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] storageAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, storageAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "写入终端信息应答：" + Encoding.ASCII.GetString(storageAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool CreateAppDF()
        {
            //创建ADF01
            if (!CreateDIR(m_ADF01, 0x3F01))
                return false;
            //创建ADF02
            if (!CreateDIR(m_ADF02, 0x3F02))
                return false;
            //创建ADF03
            if (!CreateDIR(m_ADF03, 0x3F03))
                return false;
            return true;
        }

        public bool CreateIccInfo(byte[] PsamId, byte[] TermialId)
        {
            if (!SelectFile(m_PSE, null))
                return false;
            if (!ExternalAuthentication(false))
                return false;
            if (!CreateKeyFile(0x000A, 0x15))
                return false;
            if (!CreateFCI())
                return false;
            if (!StorageFCI(m_PSE, null))
                return false;
            ////////////////////////////////////////////////////////////////////////////////////////////
            //创建ADF01，ADF02, ADF03
            if (!CreateAppDF())
                return false;
            /////////////////////////////////////////////////////////////////////////////////////////////
            if (!SelectFile(m_PSE, null))
                return false;
            if (!ExternalAuthentication(false))
                return false;
            //创建0015文件
            if (!CreateCardInfo(0x01, 0x0015, 0x60, 0x000E))
                return false;
            if (!SelectCardInfo(0x0015,false))
                return false;
            if (!StorageCardInfo(PsamId))
                return false;
            ///////////////////////////////////////////////////////////////////////////////////////////////
            if (!SelectFile(m_PSE, null))
                return false;
            if (!ExternalAuthentication(false))
                return false;
            //创建0016文件
            if (!CreateCardInfo(0x01, 0x0016, 0x60, 0x0006))
                return false;
            if (!SelectCardInfo(0x0016, false))
                return false;
            if (!StorageTermialInfo(TermialId))
                return false;
            return true;
        }

        private bool GenerateADF(byte[] byteName, ushort FileId)
        {
            m_ctrlApdu.createGenerateADFCmd(byteName, FileId);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                string strMessage = string.Format("创建{0}文件失败", GetFileDescribe(byteName));
                base.OnTextOutput(new MsgOutEvent(m_RetVal, strMessage));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] adfAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, adfAsc, nRecvLen);
                string strMessage = string.Format("创建{0}文件应答：{1}", GetFileDescribe(byteName), Encoding.ASCII.GetString(adfAsc));
                base.OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool CreateDIR(byte[] byteDIR, ushort FileId)
        {
            if (!GenerateADF(byteDIR, FileId))
                return false;
            byte[] prefix = new byte[] { 0xA0, 0x00, 0x00, 0x00, 0x03 };
            if (!SelectFile(byteDIR, prefix))
                return false;
            if (!ExternalAuthentication(false))
                return false;
            //创建Key
            if (!CreateKeyFile(0x0010, 0x15))
                return false;
            //FCI
            if (!CreateFCI())
                return false;
            if (!StorageFCI(byteDIR, prefix))
                return false;
            return true;
        }

        private bool StoragePsamInfo(IccCardInfoParam psamInfo)
        {
            m_ctrlApdu.createStoragePsamInfoCmd(psamInfo);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "写入应用公共信息失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] infoAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, infoAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "写入应用公共信息应答：" + Encoding.ASCII.GetString(infoAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool WriteMAC2()
        {
            m_ctrlApdu.createWriteMAC2Cmd(0x0A, 0x0A);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "写MAC2文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] infoAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, infoAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "写MAC2文件应答：" + Encoding.ASCII.GetString(infoAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool SetPsamCardStatus()
        {
            byte[] randomVal = GetRandomValue(m_ctrlApdu, 8);
            if (randomVal == null || randomVal.Length != 8)
                return false;
            m_ctrlApdu.createSetStatusCmd(randomVal);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "生命周期转换失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] SetStatusAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, SetStatusAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "生命周期转换应答：" + Encoding.ASCII.GetString(SetStatusAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        public bool WriteApplicationInfo(IccCardInfoParam psamInfo)
        {
            byte[] prefix = new byte[] { 0xA0, 0x00, 0x00, 0x00, 0x03 };
            if (!SelectFile(m_ADF01, prefix))
                return false;
            if (!ExternalAuthentication(false))
                return false;
            if (!CreateCardInfo(0x01, 0x0017, 0x60, 0x0025))
                return false;
            if (!SelectCardInfo(0x0017, false))
                return false;
            if (!StoragePsamInfo(psamInfo))
                return false;

            if (!SelectFile(m_ADF01, prefix))
                return false;
            if (!ExternalAuthentication(false))
                return false;
            if (!CreateCardInfo(0x02, 0x0018, 0x60, 0x0004))//数据元文件
                return false;
            if (!CreateCardInfo(0x02, 0xFF01, 0x78, 0x000C))
                return false;
            if (!CreateCardInfo(0x02, 0xFF03, 0x60, 0x0008))
                return false;
            if (!CreateCardInfo(0x02, 0xFF02, 0x60, 0x0002))//MAC2文件
                return false;
            if (!SelectCardInfo(0xFF02, false))
                return false;
            if (!WriteMAC2())
                return false;
            if (!SelectFile(m_ADF01, prefix))
                return false;
            SetPsamCardStatus();//生命周期转换
            return true;
        }

        private string GetKeyName(byte Usage, byte Ver, bool bMainKey)
        {
            string strRet = "";
            switch (Usage)
            {
                case 0x80:
                    if (Ver == 0x01)
                        strRet = "消费主密钥1";
                    else if (Ver == 0x02)
                        strRet = "消费主密钥2";
                    else if (Ver == 0x03)
                        strRet = "消费主密钥3";
                    break;
                case 0x8D:
                    if (Ver == 0x01)
                        strRet = "灰锁消费主密钥1";
                    else if (Ver == 0x02)
                        strRet = "灰锁消费主密钥2";
                    else if (Ver == 0x03)
                        strRet = "灰锁消费主密钥3";
                    break;                
                case 0x8E:
                    strRet = "TAC子密钥";
                    break;                    
                case 0x86:
                case 0x87:
                case 0x88:
                    strRet = "加密密钥";
                    break;
                case 0x82:
                    if (bMainKey)
                        strRet = "卡片维护密钥";
                    else
                        strRet = "应用维护密钥";
                    break;
                case 0x89:
                    if (bMainKey)
                        strRet = "卡片主控密钥";
                    else 
                        strRet = "应用主控密钥";
                    break;
            }
            return strRet;
        }

        private bool StoragePsamKey(byte[] keyApp, byte Usage, byte Ver, bool bMainKey)
        {
            byte[] randVal = GetRandomValue(m_ctrlApdu, 4);
            if (randVal == null || randVal.Length != 4)
                return false;
            //PSAM卡写入密钥随机值4字节+4字节0
            byte[] randomVal = new byte[8];
            Buffer.BlockCopy(randVal, 0, randomVal, 0, 4);
            m_ctrlApdu.createStorageAppKeyCmd(randomVal, keyApp, Usage, Ver);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                string strMessage = string.Format("写入{0}失败", GetKeyName(Usage, Ver, bMainKey));
                base.OnTextOutput(new MsgOutEvent(m_RetVal, strMessage));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] storageAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, storageAsc, nRecvLen);
                string strMessage = string.Format("写入{0}应答：{1}", GetKeyName(Usage, Ver, bMainKey), Encoding.ASCII.GetString(storageAsc));
                base.OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        //在ADF01应用下写密钥
        public bool SetupIccKey()
        {
            if (!SelectCardInfo(0xFE01, false))
                return false;
            if (!ExternalAuthentication(false))
                return false;
            //应用维护密钥
            if (!StoragePsamKey(m_MAMTK, 0x82, 0x02, false))
                return false;
            //消费主密钥1
            if (!StoragePsamKey(m_MPK1, 0x80, 0x01, false))
                return false;
            //消费主密钥2
            if (!StoragePsamKey(m_MPK2, 0x80, 0x02, false))
                return false;
            //消费主密钥3
            if (!StoragePsamKey(m_MPK3, 0x80, 0x03, false))
                return false;
            //灰锁主密钥1
            if (!StoragePsamKey(m_MDK1, 0x8D, 0x01, false))
                return false;
            //灰锁主密钥2
            if (!StoragePsamKey(m_MDK2, 0x8D, 0x02, false))
                return false;
            //灰锁主密钥3
            if (!StoragePsamKey(m_MDK3, 0x8D, 0x03, false))
                return false;
            //TAC子密钥
            if (!StoragePsamKey(m_DTK, 0x8E, 0x01, false))
                return false;
            //加密密钥1
            if (!StoragePsamKey(m_MADK, 0x88, 0x00, false))
                return false;
            //加密密钥2
            if (!StoragePsamKey(m_MADK, 0x86, 0x00, false))
                return false;
            //加密密钥3
            if (!StoragePsamKey(m_MADK, 0x87, 0x00, false))
                return false;
            //应用主控密钥
            if (!StoragePsamKey(m_MADK, 0x89, 0x00, false))
                return false;
            return true;
        }

        //写卡片的主控和维护密钥
        public bool SetupMainKey()
        {
            if (!SelectFile(m_PSE, null))
                return false;
            if (!SetPsamCardStatus())
                return false;
            if (!SelectCardInfo(0xFE01,true))
                return false;
            if (!ExternalAuthentication(false))
                return false;
            if (!StoragePsamKey(m_CCMK, 0x82, 0x00,true))
                return false;
            if (!StoragePsamKey(m_MCMK, 0x89, 0x00, true))
                return false;
            return true;
        }

        private byte[] calcUserCardMAC1(byte[] ASN, byte[] rand, byte[] BusinessSn, byte[] TermialSn, byte[] TermialRand, byte[] srcData)
        {
            byte[] MAC1 = new byte[4];
            byte[] sespk = GetPrivateProcessKey(ASN, m_MPK1, rand, BusinessSn, TermialSn, TermialRand);
            if (sespk == null)
                return MAC1;
             MAC1 = m_ctrlApdu.CalcMacVal(srcData, sespk);
             return MAC1;
        }

        public bool InitSamGrayLock(byte[] TermialID, byte[] random, byte[] BusinessSn, byte[] byteBalance, byte BusinessType, byte[] ASN, byte[] outData)
        {
            byte[] SysTime = GetBCDTime();
            byte[] byteData = new byte[28];
            Buffer.BlockCopy(random, 0, byteData, 0, 4);
            Buffer.BlockCopy(BusinessSn, 0, byteData, 4, 2);
            Buffer.BlockCopy(byteBalance, 0, byteData, 6, 4);
            byteData[10] = BusinessType;
            Buffer.BlockCopy(SysTime, 0, byteData, 11, 7);
            byteData[18] = 0x01;//Key Ver
            byteData[19] = 0x00; // Key Flag
            Buffer.BlockCopy(ASN, 0, byteData, 20, 8);
            m_ctrlApdu.createInitSamGrayLockCmd(byteData);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {                
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "PSAM卡MAC1计算初始化失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] InitAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, InitAsc, nRecvLen);                
                base.OnTextOutput(new MsgOutEvent(0, "PSAM卡MAC1计算初始化应答：" +Encoding.ASCII.GetString(InitAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
                //输出数据按灰锁命令数据域排列:终端交易序号，终端随机数，BCD时间，MAC1
                Buffer.BlockCopy(m_RecvData, 0, outData, 0, 4);
                Buffer.BlockCopy(m_RecvData, 4, outData, 4, 4);
                Buffer.BlockCopy(m_RecvData, 8, outData, 15, 4);                
                //PSAM卡计算的MAC1
                byte[] PSAM_MAC1 = new byte[4];
                Buffer.BlockCopy(m_RecvData, 8, PSAM_MAC1, 0, 4);

                 //加气专用过程密钥 计算
                byte[] TermialSn = new byte[4];                
                byte[] TermialRandom = new byte[4];
                Buffer.BlockCopy(m_RecvData, 0, TermialSn, 0, 4);
                Buffer.BlockCopy(m_RecvData, 4, TermialRandom, 0, 4);
                byte[] srcData = new byte[14];//用于计算MAC1的原始数据                                
                srcData[0] = BusinessType;
                Buffer.BlockCopy(TermialID, 0, srcData, 1, 6);
                Buffer.BlockCopy(SysTime, 0, srcData, 7, 7);
                byte[] MAC1 = calcUserCardMAC1(ASN, random, BusinessSn, TermialSn, TermialRandom, srcData);                      
                Buffer.BlockCopy(SysTime, 0, outData, 8, 7);
                Buffer.BlockCopy(MAC1, 0, outData, 15, 4);//MAC1
                string strInfo = string.Format("PSAM卡MAC1计算初始化 MAC: {0} PC Calc MAC: {1}", BitConverter.ToString(PSAM_MAC1), BitConverter.ToString(MAC1));
                System.Diagnostics.Trace.WriteLine(strInfo);
                if(!APDUBase.ByteDataEquals(MAC1,PSAM_MAC1))
                {
                    string strMessage = string.Format("MAC1计算验证失败：终端机编号{0}，用户卡号{1}", BitConverter.ToString(TermialID), BitConverter.ToString(ASN));
                    base.OnTextOutput(new MsgOutEvent(0, strMessage));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 加气专用消费交易过程密钥
        /// </summary>
        /// <param name="ASN">用户卡卡号</param>
        /// <param name="MasterKey">消费主密钥</param>
        /// <param name="Rand">用户卡随机数</param>
        /// <param name="OfflineSn">脱机交易序号（2字节）</param>
        /// <param name="TermialSn">终端序号（4字节）</param>
        /// <param name="TermialRand">PSAM卡随机数</param>
        /// <returns></returns>
        private byte[] GetPrivateProcessKey(byte[] ASN, byte[] MasterKey, byte[] Rand, byte[] OfflineSn, byte[] TermialSn, byte[] TermialRand)
        {
            if (ASN.Length != 8)
                return null;
            //中间密钥
            byte[] DPKKey = new byte[16];
            byte[] encryptAsn = APDUBase.TripleEncryptData(ASN, MasterKey);
            byte[] XorASN = new byte[8];
            for (int i = 0; i < 8; i++)
                XorASN[i] = (byte)(ASN[i] ^ 0xFF);
            byte[] encryptXorAsn = APDUBase.TripleEncryptData(XorASN, MasterKey);
            Buffer.BlockCopy(encryptAsn, 0, DPKKey, 0, 8);
            Buffer.BlockCopy(encryptXorAsn, 0, DPKKey, 8, 8);
            byte[] byteData = new byte[8];
            Buffer.BlockCopy(Rand, 0, byteData, 0, 4);
            Buffer.BlockCopy(OfflineSn, 0, byteData, 4, 2);
            Buffer.BlockCopy(TermialSn, 2, byteData, 6, 2);
            byte[] byteTmpck = APDUBase.TripleEncryptData(byteData, DPKKey);//中间密钥
            //计算过程密钥
            byte[] byteSESPK = m_ctrlApdu.CalcPrivateProcessKey(TermialRand, byteTmpck);
            return byteSESPK;
        }

        public bool VerifyMAC2(byte[] MAC2)
        {
            m_ctrlApdu.createVerifyMAC2Cmd(MAC2);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {                
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "PSAM卡验证MAC2失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] VerifyAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, VerifyAsc, nRecvLen);   
                base.OnTextOutput(new MsgOutEvent(0, "PSAM卡验证MAC2应答：" + Encoding.ASCII.GetString(VerifyAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
             }
            return true;
        }

        public bool CalcGMAC(byte BusinessType,byte[] ASN, int nOffLineSn, int nMoney, byte[] outGMAC)
        {
            m_ctrlApdu.createCalcGMACCmd(BusinessType, ASN, nOffLineSn, nMoney);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "PSAM卡计算GMAC失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] GmacAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, GmacAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "PSAM卡计算GMAC应答：" + Encoding.ASCII.GetString(GmacAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
                Buffer.BlockCopy(m_RecvData, 0, outGMAC, 0, 4);
            }
            return true;

        }

        private bool GetConfigKeyIdValid(ref int nOrgPsamkeyId, ref int nUserPsamKeyID, SqlHelper sqlHelp)
        {
            SqlDataReader dataReader = null;
            sqlHelp.ExecuteCommand("select OrgPsamKeyId,UsePsamKeyID from Config_SysParams", out dataReader);
            if (dataReader != null)
            {
                if (!dataReader.HasRows)
                {
                    dataReader.Close();
                    return false;
                }
                else
                {
                    if (dataReader.Read())
                    {
                        nOrgPsamkeyId = (int)dataReader["OrgPsamKeyId"];
                        nUserPsamKeyID = (int)dataReader["UsePsamKeyID"];
                    }
                    dataReader.Close();
                    return true;
                }
            }
            return false;
        }

        public bool ReadKeyValueFormDb()
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return false;
            }

            if (!GetOrgKeyValue(ObjSql))
            {
                ObjSql.CloseConnection();
                ObjSql = null;
                return false;
            }
            if (!GetPsamKeyValue(ObjSql))
            {
                ObjSql.CloseConnection();
                ObjSql = null;
                return false;
            }

            byte[] ConsumerKey = GetRelatedKey(ObjSql, APDUBase.CardCategory.CpuCard);
            if (ConsumerKey == null || !APDUBase.ByteDataEquals(ConsumerKey, m_MPK1))
            {
                base.OnTextOutput(new MsgOutEvent(0, "卡片消费密钥不一致"));
                MessageBox.Show("加气消费需要消费密钥一致，但当前使用的消费密钥不一致。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ObjSql.CloseConnection();
            ObjSql = null;
            return true;
        }

        private bool GetOrgKeyValue(SqlHelper sqlHelp)
        {
            SqlDataReader dataReader = null;
            //OrgKeyType 0-CPU卡，1-PSAM卡
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = sqlHelp.MakeParam("OrgKeyType", SqlDbType.Int, 4, ParameterDirection.Input, 1);
            sqlHelp.ExecuteProc("PROC_GetOrgKey", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (!dataReader.HasRows)
                {
                    dataReader.Close();
                    return false;
                }
                else
                {
                    if (dataReader.Read())
                    {
                        string strKey = (string)dataReader["OrgKey"];
                        byte[] byteKey = APDUBase.StringToBCD(strKey);
                        m_ctrlApdu.SetOrgKeyValue(byteKey, APDUBase.CardCategory.PsamCard);
                    }
                    dataReader.Close();
                    return true;
                }
            }
            return false;
        }

        //还用不上的密钥没有读出
        private bool GetPsamKeyValue(SqlHelper sqlHelp)
        {
            SqlDataReader dataReader = null;
            sqlHelp.ExecuteProc("PROC_GetPsamKey", out dataReader);
            if (dataReader != null)
            {
                if (!dataReader.HasRows)
                {
                    dataReader.Close();
                    return false;
                }
                else
                {
                    if (dataReader.Read())
                    {
                        string strKey = (string)dataReader["MasterKey"];
                        StrKeyToByte(strKey, m_MCMK);
                        strKey = (string)dataReader["MasterTendingKey"];
                        StrKeyToByte(strKey, m_CCMK);
                        strKey = (string)dataReader["ApplicatonMasterKey"];
                        StrKeyToByte(strKey, m_MAMK);//未用,psam卡无应用主控密钥安装
                        strKey = (string)dataReader["ApplicationTendingKey"];
                        StrKeyToByte(strKey, m_MAMTK);
                        strKey = (string)dataReader["ConsumerMasterKey"];
                        StrKeyToByte(strKey, m_MPK1);
                        strKey = (string)dataReader["GrayCardKey"];
                        StrKeyToByte(strKey, m_MDK1);
                        strKey = (string)dataReader["MacEncryptKey"];
                        StrKeyToByte(strKey, m_MADK);
                        m_ctrlApdu.SetMainKeyValue(m_MCMK, APDUBase.CardCategory.PsamCard);//卡片主控密钥

                    }
                    dataReader.Close();
                    return true;
                }
            }
            return false;
        }


        //PSAM发卡后存入数据库
        public bool SavePsamCardInfoToDb(IccCardInfoParam PsamInfoPar)
        {
            string strDbVal = null;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return false;
            }

            bool bSuccess = false;
            SqlParameter[] sqlparams = new SqlParameter[10];
            strDbVal = BitConverter.ToString(PsamInfoPar.GetBytePsamId()).Replace("-", "");
            sqlparams[0] = ObjSql.MakeParam("PsamCardId", SqlDbType.Char, 16, ParameterDirection.Input, strDbVal);
            strDbVal = BitConverter.ToString(PsamInfoPar.GetByteTermId()).Replace("-", "");
            sqlparams[1] = ObjSql.MakeParam("TerminalId", SqlDbType.VarChar, 12, ParameterDirection.Input, strDbVal);
            sqlparams[2] = ObjSql.MakeParam("ClientId", SqlDbType.Int, 4, ParameterDirection.Input, PsamInfoPar.ClientID);            
            sqlparams[3] = ObjSql.MakeParam("UseValidateDate", SqlDbType.DateTime, 8, ParameterDirection.Input, PsamInfoPar.ValidAppForm);
            sqlparams[4] = ObjSql.MakeParam("UseInvalidateDate", SqlDbType.DateTime, 8, ParameterDirection.Input, PsamInfoPar.ValidAppTo);

            strDbVal = BitConverter.ToString(PsamInfoPar.GetByteCompanyIssue()).Replace("-", "");
            sqlparams[5] = ObjSql.MakeParam("CompanyFrom", SqlDbType.VarChar, 16, ParameterDirection.Input, strDbVal);
            strDbVal = BitConverter.ToString(PsamInfoPar.GetByteCompanyRecv()).Replace("-", "");
            sqlparams[6] = ObjSql.MakeParam("CompanyTo", SqlDbType.VarChar, 16, ParameterDirection.Input, strDbVal);
            sqlparams[7] = ObjSql.MakeParam("Remark", SqlDbType.NVarChar, 50, ParameterDirection.Input, PsamInfoPar.Remark);
            //密钥
            strDbVal = BitConverter.ToString(m_ctrlApdu.CardKeyToDb(true, APDUBase.CardCategory.PsamCard)).Replace("-", "");
            sqlparams[8] = ObjSql.MakeParam("OrgKey", SqlDbType.Char, 32, ParameterDirection.Input, strDbVal);
            strDbVal = BitConverter.ToString(m_ctrlApdu.CardKeyToDb(false, APDUBase.CardCategory.PsamCard)).Replace("-", "");
            sqlparams[9] = ObjSql.MakeParam("PsamMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strDbVal);
            if (ObjSql.ExecuteProc("PROC_PublishPsamCard", sqlparams) == 0)
                bSuccess = true;
            ObjSql.CloseConnection();
            ObjSql = null;
            return bSuccess;
        }

        //检查数据库中是否有该卡的发卡记录
        public bool CheckPublishedCard(bool bMainKey, byte[] KeyInit)
        {
            //PSAM卡获取卡号不用进业务应用
            byte[] PsamAsn = GetPsamASN(false);
            if (PsamAsn == null || PsamAsn.Length != 8)
                return false;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return false;
            }
            string strDbAsn = BitConverter.ToString(PsamAsn).Replace("-", "");
            SqlDataReader dataReader = null;
            SqlParameter[] sqlparam = new SqlParameter[1];
            sqlparam[0] = ObjSql.MakeParam("PsamId", SqlDbType.Char, 16, ParameterDirection.Input, strDbAsn);            
            ObjSql.ExecuteCommand("select * from Psam_Card where PsamId = @PsamId", sqlparam, out dataReader);
            bool bRet = false;
            if (dataReader != null)
            {
                if (!dataReader.HasRows)
                    dataReader.Close();
                else
                {
                    if (dataReader.Read())
                    {
                        if (bMainKey)
                        {
                            string strMasterKey = (string)dataReader["PsamMasterKey"];
                            byte[] byteKey = APDUBase.StringToBCD(strMasterKey);
                            Buffer.BlockCopy(byteKey, 0, KeyInit, 0, 16);
                        }
                        else
                        {
                            string strOrgKey = (string)dataReader["OrgKey"];
                            byte[] byteKey = APDUBase.StringToBCD(strOrgKey);
                            Buffer.BlockCopy(byteKey, 0, KeyInit, 0, 16);
                        }
                        bRet = true;
                    }
                    dataReader.Close();
                }
            }

            ObjSql.CloseConnection();
            ObjSql = null;
            return bRet;
        }

        //获取终端机编号
        public byte[] GetTerminalId()
        {
            if (!SelectFile(m_PSE, null))
                return null;
            m_ctrlApdu.createGetEFFileCmd(0x96, 0x06);//文件标识(100+10110)0x16,终端机编号长度6
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ICC_CommandExchange(m_MtDevHandler, 0x00, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "PSAM卡读取终端机编号失败"));
                return null;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] TIDAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, TIDAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "PSAM卡读取终端机编号应答：" + Encoding.ASCII.GetString(TIDAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return null;
                byte[] TerminalID = new byte[6];
                Buffer.BlockCopy(m_RecvData, 0, TerminalID, 0, 6);
                return TerminalID;
            }
        }

        //获取PSAM序列号
        /// <summary>
        /// 原始卡无卡号，检查数据库记录时不提示错误
        /// </summary>
        public byte[] GetPsamASN(bool bMessage)
        {
            m_ctrlApdu.createGetEFFileCmd(0x95, 0x0E);//公共信息(100+10101)0x15文件长度0x0E
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                if (bMessage)
                    base.OnTextOutput(new MsgOutEvent(m_RetVal, "读取卡号失败"));
                return null;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] ASNAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, ASNAsc, nRecvLen);
                if (bMessage)
                    base.OnTextOutput(new MsgOutEvent(0, "读取卡号应答：" + Encoding.ASCII.GetString(ASNAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return null;
                byte[] PsamAsn = new byte[8];
                Buffer.BlockCopy(m_RecvData, 2, PsamAsn, 0, 8);//实际10个字节，前两个字节为0，跳过
                return PsamAsn;
            }
        }

    }
}
