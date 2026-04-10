/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 11_Type_Libraries
 * Topic: 01_Utility_Types_Overview
 * Purpose: Overview of built-in and external TypeScript utility type libraries
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe type operations
 */

/**
 * Utility Types Overview
 * =====================
 * 
 * 📚 WHAT: Comprehensive overview of built-in and external utility types
 * 💡 WHY: Understanding available utilities enables efficient type manipulation
 * 🔧 HOW: Built-in utilities + external libraries
 */

// ============================================================================
// SECTION 1: BUILT-IN UTILITY TYPES
// ============================================================================

// TypeScript provides these built-in utility types:
// - Partial<T>: Makes all properties optional
// - Required<T>: Makes all properties required
// - Readonly<T>: Makes all properties readonly
// - Record<K, T>: Constructs object type with keys K and values T
// - Pick<T, K>: Creates type with specific properties
// - Omit<T, K>: Creates type excluding properties
// - Exclude<T, U>: Excludes types from union
// - Extract<T, U>: Extracts types assignable to U
// - NonNullable<T>: Removes null and undefined
// - ReturnType<T>: Extracts return type
// - Parameters<T>: Extracts parameter types
// - InstanceType<T>: Extracts instance type

// Example 1.1: Built-in usage
interface User {
  id: number;
  name: string;
  email: string;
}

type PartialUser = Partial<User>;
type RequiredUser = Required<User>;
type UserRecord = Record<string, User>;

// ============================================================================
// SECTION 2: TS-TOOLBELT OVERVIEW
// ============================================================================

// ts-toolbelt provides advanced type operations
// Installation: npm install ts-toolbelt

// Example 2.1: Advanced type operations
// import { Object } from "ts-toolbelt";

// type Update = Object.Update<{ a: string }, { a: number }>;
// // Changes property 'a' from string to number

// Example 2.2: Deep operations
// import { Object as O } from "ts-toolbelt";

// type DeepPartial = O.DeepPartial<{ a: { b: string } }>;

// ============================================================================
// SECTION 3: TYPE-FEST OVERVIEW
// ============================================================================

// type-fest provides additional utility types
// Installation: npm install type-fest

// Example 3.1: Useful types from type-fest
// import { Simplify, JsonValue, DeepRequired } from "type-fest";

// type Config = Simplify<{ a: string } & { b: number }>;

// ============================================================================
// SECTION 4: COMPARISON OF LIBRARIES
// ============================================================================

// Built-in: Basic utilities, widely available
// ts-toolbelt: Advanced mathematical and object operations
// type-fest: Community-driven, practical utilities
// simplytyped: Lightweight alternatives

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Utility types are compile-time only. External libraries
 * may increase compilation time. Use built-in when possible.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Built-in types work with TypeScript 2.8+.
 * External libraries require specific TypeScript versions.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Utility types provide compile-time type safety.
 * Use external libraries from trusted sources.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test each utility type with various inputs.
 * Verify expected output types.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over result types. Check error messages.
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 02_TsToolbelt_Integration.ts: ts-toolbelt details
 * - 03_Type_Fest_Integration.ts: type-fest details
 * - 05_Utility_Type_Composition.ts: Combining utilities
 */

console.log("=== Utility Types Overview Complete ===");
