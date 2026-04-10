/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 04 Topic: Debug Configurations Purpose: VS Code debugging setup Difficulty: intermediate UseCase: development,debugging Version: TS4.9+ Compatibility: VS Code, Node.js, Browsers Performance: Debugging efficiency Security: Debug credentials */

/**
 * Debug Configurations - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: VS Code debugging setup
 * 💡 WHY: Configure debugging for TypeScript applications in VS Code
 * 🔧 HOW: launch.json, tasks.json configurations
 */

// ============================================================================
// SECTION 1: WHAT IS DEBUG CONFIGURATION
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// VS Code debug configurations define how to start and attach to processes
// for debugging TypeScript code

interface DebugConfig {
  purpose: string;
  files: string[];
}

const debugConfigInfo: DebugConfig = {
  purpose: "Configure debugging in VS Code",
  files: [".vscode/launch.json", ".vscode/tasks.json"]
};

// ============================================================================
// SECTION 2: LAUNCH CONFIGURATION
// ============================================================================

// Example 2.1: Basic Node.js Launch
// -----------------------------

interface NodeLaunchConfig {
  type: string;
  request: string;
  name: string;
  program: string;
  cwd: string;
  console: string;
  runtimeExecutable: string;
  skipFiles: string[];
}

const nodeLaunchConfig: NodeLaunchConfig = {
  type: "node",
  request: "launch",
  name: "Debug TypeScript",
  program: "${workspaceFolder}/src/index.ts",
  cwd: "${workspaceFolder}",
  console: "integratedTerminal",
  runtimeExecutable: "ts-node",
  skipFiles: ["<node_internals>/**"]
};

// Example 2.2: Node.js Attach Configuration
// -----------------------------------

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

// ============================================================================
// SECTION 3: NODE DEBUGGING WITH ARGS
// ============================================================================

// Example 3.1: Node with Arguments
// -----------------------------

interface NodeArgsConfig {
  type: string;
  request: string;
  name: string;
  program: string;
  args: string[];
  env: Record<string, string>;
}

const nodeArgsConfig: NodeArgsConfig = {
  type: "node",
  request: "launch",
  name: "Debug with Args",
  program: "${workspaceFolder}/src/index.ts",
  args: ["--env", "development"],
  env: {
    NODE_ENV: "development"
  }
};

// ============================================================================
// SECTION 4: COMPOUND CONFIGURATIONS
// ============================================================================

// Example 4.1: Compound Launch
// -----------------------

interface CompoundConfig {
  name: string;
  configurations: string[];
  stopAll: boolean;
}

const compoundConfig: CompoundConfig = {
  name: "Debug All",
  configurations: ["Debug Server", "Debug Client"],
  stopAll: true
};

// ============================================================================
// SECTION 5: PRE-LAUNCH TASKS
// ============================================================================

// Example 5.1: Build Before Debug
// ---------------------------

interface PreLaunchTask {
  name: string;
  type: string;
  command: string;
  problemMatcher: string[];
  group: string;
}

const preLaunchTask: PreLaunchTask = {
  name: "tsc: build",
  type: "shell",
  command: "tsc -b",
  problemMatcher: ["$tsc"],
  group: "build"
};

// Example 5.2: Full Launch Config with Task
// -----------------------------------

interface LaunchWithTask {
  type: string;
  request: string;
  name: string;
  preLaunchTask: string;
  program: string;
}

const launchWithTask: LaunchWithTask = {
  type: "node",
  request: "launch",
  name: "Debug with Build",
  preLaunchTask: "tsc: build",
  program: "${workspaceFolder}/src/index.ts"
};

// ============================================================================
// SECTION 6: ENVIRONMENT VARIABLES
// ============================================================================

// Example 6.1: Environment Configuration
// -------------------------------

interface EnvConfig {
  type: string;
  request: string;
  name: string;
  env: Record<string, string>;
  program: string;
}

const envConfig: EnvConfig = {
  type: "node",
  request: "launch",
  name: "Debug with Env",
  env: {
    DEBUG: "true",
    PORT: "3000",
    DATABASE_URL: "postgres://localhost:5432/mydb"
  },
  program: "${workspaceFolder}/src/index.ts"
};

// ============================================================================
// SECTION 7: SOURCE MAP CONFIGURATION
// ============================================================================

// Example 7.1: Enable Source Maps
// -----------------------------

interface SourceMapConfig {
  type: string;
  request: string;
  name: string;
  runtimeExecutable: string;
  runtimeArgs: string[];
  sourceMaps: boolean;
  console: string;
}

const sourceMapConfig: SourceMapConfig = {
  type: "node",
  request: "launch",
  name: "Debug with Source Maps",
  runtimeExecutable: "ts-node",
  runtimeArgs: ["--enable-source-maps"],
  sourceMaps: true,
  console: "integratedTerminal"
};

// ============================================================================
// SECTION 8: BROWSER DEBUGGING
// ============================================================================

// Example 8.1: Chrome Debugging
// -----------------------

interface ChromeConfig {
  type: string;
  request: string;
  name: string;
  url: string;
  webRoot: string;
}

const chromeConfig: ChromeConfig = {
  type: "chrome",
  request: "launch",
  name: "Debug Chrome",
  url: "http://localhost:3000",
  webRoot: "${workspaceFolder}"
};

// ============================================================================
// SECTION 9: VS CODE TASKS
// ============================================================================

// Example 9.1: Build Task
// --------------------

interface BuildTask {
  label: string;
  type: string;
  command: string;
  args: string[];
  problemMatcher: string[];
  group: string;
}

const buildTask: BuildTask = {
  label: "tsc: build",
  type: "shell",
  command: "tsc -b",
  args: ["--watch"],
  problemMatcher: ["$tsc"],
  group: "build"
};

// ============================================================================
// SECTION 10: PERFORMANCE
// ============================================================================

// Example 10.1: Debug Performance Tips
// -------------------------------

interface DebugPerformance {
  skipFiles: string;
  smartStep: string;
  sourceMaps: string;
}

const debugPerf: DebugPerformance = {
  skipFiles: "Skip node internals to speed up",
  smartStep: "Enable smart stepping through source",
  sourceMaps: "Use for proper debugging"
};

// ============================================================================
// SECTION 11: COMPATIBILITY
// ============================================================================

// Example 11.1: VS Code Version Support
// ------------------------------------

interface VSCodeVersion {
  minimum: string;
  recommended: string;
}

const vscodeVersion: VSCodeVersion = {
  minimum: "1.40",
  recommended: "1.70 or higher"
};

// ============================================================================
// SECTION 12: SECURITY
// ============================================================================

// Example 12.1: Debug Security
// -----------------------

interface DebugSecurity {
  credentials: string;
  environment: string;
  sourceMaps: string;
}

const debugSecurity: DebugSecurity = {
  credentials: "Never commit debug credentials",
  environment: "Use env vars, not hardcoded secrets",
  sourceMaps: "Disable in production"
};

// ============================================================================
// SECTION 13: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/04_Debugging_Tools/02_Source_Map_Generation.ts
// - 03_DEVELOPMENT/04_Debugging_Tools/03_Node_Debugging.ts
// - 03_DEVELOPMENT/04_Debugging_Tools/04_Browser_Debugging.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/09_Source_Maps.ts

console.log("\n=== Debug Configurations Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/04_Debugging_Tools/02_Source_Map_Generation");