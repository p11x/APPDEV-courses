/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 03_Graphics_Rendering
 * Topic: 02_Shader_Types
 * Purpose: Define shader types for GPU rendering
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers (WebGL 2.0), Node.js 18+
 * Performance: O(1) shader uniform updates, O(n) program linking
 * Security: Shader validation prevents WebGL exploits
 */

namespace ShaderTypes {
  export type ShaderStage = 'vertex' | 'fragment' | 'compute' | 'geometry' | 'tessControl' | 'tessEval';

  export interface ShaderSource {
    stage: ShaderStage;
    code: string;
    entryPoint: string;
  }

  export interface Shader {
    id: string;
    stage: ShaderStage;
    source: string;
    compiled: boolean;
    error?: string;
  }

  export interface ShaderProgram {
    id: string;
    vertexShader: Shader;
    fragmentShader: Shader;
    linked: boolean;
    uniformLocations: Map<string, WebGLUniformLocation>;
    attributeLocations: Map<string, number>;
    error?: string;
  }

  export interface Uniform {
    name: string;
    type: UniformType;
    location: WebGLUniformLocation;
    value: UniformValue;
  }

  export type UniformType = 'float' | 'int' | 'uint' | 'bool' | 'vec2' | 'vec3' | 'vec4' | 'ivec2' | 'ivec3' | 'ivec4' | 'uvec2' | 'uvec3' | 'uvec4' | 'bvec2' | 'bvec3' | 'bvec4' | 'mat2' | 'mat3' | 'mat4' | 'sampler2D' | 'samplerCube';
  export type UniformValue = number | number[] | boolean | Int32Array | Float32Array;

  export interface UniformBlock {
    name: string;
    binding: number;
    size: number;
    members: UniformBlockMember[];
  }

  export interface UniformBlockMember {
    name: string;
    offset: number;
    size: number;
    type: UniformType;
  }

  export interface ShaderBinding {
    type: 'uniform' | 'texture' | 'storage' | 'sampler';
    name: string;
    binding: number;
  }

  export interface ShaderCompiler {
    compile(source: ShaderSource): Promise<Shader>;
    link(vertex: Shader, fragment: Shader): Promise<ShaderProgram>;
    validate(program: ShaderProgram): boolean;
  }

  export interface ShaderPreprocessor {
    define(name: string, value?: string): void;
    undefine(name: string): void;
    process(source: string): string;
  }

  const builtinMacros: Record<string, string> = {
    'MAX_DIRECTIONAL_LIGHTS': '4',
    'MAX_POINT_LIGHTS': '16',
    'MAX_SHADOW_CASCADES': '4',
    'SHADOW_MAP_SIZE': '2048',
    'ENVIRONMENT_MAP_SIZE': '512',
    'MAX_BONES': '128',
  };
}

// Cross-reference: 01_Renderer_Types.ts (renderer), 03_Material_Types.ts (materials)
console.log("\n=== Shader Types ===");
console.log("Related: 01_Renderer_Types.ts, 03_Material_Types.ts");