# Cameyo Virtual Channel Server SDK

The **Cameyo Virtual Channel** backend uses [Remote Desktop Services Virtual Channels](https://learn.microsoft.com/en-us/windows/win32/termserv/terminal-services-virtual-channels) as transport.

**Cameyo Virtual Channel Server SDK** provides a .NET Standard 2.0 class library for convenient work with virtual channels. It performs two main functions:

1. getting a list of available virtual channel names;

2. provides a client to communicate over a channel.

## Code sample
Sample usage is in the [Sample.cs](src/VirtualChannelTestApp/Sample.cs) file.

##  Build
Use `MSBuild` to build SDK:
```
MSBuild src/VirtualChannelSDK/VirtualChannelSDK.csproj -t:restore,build -p:RestorePackagesConfig=true -p:Configuration=Release -p:OutDir=../../dist
```

## Contributing
Feel free to send pull-requests! All code changes must be:
* approved by a project maintainer;
* pass code analysis (build without warnings and errors).