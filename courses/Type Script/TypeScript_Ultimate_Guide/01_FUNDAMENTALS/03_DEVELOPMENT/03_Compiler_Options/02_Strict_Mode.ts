/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 03 Topic: Strict Mode Purpose: Enabling and configuring strict type checking Difficulty: intermediate UseCase: production,enterprise,code-quality Version: TS2.0+ Compatibility: Node.js, Browsers Performance: Type checking time Security: Type safety */

/**
 * Strict Mode - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Enabling and configuring strict type checking
 * 💡 WHY: Catch more errors at compile time, improve code quality
 * 🔧 HOW: strict flag, individual strict options, gradual adoption
 */

// ============================================================================
// SECTION 1: WHAT IS STRICT MODE
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// Strict mode enables all strict type checking options, catching more errors
// at compile time and preventing runtime issues

interface StrictMode {
  purpose: string;
  benefits: string[];
}

const strictModeInfo: StrictMode = {
  purpose: "Enable comprehensive type checking",
  benefits: [
    "Catch null/undefined errors early",
    "Prevent implicit any",
    "Better type inference",
    "Improved code quality"
  ]
};

// ============================================================================
// SECTION 2: ENABLING STRICT MODE
// ============================================================================

// Example 2.1: Quick Enable
// ---------------------

interface QuickStrictConfig {
  compilerOptions: {
    strict: boolean;
  };
}

const quickStrictConfig: QuickStrictConfig = {
  compilerOptions: {
    strict: true
  }
};

// Example 2.2: Gradual Adoption
// -----------------------

interface GradualConfig {
  compilerOptions: {
    strict: boolean;
    noImplicitAny: boolean;
    strictNullChecks: boolean;
    strictPropertyInitialization: boolean;
  };
}

const gradualConfig: GradualConfig = {
  compilerOptions: {
    strict: false,  // Enable individually first
    noImplicitAny: true,
    strictNullChecks: true,
    strictPropertyInitialization: false
  }
};

// ============================================================================
// SECTION 3: NO IMPLICIT ANY
// ============================================================================

// Example 3.1: What It Does
// ---------------------

// Error: Parameter 'name' implicitly has an 'any' type
function greet(name) {  // noImplicitAny: true - ERROR
  return `Hello, ${name}`;
}

// Fix: Add type annotation
function greetFixed(name: string): string {
  return `Hello, ${name}`;
}

// Example 3.2: Configuration
// ---------------------

interface NoImplicitAnyConfig {
  noImplicitAny: boolean;
}

const noImplicitAnyConfig: NoImplicitAnyConfig = {
  noImplicitAny: true
};

// ============================================================================
// SECTION 4: STRICT NULL CHECKS
// ============================================================================

// Example 4.1: What It Does
// ---------------------

interface User {
  name: string;
  email?: string;  // Optional - may be undefined
}

// Error: Object may be null
function getEmail(user: User): string {
  return user.email.toLowerCase();  // ERROR - email may be undefined
}

// Fix: Use optional chaining
function getEmailFixed(user: User): string {
  return user.email?.toLowerCase() ?? "no email";
}

// Example 4.2: Configuration
// ---------------------

interface StrictNullChecksConfig {
  strictNullChecks: boolean;
}

const strictNullChecksConfig: StrictNullChecksConfig = {
  strictNullChecks: true
};

// ============================================================================
// SECTION 5: STRICT FUNCTION TYPES
// ============================================================================

// Example 5.1: Function Parameter Bivariance
// ------------------------------------

interface Handler {
  (value: string): void;
}

interface SpecialHandler extends Handler {
  (value: string | number): void;
}

// With strictFunctionTypes: true, this is not allowed
// because function parameters are contravariant

// ============================================================================
// SECTION 6: STRICT PROPERTY INITIALIZATION
// ============================================================================

// Example 6.1: Uninitialized Properties
// ---------------------------------

class UserProfile {
  name: string;         // Error: Property not initialized
  age?: number;        // OK - optional
}

// Fix: Initialize in constructor
class UserProfileFixed {
  name: string;
  age?: number;

