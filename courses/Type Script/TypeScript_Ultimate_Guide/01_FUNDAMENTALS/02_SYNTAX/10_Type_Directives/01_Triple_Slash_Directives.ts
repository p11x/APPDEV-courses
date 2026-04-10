/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 10
 * Topic: Triple_Slash_Directives
 * Purpose: Understanding triple-slash directives
 * Difficulty: beginner
 * UseCase: backend
 * Version: TS 2.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Triple Slash Directives - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Special comments for compiler instructions
 * 💡 WHY: Control compilation and module resolution
 * 🔧 THREE_SLASH syntax
 */

// ============================================================================
// SECTION 1: REFERENCE DIRECTIVES
// ============================================================================

// Example 1.1: Reference Path
// -------------------------

/// <reference path="./types.ts" />

// Example 1.2: Reference Types
// ------------------------

/// <reference types="node" />

// Example 1.3: Reference Lib
// ---------------------

/// <reference lib="es2015" />

// ============================================================================
// SECTION 2: AMD DIRECTIVES
// ============================================================================

// Example 2.1: AMD Module
// --------------------

/// <amd-module name="NamedModule" />

// Example 2.2: AMD Dependency
// ----------------------

/// <amd-dependency name="Module" />

// ============================================================================
// SECTION 3: USING DIRECTIVES
// ============================================================================

// Example 3.1: When to Use
// ---------------------

// - Reference external .d.ts files
// - Include type definitions
// - Configure module resolution

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 2.0+
// Use with module: AMD or System

// ============================================================================
// SECURITY
// ============================================================================

// Directives affect compilation
// Be careful with external references

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 02_Reference_Directives, 03_AMD_Module_Directives

console.log("\n=== Triple Slash Directives Complete ===");
console.log("Next: 10_Type_Directives/02_Reference_Directives");