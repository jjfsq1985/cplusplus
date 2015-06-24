using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;

namespace PublishCardOperator.Dialog
{
    public partial class AddOrgKey : Form
    {
        private OrgKeyValue m_OrgKey = new OrgKeyValue();
        public AddOrgKey()
        {
            InitializeComponent();
            cmbType.SelectedIndex = 0;
            IsValid.Checked = false;
        }

        public OrgKeyValue GetOrgKeyValue()
        {
            return m_OrgKey;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textOrgKey.Text) || textOrgKey.Text.Length  != 32)
            {
                MessageBox.Show("请输入长度为32的密钥值");
                return;
            }
            if (string.IsNullOrEmpty(textKeyDetail.Text))
            {
                MessageBox.Show("请输入密钥描述");
                return;
            }
            byte[] key = PublicFunc.StringToBCD(textOrgKey.Text);
            if(key.Length == 16)
                Buffer.BlockCopy(key,0,m_OrgKey.OrgKey,0,16);
            m_OrgKey.nKeyType = cmbType.SelectedIndex;
            m_OrgKey.KeyDetail = textKeyDetail.Text;
            m_OrgKey.bValid = IsValid.Checked;
            DialogResult = DialogResult.OK;
        }
    }
}