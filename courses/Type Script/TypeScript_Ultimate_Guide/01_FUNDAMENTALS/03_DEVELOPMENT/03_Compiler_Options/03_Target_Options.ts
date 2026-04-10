/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 03 Topic: Target Options Purpose: Configuring ECMAScript target version Difficulty: beginner UseCase: browser,server,cross-platform Version: TS1.0+ Compatibility: Node.js, Browsers Performance: Transpilation output Security: Feature compatibility */

/**
 * Target Options - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Configuring ECMAScript target version
 * 💡 WHY: Determine which JavaScript features are allowed and output format
 * 🔧 HOW: target option in tsconfig.json
 */

// ============================================================================
// SECTION 1: WHAT IS TARGET
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// The target option specifies which ECMAScript version to compile to
// This determines what JavaScript features are allowed in output

interface TargetOption {
  purpose: string;
  output: string;
}

const targetInfo: TargetOption = {
  purpose: "ECMAScript version for compiled JavaScript",
  output: "Determines syntax and API features"
};

// ============================================================================
// SECTION 2: AVAILABLE TARGETS
// ============================================================================

// Example 2.1: Target Versions
// -----------------------

interface TargetVersions {
  es3: string;
  es5: string;
  es6: string;
  es2015: string;
  es2016: string;
  es2017: string;
  es2018: string;
  es2019: string;
  es2020: string;
  esnext: string;
}

const targetVersions: TargetVersions = {
  es3: "ECMAScript 3 (IE6+)",
  es5: "ECMAScript 5 (IE9+)",
  es6: "ES2015 (Modern browsers)",
  es2015: "Alias for ES6",
  es2016: "ES2016 - includes exponentiation operator",
  es2017: "ES2017 - async/await, object values",
  es2018: "ES2018 - async iterators, rest/spread",
  es2019: "ES2019 - flat, trimStart/trimEnd",
  es2020: "ES2020 - BigInt, optional chaining",
  esnext: "Latest supported features"
};

// ============================================================================
// SECTION 3: CHOOSING A TARGET
// ============================================================================

// Example 3.1: Browser Support Matrix
// -------------------------------

interface BrowserTarget {
  modern: string;
  mainstream: string;
  legacy: string;
}

const browserTarget: BrowserTarget = {
  modern: "ES2020 or ESNext - Chrome, Firefox, Safari, Edge",
  mainstream: "ES2017 or ES2020 - 95%+ user base",
  legacy: "ES5 or ES6 - IE11 support required"
};

// Example 3.2: Node.js Target
// -----------------------

interface NodeTarget {
  version: string;
  target: string;
}

const nodeTarget: NodeTarget = {
  version: "Node 14+",
  target: "ES2019 or ES2020"
};

// ============================================================================
// SECTION 4: CODE FEATURES BY TARGET
// ============================================================================

// Example 4.1: Feature Examples
// -----------------------

interface FeatureByTarget {
  es5: string[];
  es6: string[];
  es2017: string[];
  es2020: string[];
}

const featureByTarget: FeatureByTarget = {
  es5: [
    "var keyword",
    "function expressions",
    "object literals"
  ],
  es6: [
    "let/const",
    "arrow functions",
    "classes",
    "modules",
    "template literals",
    "destructuring"
  ],
  es2017: [
    "async/await",
    "Object.values/entries",
    "String padding"
  ],
  es2020: [
    "optional chaining",
    "nullish coalescing",
    "BigInt",
    "dynamic import"
  ]
};

// ============================================================================
// SECTION 5: CONFIGURATION
// ============================================================================

// Example 5.1: Basic Target Config
// -----------------------------

interface TargetConfig {
  compilerOptions: {
    target: string;
  };
}

const basicTargetConfig: TargetConfig = {
  compilerOptions: {
    target: "ES2020"
  }
};

// Example 5.2: Full Configuration
// ---------------------------

interface FullTargetConfig {
  compilerOptions: {
    target: string;
    lib: string[];
    module: string;
  };
}

const fullTargetConfig: FullTargetConfig = {
  compilerOptions: {
    target: "ES2020",
    lib: ["ES2020", "DOM"],
    module: "ES2020"
  }
};

// ============================================================================
// SECTION 6: TARGET AND LIB
// ============================================================================

// Example 6.1: Lib Options by Target
// -------------------------------

interface LibByTarget {
  es5: string[];
  es2017: string[];
  es2020: string[];
}

const libByTarget: LibByTarget = {
  es5: ["ES5", "DOM"],
  es2017: ["ES2017", "DOM"],
  es2020: ["ES2020", "DOM", "DOM.Iterable"]
};

// ============================================================================
// SECTION 7: TRANSPILED SYNTAX
// ============================================================================

// Example 7.1: ES6 to ES5
// ---------------------

// ES6 source:
const arrow = () => "Hello";
const [first, ...rest] = [1, 2, 3];

// Compiled to ES5:
var arrow = function() { return "Hello"; };
var first = rest[0], rest = rest.slice(1);

// ============================================================================
// SECTION 8: PERFORMANCE
// ============================================================================

// Example 8.1: Performance Considerations
// -----------------------------------

interface PerformanceConsiderations {
  output: string;
  polyfill: string;
  bundle: string;
}

const perfConsiderations: PerformanceConsiderations = {
  output: "ES5 has larger output due to transpilation",
  polyfill: "Older targets may need core-js polyfills",
  bundle: "Modern targets enable smaller bundles"
};

// ============================================================================
// SECTION 9: COMPATIBILITY
// ============================================================================

// Example 9.1: Browser Compatibility
// -------------------------------

interface BrowserCompatibility {
  es5: string;
  es6: string;
  es2020: string;
}

const browserCompat: BrowserCompatibility = {
  es5: "All browsers including IE9+",
  es6: "Modern browsers (Chrome 61+, FF 60+, Safari 11+)",
  es2020: "Latest browsers only"
};

// ============================================================================
// SECTION 10: MIGRATION
// ============================================================================

// Example 10.1: Target Migration
// -----------------------

interface MigrationPath {
  from: string;
  to: string;
  steps: string[];
}

const migrationPath: MigrationPath = {
  from: "ES5",
  to: "ES2020",
  steps: [
    "Test thoroughly with new target",
    "Check polyfill requirements",
    "Verify browser support",
    "Update lib if needed"
  ]
};

// ============================================================================
// SECTION 11: SECURITY
// ============================================================================

// Example 11.1: Security Considerations
// -------------------------------

interface SecurityConsiderations {
  polyfills: string;
  features: string;
}

const securityConsiderations: SecurityConsiderations = {
  polyfills: "Ensure polyfills don't introduce vulnerabilities",
  features: "Newer features may have security implications"
};

// ============================================================================
// SECTION 12: ALTERNATIVES
// ============================================================================

// Alternative approaches:
// 1. Use esbuild/swc - Faster transpilation
// 2. Use babel - Flexible transpilation
// 3. Use rollup - Module bundler
// 4. Use webpack - Full bundler

// ============================================================================
// SECTION 13: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/03_Compiler_Options/01_Compiler_Flags.ts
// - 03_DEVELOPMENT/03_Compiler_Options/04_Module_Options.ts
// - 03_DEVELOPMENT/03_Compiler_Options/05_Lib_Options.ts

console.log("\n=== Target Options Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/03_Compiler_Options/04_Module_Options");