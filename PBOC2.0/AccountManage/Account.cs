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

namespace AccountManage
{
    public partial class Account : Form , IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();
        private int m_nCurPage = 0; //当前显示页
        private int m_nRowsPerPage = 50;  //每页显示记录数
        private int m_nTotalPage = 1;  //总页数
        private List<AccountInfo> m_lstUser = new List<AccountInfo>();
        private bool m_bEditData = false;

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nAccountAuthority = 0;
        private int m_nLoginUserId;

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

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
            if ((m_nAccountAuthority & GrobalVariable.Account_Authority) != GrobalVariable.Account_Authority)
            {
                btnAdd.Enabled = false;
                btnDel.Enabled = false;
                btnEdit.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            m_nLoginUserId = nLoginUserId;
            m_nAccountAuthority = nAuthority;
        }


        private void AccountQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Account_Load(object sender, EventArgs e)
        {
            //加载用户名列表
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
                return;
            }
            UserGridView.Columns.Clear();
            UserGridView.Columns.Add("Index", "序号");
            UserGridView.Columns.Add("UserName", "用户名");
            UserGridView.Columns.Add("UserAuthority", "用户权限");
            UserGridView.Columns.Add("UserState", "状态");
            for (int i = 0; i < UserGridView.Columns.Count; i++)
                UserGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            UserGridView.Columns[2].MinimumWidth = 200;

            
            //从数据库获取
            GetAccountTotalPage();
            FillAccountGridView();

