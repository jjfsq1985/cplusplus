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

namespace StationManage
{
    public partial class StationManage : Form, IPlugin
    {
        private SqlHelper m_ObjSql = new SqlHelper();
        private List<StationParam> m_lstStationParam = new List<StationParam>();

        private List<ProvinceCode> m_lstProvCode = new List<ProvinceCode>();
        private List<CityCode> m_lstCityCode = new List<CityCode>();
        private List<SuperiorCode> m_lstSuperiorCode = new List<SuperiorCode>();
        private List<ClientParam> m_lstClient = new List<ClientParam>();
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nStationAuthority = 0;


        public StationManage()
        {
            InitializeComponent();
        }

        public MenuType GetMenuType()
        {
            return MenuType.eStationInfo;
        }

        public string PluginName()
        {
            return "StationManage";
        }

        public Guid PluginGuid()
        {
            return new Guid("0E306A49-C0F3-4e6e-A986-BD27251D5196");
        }

        public string PluginMenu()
        {
            return "站点信息管理";
        }

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
            if (m_nStationAuthority != GrobalVariable.StationInfo_Authority)
            {
                btnAdd.Enabled = false;
                btnDel.Enabled = false;
            }
        }

        public void SetAuthority(int nLoginUserId, int nAuthority)
        {
            m_nStationAuthority = nAuthority;
        }

        private void StationManage_Load(object sender, EventArgs e)
        {
            //显示所有站点
            if (!m_ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                m_ObjSql = null;
                return;
            }
            ReadProvFromDb();
            ReadCityFromDb();
            ReadSuperiorFromDb();
            ReadClientFromDb();

            ReadStationInfoFromDb();

            FillColumnContent();
        }

