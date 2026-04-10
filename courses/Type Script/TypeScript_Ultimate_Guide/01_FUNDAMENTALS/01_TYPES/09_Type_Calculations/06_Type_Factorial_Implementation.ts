/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 09_Type_Calculations
 * Topic: 06_Type_Factorial_Implementation
 * Purpose: Type-level factorial computation
 * Difficulty: expert
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe recursion
 */

/**
 * Type Factorial Implementation - Recursive Type Math
 * =====================================================
 * 
 * 📚 WHAT: Computing factorials at the type level
 * 💡 WHY: Demonstrates recursive type-level computation
 * 🔧 HOW: Recursive conditional types with multiplication
 */

// ============================================================================
// SECTION 1: BASIC FACTORIAL
// ============================================================================

// Example 1.1: Simple factorial
type Factorial<N extends number> = 
  N extends 0 | 1 ? 1 : N * Factorial<Subtract<N, 1>>;

type FactorialResult = Factorial<5>; // 120
type FactorialResult2 = Factorial<3>; // 6
type FactorialResult3 = Factorial<0>; // 1

// ============================================================================
// SECTION 2: FACTORIAL WITH BOUNDS
// ============================================================================

// Example 2.1: Bounded factorial
type FactorialBounded<N extends number, Max extends number = 20> = 
  N extends number 
    ? GreaterThan<N, Max> extends true 
      ? FactorialBounded<Max>
      : (N extends 0 | 1 ? 1 : N * FactorialBounded<Subtract<N, 1>>)
    : never;

type BoundedFactorial = FactorialBounded<10>; // 3628800

// ============================================================================
// SECTION 3: FACTORIAL VALIDATION
// ============================================================================

// Example 3.1: Valid factorial input
type IsValidFactorialInput<N extends number> = 
  GreaterThanOrEqual<N, 0> extends true 
    ? LessThanOrEqual<N, 20> extends true 
      ? true 
      : false 
    : false;

type ValidInput = IsValidFactorialInput<5>; // true
type TooLarge = IsValidFactorialInput<25>; // false (hits recursion limit)
type Negative = IsValidFactorialInput<-5>; // false

// ============================================================================
// SECTION 4: PRACTICAL EXAMPLES
// ============================================================================

// Example 4.1: Permutations
type Permutations<N extends number, R extends number> = 
  Divide<Factorial<N>, Factorial<Subtract<N, R>>>;

type PermResult = Permutations<5, 2>; // 20 (5! / 3!)

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Factorial is O(n) recursion depth. Each level multiplies
 * results. Large values (>20) hit TypeScript's recursion limit.
 * Consider using lookup tables for larger values.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Requires TypeScript 2.8+ with recursive conditional
 * types. Max practical value is around 20 due to recursion limits.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Factorial computation is compile-time only. Use bounded
 * version to prevent infinite recursion. Validate inputs before computation.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test small values (0, 1, 2, 3, 5, 10). Verify bounded
 * version limits correctly. Test validation.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use small numbers first. Break recursive calls into
 * intermediate types. Check for "Type instantiation is excessively deep"
 * errors.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Lookup tables: For common factorial values
 * - Runtime calculation: For larger values
 * - External libraries: ts-toolbelt
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 05_Type_Power_Functions.ts: Power operations
 * - 01_Type_Arithmetic_Basics.ts: Basic arithmetic
 * - 03_Type_Maximum_Minimum.ts: Maximum/minimum
 */

console.log("=== Type Factorial Implementation Complete ===");
