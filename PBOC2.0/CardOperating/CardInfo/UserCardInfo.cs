using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlServerHelper;
using IFuncPlugin;

namespace CardOperating
{
    public partial class UserCardInfo : Form
    {
        private UserCardInfoParam m_CardInfoPar = new UserCardInfoParam();
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private const Char Value_Dot = (Char)46;
        private const Char Backspace = (Char)8;
        private const Char Key_A = (Char)65;
        private const Char Key_B = (Char)66;
        private const Char Key_C = (Char)67;
        private const Char Key_D = (Char)68;
        private const Char Key_E = (Char)69;
        private const Char Key_F = (Char)70;
        private const Char Key_X = (Char)88;

        private List<ClientInfo> m_ListClientInfo = new List<ClientInfo>();//单位信息
        private List<ProvinceInfo> m_ListProvinceInfo = new List<ProvinceInfo>(); //省
        private List<CityInfo> m_ListCityInfo = new List<CityInfo>();   //地市
        private List<SuperiorInfo> m_ListSuperiorInfo = new List<SuperiorInfo>(); //上级单位（公司）
        private List<StationInfo> m_ListStationInfo = new List<StationInfo>(); //上级单位（公司）

        public UserCardInfo()
        {
            InitializeComponent();
        }


        public void SetDbInfo(SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
        }

        private int GetClientIdIndex(int nClientID)
        {
            int nSel = -1;
            int nIndex = 0;
            foreach (ClientInfo info in m_ListClientInfo)
            {
                if (info.ClientId == nClientID)
                {
                    nSel = nIndex;
                    break;
                }
                nIndex++;
            }
            return nSel;
        }

        private int GetCarCategoryIndex(string strCarType)
        {
            int nRet = 0;
            switch (strCarType)
            {
                case "不限":
                    nRet = 0;
                    break;
                case "私家车":
                    nRet = 1;
                    break;
                case "单位车":
                    nRet = 2;
                    break;
                case "出租车":
                    nRet = 3;
                    break;
                case "公交车":
                    nRet = 4;
                    break;
            }
            return nRet;
        }

        private void InitData()
        {
            if (cmbClientName.Items.Count > 0)
                cmbClientName.SelectedIndex = GetClientIdIndex(m_CardInfoPar.ClientID);
            textCompanyId.Text = m_CardInfoPar.CompanyID;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(m_CardInfoPar.CompanyID);//从数据库读出
            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            textUserCardId.Text = m_CardInfoPar.CompanyID + UserCardInfoParam.CardGroup.ToString("X2") + nCardType.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            DateFrom.Value = m_CardInfoPar.ValidCardBegin;
            DateTo.Value = m_CardInfoPar.ValidCardEnd;
            cmbUserCardType.SelectedIndex = GetCardTypeIndex(m_CardInfoPar.UserCardType);
            CustomPassword.Checked = m_CardInfoPar.DefaultPwdFlag ? false : true;
            textPassword.Enabled = m_CardInfoPar.DefaultPwdFlag ? false : true;
            textPassword.Text = m_CardInfoPar.CustomPassword;
            textUserName.Text = m_CardInfoPar.UserName;
            textPriceLevel.Text = m_CardInfoPar.PriceLevel.ToString();
            cmbIdType.SelectedIndex = GetIDTypeIndex(m_CardInfoPar.IdType);
            textUserIdentity.Text = m_CardInfoPar.UserIdentity;
            double dbRate = m_CardInfoPar.DiscountRate * 1.0 / 100.0;
            textDiscountRate.Text = dbRate.ToString("F2");
            DiscountRateExprieValid.Value = m_CardInfoPar.DiscountRateEnd;
            cmbCarCategory.SelectedIndex = GetCarCategoryIndex(m_CardInfoPar.CarType);
            if (!string.IsNullOrEmpty(m_CardInfoPar.CarNo))
                textCarNo.Text = m_CardInfoPar.CarNo;
            textTelephone.Text = m_CardInfoPar.TelePhone;
            if (!string.IsNullOrEmpty(m_CardInfoPar.SelfId))
                textSelfId.Text = m_CardInfoPar.SelfId;

            LimitCarNo.Checked = m_CardInfoPar.LimitCarNo;
            cmbLimitGasType.SelectedIndex = GetLimitGasTypeIndex(m_CardInfoPar.LimitGasType);
            cmbAreaLimit.SelectedIndex = GetAreaLimitIndex(m_CardInfoPar.LimitArea);
            FillListAreaCode(m_CardInfoPar.LimitArea);
            SetListAreaCodeChecked(m_CardInfoPar.LimitAreaCode, m_CardInfoPar.LimitArea);
            if (m_CardInfoPar.LimitGasFillCount != 0xFF)
            {
                textGasCount.Text = m_CardInfoPar.LimitGasFillCount.ToString();
            }

            if (m_CardInfoPar.LimitGasFillAmount != 0xFFFFFFFF)
            {
                double dbVal = m_CardInfoPar.LimitGasFillAmount * 1.0 / 100.0;
                textGasAmount.Text = dbVal.ToString();
            }

            textBoalNo.Text = m_CardInfoPar.BoalId;
            textBoalCount.Text = m_CardInfoPar.CylinderNum.ToString();
            textBoalVol.Text = m_CardInfoPar.CylinderVolume.ToString();
            BoalExprieValid.Value = m_CardInfoPar.BoalExprie;
            textBoalFactoryNo.Text = m_CardInfoPar.BoalFactoryID;
            textBusDistance.Text = m_CardInfoPar.BusDistance;
            textRemark.Text = m_CardInfoPar.Remark;

            this.textCompanyId.Validated += new System.EventHandler(this.textCompanyId_Validated);
            this.CustomPassword.CheckedChanged += new System.EventHandler(this.CustomPassword_CheckedChanged);
            this.cmbUserCardType.SelectedIndexChanged += new System.EventHandler(this.cmbUserCardType_SelectedIndexChanged);
            this.LimitCarNo.CheckedChanged += new System.EventHandler(this.LimitCarNo_CheckedChanged);
            this.cmbAreaLimit.SelectedIndexChanged += new System.EventHandler(this.cmbAreaLimit_SelectedIndexChanged);
        }

