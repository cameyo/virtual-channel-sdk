// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WtsVirtualChannelStream.cs" company="Cameyo Inc">
//
// MIT License
//
// Copyright (c) 2022 Cameyo Inc
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

namespace VirtualChannelSDK
{
    public class WtsVirtualChannelStream : Stream
    {
        private readonly WtsVirtualChannel _virtualChannel;
        private readonly bool _readable;
        private readonly bool _writeable;

        private bool _disposed;

        public WtsVirtualChannelStream(WtsVirtualChannel virtualChannel)
            : this(virtualChannel, FileAccess.ReadWrite)
        {
        }

        public WtsVirtualChannelStream(WtsVirtualChannel virtualChannel, FileAccess access)
        {
            if (virtualChannel == null)
            {
                throw new ArgumentNullException(nameof(virtualChannel));
            }

            if (!virtualChannel.IsOpen)
            {
                throw new IOException("Virtual channel not connected");
            }

            _virtualChannel = virtualChannel;

            switch (access)
            {
                case FileAccess.Read:
                    _readable = true;
                    break;

                case FileAccess.Write:
                    _writeable = true;
                    break;

                default:
                    _readable = true;
                    _writeable = true;
                    break;
            }
        }

        public override bool CanRead => _readable;

        public override bool CanWrite => _writeable;

        public override bool CanSeek => false;

        public override long Length =>
            throw new NotSupportedException("Virtual channels do not support seek operations");

        public override long Position
        {
            get => throw new NotSupportedException("Virtual channels do not support seek operations");
            set => throw new NotSupportedException("Virtual channels do not support seek operations");
        }

        public override long Seek(long offset, SeekOrigin origin) =>
            throw new NotSupportedException("Virtual channels do not support seek operations");

        public override void SetLength(long value) =>
            throw new NotSupportedException("Virtual channels do not support seek operations");

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead)
            {
                throw new InvalidOperationException("Write only stream");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0 || offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0 || count > buffer.Length - offset)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return _virtualChannel.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite)
            {
                throw new InvalidOperationException("Write only stream");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0 || offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0 || count > buffer.Length - offset)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            _virtualChannel.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _virtualChannel?.Close();
                base.Dispose(disposing);
            }

            _disposed = true;
        }
    }
}