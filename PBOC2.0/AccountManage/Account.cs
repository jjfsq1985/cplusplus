using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;

namespace AccountManage
{
    public partial class Account : Form , IPlugin
    {
        private bool m_bLoad = false;
        private int m_nFormWidth = 0;
        private int m_nFormHeight = 0;


        public Account()
        {
            InitializeComponent();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eSystemAccount;
        }

        public string PluginName()
        {
            return "Account";
        }

        public Guid PluginGuid()
        {
            return new Guid("9A91172D-C36D-42f1-9320-78F3461FE0CD");
        }

        public string PluginMenu()
        {
            return "账户管理";
        }

        public void ShowPluginForm(Form parent)
        {
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.MdiParent = parent;
            this.Show();
        }

        private void AccountQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Account_Load(object sender, EventArgs e)
        {
            m_nFormWidth = this.Width;
            m_nFormHeight = this.Height;
            foreach (Control ctrl in Controls)
            {
                ControlPos pos = new ControlPos();
                pos.x = ctrl.Left;
                pos.y = ctrl.Top;
                ctrl.Tag = pos;
            }
            m_bLoad = true;
        }

        private void Account_Resize(object sender, EventArgs e)
        {
            if (!m_bLoad)
                return;
            float NowRateW = (float)this.Width / m_nFormWidth;
            float NowRateH = (float)this.Height / m_nFormHeight;

            foreach (Control ctrl in Controls)
            {
                ControlPos pos = (ControlPos)ctrl.Tag;
                ctrl.Top = (int)(pos.y * NowRateH);
                ctrl.Left = (int)(pos.x * NowRateW);
                pos.x = ctrl.Left;
                pos.y = ctrl.Top;
                ctrl.Tag = pos;
            }
            m_nFormWidth = this.Width;
            m_nFormHeight = this.Height;
        }

    }
}