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

namespace CardOperating
{
    public partial class AppUserOperator : Form , IPlugin
    {
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private int m_nAppOperatorAuthority = 0;
        private int m_nLoginUserId = 0;

        private const Char Backspace = (Char)8;
        private UserCardInfoParam m_CardInfoPar = new UserCardInfoParam();
        private UserCardControl m_UserCardCtrl = null;
        private readonly byte[] m_FixedTermialId = new byte[] { 0x20, 0x15, 0x01, 0x01, 0x00, 0x01 };  //固定的终端机设备编号

        private static byte[] m_TermialId = new byte[6];            //终端机设备编号
        private static byte[] m_ASN = new byte[8];//用户卡卡号
        private static string m_strPIN = "999999";//由用户输入

        private bool m_bGray = false;   //卡已灰，不能扣款解锁
        private bool m_bTACUF = false;        

        //一个公司可以有多个单位（分公司），一个单位下可以有多个站点。
        private List<ClientInfo> m_ListClientInfo = new List<ClientInfo>();//单位信息
        private List<ProvinceInfo> m_ListProvinceInfo = new List<ProvinceInfo>(); //省
        private List<CityInfo> m_ListCityInfo = new List<CityInfo>();   //地市
        private List<SuperiorInfo> m_ListSuperiorInfo = new List<SuperiorInfo>(); //上级单位（公司）
        private List<StationInfo> m_ListStationInfo = new List<StationInfo>(); //上级单位（公司）
        private int m_hDevHandler = 0;//读卡器句柄
        public AppUserOperator()
        {
            InitializeComponent();
            textPIN.Text = m_strPIN;
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
        }

