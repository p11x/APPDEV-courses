/**
 * Category: FUNDAMENTALS
 * Subcategory: DEVELOPMENT
 * Concept: Setup_and_Installation
 * Purpose: Setting up TypeScript development environment
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Development Environment Setup - Comprehensive Guide
 * ===================================================
 * 
 * 📚 WHAT: Setting up TypeScript development environment
 * 💡 WHY: Proper setup ensures productive development
 * 🔧 HOW: npm, IDEs, build tools, configurations
 */

// ============================================================================
// SECTION 1: NPM PACKAGE MANAGEMENT
// ============================================================================

// Example 1.1: Installing TypeScript
// ------------------------------

interface PackageConfig {
  name: string;
  version: string;
  description: string;
  main: string;
  scripts: Record<string, string>;
  devDependencies: Record<string, string>;
  dependencies: Record<string, string>;
}

// Recommended: Install as dev dependency
// npm install --save-dev typescript

// Example 1.2: Type Definitions
// -----------------------

// Install type definitions for Node.js
// npm install --save-dev @types/node

// Install types for specific libraries
// npm install --save-dev @types/react
// npm install --save-dev @types/jest
// npm install --save-dev @types/express

// Example 1.3: Package.json Scripts
// -----------------------------

const packageJsonScripts: Record<string, string> = {
  "build": "tsc",
  "start": "node dist/index.js",
  "dev": "ts-node src/index.ts",
  "test": "jest",
  "type-check": "tsc --noEmit",
  "lint": "eslint . --ext .ts"
};

// ============================================================================
// SECTION 2: PROJECT INITIALIZATION
// ============================================================================

// Example 2.1: Manual Setup
// ---------------------

// 1. Create project directory
// mkdir my-typescript-project
// cd my-typescript-project

// 2. Initialize npm
// npm init -y

// 3. Install TypeScript
// npm install --save-dev typescript

// 4. Create tsconfig.json

// 5. Create source files

// Example 2.2: tsconfig.json Structure
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
  };
  include: string[];
  exclude: string[];
}

const tsConfig: TsConfig = {
  compilerOptions: {
    target: "ES2020",
    module: "CommonJS",
    lib: ["ES2020"],
    outDir: "./dist",
    rootDir: "./src",
    strict: true,
    esModuleInterop: true,
    skipLibCheck: true
  },
  include: ["src/**/*"],
  exclude: ["node_modules", "dist"]
};

// ============================================================================
// SECTION 3: IDE CONFIGURATION
// ============================================================================

// Example 3.1: VS Code Setup
// ----------------------

// Install TypeScript extension (bundled)
// Enable auto-compilation on save

interface VSCodeSettings {
  "typescript.tsdk": string;
  "typescript.updateImportsOnFileMove.enabled": string;
  "typescript.suggest.autoImports": boolean;
  "editor.formatOnSave": boolean;
  "editor.codeActionsOnSave": string;
}

const vsCodeSettings: VSCodeSettings = {
  "typescript.tsdk": "node_modules/typescript/lib",
  "typescript.updateImportsOnFileMove.enabled": "always",
  "typescript.suggest.autoImports": true,
  "editor.formatOnSave": true,
  "editor.codeActionsOnSave": "source.organizeImports"
};

// Example 3.2: Launch Configuration
// ------------------------------

interface LaunchConfig {
  type: string;
  request: string;
  name: string;
  program: string;
  cwd: string;
  console: string;
  preLaunchTask: string;
}

const launchConfig: LaunchConfig = {
  type: "node",
  request: "launch",
  name: "Debug TypeScript",
  program: "${workspaceFolder}/src/index.ts",
  cwd: "${workspaceFolder}",
  console: "integratedTerminal",
  preLaunchTask: "tsc: build - tsconfig.json"
};

// ============================================================================
// SECTION 4: BUILD TOOLS
// ============================================================================

// Example 4.1: ts-node for Development
// ---------------------------------

// npm install --save-dev ts-node
// Run: ts-node src/index.ts

// Example 4.2: Vite Configuration
// ---------------------------

interface ViteConfig {
  root: string;
  build: { outDir: string };
  server: { port: number };
  resolve: { alias: Record<string, string> };
}

const viteConfig: ViteConfig = {
  root: ".",
  build: { outDir: "dist" },
  server: { port: 3000 },
  resolve: { alias: { "@": "/src" } }
};

// ============================================================================
// SECTION 5: LINTING AND FORMATTING
// ============================================================================

// Example 5.1: ESLint Configuration
// ------------------------------

interface ESLintConfig {
  root: boolean;
  env: Record<string, boolean>;
  extends: string[];
  parser: string;
  parserOptions: Record<string, unknown>;
  rules: Record<string, string>;
}

const eslintConfig: ESLintConfig = {
  root: true,
  env: { node: true, es2021: true },
  extends: [
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended"
  ],
  parser: "@typescript-eslint/parser",
  parserOptions: { ecmaVersion: "latest", sourceType: "module" },
  rules: {
    "@typescript-eslint/no-unused-vars": "error",
    "@typescript-eslint/explicit-function-return-type": "warn"
  }
};

// Example 5.2: Prettier Configuration
// -------------------------------

interface PrettierConfig {
  semi: boolean;
  singleQuote: boolean;
  tabWidth: number;
  trailingComma: string;
  printWidth: number;
}

const prettierConfig: PrettierConfig = {
  semi: true,
  singleQuote: true,
  tabWidth: 2,
  trailingComma: "es5",
  printWidth: 100
};

console.log("\n=== Development Environment Setup Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/02_Compiler_Configuration");