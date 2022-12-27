using System;

namespace VirtualChannelSDK
{
    public class WtsException : Exception
    {
        public WtsException(uint error)
        {
            LastErrorCode = error;
        }

        public uint LastErrorCode { get; }
    }
}