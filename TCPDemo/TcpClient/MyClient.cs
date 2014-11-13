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

            try
            {
                while (m_Client.Connected)
                {

                    // Loop to receive all the data sent by the client.
                    //while ((charData = stream.ReadByte()) != -1)
                    while ((numberOfBytesRead = stream.Read(ReadBuffer, 0, ReadBuffer.Length)) != 0)
                    {
                        DealWithNetData(m_Client.Client.RemoteEndPoint.ToString(), ReadBuffer, numberOfBytesRead);
                    }

                    if (m_Client.Available == 0)
                        throw new System.Exception("socket close");

                    Thread.Sleep(1);
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
            if (nlen == 1 && bytes[0] == '\0')
            {
                m_BufferLen = 0;
            }
            else
            {
                for (int i = 0; i < nlen; i++)
                {
                    m_DataBuffer[m_BufferLen++] = bytes[i];
                    if (bytes[i] == '\n' && m_BufferLen > 0)
                    {
                        string data = System.Text.Encoding.ASCII.GetString(m_DataBuffer, 0, m_BufferLen);
                        UpdateTextRecv(strIP + "----" + data);                        
                        m_BufferLen = 0;
                    }
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!m_Client.Connected)
                return;
            string strSend = textSend.Text + "\n";            
            NetworkStream stream = m_Client.GetStream();
            byte[] byteSend = System.Text.Encoding.ASCII.GetBytes(strSend);            
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