/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 03_Type_Utilities
 * Concept: 02_Custom_Utilities
 * Topic: 03_Omit_Deep
 * Purpose: Implement deep omit type utility
 * Difficulty: intermediate
 * UseCase: type-transformations
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Omit<T, K> removes properties from a type at the top level.
 * DeepOmit recursively removes properties from nested objects.
 */

type Omit<T, K extends keyof any> = {
  [P in Exclude<keyof T, K>]: T[P];
};

type DeepOmit<T, K extends keyof any> = {
  [P in keyof T as P extends K ? never : P]: T[P] extends object ? DeepOmit<T[P], K> : T[P];
};

interface User {
  id: number;
  name: string;
  profile: {
    bio: string;
    avatar: string;
    password: string;
  };
  settings: {
    theme: string;
    apiKey: string;
  };
}

type UserWithoutPassword = DeepOmit<User, "password" | "apiKey">;

type OmitDeep<T, K extends string> = {
  [P in keyof T as P extends K ? never : P]: T[P] extends object ? OmitDeep<T[P], K> : T[P];
};

type FlatOmit<T, K extends string> = {
  [P in keyof T as P extends K ? never : P]: T[P];
};

type RecursiveOmit<T, K extends string> = {
  [P in keyof T]: P extends K ? never : T[P] extends object ? RecursiveOmit<T[P], K> : T[P];
};

console.log("\n=== Omit Deep Demo ===");
type O1 = DeepOmit<{ a: { x: 1; y: 2 }; b: 3 }, "x">;
type O2 = OmitDeep<{ a: { secret: 1; public: 2 } }, "secret">;
console.log("Omit deep works!");

/**
 * PERFORMANCE:
 * - Recursion can slow compilation for deep types
 * - Type inference may be slower for complex types
 * 
 * CROSS-REFERENCE:
 * - 01_Merge_Types.ts - Type merging
 * - 02_Diff_Types.ts - Type difference
 * - 01_Built_in_Utilities/01_Partial_Deep.ts - Deep partial
 */