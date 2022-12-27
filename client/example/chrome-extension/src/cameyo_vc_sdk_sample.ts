/**
 * @license
 * MIT License
 *
 * Copyright (c) 2022 Cameyo Inc
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

import {Cameyo} from 'cameyo-virtual-channel-sdk';

function readStringFromData(data: Uint8Array): string {
  return new TextDecoder().decode(data);
}

function stringToData(str: string): Uint8Array {
  return new TextEncoder().encode(str);
}

const transportChannelName = 'ChrExt';
const vcHandler = new Cameyo.VirtualChannelHandler();

vcHandler.addChannelOpenedListener(connectionHandle => {
  console.log(`Channel '${connectionHandle.channelName}' for session '${connectionHandle.sessionId}' was opened`);
});

vcHandler.addChannelClosedListener(connectionHandle => {
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
