/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 04_Audio_Integration
 * Topic: 02_Sound_Types
 * Purpose: Define sound effect types for audio system
 * Difficulty: intermediate
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers, Node.js 18+
 * Performance: O(1) sound lookup, streaming for large files
 * Security: Validated audio formats prevent code execution
 */

namespace SoundTypes {
  export type SoundCategory = 'sfx' | 'ui' | 'ambient' | 'voice' | 'weapon' | 'vehicle' | 'impact';

  export interface Sound {
    id: string;
    name: string;
    category: SoundCategory;
    buffer: AudioBuffer;
    duration: number;
    sampleRate: number;
    channels: number;
    bitDepth: number;
    metadata: SoundMetadata;
  }

  export interface SoundMetadata {
    designer: string;
    created: number;
    tags: string[];
    variations: string[];
  }

  export interface SoundInstance {
    id: string;
    sound: Sound;
    source: AudioSource;
    volume: number;
    pitch: number;
    pan: number;
    position: Vector3D;
    looping: boolean;
    state: PlaybackState;
  }

  export type PlaybackState = 'stopped' | 'playing' | 'paused';

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface SoundVariation {
    id: string;
    sounds: Sound[];
    selectionMode: 'random' | 'sequential' | 'weighted';
  }

  export interface SoundGroup {
    id: string;
    name: string;
    volume: number;
    mute: boolean;
    sounds: Map<string, Sound>;
  }

  export interface SoundPool {
    id: string;
    sound: Sound;
    maxInstances: number;
    instances: SoundInstance[];
    priority: number;
  }

  export interface SoundOptions {
    volume?: number;
    pitch?: number;
    pan?: number;
    position?: Vector3D;
    loop?: boolean;
    fadeIn?: number;
    fadeOut?: number;
    startTime?: number;
    variation?: string;
  }

  export interface SoundMixer {
    addGroup(group: SoundGroup): void;
    removeGroup(groupId: string): void;
    setGroupVolume(groupId: string, volume: number): void;
    setGroupMute(groupId: string, muted: boolean): void;
    playSound(sound: Sound, options?: SoundOptions): SoundInstance;
  }

  export interface Sound3D {
    sound: Sound;
    position: Vector3D;
    velocity: Vector3D;
    distance: number;
    rolloffFactor: number;
    refDistance: number;
    maxDistance: number;
    coneInnerAngle: number;
    coneOuterAngle: number;
    coneOuterGain: number;
  }
}

// Cross-reference: 01_Audio_Engine_Types.ts (engine), 03_Music_Types.ts (music)
console.log("\n=== Sound Types ===");
console.log("Related: 01_Audio_Engine_Types.ts, 03_Music_Types.ts");