/**
 * Category: 06_PLATFORM_INTEGRATION Subcategory: 01_WebAssembly Concept: 03 Topic: WASM_Function_Types Purpose: WebAssembly function exports and imports Difficulty: advanced UseCase: interoperability between JS and WASM Version: TS5.0+ Compatibility: Modern browsers, Node.js 16+ Performance: Function call overhead Security: Parameter validation */

/**
 * WebAssembly Function Types - Comprehensive Guide
 * ==============================================
 * 
 * 📚 WHAT: Type definitions for WASM function calls
 * 💡 WHY: Enable type-safe interop between JavaScript and WASM
 * 🔧 HOW: WebAssembly.Function, import/export types
 */

// ============================================================================
// SECTION 1: WHAT IS WASM FUNCTIONS
// ============================================================================

// Example 1.1: Basic Function Concept
// -------------------------------

// WebAssembly functions are compiled functions callable from JavaScript
// that can perform high-performance computations

interface WASMFunction {
  name: string;
  signature: string;
  parameters: WASMValueType[];
  returnType: WASMValueType | null;
}

// Example 1.2: Function Signatures
// ------------------------------

type WASMValueType = "i32" | "i64" | "f32" | "f64" | "v128" | "funcref" | "externref";

interface FunctionSignature {
  parameters: WASMValueType[];
  results: WASMValueType[];
}

// ============================================================================
// SECTION 2: EXPORTED FUNCTIONS
// ============================================================================

// Example 2.1: Exporting Functions
// -----------------------------

class WASMFunctionExporter {
  private instance: WebAssembly.Instance | null = null;

  getExportedFunction(name: string): WebAssembly.Func | null {
    return this.instance?.exports[name] as WebAssembly.Func ?? null;
  }

  callFunction(name: string, ...args: unknown[]): unknown {
    const func = this.getExportedFunction(name);
    if (!func) throw new Error(`Function ${name} not found`);
    return func.call(...args);
  }
}

// Example 2.2: Function Type Definitions
// ------------------------------------

interface ExportedFunction<T = unknown> {
  (): T;
}

interface ExportedFunction1<A, T = unknown> {
  (arg: A): T;
}

interface ExportedFunction2<A, B, T = unknown> {
  (arg1: A, arg2: B): T;
}

// ============================================================================
// SECTION 3: IMPORTED FUNCTIONS
// ============================================================================

// Example 3.1: Importing Functions
// -------------------------------

type WASMImportFunction = (...args: unknown[]) => unknown;

interface WASMImport {
  module: string;
  name: string;
  func: WASMImportFunction;
}

const importObject: WebAssembly.ImportObject = {
  env: {
    log: (msg: number) => console.log(msg),
    abort: () => { throw new Error("Abort called"); }
  }
};

// Example 3.2: Host Functions
// -------------------------

interface HostFunctionOptions {
  parameters: WASMValueType[];
  results: WASMValueType[];
}

function createHostFunction(
  fn: (...args: unknown[]) => unknown,
  options: HostFunctionOptions
): WebAssembly.Func {
  return WebAssembly.func(fn, {}, options);
}

// ============================================================================
// SECTION 4: FUNCTION REFERENCES
// ============================================================================

// Example 4.1: Function References in WASM
// ------------------------------------

interface WASMFuncRef {
  _index: number;
  type: string;
}

function getFunctionReference(fn: WebAssembly.Func): WASMFuncRef {
  return {
    _index: (fn as unknown as WebAssembly.Table).length,
    type: "funcref"
  };
}

// Example 4.2: Function Table
// -------------------------

class WASMFunctionTable {
  private table: WebAssembly.Table | null = null;

  createTable(initial: number, maximum?: number): WebAssembly.Table {
    this.table = new WebAssembly.Table({
      initial,
      maximum,
      element: "funcref"
    });
    return this.table;
  }

  getFunction(index: number): WebAssembly.Func | null {
    return this.table?.get(index) as WebAssembly.Func ?? null;
  }

  setFunction(index: number, fn: WebAssembly.Func): void {
    this.table?.set(index, fn);
  }
}

// ============================================================================
// SECTION 5: TYPE-SAFE WRAPPERS
// ============================================================================

// Example 5.1: Creating Type-safe Wrappers
// -------------------------------------

