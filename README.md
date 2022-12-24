# Cameyo Virtual Channel SDK v1.4

## Chrome extension side

An example test Chrome extension is located in the example/chrome-extension folder.

### [manifest.json](example/chrome-extension/public/manifest.json)
Should allow external connections from cameyo.com. To do this, just add the following:
```
{
    ...
    "externally_connectable": {
        "matches": [
            "*://*.cameyo.com/*",
            "*://*.cameyo.net/*"
        ],
        "accepts_tls_channel_id": true
    },
    ...
}
````

### Code sample
Sample usage is in the [cameyo_vc_sdk_sample.ts](example/chrome-extension/src/cameyo_vc_sdk_sample.ts) file.

## Cameyo side
First of all you need to set !VIRTCHANNEL= and !CHROMEEXTID= [PowerTags](https://helpcenter.cameyo.com/support/solutions/articles/80000254678-power-tags).

Work with many channels is supported, for this it is enough to set them using:
```
!VIRTCHANNEL=Ch1,Ch2,ChN
```

Logging can be enabled using:
```
!VIRTCHANNELLOG=1
```

**Note:** the maximum length of the channel name can be 7 characters (without the end-of-line character), and their number cannot exceed 31. If you specify a longer channel name, it will be truncated to length 7. The number of channels is limited to 20, the remaining 11 are reserved for system purposes.

When session is started and this mechanism is enabled, then CameyoApp connects to the chrome extension via chrome.runtime.connect. background.js (service worker) listens for incoming long lasting port connection. Then to this port adds onMessage message event listener.

##  Installation
In order to get started developing, run `npm install` to install the required dependencies.

##  Build
### SDK
To build the SDK, run `npm run build`, which will create the following set of files:
* `/dist/sdk.js` – the SDK you need to include;
* `/dist/sdk.js.map` – the SDK source map;
* `/dist/index.d.ts` – Typescript typings for the SDK.

Also you can use [build.bat](build.bat) file.

### Sample Chrome extension
To build sample extension, run `npm run build`, which will create the following set of files:
* `/dist/background.js` – service worker of Chrome extension;
* `/dist/manifest.json` – Chrome Extension Manifest V3 file.

Also you can use [build.bat](example/chrome-extension/build.bat) file.

## Npm package
To create an nmp package, run `npm pack` command, which will create **cameyo-virtual-channel-sdk-version.tgz**.

To install this package into your project, use `npm install cameyo-virtual-channel-sdk-version.tgz` command.