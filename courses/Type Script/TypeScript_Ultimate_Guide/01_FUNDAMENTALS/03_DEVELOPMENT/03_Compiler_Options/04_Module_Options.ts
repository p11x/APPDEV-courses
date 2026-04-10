/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 03 Topic: Module Options Purpose: Configuring module systems Difficulty: beginner UseCase: browser,server,universal Version: TS1.0+ Compatibility: Node.js, Browsers Performance: Module loading Security: Module isolation */

/**
 * Module Options - Comprehensive Guide
 * =====================================
 * 
 * 📚 WHAT: Configuring module systems
 * 💡 WHY: Determine how code is organized and loaded at runtime
 * 🔧 HOW: module, moduleResolution options in tsconfig.json
 */

// ============================================================================
// SECTION 1: WHAT ARE MODULE OPTIONS
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// Module options determine the module format of compiled JavaScript
// This affects how code is organized and loaded

interface ModuleOption {
  purpose: string;
  affects: string[];
}

const moduleInfo: ModuleOption = {
  purpose: "Module format for output JavaScript",
  affects: ["import/export syntax", "runtime behavior", "bundler compatibility"]
};

// ============================================================================
// SECTION 2: AVAILABLE MODULE FORMATS
// ============================================================================

// Example 2.1: Module Types
// -----------------------

interface ModuleTypes {
  none: string;
  commonjs: string;
  amd: string;
  umd: string;
  system: string;
  es2015: string;
  es2020: string;
  node16: string;
  nodenext: string;
}

const moduleTypes: ModuleTypes = {
  none: "No module system (globals)",
  commonjs: "Node.js CommonJS",
  amd: "RequireJS/AMD",
  umd: "Universal Module Definition",
  system: "SystemJS",
  es2015: "ES Modules (ES6)",
  es2020: "ES Modules (ES2020)",
  node16: "Node.js native modules",
  nodenext: "Node.js with resolution"
};

// ============================================================================
// SECTION 3: COMMONJS
// ============================================================================

// Example 3.1: CommonJS Syntax
// -----------------------

// Source:
import { helper } from "./utils";
export function process(data: string): string {
  return helper(data);
}

// Compiled to CommonJS:
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.process = void 0;
const utils_1 = require("./utils");
function process(data) {
  return (0, utils_1.helper)(data);
}
exports.process = process;

// ============================================================================
// SECTION 4: ES MODULES
// ============================================================================

// Example 4.1: ES Module Syntax
// -----------------------

// Source:
import { helper } from "./utils";
import type { Config } from "./types";
export function process(data: string): string {
  return helper(data);
}

// Compiled to ES2020:
import { helper } from "./utils";
import type { Config } from "./types";
export function process(data) {
  return helper(data);
}

// Example 4.2: ES Module Configuration
// -------------------------------

interface ESModuleConfig {
  module: string;
  moduleResolution: string;
  esModuleInterop: boolean;
}

const esModuleConfig: ESModuleConfig = {
  module: "ES2020",
  moduleResolution: "node",
  esModuleInterop: true
};

// ============================================================================
// SECTION 5: MODULE RESOLUTION
// ============================================================================

// Example 5.1: Module Resolution Strategies
// -------------------------------------

interface ModuleResolutionStrategies {
  node: string;
  node16: string;
  nodenext: string;
  classic: string;
}

const moduleResStrategies: ModuleResolutionStrategies = {
  node: "Node.js style resolution",
  node16: "Node.js 12+ with package.json exports",
  nodenext: "Node.js 12+ with full ESM support",
  classic: "Classic TypeScript resolution"
};

// ============================================================================
// SECTION 6: CONFIGURATION
// ============================================================================

// Example 6.1: Node.js Configuration
// -------------------------------

interface NodeConfig {
  compilerOptions: {
    module: string;
    moduleResolution: string;
    target: string;
    outDir: string;
  };
}