        private void ReadProvFromDb()
        {            
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select * from Data_Province", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ProvinceCode ProvVal = new ProvinceCode();
                        ProvVal.strProvinceName = (string)dataReader["ProvinceName"];
                        ProvVal.ProvinceVal = Convert.ToByte((string)dataReader["ProvinceCode"], 16);
                        m_lstProvCode.Add(ProvVal);
                    }
                }
                dataReader.Close();
            }
        }

        private void ReadCityFromDb()
        {
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select * from Data_City", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        CityCode CityVal = new CityCode();
                        CityVal.strCityName = (string)dataReader["CityName"];
                        byte[] codeBcd = ConvertBCD.StringToBCD((string)dataReader["CityCode"]);
                        Trace.Assert(codeBcd != null && codeBcd.Length == 2);
                        CityVal.CityVal[0] = codeBcd[0];
                        CityVal.CityVal[1] = codeBcd[1];
                        m_lstCityCode.Add(CityVal);
                    }
                }
                dataReader.Close();
            }
        }

        private void ReadSuperiorFromDb()
        {            
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select * from Data_Superior", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        SuperiorCode SuperiorVal = new SuperiorCode();
                        SuperiorVal.strSuperiorName = (string)dataReader["CompanyName"];
                        byte[] codeBcd = ConvertBCD.StringToBCD((string)dataReader["CompanyCode"]);
                        Trace.Assert(codeBcd != null && codeBcd.Length == 2);
                        SuperiorVal.SuperiorVal[0] = codeBcd[0];
                        SuperiorVal.SuperiorVal[1] = codeBcd[1];
                        m_lstSuperiorCode.Add(SuperiorVal);
                    }
                }
                dataReader.Close();
            }
        }

        private void ReadStationInfoFromDb()
        {
            StationView.Rows.Clear();
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select * from Base_Station", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    byte[] codeBcd = null;
                    while (dataReader.Read())
                    {
                        int index = StationView.Rows.Add();
                        StationParam StationVal = new StationParam();
                        StationVal.eDbState = DbStateFlag.eDbOK;
                        StationVal.nDataGridViewRowIndex = index;                        
                        StationVal.strStationName = (string)dataReader["StationName"];
                        codeBcd = ConvertBCD.StringToBCD((string)dataReader["StationId"]);
                        Trace.Assert(codeBcd != null && codeBcd.Length == 4);
                        Buffer.BlockCopy(codeBcd, 0, StationVal.StationId,0,4);
                        StationVal.ClientID = (int)dataReader["ClientId"];
                        
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("Prov")))
                        {
                            byte byteCode = 0;
                            byte.TryParse((string)dataReader["Prov"], System.Globalization.NumberStyles.HexNumber, null, out byteCode);
                            StationVal.ProvCode = byteCode;
                        }
                        
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("City")))
                        {
                            codeBcd = ConvertBCD.StringToBCD((string)dataReader["City"]);
                            Trace.Assert(codeBcd != null && codeBcd.Length == 2);
                            StationVal.CityCode[0] = codeBcd[0];
                            StationVal.CityCode[1] = codeBcd[1];
                        }
                        
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("SuperiorId")))
                        {
                            codeBcd = ConvertBCD.StringToBCD((string)dataReader["SuperiorId"]);
                            Trace.Assert(codeBcd != null && codeBcd.Length == 2);
                            StationVal.SuperiorCode[0] = codeBcd[0];
                            StationVal.SuperiorCode[1] = codeBcd[1];
                        }

                        StationView.Rows[index].Cells[0].Value = BitConverter.ToString(StationVal.StationId).Replace("-", "");
                        StationView.Rows[index].Cells[1].Value = StationVal.strStationName;
                        StationView.Rows[index].Cells[2].Value = GetProvName(StationVal.ProvCode); //省名称
                        StationView.Rows[index].Cells[3].Value = GetCityName(StationVal.CityCode); //地市名称0
                        StationView.Rows[index].Cells[4].Value = GetCompanyName(StationVal.SuperiorCode); //公司名称
                        StationView.Rows[index].Cells[5].Value = GetClientName(StationVal.ClientID); //所属单位

                        m_lstStationParam.Add(StationVal);
                    }
                }
                dataReader.Close();
            }
        }

        private void StationManage_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭窗口时判断是否需要保存
            bool bSave = false;
            foreach (StationParam value in m_lstStationParam)
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
                    SaveStationInfoDataToDb();
                }
            }
            StationView.Dispose();
            m_lstStationParam.Clear();
            if (m_ObjSql != null)
            {
                m_ObjSql.CloseConnection();
                m_ObjSql = null;
            }
        }

        private void StationView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int nColumnIndex = StationView.CurrentCell.ColumnIndex;
            int nRowIndex = StationView.CurrentCell.RowIndex;
            if (nColumnIndex < 2 || nColumnIndex > 5)
                return;
            DataGridViewComboBoxEditingControl ComboBoxCtrl = (DataGridViewComboBoxEditingControl)e.Control;
            ComboBoxCtrl.SelectedText = (string)StationView.Rows[nRowIndex].Cells[nColumnIndex].Value;
        }

        private string GetProvName(byte codeProv)
        {
            string strProv = "";
            foreach (ProvinceCode val in m_lstProvCode)
            {
                if (val.ProvinceVal == codeProv)
                {
                    strProv = val.strProvinceName;
                    break;
                }
            }
            return strProv;
        }
        private byte GetProvCode(string strName)
        {
            byte byteProv = 0x00;
            foreach (ProvinceCode val in m_lstProvCode)
            {
                if (val.strProvinceName == strName)
                {
                    byteProv = val.ProvinceVal;
                    break;
                }
            }
            return byteProv;
        }

        private string GetCityName(byte[] codeCity)
        {
            string strCity = "";
            foreach (CityCode val in m_lstCityCode)
            {
                if (val.CityVal[0] == codeCity[0] && val.CityVal[1] == codeCity[1])
                {
                    strCity = val.strCityName;
                    break;
                }
            }
            return strCity;
        }

        private byte[] GetCityCode(string strName)
        {
            byte[] byteCity = new byte[2];
            foreach (CityCode val in m_lstCityCode)
            {
                if (val.strCityName == strName)
                {
                    byteCity[0] = val.CityVal[0];
                    byteCity[1] = val.CityVal[1];
                    break;
                }
            }
            return byteCity;
        }

        private string GetCompanyName(byte[] codeSuperior)
        {
            string strSuperior = "";
            foreach (SuperiorCode val in m_lstSuperiorCode)
            {
                if (val.SuperiorVal[0] == codeSuperior[0] && val.SuperiorVal[1] == codeSuperior[1])
                {
                    strSuperior = val.strSuperiorName;
                    break;
                }
            }
            return strSuperior;
        }

        private byte[] GetCompanyCode(string strName)
        {
            byte[] byteSuperior = new byte[2];
            foreach (SuperiorCode val in m_lstSuperiorCode)
            {
                if (val.strSuperiorName == strName)
                {
                    byteSuperior[0] = val.SuperiorVal[0];
                    byteSuperior[1] = val.SuperiorVal[1];
                    break;
                }
            }
            return byteSuperior;
        }

        private string GetClientName(int nClientID)
        {
            string strClient = "未定义";
            foreach (ClientParam val in m_lstClient)
            {
                if (val.ClientId == nClientID)
                {
                    strClient = val.ClientName;
                    break;
                }
            }
            return strClient;
        }

        private int GetClientID(string strName)
        {
            int nClientID = 0;
            foreach (ClientParam val in m_lstClient)
            {
                if (val.ClientName == strName)
                {
                    nClientID = val.ClientId;
                    break;
                }
            }
            return nClientID;
        }

        private void ReadClientFromDb()
        {
            SqlDataReader dataReader = null;
            m_ObjSql.ExecuteCommand("select ClientId, ClientName from Base_Client", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ClientParam ClientVal = new ClientParam();
                        ClientVal.ClientId = (int)dataReader["ClientId"];
                        ClientVal.ClientName = (string)dataReader["ClientName"];
                        m_lstClient.Add(ClientVal);
                    }
                }
                dataReader.Close();
            }
        }

        private void FillColumnContent()
        {
            Province.Items.Clear();
            foreach (ProvinceCode val in m_lstProvCode)
            {
                Province.Items.Add(val.strProvinceName);
            }

            City.Items.Clear();
            foreach (CityCode val in m_lstCityCode)
            {
                City.Items.Add(val.strCityName);
            }

            Superior.Items.Clear();
            foreach (SuperiorCode val in m_lstSuperiorCode)
            {
                Superior.Items.Add(val.strSuperiorName);
            }

            ClientName.Items.Clear();
            foreach (ClientParam val in m_lstClient)
            {
                ClientName.Items.Add(val.ClientName);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (StationView.Rows.Count > m_lstStationParam.Count || !IsStationListCompleted(m_lstStationParam))
            {
                MessageBox.Show("请先将空行填完整");
                return;
            }
            int nIndex = StationView.Rows.Add();
            StationView.Rows[nIndex].Cells[0].Selected = true;
        }

        public bool ByteDataEquals(byte[] byteL, byte[] byteR)
        {
            if (byteL.Length != byteR.Length)
                return false;
            for (int i = 0; i < byteL.Length; i++)
            {
                if (byteL[i] != byteR[i])
                    return false;
            }
            return true;
        }

        public bool IsStationListCompleted(List<StationParam> list)
        {
            byte[] initByte = new byte[4];
            foreach (StationParam value in list)
            {
                if ((value.strStationName == "") || (ByteDataEquals(value.StationId, initByte))
                    || (value.ClientID == 0) || (value.ProvCode == 0x00)
                    || (value.CityCode[0] == 0x00 && value.CityCode[1] == 0x00)
                    || (value.SuperiorCode[0] == 0x00 && value.SuperiorCode[1] == 0x00))
                {
                    return false;
                }
            }
            return true;
        }

        private int GetIndexOfList(int nRowIndex)
        {
            int nListIndex = -1;
            int nCount = 0;
            foreach (StationParam value in m_lstStationParam)
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

        private void btnDel_Click(object sender, EventArgs e)
        {
            int nRowIndex = StationView.CurrentCell.RowIndex;
            int nListIndex = GetIndexOfList(nRowIndex);
            if (nListIndex != -1)
            {
                //未保存过的直接删除
                if (m_lstStationParam[nListIndex].eDbState == DbStateFlag.eDbAdd)
                    m_lstStationParam.RemoveAt(nListIndex);
                else
                    m_lstStationParam[nListIndex].eDbState = DbStateFlag.eDbDelete;
            }
            StationView.Rows.RemoveAt(nRowIndex);
            if (nRowIndex > 0)
                StationView.Rows[nRowIndex - 1].Cells[0].Selected = true;
        }

        private void StationView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            string strInput = (string)StationView.CurrentCell.FormattedValue;
            if (strInput == "")
                return;
            int nRowIndex = StationView.CurrentCell.RowIndex;
            int nItem = StationView.CurrentCell.ColumnIndex;
            int nListIndex = GetIndexOfList(nRowIndex);
            switch (nItem)
            {
                case 0:
                    {
                        Regex reg = new Regex(@"^[0-9]{8}$");
                        if (!reg.Match(strInput).Success)
                        {
                            StationView.CurrentCell.Value = "";
                            MessageBox.Show("站点编号只能是8位数字");
                            return;
                        }
                        byte[] codebyte = ConvertBCD.StringToBCD(strInput);
                        if (codebyte == null || codebyte.Length != 4)
                        {
                            StationView.CurrentCell.Value = "";
                            MessageBox.Show("站点编号无效");
                            return;
                        }
                        if (nListIndex == -1)
                        {
                            //新增
                            bool bAdd = true;
                            foreach (StationParam value in m_lstStationParam)
                            {
                                if (ByteDataEquals(value.StationId, codebyte))
                                {
                                    bAdd = false;
                                    StationView.CurrentCell.Value = "";
                                    MessageBox.Show("站点编号已存在");
                                    break;
                                }
                            }
                            if (bAdd)
                            {
                                StationParam newVal = new StationParam();
                                newVal.eDbState = DbStateFlag.eDbAdd;
                                newVal.strStationName = "";
                                Buffer.BlockCopy(codebyte, 0, newVal.StationId,0,4);                                
                                m_lstStationParam.Add(newVal);
                            }
                        }
                        else if (!ByteDataEquals(m_lstStationParam[nListIndex].StationId, codebyte))
                        {
                            Buffer.BlockCopy(codebyte, 0, m_lstStationParam[nListIndex].StationId, 0, 4);                            
                            if (m_lstStationParam[nListIndex].eDbState == DbStateFlag.eDbOK)
                                m_lstStationParam[nListIndex].eDbState = DbStateFlag.eDbDirty;
                        }
                    }
                    break;
                case 1:
                    {
                        Regex reg = new Regex(@"^[A-Za-z\u4e00-\u9fa5]+$");
                        if (!reg.Match(strInput).Success)
                        {
                            StationView.CurrentCell.Value = "";
                            MessageBox.Show("站点名称不能包含数字和特殊字符");
                            return;
                        }
                        if (nListIndex == -1)
                        {
                            //新增
                            bool bAdd = true;
                            foreach (StationParam value in m_lstStationParam)
                            {
                                if (value.strStationName == strInput)
                                {
                                    bAdd = false;
                                    StationView.CurrentCell.Value = "";
                                    MessageBox.Show("站点名称已存在");
                                    break;
                                }
                            }
                            if (bAdd)
                            {
                                StationParam newVal = new StationParam();
                                newVal.eDbState = DbStateFlag.eDbAdd;
                                newVal.strStationName = strInput;
                                newVal.CityCode[0] = 0;
                                newVal.CityCode[1] = 0;
                                m_lstStationParam.Add(newVal);
                            }
                        }
                        else if (strInput != m_lstStationParam[nListIndex].strStationName)
                        {
                            m_lstStationParam[nListIndex].strStationName = strInput;
                            if (m_lstStationParam[nListIndex].eDbState == DbStateFlag.eDbOK)
                                m_lstStationParam[nListIndex].eDbState = DbStateFlag.eDbDirty;
                        }
                    }
                    break;
                case 2:
                    if (nListIndex != -1 && strInput != GetProvName(m_lstStationParam[nListIndex].ProvCode))
                    {
                        m_lstStationParam[nListIndex].ProvCode = GetProvCode(strInput);
                        if (m_lstStationParam[nListIndex].eDbState == DbStateFlag.eDbOK)
                            m_lstStationParam[nListIndex].eDbState = DbStateFlag.eDbDirty;
                    }
                    break;
                case 3:
                    if (nListIndex != -1 && strInput != GetCityName(m_lstStationParam[nListIndex].CityCode))
                    {
                        byte[] codeByte = GetCityCode(strInput);
                        Trace.Assert(codeByte != null && codeByte.Length == 2);
                        m_lstStationParam[nListIndex].CityCode[0] = codeByte[0];
                        m_lstStationParam[nListIndex].CityCode[1] = codeByte[1];
                        if (m_lstStationParam[nListIndex].eDbState == DbStateFlag.eDbOK)
                            m_lstStationParam[nListIndex].eDbState = DbStateFlag.eDbDirty;
                    }
                    break;
                case 4:
                    if (nListIndex != -1 && strInput != GetCompanyName(m_lstStationParam[nListIndex].SuperiorCode))
                    {
                        byte[] codeByte = GetCompanyCode(strInput);
                        Trace.Assert(codeByte != null && codeByte.Length == 2);
                        m_lstStationParam[nListIndex].SuperiorCode[0] = codeByte[0];
                        m_lstStationParam[nListIndex].SuperiorCode[1] = codeByte[1];
                        if (m_lstStationParam[nListIndex].eDbState == DbStateFlag.eDbOK)
                            m_lstStationParam[nListIndex].eDbState = DbStateFlag.eDbDirty;
                    }
                    break;
                case 5:
                    if (nListIndex != -1 && strInput != GetClientName(m_lstStationParam[nListIndex].ClientID))
                    {
                        m_lstStationParam[nListIndex].ClientID = GetClientID(strInput);
                        if (m_lstStationParam[nListIndex].eDbState == DbStateFlag.eDbOK)
                            m_lstStationParam[nListIndex].eDbState = DbStateFlag.eDbDirty;
                    }
                    break;
            }
        }

        private void StationView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (m_nStationAuthority != GrobalVariable.StationInfo_Authority)
            {
                e.Cancel = true;
                return;
            }
                     
            int nColumnIndex = StationView.CurrentCell.ColumnIndex;
            int nRowIndex = StationView.CurrentCell.RowIndex;
            if (nColumnIndex >= 2 && nColumnIndex <= 5)
            {
                //站点编号和名称必须先填
                if (string.IsNullOrEmpty((string)StationView.Rows[nRowIndex].Cells[0].Value)
                || string.IsNullOrEmpty((string)StationView.Rows[nRowIndex].Cells[1].Value))
                {
                    e.Cancel = true;
                }
            }
        }

        private void SaveStationInfoDataToDb()
        {
            SqlParameter[] sqlparams = new SqlParameter[6];
            List<StationParam> deleteLst = new List<StationParam>();
            int nCount = m_lstStationParam.Count;
            for (int i = 0; i < nCount; i++)
            {
                StationParam value = m_lstStationParam[i];
                sqlparams[0] = m_ObjSql.MakeParam("StationId", SqlDbType.Char, 8, ParameterDirection.Input, BitConverter.ToString(value.StationId).Replace("-", ""));
                sqlparams[1] = m_ObjSql.MakeParam("StationName", SqlDbType.NVarChar, 50, ParameterDirection.Input, value.strStationName);
                sqlparams[2] = m_ObjSql.MakeParam("ProvCode", SqlDbType.Char, 2, ParameterDirection.Input, value.ProvCode.ToString("X2"));
                sqlparams[3] = m_ObjSql.MakeParam("CityCode", SqlDbType.Char, 4, ParameterDirection.Input, BitConverter.ToString(value.CityCode).Replace("-", ""));
                sqlparams[4] = m_ObjSql.MakeParam("SuperiorCode", SqlDbType.Char, 4, ParameterDirection.Input, BitConverter.ToString(value.SuperiorCode).Replace("-", ""));
                sqlparams[5] = m_ObjSql.MakeParam("ClientId", SqlDbType.Int, 4, ParameterDirection.Input, value.ClientID);
                if (value.eDbState == DbStateFlag.eDbAdd)
                {
                    m_ObjSql.ExecuteCommand("insert into Base_Station values(@StationId,@StationName,@ProvCode,@CityCode,@SuperiorCode,@ClientId,0)", sqlparams);
                    value.eDbState = DbStateFlag.eDbOK;
                }
                else if (value.eDbState == DbStateFlag.eDbDelete)
                {
                    m_ObjSql.ExecuteCommand("delete from Base_Station where StationId = @StationId and StationName = @StationName", sqlparams);
                    deleteLst.Add(value);
                }
                else if (value.eDbState == DbStateFlag.eDbDirty)
                {
                    m_ObjSql.ExecuteCommand("update Base_Station set StationId = @StationId, StationName = @StationName, Prov = @ProvCode, City=@CityCode, SuperiorId=@SuperiorCode, ClientId=@ClientId", sqlparams);
                    value.eDbState = DbStateFlag.eDbOK;
                }
                m_lstStationParam[i] = value;
            }

            foreach (StationParam temp in deleteLst)
            {
                m_lstStationParam.Remove(temp);
            }
            deleteLst.Clear();
        }

    }
}
