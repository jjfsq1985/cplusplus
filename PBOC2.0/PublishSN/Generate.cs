using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PublishSN
{
    public partial class Generate : Form
    {
        //申请码处理逻辑：先用AuthKey将物理地址的信息加密，再用LicenseKey对其进行解密得到申请码
        //注册码处理逻辑：先用AuthKey将申请码解密，再用LicenseKey对其进行加密得到注册码
        public static readonly byte[] LicenseKey = { 0x6D, 0xC5, 0xB9, 0x49, 0xFC, 0xDD, 0x44, 0xCD, 0xB9, 0x35, 0x64, 0xA1, 0x83, 0x92, 0x83, 0xF8 };
        public static readonly byte[] AuthKey = { 0x50, 0xA5, 0xAA, 0x9B, 0xD3, 0x89, 0x4C, 0xBB, 0x8F, 0x3F, 0x23, 0x14, 0xCD, 0x34, 0xDF, 0x84 };

        public Generate()
        {
            InitializeComponent();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textCode.Text) || textCode.Text.Length != 32)
            {
                MessageBox.Show("申请码不正确");
                return;
            }
            string strAuthCode = textCode.Text.ToUpper();
            bool bOk = true;
            for (int i = 0; i < strAuthCode.Length; i++)
            {   
                if (Char.IsDigit(strAuthCode[i]))
                {
                    continue;
                }
                else if (strAuthCode[i] >= 'A' && strAuthCode[i] <= 'F')
                {
                    continue;
                }
                else
                {
                    bOk = false;
                    break;
                }
            }

            if (!bOk)
            {
                MessageBox.Show("申请码不正确");
                return;
            }

            string strLicense = CalcLicense(strAuthCode);

            textLicense.Text = strLicense;
        }


        private string CalcLicense(string strSrcCode)
        {
            byte[] parseCode = new byte[16];
            if(strSrcCode.Length != 32)
            {
                return BitConverter.ToString(parseCode).Replace("-","");
            }
            for (int i = 0; i < 16; i++)
            {
                parseCode[i] = Convert.ToByte(strSrcCode.Substring(i * 2, 2), 16);
            }

            byte[] TempData = DesCrypt.TripleDecryptData(parseCode, AuthKey);
            byte[] EncryptData = DesCrypt.TripleEncryptData(TempData, LicenseKey);
            return BitConverter.ToString(EncryptData).Replace("-","");
        }

    }
}
