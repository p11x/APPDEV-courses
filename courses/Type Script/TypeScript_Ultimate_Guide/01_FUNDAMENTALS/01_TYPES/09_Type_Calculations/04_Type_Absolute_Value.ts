/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 09_Type_Calculations
 * Topic: 04_Type_Absolute_Value
 * Purpose: Type-level absolute value operations
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe numeric operations
 */

/**
 * Type Absolute Value - Absolute Value at Type Level
 * ================================================
 * 
 * 📚 WHAT: Computing absolute value of negative numbers at type level
 * 💡 WHY: Enables handling of negative numbers in type arithmetic
 * 🔧 HOW: Conditional type checking for negative values
 */

// ============================================================================
// SECTION 1: BASIC ABSOLUTE VALUE
// ============================================================================

// Example 1.1: Simple absolute value
type Abs<N extends number> = N extends -infer P ? P : N;

type AbsResult = Abs<-5>; // 5
type AbsResult2 = Abs<5>; // 5
type AbsResult3 = Abs<0>; // 0

// Example 1.2: Alternative implementation
type AbsAlt<N extends number> = N extends 0 | 1 ? N : (N extends -infer P ? P : N);

type AbsAltResult = AbsAlt<-100>; // 100
type AbsAltResult2 = AbsAlt<100>; // 100

// ============================================================================
// SECTION 2: SIGN DETECTION
// ============================================================================

// Example 2.1: Is negative
type IsNegative<N extends number> = N extends -infer P ? true : false;

type IsNegResult = IsNegative<-5>; // true
type IsPosResult = IsNegative<5>; // false

// Example 2.2: Sign type
type Sign<N extends number> = N extends -infer P ? "negative" : "positive";

type SignResult = Sign<-10>; // "negative"
type SignResult2 = Sign<10>; // "positive"

// ============================================================================
// SECTION 3: ABSOLUTE VALUE WITH BOUNDS
// ============================================================================

// Example 3.1: Abs with validation
type AbsWithMin<N extends number, Min extends number> = 
  N extends -infer P 
    ? P extends number 
      ? Max<P, Min> 
      : never 
    : N;

type AbsBounded = AbsWithMin<-100, 10>; // 100
type AbsBounded2 = AbsWithMin<-5, 10>; // 10 (clamped)

// ============================================================================
// SECTION 4: PRACTICAL EXAMPLES
// ============================================================================

// Example 4.1: Distance between two numbers
type Distance<A extends number, B extends number> = 
  Abs<Subtract<A, B>>;

type DistResult = Distance<10, 3>; // 7
type DistResult2 = Distance<3, 10>; // 7

// Example 4.2: Type-safe offset
type Offset<Index extends number, Delta extends number> = 
  Index extends number 
    ? Abs<Add<Index, Delta>> 
    : never;

type OffsetResult = Offset<5, -3>; // 8
type OffsetResult2 = Offset<5, 3>; // 8

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Absolute value uses conditional types which are O(1).
 * Performance is constant regardless of number magnitude.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Requires TypeScript 2.8+ with conditional types and
 * infer. Works in all modern environments.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Absolute value operations help maintain positive indices
 * and bounds. Use for distance calculations and offset validation.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with positive, negative, and zero values. Verify
 * sign detection works correctly. Test bounds clamping.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over result to see computed value. Check errors
 * for invalid type operations.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Direct positive literals: For known positive values
 * - Runtime Math.abs: For dynamic values
 * - Utility libraries: ts-toolbelt
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 01_Type_Arithmetic_Basics.ts: Arithmetic operations
 * - 03_Type_Maximum_Minimum.ts: Maximum and minimum
 * - 02_Type_Comparison_Operations.ts: Comparisons
 */

console.log("=== Type Absolute Value Complete ===");
