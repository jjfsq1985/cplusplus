using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ApduLoh
{
    //USB的Duali DE-620读卡器，SmartCard Reader模式
    public class DllExportDuali
    {
        //nPort为100, nBaud忽略
        [DllImport("DualCardDll.dll", EntryPoint = "DE_InitPort", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int DE_InitPort(int nPort, int nBaud);

        //断开读卡器
        [DllImport("DualCardDll.dll", EntryPoint = "DE_ClosePort", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int DE_ClosePort(int nPort);

        //设置读卡器模式为PC/SC
        //mode 1 RF, 2 RF + Contact, 3 RF+Contact+SAM，0 SmartCard Reader
        //<Inqflag> 1 设置, 0 查询 (为0时,mode输出)
        [DllImport("DualCardDll.dll", EntryPoint = "DE_ChangeDevice", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int DE_ChangeDevice(int nPort, ref byte mode, int Inqflag);        

    }
}
