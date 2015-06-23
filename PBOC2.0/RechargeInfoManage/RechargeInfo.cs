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

namespace RechargeManage
{
    public partial class RechargeRecord : Form, IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        private int m_nCurPage = 0; //当前显示页
        private int m_nRowsPerPage = 50;  //每页显示记录数
        private int m_nTotalPage = 1;  //总页数


        public RechargeRecord()
        {
            InitializeComponent();
            //RechargeInfoPos();
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

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            //empty            
        }

        /*
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
        */

        private void RechargeRecord_Load(object sender, EventArgs e)
        {
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
                return;
            }

            RechargeView.Columns.Clear();
            RechargeView.Columns.Add("Index", "序号");
            RechargeView.Columns.Add("CardId", "充值卡号");
            RechargeView.Columns.Add("ForwardBalance", "充值前余额");
            RechargeView.Columns.Add("RechargeVal", "充值金额");
            RechargeView.Columns.Add("CurrentBalance", "充值后金额");
            RechargeView.Columns.Add("Time", "充值时间");
            RechargeView.Columns.Add("Operator", "操作员");
            int[] ColumnWidth = new int[]{50,150,100,100,100,150,100};
            for (int i = 0; i < RechargeView.Columns.Count; i++)
            {
                RechargeView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                RechargeView.Columns[i].Width = ColumnWidth[i];
            }

            GetRechargeDataTotalPage();
            //加载圈存记录
            ReadRechargeData();
        }

        private void GetRechargeDataTotalPage()
        {
            int nTotal = 0;
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select count(RunningNum) Total from Data_RechargeCardRecord", out dataReader);
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
                btnPrevPage.Visible = true;
                btnNextPage.Visible = true;
            }
            else
            {
                btnPrevPage.Visible = false;
                btnNextPage.Visible = false;
            }
        }

        private void RechargeRecord_FormClosed(object sender, FormClosedEventArgs e)
        {
            RechargeView.Dispose();
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        private void ReadRechargeData()
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            RechargeView.Rows.Clear();
            SqlDataReader dataReader = null;
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam("Start", SqlDbType.Int, 4, ParameterDirection.Input, m_nCurPage * m_nRowsPerPage);
            sqlparams[1] = ObjSql.MakeParam("End", SqlDbType.Int, 4, ParameterDirection.Input, (m_nCurPage + 1) * m_nRowsPerPage);
            ObjSql.ExecuteCommand("select * from Data_RechargeCardRecord where RunningNum > @Start and RunningNum <= @End", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    int nCount = 0;
                    string strValue = "";
                    while (dataReader.Read())
                    {
                        int index = RechargeView.Rows.Add();
                        RechargeView.Rows[index].Cells[0].Value = m_nCurPage * m_nRowsPerPage + nCount + 1;
                        strValue = (string)dataReader["CardNum"];
                        RechargeView.Rows[index].Cells[1].Value = strValue;
                        decimal ForwardBal = (decimal)dataReader["ForwardBalance"];
                        RechargeView.Rows[index].Cells[2].Value = ForwardBal.ToString();
                        decimal RechargeVal = (decimal)dataReader["RechargeValue"];
                        RechargeView.Rows[index].Cells[3].Value = RechargeVal.ToString();
                        decimal CurrentBal = (decimal)dataReader["CurrentBalance"];
                        RechargeView.Rows[index].Cells[4].Value = CurrentBal.ToString();
                        DateTime RechargeTime = (DateTime)dataReader["RechargeDateTime"];
                        RechargeView.Rows[index].Cells[5].Value = RechargeTime.ToString("yyyy-MM-dd HH:mm:ss");
                        int OperatorId = (int)dataReader["OperatorId"];
                        RechargeView.Rows[index].Cells[6].Value = GetOperatorName(OperatorId);
                        nCount++;
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (m_nTotalPage <= 1)
                return;
            if (m_nCurPage > 0)
            {
                m_nCurPage--;
                ReadRechargeData();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (m_nTotalPage <= 1)
                return;
            if (m_nCurPage < m_nTotalPage - 1)
            {
                m_nCurPage++;
                ReadRechargeData();
            }
        }

        private string GetOperatorName(int nOperatorID)
        {
            string strOperator = "";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = m_ObjSql.MakeParam("UserId", SqlDbType.Int, 4, ParameterDirection.Input, nOperatorID);
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select UserName from UserDb where UserId = @UserId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    strOperator = (string)dataReader["UserName"];
                }
                dataReader.Close();
            }
            return strOperator;
        }

    }
}