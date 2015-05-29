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
using PublishCardOperator.Dialog;

namespace PublishCardOperator
{
    public partial class PSAMKeyManage : Form, IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();
        private int m_nCurPage = 0; //当前显示页
        private int m_nRowsPerPage = 50;  //每页显示记录数
        private int m_nTotalPage = 1;  //总页数
        private List<PsamKeyValue> m_lstPsamKey = new List<PsamKeyValue>();
        private int m_nValidPsamKeyId = 0;   //配置默认使用的KeyId

        private bool m_bEditData = false;
        private int m_nEntered = -1; //相同行列的DataGridView CellEnter重复调用问题

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nKeyManageAuthority = 0;


        public PSAMKeyManage()
        {
            InitializeComponent();
        }


        public MenuType GetMenuType()
        {
            return MenuType.ePsamKeyManage;
        }

        public string PluginName()
        {
            return "PSAMKeyManage";
        }

        public Guid PluginGuid()
        {
            return new Guid("C670EAFE-5966-4aef-944E-F10D5790F0F8");
        }

        public string PluginMenu()
        {
            return "PSAM卡密钥管理";
        }

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
            if (m_nKeyManageAuthority != GrobalVariable.KeyManage_Authority)
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnEditKey.Enabled = false;
                btnSaveEdit.Enabled = false;
            }
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            m_nKeyManageAuthority = nAuthority;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PSAMKeyManage_Load(object sender, EventArgs e)
        {
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
                return;
            }            
            PsamKeyView.Columns.Clear();
            PsamKeyView.Columns.Add("KeyId", "序号");
            PsamKeyView.Columns.Add("KeyDetail", "密钥描述");
            PsamKeyView.Columns.Add("MasterKey", "卡片主控密钥");
            PsamKeyView.Columns.Add("MasterTendingKey", "卡片维护密钥");
            PsamKeyView.Columns.Add("AppMasterKey", "应用主控密钥");
            PsamKeyView.Columns.Add("AppTendingKey", "应用维护密钥");
            PsamKeyView.Columns.Add("ConsumerMasterKey", "消费主密钥");
            PsamKeyView.Columns.Add("GrayCardKey", "灰锁密钥");
            PsamKeyView.Columns.Add("MacEncryptKey", "MAC加密密钥");
            PsamKeyView.Columns.Add("KeyValid", "状态");
            for (int i = 0; i < PsamKeyView.Columns.Count; i++)
                PsamKeyView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            m_nValidPsamKeyId = 1;
            GetPsamKeyValid(ref m_nValidPsamKeyId);

            GetPsamKeyTotalPage();
            FillDataGridView(m_nValidPsamKeyId);

            btnEditKey.Enabled = m_bEditData ? false : true;
            btnSaveEdit.Enabled = m_bEditData ? true : false;
        }

        /// <summary>
        /// 获取初始当前使用的初始密钥
        /// </summary>
        private void GetPsamKeyValid(ref int nPsamKeyId)
        {
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select UsePsamKeyID from Config_SysParams", out dataReader);
            if (dataReader != null)
            {
                if (!dataReader.HasRows)
                    dataReader.Close();
                else
                {
                    if (dataReader.Read())
                    {                        
                        nPsamKeyId = (int)dataReader["UsePsamKeyID"];
                    }
                    dataReader.Close();
                }
            }
        }

        private string FillKeyValue(SqlDataReader dataReader, byte[] keyVal, string strName)
        {
            string strKeyVal = (string)dataReader[strName];
            byte[] byteKey = PublishCard.StringToBCD(strKeyVal);
            if (byteKey.Length == 16)
                Buffer.BlockCopy(byteKey, 0, keyVal, 0, 16);
            return strKeyVal;
        }

        private void GetPsamKeyTotalPage()
        {
            int nTotal = 0;
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select count(KeyId) Total from Key_PsamCard", out dataReader);
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

        private void FillDataGridView(int nPsamKeyId)
        {
            PsamKeyView.Rows.Clear();
            SqlDataReader dataReader = null;
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = m_ObjSql.MakeParam("KeyIdStart", SqlDbType.Int, 4, ParameterDirection.Input, m_nCurPage * m_nRowsPerPage);
            sqlparams[1] = m_ObjSql.MakeParam("KeyIdEnd", SqlDbType.Int, 4, ParameterDirection.Input, (m_nCurPage + 1) * m_nRowsPerPage);            
            m_ObjSql.ExecuteCommand("select * from Key_PsamCard where KeyId > @KeyIdStart and KeyId <= @KeyIdEnd", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    int nCount = 0;
                    string strKey = "";
                    while (dataReader.Read())
                    {
                        int index = PsamKeyView.Rows.Add();
                        int nId = (int)dataReader["KeyId"];
                        PsamKeyValue keyval = new PsamKeyValue();
                        keyval.eDbFlag = DbStateFlag.eDbOK;
                        keyval.nKeyId = nId;
                        keyval.bValid = false;
                        PsamKeyView.Rows[index].Cells[0].Value = m_nCurPage * m_nRowsPerPage + nCount + 1;
                        keyval.KeyDetail = (string)dataReader["InfoRemark"];
                        PsamKeyView.Rows[index].Cells[1].Value = keyval.KeyDetail;
                        strKey = FillKeyValue(dataReader, keyval.MasterKey, "MasterKey");
                        PsamKeyView.Rows[index].Cells[2].Value = strKey;
                        strKey = FillKeyValue(dataReader, keyval.MasterTendingKey, "MasterTendingKey");
                        PsamKeyView.Rows[index].Cells[3].Value = strKey;
                        strKey = FillKeyValue(dataReader, keyval.AppMasterKey, "ApplicatonMasterKey");
                        PsamKeyView.Rows[index].Cells[4].Value = strKey;
                        strKey = FillKeyValue(dataReader, keyval.AppTendingKey, "ApplicationTendingKey");
                        PsamKeyView.Rows[index].Cells[5].Value = strKey;
                        strKey = FillKeyValue(dataReader, keyval.ConsumerMasterKey, "ConsumerMasterKey");
                        PsamKeyView.Rows[index].Cells[6].Value = strKey;
                        strKey = FillKeyValue(dataReader, keyval.GrayCardKey, "GrayCardKey");
                        PsamKeyView.Rows[index].Cells[7].Value = strKey;
                        strKey = FillKeyValue(dataReader, keyval.MacEncryptKey, "MacEncryptKey");
                        PsamKeyView.Rows[index].Cells[8].Value = strKey;
                        if (nId == nPsamKeyId)
                        {
                            keyval.bValid = true;
                            PsamKeyView.Rows[index].Cells[9].Value = "使用";
                        }
                        else
                        {
                            PsamKeyView.Rows[index].Cells[9].Value = "未使用";
                        }
                        m_lstPsamKey.Add(keyval);
                        nCount++;
                    }
                }
                dataReader.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddPsamKey AddForm = new AddPsamKey();
            if (AddForm.ShowDialog(this) != DialogResult.OK)
                return;
            PsamKeyValue newPsamKey = AddForm.GetPsamKeyValue();
            newPsamKey.eDbFlag = DbStateFlag.eDbAdd;
            m_lstPsamKey.Add(newPsamKey);
            int index = PsamKeyView.Rows.Add();
            PsamKeyView.Rows[index].Cells[0].Value = m_nCurPage * m_nRowsPerPage + m_lstPsamKey.Count; //m_lstPsamKey记录已增加
            PsamKeyView.Rows[index].Cells[1].Value = newPsamKey.KeyDetail;
            PsamKeyView.Rows[index].Cells[2].Value = BitConverter.ToString(newPsamKey.MasterKey).Replace("-", "");
            PsamKeyView.Rows[index].Cells[3].Value = BitConverter.ToString(newPsamKey.MasterTendingKey).Replace("-", "");
            PsamKeyView.Rows[index].Cells[4].Value = BitConverter.ToString(newPsamKey.AppMasterKey).Replace("-", "");
            PsamKeyView.Rows[index].Cells[5].Value = BitConverter.ToString(newPsamKey.AppTendingKey).Replace("-", "");
            PsamKeyView.Rows[index].Cells[6].Value = BitConverter.ToString(newPsamKey.ConsumerMasterKey).Replace("-", "");
            PsamKeyView.Rows[index].Cells[7].Value = BitConverter.ToString(newPsamKey.GrayCardKey).Replace("-", "");
            PsamKeyView.Rows[index].Cells[8].Value = BitConverter.ToString(newPsamKey.MacEncryptKey).Replace("-", "");            
            SaveLstDataToDb();
            //存数据库后重新获取配置的KeyId
            m_nValidPsamKeyId = 1;
            GetPsamKeyValid(ref m_nValidPsamKeyId);
            int nAddId = m_lstPsamKey[index].nKeyId;
            if (nAddId == m_nValidPsamKeyId)
                PsamKeyView.Rows[index].Cells[9].Value = "使用";
            else
                PsamKeyView.Rows[index].Cells[9].Value = "未使用";
            UpdatePsamKeyValid(m_nValidPsamKeyId);
        }

        private void UpdatePsamKeyValid(int nValidPsamKeyId)
        {
            int nCount = m_lstPsamKey.Count;
            for (int i = 0; i < nCount; i++)
            {
                PsamKeyValue value = m_lstPsamKey[i];
                if (value.nKeyId == nValidPsamKeyId)
                    continue;
                value.bValid = false;
                PsamKeyView.Rows[i].Cells[9].Value = "未使用";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int nIndex = PsamKeyView.CurrentCell.RowIndex;
            PsamKeyView.Rows.RemoveAt(nIndex);
            PsamKeyValue value = m_lstPsamKey[nIndex];
            value.eDbFlag = DbStateFlag.eDbDelete;
            m_lstPsamKey[nIndex] = value;
            SaveLstDataToDb();
        }

        private void btnEditKey_Click(object sender, EventArgs e)
        {
            m_bEditData = true;
            btnEditKey.Enabled = m_bEditData ? false : true;
            btnSaveEdit.Enabled = m_bEditData ? true : false;

            int nItem = PsamKeyView.CurrentCell.ColumnIndex;
            if (nItem >= 1 && nItem <= 9)
            {
                //开始编辑
                int nIndex = PsamKeyView.CurrentCell.RowIndex;
                GetModifyView(nIndex, nItem, m_lstPsamKey[nIndex]);
                PsamKeyView.BeginEdit(true);
            }
        }

        private void CheckItemText(int nIndex, int nItem, byte[] keyVal)
        {
            string strValue = BitConverter.ToString(keyVal).Replace("-", "");
            if (strValue != (string)PsamKeyView.Rows[nIndex].Cells[nItem].Value)
                PsamKeyView.Rows[nIndex].Cells[nItem].Value = strValue;
        }

        private delegate void SetCellMethod(DataGridView ctrl, int nRow, int nCol, DataGridViewComboBoxCell cell);
        private void SetCellImpl(DataGridView ctrl, int nRow, int nCol, DataGridViewComboBoxCell cell)
        {
            m_nEntered = nCol;
            ctrl.Rows[nRow].Cells[nCol] = cell;
        }

        private void SetCell(DataGridView ctrl, int nRow, int nCol, DataGridViewComboBoxCell cell)
        {
            SetCellMethod MyMethod = new SetCellMethod(SetCellImpl);
            ctrl.BeginInvoke(MyMethod, new object[] { ctrl, nRow, nCol, cell });
        }

        private void GetModifyView(int nIndex, int nItem, PsamKeyValue value)
        {
            m_nEntered = -1;
            try
            {
                switch (nItem)
                {
                    case 1:
                        if (value.KeyDetail != (string)PsamKeyView.Rows[nIndex].Cells[nItem].Value)
                            PsamKeyView.Rows[nIndex].Cells[nItem].Value = value.KeyDetail;
                        break;
                    case 2:
                        CheckItemText(nIndex, nItem, value.MasterKey);
                        break;
                    case 3:
                        CheckItemText(nIndex, nItem, value.MasterTendingKey);
                        break;
                    case 4:
                        CheckItemText(nIndex, nItem, value.AppMasterKey);
                        break;
                    case 5:
                        CheckItemText(nIndex, nItem, value.AppTendingKey);
                        break;
                    case 6:
                        CheckItemText(nIndex, nItem, value.ConsumerMasterKey);
                        break;
                    case 7:
                        CheckItemText(nIndex, nItem, value.GrayCardKey);
                        break;
                    case 8:
                        CheckItemText(nIndex, nItem, value.MacEncryptKey);
                        break;
                    case 9:
                        DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                        comboCell.Items.Add("未使用");
                        comboCell.Items.Add("使用");
                        comboCell.Value = value.bValid ? "使用" : "未使用";
                        comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        if (nIndex == nItem)
                            SetCell(PsamKeyView, nIndex, nItem, comboCell);
                        else
                            PsamKeyView.Rows[nIndex].Cells[nItem] = comboCell;
                        break;

                }
            }
            catch
            {

            }
        }

        private byte[] GetPsamKeyByte(int nIndex, int nItem, byte[] srcByte)
        {
            string strKey = (string)PsamKeyView.Rows[nIndex].Cells[nItem].EditedFormattedValue;
            byte[] byteKey = PublishCard.StringToBCD(strKey);
            if (BitConverter.ToString(srcByte).Replace("-", "") != strKey)
            {
                if (byteKey.Length == 16)
                    return byteKey;
            }
            return null;
        }

        private void SetModifyView(int nIndex, int nItem, PsamKeyValue value)
        {
            try
            {
                switch (nItem)
                {
                    case 1:
                        {
                            string strDetail = (string)PsamKeyView.Rows[nIndex].Cells[nItem].EditedFormattedValue;
                            if (value.KeyDetail != strDetail)
                            {
                                value.KeyDetail = strDetail;
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }                            
                        }
                        break;
                    case 2:
                        {
                            byte[] KeyVal = GetPsamKeyByte(nIndex, nItem, value.MasterKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, value.MasterKey, 0, 16);
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }

                        }
                        break;
                    case 3:
                        {
                            byte[] KeyVal = GetPsamKeyByte(nIndex, nItem, value.MasterTendingKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, value.MasterTendingKey, 0, 16);
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }

                        }
                        break;
                    case 4:
                        {
                            byte[] KeyVal = GetPsamKeyByte(nIndex, nItem, value.AppMasterKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, value.AppMasterKey, 0, 16);
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }

                        }
                        break;
                    case 5:
                        {
                            byte[] KeyVal = GetPsamKeyByte(nIndex, nItem, value.AppTendingKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, value.AppTendingKey, 0, 16);
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }

                        }
                        break;
                    case 6:
                        {
                            byte[] KeyVal = GetPsamKeyByte(nIndex, nItem, value.ConsumerMasterKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, value.ConsumerMasterKey, 0, 16);
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }

                        }
                        break;
                    case 7:
                        {
                            byte[] KeyVal = GetPsamKeyByte(nIndex, nItem, value.GrayCardKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, value.GrayCardKey, 0, 16);
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }

                        }
                        break;
                    case 8:
                        {
                            byte[] KeyVal = GetPsamKeyByte(nIndex, nItem, value.MacEncryptKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, value.MacEncryptKey, 0, 16);
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }

                        }
                        break;
                    case 9:
                        {
                            string strContent = (string)PsamKeyView.Rows[nIndex].Cells[nItem].EditedFormattedValue;
                            bool bValid = strContent == "使用" ? true : false;
                            if (value.bValid != bValid)
                            {
                                value.bValid = bValid;
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }
                            //更新其他密钥为不使用状态。只能有一个处于“使用"状态
                            if (bValid)
                            {
                                m_nValidPsamKeyId = value.nKeyId;
                                UpdatePsamKeyValid(value.nKeyId);
                            }
                        }
                        break;
                }
                m_lstPsamKey[nIndex] = value;
            }
            catch
            {

            } 
        }

        private void ViewInputValidated()
        {
            int nItem = PsamKeyView.CurrentCell.ColumnIndex;
            if (nItem >= 1 && nItem <= 9)
            {
                //结束编辑
                int nIndex = PsamKeyView.CurrentCell.RowIndex;
                SetModifyView(nIndex, nItem, m_lstPsamKey[nIndex]);
                PsamKeyView.EndEdit();
            }
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            ViewInputValidated();
            m_bEditData = false;
            btnEditKey.Enabled = m_bEditData ? false : true;
            btnSaveEdit.Enabled = m_bEditData ? true : false;
            //存数据库
            SaveLstDataToDb();
        }

        private void SaveLstDataToDb()
        {
            int nCount = m_lstPsamKey.Count;
            List<PsamKeyValue> deleteLst = new List<PsamKeyValue>();
            string strBcd = "";
            for (int i = 0; i < nCount; i++)
            {
                PsamKeyValue value = m_lstPsamKey[i];
                if (value.eDbFlag != DbStateFlag.eDbOK)
                {
                    SqlParameter[] sqlparams = new SqlParameter[12];
                    sqlparams[0] = m_ObjSql.MakeParam("KeyId", SqlDbType.Int, 4, ParameterDirection.Input, value.nKeyId);

                    strBcd = BitConverter.ToString(value.MasterKey).Replace("-", "");
                    sqlparams[1] = m_ObjSql.MakeParam("MasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    strBcd = BitConverter.ToString(value.MasterTendingKey).Replace("-", "");
                    sqlparams[2] = m_ObjSql.MakeParam("MasterTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    strBcd = BitConverter.ToString(value.AppMasterKey).Replace("-", "");
                    sqlparams[3] = m_ObjSql.MakeParam("AppMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    strBcd = BitConverter.ToString(value.AppTendingKey).Replace("-", "");
                    sqlparams[4] = m_ObjSql.MakeParam("AppTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    strBcd = BitConverter.ToString(value.ConsumerMasterKey).Replace("-", "");
                    sqlparams[5] = m_ObjSql.MakeParam("ConsumerMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    strBcd = BitConverter.ToString(value.GrayCardKey).Replace("-", "");
                    sqlparams[6] = m_ObjSql.MakeParam("GrayCardKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    strBcd = BitConverter.ToString(value.MacEncryptKey).Replace("-", "");
                    sqlparams[7] = m_ObjSql.MakeParam("MacEncryptKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    sqlparams[8] = m_ObjSql.MakeParam("KeyDetail", SqlDbType.NVarChar, 50, ParameterDirection.Input, value.KeyDetail);
                    sqlparams[9] = m_ObjSql.MakeParam("KeyState", SqlDbType.Bit, 1, ParameterDirection.Input, value.bValid);
                    sqlparams[10] = m_ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, value.eDbFlag);
                    sqlparams[11] = m_ObjSql.MakeParam("AddKeyId", SqlDbType.Int, 4, ParameterDirection.Output, null);
                    if (m_ObjSql.ExecuteProc("PROC_UpdatePsamKey", sqlparams) == 0)
                    {
                        if (value.eDbFlag == DbStateFlag.eDbDelete)
                        {
                            deleteLst.Add(value);
                        }
                        else
                        {
                            if (value.eDbFlag == DbStateFlag.eDbAdd)
                            {
                                value.nKeyId = (int)sqlparams[11].Value;
                            } 
                            value.eDbFlag = DbStateFlag.eDbOK;
                            m_lstPsamKey[i] = value;
                        }
                    }
                }
            }//end for
            foreach (PsamKeyValue temp in deleteLst)
            {
                m_lstPsamKey.Remove(temp);
            }
            deleteLst.Clear();
        }

        private void PsamKeyView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!m_bEditData || e.ColumnIndex < 1 || e.ColumnIndex > 9)
                return;
            //PsamKeyView 行列相同时修改Cell（DataGridViewTextBoxCell-〉DataGridViewComboBoxCell）会引起SetCurrentCellAddressCore可重入调用
            if (m_nEntered == e.ColumnIndex && e.ColumnIndex == e.RowIndex)
            {
                PsamKeyView.BeginEdit(true);
                return;
            }
            GetModifyView(e.RowIndex, e.ColumnIndex, m_lstPsamKey[e.RowIndex]);
            PsamKeyView.BeginEdit(true);
        }

        private void PsamKeyView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!m_bEditData || e.ColumnIndex < 1 || e.ColumnIndex > 9)
                return;
            SetModifyView(e.RowIndex, e.ColumnIndex, m_lstPsamKey[e.RowIndex]);
            PsamKeyView.EndEdit();
        }

        private void PSAMKeyManage_FormClosed(object sender, FormClosedEventArgs e)
        {
            bool bSave = false;
            foreach (PsamKeyValue value in m_lstPsamKey)
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

            PsamKeyView.Dispose();
            m_lstPsamKey.Clear();
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (m_nTotalPage <= 1)
                return;
            if (m_bEditData)
            {
                ViewInputValidated();
                m_bEditData = false;
                btnEditKey.Enabled = m_bEditData ? false : true;
                btnSaveEdit.Enabled = m_bEditData ? true : false;
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
                FillDataGridView(m_nValidPsamKeyId);
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (m_nTotalPage <= 1)
                return;
            if (m_bEditData)
            {
                ViewInputValidated();
                m_bEditData = false;
                btnEditKey.Enabled = m_bEditData ? false : true;
                btnSaveEdit.Enabled = m_bEditData ? true : false;
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
                FillDataGridView(m_nValidPsamKeyId);
            }
        }
    }
}