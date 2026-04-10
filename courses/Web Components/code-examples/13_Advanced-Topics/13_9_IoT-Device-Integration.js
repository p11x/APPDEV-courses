/**
 * IoT Device Integration - IoT Components
 * @description Connect to IoT devices, sensors, and smart home systems via Web Bluetooth, WebSockets, and MQTT
 * @module advanced/iot-components
 * @version 1.0.0
 */

(function() {
  'use strict';

  const IOT_PROTOCOLS = {
    bluetooth: 'bluetooth' in navigator,
    websocket: 'WebSocket' in window,
    mqtt: typeof mqtt !== 'undefined',
    coap: typeof coap !== 'undefined'
  };

  const BLUETOOTH_SERVICES = {
    battery: '180f',
    heartRate: '180d',
    deviceInfo: '180a',
    temperature: '1809',
    humidity: '180f',
    light: '1804',
    custom: '0000fff0-0000-1000-8000-00805f9b34fb'
  };

  const DEFAULT_CONFIG = {
    scanTimeout: 10000,
    connectionTimeout: 5000,
    retryAttempts: 3,
    retryDelay: 1000,
    heartbeatInterval: 30000,
    buffers: {}
  };

  class BluetoothManager {
    constructor(config = {}) {
      this.config = { ...DEFAULT_CONFIG, ...config };
      this.device = null;
      this.server = null;
      this.services = new Map();
      this.characteristics = new Map();
      this.isConnected = false;
      this.isScanning = false;
      this.scanTimeout = null;
      this.retryCount = 0;
    }

    async scan(options = {}) {
      if (!this.isBluetoothSupported()) {
        throw new Error('Web Bluetooth not supported');
      }

      const filters = options.filters || [];
      const optionalServices = options.services || Object.values(BLUETOOTH_SERVICES);

      this.isScanning = true;
      this.dispatchEvent(new CustomEvent('bluetooth-scan-start'));

      try {
        const device = await navigator.bluetooth.requestDevice({
          filters,
          optionalServices,
          acceptAllDevices: options.acceptAllDevices || false
        });

        this.device = device;
        await this.connect();

        return device;
      } catch (error) {
        this.dispatchEvent(new CustomEvent('bluetooth-scan-error', {
          detail: { error }
        }));
        throw error;
      } finally {
        this.isScanning = false;
        this.dispatchEvent(new CustomEvent('bluetooth-scan-end'));
      }
    }

    async connect() {
      if (!this.device) {
        throw new Error('No device selected');
      }

      this.retryCount++;

      try {
        this.server = await this.device.gatt.connect();

        this.device.addEventListener('gattserverdisconnected', () => {
          this.handleDisconnect();
        });

        this.isConnected = true;
        this.retryCount = 0;

        this.dispatchEvent(new CustomEvent('bluetooth-connect', {
          detail: { device: this.device }
        }));

        return this.server;
      } catch (error) {
        if (this.retryCount < this.config.retryAttempts) {
          await this.delay(this.config.retryDelay);
          return this.connect();
        }
        throw error;
      }
    }

    async disconnect() {
      if (this.device && this.device.gatt.connected) {
        this.device.gatt.disconnect();
      }
      this.isConnected = false;
      this.server = null;
    }

    async getPrimaryService(serviceUUID) {
      if (!this.server) throw new Error('Not connected');

      let service = this.services.get(serviceUUID);
      if (!service) {
        service = await this.server.getPrimaryService(serviceUUID);
        this.services.set(serviceUUID, service);
      }
      return service;
    }

    async getCharacteristic(serviceUUID, characteristicUUID) {
      const service = await this.getPrimaryService(serviceUUID);
      const key = `${serviceUUID}-${characteristicUUID}`;

      let characteristic = this.characteristics.get(key);
      if (!characteristic) {
        characteristic = await service.getCharacteristic(characteristicUUID);
        this.characteristics.set(key, characteristic);
      }
      return characteristic;
    }

    async readValue(serviceUUID, characteristicUUID) {
      const characteristic = await this.getCharacteristic(serviceUUID, characteristicUUID);
      const value = await characteristic.readValue();
      return this.decodeValue(value);
    }

    async writeValue(serviceUUID, characteristicUUID, data, options = {}) {
      const characteristic = await this.getCharacteristic(serviceUUID, characteristicUUID);
      
      const encoded = this.encodeValue(data);
      
      if (options.withResponse !== false) {
        await characteristic.writeValueWithResponse(encoded);
      } else {
        await characteristic.writeValue(encoded);
      }

      this.dispatchEvent(new CustomEvent('bluetooth-write', {
        detail: { serviceUUID, characteristicUUID, data }
      }));
    }

    async startNotifications(serviceUUID, characteristicUUID, callback) {
      const characteristic = await this.getCharacteristic(serviceUUID, characteristicUUID);

      characteristic.addEventListener('characteristicvaluechanged', (event) => {
        const value = this.decodeValue(event.target.value);
        callback(value);
      });

      await characteristic.startNotifications();
    }

    decodeValue(value) {
      if (!value) return null;

      const dataView = value;
      let result;

      if (value.getUint8) {
        if (value.byteLength === 1) {
          result = dataView.getUint8(0);
        } else if (value.byteLength === 2) {
          result = dataView.getUint16(0, true);
        } else if (value.byteLength === 4) {
          result = dataView.getUint32(0, true);
        } else {
          const buffer = new Uint8Array(value.buffer);
          result = Array.from(buffer);
        }
      } else {
        const decoder = new TextDecoder('utf-8');
        result = decoder.decode(value);
      }

      return result;
    }

    encodeValue(data) {
      if (data instanceof Uint8Array) {
        return data;
      }
      if (typeof data === 'string') {
        return new TextEncoder().encode(data);
      }
      if (typeof data === 'number') {
        const buffer = new ArrayBuffer(4);
        const view = new DataView(buffer);
        view.setUint32(0, data, true);
        return new Uint8Array(buffer);
      }
      if (Array.isArray(data)) {
        return new Uint8Array(data);
      }
      return new Uint8Array(0);
    }

    handleDisconnect() {
      this.isConnected = false;
      this.services.clear();
      this.characteristics.clear();

      this.dispatchEvent(new CustomEvent('bluetooth-disconnect'));
    }

    delay(ms) {
      return new Promise(resolve => setTimeout(resolve, ms));
    }

    isBluetoothSupported() {
      return 'bluetooth' in navigator;
    }

    get connected() {
      return this.isConnected;
    }

    getDevice() {
      return this.device;
    }
  }

  class MQTTClient {
    constructor(config = {}) {
      this.config = {
        clientId: config.clientId || `iot_${Math.random().toString(16).substr(2, 8)}`,
        host: config.host || 'localhost',
        port: config.port || 8083,
        useTLS: config.useTLS !== false,
        username: config.username,
        password: config.password,
        keepAlive: config.keepAlive || 60,
        cleanSession: config.cleanSession !== false
      };
      this.socket = null;
      this.isConnected = false;
      this.subscriptions = new Map();
      this.messageHandlers = new Map();
      this.buffer = new Uint8Array(0);
      this.reconnectAttempts = 0;
      this.maxReconnectAttempts = 10;
    }

    async connect() {
      return new Promise((resolve, reject) => {
        const protocol = this.config.useTLS ? 'wss:' : 'ws:';
        const url = `${protocol}//${this.config.host}:${this.config.port}/mqtt`;

        this.socket = new WebSocket(url, ['mqtt']);

        this.socket.onopen = () => {
          this.sendConnect();
        };

        this.socket.onmessage = (event) => {
          this.handleMessage(event.data);
        };

        this.socket.onclose = () => {
          this.handleDisconnect();
          if (this.reconnectAttempts < this.maxReconnectAttempts) {
            this.reconnect();
          }
        };

        this.socket.onerror = (error) => {
          reject(error);
        };
      });
    }

    sendConnect() {
      const packet = this.encodeConnect({
        clientId: this.config.clientId,
        username: this.config.username,
        password: this.config.password,
        keepAlive: this.config.keepAlive,
        cleanSession: this.config.cleanSession
      });
      this.socket.send(packet);
    }

    encodeConnect(options) {
      const flags = (options.cleanSession ? 0x02 : 0) | (options.username ? 0x80 : 0) | (options.password ? 0x40 : 0);
      const buffer = [0x10];

      const payload = JSON.stringify({
        clientId: options.clientId,
        username: options.username,
        password: options.password,
        keepalive: options.keepAlive
      });

      const payloadBytes = new TextEncoder().encode(payload);
      const remainingLength = payloadBytes.length + 2;

      buffer.push(remainingLength);

      return new Uint8Array([...buffer, ...payloadBytes]);
    }

    handleMessage(data) {
      const bytes = new Uint8Array(data);
      const type = bytes[0] >> 4;

      switch (type) {
        case 2:
          this.isConnected = true;
          this.reconnectAttempts = 0;
          this.dispatchEvent(new CustomEvent('mqtt-connect'));
          this.resubscribe();
          break;
        case 3:
          this.handlePublish(bytes);
          break;
        case 9:
          this.handleSubscribeAck(bytes);
          break;
        case 13:
          this.handlePublishAck(bytes);
          break;
      }
    }

    handlePublish(bytes) {
      const topicLength = (bytes[2] << 8) | bytes[3];
      const topic = new TextDecoder().decode(bytes.slice(4, 4 + topicLength));
      const payload = bytes.slice(4 + topicLength);

      const handler = this.messageHandlers.get(topic);
      if (handler) {
        handler(topic, payload);
      }

      this.dispatchEvent(new CustomEvent('mqtt-message', {
        detail: { topic, payload },
        bubbles: true,
        composed: true
      }));
    }

    publish(topic, payload, options = {}) {
      if (!this.isConnected) return;

      const packet = this.encodePublish(topic, payload, options);
      this.socket.send(packet);
    }

    encodePublish(topic, payload, options = {}) {
      const topicBytes = new TextEncoder().encode(topic);
      const payloadBytes = payload instanceof Uint8Array ? payload : new TextEncoder().encode(String(payload));

      const fixedHeader = 0x30 | (options.qos || 0);
      const remainingLength = 2 + topicBytes.length + payloadBytes.length;

      return new Uint8Array([fixedHeader, remainingLength, ...topicBytes, ...payloadBytes]);
    }

    subscribe(topic, handler) {
      if (!this.isConnected) {
        this.subscriptions.set(topic, handler);
        return;
      }

      const packet = this.encodeSubscribe(topic);
      this.socket.send(packet);

      if (handler) {
        this.messageHandlers.set(topic, handler);
      }
    }

    encodeSubscribe(topic) {
      const topicBytes = new TextEncoder().encode(topic);
      const packetId = Math.floor(Math.random() * 65535);

      const fixedHeader = 0x82;
      const remainingLength = 2 + 2 + topicBytes.length + 1;

      const buffer = new ArrayBuffer(remainingLength);
      const view = new DataView(buffer);

      view.setUint8(0, fixedHeader);
      view.setUint8(1, remainingLength - 2);
      view.setUint16(2, packetId);

      let offset = 4;
      for (let i = 0; i < topicBytes.length; i++) {
        view.setUint8(offset++, topicBytes[i]);
      }
      view.setUint8(offset, 0);

      return new Uint8Array(buffer);
    }

    unsubscribe(topic) {
      if (!this.isConnected) {
        this.subscriptions.delete(topic);
        return;
      }

      this.messageHandlers.delete(topic);
    }

    resubscribe() {
      this.subscriptions.forEach((handler, topic) => {
        this.subscribe(topic, handler);
      });
    }

    handleDisconnect() {
      this.isConnected = false;
      this.dispatchEvent(new CustomEvent('mqtt-disconnect'));
    }

    async reconnect() {
      this.reconnectAttempts++;
      await this.delay(this.config.retryDelay || 1000);
      await this.connect();
    }

    delay(ms) {
      return new Promise(resolve => setTimeout(resolve, ms));
    }

    get connected() {
      return this.isConnected;
    }
  }

  class IoTDeviceManager {
    constructor(config = {}) {
      this.config = config;
      this.bluetoothManager = null;
      this.mqttClient = null;
    }

    async discoverDevices(type = 'bluetooth', options = {}) {
      if (type === 'bluetooth') {
        this.bluetoothManager = new BluetoothManager(this.config);
        return this.bluetoothManager.scan(options);
      }
      return [];
    }

    async connectToMQTT(config) {
      this.mqttClient = new MQTTClient(config);
      await this.mqttClient.connect();
      return this.mqttClient;
    }

    getBluetooth() {
      return this.bluetoothManager;
    }

    getMQTT() {
      return this.mqttClient;
    }
  }

  class IoTDeviceCard extends HTMLElement {
    static get observedAttributes() {
      return ['device-id', 'status', 'type'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._deviceId = '';
      this._status = 'offline';
      this._deviceType = 'generic';
      this._battery = 100;
      this._signal = 0;
    }

    static get observedAttributes() {
      return ['device-id', 'status', 'type', 'battery', 'signal'];
    }

    connectedCallback() {
      this._deviceId = this.getAttribute('device-id') || '';
      this._status = this.getAttribute('status') || 'offline';
      this._deviceType = this.getAttribute('type') || 'generic';
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'status') {
          this._status = newValue;
        } else if (name === 'battery') {
          this._battery = parseInt(newValue, 10);
        } else if (name === 'signal') {
          this._signal = parseInt(newValue, 10);
        }
        this.updateStatus();
      }
    }

    updateStatus() {
      const status = this.shadowRoot.querySelector('.status-text');
      const indicator = this.shadowRoot.querySelector('.status-indicator');
      
      if (status) {
        status.textContent = this._status;
      }
      
      if (indicator) {
        indicator.className = `status-indicator ${this._status}`;
      }
    }

    updateBattery(value) {
      this._battery = value;
      this.render();
    }

    updateSignal(value) {
      this._signal = value;
      this.render();
    }

    render() {
      const icons = {
        light: '💡',
        thermostat: '🌡️',
        camera: '📷',
        lock: '🔒',
        sensor: '📡',
        speaker: '🔊',
        generic: '📱'
      };

      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .device-card {
            border: 1px solid var(--color-border, #e5e7eb);
            border-radius: 12px;
            padding: 16px;
            background: var(--color-background, #fff);
            font-family: system-ui, -apple-system, sans-serif;
          }
          .device-header {
            display: flex;
            align-items: center;
            gap: 12px;
          }
          .device-icon {
            font-size: 32px;
          }
          .device-info {
            flex: 1;
          }
          .device-name {
            font-weight: 600;
            font-size: 16px;
          }
          .status-indicator {
            display: inline-block;
            width: 8px;
            height: 8px;
            border-radius: 50%;
            margin-right: 6px;
          }
          .status-indicator.online {
            background: #10b981;
          }
          .status-indicator.offline {
            background: #ef4444;
          }
          .status-indicator.connecting {
            background: #f59e0b;
            animation: pulse 1s infinite;
          }
          .status-text {
            font-size: 14px;
            color: var(--color-text-secondary, #6b7280);
          }
          @keyframes pulse {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.5; }
          }
          .device-stats {
            display: flex;
            gap: 16px;
            margin-top: 12px;
          }
          .stat {
            font-size: 12px;
            color: var(--color-text-secondary, #6b7280);
          }
        </style>
        <div class="device-card">
          <div class="device-header">
            <span class="device-icon">${icons[this._deviceType] || icons.generic}</span>
            <div class="device-info">
              <div class="device-name">${this._deviceId || 'IoT Device'}</div>
              <div class="device-status">
                <span class="status-indicator ${this._status}"></span>
                <span class="status-text">${this._status}</span>
              </div>
            </div>
          </div>
          <div class="device-stats">
            <span class="stat">Battery: ${this._battery}%</span>
            <span class="stat">Signal: ${this._signal}%</span>
          </div>
        </div>
      `;
    }
  }

  class SensorDashboard extends HTMLElement {
    static get observedAttributes() {
      return ['sensors'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._sensors = [];
      this._sensorData = new Map();
    }

    static get observedAttributes() {
      return ['sensors', 'update-interval'];
    }

    connectedCallback() {
      const sensorsAttr = this.getAttribute('sensors');
      if (sensorsAttr) {
        try {
          this._sensors = JSON.parse(sensorsAttr);
        } catch (e) {
          this._sensors = [];
        }
      }
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue && name === 'sensors') {
        try {
          this._sensors = JSON.parse(newValue);
          this.render();
        } catch (e) {
          console.error('Invalid sensors JSON:', e);
        }
      }
    }

    addSensor(sensor) {
      this._sensors.push(sensor);
      this.render();
    }

    updateSensor(sensorId, data) {
      this._sensorData.set(sensorId, data);
      this.renderSensor(sensorId, data);
    }

    renderSensor(sensorId, data) {
      const value = this.shadowRoot.querySelector(`[data-sensor="${sensorId}"] .value`);
      if (value) {
        value.textContent = data.value;
      }
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .sensor-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 16px;
            padding: 16px;
            font-family: system-ui, -apple-system, sans-serif;
          }
          .sensor-card {
            border: 1px solid var(--color-border, #e5e7eb);
            border-radius: 8px;
            padding: 16px;
            background: var(--color-background, #fff);
          }
          .sensor-name {
            font-size: 14px;
            color: var(--color-text-secondary, #6b7280);
            margin-bottom: 8px;
          }
          .sensor-value {
            font-size: 24px;
            font-weight: 600;
          }
          .sensor-unit {
            font-size: 14px;
            color: var(--color-text-secondary, #6b7280);
          }
        </style>
        <div class="sensor-grid">
          ${this._sensors.map(sensor => `
            <div class="sensor-card" data-sensor="${sensor.id}">
              <div class="sensor-name">${sensor.name}</div>
              <div class="sensor-value">--${sensor.unit || ''}</div>
            </div>
          `).join('')}
        </div>
      `;
    }
  }

  class SmartLightControl extends HTMLElement {
    static get observedAttributes() {
      return ['brightness', 'color', 'power'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._brightness = 100;
      this._color = '#ffffff';
      this._power = false;
      this._mqttClient = null;
      this._topic = '';
    }

    static get observedAttributes() {
      return ['brightness', 'color', 'power', 'topic'];
    }

    connectedCallback() {
      this._brightness = parseInt(this.getAttribute('brightness') || '100', 10);
      this._color = this.getAttribute('color') || '#ffffff';
      this._power = this.hasAttribute('power');
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'brightness') {
          this._brightness = parseInt(newValue, 10);
        } else if (name === 'color') {
          this._color = newValue;
        } else if (name === 'power') {
          this._power = newValue !== null;
        }
        this.publishState();
      }
    }

    setMqtt(client, topic) {
      this._mqttClient = client;
      this._topic = topic;
    }

    setBrightness(value) {
      this._brightness = Math.max(0, Math.min(100, value));
      this.setAttribute('brightness', this._brightness);
      this.publishState();
    }

    setColor(color) {
      this._color = color;
      this.setAttribute('color', color);
      this.publishState();
    }

    toggle() {
      this._power = !this._power;
      if (this._power) {
        this.setAttribute('power', '');
      } else {
        this.removeAttribute('power');
      }
      this.publishState();
    }

    publishState() {
      if (this._mqttClient && this._topic) {
        this._mqttClient.publish(this._topic, {
          power: this._power,
          brightness: this._brightness,
          color: this._color
        });
      }
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .light-control {
            padding: 16px;
            border: 1px solid var(--color-border, #e5e7eb);
            border-radius: 8px;
            background: var(--color-background, #fff);
            font-family: system-ui, -apple-system, sans-serif;
          }
          .power-button {
            width: 48px;
            height: 48px;
            border-radius: 50%;
            border: 2px solid var(--color-border, #e5e7eb);
            background: ${this._color};
            cursor: pointer;
            transition: all 0.2s;
          }
          .power-button.off {
            opacity: 0.4;
          }
          .slider {
            width: 100%;
            margin-top: 12px;
          }
          .color-picker {
            width: 100%;
            height: 32px;
            margin-top: 12px;
            border: none;
            cursor: pointer;
          }
        </style>
        <div class="light-control">
          <button class="power-button ${this._power ? '' : 'off'}" aria-label="Toggle light"></button>
          <input type="range" class="slider" min="0" max="100" value="${this._brightness}" aria-label="Brightness" />
          <input type="color" class="color-picker" value="${this._color}" aria-label="Color" />
        </div>
      `;
    }
  }

  customElements.define('iot-device-card', IoTDeviceCard);
  customElements.define('sensor-dashboard', SensorDashboard);
  customElements.define('smart-light-control', SmartLightControl);

  if (typeof window !== 'undefined') {
    window.IoTComponents = {
      IOT_PROTOCOLS,
      BLUETOOTH_SERVICES,
      BluetoothManager,
      MQTTClient,
      IoTDeviceManager,
      IoTDeviceCard,
      SensorDashboard,
      SmartLightControl
    };
  }

  export {
    IOT_PROTOCOLS,
    BLUETOOTH_SERVICES,
    BluetoothManager,
    MQTTClient,
    IoTDeviceManager,
    IoTDeviceCard,
    SensorDashboard,
    SmartLightControl
  };
})();