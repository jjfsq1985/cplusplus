using System;
using System.Collections.Generic;
using System.Text;

namespace CardOperating
{
    public class UserCardControl : CardControlBase
    {
        private UserCardAPDUProvider m_ctrlApdu = new UserCardAPDUProvider();
 
        private const string m_strPSE = "1PAY.SYS.DDF01";
        private const string m_strDIR1 = "ENN ENERGY";//加气应用
        private const string m_strDIR2 = "ENN LOYALTY"; //积分应用
        private const string m_strDIR3 = "ENN SV";    //监管应用

        //加气消费密钥MPK1
        private readonly byte[] m_MPK1 = new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
        //加气应用主控密钥MCMK
        private readonly byte[] m_MCMK = new byte[] { 0xF2, 0x1B, 0x12, 0x34, 0x04, 0x38, 0x30, 0xD4, 0x48, 0x29, 0x3E, 0x66, 0x36, 0x88, 0x33, 0xCC };
        //加气消费密钥MPK2
        private readonly byte[] m_MPK2 = new byte[] { 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22 };
        //圈存主密钥MLK1
        private readonly byte[] m_MLK1 = new byte[] { 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66 };
        //圈存主密钥MLK2
        private readonly byte[] m_MLK2 = new byte[] { 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77 };
        //TAC主密钥MTK
        private readonly byte[] m_MTK = new byte[] { 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77 };
        //联机解扣主密钥 / 圈提主密钥MULK
        private readonly byte[] m_MULK = new byte[] { 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB, 0xBB };
        //透支限额主密钥MUK
        private readonly byte[] m_MUK = new byte[] { 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC };
        //PIN解锁主密钥MPUK
        private readonly byte[] m_MPUK = new byte[] { 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88 };
        //密码重装主密钥MRPK
        private readonly byte[] m_MRPK = new byte[] { 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99 };
        //应用维护主密钥MAMK
        private readonly byte[] m_MAMK = new byte[] { 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA };
        //内部认证主密钥MIAK
        private readonly byte[] m_MIAK = new byte[] { 0xF2, 0x11, 0x20, 0x6C, 0x05, 0x68, 0x30, 0xD4, 0x48, 0x29, 0x3E, 0x66, 0x36, 0x88, 0x33, 0xBB };

        public UserCardControl(int icdev)
        {
            m_MtDevHandler = icdev;
        }

