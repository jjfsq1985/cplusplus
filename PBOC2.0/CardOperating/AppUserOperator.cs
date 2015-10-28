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
using ApduParam;
using ApduCtrl;
using ApduInterface;
using CardControl;

namespace CardOperating
{
    public partial class AppUserOperator : Form , IPlugin
    {
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nAppOperatorAuthority = 0;
        private int m_nLoginUserId = 0;

        private const Char Backspace = (Char)8;
        private const Char Value_Dot = (Char)46;
        private const Char Key_X = (Char)88;

        private UserCardInfoParam m_CardInfoPar = new UserCardInfoParam();
        private IUserCardControl m_UserCardCtrl = null;
        private ISamCardControl m_SamCardCtrl = null;
        private readonly byte[] m_FixedTermialId = new byte[] { 0x14, 0x32, 0x00, 0x00, 0x00, 0x01 };  //固定的终端机设备编号

        private static byte[] m_TermialId = new byte[6];            //终端机设备编号
        private static byte[] m_TermialId_Ly = new byte[6];            //终端机设备编号
        private static byte[] m_GTAC = new byte[4];    //灰锁时的GTAC
        private static byte[] m_GTAC_Ly = new byte[4];    //灰锁时的GTAC

        private static string m_strPIN = "999999";//由用户输入
        private static string m_strPin_Ly = "999999";

        private bool m_bGray = false;   //卡已灰，不能扣款解锁
        private bool m_bGray_Ly = false;  //积分应用灰锁

        //一个公司可以有多个单位（分公司），一个单位下可以有多个站点。
        private List<ClientInfo> m_ListClientInfo = new List<ClientInfo>();//单位信息
        private List<ProvinceInfo> m_ListProvinceInfo = new List<ProvinceInfo>(); //省
        private List<CityInfo> m_ListCityInfo = new List<CityInfo>();   //地市
        private List<SuperiorInfo> m_ListSuperiorInfo = new List<SuperiorInfo>(); //上级单位（公司）
        private List<StationInfo> m_ListStationInfo = new List<StationInfo>(); //上级单位（公司）

        private ApduController m_DevControl = null;

        public AppUserOperator()
        {
            InitializeComponent();
            textPIN.Text = m_strPIN;
            textPIN_Ly.Text = m_strPin_Ly;
            cmbDevType.SelectedIndex = 0;
            cmbCardType.SelectedIndex = 0;
        }

        public MenuType GetMenuType()
        {
            return MenuType.eCardPublish;
        }

        public string PluginName()
        {
            return "AppUserOperator";
        }

        public Guid PluginGuid()
        {
            return new Guid("4F0D50FF-AAE0-4504-9B20-701417840786");
        }

        public string PluginMenu()
        {
            return "卡信息维护";
        }

        public void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo)
        {
            m_DBInfo = DbInfo;
            //必须，否则不能作为子窗口显示
            this.TopLevel = false;
            this.Parent = parent;
            this.Show();
            this.BringToFront();
            if (m_nAppOperatorAuthority != GrobalVariable.CardPublish_Authority)
            {
                btnModifyCard.Enabled = false;
                btnUnlockGrayCard.Enabled = false;
                btnCardLoad.Enabled = false;
                btnPinUnlock.Enabled = false;
                btnPinReset.Enabled = false;
                btnPinChange.Enabled = false;
            }
        }

        public void SetAuthority(int nLoginUserId,int nAuthority)
        {
            m_nLoginUserId = nLoginUserId;
            m_nAppOperatorAuthority = nAuthority;
        }

