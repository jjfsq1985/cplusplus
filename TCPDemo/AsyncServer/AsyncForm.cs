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

namespace AsyncServer
{
    public partial class AsyncForm : Form
    {
        public static ManualResetEvent m_AllDown = new ManualResetEvent(false);

        public delegate void RecvText(string strRecv);

        private bool m_bStart = false;
        private Socket m_listen = null;

        public AsyncForm()
        {
            InitializeComponent();
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



        public void AcceptCallback(IAsyncResult ar)
        {
            if (!m_bStart)
                return;
            m_AllDown.Set();
            Socket listen = (Socket)ar.AsyncState;
            Socket work = listen.EndAccept(ar);

            StateObject obj = new StateObject();
            obj.WorkSocket = work;
            work.BeginReceive(obj.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(RecvCallback), obj);

        }

        public void RecvCallback(IAsyncResult ar)
        {
            try
            {
                string strContent = String.Empty;
                StateObject state = (StateObject)ar.AsyncState;
                Socket work = state.WorkSocket;
                int nByteRecv = work.EndReceive(ar);
                if (nByteRecv > 0)
                {
                    state.recvString.Append(Encoding.UTF8.GetString(state.Buffer, 0, nByteRecv));

                    strContent = state.recvString.ToString();
                    if (strContent.LastIndexOf("<IDLE>") != -1)
                    {
                        //心跳
                        state.recvString.Remove(0, strContent.LastIndexOf("<IDLE>")+6);
                        SendHeartBeat(work, strContent);
                    }
                    else if (strContent.LastIndexOf("<END>") != -1)
                    {
                        //接收完成
                        state.recvString.Remove(0, strContent.LastIndexOf("<END>") + 5);
                        DisplayRecv(strContent.Substring(0, strContent.LastIndexOf("<END>")));
                    }
                    work.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(RecvCallback), state);
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public void DisplayRecv(string strRecv)
        {
            if (textAsyncRecv.InvokeRequired)
            {
                RecvText _myInvoke = new RecvText(DisplayRecv);
                this.Invoke(_myInvoke, new object[] { strRecv });
            }
            else
            {
                this.textAsyncRecv.AppendText(strRecv + "\n");
            }
        }


        private void Send(Socket work, string Data)
        {
            if (Data.LastIndexOf("<END>") == -1)
                Data += "<END>";

            byte[] byteData = Encoding.UTF8.GetBytes(Data);

            work.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), work);
        }

        private void SendHeartBeat(Socket work, string Data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(Data);
            work.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), work);
         }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket work = (Socket)ar.AsyncState;
            int nByteSend = work.EndSend(ar);
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            int nPort = Convert.ToInt32(textPort.Text);

            if (!m_bStart)
            {   
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, nPort);
                m_listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_listen.Bind(endPoint);
                m_listen.Listen(10);

                m_bStart = true;                                
                Thread ServerListen = new Thread(new ParameterizedThreadStart(StartListening));
                ServerListen.Start(m_listen);

                btnListen.Text = "Stop";
            }
            else
            {
                m_bStart = false;
                m_AllDown.Set();
                m_listen.Close();
                btnListen.Text = "Listen";
            }
        }
    }

    public class StateObject
    {
        public Socket WorkSocket = null;
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder recvString = new StringBuilder();
    }
}