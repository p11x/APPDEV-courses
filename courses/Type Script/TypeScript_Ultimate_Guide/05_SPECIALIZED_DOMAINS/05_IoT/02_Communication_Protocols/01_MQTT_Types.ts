/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 05_IoT
 * Concept: 02_Communication_Protocols
 * Topic: 01_MQTT_Types
 * Purpose: Define MQTT protocol types for IoT communication
 * Difficulty: intermediate
 * UseCase: IoT
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(1) publish/subscribe, lightweight overhead
 * Security: TLS encryption, client authentication
 */

namespace MQTTTypes {
  export type MQTTVersion = '3.1' | '3.1.1' | '5.0';

  export interface MQTTConfig {
    clientId: string;
    host: string;
    port: number;
    protocol: 'mqtt' | 'mqtts' | 'ws' | 'wss';
    version?: MQTTVersion;
    cleanSession: boolean;
    keepAlive: number;
    connectTimeout: number;
    reconnectPeriod: number;
    credentials?: Credentials;
    will?: WillMessage;
  }

  export interface Credentials {
    username?: string;
    password?: string;
    clientKey?: string;
    clientCert?: string;
    ca?: string;
  }

  export interface WillMessage {
    topic: string;
    payload: string;
    qos: QoS;
    retain: boolean;
  }

  export type QoS = 0 | 1 | 2;

  export interface ConnectPacket {
    type: 'connect';
    protocolName: string;
    protocolVersion: number;
    flags: ConnectFlags;
    keepAlive: number;
    clientId: string;
    will?: WillMessage;
    username?: string;
    password?: string;
  }

  export interface ConnectFlags {
    username: boolean;
    password: boolean;
    willRetain: boolean;
    willQoS: QoS;
    will: boolean;
    cleanSession: boolean;
  }

  export interface PublishPacket {
    type: 'publish';
    packetId?: number;
    topic: string;
    qos: QoS;
    retain: boolean;
    dup: boolean;
    payload: Uint8Array;
  }

  export interface SubscribePacket {
    type: 'subscribe';
    packetId: number;
    subscriptions: Subscription[];
  }

  export interface Subscription {
    topic: string;
    qos: QoS;
    noLocal?: boolean;
    retainAsPublished?: boolean;
    retainHandling?: 'send' | 'send_on_new' | 'not_send';
  }

  export interface UnsubscribePacket {
    type: 'unsubscribe';
    packetId: number;
    topics: string[];
  }

  export interface PubackPacket {
    type: 'puback';
    packetId: number;
  }

  export interface PubrecPacket {
    type: 'pubrec';
    packetId: number;
  }

  export interface PubrelPacket {
    type: 'pubrel';
    packetId: number;
  }

  export interface PubcompPacket {
    type: 'pubcomp';
    packetId: number;
  }

  export interface PingreqPacket {
    type: 'pingreq';
  }

  export interface PingrespPacket {
    type: 'pingresp';
  }

  export interface DisconnectPacket {
    type: 'disconnect';
    reasonCode?: number;
    properties?: Record<string, unknown>;
  }

  export interface MQTTMessage {
    topic: string;
    payload: string | Uint8Array;
    qos: QoS;
    retain: boolean;
    dup: boolean;
    packetId?: number;
    timestamp: number;
  }

  export interface MQTTClient {
    connect(): Promise<void>;
    disconnect(): Promise<void>;
    subscribe(topics: Subscription[]): Promise<number>;
    unsubscribe(topics: string[]): Promise<void>;
    publish(topic: string, payload: string | Uint8Array, qos?: QoS, retain?: boolean): Promise<number>;
    on(event: 'message' | 'connect' | 'disconnect' | 'error' | 'offline' | 'reconnect', callback: (...args: unknown[]) => void): void;
    removeListener(event: string, callback: (...args: unknown[]) => void): void;
  }

  export interface MQTTBroker {
    id: string;
    address: string;
    port: number;
    clients: number;
    topics: string[];
    maxMessageSize: number;
    maxConnections: number;
  }

  export interface MessageQueue {
    topic: string;
    messages: MQTTMessage[];
    maxSize: number;
    overflow: 'drop_oldest' | 'drop_newest' | 'block';
  }

  export interface RetainedMessage {
    topic: string;
    payload: Uint8Array;
    qos: QoS;
  }

  export interface LastWill {
    topic: string;
    payload: Uint8Array;
    qos: QoS;
    retain: boolean;
  }
}

// Cross-reference: 02_CoAP_Types.ts (alternative protocol), 01_Sensor_Types.ts (data)
console.log("\n=== MQTT Types ===");
console.log("Related: 02_CoAP_Types.ts, 01_Sensor_Types.ts");