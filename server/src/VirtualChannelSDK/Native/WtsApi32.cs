// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WtsApi32.cs" company="Cameyo Inc">
//
// MIT License
//
// Copyright (c) 2023 Cameyo Inc
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace VirtualChannelSDK.Native
{
    internal static class WtsApi32
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