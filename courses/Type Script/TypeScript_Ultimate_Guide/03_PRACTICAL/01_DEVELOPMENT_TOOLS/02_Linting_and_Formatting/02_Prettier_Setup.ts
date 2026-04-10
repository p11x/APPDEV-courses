/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Linting_and_Formatting
 * Purpose: Prettier configuration for TypeScript projects
 * Difficulty: beginner
 * UseCase: web, backend
 */

/**
 * Prettier Setup - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Setting up Prettier for code formatting
 * 💡 WHY: Consistent code style across the project
 * 🔧 HOW: Configuration, plugins, editor integration
 */

// ============================================================================
// SECTION 1: PRETTIER CONFIG TYPES
// ============================================================================

// Example 1.1: Prettier Configuration
// -----------------------

interface PrettierConfig {
  semi: boolean;
  singleQuote: boolean;
  tabWidth: number;
  trailingComma: string;
  printWidth: number;
  bracketSpacing: boolean;
  arrowParens: string;
  endOfLine: string;
  jsxSingleQuote: boolean;
  proseWrap: string;
}

const prettierConfig: PrettierConfig = {
  semi: true,
  singleQuote: true,
  tabWidth: 2,
  trailingComma: "es5",
  printWidth: 100,
  bracketSpacing: true,
  arrowParens: "always",
  endOfLine: "lf",
  jsxSingleQuote: false,
  proseWrap: "preserve"
};

// ============================================================================
// SECTION 2: EDITORCONFIG
// ============================================================================

// Example 2.1: .editorconfig for Prettier
// ---------------------------

const editorConfig = `
root = true

[*]
indent_style = space
indent_size = 2
end_of_line = lf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

[*.{js,ts,tsx}]
indent_size = 2

[*.{json,yml,yaml}]
indent_size = 2

[*.md]
trim_trailing_whitespace = false
`;

// ============================================================================
// SECTION 3: VS CODE INTEGRATION
// ============================================================================

// Example 3.1: VS Code Settings
// -----------------------

const vsCodeSettings = {
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "editor.formatOnSave": true,
  "editor.codeActionsOnSave": {
    "source.fixAll.eslint": true,
    "source.organizeImports": true
  },
  "[typescript]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  },
  "[typescriptreact]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  }
};

console.log("\n=== Prettier Setup Complete ===");
console.log("Next: PRACTICAL/DATA_PROCESSING/01_Data_Structures/02_Algorithms_Implementation.ts");