/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 06_Network_Multiplayer
 * Topic: 03_Lobby_Types
 * Purpose: Define lobby and matchmaking types
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) player matching, O(log n) lobby lookup
 * Security: Session tokens prevent unauthorized access
 */

namespace LobbyTypes {
  export type LobbyState = 'pending' | 'waiting' | 'starting' | 'active' | 'finished' | 'cancelled';
  export type LobbyVisibility = 'public' | 'private' | 'friends' | 'invite_only';

  export interface Lobby {
    id: string;
    name: string;
    host: PlayerInfo;
    players: PlayerInfo[];
    maxPlayers: number;
    minPlayers: number;
    state: LobbyState;
    visibility: LobbyVisibility;
    password?: string;
    settings: LobbySettings;
    createdAt: number;
  }

  export interface PlayerInfo {
    id: string;
    name: string;
    avatar?: string;
    ready: boolean;
    team?: string;
    latency: number;
    skills: SkillRating;
  }

  export interface SkillRating {
    rank: number;
    rating: number;
    wins: number;
    losses: number;
  }

  export interface LobbySettings {
    map?: string;
    gameMode: string;
    duration: number;
    allowLateJoin: boolean;
    friendlyFire: boolean;
    maxTeams: number;
    teams: TeamConfig[];
    rules: Record<string, unknown>;
  }

  export interface TeamConfig {
    name: string;
    color: string;
    maxPlayers: number;
    autoBalance: boolean;
  }

  export interface MatchmakingConfig {
    region: string;
    gameMode: string;
    minPlayers: number;
    maxPlayers: number;
    skillRange: SkillRange;
    timeout: number;
  }

  export interface SkillRange {
    min: number;
    max: number;
    tolerance: number;
  }

  export interface MatchmakingQueue {
    id: string;
    config: MatchmakingConfig;
    players: PlayerInfo[];
    status: QueueStatus;
    estimatedWait: number;
  }

  export type QueueStatus = 'searching' | 'matching' | 'creating' | 'ready' | 'failed';

  export interface MatchResult {
    lobbyId: string;
    winner: string;
    duration: number;
    scores: Map<string, number>;
    playerStats: PlayerMatchStats[];
  }

  export interface PlayerMatchStats {
    playerId: string;
    kills: number;
    deaths: number;
    assists: number;
    damage: number;
    healing: number;
  }

  export interface LobbyManager {
    createLobby(settings: LobbySettings): Promise<Lobby>;
    joinLobby(lobbyId: string, password?: string): Promise<Lobby>;
    leaveLobby(): Promise<void>;
    startGame(): Promise<void>;
    kickPlayer(playerId: string): void;
    setReady(ready: boolean): void;
    updateSettings(settings: Partial<LobbySettings>): void;
  }

  export interface Matchmaker {
    enqueue(config: MatchmakingConfig): Promise<MatchmakingQueue>;
    dequeue(): Promise<void>;
    getStatus(): QueueStatus;
  }
}

// Cross-reference: 01_Network_Protocol_Types.ts (network), 02_Sync_Types.ts (sync)
console.log("\n=== Lobby Types ===");
console.log("Related: 01_Network_Protocol_Types.ts, 02_Sync_Types.ts");