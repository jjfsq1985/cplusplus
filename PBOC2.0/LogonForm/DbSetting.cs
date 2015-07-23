using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SqlServerHelper;
using IFuncPlugin;
using System.Diagnostics;

namespace FNTMain
{
    public partial class DbSetting : Form
    {
        private SqlConnectInfo m_DbInfo = new SqlConnectInfo();
        private int SecurityType = 0;

        public DbSetting()
        {
            InitializeComponent();
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
            this.CmbSecurity.SelectedIndex = 0;
        }

        public SqlConnectInfo GetDbInfo()
        {
            return m_DbInfo;
        }

        public void SetDbInfo(SqlConnectInfo DbInfo)
        {
            m_DbInfo = DbInfo;
            textDbServer.Text = m_DbInfo.strServerName;
            textDbName.Text = m_DbInfo.strDbName;
            if (m_DbInfo.strUser == "" && m_DbInfo.strUserPwd == "")
            {
                SecurityType = 1;
                textDbUser.Text = "";
                textDbPwd.Text = "";
            }
            else
            {
                SecurityType = 0;
                textDbUser.Text = m_DbInfo.strUser;
                textDbPwd.Text = m_DbInfo.strUserPwd;
            }
            this.CmbSecurity.SelectedIndex = SecurityType;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SqlHelper ObjSql = new SqlHelper();
            try
            {
                bool bConnect = false;
                if (SecurityType == 0)
                    bConnect = ObjSql.OpenSqlServerConnection(textDbServer.Text, textDbName.Text, textDbUser.Text, textDbPwd.Text);
                else
                    bConnect = ObjSql.OpenSqlServerConnection(textDbServer.Text, textDbName.Text, "", "");
                if (!bConnect)
                {
                    ObjSql = null;
                    MessageBox.Show("数据库连接错误，请检查!");
                    return;
                }
                ObjSql.CloseConnection();
                ObjSql = null;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);	
                MessageBox.Show("数据库连接错误，请检查!");
                ObjSql = null;
                return;
            }
            m_DbInfo.strServerName = textDbServer.Text;
            m_DbInfo.strDbName = textDbName.Text;
            if (SecurityType == 0)
            {
                m_DbInfo.strUser = textDbUser.Text;
                m_DbInfo.strUserPwd = textDbPwd.Text;
            }
            else
            {
                m_DbInfo.strUser = "";
                m_DbInfo.strUserPwd = "";
            }
            m_DbInfo.m_bConfig = true;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void CmbSecurity_SelectedIndexChanged(object sender, EventArgs e)
        {
            SecurityType = CmbSecurity.SelectedIndex;
            if (SecurityType == 0)
            {
                textDbUser.Enabled = true;
                textDbPwd.Enabled = true;
            }
            else
            {
                textDbUser.Enabled = false;
                textDbPwd.Enabled = false;
            }
        }
    }
}
