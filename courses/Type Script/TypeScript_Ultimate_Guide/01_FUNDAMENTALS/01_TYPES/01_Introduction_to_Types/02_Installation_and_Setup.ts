/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Introduction_to_Types
 * Purpose: Detailed guide to installing and setting up TypeScript
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Installation and Setup - Complete Guide
 * ========================================
 * 
 * 📚 WHAT: Installing TypeScript and configuring the development environment
 * 💡 WHY: Proper setup ensures type safety and optimal development experience
 * 🔧 HOW: npm/yarn/pnpm installation, tsconfig.json configuration, IDE setup
 */

// ============================================================================
// SECTION 1: INSTALLATION METHODS
// ============================================================================

/**
 * TypeScript can be installed in multiple ways:
 * 1. Global installation (not recommended for projects)
 * 2. Local installation (recommended)
 * 3. Using build tools (webpack, vite, etc.)
 * 4. Using Deno (native TypeScript runtime)
 */

// Example 1.1: Global Installation (Development Machine)
// -----------------------------------------------------

// Terminal commands:
// npm install -g typescript
// tsc --version

// Pros: Available everywhere
// Cons: Version conflicts between projects

// Example 1.2: Local Installation (Project-Based - Recommended)
// ----------------------------------------------------------

// In package.json dependencies:
// "dependencies": {
//   "typescript": "^5.3.0"
// }

// Terminal command:
// npm install typescript --save-dev

// Best Practice: Use npm scripts

// package.json example
interface PackageJson {
  name: string;
  version: string;
  scripts: {
    build: string;
    start: string;
    test: string;
    "type-check": string;
  };
  devDependencies: {
    typescript: string;
    "@types/node": string;
  };
}

const packageJson: PackageJson = {
  name: "my-typescript-project",
  version: "1.0.0",
  scripts: {
    build: "tsc",
    start: "npm run build && node dist/index.js",
    test: "jest",
    "type-check": "tsc --noEmit"
  },
  devDependencies: {
    typescript: "^5.3.0",
    "@types/node": "^20.10.0"
  }
};

// Example 1.3: Using Yarn
// --------------------

// Terminal:
// yarn add typescript --dev

// Example 1.4: Using PNPM
// ---------------------

// Terminal:
// pnpm add typescript -D

// ============================================================================
// SECTION 2: COMPILER CONFIGURATION (tsconfig.json)
// ============================================================================

/**
 * The tsconfig.json file is the heart of TypeScript configuration.
 * It controls how TypeScript compiles your code.
 */

// Example 2.1: Basic tsconfig.json
// -------------------------------

interface TsConfig {
  compilerOptions: {
    target: string;
    module: string;
    lib: string[];
    outDir: string;
    rootDir: string;
    strict: boolean;
    esModuleInterop: boolean;
    skipLibCheck: boolean;
    forceConsistentCasingInFileNames: boolean;
  };
  include: string[];
  exclude: string[];
}

const basicTsConfig: TsConfig = {
  compilerOptions: {
    target: "ES2020",
    module: "commonjs",
    lib: ["ES2020"],
    outDir: "./dist",
    rootDir: "./src",
    strict: true,
    esModuleInterop: true,
    skipLibCheck: true,
    forceConsistentCasingInFileNames: true
  },
  include: ["src/**/*"],
  exclude: ["node_modules", "dist"]
};

// Example 2.2: Advanced Configuration
// ---------------------------------

interface AdvancedTsConfig {
  compilerOptions: {
    // Output settings
    target: string;
    module: string;
    outDir: string;
    rootDir: string;
    
    // Library settings
    lib: string[];
    types: string[];
    
    // Type checking (strict mode recommended)
    strict: boolean;
    noImplicitAny: boolean;
    strictNullChecks: boolean;
    strictFunctionTypes: boolean;
    noImplicitReturns: boolean;
    noFallthroughCasesInSwitch: boolean;
    
    // Module resolution
    moduleResolution: string;
    baseUrl: string;
    paths: Record<string, string[]>;
    
    // Emit settings
    declaration: boolean;
    declarationMap: boolean;
    sourceMap: boolean;
    removeComments: boolean;
    
    // Interop
    esModuleInterop: boolean;
    allowSyntheticDefaultImports: boolean;
    forceConsistentCasingInFileNames: boolean;
    
    // Advanced
    skipLibCheck: boolean;
    resolveJsonModule: boolean;
    isolatedModules: boolean;
    incremental: boolean;
    tsBuildInfoFile: string;
  };
  include: string[];
  exclude: string[];
  files: string[];
  references: { path: string }[];
}

