/**
 * Category: 06_PLATFORM_INTEGRATION Subcategory: 01_WebAssembly Concept: 04 Topic: Loading_WASM Purpose: Loading and instantiating WebAssembly modules Difficulty: intermediate UseCase: web applications, Node.js Version: TS5.0+ Compatibility: Modern browsers, Node.js 16+ Performance: Module loading time Security: Trust validation, code signing */

/**
 * Loading WebAssembly - Comprehensive Guide
 * =========================================
 * 
 * 📚 WHAT: Loading and initializing WASM modules
 * 💡 WHY: Load compiled WASM code into JavaScript environments
 * 🔧 HOW: WebAssembly.instantiateStreaming, fetch, bytes
 */

// ============================================================================
// SECTION 1: WHAT IS LOADING
// ============================================================================

// Example 1.1: Basic Loading Concept
// -------------------------------

// Loading WebAssembly involves fetching the .wasm file, compiling it into a module,
// and instantiating it to create callable functions

interface WASMLoader {
  module: WebAssembly.Module | null;
  instance: WebAssembly.Instance | null;
  exports: WebAssembly.Exports;
}

// Example 1.2: Loading Flow
// ----------------------

interface LoadingFlow {
  fetch: string;
  compile: string;
  instantiate: string;
}

const loadingFlow: LoadingFlow = {
  fetch: "Fetch the .wasm file",
  compile: "Compile to WebAssembly.Module",
  instantiate: "Create WebAssembly.Instance"
};

// ============================================================================
// SECTION 2: FETCH AND LOAD
// ============================================================================

// Example 2.1: Fetching WASM
// -------------------------

async function fetchWASM(url: string): Promise<ArrayBuffer> {
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error(`Failed to fetch ${url}: ${response.status}`);
  }
  const arrayBuffer = await response.arrayBuffer();
  return arrayBuffer;
}

// Example 2.2: Loading from Various Sources
// ------------------------------------

type WASMSource = RequestInfo | URL | string;

async function loadWASMSource(source: WASMSource): Promise<ArrayBuffer> {
  const response = await fetch(source);
  return response.arrayBuffer();
}

// ============================================================================
// SECTION 3: COMPILATION
// ============================================================================

// Example 3.1: Compiling Module
// ----------------------------

async function compileWASM(bytes: ArrayBuffer): Promise<WebAssembly.Module> {
  const module = await WebAssembly.compile(bytes);
  return module;
}

// Example 3.2: Streaming Compilation
// ---------------------------------

async function compileStreaming(source: Response): Promise<WebAssembly.Module> {
  const module = await WebAssembly.compileStreaming(source);
  return module;
}

// ============================================================================
// SECTION 4: INSTANTIATION
// ============================================================================

// Example 4.1: Basic Instantiation
// ------------------------------

async function instantiateWASM(
  module: WebAssembly.Module, 
  importObject?: WebAssembly.ImportObject
): Promise<WebAssembly.Instance> {
  const instance = await WebAssembly.instantiate(module, importObject);
  return instance.instance;
}

// Example 4.2: One-Step Loading
// --------------------------

async function loadAndInstantiate(
  bytes: ArrayBuffer,
  importObject?: WebAssembly.ImportObject
): Promise<WebAssembly.Instance> {
  const result = await WebAssembly.instantiate(bytes, importObject);
  return result.instance;
}

// ============================================================================
// SECTION 5: STREAMING LOAD
// ============================================================================

// Example 5.1: Streaming Load
// --------------------------

async function loadWASMStreaming(
  url: string,
  importObject?: WebAssembly.ImportObject
): Promise<WebAssembly.Instance> {
  const response = await fetch(url);
  const result = await WebAssembly.instantiateStreaming(response, importObject);
  return result.instance;
}

// Example 5.2: With Progress Tracking
// ---------------------------------

interface LoadProgress {
  loaded: number;
  total: number;
  stage: string;
}

function trackProgress(event: ProgressEvent): LoadProgress {
  return {
    loaded: event.loaded,
    total: event.total,
    stage: event.type === "loadend" ? "complete" : "loading"
  };
}

// ============================================================================
// SECTION 6: ERROR HANDLING
// ============================================================================

