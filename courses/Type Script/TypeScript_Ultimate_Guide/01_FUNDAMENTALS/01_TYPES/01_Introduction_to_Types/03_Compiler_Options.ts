/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Introduction_to_Types
 * Purpose: Deep dive into TypeScript compiler options and configurations
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Compiler Options - Complete Reference
 * ======================================
 * 
 * 📚 WHAT: Understanding all TypeScript compiler options
 * 💡 WHY: Proper configuration ensures type safety and optimal output
 * 🔧 HOW: tsconfig.json options, command line flags, API options
 */

// ============================================================================
// SECTION 1: COMPILER OPTIONS CATEGORIES
// ============================================================================

/**
 * TypeScript compiler options are grouped into categories:
 * 1. Language - JavaScript version and syntax
 * 2. Modules - Module system and resolution
 * 3. Emit - Output generation
 * 4. Type Checking - Strict type enforcement
 * 5. Output Formatting - Code generation
 * 6. Completeness - Strict checks
 */

// ============================================================================
// SECTION 2: LANGUAGE AND COMPATIBILITY OPTIONS
// ============================================================================

/**
 * Example 2.1: Target and Language Version
 * ----------------------------------------
 * 
 * target: Specifies ECMAScript version for output
 * Values: "ES3", "ES5", "ES6", "ES2015", "ES2017", "ES2020", "ES2022", "ESNext"
 */

interface TargetOptions {
  compilerOptions: {
    target: string;  // Output JavaScript version
    lib: string[];  // Included type definitions
    allowJs: boolean;  // Allow .js files
    checkJs: boolean;  // Type check .js files
  };
}

const targetConfig: TargetOptions = {
  compilerOptions: {
    target: "ES2022",
    lib: ["ES2022", "DOM", "DOM.Iterable"],
    allowJs: true,
    checkJs: false
  }
};

// Compile example
function demonstrateTarget(): void {
  // ES2022 features like top-level await are available when targeting ES2022
  // The compiler transforms code to match target
}

// Example 2.2: Module System Options
// ---------------------------------

interface ModuleOptions {
  compilerOptions: {
    module: string;
    moduleResolution: string;
    allowSyntheticDefaultImports: boolean;
    esModuleInterop: boolean;
    resolveJsonModule: boolean;
  };
}

const moduleConfig: ModuleOptions = {
  compilerOptions: {
    module: "ESNext",        // Module format
    moduleResolution: "bundler", // Resolution strategy
    allowSyntheticDefaultImports: true,
    esModuleInterop: true,
    resolveJsonModule: true
  }
};

// Module values: "none", "commonjs", "amd", "umd", "system", "es6", "es2022", "esnext"
// ModuleResolution values: "node", "classic", "nodenext", "bundler"

// ============================================================================
// SECTION 3: STRICT TYPE CHECKING OPTIONS
// ============================================================================

/**
 * Example 3.1: Full Strict Mode
 * ---------------------------
 * 
 * strict: Enables all strict type checking options
 * Recommended for production code
 */

interface StrictOptions {
  compilerOptions: {
    strict: boolean;
    noImplicitAny: boolean;
    strictNullChecks: boolean;
    strictFunctionTypes: boolean;
    strictBindCallApply: boolean;
    strictPropertyInitialization: boolean;
    noImplicitThis: boolean;
    useUnknownInCatchVariables: boolean;
  };
}

const strictConfig: StrictOptions = {
  compilerOptions: {
    strict: true,
    noImplicitAny: true,           // Error on implicit any
    strictNullChecks: true,        // Null/undefined must be handled
    strictFunctionTypes: true,     // Function parameter bivariance check
    strictBindCallApply: true,    // Check bind/call/apply
    strictPropertyInitialization: true, // Class properties must be initialized
    noImplicitThis: true,          // Error on 'this' with implicit type
    useUnknownInCatchVariables: true  // catch variables are 'unknown' not 'any'
  }
};

// Example 3.2: Individual Strict Options Detail
// --------------------------------------------

function demonstrateStrictOptions(): void {
  // noImplicitAny: requires explicit type for untyped parameters
  // function process(data) { } // Error without type
  
  // strictNullChecks: requires null handling
  // const name: string = null; // Error
  
  // strictFunctionTypes: function params are contravariant
  // class Animal { }
  // class Dog extends Animal { }
  // type AnimalFn = (animal: Animal) => void;
  // type DogFn = (dog: Dog) => void;
  // const animalFn: AnimalFn = (dog: Dog) => { }; // Error in strict mode
  
  // strictPropertyInitialization
  // class User { name: string; } // Error: not initialized
}

// ============================================================================
// SECTION 4: OUTPUT AND EMIT OPTIONS
// ============================================================================

/**
 * Example 4.1: Emit Options
 * -----------------------
 */

