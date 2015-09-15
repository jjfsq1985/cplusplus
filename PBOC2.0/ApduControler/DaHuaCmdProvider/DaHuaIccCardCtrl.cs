using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using SqlServerHelper;
using System.Data;
using IFuncPlugin;
using ApduParam;
using ApduInterface;
using ApduCtrl;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using CardControl;

namespace DaHuaApduCtrl
{
    public class DaHuaIccCardCtrl : DaHuaCardCtrlBase, ISamCardControl
    {
        public event MessageOutput TextOutput = null;

        private ApduController m_ctrlApdu = null;
        private ISamApduProvider m_CmdProvider = null;

        private static byte[] m_PSE = new byte[] { 0x31, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46, 0x30, 0x31 };//"1PAY.SYS.DDF01"
        private static byte[] m_ADF01 = new byte[] { 0x45, 0x4E, 0x45, 0x52, 0x47, 0x59, 0x2E, 0x30, 0x31 };//ENERGY.01
        private static byte[] m_ADF02 = new byte[] { 0x45, 0x4E, 0x45, 0x52, 0x47, 0x59, 0x2E, 0x30, 0x32 };//ENERGY.02
        private static byte[] m_ADF03 = new byte[] { 0x45, 0x4E, 0x45, 0x52, 0x47, 0x59, 0x2E, 0x30, 0x33 };//ENERGY.03

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


        public DaHuaIccCardCtrl(ApduController ApduCtrlObj, SqlConnectInfo DbInfo)
        {
            m_ctrlApdu = ApduCtrlObj;
            m_CmdProvider = m_ctrlApdu.GetPsamApduProvider();
            m_DBInfo = DbInfo;
        }

        protected virtual void OnTextOutput(MsgOutEvent args)
        {
            Trace.WriteLine(args.Message);
            if (TextOutput != null)
                TextOutput(args);
        }

        private bool SelectSamFile(bool bSamSlot, byte[] byteArray, byte[] prefixData)
        {
            m_CmdProvider.createSelectCmd(byteArray, prefixData);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.SAMCmdExchange(bSamSlot, data, datalen, RecvData, ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "SAM卡选择" + GetFileDescribe(byteArray) + "文件失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "SAM卡选择" + GetFileDescribe(byteArray) + "文件应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool SelectFile(byte[] byteArray, byte[] prefixData)
        {
            m_CmdProvider.createSelectCmd(byteArray, prefixData);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];            
            int nRecvLen = 0;  
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "SAM卡选择" + GetFileDescribe(byteArray) + "文件失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "SAM卡选择" + GetFileDescribe(byteArray) + "文件应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private byte[] GetRandomValue(int nRandomLen)
        {
            m_CmdProvider.createGetChallengeCmd(nRandomLen);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen, RecvData, ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "获取随机值失败"));
                return null;
            }
            else
            {
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return null;
            }
            byte[] RandomValue = new byte[nRandomLen];
            Buffer.BlockCopy(RecvData, 0, RandomValue, 0, nRandomLen);
            return RandomValue;
        }

        private bool ExternalAuthWithKey(byte[] KeyVal)
        {
            byte[] randByte = GetRandomValue(8);
            if (randByte == null || randByte.Length != 8)
                return false;

            OnTextOutput(new MsgOutEvent(0, "使用密钥：" + BitConverter.ToString(KeyVal) + "进行外部认证"));

            return ExternalAuthenticate(randByte, KeyVal);
        }

        private bool ExternalAuthentication(bool bMainKey)
        {
            byte[] randByte = GetRandomValue(8);
            if (randByte == null || randByte.Length != 8)
                return false;

            byte[] KeyVal = GetKeyVal(bMainKey, CardCategory.PsamCard);

            return ExternalAuthenticate(randByte, KeyVal);
        }