const nodeConfig: NodeConfig = {
  compilerOptions: {
    module: "CommonJS",
    moduleResolution: "node",
    target: "ES2020",
    outDir: "./dist"
  }
};

// Example 6.2: Browser/ES Module Configuration
// -------------------------------------

interface BrowserConfig {
  compilerOptions: {
    module: string;
    moduleResolution: string;
    target: string;
    lib: string[];
  };
}

const browserConfig: BrowserConfig = {
  compilerOptions: {
    module: "ES2020",
    moduleResolution: "node",
    target: "ES2020",
    lib: ["ES2020", "DOM"]
  }
};

// ============================================================================
// SECTION 7: ESMODULEINTEROP
// ============================================================================

// Example 7.1: Enabling Interop
// -----------------------

interface InteropConfig {
  compilerOptions: {
    esModuleInterop: boolean;
    allowSyntheticDefaultImports: boolean;
  };
}

const interopConfig: InteropConfig = {
  compilerOptions: {
    esModuleInterop: true,
    allowSyntheticDefaultImports: true
  }
};

// Without esModuleInterop:
// import * as fs from 'fs';  // Required
// With esModuleInterop:
// import fs from 'fs';  // Works!

// ============================================================================
// SECTION 8: NODE16/NEXT
// ============================================================================

// Example 8.1: Node.js Native Modules
// -------------------------------

interface Node16Config {
  compilerOptions: {
    module: string;
    moduleResolution: string;
  };
}

const node16Config: Node16Config = {
  module: "node16",
  moduleResolution: "node16"
};

// Example 8.2: Package.json Exports
// ---------------------------

interface PackageExports {
  exports: {
    ".": string;
    "./utils": "./dist/utils.js";
    "./types": "./dist/types.d.ts";
  };
}

const packageExports: PackageExports = {
  exports: {
    ".": "./dist/index.js",
    "./utils": "./dist/utils.js",
    "./types": "./dist/types.d.ts"
  }
};

// ============================================================================
// SECTION 9: PERFORMANCE
// ============================================================================

// Example 9.1: Module Performance
// -----------------------

interface ModulePerformance {
  commonjs: string;
  esm: string;
}

const modulePerf: ModulePerformance = {
  commonjs: "Native Node.js support, faster loading",
  esm: "Better tree-shaking, modern bundlers"
};

// ============================================================================
// SECTION 10: COMPATIBILITY
// ============================================================================

// Example 10.1: Environment Compatibility
// -------------------------------

interface EnvCompatibility {
  node: string;
  browser: string;
  bundler: string;
}

const envCompat: EnvCompatibility = {
  node: "CommonJS, ESM, node16, nodenext",
  browser: "ESM (webpack, rollup, vite)",
  bundler: "ESM, CommonJS, AMD"
};

// ============================================================================
// SECTION 11: SECURITY
// ============================================================================

// Example 11.1: Module Security
// -----------------------

interface ModuleSecurity {
  isolation: string;
  circular: string;
}

const moduleSecurity: ModuleSecurity = {
  isolation: "Modules provide code isolation",
  circular: "Handle circular dependencies carefully"
};

// ============================================================================
// SECTION 12: ALTERNATIVES
// ============================================================================

// Alternative module systems:
// 1.AMD - For browser modules (RequireJS)
// 2.UMD - Universal (browser + Node.js)
// 3.SystemJS - Dynamic module loader
// 4.ESBuild - Fast bundler with ESM

// ============================================================================
// SECTION 13: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/03_Compiler_Options/01_Compiler_Flags.ts
// - 03_DEVELOPMENT/03_Compiler_Options/03_Target_Options.ts
// - 03_DEVELOPMENT/03_Compiler_Options/05_Lib_Options.ts
// - 01_FUNDAMENTALS/02_SYNTAX/04_Modules_and_Namespaces.ts

console.log("\n=== Module Options Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/03_Compiler_Options/05_Lib_Options");