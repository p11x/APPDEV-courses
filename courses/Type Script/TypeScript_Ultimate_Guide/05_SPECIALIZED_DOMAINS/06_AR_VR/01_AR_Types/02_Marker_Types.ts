/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 06_AR_VR
 * Concept: 01_AR_Types
 * Topic: 02_Marker_Types
 * Purpose: Define AR marker and tracking types
 * Difficulty: intermediate
 * UseCase: AR/VR
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers, Node.js 18+
 * Performance: Real-time marker detection, multiple markers
 * Security: Marker validation prevents spoofing
 */

namespace MarkerTypes {
  export type MarkerType = 'qr' | 'barcode' | 'fiducial' | 'natural' | 'image' | 'nft';

  export interface ARMarker {
    id: string;
    type: MarkerType;
    transform: MarkerTransform;
    trackingState: TrackingState;
    confidence: number;
    lastDetected: number;
  }

  export interface MarkerTransform {
    position: Vector3D;
    rotation: Quaternion;
    scale: Vector3D;
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

  export type TrackingState = 'tracked' | 'limited' | 'not_tracked';

  export interface QRMarker extends ARMarker {
    type: 'qr';
    content: string;
    errorCorrection: 'L' | 'M' | 'Q' | 'H';
    version: number;
    size: number;
  }

  export interface BarcodeMarker extends ARMarker {
    type: 'barcode';
    format: BarcodeFormat;
    content: string;
    bounds: BoundingBox;
  }

  export type BarcodeFormat = 
    | 'qr_code' | 'ean_13' | 'ean_8' | 'upc_a' | 'upc_e' 
    | 'code_128' | 'code_39' | 'code_93' | 'itf' | 'pdf417' | 'aztec' | 'data_matrix';

  export interface BoundingBox {
    x: number;
    y: number;
    width: number;
    height: number;
  }

  export interface FiducialMarker extends ARMarker {
    type: 'fiducial';
    family: FiducialFamily;
    id: number;
    size: number;
    physicalSize: number;
    occluded: boolean;
  }

  export type FiducialFamily = 'aruco' | 'chessboard' | 'charuco' | 'apriltag' | 'caltag';

  export interface NaturalFeatureMarker extends ARMarker {
    type: 'natural';
    descriptors: FeatureDescriptor[];
    keypoints: Keypoint[];
    confidence: number;
    descriptorType: 'orb' | 'sift' | 'surf' | 'akaze';
  }

  export interface FeatureDescriptor {
    type: 'binary' | 'float';
    data: Float32Array | Uint8Array;
    length: number;
  }

  export interface Keypoint {
    x: number;
    y: number;
    size: number;
    angle: number;
    octave: number;
    response: number;
  }

  export interface ImageMarker extends ARMarker {
    type: 'image';
    imageSource: ImageSource;
    matchScore: number;
    physicalWidth?: number;
  }

  export interface ImageSource {
    url?: string;
    base64?: string;
    canvas?: HTMLCanvasElement;
    texture?: WebGLTexture;
  }

  export interface NFTMarker extends ARMarker {
    type: 'nft';
    tokenId: string;
    contractAddress: string;
    metadata: NFTMetadata;
    imageData: ImageSource;
  }

  export interface NFTMetadata {
    name: string;
    description: string;
    image: string;
    animationUrl?: string;
    externalUrl?: string;
    attributes?: NFTAttribute[];
  }

  export interface NFTAttribute {
    trait_type: string;
    value: string | number;
    display_type?: string;
  }

  export interface MarkerTracker {
    initialize(config: MarkerTrackerConfig): Promise<void>;
    detect(image: ImageSource): Promise<ARMarker[]>;
    track(markerId: string): Promise<ARMarker | null>;
    addMarker(marker: ARMarker): void;
    removeMarker(markerId: string): void;
    setEnabled(enabled: boolean): void;
  }

  export interface MarkerTrackerConfig {
    markerTypes: MarkerType[];
    maxMarkers: number;
    updateRate: number;
    confidenceThreshold: number;
    smoothing: boolean;
    smoothingFactor: number;
  }

  export interface MarkerTrackerResult {
    markers: ARMarker[];
    processingTime: number;
    timestamp: number;
    trackingFeatures: TrackingFeatures;
  }

  export interface TrackingFeatures {
    frameCount: number;
    droppedFrames: number;
    averageConfidence: number;
  }

  export interface MarkerBasedAR {
    addMarker(marker: ARMarker): void;
    removeMarker(markerId: string): void;
    getMarkerTransform(markerId: string): MarkerTransform | null;
    setMarkerEnabled(markerId: string, enabled: boolean): void;
    getTrackedMarkers(): ARMarker[];
  }

  export interface MultiMarkerConfig {
    type: 'linear' | 'planar' | 'arbitrary';
    markers: string[];
    transform: MarkerTransform;
    confidence: number;
  }
}

// Cross-reference: 01_AR_Session_Types.ts (AR sessions), 02_Controller_Types.ts (VR controllers)
console.log("\n=== Marker Types ===");
console.log("Related: 01_AR_Session_Types.ts, 02_Controller_Types.ts");