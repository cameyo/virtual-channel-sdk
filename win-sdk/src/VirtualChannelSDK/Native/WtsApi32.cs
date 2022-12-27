using System;
using System.Runtime.InteropServices;

namespace VirtualChannelSDK.Native
{
    internal class WtsApi32
    {
        private const string Wtsapi32DllName = "Wtsapi32.dll";

        [DllImport(Wtsapi32DllName, SetLastError = true)]
        public static extern IntPtr WTSVirtualChannelOpen(IntPtr server,
            int sessionId,
            [MarshalAs(UnmanagedType.LPStr)] string virtualName);

        [DllImport(Wtsapi32DllName, SetLastError = true)]
        public static extern int WTSVirtualChannelWrite(IntPtr channelHandle,
            byte[] buffer,
            uint length,
            out uint bytesWritten);

        [DllImport(Wtsapi32DllName, SetLastError = true)]
        public static extern int WTSVirtualChannelRead(IntPtr channelHandle,
            uint timeOut,
            byte[] buffer,
            uint length,
            out uint bytesRead);

        [DllImport(Wtsapi32DllName, SetLastError = true)]
        public static extern bool WTSVirtualChannelClose(IntPtr channelHandle);
    }
}