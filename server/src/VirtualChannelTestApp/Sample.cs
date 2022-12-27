using System;
using System.Linq;
using System.Text;
using VirtualChannelSDK;

namespace VirtualChannelTestApp
{
    public class Sample
    {
        public static void Main(string[] args)
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