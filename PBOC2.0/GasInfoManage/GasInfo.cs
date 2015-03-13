using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;

namespace GasInfoManage
{
    public partial class GasInfo : Form, IPlugin
    {
        private bool m_bLoad = false;
        private int m_nFormWidth = 0;
        private int m_nFormHeight = 0;

        public GasInfo()
        {
            InitializeComponent();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eGasDataList;
        }

        public string PluginName()
        {
            return "GasInfo";
        }

        public Guid PluginGuid()
        {
            return new Guid("5315D784-78EC-4bf7-AE8B-E639BE54B784");
        }

        public string PluginMenu()
        {
            return "加气信息管理";
        }

        public void ShowPluginForm(Form parent)
        {
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.MdiParent = parent;
            this.Show();
        }

        private void GasInfo_Load(object sender, EventArgs e)
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

        private void GasInfo_Resize(object sender, EventArgs e)
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