type WASMFunction0<R> = () => R;
type WASMFunction1<A, R> = (a: A) => R;
type WASMFunction2<A, B, R> = (a: A, b: B) => R;
type WASMFunction3<A, B, C, R> = (a: A, b: B, c: C) => R;

interface WASMWrapper<T> {
  readonly __wasm: WebAssembly.Instance;
  readonly type: T;
}

function createWASMWrapper<T>(
  instance: WebAssembly.Instance, 
  type: new () => T
): WASMWrapper<T> {
  return {
    __wasm: instance,
    type: new type()
  };
}

// Example 5.2: Typed Function Calls
// -------------------------------

class TypedWASMCaller {
  call<R>(func: WebAssembly.Func): R {
    return func.call() as R;
  }

  call1<A, R>(func: WebAssembly.Func, arg: A): R {
    return func.call(arg) as R;
  }

  call2<A, B, R>(func: WebAssembly.Func, arg1: A, arg2: B): R {
    return func.call(arg1, arg2) as R;
  }
}

// ============================================================================
// SECTION 6: PERFORMANCE
// ============================================================================

// Example 6.1: Function Call Performance
// ------------------------------

interface FunctionPerformance {
  directCall: string;
  indirectCall: string;
  callOverhead: string;
}

const functionPerformance: FunctionPerformance = {
  directCall: "~1-2 CPU cycles for direct calls",
  indirectCall: "~5-10 CPU cycles for indirect calls",
  callOverhead: "Minimize parameters for better performance"
};

// Example 6.2: Optimization
// -----------------------

interface FunctionOptimization {
  inlining: boolean;
  parameterCount: number;
  useGlobals: boolean;
}

const funcOptimization: FunctionOptimization = {
  inlining: true,
  parameterCount: "Max 4 parameters recommended",
  useGlobals: "Use globals for frequently accessed values"
};

// ============================================================================
// SECTION 7: COMPATIBILITY
// ============================================================================

// Example 7.1: Browser Support
// --------------------------

interface FunctionCompatibility {
  browsers: string[];
  nodeVersion: string;
  features: string[];
}

const functionCompatibility: FunctionCompatibility = {
  browsers: ["Chrome 57+", "Firefox 52+", "Safari 11+", "Edge 16+"],
  nodeVersion: "8+",
  features: ["Direct function calls", "WebAssembly.Function", "Function tables"]
};

// ============================================================================
// SECTION 8: SECURITY
// ============================================================================

// Example 8.1: Security Considerations
// -----------------------------

interface FunctionSecurity {
  validation: string;
  bounds: string;
  injection: string;
}

const functionSecurity: FunctionSecurity = {
  validation: "Validate all parameters before calling",
  bounds: "Check table bounds for indirect calls",
  injection: "Avoid passing user data as function references"
};

// ============================================================================
// SECTION 9: TESTING
// ============================================================================

// Example 9.1: Testing Function Calls
// ---------------------------------

interface FunctionTest {
  callWithArgs: boolean;
  returnValue: boolean;
  exceptionHandling: boolean;
  threading: boolean;
}

const functionTests: FunctionTest = {
  callWithArgs: true,
  returnValue: true,
  exceptionHandling: true,
  threading: true
};

// ============================================================================
// SECTION 10: DEBUGGING
// ============================================================================

// Example 10.1: Debugging Functions
// ------------------------------

interface FunctionDebug {
  traceCalls: boolean;
  measureTime: boolean;
  validateParams: boolean;
}

const functionDebug: FunctionDebug = {
  traceCalls: true,
  measureTime: true,
  validateParams: true
};

// ============================================================================
// SECTION 11: ALTERNATIVE
// ============================================================================

// Example 11.1: Alternatives
// -------------------------

interface FunctionAlternatives {
  wasm_bindgen: string;
  wasmer: string;
  wasmtime: string;
}

const alternatives: FunctionAlternatives = {
  wasm_bindgen: "Use wasm-bindgen for high-level bindings",
  wasmer: "Use wasmer for server-side WASM",
  wasmtime: "Use wasmtime for embedded runtime"
};

// ============================================================================
// SECTION 12: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 01_WASM_Types/01_WebAssembly_Integration.ts
// - 01_WASM_Types/02_WASM_Memory_Types.ts
// - 02_WASM_Integration/01_Loading_WASM.ts
// - 02_WASM_Integration/03_Calling_JavaScript.ts

console.log("\n=== WASM Function Types Complete ===");
console.log("Next: 02_WASM_Integration/01_Loading_WASM");