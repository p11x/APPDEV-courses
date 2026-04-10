/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 03_Type_Utilities
 * Concept: 01_Built_in_Utilities
 * Topic: 02_Required_Deep
 * Purpose: Learn deep required type utility
 * Difficulty: intermediate
 * UseCase: type-transformations
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Required<T> makes all properties required at the top level.
 * DeepRequired recursively applies this to nested objects.
 */

type DeepRequired<T> = {
  [P in keyof T]-?: T[P] extends object ? DeepRequired<T[P]> : T[P];
};

interface OptionalUser {
  id?: number;
  name?: string;
  profile?: {
    bio?: string;
    avatar?: string;
    settings?: {
      theme?: string;
      notifications?: boolean;
    };
  };
}

type RequiredUser = DeepRequired<OptionalUser>;

const requiredUser: RequiredUser = {
  id: 1,
  name: "Alice",
  profile: {
    bio: "Hello",
    avatar: "img.png",
    settings: {
      theme: "dark",
      notifications: true
    }
  }
};

type NonNullableDeep<T> = {
  [P in keyof T]: T[P] extends null | undefined ? never : T[P] extends object ? NonNullableDeep<T[P]> : T[P];
};

type NullableUser = {
  id: number | null;
  name: string | null;
  profile: {
    bio: string | undefined;
    avatar: string | null;
  } | null;
};

type NonNullableUser = NonNullableDeep<NullableUser>;

console.log("\n=== Deep Required Demo ===");
console.log("Required user:", requiredUser);

/**
 * CROSS-REFERENCE:
 * - 01_Partial_Deep.ts - Deep partial
 * - 03_Readonly_Deep.ts - Deep readonly
 */