const advancedConfig: AdvancedTsConfig = {
  compilerOptions: {
    target: "ES2022",
    module: "ESNext",
    outDir: "./dist",
    rootDir: "./src",
    lib: ["ES2022", "DOM", "DOM.Iterable"],
    types: ["node"],
    strict: true,
    noImplicitAny: true,
    strictNullChecks: true,
    strictFunctionTypes: true,
    noImplicitReturns: true,
    noFallthroughCasesInSwitch: true,
    moduleResolution: "bundler",
    baseUrl: ".",
    paths: {
      "@/*": ["src/*"],
      "@components/*": ["src/components/*"],
      "@utils/*": ["src/utils/*"]
    },
    declaration: true,
    declarationMap: true,
    sourceMap: true,
    removeComments: false,
    esModuleInterop: true,
    allowSyntheticDefaultImports: true,
    forceConsistentCasingInFileNames: true,
    skipLibCheck: true,
    resolveJsonModule: true,
    isolatedModules: true,
    incremental: true,
    tsBuildInfoFile: ".tsbuildinfo"
  },
  include: ["src/**/*"],
  exclude: ["node_modules", "dist", "**/*.test.ts"],
  files: []
};

// Example 2.3: Configuration for Different Targets
// ---------------------------------------------

// Node.js Backend
const nodeConfig = {
  compilerOptions: {
    target: "ES2022",
    module: "commonjs",
    lib: ["ES2022"],
    outDir: "./dist",
    strict: true,
    esModuleInterop: true,
    skipLibCheck: true,
    forceConsistentCasingInFileNames: true,
    declaration: true,
    declarationMap: true,
    sourceMap: true
  }
};

// Browser Frontend
const browserConfig = {
  compilerOptions: {
    target: "ES2020",
    module: "ESNext",
    lib: ["ES2020", "DOM", "DOM.Iterable"],
    outDir: "./dist",
    moduleResolution: "bundler",
    strict: true,
    jsx: "react-jsx",
    noEmit: false,
    isolatedModules: true,
    esModuleInterop: true,
    skipLibCheck: true,
    forceConsistentCasingInFileNames: true
  }
};

// Library/SDK
const libraryConfig = {
  compilerOptions: {
    target: "ES2020",
    module: "ESNext",
    lib: ["ES2020"],
    outDir: "./dist",
    declaration: true,
    declarationMap: true,
    sourceMap: true,
    removeComments: true,
    strict: true,
    noUnusedLocals: true,
    noUnusedParameters: true,
    noImplicitReturns: true,
    noFallthroughCasesInSwitch: true,
    esModuleInterop: true,
    forceConsistentCasingInFileNames: true,
    skipLibCheck: true
  }
};

// ============================================================================
// SECTION 3: IDE INTEGRATION
// ============================================================================

/**
 * Popular IDEs and their TypeScript support:
 * 1. Visual Studio Code (Native)
 * 2. WebStorm/IntelliJ
 * 3. Atom (with packages)
 * 4. Sublime Text (with plugins)
 * 5. Vim (with plugins)
 */

// Example 3.1: VS Code Settings (settings.json)
// --------------------------------------

interface VSCodeSettings {
  "typescript.tsdk": string;
  "typescript.updateImportsOnFileMove.enabled": string;
  "typescript.suggest.autoImports": boolean;
  "typescript.format.enable": boolean;
  "typescript.preferences.importModuleSpecifier": string;
  "typescript.survey.show": boolean;
}

const vscodeSettings: VSCodeSettings = {
  "typescript.tsdk": "node_modules/typescript/lib",
  "typescript.updateImportsOnFileMove.enabled": "always",
  "typescript.suggest.autoImports": true,
  "typescript.format.enable": true,
  "typescript.preferences.importModuleSpecifier": "relative",
  "typescript.survey.show": false
};

// Example 3.2: WebStorm Settings
// ---------------------------

interface WebStormSettings {
  typescriptService: string;
  tsconfigPath: string;
  codeCompletion: string;
  typeInfo: boolean;
  inlineTypeInfo: boolean;
}

// ============================================================================
// SECTION 4: BUILD TOOLS INTEGRATION
// ============================================================================

/**
 * Build tools and their TypeScript integration:
 * 1. Vite - Native .ts/.tsx support
 * 2. Webpack - ts-loader or babel-loader
 * 3. Rollup - @rollup/plugin-typescript
 * 4. Parcel - Native support
 * 5. esbuild - Native TypeScript
 * 6. SWC - Fast TypeScript compiler
 */

// Example 4.1: Vite Configuration
// ---------------------------

interface ViteConfig {
  root: string;
  build: {
    outDir: string;
    rollupOptions: {
      input: string;
    };
  };
  resolve: {
    alias: Record<string, string>;
  };
  server: {
    port: number;
  };
  optimizeDeps: {
    include: string[];
  };
}

