/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 02 Topic: Project References Purpose: Using TypeScript project references Difficulty: intermediate UseCase: enterprise,large-scale-applications Version: TS3.0+ Compatibility: Node.js, Browsers Performance: Incremental builds, parallel compilation Security: Type isolation */

/**
 * Project References - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Using TypeScript project references
 * 💡 WHY: Enable incremental builds and proper type checking across projects
 * 🔧 HOW: tsconfig.json references, composite projects, build modes
 */

// ============================================================================
// SECTION 1: WHAT ARE PROJECT REFERENCES
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// Project references allow TypeScript to understand the relationship
// between projects in a monorepo, enabling:
// - Incremental compilation
// - Type checking across project boundaries
// - Proper build ordering
// - Declaration file generation

interface ProjectReference {
  path: string;
  prepend?: boolean;
}

// Example 1.2: Reference Structure
// ----------------------------

interface TsConfigWithReferences {
  compilerOptions: Record<string, unknown>;
  references: ProjectReference[];
}

const referencingConfig: TsConfigWithReferences = {
  compilerOptions: {
    composite: true,
    outDir: "./dist"
  },
  references: [
    { path: "../shared" },
    { path: "../utils" }
  ]
};

// ============================================================================
// SECTION 2: SETTING UP REFERENCES
// ============================================================================

// Example 2.1: Root Project Configuration
// ----------------------------------

interface RootProjectTsConfig {
  files: string[];
  references: ProjectReference[];
}

const rootProjectConfig: RootProjectTsConfig = {
  files: [],
  references: [
    { path: "./packages/shared" },
    { path: "./packages/frontend" },
    { path: "./packages/backend" }
  ]
};

// Example 2.2: Referenced Project Configuration
// -------------------------------------

interface ReferencedProjectConfig {
  compilerOptions: {
    composite: boolean;
    outDir: string;
    rootDir: string;
    declaration: boolean;
    declarationMap: boolean;
  };
  include: string[];
}

const sharedProjectConfig: ReferencedProjectConfig = {
  compilerOptions: {
    composite: true,
    outDir: "./dist",
    rootDir: "./src",
    declaration: true,
    declarationMap: true
  },
  include: ["src/**/*"]
};

// ============================================================================
// SECTION 3: BUILDING WITH REFERENCES
// ============================================================================

// Example 3.1: Building All Projects
// ------------------------------

interface BuildCommands {
  buildAll: string;
  buildSingle: string;
  buildWithReferences: string;
}

const buildCommands: BuildCommands = {
  buildAll: "tsc -b",                          // Build all references
  buildSingle: "tsc -b packages/frontend",     // Build single project
  buildWithReferences: "tsc -b --force"        // Force rebuild all
};

// Example 3.2: Incremental Build Behavior
// ----------------------------------

interface IncrementalBehavior {
  unchangedFiles: string;
  changedFiles: string;
  autoBuild: string;
}

const incBehavior: IncrementalBehavior = {
  unchangedFiles: "Skip compilation (use cached)",
  changedFiles: "Rebuild + dependent projects",
  autoBuild: "tsc --build --watch for watch mode"
};

// ============================================================================
// SECTION 4: TYPE CHECKING ACROSS REFERENCES
// ============================================================================

// Example 4.1: Type Propagation
// -------------------------

// When project A references project B:
// - Types from B are available in A
// - Changes in B trigger rebuild of A
// - Full type checking includes referenced types

interface TypeCheckingConfig {
  compilerOptions: {
    skipLibCheck: boolean;
    strict: boolean;
  };
}

const typeCheckConfig: TypeCheckingConfig = {
  compilerOptions: {
    skipLibCheck: true,
    strict: true
  }
};

// Example 4.2: Composite Project Types
// ------------------------------

interface CompositeProject {
  name: string;
  tsconfig: string;
  outDir: string;
  type: "library" | "application";
}

const projects: CompositeProject[] = [
  { name: "shared", tsconfig: "./tsconfig.json", outDir: "./dist", type: "library" },
  { name: "frontend", tsconfig: "./tsconfig.json", outDir: "./dist", type: "application" },
  { name: "backend", tsconfig: "./tsconfig.json", outDir: "./dist", type: "application" }
];

// ============================================================================
// SECTION 5: PERFORMANCE OPTIMIZATION
// ============================================================================

// Example 5.1: Parallel Builds
// -----------------------

interface ParallelBuildConfig {
  parallel: boolean;
  maxWorkers: number;
}

const parallelConfig: ParallelBuildConfig = {
  parallel: true,
  maxWorkers: 4  // Based on CPU cores
};

