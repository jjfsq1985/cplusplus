using System;
using System.Collections.Generic;
using System.Windows.Forms;

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

            LoginMain logonMain = new LoginMain();            
            if (logonMain.ShowDialog() == DialogResult.OK)
                Application.Run(new Main(logonMain.UserLoginId, logonMain.GetDbInfo()));
            else
                return;            
        }
    }
}