/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 06_AR_VR
 * Concept: 01_AR_Types
 * Topic: 01_AR_Session_Types
 * Purpose: Define AR session types for augmented reality
 * Difficulty: advanced
 * UseCase: AR/VR
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers (WebXR), Node.js 18+
 * Performance: 60fps tracking, real-time rendering
 * Security: Session isolation, camera access control
 */

namespace ARSessionTypes {
  export type ARSessionMode = 'reality' | 'immersive-vr' | 'overlay';

  export interface ARSession {
    mode: ARSessionMode;
    environment: AREnvironment;
    tracking: ARTracking;
    anchors: ARAnchor[];
    lightEstimation: LightEstimation;
    hitTest: HitTestSource;
    referencespace: XRReferenceSpace;
  }

  export interface AREnvironment {
    worldTracking: WorldTrackingState;
    planeDetection: PlaneDetectionState;
    depthDetection: DepthDetectionState;
    hitTest: HitTestState;
  }

  export interface WorldTrackingState {
    trackingState: TrackingState;
    origin: XRRigidTransform;
    mappingState: 'none' | 'limited' | 'mapped';
    features: TrackingFeatures;
  }

  export type TrackingState = 'limited' | 'normal';
  export type TrackingFeatures = 'orientation' | 'position' | 'eye-level' | 'floor' | 'gravity';

  export interface PlaneDetectionState {
    enabled: boolean;
    alignment: 'horizontal' | 'vertical' | 'any';
    detectedPlanes: XRPlane[];
  }

  export interface XRPlane {
    id: string;
    orientation: 'horizontal' | 'vertical';
    position: XRPoint3D;
    polygon: XRPoint3D[];
    lastUpdated: number;
  }

  export interface XRPoint3D {
    x: number;
    y: number;
    z: number;
  }

  export interface DepthDetectionState {
    enabled: boolean;
    dataFormat: 'float32' | 'uint16';
    resolution: XRResolution;
    updateRate: number;
  }

  export interface XRResolution {
    width: number;
    height: number;
  }

  export interface HitTestState {
    enabled: boolean;
    profile: HitTestProfile;
  }

  export type HitTestProfile = 'viewer' | 'target-point' | 'target-plane';

  export interface ARTracking {
    position: XRPoint3D;
    orientation: XRQuaternion;
    linearVelocity: XRPoint3D;
    angularVelocity: XRQuaternion;
    timestamp: number;
    accuracy: TrackingAccuracy;
  }

  export interface XRQuaternion {
    x: number;
    y: number;
    z: number;
    w: number;
  }

  export interface TrackingAccuracy {
    position: 'high' | 'medium' | 'low';
    orientation: 'high' | 'medium' | 'low';
  }

  export interface ARAnchor {
    id: string;
    anchorType: AnchorType;
    transform: XRRigidTransform;
    trackingState: TrackingState;
    parent?: string;
    children?: string[];
    metadata?: Record<string, unknown>;
  }

  export type AnchorType = 'face' | 'plane' | 'point' | 'image' | 'object' | 'body';

  export interface XRRigidTransform {
    position: XRPoint3D;
    orientation: XRQuaternion;
  }

  export interface LightEstimation {
    enabled: boolean;
    ambientIntensity: number;
    ambientColorTemperature: number;
    primaryLightDirection: XRPoint3D;
    primaryLightIntensity: number;
    sphericalHarmonics?: SphericalHarmonics;
  }

  export interface SphericalHarmonics {
    coefficients: number[][];
    type: 'irradiance' | 'radiance';
  }

  export interface HitTestSource {
    requestHitTestSource(options: HitTestOptions): XRHitTestSource;
    cancelHitTestSource(source: XRHitTestSource): void;
  }

  export interface HitTestOptions {
    space: XRReferenceSpace;
    profile: HitTestProfile;
  }

  export interface XRHitTestSource {
    id: number;
    cancel(): void;
  }

  export interface HitTestResult {
    hitTestResults: XRHitTestResult[];
    timestamp: number;
  }

  export interface XRHitTestResult {
    transform: XRRigidTransform;
    anchor?: ARAnchor;
    plane?: XRPlane;
  }

  export interface ARCamera {
    projectionMatrix: Float32Array;
    viewMatrix: Float32Array;
    imagePlane: XRPlane;
    displayTransform: XRRigidTransform;
  }

  export interface ARFrame {
    session: ARSession;
    timestamp: number;
    tracking: ARTracking;
    views: ARView[];
    worldTracking: WorldTrackingState;
    detectedPlanes: XRPlane[];
    anchors: ARAnchor[];
    lightEstimation: LightEstimation;
  }

  export interface ARView {
    eye: 'left' | 'right';
    projectionMatrix: Float32Array;
    viewMatrix: Float32Array;
  }

  export interface ARSessionManager {
    requestSession(mode: ARSessionMode, options?: XRSessionInit): Promise<ARSession>;
    endSession(): Promise<void>;
    getSession(): ARSession | null;
    getReferenceSpace(type: 'viewer' | 'local' | 'local-floor' | 'bounded-floor'): XRReferenceSpace;
  }
}

// Cross-reference: 02_Marker_Types.ts (AR markers), 01_VR_Session_Types.ts (VR sessions)
console.log("\n=== AR Session Types ===");
console.log("Related: 02_Marker_Types.ts, 01_VR_Session_Types.ts");