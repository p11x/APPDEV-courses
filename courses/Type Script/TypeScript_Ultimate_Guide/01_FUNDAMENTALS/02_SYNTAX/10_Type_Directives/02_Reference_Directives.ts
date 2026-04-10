/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 10
 * Topic: Reference_Directives
 * Purpose: Using reference directives for type includes
 * Difficulty: beginner
 * UseCase: backend
 * Version: TS 2.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Reference Directives - Comprehensive Guide
 * =======================================
 * 
 * 📚 WHAT: Including type definitions
 * 💡 WHY: Access external type information
 * 🔧 HOW: Triple-slash reference syntax
 */

// ============================================================================
// SECTION 1: PATH REFERENCES
// ============================================================================

// Example 1.1: Local Reference
// ----------------------

/// <reference path="../types/common.ts" />

// Example 1.2: Multiple References
// -----------------------------

/// <reference path="./a.ts" />
/// <reference path="./b.ts" />

// ============================================================================
// SECTION 2: TYPES REFERENCES
// ============================================================================

// Example 2.1: Node Types
// --------------------

/// <reference types="node" />

// Example 2.2: Multiple Type Roots
// --------------------------

/// <reference types="node" />
/// <reference types="express" />

// ============================================================================
// SECTION 3: LIB REFERENCES
// ============================================================================

// Example 3.1: ES Libs
// ----------------

/// <reference lib="es2015" />
/// <reference lib="es2017" />

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 2.0+
// All targets

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 01_Triple_Slash_Directives, 04_Custom_Type_Declarations

console.log("\n=== Reference Directives Complete ===");
console.log("Next: 10_Type_Directives/03_AMD_Module_Directives");