interface EmitOptions {
  compilerOptions: {
    outDir: string;           // Output directory
    rootDir: string;          // Root directory of input files
    declaration: boolean;     // Generate .d.ts files
    declarationMap: boolean;  // Generate source maps for declarations
    sourceMap: boolean;       // Generate .map files
    inlineSourceMap: boolean; // Inline source maps
    removeComments: boolean;  // Remove comments
    noEmit: boolean;          // Don't emit output (type-check only)
    emitDeclarationOnly: boolean; // Only emit declarations
    declarationDir: string;   // Output directory for declarations
    preserveSymlinks: boolean; // Preserve symlinks
  };
}

const emitConfig: EmitOptions = {
  compilerOptions: {
    outDir: "./dist",
    rootDir: "./src",
    declaration: true,
    declarationMap: true,
    sourceMap: true,
    inlineSourceMap: false,
    removeComments: false,
    noEmit: false,
    emitDeclarationOnly: false,
    declarationDir: "./dist/types",
    preserveSymlinks: false
  }
};

// Example 4.2: Incremental Build Options
// ------------------------------------

interface IncrementalOptions {
  compilerOptions: {
    incremental: boolean;
    tsBuildInfoFile: string;
  };
}

const incrementalConfig: IncrementalOptions = {
  compilerOptions: {
    incremental: true,
    tsBuildInfoFile: ".tsbuildinfo"
  }
};

// ============================================================================
// SECTION 5: MODULE RESOLUTION OPTIONS
// ============================================================================

/**
 * Example 5.1: Module Resolution Configuration
 * -------------------------------------------
 */

interface ModuleResolutionOptions {
  compilerOptions: {
    baseUrl: string;           // Base directory for non-relative imports
    paths: Record<string, string[]>;  // Path mappings
    rootDirs: string[];         // Multiple root directories
    typeRoots: string[];        // Type definition directories
    types: string[];            // Included type packages
  };
}

const resolutionConfig: ModuleResolutionOptions = {
  compilerOptions: {
    baseUrl: ".",
    paths: {
      "@/*": ["src/*"],
      "@components/*": ["src/components/*"],
      "@utils/*": ["src/utils/*"],
      "@types/*": ["src/types/*"],
      "assets/*": ["public/assets/*"]
    },
    rootDirs: ["src", "scripts"],
    typeRoots: ["./node_modules/@types", "./src/types"],
    types: ["node", "react"]
  }
};

// Example 5.2: Path Mapping Examples
// ---------------------------------

// When importing from src/utils/logger.ts
// import { Logger } from "@utils/logger"; // Maps to src/utils/logger.ts

// ============================================================================
// SECTION 6: JSX OPTIONS
// ============================================================================

/**
 * Example 6.1: JSX Configuration
 * ---------------------------
 */

interface JsxOptions {
  compilerOptions: {
    jsx: string;           // JSX output mode
    jsxFactory: string;    // JSX factory function
    jsxFragmentFactory: string; // JSX fragment
    jsxImportSource: string;   // Source for JSX pragma
  };
}

const jsxConfig: JsxOptions = {
  compilerOptions: {
    jsx: "react-jsx",      // "preserve", "react", "react-native"
    jsxFactory: "React.createElement",
    jsxFragmentFactory: "React.Fragment",
    jsxImportSource: "react"
  }
};

// jsx values:
// "react" - Transforms JSX to React.createElement
// "react-jsx" - Transforms to _jsx (automatic runtime)
// "react-native" - Preserves JSX structure
// "preserve" - Keep JSX for other transformers

// ============================================================================
// SECTION 7: LIBRARY AND ENVIRONMENT OPTIONS
// ============================================================================

/**
 * Example 7.1: Library Configuration
 * -------------------------------
 */

interface LibraryOptions {
  compilerOptions: {
    lib: string[];        // Included type definitions
    types: string[];      // Explicitly included type packages
    typeRoots: string[];  // Type root directories
  };
}

const libraryConfig: LibraryOptions = {
  compilerOptions: {
    lib: [
      "ES2022",
      "DOM",
      "DOM.Iterable",
      "ES2022.BigInt",
      "ES2022.String"
    ],
    types: ["node"],
    typeRoots: ["./node_modules/@types", "./src/@types"]
  }
};

// Common lib values:
// "ES5", "ES2015", "ES2020", "ES2022"
// "DOM", "DOM.Iterable"
// "WebWorker"
// "ES2022.BigInt", "ES2022.Array"

// ============================================================================
// SECTION 8: FILE INCLUSION AND EXCLUSION
// ============================================================================

/**
 * Example 8.1: File Configuration
 * ---------------------------
 */

interface FileConfig {
  include: string[];      // Files to include
  exclude: string[];      // Files to exclude
  files: string[];        // Explicit file list
}

const fileConfig: FileConfig = {
  include: [
    "src/**/*.ts",
    "src/**/*.tsx",
    "src/**/*.d.ts"
  ],
  exclude: [
    "node_modules",
    "dist",
    "build",
    "**/*.test.ts",
    "**/*.spec.ts",
    "**/__tests__/**",
    "**/__mocks__/**"
  ],
  files: []
};

