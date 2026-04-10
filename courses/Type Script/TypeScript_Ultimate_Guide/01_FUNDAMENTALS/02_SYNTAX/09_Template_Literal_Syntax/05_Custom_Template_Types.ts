/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 09
 * Topic: Custom_Template_Types
 * Purpose: Creating custom template literal types
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TS 4.1+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Custom Template Types - Comprehensive Guide
 * =========================================
 * 
 * 📚 WHAT: Building custom template literal types
 * 💡 WHY: Specialized string type transformations
 * 🔧 HOW: Conditional types with inference
 */

// ============================================================================
// SECTION 1: SNAKE CASE
// ============================================================================

// Example 1.1: Snake Case Type
// -------------------------

type SnakeCase<T> = T extends `${infer F}${infer R}` 
  ? `${F extends Uppercase<F> ? `_${Lowercase<F>}` : F}${SnakeCase<R>}` 
  : T;

type T1 = SnakeCase<"helloWorld">; // "hello_world"
type T2 = SnakeCase<"HTTPClient">; // "h_t_t_p_client"

// ============================================================================
// SECTION 2: CAMEL CASE
// ============================================================================

// Example 2.1: Camel Case Type
// -------------------------

type CamelCase<T> = SnakeCase<T>;

// ============================================================================
// SECTION 3: PASCAL CASE
// ============================================================================

// Example 3.1: Pascal Case Type
// -----------------------

type PascalCase<T> = Capitalize<CamelCase<T>>;

// ============================================================================
// SECTION 4: KEBAB CASE
// ============================================================================

// Example 4.1: Kebab Case Type
// ------------------------

type KebabCase<T> = SnakeCase<T> extends `${infer F}_${infer R}` 
  ? `${F}-${KebabCase<R>}` 
  : T;

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 4.1+
// Complex recursive types

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 04_Built_In_Template_Types, 06_Pattern_Extraction_Types

console.log("\n=== Custom Template Types Complete ===");
console.log("Next: 09_Template_Literal_Syntax/06_Pattern_Extraction_Types");