/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 04 Topic: Node Debugging Purpose: Debugging Node.js applications Difficulty: intermediate UseCase: development,backend,server Version: TS4.9+ Compatibility: Node.js 12+, VS Code Performance: Debug performance Security: Debug port security */

/**
 * Node Debugging - Comprehensive Guide
 * =====================================
 * 
 * 📚 WHAT: Debugging Node.js applications
 * 💡 WHY: Debug TypeScript code running in Node.js environment
 * 🔧 HOW: Node inspector, VS Code debugger, source maps
 */

// ============================================================================
// SECTION 1: NODE DEBUGGING OVERVIEW
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// Node.js debugging uses the V8 inspector protocol to debug TypeScript code
// through source maps

interface NodeDebugInfo {
  protocol: string;
  port: number;
  tools: string[];
}

const nodeDebugInfo: NodeDebugInfo = {
  protocol: "V8 Inspector Protocol",
  port: 9229,
  tools: ["VS Code", "Chrome DevTools", "node-inspector"]
};

// ============================================================================
// SECTION 2: ENABLE SOURCE MAPS
// ============================================================================

// Example 2.1: tsconfig.json Settings
// -------------------------------

interface NodeSourceMapConfig {
  compilerOptions: {
    sourceMap: boolean;
    inlineSourceMap: boolean;
  };
}

const nodeSourceMapConfig: NodeSourceMapConfig = {
  compilerOptions: {
    sourceMap: true,
    inlineSourceMap: false
  }
};

// ============================================================================
// SECTION 3: TS-NODE DEBUGGING
// ============================================================================

// Example 3.1: ts-node Configuration
// -------------------------------

interface TSNodeDebugConfig {
  compilerOptions: {
    compiler: string;
    transpileOnly: boolean;
  };
}

const tsNodeDebugConfig: TSNodeDebugConfig = {
  compilerOptions: {
    compiler: "typescript",
    transpileOnly: false
  }
};

// Run with:
// ts-node --inspect-brk src/index.ts
// ts-node --inspect src/index.ts

// ============================================================================
// SECTION 4: NODE INSPECTOR FLAGS
// ============================================================================

// Example 4.1: Inspector Flags
// -----------------------

interface InspectorFlags {
  inspect: string;
  inspectBrk: string;
  enableSourceMaps: string;
}

const inspectorFlags: InspectorFlags = {
  inspect: "--inspect - Enable inspector",
  inspectBrk: "--inspect-brk - Break on first line",
  enableSourceMaps: "--enable-source-maps - Enable source maps"
};

// ============================================================================
// SECTION 5: VS CODE NODE DEBUGGING
// ============================================================================

// Example 5.1: VS Code Launch Configuration
// -----------------------------------

interface VSCodeNodeDebugConfig {
  type: string;
  request: string;
  name: string;
  program: string;
  runtimeExecutable: string;
  runtimeArgs: string[];
  console: string;
}

const vscodeNodeDebugConfig: VSCodeNodeDebugConfig = {
  type: "node",
  request: "launch",
  name: "Debug Node.js",
  program: "${workspaceFolder}/src/index.ts",
  runtimeExecutable: "ts-node",
  runtimeArgs: ["--enable-source-maps"],
  console: "integratedTerminal"
};

// ============================================================================
// SECTION 6: ATTACH TO RUNNING PROCESS
// ============================================================================

// Example 6.1: Attach Configuration
// -----------------------------

interface NodeAttachConfig {
  type: string;
  request: string;
  name: string;
  port: number;
  restart: boolean;
  skipFiles: string[];
}

const nodeAttachConfig: NodeAttachConfig = {
  type: "node",
  request: "attach",
  name: "Attach to Node",
  port: 9229,
  restart: true,
  skipFiles: ["<node_internals>/**"]
};

// Start app with: node --inspect=9229 dist/index.js

// ============================================================================
// SECTION 7: ENVIRONMENT VARIABLES
// ============================================================================

// Example 7.1: Debug Environment
// -----------------------

interface DebugEnvConfig {
  env: Record<string, string>;
  NODE_OPTIONS: string;
}

const debugEnvConfig: DebugEnvConfig = {
  env: {
    DEBUG: "true",
    NODE_ENV: "development"
  },
  NODE_OPTIONS: "--inspect"
};

// ============================================================================
// SECTION 8: BREAKPOINTS
// ============================================================================

// Example 8.1: Line Breakpoints
// -----------------------

// Set breakpoints by clicking in the gutter in VS Code
// Or use: debugger;

// Example 8.2: Conditional Breakpoints
// -------------------------------

// Right-click on breakpoint > Edit Condition
// Expression: user.id === 1

// ============================================================================
// SECTION 9: CALL STACK
// ============================================================================

// Example 9.1: Understanding Call Stack
// -------------------------------

// The call stack shows the execution path
// Stack frames can be clicked to navigate to source

// ============================================================================
// SECTION 10: PERFORMANCE
// ============================================================================

// Example 10.1: Debug Performance Tips
// -------------------------------

interface DebugPerformance {
  skipFiles: string;
  performance: string;
}

const debugPerf: DebugPerformance = {
  skipFiles: "Skip <node_internals>/** for faster stepping",
  performance: "Debugging adds overhead but manageable"
};

// ============================================================================
// SECTION 11: COMPATIBILITY
// ============================================================================

// Example 11.1: Node Version Support
// -------------------------------

interface NodeVersionSupport {
  minimum: string;
  recommended: string;
}

const nodeVersionSupport: NodeVersionSupport = {
  minimum: "6.0",
  recommended: "18.0 or higher"
};

// ============================================================================
// SECTION 12: SECURITY
// ============================================================================

// Example 12.1: Debug Security
// -----------------------

interface DebugSecurity {
  port: string;
  credentials: string;
}

const debugSecurity: DebugSecurity = {
  port: "Don't expose debug port to internet",
  credentials: "Use authentication in production"
};

// ============================================================================
// SECTION 13: TROUBLESHOOTING
// ============================================================================

// Example 13.1: Common Issues
// -----------------------

interface NodeDebugIssues {
  sourceMaps: string;
  breakpoints: string;
  attach: string;
}

const nodeDebugIssues: NodeDebugIssues = {
  sourceMaps: "Ensure --enable-source-maps flag",
  sourceMaps: "Check sourceMap: true in tsconfig",
  attach: "Verify port is correct and not in use"
};

// ============================================================================
// SECTION 14: ALTERNATIVES
// ============================================================================

// Alternative debugging tools:
// 1. Chrome DevTools - Browser-based debugger
// 2. ndb - Improved Node debugging
// 3. nodemon - Auto-restart with debugging
// 4. winston - Logging for debugging

// ============================================================================
// SECTION 15: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/04_Debugging_Tools/01_Debug_Configurations.ts
// - 03_DEVELOPMENT/04_Debugging_Tools/02_Source_Map_Generation.ts
// - 03_DEVELOPMENT/04_Debugging_Tools/04_Browser_Debugging.ts
// - 03_DEVELOPMENT/04_Debugging_Tools/05_Error_Messages.ts

console.log("\n=== Node Debugging Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/04_Debugging_Tools/04_Browser_Debugging");