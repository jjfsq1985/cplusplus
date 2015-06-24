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
        public InsertAppKey()
        {
            InitializeComponent();
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
            if (!FillAppKeyValue(textAppLoadKey.Text, m_AppKeyVal.LoadMasterKey, "圈存密钥"))
                return;
            if (!FillAppKeyValue(textTacKey.Text, m_AppKeyVal.TacMasterKey, "TAC密钥"))
                return;
            if (!FillAppKeyValue(textUnlockUnloadKey.Text, m_AppKeyVal.UnlockUnloadKey, "联机解扣密钥"))
                return;
            if (!FillAppKeyValue(textOverdraftKey.Text, m_AppKeyVal.OverdraftKey, "修改透支限额密钥"))
                return;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
