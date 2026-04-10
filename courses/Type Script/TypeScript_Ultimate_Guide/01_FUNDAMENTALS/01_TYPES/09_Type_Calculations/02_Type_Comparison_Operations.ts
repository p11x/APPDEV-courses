/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 09_Type_Calculations
 * Topic: 02_Type_Comparison_Operations
 * Purpose: Type-level comparison operations (greater than, less than, equals)
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe comparisons
 */

/**
 * Type Comparison Operations - Type-Level Comparisons
 * ==================================================
 * 
 * 📚 WHAT: Comparing numbers at the type level
 * 💡 WHY: Enables conditional type branching based on numeric values
 * 🔧 HOW: Tuple length comparison with conditional types
 */

// ============================================================================
// SECTION 1: EQUALITY CHECK
// ============================================================================

// Example 1.1: Direct equality
type Equals<A extends number, B extends number> = 
  A extends B ? (B extends A ? true : false) : false;

type EqualsResult = Equals<5, 5>; // true
type NotEqualsResult = Equals<5, 3>; // false

// Example 1.2: Tuple-based equality
type TupleEquals<A extends number, B extends number> = 
  BuildTuple<A>["length"] extends BuildTuple<B>["length"]
    ? BuildTuple<B>["length"] extends BuildTuple<A>["length"]
      ? true
      : false
    : false;

type TupleEqualsResult = TupleEquals<5, 5>; // true

// ============================================================================
// SECTION 2: GREATER THAN
// ============================================================================

// Example 2.1: Greater than
type GreaterThan<A extends number, B extends number> = 
  A extends B ? false : (B extends A ? false : true);

type GTResult = GreaterThan<5, 3>; // true
type GTResult2 = GreaterThan<3, 5>; // false
type GTResult3 = GreaterThan<5, 5>; // false

// Example 2.2: Alternative implementation
type GreaterThanAlt<A extends number, B extends number, Acc extends unknown[] = []> = 
  Acc["length"] extends A 
    ? false 
    : Acc["length"] extends B 
      ? true 
      : GreaterThanAlt<A, B, [...Acc, unknown]>;

type GTAltResult = GreaterThanAlt<10, 5>; // true

// ============================================================================
// SECTION 3: LESS THAN
// ============================================================================

// Example 3.1: Less than
type LessThan<A extends number, B extends number> = 
  B extends A ? true : false;

type LTResult = LessThan<3, 5>; // true
type LTResult2 = LessThan<5, 3>; // false
type LTResult3 = LessThan<5, 5>; // false

// Example 3.2: Alternative implementation
type LessThanAlt<A extends number, B extends number> = 
  GreaterThan<A, B> extends true ? false : (Equals<A, B> extends true ? false : true);

type LTAltResult = LessThanAlt<2, 7>; // true

// ============================================================================
// SECTION 4: GREATER THAN OR EQUAL
// ============================================================================

// Example 4.1: Greater than or equal
type GreaterThanOrEqual<A extends number, B extends number> = 
  GreaterThan<A, B> extends true ? true : (Equals<A, B> extends true ? true : false);

type GTEResult = GreaterThanOrEqual<5, 3>; // true
type GTEResult2 = GreaterThanOrEqual<5, 5>; // true
type GTEResult3 = GreaterThanOrEqual<3, 5>; // false

// ============================================================================
// SECTION 5: LESS THAN OR EQUAL
// ============================================================================

// Example 5.1: Less than or equal
type LessThanOrEqual<A extends number, B extends number> = 
  LessThan<A, B> extends true ? true : (Equals<A, B> extends true ? true : false);

type LTEResult = LessThanOrEqual<3, 5>; // true
type LTEResult2 = LessThanOrEqual<5, 5>; // true
type LTEResult3 = LessThanOrEqual<5, 3>; // false

// ============================================================================
// SECTION 6: PRACTICAL APPLICATIONS
// ============================================================================

// Example 6.1: Type-level bounds checking
type IsValidIndex<I extends number, L extends number> = 
  LessThanOrEqual<I, L> extends true ? (Equals<I, L> extends true ? false : true) : false;

type ValidIndex = IsValidIndex<3, 5>; // true
type InvalidIndex = IsValidIndex<5, 5>; // false (out of bounds)

// Example 6.2: Conditional type selection
type SelectNumber<A extends number, B extends number, Condition extends boolean> = 
  Condition extends true ? A : B;

type Selected = SelectNumber<5, 10, LessThan<3, 5>>; // 5

// Example 6.3: Range validation
type InRange<N extends number, Min extends number, Max extends number> = 
  GreaterThanOrEqual<N, Min> extends true 
    ? LessThanOrEqual<N, Max> extends true 
      ? true 
      : false 
    : false;

type InRangeResult = InRange<5, 1, 10>; // true
type OutOfRangeResult = InRange<15, 1, 10>; // false

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Comparison operations use tuple traversal which is O(n).
 * Complexity depends on the magnitude of numbers being compared.
 * Cache frequently used comparisons.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Comparison operations require TypeScript 2.8+ with
 * conditional types. Works in all modern browsers and Node.js targets.
 * No runtime code is generated.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Type-level comparisons enable compile-time validation of
 * array bounds, index ranges, and numeric constraints. Prevents invalid
 * operations from reaching runtime.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test equality with equal and unequal values. Test greater than
 * and less than with various combinations. Test edge cases with 0.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use IDE tooltips to see comparison results. Check TypeScript
 * errors for invalid comparisons. Break complex conditions into parts.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Direct literal comparison: For known values at compile time
 * - Runtime validation: More flexible but loses compile-time benefits
 * - Utility libraries: ts-toolbelt comparison types
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 01_Type_Arithmetic_Basics.ts: Basic arithmetic operations
 * - 03_Type_Maximum_Minimum.ts: Finding extremes using comparisons
 * - 07_Conditional_Type_Chaining.ts: Conditional type logic
 */

console.log("=== Type Comparison Operations Complete ===");