        private void AppUserOperator_Load(object sender, EventArgs e)
        {
            ReadInfoFromDb();

            cmbClientName.Items.Clear();
            foreach (ClientInfo info in m_ListClientInfo)
            {
                cmbClientName.Items.Add(info.strClientName);
            }

            m_DevControl = new ApduController(ApduDomain.DaHua);
            cmbDevType.SelectedIndexChanged += new System.EventHandler(this.cmbDevType_SelectedIndexChanged);
            ContactCard.Checked = false;
            ContactCard.Enabled = false;

            OpenDevice();
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
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName,m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
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

        private int GetCardTypeIndex(CardType eCardType)
        {
            int nSel = 0;
            switch (eCardType)
            {
                case CardType.PersonalCard:
                    nSel = 0;
                    break;
                case CardType.ManagerCard:
                    nSel = 1;
                    break;
                case CardType.EmployeeCard:
                    nSel = 2;
                    break;
                case CardType.ServiceCard:
                    nSel = 3;
                    break;
                case CardType.CompanySubCard:
                    nSel = 4;
                    break;
                case CardType.CompanyMotherCard:
                    nSel = 5;
                    break;
            }
            return nSel;
        }

        private void ShowDataToForm()
        {            
            cmbCardType.SelectedIndex = GetCardTypeIndex(m_CardInfoPar.UserCardType);
            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            textUserCardId.Text = m_CardInfoPar.CompanyID + UserCardInfoParam.CardGroup.ToString("X2") + nCardType.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            if (cmbClientName.Items.Count > 0)
                cmbClientName.SelectedIndex = GetClientIdIndex(m_CardInfoPar.ClientID);
            SetControlState(m_CardInfoPar.UserCardType);           
            if (m_CardInfoPar.UserCardType == CardType.CompanySubCard)
            {
                byte[] MotherCard = m_CardInfoPar.GetRelatedMotherCardID();
                if (MotherCard != null)
                {                    
                    cmbMotherCard.Text = BitConverter.ToString(MotherCard).Replace("-", "");
                }
                else
                {
                    ReadMotherCardList(m_CardInfoPar.CompanyID, m_CardInfoPar.UserCardType);
                    cmbMotherCard.Enabled = true;
                }
            }

            DateFrom.Value = m_CardInfoPar.ValidCardBegin;
            DateTo.Value = m_CardInfoPar.ValidCardEnd;
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

            this.LimitCarNo.CheckedChanged += new System.EventHandler(this.LimitCarNo_CheckedChanged);
            this.cmbAreaLimit.SelectedIndexChanged += new System.EventHandler(this.cmbAreaLimit_SelectedIndexChanged);
        }

        private void SetControlState(CardType eType)
        {
            cmbMotherCard.Enabled = false;
            if (eType == CardType.PersonalCard)
            {
                LimitCarNo.Enabled = true;
                cmbCarCategory.Enabled = true;
                textCarNo.Enabled = true;
                textSelfId.Enabled = true;
                textSelfId.Visible = true;
                textCarNo.Enabled = true;
                textDiscountRate.Enabled = true;
                DiscountRateExprieValid.Enabled = true;
                LabelMotherCard.Visible = false;
                cmbMotherCard.Visible = false;
            }
            else if (eType == CardType.CompanySubCard)
            {
                LimitCarNo.Enabled = true;
                cmbCarCategory.Enabled = true;
                textCarNo.Enabled = true;
                textSelfId.Enabled = true;
                textSelfId.Visible = true;
                textCarNo.Enabled = true;
                textDiscountRate.Enabled = true;
                DiscountRateExprieValid.Enabled = true;
                LabelMotherCard.Visible = true;
                cmbMotherCard.Visible = true;                
            }
            else
            {
                LimitCarNo.Checked = false;
                LimitCarNo.Enabled = false;
                cmbCarCategory.Enabled = false;
                textCarNo.Text = "";
                textCarNo.Enabled = false;
                textDiscountRate.Enabled = false;
                DiscountRateExprieValid.Enabled = false;
                textSelfId.Text = "";
                textSelfId.Enabled = false;
                textSelfId.Visible = false;
                LabelMotherCard.Visible = false;
                cmbMotherCard.Visible = false;
            }
        }

        private void ReadMotherCardList(string companyId, CardType eType)
        {
            if (eType != CardType.CompanySubCard)
            {
                return;
            }
            cmbMotherCard.Items.Clear();

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }

            string CardNoMin = companyId + "022100000000";//按公司代码获取单位母卡卡号
            string CardNoMax = companyId + "022100999999"; //最大单位母卡卡号
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam("CardNoMin", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMin);
            sqlparams[1] = ObjSql.MakeParam("CardNoMax", SqlDbType.Char, 16, ParameterDirection.Input, CardNoMax);

            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select CardNum from Base_Card where CardNum > @CardNoMin and CardNum < @CardNoMax", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbMotherCard.Items.Add((string)dataReader["CardNum"]);
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
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

        private void LimitCarNo_CheckedChanged(object sender, EventArgs e)
        {
            if (LimitCarNo.Checked && string.IsNullOrEmpty(textCarNo.Text))
            {
                MessageBox.Show("请输入车牌号");
                LimitCarNo.Checked = false;
            }
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

        private void cmbAreaLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //修改地域限制代码内容
            byte byteLimit = GetAreaLimit(cmbAreaLimit.SelectedIndex);
            FillListAreaCode(byteLimit);
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

        private bool OpenUserCard()
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            if (m_UserCardCtrl != null)
                return true;
            m_UserCardCtrl = m_DevControl.UserCardConstructor(ContactCard.Checked, m_DBInfo);

            string cardInfo = "";
            if (ContactCard.Checked)
                return m_DevControl.OpenContactCard(ref cardInfo);                
            else
                return m_DevControl.OpenCard(ref cardInfo);
        }

        private bool CloseUserCard()
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            if (ContactCard.Checked)
                m_DevControl.CloseContactCard();
            else
                m_DevControl.CloseCard();
            m_UserCardCtrl = null;
            return true;
        }

        private void btnReadCard_Click(object sender, EventArgs e)
        {
            if (!OpenUserCard())
            {
                MessageBox.Show("打开卡片失败");
                return;
            }
            ReadCardInfo(m_CardInfoPar);
            ReadCardInfoFormDb(m_CardInfoPar.GetUserCardID(), m_CardInfoPar);
            CloseUserCard();

            ShowDataToForm();
        }

        private void ReadCardInfo(UserCardInfoParam CardInfo)
        {
            if (!m_UserCardCtrl.SelectCardApp(1))
                return;            
            m_UserCardCtrl.GetUserCardInfo(CardInfo);
        }

