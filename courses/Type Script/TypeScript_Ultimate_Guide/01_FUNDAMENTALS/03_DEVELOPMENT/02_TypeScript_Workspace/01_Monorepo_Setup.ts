/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 02 Topic: Monorepo Setup Purpose: Setting up TypeScript monorepo with workspaces Difficulty: intermediate UseCase: enterprise,large-scale-applications Version: TS4.5+ Compatibility: Node.js 12+, Browsers Performance: Build time optimization Security: Package isolation */

/**
 * Monorepo Setup - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Setting up TypeScript monorepo with workspaces
 * 💡 WHY: Enable code sharing, simplify dependency management, improve build efficiency
 * 🔧 HOW: npm workspaces, yarn workspaces, pnpm workspaces, TypeScript project references
 */

// ============================================================================
// SECTION 1: WHAT IS A MONOREPO
// ============================================================================

// Example 1.1: Monorepo Structure
// ---------------------------

// A monorepo is a single repository that contains multiple projects
// Structure:
//   /packages
//     /shared        - Shared utilities and types
//     /frontend     - Frontend application
//     /backend      - Backend application
//     /cli          - CLI tool

interface MonorepoStructure {
  name: string;
  version: string;
  private: boolean;
  workspaces: string[];
  packages: string[];
}

// Example 1.2: Benefits of Monorepo
// ------------------------------

interface MonorepoBenefits {
  codeSharing: "Share utilities across projects";
  atomicCommits: "Make changes across projects in one commit";
  unifiedDependencies: "Single node_modules, deduplicated deps";
  simplifiedTesting: "Test changes across projects together";
  centralizedConfig: "One linter, one formatter, one build system";
}

// ============================================================================
// SECTION 2: NPM WORKSPACES
// ============================================================================

// Example 2.1: package.json Configuration
// -----------------------------------

interface NpmWorkspacesConfig {
  name: string;
  version: string;
  private: boolean;
  workspaces: string[];
}

const rootPackageJson: NpmWorkspacesConfig = {
  name: "my-monorepo",
  version: "1.0.0",
  private: true,
  workspaces: ["packages/*"]
};

// Example 2.2: Workspace Package Structure
// ------------------------------------

interface WorkspacePackage {
  name: string;
  version: string;
  main: string;
  types: string;
  dependencies: Record<string, string>;
}

const sharedPackage: WorkspacePackage = {
  name: "@myorg/shared",
  version: "1.0.0",
  main: "dist/index.js",
  types: "dist/index.d.ts",
  dependencies: {}
};

const frontendPackage: WorkspacePackage = {
  name: "@myorg/frontend",
  version: "1.0.0",
  main: "dist/index.js",
  types: "dist/index.d.ts",
  dependencies: {
    "@myorg/shared": "^1.0.0"
  }
};

// ============================================================================
// SECTION 3: YARN WORKSPACES
// ============================================================================

// Example 3.1: Yarn Workspaces Configuration
// -------------------------------------

interface YarnWorkspacesConfig {
  name: string;
  private: boolean;
  workspaces: string[];
}

const yarnRootConfig: YarnWorkspacesConfig = {
  name: "my-yarn-monorepo",
  private: true,
  workspaces: ["packages/*"]
};

// Example 3.2: Yarn Workspaces Commands
// ---------------------------------

// yarn install        - Install all workspaces
// yarn workspace @myorg/shared build   - Build specific workspace
// yarn -W run build  - Run command in all workspaces

// ============================================================================
// SECTION 4: PNPM WORKSPACES
// ============================================================================

// Example 4.1: pnpm-workspace.yaml
// ---------------------------

interface PnpmWorkspaceConfig {
  packages: string[];
}

const pnpmWorkspaceYaml: PnpmWorkspaceConfig = {
  packages: ["packages/*"]
};

// Example 4.2: Root package.json
// --------------------------

interface PnpmRootConfig {
  name: string;
  version: string;
  private: boolean;
  scripts: Record<string, string>;
}

const pnpmRoot: PnpmRootConfig = {
  name: "my-pnpm-monorepo",
  version: "1.0.0",
  private: true,
  scripts: {
    "build": "pnpm -r run build",
    "dev": "pnpm -r run dev"
  }
};

// ============================================================================
// SECTION 5: TSCONFIG FOR MONOREPO
// ============================================================================

// Example 5.1: Base tsconfig.json
// ---------------------------

interface BaseTsConfig {
  compilerOptions: {
    target: string;
    module: string;
    lib: string[];
    declaration: boolean;
    strict: boolean;
    esModuleInterop: boolean;
    skipLibCheck: boolean;
  };
}

const baseTsConfig: BaseTsConfig = {
  compilerOptions: {
    target: "ES2020",
    module: "commonjs",
    lib: ["ES2020"],
    declaration: true,
    strict: true,
    esModuleInterop: true,
    skipLibCheck: true
  }
};

// Example 5.2: Package tsconfig.json
// -----------------------------

interface PackageTsConfig extends BaseTsConfig {
  extends: string;
  compilerOptions: BaseTsConfig["compilerOptions"] & {
    outDir: string;
    rootDir: string;
  };
  include: string[];
}

