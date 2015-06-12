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
using System.Text.RegularExpressions;

namespace CodeTable
{
    public partial class ProvinceCode : Form , IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();
        private List<ProvinceCodeTable> m_lstProvCode = new List<ProvinceCodeTable>();
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nAuthority = 0;


        public ProvinceCode()
        {
            InitializeComponent();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eProvinceCode;
        }

        public string PluginName()
        {
            return "ProvinceCode";
        }

        public Guid PluginGuid()
        {
            return new Guid("2F016FD9-8E92-4f30-989D-8687E22D76EB");
        }

        public string PluginMenu()
        {
            return "省代码";
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

        private void ProvinceCode_Load(object sender, EventArgs e)
        {
            //显示所有省代码
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
                return;
            }
            ReadProvCodeFromDb();
        }

        private void ReadProvCodeFromDb()
        {
            ProvinceView.Rows.Clear();
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select * from Data_Province", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {                    
                    while (dataReader.Read())
                    {
                        int index = ProvinceView.Rows.Add();
                        ProvinceCodeTable ProvVal = new ProvinceCodeTable();
                        ProvVal.eDbState = DbStateFlag.eDbOK;
                        ProvVal.nDataGridViewRowIndex = index;
                        ProvVal.strProvinceName = (string)dataReader["ProvinceName"];
                        ProvVal.ProvinceCode = Convert.ToByte((string)dataReader["ProvinceCode"],16);
                        ProvinceView.Rows[index].Cells[0].Value = ProvVal.strProvinceName;
                        ProvinceView.Rows[index].Cells[1].Value = ProvVal.ProvinceCode.ToString("X2");
                        m_lstProvCode.Add(ProvVal);                        
                    }
                }
                dataReader.Close();
            }
        }

        private void ProvinceCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭窗口时判断是否需要保存
            bool bSave = false;
            foreach (ProvinceCodeTable value in m_lstProvCode)
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
                    SaveProvCodeDataToDb();
                }
            }
            ProvinceView.Dispose();
            m_lstProvCode.Clear();
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        private int GetIndexOfList(int nRowIndex)
        {
            int nListIndex = -1;
            int nCount = 0;
            foreach (ProvinceCodeTable value in m_lstProvCode)
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

        private void ProvinceView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            string strInput = (string)ProvinceView.CurrentCell.FormattedValue;
            if (strInput == "")
                return;
            int nRowIndex = ProvinceView.CurrentCell.RowIndex;
            int nItem = ProvinceView.CurrentCell.ColumnIndex;
            if (nItem == 0)
            {
                Regex reg = new Regex(@"^[A-Za-z\u4e00-\u9fa5]+$");
                if (!reg.Match(strInput).Success)
                {
                    ProvinceView.CurrentCell.Value = "";
                    MessageBox.Show("省名称不能包含数字和特殊字符");
                    return;
                }
                int nListIndex = GetIndexOfList(nRowIndex);
                if (nListIndex == -1)
                {
                    //新增
                    bool bAdd = true;
                    foreach (ProvinceCodeTable value in m_lstProvCode)
                    {
                        if(value.strProvinceName == strInput)
                        {
                            bAdd = false;
                            ProvinceView.CurrentCell.Value = "";
                            MessageBox.Show("省名称已存在");
                            break;
                        }
                    }
                    if (bAdd)
                    {
                        ProvinceCodeTable newVal = new ProvinceCodeTable();
                        newVal.eDbState = DbStateFlag.eDbAdd;
                        newVal.nDataGridViewRowIndex = nRowIndex;
                        newVal.strProvinceName = strInput;
                        newVal.ProvinceCode = 0;
                        m_lstProvCode.Add(newVal);
                    }
                }
                else if (strInput != m_lstProvCode[nListIndex].strProvinceName)
                {
                    m_lstProvCode[nListIndex].strProvinceName = strInput;
                    if (m_lstProvCode[nListIndex].eDbState == DbStateFlag.eDbOK)
                        m_lstProvCode[nListIndex].eDbState = DbStateFlag.eDbDirty;
                }
            }
            else if (nItem == 1)
            {
                Regex reg = new Regex(@"^[0-9]+$");
                if (!reg.Match(strInput).Success)
                {
                    ProvinceView.CurrentCell.Value = "";
                    MessageBox.Show("省代码只能是数字");
                    return;
                }

                byte codebyte = 0;
                byte.TryParse(strInput,System.Globalization.NumberStyles.HexNumber,null, out codebyte);
                if (codebyte <= 0)
                {
                    ProvinceView.CurrentCell.Value = "";
                    MessageBox.Show("省代码无效");
                    return;
                }
                int nListIndex = GetIndexOfList(nRowIndex);
                if (nListIndex == -1)
                {
                    //新增
                    bool bAdd = true;
                    foreach (ProvinceCodeTable value in m_lstProvCode)                    
                    {
                        if (value.ProvinceCode == codebyte)
                        {
                            bAdd = false;
                            ProvinceView.CurrentCell.Value = "";
                            MessageBox.Show("省代码已存在");
                            break;
                        }
                    }
                    if (bAdd)
                    {
                        ProvinceCodeTable newVal = new ProvinceCodeTable();
                        newVal.eDbState = DbStateFlag.eDbAdd;
                        newVal.nDataGridViewRowIndex = nRowIndex;
                        newVal.strProvinceName = "";
                        newVal.ProvinceCode = codebyte;
                        m_lstProvCode.Add(newVal);
                    }
                }
                else if (codebyte != m_lstProvCode[nListIndex].ProvinceCode)
                {
                    m_lstProvCode[nListIndex].ProvinceCode = codebyte;
                    if (m_lstProvCode[nListIndex].eDbState == DbStateFlag.eDbOK)
                        m_lstProvCode[nListIndex].eDbState = DbStateFlag.eDbDirty;
                }
            }                
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ProvinceView.Rows.Count > m_lstProvCode.Count || !CodeTable.IsProvListCompleted(m_lstProvCode))
            {
                MessageBox.Show("请先将空行填完整");
                return;
            }
            int nIndex = ProvinceView.Rows.Add();
            ProvinceView.Rows[nIndex].Cells[0].Selected = true;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int nRowIndex = ProvinceView.CurrentCell.RowIndex;
            int nListIndex = GetIndexOfList(nRowIndex);
            if(nListIndex != -1)
            {
                //未保存过的直接删除
                if (m_lstProvCode[nListIndex].eDbState == DbStateFlag.eDbAdd)
                    m_lstProvCode.RemoveAt(nListIndex);
                else
                    m_lstProvCode[nListIndex].eDbState = DbStateFlag.eDbDelete;
            }
            ProvinceView.Rows.RemoveAt(nRowIndex);
            if (nRowIndex > 0)
                ProvinceView.Rows[nRowIndex - 1].Cells[0].Selected = true;
        }

        private void SaveProvCodeDataToDb()
        {
            SqlParameter[] sqlparams = new SqlParameter[2];
            List<ProvinceCodeTable> deleteLst = new List<ProvinceCodeTable>();
            int nCount = m_lstProvCode.Count;
            for (int i = 0; i < nCount; i++)
            {
                ProvinceCodeTable value = m_lstProvCode[i];
                sqlparams[0] = m_ObjSql.MakeParam("Code", SqlDbType.VarChar, 2, ParameterDirection.Input, value.ProvinceCode.ToString("X2"));
                sqlparams[1] = m_ObjSql.MakeParam("Name", SqlDbType.NVarChar, 50, ParameterDirection.Input, value.strProvinceName);
                if (value.eDbState == DbStateFlag.eDbAdd)
                {
                    m_ObjSql.ExecuteCommand("insert into Data_Province values(@Code,@Name)", sqlparams);
                    value.eDbState = DbStateFlag.eDbOK;
                }
                else if (value.eDbState == DbStateFlag.eDbDelete)
                {
                    m_ObjSql.ExecuteCommand("delete from Data_Province where ProvinceCode = @Code and ProvinceName = @Name", sqlparams);
                    deleteLst.Add(value);
                }
                else if (value.eDbState == DbStateFlag.eDbDirty)
                {
                    m_ObjSql.ExecuteCommand("update Data_Province set ProvinceCode = @Code, ProvinceName = @Name", sqlparams);
                    value.eDbState = DbStateFlag.eDbOK;
                }
                m_lstProvCode[i] = value;
            }

            foreach (ProvinceCodeTable temp in deleteLst)
            {
                m_lstProvCode.Remove(temp);
            }
            deleteLst.Clear();
        }

        private void ProvinceView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (m_nAuthority != GrobalVariable.CodeTable_Authority)
                e.Cancel = true;
        }
    }
}
