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
        public const uint SCARD_PROTOCOL_RAW       = 0x00010000;  // Raw is the active protocol.

        public const uint SCARD_SHARE_EXCLUSIVE    = 1; // This application is not willing to share this card with other applications.
        public const uint SCARD_SHARE_SHARED       = 2; // This application is willing to share this card with other applications.                                                        
        public const uint SCARD_SHARE_DIRECT       = 3; // This application demands direct control of the reader, so it is not available to other applications.

        [DllImport("winscard.dll",CharSet = CharSet.Auto,EntryPoint="SCardEstablishContext",
            SetLastError = true, CallingConvention= CallingConvention.StdCall)]
        public static extern int SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, out UIntPtr phContext);

        [DllImport("winscard.dll",CharSet = CharSet.Auto,EntryPoint="SCardReleaseContext",
            SetLastError = true, CallingConvention= CallingConvention.StdCall)]
        public static extern int SCardReleaseContext(UIntPtr hContext);

        [DllImport("winscard.dll",CharSet = CharSet.Auto,EntryPoint="SCardConnect",
            SetLastError = true, CallingConvention= CallingConvention.StdCall)]
        public static extern int SCardConnect(UIntPtr hContext, [MarshalAs(UnmanagedType.LPTStr)]String szReader, uint dwShareMode, uint dwPreferredProtocols, ref UIntPtr phCard, ref uint dwActiveProtocol);

        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardDisconnect",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardDisconnect(UIntPtr hCard, uint dwDisposition);

        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardStatus",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardStatus(UIntPtr hCard, [MarshalAs(UnmanagedType.LPArray)]Char[] szReaderName, ref uint chReaderLen, ref uint dwState, ref uint dwProtocol, [MarshalAs(UnmanagedType.LPArray)]byte[] pbAtr, ref uint cbAtrLen);


        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardTransmit",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardTransmit(UIntPtr hCard, IntPtr pioSendPci, [MarshalAs(UnmanagedType.LPArray)]byte[] pbSendBuffer, uint cbSendLength, IntPtr pioRecvPci, [MarshalAs(UnmanagedType.LPArray)]byte[] pbRecvBuffer, ref uint cbRecvLength);

        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardListReaders",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardListReaders(UIntPtr hContext, [MarshalAs(UnmanagedType.LPArray)]Char[] mszGroups, [MarshalAs(UnmanagedType.LPArray)]Char[] mszReaders, ref uint chReaders);

        [DllImport("winscard.dll", CharSet = CharSet.Auto, EntryPoint = "SCardFreeMemory",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SCardFreeMemory(UIntPtr hContext, [MarshalAs(UnmanagedType.AsAny)]Object pvMem);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "LoadLibrary",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPTStr)]String lpLibFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "FreeLibrary",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool FreeLibrary(IntPtr hLibModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GetProcAddress",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetProcAddress(IntPtr hLibModule,[MarshalAs(UnmanagedType.LPTStr)]String lpProcName);

        public static IntPtr SCardT0Pci()
        {
            IntPtr handle = LoadLibrary("winscard.dll");
            IntPtr pci = GetProcAddress(handle, "g_rgSCardT0Pci");
            FreeLibrary(handle);
            IntPtr ioT0 = pci;
            return ioT0;
        }

        public static IntPtr SCardT1Pci()
        {
            IntPtr handle = LoadLibrary("winscard.dll");
            IntPtr pci = GetProcAddress(handle, "g_rgSCardT1Pci");
            FreeLibrary(handle);
            IntPtr ioT1 = pci;
            return ioT1;
        }

        public static IntPtr SCardRawPci()
        {
            IntPtr handle = LoadLibrary("winscard.dll");
            IntPtr pci = GetProcAddress(handle, "g_rgSCardRawPci");
            FreeLibrary(handle);
            IntPtr ioRaw = pci;
            return ioRaw;
        }
       
    }
}
