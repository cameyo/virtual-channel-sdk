/**
 * @license
 * MIT License
 *
 * Copyright (c) 2023 Cameyo Inc
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

const divider = '_###_';
const connectionHandleRegex = new RegExp(`^(.*)${divider}([a-zA-Z0-9]{8}-(?:[a-zA-Z0-9]{4}-){3}[a-zA-Z0-9]{12})$`);

export type ConnectionHandle = {
  channelName: string;
  sessionId: string;
};

export type ChannelOpenedListener = (connectionHandle: ConnectionHandle) => void;
export type ChannelClosedListener = (connectionHandle: ConnectionHandle) => void;
export type DataReceivedListener = (connectionHandle: ConnectionHandle, data: Uint8Array) => void;

export class VirtualChannelHandler {
  protected channelOpenedListeners = new Set<ChannelOpenedListener>();
  protected channelClosedListeners = new Set<ChannelClosedListener>();
  protected dataReceivedListeners = new Set<DataReceivedListener>();

  // Maps serialized ConnectionHandle to ports.
  protected ports = new Map<string, chrome.runtime.Port>();

  constructor() {
    chrome.runtime.onConnectExternal.addListener(port => {
      this.onPortConnected(port);
    });
  }

  addChannelOpenedListener(channelOpenedListener: ChannelOpenedListener) {
    this.channelOpenedListeners.add(channelOpenedListener);
  }

  removeChannelOpenedListener(channelOpenedListener: ChannelOpenedListener) {
    this.channelOpenedListeners.delete(channelOpenedListener);
  }

  addChannelClosedListener(channelClosedListener: ChannelClosedListener) {
    this.channelClosedListeners.add(channelClosedListener);
  }

  removeChannelClosedListener(channelClosedListener: ChannelClosedListener) {
    this.channelClosedListeners.delete(channelClosedListener);
  }

  addDataReceivedListener(dataReceivedListener: DataReceivedListener) {
    this.dataReceivedListeners.add(dataReceivedListener);
  }

  removeDataReceivedListener(dataReceivedListener: DataReceivedListener) {
    this.dataReceivedListeners.delete(dataReceivedListener);
  }

  sendData(connectionHandle: ConnectionHandle, data: Uint8Array) {
    const port = this.getPort(connectionHandle);
    port.postMessage(Array.from(data));
  }

  disconnect(connectionHandle: ConnectionHandle) {
    const port = this.getPort(connectionHandle);
    port.disconnect();
    this.onPortDisconnected(connectionHandle);
  }

  protected onPortConnected(port: chrome.runtime.Port) {
    if (!port.sender.origin.includes('cameyo.com') && !port.sender.origin.includes('cameyo.net')) {
      return;
    }

    const connectionHandle = this.getConnectionHandleForNewPort(port);
    if (connectionHandle === undefined) return;

    port.onDisconnect.addListener(_port => {
      this.onPortDisconnected(connectionHandle);
    });

    port.onMessage.addListener((message, _port) => {
      this.onPortMessage(connectionHandle, message);
    });

    const serializeConnectionHandle = this.serializeConnectionHandle(connectionHandle);
    this.ports.set(serializeConnectionHandle, port);

    for (const channelOpenedListener of this.channelOpenedListeners) {
      channelOpenedListener(connectionHandle);
    }
  }

  protected onPortDisconnected(connectionHandle: ConnectionHandle) {
    for (const channelClosedListener of this.channelClosedListeners) {
      channelClosedListener(connectionHandle);
    }

    const serializeConnectionHandle = this.serializeConnectionHandle(connectionHandle);
    this.ports.delete(serializeConnectionHandle);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  protected onPortMessage(connectionHandle: ConnectionHandle, message: any) {
    const data = new Uint8Array(message);
    for (const dataReceivedListener of this.dataReceivedListeners) {
      dataReceivedListener(connectionHandle, data);
    }
  }

  protected getPort(connectionHandle: ConnectionHandle): chrome.runtime.Port {
    const serializeConnectionHandle = this.serializeConnectionHandle(connectionHandle);
    const port = this.ports.get(serializeConnectionHandle);
    if (port === undefined) {
      throw new Error(`Unknown connection '${serializeConnectionHandle}'`);
    }

    return port;
  }

  protected getConnectionHandleForNewPort(port: chrome.runtime.Port): ConnectionHandle | undefined {
    const result = connectionHandleRegex.exec(port.name);
    if (result === null) {
      return undefined;
    }

    const connectionHandle: ConnectionHandle = {
      channelName: result[1],
      sessionId: result[2],
    };

    return connectionHandle;
  }

  protected serializeConnectionHandle(connectionHandle: ConnectionHandle): string {
    return `${connectionHandle.channelName}${divider}${connectionHandle.sessionId}`;
  }
}
