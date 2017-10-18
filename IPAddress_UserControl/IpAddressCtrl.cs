using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPAddress
{
    public partial class IpAddressCtrl : UserControl
    {
        private string _IpValue = string.Empty;
        private bool _IsNetmask = false;

        public bool IsNetmask
        {
            get
            {
                return _IsNetmask;
            }

            set
            {
                _IsNetmask = value;
            }
        }

        public void IPAddressInputFunc(int nBoxIndex)
        {
            switch(nBoxIndex)
            {
                case 1:
                    textIP1.Focus();//向前一个
                    break;
                case 2:
                    textIP2.Focus();
                    break;
                case 3:
                    textIP3.Focus();
                    break;
                default:
                    break;
            }
        }


        public IpAddressCtrl()
        {
            InitializeComponent();
            textIP1.TextInputBackEvent += new IpEditBox.InputBackevent(IPAddressInputFunc);
            textIP2.TextInputBackEvent += new IpEditBox.InputBackevent(IPAddressInputFunc);
            textIP3.TextInputBackEvent += new IpEditBox.InputBackevent(IPAddressInputFunc);
            textIP4.TextInputBackEvent += new IpEditBox.InputBackevent(IPAddressInputFunc);
        }

        public void UpdateIpAddress()
        {
            try
            {
                string[] sArray = _IpValue.Split(new char[] { '.' });
                textIP1.Text = sArray[0];
                textIP2.Text = sArray[1];
                textIP3.Text = sArray[2];
                textIP4.Text = sArray[3];
            }
            catch
            {

            }
        }

        public string IPAddressString
        {
            set
            {
                _IpValue = value;
            }
            get
            {
                string str1 = textIP1.Text;
                string str2 = textIP2.Text;
                string str3 = textIP3.Text;
                string str4 = textIP4.Text;
                string strDot = ".";
                _IpValue = str1 + strDot + str2 + strDot + str3 + strDot + str4;
                return _IpValue;
            }
        }
    }
}
