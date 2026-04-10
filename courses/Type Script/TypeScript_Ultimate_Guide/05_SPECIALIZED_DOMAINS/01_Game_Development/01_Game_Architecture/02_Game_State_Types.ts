/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 01_Game_Architecture
 * Topic: 02_Game_State_Types
 * Purpose: Define game state types for state management in game development
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(1) state access with proper caching
 * Security: Immutable state patterns prevent direct mutation
 */

namespace GameStateTypes {
  export type GamePhase = 
    | 'loading' 
    | 'menu' 
    | 'playing' 
    | 'paused' 
    | 'gameOver' 
    | 'victory';

  export interface GameState<T = unknown> {
    phase: GamePhase;
    data: T;
    timestamp: number;
    version: number;
  }

  export interface PlayerState {
    id: string;
    name: string;
    health: number;
    maxHealth: number;
    position: Vector3D;
    velocity: Vector3D;
    rotation: EulerAngles;
    inventory: InventoryItem[];
    stats: PlayerStats;
  }

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface EulerAngles {
    pitch: number;
    yaw: number;
    roll: number;
  }

  export interface InventoryItem {
    id: string;
    name: string;
    quantity: number;
    metadata: Record<string, unknown>;
  }

  export interface PlayerStats {
    strength: number;
    agility: number;
    intelligence: number;
    experience: number;
    level: number;
  }

  export interface WorldState {
    entities: EntityState[];
    timeOfDay: number;
    weather: WeatherCondition;
    region: string;
  }

  export interface EntityState {
    id: string;
    type: string;
    position: Vector3D;
    rotation: EulerAngles;
    scale: Vector3D;
    components: Record<string, unknown>;
  }

  export type WeatherCondition = 
    | 'clear' 
    | 'rainy' 
    | 'snowy' 
    | 'foggy' 
    | 'stormy';

  export interface GameStateManager<T extends GameState> {
    getState(): Readonly<T>;
    setState(state: Partial<T>): void;
    subscribe(listener: StateListener<T>): UnsubscribeFn;
    rollback(version: number): void;
  }

  export type StateListener<T> = (state: T, previousState: T) => void;
  export type UnsubscribeFn = () => void;

  class GameStateManagerImpl<T extends GameState> implements GameStateManager<T> {
    private state: T;
    private listeners: Set<StateListener<T>> = new Set();
    private history: T[] = [];
    private maxHistorySize = 100;

    constructor(initialState: T) {
      this.state = { ...initialState };
    }

    getState(): Readonly<T> {
      return Object.freeze({ ...this.state });
    }

    setState(newState: Partial<T>): void {
      const previousState = { ...this.state };
      this.state = { ...this.state, ...newState, version: this.state.version + 1, timestamp: Date.now() };
      this.history.push({ ...previousState });
      if (this.history.length > this.maxHistorySize) {
        this.history.shift();
      }
      this.listeners.forEach(listener => listener(this.state, previousState));
    }

    subscribe(listener: StateListener<T>): UnsubscribeFn {
      this.listeners.add(listener);
      return () => this.listeners.delete(listener);
    }

    rollback(version: number): void {
      const targetState = this.history.find(s => s.version === version);
      if (targetState) {
        this.state = { ...targetState };
      }
    }
  }
}

// Cross-reference: 01_Game_Types.ts (base types), 03_Entity_Component.ts (entity states)
console.log("\n=== Game State Types ===");
console.log("Related: 01_Game_Types.ts, 03_Entity_Component.ts");