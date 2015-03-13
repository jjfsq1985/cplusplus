using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;

namespace CommunicationManage
{
    public partial class Communication : Form, IPlugin
    {
        private bool m_bLoad = false;
        private int m_nFormWidth = 0;
        private int m_nFormHeight = 0;

        public Communication()
        {
            InitializeComponent();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eCommunicationUDP;
        }

        public string PluginName()
        {
            return "Communication";
        }

        public Guid PluginGuid()
        {
            return new Guid("EAF11A51-B785-4d78-A1B6-73AA3581DD1E");
        }

        public string PluginMenu()
        {
            return "UDP通讯";
        }

        public void ShowPluginForm(Form parent)
        {
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.MdiParent = parent;
            this.Show();
        }

        private void Communication_Load(object sender, EventArgs e)
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

        private void Communication_Resize(object sender, EventArgs e)
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