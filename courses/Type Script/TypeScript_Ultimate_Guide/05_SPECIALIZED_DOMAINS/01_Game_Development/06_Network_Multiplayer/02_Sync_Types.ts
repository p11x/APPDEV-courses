/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 06_Network_Multiplayer
 * Topic: 02_Sync_Types
 * Purpose: Define synchronization types for multiplayer games
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) entity updates, interpolation for smoothness
 * Security: Server authority prevents client manipulation
 */

namespace SyncTypes {
  export type SyncMode = 'authoritative' | 'deterministic' | 'interpolated' | 'extrapolated';

  export interface SyncState {
    frame: number;
    timestamp: number;
    entities: SyncEntity[];
  }

  export interface SyncEntity {
    id: string;
    type: EntityType;
    position: Vector3D;
    rotation: Quaternion;
    velocity: Vector3D;
    state: Record<string, unknown>;
  }

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface Quaternion {
    x: number;
    y: number;
    z: number;
    w: number;
  }

  export type EntityType = 'player' | 'npc' | 'projectile' | 'vehicle' | 'object';

  export interface SyncMessage {
    type: 'full' | 'delta' | 'input' | 'correction';
    state: SyncState;
    baseFrame: number;
  }

  export interface InputFrame {
    frame: number;
    playerId: string;
    inputs: InputState[];
    timestamp: number;
  }

  export interface InputState {
    type: InputType;
    data: unknown;
  }

  export type InputType = 'move' | 'aim' | 'action' | 'chat' | 'custom';

  export interface StateUpdate {
    entityId: string;
    property: string;
    value: unknown;
    timestamp: number;
    method: UpdateMethod;
  }

  export type UpdateMethod = 'set' | 'add' | 'multiply' | 'lerp' | 'slerp';

  export interface InterpolationSettings {
    bufferSize: number;
    maxInterpolationTime: number;
    extrapolationTime: number;
    snapThreshold: number;
  }

  export interface SyncConfig {
    mode: SyncMode;
    updateRate: number;
    compression: boolean;
    interpolation: InterpolationSettings;
    prediction: PredictionSettings;
  }

  export interface PredictionSettings {
    enabled: boolean;
    localExtrapolation: boolean;
    serverReconciliation: boolean;
    inputDelay: number;
  }

  export interface EntitySync {
    entityId: string;
    syncPolicy: SyncPolicy;
    lastSync: number;
    pendingUpdates: StateUpdate[];
  }

  export interface SyncPolicy {
    sendRate: number;
    priority: number;
    fullSyncInterval: number;
    deltaCompression: boolean;
  }

  export interface NetworkSyncer {
    sync(state: SyncState): void;
    interpolate(current: SyncState, target: SyncState, alpha: number): SyncState;
    extrapolate(state: SyncState, time: number): SyncState;
    reconcile(serverState: SyncState): void;
  }
}

// Cross-reference: 01_Network_Protocol_Types.ts (network), 03_Lobby_Types.ts (lobbies)
console.log("\n=== Sync Types ===");
console.log("Related: 01_Network_Protocol_Types.ts, 03_Lobby_Types.ts");