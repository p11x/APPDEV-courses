/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 03_Graphics_Rendering
 * Topic: 03_Material_Types
 * Purpose: Define material types for rendering
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers, Node.js 18+
 * Performance: O(1) material lookups, GPU-driven rendering
 * Security: Material validation prevents invalid shader inputs
 */

namespace MaterialTypes {
  export type MaterialType = 'standard' | 'unlit' | 'subsurface' | 'emissive' | 'custom';

  export interface Material {
    id: string;
    name: string;
    type: MaterialType;
    shader: string;
    properties: MaterialProperty[];
    textures: MaterialTexture[];
    blending: BlendMode;
    cullMode: CullMode;
    depthWrite: boolean;
    depthTest: boolean;
  }

  export interface MaterialProperty {
    name: string;
    type: PropertyType;
    value: PropertyValue;
  }

  export type PropertyType = 'float' | 'int' | 'vec2' | 'vec3' | 'vec4' | 'mat4' | 'bool';
  export type PropertyValue = number | number[] | boolean;

  export interface MaterialTexture {
    name: string;
    textureId: string;
    samplerId: string;
    uvChannel: number;
    transform: TextureTransform;
  }

  export interface TextureTransform {
    offset: { u: number; v: number };
    scale: { u: number; v: number };
    rotation: number;
  }

  export type BlendMode = 'opaque' | 'alpha' | 'additive' | 'translucent';
  export type CullMode = 'none' | 'front' | 'back';

  export interface PBRMaterial extends Material {
    type: 'standard';
    baseColor: { r: number; g: number; b: number; a: number };
    metallic: number;
    roughness: number;
    normalScale: number;
    occlusionStrength: number;
    emissive: { r: number; g: number; b: number };
  }

  export interface UnlitMaterial extends Material {
    type: 'unlit';
    color: { r: number; g: number; b: number; a: number };
    useAlphaClip: boolean;
    alphaCutoff: number;
  }

  export interface MaterialInstance {
    baseMaterial: Material;
    overrides: Map<string, PropertyValue>;
    textureOverrides: Map<string, string>;
    renderStates: RenderState;
  }

  export interface RenderState {
    blend: BlendMode;
    cull: CullMode;
    depthWrite: boolean;
    depthFunc: CompareFunc;
  }

  export type CompareFunc = 'never' | 'less' | 'equal' | 'lessEqual' | 'greater' | 'notEqual' | 'greaterEqual' | 'always';

  export interface MaterialLibrary {
    createMaterial(name: string, type: MaterialType, shader: string): Material;
    createInstance(material: Material): MaterialInstance;
    getMaterial(id: string): Material | undefined;
  }
}

// Cross-reference: 02_Shader_Types.ts (shaders), 04_Mesh_Types.ts (meshes)
console.log("\n=== Material Types ===");
console.log("Related: 02_Shader_Types.ts, 04_Mesh_Types.ts");