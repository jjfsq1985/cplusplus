using System;
using System.Collections.Generic;
using System.Text;

namespace CardOperating
{
    //卡中的加气交易明细
    public class CardRecord
    {
        public int BusinessSn;  //交易序号
        public double OverdraftMoney;  //透支限额
        public double Amount;          //交易金额
        public byte BusinessType;    //交易标识
        public string TerminalID;  //终端机编号
        public string BusinessTime;   //交易时间
    }

    //用户卡数据结构
    public class UserCardInfoParam
    {
        public const byte CardGroup = 0x02; //卡种类：CPU卡

        private int m_nClientId;
        public int ClientID
        {
            get { return m_nClientId; }
            set { m_nClientId = value; }
        }

        public enum CardType
        {
            PersonalCard = 0x01, //个人卡
            ManagerCard = 0x02,   //管理卡
            EmployeeCard = 0x04,  //员工卡
            ServiceCard = 0x06,  //维修卡
            CompanySubCard = 0x11,  //单位子卡
            CompanyMotherCard = 0x21 //单位母卡
        }

        private string m_strCardId;

        private string m_strCompanyId;  //发卡网点公司ID
        public string CompanyID
        {
            get { return m_strCompanyId; }
            set { m_strCompanyId = value; }
        }

        private CardType m_eCardType; //卡类型
        public CardType UserCardType
        {
            get { return m_eCardType; }
            set { m_eCardType = value; }
        }

        private string m_strCardIndex;  //卡流水号
        public string CardOrderNo
        {
            get { return m_strCardIndex; }
            set { m_strCardIndex = value; }
        }

        private DateTime m_CardValidFrom;
        public DateTime ValidCardBegin
        {
            get { return m_CardValidFrom; }
            set { m_CardValidFrom = value; }
        }

        private DateTime m_CardValidTo;
        public DateTime ValidCardEnd
        {
            get { return m_CardValidTo; }
            set { m_CardValidTo = value; }
        }

        private bool m_bDefaultPwd;
        public bool DefaultPwdFlag
        {
            get { return m_bDefaultPwd; }
            set { m_bDefaultPwd = value; }
        }

        private string m_strCustomPassword; //自定义密码
        public string CustomPassword
        {
            get { return m_strCustomPassword; }
            set { m_strCustomPassword = value; }
        }

        private string m_UserName;//持卡人姓名 
        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }

        private string m_strTelephone;           //持卡人电话
        public string TelePhone
        {
            get { return m_strTelephone; }
            set { m_strTelephone = value; }

        }

        private string m_UserIdentity; //身份证号   
        public string UserIdentity
        {
            get { return m_UserIdentity; }
            set { m_UserIdentity = value; }
        }

        private int m_DiscountRate;//折扣率
        public int DiscountRate
        {
            get { return m_DiscountRate; }            
        }

        private DateTime m_dtDiscountRateEnd; //折扣有效期
        public DateTime DiscountRateEnd
        {
            get { return m_dtDiscountRateEnd; }            
        }

        private byte m_nPriceLevel;   //价格等级
        public byte PriceLevel
        {
            get { return m_nPriceLevel; }
            set 
            {
                if (value >= 1 && value <= 20)
                    m_nPriceLevel = value;
                else
                    m_nPriceLevel = 1; 
            }
        }

        private string m_UserAccount; //账号
        public string UserAccount
        {
            get { return m_UserAccount; }            
        }

        private ushort m_LimitGasType;   //限制加气类型
        public ushort LimitGasType
        {
            get { return m_LimitGasType; }
            set { m_LimitGasType = value; }
        }

        private bool m_bLimitCarNo;          //限制车牌号
        public bool LimitCarNo
        {
            get { return m_bLimitCarNo; }
            set { m_bLimitCarNo = value; }
        }

        private string m_strCarCategory;    //车类别
        public string CarType
        {
            get { return m_strCarCategory; }
            set { m_strCarCategory = value; }

        }

        private string m_strCarNo;              //车牌号
        public string CarNo
        {
            get { return m_strCarNo; }
            set { m_strCarNo = value; }
        }

        private string m_strSelfId;         //自编号
        public string SelfId
        {
            get { return m_strSelfId; }
            set { m_strSelfId = value; }
        }

        private byte m_LimitArea;               //限制地域
        public byte LimitArea
        {
            get { return m_LimitArea; }            
        }

        private string m_LimitAreaCode;         //限制地域代码
        public string LimitAreaCode
        {
            get { return m_LimitAreaCode; }
        }

        private uint m_LimitFixDepartment;      //限制定点单位代码
        public uint LimitFixDepartment
        {
            get { return m_LimitFixDepartment; }
            set { m_LimitFixDepartment = value; }
        }

        private byte m_LimitGasFillCount;       //限制加气次数
        public byte LimitGasFillCount
        {
            get { return m_LimitGasFillCount; }
            set
            {
                if (value > 0 && value < 10)
                    m_LimitGasFillCount = value; 
                else
                    m_LimitGasFillCount = 0xFF; 
            }
        }

