[![ESLint](https://github.com/cameyo/virtual-channel-sdk/actions/workflows/eslint.yml/badge.svg)](https://github.com/cameyo/virtual-channel-sdk/actions/workflows/eslint.yml)
[![Prettier](https://github.com/cameyo/virtual-channel-sdk/actions/workflows/prettier.yml/badge.svg)](https://github.com/cameyo/virtual-channel-sdk/actions/workflows/prettier.yml)
[![Publish to NPM](https://github.com/cameyo/virtual-channel-sdk/actions/workflows/npm-publish.yml/badge.svg)](https://github.com/cameyo/virtual-channel-sdk/actions/workflows/npm-publish.yml)

# Cameyo Virtual Channel Client SDK

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

##  Installation
In order to get started developing, run `npm install` to install the required dependencies.

##  Build
### SDK
To build the SDK, run `npm run build`, which will create the following set of files:
* `/dist/sdk.js` – the SDK you need to include;
* `/dist/sdk.js.map` – the SDK source map;
* `/dist/index.d.ts` – Typescript typings for the SDK.

### Sample Chrome extension
To build sample extension, run `npm run build`, which will create the following set of files:
* `/dist/background.js` – service worker of Chrome extension;
* `/dist/manifest.json` – Chrome Extension Manifest V3 file.

## Npm package
To create an nmp package, run `npm pack` command, which will create **cameyo-virtual-channel-sdk-version.tgz**.

To install this package into your project, use `npm install cameyo-virtual-channel-sdk-version.tgz` command.

## Contributing
Feel free to send pull-requests! All code changes must be:
* approved by a project maintainer;
* pass linting (use `npm run lint`);
* be properly formatted (use `npm run format` or `npm run formatCheck`).