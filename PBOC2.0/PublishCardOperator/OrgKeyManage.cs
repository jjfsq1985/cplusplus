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
    public partial class OrgKeyManage : Form, IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();        
        private List<OrgKeyValue> m_lstOrgKey = new List<OrgKeyValue>();
        private bool m_bEditData = false;
        private int m_nEntered = -1; //相同行列的DataGridView CellEnter重复调用问题
        private enum OrgKeyItem
        {
            eKeyCategory, //种类
            eKeyState   //状态
        }

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nKeyManageAuthority = 0;


        public OrgKeyManage()
        {
            InitializeComponent();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eOrgKeyManage;
        }

        public string PluginName()
        {
            return "OrgKeyManage";
        }

        public Guid PluginGuid()
        {
            return new Guid("439DF630-0D7E-4cb8-B633-24CBCFB31499");
        }

        public string PluginMenu()
        {
            return "初始密钥管理";
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
                btnAddOrgKey.Enabled = false;
                btnDelOrgKey.Enabled = false;
                btnModifyOrgKey.Enabled = false;
                btnSaveEdit.Enabled = false;
            }
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            m_nKeyManageAuthority = nAuthority;
        }

        private int GetOrgKeyTypeIndex(string strText)
        {
            if (strText == "用户卡密钥")
                return 0;
            else if (strText == "SAM卡密钥")
                return 1;
            else if (strText == "公共初始密钥")
                return 2;
            return -1;
        }

        private string GetOrgKeyType(int nType)
        {
            string strType = "";
            switch(nType)
            {
                case 0:
                    strType = "用户卡密钥";
                    break;
                case 1:
                    strType = "SAM卡密钥";
                    break;
                case 2:
                    strType = "公共初始密钥";
                    break;
            }
            return strType;
        }

        private void FillDataGridView(int nKeyId, int nPsamKeyId)
        {
            OrgKeyGridView.Rows.Clear();
            SqlDataReader dataReader = null;            
            m_ObjSql.ExecuteCommand("select * from Key_OrgRoot", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    int nCount = 0;
                    while (dataReader.Read())
                    {
                        int index = OrgKeyGridView.Rows.Add();
                        int nId = (int)dataReader["KeyId"];
                        string strDetail = (string)dataReader["InfoRemark"];
                        string strKey = (string)dataReader["OrgKey"];
                        int nType = (int)dataReader["KeyType"];

                        OrgKeyValue keyval = new OrgKeyValue();
                        keyval.eDbFlag = DbStateFlag.eDbOK;
                        keyval.nKeyId = nId;
                        keyval.bValid = false;
                        keyval.KeyDetail = strDetail;
                        byte[] byteKey = PublicFunc.StringToBCD(strKey);                        
                        if(byteKey.Length == 16)
                            Buffer.BlockCopy(byteKey, 0, keyval.OrgKey, 0, 16);
                        keyval.nKeyType = nType;
                        OrgKeyGridView.Rows[index].Cells[0].Value = nCount + 1;
                        OrgKeyGridView.Rows[index].Cells[1].Value = strDetail;
                        OrgKeyGridView.Rows[index].Cells[2].Value = strKey;
                        OrgKeyGridView.Rows[index].Cells[3].Value = GetOrgKeyType(nType);
                        if ((nId == nKeyId) || (nId == nPsamKeyId))
                        {
                            keyval.bValid = true;                            
                            if (nKeyId == nPsamKeyId && nType != 2)
                                OrgKeyGridView.Rows[index].Cells[3].Value += "！不能共用";
                            OrgKeyGridView.Rows[index].Cells[4].Value = "使用";
                        }
                        else
                        {
                            OrgKeyGridView.Rows[index].Cells[4].Value = "未使用";                        
                        }
                        m_lstOrgKey.Add(keyval);
                        nCount++;
                    }
                }
                dataReader.Close();
            }
        }

        /// <summary>
        /// 获取初始当前使用的初始密钥
        /// </summary>
        private void GetOrgKeyValid(ref int nkeyId, ref int nPsamKeyId)
        {
            SqlDataReader dataReader = null;            
            m_ObjSql.ExecuteCommand("select OrgKeyId,OrgPsamKeyId from Config_SysParams", out dataReader);
            if (dataReader != null)
            {
                if (!dataReader.HasRows)
                    dataReader.Close();
                else
                {
                    if (dataReader.Read())
                    {
                        nkeyId = (int)dataReader["OrgKeyId"];
                        nPsamKeyId = (int)dataReader["OrgPsamKeyId"];
                    }
                    dataReader.Close();
                }                
            }

        }

        private void OrgKeyManage_Load(object sender, EventArgs e)
        {
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
                return;
            }
            OrgKeyGridView.Columns.Clear();
            OrgKeyGridView.Columns.Add("KeyId", "序号");
            OrgKeyGridView.Columns.Add("KeyDetail", "密钥描述");
            OrgKeyGridView.Columns.Add("OrgKey", "初始密钥");
            OrgKeyGridView.Columns.Add("KeyType", "密钥种类");
            OrgKeyGridView.Columns.Add("KeyValid", "状态");
            for (int i = 0; i < OrgKeyGridView.Columns.Count; i++)
                OrgKeyGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            int nKeyId = 1;
            int nPsamKeyId = 1;
            GetOrgKeyValid(ref nKeyId,ref nPsamKeyId);
            FillDataGridView(nKeyId,nPsamKeyId);

            btnModifyOrgKey.Enabled = m_bEditData ? false : true;
            btnSaveEdit.Enabled = m_bEditData ? true : false;
        }

        private void OrgKeyManage_FormClosed(object sender, FormClosedEventArgs e)
        {
            bool bSave = false;
            foreach (OrgKeyValue value in m_lstOrgKey)
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

            OrgKeyGridView.Dispose();
            m_lstOrgKey.Clear();
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnModifyOrgKey_Click(object sender, EventArgs e)
        {
            m_bEditData = true;
            btnModifyOrgKey.Enabled = m_bEditData ? false : true;
            btnSaveEdit.Enabled = m_bEditData ? true : false;
            int nItem = OrgKeyGridView.CurrentCell.ColumnIndex;
            if (nItem >= 1 && nItem <= 3)
            {
                //开始编辑
                int nIndex = OrgKeyGridView.CurrentCell.RowIndex;
                GetModifyView(nIndex, nItem, m_lstOrgKey[nIndex]);
                OrgKeyGridView.BeginEdit(true);
            }
        }

        private void SetModifyView(int nContentIndex, int nItem, OrgKeyValue value)
        {
            try
            {
                string strContent = (string)OrgKeyGridView.Rows[nContentIndex].Cells[nItem].EditedFormattedValue;
                switch (nItem)
                {
                    case 1:
                        {                            
                            if (value.KeyDetail != strContent)
                            {
                                value.KeyDetail = strContent;
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 2:
                        {                            
                            byte[] byteKey = PublicFunc.StringToBCD(strContent);
                            if (BitConverter.ToString(value.OrgKey).Replace("-", "") != strContent)
                            {
                                if (byteKey.Length == 16)
                                    Buffer.BlockCopy(byteKey, 0, value.OrgKey, 0, 16);
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 3:
                        {
                            int nKeyType = GetOrgKeyTypeIndex(strContent);
                            if (nKeyType != value.nKeyType)
                            {
                                value.nKeyType = nKeyType;
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                    case 4:
                        {
                            bool bValid = strContent == "使用" ? true : false;
                            if (value.bValid != bValid)
                            {
                                value.bValid = bValid;
                                value.eDbFlag = DbStateFlag.eDbDirty;
                            }
                        }
                        break;
                }
                m_lstOrgKey[nContentIndex] = value;
            }
            catch
            {

            }
        }

        private void FillCombox(DataGridViewComboBoxCell ComboBoxCell, OrgKeyItem eItem)
        {
            ComboBoxCell.Items.Clear();
            if (eItem == OrgKeyItem.eKeyCategory)
            {
                ComboBoxCell.Items.Add("用户卡密钥");
                ComboBoxCell.Items.Add("SAM卡密钥");
                ComboBoxCell.Items.Add("公共初始密钥");
            }
            else if (eItem == OrgKeyItem.eKeyState)
            {
                ComboBoxCell.Items.Add("未使用");
                ComboBoxCell.Items.Add("使用");
            }
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

        private void GetModifyView(int nContentIndex, int nItem,OrgKeyValue value)
        {
            m_nEntered = -1;
            try
            {
                switch (nItem)
                {
                    case 1:
                        if (value.KeyDetail != (string)OrgKeyGridView.Rows[nContentIndex].Cells[nItem].Value)
                            OrgKeyGridView.Rows[nContentIndex].Cells[nItem].Value = value.KeyDetail;
                        break;
                    case 2:
                        {
                            string strValue = BitConverter.ToString(value.OrgKey).Replace("-", "");
                            if (strValue != (string)OrgKeyGridView.Rows[nContentIndex].Cells[nItem].Value)
                                OrgKeyGridView.Rows[nContentIndex].Cells[nItem].Value = strValue;
                        }
                        break;
                    case 3:
                        {
                            DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                            FillCombox(comboCell, OrgKeyItem.eKeyCategory);
                            comboCell.Value = GetOrgKeyType(value.nKeyType);
                            comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                            if (nContentIndex == nItem)
                                SetCell(OrgKeyGridView,nContentIndex,nItem,comboCell);                                
                            else
                                OrgKeyGridView.Rows[nContentIndex].Cells[nItem] = comboCell;
                        }
                        break;
                    case 4:
                        {
                            DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                            FillCombox(comboCell, OrgKeyItem.eKeyState);
                            comboCell.Value = value.bValid ? "使用" : "未使用";
                            comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                            if (nContentIndex == nItem)
                                SetCell(OrgKeyGridView, nContentIndex, nItem, comboCell);
                            else
                                OrgKeyGridView.Rows[nContentIndex].Cells[nItem] = comboCell;
                        }
                        break;
                }
            }
            catch
            {

            }
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            int nItem = OrgKeyGridView.CurrentCell.ColumnIndex;
            if (nItem >= 1 && nItem <= 4)
            {
                //结束编辑
                int nIndex = OrgKeyGridView.CurrentCell.RowIndex;
                SetModifyView(nIndex, nItem, m_lstOrgKey[nIndex]);
                OrgKeyGridView.EndEdit();
            }
            m_bEditData = false;
            btnModifyOrgKey.Enabled = m_bEditData ? false : true;
            btnSaveEdit.Enabled = m_bEditData ? true : false;
            //存数据库
            SaveLstDataToDb();
        }

        private void OrgKeyGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!m_bEditData || e.ColumnIndex < 1 || e.ColumnIndex > 4)
                return;
            //DataGridView 行列相同时修改Cell（DataGridViewTextBoxCell-〉DataGridViewComboBoxCell）会引起SetCurrentCellAddressCore可重入调用
            if (m_nEntered == e.ColumnIndex && e.ColumnIndex == e.RowIndex)
            {
                OrgKeyGridView.BeginEdit(true);
                return;
            }
            GetModifyView(e.RowIndex, e.ColumnIndex, m_lstOrgKey[e.RowIndex]);
            OrgKeyGridView.BeginEdit(true);
        }

        private void OrgKeyGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!m_bEditData || e.ColumnIndex < 1 || e.ColumnIndex > 4)
                return;
            SetModifyView(e.RowIndex, e.ColumnIndex, m_lstOrgKey[e.RowIndex]);
            OrgKeyGridView.EndEdit();
        }

        private void btnAddOrgKey_Click(object sender, EventArgs e)
        {
            AddOrgKey AddForm = new AddOrgKey();
            if (AddForm.ShowDialog(this) != DialogResult.OK)
                return;
            OrgKeyValue newOrgKey = AddForm.GetOrgKeyVal();     
            newOrgKey.eDbFlag = DbStateFlag.eDbAdd;            
            m_lstOrgKey.Add(newOrgKey);
            int index = OrgKeyGridView.Rows.Add();
            OrgKeyGridView.Rows[index].Cells[0].Value = m_lstOrgKey.Count; //m_lstOrgKey记录已增加
            OrgKeyGridView.Rows[index].Cells[1].Value = newOrgKey.KeyDetail;
            OrgKeyGridView.Rows[index].Cells[2].Value = BitConverter.ToString(newOrgKey.OrgKey).Replace("-","");
            OrgKeyGridView.Rows[index].Cells[3].Value = GetOrgKeyType(newOrgKey.nKeyType);
            OrgKeyGridView.Rows[index].Cells[4].Value = "未使用";
            SaveLstDataToDb();
            int nKeyId = 1;
            int nPsamKeyId = 1;
            GetOrgKeyValid(ref nKeyId, ref nPsamKeyId);
            int nAddId = m_lstOrgKey[index].nKeyId;
            if ((nAddId == nKeyId) || (nAddId == nPsamKeyId))
            {
                if (nKeyId == nPsamKeyId && newOrgKey.nKeyType != 2)
                    OrgKeyGridView.Rows[index].Cells[3].Value += "！不能共用";
                OrgKeyGridView.Rows[index].Cells[4].Value = "使用";
            }
        }

        private void btnDelOrgKey_Click(object sender, EventArgs e)
        {
            int nIndex = OrgKeyGridView.CurrentCell.RowIndex;
            OrgKeyGridView.Rows.RemoveAt(nIndex);
            OrgKeyValue value = m_lstOrgKey[nIndex];
            value.eDbFlag = DbStateFlag.eDbDelete;
            m_lstOrgKey[nIndex] = value;
            SaveLstDataToDb();
        }

        private void SaveLstDataToDb()
        {
            int nCount = m_lstOrgKey.Count;
            List<OrgKeyValue> deleteLst = new List<OrgKeyValue>();
            for (int i = 0; i < nCount; i++)
            {
                OrgKeyValue value = m_lstOrgKey[i];
                if (value.eDbFlag != DbStateFlag.eDbOK)
                {
                    SqlParameter[] sqlparams = new SqlParameter[7];
                    string strBcd = BitConverter.ToString(value.OrgKey).Replace("-", "");
                    sqlparams[0] = m_ObjSql.MakeParam("KeyId", SqlDbType.Int, 4, ParameterDirection.Input, value.nKeyId);
                    sqlparams[1] = m_ObjSql.MakeParam("OrgKey", SqlDbType.Char, 32, ParameterDirection.Input, strBcd);
                    sqlparams[2] = m_ObjSql.MakeParam("KeyType", SqlDbType.Int, 4, ParameterDirection.Input, value.nKeyType);
                    sqlparams[3] = m_ObjSql.MakeParam("KeyDetail", SqlDbType.NVarChar, 50, ParameterDirection.Input, value.KeyDetail);
                    sqlparams[4] = m_ObjSql.MakeParam("KeyState", SqlDbType.Bit, 1, ParameterDirection.Input, value.bValid);
                    sqlparams[5] = m_ObjSql.MakeParam("DbState", SqlDbType.Int, 4, ParameterDirection.Input, value.eDbFlag);
                    sqlparams[6] = m_ObjSql.MakeParam("AddKeyId", SqlDbType.Int, 4, ParameterDirection.Output,null);
                    if (m_ObjSql.ExecuteProc("PROC_UpdateOrgKeyRoot", sqlparams) == 0)
                    {
                        if (value.eDbFlag == DbStateFlag.eDbDelete)
                        {
                            deleteLst.Add(value);
                        }
                        else
                        {
                            if (value.eDbFlag == DbStateFlag.eDbAdd)
                            {
                                value.nKeyId = (int)sqlparams[6].Value; ;
                            }
                            value.eDbFlag = DbStateFlag.eDbOK;
                            m_lstOrgKey[i] = value;
                        }
                    }
                }
            }//end for
            foreach (OrgKeyValue temp in deleteLst)
            {
                m_lstOrgKey.Remove(temp);
            }
            deleteLst.Clear();
        }

    }
}