        private bool SelectFile(string strName, byte[] prefixData)
        {
            if (string.IsNullOrEmpty(strName))
                return false;
            byte[] byteName = Encoding.ASCII.GetBytes(strName);
            m_ctrlApdu.createSelectCmd(byteName, prefixData);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "选择" + GetFileDescribe(strName) + "文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] selectAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, selectAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "选择" + GetFileDescribe(strName) + "文件应答：" + Encoding.ASCII.GetString(selectAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private byte[] GetRandomValue(APDUBase provider, int nRandomLen)
        {
            provider.createGetChallengeCmd(nRandomLen);
            byte[] data = provider.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
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

        private bool ExternalAuthentication(bool bMainKey)
        {
            byte[] randByte = GetRandomValue(m_ctrlApdu,8);
            if (randByte == null || randByte.Length != 8)
                return false;

            m_ctrlApdu.createExternalAuthenticationCmd(randByte, bMainKey, APDUBase.CardCategory.CpuCard);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
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
                string strErrAsc = Encoding.ASCII.GetString(ExAuthAsc);
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                {
                    string strErr = GetErrString(m_RecvData[nRecvLen - 2], m_RecvData[nRecvLen - 1],strErrAsc);
                    base.OnTextOutput(new MsgOutEvent(0, "外部认证错误：" + strErr));
                    return false;
                }
                else
                {
                    base.OnTextOutput(new MsgOutEvent(0, "外部认证应答：" + strErrAsc));
                }
            }
            return true;
        }

        private bool DeleteMF(bool bMainKey)
        {
            byte[] randByte = GetRandomValue(m_ctrlApdu, 8);
            if (randByte == null || randByte.Length != 8)
                return false;

            m_ctrlApdu.createClearMFcmd(randByte, bMainKey, APDUBase.CardCategory.CpuCard);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
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

        private string GetFileDescribe(string strName)
        {
            if (strName == m_strPSE)
                return "MF";
            else if (strName == m_strDIR1)
                return "ADF";
            return "";
        }

        public int InitCard(bool bMainKey)
        {
            if (!SelectFile(m_strPSE, null))
                return 1;
            if (!ExternalAuthentication(bMainKey))
                return 2;
            if (!DeleteMF(bMainKey))
                return 3;
            return 0;
        }

        private bool CreateFCI()
        {
            m_ctrlApdu.createGenerateFCICmd();
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
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

        private bool StorageFCI(string strName, byte[] param, byte[] prefix)
        {
            m_ctrlApdu.createStorageFCICmd(strName, param, prefix);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
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


        public bool CreateDIR()
        {
            if (!SelectFile(m_strPSE, null))
                return false;
            if (!ExternalAuthentication(false))
                return false;
            if (!CreateFCI())
                return false;
            byte[] param = new byte[]{0x88,0x01,0x01};
            if (!StorageFCI(m_strPSE, param, null))
                return false;
            if (!CreateEFDir())
                return false;
            bool bRet = true;
            string[] strDirName = new string[] { m_strDIR1, m_strDIR2, m_strDIR3 };
            for (int i = 0; i < 3; i++)
            {
                if (!UpdateDir(i + 1, strDirName[i]))
                {
                    bRet = false;
                    break;
                }
            }
            return bRet;
        }

        //EF01
        private bool CreateEFDir()
        {
            m_ctrlApdu.createGenerateEFCmd(0xEF01, 0x44, 0x40, 0x00, 0, 0, 0, 0);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "创建目录EF01失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] fciAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, fciAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "创建目录EF01应答：" + Encoding.ASCII.GetString(fciAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool UpdateDir(int nIndex, string strName)
        {
            m_ctrlApdu.createUpdateEF01Cmd((byte)nIndex, strName);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                string strErr = string.Format("更新目录文件{0}失败", nIndex);
                base.OnTextOutput(new MsgOutEvent(m_RetVal, strErr));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] dirAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, dirAsc, nRecvLen);
                string strMsg = string.Format("更新目录文件{0}应答：{1}", nIndex, Encoding.ASCII.GetString(dirAsc));
                base.OnTextOutput(new MsgOutEvent(0, strMsg));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        //安装密钥
        public void CreateKey()
        {
            if (!CreateKeyFile())
                return;
            WriteKeyMK();
        }


        //生成Key文件
        private bool CreateKeyFile()
        {
            m_ctrlApdu.createGenerateKeyCmd();
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
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

        //安装Key,需要随机值
        private bool WriteKeyMK()
        {
            byte[] randByte = GetRandomValue(m_ctrlApdu, 8);
            if (randByte == null || randByte.Length != 8)
                return false;
            //安装Key的计算MAC需要随机数参与
            m_ctrlApdu.createStorageKeyCmd(randByte);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "安装Key文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] keyAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, keyAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "安装Key文件应答：" + Encoding.ASCII.GetString(keyAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool CreateEFFile(ushort fileID, ushort fileLen, byte keyIndex, ushort ACr, ushort ACw)
        {
            m_ctrlApdu.createGenerateEFCmd(fileID, 0x01, fileLen, keyIndex, 0, 0, ACr, ACw);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                string strMessage = string.Format("创建{0}文件失败", fileID.ToString("X4"));
                base.OnTextOutput(new MsgOutEvent(m_RetVal, strMessage));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] efAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, efAsc, nRecvLen);
                string strMessage = string.Format("创建{0}文件应答：{1}", fileID.ToString("X4"), Encoding.ASCII.GetString(efAsc));
                base.OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool CreateRecordFile(ushort fileID, byte fileType, byte RecordNum, byte RecordLen, ushort ACr, ushort ACw)
        {
            m_ctrlApdu.createGenerateEFCmd(fileID, fileType, 0, 0, RecordNum, RecordLen, ACr, ACw);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                string strMessage = string.Format("创建{0}记录文件失败", fileID.ToString("X4"));
                base.OnTextOutput(new MsgOutEvent(m_RetVal, strMessage));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] recordAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, recordAsc, nRecvLen);
                string strMessage = string.Format("创建{0}记录文件应答：{1}", fileID.ToString("X4"), Encoding.ASCII.GetString(recordAsc));
                base.OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool CreateEFFileWithType(ushort fileID, byte fileType, ushort fileLen, byte keyIndex, ushort ACr, ushort ACw)
        {
            m_ctrlApdu.createGenerateEFCmd(fileID, fileType, fileLen, keyIndex, 0, 0, ACr, ACw);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                string strMessage = string.Format("创建{0}文件失败", fileID.ToString("X4"));
                base.OnTextOutput(new MsgOutEvent(m_RetVal, strMessage));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] efAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, efAsc, nRecvLen);
                string strMessage = string.Format("创建{0}文件应答：{1}", fileID.ToString("X4"), Encoding.ASCII.GetString(efAsc));
                base.OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool StroageApplicationFile()
        {
            m_ctrlApdu.createStorageApplicationCmd();
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "更新文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] appAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, appAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "更新文件应答：" + Encoding.ASCII.GetString(appAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool StoragePINFile(bool bDefaultPwd, string strCustomPwd)
        {
            byte[] PwdData = new byte[6];
            if (strCustomPwd.Length == 6)
            {
                for (int i = 0; i < 6; i++)
                    PwdData[i] = Convert.ToByte(strCustomPwd.Substring(i, 1), 10);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    PwdData[i] = 0x09;
            }
            m_ctrlApdu.createStoragePINFileCmd(bDefaultPwd, PwdData);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "安装PIN文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] pinAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, pinAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "安装PIN文件应答：" + Encoding.ASCII.GetString(pinAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool GenerateADF(string strADFName)
        {
            m_ctrlApdu.createGenerateADFCmd(strADFName);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                string strMessage = string.Format("创建ADF文件{0}失败", strADFName);
                base.OnTextOutput(new MsgOutEvent(m_RetVal, strMessage));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] efAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, efAsc, nRecvLen);
                string strMessage = string.Format("创建ADF文件{0}应答：{1}", strADFName, Encoding.ASCII.GetString(efAsc));
                base.OnTextOutput(new MsgOutEvent(0, strMessage));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        //创建加气应用ADF01
        public bool CreateADFApp()
        {
            if (!GenerateADF(m_strDIR1))
                return false;
            byte[] prefix = new byte[] { 0xA0, 0x00, 0x00, 0x00, 0x03 };
            if (!SelectFile(m_strDIR1, prefix))
                return false;
            if (!ExternalAuthentication(false))//外部认证,未离开MF主目录(1PAY.SYS.DDF01)，不使用刚安装的主控密钥
                return false;
            if (!CreateFCI())
                return false;
            byte[] param = new byte[] { 0x9F, 0x08, 0x01, 0x01, 0xBF, 0x0C, 0x02, 0x55, 0x66 };            
            if (!StorageFCI(m_strDIR1, param, prefix))
                return false;
            return true;
        }

        public bool CreateApplication(byte[] byteASN, bool bDefaultPwd, string strCustomPwd)
        {
            //公共应用基本数据文件
            if (!CreateEFFile(0xEF15, 0x1C, 0x41, 0, 0))
                return false;
            //持卡人基本数据文件
            if (!CreateEFFile(0xEF16, 0x46, 0x41, 0, 0))
                return false;
            //交易明细文件EF18 循环记录文件
            if (!CreateRecordFile(0xEF18, 0x07, 0xC8, 0x17, 0x02, 0xFFFF))
                return false;
            //气票应用普通信息数据文件
            if (!CreateEFFile(0xEF0B, 0x20, 0x01, 0, 0x02))
                return false;
            //定长记录文件EF05
            if (!CreateRecordFile(0xEF05, 0x8A, 0x01, 0x1D, 0x02, 0x7FFF))
                return false;
            StroageApplicationFile();//存盘
            //创建灰锁文件
            if (!CreateEFFileWithType(0xEF10, 0x89, 0x28, 0x41, 0, 0))
                return false;
            //气票应用敏感信息数据文件
            if (!CreateEFFile(0xEF1C, 0x60, 0x41, 0, 0))
                return false;
            //气票专用数据文件
            if (!CreateEFFile(0xEF0D, 0x40, 0x41, 0, 0))
                return false;
            //创建PIN文件
            if (!CreateEFFileWithType(0xEF03, 0x2A, 0x010F, 0, 0, 0))
                return false;
            StoragePINFile(bDefaultPwd, strCustomPwd);//PIN安装
            //气票交易密钥
            if (!CreateEFKeyFile())
                return false;
            return StorageEncryptyKey(byteASN);
        }

        private bool CreateEFKeyFile()
        {
            m_ctrlApdu.createGenerateEFKeyCmd();
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "创建气票交易密钥失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] efkeyAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, efkeyAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "创建气票交易密钥应答：" + Encoding.ASCII.GetString(efkeyAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        //安装各种密钥
        private bool StorageEncryptyKey(byte[] byteASN)
        {
            StorageKeyParam KeyInfo = null;
            byte[] keyDiversify = StorageKeyParam.GetDiversify(byteASN);
            if (keyDiversify == null)
                return false;
            //加气应用主控密钥MCMK
            KeyInfo = new StorageKeyParam("安装应用主控密钥", 0x02, 0x49, 0x00, 0xFF, 0x01);
            KeyInfo.SetParam(byteASN, m_MCMK);
            if (!storageUserKey(KeyInfo))
                return false;
            //主控密钥安装后命令的MAC需要使用主控密钥的分散密钥计算
            //加气消费密钥MPK1,并将卡号写入EF15文件
            KeyInfo = new StorageKeyParam("安装加气消费密钥1", 0x01, 0x40, 0x01, 0x33, 0x00);
            KeyInfo.SetParam(byteASN, m_MPK1);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //加气消费密钥MPK2
            KeyInfo = new StorageKeyParam("安装加气消费密钥2", 0x03, 0x40, 0x02, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MPK2);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //圈存主密钥MLK1
            KeyInfo = new StorageKeyParam("安装圈存密钥1", 0x03, 0x41, 0x01, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MLK1);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //圈存主密钥MLK2
            KeyInfo = new StorageKeyParam("安装圈存密钥2", 0x04, 0x41, 0x02, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MLK2);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //TAC主密钥MTK
            KeyInfo = new StorageKeyParam("安装TAC主密钥", 0x05, 0x42, 0x01, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MTK);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //圈提主密钥MULK
            KeyInfo = new StorageKeyParam("安装圈提主密钥", 0x06, 0x46, 0x01, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MULK);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //透支限额主密钥MUK
            KeyInfo = new StorageKeyParam("安装透支限额主密钥", 0x07, 0x47, 0x01, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MUK);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //PIN解锁主密钥MPUK
            KeyInfo = new StorageKeyParam("安装PIN解锁主密钥", 0x08, 0x43, 0x01, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MPUK);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //密码重装主密钥MRPK
            KeyInfo = new StorageKeyParam("安装密码重装主密钥", 0x09, 0x44, 0x01, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MRPK);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //应用维护主密钥MAMK
            KeyInfo = new StorageKeyParam("安装应用维护主密钥", 0x0A, 0x45, 0x01, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MAMK);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //内部认证主密钥MIAK
            KeyInfo = new StorageKeyParam("安装内部认证主密钥", 0x0B, 0x48, 0x01, 0xFF, 0x00);
            KeyInfo.SetParam(byteASN, m_MIAK);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //内部认证加气消费密钥MPK1
            KeyInfo = new StorageKeyParam("内部认证加气消费密钥1", 0x0C, 0x4F, 0x01, 0x33, 0x00);
            KeyInfo.SetParam(byteASN, m_MPK1);
            KeyInfo.SetDiversify(keyDiversify);
            if (!storageUserKey(KeyInfo))
                return false;
            //完成密钥安装，生命周期转换
            SetUserCardStatus(keyDiversify);
            return true;
        }

        private bool storageUserKey(StorageKeyParam Param)
        {
            byte[] randomVal = GetRandomValue(m_ctrlApdu, 8);
            if (randomVal == null || randomVal.Length != 8)
                return false;
            m_ctrlApdu.createWriteUserKeyCmd(randomVal, Param);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, Param.PromptInfo + "失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UserkeyAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UserkeyAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, Param.PromptInfo + "应答：" + Encoding.ASCII.GetString(UserkeyAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool SetUserCardStatus(byte[] keyCalc)
        {
            byte[] randomVal = GetRandomValue(m_ctrlApdu, 8);
            if (randomVal == null || randomVal.Length != 8)
                return false;
            m_ctrlApdu.createSetStatusCmd(randomVal, keyCalc);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
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

        private bool UpdateEF15File(byte[] key, byte[] ASN, DateTime dateBegin, DateTime dateEnd)
        {
            byte[] randomVal = GetRandomValue(m_ctrlApdu, 8);
            if (randomVal == null || randomVal.Length != 8)
                return false;
            m_ctrlApdu.createUpdateEF15FileCmd(key, randomVal, ASN, dateBegin, dateEnd);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "更新公共应用基本数据文件EF15失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UpdateFileAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UpdateFileAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "更新公共应用基本数据文件EF15应答：" + Encoding.ASCII.GetString(UpdateFileAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool UpdateEF16File(byte[] key, UserCardInfoParam cardInfo)
        {
            byte[] randomVal = GetRandomValue(m_ctrlApdu, 8);
            if (randomVal == null || randomVal.Length != 8)
                return false;
            m_ctrlApdu.createUpdateEF16FileCmd(key, randomVal, cardInfo);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "更新持卡人基本数据文件EF16失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UpdateFileAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UpdateFileAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "更新持卡人基本数据文件EF16应答：" + Encoding.ASCII.GetString(UpdateFileAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool VerifyPIN(bool bDefaultPwd, string strCustomPwd)
        {
            byte[] PwdData = new byte[6];
            if (strCustomPwd.Length == 6)
            {
                for (int i = 0; i < 6; i++)
                    PwdData[i] = Convert.ToByte(strCustomPwd.Substring(i, 1), 10);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    PwdData[i] = 0x09;
            }
            m_ctrlApdu.createVerifyPINCmd(bDefaultPwd, PwdData);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "验证PIN失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UpdateFileAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UpdateFileAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "验证PIN应答：" + Encoding.ASCII.GetString(UpdateFileAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool UpdateEF0BFile(bool bDefaultPwd)
        {
            m_ctrlApdu.createUpdateEF0BFileCmd(bDefaultPwd);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "更新普通信息数据文件EF0B失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UpdateFileAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UpdateFileAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "更新普通信息数据文件EF0B应答：" + Encoding.ASCII.GetString(UpdateFileAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool UpdateEF1CFile(byte[] key, UserCardInfoParam cardInfo)
        {
            byte[] randomVal = GetRandomValue(m_ctrlApdu, 8);
            if (randomVal == null || randomVal.Length != 8)
                return false;
            m_ctrlApdu.createUpdateEF1CFileCmd(key, randomVal, cardInfo);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "更新敏感信息数据文件EF1C失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UpdateFileAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UpdateFileAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "更新敏感信息数据文件EF1C应答：" + Encoding.ASCII.GetString(UpdateFileAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        private bool UpdateEF0DFile(byte[] key, UserCardInfoParam cardInfo)
        {
            byte[] randomVal = GetRandomValue(m_ctrlApdu, 8);
            if (randomVal == null || randomVal.Length != 8)
                return false;
            m_ctrlApdu.createUpdateEF0DFileCmd(key, randomVal, cardInfo);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "更新气票专用数据文件EF0D失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UpdateFileAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UpdateFileAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "更新气票专用数据文件EF0D应答：" + Encoding.ASCII.GetString(UpdateFileAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        public bool UpdateEF10File(byte[] key)
        {
            byte[] randomVal = GetRandomValue(m_ctrlApdu, 8);
            if (randomVal == null || randomVal.Length != 8)
                return false;
            m_ctrlApdu.createUpdateEF10FileCmd(key, randomVal);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "更新灰锁文件失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UpdateFileAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UpdateFileAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "更新灰锁文件应答：" + Encoding.ASCII.GetString(UpdateFileAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

        //更新加气应用文件
        public bool UpdateApplicationFile(UserCardInfoParam UserCardInfoPar)
        {
            //选择ADF01
            byte[] prefix = new byte[] { 0xA0, 0x00, 0x00, 0x00, 0x03 };
            if (!SelectFile(m_strDIR1, prefix))
                return false;
            byte[] byteCardId = UserCardInfoPar.GetUserCardID();
            byte[] keyUpdate = StorageKeyParam.GetUpdateEFKey(m_MAMK, byteCardId);
            if (keyUpdate == null)
                return false;
            //更新公共应用基本数据文件EF15
            if (!UpdateEF15File(keyUpdate, byteCardId, UserCardInfoPar.ValidCardBegin, UserCardInfoPar.ValidCardEnd))
                return false;
            //更新持卡人基本数据文件EF16
            if (!UpdateEF16File(keyUpdate, UserCardInfoPar))
                return false;
            //验证PIN
            if (!VerifyPIN(UserCardInfoPar.DefaultPwdFlag, UserCardInfoPar.CustomPassword))
                return false;
            //更新普通信息数据文件EF0B
            if (!UpdateEF0BFile(UserCardInfoPar.DefaultPwdFlag))
                return false;
            //敏感信息文件
            if (!UpdateEF1CFile(keyUpdate, UserCardInfoPar))
                return false;
            //气票专用文件
            if (!UpdateEF0DFile(keyUpdate, UserCardInfoPar))
                return false;
            //灰锁文件
            if (!UpdateEF10File(keyUpdate))
                return false;
            //切换生命周期
            SelectFile(m_strPSE, null);
            SetUserCardStatus(null);
            return true;
        }

        private bool InitializeForLoad(int nMoney, byte[] TermId, byte[] outData)
        {
            m_ctrlApdu.createInitializeLoadCmd(nMoney,TermId);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "圈存初始化失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] InitAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, InitAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "圈存初始化应答：" + Encoding.ASCII.GetString(InitAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
               Buffer.BlockCopy(m_RecvData, 0, outData, 0, 16);
            }
            return true;
        }

        //计算MAC2
        private byte[] CalcMAC2(byte BusinessType,int nMoneyValue, byte[] TermialID, byte[] TimeBcd, byte[] byteKey)
        {   
            byte[] srcData = new byte[18];
            byte[] byteMoney = BitConverter.GetBytes(nMoneyValue);
            srcData[0] = byteMoney[3];
            srcData[1] = byteMoney[2];
            srcData[2] = byteMoney[1];
            srcData[3] = byteMoney[0];
            srcData[4] = BusinessType;
            Buffer.BlockCopy(TermialID, 0, srcData, 5, 6);
            Buffer.BlockCopy(TimeBcd, 0, srcData, 11, 7);
            byte[] MAC2 = m_ctrlApdu.CalcMacVal(srcData, byteKey);
            return MAC2;
        }

        private bool CreditForLoad(byte[] byteMAC2, byte[] TimeBcd)
        {
            m_ctrlApdu.createCreditLoadCmd(byteMAC2, TimeBcd);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "圈存交易失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] InitAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, InitAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "圈存交易应答：" + Encoding.ASCII.GetString(InitAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
                base.OnTextOutput( new MsgOutEvent(0, "圈存TAC:" + BitConverter.ToString(m_RecvData,0,4)) );//前4字节为TAC
            }
            return true;
        }

        public bool VerifyUserPin(string strPIN)
        {
            if (!SelectFile(m_strPSE, null))
                return false;
            byte[] prefix = new byte[] { 0xA0, 0x00, 0x00, 0x00, 0x03 };
            if (!SelectFile(m_strDIR1, prefix))
                return false;
            if (!VerifyPIN(false, strPIN))
                return false;
            return true;
        }

        //圈存功能
        public bool UserCardLoad(byte[] ASN, byte[] TermId, int nMoneyValue)
        {            
            const byte BusinessType = 0x02; //交易类型标识：圈存 
            byte[] outData = new byte[16];
            byte[] SysTime = GetBCDTime();
            if (!InitializeForLoad(nMoneyValue, TermId, outData))
                return false;
            byte[] byteBalance = new byte[4];
            Buffer.BlockCopy(outData, 0, byteBalance, 0, 4);            
            byte[] OnlineSn = new byte[2];//交易序号
            Buffer.BlockCopy(outData, 4, OnlineSn, 0, 2);
            byte keyVer = (byte)outData[6];            
            byte keyFlag = (byte)outData[7];
            byte[] rand = new byte[4];
            Buffer.BlockCopy(outData, 8, rand, 0, 4);
            byte[] MAC1 = new byte[4];
            Buffer.BlockCopy(outData, 12, MAC1, 0, 4);
            //判断MAC1是否正确
            byte[] seslk = GetProcessKey(ASN, m_MLK1, rand, OnlineSn);//m_MLK1圈存主密钥1（DLK）
            if (seslk == null)
                return false;
            byte[] srcData = new byte[15];//用于计算MAC1的原始数据
            Buffer.BlockCopy(byteBalance, 0, srcData, 0, 4);
            byte[] byteMoney = BitConverter.GetBytes(nMoneyValue);
            srcData[4] = byteMoney[3];
            srcData[5] = byteMoney[2];
            srcData[6] = byteMoney[1];
            srcData[7] = byteMoney[0];
            srcData[8] = BusinessType;
            Buffer.BlockCopy(TermId, 0, srcData, 9, 6);
            byte[] MAC1Compare = m_ctrlApdu.CalcMacVal(srcData, seslk);
            if (!APDUBase.ByteDataEquals(MAC1, MAC1Compare))//MAC1检查
            {
                string strInfo = string.Format("圈存功能 Output MAC: {0} PC Calc MAC: {1}", BitConverter.ToString(MAC1), BitConverter.ToString(MAC1Compare));
                System.Diagnostics.Trace.WriteLine(strInfo);
                return false;
            }
            byte[] MAC2 = CalcMAC2(BusinessType,nMoneyValue, TermId, SysTime, seslk);
            CreditForLoad(MAC2, SysTime);
            return true;
        }



        public bool UserCardBalance(ref float fltBalance)
        {
            m_ctrlApdu.createCardBalanceCmd();
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "读取余额失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] BalAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, BalAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "读取余额应答：" + Encoding.ASCII.GetString(BalAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
                byte[] byteBalance = new byte[4];
                byteBalance[0] = m_RecvData[3];
                byteBalance[1] = m_RecvData[2];
                byteBalance[2] = m_RecvData[1];
                byteBalance[3] = m_RecvData[0];
                fltBalance = (float)(BitConverter.ToInt32(byteBalance, 0) / 100.0);                
            }
            return true;
        }

        public bool UserCardGray(ref bool bGray, ref bool bTACUF)
        {
            m_ctrlApdu.createrCardGrayCmd(false);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "读取灰锁状态失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] BalAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, BalAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "读取灰锁状态应答：" + Encoding.ASCII.GetString(BalAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;               
                bGray = m_RecvData[0] == 0x01 ? true : false;
                bTACUF = m_RecvData[0] == 0x10 ? true : false;
            }
            return true;
        }



        public bool InitForGray(byte[] TermialID, byte[] outData)
        {
            m_ctrlApdu.createrInitForGrayCmd(TermialID);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "灰锁初始化失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] InitAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, InitAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "灰锁初始化应答：" + Encoding.ASCII.GetString(InitAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
                Buffer.BlockCopy(m_RecvData, 0, outData, 0, 15);                
            }
            return true;
        }

        //加气灰锁
        public bool GrayLock(byte[] Data, byte[] outGTAC, byte[] outMAC2)
        {
            m_ctrlApdu.createrGrayLockCmd(Data);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "灰锁失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] LockAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, LockAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "灰锁应答：" + Encoding.ASCII.GetString(LockAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;                
                Buffer.BlockCopy(m_RecvData, 0, outGTAC, 0, 4);
                Buffer.BlockCopy(m_RecvData, 4, outMAC2, 0, 4);
            }
            return true;
        }

        //联机解扣初始化
        public bool InitForUnlockGreyCard(byte[] TermialID, byte[] outData)
        {
            m_ctrlApdu.createrInitForUnlockCardCmd(TermialID);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "联机解扣初始化失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] InitAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, InitAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "联机解扣初始化应答：" + Encoding.ASCII.GetString(InitAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
                Buffer.BlockCopy(m_RecvData, 0, outData, 0, 18);
            }
            return true;
        }

        //联机解扣
        public bool UnLockGrayCard(byte[] ASN, byte[] TermialID, int nUnlockMoney)
        {
            const byte BusinessType = 0x95; //交易类型标识：联机解扣
            byte[] outData = new byte[18];
            if (!InitForUnlockGreyCard(TermialID, outData))
                return false;
            byte[] SysTime = GetBCDTime();
            byte[] byteBalance = new byte[4];
            Buffer.BlockCopy(outData, 0, byteBalance, 0, 4);
            byte[] OfflineSn = new byte[2];//脱机交易序号
            Buffer.BlockCopy(outData, 4, OfflineSn, 0, 2);
            byte[] OnlineSn = new byte[2];//联机交易序号
            Buffer.BlockCopy(outData, 6, OnlineSn, 0, 2); 
            byte keyVer = outData[8];
            byte keyFlag = outData[9];
            byte[] rand = new byte[4];
            Buffer.BlockCopy(outData, 10, rand, 0, 4);
            byte[] MAC1 = new byte[4];
            Buffer.BlockCopy(outData, 14, MAC1, 0, 4);
            //判断MAC1是否正确
            byte[] sesukk = GetProcessKey(ASN, m_MULK, rand, OnlineSn);//联机解扣密钥
            if (sesukk == null)
                return false;
            byte[] srcData = new byte[13];//用于计算MAC1的原始数据
            Buffer.BlockCopy(byteBalance, 0, srcData, 0, 4);
            Buffer.BlockCopy(OfflineSn, 0, srcData, 4, 2);           
            srcData[6] = BusinessType;
            Buffer.BlockCopy(TermialID, 0, srcData, 7, 6);
            byte[] MAC1Compare = m_ctrlApdu.CalcMacVal(srcData, sesukk);
            if (!APDUBase.ByteDataEquals(MAC1, MAC1Compare))//MAC1检查
            {
                string strInfo = string.Format("联机解扣 Output MAC: {0} PC Calc MAC: {1}", BitConverter.ToString(MAC1), BitConverter.ToString(MAC1Compare));
                System.Diagnostics.Trace.WriteLine(strInfo);         
                return false;
            }
            byte[] MAC2 = CalcMAC2(BusinessType, nUnlockMoney, TermialID, SysTime, sesukk);
            UnLockForGreyCard(nUnlockMoney,MAC2, SysTime);
            return true;
        }

        private bool UnLockForGreyCard(int nUnlockMoney,byte[] byteMAC2, byte[] TimeBcd)
        {
            m_ctrlApdu.createGreyCardUnLockCmd(nUnlockMoney,byteMAC2, TimeBcd);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "联机解扣失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UnlockAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UnlockAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "联机解扣应答：" + Encoding.ASCII.GetString(UnlockAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
                base.OnTextOutput(new MsgOutEvent(0, "联机解扣MAC3:" + BitConverter.ToString(m_RecvData, 0, 4)));
            }
            return true;
        }

        public bool DebitForUnlock(byte[] byteData)
        {
            m_ctrlApdu.createDebitForUnlockCmd(byteData);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "卡解扣失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] UnlockAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, UnlockAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "卡解扣应答：" + Encoding.ASCII.GetString(UnlockAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
                base.OnTextOutput(new MsgOutEvent(0, "解扣TAC:" + BitConverter.ToString(m_RecvData, 0, 4)));
            }
            return true;
        }

        public bool ClearTACUF()
        {
            m_ctrlApdu.createrCardGrayCmd(true);
            byte[] data = m_ctrlApdu.GetOutputCmd();
            short datalen = (short)data.Length;
            Buffer.BlockCopy(m_InitByte, 0, m_RecvData, 0, 128);
            Buffer.BlockCopy(m_InitByte, 0, m_RecvDataLen, 0, 4);
            m_RetVal = DllExportMT.ExchangePro(m_MtDevHandler, data, datalen, m_RecvData, m_RecvDataLen);
            if (m_RetVal != 0)
            {
                base.OnTextOutput(new MsgOutEvent(m_RetVal, "清除TACUF失败"));
                return false;
            }
            else
            {
                uint nRecvLen = BitConverter.ToUInt32(m_RecvDataLen, 0);
                uint nAscLen = nRecvLen * 2;
                byte[] clearAsc = new byte[nAscLen];
                DllExportMT.hex_asc(m_RecvData, clearAsc, nRecvLen);
                base.OnTextOutput(new MsgOutEvent(0, "清除TACUF应答：" + Encoding.ASCII.GetString(clearAsc)));
                if (!(nRecvLen >= 2 && m_RecvData[nRecvLen - 2] == 0x90 && m_RecvData[nRecvLen - 1] == 0x00))
                    return false;
            }
            return true;
        }

    }
}
