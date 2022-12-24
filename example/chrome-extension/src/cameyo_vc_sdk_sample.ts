import { Cameyo } from "cameyo-virtual-channel-sdk";

function readStringFromData(data: Uint8Array): string {
    return new TextDecoder().decode(data);
}

function stringToData(str: string): Uint8Array {
    return new TextEncoder().encode(str);
}

const transportChannelName = 'ChrExt';
const vcHandler = new Cameyo.VirtualChannelHandler();

vcHandler.addChannelOpenedListener((connectionHandle) => {
    console.log(`Channel '${connectionHandle.channelName}' for session '${connectionHandle.sessionId}' was opened`);
});

vcHandler.addChannelClosedListener((connectionHandle) => {
    console.log(`Channel '${connectionHandle.channelName}' for session '${connectionHandle.sessionId}' was closed`);
});

vcHandler.addDataReceivedListener((connectionHandle, data) => {
    if (connectionHandle.channelName !== transportChannelName) {
        console.log(`Channel '${connectionHandle.channelName}' not supported`);
        vcHandler.disconnect(connectionHandle);
        return;
    }

    const message = readStringFromData(data);
    console.log(`Received message '${message}' from session '${connectionHandle.sessionId}'`);
    if (message !== 'ping') {
        return;
    }

    const responseData = stringToData('pong');
    vcHandler.sendData(connectionHandle, responseData);
    vcHandler.disconnect(connectionHandle);
});