/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 02 Topic: Composite Projects Purpose: Setting up composite projects Difficulty: intermediate UseCase: enterprise,large-scale-applications Version: TS3.0+ Compatibility: Node.js, Browsers Performance: Project isolation, proper dependency tracking Security: Type isolation */

/**
 * Composite Projects - Comprehensive Guide
 * ========================================
 * 
 * 📚 WHAT: Setting up composite projects
 * 💡 WHY: Enable proper project references and incremental builds
 * 🔧 HOW: composite flag, declaration files, project references
 */

// ============================================================================
// SECTION 1: WHAT ARE COMPOSITE PROJECTS
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// A composite project is a TypeScript project that can be referenced by other
// projects. It provides proper type isolation and build orchestration.

interface CompositeProject {
  name: string;
  composite: boolean;
  outDir: string;
  rootDir: string;
}

const exampleProject: CompositeProject = {
  name: "shared-utils",
  composite: true,
  outDir: "./dist",
  rootDir: "./src"
};

// Example 1.2: Composite vs Non-Composite
// ----------------------------------

interface ProjectComparison {
  composite: string[];
  nonComposite: string[];
}

const comparison: ProjectComparison = {
  composite: [
    "Can be referenced by other projects",
    "Generates .tsbuildinfo cache",
    "Requires outDir and rootDir",
    "Always generates declarations"
  ],
  nonComposite: [
    "Cannot be referenced via project references",
    "No build cache",
    "Optional outDir and rootDir",
    "Optional declarations"
  ]
};

// ============================================================================
// SECTION 2: CONFIGURING COMPOSITE PROJECTS
// ============================================================================

// Example 2.1: Basic Composite Config
// -------------------------------

interface CompositeConfig {
  compilerOptions: {
    composite: boolean;
    outDir: string;
    rootDir: string;
    declaration: boolean;
    declarationMap: boolean;
  };
  include: string[];
  exclude?: string[];
}

const basicComposite: CompositeConfig = {
  compilerOptions: {
    composite: true,
    outDir: "./dist",
    rootDir: "./src",
    declaration: true,
    declarationMap: true
  },
  include: ["src/**/*"],
  exclude: ["src/**/*.test.ts"]
};

// Example 2.2: Complete Package Config
// -------------------------------

interface FullCompositeConfig {
  extends: string;
  compilerOptions: CompositeConfig["compilerOptions"] & {
    target: string;
    module: string;
    lib: string[];
    strict: boolean;
    esModuleInterop: boolean;
  };
  include: string[];
}

const fullComposite: FullCompositeConfig = {
  extends: "../../tsconfig.base.json",
  compilerOptions: {
    composite: true,
    outDir: "./dist",
    rootDir: "./src",
    declaration: true,
    declarationMap: true,
    target: "ES2020",
    module: "commonjs",
    lib: ["ES2020"],
    strict: true,
    esModuleInterop: true
  },
  include: ["src/**/*"]
};

// ============================================================================
// SECTION 3: PROJECT REFERENCES WITH COMPOSITE
// ============================================================================

// Example 3.1: Referencing Composite Projects
// ------------------------------------

interface ReferenceConfig {
  references: { path: string }[];
}

const withReferences: ReferenceConfig = {
  references: [
    { path: "../shared" },
    { path: "../utils" },
    { path: "../types" }
  ]
};

// Example 3.2: Reference Build Order
// ------------------------------

interface BuildOrder {
  step1: string;
  step2: string;
  step3: string;
}

const buildOrder: BuildOrder = {
  step1: "tsc -b packages/shared",
  step2: "tsc -b packages/utils",
  step3: "tsc -b packages/frontend"
};

// ============================================================================
// SECTION 4: MONOREPO STRUCTURE
// ============================================================================

// Example 4.1: Multi-Project Setup
// ---------------------------

interface MonorepoStructure {
  root: string;
  packages: PackageConfig[];
}

interface PackageConfig {
  name: string;
  path: string;
  type: "library" | "application";
}

const monorepo: MonorepoStructure = {
  root: "packages",
  packages: [
    { name: "@myorg/types", path: "types", type: "library" },
    { name: "@myorg/utils", path: "utils", type: "library" },
    { name: "@myorg/shared", path: "shared", type: "library" },
    { name: "@myorg/frontend", path: "frontend", type: "application" },
    { name: "@myorg/backend", path: "backend", type: "application" }
  ]
};

