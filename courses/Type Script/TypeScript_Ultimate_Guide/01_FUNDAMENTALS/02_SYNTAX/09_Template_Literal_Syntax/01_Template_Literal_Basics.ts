/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 09
 * Topic: Template_Literal_Basics
 * Purpose: Understanding template literal types
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TS 4.1+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Template Literal Basics - Comprehensive Guide
 * ===================================
 * 
 * 📚 WHAT: Creating types from string literals
 * 💡 WHY: Enable type-safe string operations
 * 🔧 HOW: Backtick syntax, interpolation
 */

// ============================================================================
// SECTION 1: BASIC SYNTAX
// ============================================================================

// Example 1.1: Simple Template Literal
// -----------------------------

type Greeting = `Hello, ${string}`;

const greeting: Greeting = "Hello, World";

// Example 1.2: Fixed Prefix
// ----------------------

type URL = `https://${string}`;

const url: URL = "https://example.com";

// ============================================================================
// SECTION 2: MULTIPLE INTERPOLATIONS
// ============================================================================

// Example 2.1: Multiple Parts
// -----------------------

type EventName = `on${string}Click`;

const event: EventName = "onButtonClick";

// ============================================================================
// SECTION 3: CONST LITERAL TYPES
// ============================================================================

// Example 3.1: Const Literal
// -----------------------

const prefix = "get";
type Getter = `${typeof prefix}${Capitalize<string>}`;

// ============================================================================
// SECTION 4: UNION TEMPLATES
// ============================================================================

// Example 4.1: Union in Template
// ---------------------------

type HTTPMethod = "get" | "post" | "put" | "delete";
type Handler = `${HTTPMethod}_handler`;

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 4.1+
// ES2021+

// ============================================================================
// SECURITY
// ============================================================================

// Validates string patterns
// Prevents invalid strings

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 02_String_Interpolation_Types, 03_Generic_Template_Types

console.log("\n=== Template Literal Basics Complete ===");
console.log("Next: 09_Template_Literal_Syntax/02_String_Interpolation_Types");