  constructor(name: string) {
    this.name = name;
  }
}

// Or use definite assignment
class UserProfileFixed2 {
  name!: string;  // Definite assignment assertion
}

// ============================================================================
// SECTION 7: STRICT BIND CALL APPLY
// ============================================================================

// Example 7.1: Function Binding
// -----------------------

function greet(message: string): string {
  return message;
}

const greetBound: (msg: string) => string = greet.bind(null);
const callResult = greet.call(null, "Hello");

// With strictBindCallApply: true, these are properly typed

// ============================================================================
// SECTION 8: NO IMPLICIT THIS
// ============================================================================

// Example 8.1: Implicit Any This
// --------------------------

// Error: Implicit 'any' type for 'this' parameter
function logMessage() {
  console.log(this.message);  // ERROR
}

// Fix: Explicit this type
function logMessageFixed(this: { message: string }) {
  console.log(this.message);
}

// ============================================================================
// SECTION 9: ALWAYS STRICT
// ============================================================================

// Example 9.1: ES Strict Mode
// -----------------------

// Always strict ensures "use strict" is emitted in all modules
// and parses in strict mode

interface AlwaysStrictConfig {
  alwaysStrict: boolean;
}

const alwaysStrictConfig: AlwaysStrictConfig = {
  alwaysStrict: true
};

// ============================================================================
// SECTION 10: MIGRATION STRATEGY
// ============================================================================

// Example 10.1: Migration Steps
// -------------------------

interface MigrationSteps {
  step1: string;
  step2: string;
  step3: string;
  step4: string;
}

const migrationSteps: MigrationSteps = {
  step1: "Enable noImplicitAny",
  step2: "Fix any errors",
  step3: "Enable strictNullChecks",
  step4: "Enable remaining strict options"
};

// Example 10.2: Package.json Scripts
// -----------------------------

interface MigrationScripts {
  scripts: Record<string, string>;
}

const migrationScripts: MigrationScripts = {
  scripts: {
    "type-check": "tsc --noEmit --strict",
    "type-check:loose": "tsc --noEmit --noImplicitAny false",
    "type-check:strict": "tsc --noEmit --strict"
  }
};

// ============================================================================
// SECTION 11: PERFORMANCE
// ============================================================================

// Example 11.1: Performance Impact
// -----------------------------

interface PerformanceImpact {
  strictMode: string;
  suggestion: string;
}

const perfImpact: PerformanceImpact = {
  strictMode: "Slightly slower type checking but catches more errors",
  suggestion: "Use incremental builds to speed up"
};

// ============================================================================
// SECTION 12: COMPATIBILITY
// ============================================================================

// Example 12.1: Version Requirements
// -----------------------------

interface VersionSupport {
  minimum: string;
  recommended: string;
}

const versionSupport: VersionSupport = {
  minimum: "2.0",
  recommended: "4.5 or higher"
};

// ============================================================================
// SECTION 13: SECURITY
// ============================================================================

// Example 13.1: Security Benefits
// ---------------------------

interface SecurityBenefits {
  typeSafety: string;
  nullChecks: string;
  anyPrevention: string;
}

const securityBenefits: SecurityBenefits = {
  typeSafety: "Prevents type confusion attacks",
  nullChecks: "Catches undefined errors early",
  anyPrevention: "Reduces implicit any vulnerabilities"
};

// ============================================================================
// SECTION 14: TESTING
// ============================================================================

// Example 14.1: Testing Strict Mode
// -----------------------------

interface TestConfig {
  strict: boolean;
  noImplicitAny: boolean;
}

const testConfig: TestConfig = {
  strict: true,
  noImplicitAny: true
};

// ============================================================================
// SECTION 15: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/03_Compiler_Options/01_Compiler_Flags.ts
// - 01_FUNDAMENTALS/01_TYPES/01_Basic_Types.ts
// - 02_ADVANCED/01_PATTERNS/01_Type_Safety_Patterns.ts

console.log("\n=== Strict Mode Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/03_Compiler_Options/03_Target_Options");