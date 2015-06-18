using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace PcscLH
{
    public class WinSCard_Dll
    {
        public const uint SCARD_PROTOCOL_UNDEFINED = 0x00000000;  // There is no active protocol.
        public const uint SCARD_PROTOCOL_T0        = 0x00000001;  // T=0 is the active protocol.
        public const uint SCARD_PROTOCOL_T1        = 0x00000002;  // T=1 is the active protocol.        

        public const uint SCARD_SHARE_EXCLUSIVE    = 1; // This application is not willing to share this card with other applications.
        public const uint SCARD_SHARE_SHARED       = 2; // This application is willing to share this card with other applications.                                                        
        public const uint SCARD_SHARE_DIRECT       = 3; // This application demands direct control of the reader, so it is not available to other applications.

        public const uint SCARD_LEAVE_CARD         = 0; // Don't do anything special on close
        public const uint SCARD_RESET_CARD         = 1; // Reset the card on close
        public const uint SCARD_UNPOWER_CARD       = 2; // Power down the card on close
        public const uint SCARD_EJECT_CARD         = 3; // Eject the card on close

        public const uint SCARD_SCOPE_USER         = 0;  // The context is a user context, and any database operations are performed within the domain of the user.

        //连接读卡器
        [DllImport("winscard.dll",CharSet = CharSet.Auto,EntryPoint="SCardEstablishContext",
            SetLastError = true, CallingConvention= CallingConvention.StdCall)]
        public static extern int SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, ref UIntPtr phContext);

        //获取所有可用的读卡器
        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardListReaders",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardListReaders(UIntPtr hContext, [MarshalAs(UnmanagedType.LPArray)]Char[] mszGroups, [MarshalAs(UnmanagedType.LPArray)]Char[] mszReaders, ref uint chReaders);

        //断开读卡器
        [DllImport("winscard.dll",CharSet = CharSet.Auto,EntryPoint="SCardReleaseContext",
            SetLastError = true, CallingConvention= CallingConvention.StdCall)]
        public static extern int SCardReleaseContext(UIntPtr hContext);

        //打开卡片
        [DllImport("winscard.dll",CharSet = CharSet.Auto,EntryPoint="SCardConnect",
            SetLastError = true, CallingConvention= CallingConvention.StdCall)]
        public static extern int SCardConnect(UIntPtr hContext, [MarshalAs(UnmanagedType.LPTStr)]String szReader, uint dwShareMode, uint dwPreferredProtocols, ref UIntPtr phCard, ref uint dwActiveProtocol);

        //关闭卡片
        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardDisconnect",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardDisconnect(UIntPtr hCard, uint dwDisposition);

        //获取卡片状态
        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardStatus",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardStatus(UIntPtr hCard, [MarshalAs(UnmanagedType.LPArray)]Char[] szReaderName, ref uint chReaderLen, ref uint dwState, ref uint dwProtocol, [MarshalAs(UnmanagedType.LPArray)]byte[] pbAtr, ref uint cbAtrLen);

        //直接控制读卡器
        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardControl",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardControl(UIntPtr hCard, uint dwControlCode, [MarshalAs(UnmanagedType.LPArray)]byte[] lpInBuffer, uint nInBufferSize, [MarshalAs(UnmanagedType.LPArray)]byte[] lpOutBuffer, uint nOutBufferSize, ref uint BytesReturned);

        //收发数据
        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardTransmit",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardTransmit(UIntPtr hCard, IntPtr pioSendPci, [MarshalAs(UnmanagedType.LPArray)]byte[] pbSendBuffer, uint cbSendLength, IntPtr pioRecvPci, [MarshalAs(UnmanagedType.LPArray)]byte[] pbRecvBuffer, ref uint cbRecvLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "LoadLibrary",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPTStr)]String lpLibFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "FreeLibrary",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool FreeLibrary(IntPtr hLibModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GetProcAddress",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetProcAddress(IntPtr hLibModule,[MarshalAs(UnmanagedType.LPTStr)]String lpProcName);

        //          struct SCARD_IO_REQUEST
        //            {
        //               uint dwProtocol;   // Protocol identifier
        //               uint cbPciLength;  // Protocol Control Information Length
        //            }
        //获取C#可用的结构SCARD_IO_REQUEST，winscard.dll中是全局变量,protocol为T0时用g_rgSCardT0Pci
        public static IntPtr SCardT0Pci()
        {
            IntPtr handle = LoadLibrary("winscard.dll");
            IntPtr pci = GetProcAddress(handle, "g_rgSCardT0Pci");
            FreeLibrary(handle);
            IntPtr ioT0 = pci;
            return ioT0;
        }

        //获取C#可用的结构SCARD_IO_REQUEST，winscard.dll中是全局变量,protocol为T1时用g_rgSCardT1Pci
        public static IntPtr SCardT1Pci()
        {
            IntPtr handle = LoadLibrary("winscard.dll");
            IntPtr pci = GetProcAddress(handle, "g_rgSCardT1Pci");
            FreeLibrary(handle);
            IntPtr ioT1 = pci;
            return ioT1;
        }       
    }
}