        private void GetClientInfo(SqlHelper ObjSql)
        {
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select ClientId,ClientName from Base_Client", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ClientInfo info = new ClientInfo();
                        info.ClientId = (int)dataReader["ClientId"];
                        info.strClientName = (string)dataReader["ClientName"];
                        m_ListClientInfo.Add(info);
                    }
                }
                dataReader.Close();
            }
        }

        private void GetProvinceInfo(SqlHelper ObjSql)
        {
            string strCode = "";
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Data_Province", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ProvinceInfo info = new ProvinceInfo();
                        strCode = (string)dataReader["ProvinceCode"];
                        info.ProvinceCode[0] = Convert.ToByte(strCode, 16);
                        info.strProvinceName = (string)dataReader["ProvinceName"];
                        m_ListProvinceInfo.Add(info);
                    }
                }
                dataReader.Close();
            }
        }

        private void GetCityInfo(SqlHelper ObjSql)
        {
            string strCode = "";
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Data_City", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        CityInfo info = new CityInfo();
                        strCode = (string)dataReader["CityCode"];
                        for (int i = 0; i < 2; i++)
                        {
                            info.CityCode[i] = Convert.ToByte(strCode.Substring(i * 2, 2), 16);
                        }
                        info.strCityName = (string)dataReader["CityName"];
                        m_ListCityInfo.Add(info);
                    }
                }
                dataReader.Close();
            }
        }

        private void GetSuperiorInfo(SqlHelper ObjSql)
        {
            string strCode = "";
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Data_Superior", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        SuperiorInfo info = new SuperiorInfo();
                        strCode = (string)dataReader["CompanyCode"];
                        for (int i = 0; i < 2; i++)
                        {
                            info.SuperiorCode[i] = Convert.ToByte(strCode.Substring(i * 2, 2), 16);
                        }
                        info.strSuperiorName = (string)dataReader["CompanyName"];
                        m_ListSuperiorInfo.Add(info);
                    }
                }
                dataReader.Close();
            }
        }

        private void GetStationInfo(SqlHelper ObjSql)
        {
            string strCode = "";
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Base_Station", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        StationInfo info = new StationInfo();
                        strCode = (string)dataReader["StationId"];
                        for (int i = 0; i < 4; i++)
                        {
                            info.StationCode[i] = Convert.ToByte(strCode.Substring(i * 2, 2), 16);
                        }
                        info.strStationName = (string)dataReader["StationName"];
                        m_ListStationInfo.Add(info);
                    }
                }
                dataReader.Close();
            }
        }

        private void ReadInfoFromDb()
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            GetClientInfo(ObjSql);
            GetProvinceInfo(ObjSql);
            GetCityInfo(ObjSql);
            GetSuperiorInfo(ObjSql);
            GetStationInfo(ObjSql);

            ObjSql.CloseConnection();
            ObjSql = null;
        }

        public UserCardInfoParam GetUserCardParam()
        {
            return m_CardInfoPar;
        }

        private int GetIDTypeIndex(UserCardInfoParam.IdentityType eIdType)
        {
            int nSel = 0;
            switch (eIdType)
            {
                case UserCardInfoParam.IdentityType.IdentityCard:
                    nSel = 0;
                    break;
                case UserCardInfoParam.IdentityType.DriverCard:
                    nSel = 1;
                    break;
                case UserCardInfoParam.IdentityType.OfficerCard:
                    nSel = 2;
                    break;
                case UserCardInfoParam.IdentityType.OtherCard:
                    nSel = 3;
                    break;
            }
            return nSel;
        }

        private int GetCardTypeIndex(UserCardInfoParam.CardType eCardType)
        {
            int nSel = 0;
            switch (eCardType)
            {
                case UserCardInfoParam.CardType.PersonalCard:
                    nSel = 0;
                    break;
                case UserCardInfoParam.CardType.ManagerCard:
                    nSel = 1;
                    break;
                case UserCardInfoParam.CardType.EmployeeCard:
                    nSel = 2;
                    break;
                case UserCardInfoParam.CardType.ServiceCard:
                    nSel = 3;
                    break;
                case UserCardInfoParam.CardType.CompanySubCard:
                    nSel = 4;
                    break;
                case UserCardInfoParam.CardType.CompanyMotherCard:
                    nSel = 5;
                    break;
            }
            return nSel;
        }

        private int GetAreaLimitIndex(byte LimitArea)
        {
            int nSel = 0;
            switch (LimitArea)
            {
                case 0xFF:
                    nSel = 0;//不限
                    break;
                case 0x01:
                    nSel = 1;//限省
                    break;
                case 0x02:
                    nSel = 2; //限地市
                    break;
                case 0x03:
                    nSel = 3;//限上级单位
                    break;
                case 0x04:
                    nSel = 4;//限站点
                    break;
            }
            return nSel;
        }

        private string GetCarCateGory(int nIndex)
        {
            string strRet = "不限";
            switch (nIndex)
            {
                case 0:
                    strRet = "不限";
                    break;
                case 1:
                    strRet = "私家车";
                    break;
                case 2:
                    strRet = "单位车";
                    break;
                case 3:
                    strRet = "出租车";
                    break;
                case 4:
                    strRet = "公交车";
                    break;
            }
            return strRet;
        }

        private void SaveClose_Click(object sender, EventArgs e)
        {
            //保存数据
            if (cmbClientName.SelectedIndex >= 0 && cmbClientName.SelectedIndex < m_ListClientInfo.Count)
                m_CardInfoPar.ClientID = m_ListClientInfo[cmbClientName.SelectedIndex].ClientId;
            m_CardInfoPar.SetCardId(textCompanyId.Text);
            if (DateFrom.Value < DateTo.Value)
            {
                m_CardInfoPar.ValidCardBegin = DateFrom.Value;
                m_CardInfoPar.ValidCardEnd = DateTo.Value;
            }
            m_CardInfoPar.DefaultPwdFlag = CustomPassword.Checked ? false : true;

            if (!m_CardInfoPar.DefaultPwdFlag && string.IsNullOrEmpty(textPassword.Text))
            {
                m_CardInfoPar.CustomPassword = textPassword.Text;
            }
            else
            {
                m_CardInfoPar.CustomPassword = "999999";
            }

            m_CardInfoPar.UserName = textUserName.Text;

            double dbRate = 0;
            double.TryParse(textDiscountRate.Text, out dbRate);
            if (dbRate >= 1 && dbRate <= 99)
            {
                m_CardInfoPar.setDiscountRate(dbRate, DiscountRateExprieValid.Value);
            }

            if (string.IsNullOrEmpty(textPriceLevel.Text))
            {
                m_CardInfoPar.PriceLevel = Convert.ToByte(textPriceLevel.Text, 10);
            }

            m_CardInfoPar.IdType = (UserCardInfoParam.IdentityType)(cmbIdType.SelectedIndex + 1);
            m_CardInfoPar.UserIdentity = textUserIdentity.Text;

            m_CardInfoPar.CarType = GetCarCateGory(cmbCarCategory.SelectedIndex);
            m_CardInfoPar.CarNo = textCarNo.Text;
            m_CardInfoPar.TelePhone = textTelephone.Text;
            m_CardInfoPar.SelfId = textCarNo.Text;

            m_CardInfoPar.LimitCarNo = LimitCarNo.Checked;
            m_CardInfoPar.LimitGasType = GetLimitGasType(cmbLimitGasType.SelectedIndex);

            byte AreaLimit = GetAreaLimit(cmbAreaLimit.SelectedIndex);
            string strLimitAreaCode = GetListAreaCode(AreaLimit);
            m_CardInfoPar.setLimitArea(AreaLimit, strLimitAreaCode);

            byte LimitGasCount = 0;
            byte.TryParse(textGasCount.Text, out LimitGasCount);
            m_CardInfoPar.LimitGasFillCount = LimitGasCount;

            double dbAmount = 0;
            double.TryParse(textGasAmount.Text, out dbAmount);
            if (dbAmount > 0 && dbAmount < 1000000.0)
            {
                m_CardInfoPar.LimitGasFillAmount = (uint)(dbAmount * 100.0);
            }
            else
            {
                m_CardInfoPar.LimitGasFillAmount = 0xFFFFFFFF;
            }

            m_CardInfoPar.BoalId = textBoalNo.Text;
            m_CardInfoPar.CylinderNum = Convert.ToInt32(textBoalCount.Text);
            m_CardInfoPar.CylinderVolume = Convert.ToUInt16(textBoalVol.Text);
            m_CardInfoPar.BoalExprie = BoalExprieValid.Value;
            m_CardInfoPar.BoalFactoryID = textBoalFactoryNo.Text;
            m_CardInfoPar.BusDistance = textBusDistance.Text;
            m_CardInfoPar.Remark = textRemark.Text;
        }

        private byte GetAreaLimit(int nSelectIndex)
        {
            byte byteAreaLimit = 0xFF;
            switch (nSelectIndex)
            {
                case 0:
                    byteAreaLimit = 0xFF;
                    break;
                case 1:
                    byteAreaLimit = 0x01;//限省
                    break;
                case 2:
                    byteAreaLimit = 0x02; //限地市
                    break;
                case 3:
                    byteAreaLimit = 0x03; //限上级单位
                    break;
                case 4:
                    byteAreaLimit = 0x04;//限站点
                    break;
            }
            return byteAreaLimit;
        }

        private UserCardInfoParam.CardType GetCardType(int nSelectIndex)
        {
            UserCardInfoParam.CardType eType = UserCardInfoParam.CardType.PersonalCard;

            switch (nSelectIndex)
            {
                case 0:
                    eType = UserCardInfoParam.CardType.PersonalCard;
                    break;
                case 1:
                    eType = UserCardInfoParam.CardType.ManagerCard;
                    break;
                case 2:
                    eType = UserCardInfoParam.CardType.EmployeeCard;
                    break;
                case 3:
                    eType = UserCardInfoParam.CardType.ServiceCard;
                    break;
                case 4:
                    eType = UserCardInfoParam.CardType.CompanySubCard;
                    break;
                case 5:
                    eType = UserCardInfoParam.CardType.CompanyMotherCard;
                    break;
            }
            return eType;
        }

        private bool IsHexKey(Char cVal)
        {
            bool bRet = false;
            switch (cVal)
            {
                case Key_A:
                case Key_B:
                case Key_C:
                case Key_D:
                case Key_E:
                case Key_F:
                    bRet = true;
                    break;
                default:
                    break;
            }
            return bRet;
        }

        private void textCompanyId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textUserCardId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textPriceLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textFixDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace && !IsHexKey(e.KeyChar))
                e.Handled = true;//不接受非数字值
        }

        private void textLimitAreaCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值            
        }

        private void textGasCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值   
        }

        private void textGasAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace && e.KeyChar != Value_Dot)
                e.Handled = true;//不接受非数字值   
        }

        private void CustomPassword_CheckedChanged(object sender, EventArgs e)
        {
            textPassword.Text = m_CardInfoPar.CustomPassword;
            if (CustomPassword.Checked)
                textPassword.Enabled = true;
            else
                textPassword.Enabled = false;
        }

        private void textDiscountRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace && e.KeyChar != Value_Dot)
                e.Handled = true;//不接受非数字值
        }

        private void textGasType_KeyPress(object sender, KeyPressEventArgs e)
        {
            //只接受Hex数字
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace && !IsHexKey(e.KeyChar))
                e.Handled = true;
        }

        private void textUserIdentity_KeyPress(object sender, KeyPressEventArgs e)
        {
            //身份证号只接受数字值和字母X
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace && e.KeyChar != Key_X)
                e.Handled = true;
        }

        private void cmbAreaLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //修改地域限制代码内容
            byte byteLimit = GetAreaLimit(cmbAreaLimit.SelectedIndex);
            FillListAreaCode(byteLimit);
        }

        private void cmbUserCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_CardInfoPar.UserCardType = GetCardType(cmbUserCardType.SelectedIndex);
            if (m_CardInfoPar.UserCardType != UserCardInfoParam.CardType.PersonalCard && m_CardInfoPar.UserCardType != UserCardInfoParam.CardType.CompanySubCard)
            {
                LimitCarNo.Checked = false;
                LimitCarNo.Enabled = false;
                textCarNo.Text = "";
                textCarNo.Enabled = false;
                textSelfId.Text = "";
                textSelfId.Enabled = false;
            }
            else
            {
                LimitCarNo.Enabled = true;
                textCarNo.Enabled = true;
                textSelfId.Enabled = true;
            }
            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            textUserCardId.Text = m_CardInfoPar.CompanyID + UserCardInfoParam.CardGroup.ToString("X2") + nCardType.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
        }

        private void LimitCarNo_CheckedChanged(object sender, EventArgs e)
        {
            if (LimitCarNo.Checked && string.IsNullOrEmpty(textCarNo.Text))
            {
                MessageBox.Show("请输入车牌号");
                LimitCarNo.Checked = false;
            }
        }

        private string GetCardOrderNoFromDb(string companyId)
        {
            int nOrderNo = 1;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return nOrderNo.ToString().PadLeft(6, '0');
            }

            string CardNoMin = companyId + "020100000000";//按公司代码计算卡流水号
            string CardNoMax = companyId + "020900999999"; //最大卡号
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam("CardNoMin", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMin);
            sqlparams[1] = ObjSql.MakeParam("CardNoMax", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMax);

            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select count(CardNum) as OrderNo from Base_Card where CardNum > @CardNoMin and CardNum < @CardNoMax", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    nOrderNo = (int)dataReader["OrderNo"] + 1;
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return nOrderNo.ToString().PadLeft(6, '0');
        }

        private void textCompanyId_Validated(object sender, EventArgs e)
        {
            m_CardInfoPar.CompanyID = textCompanyId.Text.PadLeft(4, '0');
            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            m_CardInfoPar.CardOrderNo = GetCardOrderNoFromDb(m_CardInfoPar.CompanyID);//从数据库读出
            textUserCardId.Text = m_CardInfoPar.CompanyID + UserCardInfoParam.CardGroup.ToString("X2") + nCardType.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
        }

        private void FillListAreaCode(byte byteLimitArea)
        {
            listLimitArea.Items.Clear();
            listLimitArea.Enabled = true;
            switch (byteLimitArea)
            {
                case 0xFF:
                    listLimitArea.Enabled = false;
                    break;
                case 0x01://限省
                    foreach (ProvinceInfo info in m_ListProvinceInfo)
                    {
                        listLimitArea.Items.Add(info.strProvinceName);
                    }
                    break;
                case 0x02://限地市
                    foreach (CityInfo info in m_ListCityInfo)
                    {
                        listLimitArea.Items.Add(info.strCityName);
                    }
                    break;
                case 0x03://限上级单位
                    foreach (SuperiorInfo info in m_ListSuperiorInfo)
                    {
                        listLimitArea.Items.Add(info.strSuperiorName);
                    }
                    break;
                case 0x04://限站点                    
                    foreach (StationInfo info in m_ListStationInfo)
                    {
                        listLimitArea.Items.Add(info.strStationName);
                    }
                    break;

            }
        }

        private string GetListAreaCode(byte byteAreaLimit)
        {
            string strLimitAreaCode = "";
            int nCount = listLimitArea.CheckedItems.Count;
            //检查listLimitArea中的选中项
            if (nCount > 0)
            {
                switch (byteAreaLimit)
                {
                    case 0x01://限省                           
                        foreach (ProvinceInfo info in m_ListProvinceInfo)
                        {
                            if (listLimitArea.CheckedItems.Contains(info.strProvinceName))
                            {
                                strLimitAreaCode = BitConverter.ToString(info.ProvinceCode);
                                break;
                            }
                        }
                        break;
                    case 0x02://限地市
                        foreach (CityInfo info in m_ListCityInfo)
                        {
                            if (listLimitArea.CheckedItems.Contains(info.strCityName))
                            {
                                strLimitAreaCode = BitConverter.ToString(info.CityCode).Replace("-", "");
                                break;
                            }
                        }
                        break;
                    case 0x03://限上级单位（公司）
                        foreach (SuperiorInfo info in m_ListSuperiorInfo)
                        {
                            if (listLimitArea.CheckedItems.Contains(info.strSuperiorName))
                            {
                                strLimitAreaCode = BitConverter.ToString(info.SuperiorCode).Replace("-", "");
                                break;
                            }
                        }
                        break;
                    case 0x04://限站（最多10个）
                        {
                            int nLimitCount = 0;
                            foreach (StationInfo info in m_ListStationInfo)
                            {
                                if (nLimitCount >= 10)
                                    break;
                                if (listLimitArea.CheckedItems.Contains(info.strStationName))
                                {
                                    strLimitAreaCode += BitConverter.ToString(info.StationCode).Replace("-", "");
                                    nLimitCount++;
                                }
                            }
                        }
                        break;
                }
            }
            return strLimitAreaCode;
        }

        private void SetListAreaCodeChecked(string strLimitAreaCode, byte byteLimit)
        {
            if (byteLimit == 0xFF || strLimitAreaCode.Length % 2 != 0)
                return;
            byte[] AreaCode = APDUBase.StringToBCD(strLimitAreaCode);
            if (AreaCode == null)
                return;
            switch (byteLimit)
            {
                case 0x01:
                    {
                        int i = 0;
                        foreach (ProvinceInfo info in m_ListProvinceInfo)
                        {
                            if (info.ProvinceCode[0] == AreaCode[0])
                            {
                                listLimitArea.SetItemChecked(i, true);
                                break;
                            }
                            i++;
                        }
                    }
                    break;
                case 0x02:
                    {
                        int i = 0;
                        foreach (CityInfo info in m_ListCityInfo)
                        {
                            if ((info.CityCode[0] == AreaCode[0]) && (info.CityCode[1] == AreaCode[1]))
                            {
                                listLimitArea.SetItemChecked(i, true);
                                break;
                            }
                            i++;
                        }
                    }
                    break;
                case 0x03:
                    {
                        int i = 0;
                        foreach (SuperiorInfo info in m_ListSuperiorInfo)
                        {
                            if ((info.SuperiorCode[0] == AreaCode[0]) && (info.SuperiorCode[1] == AreaCode[1]))
                            {
                                listLimitArea.SetItemChecked(i, true);
                                break;
                            }
                            i++;
                        }
                    }
                    break;
                case 0x04:
                    {
                        byte[] StationCode = new byte[4];
                        for (int nLimitIndex = 0; nLimitIndex < AreaCode.Length; nLimitIndex += 4)
                        {
                            int i = 0;
                            foreach (StationInfo info in m_ListStationInfo)
                            {
                                Buffer.BlockCopy(AreaCode, nLimitIndex, StationCode, 0, 4);
                                if (APDUBase.ByteDataEquals(info.StationCode, StationCode))
                                {
                                    listLimitArea.SetItemChecked(i, true);
                                    break;
                                }
                                i++;
                            }
                        }
                    }
                    break;
            }
        }

        private void UserCardInfo_Load(object sender, EventArgs e)
        {
            ReadInfoFromDb();

            cmbClientName.Items.Clear();
            foreach (ClientInfo info in m_ListClientInfo)
            {
                cmbClientName.Items.Add(info.strClientName);
            }

            InitData();
        }

        private int GetLimitGasTypeIndex(ushort LimitGasType)
        {
            int nSelectedIndex = 0;
            switch (LimitGasType)
            {
                case 0xFFFF:
                    nSelectedIndex = 0;
                    break;
                case 0x0001:
                    nSelectedIndex = 1;
                    break;
                case 0x0002:
                    nSelectedIndex = 2;
                    break;
            }
            return nSelectedIndex;
        }

        private ushort GetLimitGasType(int nIndex)
        {
            ushort LimitGasType = 0xFFFF;
            switch (nIndex)
            {
                case 0:
                    LimitGasType = 0xFFFF;
                    break;
                case 1:
                    LimitGasType = 0x0001;
                    break;
                case 2:
                    LimitGasType = 0x0002;
                    break;
            }
            return LimitGasType;
        }

    }
}