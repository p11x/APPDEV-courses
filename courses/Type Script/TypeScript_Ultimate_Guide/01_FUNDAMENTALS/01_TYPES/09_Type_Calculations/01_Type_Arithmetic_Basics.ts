/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 09_Type_Calculations
 * Topic: 01_Type_Arithmetic_Basics
 * Purpose: Type-level arithmetic operations (addition, subtraction)
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe compile-time calculations
 */

/**
 * Type Arithmetic Basics - Type-Level Math Operations
 * ==================================================
 * 
 * 📚 WHAT: Compile-time arithmetic operations using TypeScript types
 * 💡 WHY: Enables type-level computation and validation
 * 🔧 HOW: Tuple types with length for numeric computation
 */

// ============================================================================
// SECTION 1: BUILD TUPLE TYPE
// ============================================================================

// Example 1.1: Build tuple of specific length
type BuildTuple<N extends number, Acc extends unknown[] = []> = 
  Acc["length"] extends N 
    ? Acc 
    : BuildTuple<N, [...Acc, unknown]>;

type TupleOf5 = BuildTuple<5>; // [unknown, unknown, unknown, unknown, unknown]
type TupleOf10 = BuildTuple<10>;

// Example 1.2: Verify tuple length
type TupleLength<T extends unknown[]> = T["length"];

type LengthOfTuple = TupleLength<TupleOf5>; // 5

// ============================================================================
// SECTION 2: ADDITION
// ============================================================================

// Example 2.1: Add two numbers at type level
type Add<A extends number, B extends number> = 
  [...BuildTuple<A>, ...BuildTuple<B>]["length"];

type AddResult = Add<2, 3>; // 5
type AddResult2 = Add<10, 20>; // 30
type AddResult3 = Add<0, 5>; // 5

// Example 2.2: Adding multiple numbers
type Sum<A extends number, B extends number, C extends number> = 
  Add<Add<A, B>, C>;

type SumResult = Sum<1, 2, 3>; // 6

// ============================================================================
// SECTION 3: SUBTRACTION
// ============================================================================

// Example 3.1: Subtract two numbers
type Subtract<A extends number, B extends number> = 
  BuildTuple<A> extends [...infer Rest, ...BuildTuple<B>] 
    ? Rest["length"] 
    : never;

type SubtractResult = Subtract<5, 2>; // 3
type SubtractResult2 = Subtract<10, 5>; // 5
type SubtractResult3 = Subtract<5, 0>; // 5

// ============================================================================
// SECTION 4: INCREMENT AND DECREMENT
// ============================================================================

// Example 4.1: Increment
type Increment<N extends number> = Add<N, 1>;

type IncResult = Increment<5>; // 6

// Example 4.2: Decrement
type Decrement<N extends number> = Subtract<N, 1>;

type DecResult = Decrement<5>; // 4

// ============================================================================
// SECTION 5: PRACTICAL EXAMPLES
// ============================================================================

// Example 5.1: Array length calculation
type FixedArray<T extends unknown[]> = T["length"] extends 0 
  ? "empty" 
  : T["length"];

type ArrayLength = FixedArray<[1, 2, 3]>; // 3

// Example 5.2: Index calculation
type IndexOf<T extends unknown[], I extends number> = 
  I extends 0 ? T[0] : 
  I extends 1 ? T[1] : 
  I extends 2 ? T[2] : 
  never;

type ThirdElement = IndexOf<["a", "b", "c"], 2>; // "c"

// ============================================================================
// SECTION 6: CHAINED ARITHMETIC
// ============================================================================

// Example 6.1: Chain multiple operations
type ComplexCalc = Add<Subtract<10, 5>, Increment<3>>;
// Add<5, 4> = 9

// Example 6.2: Formula-like type
type Formula<A extends number, B extends number> = 
  Add<Multiply<A, B>, Subtract<A, B>>;

type FormulaResult = Formula<4, 3>;
// Multiply<4, 3> = 12
// Subtract<4, 3> = 1
// Add<12, 1> = 13

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Type arithmetic uses tuple concatenation which is O(n) where
 * n is the magnitude of numbers. Large numbers cause slower compilation.
 * Consider caching frequently used type calculations.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Type arithmetic requires TypeScript 2.8+ with conditional
 * types. Works in all modern browsers and Node.js targets. No runtime code
 * is generated for these types.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Type arithmetic provides compile-time validation. Use it to
 * validate array bounds, index ranges, and numeric constraints at compile
 * time. Prevents runtime errors from out-of-bounds access.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test each arithmetic operation with known inputs. Verify edge
 * cases with 0 and negative numbers (if supported). Test compound operations.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use TypeScript's error messages for invalid operations. Hover
 * over result types to see computed values. Break complex formulas into
 * intermediate types.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Runtime calculations: More flexible but loses compile-time benefits
 * - Literal number types: Direct literal types for known values
 * - External libraries: ts-toolbelt provides optimized math types
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 02_Type_Comparison_Operations.ts: Comparison at type level
 * - 03_Type_Maximum_Minimum.ts: Finding extremes
 * - 04_Type_Absolute_Value.ts: Absolute value operations
 */

console.log("=== Type Arithmetic Basics Complete ===");
