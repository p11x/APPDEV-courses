/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 03_Type_Utilities
 * Concept: 01_Built_in_Utilities
 * Topic: 01_Partial_Deep
 * Purpose: Learn deep partial type utility
 * Difficulty: intermediate
 * UseCase: type-transformations
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Partial<T> makes all properties optional, but only at the top level.
 * DeepPartial recursively applies this to nested objects.
 */

type DeepPartial<T> = {
  [P in keyof T]?: T[P] extends object ? DeepPartial<T[P]> : T[P];
};

interface User {
  id: number;
  name: string;
  profile: {
    bio: string;
    avatar: string;
    settings: {
      theme: string;
      notifications: boolean;
    };
  };
}

type PartialUser = Partial<User>;
type DeepPartialUser = DeepPartial<User>;

const deepPartialUser: DeepPartialUser = {
  profile: {
    settings: {
      theme: "dark"
    }
  }
};

type NestedPartial<T> = {
  [P in keyof T]?: T[P] extends object ? NestedPartial<T[P]> : T[P];
};

function updateUser(user: User, updates: DeepPartial<User>): User {
  return { ...user, ...updates };
}

const currentUser: User = {
  id: 1,
  name: "Alice",
  profile: {
    bio: "Hello",
    avatar: "img.png",
    settings: {
      theme: "light",
      notifications: true
    }
  }
};

const updated = updateUser(currentUser, {
  name: "Bob",
  profile: {
    bio: "Hi there"
  }
});

console.log("\n=== Deep Partial Demo ===");
console.log("Updated user:", updated);

/**
 * PERFORMANCE:
 * - Type computation is compile-time only
 * - No runtime overhead
 * 
 * COMPATIBILITY:
 * - Works with all TS targets
 * - TS 4.0+ for recursive types
 * 
 * CROSS-REFERENCE:
 * - 02_Required_Deep.ts - Deep required
 * - 03_Readonly_Deep.ts - Deep readonly
 * - 02_Custom_Utilities/03_Omit_Deep.ts - Deep omit
 */