/**
 * Category: 06_PLATFORM_INTEGRATION Subcategory: 02_Native_Scripting Concept: 02 Topic: FFI_Types Purpose: Foreign Function Interface types Difficulty: intermediate UseCase: native interop, dynamic libraries Version: TS5.0+ Compatibility: Node.js 14+ Performance: Call penalty ~100ns Security: Memory safety, validation */

/**
 * FFI Types - Comprehensive Guide
 * ============================
 * 
 * 📚 WHAT: Foreign Function Interface for calling native functions
 * 💡 WHY: Call C/C++ libraries without building addons
 * 🔧 HOW: ffi-napi, ref-napi, dynamic loading
 */

// ============================================================================
// SECTION 1: WHAT IS FFI
// ============================================================================

// Example 1.1: Basic Concept
// -------------------------------

// FFI allows calling native functions from JavaScript without writing
// C++ bindings. It uses dynamic loading to resolve and call functions

interface FFI_library {
  name: string;
  path: string;
  functions: FFI_Function[];
}

// ============================================================================
// SECTION 2: TYPES AND SIGNATURES
// ============================================================================

// Example 2.1: Type Definitions
// -----------------------------

type FFI_Type =
  | "void"
  | "int8" | "uint8"
  | "int16" | "uint16"
  | "int32" | "uint32"
  | "int64" | "uint64"
  | "float" | "double"
  | "pointer" | "string"
  | "bool";

interface FFIFunctionSignature {
  returnType: FFI_Type;
  argumentTypes: FFI_Type[];
}

// Example 2.2: Complex Types
// -----------------------------

interface FFI_Struct {
  name: string;
  fields: { name: string; type: FFI_Type }[];
}

const pointStruct: FFI_Struct = {
  name: "Point",
  fields: [
    { name: "x", type: "int32" },
    { name: "y", type: "int32" }
  ]
};

// ============================================================================
// SECTION 3: FUNCTION DECLARATIONS
// ============================================================================

// Example 3.1: Declaring Functions
// -----------------------------

interface FFIFunctionDeclaration {
  name: string;
  library: string;
  returnType: FFI_Type;
  argumentTypes: FFI_Type[];
  async?: boolean;
}

const nativeFunctions: FFIFunctionDeclaration[] = [
  {
    name: "sqrt",
    library: "libm",
    returnType: "double",
    argumentTypes: ["double"]
  },
  {
    name: "strlen",
    library: "libc",
    returnType: "int64",
    argumentTypes: ["string"]
  },
  {
    name: "memcpy",
    library: "libc",
    returnType: "pointer",
    argumentTypes: ["pointer", "pointer", "int64"]
  }
];

// ============================================================================
// SECTION 4: LOADING LIBRARIES
// ============================================================================

// Example 4.1: Dynamic Loading
// -----------------------------

interface FFILibrary {
  handle: unknown;
  name: string;
}

class FFILoader {
  private libraries = new Map<string, FFILibrary>();

  async load(name: string, path?: string): Promise<FFILibrary> {
    const existing = this.libraries.get(name);
    if (existing) return existing;
    
    const library: FFILibrary = {
      handle: null as unknown,
      name
    };
    
    this.libraries.set(name, library);
    return library;
  }

  get(name: string): FFILibrary | undefined {
    return this.libraries.get(name);
  }
}

// ============================================================================
// SECTION 5: CALLING FUNCTIONS
// ============================================================================

// Example 5.1: Function Calls
// -----------------------------

class FFICaller {
  private library: FFILibrary | null = null;

  call<R>(funcName: string, ...args: unknown[]): R {
    return null as unknown as R;
  }

  callAsync<R>(funcName: string, ...args: unknown[]): Promise<R> {
    return Promise.resolve(null as unknown as R);
  }
}

// ============================================================================
// SECTION 6: BUFFER AND POINTERS
// ============================================================================

// Example 6.1: Working with Pointers
// -----------------------------

interface Pointer {
  address: number;
  type: FFI_Type;
  offset: number;
}

function createPointer(address: number, type: FFI_Type): Pointer {
  return { address, type, offset: 0 };
}

function derefPointer(ptr: Pointer): unknown {
  return null;
}

// Example 6.2: Buffer Operations
// -----------------------------

interface Buffer {
  address: number;
  length: number;
  type: FFI_Type;
}

function allocateBuffer(length: number, type: FFI_Type = "int8"): Buffer {
  return { address: 0, length, type };
}

// ============================================================================
// SECTION 7: CALLBACKS
// ============================================================================

// Example 7.1: Native Callbacks
// -----------------------------

type FFICallback = (...args: unknown[]) => unknown;

interface FFICallbackInfo {
  id: number;
  func: FFICallback;
  returnType: FFI_Type;
  argumentTypes: FFI_Type[];
}

class FFICallbackHandler {
  private callbacks = new Map<number, FFICallbackInfo>();
  private nextId = 0;

  createCallback(
    func: FFICallback,
    returnType: FFI_Type,
    argumentTypes: FFI_Type[]
  ): number {
    const id = this.nextId++;
    this.callbacks.set(id, { id, func, returnType, argumentTypes });
    return id;
  }

  invoke(id: number, ...args: unknown[]): unknown {
    const callback = this.callbacks.get(id);
    return callback?.func(...args);
  }

  freeCallback(id: number): void {
    this.callbacks.delete(id);
  }
}

// ============================================================================
// SECTION 8: ERROR HANDLING
// ============================================================================

// Example 8.1: Error Handling
// -----------------------------

interface FFIError {
  code: string;
  message: string;
  library?: string;
  function?: string;
}

type FFIErrorCode =
  | "LOAD_FAILED"
  | "FUNCTION_NOT_FOUND"
  | "INVALID_ARGUMENT"
  | "CALL_FAILED";

function createFFIError(code: FFIErrorCode, message: string): FFIError {
  return { code, message };
}

// ============================================================================
// SECTION 9: PERFORMANCE
// ============================================================================

// Example 9.1: Performance
// -----------------------

interface FFIPerformance {
  callOverhead: string;
  callbackOverhead: string;
}

const ffiPerformance: FFIPerformance = {
  callOverhead: "~50-100ns per call",
  callbackOverhead: "~200-500ns per callback"
};

// ============================================================================
// SECTION 10: COMPATIBILITY
// ============================================================================

// Example 10.1: Compatibility
// -----------------------

interface FFICompatibility {
  platforms: string[];
  nodeVersion: string;
}

const ffiCompatibility: FFICompatibility = {
  platforms: ["Linux", "macOS", "Windows"],
  nodeVersion: "14+"
};

// ============================================================================
// SECTION 11: SECURITY
// ============================================================================

// Example 11.1: Security
// -----------------------

interface FFISecurity {
  validation: string;
  sandbox: string;
}

const ffiSecurity: FFISecurity = {
  validation: "Validate all function signatures",
  sandbox: "FFI runs in process space"
};

// ============================================================================
// SECTION 12: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 02_Native_Scripting/01_Node_Native_Modules/01_NAPI_Types.ts
// - 02_Native_Scripting/02_Electron_Integration/02_IPC_Types.ts

console.log("\n=== FFI Types Complete ===");
console.log("Next: 02_Native_Scripting/02_Electron_Integration/01_Electron_Types");