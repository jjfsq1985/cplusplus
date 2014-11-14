using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;


namespace MyTcpClient
{
    public partial class Form1 : Form
    {
        private TcpClient m_Client = null;
        private Byte[] m_DataBuffer = new Byte[32768];
        private int m_BufferLen = 0;

        public delegate void UpdateRecvCtrl(string strText);
        public delegate void ClientDisconnect();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {

                if (m_Client != null && m_Client.Connected)
                {
                    m_Client.Close();
                    btnConnect.Text = "Connect";
                }
                else
                {
                    int nPort = Convert.ToInt32(textPort.Text);
                    if (nPort <= 1024 || nPort >= 65536)
                        return;
                    m_Client = new TcpClient(textIPAddr.Text, nPort);
                    if (m_Client.Connected)
                    {
                        btnConnect.Text = "Disconnect";
                        Thread ServerListen = new Thread(RecvThread);
                        ServerListen.Start();
                        Thread HeartBeat = new Thread(HeartBeatThread);
                        HeartBeat.Start();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void RecvThread()
        {

            byte[] ReadBuffer = new byte[1024];
            int numberOfBytesRead = 0;

            NetworkStream stream = m_Client.GetStream();
            stream.ReadTimeout = 5000;

            try
            {
                while (m_Client.Connected)
                {                    
                    if ((numberOfBytesRead = stream.Read(ReadBuffer, 0, ReadBuffer.Length)) != 0)
                    {
                        DealWithNetData(m_Client.Client.RemoteEndPoint.ToString(), ReadBuffer, numberOfBytesRead);
                    }

                    Thread.Sleep(10);
                }
                                
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Disconnect();
            }
        }

        private void HeartBeatThread()
        {
            const string strIdle = "<IDLE>";
            byte[] Buffer = Encoding.UTF8.GetBytes(strIdle);            

            NetworkStream stream = m_Client.GetStream();
            stream.WriteTimeout = 500;

            int nCount = 0;

            try
            {
                //1秒一次
                while (m_Client.Connected)
                {
                    if (nCount > 1)
                    {
                        stream.Write(Buffer, 0, Buffer.Length);
                        nCount = 0;
                    }
                    nCount++;
                    Thread.Sleep(500);
                }

            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Disconnect();
            }

        }

        private void Disconnect()
        {
            if (btnConnect.InvokeRequired)
            {
                ClientDisconnect _myInvoke = new ClientDisconnect(Disconnect);
                this.Invoke(_myInvoke);
            }
            else
            {                
                m_Client.Close();
                this.btnConnect.Text = "Connect";
            }
        }

        private void DealWithNetData(string strIP, byte[] bytes, int nlen)
        {
            const string strIdle = "<IDLE>";            

            if (nlen == 6 && System.Text.Encoding.UTF8.GetString(bytes, 0, 6) == strIdle)
            {
                //心跳
                m_BufferLen = 0;
            }
            else
            {                
                for (int i = 0; i < nlen; i++)
                {
                    if (bytes[i] == '\0')
                        continue;
                    m_DataBuffer[m_BufferLen++] = bytes[i];//m_BufferLen已+1

                    if (m_DataBuffer[m_BufferLen-1] == '>' && m_DataBuffer[m_BufferLen-5] == '<' && m_BufferLen > 5)
                    {
                        string data = System.Text.Encoding.UTF8.GetString(m_DataBuffer, 0, m_BufferLen);
                        int nEnd = data.LastIndexOf("<END>");
                        if (nEnd != -1)
                        {
                            data = data.Substring(0, nEnd);
                            UpdateTextRecv(strIP + "----" + data);
                            m_BufferLen = 0;
                        }
                    }                    
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!m_Client.Connected)
                return;
            string strSend = textSend.Text + "<END>";            
            NetworkStream stream = m_Client.GetStream();
            byte[] byteSend = System.Text.Encoding.UTF8.GetBytes(strSend);            
            stream.Write(byteSend, 0, byteSend.Length);            
        }

        private void UpdateTextRecv(string strText)
        {
            if (textRecv.InvokeRequired)
            {
                UpdateRecvCtrl _myInvoke = new UpdateRecvCtrl(UpdateTextRecv);
                this.Invoke(_myInvoke, new object[] { strText });
            }
            else
            {
                this.textRecv.AppendText(strText + "\n");
            }
        }
    }
}