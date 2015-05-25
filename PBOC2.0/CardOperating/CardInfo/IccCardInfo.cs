using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SqlServerHelper;
using System.Data.SqlClient;

namespace CardOperating
{
    public partial class IccCardInfo : Form
    {
        private IccCardInfoParam m_IccCardInfoPar = new IccCardInfoParam();
        private const Char Backspace = (Char)8;
        private List<ClientInfo> m_ListClientInfo = new List<ClientInfo>();

        public IccCardInfo()
        {
            InitializeComponent();
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

        private void ReadInfoFromDb()
        {
            cmbClientName.Items.Clear();
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection("(local)", "FunnettStation", "sa", "sasoft"))
            {
                ObjSql = null;
                return;
            }
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
                        cmbClientName.Items.Add(info.strClientName);
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
        }


        private void InitData()
        {
            if(cmbClientName.Items.Count > 0)
                cmbClientName.SelectedIndex = GetClientIdIndex(m_IccCardInfoPar.ClientID);
            textPSAMNo.Text = m_IccCardInfoPar.PSAMCardID;
            textTermialID.Text = m_IccCardInfoPar.TermialID;
            textCompanyFrom.Text = m_IccCardInfoPar.CompanyIssue;
            textCompanyTo.Text = m_IccCardInfoPar.CompanyRecv;
            AppValidDateFrom.Value = m_IccCardInfoPar.ValidAppForm;
            AppValidDateTo.Value = m_IccCardInfoPar.ValidAppTo;
            textRemark.Text = m_IccCardInfoPar.Remark;
        }

        public IccCardInfoParam GetPSAMCardParam()
        {
            return m_IccCardInfoPar;
        }

        private void SaveClose_Click(object sender, EventArgs e)
        {
            if (cmbClientName.SelectedIndex >= 0 && cmbClientName.SelectedIndex < m_ListClientInfo.Count)
                m_IccCardInfoPar.ClientID = m_ListClientInfo[cmbClientName.SelectedIndex].ClientId;
            m_IccCardInfoPar.PSAMCardID = textPSAMNo.Text;
            m_IccCardInfoPar.TermialID = textTermialID.Text;
            if (AppValidDateFrom.Value < AppValidDateTo.Value)
            {
                m_IccCardInfoPar.ValidAppForm = AppValidDateFrom.Value;
                m_IccCardInfoPar.ValidAppTo = AppValidDateTo.Value;
            }
            m_IccCardInfoPar.CompanyIssue = textCompanyFrom.Text;
            m_IccCardInfoPar.CompanyRecv = textCompanyTo.Text;
            m_IccCardInfoPar.Remark = textRemark.Text;
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

        private void IccCardInfo_Load(object sender, EventArgs e)
        {
            ReadInfoFromDb();
            InitData();
        }
 
    }
}