/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 09_Type_Calculations
 * Topic: 03_Type_Maximum_Minimum
 * Purpose: Type-level maximum and minimum operations
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe numeric operations
 */

/**
 * Type Maximum and Minimum - Finding Extremes at Type Level
 * ==========================================================
 * 
 * 📚 WHAT: Determining greater and lesser values at compile time
 * 💡 WHY: Enables type-level bounds checking and validation
 * 🔧 HOW: Comparison types with conditional branching
 */

// ============================================================================
// SECTION 1: MAXIMUM
// ============================================================================

// Example 1.1: Max of two numbers
type Max<A extends number, B extends number> = 
  GreaterThan<A, B> extends true ? A : B;

type MaxResult = Max<10, 20>; // 20
type MaxResult2 = Max<30, 15>; // 30
type MaxResult3 = Max<5, 5>; // 5

// Example 1.2: Max implementation using tuple comparison
type MaxAlt<A extends number, B extends number> = 
  A extends B ? A : (B extends A ? B : (A extends B ? A : A));

type MaxAltResult = MaxAlt<100, 50>; // 100

// ============================================================================
// SECTION 2: MINIMUM
// ============================================================================

// Example 2.1: Min of two numbers
type Min<A extends number, B extends number> = 
  LessThan<A, B> extends true ? A : B;

type MinResult = Min<10, 20>; // 10
type MinResult2 = Min<30, 15>; // 15
type MinResult3 = Min<5, 5>; // 5

// Example 2.2: Min implementation using Max
type MinAlt<A extends number, B extends number> = 
  A extends B ? A : (B extends A ? B : (GreaterThan<A, B> extends true ? A : B));

type MinAltResult = MinAlt<100, 50>; // 50

// ============================================================================
// SECTION 3: MAXIMUM OF ARRAY
// ============================================================================

// Example 3.1: Max of tuple
type MaxOfTuple<T extends number[], Acc extends number = 0> = 
  T extends [infer F extends number, ...infer R extends number[]] 
    ? MaxOfTuple<R, Max<Acc, F>> 
    : Acc;

type MaxTuple = MaxOfTuple<[3, 1, 4, 1, 5, 9, 2, 6]>; // 9

// Example 3.2: Min of tuple
type MinOfTuple<T extends number[], Acc extends number = 9999> = 
  T extends [infer F extends number, ...infer R extends number[]] 
    ? MinOfTuple<R, Min<Acc, F>> 
    : Acc;

type MinTuple = MinOfTuple<[3, 1, 4, 1, 5, 9, 2, 6]>; // 1

// ============================================================================
// SECTION 4: CLAMPING VALUES
// ============================================================================

// Example 4.1: Clamp value between min and max
type Clamp<Value extends number, MinVal extends number, MaxVal extends number> = 
  LessThan<Value, MinVal> extends true 
    ? MinVal 
    : GreaterThan<Value, MaxVal> extends true 
      ? MaxVal 
      : Value;

type ClampedValue = Clamp<15, 0, 10>; // 10
type ClampedValue2 = Clamp<-5, 0, 10>; // 0
type ClampedValue3 = Clamp<5, 0, 10>; // 5

// ============================================================================
// SECTION 5: BOUNDS VALIDATION
// ============================================================================

// Example 5.1: Valid range check
type IsInRange<Value extends number, MinVal extends number, MaxVal extends number> = 
  GreaterThanOrEqual<Value, MinVal> extends true 
    ? LessThanOrEqual<Value, MaxVal> extends true 
      ? true 
      : false 
    : false;

type InRange = IsInRange<5, 1, 10>; // true
type OutOfRange = IsInRange<15, 1, 10>; // false

// Example 5.2: Array bounds
type ArrayBoundsCheck<Index extends number, Length extends number> = 
  Index extends Length ? false : IsInRange<Index, 0, Subtract<Length, 1>>;

type ValidIndex = ArrayBoundsCheck<3, 5>; // true
type InvalidIndex = ArrayBoundsCheck<5, 5>; // false (at boundary)
type NegativeIndex = ArrayBoundsCheck<-1, 5>; // false

// ============================================================================
// SECTION 6: PRACTICAL APPLICATIONS
// ============================================================================

// Example 6.1: Type-safe array access
type SafeArrayIndex<T extends unknown[], Index extends number> = 
  ArrayBoundsCheck<Index, T["length"]> extends true 
    ? T[Index] 
    : never;

type SafeElement = SafeArrayIndex<[string, number, boolean], 1>; // number
type InvalidElement = SafeArrayIndex<[string, number], 5>; // never

// Example 6.2: Pagination calculation
type PageSize = 10;
type MaxPageIndex<TotalItems extends number> = 
  Subtract<DivCeil<TotalItems, PageSize>, 1>;

type CurrentPage = MaxPageIndex<55>; // 4 (pages 0-4 for 55 items)

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Max/Min operations use comparison which is O(n) on tuple
 * size. For arrays, complexity is O(n). Consider caching results for
 * frequently used values.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Requires TypeScript 2.8+ with conditional types. Works
 * in all modern browsers and Node.js targets. No runtime code generated.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Type-level bounds checking prevents out-of-bounds errors at
 * compile time. Use clamping to ensure values stay within valid ranges.
 * Prevents security issues from invalid array access.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test Max with various combinations. Test Min with edge cases.
 * Test clamping with boundary values. Test array bounds thoroughly.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use IDE tooltips to see computed values. Break complex
 * operations into intermediate steps. Test each comparison individually.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Direct literal values: For known constants
 * - Runtime validation: More flexible but loses compile-time safety
 * - Utility libraries: ts-toolbelt math types
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 02_Type_Comparison_Operations.ts: Comparison operations
 * - 01_Type_Arithmetic_Basics.ts: Basic arithmetic
 * - 04_Type_Absolute_Value.ts: Absolute value
 */

console.log("=== Type Maximum and Minimum Complete ===");
