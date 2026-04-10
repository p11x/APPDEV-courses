/**
 * Category: ADVANCED
 * Subcategory: COMPILERS
 * Concept: Type_System_Internals
 * Purpose: Control flow analysis internals
 * Difficulty: expert
 * UseCase: backend
 */

/**
 * Control Flow Analysis - Comprehensive Guide
 * ===========================================
 * 
 * 📚 WHAT: TypeScript control flow analysis
 * 💡 WHERE: Narrowing, type guards
 * 🔧 HOW: Flow analysis, type narrowing
 */

// ============================================================================
// SECTION 1: TYPE NARROWING
// ============================================================================

// Example 1.1: typeof Narrowing
// ---------------------------------

function narrowByType(value: string | number): void {
  if (typeof value === "string") {
    console.log(value.toUpperCase());
  } else {
    console.log(value.toFixed(2));
  }
}

// ============================================================================
// SECTION 2: INSTANCEOF NARROWING
// ---------------------------------

console.log("\n=== Control Flow Analysis Complete ===");
console.log("Next: ADVANCED/COMPILERS/02_Compiler_Plugins/01_Transformer_API.ts");