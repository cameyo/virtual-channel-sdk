# Cameyo Virtual Channel SDK

**Cameyo Virtual Channel SDK** is designed to use in-session Cameyo Virtual Channels. It allows to organize data exchange between the Chrome extension and a remote application in a Cameyo session and provides a set of tools for this.

This SDK consists of client and server parts.

## Client SDK
**Client SDK** provides an SDK for integrating your Chrome extension with Cameyo Virtual Channels. More details in [Cameyo Virtual Channel Client SDK](client/README.md).

It is also available as a [npmjs package](https://www.npmjs.com/package/cameyo-virtual-channel-sdk).

## Server SDK
**Server SDK** provides an SDK for integrating your remote application with Cameyo Virtual Channels. More details in [Cameyo Virtual Channel Server SDK](server/README.md).

It is also available as a [NuGet package](https://www.nuget.org/packages/VirtualChannelSDK).

## Activation
To enable Cameyo Virtual Channel SDK need to set `!VIRTCHANNEL=` and `!CHROMEEXTID=` [PowerTags](https://helpcenter.cameyo.com/support/solutions/articles/80000254678-power-tags).

Work with many channels is supported, for this it is enough to set them using:
```
!VIRTCHANNEL=Ch1,Ch2,ChN
```

**Note:** the maximum length of the channel name can be 7 characters (without the end-of-line character), and their number cannot exceed 31. If you specify a longer channel name, it will be truncated to length 7. The number of channels is limited to 20, the remaining 11 are reserved for system purposes.

Browser logging can be enabled with:
```
!VIRTCHANNELLOG=1
```

When a session is running and this mechanism is enabled, the Cameyo session connects to the Chrome extension using the specified *Chrome Extension ID* (`!CHROMEEXTID=`) and creates a long-lived port connection for each specified *virtual channel name* (`!VIRTCHANNEL=`) to data exchange.