const packageTsConfig: PackageTsConfig = {
  extends: "../../tsconfig.base.json",
  compilerOptions: {
    ...baseTsConfig.compilerOptions,
    outDir: "./dist",
    rootDir: "./src"
  },
  include: ["src/**/*"]
};

// ============================================================================
// SECTION 6: BUILDING WORKSPACES
// ============================================================================

// Example 6.1: Build Script
// ----------------------

interface BuildScriptConfig {
  scripts: Record<string, string>;
}

const monorepoScripts: BuildScriptConfig = {
  scripts: {
    "build": "tsc -b",
    "build:shared": "tsc -b packages/shared",
    "build:frontend": "tsc -b packages/frontend",
    "build:backend": "tsc -b packages/backend",
    "clean": "rm -rf packages/*/dist"
  }
};

// Example 6.2: Running Builds
// -----------------------

// Build all packages in order
// tsc -b packages/shared/tsconfig.json
// tsc -b packages/frontend/tsconfig.json
// tsc -b packages/backend/tsconfig.json

// ============================================================================
// SECTION 7: LINTING AND TESTING
// ============================================================================

// Example 7.1: ESLint in Monorepo
// ---------------------------

interface MonorepoESLintConfig {
  root: boolean;
  packages: string[];
  ignorePatterns: string[];
}

const monorepoEslint: MonorepoESLintConfig = {
  root: true,
  packages: ["packages/*"],
  ignorePatterns: ["**/dist", "**/node_modules"]
};

// Example 7.2: Testing Setup
// -----------------------

interface TestConfig {
  projects: string[];
  coverage: boolean;
}

const monorepoTest: TestConfig = {
  projects: ["<rootDir>/packages/*/jest.config.js"],
  coverage: true
};

// ============================================================================
// SECTION 8: PERFORMANCE CONSIDERATIONS
// ============================================================================

// Example 8.1: Performance Optimization
// ---------------------------------

interface PerformanceConfig {
  incremental: boolean;
  parallel: boolean;
  cache: boolean;
}

const perfConfig: PerformanceConfig = {
  incremental: true,
  parallel: true,
  cache: true
};

// Best practices:
// - Use TypeScript project references
// - Enable incremental compilation
// - Use build caching
// - Parallelize independent builds

// ============================================================================
// SECTION 9: COMPATIBILITY
// ============================================================================

// Example 9.1: Node.js Version Requirements
// -----------------------------------

interface NodeVersionRequirement {
  minimum: string;
  recommended: string;
}

const nodeVersion: NodeVersionRequirement = {
  minimum: "12.20.0",
  recommended: "18.0.0 or higher"
};

// Example 9.2: Package Manager Support
// -------------------------------

interface PackageManagerSupport {
  npm: string;
  yarn: string;
  pnpm: string;
}

const pmSupport: PackageManagerSupport = {
  npm: "7.0.0+",
  yarn: "1.22.0+",
  pnpm: "7.0.0+"
};

// ============================================================================
// SECTION 10: SECURITY CONSIDERATIONS
// ============================================================================

// Example 10.1: Workspace Security
// ---------------------------

interface SecurityConfig {
  privatePackages: boolean;
  access: string;
  publishConfig: Record<string, string>;
}

const securityConfig: SecurityConfig = {
  privatePackages: true,
  access: "restricted",
  publishConfig: {
    registry: "https://npm.pkg.github.com"
  }
};

// Best practices:
// - Keep internal packages private
// - Use scoped packages (@myorg/...)
// - Configure npm access properly
// - Audit dependencies regularly

// ============================================================================
// SECTION 11: TESTING STRATEGIES
// ============================================================================

// Example 11.1: Testing Across Packages
// ---------------------------------

interface PackageTestSetup {
  testEnvironment: string;
  roots: string[];
  testMatch: string[];
}

const testSetup: PackageTestSetup = {
  testEnvironment: "node",
  roots: ["<rootDir>/packages"],
  testMatch: ["**/__tests__/**/*.ts", "**/*.test.ts"]
};

// ============================================================================
// SECTION 12: DEBUGGING
// ============================================================================

// Example 12.1: Debug Configuration
// -----------------------------

interface DebugConfig {
  type: string;
  request: string;
  runtimeExecutable: string;
  env: Record<string, string>;
}

const debugConfig: DebugConfig = {
  type: "node",
  request: "launch",
  runtimeExecutable: "${workspaceFolder}/node_modules/.bin/ts-node",
  env: {
    TS_NODE_PROJECT: "${workspaceFolder}/packages/backend/tsconfig.json"
  }
};

// ============================================================================
// SECTION 13: ALTERNATIVES
// ============================================================================

// Alternative approaches to monorepos:
// 1. Git submodules - Git-based code sharing
// 2. npm link - Local package linking
// 3. Turborepo - Build system for monorepos
// 4. Nx - Extensible dev tools for monorepos
// 5. Lerna - Tool for managing JS monorepos

// ============================================================================
// SECTION 14: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/02_TypeScript_Workspace/02_Project_References.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/03_Incremental_Builds.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/04_Composite_Projects.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/07_Path_Mapping.ts

console.log("\n=== Monorepo Setup Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/02_TypeScript_Workspace/02_Project_References");