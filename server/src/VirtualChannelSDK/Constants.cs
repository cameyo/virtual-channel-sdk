// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Cameyo Inc">
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

namespace VirtualChannelSDK
{
    public static class Constants
    {
        public const int MAX_DATA_CHUNK = 1600;

        internal static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        internal const int WTS_CURRENT_SESSION = -1;

        internal const uint INFINITY = 0xffffffff;

    }
}