// ============================================================================
// SECTION 9: ADVANCED OPTIONS
// ============================================================================

/**
 * Example 9.1: Advanced Compiler Options
 * -------------------------------------
 */

interface AdvancedOptions {
  compilerOptions: {
    // Completeness
    noImplicitReturns: boolean;  // Check all code paths return
    noFallthroughCasesInSwitch: boolean;
    noUncheckedIndexedAccess: boolean;
    noImplicitOverride: boolean;
    noPropertyAccessFromIndexSignature: boolean;
    
    // Performance
    skipLibCheck: boolean;
    isolateModules: boolean;
    allowImportingTsExtensions: boolean;
    verbatimModuleSyntax: boolean;
    
    // Output
    emitDecoratorMetadata: boolean;
    experimentalDecorators: boolean;
    emitDecoratorMetadata: boolean;
    
    // Compatibility
    forceConsistentCasingInFileNames: boolean;
    noEmitOnError: boolean;
    disableReferencedProjectLoad: boolean;
  };
}

const advancedConfig: AdvancedOptions = {
  compilerOptions: {
    noImplicitReturns: true,
    noFallthroughCasesInSwitch: true,
    noUncheckedIndexedAccess: false,
    noImplicitOverride: true,
    noPropertyAccessFromIndexSignature: false,
    skipLibCheck: true,
    isolateModules: true,
    allowImportingTsExtensions: false,
    verbatimModuleSyntax: false,
    experimentalDecorators: true,
    emitDecoratorMetadata: false,
    forceConsistentCasingInFileNames: true,
    noEmitOnError: false,
    disableReferencedProjectLoad: false
  }
};

// ============================================================================
// SECTION 10: COMMAND LINE OPTIONS
// ============================================================================

/**
 * Example 10.1: Common CLI Commands
 * -------------------------------
 */

interface CliCommands {
  [command: string]: string;
}

const cliCommands: CliCommands = {
  compile: "tsc",
  watch: "tsc --watch",
  typeCheck: "tsc --noEmit",
  build: "tsc --build",
  clean: "tsc --build --clean",
  showConfig: "tsc --showConfig",
  help: "tsc --help"
};

// Common flags:
// --watch, --help, --version
// --noEmit, --emitDeclarationOnly
// --strict, --noImplicitAny
// --outDir, --rootDir
// --module, --target
// --declaration, --declarationMap
// --project, --build

// ============================================================================
// SECTION 11: ENVIRONMENT-SPECIFIC CONFIGS
// ============================================================================

/**
 * Example 11.1: Development vs Production
 * --------------------------------------
 */

interface EnvironmentConfig {
  compilerOptions: {
    // Development
    sourceMap: boolean;
    noUnusedLocals: boolean;
    noUnusedParameters: boolean;
    noImplicitReturns: boolean;
    noFallthroughCasesInSwitch: boolean;
    
    // Production
    removeComments: boolean;
    minify: boolean;
    stripInternal: boolean;
  };
}

// Development config
const devConfig: EnvironmentConfig = {
  compilerOptions: {
    sourceMap: true,
    noUnusedLocals: false,
    noUnusedParameters: false,
    noImplicitReturns: false,
    noFallthroughCasesInSwitch: false,
    removeComments: false,
    minify: false,
    stripInternal: false
  }
};

// Production config
const prodConfig: EnvironmentConfig = {
  compilerOptions: {
    sourceMap: false,
    noUnusedLocals: true,
    noUnusedParameters: true,
    noImplicitReturns: true,
    noFallthroughCasesInSwitch: true,
    removeComments: true,
    minify: true,
    stripInternal: true
  }
};

// ============================================================================
// SECTION 12: COMMON ISSUES AND SOLUTIONS
// ============================================================================

/**
 * ⚠️ COMMON ISSUE 1: Type errors from dependencies
 * -----------------------------------------------
 * Solution: skipLibCheck: true
 */

function fixDependencyErrors(): void {
  // Add to tsconfig.json:
  // "compilerOptions": {
  //   "skipLibCheck": true
  // }
}

/**
 * ⚠️ COMMON ISSUE 2: Module resolution failures
 * ------------------------------------------
 * Solution: Match moduleResolution with your environment
 */

function fixModuleResolution(): void {
  // Node.js: moduleResolution: "node"
  // Vite/Webpack: moduleResolution: "bundler"
  // Native ESM: moduleResolution: "nodenext" or "node16"
}

/**
 * ⚠️ COMMON ISSUE 3: Slow compilation
 * -------------------------------
 * Solution: Enable incremental and use project references
 */

function optimizeCompilation(): void {
  // 1. Enable incremental builds
  // 2. Use skipLibCheck
  // 3. Use isolatedModules
  // 4. Consider splitting into projects
}

console.log("\n=== Compiler Options Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/02_Primitive_Types");