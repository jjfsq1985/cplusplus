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

        public DbSetting()
        {
            InitializeComponent();
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
            textDbUser.Text = m_DbInfo.strUser;
            textDbPwd.Text = m_DbInfo.strUserPwd;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SqlHelper ObjSql = new SqlHelper();
            try
            {
                bool bConnect = ObjSql.OpenSqlServerConnection(textDbServer.Text, textDbName.Text, textDbUser.Text, textDbPwd.Text);
                if (!bConnect)
                {
                    ObjSql = null;
                    MessageBox.Show("数据库连接错误，请检查!");
                    return;
                }
                ObjSql.CloseConnection();
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);	
                MessageBox.Show("数据库连接错误，请检查!");
                ObjSql = null;
                return;
            }
            m_DbInfo.strServerName = textDbServer.Text;
            m_DbInfo.strDbName = textDbName.Text;
            m_DbInfo.strUser = textDbUser.Text;
            m_DbInfo.strUserPwd = textDbPwd.Text;
            m_DbInfo.m_bConfig = true;
            this.DialogResult = DialogResult.OK;
        }
    }
}
