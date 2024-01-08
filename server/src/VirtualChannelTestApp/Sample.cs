// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sample.cs" company="Cameyo Inc">
//
// MIT License
//
// Copyright (c) 2024 Cameyo Inc
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
using System.Linq;
using System.Text;
using VirtualChannelSDK;

namespace VirtualChannelTestApp
{
    public static class Sample
    {
        public static void Main(string[] _)
        {
            var wtsChannelNameReader = new WtsChannelNameReader();
            var virtualChannelNames = wtsChannelNameReader.GetVirtualChannelNames();

            var wtsChannelName = virtualChannelNames.FirstOrDefault();
            if (wtsChannelName == null)
            {
                Console.WriteLine("Virtual channel not found");
                Console.ReadKey();
                return;
            }

            using (var virtualChannelClient = new WtsVirtualChannelClient(wtsChannelName))
            {
                try
                {
                    virtualChannelClient.Connect();
                    var virtualChannelStream = virtualChannelClient.GetStream();

                    var message = "ping";
                    var messageBytes = Encoding.ASCII.GetBytes(message);
                    virtualChannelStream.Write(messageBytes, 0, messageBytes.Length);
                    Console.WriteLine($"Sent message: {message}");

                    var buffer = new byte[Constants.MAX_DATA_CHUNK];
                    var readBytes = virtualChannelStream.Read(buffer, 0, Constants.MAX_DATA_CHUNK);
                    var receivedMessage = Encoding.ASCII.GetString(buffer, 0, readBytes);
                    Console.WriteLine($"Received message: {receivedMessage}");
                }
                catch (WtsException ex)
                {
                    Console.WriteLine($"Virtual channel client error: last error code = {ex.LastErrorCode}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Virtual channel client error: {ex.Message}");
                }
            }

            Console.Write("Done");
            Console.ReadKey();
        }
    }
}