// Example 6.1: Handling Load Errors
// -------------------------------

interface WASMLoadError {
  type: "fetch" | "compile" | "instantiate" | "link";
  message: string;
  original?: Error;
}

async function safeLoadWASM(
  url: string,
  importObject?: WebAssembly.ImportObject
): Promise<WebAssembly.Instance> {
  try {
    return await loadWASMStreaming(url, importObject);
  } catch (error) {
    if (error instanceof WebAssembly.CompileError) {
      throw new Error(`WASM compilation failed: ${error.message}`);
    }
    if (error instanceof WebAssembly.LinkError) {
      throw new Error(`WASM linking failed: ${error.message}`);
    }
    throw error;
  }
}

// ============================================================================
// SECTION 7: CONFIGURATION
// ============================================================================

// Example 7.1: Loading Configuration
// ----------------------------------

interface WASMLoadConfig {
  url: string;
  importObject?: WebAssembly.ImportObject;
  cache?: boolean;
}

const loadConfig: WASMLoadConfig = {
  url: "/wasm/module.wasm",
  importObject: {},
  cache: true
};

// ============================================================================
// SECTION 8: CACHING
// ============================================================================

// Example 8.1: Caching Modules
// --------------------------

class WASMModuleCache {
  private cache = new Map<string, WebAssembly.Module>();

  async getOrCreate(
    url: string,
    bytes?: ArrayBuffer
  ): Promise<WebAssembly.Module> {
    const cached = this.cache.get(url);
    if (cached) return cached;

    const module = bytes 
      ? await WebAssembly.compile(bytes)
      : await this.fetchAndCompile(url);
    
    this.cache.set(url, module);
    return module;
  }

  private async fetchAndCompile(url: string): Promise<WebAssembly.Module> {
    const response = await fetch(url);
    const bytes = await response.arrayBuffer();
    return WebAssembly.compile(bytes);
  }
}

// ============================================================================
// SECTION 9: PERFORMANCE
// ============================================================================

// Example 9.1: Performance Considerations
// ------------------------------------

interface LoadPerformance {
  streaming: string;
  caching: string;
  compression: string;
}

const loadPerformance: LoadPerformance = {
  streaming: "Use instantiateStreaming for best performance",
  caching: "Cache compiled modules when possible",
  compression: "Use gzip for smaller WASM files"
};

// ============================================================================
// SECTION 10: COMPATIBILITY
// ============================================================================

// Example 10.1: Browser Support
// --------------------------

interface LoadCompatibility {
  browsers: string[];
  nodeVersion: string;
}

const loadCompatibility: LoadCompatibility = {
  browsers: ["Chrome 57+", "Firefox 52+", "Safari 11+", "Edge 16+"],
  nodeVersion: "8+"
};

// ============================================================================
// SECTION 11: SECURITY
// ============================================================================

// Example 11.1: Security Considerations
// -----------------------------

interface LoadSecurity {
  sameOrigin: string;
  validation: string;
  csp: string;
}

const loadSecurity: LoadSecurity = {
  sameOrigin: "Load from same origin or use CORS",
  validation: "Validate WASM module signature",
  csp: "Configure CSP for WASM loading"
};

// ============================================================================
// SECTION 12: TESTING
// ============================================================================

// Example 12.1: Testing Load
// -----------------------

interface LoadTest {
  fetch: boolean;
  compile: boolean;
  instantiate: boolean;
}

const loadTests: LoadTest = {
  fetch: true,
  compile: true,
  instantiate: true
};

// ============================================================================
// SECTION 13: DEBUGGING
// ============================================================================

// Example 13.1: Debugging Load
// -----------------------

interface LoadDebug {
  trace: (stage: string, details?: unknown) => void;
  measure: (url: string) => Promise<number>;
}

// ============================================================================
// SECTION 14: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 01_WASM_Types/01_WebAssembly_Integration.ts
// - 01_WASM_Types/02_WASM_Memory_Types.ts
// - 02_WASM_Integration/02_Memory_Management.ts
// - 02_WASM_Integration/03_Calling_JavaScript.ts

console.log("\n=== Loading WASM Complete ===");
console.log("Next: 02_WASM_Integration/02_Memory_Management");