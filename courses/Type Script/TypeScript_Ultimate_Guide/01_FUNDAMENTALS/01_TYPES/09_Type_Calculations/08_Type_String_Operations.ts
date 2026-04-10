/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 09_Type_Calculations
 * Topic: 08_Type_String_Operations
 * Purpose: Type-level string manipulation operations
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe string operations
 */

/**
 * Type String Operations - String Manipulation at Type Level
 * ==========================================================
 * 
 * 📚 WHAT: String operations like uppercase, lowercase, length at type level
 * 💡 WHY: Enables type-safe string transformations
 * 🔧 HOW: Template literal types and conditional types
 */

// ============================================================================
// SECTION 1: STRING LENGTH
// ============================================================================

// Example 1.1: String length (requires helper)
type StringLength<S extends string> = S["length"];

type StrLenResult = StringLength<"hello">; // 5 (number literal)
type StrLenResult2 = StringLength<"">; // 0

// ============================================================================
// SECTION 2: CASE TRANSFORMATIONS
// ============================================================================

// Example 2.1: Uppercase
type UppercaseString<S extends string> = 
  S extends `${infer L}${infer R}` 
    ? `${Uppercase<L>}${UppercaseString<R>}` 
    : S;

type UpperResult = UppercaseString<"hello">; // "HELLO"

// Example 2.2: Lowercase
type LowercaseString<S extends string> = 
  S extends `${infer L}${infer R}` 
    ? `${Lowercase<L>}${LowercaseString<R>}` 
    : S;

type LowerResult = LowercaseString<"HELLO">; // "hello"

// ============================================================================
// SECTION 3: STRING CONCATENATION
// ============================================================================

// Example 3.1: String concat
type ConcatString<A extends string, B extends string> = `${A}${B}`;

type ConcatResult = ConcatString<"hello", "world">; // "helloworld"

// Example 3.2: Prefix and suffix
type WithPrefix<S extends string, P extends string> = `${P}${S}`;
type WithSuffix<S extends string, Sfx extends string> = `${S}${Sfx}`;

type Prefixed = WithPrefix<"name", "my_">; // "my_name"
type Suffixed = WithSuffix<"file", ".ts">; // "file.ts"

// ============================================================================
// SECTION 4: STRING SPLIT
// ============================================================================

// Example 4.1: Split into tuple
type Split<S extends string, Sep extends string> = 
  S extends `${infer L}${Sep}${infer R}` 
    ? [L, ...Split<R, Sep>] 
    : S extends "" 
      ? [] 
      : [S];

type SplitResult = Split<"a,b,c", ",">; // ["a", "b", "c"]

// ============================================================================
// SECTION 5: PRACTICAL EXAMPLES
// ============================================================================

// Example 5.1: camelCase to snake_case
type SnakeCase<S extends string> = 
  S extends `${infer T}${infer U}`
    ? T extends Uppercase<T>
      ? `_${Lowercase<T>}${SnakeCase<U>}`
      : `${T}${SnakeCase<U>}`
    : S;

type SnakeResult = SnakeCase<"helloWorld">; // "hello_world"

// Example 5.2: Template paths
type JoinPath<Parts extends string[]> = 
  Parts extends [infer F extends string, ...infer R extends string[]]
    ? F extends ""
      ? JoinPath<R>
      : `${F}/${JoinPath<R>}`
    : "";

type PathResult = JoinPath<["src", "components", "Button"]>; // "src/components/Button"

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: String operations are O(n) where n is string length.
 * Each character requires recursive processing. Large strings
 * may cause slower compilation.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Template literal types require TypeScript 4.1+.
 * Case transformations require TypeScript 4.5+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Type-level string operations provide compile-time safety.
 * Use for path manipulation, naming conventions, and validation.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test empty strings. Test various case combinations.
 * Test path joining with multiple segments.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over result types. Break complex transformations
 * into intermediate steps.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Runtime string methods: For dynamic strings
 * - Direct literals: For known strings
 * - Utility libraries: ts-toolbelt
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 06_Mapped_Type_Constraints.ts: Key transformations
 * - 01_Type_Arithmetic_Basics.ts: Basic operations
 * - 10_Infer_Type_Patterns.ts: Type inference
 */

console.log("=== Type String Operations Complete ===");
