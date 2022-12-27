using System;

namespace VirtualChannelSDK
{
    public class WtsVirtualChannelClient : IDisposable
    {
        private WtsVirtualChannel _virtualChannel;
        private WtsVirtualChannelStream _virtualChannelStream;

        private bool _disposed;

        public WtsVirtualChannelClient(string wtsChannelName)
            : this(new WtsVirtualChannel(wtsChannelName))
        {
        }

        public WtsVirtualChannelClient(WtsVirtualChannel virtualChannel)
        {
            _virtualChannel = virtualChannel;
        }

        public bool Connected => _virtualChannel.IsOpen;

        public WtsVirtualChannelStream GetStream()
        {
            if (!Connected)
            {
                throw new InvalidOperationException("Virtual channel not connected");
            }

            if (_virtualChannelStream == null)
            {
                _virtualChannelStream = new WtsVirtualChannelStream(_virtualChannel);
            }

            return _virtualChannelStream;
        }

        public WtsVirtualChannel Client => _disposed ? null : _virtualChannel;

        public void Connect()
        {
            _virtualChannel.Open();
        }

        public void Close() => Dispose();

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _virtualChannel?.Dispose();
            }

            _disposed = true;
        }
    }
}