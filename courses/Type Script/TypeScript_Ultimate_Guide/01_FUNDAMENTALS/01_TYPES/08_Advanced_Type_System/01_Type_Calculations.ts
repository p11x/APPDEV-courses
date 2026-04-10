/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 08_Advanced_Type_System
 * Topic: 01_Type_Calculations
 * Purpose: Type-level arithmetic operations and calculations
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 */

/**
 * Type Calculations - Advanced Type-Level Operations
 * ==================================================
 * 
 * 📚 WHAT: Type-level arithmetic and calculations
 * 💡 WHY: Enables compile-time computation
 * 🔧 HOW: Conditional types and inference
 */

// ============================================================================
// SECTION 1: BASIC TYPE MATH
// ============================================================================

// Example 1.1: Add Types
type Add<A extends number, B extends number> = 
  [...BuildTuple<A>, ...BuildTuple<B>]["length"];

type BuildTuple<N extends number, Acc extends unknown[] = []> = 
  Acc["length"] extends N 
    ? Acc 
    : BuildTuple<N, [...Acc, unknown]>;

type SumResult = Add<2, 3>; // 5

// Example 1.2: Subtract Types
type Subtract<A extends number, B extends number> = 
  BuildTuple<A> extends [...infer Rest, ...BuildTuple<B>] 
    ? Rest["length"] 
    : never;

type SubtractResult = Subtract<5, 2>; // 3

// Example 1.3: Multiply Types
type Multiply<A extends number, B extends number, Acc extends unknown[] = []> = 
  Acc["length"] extends (A * B) 
    ? Acc 
    : Multiply<A, B, [...Acc, unknown]>;

type MultiplyResult = Multiply<3, 4>; // 12

console.log("Type Calculations:", { SumResult, SubtractResult, MultiplyResult });

// ============================================================================
// SECTION 2: COMPARISON TYPES
// ============================================================================

// Example 2.1: Greater Than
type GreaterThan<A extends number, B extends number> = 
  A extends B ? false : (B extends A ? false : true);

type GTResult = GreaterThan<5, 3>;

// Example 2.2: Less Than
type LessThan<A extends number, B extends number> = 
  B extends A ? true : false;

type LTResult = LessThan<3, 5>;

// ============================================================================
// SECTION 3: MAXIMUM AND MINIMUM
// ============================================================================

// Example 3.1: Maximum Type
type Max<A extends number, B extends number> = 
  GreaterThan<A, B> extends true ? A : B;

type MaxResult = Max<10, 20>;

// Example 3.2: Minimum Type
type Min<A extends number, B extends number> = 
  LessThan<A, B> extends true ? A : B;

type MinResult = Min<10, 20>;

// ============================================================================
// SECTION 4: ABSOLUTE VALUE
// ============================================================================

type Abs<N extends number> = 
  N extends -infer P ? P : N;

type AbsResult = Abs<-5>;

// ============================================================================
// SECTION 5: POWER AND FACTORIAL
// ============================================================================

// Example 5.1: Power
type Power<Base extends number, Exp extends number, Acc extends unknown[] = []> = 
  Exp extends 0 
    ? 1 
    : Acc["length"] extends (Base ** Exp)
      ? Acc 
      : Power<Base, Exp, [...Acc, unknown]>;

type PowerResult = Power<2, 3>;

// Example 5.2: Factorial
type Factorial<N extends number> = 
  N extends 0 | 1 ? 1 : N * Factorial<N extends 1 ? never : N - 1>;

type FactorialResult = Factorial<5>;

console.log("\n=== Type Calculations Complete ===");
console.log("Next: 01_FUNDAMENTALS/01_TYPES/08_Advanced_Type_System/02_Advanced_Type_Guards.ts");