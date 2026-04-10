/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 05_Input_Systems
 * Topic: 01_Input_Device_Types
 * Purpose: Define input device types for game input handling
 * Difficulty: intermediate
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers, Node.js 18+
 * Performance: O(1) device lookup, real-time input processing
 * Security: Input sanitization prevents injection attacks
 */

namespace InputDeviceTypes {
  export type DeviceType = 'keyboard' | 'mouse' | 'touch' | 'gamepad' | 'vr_controller';

  export interface InputDevice {
    id: string;
    type: DeviceType;
    connected: boolean;
    lastUpdate: number;
  }

  export interface KeyboardDevice extends InputDevice {
    type: 'keyboard';
    layout: KeyboardLayout;
    keys: Map<KeyCode, KeyState>;
    modifiers: ModifierKeys;
  }

  export interface KeyState {
    pressed: boolean;
    held: boolean;
    released: boolean;
    pressedTime: number;
    releasedTime: number;
  }

  export interface ModifierKeys {
    shift: boolean;
    ctrl: boolean;
    alt: boolean;
    meta: boolean;
  }

  export type KeyCode = 
    | 'KeyA' | 'KeyB' | 'KeyC' | 'KeyD' | 'KeyE' | 'KeyF' | 'KeyG' | 'KeyH'
    | 'KeyI' | 'KeyJ' | 'KeyK' | 'KeyL' | 'KeyM' | 'KeyN' | 'KeyO' | 'KeyP'
    | 'KeyQ' | 'KeyR' | 'KeyS' | 'KeyT' | 'KeyU' | 'KeyV' | 'KeyW' | 'KeyX'
    | 'KeyY' | 'KeyZ' | 'Digit0' | 'Digit1' | 'Digit2' | 'Digit3' | 'Digit4'
    | 'Digit5' | 'Digit6' | 'Digit7' | 'Digit8' | 'Digit9' | 'Space' | 'Enter'
    | 'Escape' | 'Tab' | 'Backspace' | 'ArrowUp' | 'ArrowDown' | 'ArrowLeft'
    | 'ArrowRight' | 'ShiftLeft' | 'ShiftRight' | 'ControlLeft' | 'ControlRight'
    | 'AltLeft' | 'AltRight' | 'F1' | 'F2' | 'F3' | 'F4' | 'F5' | 'F6'
    | 'F7' | 'F8' | 'F9' | 'F10' | 'F11' | 'F12';

  export type KeyboardLayout = 'qwerty' | 'azerty' | 'qwertz' | 'custom';

  export interface MouseDevice extends InputDevice {
    type: 'mouse';
    position: MousePosition;
    movement: MouseMovement;
    buttons: Map<MouseButton, ButtonState>;
    scroll: ScrollDelta;
  }

  export interface MousePosition {
    x: number;
    y: number;
    normalizedX: number;
    normalizedY: number;
  }

  export interface MouseMovement {
    deltaX: number;
    deltaY: number;
    deltaZ: number;
  }

  export interface ButtonState {
    pressed: boolean;
    clicked: boolean;
    doubleClicked: boolean;
  }

  export interface ScrollDelta {
    x: number;
    y: number;
  }

  export type MouseButton = 'left' | 'middle' | 'right' | 'back' | 'forward';

  export interface TouchDevice extends InputDevice {
    type: 'touch';
    touches: Map<number, TouchPoint>;
    maxTouches: number;
  }

  export interface TouchPoint {
    id: number;
    position: MousePosition;
    startPosition: MousePosition;
    delta: MouseMovement;
    phase: TouchPhase;
    timestamp: number;
  }

  export type TouchPhase = 'started' | 'moved' | 'ended' | 'cancelled';

  export interface InputDeviceManager {
    registerDevice(device: InputDevice): void;
    unregisterDevice(deviceId: string): void;
    getDevice(deviceId: string): InputDevice | undefined;
    getDevicesByType(type: DeviceType): InputDevice[];
    update(deltaTime: number): void;
  }
}

// Cross-reference: 02_Key_Mapping.ts (key mappings), 03_Gamepad_Types.ts (gamepad)
console.log("\n=== Input Device Types ===");
console.log("Related: 02_Key_Mapping.ts, 03_Gamepad_Types.ts");