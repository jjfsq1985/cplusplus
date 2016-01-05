using System;
using System.Collections.Generic;
using System.Text;

namespace ApduParam
{
    public enum ICC_Status
    {
        ICC_PowerOff = 0, //未上电
        ICC_PowerOn     //已上电
    }

    public class MsgOutEvent : EventArgs
    {
        private int m_nErrorColor = 0;
        public int ErrColor
        {
            get { return m_nErrorColor; }            
        }

        private string m_strMessage = "";
        public string Message
        {
            get { return m_strMessage; }            
        }

        public MsgOutEvent(int nErrClr, string strMsg)
        {
            m_nErrorColor = nErrClr;
            m_strMessage = strMsg;
        }
    }
    public delegate void MessageOutput(MsgOutEvent args);
}
