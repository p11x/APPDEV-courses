/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Build_Automation
 * Purpose: Rollup configuration for TypeScript projects
 * Difficulty: intermediate
 * UseCase: web, libraries
 */

/**
 * Rollup Configuration - Comprehensive Guide
 * ======================================
 * 
 * 📚 WHAT: Setting up Rollup with TypeScript for library builds
 * 💡 WHY: Excellent for library and bundle optimization
 * 🔧 HOW: Plugins, input/output, treeshaking
 */

// ============================================================================
// SECTION 1: BASIC ROLLUP CONFIG
// ============================================================================

// Example 1.1: Basic Rollup Configuration
// ---------------------------------

interface RollupConfig {
  input: string;
  output: {
    file: string;
    format: string;
    sourcemap: boolean;
    name?: string;
  };
  plugins: RollupPlugin[];
}

interface RollupPlugin {
  name: string;
  build?: () => void;
  generateBundle?: (options: unknown, bundle: unknown) => void;
}

const rollupConfig: RollupConfig = {
  input: "src/index.ts",
  output: {
    file: "dist/bundle.js",
    format: "cjs",
    sourcemap: true
  },
  plugins: []
};

// ============================================================================
// SECTION 2: TYPESCRIPT PLUGIN
// ============================================================================

// Example 2.1: TypeScript Plugin Options
// ---------------------------------

interface TypeScriptPluginOptions {
  typescript: unknown;
  include?: string[];
  exclude?: string[];
}

const typescriptPlugin: RollupPlugin = {
  name: "typescript",
  build: () => {
    console.log("Building TypeScript...");
  }
};

// ============================================================================
// SECTION 3: EXTERNAL MODULES
// ============================================================================

// Example 3.1: Marking Externals
// ---------------------------------

interface ExternalOption {
  (id: string): boolean | string | undefined;
}

const externalModules: ExternalOption = (id) => {
  if (id.startsWith("node:")) return false;
  return !id.startsWith(".") && !id.startsWith("/");
};

console.log("\n=== Rollup Configuration Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/01_Build_Automation/04_Babel_Integration.ts");