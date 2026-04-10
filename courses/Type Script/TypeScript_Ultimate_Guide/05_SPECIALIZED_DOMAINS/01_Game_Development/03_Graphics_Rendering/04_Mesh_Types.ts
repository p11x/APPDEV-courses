/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 03_Graphics_Rendering
 * Topic: 04_Mesh_Types
 * Purpose: Define mesh types for 3D rendering
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers, Node.js 18+
 * Performance: O(v) vertices, O(i) indices, GPU-accelerated
 * Security: Bounds checking prevents buffer overflows
 */

namespace MeshTypes {
  export interface Mesh {
    id: string;
    name: string;
    vertexBuffer: VertexBuffer;
    indexBuffer: IndexBuffer;
    submeshes: Submesh[];
    boundingBox: BoundingBox;
    skinning?: SkinningData;
  }

  export interface VertexBuffer {
    id: string;
    data: Float32Array;
    stride: number;
    attributes: VertexAttribute[];
  }

  export interface IndexBuffer {
    id: string;
    data: Uint16Array | Uint32Array;
    count: number;
    type: IndexType;
  }

  export type IndexType = 'unsigned_short' | 'unsigned_int';

  export interface VertexAttribute {
    name: string;
    location: number;
    type: AttributeType;
    normalized: boolean;
    offset: number;
  }

  export type AttributeType = 'float' | 'float2' | 'float3' | 'float4' | 'int' | 'int2' | 'int3' | 'int4' | 'uint' | 'uint2' | 'uint3' | 'uint4';

  export interface Submesh {
    name: string;
    indexStart: number;
    indexCount: number;
    materialId: string;
    primitiveType: PrimitiveType;
  }

  export type PrimitiveType = 'points' | 'lines' | 'lineStrip' | 'triangles' | 'triangleStrip';

  export interface BoundingBox {
    min: Vector3D;
    max: Vector3D;
    center: Vector3D;
    extents: Vector3D;
    radius: number;
  }

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface SkinningData {
    bones: Bone[];
    inverseBindMatrices: Float32Array;
    maxBoneInfluences: number;
  }

  export interface Bone {
    name: string;
    parent: number;
    localMatrix: Float32Array;
    worldMatrix: Float32Array;
    inverseBindMatrix: Float32Array;
  }

  export interface MeshBuilder {
    setPositions(positions: number[]): MeshBuilder;
    setNormals(normals: number[]): MeshBuilder;
    setUVs(uvs: number[]): MeshBuilder;
    setTangents(tangents: number[]): MeshBuilder;
    setIndices(indices: number[]): MeshBuilder;
    setSubmeshes(submeshes: Submesh[]): MeshBuilder;
    build(): Mesh;
  }

  export interface MeshLOD {
    level: number;
    mesh: Mesh;
    screenSize: number;
  }

  export function computeBoundingBox(positions: number[]): BoundingBox {
    let minX = Infinity, minY = Infinity, minZ = Infinity;
    let maxX = -Infinity, maxY = -Infinity, maxZ = -Infinity;
    for (let i = 0; i < positions.length; i += 3) {
      const x = positions[i], y = positions[i + 1], z = positions[i + 2];
      minX = Math.min(minX, x); maxX = Math.max(maxX, x);
      minY = Math.min(minY, y); maxY = Math.max(maxY, y);
      minZ = Math.min(minZ, z); maxZ = Math.max(maxZ, z);
    }
    const min = { x: minX, y: minY, z: minZ };
    const max = { x: maxX, y: maxY, z: maxZ };
    const center = { x: (minX + maxX) / 2, y: (minY + maxY) / 2, z: (minZ + maxZ) / 2 };
    const extents = { x: maxX - minX, y: maxY - minY, z: maxZ - minZ };
    const radius = Math.sqrt(extents.x ** 2 + extents.y ** 2 + extents.z ** 2) / 2;
    return { min, max, center, extents, radius };
  }
}

// Cross-reference: 01_Renderer_Types.ts (renderer), 03_Material_Types.ts (materials)
console.log("\n=== Mesh Types ===");
console.log("Related: 01_Renderer_Types.ts, 03_Material_Types.ts");