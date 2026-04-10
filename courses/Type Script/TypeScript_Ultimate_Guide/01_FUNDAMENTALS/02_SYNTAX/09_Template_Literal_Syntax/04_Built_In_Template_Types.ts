/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 09
 * Topic: Built_In_Template_Types
 * Purpose: Using built-in template literal types
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TS 4.1+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Built In Template Types - Comprehensive Guide
 * =========================================
 * 
 * 📚 WHAT: Predefined template literal utility types
 * 💡 WHY: Common string transformations
 * 🔧 HOW: Uppercase, Lowercase, Capitalize, Uncapitalize
 */

// ============================================================================
// SECTION 1: CASE TYPES
// ============================================================================

// Example 1.1: Uppercase Type
// ----------------------

type T1 = Uppercase<"hello">;  // "HELLO"
type T2 = Uppercase<"foo">;   // "FOO"

// Example 1.2: Lowercase Type
// ----------------------

type T3 = Lowercase<"HELLO">;  // "hello"
type T4 = Lowercase<"FOO">;   // "foo"

// Example 1.3: Capitalize Type
// -----------------------

type T5 = Capitalize<"hello">;  // "Hello"
type T6 = Capitalize<"foo">;   // "Foo"

// Example 1.4: Uncapitalize Type
// -----------------------

type T7 = Uncapitalize<"Hello">;  // "hello"
type T8 = Uncapitalize<"Foo">;     // "foo"

// ============================================================================
// SECTION 2: PRACTICAL USAGE
// ============================================================================

// Example 2.1: API Naming
// ---------------------

type ApiMethod = Uppercase<"get" | "post" | "put" | "delete">;

// Example 2.2: Event Naming
// ---------------------

type EventNames = `on${Capitalize<"click" | "hover" | "focus">}`;

// ============================================================================
// SECTION 3: COMBINED WITH UTILITY TYPES
// ============================================================================

// Example 3.1: With Partial
// ---------------------

type Props<T> = {
  [K in keyof T as `set${Capitalize<K & string>}`]: (value: T[K]) => void;
};

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 4.1+
// All targets

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 01_Template_Literal_Basics, 03_Generic_Template_Types

console.log("\n=== Built In Template Types Complete ===");
console.log("Next: 09_Template_Literal_Syntax/05_Custom_Template_Types");