        private void ReadCardInfoFormDb(byte[] CardId, UserCardInfoParam CardInfo)
        {
            if (CardId == null)
                return;
            string strCardId = BitConverter.ToString(CardId).Replace("-", "");

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            bool bHaveRecordInDb = false;
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);

            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Base_Card where CardNum = @CardId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    bHaveRecordInDb = true;
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("ClientId")))
                        CardInfo.ClientID = (int)dataReader["ClientId"];
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("RelatedMotherCard")))
                    {
                        string strMotherCard = (string)dataReader["RelatedMotherCard"];
                        if(strMotherCard != new string(' ',16))//母卡为空时，读取到的卡号为16个空格
                            CardInfo.SetMotherCard(strMotherCard);
                    }
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("DriverTel")))
                        CardInfo.TelePhone = (string)dataReader["DriverTel"];
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("SelfId")))
                        CardInfo.SelfId = (string)dataReader["SelfId"];
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Remark")))
                        CardInfo.Remark = (string)dataReader["Remark"];
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            if (!bHaveRecordInDb)
            {
                if (MessageBox.Show("无此卡号的发卡记录，是否增加？", "发卡", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    m_UserCardCtrl.SaveCpuCardInfoToDb(CardInfo, false);
                }
            }
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

        private void SaveCardInfoParam()
        {
            if (m_CardInfoPar.UserCardType == CardType.CompanySubCard && cmbMotherCard.SelectedIndex >= 0)
            {                
                string strCardId = BitConverter.ToString(m_CardInfoPar.GetUserCardID()).Replace("-", "");
                string strMotherCardId = (string)cmbMotherCard.Items[cmbMotherCard.SelectedIndex];
                string strPrompt = string.Format("单位子卡关联单位母卡只能进行一次\n是否将子卡{0}的母卡设为{1}？", strCardId, strMotherCardId);
                if (MessageBox.Show(strPrompt, "提醒", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    m_CardInfoPar.SetMotherCard(strMotherCardId);
                else
                    cmbMotherCard.SelectedIndex = -1;
            }
                        //保存数据
            if (cmbClientName.SelectedIndex >= 0 && cmbClientName.SelectedIndex < m_ListClientInfo.Count)
                m_CardInfoPar.ClientID = m_ListClientInfo[cmbClientName.SelectedIndex].ClientId;            
            if (DateFrom.Value < DateTo.Value)
            {
                m_CardInfoPar.ValidCardBegin = DateFrom.Value;
                m_CardInfoPar.ValidCardEnd = DateTo.Value;
            }
                
            m_CardInfoPar.UserName = textUserName.Text;

            double dbRate = 0;
            double.TryParse(textDiscountRate.Text, out dbRate);
            if (dbRate >= 1 && dbRate <= 99)
            {
                m_CardInfoPar.setDiscountRate(dbRate,DiscountRateExprieValid.Value);                
            }

            byte pricelevel = 1;
            byte.TryParse(textPriceLevel.Text, out pricelevel);
            m_CardInfoPar.PriceLevel = pricelevel;


            m_CardInfoPar.IdType = (UserCardInfoParam.IdentityType)(cmbIdType.SelectedIndex + 1);
            m_CardInfoPar.UserIdentity = textUserIdentity.Text;            

            m_CardInfoPar.CarType = GetCarCateGory(cmbCarCategory.SelectedIndex);//车类别
            m_CardInfoPar.CarNo = textCarNo.Text;
            m_CardInfoPar.TelePhone = textTelephone.Text;
            m_CardInfoPar.SelfId = textSelfId.Text;

            m_CardInfoPar.LimitCarNo = LimitCarNo.Checked;
            m_CardInfoPar.LimitGasType = GetLimitGasType(cmbLimitGasType.SelectedIndex);            

            byte AreaLimit = GetAreaLimit(cmbAreaLimit.SelectedIndex);
            string strLimitAreaCode = GetListAreaCode(AreaLimit);
            m_CardInfoPar.setLimitArea(AreaLimit, strLimitAreaCode);

            byte LimitGasCount = 0;
            byte.TryParse(textGasCount.Text, out LimitGasCount);
            m_CardInfoPar.LimitGasFillCount = LimitGasCount;
            
            double dbAmount = 0;
            double.TryParse(textGasAmount.Text,System.Globalization.NumberStyles.AllowThousands,null, out dbAmount);
            if (dbAmount > 0 && dbAmount < 1000000.0)
            {
                m_CardInfoPar.LimitGasFillAmount = (uint)(dbAmount * 100.0);
            }
            else
            {
                m_CardInfoPar.LimitGasFillAmount = 0xFFFFFFFF;
            }

            m_CardInfoPar.BoalId = textBoalNo.Text;
            int nBoalCount = 2;
            int.TryParse(textBoalCount.Text, out nBoalCount);            
            m_CardInfoPar.CylinderNum = nBoalCount;

            ushort volume = 375;
            ushort.TryParse(textBoalVol.Text, out volume);
            m_CardInfoPar.CylinderVolume = volume;            
            m_CardInfoPar.BoalExprie = BoalExprieValid.Value;
            m_CardInfoPar.BoalFactoryID = textBoalFactoryNo.Text;
            m_CardInfoPar.BusDistance = textBusDistance.Text;
            m_CardInfoPar.Remark = textRemark.Text;
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

        private void SetListAreaCodeChecked(string strLimitAreaCode, byte byteLimit)
        {
            if (byteLimit == 0xFF || strLimitAreaCode.Length % 2 != 0)
                return;
            byte[] AreaCode = PublicFunc.StringToBCD(strLimitAreaCode);
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
                                if (PublicFunc.ByteDataEquals(info.StationCode, StationCode))
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

        private void listLimitArea_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            byte AreaLimit = GetAreaLimit(cmbAreaLimit.SelectedIndex);
            if (AreaLimit != 0x04)
            {
                //非限站点只能选一个
                int nCount = listLimitArea.Items.Count;
                for(int i=0; i<nCount; i++)
                     listLimitArea.SetItemChecked(i,false);
            }
            else if (listLimitArea.CheckedItems.Count >= 20)
            {
                e.NewValue = e.CurrentValue;
            }
        }

        private void btnModifyCard_Click(object sender, EventArgs e)
        {
            //获取卡信息修改，更新卡片文件，成功后写入数据库
            SaveCardInfoParam();
            if (MessageBox.Show("是否更新卡片？", "确认", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            if (!OpenUserCard())
            {
                MessageBox.Show("打开卡片失败");
                return;
            }
            bool bWriteToCard = m_UserCardCtrl.UpdateCardInfo(m_CardInfoPar);
            if(!bWriteToCard)
            {
                MessageBox.Show("修改卡片信息失败");                
            }
            else
            {
                m_UserCardCtrl.SaveCpuCardInfoToDb(m_CardInfoPar,true);
                MessageBox.Show("修改卡片信息成功");
            }
            CloseUserCard();
        }


        private void textPIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textPIN_Validated(object sender, EventArgs e)
        {
            m_strPIN = textPIN.Text;
        }

        //卡片圈存,未灰锁的卡读终端机编号为0，使用固定终端机编号
        private void btnCardLoad_Click(object sender, EventArgs e)
        {
            if (!OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp(1))
                return;
            if (!OpenSAMCard(false))
            {
                MessageBox.Show("插入PSAM卡后才能圈存");
                return;
            }

            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;

            decimal MoneyLoad = 0;
            decimal.TryParse(textMoney.Text, System.Globalization.NumberStyles.AllowThousands, null, out MoneyLoad);
            double dbMoneyLoad = decimal.ToDouble(MoneyLoad);
            if (dbMoneyLoad < 1)
                return;

            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            else if (ASN[3] == 0x02)
            {
                MessageBox.Show("管理卡不能圈存", "卡充值",MessageBoxButtons.OK);
                return;
            }
            else if (ASN[3] == 0x06)
            {
                MessageBox.Show("维修卡不能圈存", "卡充值", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string strCardType = PublicFunc.GetCardTypeString(ASN[3]);
                string strTemp = "圈存";
                if( (ASN[3] == 0x11) || (ASN[3] == 0x21) )
                {
                    strTemp = "充值";
                }

                string strMsg = "确实要对" + strCardType + BitConverter.ToString(ASN).Replace("-", "") + strTemp + dbMoneyLoad.ToString("F2") + "元吗？";
                if (MessageBox.Show(strMsg, "卡充值", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }


            if ((ASN[3] == 0x11) || (ASN[3] == 0x21))
            {
                RechargeCompanyCard(m_FixedTermialId, ASN, dbMoneyLoad);
            }
            else
            {
                int nRet = m_UserCardCtrl.VerifyUserPin(m_strPIN);
                if (nRet == 1)
                {
                    LoadUserCard(m_FixedTermialId, ASN, dbMoneyLoad);
                }
                else if (nRet == 2)
                {
                    MessageBox.Show("PIN码已锁!");
                }
                else
                {
                    MessageBox.Show("PIN码验证失败!");
                }
            }

            CloseSAMCard(false);
            CloseUserCard();           
        }

        /// <summary>
        /// 用户卡圈存
        /// </summary>
        /// <param name="TerminalId">固定终端机 size 6</param>
        /// <param name="ASN">用户卡号 size 8</param>
        /// <param name="dbMoneyLoad">圈存金额 单位 元</param>
        private void LoadUserCard(byte[] TerminalId, byte[]  ASN, double dbMoneyLoad)
        {
            int nBalance = 0;
            if (m_UserCardCtrl.UserCardBalance(ref nBalance, BalanceType.Balance_ED))//圈存前读余额
            {
                double dbBalance = (double)(nBalance / 100.0);
                if (m_UserCardCtrl.UserCardLoad(ASN, TerminalId, (int)(dbMoneyLoad * 100.0), true))
                {
                    //写圈存数据库记录
                    SaveLoadRecord(ASN, dbMoneyLoad, dbBalance, "CardBalance");
                    string strInfo = string.Format("成功对卡号{0}圈存{1}元.", BitConverter.ToString(ASN).Replace("-", ""), dbMoneyLoad.ToString("F2"));
                    MessageBox.Show(strInfo);
                }
                else
                {
                    MessageBox.Show("圈存失败");
                }
            }
            else
            {
                MessageBox.Show("读取余额失败");
            }
        }

        /// <summary>
        /// 积分圈存
        /// </summary>
        /// <param name="TerminalId">固定终端机</param>
        /// <param name="ASN">用户卡号</param>
        /// <param name="Loyalty">积分额</param>
        private void LoadUserCardLy(byte[] TerminalId, byte[] ASN, int Loyalty)
        {
            int nBalance = 0;
            if (m_UserCardCtrl.UserCardBalance(ref nBalance, BalanceType.Balance_EP))//圈存前读积分余额
            {
                if (m_UserCardCtrl.LoyaltyLoad(ASN, TerminalId, Loyalty, true))
                {
                    //写圈存数据库记录
                    SaveLoadLoyaltyRecord(ASN, Loyalty, nBalance, "CreditsTotal");
                    string strInfo = string.Format("成功对卡号{0}圈存{1}积分.", BitConverter.ToString(ASN).Replace("-", ""), Loyalty.ToString());
                    MessageBox.Show(strInfo);
                }
                else
                {
                    MessageBox.Show("圈存积分失败");
                }
            }
            else
            {
                MessageBox.Show("读取积分余额失败");
            }
        }

        //读余额
        private void btnBalance_Click(object sender, EventArgs e)
        {
            if (!OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp(1))
                return;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }            
            int nRet = m_UserCardCtrl.VerifyUserPin(m_strPIN);
            if (nRet == 1)
            {
                m_bGray = false;
                //未灰锁时终端机编号输出为0
                int nCardStatus = 0;
                if (m_UserCardCtrl.UserCardGray(ref nCardStatus, m_TermialId, m_GTAC))
                {
                    if (nCardStatus == 2)
                    {
                        //当前TAC未读，需要清空后重读
                        m_UserCardCtrl.ClearTACUF();
                        nCardStatus = 0;
                        m_UserCardCtrl.UserCardGray(ref nCardStatus, m_TermialId, m_GTAC);
                        m_bGray = nCardStatus == 1 ? true : false;
                    }
                    else
                    {
                        m_bGray = nCardStatus == 1 ? true : false;
                    }
                    GrayFlag.CheckState = m_bGray ? CheckState.Checked : CheckState.Unchecked;
                    GrayFlag.Checked = m_bGray;
                }
                else
                {
                    GrayFlag.CheckState = CheckState.Indeterminate;
                    GrayFlag.Checked = false;
                }

                int nBalance = 0;
                if (m_UserCardCtrl.UserCardBalance(ref nBalance, BalanceType.Balance_ED))
                {
                    double dbBalance = (double)(nBalance / 100.0);
                    textBalance.Text = dbBalance.ToString("F2");
                    SaveCardValueToDb(ASN, dbBalance, "CardBalance");
                }
                else
                {
                    textBalance.Text = "0.00";
                }
                
            }
            else if (nRet == 2)
            {
                MessageBox.Show("PIN码已锁!");
            }
            else
            {
                MessageBox.Show("PIN码验证失败!");
            }
            CloseUserCard();
        }

        //解灰,未灰锁的卡读终端机编号为0，使用固定终端机编号
        private void btnUnlockGrayCard_Click(object sender, EventArgs e)
        {
            //未灰状态不可解灰
            if (!m_bGray || !OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp(1))
                return;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }

            //获取数据库灰卡记录和终端机编号
            GrayCardInfo GrayInfo = new GrayCardInfo();
            if (!GetCardGrayRecord(ASN, m_TermialId, m_GTAC, GrayInfo))
            {
                MessageBox.Show("没有此卡的灰卡记录。");
                return;
            }

            int nRet = m_UserCardCtrl.VerifyUserPin(m_strPIN);
            if (nRet == 1)
            {
                if (m_UserCardCtrl.UnLockGrayCard(ASN, m_TermialId, (int)(GrayInfo.Money * 100.0), true,1))
                {
                    m_bGray = false;
                    SaveUnGrayRecord(GrayInfo);
                }
            }
            else if (nRet == 2)
            {
                MessageBox.Show("PIN码已锁!");
            }
            else
            {
                MessageBox.Show("PIN码验证失败!");
            }
            CloseUserCard();
        }

        //只要原ＰＩＮ
        private void btnPinUnlock_Click(object sender, EventArgs e)
        {
            if (textOldPIN.Text.Length != 6)
            {
                MessageBox.Show("请输入原PIN码");
                return;
            }

            if (!OpenUserCard() || !m_UserCardCtrl.SelectCardApp(1))
                return;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            if (m_UserCardCtrl.PINUnLock(ASN, textOldPIN.Text,1))
            {
                MessageBox.Show("PIN码已解锁");
            }
            else
            {
                MessageBox.Show("PIN码解锁失败");
            }
            CloseUserCard();
        }

        //只要新ＰＩＮ
        private void btnPinReset_Click(object sender, EventArgs e)
        {
            if (textNewPIN.Text.Length != 6)
            {
                MessageBox.Show("请输入重新装入的新PIN码");
                return;
            }
            if (!OpenUserCard() || !m_UserCardCtrl.SelectCardApp(1))
                return;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            if (m_UserCardCtrl.PINReset(ASN, textNewPIN.Text,1))
            {
                MessageBox.Show("新PIN码已装入");
            }
            else
            {
                MessageBox.Show("PIN码重装失败");
            }
            CloseUserCard();
        }

        //修改ＰＩＮ码
        private void btnPinChange_Click(object sender, EventArgs e)
        {
            if(textOldPIN.Text.Length != 6 || textNewPIN.Text.Length != 6)
                return;
            if (!OpenUserCard() || !m_UserCardCtrl.SelectCardApp(1))
                return;
            if (m_UserCardCtrl.ChangePIN(textOldPIN.Text, textNewPIN.Text))
            {
                if (m_UserCardCtrl.VerifyUserPin(textNewPIN.Text) == 1)
                    MessageBox.Show("PIN码修改成功");
                else
                    MessageBox.Show("PIN码修改后验证失败，请重试");
            }
            else
            {
                MessageBox.Show("PIN码修改失败");
            }
            CloseUserCard();

        }

        private void textOldPIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textNewPIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void SaveLoadLoyaltyRecord(byte[] ASN, int LoadLoyalty, int nBalance, string strUpdateField)
        {
            string strCardId = BitConverter.ToString(ASN).Replace("-", "");

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }

            int nTotal = nBalance + LoadLoyalty;
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam(strUpdateField, SqlDbType.Int, 4, ParameterDirection.Input, nTotal);
            sqlparams[1] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
            ObjSql.ExecuteCommand("update Base_Card set " + strUpdateField + "=@" + strUpdateField + " where CardNum=@CardId", sqlparams);

            ObjSql.CloseConnection();
            ObjSql = null;
        }

        /// <summary>
        /// 圈存记录
        /// </summary>
        /// <param name="ASN">卡号</param>
        /// <param name="dbLoadMoney">圈存金额</param>
        /// <param name="dbBalance">之前的余额</param>
        /// <param name="strCardBalanceField">更新字段名称</param>
        private void SaveLoadRecord(byte[]  ASN, double dbLoadMoney,double dbBalance, string strUpdateField)
        {
            string strCardId = BitConverter.ToString(ASN).Replace("-", "");

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }

            double dblTotal = dbBalance + dbLoadMoney;

            SqlParameter[] sqlparams = new SqlParameter[7];
            sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
            sqlparams[1] = ObjSql.MakeParam("Balance", SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(dbBalance));
            sqlparams[2] = ObjSql.MakeParam("Recharge", SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(dbLoadMoney));
            sqlparams[3] = ObjSql.MakeParam("Total", SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(dblTotal));
            sqlparams[4] = ObjSql.MakeParam("Time", SqlDbType.DateTime, 8, ParameterDirection.Input, DateTime.Now);
            sqlparams[5] = ObjSql.MakeParam("OperatorId", SqlDbType.Int, 4, ParameterDirection.Input, m_nLoginUserId);            
            sqlparams[6] = ObjSql.MakeParam("TimeStr", SqlDbType.VarChar, 10, ParameterDirection.Input, DateTime.Now.ToString("yyyyMMdd") + "01");


            ObjSql.ExecuteCommand("insert into Data_RechargeCardRecord values(@CardId,N'充值',@Balance,@Recharge,0,@Recharge,@Total,@Time,@OperatorId,N'现金支付',@TimeStr,0)", sqlparams);

            //更新Base_Card充值总额和当前卡余额
            double dbRechargeTotal = GetBaseCardMoneyValue(ObjSql, strCardId, "RechargeTotal") + dbLoadMoney;
            UpdateBaseCardValue(ObjSql, strCardId, "RechargeTotal", dbRechargeTotal);
            UpdateBaseCardValue(ObjSql, strCardId, strUpdateField, dblTotal);

            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void UpdateBaseCardValue(SqlHelper ObjSql, string strCardId, string strFieldName, double dbValue)
        {
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam(strFieldName, SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(dbValue));
            sqlparams[1] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
            ObjSql.ExecuteCommand("update Base_Card set " + strFieldName + "=@" + strFieldName + " where CardNum=@CardId", sqlparams);
        }

        private double GetBaseCardMoneyValue(SqlHelper ObjSql, string strCardId, string strFieldName)
        {
            double dbValue = 0.0;
            SqlParameter[] sqlparams = new SqlParameter[1];            
            sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
            
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select " + strFieldName + " from Base_Card where CardNum = @CardId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    decimal FieldValue = (decimal)dataReader[strFieldName];
                    dbValue = decimal.ToDouble(FieldValue);
                }
                dataReader.Close();
            }
            return dbValue;
        }

        private void SaveCardValueToDb(byte[] ASN, double dbBalance, string strFieldName)
        {
            string strCardId = BitConverter.ToString(ASN).Replace("-", "");

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            UpdateBaseCardValue(ObjSql, strCardId, strFieldName, dbBalance);

            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void cmbDevType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSel = cmbDevType.SelectedIndex;
            if (nSel == 0)
            {
                CloseDevice();
                m_DevControl = new ApduController(ApduDomain.DaHua);
                ContactCard.Checked = false;
                ContactCard.Enabled = false;
                OpenDevice();
            }
            else if (nSel == 1)
            {
                CloseDevice();
                m_DevControl = new ApduController(ApduDomain.LongHuan);
                ContactCard.Checked = false;
                ContactCard.Enabled = true;
                OpenDevice();
            }
            else if (nSel == 2)
            {
                CloseDevice();
                m_DevControl = new ApduController(ApduDomain.LoH_at_MT);
                ContactCard.Checked = false;
                ContactCard.Enabled = true;
                OpenDevice();
            }
            else
            {
                m_DevControl = null;
                ContactCard.Checked = false;
                ContactCard.Enabled = false;
            }
        }

        private bool OpenDevice()
        {
            if (m_DevControl == null)
                return false;
            if (m_DevControl.IsDeviceOpen())
                return true;
            return m_DevControl.Open_Device();
        }

        private bool CloseDevice()
        {
            if (m_DevControl == null)
                return false;
            if (!m_DevControl.IsDeviceOpen())
                return false;
            m_DevControl.Close_Device();
            return true;
        }

        private void textUserIdentity_KeyPress(object sender, KeyPressEventArgs e)
        {
            //身份证号只接受数字值和字母X
            if (textUserIdentity.Text.Length < 17)
            {
                if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                    e.Handled = true;
            }
            else
            {
                if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace && e.KeyChar != Key_X)
                    e.Handled = true;
            }
        }

        private void textDiscountRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace && e.KeyChar != Value_Dot)
                e.Handled = true;//不接受非数字值
        }

        private void textGasAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace && e.KeyChar != Value_Dot)
                e.Handled = true;//不接受非数字值   
        }

        private void textGasCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值   
        }

        private void textPriceLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textGasCount_Validated(object sender, EventArgs e)
        {
            byte LimitGasCount = 0;
            byte.TryParse(textGasCount.Text, out LimitGasCount);
            if (LimitGasCount <= 0 || LimitGasCount > 9)
            {
                textGasCount.Text = "";
                MessageBox.Show("每日限制加气1～9次。");
            }
        }

        private void textGasAmount_Validated(object sender, EventArgs e)
        {
            double dbAmount = 0;
            double.TryParse(textGasAmount.Text, System.Globalization.NumberStyles.AllowThousands, null, out dbAmount);
            if (dbAmount <= 0 && dbAmount >= 1000000.0)
            {
                textGasCount.Text = "";
                MessageBox.Show("每日限制总加气金额最多1000,000.00元");                
            }
        }

        private void AppUserOperator_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseDevice();
        }

        private void SaveUnGrayRecord(GrayCardInfo GrayInfo)
        {
            string strCardId = BitConverter.ToString(GrayInfo.ASN).Replace("-", "");
            string strStationNo = BitConverter.ToString(GrayInfo.StationNo).Replace("-", "");            

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            SqlParameter[] sqlparams = new SqlParameter[10];
            sqlparams[0] = ObjSql.MakeParam("StationNo", SqlDbType.Char, 8, ParameterDirection.Input, strStationNo);
            sqlparams[1] = ObjSql.MakeParam("GunNo", SqlDbType.Int, 4, ParameterDirection.Input, GrayInfo.GunNo);
            sqlparams[0] = ObjSql.MakeParam("CardNo", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
            sqlparams[1] = ObjSql.MakeParam("TradeDateTime", SqlDbType.DateTime, 8, ParameterDirection.Input, GrayInfo.ConsumerTime);
            sqlparams[2] = ObjSql.MakeParam("GrayPrice", SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(GrayInfo.Price));
            sqlparams[2] = ObjSql.MakeParam("GrayGas", SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(GrayInfo.Gas));
            sqlparams[3] = ObjSql.MakeParam("GrayMoney", SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(GrayInfo.Money));
            sqlparams[3] = ObjSql.MakeParam("ResidualAmount", SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(GrayInfo.ResidualAmount));
            sqlparams[5] = ObjSql.MakeParam("Operator", SqlDbType.Int, 4, ParameterDirection.Input, m_nLoginUserId);
            sqlparams[6] = ObjSql.MakeParam("OperateTime", SqlDbType.DateTime, 8, ParameterDirection.Input, DateTime.Now);


            ObjSql.ExecuteCommand("insert into Data_GreyCardRecord values(@StationNo,@GunNo,@CardNo,@TradeDateTime,@GrayPrice,@GrayGas,@GrayMoney,@ResidualAmount,@Operator,@OperateTime)", sqlparams);

            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private bool GetCardGrayRecord(byte[] ASN, byte[] PSAM_TID, byte[] GTAC, GrayCardInfo GrayCardInfoInDb)
        {
            string strCardId = BitConverter.ToString(ASN).Replace("-", "");
            string strTerminalId = BitConverter.ToString(PSAM_TID).Replace("-", "");
            string strGTAC = BitConverter.ToString(GTAC).Replace("-", "");

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return false;
            }

            SqlParameter[] sqlparams = new SqlParameter[10];
            sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
            sqlparams[1] = ObjSql.MakeParam("PSAM_TID", SqlDbType.Char, 12, ParameterDirection.Input, strTerminalId);
            sqlparams[2] = ObjSql.MakeParam("GTAC", SqlDbType.Char, 8, ParameterDirection.Input, strGTAC);

            sqlparams[3] = ObjSql.MakeParam("StationNo", SqlDbType.Char, 8, ParameterDirection.Output, null);
            sqlparams[4] = ObjSql.MakeParam("GunNo", SqlDbType.Int, 4, ParameterDirection.Output, null);
            sqlparams[5] = ObjSql.MakeParam("ConsumerTime", SqlDbType.DateTime, 8, ParameterDirection.Output, null);
            sqlparams[6] = ObjSql.MakeParam("Price", SqlDbType.Decimal, 16, ParameterDirection.Output, null);
            sqlparams[7] = ObjSql.MakeParam("Gas", SqlDbType.Decimal, 16, ParameterDirection.Output, null);
            sqlparams[8] = ObjSql.MakeParam("Money", SqlDbType.Decimal, 16, ParameterDirection.Output, null);
            sqlparams[9] = ObjSql.MakeParam("ResidualAmount", SqlDbType.Decimal, 16, ParameterDirection.Output, null);

            if (ObjSql.ExecuteProc("PROC_GetGrayRecord", sqlparams) != 0)
            {
                ObjSql.CloseConnection();
                ObjSql = null;
                return false;
            }

            bool bRet = true;
            byte[] data = null;
            Buffer.BlockCopy(ASN, 0, GrayCardInfoInDb.ASN, 0, 8);

            if (sqlparams[3].Value != null && sqlparams[3].Value != DBNull.Value)
            {
                data = PublicFunc.StringToBCD((string)sqlparams[3].Value);
                Buffer.BlockCopy(data, 0, GrayCardInfoInDb.StationNo, 0, 4);
            }

            if (sqlparams[4].Value != null && sqlparams[4].Value != DBNull.Value)
                GrayCardInfoInDb.GunNo = (int)sqlparams[4].Value;
            else
                bRet = false;

            if (sqlparams[5].Value != null && sqlparams[5].Value != DBNull.Value)
                GrayCardInfoInDb.ConsumerTime = (DateTime)sqlparams[5].Value;
            else
                bRet = false;

            if (sqlparams[6].Value != null && sqlparams[6].Value != DBNull.Value)
                GrayCardInfoInDb.Price = decimal.ToDouble((decimal)sqlparams[6].Value);
            else
                bRet = false;


            if (sqlparams[7].Value != null && sqlparams[7].Value != DBNull.Value)
                GrayCardInfoInDb.Gas = decimal.ToDouble((decimal)sqlparams[7].Value);
            else
                bRet = false;


            if (sqlparams[8].Value != null && sqlparams[8].Value != DBNull.Value)
                GrayCardInfoInDb.Money = decimal.ToDouble((decimal)sqlparams[8].Value);
            else
                bRet = false;

            if (sqlparams[9].Value != null && sqlparams[9].Value != DBNull.Value)
                GrayCardInfoInDb.ResidualAmount = decimal.ToDouble((decimal)sqlparams[9].Value);
            else
                bRet = false;

            ObjSql.CloseConnection();
            ObjSql = null;
            return bRet;
        }


        /// <summary>
        /// 单位子母卡圈存
        /// </summary>
        private void RechargeCompanyCard(byte[] TerminalId, byte[] ASN, double dbMoneyLoad)
        {
            //通过卡号获取关联母卡
            if (ASN[3] == 0x11)
            {
                string strMotherCardAsn = GlobalControl.GetPublishedCardInfoFormDb(m_DBInfo, ASN, "RelatedMotherCard", 1);
                if (string.IsNullOrEmpty(strMotherCardAsn))
                {
                    MessageBox.Show("请先关联该卡的母卡。\n如果关联母卡列表为空，应先制单位母卡。", "子卡充值", MessageBoxButtons.OK);                    
                    return;
                }
                byte[] MotherAsn = PublicFunc.StringToBCD(strMotherCardAsn);
                //获取母卡余额
                string strBalance = GlobalControl.GetPublishedCardInfoFormDb(m_DBInfo, MotherAsn, "AccountBalance", 1);
                decimal Balance = 0;
                decimal.TryParse(strBalance, System.Globalization.NumberStyles.AllowThousands, null, out Balance);
                double dbBalance = decimal.ToDouble(Balance);
                
                if (dbBalance < dbMoneyLoad)
                {
                    string strMsg = string.Format("单位母卡的余额{0}元，不能对单位子卡圈存{1}元",dbBalance.ToString("F2"),dbMoneyLoad.ToString("F2"));
                    MessageBox.Show(strMsg,"子卡充值",MessageBoxButtons.OK);
                    return;
                }
                LoadUserCard(TerminalId, ASN, dbMoneyLoad);
                //单位母卡金额减掉
                SaveCardValueToDb(MotherAsn, dbBalance - dbMoneyLoad, "AccountBalance");
            }
            else if(ASN[3] == 0x21)
            {
                //母卡充值
                string strBalance = GlobalControl.GetPublishedCardInfoFormDb(m_DBInfo, ASN, "AccountBalance", 1);
                decimal Balance = 0;
                decimal.TryParse(strBalance, System.Globalization.NumberStyles.AllowThousands, null, out Balance);
                double dbBalance = decimal.ToDouble(Balance);

                SaveLoadRecord(ASN, dbMoneyLoad, dbBalance, "AccountBalance");//单位母卡充值后更新字段不一样
                string strInfo = string.Format("成功对单位母卡{0}充值{1}元.", BitConverter.ToString(ASN).Replace("-", ""), dbMoneyLoad.ToString("F2"));
                MessageBox.Show(strInfo);                
            }
        }

        private void LoadLoyalty_Click(object sender, EventArgs e)
        {
            int LoadLoyalty = 0;
            int.TryParse(textLoadValue.Text,System.Globalization.NumberStyles.AllowThousands, null,out LoadLoyalty);
            if (LoadLoyalty < 1)
                return;

            if (!OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp(2))
                return;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
                    
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            else if (ASN[3] != 0x01)
            {
                MessageBox.Show("只有用户卡可以积分", "圈存", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string strMsg = "确实要对用户卡" + BitConverter.ToString(ASN).Replace("-", "") + "充值" + LoadLoyalty.ToString() + "积分吗？";
                if (MessageBox.Show(strMsg, "积分充值", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

            }

            int nRet = m_UserCardCtrl.VerifyUserPin(m_strPin_Ly);
            if (nRet == 1)
            {
                LoadUserCardLy(m_FixedTermialId, ASN, LoadLoyalty);
            }
            else if (nRet == 2)
            {
                MessageBox.Show("积分PIN码已锁!");
            }
            else
            {
                MessageBox.Show("积分PIN码验证失败!");
            }

            CloseUserCard();    
        }

        private void ReadLoyalty_Click(object sender, EventArgs e)
        {
            if (!OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp(2))
                return;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            int nRet = m_UserCardCtrl.VerifyUserPin(m_strPin_Ly);
            if (nRet == 1)
            {
                m_bGray = false;
                //未灰锁时终端机编号输出为0
                int nCardStatus = 0;
                if (m_UserCardCtrl.UserCardGray(ref nCardStatus, m_TermialId_Ly, m_GTAC_Ly))
                {
                    if (nCardStatus == 2)
                    {
                        //当前TAC未读，需要清空后重读
                        m_UserCardCtrl.ClearTACUF();
                        nCardStatus = 0;
                        m_UserCardCtrl.UserCardGray(ref nCardStatus, m_TermialId_Ly, m_GTAC_Ly);
                        m_bGray_Ly = nCardStatus == 1 ? true : false;
                    }
                    else
                    {
                        m_bGray_Ly = nCardStatus == 1 ? true : false;
                    }
                    ChkGrayLy.CheckState = m_bGray_Ly ? CheckState.Checked : CheckState.Unchecked;
                    ChkGrayLy.Checked = m_bGray_Ly;
                }
                else
                {
                    ChkGrayLy.CheckState = CheckState.Indeterminate;
                    ChkGrayLy.Checked = false;
                }

                int nBalance = 0;
                if (m_UserCardCtrl.UserCardBalance(ref nBalance, BalanceType.Balance_EP))
                {                    
                    textLyBalance.Text = nBalance.ToString();
                    SaveCardValueToDb(ASN, nBalance, "CreditsTotal");
                }
                else
                {
                    textLyBalance.Text = "0.00";
                }

            }
            else if (nRet == 2)
            {
                MessageBox.Show("积分PIN码已锁!");
            }
            else
            {
                MessageBox.Show("积分PIN码验证失败!");
            }
            CloseUserCard();
        }

        private void textPIN_Ly_TextChanged(object sender, EventArgs e)
        {
            m_strPin_Ly = textPIN_Ly.Text;
        }

        private void textPIN_Ly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void UnGrayLy_Click(object sender, EventArgs e)
        {
            //未灰状态不可强制解灰
            if (!m_bGray_Ly || !OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp(2))
                return;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }

            //积分灰锁强制解灰
            int nUnGrayLoyalty = 0; 

            int nRet = m_UserCardCtrl.VerifyUserPin(m_strPin_Ly);
            if (nRet == 1)
            {
                if (m_UserCardCtrl.UnLockGrayCard(ASN, m_TermialId_Ly, nUnGrayLoyalty, true, 2))
                {
                    m_bGray_Ly = false;                    
                }
            }
            else if (nRet == 2)
            {
                MessageBox.Show("积分PIN码已锁!");
            }
            else
            {
                MessageBox.Show("积分PIN码验证失败!");
            }
            CloseUserCard();
        }

        private void PinUnlock_Ly_Click(object sender, EventArgs e)
        {
            if (textOldPin_Ly.Text.Length != 6)
            {
                MessageBox.Show("请输入原积分PIN码");
                return;
            }

            if (!OpenUserCard() || !m_UserCardCtrl.SelectCardApp(2))
                return;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            if (m_UserCardCtrl.PINUnLock(ASN, textOldPin_Ly.Text,2))
            {
                MessageBox.Show("积分PIN码已解锁");
            }
            else
            {
                MessageBox.Show("积分PIN码解锁失败");
            }
            CloseUserCard();
        }

        private void PinReset_Ly_Click(object sender, EventArgs e)
        {
            if (textNewPin_Ly.Text.Length != 6)
            {
                MessageBox.Show("请输入新的积分PIN码");
                return;
            }
            if (!OpenUserCard() || !m_UserCardCtrl.SelectCardApp(2))
                return;
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            if (m_UserCardCtrl.PINReset(ASN, textNewPin_Ly.Text, 2))
            {
                MessageBox.Show("新积分PIN码已装入");
            }
            else
            {
                MessageBox.Show("积分PIN码重装失败");
            }
            CloseUserCard();
        }

        private void ChangePin_Ly_Click(object sender, EventArgs e)
        {
            if (textOldPin_Ly.Text.Length != 6 || textNewPin_Ly.Text.Length != 6)
                return;
            if (!OpenUserCard() || !m_UserCardCtrl.SelectCardApp(2))
                return;
            if (m_UserCardCtrl.ChangePIN(textOldPin_Ly.Text, textNewPin_Ly.Text))
            {
                if (m_UserCardCtrl.VerifyUserPin(textNewPin_Ly.Text) == 1)
                    MessageBox.Show("积分PIN码修改成功");
                else
                    MessageBox.Show("积分PIN码修改后验证失败，请重试");
            }
            else
            {
                MessageBox.Show("积分PIN码修改失败");
            }
            CloseUserCard();
        }

        private bool OpenSAMCard(bool bSamSlot)
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            m_SamCardCtrl = m_DevControl.SamCardConstructor(null);

            string strCardInfo = "";
            return m_DevControl.SAMPowerOn(bSamSlot, ref strCardInfo);
        }


        private bool CloseSAMCard(bool bSamSlot)
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            m_DevControl.SAMPowerOff(bSamSlot);            
            m_SamCardCtrl = null;
            return true;
        }


    }
}
