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
    public partial class KeyManage : Form, IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();
        private int m_nCurPage = 0; //当前显示页
        private int m_nRowsPerPage = 50;  //每页显示记录数
        private int m_nTotalPage = 1;  //总页数
        private List<CpuKeyValue> m_lstCpuKey = new List<CpuKeyValue>();
        private bool m_bEditData = false;
        private int m_nFocusKeyId = 0;
        private int m_nClickGridView = 1;

        private int m_nValidKeyId = 0;   //配置默认使用的KeyId

        private int MAX_APP_COUNT = 3; //CPU卡的最大应用数
        private int m_nEnteredCpuGrid = -1; //相同行列的DataGridView CellEnter重复调用问题
        private int m_nEnteredAppGrid = -1;

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nKeyManageAuthority = 0;


        public KeyManage()
        {
            InitializeComponent();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eUserKeysManage;
        }

        public string PluginName()
        {
            return "KeyManage";
        }

        public Guid PluginGuid()
        {
            return new Guid("A24CEFE8-E4ED-4808-891B-E3DBB203C600");
        }

        public string PluginMenu()
        {
            return "用户卡密钥管理";
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

        private void KeyManage_Load(object sender, EventArgs e)
        {
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
                return;
            }
            CpuKeyGridView.Columns.Clear();
            CpuKeyGridView.Columns.Add("KeyId", "序号");
            CpuKeyGridView.Columns.Add("KeyDetail", "密钥描述");
            CpuKeyGridView.Columns.Add("MasterKey", "卡片主控密钥");
            CpuKeyGridView.Columns.Add("MasterTendingKey", "卡片维护密钥");
            CpuKeyGridView.Columns.Add("InternalAuthKey", "内部认证密钥");
            CpuKeyGridView.Columns.Add("KeyValid", "状态");
            for (int i = 0; i < CpuKeyGridView.Columns.Count; i++)
                CpuKeyGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            AppKeyGridView.Columns.Clear();
            AppKeyGridView.Columns.Add("Id", "");
            AppKeyGridView.Columns.Add("AppIndex", "应用号");
            AppKeyGridView.Columns.Add("AppMasterKey", "应用主控密钥");
            AppKeyGridView.Columns.Add("AppTendingKey", "应用维护密钥");
            AppKeyGridView.Columns.Add("AppInternalAuthKey", "应用内部认证\n密钥");
            AppKeyGridView.Columns.Add("AppPinResetKey", "PIN密码重装\n密钥");
            AppKeyGridView.Columns.Add("AppPinUnlockKey", "PIN解锁密钥");
            AppKeyGridView.Columns.Add("ConsumerMasterKey", "消费主密钥");
            AppKeyGridView.Columns.Add("LoadKey", "圈存密钥");
            AppKeyGridView.Columns.Add("UnloadKey", "圈提密钥");
            AppKeyGridView.Columns.Add("TacMasterKey", "TAC密钥");
            AppKeyGridView.Columns.Add("UnGrayKey", "联机解扣密钥");
            AppKeyGridView.Columns.Add("OverdraftKey", "修改透支限额\n密钥");
            for (int i = 0; i < AppKeyGridView.Columns.Count; i++)
                AppKeyGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            AppKeyGridView.Columns[0].Width = 0;

            m_nValidKeyId = 1;
            GetCpuKeyValid(ref m_nValidKeyId);

            GetCpuKeyTotalPage();
            FillDataGridView(m_nValidKeyId);
            if (m_lstCpuKey.Count > 0)
            {
                m_nFocusKeyId = m_lstCpuKey[0].nKeyId;
                FillAppGridView(m_nFocusKeyId);
            }
            btnEditKey.Enabled = m_bEditData ? false : true;
            btnSaveEdit.Enabled = m_bEditData ? true : false;
        }

        private void GetCpuKeyValid(ref int nCpuKeyId)
        {
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select UseKeyID from Config_SysParams", out dataReader);
            if (dataReader != null)
            {
                if (!dataReader.HasRows)
                    dataReader.Close();
                else
                {
                    if (dataReader.Read())
                    {
                        nCpuKeyId = (int)dataReader["UseKeyID"];
                    }
                    dataReader.Close();
                }
            }
        }

        private string FillKeyValue(SqlDataReader dataReader, byte[] keyVal, string strName)
        {
            string strKeyVal = (string)dataReader[strName];
            byte[] byteKey = PublicFunc.StringToBCD(strKeyVal);
            if (byteKey.Length == 16)
                Buffer.BlockCopy(byteKey, 0, keyVal, 0, 16);
            return strKeyVal;
        }

        private void GetCpuKeyTotalPage()
        {
            int nTotal = 0;
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select count(KeyId) Total from Key_CpuCard", out dataReader);
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


        private void FillDataGridView(int nCpuKeyId)
        {
            CpuKeyGridView.Rows.Clear();
            SqlDataReader dataReader = null;
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = m_ObjSql.MakeParam("KeyIdStart", SqlDbType.Int, 4, ParameterDirection.Input, m_nCurPage * m_nRowsPerPage);
            sqlparams[1] = m_ObjSql.MakeParam("KeyIdEnd", SqlDbType.Int, 4, ParameterDirection.Input, (m_nCurPage + 1) * m_nRowsPerPage);
            m_ObjSql.ExecuteCommand("select * from Key_CpuCard where KeyId > @KeyIdStart and KeyId <= @KeyIdEnd", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    int nCount = 0;
                    string strKey = "";
                    while (dataReader.Read())
                    {
                        int index = CpuKeyGridView.Rows.Add();
                        int nId = (int)dataReader["KeyId"];
                        CpuKeyValue keyval = new CpuKeyValue();
                        keyval.eDbFlag = DbStateFlag.eDbOK;
                        keyval.nKeyId = nId;
                        keyval.bValid = false;
                        CpuKeyGridView.Rows[index].Cells[0].Value = m_nCurPage * m_nRowsPerPage + nCount + 1;
                        keyval.KeyDetail = (string)dataReader["InfoRemark"];
                        CpuKeyGridView.Rows[index].Cells[1].Value = keyval.KeyDetail;
                        strKey = FillKeyValue(dataReader, keyval.MasterKey, "MasterKey");
                        CpuKeyGridView.Rows[index].Cells[2].Value = strKey;
                        strKey = FillKeyValue(dataReader, keyval.MasterTendingKey, "MasterTendingKey");
                        CpuKeyGridView.Rows[index].Cells[3].Value = strKey;
                        strKey = FillKeyValue(dataReader, keyval.InternalAuthKey, "InternalAuthKey");
                        CpuKeyGridView.Rows[index].Cells[4].Value = strKey;                        
                        if (nId == nCpuKeyId)
                        {
                            keyval.bValid = true;
                            CpuKeyGridView.Rows[index].Cells[5].Value = "使用";
                        }
                        else
                        {
                            CpuKeyGridView.Rows[index].Cells[5].Value = "未使用";
                        }
                        m_lstCpuKey.Add(keyval);
                        nCount++;
                    }
                }
                dataReader.Close();
            }

            foreach (CpuKeyValue value in m_lstCpuKey)
            {
                FillLstAppKey(value.nKeyId, value.LstAppKeyGroup);
            }

        }

        private void FillLstAppKey(int nRelatedKeyId, List<AppKeyValueGroup> lstAppKey)
        {
            SqlDataReader dataReader = null;
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = m_ObjSql.MakeParam("RelatedKeyId", SqlDbType.Int, 4, ParameterDirection.Input, nRelatedKeyId);
            m_ObjSql.ExecuteCommand("select * from Key_CARD_ADF where RelatedKeyId = @RelatedKeyId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        AppKeyValueGroup keyval = new AppKeyValueGroup();
                        keyval.eDbFlag = DbStateFlag.eDbOK;
                        keyval.AppIndex = (int)dataReader["ApplicationIndex"];
                        FillKeyValue(dataReader, keyval.AppMasterKey, "ApplicatonMasterKey");
                        FillKeyValue(dataReader, keyval.AppTendingKey, "ApplicationTendingKey");
                        FillKeyValue(dataReader, keyval.AppInternalAuthKey, "AppInternalAuthKey");
                        FillKeyValue(dataReader, keyval.PINResetKey, "PINResetKey");
                        FillKeyValue(dataReader, keyval.PINUnlockKey, "PINUnlockKey");
                        FillKeyValue(dataReader, keyval.ConsumerMasterKey, "ConsumerMasterKey");
                        FillKeyValue(dataReader, keyval.LoadKey, "LoadKey");
                        FillKeyValue(dataReader, keyval.UnLoadKey, "UnLoadKey");
                        FillKeyValue(dataReader, keyval.TacMasterKey, "TacMasterKey");
                        FillKeyValue(dataReader, keyval.UnGrayKey, "UnGrayKey");
                        FillKeyValue(dataReader, keyval.OverdraftKey, "OverdraftKey");
                        lstAppKey.Add(keyval);
                    }
                }
                dataReader.Close();
            }
        }

        private void FillAppGridView(int nRelatedKeyId)
        {
            AppKeyGridView.Rows.Clear();
            int nListIndex = GetFocusAppKeyIndex(nRelatedKeyId);
            if (nListIndex == -1)
                return;
            int nCount = m_lstCpuKey[nListIndex].LstAppKeyGroup.Count;
            for (int i = 0; i < nCount; i++)
            {
                if (i >= MAX_APP_COUNT)
                    break;
                AppKeyValueGroup AppKeyGroup = m_lstCpuKey[nListIndex].LstAppKeyGroup[i];
                int index = AppKeyGridView.Rows.Add();
                AppKeyGridView.Rows[index].Cells[0].Value = "";
                AppKeyGridView.Rows[index].Cells[1].Value = AppKeyGroup.AppIndex;
                AppKeyGridView.Rows[index].Cells[2].Value = BitConverter.ToString(AppKeyGroup.AppMasterKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[3].Value = BitConverter.ToString(AppKeyGroup.AppTendingKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[4].Value = BitConverter.ToString(AppKeyGroup.AppInternalAuthKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[5].Value = BitConverter.ToString(AppKeyGroup.PINResetKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[6].Value = BitConverter.ToString(AppKeyGroup.PINUnlockKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[7].Value = BitConverter.ToString(AppKeyGroup.ConsumerMasterKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[8].Value = BitConverter.ToString(AppKeyGroup.LoadKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[9].Value = BitConverter.ToString(AppKeyGroup.UnLoadKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[10].Value = BitConverter.ToString(AppKeyGroup.TacMasterKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[11].Value = BitConverter.ToString(AppKeyGroup.UnGrayKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[12].Value = BitConverter.ToString(AppKeyGroup.OverdraftKey).Replace("-", "");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (m_nClickGridView == 1)
            {
                int nIndex = CpuKeyGridView.CurrentCell.RowIndex;
                CpuKeyGridView.Rows.RemoveAt(nIndex);
                AppKeyGridView.Rows.Clear();
                CpuKeyValue value = m_lstCpuKey[nIndex];
                for (int i = 0; i < value.LstAppKeyGroup.Count; i++)
                {
                    value.LstAppKeyGroup[i].eDbFlag = DbStateFlag.eDbDelete;
                }
                value.eDbFlag = DbStateFlag.eDbDelete;
                m_lstCpuKey[nIndex] = value;
                SaveLstDataToDb();
            }
            else if (m_nClickGridView == 2)
            {
                int nIndex = AppKeyGridView.CurrentCell.RowIndex;
                AppKeyGridView.Rows.RemoveAt(nIndex);
                int nListIndex = GetFocusAppKeyIndex(m_nFocusKeyId);
                if (nListIndex != -1)
                {
                    AppKeyValueGroup value = m_lstCpuKey[nListIndex].LstAppKeyGroup[nIndex];
                    value.eDbFlag = DbStateFlag.eDbDelete;
                    m_lstCpuKey[nListIndex].LstAppKeyGroup[nIndex] = value;
                    SaveLstDataToDb();
                }
            }
        }

        private void btnEditKey_Click(object sender, EventArgs e)
        {
            m_bEditData = true;
            btnEditKey.Enabled = m_bEditData ? false : true;
            btnSaveEdit.Enabled = m_bEditData ? true : false;

            if (m_nClickGridView == 1)
            {
                int nItem = CpuKeyGridView.CurrentCell.ColumnIndex;
                if (nItem >= 1 && nItem <= 5)
                {
                    //开始编辑
                    int nIndex = CpuKeyGridView.CurrentCell.RowIndex;
                    GetModifyCpuView(nIndex, nItem, m_lstCpuKey[nIndex]);
                    CpuKeyGridView.BeginEdit(true);
                }
            }
            else if (m_nClickGridView == 2)
            {
                int nItem = AppKeyGridView.CurrentCell.ColumnIndex;
                //开始编辑
                int nIndex = AppKeyGridView.CurrentCell.RowIndex;
                int nKeyIndex = GetFocusAppKeyIndex(m_nFocusKeyId);
                if (nKeyIndex != -1)
                {
                    GetModifyAppView(nIndex, nItem, m_lstCpuKey[nKeyIndex].LstAppKeyGroup[nIndex]);
                    AppKeyGridView.BeginEdit(true);
                }
            }
        }

        private void ViewInputValidated()
        {
            if (m_nClickGridView == 1)
            {
                int nItem = CpuKeyGridView.CurrentCell.ColumnIndex;
                if (nItem >= 1 && nItem <= 5)
                {
                    //结束编辑
                    int nIndex = CpuKeyGridView.CurrentCell.RowIndex;
                    SetModifyCpuView(nIndex, nItem, m_lstCpuKey[nIndex]);
                    CpuKeyGridView.EndEdit();
                }
            }
            else if (m_nClickGridView == 2)
            {
                int nItem = AppKeyGridView.CurrentCell.ColumnIndex;
                //结束编辑
                int nIndex = AppKeyGridView.CurrentCell.RowIndex;
                int nKeyIndex = GetFocusAppKeyIndex(m_nFocusKeyId);
                if (nKeyIndex != -1)
                {
                    SetModifyAppView(nIndex, nItem, m_lstCpuKey[nKeyIndex].LstAppKeyGroup[nIndex]);
                    AppKeyGridView.EndEdit();
                }
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

        //更新AppKeyGridView
        private void CpuKeyGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (m_lstCpuKey.Count <= 0 || CpuKeyGridView.CurrentCell == null)
                return;
            int nIndex = CpuKeyGridView.CurrentCell.RowIndex;
            int nRelatedId = m_lstCpuKey[nIndex].nKeyId;
            if (m_nFocusKeyId != nRelatedId)
            {
                m_nFocusKeyId = nRelatedId;
                FillAppGridView(m_nFocusKeyId);
            }

        }

        private int GetFocusAppKeyIndex(int nRelatedKeyId)
        {
            int nSelIndex = -1;
            int nListIndex = 0;
            foreach (CpuKeyValue value in m_lstCpuKey)
            {
                if (value.nKeyId == nRelatedKeyId)
                {
                    nSelIndex = nListIndex;
                    break;
                }
                nListIndex++;
            }
            return nSelIndex;
        }

        private void AppKeyGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!m_bEditData || e.ColumnIndex < 1 || e.ColumnIndex > 11)
                return;
            CpuKeyGridView.ReadOnly = true;
            //行列相同时修改Cell（DataGridViewTextBoxCell-〉DataGridViewComboBoxCell）会引起SetCurrentCellAddressCore可重入调用
            if (m_nEnteredAppGrid == e.ColumnIndex && e.ColumnIndex == e.RowIndex)
            {
                AppKeyGridView.BeginEdit(true);
                return;
            }
            int nIndex = GetFocusAppKeyIndex(m_nFocusKeyId);
            if (nIndex != -1)
            {
                GetModifyAppView(e.RowIndex, e.ColumnIndex, m_lstCpuKey[nIndex].LstAppKeyGroup[e.RowIndex]);
                AppKeyGridView.BeginEdit(true);
            }
        }

        private void AppKeyGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!m_bEditData || e.ColumnIndex < 1 || e.ColumnIndex > 11)
                return;
            int nIndex = GetFocusAppKeyIndex(m_nFocusKeyId);
            if (nIndex != -1)
            {
                SetModifyAppView(e.RowIndex, e.ColumnIndex, m_lstCpuKey[nIndex].LstAppKeyGroup[e.RowIndex]);
                AppKeyGridView.EndEdit();
                CpuKeyGridView.ReadOnly = false;
            }
        }

        private void CpuKeyGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {            
            if (!m_bEditData || e.ColumnIndex < 1 || e.ColumnIndex > 5)
                return;
            AppKeyGridView.ReadOnly = true;
            //PsamKeyView 行列相同时修改Cell（DataGridViewTextBoxCell-〉DataGridViewComboBoxCell）会引起SetCurrentCellAddressCore可重入调用
            if (m_nEnteredAppGrid == e.ColumnIndex && e.ColumnIndex == e.RowIndex)
            {
                CpuKeyGridView.BeginEdit(true);
                return;
            }
            GetModifyCpuView(e.RowIndex, e.ColumnIndex, m_lstCpuKey[e.RowIndex]);
            CpuKeyGridView.BeginEdit(true);
        }

        private void CpuKeyGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!m_bEditData || e.ColumnIndex < 1 || e.ColumnIndex > 5)
                return;
            SetModifyCpuView(e.RowIndex, e.ColumnIndex, m_lstCpuKey[e.RowIndex]);
            CpuKeyGridView.EndEdit();
            AppKeyGridView.ReadOnly = false;
        }

        private delegate void SetCellMethod(DataGridView ctrl, int nRow, int nCol, DataGridViewComboBoxCell cell);
        private void SetCellImpl(DataGridView ctrl, int nRow, int nCol, DataGridViewComboBoxCell cell)
        {
            if (ctrl == CpuKeyGridView)
                m_nEnteredCpuGrid = nCol;
            else if (ctrl == AppKeyGridView)
                m_nEnteredAppGrid = nCol;
            ctrl.Rows[nRow].Cells[nCol] = cell;
        }

        private void SetCell(DataGridView ctrl, int nRow, int nCol, DataGridViewComboBoxCell cell)
        {
            SetCellMethod MyMethod = new SetCellMethod(SetCellImpl);
            ctrl.BeginInvoke(MyMethod, new object[] { ctrl, nRow, nCol, cell });
        }

        private void GetModifyCpuView(int nIndex, int nItem, CpuKeyValue KeyVal)
        {
            m_nEnteredCpuGrid = -1;
            try
            {
                switch (nItem)
                {
                    case 1:
                        if (KeyVal.KeyDetail != (string)AppKeyGridView.Rows[nIndex].Cells[nItem].Value)
                            AppKeyGridView.Rows[nIndex].Cells[nItem].Value = KeyVal.KeyDetail;
                        break;
                    case 2:
                        CheckItemText(nIndex, nItem, KeyVal.MasterKey);
                        break;
                    case 3:
                        CheckItemText(nIndex, nItem, KeyVal.MasterTendingKey);
                        break;
                    case 4:
                        CheckItemText(nIndex, nItem, KeyVal.InternalAuthKey);
                        break;
                    case 5:
                        {
                            DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                            comboCell.Items.Add("未使用");
                            comboCell.Items.Add("使用");
                            comboCell.Value = KeyVal.bValid ? "使用" : "未使用";
                            comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                            if (nIndex == nItem)
                                SetCell(CpuKeyGridView, nIndex, nItem, comboCell);
                            else
                                CpuKeyGridView.Rows[nIndex].Cells[nItem] = comboCell;
                        }
                        break;
                }
            }
            catch
            {

            }
        }

        private void SetModifyCpuView(int nIndex, int nItem, CpuKeyValue KeyVal)
        {
            try
            {
                switch (nItem)
                {
                    case 1:
                        {
                            string strDetail = (string)CpuKeyGridView.Rows[nIndex].Cells[nItem].EditedFormattedValue;
                            if(strDetail  != KeyVal.KeyDetail)
                            {
                                KeyVal.KeyDetail = strDetail;
                                KeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 2:
                        {
                            byte[] ByteVal = GetCpuKeyByte(CpuKeyGridView, nIndex, nItem, KeyVal.MasterKey);
                            if (ByteVal != null)
                            {
                                Buffer.BlockCopy(ByteVal, 0, KeyVal.MasterKey, 0, 16);
                                KeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 3:
                        {
                            byte[] ByteVal = GetCpuKeyByte(CpuKeyGridView, nIndex, nItem, KeyVal.MasterTendingKey);
                            if (ByteVal != null)
                            {
                                Buffer.BlockCopy(ByteVal, 0, KeyVal.MasterTendingKey, 0, 16);
                                KeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 4:
                        {
                            byte[] ByteVal = GetCpuKeyByte(CpuKeyGridView, nIndex, nItem, KeyVal.InternalAuthKey);
                            if (ByteVal != null)
                            {
                                Buffer.BlockCopy(ByteVal, 0, KeyVal.InternalAuthKey, 0, 16);
                                KeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 5:
                        {
                            string strContent = (string)CpuKeyGridView.Rows[nIndex].Cells[nItem].EditedFormattedValue;
                            bool bValid = strContent == "使用" ? true : false;
                            if (KeyVal.bValid != bValid)
                            {
                                KeyVal.bValid = bValid;
                                KeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                            //更新其他密钥为不使用状态。只能有一个处于“使用"状态
                            if (bValid)
                            {
                                m_nValidKeyId = KeyVal.nKeyId;
                                UpdateCpuKeyValid(KeyVal.nKeyId);
                            }
                        }
                        break;
                }
            }
            catch
            {

            }
        }

        private void UpdateCpuKeyValid(int nValidKeyId)
        {
            int nCount = m_lstCpuKey.Count;
            for (int i = 0; i < nCount; i++)
            {
                CpuKeyValue value = m_lstCpuKey[i];
                if (value.nKeyId == nValidKeyId)
                    continue;
                value.bValid = false;
                CpuKeyGridView.Rows[i].Cells[5].Value = "未使用";
            }
        }

        /// <summary>
        /// 应用密钥修改
        /// </summary>
        /// <param name="nAppIndex"></param>
        /// <param name="nItem"></param>
        /// <param name="AppKeyVal"></param>
        private void GetModifyAppView(int nAppIndex, int nItem, AppKeyValueGroup AppKeyVal)
        {
            m_nEnteredAppGrid = -1;
            try
            {
                switch (nItem)
                {
                    case 1:
                        {
                            DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                            comboCell.Items.Add("1");
                            comboCell.Items.Add("2");
                            comboCell.Items.Add("3");
                            comboCell.Value = AppKeyVal.AppIndex.ToString();
                            comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                            if (nAppIndex == nItem)
                                SetCell(AppKeyGridView, nAppIndex, nItem, comboCell);
                            else
                                AppKeyGridView.Rows[nAppIndex].Cells[nItem] = comboCell;
                        }
                        break;
                    case 2:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.AppMasterKey);
                        break;
                    case 3:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.AppTendingKey);
                        break;
                    case 4:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.AppInternalAuthKey);
                        break;
                    case 5:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.PINResetKey);
                        break;
                    case 6:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.PINUnlockKey);
                        break;
                    case 7:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.ConsumerMasterKey);
                        break;
                    case 8:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.LoadKey);
                        break;
                    case 9:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.UnLoadKey);
                        break;
                    case 10:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.TacMasterKey);
                        break;
                    case 11:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.UnGrayKey);
                        break;
                    case 12:
                        CheckItemText(nAppIndex, nItem, AppKeyVal.OverdraftKey);
                        break;
                }
            }
            catch
            {

            }
        }

        private void CheckItemText(int nIndex, int nItem, byte[] keyVal)
        {
            string strValue = BitConverter.ToString(keyVal).Replace("-", "");
            if (strValue != (string)AppKeyGridView.Rows[nIndex].Cells[nItem].Value)
                AppKeyGridView.Rows[nIndex].Cells[nItem].Value = strValue;
        }

        private byte[] GetCpuKeyByte(DataGridView GridView, int nIndex, int nItem, byte[] srcByte)
        {
            string strKey = (string)GridView.Rows[nIndex].Cells[nItem].EditedFormattedValue;
            byte[] byteKey = PublicFunc.StringToBCD(strKey);
            if (BitConverter.ToString(srcByte).Replace("-", "") != strKey)
            {
                if (byteKey.Length == 16)
                    return byteKey;
            }
            return null;
        }


        /// <summary>
        /// 应用密钥修改
        /// </summary>
        /// <param name="nAppIndex">记录索引号</param>
        /// <param name="nItem">列索引</param>
        /// <param name="AppKeyVal">应用密钥</param>
        private void SetModifyAppView(int nAppIndex, int nItem, AppKeyValueGroup AppKeyVal)
        {
            try
            {
                switch (nItem)
                {
                    case 1:
                        {
                            string strContent = (string)AppKeyGridView.Rows[nAppIndex].Cells[nItem].EditedFormattedValue;
                            int nEditedAppIndex = Convert.ToInt32(strContent);
                            if (AppKeyVal.AppIndex != nEditedAppIndex)
                            {
                                AppKeyVal.AppIndex = nEditedAppIndex;
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 2:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.AppMasterKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.AppMasterKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 3:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.AppTendingKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.AppTendingKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 4:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.AppInternalAuthKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.AppInternalAuthKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 5:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.PINResetKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.PINResetKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 6:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.PINUnlockKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.PINUnlockKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 7:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.ConsumerMasterKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.ConsumerMasterKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 8:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.LoadKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.LoadKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 9:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.UnLoadKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.UnLoadKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 10:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.TacMasterKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.TacMasterKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 11:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.UnGrayKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.UnGrayKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 12:
                        {
                            byte[] KeyVal = GetCpuKeyByte(AppKeyGridView, nAppIndex, nItem, AppKeyVal.OverdraftKey);
                            if (KeyVal != null)
                            {
                                Buffer.BlockCopy(KeyVal, 0, AppKeyVal.OverdraftKey, 0, 16);
                                AppKeyVal.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;

                }
                int nKeyIndex = GetFocusAppKeyIndex(m_nFocusKeyId);
                if (nKeyIndex != -1)
                    m_lstCpuKey[nKeyIndex].LstAppKeyGroup[nAppIndex] = AppKeyVal;
            }
            catch
            {

            }
        }

        private void SaveLstDataToDb()
        {
            string strBcd = "";
            //先要存储卡片密钥，以便新增的记录获得KeyId
            List<CpuKeyValue> deleteLst = new List<CpuKeyValue>();
            for (int i = 0; i < m_lstCpuKey.Count; i++)
            {
                CpuKeyValue value = m_lstCpuKey[i];
                if (value.eDbFlag != DbStateFlag.eDbOK)
                {
                    SqlParameter[] sqlparams = new SqlParameter[8];
                    sqlparams[0] = m_ObjSql.MakeParam("KeyId", SqlDbType.Int, 4, ParameterDirection.Input, value.nKeyId);

                    strBcd = BitConverter.ToString(value.MasterKey).Replace("-", "");
                    sqlparams[1] = m_ObjSql.MakeParam("MasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    strBcd = BitConverter.ToString(value.MasterTendingKey).Replace("-", "");
                    sqlparams[2] = m_ObjSql.MakeParam("MasterTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    strBcd = BitConverter.ToString(value.InternalAuthKey).Replace("-", "");
                    sqlparams[3] = m_ObjSql.MakeParam("InternalAuthKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                    sqlparams[4] = m_ObjSql.MakeParam("KeyDetail", SqlDbType.NVarChar, 50, ParameterDirection.Input, value.KeyDetail);
                    sqlparams[5] = m_ObjSql.MakeParam("KeyState", SqlDbType.Bit, 1, ParameterDirection.Input, value.bValid);
                    sqlparams[6] = m_ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, value.eDbFlag);
                    sqlparams[7] = m_ObjSql.MakeParam("AddKeyId", SqlDbType.Int, 4, ParameterDirection.Output, null);
                    if (m_ObjSql.ExecuteProc("PROC_UpdateCpuKey", sqlparams) == 0)
                    {
                        if (value.eDbFlag == DbStateFlag.eDbDelete)
                        {
                            deleteLst.Add(value);
                        }
                        else
                        {
                            if (value.eDbFlag == DbStateFlag.eDbAdd)
                            {
                                value.nKeyId = (int)sqlparams[7].Value;
                            }
                            value.eDbFlag = DbStateFlag.eDbOK;
                            m_lstCpuKey[i] = value;
                        }
                    }
                }
            }//end for

            //存储应用密钥
            for (int i = 0; i < m_lstCpuKey.Count; i++)
            {
                int nCount = m_lstCpuKey[i].LstAppKeyGroup.Count;
                int nRelatedKeyId = m_lstCpuKey[i].nKeyId;
                List<AppKeyValueGroup> deleteApp = new List<AppKeyValueGroup>();
                for (int j = 0; j < nCount; j++)
                {
                    if (j >= MAX_APP_COUNT)
                        break;
                    AppKeyValueGroup AppKey = m_lstCpuKey[i].LstAppKeyGroup[j];
                    if (AppKey.eDbFlag != DbStateFlag.eDbOK)
                    {
                        SqlParameter[] sqlparams = new SqlParameter[14];
                        sqlparams[0] = m_ObjSql.MakeParam("RelatedKeyId", SqlDbType.Int, 4, ParameterDirection.Input, nRelatedKeyId);
                        sqlparams[1] = m_ObjSql.MakeParam("AppIndex", SqlDbType.Int, 4, ParameterDirection.Input, AppKey.AppIndex);

                        strBcd = BitConverter.ToString(AppKey.AppMasterKey).Replace("-", "");
                        sqlparams[2] = m_ObjSql.MakeParam("AppMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.AppTendingKey).Replace("-", "");
                        sqlparams[3] = m_ObjSql.MakeParam("AppTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.AppInternalAuthKey).Replace("-", "");
                        sqlparams[4] = m_ObjSql.MakeParam("AppInternalAuthKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.PINResetKey).Replace("-", "");
                        sqlparams[5] = m_ObjSql.MakeParam("PinResetKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.PINUnlockKey).Replace("-", "");
                        sqlparams[6] = m_ObjSql.MakeParam("PinUnlockKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.ConsumerMasterKey).Replace("-", "");
                        sqlparams[7] = m_ObjSql.MakeParam("ConsumerMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.LoadKey).Replace("-", "");
                        sqlparams[8] = m_ObjSql.MakeParam("LoadKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.UnLoadKey).Replace("-", "");
                        sqlparams[9] = m_ObjSql.MakeParam("UnLoadKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.TacMasterKey).Replace("-", "");
                        sqlparams[10] = m_ObjSql.MakeParam("TacMasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.UnGrayKey).Replace("-", "");
                        sqlparams[11] = m_ObjSql.MakeParam("UnGrayKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        strBcd = BitConverter.ToString(AppKey.OverdraftKey).Replace("-", "");
                        sqlparams[12] = m_ObjSql.MakeParam("OvertraftKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);

                        sqlparams[13] = m_ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, AppKey.eDbFlag);

                        if (m_ObjSql.ExecuteProc("PROC_UpdateCpuAppKey", sqlparams) == 0)
                        {
                            if (AppKey.eDbFlag == DbStateFlag.eDbDelete)
                            {
                                deleteApp.Add(AppKey);
                            }
                            else
                            {
                                AppKey.eDbFlag = DbStateFlag.eDbOK;
                                m_lstCpuKey[i].LstAppKeyGroup[j] = AppKey;
                            }
                        }

                    }
                }//end for
                foreach (AppKeyValueGroup temp in deleteApp)
                {
                    m_lstCpuKey[i].LstAppKeyGroup.Remove(temp);
                }
                deleteApp.Clear();
            }

            //删除卡片密钥：应用密钥删除后才能操作
            foreach (CpuKeyValue temp in deleteLst)
            {
                m_lstCpuKey.Remove(temp);
            }
            deleteLst.Clear();
        }

        private void CpuKeyGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (CpuKeyGridView.CurrentCell.ColumnIndex < 2 || CpuKeyGridView.CurrentCell.ColumnIndex > 4)
                return;
            DataGridViewTextBoxEditingControl TextBoxCtrl = (DataGridViewTextBoxEditingControl)e.Control;
            TextBoxCtrl.MaxLength = 32;
        }

        private void AppKeyGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (AppKeyGridView.CurrentCell.ColumnIndex < 2 || AppKeyGridView.CurrentCell.ColumnIndex > 11)
                return;
            DataGridViewTextBoxEditingControl TextBoxCtrl = (DataGridViewTextBoxEditingControl)e.Control;
            TextBoxCtrl.MaxLength = 32;
        }

        //添加CPU卡相关密钥,添加卡片主控密钥时，所有应用密钥eDbState都要设置为eDbAdd
        private void btnAdd_Click(object sender, EventArgs e)
        {            
            if (m_nClickGridView == 1)
            {
                AddCpuKey AddForm = new AddCpuKey();
                AddForm.SetMaxAppCount(MAX_APP_COUNT);
                if (AddForm.ShowDialog(this) != DialogResult.OK)
                    return;
                CpuKeyValue newCpuKey = AddForm.GetCpuKeyValue();
                newCpuKey.eDbFlag = DbStateFlag.eDbAdd;
                for (int i = 0; i < newCpuKey.LstAppKeyGroup.Count; i++)
                    newCpuKey.LstAppKeyGroup[i].eDbFlag = DbStateFlag.eDbAdd;
                m_lstCpuKey.Add(newCpuKey);
                int index = CpuKeyGridView.Rows.Add();
                CpuKeyGridView.Rows[index].Cells[0].Value = m_nCurPage * m_nRowsPerPage + m_lstCpuKey.Count; //m_lstCpuKey记录已增加
                CpuKeyGridView.Rows[index].Cells[1].Value = newCpuKey.KeyDetail;                
                CpuKeyGridView.Rows[index].Cells[2].Value = BitConverter.ToString(newCpuKey.MasterKey).Replace("-", "");
                CpuKeyGridView.Rows[index].Cells[3].Value = BitConverter.ToString(newCpuKey.MasterTendingKey).Replace("-", "");
                CpuKeyGridView.Rows[index].Cells[4].Value = BitConverter.ToString(newCpuKey.InternalAuthKey).Replace("-", "");
                SaveLstDataToDb();
                //存数据库后重新获取配置的KeyId
                m_nValidKeyId = 1;
                GetCpuKeyValid(ref m_nValidKeyId);
                int nAddId = m_lstCpuKey[index].nKeyId;
                if (nAddId == m_nValidKeyId)
                    CpuKeyGridView.Rows[index].Cells[5].Value = "使用";
                else
                    CpuKeyGridView.Rows[index].Cells[5].Value = "未使用";
                m_nFocusKeyId = nAddId;
                FillAppGridView(m_nFocusKeyId);
                UpdateCpuKeyValid(m_nValidKeyId);

            }
            else if (m_nClickGridView == 2)
            {
                int nKeyIndex = GetFocusAppKeyIndex(m_nFocusKeyId);
                if(nKeyIndex == -1)
                    return;
                int nAppCount = m_lstCpuKey[nKeyIndex].LstAppKeyGroup.Count;
                if (nAppCount >= MAX_APP_COUNT)
                {
                    string strMsg = string.Format("卡片最大支持{0}个应用密钥,不能再增加。", MAX_APP_COUNT);
                    MessageBox.Show(strMsg);
                    return;
                }

                InsertAppKey InsertForm = new InsertAppKey();
                if (InsertForm.ShowDialog(this) != DialogResult.OK)
                    return;
                AppKeyValueGroup newAppKey = InsertForm.GetAppKeyValue();
                newAppKey.eDbFlag = DbStateFlag.eDbAdd;
                newAppKey.AppIndex = nAppCount + 1;
                m_lstCpuKey[nKeyIndex].LstAppKeyGroup.Add(newAppKey);
                int index = AppKeyGridView.Rows.Add();
                AppKeyGridView.Rows[index].Cells[0].Value = "";
                AppKeyGridView.Rows[index].Cells[1].Value = newAppKey.AppIndex.ToString(); //m_lstPsamKey记录已增加
                AppKeyGridView.Rows[index].Cells[2].Value = BitConverter.ToString(newAppKey.AppMasterKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[3].Value = BitConverter.ToString(newAppKey.AppTendingKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[4].Value = BitConverter.ToString(newAppKey.AppInternalAuthKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[5].Value = BitConverter.ToString(newAppKey.PINResetKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[6].Value = BitConverter.ToString(newAppKey.PINUnlockKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[7].Value = BitConverter.ToString(newAppKey.ConsumerMasterKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[8].Value = BitConverter.ToString(newAppKey.LoadKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[9].Value = BitConverter.ToString(newAppKey.UnLoadKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[10].Value = BitConverter.ToString(newAppKey.TacMasterKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[11].Value = BitConverter.ToString(newAppKey.UnGrayKey).Replace("-", "");
                AppKeyGridView.Rows[index].Cells[12].Value = BitConverter.ToString(newAppKey.OverdraftKey).Replace("-", "");
                SaveLstDataToDb();
            }
        }

        private void KeyManage_FormClosed(object sender, FormClosedEventArgs e)
        {
            bool bSave = false;
            foreach (CpuKeyValue value in m_lstCpuKey)
            {
                if (value.eDbFlag != DbStateFlag.eDbOK)
                {
                    bSave = true;
                    break;
                }
                foreach (AppKeyValueGroup AppKey in value.LstAppKeyGroup)
                {
                    if (AppKey.eDbFlag != DbStateFlag.eDbOK)
                    {
                        bSave = true;
                        break;
                    }
                }
                if (bSave)
                    break;
            }
            if (bSave)
            {
                DialogResult result = MessageBox.Show("是否保存更改的数据？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SaveLstDataToDb();
                }
            }

            CpuKeyGridView.Dispose();
            AppKeyGridView.Dispose();
            m_lstCpuKey.Clear();
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        private void CpuKeyGridView_Click(object sender, EventArgs e)
        {
            m_nClickGridView = 1;
        }

        private void AppKeyGridView_Click(object sender, EventArgs e)
        {
            m_nClickGridView = 2;
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
                FillDataGridView(m_nValidKeyId);
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
                FillDataGridView(m_nValidKeyId);
            }
        }
    }

}
