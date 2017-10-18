using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPAddress
{
    class IpEditBox : TextBox
    {
        public delegate void InputBackevent(int nBoxIndex);
        public event InputBackevent TextInputBackEvent;

        private bool _IsNetMask = false;
        private bool _IsSendKey = false;
        private int _IpIndex = 0;

        public int IpIndex
        {
            get
            {
                return _IpIndex;
            }

            set
            {
                _IpIndex = value;
            }
        }

        public bool IsMask
        {
            get
            {
                return _IsNetMask;
            }

            set
            {
                _IsNetMask = value;
            }
        }

        public bool IsSendKey
        {
            get
            {
                return _IsSendKey;
            }

            set
            {
                _IsSendKey = value;
            }
        }

        public IpEditBox()
        {

        }

        public IpEditBox(int nIndex)
        {
            _IpIndex = nIndex;
            this.TextAlign = HorizontalAlignment.Center;
            this.MaxLength = 3;
        }

        public IpEditBox(int nIndex,string strValue)
        {
            _IpIndex = nIndex;
            this.Text = strValue;
            this.TextAlign = HorizontalAlignment.Center;
            this.MaxLength = 3;
        }

        private void InputBackeventFunc(int nEditBoxIndex)
        {
            if(TextInputBackEvent != null)
            {
                TextInputBackEvent(nEditBoxIndex);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if(this.Text == "")
            {
                if(e.KeyCode.ToString() == "Back")
                {
                    this.InputBackeventFunc(this.IpIndex);
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            e.Handled = true;
            if((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8) )
            {
                if(e.KeyChar == (char)8)
                {
                    e.Handled = false;
                    return;
                }
                else
                {
                    int nLen = this.Text.Length;
                    if(nLen < 4)
                    {
                        if (nLen == 0 && e.KeyChar == '0' && this.IpIndex == 0 && this.IsMask == false)
                           return;
                        e.Handled = false;
                    }
                }
            }
            else if(e.KeyChar == '.')
            {
                if(this.Text.Length != 0)
                {
                    SendKeys.SendWait("{TAB}");
                }
            }
            else if(this.IsSendKey && this.Text.Length == 3)
            {
                this.SelectAll();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            try
            {
                IsSendKey = false;
                int nNumber = Convert.ToInt32(this.Text);
                this.SelectionStart = this.Text.Length;
                string strInput = this.Text;
                if (!IsMask)//IP
                {
                    if (IpIndex == 0 && (nNumber == 0 || nNumber > 223))
                    {
                        MessageBox.Show(strInput + "不是有效项。请指定介于1和223之间的值。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (this.IpIndex == 0 && nNumber == 0)
                            this.Text = "1";
                        else
                            this.Text = "223";
                        IsSendKey = true;
                        this.Focus();
                        this.SelectAll();
                    }
                    else if (nNumber > 255)
                    {
                        MessageBox.Show(strInput + "不是有效项。请指定介于0和255间的值。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Text = "255";
                        this.Focus();
                        this.SelectAll();
                    }
                    else
                    {
                        if (strInput.Length == 3 && IsSendKey == false)
                        {
                            //当输入字符个数==3，跳入另一输入框
                            if (nNumber == 0)
                            {
                                this.Text = "0";
                            }
                            if (IpIndex != 3)
                            {
                                IsSendKey = true;
                                SendKeys.SendWait("{TAB}");
                            }
                        }
                    }
                }
                else
                {
                    if (nNumber > 255)
                    {
                        MessageBox.Show(strInput + "不是有效项。请指定介于0和255间的值。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Text = "255";
                        this.Focus();
                        this.SelectAll();
                    }
                    else
                    {
                        if (strInput.Length == 3 && IsSendKey == false)
                        {
                            //当输入字符个数==3，跳入另一输入框
                            if (nNumber == 0)
                            {
                                this.Text = "0";
                            }
                            if (IpIndex != 3)
                            {
                                IsSendKey = true;
                                SendKeys.SendWait("{TAB}");
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