// Example 5.2: Build Cache
// -------------------

interface BuildCacheConfig {
  incremental: boolean;
  cacheDirectory: string;
}

const cacheConfig: BuildCacheConfig = {
  incremental: true,
  cacheDirectory: ".tsbuildinfo"
};

// ============================================================================
// SECTION 6: COMPOSITE PROJECTS
// ============================================================================

// Example 6.1: Composite Project Setup
// --------------------------------

interface CompositeConfig {
  compilerOptions: {
    composite: boolean;
    declaration: boolean;
    declarationMap: boolean;
    outDir: string;
  };
}

const compositeConfig: CompositeConfig = {
  compilerOptions: {
    composite: true,
    declaration: true,
    declarationMap: true,
    outDir: "./dist"
  }
};

// Example 6.2: References with Prepend
// -------------------------------

// prepend: true adds the referenced project's output before this project's output
const referenceWithPrepend: ProjectReference = {
  path: "../common",
  prepend: true
};

// ============================================================================
// SECTION 7: BUILD MODES
// ============================================================================

// Example 7.1: Different Build Modes
// ---------------------------

interface BuildModes {
  full: string;
  incremental: string;
  watch: string;
  emit: string;
}

const buildModes: BuildModes = {
  full: "tsc -b --force",
  incremental: "tsc -b",
  watch: "tsc -b --watch",
  emit: "tsc -b --force --preserveWatchOutput"
};

// Example 7.2: Custom Build Scripts
// -----------------------------

interface CustomBuildScripts {
  prebuild: string;
  build: string;
  postbuild: string;
}

const customScripts: CustomBuildScripts = {
  prebuild: "tsc -b --clean",
  build: "tsc -b",
  postbuild: "tsc-alias"
};

// ============================================================================
// SECTION 8: COMPATIBILITY
// ============================================================================

// Example 8.1: Supported TypeScript Versions
// ------------------------------------

interface TsVersionSupport {
  minimum: string;
  recommended: string;
}

const tsVersion: TsVersionSupport = {
  minimum: "3.0",
  recommended: "4.5 or higher"
};

// Example 8.2: Module Compatibility
// ---------------------------

interface ModuleCompatibility {
  sameProject: string;
  differentModule: string;
}

const moduleCompat: ModuleCompatibility = {
  sameProject: "Direct reference",
  differentModule: "Use outDir and module resolution"
};

// ============================================================================
// SECTION 9: SECURITY CONSIDERATIONS
// ============================================================================

// Example 9.1: Type Safety Between Projects
// -------------------------------------

interface TypeSafetyConfig {
  strict: boolean;
  skipLibCheck: boolean;
  forceConsistentCasingInFileNames: boolean;
}

const safetyConfig: TypeSafetyConfig = {
  strict: true,
  skipLibCheck: true,
  forceConsistentCasingInFileNames: true
};

// ============================================================================
// SECTION 10: TESTING WITH REFERENCES
// ============================================================================

// Example 10.1: Test Configuration
// ---------------------------

interface TestWithReferences {
  include: string[];
  references: string[];
}

const testConfig: TestWithReferences = {
  include: ["src/**/*.test.ts"],
  references: ["../shared"]
};

// ============================================================================
// SECTION 11: DEBUGGING
// ============================================================================

// Example 11.1: Debug Build Issues
// ---------------------------

interface DebugBuildConfig {
  verbose: boolean;
  traceResolution: boolean;
  logFiles: boolean;
}

const debugConfig: DebugBuildConfig = {
  verbose: false,
  traceResolution: false,
  logFiles: false
};

// ============================================================================
// SECTION 12: TROUBLESHOOTING
// ============================================================================

// Common issues and solutions:
// 1. "Project references must be built first" -> Run tsc -b
// 2. Circular references -> Restructure dependencies
// 3. Type errors in references -> Check declaration files
// 4. Build cache issues -> Delete .tsbuildinfo files

// ============================================================================
// SECTION 13: ALTERNATIVES
// ============================================================================

// Alternative approaches:
// 1. npm/yarn workspaces with symlinks
// 2. TurboRepo - High-performance build system
// 3. Nx - Extensible dev tools
// 4. Lerna - JS package management
// 5. Rush - Large-scale monorepos

// ============================================================================
// SECTION 14: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/02_TypeScript_Workspace/01_Monorepo_Setup.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/03_Incremental_Builds.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/04_Composite_Projects.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/05_Build_Modes.ts

console.log("\n=== Project References Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/02_TypeScript_Workspace/03_Incremental_Builds");