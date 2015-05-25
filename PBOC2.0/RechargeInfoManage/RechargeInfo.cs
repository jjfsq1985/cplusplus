using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;

namespace RechargeManage
{
    public partial class RechargeRecord : Form, IPlugin
    {
        public RechargeRecord()
        {
            InitializeComponent();
            RechargeInfoPos();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eRechargeList;
        }

        public string PluginName()
        {
            return "RechargeRecord";
        }

        public Guid PluginGuid()
        {
            return new Guid("5315D784-78EC-4bf7-AE8B-E639BE54B784");
        }

        public string PluginMenu()
        {
            return "充值信息";
        }

        public void ShowPluginForm(Form parent)
        {
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.MdiParent = parent;
            this.Show();
        }

        private void RechargeInfoPos()
        {
            foreach (Control ctrl in Controls)
            {
                ControlPos pos = new ControlPos();
                pos.x = ctrl.Left;
                pos.dbRateH = (double)ctrl.Right / this.Width;
                pos.y = ctrl.Top;
                pos.dbRateV = (double)ctrl.Bottom / this.Height;
                ctrl.Tag = pos;
            }
        }

        private void RechargeInfo_Resize(object sender, EventArgs e)
        {
            foreach (Control ctrl in Controls)
            {
                ControlPos pos = (ControlPos)ctrl.Tag;
                pos.x = (int)((this.Width * pos.dbRateH) - ctrl.Width);
                pos.y = (int)((this.Height * pos.dbRateV) - ctrl.Height);
                ctrl.Left = pos.x;
                ctrl.Top = pos.y;
                ctrl.Tag = pos;
            }       
        }
    }
}