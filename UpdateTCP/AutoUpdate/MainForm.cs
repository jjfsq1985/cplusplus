using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutoUpdate
{
    public enum UpdateFileType
    {
        eUnknown = 0,
        ePcd,   //PCD程序
        eKeyboard,  //键盘
        eMainboard,  //主板
        eFontLibrary  //字库
    }

    public partial class MainForm : Form
    {
        public static ManualResetEvent m_AllDown = new ManualResetEvent(false);

        private bool m_bStart = false;
        private bool m_bBackground = false;
        private Socket m_listen = null;
                
        private int m_nUpdateDevCount = 0;

        List<UpdateFileParam> lstUpdateFile = new List<UpdateFileParam>();
        BuildUpdateData updateObj = new BuildUpdateData();

        public delegate void RemoteIPHandler(string strContent);

        //UDP广播线程参数
        public class UpdThreadParam
        {
            public UdpClient Client = null;
            public IPEndPoint UdpEP = null;
            public IPAddress LocalAddr = IPAddress.None;
            public int TcpPort = 0;
            public List<UpdateFileParam> lstAllUpdateFile = null;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void StartTcp(IPAddress localaddr, int nPort)
        {
            if (!m_bStart)
            {
                IPEndPoint endPoint = new IPEndPoint(localaddr, nPort);
                m_listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_listen.Bind(endPoint);
                m_listen.Listen(10);

                m_bStart = true;
                Thread ServerListen = new Thread(new ParameterizedThreadStart(StartListening));
                ServerListen.Start(m_listen);
            }
            else
            {
                bool bClose = true;
                if (m_nUpdateDevCount > 0)
                {
                    if (MessageBox.Show("仍有设备在升级，确定要停止?", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        bClose = false;
                    }
                }
                if (bClose)
                {
                    m_bStart = false;
                    m_bBackground = false;
                    m_AllDown.Set();
                    m_listen.Close();
                }
            }
        }

        private void StartUdpBroadcast(IPAddress localaddr, int nPort, int nPortInData)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            UdpClient broadcastUdp = new UdpClient(endPoint);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), nPort);
            //后台线程参数
            UpdThreadParam ThreadPar = new UpdThreadParam();
            ThreadPar.Client = broadcastUdp;
            ThreadPar.UdpEP = endpoint;
            ThreadPar.LocalAddr = localaddr;
            ThreadPar.TcpPort = nPortInData;
            ThreadPar.lstAllUpdateFile = lstUpdateFile;
            m_bBackground = true;
            Thread t = new Thread(new ParameterizedThreadStart(BroadcastThread));
            t.IsBackground = true;
            t.Start(ThreadPar); 
        }

        private byte[] GetVerData(UpdateFileParam fileInfo)
        {
            if (string.IsNullOrEmpty(fileInfo.FileVersion))
                return null;
            int nLen = fileInfo.FileVersion.Length + 3;
            byte[] retVal = new byte[nLen];
            retVal[0] = (byte)fileInfo.eType;
            retVal[1] = (byte)fileInfo.FileVersion.Length;
            byte[] VerData = Encoding.ASCII.GetBytes(fileInfo.FileVersion);
            Buffer.BlockCopy(VerData, 0, retVal, 2, VerData.Length);
            retVal[nLen - 1] = 0x00;//结束符
            return retVal;
        }

        public byte[] BuildUDPPacket(UpdThreadParam param)
        {
            byte[] ipAddr = param.LocalAddr.GetAddressBytes();
            if(ipAddr.Length != 4)
                return null;
            List<byte> bytePacket = new List<byte>();
            bytePacket.Add(0x02);
            bytePacket.Add(ipAddr[0]);
            bytePacket.Add(ipAddr[1]);
            bytePacket.Add(ipAddr[2]);
            bytePacket.Add(ipAddr[3]);
            byte[] port = BitConverter.GetBytes((ushort)param.TcpPort);
            bytePacket.Add(port[0]);
            bytePacket.Add(port[1]);            

            ushort VerDataLen = 0;
            List<byte> byteVer = new List<byte>();
            for(int i=0; i< param.lstAllUpdateFile.Count; i++)
            {
                byte[] VerData = GetVerData(param.lstAllUpdateFile[i]);
                if(VerData != null)
                {
                    byteVer.AddRange(VerData);
                    VerDataLen += (ushort)VerData.Length;
                }
            }

            byte[] byteVerLen = BitConverter.GetBytes(VerDataLen);
            bytePacket.Add(byteVerLen[0]);
            bytePacket.Add(byteVerLen[1]); 
            bytePacket.AddRange(byteVer);
            bytePacket.Add(0x03);
            byte xorByte = GetXorValue(bytePacket,1,bytePacket.Count-2);
            bytePacket.Add(xorByte);
            return bytePacket.ToArray();
        }

        public void BroadcastThread(object obj)
        {
            UpdThreadParam Par = obj as UpdThreadParam;

            //生成广播数据包
            byte[] PacketData = BuildUDPPacket(Par);
            if (PacketData == null)
                return;

            while(m_bBackground)
            {
                Par.Client.Send(PacketData, PacketData.Length, Par.UdpEP);
                Thread.Sleep(500);
            }
        }

        //获取使用中的有效IPv4型IP
        private IPAddress GetLocalIPv4()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach(NetworkInterface adapter in nics)
            {
                if(adapter.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties ip = adapter.GetIPProperties();
                    //单播地址集                    
                    foreach (UnicastIPAddressInformation ipaddr in ip.UnicastAddresses)
                    {
                        if (ipaddr.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ipaddr.Address;
                        }
                    }

                }
            }
            return IPAddress.None;
        }

        private void StartServer_Click(object sender, EventArgs e)
        {
            LabelPrompt.Text = "";
            //读取升级文件版本
            ReadUpdateFileVersion();
            if(lstUpdateFile.Count == 0)
            {
                LabelPrompt.Text = "未找到更新文件";
                return;
            }

            int nUdpPort = Convert.ToInt32(textUdpPort.Text);
            int nTcpPort = Convert.ToInt32(textTcpPort.Text);

            IPAddress localaddr = GetLocalIPv4();

            lstClient.Items.Clear();
            StartUdpBroadcast(localaddr, nUdpPort, nTcpPort);
            StartTcp(localaddr, nTcpPort);
            if (m_bStart)
            {
                ServerOnOff.Text = "停止";
            }
            else
            {
                ServerOnOff.Text = "启动";
            }
        }

        public void StartListening(object obj)
        {
            Socket listen = obj as Socket;
            try
            {
                while (m_bStart)
                {
                    m_AllDown.Reset();
                    listen.BeginAccept(new AsyncCallback(AcceptCallback), listen);
                    m_AllDown.WaitOne();
                }

            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }


        public void SetLstContent(string strContent)
        {
            if (this.lstClient.InvokeRequired)
            {
                RemoteIPHandler RemoteIp = new RemoteIPHandler(SetLstContent);
                this.Invoke(RemoteIp, new object[] { strContent });
            }
            else
            {
                this.lstClient.Items.Add(strContent);
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            if (!m_bStart)
                return;
            m_AllDown.Set();
            Socket listen = (Socket)ar.AsyncState;
            Socket work = listen.EndAccept(ar);

            SetLstContent(work.RemoteEndPoint.ToString());

            StateObject obj = new StateObject();
            obj.WorkSocket = work;
            work.BeginReceive(obj.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(RecvCallback), obj);
        }

        public void RecvCallback(IAsyncResult ar)
        {
            try
            {                
                StateObject state = (StateObject)ar.AsyncState;
                Socket work = state.WorkSocket;
                int nByteRecv = work.EndReceive(ar);
                if (nByteRecv > 0)
                {
                    //处理state.Buffer中接收到的数据
                    state.RecvLen += nByteRecv;
                    ParseRecvData(state);
                    //继续接收
                    work.BeginReceive(state.Buffer, state.RecvLen, StateObject.BufferSize - state.RecvLen, SocketFlags.None, new AsyncCallback(RecvCallback), state);
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void ReadUpdateFileVersion()
        {
            lstUpdateFile.Clear();
            string[] updatefiles = Directory.GetFiles(Application.StartupPath, "*.bin",SearchOption.TopDirectoryOnly);
            foreach (string strFileName in updatefiles)
            {
                //文件名固定n-ver.bin(如键盘程序为2-K.ZHY.1.23.48.bin)
                int nPos = strFileName.LastIndexOf("\\") + 1;
                string strFileParse = strFileName.Substring(nPos, strFileName.LastIndexOf(".bin") - nPos);
                int nType = Convert.ToInt32(strFileParse.Substring(0, 1));
                string strVersion = strFileParse.Substring(2, strFileParse.Length - 2);
                if (nType >= 1 && nType <= 4)
                {
                    UpdateFileParam par = new UpdateFileParam();
                    FileInfo fileInfo = new FileInfo(strFileName);
                    par.eType = (UpdateFileType)nType;
                    par.strFileFullPath = strFileName;
                    par.FileLen = Convert.ToInt32(fileInfo.Length);
                    par.FileVersion = strVersion;
                    lstUpdateFile.Add(par);
                }
            }
        }

        private byte GetXorValue(List<byte> byteData, int nIndex, int nLen)
        {
            byte ret = 0;
            for (int i = nIndex; i < nLen; i++)
            {
                ret ^= byteData[i];
            }
            return ret;
        }


        private bool ParseRecvData(StateObject recvData)
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
                    int nRepeat = GetRepeatCount(recvData.Buffer,recvData.RecvLen);
                    if (recvData.RecvLen < nLen + 8 + nRepeat + i)
                        break;
                    byte[] Data = RemoveRepeat(recvData.Buffer,recvData.RecvLen,i,nLen + 8);
                    nThrow += (nLen + 8 + nRepeat);
                    ushort crcRecv = Convert.ToUInt16((Data[nLen + 6] << 8) + Data[nLen + 7]);//CRC高位在前
                    if (CalcCrc(Data, 1, nLen + 5) != crcRecv)
                        break;
                    //解析有效数据
                    DealwithData(recvData.WorkSocket,Data);
                    break;
                }
            }

            if(nThrow > 0)
            {
                int nRemain = recvData.RecvLen - nThrow;
                if(nRemain > 0)
                {
                    byte[] nMoveLen = new byte[nRemain];
                    Buffer.BlockCopy(recvData.Buffer, nThrow, nMoveLen, 0, nRemain);
                }
                recvData.RecvLen = nRemain;
            }
            return true;
        }

        private int GetRepeatCount(byte[] byteData, int nLen)
        {
            int nRepeatCount = 0;
            bool bRepeat = false;
            for(int i=0; i< nLen-1; i++)
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
        
        private byte[] RemoveRepeat(byte[] byteData,int nLen,int nIndex, int nValidLen)
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

        private void DealwithData(Socket tcpSock,byte[] DeviceData)
        {
            const int nSegmentSize = 1024;
            byte byteOp = DeviceData[6];
            switch(byteOp)
            {
                case 0x2D:
                    {
                        UpdateFileParam UpdatePar = GetUpdateFile(DeviceData[7]);
                        if (UpdatePar == null)
                            break;
                        byte[] buildData = updateObj.CreateHeader(UpdatePar, nSegmentSize, m_nUpdateDevCount);
                        byte[] sendData = BuildSendData(buildData);
                        tcpSock.Send(sendData);
                        m_nUpdateDevCount++;
                    }
                    break;
                case 0x2E:
                    {
                        UpdateFileParam UpdatePar = GetUpdateFile(DeviceData[7]);
                        if (UpdatePar == null)
                            break;
                        if (DeviceData[8] != 0x00 && m_nUpdateDevCount > 0)
                            m_nUpdateDevCount--;
                        int nOffset = BitConverter.ToInt32(DeviceData, 9);
                        if (nOffset < UpdatePar.FileLen && m_nUpdateDevCount > 0)
                        {
                            byte[] buildData = updateObj.CreateBody(UpdatePar, nOffset, nSegmentSize);
                            byte[] sendData = BuildSendData(buildData);
                            tcpSock.Send(sendData);
                        }
                    }
                    break;
                default:
                    break;
            }

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
            int i=1;
            while (i < retData.Count)
            {
                if(retData[i] == 0xFA)
                {
                    retData.Insert(i, 0xFA);//插在前面
                    i++;
                }
                i++;
            }
            return retData.ToArray();
        }

        private UpdateFileParam GetUpdateFile(byte nType)
        {
            for(int i=0; i< lstUpdateFile.Count; i++)
            {
                if(lstUpdateFile[i].eType == (UpdateFileType)nType)
                {
                    return lstUpdateFile[i];
                }

            }
            return null;
        }
    }

    

    /// <summary>
    /// 数据处理线程结构体
    /// </summary>
    public class StateObject
    {
        public Socket WorkSocket = null;
        public const int BufferSize = 2048;        
        public int RecvLen = 0;
        public byte[] Buffer = new byte[BufferSize];       
    }

    public class UpdateFileParam
    {
        public UpdateFileType eType = UpdateFileType.eUnknown;
        public string strFileFullPath;
        public int FileLen = 0;
        public string FileVersion = "";
    }

}
