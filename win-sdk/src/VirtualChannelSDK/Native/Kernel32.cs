using System.Runtime.InteropServices;

namespace VirtualChannelSDK.Native
{
    internal class Kernel32
    {
        private const string Kernel32DllName = "Kernel32.dll";

        [DllImport(Kernel32DllName)]
        public static extern uint GetLastError();
    }
}