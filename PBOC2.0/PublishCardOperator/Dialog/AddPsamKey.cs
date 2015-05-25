using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PublishCardOperator.Dialog
{
    public partial class AddPsamKey : Form
    {
        private PsamKeyValue m_PsamKey = new PsamKeyValue();
        public AddPsamKey()
        {
            InitializeComponent();
            IsValid.Checked = false;
        }

        public PsamKeyValue GetPsamKeyValue()
        {
            return m_PsamKey;
        }

        private bool FillPsamKeyValue(string strText, byte[] keyData, string strKeyName)
        {
            if (string.IsNullOrEmpty(strText) || strText.Length  != 32)
            {
                string strMessage = string.Format("请输入长度为32的{0}值",strKeyName);
                MessageBox.Show(strMessage);
                return false;
            }
            byte[] key = PublishCard.StringToBCD(strText);
            if(key.Length == 16)
                Buffer.BlockCopy(key,0,keyData,0,16);
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if(!FillPsamKeyValue(textMasterKey.Text,m_PsamKey.MasterKey,"卡片主控密钥"))
                return;
            if (!FillPsamKeyValue(textMasterTendingKey.Text, m_PsamKey.MasterTendingKey, "卡片维护密钥"))
                return;
            if (!FillPsamKeyValue(textAppMasterKey.Text, m_PsamKey.AppMasterKey, "应用主控密钥"))
                return;
            if (!FillPsamKeyValue(textAppTendingKey.Text, m_PsamKey.AppTendingKey, "应用维护密钥"))
                return;
            if (!FillPsamKeyValue(textConsumerMasterKey.Text, m_PsamKey.ConsumerMasterKey, "消费主密钥"))
                return;
            if (!FillPsamKeyValue(textGrayLockKey.Text, m_PsamKey.GrayCardKey, "灰锁密钥"))
                return;
            if (!FillPsamKeyValue(textMACEncryptKey.Text, m_PsamKey.MacEncryptKey, "MAC加密密钥"))
                return;
            if (string.IsNullOrEmpty(textKeyDetail.Text))
            {
                MessageBox.Show("请输入密钥描述");
                return;
            }
            m_PsamKey.KeyDetail = textKeyDetail.Text;
            m_PsamKey.bValid = IsValid.Checked;            
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
