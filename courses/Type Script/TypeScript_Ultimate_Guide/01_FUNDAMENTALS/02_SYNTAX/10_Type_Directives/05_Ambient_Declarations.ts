/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 10
 * Topic: Ambient_Declarations
 * Purpose: Understanding ambient declarations
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TS 2.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Ambient Declarations - Comprehensive Guide
 * =====================================
 * 
 * 📚 WHAT: Declaring types without implementation
 * 💡 WHY: Type existing JavaScript code
 * 🔧 HOW: declare keyword, .d.ts files
 */

// ============================================================================
// SECTION 1: AMBIENT VARIABLES
// ============================================================================

// Example 1.1: Declare Variable
// --------------------------

declare var globalConfig: {
  apiUrl: string;
  debug: boolean;
};

// ============================================================================
// SECTION 2: AMBIENT FUNCTIONS
// ============================================================================

// Example 2.1: Declare Function
// -------------------------

declare function fetchData(url: string): Promise<unknown>;

// ============================================================================
// SECTION 3: AMBIENT CLASSES
// ============================================================================

// Example 3.1: Declare Class
// -----------------------

declare class HttpClient {
  constructor(baseUrl: string);
  get(url: string): Promise<unknown>;
}

// ============================================================================
// SECTION 4: AMBIENT ENUMS
// ============================================================================

// Example 4.1: Declare Enum
// --------------------

declare enum HttpStatus {
  OK = 200,
  NotFound = 404,
  Error = 500
}

// ============================================================================
// SECTION 5: AMBIENT MODULES
// ============================================================================

// Example 5.1: Ambient Module
// -------------------------

declare module "fs" {
  function readFile(path: string): Promise<string>;
  function writeFile(path: string, data: string): Promise<void>;
}

// ============================================================================
// PERFORMANCE
// ============================================================================

// Ambient declarations add no runtime code
// Only for type checking

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 2.0+
// All targets

// ============================================================================
// SECURITY
// ============================================================================

// Verify declarations match runtime
// Use trusted declaration files

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 04_Custom_Type_Declarations

console.log("\n=== Ambient Declarations Complete ===");
console.log("10_Type_Directives Complete");
console.log("TypeScript Ultimate Guide - SYNTAX Complete");