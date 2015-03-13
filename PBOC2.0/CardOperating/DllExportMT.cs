using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CardOperating
{
    public class DllExportMT
    {
        //说明：打开通讯接口
        [DllImport("mt_32.dll", EntryPoint = "open_device", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern int open_device(byte nPort, long ulBaud);

        //说明：关闭通讯口
        [DllImport("mt_32.dll", EntryPoint = "close_device", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short close_device(int icdev);

        [DllImport("mt_32.dll", EntryPoint = "hex_asc", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short hex_asc([MarshalAs(UnmanagedType.LPArray)]byte[] sHex, [MarshalAs(UnmanagedType.LPArray)]byte[] sAsc, uint ulLength);

        [DllImport("mt_32.dll", EntryPoint = "asc_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short asc_hex([MarshalAs(UnmanagedType.LPArray)]byte[] sAsc, [MarshalAs(UnmanagedType.LPArray)]byte[] sHex, uint ulLength);

        [DllImport("mt_32.dll", EntryPoint = "ICC_Reset", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short ICC_Reset(int icdev, byte nCardSet, [MarshalAs(UnmanagedType.LPArray)]byte[] sAtr, [MarshalAs(UnmanagedType.LPArray)]byte[] nAtrLen);

        [DllImport("mt_32.dll", EntryPoint = "ICC_PowerOn", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short ICC_PowerOn(int icdev, byte nCardSet, [MarshalAs(UnmanagedType.LPArray)]byte[] sAtr, [MarshalAs(UnmanagedType.LPArray)]byte[] nAtrLen);

        [DllImport("mt_32.dll", EntryPoint = "ICC_CommandExchange", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short ICC_CommandExchange(int icdev, byte nCardSet, [MarshalAs(UnmanagedType.LPArray)]byte[] sCmd, short nCmdLen, [MarshalAs(UnmanagedType.LPArray)]byte[] sResp, [MarshalAs(UnmanagedType.LPArray)]byte[] nRespLen);

        [DllImport("mt_32.dll", EntryPoint = "ICC_PowerOff", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short ICC_PowerOff(int icdev, byte nCardSet);

        [DllImport("mt_32.dll", EntryPoint = "OpenCard", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short OpenCard(int icdev, byte mode, [MarshalAs(UnmanagedType.LPArray)]byte[] sAtr, [MarshalAs(UnmanagedType.LPArray)]byte[] snr, [MarshalAs(UnmanagedType.LPArray)]byte[] nAtrLen);

        [DllImport("mt_32.dll", EntryPoint = "ExchangePro", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short ExchangePro(int icdev, [MarshalAs(UnmanagedType.LPArray)]byte[] sAtr, short len, [MarshalAs(UnmanagedType.LPArray)]byte[] Rec, [MarshalAs(UnmanagedType.LPArray)]byte[] nAtrLen);

        [DllImport("mt_32.dll", EntryPoint = "CloseCard", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short CloseCard(int icdev);

        [DllImport("mt_32.dll", EntryPoint = "contact_select", SetLastError = true,
     CharSet = CharSet.Auto, ExactSpelling = false,
     CallingConvention = CallingConvention.StdCall)]
        public static extern short contact_select(int icdev, byte nCardType);

        [DllImport("mt_32.dll", EntryPoint = "contact_verify", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short contact_verify(int icdev, [MarshalAs(UnmanagedType.LPArray)]byte[] nCardType);

        [DllImport("mt_32.dll", EntryPoint = "rf_card", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short rf_card(int icdev, byte mode, [MarshalAs(UnmanagedType.LPArray)]byte[] sAtr, [MarshalAs(UnmanagedType.LPArray)]byte[] len);

        [DllImport("mt_32.dll", EntryPoint = "rf_authentication_key", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short rf_authentication_key(int icdev, byte mode, byte Add, [MarshalAs(UnmanagedType.LPArray)]byte[] key);

        [DllImport("mt_32.dll", EntryPoint = "rf_read", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short rf_read(int icdev, byte Add, [MarshalAs(UnmanagedType.LPArray)]byte[] read);

        [DllImport("mt_32.dll", EntryPoint = "rf_write", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short rf_write(int icdev, byte Add, [MarshalAs(UnmanagedType.LPArray)]byte[] write);

        [DllImport("mt_32.dll", EntryPoint = "rf_initval", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short rf_initval(int icdev, byte Add, long val);

        [DllImport("mt_32.dll", EntryPoint = "rf_increment", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short rf_increment(int icdev, byte Add, int val);

        [DllImport("mt_32.dll", EntryPoint = "rf_decrement", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short rf_decrement(int icdev, byte Add, int val);

        [DllImport("mt_32.dll", EntryPoint = "rf_readval", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern short rf_readval(int icdev, byte Add, [MarshalAs(UnmanagedType.LPArray)]int[] val);
    }
}
