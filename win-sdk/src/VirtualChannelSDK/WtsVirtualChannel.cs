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

        public WtsVirtualChannel(string channelName)
            : this(Constants.WTS_CURRENT_SERVER_HANDLE, Constants.WTS_CURRENT_SESSION, channelName)
        {
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
            Close();
        }
    }
}