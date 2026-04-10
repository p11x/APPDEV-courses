/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 02 Topic: Path Mapping Purpose: Configuring path aliases and mappings Difficulty: intermediate UseCase: monorepo,modular-applications Version: TS2.0+ Compatibility: Node.js, Browsers Performance: Module resolution Security: Path traversal protection */

/**
 * Path Mapping - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Configuring path aliases and mappings
 * 💡 WHY: Simplify imports, improve code organization, enable monorepo patterns
 * 🔧 HOW: baseUrl, paths in tsconfig.json
 */

// ============================================================================
// SECTION 1: WHAT IS PATH MAPPING
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// Path mapping allows you to use custom import paths instead of relative paths
// This makes imports cleaner and easier to maintain

interface PathMapping {
  before: string;
  after: string;
}

const basicMapping: PathMapping = {
  before: "../../../utils/helper",
  after: "@shared/utils/helper"
};

// Example 1.2: Use Cases
// -----------------

interface UseCases {
  monorepo: string;
  alias: string;
  cleanup: string;
}

const useCases: UseCases = {
  monorepo: "Reference packages by name",
  alias: "Create short aliases for long paths",
  cleanup: "Simplify deep relative paths"
};

// ============================================================================
// SECTION 2: BASIC PATH MAPPING
// ============================================================================

// Example 2.1: tsconfig Configuration
// -------------------------------

interface PathMappingConfig {
  compilerOptions: {
    baseUrl: string;
    paths: Record<string, string[]>;
  };
}

const pathMappingConfig: PathMappingConfig = {
  compilerOptions: {
    baseUrl: "./src",
    paths: {
      "@/*": ["./*"],
      "@components/*": ["./components/*"],
      "@utils/*": ["./utils/*"]
    }
  }
};

// Example 2.2: More Complex Mapping
// ---------------------------

interface ComplexMapping {
  baseUrl: string;
  paths: Record<string, string[]>;
}

const complexMapping: ComplexMapping = {
  baseUrl: ".",
  paths: {
    "@myorg/types": ["./packages/types/src/index"],
    "@myorg/utils": ["./packages/utils/src/index"],
    "@myorg/components": ["./packages/components/src/index"],
    "@shared/*": ["./packages/shared/src/*"]
  }
};

// ============================================================================
// SECTION 3: PATH MAPPING WITH GLOB
// ============================================================================

// Example 3.1: Wildcard Matching
// -----------------------

interface WildcardMapping {
  baseUrl: string;
  paths: Record<string, string[]>;
}

const wildcardMapping: WildcardMapping = {
  baseUrl: "./src",
  paths: {
    "@features/*": ["./features/*"],
    "@components/*": ["./components/*"],
    "@utils/*": ["./utils/*"]
  }
};

// Example 3.2: Multiple Resolution Paths
// ---------------------------------

interface MultiPathMapping {
  paths: Record<string, string[]>;
}

const multiPathMapping: MultiPathMapping = {
  paths: {
    "@utils/*": [
      "./utils/*",
      "./common/utils/*",
      "./shared/utils/*"
    ]
  }
};

// ============================================================================
// SECTION 4: MONOREPO PATH MAPPING
// ============================================================================

// Example 4.1: Monorepo Structure
// ---------------------------

interface MonorepoPathMapping {
  baseUrl: string;
  paths: Record<string, string[]>;
}

const monorepoPaths: MonorepoPathMapping = {
  baseUrl: ".",
  paths: {
    "@myorg/types": ["./packages/types/src/index"],
    "@myorg/utils": ["./packages/utils/src/index"],
    "@myorg/shared": ["./packages/shared/src/index"],
    "@myorg/ui": ["./packages/ui/src/index"],
    "@myorg/api-client": ["./packages/api-client/src/index"]
  }
};

// Example 4.2: Workspace Package Reference
// ----------------------------------

interface WorkspaceReference {
  name: string;
  path: string;
  types: string;
}

const workspaceRef: WorkspaceReference = {
  name: "@myorg/utils",
  path: "./packages/utils",
  types: "dist/index.d.ts"
};

