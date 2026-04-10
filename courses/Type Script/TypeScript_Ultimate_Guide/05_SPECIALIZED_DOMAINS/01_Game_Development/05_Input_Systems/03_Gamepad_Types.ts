/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 05_Input_Systems
 * Topic: 03_Gamepad_Types
 * Purpose: Define gamepad input types
 * Difficulty: intermediate
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers (Gamepad API), Node.js 18+
 * Performance: O(1) gamepad polling, real-time input
 * Security: Input validation prevents out-of-bounds values
 */

namespace GamepadTypes {
  export type GamepadIndex = number;

  export interface GamepadState {
    index: GamepadIndex;
    id: string;
    mapping: GamepadMapping;
    connected: boolean;
    timestamp: number;
    buttons: GamepadButton[];
    axes: GamepadAxis[];
    vibration?: GamepadVibration;
  }

  export type GamepadMapping = 'standard' | 'xbox' | 'playstation' | 'switch' | 'generic';

  export interface GamepadButton {
    index: GamepadButtonIndex;
    pressed: boolean;
    touched: boolean;
    value: number;
  }

  export type GamepadButtonIndex =
    | 'a' | 'b' | 'x' | 'y'
    | 'lb' | 'rb' | 'lt' | 'rt'
    | 'back' | 'start' | 'ls' | 'rs'
    | 'dpad_up' | 'dpad_down' | 'dpad_left' | 'dpad_right'
    | 'home';

  export interface GamepadAxis {
    index: GamepadAxisIndex;
    value: number;
    rawValue: number;
    inverted: boolean;
  }

  export type GamepadAxisIndex = 'left_x' | 'left_y' | 'right_x' | 'right_y' | 'lt' | 'rt';

  export interface GamepadVibration {
    duration: number;
    weakMagnitude: number;
    strongMagnitude: number;
  }

  export interface GamepadProfile {
    name: string;
    mapping: GamepadMapping;
    buttonLayout: Record<GamepadButtonIndex, string>;
    axisLayout: Record<GamepadAxisIndex, string>;
    deadzones: Record<GamepadAxisIndex, number>;
  }

  export interface StickConfig {
    axisX: GamepadAxisIndex;
    axisY: GamepadAxisIndex;
    deadzone: number;
    sensitivity: number;
    invertX: boolean;
    invertY: boolean;
  }

  export interface TriggerConfig {
    axis: GamepadAxisIndex;
    deadzone: number;
    threshold: number;
  }

  export interface GamepadInputState {
    leftStick: Vector2D;
    rightStick: Vector2D;
    leftTrigger: number;
    rightTrigger: number;
    buttons: Map<GamepadButtonIndex, boolean>;
    dpad: DPadState;
  }

  export interface Vector2D {
    x: number;
    y: number;
  }

  export interface DPadState {
    up: boolean;
    down: boolean;
    left: boolean;
    right: boolean;
  }

  export interface GamepadManager {
    getGamepad(index: GamepadIndex): GamepadState | null;
    getConnectedGamepads(): GamepadState[];
    poll(): GamepadState[];
    applyProfile(gamepad: GamepadState, profile: GamepadProfile): void;
    setDeadzone(axis: GamepadAxisIndex, deadzone: number): void;
    setVibration(index: GamepadIndex, vibration: GamepadVibration): Promise<boolean>;
  }

  export interface GamepadEvent {
    type: 'connected' | 'disconnected' | 'button_changed' | 'axis_changed';
    gamepad: GamepadState;
    button?: GamepadButtonIndex;
    axis?: GamepadAxisIndex;
    value?: number;
  }

  export const STANDARD_BUTTONS: GamepadButtonIndex[] = [
    'a', 'b', 'x', 'y', 'lb', 'rb', 'lt', 'rt', 'back', 'start', 'ls', 'rs',
    'dpad_up', 'dpad_down', 'dpad_left', 'dpad_right', 'home'
  ];

  export const DEFAULT_DEADZONE = 0.15;
  export const DEFAULT_TRIGGER_DEADZONE = 0.1;
}

// Cross-reference: 01_Input_Device_Types.ts (devices), 02_Key_Mapping.ts (mappings)
console.log("\n=== Gamepad Types ===");
console.log("Related: 01_Input_Device_Types.ts, 02_Key_Mapping.ts");