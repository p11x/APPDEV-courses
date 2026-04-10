/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 06_AR_VR
 * Concept: 02_VR_Types
 * Topic: 02_Controller_Types
 * Purpose: Define VR controller types
 * Difficulty: intermediate
 * UseCase: AR/VR
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers (WebXR), Node.js 18+
 * Performance: 1000Hz tracking, haptic feedback
 * Security: Input validation prevents unauthorized access
 */

namespace ControllerTypes {
  export type ControllerType = 'hand' | 'motion' | 'eye' | 'body';

  export interface VRController {
    id: string;
    type: ControllerType;
    handedness: Handedness;
    transform: ControllerTransform;
    input: ControllerInput;
    tracking: ControllerTracking;
    capabilities: ControllerCapabilities;
  }

  export interface Handedness {
    side: 'left' | 'right' | 'none';
    dominant: boolean;
  }

  export interface ControllerTransform {
    position: Vector3D;
    rotation: Quaternion;
    scale: Vector3D;
    timestamp: number;
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

  export interface ControllerInput {
    select: InputState;
    squeeze: InputState;
    thumbstick?: JoystickState;
    touchpad?: TouchpadState;
    buttons: ButtonState[];
    trigger: TriggerState;
    grip: TriggerState;
  }

  export interface InputState {
    pressed: boolean;
    touched: boolean;
    value: number;
  }

  export interface JoystickState {
    x: number;
    y: number;
    pressed: boolean;
    touched: boolean;
  }

  export interface TouchpadState {
    x: number;
    y: number;
    touched: boolean;
    pressure: number;
  }

  export interface ButtonState {
    buttonId: string;
    pressed: boolean;
    touched: boolean;
    value: number;
  }

  export interface TriggerState {
    pressed: boolean;
    value: number;
    analog: number;
  }

  export interface ControllerTracking {
    trackingState: TrackingState;
    linearVelocity: Vector3D;
    angularVelocity: Quaternion;
    accuracy: TrackingAccuracy;
    lastUpdate: number;
  }

  export type TrackingState = 'tracked' | 'excluded' | 'hidden';

  export type TrackingAccuracy = 'high' | 'medium' | 'low';

  export interface ControllerCapabilities {
    hasThumbstick: boolean;
    hasTouchpad: boolean;
    hasTrigger: boolean;
    hasGrip: boolean;
    hasHapticActuator: boolean;
    hasVibrationActuator: boolean;
    hasEyeTracking: boolean;
    hasFingerTracking: boolean;
    supportedHaptics: HapticType[];
  }

  export type HapticType = 'vibration' | 'dual' | 'forceFeedback';

  export interface HandController extends VRController {
    type: 'hand';
    skeleton: HandSkeleton;
    gestures: HandGesture[];
    handMesh: HandMesh;
  }

  export interface HandSkeleton {
    joints: SkeletonJoint[];
    bones: SkeletonBone[];
    handedness: 'left' | 'right';
  }

  export interface SkeletonJoint {
    jointId: string;
    name: string;
    parent: string;
    position: Vector3D;
    rotation: Quaternion;
  }

  export interface SkeletonBone {
    boneId: string;
    startJoint: string;
    endJoint: string;
    length: number;
  }

  export interface HandGesture {
    name: string;
    fingers: FingerGesture[];
    confidence: number;
  }

  export interface FingerGesture {
    finger: FingerName;
    state: 'open' | 'closed' | 'pointing';
  }

  export type FingerName = 'thumb' | 'index' | 'middle' | 'ring' | 'pinky';

  export interface HandMesh {
    vertices: Float32Array;
    indices: Uint16Array;
    normals: Float32Array;
    uvs: Float32Array;
    skinIndices?: Uint16Array;
    skinWeights?: Float32Array;
  }

  export interface MotionController extends VRController {
    type: 'motion';
    trackingType: '6dof' | '3dof';
    sensorFusion: SensorFusion;
    calibration: ControllerCalibration;
  }

  export interface SensorFusion {
    accelerometer: boolean;
    gyroscope: boolean;
    magnetometer: boolean;
    filterType: 'complementary' | 'kalman' | 'madgwick';
  }

  export interface ControllerCalibration {
    autoZero: boolean;
    userOffset: Vector3D;
    rotationOffset: Quaternion;
  }

  export interface EyeController extends VRController {
    type: 'eye';
    gaze: GazeData;
    vergence: VergenceData;
    blink: BlinkData;
    pupil: PupilData;
  }

  export interface GazeData {
    origin: Vector3D;
    direction: Vector3D;
    hitPoint?: Vector3D;
    hitObject?: string;
  }

  export interface VergenceData {
    distance: number;
    angle: number;
  }

  export interface BlinkData {
    leftClosed: boolean;
    rightClosed: boolean;
    blinkRate: number;
  }

  export interface PupilData {
    diameter: number;
    position: Vector2D;
  }

  export interface Vector2D {
    x: number;
    y: number;
  }

  export interface HapticActuator {
    id: string;
    type: HapticType;
    maxAmplitude: number;
    maxDuration: number;
    frequencies?: number[];
    pulse(intensity: number, duration: number): Promise<void>;
    vibrate(pattern: HapticPattern): Promise<void>;
  }

  export interface HapticPattern {
    duration: number;
    intensity: number;
    frequency?: number;
    delay?: number;
    repeat?: number;
  }

  export interface ControllerManager {
    getControllers(): VRController[];
    getController(id: string): VRController | undefined;
    onControllerConnected(callback: (controller: VRController) => void): void;
    onControllerDisconnected(callback: (controllerId: string) => void): void;
  }
}

// Cross-reference: 01_VR_Session_Types.ts (VR sessions), 01_AR_Session_Types.ts (AR sessions)
console.log("\n=== Controller Types ===");
console.log("Related: 01_VR_Session_Types.ts, 01_AR_Session_Types.ts");