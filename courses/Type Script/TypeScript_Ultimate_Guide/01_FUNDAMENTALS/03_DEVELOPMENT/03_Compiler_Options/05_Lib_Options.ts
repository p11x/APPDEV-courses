/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 03 Topic: Lib Options Purpose: Configuring included type definitions Difficulty: beginner UseCase: browser,server,mixed Version: TS2.0+ Compatibility: Node.js, Browsers Performance: Type checking Security: Type availability */

/**
 * Lib Options - Comprehensive Guide
 * =================================
 * 
 * 📚 WHAT: Configuring included type definitions
 * 💡 WHY: Determine which built-in types and APIs are available in your code
 * 🔧 HOW: lib option in tsconfig.json
 */

// ============================================================================
// SECTION 1: WHAT IS LIB
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// The lib option specifies which built-in type definitions are included
// This determines what APIs are available (e.g., Array, Promise, Math)

interface LibOption {
  purpose: string;
  affects: string[];
}

const libInfo: LibOption = {
  purpose: "Include built-in type definitions",
  affects: ["Global types", "DOM APIs", "ES runtime APIs"]
};

// ============================================================================
// SECTION 2: AVAILABLE LIB OPTIONS
// ============================================================================

// Example 2.1: ES Lib Options
// -----------------------

interface ESLibOptions {
  es5: string;
  es6: string;
  es2015: string;
  es2016: string;
  es2017: string;
  es2018: string;
  es2019: string;
  es2020: string;
  es2021: string;
  esnext: string;
}

const esLibOptions: ESLibOptions = {
  es5: "ES5 built-in types",
  es6: "ES2015 built-in types",
  es2015: "Alias for ES6",
  es2016: "Array.prototype.includes",
  es2017: "Object.values/entries, async",
  es2018: "Async iterables, spread",
  es2019: "Array.prototype.flat, trimStart",
  es2020: "BigInt, optional chaining",
  es2021: "Promise.allSettled, logical assignment",
  esnext: "Latest features"
};

// Example 2.2: DOM Lib Options
// -----------------------

interface DOMLibOptions {
  dom: string;
  dom: string;
  webworker: string;
  webworker: string;
}

const domLibOptions: DOMLibOptions = {
  dom: "DOM APIs (document, window)",
  webworker: "Web Worker APIs"
};

// ============================================================================
// SECTION 3: CONFIGURING LIB
// ============================================================================

// Example 3.1: Basic Lib Config
// -----------------------------

interface BasicLibConfig {
  compilerOptions: {
    target: string;
    lib: string[];
  };
}

const basicLibConfig: BasicLibConfig = {
  target: "ES2020",
  lib: ["ES2020"]
};

// Example 3.2: Full Browser Config
// -----------------------------

interface BrowserLibConfig {
  compilerOptions: {
    target: string;
    lib: string[];
  };
}

const browserLibConfig: BrowserLibConfig = {
  target: "ES2020",
  lib: ["ES2020", "DOM", "DOM.Iterable"]
};

// Example 3.3: Node.js Config
// -----------------------------

interface NodeLibConfig {
  compilerOptions: {
    target: string;
    lib: string[];
  };
}

const nodeLibConfig: NodeLibConfig = {
  target: "ES2020",
  lib: ["ES2020"]
};

// ============================================================================
// SECTION 4: LIB AND TARGET
// ============================================================================

// Example 4.1: Default Lib by Target
// -------------------------------

interface DefaultLibMapping {
  es3: string[];
  es5: string[];
  es6: string[];
  es2020: string[];
}

const defaultLibMapping: DefaultLibMapping = {
  es3: ["ES3"],
  es5: ["ES5"],
  es6: ["ES6", "DOM"],
  es2020: ["ES2020", "DOM"]
};

// ============================================================================
// SECTION 5: COMMON LIB COMBINATIONS
// ============================================================================

// Example 5.1: Modern Browser
// -----------------------

interface ModernBrowserLib {
  target: string;
  lib: string[];
}

const modernBrowser: ModernBrowserLib = {
  target: "ES2020",
  lib: ["ES2020", "DOM", "DOM.Iterable"]
};

// Example 5.2: Node.js Server
// -----------------------

interface NodeServerLib {
  target: string;
  lib: string[];
}

const nodeServer: NodeServerLib = {
  target: "ES2020",
  lib: ["ES2020"]
};

// Example 5.3: Legacy Browser
// -----------------------

interface LegacyBrowserLib {
  target: string;
  lib: string[];
}

const legacyBrowser: LegacyBrowserLib = {
  target: "ES5",
  lib: ["ES5", "DOM"]
};

// ============================================================================
// SECTION 6: WEBWORKER
// ============================================================================

// Example 6.1: Web Worker Configuration
// -------------------------------

interface WebWorkerConfig {
  compilerOptions: {
    target: string;
    lib: string[];
  };
}

const webWorkerConfig: WebWorkerConfig = {
  target: "ES2020",
  lib: ["ES2020", "WebWorker"]
};

// ============================================================================
// SECTION 7: PERFORMANCE
// ============================================================================

// Example 7.1: Lib Impact
// ---------------------

interface LibPerformance {
  moreLibs: string;
  lessLibs: string;
}

const libPerf: LibPerformance = {
  moreLibs: "More types available, slightly slower type checking",
  lessLibs: "Faster type checking, may need manual types"
};

// ============================================================================
// SECTION 8: COMPATIBILITY
// ============================================================================

// Example 8.1: Browser Compatibility
// -------------------------------

interface BrowserLibCompat {
  dom: string;
  dom: string;
  webworker: string;
}

const browserLibCompat: BrowserLibCompat = {
  dom: "All browsers",
  webworker: "Modern browsers with Worker support"
};

// ============================================================================
// SECTION 9: SECURITY
// ============================================================================

// Example 9.1: Lib Security
// ---------------------

interface LibSecurity {
  globalAPIs: string;
  polyfills: string;
}

const libSecurity: LibSecurity = {
  globalAPIs: "Lib defines global APIs - be aware of what's available",
  polyfills: "Consider adding core-js for older targets"
};

// ============================================================================
// SECTION 10: ALTERNATIVES
// ============================================================================

// Alternative approaches:
// 1. @types/node - Node.js type definitions
// 2. core-js - Polyfills for older targets
// 3. @webgpu/types - GPU types
// 4. Custom lib - Create custom .d.ts files

// ============================================================================
// SECTION 11: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/03_Compiler_Options/03_Target_Options.ts
// - 03_DEVELOPMENT/03_Compiler_Options/04_Module_Options.ts
// - 01_FUNDAMENTALS/01_TYPES/11_Type_Libraries/01_Utility_Types_Overview.ts

console.log("\n=== Lib Options Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/03_Compiler_Options/06_Output_Options");