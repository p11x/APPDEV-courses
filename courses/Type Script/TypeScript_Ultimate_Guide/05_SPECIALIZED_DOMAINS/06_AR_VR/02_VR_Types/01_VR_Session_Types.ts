/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 06_AR_VR
 * Concept: 02_VR_Types
 * Topic: 01_VR_Session_Types
 * Purpose: Define VR session types for virtual reality
 * Difficulty: advanced
 * UseCase: AR/VR
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers (WebXR), Node.js 18+
 * Performance: 90fps rendering, low-latency tracking
 * Security: Session isolation, boundary enforcement
 */

namespace VRSessionTypes {
  export type VRSessionMode = 'immersive-vr' | 'immersive-ar' | 'inline';

  export interface VRSession {
    mode: VRSessionMode;
    immersive: boolean;
    referenceSpace: XRReferenceSpace;
    views: XRView[];
    inputSources: XRInputSource[];
    renderState: XRRenderState;
    environment: VREnvironment;
  }

  export interface XRReferenceSpace {
    type: ReferenceSpaceType;
    transform: XRRigidTransform;
    bounds?: VRBoundary;
  }

  export type ReferenceSpaceType = 
    | 'viewer' 
    | 'local' 
    | 'local-floor' 
    | 'bounded-floor' 
    | 'unbounded';

  export interface XRRigidTransform {
    position: Vector3D;
    orientation: Quaternion;
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

  export interface VRBoundary {
    geometry: Vector3D[];
    bounds: VRBounds;
  }

  export interface VRBounds {
    minX: number;
    maxX: number;
    minZ: number;
    maxZ: number;
  }

  export interface XRView {
    eye: 'left' | 'right';
    projectionMatrix: Float32Array;
    viewMatrix: Float32Array;
    viewport: XRViewport;
  }

  export interface XRViewport {
    x: number;
    y: number;
    width: number;
    height: number;
  }

  export interface XRInputSource {
    inputSourceId: string;
    handedness: Handedness;
    targetRayMode: TargetRayMode;
    targetRaySpace: XRPose;
    gripSpace?: XRPose;
    gamepad?: Gamepad;
    handednessSupport: 'optional' | 'required' | 'none';
  }

  export type Handedness = 'left' | 'right' | 'none';
  export type TargetRayMode = 'gaze' | 'tracked-pointer' | 'screen';

  export interface XRPose {
    transform: XRRigidTransform;
    linearVelocity?: Vector3D;
    angularVelocity?: Quaternion;
    emulatedPosition: boolean;
  }

  export interface XRRenderState {
    baseLayer: XRLayer;
    baseReferenceSpace?: XRReferenceSpace;
    layers?: XRLayer[];
    depthNear: number;
    depthFar: number;
    inlineVerticalFieldOfView?: number;
  }

  export interface XRLayer {
    context: XRWebGLLayer | XRImageLayer;
  }

  export interface VREnvironment {
    lighting: VRLighting;
    audio: VRSpatialAudio;
    haptic: HapticFeedback;
  }

  export interface VRLighting {
    ambientLight: number;
    directionalLights: DirectionalLight[];
    environmentMap?: XRTexture;
  }

  export interface DirectionalLight {
    direction: Vector3D;
    intensity: number;
    color: string;
    castShadow: boolean;
  }

  export interface XRTexture {
    format: 'rgba8' | 'rgba16f' | 'rgba32f';
    width: number;
    height: number;
  }

  export interface VRSpatialAudio {
    listenerPosition: Vector3D;
    listenerOrientation: Quaternion;
    audioContext: AudioContext;
  }

  export interface HapticFeedback {
    vibrate(intensity: number, duration: number): Promise<void>;
    pulse(intensity: number, duration: number, repeat?: number): Promise<void>;
  }

  export interface VRFrame {
    session: VRSession;
    timestamp: number;
    predictedDisplayTime: number;
    views: XRView[];
    inputSources: XRInputSource[];
    worldInformation: WorldInformation;
  }

  export interface WorldInformation {
    referenceSpace: XRReferenceSpace;
    hitTestResults?: XRHitTestResult[];
    planeDetectionResults?: XRPlane[];
  }

  export interface XRHitTestResult {
    hitTestResults: XRPose[];
    timestamp: number;
  }

  export interface XRPlane {
    id: string;
    orientation: 'horizontal' | 'vertical';
    position: Vector3D;
    polygon: Vector3D[];
    lastUpdated: number;
  }

  export interface VRPlayer {
    position: Vector3D;
    rotation: Quaternion;
    height: number;
    scale: number;
    teleportation: TeleportationConfig;
    locomotion: LocomotionConfig;
  }

  export interface TeleportationConfig {
    enabled: boolean;
    fadeDuration: number;
    markerVisible: boolean;
    validSurfaces: ('horizontal' | 'vertical')[];
  }

  export interface LocomotionConfig {
    type: 'smooth' | 'snap' | 'continuous';
    speed: number;
    rotationSpeed: number;
    bounds: VRBounds;
  }

  export interface VRSessionManager {
    requestSession(mode: VRSessionMode, options?: XRSessionInit): Promise<VRSession>;
    endSession(): Promise<void>;
    getSession(): VRSession | null;
    isSessionSupported(mode: VRSessionMode): Promise<boolean>;
  }
}

// Cross-reference: 02_Controller_Types.ts (controllers), 01_AR_Session_Types.ts (AR sessions)
console.log("\n=== VR Session Types ===");
console.log("Related: 02_Controller_Types.ts, 01_AR_Session_Types.ts");