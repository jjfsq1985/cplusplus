using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TCPServer
{
    public partial class MyServer : Form
    {
        private TcpListener m_Server;
        private bool m_bStart = false;

        private ArrayList m_ArrayClient = new ArrayList();
        private Byte[] m_DataBuffer = new Byte[32768];
        private int m_BufferLen = 0;       

        public delegate void UpdateListClient(string strIP);
        public delegate void UpdateRecvCtrl(string strText);
        public delegate void ClientDisconnect(TcpClient client);

        public MyServer()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            try
            {

                int nPort = Convert.ToInt32(textPort.Text);

                if (!m_bStart && nPort > 1024 && nPort < 65536)
                {
                    IPAddress local = IPAddress.Any;
                    m_Server = new TcpListener(local, nPort);
                    m_Server.Start();
                    m_bStart = true;
                    btnListen.Text = "Stop";
                    listClient.Items.Clear();
                    Thread ServerListen = new Thread(ServerListenThread);
                    ServerListen.Start();

                }
                else
                {
                    m_bStart = false;
                    m_Server.Stop();
                    btnListen.Text = "Listen";

                    m_ArrayClient.Clear();
                    listClient.Items.Clear();

                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void ListClientAdd(string strIP)
        {
            if (listClient.InvokeRequired)
            {
                UpdateListClient _myInvoke = new UpdateListClient(ListClientAdd);
                this.Invoke(_myInvoke, new object[] { strIP });
            } 
            else
            {
                this.listClient.Items.Add(strIP); 
            }
        }

        private void ClientRemove(TcpClient client)
        {
            if (listClient.InvokeRequired)
            {
                ClientDisconnect _myInvoke = new ClientDisconnect(ClientRemove);
                this.Invoke(_myInvoke, new object[] { client });
            }
            else
            {
                this.listClient.Items.Remove(client.Client.RemoteEndPoint.ToString());                
                client.Close();
            }
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

        private void ServerListenThread()
        {
            try
            {
                while (m_bStart)
                {
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = m_Server.AcceptTcpClient();
                    
                    m_ArrayClient.Add(client);
                    ListClientAdd(client.Client.RemoteEndPoint.ToString());
                    Thread DataRecv = new Thread(new ParameterizedThreadStart(DataRecvThread));
                    DataRecv.Start(client);

                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private bool DealWithNetData(string strIP,byte[] bytes, int nlen, out string strResponse)
        {
            bool bRet = false;
            strResponse = "";
            const string strIdle = "<IDLE>";

            if (nlen == 6 && System.Text.Encoding.UTF8.GetString(bytes, 0, 6) == strIdle)
            {
                //ÐÄÌø
                strResponse = strIdle;
                m_BufferLen = 0;
                bRet = true;
            }
            else
            {
                for (int i = 0; i < nlen; i++)
                {
                    if (bytes[i] == '\0')
                        continue;
                    m_DataBuffer[m_BufferLen++] = bytes[i]; //m_BufferLenÒÑ+1

                    if (m_DataBuffer[m_BufferLen-1] == '>' && m_DataBuffer[m_BufferLen-5] == '<' && m_BufferLen > 5)
                    {
                        string data = System.Text.Encoding.UTF8.GetString(m_DataBuffer, 0, m_BufferLen);
                        int nEnd = data.LastIndexOf("<END>");
                        if (nEnd != -1)
                        {
                            data = data.Substring(0, nEnd);
                            UpdateTextRecv(strIP + "----" + data);
                            strResponse = data;
                            m_BufferLen = 0;
                            bRet = true;
                        }
                    }                    
                }
            }

            return bRet;
        }

        private void DataRecvThread(object obj)
        {

            TcpClient client = obj as TcpClient;

            byte[] ReadBuffer = new byte[1024];
            int numberOfBytesRead = 0;

           
            String data = null;
            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = 5000;

            try
            {
                while (client.Connected)
                {
                    if ((numberOfBytesRead = stream.Read(ReadBuffer, 0, ReadBuffer.Length)) != 0)
                    {                        
                        bool bDataFull = DealWithNetData(client.Client.RemoteEndPoint.ToString(), ReadBuffer, numberOfBytesRead, out data);
                        Trace.WriteLine("Read:  " + data + "\n");

                        if (bDataFull)
                        {
                            byte[] response = System.Text.Encoding.UTF8.GetBytes(data);

                            stream.Write(response, 0, response.Length);
                            
                        }                        
                    }
 
                    Thread.Sleep(10);
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
                ClientRemove(client);
            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(listClient.SelectedIndex == -1)
                return;
            string strSend = textSend.Text + "<END>";
            TcpClient client = m_ArrayClient[listClient.SelectedIndex] as TcpClient;
            NetworkStream stream = client.GetStream();
            byte[] byteSend = System.Text.Encoding.UTF8.GetBytes(strSend);            
            stream.Write(byteSend, 0, byteSend.Length);            
        }
    }
}