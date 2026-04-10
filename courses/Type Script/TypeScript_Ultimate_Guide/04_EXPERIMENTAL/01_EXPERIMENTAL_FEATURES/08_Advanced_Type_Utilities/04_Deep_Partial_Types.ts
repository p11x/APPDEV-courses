/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 08_Advanced_Type_Utilities
 * Topic: Deep_Partial_Types
 * Purpose: Creating deeply optional types
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(n) for object depth
 * Security: Compile-time only
 */

/**
 * Deep Partial Types - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Making nested properties optional recursively
 * 💡 WHERE: Update operations, patch requests, form handling
 * 🔧 HOW: Recursive mapped types with optional modifier
 */

// ============================================================================
// SECTION 1: WHAT - Deep Partial
// ============================================================================

/**
 * WHAT is Deep Partial?
 * - Making all nested properties optional
 * - Recursive application of Partial<T>
 * - Useful for update operations
 * - Supports partial object updates
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use Deep Partial?
 * - PATCH API requests
 * - Form state updates
 * - Configuration updates
 * - Partial data handling
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Basic Deep Partial
// --------------------------------

type DeepPartial<T> = {
  [K in keyof T]?: T[K] extends object ? DeepPartial<T[K]> : T[K];
};

type PartialUser = DeepPartial<{
  name: string;
  address: { city: string; country: string };
}>;
// { name?: string; address?: { city?: string; country?: string } }

// Example 3.2: Deep Partial with Required Base
// -------------------------------------------

type DeepPartialRequired<T, K extends keyof T> = Omit<T, K> & DeepPartial<Pick<T, K>>;

type PartialWithRequired = DeepPartialRequired<{ id: string; name: string }, "id">;
// { id: string; name?: string }

// Example 3.3: Conditional Deep Partial
// -------------------------------------

type ConditionalDeepPartial<T> = T extends Function 
  ? T 
  : T extends object 
    ? { [K in keyof T]?: ConditionalDeepPartial<T[K]> }
    : T;

// Example 3.4: Practical Example - API Update
// -------------------------------------------

interface User {
  id: string;
  profile: {
    name: string;
    email: string;
    settings: {
      theme: string;
      notifications: boolean;
    };
  };
}

type UserUpdate = DeepPartial<User>;

function updateUser(current: User, update: UserUpdate): User {
  return {
    ...current,
    ...update,
    profile: {
      ...current.profile,
      ...update.profile,
      settings: {
        ...current.profile.settings,
        ...update.profile?.settings,
      },
    },
  };
}

// Example 3.5: Deep Partial with Array Handling
// --------------------------------------------

type DeepPartialArray<T> = {
  [K in keyof T]?: T[K] extends Array<infer U> 
    ? Array<DeepPartial<U>> 
    : T[K] extends object 
      ? DeepPartialArray<T[K]> 
      : T[K];
};

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Recursive types may hit depth limits
 * - Consider depth parameter for control
 * - Efficient for typical use cases
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - TypeScript 4.1+ for full support
 * - Works in all modern environments
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Compile-time only
 * - No runtime security implications
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test with various depth levels
 * - Verify optional properties at all levels
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over type to verify optional
 * - Test update function behavior
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Manual partial type definition
 * - Library utilities
 * - Use spread operator at runtime
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Deep Partial Types Complete ===");
console.log("Next: 08_Advanced_Type_Utilities/05_Nested_Property_Types.ts");
console.log("Previous: 01_Type_Intersection_Operations.ts, 02_Type_Union_Operations.ts, 03_Deep_Readonly_Types.ts");
console.log("Related: 02_Conditional_and_Mapped_Types/03_Type_Transformation.ts");