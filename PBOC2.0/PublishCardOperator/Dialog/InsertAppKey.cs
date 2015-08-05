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
    public partial class InsertAppKey : Form
    {
        private AppKeyValueGroup m_AppKeyVal = new AppKeyValueGroup();

        private byte[] m_RelatedConsumerKey = new byte[16];


        public InsertAppKey(byte[] RelatedKey)
        {
            InitializeComponent();

            Buffer.BlockCopy(RelatedKey, 0, m_RelatedConsumerKey, 0, 16);
        }

        public AppKeyValueGroup GetAppKeyValue()
        {
            return m_AppKeyVal;
        }

        private bool FillAppKeyValue(string strText, byte[] keyData, string strKeyName)
        {
            if (string.IsNullOrEmpty(strText) || strText.Length != 32)
            {
                string strMessage = string.Format("请输入长度为32的{0}值", strKeyName);
                MessageBox.Show(strMessage);
                return false;
            }
            byte[] key = PublicFunc.StringToBCD(strText);
            if (key.Length == 16)
                Buffer.BlockCopy(key, 0, keyData, 0, 16);
            return true;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {

            if (!FillAppKeyValue(textAMKey.Text, m_AppKeyVal.AppMasterKey, "应用主控密钥"))
                return;
            if (!FillAppKeyValue(textAMTendingKey.Text, m_AppKeyVal.AppTendingKey, "应用维护密钥"))
                return;
            if (!FillAppKeyValue(textAIAuthKey.Text, m_AppKeyVal.AppInternalAuthKey, "应用内部认证密钥"))
                return;
            if (!FillAppKeyValue(textPinResetKey.Text, m_AppKeyVal.PINResetKey, "PIN密码重装密钥"))
                return;
            if (!FillAppKeyValue(textPinUnlockKey.Text, m_AppKeyVal.PINUnlockKey, "PIN解锁密钥"))
                return;
            if (!FillAppKeyValue(textCMKey.Text, m_AppKeyVal.ConsumerMasterKey, "消费主密钥"))
                return;
            if (!FillAppKeyValue(textAppLoadKey.Text, m_AppKeyVal.LoadKey, "圈存密钥"))
                return;
            if (!FillAppKeyValue(textTacKey.Text, m_AppKeyVal.TacMasterKey, "TAC密钥"))
                return;
            if (!FillAppKeyValue(textUnGrayKey.Text, m_AppKeyVal.UnGrayKey, "联机解扣密钥"))
                return;
            if (!FillAppKeyValue(textAppLoadKey.Text, m_AppKeyVal.UnLoadKey, "圈提密钥"))
                return;
            if (!FillAppKeyValue(textOverdraftKey.Text, m_AppKeyVal.OverdraftKey, "修改透支限额密钥"))
                return;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否使用与PSAM卡一致的消费密钥？", "生成",MessageBoxButtons.YesNo) == DialogResult.Yes)
                InitKey(false);
            else
                InitKey(true);
        }

        private void InitKey(bool bNewConsumerKey)
        {
            Guid temp = Guid.Empty;
            string strKey = "";

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textAMKey.Text = strKey;

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textAMTendingKey.Text = strKey;

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textAMTendingKey.Text = strKey;

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textAIAuthKey.Text = strKey;

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textPinResetKey.Text = strKey;

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textPinUnlockKey.Text = strKey;

            if (bNewConsumerKey)
            {
                temp = Guid.NewGuid();
                strKey = temp.ToString().Replace("-", "");
                textCMKey.Text = strKey;
            }
            else
            {
                textCMKey.Text = BitConverter.ToString(m_RelatedConsumerKey).Replace("-","");
            }

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textAppLoadKey.Text = strKey;

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textTacKey.Text = strKey;

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textUnGrayKey.Text = strKey;

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textAppUnLoadKey.Text = strKey;

            temp = Guid.NewGuid();
            strKey = temp.ToString().Replace("-", "");
            textOverdraftKey.Text = strKey;
        }

    }
}
