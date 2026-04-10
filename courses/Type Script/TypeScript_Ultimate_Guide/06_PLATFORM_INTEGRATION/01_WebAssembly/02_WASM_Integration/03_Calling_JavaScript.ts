/**
 * Category: 06_PLATFORM_INTEGRATION Subcategory: 01_WebAssembly Concept: 06 Topic: Calling_JavaScript Purpose: Calling JS from WebAssembly Difficulty: intermediate UseCase: interoperability, callbacks Version: TS5.0+ Compatibility: Modern browsers, Node.js 16+ Performance: Call overhead Security: Validation, trust */

/**
 * Calling JavaScript from WASM - Comprehensive Guide
 * =============================================
 * 
 * 📚 WHAT: Invoking JavaScript functions from WebAssembly
 * 💡 WHY: Enable WASM to interact with browser APIs
 * 🔧 HOW: Import functions, function tables, funcref
 */

// ============================================================================
// SECTION 1: WHAT IS CALLING JAVASCRIPT
// ============================================================================

// Example 1.1: Basic Concept
// -------------------------------

// WebAssembly can call JavaScript functions when they are imported into the WASM module.
// This enables WASM code to interact with browser APIs, DOM, and other JavaScript functionality

interface JSFunctionImport {
  name: string;
  func: (...args: unknown[]) => unknown;
  signature: string;
}

// Example 1.2: Calling Flow
// ----------------------

interface CallingFlow {
  define: string;
  import: string;
  call: string;
  callback: string;
}

const callingFlow: CallingFlow = {
  define: "Define JS function in import object",
  import: "Import into WASM module",
  call: "Call from WASM code",
  callback: "Receive results in JS"
};

// ============================================================================
// SECTION 2: IMPORT FUNCTIONS
// ============================================================================

// Example 2.1: Importing Functions
// -----------------------------

const importObject: WebAssembly.ImportObject = {
  env: {
    log: (message: number) => {
      console.log(`WASM says: ${message}`);
    },
    consoleLog: (strPtr: number, memory: WebAssembly.Memory) => {
      const view = new Uint8Array(memory.buffer);
      let end = strPtr;
      while (view[end] !== 0) end++;
      const message = new TextDecoder().decode(view.slice(strPtr, end));
      console.log(message);
    },
    getTimestamp: () => Date.now()
  },
  wasi: {
    proc_exit: (code: number) => {
      process.exit(code);
    }
  }
};

// Example 2.2: Typed Imports
// -----------------------------

interface TypedImports {
  log: (msg: number) => void;
  consoleLog: (ptr: number) => void;
  getTimestamp: () => number;
  random: () => number;
}

// ============================================================================
// SECTION 3: CALLBACKS
// ============================================================================

// Example 3.1: Callback Functions
// -----------------------------

type CallbackFn = (...args: unknown[]) => void;

class WASMCallbackHandler {
  private callbacks: Map<number, CallbackFn> = new Map();
  private nextId = 0;

  registerCallback(fn: CallbackFn): number {
    const id = this.nextId++;
    this.callbacks.set(id, fn);
    return id;
  }

  unregisterCallback(id: number): void {
    this.callbacks.delete(id);
  }

  invokeCallback(id: number, ...args: unknown[]): void {
    const callback = this.callbacks.get(id);
    if (callback) {
      callback(...args);
    }
  }

  getCallbackCount(): number {
    return this.callbacks.size;
  }
}

// ============================================================================
// SECTION 4: EVENT HANDLERS
// ============================================================================

// Example 4.1: Event Handler Integration
// -----------------------------

interface EventHandlerImport {
  addEventListener: (
    target: string, 
    event: string, 
    callback: (...args: unknown[]) => void
  ) => void;
  removeEventListener: (target: string, event: string) => void;
}

