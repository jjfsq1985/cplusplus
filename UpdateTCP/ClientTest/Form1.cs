using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClientTest
{
    public enum UpdateFileType
    {
        eUnknown = 0,
        ePcd,   //PCD程序
        eKeyboard,  //键盘
        eMainboard,  //主板
        eFontLibrary  //字库
    }

    public partial class Form1 : Form
    {
        private bool m_bRecvUdp = false;
        private bool m_bWantConnect = false;
        private bool m_bUpdating = false;
        private TcpClient m_Client = null;

        public bool m_bResponseTcp = false;        
        List<VersionObject> m_lstLocalVersion = new List<VersionObject>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            m_bRecvUdp = true;
            Thread t = new Thread(new ThreadStart(RecvUdp));
            t.IsBackground = true;
            t.Start();
        }

        public void RecvUdp()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 4646);
            UdpClient UdpObj = new UdpClient(remoteEP);
            IPEndPoint recvEP = new IPEndPoint(IPAddress.Any, 0);
            while (m_bRecvUdp)
            {
                byte[] data = UdpObj.Receive(ref recvEP);
                if (data != null && data.Length > 9)
                {
                    int nlen = (data[8]<< 8) + data[7];
                    //计算多一个异或校验
                    if(data.Length >= nlen + 11 && data[0] == 0x02 && data[nlen + 9] == 0x03)
                    {
                            byte[] ip = new byte[4];
                            ip[0] = data[1];
                            ip[1] = data[2];
                            ip[2] = data[3];
                            ip[3] = data[4];
                            int nPort = (data[6] << 8) + data[5];
                            IPAddress ipAddr = new IPAddress(ip);
                            byte[] verData = new byte[nlen];
                            Buffer.BlockCopy(data, 9, verData, 0, nlen);                            
                            ParseUdpData(verData);
                            if (m_bWantConnect)
                            {
                                ConnectTcp(ipAddr, nPort);
                                m_bWantConnect = false;
                            }
                    }
                }

                Thread.Sleep(200);
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_bRecvUdp = false;
        }

        private void ConnectTcp(IPAddress remoteIp, int nPort)
        {
            try
            {
                if (m_Client != null && m_Client.Connected)
                    return;
                m_Client = new TcpClient(AddressFamily.InterNetwork);
                m_Client.Connect(remoteIp, nPort);                
                if (m_Client.Connected)
                {                    
                    StateObject  obj = new StateObject();
                    obj.WorkStream = m_Client.GetStream();
                    obj.WorkStream.BeginRead(obj.Buffer, 0, StateObject.BufferSize, new AsyncCallback(RecvCallback), obj);

                    m_bUpdating = true;
                    Thread SendTh = new Thread(new ThreadStart(TcpSendThread));
                    SendTh.Start();
                }

            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public void ParseUdpData(byte[] TcpData)
        {
            List<VersionObject> allUpdate = new List<VersionObject>();

            int i = 0;
            while (i < TcpData.Length)
            {
                byte eType = TcpData[i];
                byte VerLen = TcpData[i + 1];
                string strVer = Encoding.ASCII.GetString(TcpData, i + 2, VerLen);

                VersionObject obj = new VersionObject();
                obj.eType = (UpdateFileType)eType;
                obj.strVersion = strVer;
                allUpdate.Add(obj);
                i += VerLen + 3; //多带一个'\0'
            }
            if (allUpdate.Count <= 0)
                return;
            for (i = 0; i < allUpdate.Count; i++)
            {
                EditLstObj(allUpdate[i]);
            }
            if (!m_bUpdating)
            {
                for (i = 0; i < m_lstLocalVersion.Count; i++)
                {
                    if (!m_lstLocalVersion[i].bUpdated)
                    {
                        m_bWantConnect = true;
                        break;
                    }
                }
            }
          
        }

        public void EditLstObj(VersionObject newObj)
        {
            bool bFind = false;
            for (int i = 0; i < m_lstLocalVersion.Count; i++)
            {
                if(newObj.eType == m_lstLocalVersion[i].eType)
                {
                    bFind = true;
                    if(newObj.strVersion == m_lstLocalVersion[i].strVersion)
                    {
                        break;
                    }
                    else 
                    {
                        m_lstLocalVersion[i].strVersion = newObj.strVersion;
                        m_lstLocalVersion[i].bUpdated = false;
                        m_lstLocalVersion[i].nFileOffset = 0;
                        m_lstLocalVersion[i].nFileSize = 0;
                        
                        break;
                    }
                }
            }
            if(!bFind)
            {
                m_lstLocalVersion.Add(newObj);
            }
        }

        //计算CRC, 多项式为0x5010,(不使用0xA001)
        private ushort CalcCrc(byte[] data, int nIndex, int nLen)
        {
            byte lsb0 = 0;
            byte lsb1 = 0;

            byte crcHigh = 0;
            byte crcLow = 0;

            for (int i = nIndex; i < nLen; i++)
            {
                crcHigh ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    lsb0 = Convert.ToByte(crcLow & 1);
                    crcLow >>= 1;
                    lsb1 = Convert.ToByte(crcHigh & 1);
                    crcHigh >>= 1;
                    if (lsb0 != 0)
                    {
                        crcHigh |= 0x80;
                    }
                    if (lsb1 != 0)
                    {
                        crcLow ^= 0x50;
                        crcHigh ^= 0x10;
                    }
                }
            }

            return Convert.ToUInt16((crcHigh << 8) + crcLow);
        }

        private byte[] BuildSendData(byte[] srcData)
        {
            ushort crc16 = CalcCrc(srcData, 1, srcData.Length - 1);
            int nLen = srcData.Length + 2;
            List<byte> retData = new List<byte>();
            retData.AddRange(srcData);
            //CRC高位在前
            retData.Add(Convert.ToByte((crc16 >> 8) & 0xFF));
            retData.Add(Convert.ToByte(crc16 & 0xFF));
            int i = 1;
            while (i < retData.Count)
            {
                if (retData[i] == 0xFA)
                {
                    retData.Insert(i, 0xFA);//插在前面
                    i++;
                }
                i++;
            }
            return retData.ToArray();
        }


        private void requestFileLen(int nType)
        {
            byte[] buildData = new byte[8];
            buildData[0] = 0xFA;
            buildData[1] = 0;
            buildData[2] = 0;
            buildData[3] = 0;
            buildData[4] = 0x00;
            buildData[5] = 0x02;
            buildData[6] = 0x2D;
            buildData[7] = (byte)nType;
            byte[] sendData = BuildSendData(buildData);        
            NetworkStream stream = m_Client.GetStream();
            stream.Write(sendData, 0, sendData.Length);
        }

        private void requestFileData(int nType,bool bCompleted, int nFileOffset)
        {
            byte[] buildData = new byte[13];
            buildData[0] = 0xFA;
            buildData[1] = 0;
            buildData[2] = 0;
            buildData[3] = 0;
            buildData[4] = 0x00;
            buildData[5] = 0x07;
            buildData[6] = 0x2E;
            buildData[7] = (byte)nType;
            buildData[8] = bCompleted ? (byte)1 : (byte)0;
            byte[] fileSeek = BitConverter.GetBytes(nFileOffset);
            Buffer.BlockCopy(fileSeek, 0, buildData, 9, 4);
            byte[] sendData = BuildSendData(buildData);
            NetworkStream stream = m_Client.GetStream();
            stream.Write(sendData, 0, sendData.Length);
        }

        //tcp 发送
        public void TcpSendThread()
        {
            bool bNewFile = false;
            int nFile = 0;
            while (m_bUpdating)
            {
                if (!bNewFile)
                {
                    for (int i = 0; i < m_lstLocalVersion.Count; i++)
                    {
                        if (m_lstLocalVersion[i].bUpdated)
                            continue;
                        bNewFile = true;
                        nFile = i;
                        requestFileLen((int)(m_lstLocalVersion[i].eType));
                        m_lstLocalVersion[i].nFileOffset = 0;
                        m_lstLocalVersion[i].nFileSize = 0;
                        break;
                    }
                }
                else if (m_bResponseTcp)
                {
                    requestFileData((int)(m_lstLocalVersion[nFile].eType), m_lstLocalVersion[nFile].bUpdated, m_lstLocalVersion[nFile].nFileOffset);
                    m_bResponseTcp = false;
                }

                Thread.Sleep(100);

            }
        }

        private int GetRepeatCount(byte[] byteData, int nLen)
        {
            int nRepeatCount = 0;
            bool bRepeat = false;
            for (int i = 0; i < nLen - 1; i++)
            {
                if (!bRepeat && (byteData[i] == 0xFA) && (byteData[i + 1] == 0xFA))
                {
                    bRepeat = true;
                    nRepeatCount++;
                    continue;
                }
                bRepeat = false;
            }
            return nRepeatCount;
        }

        private byte[] RemoveRepeat(byte[] byteData, int nLen, int nIndex, int nValidLen)
        {
            //两个FA,存一个
            byte[] copyData = new byte[nValidLen];
            bool bRepeat = false;
            int nPosition = 0;
            int i = 0;
            for (i = nIndex; i < nLen - 1; i++)
            {
                if (nPosition >= nValidLen - 1)
                    break;
                if (!bRepeat && (byteData[i] == 0xFA) && (byteData[i + 1] == 0xFA))
                {
                    bRepeat = true;
                    continue;
                }
                bRepeat = false;
                copyData[nPosition] = byteData[i];
                nPosition++;
            }
            //最后一个字节
            copyData[nPosition] = byteData[i];

            return copyData;
        }

        public bool ParseRecvData(StateObject recvData)
        {
            if (recvData.RecvLen < 8)
                return false;
            int nThrow = 0;
            for (int i = 0; i < recvData.RecvLen; i++)
            {
                if (recvData.Buffer[i] == 0xFA)
                {
                    nThrow = i;
                    int nLen = Convert.ToInt32(recvData.Buffer[4].ToString("X2") + recvData.Buffer[5].ToString("X2"));//BCD to int
                    int nRepeat = GetRepeatCount(recvData.Buffer, recvData.RecvLen);
                    if (recvData.RecvLen < nLen + 8 + nRepeat + i)
                        break;
                    byte[] Data = RemoveRepeat(recvData.Buffer, recvData.RecvLen, i, nLen + 8);
                    nThrow += (nLen + 8 + nRepeat);
                    ushort crcRecv = Convert.ToUInt16((Data[nLen + 6] << 8) + Data[nLen + 7]);//CRC高位在前
                    if (CalcCrc(Data, 1, nLen + 5) != crcRecv)
                        break;
                    //解析有效数据
                    DealwithData(Data);
                    break;
                }
            }

            if (nThrow > 0)
            {
                int nRemain = recvData.RecvLen - nThrow;
                if (nRemain > 0)
                {
                    byte[] nMoveLen = new byte[nRemain];
                    Buffer.BlockCopy(recvData.Buffer, nThrow, nMoveLen, 0, nRemain);
                }
                recvData.RecvLen = nRemain;
            }
            return true;
        }

        //tcp 接收
        public void RecvCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                int nByteRecv = state.WorkStream.EndRead(ar);
                if (nByteRecv > 0)
                {
                    //处理state.Buffer中接收到的数据
                    state.RecvLen += nByteRecv;
                    ParseRecvData(state);
                    //继续接收
                    state.WorkStream.BeginRead(state.Buffer, state.RecvLen, StateObject.BufferSize - state.RecvLen, new AsyncCallback(RecvCallback), state);
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void FileSizeToList(UpdateFileType eType, int nSize)
        {
            for(int i=0; i< m_lstLocalVersion.Count; i++)
            {
                if(m_lstLocalVersion[i].eType == eType)
                {
                    m_lstLocalVersion[i].nFileSize = nSize;
                    string strFileName = Application.StartupPath + @"\" + m_lstLocalVersion[i].strVersion + ".bin";
                    if (File.Exists(strFileName))
                        File.Delete(strFileName);
                    break;
                }
            }
        }


        private void FileOffsetToList(UpdateFileType eType, int nOffset)
        {            
            for (int i = 0; i < m_lstLocalVersion.Count; i++)
            {
                if (m_lstLocalVersion[i].eType == eType)
                {                    
                    m_lstLocalVersion[i].nFileOffset = nOffset;
                    break;
                }
            }
        }

        private UInt16 BcdToUInt16(byte[] Data, int nIndex)
        {
            try
            {
                byte byteH = Data[nIndex];
                byte byteL = Data[nIndex + 1];
                string strVal = byteH.ToString("X2") + byteL.ToString("X2");
                return Convert.ToUInt16(strVal);
            }
            catch 
            {
                return 0;
            }
        }

        private void DealwithData(byte[] DeviceData)
        {
            int nLen = (int)BcdToUInt16(DeviceData,4);            
            byte byteOp = DeviceData[6];
            switch (byteOp)
            {
                case 0x2D:
                    {
                        UpdateFileType eFileType = (UpdateFileType)DeviceData[7];
                        if (DeviceData[8] != 0)
                            break;
                        int nFileSize = BitConverter.ToInt32(DeviceData, 9);
                        FileSizeToList(eFileType, nFileSize);
                        m_bResponseTcp = true;
                    }
                    break;
                case 0x2E:
                    {
                        UpdateFileType eFileType = (UpdateFileType)DeviceData[7];
                        string strFileName = GetFileName(eFileType);
                        int nOffset = BitConverter.ToInt32(DeviceData, 8);
                        int nFileSize = GetFileSize(eFileType);
                        if (nOffset < nFileSize && !string.IsNullOrEmpty(strFileName))
                        {
                            WriteBinFile(strFileName,DeviceData,12,nLen-6);
                            nOffset += (nLen - 6);
                            FileOffsetToList(eFileType, nOffset);
                            if (nOffset < nFileSize)
                            {
                                m_bResponseTcp = true;
                            }
                            else
                            {
                                m_bUpdating = false;
                                requestFileData((int)(eFileType), true, nOffset);
                                FileCompleted(eFileType);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

        }


        private string GetFileName(UpdateFileType eType)
        {
            string strName = "";
            for (int i = 0; i < m_lstLocalVersion.Count; i++)
            {
                if (m_lstLocalVersion[i].eType == eType)
                {
                    strName = m_lstLocalVersion[i].strVersion;
                    break;
                }
            }
            return strName;
        }

        private int GetFileSize(UpdateFileType eType)
        {
            int nSize = 0;
            for (int i = 0; i < m_lstLocalVersion.Count; i++)
            {
                if (m_lstLocalVersion[i].eType == eType)
                {
                    nSize = m_lstLocalVersion[i].nFileSize;
                    break;
                }
            }
            return nSize;
        }

        private void FileCompleted(UpdateFileType eType)
        {   
            for (int i = 0; i < m_lstLocalVersion.Count; i++)
            {
                if (m_lstLocalVersion[i].eType == eType)
                {
                    m_lstLocalVersion[i].bUpdated = true;
                    break;
                }
            }            
        }


        private void WriteBinFile(string strFile, byte[] data, int nIndex, int nLen)
        {
            string strFileName = Application.StartupPath + @"\" + strFile + ".bin";
            FileStream fs = File.Open(strFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            fs.Seek(0, SeekOrigin.End);
            fs.Write(data, nIndex, nLen);
            fs.Close();
        }

    }

    public class VersionObject
    {
        public UpdateFileType eType = UpdateFileType.eUnknown;
        public string strVersion;
        public bool bUpdated = false;
        public int nFileOffset = 0;
        public int nFileSize = 0;
    }

    public class StateObject
    {
        public NetworkStream WorkStream = null;
        public const int BufferSize = 2048;        
        public int RecvLen = 0;
        public byte[] Buffer = new byte[BufferSize];       
    }

}
