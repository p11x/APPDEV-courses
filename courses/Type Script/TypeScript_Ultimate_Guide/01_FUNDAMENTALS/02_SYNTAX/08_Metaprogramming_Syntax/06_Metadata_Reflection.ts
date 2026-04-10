/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 08
 * Topic: Metadata_Reflection
 * Purpose: Using reflect-metadata for decorator patterns
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TS 5.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Metadata Reflection - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Using reflection metadata with decorators
 * 💡 WHY: Store and retrieve type/member metadata
 * 🔧 HOW: Reflect metadata API, design-time metadata
 */

// ============================================================================
// SECTION 1: BASIC METADATA
// ============================================================================

// Example 1.1: Store Metadata
// -----------------------

const METADATA_KEY = "design:paramtypes";

function trackMetadata(target: Object, propertyKey: string | symbol, parameterIndex: number) {
  const existing = Reflect.getMetadata(METADATA_KEY, target) || [];
  existing.push(parameterIndex);
  Reflect.defineMetadata(METADATA_KEY, existing, target);
}

// ============================================================================
// SECTION 2: TYPE METADATA
// ============================================================================

// Example 2.1: Store Type Information
// ------------------------------

const TYPE_KEY = "design:type";

function trackType(target: Object, propertyKey: string | symbol, parameterIndex: number, type: Function) {
  Reflect.defineMetadata(TYPE_KEY, type, target, propertyKey, parameterIndex);
}

// ============================================================================
// SECTION 3: CUSTOM METADATA KEYS
// ============================================================================

// Example 3.1: Custom Metadata Keys
// ---------------------------

const ROUTES_KEY = "app:routes";

function trackRoute(path: string) {
  return function(target: Object, propertyKey: string | symbol) {
    const routes = Reflect.getMetadata(ROUTES_KEY, target.constructor) || [];
    routes.push({ path, method: propertyKey });
    Reflect.defineMetadata(ROUTES_KEY, routes, target.constructor);
  };
}

// ============================================================================
// SECTION 4: READING METADATA
// ============================================================================

// Example 4.1: Retrieve Metadata
// -------------------------

function getRoutes(target: Function): Array<{ path: string; method: PropertyKey }> {
  return Reflect.getMetadata(ROUTES_KEY, target) || [];
}

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Requires reflect-metadata package
// ES2015+

// ============================================================================
// SECURITY
// ============================================================================

// Metadata storage is on prototypes
// Consider security implications

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 05_Decorator_Factories, 07_Compile_Time_Code

console.log("\n=== Metadata Reflection Complete ===");
console.log("Next: 08_Metaprogramming_Syntax/07_Compile_Time_Code");