        private uint m_nLimitGasFillAmount;         //限制加气金额(单位：分)
        public uint LimitGasFillAmount
        {
            get { return m_nLimitGasFillAmount; }
            set { m_nLimitGasFillAmount = value; }//大于0，小于100,000,000
        }

        private string m_strBoalId;             //钢瓶编号
        public string BoalId
        {
            get { return m_strBoalId; }
            set { m_strBoalId = value; }
        }

        private int m_nBoalCount;    //钢瓶数量
        public int CylinderNum
        {
            get { return m_nBoalCount; }
            set { m_nBoalCount = value; }
        }

        private ushort m_nBoalVolume;     //钢瓶容积
        public ushort CylinderVolume
        {
            get { return m_nBoalVolume; }
            set { m_nBoalVolume = value; }
        }

        private DateTime m_dtBoalExprie;  //钢瓶有效期
        public DateTime BoalExprie
        {
            get { return m_dtBoalExprie; }
            set { m_dtBoalExprie = value; }
        }

        private string m_strBoalFactoryID; //钢瓶厂家编号
        public string BoalFactoryID
        {
            get { return m_strBoalFactoryID; }
            set { m_strBoalFactoryID = value; }
        }

        private string m_strBusDistance;  //公交路数
        public string BusDistance
        {
            get { return m_strBusDistance; }
            set { m_strBusDistance = value; }
        }

        private string m_strRemark;
        public string Remark
        {
            get { return m_strRemark; }
            set { m_strRemark = value; }
        }


        public UserCardInfoParam()
        {
            m_nClientId = 1;
            m_strCardId = "";
            m_strCompanyId = "0001";
            m_eCardType = CardType.PersonalCard;
            m_strCardIndex = "000001";
            m_CardValidFrom = new DateTime(2010, 1, 1);
            m_CardValidTo = new DateTime(2020, 12, 31);
            m_bDefaultPwd = true;
            m_strCustomPassword = "999999";
            m_UserName = "";
            m_strTelephone = "";
            m_UserIdentity = "";
            m_DiscountRate = 0;
            m_dtDiscountRateEnd = new DateTime(2010, 1, 1);
            m_nPriceLevel = 1;
            m_UserAccount = "";
            m_LimitGasType = 0xFFFF;
            m_bLimitCarNo = false;
            m_strCarCategory = "不限";
            m_strCarNo = "";
            m_strSelfId = "";
            m_LimitArea = 0xFF;
            m_LimitAreaCode = "";
            m_LimitFixDepartment = 0xFFFFFFFF;
            m_LimitGasFillCount = 0xFF;
            m_nLimitGasFillAmount = 0xFFFFFFFF;
            m_strBoalId = "";
            m_nBoalCount = 2;
            m_nBoalVolume = 375;
            m_dtBoalExprie = new DateTime(2020, 12, 31);
            m_strBoalFactoryID = "";
            m_strBusDistance = "";
            m_strRemark = "";
        }

        public void SetCardId(string strCompanyId)
        {
            if (strCompanyId.Length <= 0 || strCompanyId.Length > 4)
                return;            
            m_strCompanyId = strCompanyId;
            byte nCardType = (byte)m_eCardType;
            m_strCardId = m_strCompanyId + "02" + nCardType.ToString("X2") + "00" + m_strCardIndex;
            m_UserAccount = m_strCardId;//客户账号，与序列号相同
        }

        public void setLimitArea(byte LimitArea, string strLimitAreaCode )
        {
            if (LimitArea > 0 && LimitArea < 5 && strLimitAreaCode.Length %2 == 0)
            {
                m_LimitArea = LimitArea;
                m_LimitAreaCode = strLimitAreaCode;
            }
            else
            {
                m_LimitArea = 0xFF;
                m_LimitAreaCode = "";
            }            
        }

        public void setDiscountRate(double dbDiscountRate, DateTime dtDiscountRateEnd)
        {
            if (dbDiscountRate < 1 || dbDiscountRate > 99)
                m_DiscountRate = 0;
            else                
                m_DiscountRate = (int)(dbDiscountRate * 100);
            m_dtDiscountRateEnd = dtDiscountRateEnd;
        }

        public byte[] GetUserCardID()
        {
            int nLen = m_strCardId.Length;
            if (nLen != 16)
                return null;
            int nByteSize = nLen / 2;
            byte[] byteCardId = new byte[nByteSize];

            for (int i = 0; i < nByteSize; i++)
            {
                byteCardId[i] = Convert.ToByte(m_strCardId.Substring(i * 2, 2), 16);
            }
            return byteCardId;
        }

        public byte GetByteCarType()
        {
            byte byteRet = 0xFF;
            switch (m_strCarCategory)
            {
                case "不限":
                    byteRet = 0xFF;
                    break;
                case "私家车":
                    byteRet = 0x01;
                    break;
                case "单位车":
                    byteRet = 0x02;
                    break;
                case "出租车":
                    byteRet = 0x03;
                    break;
                case "公交车":
                    byteRet = 0x04;
                    break;
            }
            return byteRet;
        }
    }
}
