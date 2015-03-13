using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CardOperating
{
    public partial class IccCardInfo : Form
    {
        private IccCardInfoParam m_IccCardInfoPar = new IccCardInfoParam();
        private const Char Backspace = (Char)8;
        public IccCardInfo()
        {
            InitializeComponent();

            InitData();
        }

        private void InitData()
        {
            textPSAMNo.Text = m_IccCardInfoPar.PSAMCardID;
            textTermialID.Text = m_IccCardInfoPar.TermialID;
            textCompanyFrom.Text = m_IccCardInfoPar.CompanyIssue;
            textCompanyTo.Text = m_IccCardInfoPar.CompanyRecv;
            AppValidDateFrom.Value = m_IccCardInfoPar.ValidAppForm;
            AppValidDateTo.Value = m_IccCardInfoPar.ValidAppTo;
        }

        public IccCardInfoParam GetPSAMCardParam()
        {
            return m_IccCardInfoPar;
        }

        private void SaveClose_Click(object sender, EventArgs e)
        {
            m_IccCardInfoPar.PSAMCardID = textPSAMNo.Text;
            m_IccCardInfoPar.TermialID = textTermialID.Text;
            if (AppValidDateFrom.Value < AppValidDateTo.Value)
            {
                m_IccCardInfoPar.ValidAppForm = AppValidDateFrom.Value;
                m_IccCardInfoPar.ValidAppTo = AppValidDateTo.Value;
            }
            m_IccCardInfoPar.CompanyIssue = textCompanyFrom.Text;
            m_IccCardInfoPar.CompanyRecv = textCompanyTo.Text;
        }

        private void textPSAMNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }

        private void textTermialID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }
 
    }
}