// ============================================================================
// SECTION 5: PATH MAPPING WITH PROJECT REFERENCES
// ============================================================================

// Example 5.1: Combined Configuration
// -------------------------------

interface CombinedConfig {
  compilerOptions: {
    baseUrl: string;
    paths: Record<string, string[]>;
    composite: boolean;
    outDir: string;
  };
  references: { path: string }[];
}

const combinedConfig: CombinedConfig = {
  compilerOptions: {
    baseUrl: ".",
    paths: {
      "@shared/*": ["./shared/src/*"]
    },
    composite: true,
    outDir: "./dist"
  },
  references: [
    { path: "../shared" }
  ]
};

// ============================================================================
// SECTION 6: IDE INTEGRATION
// ============================================================================

// Example 6.1: VS Code Path Support
// ---------------------------

interface VSCodePathConfig {
  typescript: {
    tsdk: string;
    suggest: {
      autoImports: boolean;
      paths: boolean;
    };
  };
}

const vscodeConfig: VSCodePathConfig = {
  typescript: {
    tsdk: "node_modules/typescript/lib",
    suggest: {
      autoImports: true,
      paths: true
    }
  }
};

// ============================================================================
// SECTION 7: PERFORMANCE
// ============================================================================

// Example 7.1: Performance Considerations
// -----------------------------------

interface PerformanceConfig {
  baseUrl: string;
  paths: Record<string, string[]>;
  suggestion: string;
}

const perfConfig: PerformanceConfig = {
  baseUrl: "./src",
  paths: {
    "@/*": ["./*"]
  },
  suggestion: "Use specific paths, avoid overly broad wildcards"
};

// Example 7.2: Module Resolution
// -----------------------

interface ModuleResolution {
  strategy: string;
  traceResolution: boolean;
}

const moduleRes: ModuleResolution = {
  strategy: "node",
  traceResolution: false
};

// ============================================================================
// SECTION 8: COMPATIBILITY
// ============================================================================

// Example 8.1: TypeScript Version Support
// ------------------------------------

interface TsVersionSupport {
  minimum: string;
  recommended: string;
}

const tsVersion: TsVersionSupport = {
  minimum: "2.0",
  recommended: "4.5 or higher"
};

// Example 8.2: Module System Support
// ------------------------------

interface ModuleSystemSupport {
  commonjs: string;
  esm: string;
  amd: string;
}

const moduleSupport: ModuleSystemSupport = {
  commonjs: "Supported",
  esm: "Supported",
  amd: "Supported"
};

// ============================================================================
// SECTION 9: SECURITY
// ============================================================================

// Example 9.1: Path Security
// ---------------------

interface PathSecurity {
  traversal: string;
  validation: string;
}

const pathSecurity: PathSecurity = {
  traversal: "Ensure paths don't allow directory traversal",
  validation: "Validate resolved paths are within project"
};

// ============================================================================
// SECTION 10: COMMON ISSUES
// ============================================================================

// Example 10.1: Troubleshooting
// -----------------------

interface Troubleshooting {
  notFound: string;
  wrongPath: string;
  ideNotWorking: string;
}

const troubleshooting: Troubleshooting = {
  notFound: "Check baseUrl is correct relative to tsconfig.json",
  wrongPath: "Verify path patterns match actual file structure",
  ideNotWorking: "Restart TypeScript server or reload VS Code"
};

// ============================================================================
// SECTION 11: ALTERNATIVES
// ============================================================================

// Alternative approaches:
// 1. Relative imports - Simple but verbose
// 2. npm aliases - Package name aliases
// 3. Module aliases - webpack/rollup aliases
// 4. File aliases - bundler-specific aliases
// 5. tsconfig-paths - Node.js path resolution

// ============================================================================
// SECTION 12: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/02_TypeScript_Workspace/01_Monorepo_Setup.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/06_Workspace_Sharing.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/08_Declaration_Maps.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/09_Source_Maps.ts

console.log("\n=== Path Mapping Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/02_TypeScript_Workspace/08_Declaration_Maps");