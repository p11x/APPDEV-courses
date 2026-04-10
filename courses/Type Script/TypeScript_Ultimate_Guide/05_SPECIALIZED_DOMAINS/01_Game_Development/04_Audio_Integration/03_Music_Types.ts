/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 04_Audio_Integration
 * Topic: 03_Music_Types
 * Purpose: Define music and background audio types
 * Difficulty: intermediate
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers, Node.js 18+
 * Performance: Streaming audio reduces memory usage
 * Security: Validated audio formats prevent exploits
 */

namespace MusicTypes {
  export type MusicLayer = 'base' | 'melody' | 'harmony' | 'percussion' | 'ambient';

  export interface MusicTrack {
    id: string;
    name: string;
    layers: MusicLayerData[];
    bpm: number;
    timeSignature: [number, number];
    duration: number;
    loop: boolean;
    fadeIn: number;
    fadeOut: number;
  }

  export interface MusicLayerData {
    layer: MusicLayer;
    audioBuffer?: AudioBuffer;
    volume: number;
    mute: boolean;
    solo: boolean;
  }

  export interface MusicPlayer {
    id: string;
    currentTrack: MusicTrack | null;
    state: MusicState;
    currentTime: number;
    layers: Map<MusicLayer, boolean>;
    volume: number;
    crossfade: CrossfadeSettings;
  }

  export type MusicState = 'stopped' | 'playing' | 'paused' | 'transitioning';

  export interface CrossfadeSettings {
    enabled: boolean;
    duration: number;
    curve: 'linear' | 'exponential' | 'sine';
  }

  export interface MusicTransition {
    from: MusicTrack;
    to: MusicTrack;
    type: TransitionType;
    duration: number;
    trigger: TransitionTrigger;
  }

  export type TransitionType = 'crossfade' | 'cut' | 'fadeOutIn' | 'custom';
  export type TransitionTrigger = 'manual' | 'automatic' | 'event';

  export interface MusicCue {
    id: string;
    name: string;
    trackId: string;
    time: number;
    action: CueAction;
    parameters: Record<string, unknown>;
  }

  export type CueAction = 'play_sound' | 'stop_music' | 'change_layer' | 'set_volume' | 'custom';

  export interface MusicSystem {
    loadTrack(url: string): Promise<MusicTrack>;
    play(track: MusicTrack): void;
    pause(): void;
    stop(): void;
    setVolume(volume: number): void;
    crossfade(track: MusicTrack, settings?: CrossfadeSettings): void;
    triggerCue(cueId: string): void;
    subscribeToStateChange(callback: (state: MusicState) => void): () => void;
  }

  export interface DynamicMusic {
    baseLayer: MusicLayerData;
    adaptiveLayers: Map<MusicLayer, AdaptiveLayerSettings>;
    intensity: number;
    updateIntensity(value: number): void;
  }

  export interface AdaptiveLayerSettings {
    enabled: boolean;
    minIntensity: number;
    maxIntensity: number;
    fadeTime: number;
  }

  export interface MusicBank {
    id: string;
    tracks: Map<string, MusicTrack>;
    categories: Map<string, string[]>;
    loadCategory(category: string): Promise<MusicTrack[]>;
  }
}

// Cross-reference: 01_Audio_Engine_Types.ts (engine), 02_Sound_Types.ts (sounds)
console.log("\n=== Music Types ===");
console.log("Related: 01_Audio_Engine_Types.ts, 02_Sound_Types.ts");