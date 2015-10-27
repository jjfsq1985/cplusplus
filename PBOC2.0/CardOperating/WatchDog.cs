using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ApduCtrl;
using ApduInterface;
using ApduParam;
using System.Windows.Forms;

namespace CardOperating
{
    //检查超级管理卡的看门狗
    public class WatchDog
    {
        private class ThreadProc
        {
            public bool m_bThreadRun = false;
            public bool m_bCalc = false;
            public ApduController m_DevController = null;

            public byte[] m_CardId = new byte[8];
            public byte m_Type = 0;
            public int m_nMoney = 0;
            public byte[] m_DateTime = new byte[7];
            public bool m_bSamSlot = false;  //使用读卡器内部SAM卡槽

            private ISamCardControl m_SamCardCtrl = null;
            public event MessageOutput TextOutput = null;
         
            public void WatchDogProc()
            {                
                while (m_bThreadRun)
                {
                    if (m_bCalc)
                    {                        
                        CalcData();
                        m_bCalc = false;
                    }
                    Thread.Sleep(1000);
                }
            }

            public bool OpenAdminCard(bool bSamSlot)
            {
                if (m_DevController == null || !m_DevController.IsDeviceOpen())
                    return false;
                m_SamCardCtrl = m_DevController.SamCardConstructor(null);
                string strCardInfo = "";
                return m_DevController.SAMPowerOn(bSamSlot, ref strCardInfo);
            }

            public bool CloseAdminCard(bool bSamSlot)
            {
                if (m_DevController == null || !m_DevController.IsDeviceOpen())
                    return false;
                m_DevController.SAMPowerOff(bSamSlot);
                m_SamCardCtrl = null;
                return true;
            }

            public void CalcData()
            {

            }
        }

        private ThreadProc m_CardSafeProc = new ThreadProc();
        private static readonly object m_lock = new object();
        private Thread m_thread = null;
        private bool m_bMessage = false;


        public void SetCardId(byte[] UserCardId,byte Type, int nMoney, byte[] bcdNow)
        {
            if (UserCardId.Length != 8 || Type <= 0 || nMoney <= 0 || bcdNow.Length != 7)
                return;
            lock (m_lock)
            {
                Buffer.BlockCopy(UserCardId, 0, m_CardSafeProc.m_CardId, 0, 8);
                m_CardSafeProc.m_Type = Type;
                m_CardSafeProc.m_nMoney = nMoney;
                Buffer.BlockCopy(bcdNow, 0, m_CardSafeProc.m_DateTime, 0, 7);
            }
        }
        

        public bool startWatchDog(ApduController DevCtrl,bool bSamSlot)
        {            
            lock(m_lock)
            {
                if(m_CardSafeProc == null)
                    return false;
                else if(m_CardSafeProc.m_bThreadRun)
                    return true;
                m_CardSafeProc.m_DevController = DevCtrl;
                m_CardSafeProc.m_bSamSlot = bSamSlot;
                m_CardSafeProc.m_bThreadRun = true;
                m_CardSafeProc.TextOutput += new MessageOutput(OnMessageOutput);
            }            
            m_thread = new Thread(new ThreadStart(m_CardSafeProc.WatchDogProc));
            m_thread.Start();
            return true;
        }

        public bool stopWatchDog()
        {
            lock (m_lock)
            {
                if (m_CardSafeProc == null)
                    return false;
            }                        
            m_CardSafeProc.m_bThreadRun = false;
            m_thread.Join();
            m_CardSafeProc.m_DevController = null;
            return true;
        }

        private void OnMessageOutput(MsgOutEvent args)
        {
            if (!m_bMessage)
            {
                m_bMessage = true;
                MessageBox.Show(args.Message);
                m_bMessage = false;
            }
        }
    }
}
