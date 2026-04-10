/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 06_AR_VR
 * Concept: 03_Mixed_Reality
 * Topic: 01_Spatial_Mapping
 * Purpose: Define spatial mapping and mixed reality types
 * Difficulty: advanced
 * UseCase: AR/VR
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers (WebXR), Node.js 18+
 * Performance: Real-time mesh reconstruction, 60fps processing
 * Security: Spatial data privacy, boundary detection
 */

namespace SpatialMappingTypes {
  export type MappingMode = 'realtime' | 'offline' | 'hybrid';
  export type SurfaceType = 'floor' | 'ceiling' | 'wall' | 'table' | 'object' | 'unknown';

  export interface SpatialMap {
    id: string;
    mode: MappingMode;
    timestamp: number;
    bounds: SpatialBounds;
    surfaces: SpatialSurface[];
    meshes: SpatialMesh[];
    anchors: SpatialAnchor[];
  }

  export interface SpatialBounds {
    min: Vector3D;
    max: Vector3D;
    center: Vector3D;
    radius: number;
  }

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface SpatialSurface {
    id: string;
    type: SurfaceType;
    transform: SurfaceTransform;
    bounds: SurfaceBounds;
    polygon: Vector3D[];
    normal: Vector3D;
    area: number;
    confidence: number;
    mesh?: SpatialMesh;
    texture?: SurfaceTexture;
  }

  export interface SurfaceTransform {
    position: Vector3D;
    rotation: Quaternion;
    scale: Vector3D;
  }

  export interface Quaternion {
    x: number;
    y: number;
    z: number;
    w: number;
  }

  export interface SurfaceBounds {
    min: Vector3D;
    max: Vector3D;
    width: number;
    height: number;
  }

  export interface SurfaceTexture {
    type: 'color' | 'normal' | 'depth' | 'semantic';
    resolution: { width: number; height: number };
    data: Uint8Array | Float32Array;
  }

  export interface SpatialMesh {
    id: string;
    vertices: Float32Array;
    indices: Uint16Array;
    normals: Float32Array;
    uvs?: Float32Array;
    colors?: Uint8Array;
    material?: MeshMaterial;
    bounds: SpatialBounds;
    resolution: number;
    updateTime: number;
  }

  export interface MeshMaterial {
    albedo?: Vector3D;
    roughness: number;
    metallic: number;
    opacity: number;
    semantic?: SemanticLabel;
  }

  export type SemanticLabel = 
    | 'wall' | 'floor' | 'ceiling' | 'window' | 'door' 
    | 'table' | 'chair' | 'sofa' | 'bed' | 'tv' 
    | 'lamp' | 'plant' | 'unknown';

  export interface SpatialAnchor {
    id: string;
    transform: SpatialTransform;
    trackingState: AnchorTrackingState;
    label?: string;
    metadata?: Record<string, unknown>;
  }

  export interface SpatialTransform {
    position: Vector3D;
    rotation: Quaternion;
  }

  export type AnchorTrackingState = 'tracked' | 'limited' | 'not_tracked';

  export interface SpatialMapper {
    initialize(config: SpatialMapperConfig): Promise<void>;
    update(frame: XRFrame): Promise<SpatialMap>;
    getSurfaces(type?: SurfaceType): SpatialSurface[];
    getMesh(surfaceId: string): SpatialMesh | null;
    createAnchor(transform: SpatialTransform): Promise<SpatialAnchor>;
    removeAnchor(anchorId: string): void;
    exportMesh(format: MeshFormat): Promise<ArrayBuffer>;
  }

  export interface SpatialMapperConfig {
    mode: MappingMode;
    maxSurfaces: number;
    meshResolution: number;
    updateFrequency: number;
    semanticSegmentation: boolean;
    planeDetection: boolean;
    occlusion: boolean;
    spatialAnchors: boolean;
  }

  export type MeshFormat = 'obj' | 'ply' | 'glb' | 'stl';

  export interface OcclusionData {
    depthTexture: XRDepthBuffer;
    meshOcclusion: boolean;
    computeOcclusion(meshId: string, position: Vector3D): Promise<OcclusionResult>;
  }

  export interface XRDepthBuffer {
    width: number;
    height: number;
    near: number;
    far: number;
    data: Float32Array;
  }

  export interface OcclusionResult {
    visible: boolean;
    distance: number;
    confidence: number;
  }

  export interface SpatialQuery {
    bounds?: { center: Vector3D; radius: number };
    type?: SurfaceType[];
    minArea?: number;
    maxArea?: number;
    includeMeshes?: boolean;
  }

  export interface PlaneDetector {
    detectPlanes(frame: XRFrame): Promise<XRPlane[]>;
    getPlanes(): XRPlane[];
    subscribe(callback: (planes: XRPlane[]) => void): () => void;
  }

  export interface XRPlane {
    id: string;
    orientation: 'horizontal' | 'vertical';
    position: Vector3D;
    polygon: Vector3D[];
    transform: SpatialTransform;
    lastUpdated: number;
  }

  export interface MeshReconstruction {
    enabled: boolean;
    resolution: number;
    faces: number;
    vertices: number;
    updateMesh(meshId: string): Promise<void>;
    simplify(factor: number): Promise<SpatialMesh>;
    smooth(iterations: number): Promise<SpatialMesh>;
  }

  export interface SpatialAudio {
    listenerPosition: Vector3D;
    listenerOrientation: Quaternion;
    occlusions: Map<string, AudioOcclusion>;
  }

  export interface AudioOcclusion {
    meshId: string;
    material: AudioMaterial;
    attenuation: number;
  }

  export interface AudioMaterial {
    absorption: number;
    scattering: number;
  }

  export interface MixedRealityFusion {
    arSession?: ARSession;
    vrSession?: VRSession;
    spatialMap: SpatialMap;
    integrate(session: XRFrame): Promise<FusionResult>;
  }

  export interface FusionResult {
    worldTransform: SpatialTransform;
    occlusions: OcclusionResult[];
    surfaceMatches: SurfaceMatch[];
  }

  export interface SurfaceMatch {
    arSurface: XRPlane;
    vrPlane: XRPlane;
    transform: SpatialTransform;
    confidence: number;
  }

  export interface ARSession {
    mode: string;
    environment: unknown;
  }

  export interface VRSession {
    mode: string;
    referenceSpace: unknown;
  }

  export interface XRFrame {
    session: unknown;
    timestamp: number;
  }
}

// Cross-reference: 01_AR_Session_Types.ts (AR), 01_VR_Session_Types.ts (VR)
console.log("\n=== Spatial Mapping Types ===");
console.log("Related: 01_AR_Session_Types.ts, 01_VR_Session_Types.ts");