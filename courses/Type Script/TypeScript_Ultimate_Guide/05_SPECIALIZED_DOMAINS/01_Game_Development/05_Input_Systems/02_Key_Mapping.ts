/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 05_Input_Systems
 * Topic: 02_Key_Mapping
 * Purpose: Define key mapping and input binding types
 * Difficulty: intermediate
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers, Node.js 18+
 * Performance: O(1) key lookup, hashed action bindings
 * Security: Input validation prevents invalid bindings
 */

namespace KeyMappingTypes {
  export interface InputBinding {
    id: string;
    action: string;
    device: DeviceType;
    input: InputSource;
    modifiers: ModifierCombination[];
    negate: boolean;
  }

  export type DeviceType = 'keyboard' | 'mouse' | 'touch' | 'gamepad';

  export interface InputSource {
    type: 'key' | 'button' | 'axis' | 'pointer';
    id: string;
  }

  export interface ModifierCombination {
    shift: boolean;
    ctrl: boolean;
    alt: boolean;
    meta: boolean;
  }

  export interface InputAction {
    name: string;
    type: ActionType;
    defaultBinding: InputBinding;
    alternativeBindings: InputBinding[];
    holdTime?: number;
    repeatRate?: number;
  }

  export type ActionType = 'digital' | 'analog' | 'trigger' | 'toggle';

  export interface ActionMap {
    id: string;
    name: string;
    context: string;
    actions: Map<string, InputAction>;
    bindings: Map<string, InputBinding[]>;
  }

  export interface InputContext {
    name: string;
    maps: ActionMap[];
    priority: number;
    parent?: string;
  }

  export interface KeyMapping {
    fromKey: string;
    toAction: string;
    modifiers?: ModifierCombination;
    context?: string;
  }

  export interface AxisBinding {
    positive: InputBinding;
    negative: InputBinding;
    deadzone: number;
    sensitivity: number;
    invert: boolean;
  }

  export interface VirtualButton {
    name: string;
    bindings: InputBinding[];
    requiresAll: boolean;
  }

  export interface InputMapper {
    mapKey(key: string, action: string, context?: string): void;
    unmapKey(key: string, action: string, context?: string): void;
    getBinding(action: string, context?: string): InputBinding | undefined;
    getBindingsForKey(key: string, context?: string): InputBinding[];
    loadMappings(mappings: KeyMapping[]): void;
    saveMappings(): KeyMapping[];
    resetToDefaults(context?: string): void;
  }

  export interface InputActionState {
    isPressed: boolean;
    wasPressed: boolean;
    wasReleased: boolean;
    value: number;
    active: boolean;
  }

  export interface InputProcessor {
    processInput(device: InputDevice, deltaTime: number): Map<string, InputActionState>;
    getActionState(action: string): InputActionState;
    isActionActive(action: string): boolean;
    isActionJustPressed(action: string): boolean;
    isActionJustReleased(action: string): boolean;
    getActionValue(action: string): number;
  }

  export const DEFAULT_DEADZONE = 0.15;
  export const DEFAULT_SENSITIVITY = 1.0;
  export const DEFAULT_HOLD_TIME = 0.3;
  export const DEFAULT_REPEAT_RATE = 0.1;
}

// Cross-reference: 01_Input_Device_Types.ts (devices), 03_Gamepad_Types.ts (gamepad)
console.log("\n=== Key Mapping Types ===");
console.log("Related: 01_Input_Device_Types.ts, 03_Gamepad_Types.ts");