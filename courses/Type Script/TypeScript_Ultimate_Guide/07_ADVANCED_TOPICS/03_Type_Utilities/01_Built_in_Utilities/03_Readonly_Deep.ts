/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 03_Type_Utilities
 * Concept: 01_Built_in_Utilities
 * Topic: 03_Readonly_Deep
 * Purpose: Learn deep readonly type utility
 * Difficulty: intermediate
 * UseCase: type-transformations
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Readonly<T> makes all properties readonly at the top level.
 * DeepReadonly recursively applies this to nested objects.
 */

type DeepReadonly<T> = {
  readonly [P in keyof T]: T[P] extends object ? DeepReadonly<T[P]> : T[P];
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

type ReadonlyUser = DeepReadonly<User>;

const user: ReadonlyUser = {
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

type Immutable<T> = {
  readonly [P in keyof T]: T[P] extends object ? Immutable<T[P]> : T[P];
};

function freeze<T extends object>(obj: T): DeepReadonly<T> {
  return Object.freeze(obj) as DeepReadonly<T>;
}

const frozen = freeze(user);
console.log("\n=== Deep Readonly Demo ===");
console.log("Frozen user:", frozen);

type DeepWritable<T> = {
  -readonly [P in keyof T]: T[P] extends object ? DeepWritable<T[P]> : T[P];
};

type WritableUser = DeepWritable<ReadonlyUser>;

console.log("\n=== Deep Writable Demo ===");
const mutable: WritableUser = { ...user };
mutable.name = "Bob";

/**
 * PERFORMANCE:
 * - Type computation compile-time only
 * - No runtime overhead unless using freeze()
 * 
 * CROSS-REFERENCE:
 * - 01_Partial_Deep.ts - Deep partial
 * - 02_Required_Deep.ts - Deep required
 */