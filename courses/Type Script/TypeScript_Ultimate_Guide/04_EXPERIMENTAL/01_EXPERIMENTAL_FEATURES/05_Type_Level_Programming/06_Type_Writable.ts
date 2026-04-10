/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 05_Type_Level_Programming
 * Topic: Type_Writable
 * Purpose: Making types writable (removing readonly)
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(n) where n is number of keys
 * Security: Compile-time only
 */

/**
 * Type Writable - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Removing readonly modifiers from types
 * 💡 WHERE: Mutable state management, API handling, object updates
 * 🔧 HOW: Mapped types with -readonly modifier
 */

// ============================================================================
// SECTION 1: WHAT - Writable Types
// ============================================================================

/**
 * WHAT are writable types?
 * - Types with readonly modifier removed
 * - Allow mutation of properties
 * - Used for mutable state
 * - Opposite of Readonly<T>
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use writable types?
 * - Handle mutable state in applications
 * - Create copy-before-modification patterns
 * - Work with external APIs that expect mutable objects
 * - Implement undo/redo functionality
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Basic Writable Type
// --------------------------------

type Writable<T> = {
  -readonly [K in keyof T]: T[K];
};

type ReadonlyType = { readonly name: string; readonly age: number };
type WritableType = Writable<ReadonlyType>;
// { name: string; age: number }

// Example 3.2: Nested Writable
// --------------------------------

type DeepWritable<T> = {
  -readonly [K in keyof T]: T[K] extends object ? DeepWritable<T[K]> : T[K];
};

type NestedReadonly = {
  readonly user: { readonly name: string; readonly age: number };
};

type NestedWritable = DeepWritable<NestedReadonly>;
// { user: { name: string; age: number } }

// Example 3.3: Recursive Writable with Tuples
// -------------------------------------------

type WritableDeep<T> = T extends object 
  ? { -readonly [K in keyof T]: WritableDeep<T[K]> }
  : T;

type WithArrays = { readonly items: readonly string[] };
type ArrayWritable = WritableDeep<WithArrays>;
// { items: string[] }

// Example 3.4: Selective Writable
// --------------------------------

type MakeWritableKeys<T, K extends keyof T> = {
  [P in keyof T]: P extends K ? T[P] : T[P];
} & {
  -readonly [P in K]: T[P];
};

// Example 3.5: Writable with Required
// ------------------------------------

type WritableRequired<T> = {
  -readonly [K in keyof T]-?: T[K];
};

type OptionalReadonly = { readonly name?: string; readonly age?: number };
type MandatoryWritable = WritableRequired<OptionalReadonly>;
// { name: string; age: number }

// Example 3.6: Practical Usage - State Management
// ----------------------------------------------

interface AppState {
  readonly user: {
    readonly name: string;
    readonly permissions: readonly string[];
  };
  readonly config: {
    readonly theme: string;
    readonly debug: boolean;
  };
}

type MutableState = WritableDeep<AppState>;

function updateState(state: MutableState, updates: Partial<MutableState>): MutableState {
  return { ...state, ...updates };
}

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - O(n) where n is number of keys
 * - Deep version may hit recursion limits
 * - Generally fast for typical use cases
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - TypeScript 4.1+ for key remapping
 * - Works in earlier versions for basic cases
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - May enable mutation where not intended
 * - Document mutable vs immutable interfaces
 * - Consider usingImmutability patterns
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test with nested objects
 * - Test with arrays and tuples
 * - Verify readonly is actually removed
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over type to verify
 * - Test assignment to readonly properties
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Manual type copying
 * - Use immer for immutable patterns
 * - Separate mutable/immutable type definitions
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Type Writable Complete ===");
console.log("Previous: 01_Type_Calculations.ts, 02_Type_Nats.ts, 03_Type_Booleans.ts, 04_Type_Level_Functions.ts, 05_Type_IsEqual.ts");
console.log("Related: 08_Advanced_Type_Utilities/03_Deep_Readonly_Types.ts, 04_Deep_Partial_Types.ts");