// Example 4.2: Root tsconfig.json
// ---------------------------

interface RootTsConfig {
  files: string[];
  references: { path: string }[];
}

const rootConfig: RootTsConfig = {
  files: [],
  references: [
    { path: "./packages/types" },
    { path: "./packages/utils" },
    { path: "./packages/shared" },
    { path: "./packages/frontend" },
    { path: "./packages/backend" }
  ]
};

// ============================================================================
// SECTION 5: BUILD ORCHESTRATION
// ============================================================================

// Example 5.1: Build Scripts
// ----------------------

interface BuildScripts {
  scripts: Record<string, string>;
}

const buildScripts: BuildScripts = {
  scripts: {
    "build": "tsc -b",
    "build:types": "tsc -b packages/types",
    "build:utils": "tsc -b packages/utils",
    "build:shared": "tsc -b packages/shared",
    "clean": "tsc -b --clean && rm -rf packages/*/dist"
  }
};

// Example 5.2: Build Dependencies
// ---------------------------

interface DependencyGraph {
  frontend: string[];
  backend: string[];
  shared: string[];
  utils: string[];
  types: string[];
}

const dependencyGraph: DependencyGraph = {
  frontend: ["shared", "utils", "types"],
  backend: ["shared", "utils", "types"],
  shared: ["types"],
  utils: ["types"],
  types: []
};

// ============================================================================
// SECTION 6: TYPE SHARING
// ============================================================================

// Example 6.1: Shared Types
// ----------------------

interface SharedTypes {
  common: string;
  types: string;
  index: string;
}

const sharedTypes: SharedTypes = {
  common: "export * from './common'",
  types: "export * from './types'",
  index: "export { type Common, type Types } from './index'"
};

// Example 6.2: Type Resolution
// -----------------------

interface TypeResolution {
  projectReferences: boolean;
  nodeResolution: boolean;
  baseUrl: string;
}

const typeResolution: TypeResolution = {
  projectReferences: true,
  nodeResolution: true,
  baseUrl: "./src"
};

// ============================================================================
// SECTION 7: PERFORMANCE
// ============================================================================

// Example 7.1: Performance Optimization
// ---------------------------------

interface PerformanceConfig {
  incremental: boolean;
  parallel: boolean;
  skipLibCheck: boolean;
}

const perfConfig: PerformanceConfig = {
  incremental: true,
  parallel: true,
  skipLibCheck: true
};

// ============================================================================
// SECTION 8: COMPATIBILITY
// ============================================================================

// Example 8.1: Version Requirements
// -----------------------------

interface VersionInfo {
  minimum: string;
  recommended: string;
}

const versionInfo: VersionInfo = {
  minimum: "3.0",
  recommended: "4.5 or higher"
};

// ============================================================================
// SECTION 9: SECURITY
// ============================================================================

// Example 9.1: Security Considerations
// -------------------------------

interface SecurityConfig {
  strict: boolean;
  noImplicitAny: boolean;
  strictNullChecks: boolean;
}

const securityConfig: SecurityConfig = {
  strict: true,
  noImplicitAny: true,
  strictNullChecks: true
};

// ============================================================================
// SECTION 10: TESTING
// ============================================================================

// Example 10.1: Test Configuration
// ---------------------------

interface TestSetup {
  testFramework: string;
  coverage: boolean;
  references: string[];
}

const testSetup: TestSetup = {
  testFramework: "jest",
  coverage: true,
  references: ["../shared", "../utils"]
};

// ============================================================================
// SECTION 11: ALTERNATIVES
// ============================================================================

// Alternative approaches:
// 1. npm workspaces with symlinks
// 2. Yarn Berry with workspaces
// 3. pnpm with strict dependencies
// 4. Turborepo with remote caching
// 5. Nx with distributed caching

// ============================================================================
// SECTION 12: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/02_TypeScript_Workspace/01_Monorepo_Setup.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/02_Project_References.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/03_Incremental_Builds.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/06_Workspace_Sharing.ts

console.log("\n=== Composite Projects Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/02_TypeScript_Workspace/05_Build_Modes");