        private bool ExternalAuthenticate(byte[] randByte, byte[] KeyVal)
        {
            m_CmdProvider.createExternalAuthenticationCmd(randByte, KeyVal);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen, RecvData, ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "外部认证失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "外部认证应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool DeleteMF(bool bMainKey)
        {
            byte[] randByte = GetRandomValue(8);
            if (randByte == null || randByte.Length != 8)
                return false;

            byte[] KeyVal = GetKeyVal(bMainKey, CardCategory.PsamCard);

            return ClearMF(randByte, KeyVal);
        }

        private bool DeleteMFWithKey(byte[] KeyVal)
        {
            byte[] randByte = GetRandomValue(8);
            if (randByte == null || randByte.Length != 8)
                return false;

            OnTextOutput(new MsgOutEvent(0, "使用密钥：" + BitConverter.ToString(KeyVal) + "初始化"));

            return ClearMF(randByte, KeyVal);
        }

        private bool ClearMF(byte[] randByte, byte[] KeyVal)
        {
            m_CmdProvider.createClearMFcmd(randByte, KeyVal);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen, RecvData, ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "初始化失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "初始化应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private string GetFileDescribe(byte[] byteArray)
        {
            if (PublicFunc.ByteDataEquals(byteArray, m_PSE))
                return "MF";
            else if (PublicFunc.ByteDataEquals(byteArray, m_ADF01))
                return "ADF01";
            else if (PublicFunc.ByteDataEquals(byteArray, m_ADF02))
                return "ADF02";
            else if (PublicFunc.ByteDataEquals(byteArray, m_ADF03))
                return "ADF03";
            else
                return "";
        }

        public int InitIccCard(bool bMainKey)
        {
            byte[] KeyInit = new byte[16];
            bool bPublished = CheckPublishedCard(bMainKey, KeyInit);
            if (bPublished)
            {
                if (!SelectFile(m_PSE, null))
                    return 1;
                if (!ExternalAuthWithKey(KeyInit))
                    return 2;
                if (!DeleteMFWithKey(KeyInit))
                    return 3;
            }
            else
            {
                if (SelectFile(m_PSE, null))
                {
                    if (!ExternalAuthentication(bMainKey))
                        return 2;
                    if (!DeleteMF(bMainKey))
                        return 3;
                }
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

        public bool SamAppSelect(bool bSamSlot)
        {
            if (!SelectSamFile(bSamSlot,m_PSE, null))
                return false;
            byte[] prefix = new byte[] { 0xA0, 0x00, 0x00, 0x00, 0x03 };
            if (!SelectSamFile(bSamSlot,m_ADF01, prefix))
                return false;
            return true; 
        }

        private bool CreateKeyFile(ushort uFileId,ushort RecordCount, byte RecordLength)
        {
            m_CmdProvider.createGenerateKeyCmd(uFileId,RecordCount, RecordLength);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "创建Key文件失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "创建Key文件应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool CreateFCI()
        {
            m_CmdProvider.createGenerateEFCmd(0x00, 0xEF1E, 0xA4, 0x23);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "创建FCI文件失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "创建FCI文件应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool StorageFCI(byte[] byteName, byte[] prefix)
        {
            m_CmdProvider.createStorageFCICmd(byteName, prefix);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "安装FCI文件失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "安装FCI文件应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool CreateCardInfo(byte GenerateFlag, ushort FileId, byte FileType, ushort FileLen)
        {
            m_CmdProvider.createGenerateEFCmd(GenerateFlag, FileId, FileType, FileLen);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                string strMsg = string.Format("创建文件{0}失败", FileId.ToString("X"));
                OnTextOutput(new MsgOutEvent(nRet, strMsg));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                string strMsg = string.Format("创建文件{0}应答：{1}", FileId.ToString("X"), strData);
                OnTextOutput(new MsgOutEvent(0, strMsg));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
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
            m_CmdProvider.createSelectEFCmd(FileId);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                string strMessage = string.Format("SAM卡选择{0}失败", GetFileName(FileId, bMainKey));
                OnTextOutput(new MsgOutEvent(nRet, strMessage));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                string strMessage = string.Format("SAM卡选择{0}应答：{1}", GetFileName(FileId, bMainKey), strData);
                OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool StorageCardInfo(byte[] PsamId)
        {
            m_CmdProvider.createStorageCardInfoCmd(PsamId);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "写入卡片公共信息失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "写入卡片公共信息应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool StorageTermialInfo(byte[] TermialId)
        {
            m_CmdProvider.createStorageTermInfoCmd(TermialId);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "写入终端信息失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "写入终端信息应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
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
            if (!CreateKeyFile(0xFE01,0x000A, 0x15))
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
            m_CmdProvider.createGenerateADFCmd(byteName, FileId);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                string strMessage = string.Format("创建{0}文件失败", GetFileDescribe(byteName));
                OnTextOutput(new MsgOutEvent(nRet, strMessage));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                string strMessage = string.Format("创建{0}文件应答：{1}", GetFileDescribe(byteName), strData);
                OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
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
            if (!CreateKeyFile(0xFE01,0x0010, 0x15))
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
            m_CmdProvider.createStoragePsamInfoCmd(psamInfo);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "写入应用公共信息失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "写入应用公共信息应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool WriteMAC2()
        {
            m_CmdProvider.createWriteMAC2Cmd(0x0A, 0x0A);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "写MAC2文件失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "写MAC2文件应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool SetPsamCardStatus()
        {
            byte[] randomVal = GetRandomValue(8);
            if (randomVal == null || randomVal.Length != 8)
                return false;
            m_CmdProvider.createSetStatusCmd(randomVal, m_PsamKeyOrg);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "生命周期转换失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "生命周期转换应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
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
            byte[] randVal = GetRandomValue(4);
            if (randVal == null || randVal.Length != 4)
                return false;
            //PSAM卡写入密钥随机值4字节+4字节0
            byte[] randomVal = new byte[8];
            Buffer.BlockCopy(randVal, 0, randomVal, 0, 4);
            m_CmdProvider.createStorageAppKeyCmd(randomVal, keyApp, Usage, Ver, m_PsamKeyOrg);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen,RecvData,ref nRecvLen);
            if (nRet < 0)
            {
                string strMessage = string.Format("写入{0}失败", GetKeyName(Usage, Ver, bMainKey));
                OnTextOutput(new MsgOutEvent(nRet, strMessage));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                string strMessage = string.Format("写入{0}应答：{1}", GetKeyName(Usage, Ver, bMainKey), strData);
                OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
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
            //密钥（MAC加密等）
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
            if (!StoragePsamKey(m_KeyPsamMaintain, 0x82, 0x00,true))
                return false;
            if (!StoragePsamKey(m_KeyPsamMain, 0x89, 0x00, true))
                return false;
            return true;
        }

        private byte[] calcUserCardMAC1(byte[] ASN, byte[] rand, byte[] BusinessSn, byte[] TermialSn, byte[] TermialRand, byte[] srcData)
        {
            byte[] MAC1 = new byte[4];
            byte[] sespk = GetPrivateProcessKey(ASN, m_MPK1, rand, BusinessSn, TermialSn, TermialRand);
            if (sespk == null)
                return MAC1;
             MAC1 = m_CmdProvider.CalcMacVal_DES(srcData, sespk);
             return MAC1;
        }

        public bool InitSamGrayLock(bool bSamSlot, byte[] TermialID, byte[] random, byte[] BusinessSn, byte[] byteBalance, byte BusinessType, byte[] ASN, byte[] outData)
        {
            byte[] SysTime = PublicFunc.GetBCDTime();
            byte[] byteData = new byte[28];
            Buffer.BlockCopy(random, 0, byteData, 0, 4);
            Buffer.BlockCopy(BusinessSn, 0, byteData, 4, 2);
            Buffer.BlockCopy(byteBalance, 0, byteData, 6, 4);
            byteData[10] = BusinessType;
            Buffer.BlockCopy(SysTime, 0, byteData, 11, 7);
            byteData[18] = 0x01;//Key Ver
            byteData[19] = 0x00; // Key Flag
            Buffer.BlockCopy(ASN, 0, byteData, 20, 8);
            m_CmdProvider.createInitSamGrayLockCmd(byteData);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.SAMCmdExchange(bSamSlot, data, datalen, RecvData, ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "SAM卡MAC1计算初始化失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "SAM卡MAC1计算初始化应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
                //输出数据按灰锁命令数据域排列:终端交易序号，终端随机数，BCD时间，MAC1
                Buffer.BlockCopy(RecvData, 0, outData, 0, 4);
                Buffer.BlockCopy(RecvData, 4, outData, 4, 4);
                Buffer.BlockCopy(RecvData, 8, outData, 15, 4);                
                //PSAM卡计算的MAC1
                byte[] PSAM_MAC1 = new byte[4];
                Buffer.BlockCopy(RecvData, 8, PSAM_MAC1, 0, 4);

                 //加气专用过程密钥 计算
                byte[] TermialSn = new byte[4];                
                byte[] TermialRandom = new byte[4];
                Buffer.BlockCopy(RecvData, 0, TermialSn, 0, 4);
                Buffer.BlockCopy(RecvData, 4, TermialRandom, 0, 4);
                byte[] srcData = new byte[14];//用于计算MAC1的原始数据                                
                srcData[0] = BusinessType;
                Buffer.BlockCopy(TermialID, 0, srcData, 1, 6);
                Buffer.BlockCopy(SysTime, 0, srcData, 7, 7);
                byte[] MAC1 = calcUserCardMAC1(ASN, random, BusinessSn, TermialSn, TermialRandom, srcData);                      
                Buffer.BlockCopy(SysTime, 0, outData, 8, 7);
                Buffer.BlockCopy(MAC1, 0, outData, 15, 4);//MAC1
                string strInfo = string.Format("SAM卡MAC1计算初始化 MAC: {0} PC Calc MAC: {1}", BitConverter.ToString(PSAM_MAC1), BitConverter.ToString(MAC1));
                System.Diagnostics.Trace.WriteLine(strInfo);
                if(!PublicFunc.ByteDataEquals(MAC1,PSAM_MAC1))
                {
                    string strMessage = string.Format("MAC1计算验证失败：终端机编号{0}，用户卡号{1}", BitConverter.ToString(TermialID), BitConverter.ToString(ASN));
                    OnTextOutput(new MsgOutEvent(0, strMessage));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 加气专用消费交易过程密钥
        /// </summary>
        /// <param name="ASN">用户卡卡号</param>
        /// <param name="MPKKey">消费主密钥</param>
        /// <param name="Rand">用户卡随机数</param>
        /// <param name="OfflineSn">脱机交易序号（2字节）</param>
        /// <param name="TermialSn">终端序号（4字节）</param>
        /// <param name="TermialRand">SAM卡随机数</param>
        /// <returns></returns>
        private byte[] GetPrivateProcessKey(byte[] ASN, byte[] MPKKey, byte[] Rand, byte[] OfflineSn, byte[] TermialSn, byte[] TermialRand)
        {
            if (ASN.Length != 8)
                return null;
            //中间密钥
            byte[] DPKKey = new byte[16];
            byte[] encryptAsn = DesCryptography.TripleEncryptData(ASN, MPKKey);
            byte[] XorASN = new byte[8];
            for (int i = 0; i < 8; i++)
                XorASN[i] = (byte)(ASN[i] ^ 0xFF);
            byte[] encryptXorAsn = DesCryptography.TripleEncryptData(XorASN, MPKKey);
            Buffer.BlockCopy(encryptAsn, 0, DPKKey, 0, 8);
            Buffer.BlockCopy(encryptXorAsn, 0, DPKKey, 8, 8);
            byte[] byteData = new byte[8];
            Buffer.BlockCopy(Rand, 0, byteData, 0, 4);
            Buffer.BlockCopy(OfflineSn, 0, byteData, 4, 2);
            Buffer.BlockCopy(TermialSn, 2, byteData, 6, 2);
            byte[] byteTmpck = DesCryptography.TripleEncryptData(byteData, DPKKey);//中间密钥
            //计算过程密钥
            byte[] byteSESPK = m_CmdProvider.CalcPrivateProcessKey(TermialRand, byteTmpck);
            return byteSESPK;
        }

        public bool VerifyMAC2(bool bSamSlot,byte[] MAC2)
        {
            m_CmdProvider.createVerifyMAC2Cmd(MAC2);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.SAMCmdExchange(bSamSlot, data, datalen, RecvData, ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "SAM卡验证MAC2失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "SAM卡验证MAC2应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
             }
            return true;
        }

        public bool CalcGMAC(bool bSamSlot, byte BusinessType, byte[] ASN, int nOffLineSn, int nMoney, byte[] outGMAC)
        {
            m_CmdProvider.createCalcGMACCmd(BusinessType, ASN, nOffLineSn, nMoney);
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.SAMCmdExchange(bSamSlot, data, datalen, RecvData, ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "SAM卡计算GMAC失败"));
                return false;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "SAM卡计算GMAC应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return false;
                Buffer.BlockCopy(RecvData, 0, outGMAC, 0, 4);
            }
            return true;

        }

        private bool ReadKeyFromDb()
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return false;
            }

            if (!GetDbPsamKeyValue(ObjSql))
            {
                ObjSql.CloseConnection();
                ObjSql = null;
                return false;
            }

            byte[] ConsumerKey = GlobalControl.GetDbConsumerKey(ObjSql, "PROC_GetCpuKey", "AppConsumerKey", 1);
            if (ConsumerKey == null || !PublicFunc.ByteDataEquals(ConsumerKey, m_MPK1))
            {
                OnTextOutput(new MsgOutEvent(0, "加气消费密钥不一致"));
                MessageBox.Show("加气消费需要密钥一致，但当前使用的消费密钥不一致。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            byte[] ConsumerKey_Ly = GlobalControl.GetDbConsumerKey(ObjSql, "PROC_GetCpuKey", "AppConsumerKey", 2);
            if (ConsumerKey_Ly != null && !PublicFunc.ByteDataEquals(ConsumerKey_Ly, m_MPK1))
            {
                OnTextOutput(new MsgOutEvent(0, "积分消费密钥不一致"));
                MessageBox.Show("积分消费需要密钥一致，但当前使用的积分消费密钥不一致。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ObjSql.CloseConnection();
            ObjSql = null;
            return true;
        }

        public int ReadKeyValueFromSource()
        {
            int nRet = 0;
            if (m_ctrlApdu.m_CardKeyFrom == CardKeySource.CardKeyFromXml)
            {
                if (!ReadKeyFromXml())
                    nRet = 2;
            }
            else
            {
                if (!ReadKeyFromDb())
                    nRet = 1;
            }
            return nRet;
                
        }

        //还用不上的密钥没有读出
        private bool GetDbPsamKeyValue(SqlHelper sqlHelp)
        {
            PsamKeyData KeyVal = new PsamKeyData();
            if (!GlobalControl.GetDbPsamKeyVal(sqlHelp, KeyVal))
                return false;
            SetOrgKeyValue(KeyVal.OrgKeyVal, CardCategory.PsamCard);
            SetMainKeyValue(KeyVal.MasterKeyVal, CardCategory.PsamCard);  //卡片主控密钥  
            SetMaintainKeyValue(KeyVal.MasterTendingKeyVal, CardCategory.PsamCard);  //卡片维护密钥
            Buffer.BlockCopy(KeyVal.ApplicationMasterKey, 0, m_MAMK, 0, 16);//未用,psam卡无应用主控密钥安装
            Buffer.BlockCopy(KeyVal.ApplicationTendingKey, 0, m_MAMTK, 0, 16);
            Buffer.BlockCopy(KeyVal.ConsumerMasterKey, 0, m_MPK1, 0, 16);
            Buffer.BlockCopy(KeyVal.GrayCardKey, 0, m_MDK1, 0, 16);
            Buffer.BlockCopy(KeyVal.MacEncryptKey, 0, m_MADK, 0, 16);
            return true;
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
            strDbVal = BitConverter.ToString(CardKeyToDb(true, CardCategory.PsamCard)).Replace("-", "");
            sqlparams[8] = ObjSql.MakeParam("OrgKey", SqlDbType.Char, 32, ParameterDirection.Input, strDbVal);
            strDbVal = BitConverter.ToString(CardKeyToDb(false, CardCategory.PsamCard)).Replace("-", "");
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
                            byte[] byteKey = PublicFunc.StringToBCD(strMasterKey);
                            Buffer.BlockCopy(byteKey, 0, KeyInit, 0, 16);
                        }
                        else
                        {
                            string strOrgKey = (string)dataReader["OrgKey"];
                            byte[] byteKey = PublicFunc.StringToBCD(strOrgKey);
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
        public byte[] GetTerminalId(bool bSamSlot)
        {
            if (!SelectSamFile(bSamSlot,m_PSE, null))
                return null;
            m_CmdProvider.createGetEFFileCmd(0x96, 0x06);//文件标识(100+10110)0x16,终端机编号长度6
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.SAMCmdExchange(bSamSlot, data, datalen, RecvData, ref nRecvLen);
            if (nRet < 0)
            {
                OnTextOutput(new MsgOutEvent(nRet, "SAM卡读取终端机编号失败"));
                return null;
            }
            else
            {
                string strData = m_ctrlApdu.hex2asc(RecvData, nRecvLen);
                OnTextOutput(new MsgOutEvent(0, "SAM卡读取终端机编号应答：" + strData));
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return null;
                byte[] TerminalID = new byte[6];
                Buffer.BlockCopy(RecvData, 0, TerminalID, 0, 6);
                return TerminalID;
            }
        }

        //获取PSAM序列号
        /// <summary>
        /// 原始卡无卡号，检查数据库记录时不提示错误
        /// </summary>
        public byte[] GetPsamASN(bool bMessage)
        {
            m_CmdProvider.createGetEFFileCmd(0x95, 0x0E);//公共信息(100+10101)0x15文件长度0x0E
            byte[] data = m_CmdProvider.GetOutputCmd();
            int datalen = data.Length;
            byte[] RecvData = new byte[128];
            int nRecvLen = 0;
            int nRet = m_ctrlApdu.IccCmdExchange(data, datalen, RecvData, ref nRecvLen);
            if (nRet < 0)
            {
                if (bMessage)
                    OnTextOutput(new MsgOutEvent(nRet, "读取卡号失败"));
                return null;
            }
            else
            {
                if (!(nRecvLen >= 2 && RecvData[nRecvLen - 2] == 0x90 && RecvData[nRecvLen - 1] == 0x00))
                    return null;
                byte[] PsamAsn = new byte[8];
                Buffer.BlockCopy(RecvData, 2, PsamAsn, 0, 8);//实际10个字节，前两个字节为0，跳过
                if (bMessage)
                    OnTextOutput(new MsgOutEvent(0, "读取到卡号：" + BitConverter.ToString(PsamAsn)));
                return PsamAsn;
            }
        }

        //从XML文件中读取密钥
        private bool ReadKeyFromXml()
        {
            string strXmlPath = m_ctrlApdu.m_strCardKeyPath;
            try
            {
                XmlNode node = null;
                XmlDocument xml = new XmlDocument();                
                xml.Load(strXmlPath);//按路径读xml文件
                XmlNode root = xml.DocumentElement;//指向根节点
                node = root.SelectSingleNode("Seed");
                byte[] InitData = PublicFunc.StringToBCD(node.InnerText);
                node = root.SelectSingleNode("InitKey");
                byte[] InitKey = PublicFunc.StringToBCD(node.InnerText);

                byte[] Left = DesCryptography.TripleEncryptData(InitData, InitKey);
                byte[] Right = DesCryptography.TripleDecryptData(InitData, InitKey);
                byte[] EncryptKey = new byte[16];
                Buffer.BlockCopy(Left, 0, EncryptKey, 0, 8);
                Buffer.BlockCopy(Right, 0, EncryptKey, 8, 8);


                node = root.SelectSingleNode("PsamOrgKey");
                byte[] PsamOrgKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText),EncryptKey);
                SetOrgKeyValue(PsamOrgKey, CardCategory.PsamCard);                

                GetXmlPsamKeyValue(root, EncryptKey);

                byte[] ConsumerKey = GlobalControl.GetPrivateKeyFromXml(strXmlPath,"UserKeyValue_App1","AppConsumerKey");
                if (ConsumerKey == null || !PublicFunc.ByteDataEquals(ConsumerKey, m_MPK1))
                {
                    OnTextOutput(new MsgOutEvent(0, "加气消费密钥不一致"));
                    MessageBox.Show("加气消费需要消费密钥一致，但当前使用的消费密钥不一致。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                byte[] ConsumerKey_Ly = GlobalControl.GetPrivateKeyFromXml(strXmlPath, "UserKeyValue_App2", "AppConsumerKey");
                if (ConsumerKey_Ly != null && !PublicFunc.ByteDataEquals(ConsumerKey_Ly, m_MPK1))
                {
                    OnTextOutput(new MsgOutEvent(0, "积分消费密钥不一致"));
                    MessageBox.Show("积分消费需要密钥一致，但当前使用的积分消费密钥不一致。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool GetXmlPsamKeyValue(XmlNode ParentNode, byte[] EncryptKey)
        {
            XmlNode node = null;
            byte[] byteKey = null;
            XmlNode PsamKeyNode = ParentNode.SelectSingleNode("PsamKeyValue");

            node = PsamKeyNode.SelectSingleNode("MasterKey");
            byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
            SetMainKeyValue(byteKey, CardCategory.PsamCard);  //卡片主控密钥   

            node = PsamKeyNode.SelectSingleNode("MasterTendingKey");
            byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
            SetMaintainKeyValue(byteKey, CardCategory.PsamCard);  //卡片维护密钥

            node = PsamKeyNode.SelectSingleNode("ApplicationMasterKey");
            byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
            Buffer.BlockCopy(byteKey,0,m_MAMK,0,16);//未用,psam卡无应用主控密钥安装  

            node = PsamKeyNode.SelectSingleNode("ApplicationTendingKey");
            byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
            Buffer.BlockCopy(byteKey, 0, m_MAMTK, 0, 16);

            node = PsamKeyNode.SelectSingleNode("ConsumerMasterKey");
            byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
            Buffer.BlockCopy(byteKey, 0, m_MPK1, 0, 16);

            node = PsamKeyNode.SelectSingleNode("GrayCardKey");
            byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
            Buffer.BlockCopy(byteKey, 0, m_MDK1, 0, 16);

            node = PsamKeyNode.SelectSingleNode("MacEncryptKey");
            byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
            Buffer.BlockCopy(byteKey, 0, m_MADK, 0, 16);            

            return true;
        }

    }
}