            btnEdit.Enabled = m_bEditData ? false : true;
            btnSave.Enabled = m_bEditData ? true : false;

        }

        private void GetAccountTotalPage()
        {
            int nTotal = 0;
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select count(UserId) Total from UserDb", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    nTotal = (int)dataReader["Total"];
                }
                dataReader.Close();
             }

            if (nTotal == 0 || nTotal % m_nRowsPerPage != 0)
                m_nTotalPage = nTotal / m_nRowsPerPage + 1;
            else
                m_nTotalPage = nTotal / m_nRowsPerPage;

            if (m_nTotalPage > 1)
            {
                PrevPage.Visible = true;
                NextPage.Visible = true;
            }
            else
            {
                PrevPage.Visible = false;
                NextPage.Visible = false;
            }
        }

        private void FillAccountGridView()
        {
            UserGridView.Rows.Clear();
            SqlDataReader dataReader = null;
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = m_ObjSql.MakeParam("IdStart", SqlDbType.Int, 4, ParameterDirection.Input, m_nCurPage * m_nRowsPerPage);
            sqlparams[1] = m_ObjSql.MakeParam("IdEnd", SqlDbType.Int, 4, ParameterDirection.Input, (m_nCurPage + 1) * m_nRowsPerPage);
            m_ObjSql.ExecuteCommand("select * from UserDb where UserId > @IdStart and UserId <= @IdEnd", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    int nCount = 0;
                    while (dataReader.Read())
                    {
                        int index = UserGridView.Rows.Add();
                        int nId = (int)dataReader["UserId"];
                        AccountInfo AccountVal = new AccountInfo();
                        AccountVal.eDbFlag = DbStateFlag.eDbOK;
                        AccountVal.nUserId = nId;
                        AccountVal.strUserName = (string)dataReader["UserName"];
                        AccountVal.strPassword = (string)dataReader["Password"];
                        AccountVal.UserAuthority = (int)dataReader["Authority"];
                        AccountVal.UserStatus = (int)dataReader["Status"];
                        UserGridView.Rows[index].Cells[0].Value = m_nCurPage * m_nRowsPerPage + nCount + 1;
                        UserGridView.Rows[index].Cells[1].Value = AccountVal.strUserName;
                        UserGridView.Rows[index].Cells[2].Value = GetAuthorityString(AccountVal.UserAuthority);
                        UserGridView.Rows[index].Cells[3].Value = GetStatusString(AccountVal.UserStatus);
                        m_lstUser.Add(AccountVal);
                        nCount++;
                    }
                }
                dataReader.Close();
            }
        }

        //按权限值返回字符串
        private string GetAuthorityString(int UserAuthority)
        {
            string strRet = "查看";
            int nCount = 0;
            for (int i = 0; i < GrobalVariable.Authority_Config_Count; i++)
            {
                if( (UserAuthority&(1<<i)) == (1<<i) )
                {
                    strRet += "," + GrobalVariable.strAuthority[i];
                    nCount++;
                }
            }
            if (nCount == GrobalVariable.Authority_Config_Count)
                return "完整权限";
            else
                return strRet;
        }

        private void UserGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!m_bEditData)
                return;
            //修改权限和密码 
            AccountEdit modifyForm = new AccountEdit();
            int nIndex = UserGridView.CurrentCell.RowIndex;
            modifyForm.SetAccountInfo(m_lstUser[nIndex],m_nAccountAuthority);
            if (modifyForm.ShowDialog(this) != DialogResult.OK)
                return;
            AccountInfo AccountInfoModify = modifyForm.GetAccountInfo();
            AccountInfoModify.eDbFlag = DbStateFlag.eDbDirty;
            m_lstUser[nIndex] = AccountInfoModify;
            //存数据库
            SaveLstDataToDb(); 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddAccount AddForm = new AddAccount();
            AddForm.SetInfo(m_DBInfo,m_nAccountAuthority);
            if (AddForm.ShowDialog(this) != DialogResult.OK)
                return;
            AccountInfo newAccount = AddForm.GetAccountInfo();
            newAccount.eDbFlag = DbStateFlag.eDbAdd;
            m_lstUser.Add(newAccount);
            int index = UserGridView.Rows.Add();
            UserGridView.Rows[index].Cells[0].Value = m_nCurPage * m_nRowsPerPage + m_lstUser.Count; //m_lstUser记录已增加
            UserGridView.Rows[index].Cells[1].Value = newAccount.strUserName;
            UserGridView.Rows[index].Cells[2].Value = GetAuthorityString(newAccount.UserAuthority);
            UserGridView.Rows[index].Cells[3].Value = GetStatusString(newAccount.UserStatus);
            SaveLstDataToDb();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {            
            int nIndex = UserGridView.CurrentCell.RowIndex;
            AccountInfo value = m_lstUser[nIndex];
            if (value.nUserId == m_nLoginUserId || value.UserStatus == 1)
            {
                MessageBox.Show("账户已登录，不能删除。");
                return;
            }
            UserGridView.Rows.RemoveAt(nIndex);
            value.eDbFlag = DbStateFlag.eDbDelete;
            m_lstUser[nIndex] = value;
            SaveLstDataToDb();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            m_bEditData = true;
            btnEdit.Enabled = m_bEditData ? false : true;
            btnSave.Enabled = m_bEditData ? true : false;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            m_bEditData = false;
            btnEdit.Enabled = m_bEditData ? false : true;
            btnSave.Enabled = m_bEditData ? true : false;
            //存数据库
            SaveLstDataToDb();
        }

        private void Account_FormClosed(object sender, FormClosedEventArgs e)
        {
            bool bSave = false;
            foreach (AccountInfo value in m_lstUser)
            {
                if (value.eDbFlag != DbStateFlag.eDbOK)
                {
                    bSave = true;
                    break;
                }
            }
            if (bSave)
            {
                DialogResult result = MessageBox.Show("是否保存更改的数据？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SaveLstDataToDb();
                }
            }

            UserGridView.Dispose();
            m_lstUser.Clear();
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        private void SaveLstDataToDb()
        {
            int nCount = m_lstUser.Count;
            List<AccountInfo> deleteLst = new List<AccountInfo>();
            for (int i = 0; i < nCount; i++)
            {
                AccountInfo value = m_lstUser[i];
                if (value.eDbFlag != DbStateFlag.eDbOK)
                {
                    SqlParameter[] sqlparams = new SqlParameter[7];
                    sqlparams[0] = m_ObjSql.MakeParam("UserId", SqlDbType.Int, 4, ParameterDirection.Input, value.nUserId);
                    sqlparams[1] = m_ObjSql.MakeParam("UserName", SqlDbType.VarChar, 32, ParameterDirection.Input, value.strUserName);
                    sqlparams[2] = m_ObjSql.MakeParam("Password", SqlDbType.VarChar, 64, ParameterDirection.Input, value.strPassword);
                    sqlparams[3] = m_ObjSql.MakeParam("Authority", SqlDbType.Int, 4, ParameterDirection.Input, value.UserAuthority);
                    sqlparams[4] = m_ObjSql.MakeParam("Status", SqlDbType.Int, 4, ParameterDirection.Input, value.UserStatus);
                    sqlparams[5] = m_ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, value.eDbFlag);
                    sqlparams[6] = m_ObjSql.MakeParam("AddUserId", SqlDbType.Int, 4, ParameterDirection.Output, null);
                    if (m_ObjSql.ExecuteProc("PROC_UpdateAccount", sqlparams) == 0)
                    {
                        if (value.eDbFlag == DbStateFlag.eDbDelete)
                        {
                            deleteLst.Add(value);
                        }
                        else
                        {
                            if (value.eDbFlag == DbStateFlag.eDbAdd)
                            {
                                value.nUserId = (int)sqlparams[6].Value;
                            }
                            value.eDbFlag = DbStateFlag.eDbOK;
                            m_lstUser[i] = value;
                        }
                    }
                }
            }//end for
            foreach (AccountInfo temp in deleteLst)
            {
                m_lstUser.Remove(temp);
            }
            deleteLst.Clear();
        }

        private string GetStatusString(int nStatus)
        {
            string strStatus = "";
            switch (nStatus)
            {
                case 0:
                    strStatus = "未登录";
                    break;
                case 1:
                    strStatus = "已登录";
                    break;
                case 2:
                    strStatus = "停用";
                    break;
            }
            return strStatus;
        }

        private void PrevPage_Click(object sender, EventArgs e)
        {
            if (m_nTotalPage <= 1)
                return;
            if (m_bEditData)
            {
                m_bEditData = false;
                btnEdit.Enabled = m_bEditData ? false : true;
                btnSave.Enabled = m_bEditData ? true : false;
                DialogResult result = MessageBox.Show("是否保存更改的数据？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //存数据库
                    SaveLstDataToDb();
                }
            }
            if (m_nCurPage > 0)
            {
                m_nCurPage--;
                FillAccountGridView();
            }
        }

        private void NextPage_Click(object sender, EventArgs e)
        {
            if (m_nTotalPage <= 1)
                return;
            if (m_bEditData)
            {
                m_bEditData = false;
                btnEdit.Enabled = m_bEditData ? false : true;
                btnSave.Enabled = m_bEditData ? true : false;
                DialogResult result = MessageBox.Show("是否保存更改的数据？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //存数据库
                    SaveLstDataToDb();
                }
            }
            if (m_nCurPage < m_nTotalPage - 1)
            {
                m_nCurPage++;
                FillAccountGridView();
            }
        }

    }
}