function createEventHandlers(): EventHandlerImport {
  return {
    addEventListener: (target, event, callback) => {
      const element = document.querySelector(target);
      element?.addEventListener(event, callback as EventListener);
    },
    removeEventListener: (target, event) => {
      const element = document.querySelector(target);
      element?.removeEventListener(event, () => {});
    }
  };
}

// ============================================================================
// SECTION 5: PROMISES AND ASYNC
// ============================================================================

// Example 5.1: Async Callbacks
// -----------------------------

async function callAsyncFromWASM<T>(
  fn: () => Promise<T>,
  timeout: number = 5000
): Promise<T> {
  return Promise.race([
    fn,
    new Promise<never>((_, reject) => 
      setTimeout(() => reject(new Error("Timeout")), timeout)
    )
  ]);
}

// Example 5.2: Promise Resolution
// -----------------------------

interface PromiseResolver {
  resolve: (value: unknown) => void;
  reject: (error: Error) => void;
}

function createPromiseCallbacks(): PromiseResolver {
  let resolve: (value: unknown) => void = () => {};
  let reject: (error: Error) => void = () => {};
  
  const promise = new Promise<unknown>((res, rej) => {
    resolve = res;
    reject = rej;
  });
  
  return { resolve, reject };
}

// ============================================================================
// SECTION 6: ERROR HANDLING
// ============================================================================

// Example 6.1: Error Handling
// -----------------------------

function safeCallJS<T>(
  fn: () => T,
  onError: (error: Error) => void
): T | null {
  try {
    return fn();
  } catch (error) {
    onError(error as Error);
    return null;
  }
}

// ============================================================================
// SECTION 7: PERFORMANCE
// ============================================================================

// Example 7.1: Performance Considerations
// ------------------------------------

interface CallPerformance {
  directCall: string;
  indirectCall: string;
  callback: string;
}

const callPerformance: CallPerformance = {
  directCall: "~10-50 CPU cycles",
  indirectCall: "~100-200 CPU cycles",
  callback: "Minimize for performance"
};

// ============================================================================
// SECTION 8: COMPATIBILITY
// ============================================================================

// Example 8.1: Browser Support
// --------------------------

interface CallCompatibility {
  browsers: string[];
  nodeVersion: string;
}

const callCompatibility: CallCompatibility = {
  browsers: ["Chrome 57+", "Firefox 52+", "Safari 11+", "Edge 16+"],
  nodeVersion: "8+"
};

// ============================================================================
// SECTION 9: SECURITY
// ============================================================================

// Example 9.1: Security Considerations
// -----------------------------

interface CallSecurity {
  validation: string;
  sandbox: string;
  trust: string;
}

const callSecurity: CallSecurity = {
  validation: "Validate all callback parameters",
  sandbox: "WASM runs in sandboxed environment",
  trust: "Only import trusted functions"
};

// ============================================================================
// SECTION 10: TESTING
// ============================================================================

// Example 10.1: Testing Calls
// -----------------------

interface CallTest {
  importFunction: boolean;
  callback: boolean;
  asyncCall: boolean;
  errorHandling: boolean;
}

const callTests: CallTest = {
  importFunction: true,
  callback: true,
  asyncCall: true,
  errorHandling: true
};

// ============================================================================
// SECTION 11: DEBUGGING
// ============================================================================

// Example 11.1: Debugging Calls
// -----------------------

interface CallDebug {
  trace: boolean;
  measure: boolean;
  breakOnCall: boolean;
}

const callDebug: CallDebug = {
  trace: true,
  measure: true,
  breakOnCall: true
};

// ============================================================================
// SECTION 12: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 01_WASM_Types/01_WebAssembly_Integration.ts
// - 01_WASM_Types/03_WASM_Function_Types.ts
// - 02_WASM_Integration/01_Loading_WASM.ts

console.log("\n=== Calling JavaScript Complete ===");
console.log("Next: 02_Native_Scripting/01_Node_Native_Modules/01_NAPI_Types");