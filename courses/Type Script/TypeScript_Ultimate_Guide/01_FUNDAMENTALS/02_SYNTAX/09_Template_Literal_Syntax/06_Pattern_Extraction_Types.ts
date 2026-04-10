/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 09
 * Topic: Pattern_Extraction_Types
 * Purpose: Extracting patterns from template literal types
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TS 4.1+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Pattern Extraction Types - Comprehensive Guide
 * =============================================
 * 
 * 📚 WHAT: Extracting types from string patterns
 * 💡 WHY: Type inference from strings
 * 🔧 HOW: Infer in template literals
 */

// ============================================================================
// SECTION 1: PARAMETER EXTRACTION
// ============================================================================

// Example 1.1: Extract Parameter
// ---------------------------

type ExtractParam<T> = T extends `${infer P}=${infer V}` ? V : never;

type ParamValue = ExtractParam<"name=john">; // "john"

// ============================================================================
// SECTION 2: PREFIX EXTRACTION
// ============================================================================

// Example 2.1: Extract Prefix
// ----------------------

type ExtractPrefix<T> = T extends `${infer P}_${string}` ? P : never;

type Method = ExtractPrefix<"get_user">; // "get"

// ============================================================================
// SECTION 3: PATH EXTRACTION
// ============================================================================

// Example 3.1: Extract Path Parts
// -------------------------

type PathParts<T> = T extends `${infer A}/${infer B}` ? [A, B] : [T];

type Parts = PathParts<"users/profile">; // ["users", "profile"]

// ============================================================================
// SECTION 4: COMPLEX EXTRACTION
// ============================================================================

// Example 4.1: Extract Query Params
// --------------------------

type QueryParams<T> = T extends `${string}?${infer Q}` ? Q : never;

type Query = QueryParams<"/api/users?page=1">; // "page=1"

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 4.1+
// All targets

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 05_Custom_Template_Types

console.log("\n=== Pattern Extraction Types Complete ===");
console.log("09_Template_Literal_Syntax Complete");
console.log("Next: 10_Type_Directives/01_Triple_Slash_Directives");