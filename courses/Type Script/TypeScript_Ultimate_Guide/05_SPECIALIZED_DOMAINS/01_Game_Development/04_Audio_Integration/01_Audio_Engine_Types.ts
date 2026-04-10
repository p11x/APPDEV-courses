/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 04_Audio_Integration
 * Topic: 01_Audio_Engine_Types
 * Purpose: Define audio engine types for game audio
 * Difficulty: intermediate
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers (Web Audio API), Node.js 18+
 * Performance: O(n) concurrent sounds, real-time audio processing
 * Security: Audio context isolation prevents unauthorized access
 */

namespace AudioEngineTypes {
  export type AudioContextState = 'suspended' | 'running' | 'closed';

  export interface AudioEngine {
    context: AudioContext;
    masterGain: GainNode;
    listener: AudioListener;
    initialize(): Promise<void>;
    update(deltaTime: number): void;
    setMasterVolume(volume: number): void;
    suspend(): void;
    resume(): void;
  }

  export interface AudioListener {
    position: Vector3D;
    forward: Vector3D;
    up: Vector3D;
    velocity: Vector3D;
    setPosition(pos: Vector3D): void;
    setOrientation(forward: Vector3D, up: Vector3D): void;
    setVelocity(vel: Vector3D): void;
  }

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface AudioBus {
    name: string;
    gainNode: GainNode;
    nodes: AudioNode[];
    effects: AudioEffect[];
  }

  export interface AudioEffect {
    type: EffectType;
    node: AudioNode;
    enabled: boolean;
    parameters: Record<string, number>;
  }

  export type EffectType = 'reverb' | 'delay' | 'eq' | 'compressor' | 'filter' | 'distortion';

  export interface AudioSource {
    id: string;
    buffer?: AudioBuffer;
    sourceNode: AudioBufferSourceNode | MediaElementAudioSourceNode;
    gainNode: GainNode;
    pannerNode?: PannerNode;
    loop: boolean;
    playbackRate: number;
    spatial: boolean;
    position: Vector3D;
  }

  export interface AudioResource {
    id: string;
    name: string;
    type: ResourceType;
    buffer?: AudioBuffer;
    url: string;
    loaded: boolean;
    loading: boolean;
  }

  export type ResourceType = 'sound' | 'music' | 'voice';

  export interface AudioManager {
    load(id: string, url: string, type: ResourceType): Promise<AudioResource>;
    play(id: string, options?: PlayOptions): AudioSource;
    stop(sourceId: string): void;
    stopAll(): void;
    getActiveSources(): AudioSource[];
  }

  export interface PlayOptions {
    volume?: number;
    loop?: boolean;
    rate?: number;
    position?: Vector3D;
    fadeIn?: number;
  }

  export interface AudioStatistics {
    activeSources: number;
    totalMemory: number;
    decodedSamples: number;
  }
}

// Cross-reference: 02_Sound_Types.ts (sounds), 03_Music_Types.ts (music)
console.log("\n=== Audio Engine Types ===");
console.log("Related: 02_Sound_Types.ts, 03_Music_Types.ts");