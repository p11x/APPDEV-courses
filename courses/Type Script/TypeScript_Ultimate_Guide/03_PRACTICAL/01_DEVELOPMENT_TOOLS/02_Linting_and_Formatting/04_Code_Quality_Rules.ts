/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Linting_and_Formatting
 * Purpose: Code quality rules for TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * Code Quality Rules - Comprehensive Guide
 * =========================================
 * 
 * 📚 WHAT: ESLint rules for code quality
 * 💡 WHERE: Enforcing code standards
 * 🔧 HOW: Best practice rules, type safety
 */

// ============================================================================
// SECTION 1: TYPE SAFETY RULES
// ============================================================================

// Example 1.1: Type Safety Rules
// ---------------------------------

const typeSafetyRules = {
  "@typescript-eslint/no-explicit-any": "warn",
  "@typescript-eslint/no-inferrable-types": "off",
  "@typescript-eslint/explicit-function-return-type": [
    "warn",
    {
      allowExpressions: true,
      allowTypedFunctionExpressions: true
    }
  ],
  "@typescript-eslint/explicit-module-boundary-types": "off"
};

// ============================================================================
// SECTION 2: CODE STYLE RULES
// ============================================================================

// Example 2.1: Code Style Rules
// ---------------------------------

const codeStyleRules = {
  "prefer-const": "error",
  "no-var": "error",
  "object-shorthand": ["error", "always"],
  "prettier/prettier": ["error", { "singleQuote": true }]
};

// ============================================================================
// SECTION 3: BEST PRACTICE RULES
// ============================================================================

// Example 3.1: Best Practice Rules
// ---------------------------------

const bestPracticeRules = {
  "eqeqeq": ["error", "always"],
  "no-console": "warn",
  "no-debugger": "error",
  "no-alert": "error"
};

console.log("\n=== Code Quality Rules Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/02_Linting_and_Formatting/05_ESLint_Best_Practices.ts");