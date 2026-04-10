/**
 * Cross-Shadow-DOM Communication - Inter-component messaging system
 * @module shadow-dom/04_8_Cross-Shadow-DOM-Communication
 * @version 1.0.0
 * @example <message-bus></message-bus>
 */

class MessageBus extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._listeners = new Map();
    this._messageQueue = [];
    this._history = [];
    this._maxHistory = 50;
    this._eventTarget = new EventTarget();
    this._namespace = 'default';
    this._delimiter = ':';
  }

  static get observedAttributes() {
    return ['namespace', 'max-history', 'buffered'];
  }

  connectedCallback() {
    this._render();
    this._attachEventListeners();
    this._processQueue();
  }

  disconnectedCallback() {
    this._cleanup();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._handleAttributeChange(name, newValue);
    }
  }

  _handleAttributeChange(name, value) {
    switch (name) {
      case 'namespace':
        this._namespace = value || 'default';
        break;
      case 'max-history':
        this._maxHistory = parseInt(value) || 50;
        this._pruneHistory();
        break;
      case 'buffered':
        this._processQueue();
        break;
    }
  }

  _render() {
    const style = this._getStyles();
    const template = this._getTemplate();
    this.shadowRoot.innerHTML = `${style}${template}`;
    this._cacheElements();
  }

  _getStyles() {
    return `
      <style>
        :host {
          display: block;
          --bus-bg: #0d1117;
          --bus-border: #30363d;
          --bus-text: #c9d1d9;
          --bus-accent: #58a6ff;
          --bus-success: #3fb950;
          --bus-warning: #d29922;
          --bus-error: #f85149;
          font-family: 'SF Mono', 'Consolas', monospace;
          font-size: 12px;
        }

        :host([hidden]) {
          display: none;
        }

        .bus-container {
          background: var(--bus-bg);
          border: 1px solid var(--bus-border);
          border-radius: 6px;
          padding: 12px;
        }

        .bus-header {
          display: flex;
          align-items: center;
          justify-content: space-between;
          margin-bottom: 12px;
          padding-bottom: 8px;
          border-bottom: 1px solid var(--bus-border);
        }

        .bus-title {
          color: var(--bus-text);
          font-weight: 600;
          display: flex;
          align-items: center;
          gap: 8px;
        }

        .bus-status {
          display: flex;
          align-items: center;
          gap: 6px;
          font-size: 11px;
          color: #8b949e;
        }

        .status-dot {
          width: 8px;
          height: 8px;
          border-radius: 50%;
          background: var(--bus-success);
        }

        .status-dot.inactive {
          background: var(--bus-error);
        }

        .channel-list {
          display: flex;
          flex-direction: column;
          gap: 8px;
          max-height: 200px;
          overflow-y: auto;
        }

        .channel-item {
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding: 8px 12px;
          background: #161b22;
          border-radius: 4px;
          border: 1px solid var(--bus-border);
        }

        .channel-name {
          color: var(--bus-accent);
          font-weight: 500;
        }

        .channel-badge {
          font-size: 10px;
          padding: 2px 6px;
          border-radius: 10px;
          background: #21262d;
          color: #8b949e;
        }

        .message-log {
          margin-top: 12px;
          background: #0d1117;
          border: 1px solid var(--bus-border);
          border-radius: 4px;
          max-height: 150px;
          overflow-y: auto;
        }

        .message-item {
          padding: 6px 10px;
          border-bottom: 1px solid #21262d;
          display: grid;
          grid-template-columns: 60px 80px 1fr;
          gap: 10px;
          align-items: center;
        }

        .message-item:last-child {
          border-bottom: none;
        }

        .message-time {
          color: #8b949e;
          font-size: 10px;
        }

        .message-channel {
          color: var(--bus-accent);
          font-size: 11px;
        }

        .message-payload {
          color: var(--bus-text);
          font-size: 11px;
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
        }

        .message-type {
          font-size: 9px;
          text-transform: uppercase;
          padding: 2px 4px;
          border-radius: 2px;
        }

        .message-type.publish { background: #1f6feb22; color: var(--bus-accent); }
        .message-type.subscribe { background: #3fb95022; color: var(--bus-success); }
        .message-type.request { background: #d2992222; color: var(--bus-warning); }
        .message-type.response { background: #a371f722; color: #a371f7; }

        .bus-actions {
          display: flex;
          gap: 8px;
          margin-top: 12px;
        }

        .bus-btn {
          flex: 1;
          padding: 8px;
          background: #21262d;
          border: 1px solid var(--bus-border);
          color: var(--bus-text);
          border-radius: 4px;
          cursor: pointer;
          font-family: inherit;
          font-size: 11px;
          transition: background 0.15s ease;
        }

        .bus-btn:hover {
          background: #30363d;
        }

        .bus-input {
          display: flex;
          gap: 8px;
          margin-top: 12px;
        }

        .input-channel {
          width: 100px;
          padding: 8px;
          background: #0d1117;
          border: 1px solid var(--bus-border);
          color: var(--bus-text);
          border-radius: 4px;
          font-family: inherit;
          font-size: 11px;
        }

        .input-message {
          flex: 1;
          padding: 8px;
          background: #0d1117;
          border: 1px solid var(--bus-border);
          color: var(--bus-text);
          border-radius: 4px;
          font-family: inherit;
          font-size: 11px;
        }

        .input-message::placeholder {
          color: #6e7681;
        }

        .subscribe-form {
          display: flex;
          gap: 8px;
          margin-top: 8px;
        }
      </style>
    `;
  }

  _getTemplate() {
    return `
      <div class="bus-container">
        <div class="bus-header">
          <div class="bus-title">
            <span>Message Bus</span>
            <span class="channel-badge" id="listener-count">0</span>
          </div>
          <div class="bus-status">
            <span class="status-dot" id="status-dot"></span>
            <span id="status-text">Active</span>
          </div>
        </div>
        <div class="channel-list" id="channel-list"></div>
        <div class="message-log" id="message-log"></div>
        <div class="subscribe-form">
          <input type="text" class="input-channel" id="sub-channel" placeholder="Channel">
          <button class="bus-btn" id="subscribe-btn">Subscribe</button>
        </div>
        <div class="bus-input">
          <input type="text" class="input-channel" id="pub-channel" placeholder="Channel">
          <input type="text" class="input-message" id="pub-message" placeholder="Message payload">
          <button class="bus-btn" id="publish-btn">Publish</button>
        </div>
        <div class="bus-actions">
          <button class="bus-btn" id="clear-btn">Clear Log</button>
          <button class="bus-btn" id="history-btn">View History</button>
          <button class="bus-btn" id="export-btn">Export</button>
        </div>
      </div>
    `;
  }

  _cacheElements() {
    this._channelList = this.shadowRoot.getElementById('channel-list');
    this._messageLog = this.shadowRoot.getElementById('message-log');
    this._listenerCount = this.shadowRoot.getElementById('listener-count');
    this._statusDot = this.shadowRoot.getElementById('status-dot');
    this._statusText = this.shadowRoot.getElementById('status-text');
    this._pubChannel = this.shadowRoot.getElementById('pub-channel');
    this._pubMessage = this.shadowRoot.getElementById('pub-message');
    this._subChannel = this.shadowRoot.getElementById('sub-channel');
  }

  _attachEventListeners() {
    this.shadowRoot.getElementById('publish-btn').addEventListener('click', () => this._publishFromInput());
    this.shadowRoot.getElementById('subscribe-btn').addEventListener('click', () => this._subscribeFromInput());
    this.shadowRoot.getElementById('clear-btn').addEventListener('click', () => this._clearLog());
    this.shadowRoot.getElementById('history-btn').addEventListener('click', () => this._viewHistory());
    this.shadowRoot.getElementById('export-btn').addEventListener('click', () => this._exportMessages());
  }

  subscribe(channel, callback, options = {}) {
    if (!channel || typeof callback !== 'function') {
      throw new Error('Invalid channel or callback');
    }

    const fullChannel = this._getFullChannel(channel);

    if (!this._listeners.has(fullChannel)) {
      this._listeners.set(fullChannel, new Set());
    }

    const listener = {
      callback,
      once: options.once || false,
      context: options.context || null,
      subscribeTime: Date.now()
    };

    this._listeners.get(fullChannel).add(listener);
    this._updateChannelList();
    this._logMessage('subscribe', channel, 'Listener subscribed');

    this.dispatchEvent(new CustomEvent('subscribe', {
      detail: { channel, listenerCount: this._listeners.get(fullChannel).size },
      bubbles: true,
      composed: true
    }));

    return () => this.unsubscribe(channel, callback);
  }

  unsubscribe(channel, callback) {
    const fullChannel = this._getFullChannel(channel);
    const listeners = this._listeners.get(fullChannel);

    if (!listeners) return false;

    for (const listener of listeners) {
      if (listener.callback === callback) {
        listeners.delete(listener);
        this._updateChannelList();
        this._logMessage('unsubscribe', channel, 'Listener removed');
        return true;
      }
    }

    return false;
  }

  publish(channel, payload, options = {}) {
    const fullChannel = this._getFullChannel(channel);
    const message = {
      channel: fullChannel,
      payload,
      timestamp: Date.now(),
      source: options.source || 'unknown',
      id: this._generateId()
    };

    this._history.push(message);
    this._pruneHistory();

    if (!this.hasListeners(fullChannel)) {
      if (this.getAttribute('buffered') !== 'false') {
        this._messageQueue.push(message);
      }
      this._logMessage('publish', channel, this._truncatePayload(payload), 'publish');
      return false;
    }

    const listeners = this._listeners.get(fullChannel);
    const toRemove = [];

    for (const listener of listeners) {
      try {
        listener.callback.call(listener.context, payload, message);
      } catch (error) {
        console.error(`[MessageBus] Callback error on ${fullChannel}:`, error);
      }

      if (listener.once) {
        toRemove.push(listener);
      }
    }

    toRemove.forEach(listener => listeners.delete(listener));
    this._logMessage('publish', channel, this._truncatePayload(payload), 'publish');

    this.dispatchEvent(new CustomEvent('message', {
      detail: message,
      bubbles: true,
      composed: true
    }));

    return true;
  }

  request(channel, payload, timeout = 5000) {
    return new Promise((resolve, reject) => {
      const requestId = this._generateId();
      const responseChannel = `${channel}:response:${requestId}`;

      const timeoutId = setTimeout(() => {
        this.unsubscribe(responseChannel, handleResponse);
        reject(new Error(`Request timeout on channel: ${channel}`));
      }, timeout);

      const handleResponse = (response) => {
        clearTimeout(timeoutId);
        this.unsubscribe(responseChannel, handleResponse);
        resolve(response);
      };

      this.subscribe(responseChannel, handleResponse, { once: true });
      this.publish(channel, { payload, requestId, responseChannel }, { source: 'request' });
    });
  }

  broadcast(event, data) {
    this.publish('broadcast', { event, data, namespace: this._namespace }, { source: 'broadcast' });
  }

  hasListeners(channel) {
    const fullChannel = this._getFullChannel(channel);
    const listeners = this._listeners.get(fullChannel);
    return listeners && listeners.size > 0;
  }

  getListeners(channel) {
    const fullChannel = this._getFullChannel(channel);
    return this._listeners.get(fullChannel)?.size || 0;
  }

  getChannels() {
    return Array.from(this._listeners.keys()).map(c => c.replace(`${this._namespace}${this._delimiter}`, ''));
  }

  getHistory(channel = null) {
    if (channel) {
      const fullChannel = this._getFullChannel(channel);
      return this._history.filter(m => m.channel === fullChannel);
    }
    return [...this._history];
  }

  _getFullChannel(channel) {
    return `${this._namespace}${this._delimiter}${channel}`;
  }

  _generateId() {
    return `${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
  }

  _truncatePayload(payload) {
    const str = typeof payload === 'object' ? JSON.stringify(payload) : String(payload);
    return str.length > 50 ? str.substring(0, 50) + '...' : str;
  }

  _pruneHistory() {
    while (this._history.length > this._maxHistory) {
      this._history.shift();
    }
  }

  _processQueue() {
    while (this._messageQueue.length > 0) {
      const message = this._messageQueue.shift();
      if (this.hasListeners(message.channel)) {
        this.publish(message.channel, message.payload, { source: message.source });
      }
    }
  }

  _logMessage(type, channel, payload, messageType = 'publish') {
    const entry = {
      type,
      channel,
      payload,
      messageType,
      timestamp: Date.now()
    };

    this._renderMessageLog(entry);
  }

  _renderMessageLog(entry) {
    const item = document.createElement('div');
    item.className = 'message-item';
    item.innerHTML = `
      <span class="message-time">${this._formatTime(entry.timestamp)}</span>
      <span class="message-channel">${entry.channel}</span>
      <span class="message-payload">${this._escapeHtml(entry.payload)}</span>
      <span class="message-type ${entry.messageType}">${entry.messageType}</span>
    `;

    this._messageLog.appendChild(item);
    this._messageLog.scrollTop = this._messageLog.scrollHeight;

    while (this._messageLog.children.length > 50) {
      this._messageLog.removeChild(this._messageLog.firstChild);
    }
  }

  _updateChannelList() {
    this._channelList.innerHTML = '';
    let totalListeners = 0;

    for (const [channel, listeners] of this._listeners) {
      const shortChannel = channel.replace(`${this._namespace}${this._delimiter}`, '');
      totalListeners += listeners.size;

      const item = document.createElement('div');
      item.className = 'channel-item';
      item.innerHTML = `
        <span class="channel-name">${shortChannel}</span>
        <span class="channel-badge">${listeners.size}</span>
      `;
      this._channelList.appendChild(item);
    }

    this._listenerCount.textContent = totalListeners;
  }

  _formatTime(timestamp) {
    return new Date(timestamp).toLocaleTimeString('en-US', { 
      hour12: false, 
      hour: '2-digit', 
      minute: '2-digit', 
      second: '2-digit' 
    });
  }

  _escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }

  _publishFromInput() {
    const channel = this._pubChannel.value.trim();
    const message = this._pubMessage.value;

    if (!channel) {
      console.warn('[MessageBus] Please enter a channel name');
      return;
    }

    try {
      const payload = message ? JSON.parse(message) : { text: 'ping' };
      this.publish(channel, payload, { source: 'ui' });
      this._pubMessage.value = '';
    } catch (e) {
      this.publish(channel, message || 'ping', { source: 'ui' });
      this._pubMessage.value = '';
    }
  }

  _subscribeFromInput() {
    const channel = this._subChannel.value.trim();

    if (!channel) {
      console.warn('[MessageBus] Please enter a channel name');
      return;
    }

    this.subscribe(channel, (payload, message) => {
      console.log(`[${channel}] Received:`, payload);
    });

    this._subChannel.value = '';
  }

  _clearLog() {
    this._messageLog.innerHTML = '';
  }

  _viewHistory() {
    console.log('=== Message Bus History ===');
    console.table(this._history);
    console.log('===========================');
  }

  _exportMessages() {
    const data = {
      exportTime: new Date().toISOString(),
      namespace: this._namespace,
      history: this._history,
      channels: this.getChannels(),
      listenerCount: this._listenerCount.textContent
    };

    const blob = new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `message-bus-${Date.now()}.json`;
    a.click();
    URL.revokeObjectURL(url);
  }

  _cleanup() {
    this._listeners.clear();
    this._messageQueue = [];
  }
}

customElements.define('message-bus', MessageBus);

const messageBus = new MessageBus();
document.body.appendChild(messageBus);

export { MessageBus, messageBus };

window.MessageBus = MessageBus;
window.messageBus = messageBus;