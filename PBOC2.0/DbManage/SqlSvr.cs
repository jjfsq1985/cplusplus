using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;
using SqlServerHelper;
using System.Data.SqlClient;

namespace DbManage
{
    public partial class SqlSvr : Form, IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();        
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nDbAuthority = 0;

        public SqlSvr()
        {
            InitializeComponent();
        }
        public MenuType GetMenuType()
        {
            return MenuType.eDbManage;
        }

        public string PluginName()
        {
            return "DbManage";
        }

        public Guid PluginGuid()
        {
            return new Guid("6A1B65FB-DA7D-40c4-AD11-B8B5ECB7411A");
        }

        public string PluginMenu()
        {
            return "数据库备份与还原";
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            m_nDbAuthority = nAuthority;
        }

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
            if (m_nDbAuthority  != GrobalVariable.DbManage_Authority)
            {
                btnBackup.Enabled = false;
                btnBackupPath.Enabled = false;
                btnRestore.Enabled = false;
                btnRestorePath.Enabled = false;                
            }
        }

        private void btnBackupPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog backup = new SaveFileDialog();
            backup.Filter = "数据库备份文件（*.bak）|*.bak|所有文件（*.*）|*.*";
            backup.FilterIndex = 1;
            backup.RestoreDirectory = true;
            if(backup.ShowDialog() == DialogResult.OK)
            {
                textBackup.Text = backup.FileName;
            }
        }

        private void btnRestorePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog restore = new OpenFileDialog();
            restore.InitialDirectory = "C:\\";
            restore.Filter = "数据库备份文件（*.bak）|*.bak|所有文件（*.*）|*.*";
            restore.FilterIndex = 1;
            restore.RestoreDirectory = true;
            if (restore.ShowDialog() == DialogResult.OK)
            {
                textRestore.Text = restore.FileName;
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                SqlHelper.ClearConnectionPool();
                m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, "master", m_DBInfo.strUser, m_DBInfo.strUserPwd);
                string strSavePath = textBackup.Text;
                string strName = "Funnett_" + DateTime.Now.ToString("yyyy-MM-dd");
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = m_ObjSql.MakeParam("Path", SqlDbType.NVarChar, 256, ParameterDirection.Input, strSavePath);
                sqlparams[1] = m_ObjSql.MakeParam("Name", SqlDbType.NVarChar, 256, ParameterDirection.Input, strName);
                m_ObjSql.ExecuteCommand("BACKUP DATABASE FunnettStation TO DISK = @Path WITH INIT,FORMAT, NAME = @Name;", sqlparams);
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
                MessageBox.Show("数据库备份成功");
            }
            catch (SystemException ex)
            {                
                MessageBox.Show(ex.Message + "\n数据库备份失败");
            }

        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                SqlHelper.ClearConnectionPool();
                m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, "master", m_DBInfo.strUser, m_DBInfo.strUserPwd);
                string strReadPath = textRestore.Text;
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = m_ObjSql.MakeParam("Path", SqlDbType.NVarChar, 256, ParameterDirection.Input, strReadPath);
                m_ObjSql.ExecuteCommand("RESTORE DATABASE FunnettStation FROM DISK = @Path WITH NOUNLOAD,REPLACE;", sqlparams);
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
                MessageBox.Show("数据库恢复成功,请重新登录本软件。");
                Application.Exit();
            }
            catch (SystemException ex)
            {                
                MessageBox.Show(ex.Message + "\n数据库恢复失败");
            }
        }

    }
}
