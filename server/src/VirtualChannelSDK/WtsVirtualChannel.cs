// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WtsVirtualChannel.cs" company="Cameyo Inc">
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
using System.IO;
using static VirtualChannelSDK.Native.Kernel32;
using static VirtualChannelSDK.Native.WtsApi32;

namespace VirtualChannelSDK
{
    public class WtsVirtualChannel : IDisposable
    {
        private readonly IntPtr _server;
        private readonly int _sessionId;
        private readonly string _channelName;

        private IntPtr _channelHandle;
        private bool _disposed;

        public WtsVirtualChannel(string channelName)
            : this(Constants.WTS_CURRENT_SERVER_HANDLE, Constants.WTS_CURRENT_SESSION, channelName)
        {
        }

        ~WtsVirtualChannel()
        {
            Dispose(false);
        }

        internal WtsVirtualChannel(IntPtr server, int sessionId, string channelName)
        {
            _server = server;
            _sessionId = sessionId;
            _channelName = channelName;

            _channelHandle = IntPtr.Zero;
        }

        public bool IsOpen => _channelHandle != IntPtr.Zero;

        public void Open()
        {
            if (IsOpen)
            {
                return;
            }

            _channelHandle = WTSVirtualChannelOpen(_server, _sessionId, _channelName);
            if (!IsOpen)
            {
                throw new WtsException(GetLastError());
            }
        }

        public void Close()
        {
            if (!IsOpen)
            {
                return;
            }

            if (WTSVirtualChannelClose(_channelHandle))
            {
                _channelHandle = IntPtr.Zero;
            }
        }

        public int Read(byte[] buffer, int offset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0 || offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (length < 0 || length > buffer.Length - offset)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (!IsOpen || buffer.Length == 0)
            {
                return 0;
            }

            var status = WTSVirtualChannelRead(_channelHandle, Constants.INFINITY, buffer, (uint)length, out var bytesRead);
            if (status == 0)
            {
                throw new WtsException(GetLastError());
            }

            return (int)bytesRead;
        }

        public int Write(byte[] buffer, int offset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0 || offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (length < 0 || length > buffer.Length - offset)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (!IsOpen || buffer.Length == 0)
            {
                return 0;
            }

            var status = WTSVirtualChannelWrite(_channelHandle, buffer, (uint)length, out var bytesWritten);
            if (status == 0)
            {
                throw new WtsException(GetLastError());
            }

            if (bytesWritten != length)
            {
                throw new IOException($"Not all data was written ({bytesWritten} from {length})");
            }

            return (int)bytesWritten;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Close();
            }

            _disposed = true;
        }
    }
}