const viteConfig: ViteConfig = {
  root: ".",
  build: {
    outDir: "dist",
    rollupOptions: {
      input: "./index.html"
    }
  },
  resolve: {
    alias: {
      "@": "/src"
    }
  },
  server: {
    port: 3000
  },
  optimizeDeps: {
    include: ["typescript"]
  }
};

// Example 4.2: Webpack Configuration
// ------------------------------

interface WebpackConfig {
  entry: string;
  output: {
    path: string;
    filename: string;
  };
  resolve: {
    extensions: string[];
    alias: Record<string, string>;
  };
  module: {
    rules: {
      test: RegExp;
      use: {
        loader: string;
        options: {
          configFile: string;
        };
      }[];
    }[];
  };
  plugins: unknown[];
  devServer: {
    port: number;
    hot: boolean;
  };
}

// Example 4.3: Rollup Configuration
// --------------------------------

interface RollupConfig {
  input: string;
  output: {
    dir: string;
    format: string;
    sourcemap: boolean;
  };
  plugins: unknown[];
  external: string[];
}

// ============================================================================
// SECTION 5: DEBUGGING CONFIGURATION
// ============================================================================

/**
 * Proper debugging setup is crucial for TypeScript development.
 */

// Example 5.1: VS Code Debug Configuration (.vscode/launch.json)
// ----------------------------------------------------------

interface LaunchConfig {
  type: string;
  request: string;
  name: string;
  program: string;
  args: string[];
  cwd: string;
  console: string;
  internalConsoleOptions: string;
  preLaunchTask: string;
}

const debugConfig: LaunchConfig = {
  type: "node",
  request: "launch",
  name: "Debug TypeScript",
  program: "${workspaceFolder}/src/index.ts",
  args: [],
  cwd: "${workspaceFolder}",
  console: "integratedTerminal",
  internalConsoleOptions: "neverOpen",
  preLaunchTask: "tsc: build - tsconfig.json"
};

// Example 5.2: Source Map Configuration
// ---------------------------------

interface SourceMapConfig {
  compilerOptions: {
    sourceMap: boolean;
    declarationMap: boolean;
    inlineSources: boolean;
  };
}

// This enables proper debugging in browser devtools

// ============================================================================
// SECTION 6: COMMON SETUP ISSUES AND SOLUTIONS
// ============================================================================

/**
 * ⚠️ COMMON ISSUE 1: TypeScript version conflicts
 * --------------------------------------------
 * Problem: Different projects need different TS versions
 * Solution: Use local installation and nvm
 */

function setupNvm(): void {
  // Install nvm for version management
  // Use .nvmrc file in project root
  // Run: nvm use
}

/**
 * ⚠️ COMMON ISSUE 2: Module resolution errors
 * -------------------------------------
 * Problem: Cannot resolve modules
 * Solution: Check moduleResolution and paths config
 */

function fixModuleResolution(): void {
  // Ensure moduleResolution matches your package manager
  // "moduleResolution": "node" for CommonJS
  // "moduleResolution": "bundler" for ESM
}

/**
 * ⚠️ COMMON ISSUE 3: Strict mode errors
 * ----------------------------------
 * Problem: Stricter type checking produces many errors
 * Solution: Enable progressively or configure baseline
 */

function configureStrictMode(): void {
  // Start with minimal strictness, increase over time
  // Or use // @ts-strict-override comments
}

/**
 * ⚠️ COMMON ISSUE 4: Type declaration errors
 * -----------------------------------------
 * Problem: Missing @types packages
 * Solution: Install corresponding @types package
 */

function installTypes(): void {
  // npm install @types/node
  // npm install @types/react
  // npm install @types/jest
}

// ============================================================================
// SECTION 7: PERFORMANCE OPTIMIZATION
// ============================================================================

/**
 * TypeScript compilation performance:
 * - Enable incremental compilation
 * - Use project references for large codebases
 * - Enable skipLibCheck
 * - Use buildMode for faster builds
 */

// Example 7.1: Incremental Compilation
// ---------------------------------

const incrementalConfig = {
  compilerOptions: {
    incremental: true,
    tsBuildInfoFile: ".tsbuildinfo"
  }
};

// Example 7.2: Project References
// ------------------------------

interface ProjectReferences {
  references: { path: string }[];
}

// For monorepos, split into projects and use references

// ============================================================================
// SECTION 8: VERIFICATION AND TESTING
// ============================================================================

/**
 * Verify TypeScript setup with:
 * 1. tsc --version (should show installed version)
 * 2. tsc --noEmit (type check without emit)
 * 3. tsc --build (incremental build)
 * 4. VS Code TypeScript service status
 */

function verifySetup(): void {
  console.log("TypeScript Setup Verification:");
  console.log("1. Run: tsc --version");
  console.log("2. Run: tsc --noEmit");
  console.log("3. Check VS Code status bar");
}

verifySetup();

console.log("\n=== Installation and Setup Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/02_Primitive_Types");