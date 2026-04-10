/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Build_Automation
 * Purpose: Babel integration for TypeScript
 * Difficulty: intermediate
 * UseCase: web
 */

/**
 * Babel Integration - Comprehensive Guide
 * ======================================
 * 
 * 📚 WHAT: Using Babel with TypeScript for transpilation
 * 💡 WHY: Fast transpilation with modern JS features
 * 🔧 HOW: Presets, plugins, configurations
 */

// ============================================================================
// SECTION 1: BABEL CONFIG TYPES
// ============================================================================

// Example 1.1: Babel Configuration
// ---------------------------------

interface BabelConfig {
  presets: BabelPreset[];
  plugins: BabelPlugin[];
}

interface BabelPreset {
  name: string;
  options?: Record<string, unknown>;
}

interface BabelPlugin {
  name: string;
  options?: Record<string, unknown>;
}

const babelConfig: BabelConfig = {
  presets: [
    ["@babel/preset-env", { targets: { node: "current" } }],
    ["@babel/preset-typescript", { allExtensions: true }],
    ["@babel/preset-react", { runtime: "automatic" }]
  ],
  plugins: []
};

// ============================================================================
// SECTION 2: TYPESCRIPT PRESET
// ============================================================================

// Example 2.1: TypeScript Configuration
// ---------------------------------

interface TypeScriptPresetOptions {
  allExtensions: boolean;
  allowNamespaces: boolean;
  jsx: string;
}

const typescriptPreset: BabelPreset = {
  name: "@babel/preset-typescript",
  options: {
    allExtensions: true,
    allowNamespaces: true,
    jsx: "automatic"
  } as TypeScriptPresetOptions
};

// ============================================================================
// SECTION 3: CUSTOM PLUGINS
// ============================================================================

// Example 3.1: Custom Transformer
// ---------------------------------

function customPlugin() {
  return {
    name: "custom-transformer",
    visitor: {
      Identifier(path: unknown) {
        // Transform identifier
      }
    }
  };
}

console.log("\n=== Babel Integration Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/01_Build_Automation/06_Watch_Mode_and_Incremental_Compilation.ts");