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
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CodeTable
{
    public partial class CityCode : Form, IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();
        private List<CityCodeTable> m_lstCityCode = new List<CityCodeTable>();
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nAuthority = 0;


        public CityCode()
        {
            InitializeComponent();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eCityCode;
        }

        public string PluginName()
        {
            return "CityCode";
        }

        public Guid PluginGuid()
        {
            return new Guid("7094543C-D6FC-4453-84D7-0C9962FC7052");
        }

        public string PluginMenu()
        {
            return "地市代码";
        }

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
            if (m_nAuthority != GrobalVariable.CodeTable_Authority)
            {
                btnAdd.Enabled = false;
                btnDel.Enabled = false;
            }
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            m_nAuthority = nAuthority;
        }

        private void CityCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭窗口时判断是否需要保存
            bool bSave = false;            
            foreach (CityCodeTable value in m_lstCityCode)
            {
                if (value.eDbState != DbStateFlag.eDbOK)
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
                    SaveCityCodeDataToDb();
                }
            }
            CityView.Dispose();
            m_lstCityCode.Clear();
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        private void CityCode_Load(object sender, EventArgs e)
        {
            //显示所有地市代码
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
                return;
            }
            ReadCityCodeFromDb();
        }

        private void ReadCityCodeFromDb()
        {
            CityView.Rows.Clear();
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select * from Data_City", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        int index = CityView.Rows.Add();
                        CityCodeTable CityVal = new CityCodeTable();
                        CityVal.eDbState = DbStateFlag.eDbOK;
                        CityVal.nDataGridViewRowIndex = index;
                        CityVal.strCityName = (string)dataReader["CityName"];
                        byte[] codeBcd = PublicFunc.StringToBCD((string)dataReader["CityCode"]);
                        Trace.Assert(codeBcd != null && codeBcd.Length == 2);
                        CityVal.CityCode[0] = codeBcd[0];
                        CityVal.CityCode[1] = codeBcd[1];
                        CityView.Rows[index].Cells[0].Value = CityVal.strCityName;
                        CityView.Rows[index].Cells[1].Value = BitConverter.ToString(codeBcd).Replace("-", "");
                        m_lstCityCode.Add(CityVal);
                    }
                }
                dataReader.Close();
            }
        }

        private int GetIndexOfList(int nRowIndex)
        {
            int nListIndex = -1;
            int nCount = 0;
            foreach (CityCodeTable value in m_lstCityCode)
            {
                if (value.nDataGridViewRowIndex == nRowIndex)
                {
                    nListIndex = nCount;
                    break;
                }
                nCount++;
            }
            return nListIndex;
        }

        private void CityView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (CityView.CurrentCell == null)
                return;
            string strInput = (string)CityView.CurrentCell.FormattedValue;
            if (strInput == "")
                return;
            int nRowIndex = CityView.CurrentCell.RowIndex;
            int nItem = CityView.CurrentCell.ColumnIndex;
            if (nItem == 0)
            {
                Regex reg = new Regex(@"^[A-Za-z\u4e00-\u9fa5]+$");
                if (!reg.Match(strInput).Success)
                {
                    CityView.CurrentCell.Value = "";
                    MessageBox.Show("地市名称不能包含数字和特殊字符");
                    return;
                }
                int nListIndex = GetIndexOfList(nRowIndex);
                if (nListIndex == -1)
                {
                    //新增
                    bool bAdd = true;
                    foreach (CityCodeTable value in m_lstCityCode)
                    {
                        if (value.strCityName == strInput)
                        {
                            bAdd = false;
                            CityView.CurrentCell.Value = "";
                            MessageBox.Show("地市名称已存在");
                            break;
                        }
                    }
                    if (bAdd)
                    {
                        CityCodeTable newVal = new CityCodeTable();
                        newVal.eDbState = DbStateFlag.eDbAdd;
                        newVal.nDataGridViewRowIndex = nRowIndex;
                        newVal.strCityName = strInput;
                        newVal.CityCode[0] = 0;
                        newVal.CityCode[1] = 0;
                        m_lstCityCode.Add(newVal);
                    }
                }
                else if (strInput != m_lstCityCode[nListIndex].strCityName)
                {
                    m_lstCityCode[nListIndex].strCityName = strInput;
                    if (m_lstCityCode[nListIndex].eDbState == DbStateFlag.eDbOK)
                        m_lstCityCode[nListIndex].eDbState = DbStateFlag.eDbDirty;
                }
            }
            else if (nItem == 1)
            {
                Regex reg = new Regex(@"^[0-9]+$");
                if (!reg.Match(strInput).Success)
                {
                    CityView.CurrentCell.Value = "";
                    MessageBox.Show("地市代码只能是数字");
                    return;
                }
                byte[] codebyte = PublicFunc.StringToBCD(strInput);
                if (codebyte == null || codebyte.Length != 2)
                {
                    CityView.CurrentCell.Value = "";
                    MessageBox.Show("地市代码无效");
                    return;
                }
                int nListIndex = GetIndexOfList(nRowIndex);
                if (nListIndex == -1)
                {
                    //新增
                    bool bAdd = true;
                    foreach (CityCodeTable value in m_lstCityCode)
                    {
                        if (value.CityCode[0] == codebyte[0] && value.CityCode[1] == codebyte[1])
                        {
                            bAdd = false;
                            CityView.CurrentCell.Value = "";
                            MessageBox.Show("省代码已存在");
                            break;
                        }
                    }
                    if (bAdd)
                    {
                        CityCodeTable newVal = new CityCodeTable();
                        newVal.eDbState = DbStateFlag.eDbAdd;
                        newVal.nDataGridViewRowIndex = nRowIndex;
                        newVal.strCityName = "";
                        newVal.CityCode[0] = codebyte[0];
                        newVal.CityCode[1] = codebyte[1];
                        m_lstCityCode.Add(newVal);
                    }
                }
                else if( (codebyte[0] != m_lstCityCode[nListIndex].CityCode[0]) || (codebyte[1] != m_lstCityCode[nListIndex].CityCode[1]) )
                {
                    m_lstCityCode[nListIndex].CityCode[0] = codebyte[0];
                    m_lstCityCode[nListIndex].CityCode[1] = codebyte[1];
                    if (m_lstCityCode[nListIndex].eDbState == DbStateFlag.eDbOK)
                        m_lstCityCode[nListIndex].eDbState = DbStateFlag.eDbDirty;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CityView.Rows.Count > m_lstCityCode.Count || !CodeTable.IsCityListCompleted(m_lstCityCode))
            {
                MessageBox.Show("请先将空行填完整");
                return;
            }
            int nIndex = CityView.Rows.Add();
            CityView.Rows[nIndex].Cells[0].Selected = true;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (CityView.CurrentCell == null)
                return;
            int nRowIndex = CityView.CurrentCell.RowIndex;
            int nListIndex = GetIndexOfList(nRowIndex);
            if (nListIndex != -1)
            {
                //未保存过的直接删除
                if (m_lstCityCode[nListIndex].eDbState == DbStateFlag.eDbAdd)
                    m_lstCityCode.RemoveAt(nListIndex);
                else
                    m_lstCityCode[nListIndex].eDbState = DbStateFlag.eDbDelete;
            }
            CityView.Rows.RemoveAt(nRowIndex);
            if (nRowIndex > 0)
                CityView.Rows[nRowIndex - 1].Cells[0].Selected = true;
        }

        private void SaveCityCodeDataToDb()
        {
            SqlParameter[] sqlparams = new SqlParameter[2];
            List<CityCodeTable> deleteLst = new List<CityCodeTable>();
            int nCount = m_lstCityCode.Count;
            for (int i = 0; i < nCount; i++)
            {
                CityCodeTable value = m_lstCityCode[i];
                sqlparams[0] = m_ObjSql.MakeParam("Code", SqlDbType.VarChar, 4, ParameterDirection.Input, BitConverter.ToString(value.CityCode).Replace("-",""));
                sqlparams[1] = m_ObjSql.MakeParam("Name", SqlDbType.NVarChar, 50, ParameterDirection.Input, value.strCityName);
                if (value.eDbState == DbStateFlag.eDbAdd)
                {
                    m_ObjSql.ExecuteCommand("insert into Data_City values(@Code,@Name)", sqlparams);
                    value.eDbState = DbStateFlag.eDbOK;
                }
                else if (value.eDbState == DbStateFlag.eDbDelete)
                {
                    m_ObjSql.ExecuteCommand("delete from Data_City where CityCode = @Code and CityName = @Name", sqlparams);
                    deleteLst.Add(value);
                }
                else if (value.eDbState == DbStateFlag.eDbDirty)
                {
                    m_ObjSql.ExecuteCommand("update Data_City set CityCode = @Code, CityName = @Name", sqlparams);
                    value.eDbState = DbStateFlag.eDbOK;
                }
                m_lstCityCode[i] = value;
            }

            foreach (CityCodeTable temp in deleteLst)
            {
                m_lstCityCode.Remove(temp);
            }
            deleteLst.Clear();
        }

        private void CityView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (m_nAuthority != GrobalVariable.CodeTable_Authority)
                e.Cancel = true;
        }

    }
}
