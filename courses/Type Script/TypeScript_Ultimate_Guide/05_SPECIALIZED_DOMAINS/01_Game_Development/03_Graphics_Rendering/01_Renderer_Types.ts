/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 03_Graphics_Rendering
 * Topic: 01_Renderer_Types
 * Purpose: Define renderer types for game graphics
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers (WebGL 2.0), Node.js 18+
 * Performance: O(n) draw calls, GPU-accelerated rendering
 * Security: WebGL context isolation prevents sandbox escape
 */

namespace RendererTypes {
  export type RenderBackend = 'webgl2' | 'webgl1' | 'metal' | 'vulkan';
  export type RenderMode = 'forward' | 'deferred' | 'forward_plus';

  export interface RendererConfig {
    backend: RenderBackend;
    mode: RenderMode;
    width: number;
    height: number;
    antialias: boolean;
    vsync: boolean;
    HDR: boolean;
    shadowQuality: ShadowQuality;
  }

  export type ShadowQuality = 'off' | 'low' | 'medium' | 'high' | 'ultra';

  export interface RenderTarget {
    id: string;
    width: number;
    height: number;
    colorTexture: Texture;
    depthTexture: Texture;
    stencilTexture?: Texture;
  }

  export interface Texture {
    id: string;
    width: number;
    height: number;
    format: PixelFormat;
    type: TextureType;
    minFilter: FilterMode;
    magFilter: FilterMode;
    wrapS: WrapMode;
    wrapT: WrapMode;
    anisotropy: number;
  }

  export type PixelFormat = 'rgba8' | 'rgba16f' | 'rgba32f' | 'depth24' | 'depth32f';
  export type TextureType = '2d' | '2dArray' | 'cube' | '3d';
  export type FilterMode = 'nearest' | 'linear' | 'linearMipmapLinear';
  export type WrapMode = 'clampToEdge' | 'repeat' | 'mirroredRepeat';

  export interface Viewport {
    x: number;
    y: number;
    width: number;
    height: number;
    minDepth: number;
    maxDepth: number;
  }

  export interface Scissor {
    x: number;
    y: number;
    width: number;
    height: number;
  }

  export interface RenderPass {
    name: string;
    renderTargets: RenderTarget[];
    clearColor: number[];
    clearDepth: number;
    clearStencil: number;
    viewport: Viewport;
    scissor?: Scissor;
  }

  export interface RenderStatistics {
    drawCalls: number;
    triangles: number;
    vertices: number;
    textures: number;
    buffers: number;
    frameTime: number;
    gpuTime: number;
  }

  export interface Renderer {
    initialize(config: RendererConfig): Promise<void>;
    beginFrame(): void;
    endFrame(): void;
    submitRenderPass(pass: RenderPass): void;
    getStatistics(): RenderStatistics;
    resize(width: number, height: number): void;
  }
}

// Cross-reference: 02_Shader_Types.ts (shaders), 04_Mesh_Types.ts (meshes)
console.log("\n=== Renderer Types ===");
console.log("Related: 02_Shader_Types.ts, 04_Mesh_Types.ts");