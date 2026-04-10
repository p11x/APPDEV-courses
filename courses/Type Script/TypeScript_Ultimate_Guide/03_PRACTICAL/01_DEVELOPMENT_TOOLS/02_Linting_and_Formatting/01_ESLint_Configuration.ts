/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Linting_and_Formatting
 * Purpose: ESLint configuration for TypeScript projects
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * ESLint Configuration - Comprehensive Guide
 * =======================================
 * 
 * 📚 WHAT: Setting up ESLint for TypeScript code quality
 * 💡 WHY: Catches bugs, enforces style, improves code quality
 * 🔧 HOW: Parser, plugins, rules, configurations
 */

// ============================================================================
// SECTION 1: ESLINT CONFIG TYPES
// ============================================================================

// Example 1.1: ESLint Configuration
// -----------------------

interface ESLintConfig {
  root: boolean;
  env: Record<string, boolean>;
  parser: string;
  parserOptions: ParserOptions;
  extends: string[];
  plugins: string[];
  rules: Record<string, unknown>;
  overrides?: Override[];
}

interface ParserOptions {
  ecmaVersion: string | number;
  sourceType: string;
  ecmaFeatures: Record<string, boolean>;
}

interface Override {
  files: string[];
  rules: Record<string, unknown>;
}

const eslintConfig: ESLintConfig = {
  root: true,
  env: {
    browser: true,
    es2021: true,
    node: true
  },
  parser: "@typescript-eslint/parser",
  parserOptions: {
    ecmaVersion: "latest",
    sourceType: "module",
    ecmaFeatures: {
      jsx: true
    }
  },
  extends: [
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:react/recommended",
    "prettier"
  ],
  plugins: [
    "@typescript-eslint",
    "react",
    "react-hooks"
  ],
  rules: {
    "@typescript-eslint/no-unused-vars": ["error", { argsIgnorePattern: "^_" }],
    "@typescript-eslint/explicit-function-return-type": "off",
    "@typescript-eslint/no-explicit-any": "warn",
    "react/react-in-jsx-scope": "off",
    "react/prop-types": "off"
  },
  overrides: [
    {
      files: ["*.ts", "*.tsx"],
      rules: {
        "@typescript-eslint/explicit-function-return-type": ["warn"]
      }
    }
  ]
};

// ============================================================================
// SECTION 2: TYPESCRIPT-SPECIFIC RULES
// ============================================================================

// Example 2.1: TypeScript ESLint Rules
// --------------------------------

const typescriptRules = {
  "@typescript-eslint/no-unused-vars": [
    "error",
    {
      argsIgnorePattern: "^_",
      varsIgnorePattern: "^_",
      caughtErrorsIgnorePattern: "^_"
    }
  ],
  "@typescript-eslint/explicit-function-return-type": [
    "warn",
    {
      allowExpressions: true,
      allowTypedFunctionExpressions: true,
      allowHigherOrderFunctions: true
    }
  ],
  "@typescript-eslint/no-explicit-any": "warn",
  "@typescript-eslint/no-inferrable-types": "off",
  "@typescript-eslint/explicit-module-boundary-types": "off",
  "@typescript-eslint/ban-ts-comment": [
    "error",
    {
      "ts-expect-error": "allow-with-description",
      "ts-ignore": true,
      "ts-nocheck": true,
      "ts-check": false
    }
  ]
};

// ============================================================================
// SECTION 3: REACT RULES
// ============================================================================

// Example 3.1: React ESLint Rules
// -----------------------

const reactRules = {
  "react/prop-types": "off",
  "react-hooks/rules-of-hooks": "error",
  "react-hooks/exhaustive-deps": "warn",
  "react/jsx-uses-react": "off",
  "react/react-in-jsx-scope": "off"
};

console.log("\n=== ESLint Configuration Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/02_Linting_and_Formatting/02_Prettier_Setup.ts");