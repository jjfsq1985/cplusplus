using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PublishCardOperator.Dialog
{
    public partial class AddCpuKey : Form
    {
        private CpuKeyValue m_CpuKey = new CpuKeyValue();

        private int m_nMaxAppCount = 0;
        public AddCpuKey()
        {
            InitializeComponent();
            m_CpuKey.bValid = false;
        }        

        public CpuKeyValue GetCpuKeyValue()
        {
            return m_CpuKey;
        }

        public void SetMaxAppCount(int nCount)
        {
            m_nMaxAppCount = nCount;
        }

        private bool FillKeyValue(string strText, byte[] keyData, string strKeyName)
        {
            if (string.IsNullOrEmpty(strText) || strText.Length != 32)
            {
                string strMessage = string.Format("请输入长度为32的{0}值", strKeyName);
                MessageBox.Show(strMessage);
                return false;
            }
            byte[] key = PublishCard.StringToBCD(strText);
            if (key.Length == 16)
                Buffer.BlockCopy(key, 0, keyData, 0, 16);
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!FillKeyValue(textAppMasterKey.Text, m_CpuKey.MasterKey, "卡片主控密钥"))
                return;
            if (!FillKeyValue(textTendingKey.Text, m_CpuKey.MasterTendingKey, "卡片维护密钥"))
                return;
            if (!FillKeyValue(textAuthKey.Text, m_CpuKey.InternalAuthKey, "内部认证密钥"))
                return;
            if (string.IsNullOrEmpty(textKeyDetail.Text))
            {
                MessageBox.Show("请输入密钥描述");
                return;
            }
            m_CpuKey.KeyDetail = textKeyDetail.Text;
            m_CpuKey.bValid = IsValid.Checked;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }  

        private void btnAddKey_Click(object sender, EventArgs e)
        {
            int nCount = m_CpuKey.LstAppKeyGroup.Count;
            if (nCount >= m_nMaxAppCount)
            {
                string strMsg = string.Format("卡片最大支持{0}个应用密钥,不能再增加。", m_nMaxAppCount);
                MessageBox.Show(strMsg);
                return;
            }
            InsertAppKey InsertForm = new InsertAppKey();
            if (InsertForm.ShowDialog(this) != DialogResult.OK)
                return;
            AppKeyValueGroup newAppKey = InsertForm.GetAppKeyValue();
            newAppKey.eDbFlag = DbStateFlag.eDbAdd;
            newAppKey.AppIndex = nCount + 1;
            m_CpuKey.LstAppKeyGroup.Add(newAppKey);
            ListViewItem item = new ListViewItem();
            item.Text = newAppKey.AppIndex.ToString();            
            item.SubItems.Add(BitConverter.ToString(newAppKey.AppMasterKey).Replace("-", ""));
            item.SubItems.Add(BitConverter.ToString(newAppKey.AppTendingKey).Replace("-", ""));
            item.SubItems.Add(BitConverter.ToString(newAppKey.AppInternalAuthKey).Replace("-", ""));
            item.SubItems.Add(BitConverter.ToString(newAppKey.PINResetKey).Replace("-", ""));
            item.SubItems.Add(BitConverter.ToString(newAppKey.PINUnlockKey).Replace("-", ""));
            item.SubItems.Add(BitConverter.ToString(newAppKey.ConsumerMasterKey).Replace("-", ""));
            item.SubItems.Add(BitConverter.ToString(newAppKey.LoadMasterKey).Replace("-", ""));
            item.SubItems.Add(BitConverter.ToString(newAppKey.TacMasterKey).Replace("-", ""));
            item.SubItems.Add(BitConverter.ToString(newAppKey.UnlockUnloadKey).Replace("-", ""));
            item.SubItems.Add(BitConverter.ToString(newAppKey.OverdraftKey).Replace("-", ""));            
            listAppKey.Items.Add(item);
        }

        private void btnDelKey_Click(object sender, EventArgs e)
        {
            int nCount = m_CpuKey.LstAppKeyGroup.Count;
            if (nCount <= 0)
                return;
            listAppKey.Items.RemoveAt(nCount - 1);
            m_CpuKey.LstAppKeyGroup.RemoveAt(nCount - 1);
        }
    }
}
