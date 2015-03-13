using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CardOperating
{
    public partial class UserCardInfo : Form
    {
        private UserCardInfoParam m_CardInfoPar = new UserCardInfoParam();
        private const Char Value_Dot = (Char)46;
        private const Char Backspace = (Char)8;
        private const Char Key_A = (Char)65;
        private const Char Key_B = (Char)66;
        private const Char Key_C = (Char)67;
        private const Char Key_D = (Char)68;
        private const Char Key_E = (Char)69;
        private const Char Key_F = (Char)70;
        private const Char Key_X = (Char)88;
        public UserCardInfo()
        {
            InitializeComponent();

            InitData();
        }

        private void InitData()
        {
            textCompanyId.Text = m_CardInfoPar.CompanyID;
            textUserCardId.Text = m_CardInfoPar.CardOrderNo;
            DateFrom.Value = m_CardInfoPar.ValidCardBegin;
            DateTo.Value = m_CardInfoPar.ValidCardEnd;
            cmbUserCardType.SelectedIndex = GetCardTypeIndex(m_CardInfoPar.UserCardType);
            CustomPassword.Checked = m_CardInfoPar.DefaultPwdFlag ? false : true;
            textPassword.Enabled = m_CardInfoPar.DefaultPwdFlag ? false : true;
            textPassword.Text = m_CardInfoPar.CustomPassword;
            textUserName.Text = m_CardInfoPar.UserName;
            textPriceLevel.Text = m_CardInfoPar.PriceLevel.ToString();
            textUserIdentity.Text = m_CardInfoPar.UserIdentity;
            textUserAccount.Text = m_CardInfoPar.UserAccount;
            double dbRate = m_CardInfoPar.DiscountRate * 1.0 / 100.0;
            textDiscountRate.Text = dbRate.ToString("F2");
            DiscountRateExprieValid.Value = m_CardInfoPar.DiscountRateEnd;
            textGasType.Text = m_CardInfoPar.LimitGasType.ToString("X4");            
            textCarNo.Text = m_CardInfoPar.LimtCarNo;
            cmbAreaLimit.SelectedIndex = GetAreaLimitIndex(m_CardInfoPar.LimitArea);
            textLimitAreaCode.Text = m_CardInfoPar.LimitAreaCode;
            if (m_CardInfoPar.LimitFixDepartment != 0xFFFFFFFF)
            {
                textFixDepartment.Text = m_CardInfoPar.LimitFixDepartment.ToString("X8");
            }

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
            BoalExprieValid.Value = m_CardInfoPar.BoalExprie;
            textBoalFactoryNo.Text = m_CardInfoPar.BoalFactoryID;
        }

        public UserCardInfoParam GetUserCardParam()
        {
            return m_CardInfoPar;
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

        private void SaveClose_Click(object sender, EventArgs e)
        {
            //保存数据
            m_CardInfoPar.setCardBaseInfo(textCompanyId.Text, GetCardType(cmbUserCardType.SelectedIndex), textUserCardId.Text);
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
                m_CardInfoPar.setDiscountRate(dbRate,DiscountRateExprieValid.Value);                
            }

            if (string.IsNullOrEmpty(textPriceLevel.Text))
            {
                m_CardInfoPar.PriceLevel = Convert.ToByte(textPriceLevel.Text, 10);
            }

            m_CardInfoPar.UserIdentity = textUserIdentity.Text;
            m_CardInfoPar.UserAccount = textUserAccount.Text;

            if (string.IsNullOrEmpty(textGasType.Text))
            {
                m_CardInfoPar.LimitGasType = Convert.ToUInt16(textGasType.Text, 16);
            }

            m_CardInfoPar.LimtCarNo = textCarNo.Text;

            m_CardInfoPar.setLimitArea(GetAreaLimit(cmbAreaLimit.SelectedIndex),textLimitAreaCode.Text);
            
            if (!string.IsNullOrEmpty(textFixDepartment.Text))
            {
                m_CardInfoPar.LimitFixDepartment = Convert.ToUInt32(textFixDepartment.Text, 16);
            }

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
            m_CardInfoPar.BoalExprie = BoalExprieValid.Value;
            m_CardInfoPar.BoalFactoryID = textBoalFactoryNo.Text;
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
    }
}