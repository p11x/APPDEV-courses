/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 05_IoT
 * Concept: 02_Communication_Protocols
 * Topic: 02_CoAP_Types
 * Purpose: Define CoAP protocol types for IoT communication
 * Difficulty: intermediate
 * UseCase: IoT
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Embedded Systems
 * Performance: Lightweight, low overhead, UDP-based
 * Security: DTLS encryption, OSCORE
 */

namespace CoAPTypes {
  export type CoAPVersion = 1;

  export interface CoAPConfig {
    host: string;
    port: number;
    protocol: 'coap' | 'coaps';
    ackTimeout: number;
    ackRandomFactor: number;
    maxRetransmit: number;
    maxLatency: number;
    dtls?: DTLSConfig;
  }

  export interface DTLSConfig {
    enabled: boolean;
    verifyClient: boolean;
    ca?: string;
    cert?: string;
    key?: string;
    psk?: { identity: string; key: string };
  }

  export interface CoAPMessage {
    version: CoAPVersion;
    type: MessageType;
    code: CoAPCode;
    id: number;
    token?: string;
    options: CoAPOption[];
    payload?: Uint8Array;
  }

  export type MessageType = 'con' | 'non' | 'ack' | 'rst';

  export interface CoAPCode {
    class: number;
    detail: number;
    toString(): string;
  }

  export const CoAPCodes = {
    GET: { class: 0, detail: 1 },
    POST: { class: 0, detail: 2 },
    PUT: { class: 0, detail: 3 },
    DELETE: { class: 0, detail: 4 },
    CREATED: { class: 2, detail: 1 },
    DELETED: { class: 2, detail: 2 },
    VALID: { class: 2, detail: 3 },
    CHANGED: { class: 2, detail: 4 },
    CONTENT: { class: 2, detail: 5 },
    CONTINUE: { class: 2, detail: 31 },
    BAD_REQUEST: { class: 4, detail: 0 },
    UNAUTHORIZED: { class: 4, detail: 1 },
    NOT_FOUND: { class: 4, detail: 4 },
    METHOD_NOT_ALLOWED: { class: 4, detail: 5 },
    NOT_ACCEPTABLE: { class: 4, detail: 6 },
    REQUEST_ENTITY_INCOMPLETE: { class: 4, detail: 8 },
    CONFLICT: { class: 4, detail: 9 },
    INTERNAL_SERVER_ERROR: { class: 5, detail: 0 },
    NOT_IMPLEMENTED: { class: 5, detail: 1 },
    BAD_GATEWAY: { class: 5, detail: 2 },
    SERVICE_UNAVAILABLE: { class: 5, detail: 3 },
    GATEWAY_TIMEOUT: { class: 5, detail: 4 },
    PROXYING_NOT_SUPPORTED: { class: 5, detail: 5 },
  };

  export interface CoAPOption {
    number: OptionNumber;
    value: string | Uint8Array;
  }

  export type OptionNumber = 
    | 1  // If-Match
    | 3  // Uri-Host
    | 4  // Etag
    | 5  // If-None-Match
    | 6  // Observe
    | 7  // Uri-Port
    | 8  // Location-Path
    | 9  // Uri-Path
    | 11 // Content-Format
    | 12 // Max-Age
    | 14 // Accept
    | 15 // Location-Query
    | 17 // Proxy-Uri
    | 20 // Size1
    | 23 // ETag
    | 27 // Observe;

  export interface CoAPRequest extends CoAPMessage {
    method: 'GET' | 'POST' | 'PUT' | 'DELETE';
    observe?: number;
  }

  export interface CoAPResponse extends CoAPMessage {
    statusCode: CoAPCode;
  }

  export interface CoAPClient {
    request(request: CoAPRequest): Promise<CoAPResponse>;
    get(path: string, options?: RequestOptions): Promise<CoAPResponse>;
    post(path: string, payload: Uint8Array, contentFormat?: ContentFormat): Promise<CoAPResponse>;
    put(path: string, payload: Uint8Array, contentFormat?: ContentFormat): Promise<CoAPResponse>;
    delete(path: string): Promise<CoAPResponse>;
    observe(path: string, callback: (response: CoAPResponse) => void): Promise<number>;
    cancelObserve(observeNumber: number): void;
  }

  export interface RequestOptions {
    timeout?: number;
    confirmable?: boolean;
    token?: string;
    blockSize?: BlockSize;
    contentFormat?: ContentFormat;
    accept?: ContentFormat;
    ifMatch?: string;
    ifNoneMatch?: boolean;
  }

  export type ContentFormat = 
    | 'text/plain'
    | 'application/json'
    | 'application/xml'
    | 'application/octet-stream'
    | 'application/exi'
    | 'application/cbor'
    | 'application/link-format';

  export type BlockSize = 16 | 32 | 64 | 128 | 256 | 512 | 1024;

  export interface BlockOption {
    blockNumber: number;
    more: boolean;
    size: BlockSize;
  }

  export interface ObserveOption {
    value: number;
    rel?: number;
  }

  export interface Resource {
    path: string;
    contentType: ContentFormat;
    observable: boolean;
    observableCallback?: (request: CoAPRequest) => void;
    get?: (request: CoAPRequest) => Promise<CoAPResponse>;
    post?: (request: CoAPRequest) => Promise<CoAPResponse>;
    put?: (request: CoAPRequest) => Promise<CoAPResponse>;
    delete?: (request: CoAPRequest) => Promise<CoAPResponse>;
  }

  export interface CoAPServer {
    register(resource: Resource): void;
    unregister(path: string): void;
    start(): Promise<void>;
    stop(): Promise<void>;
  }

  export interface OSCOREConfig {
    masterSalt: Uint8Array;
    masterSecret: Uint8Array;
    idContext?: string;
    algorithm: 'AES-CCM-16-64-128' | 'AES-CCM-16-64-256';
  }
}

// Cross-reference: 01_MQTT_Types.ts (alternative protocol), 03_Edge_Device_Types.ts (edge)
console.log("\n=== CoAP Types ===");
console.log("Related: 01_MQTT_Types.ts, 03_Edge_Device_Types.ts");