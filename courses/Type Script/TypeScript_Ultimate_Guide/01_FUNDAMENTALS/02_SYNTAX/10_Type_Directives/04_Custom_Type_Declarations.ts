/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 10
 * Topic: Custom_Type_Declarations
 * Purpose: Creating custom type declarations
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TS 2.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Custom Type Declarations - Comprehensive Guide
 * ========================================
 * 
 * 📚 WHAT: Creating .d.ts declaration files
 * 💡 WHY: Add types for untyped code
 * 🔧 HOW: Declare modules, globals, namespaces
 */

// ============================================================================
// SECTION 1: MODULE DECLARATIONS
// ============================================================================

// Example 1.1: Declare Module
// ----------------------

declare module "custom-package" {
  export function doSomething(): void;
  export class CustomClass { }
}

// ============================================================================
// SECTION 2: GLOBAL DECLARATIONS
// ============================================================================

// Example 2.1: Global Variables
// -----------------------

declare const GLOBAL_VALUE: string;

// Example 2.2: Global Functions
// -------------------------

declare function globalMethod(): void;

// ============================================================================
// SECTION 3: INTERFACE AUGMENTATION
// ============================================================================

// Example 3.1: Extend Existing
// -----------------------

interface Window {
  customProperty: string;
}

// ============================================================================
// SECTION 4: NAMESPACE DECLARATIONS
// ============================================================================

// Example 4.1: Namespace
// --------------------

declare namespace MyNamespace {
  export function method(): void;
}

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 2.0+
// All targets

// ============================================================================
// SECURITY
// ============================================================================

// Only declare types you control
// Verify external declarations

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 05_Ambient_Declarations, 04_Custom_Type_Declarations

console.log("\n=== Custom Type Declarations Complete ===");
console.log("Next: 10_Type_Directives/05_Ambient_Declarations");