        private int GetClientIdIndex(int nClientID)
        {
            int nIndex = 0;
            foreach (ClientInfo info in m_ListClientInfo)
            {
                if (info.ClientId == nClientID)
                {
                    break;
                }
                nIndex++;
            }
            return nIndex;
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
                        for (int i = 0; i < 2; i++)
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

        private void ShowDataToForm()
        {
            byte nCardType = (byte)m_CardInfoPar.UserCardType;
            textUserCardId.Text = m_CardInfoPar.CompanyID + UserCardInfoParam.CardGroup.ToString("X2") + nCardType.ToString("X2") + "00" + m_CardInfoPar.CardOrderNo;
            if (cmbClientName.Items.Count > 0)
                cmbClientName.SelectedIndex = GetClientIdIndex(m_CardInfoPar.ClientID);
            DateFrom.Value = m_CardInfoPar.ValidCardBegin;
            DateTo.Value = m_CardInfoPar.ValidCardEnd;
            textUserName.Text = m_CardInfoPar.UserName;
            textPriceLevel.Text = m_CardInfoPar.PriceLevel.ToString();
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
            if (m_hDevHandler > 0 || m_UserCardCtrl != null)
                return true;
            m_hDevHandler = DllExportMT.open_device(0, 9600);
            if (m_hDevHandler <= 0)
                return false;
            m_UserCardCtrl = new UserCardControl(m_hDevHandler, m_DBInfo);

            byte[] cardUid = new byte[4];
            byte[] cardInfo = new byte[64];
            byte[] cardInfolen = new byte[4];            
            short nRetValue = DllExportMT.OpenCard(m_hDevHandler, 1, cardUid, cardInfo, cardInfolen);
            if (nRetValue != 0)              
                return false;
            else
                return true;
        }

        private bool CloseUserCard()
        {
            if (m_hDevHandler <= 0)
                return false;
            DllExportMT.CloseCard(m_hDevHandler);
            DllExportMT.close_device(m_hDevHandler);
            m_UserCardCtrl = null;
            m_hDevHandler = 0;
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
            if (!m_UserCardCtrl.SelectCardApp())
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
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);

            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Base_Card where CardNum = @CardId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("ClientId")))
                        CardInfo.ClientID = (int)dataReader["ClientId"];
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

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }

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
            ObjSql.CloseConnection();
            ObjSql = null;

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
                        for (int nLimitIndex = 0; nLimitIndex < AreaCode.Length; nLimitIndex += 2)
                        {
                            int i = 0;
                            foreach (StationInfo info in m_ListStationInfo)
                            {
                                if ((info.StationCode[0] == AreaCode[nLimitIndex]) && (info.StationCode[1] == AreaCode[nLimitIndex+1]))
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
                    case 0x04://限站（最多20个）
                        foreach (StationInfo info in m_ListStationInfo)
                        {
                            if (listLimitArea.CheckedItems.Contains(info.strStationName))
                            {
                                strLimitAreaCode += BitConverter.ToString(info.StationCode).Replace("-", "");                                
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
            if (!OpenUserCard())
            {
                MessageBox.Show("打开卡片失败");
                return;
            }
            if (!m_UserCardCtrl.UpdateCardInfo(m_CardInfoPar))
            {
                MessageBox.Show("修改卡片信息失败");
                ReadCardInfo(m_CardInfoPar);
            }
            else
            {
                m_UserCardCtrl.UpdateCardInfoToDb(m_CardInfoPar);
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
            if (!m_UserCardCtrl.SelectCardApp())
                return;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            Buffer.BlockCopy(ASN, 0, m_ASN, 0, 8);
            decimal MoneyLoad = decimal.Parse(textMoney.Text, System.Globalization.NumberStyles.Number);
            double dbMoneyLoad = decimal.ToDouble(MoneyLoad);
            int nRet = m_UserCardCtrl.VerifyUserPin(m_strPIN);
            if (nRet == 1)
            {
                byte[] TerminalId = new byte[6];
                if (APDUBase.ByteDataEquals(TerminalId, m_TermialId))//未读到终端机编号，使用固定编号
                    Buffer.BlockCopy(m_FixedTermialId, 0, TerminalId, 0, 6);
                else
                    Buffer.BlockCopy(m_TermialId, 0, TerminalId, 0, 6);

                double dbBalance = 0.0;
                if (m_UserCardCtrl.UserCardBalance(ref dbBalance))//圈存前读余额
                {
                    if (m_UserCardCtrl.UserCardLoad(m_ASN, TerminalId, (int)(dbMoneyLoad * 100.0)))
                    {
                        //写圈存数据库记录
                        SaveLoadRecord(dbMoneyLoad, dbBalance);
                        string strInfo = string.Format("对卡号{0}圈存{1}元成功", BitConverter.ToString(m_ASN), dbMoneyLoad.ToString("F2"));
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

        //读余额
        private void btnBalance_Click(object sender, EventArgs e)
        {
            if (!OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp())
                return;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            Buffer.BlockCopy(ASN, 0, m_ASN, 0, 8);
            int nRet = m_UserCardCtrl.VerifyUserPin(m_strPIN);
            if (nRet == 1)
            {
                double dbBalance = 0.0;
                if (m_UserCardCtrl.UserCardBalance(ref dbBalance))
                    textBalance.Text = dbBalance.ToString("F2");
                else
                    textBalance.Text = "0.00";
                m_bGray = false;
                m_bTACUF = false;
                //未灰锁时终端机编号输出为0
                if (m_UserCardCtrl.UserCardGray(ref m_bGray, ref m_bTACUF, m_TermialId))
                {
                    GrayFlag.CheckState = m_bGray ? CheckState.Checked : CheckState.Unchecked;
                    GrayFlag.Checked = m_bGray;
                }
                else
                {
                    GrayFlag.CheckState = CheckState.Indeterminate;
                    GrayFlag.Checked = false;
                }
                if (m_bTACUF)
                    m_UserCardCtrl.ClearTACUF();

                SaveCardMoneyToDb(dbBalance);
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
            //未灰状态不可强制解灰
            if (!m_bGray || !OpenUserCard())
                return;
            if (!m_UserCardCtrl.SelectCardApp())
                return;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            Buffer.BlockCopy(ASN, 0, m_ASN, 0, 8);
            int nRet = m_UserCardCtrl.VerifyUserPin(m_strPIN);
            if (nRet == 1)
            {
                byte[] TerminalId = new byte[6];
                if (APDUBase.ByteDataEquals(TerminalId, m_TermialId))//未读到终端机编号，使用固定编号
                    Buffer.BlockCopy(m_FixedTermialId, 0, TerminalId, 0, 6);
                else
                    Buffer.BlockCopy(m_TermialId, 0, TerminalId, 0, 6);
                const double BusinessMoney = 0.0;//强制联机解灰 0 扣款
                m_UserCardCtrl.UnLockGrayCard(m_ASN, TerminalId, (int)(BusinessMoney * 100.0));
                m_bGray = false;
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

            if (!OpenUserCard() || !m_UserCardCtrl.SelectCardApp())
                return;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            Buffer.BlockCopy(ASN, 0, m_ASN, 0, 8);
            if (m_UserCardCtrl.PINUnLock(m_ASN, textOldPIN.Text))
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
            if (!OpenUserCard() || !m_UserCardCtrl.SelectCardApp())
                return;
            byte[] ASN = m_UserCardCtrl.GetUserCardASN(false);
            if (ASN == null)
            {
                MessageBox.Show("未读到卡号");
                return;
            }
            Buffer.BlockCopy(ASN, 0, m_ASN, 0, 8);
            if (m_UserCardCtrl.PINReset(m_ASN, textNewPIN.Text))
            {
                MessageBox.Show("新PIN码PIN码已装入");
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
            if (!OpenUserCard() || !m_UserCardCtrl.SelectCardApp())
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

        /// <summary>
        /// 圈存记录
        /// </summary>
        /// <param name="dbLoadMoney">圈存金额</param>
        /// <param name="dbBalance">卡余额</param>
        private void SaveLoadRecord(double dbLoadMoney,double dbBalance)
        {
            string strCardId = BitConverter.ToString(m_ASN).Replace("-", "");

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
            UpdateBaseCardMoneyValue(ObjSql, strCardId, "RechargeTotal", dbRechargeTotal);
            UpdateBaseCardMoneyValue(ObjSql, strCardId, "CardBalance", dblTotal);

            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void UpdateBaseCardMoneyValue(SqlHelper ObjSql, string strCardId, string strFieldName, double dbValue)
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
            ObjSql.ExecuteCommand("select " + strFieldName + " form Base_Card where CardNum=@CardId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    dbValue = (double)dataReader[strFieldName];
                }
                dataReader.Close();
            }
            return dbValue;
        }

        private void SaveCardMoneyToDb(double dbBalance)
        {
            string strCardId = BitConverter.ToString(m_ASN).Replace("-", "");

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            UpdateBaseCardMoneyValue(ObjSql, strCardId, "CardBalance", dbBalance);

            ObjSql.CloseConnection();
            ObjSql = null;
        }
    }
}
