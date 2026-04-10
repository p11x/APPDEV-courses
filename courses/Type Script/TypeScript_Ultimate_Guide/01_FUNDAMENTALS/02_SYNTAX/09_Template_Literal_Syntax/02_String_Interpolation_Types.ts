/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 09
 * Topic: String_Interpolation_Types
 * Purpose: Using string types in template literals
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TS 4.1+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * String Interpolation Types - Comprehensive Guide
 * ======================================
 * 
 * 📚 WHAT: Interpolating strings in types
 * 💡 WHY: Dynamic type generation from strings
 * 🔧 HOW: Uppercase, Lowercase, Capitalize, Uncapitalize
 */

// ============================================================================
// SECTION 1: CASE CONVERSION
// ============================================================================

// Example 1.1: Uppercase
// --------------------

type UpperCaseName = Uppercase<"hello">; // "HELLO"

// Example 1.2: Lowercase
// --------------------

type LowerCaseName = Lowercase<"HELLO">; // "hello"

// Example 1.3: Capitalize
// --------------------

type Capitalized = Capitalize<"hello">; // "Hello"

// Example 1.4: Uncapitalize
// --------------------

type Uncapitalized = Uncapitalize<"Hello">; // "hello"

// ============================================================================
// SECTION 2: CUSTOM CASE CONVERSION
// ============================================================================

// Example 2.1: Snake Case
// ---------------------

type SnakeCase<T> = T extends `${infer F}${infer R}` 
  ? `${F}${R extends Capitalize<R> ? `_${Lowercase<R>}` : SnakeCase<R>}` 
  : T;

// ============================================================================
// SECTION 3: PREFIX/SUFFIX
// ============================================================================

// Example 3.1: Add Prefix
// ---------------------

type WithPrefix<T extends string> = `prefix_${T}`;

// Example 3.2: Add Suffix
// ---------------------

type WithSuffix<T extends string> = `${T}_suffix`;

// ============================================================================
// SECTION 4: PRACTICAL USES
// ============================================================================

// Example 4.1: Event Types
// ---------------------

type Event<T extends string> = `on${Capitalize<T>}`;

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 4.1+
// All targets

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 01_Template_Literal_Basics, 03_Generic_Template_Types

console.log("\n=== String Interpolation Types Complete ===");
console.log("Next: 09_Template_Literal_Syntax/03_Generic_Template_Types");