/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 06_Network_Multiplayer
 * Topic: 01_Network_Protocol_Types
 * Purpose: Define network protocol types for multiplayer games
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) message processing, O(log n) routing
 * Security: Encrypted channels prevent eavesdropping
 */

namespace NetworkProtocolTypes {
  export type ProtocolType = 'tcp' | 'udp' | 'websockets' | 'webrtc';
  export type ConnectionState = 'disconnected' | 'connecting' | 'connected' | 'reconnecting';

  export interface NetworkMessage {
    id: string;
    type: MessageType;
    payload: Uint8Array;
    timestamp: number;
    channel: MessageChannel;
    reliable: boolean;
    encrypted: boolean;
  }

  export type MessageType = 'player_join' | 'player_leave' | 'player_update' | 'game_state' | 'chat' | 'custom';
  export type MessageChannel = 'reliable' | 'unreliable' | 'fragmented';

  export interface NetworkPeer {
    id: string;
    address: string;
    port: number;
    state: ConnectionState;
    roundTripTime: number;
    lastReceived: number;
    bandwidth: BandwidthStats;
  }

  export interface BandwidthMetrics {
    bytesSent: number;
    bytesReceived: number;
    packetsSent: number;
    packetsReceived: number;
    packetLoss: number;
    roundTripTime: number;
  }

  export interface BandwidthStats {
    upload: number;
    download: number;
    ping: number;
    quality: ConnectionQuality;
  }

  export type ConnectionQuality = 'excellent' | 'good' | 'fair' | 'poor' | 'critical';

  export interface NetworkPacket {
    id: string;
    sequence: number;
    timestamp: number;
    data: Uint8Array;
    size: number;
  }

  export interface PacketHeader {
    protocolVersion: number;
    messageType: MessageType;
    sequence: number;
    timestamp: number;
    sourceId: string;
    destinationId: string;
    flags: PacketFlags;
  }

  export interface PacketFlags {
    reliable: boolean;
    encrypted: boolean;
    compressed: boolean;
    fragmented: boolean;
  }

  export interface NetworkConfig {
    protocol: ProtocolType;
    serverAddress: string;
    serverPort: number;
    reconnectAttempts: number;
    reconnectDelay: number;
    timeout: number;
    maxPacketSize: number;
  }

  export interface NetworkClient {
    id: string;
    peer: NetworkPeer;
    messageQueue: NetworkMessage[];
    isConnected: boolean;
    connect(config: NetworkConfig): Promise<void>;
    disconnect(): void;
    send(message: NetworkMessage): void;
    receive(): NetworkMessage | null;
  }

  export interface NetworkServer {
    id: string;
    peers: Map<string, NetworkPeer>;
    maxConnections: number;
    start(config: NetworkConfig): Promise<void>;
    stop(): void;
    broadcast(message: NetworkMessage, exclude?: string[]): void;
    sendTo(peerId: string, message: NetworkMessage): void;
  }
}

// Cross-reference: 02_Sync_Types.ts (synchronization), 03_Lobby_Types.ts (lobbies)
console.log("\n=== Network Protocol Types ===");
console.log("Related: 02_Sync_Types.ts, 03_Lobby_Types.ts");