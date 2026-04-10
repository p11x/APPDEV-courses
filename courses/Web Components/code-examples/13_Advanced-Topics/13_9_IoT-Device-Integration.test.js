/**
 * @group unit
 * @group advanced-topics
 */
import { expect, fixture, html } from '@open-wc/testing';
import './13_9_IoT-Device-Integration.js';

describe('IoT Device Integration', () => {
  it('should have IOT_PROTOCOLS', () => {
    expect(IOT_PROTOCOLS).to.exist;
    expect(IOT_PROTOCOLS.bluetooth).to.be.a('boolean');
    expect(IOT_PROTOCOLS.websocket).to.be.a('boolean');
  });

  it('should have BLUETOOTH_SERVICES', () => {
    expect(BLUETOOTH_SERVICES).to.exist;
    expect(BLUETOOTH_SERVICES.battery).to.equal('180f');
    expect(BLUETOOTH_SERVICES.heartRate).to.equal('180d');
    expect(BLUETOOTH_SERVICES.deviceInfo).to.equal('180a');
    expect(BLUETOOTH_SERVICES.temperature).to.equal('1809');
  });

  it('should have DEFAULT_CONFIG', () => {
    expect(DEFAULT_CONFIG.scanTimeout).to.equal(10000);
    expect(DEFAULT_CONFIG.connectionTimeout).to.equal(5000);
    expect(DEFAULT_CONFIG.retryAttempts).to.equal(3);
    expect(DEFAULT_CONFIG.heartbeatInterval).to.equal(30000);
  });
});

describe('BluetoothManager', () => {
  let manager;

  beforeEach(() => {
    manager = new BluetoothManager();
  });

  it('should create manager', () => {
    expect(manager).to.exist;
    expect(manager.device).to.be.null;
  });

  it('should have config', () => {
    expect(manager.config.scanTimeout).to.equal(10000);
  });

  it('should scan', () => {
    expect(manager.scan).to.be.a('function');
  });

  it('should connect', () => {
    expect(manager.connect).to.be.a('function');
  });
});

describe('MQTTClient', () => {
  let client;

  beforeEach(() => {
    client = new MQTTClient('mqtt://localhost');
  });

  it('should create client', () => {
    expect(client).to.exist;
    expect(client.brokerUrl).to.equal('mqtt://localhost');
  });

  it('should connect', () => {
    expect(client.connect).to.be.a('function');
  });

  it('should publish', () => {
    expect(client.publish).to.be.a('function');
  });

  it('should subscribe', () => {
    expect(client.subscribe).to.be.a('function');
  });
});