using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualChannelSDK
{
    public class WtsChannelNameReader
    {
        private const string CameyoVirtualChannelEnv = "CAMEYO_VIRTCHANNEL";

        public IEnumerable<string> GetVirtualChannelNames()
        {
            var virtualChannelNames = Environment.GetEnvironmentVariable(CameyoVirtualChannelEnv);
            if (virtualChannelNames == null)
            {
                return Enumerable.Empty<string>();
            }

            return virtualChannelNames.Split(',').Select(channelName => channelName.Trim());
        }
    }
}