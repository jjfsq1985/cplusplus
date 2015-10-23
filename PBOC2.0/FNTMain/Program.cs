using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IFuncPlugin;

namespace FNTMain
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //检查注册码
            string strPhysicalCode = PublicFunc.GetPhysicalAddress();
            if (string.IsNullOrEmpty(strPhysicalCode))
            {
                MessageBox.Show("获取硬件信息失败，请检查网卡是否正确安装。","错误");
                return;
            }
            string strSrcCode = LicenseCalc.CalcSrcCode(strPhysicalCode);
            if (!CompareCodeInReg(strSrcCode))
            {
                //请注册
                LicenseForm lic = new LicenseForm(strSrcCode);
                if (lic.ShowDialog() == DialogResult.Cancel)
                    return;
            }

            LoginMain logonMain = new LoginMain();            
            if (logonMain.ShowDialog() == DialogResult.OK)
                Application.Run(new Main(logonMain.UserLoginId, logonMain.GetDbInfo()));
            else
                return;            
        }

        static bool CompareCodeInReg(string strCode)
        {
            string strRegCode = LicenseCalc.GetSN();
            if (string.IsNullOrEmpty(strRegCode) || string.IsNullOrEmpty(strCode))
                return false;
            if (!LicenseCalc.LicenseVerify(strCode, strRegCode))
            {
                MessageBox.Show("注册码不正确，请重新注册。","注册码验证");
                return false;
            }
            else
            {
                return true;
            }            
        }
    }
}