/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 08_Advanced_Type_System
 * Topic: 06_Mapped_Type_Constraints
 * Purpose: Constraining mapped types with key and value constraints
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe transformations
 */

/**
 * Mapped Type Constraints - Constraining Mapped Types
 * ====================================================
 * 
 * 📚 WHAT: Adding constraints to mapped types for key filtering and value transformation
 * 💡 WHY: Enables precise type transformations with conditions
 * 🔧 HOW: Key remapping and conditional types in mapped types
 */

// ============================================================================
// SECTION 1: BASIC MAPPED TYPE CONSTRAINTS
// ============================================================================

// Example 1.1: Pick keys by type
type PickByType<T, V> = {
  [K in keyof T as T[K] extends V ? K : never]: T[K];
};

interface User {
  id: number;
  name: string;
  age: number;
  email: string;
  active: boolean;
}

type StringProperties = PickByType<User, string>; // { name: string; email: string }
type NumberProperties = PickByType<User, number>; // { id: number; age: number }

// Example 1.2: Filter by key pattern
type FilterKeys<T, Prefix extends string> = {
  [K in keyof T as K extends `${Prefix}${string}` ? K : never]: T[K];
};

interface ApiConfig {
  apiUrl: string;
  apiKey: string;
  timeout: number;
  retryCount: number;
  debugMode: boolean;
}

type ApiSettings = FilterKeys<ApiConfig, "api">; // { apiUrl: string; apiKey: string }

// ============================================================================
// SECTION 2: CONDITIONAL KEY MAPPING
// ============================================================================

// Example 2.1: Make specific keys optional
type MakeOptional<T, K extends keyof T> = Omit<T, K> & Partial<Pick<T, K>>;

interface Config {
  host: string;
  port: number;
  ssl: boolean;
  auth: string;
}

type OptionalAuth = MakeOptional<Config, "auth">;

// Example 2.2: Make specific keys required
type MakeRequired<T, K extends keyof T> = Omit<T, K> & Required<Pick<T, K>>;

interface FormData {
  name?: string;
  email?: string;
  age?: number;
}

type RequiredFormData = MakeRequired<FormData, "name" | "email">;

// ============================================================================
// SECTION 3: VALUE TRANSFORMATION CONSTRAINTS
// ============================================================================

// Example 3.1: Transform value to return type
type MethodReturnTypes<T> = {
  [K in keyof T as T[K] extends (...args: any[]) => any ? K : never]: T[K] extends (...args: any[]) => infer R ? R : never;
};

class Service {
  getUser(id: number): Promise<{ name: string }> {
    return Promise.resolve({ name: "John" });
  }
  
  saveUser(user: { name: string }): Promise<void> {
    return Promise.resolve();
  }
  
  validate(value: string): boolean {
    return true;
  }
  
  config: string = "config";
}

type ServiceMethods = MethodReturnTypes<Service>;
// { getUser: Promise<{ name: string }>; saveUser: Promise<void>; validate: boolean }

// Example 3.2: Nullable to optional
type NullableToOptional<T> = {
  [K in keyof T as T[K] extends null | undefined ? K : never]?: T[K];
} & {
  [K in keyof T as T[K] extends null | undefined ? never : K]: T[K];
};

interface ApiResponse {
  data: { id: number } | null;
  error: string | null;
  loading: boolean;
}

type TransformedResponse = NullableToOptional<ApiResponse>;

// ============================================================================
// SECTION 4: KEY REMAPPING
// ============================================================================

// Example 4.1: Add prefix to keys
type PrefixKeys<T, P extends string> = {
  [K in keyof T as `${P}${K & string}`]: T[K];
};

interface DatabaseConfig {
  host: string;
  port: number;
  name: string;
}

type PrefixedConfig = PrefixKeys<DatabaseConfig, "DB_">;
// { DB_host: string; DB_port: number; DB_name: string }

// Example 4.2: Convert camelCase to snake_case
type SnakeCase<S extends string> = S extends `${infer T}${infer U}`
  ? T extends Uppercase<T>
    ? `_${Lowercase<T>}${SnakeCase<U>}`
    : `${T}${SnakeCase<U>}`
  : S;

type SnakeCaseKeys<T> = {
  [K in keyof T as SnakeCase<K & string>]: T[K];
};

interface UserProfile {
  userName: string;
  emailAddress: string;
  createdAt: Date;
}

type SnakeCaseProfile = SnakeCaseKeys<UserProfile>;
// { user_name: string; email_address: string; created_at: Date }

// ============================================================================
// SECTION 5: COMPOSED CONSTRAINTS
// ============================================================================

// Example 5.1: Readonly and optional combination
type ReadonlyOptional<T> = {
  readonly [K in keyof T]?: T[K];
};

interface Settings {
  theme: string;
  notifications: boolean;
}

type ConstrainedSettings = ReadonlyOptional<Settings>;

// Example 5.2: Filter and transform
type Getters<T> = {
  [K in keyof T as `get${Capitalize<K & string>}`]: () => T[K];
};

interface State {
  count: number;
  name: string;
  active: boolean;
}

type StateGetters = Getters<State>;
// { getCount: () => number; getName: () => string; getActive: () => boolean }

// ============================================================================
// SECTION 6: ADVANCED CONSTRAINT PATTERNS
// ============================================================================

// Example 6.1: Exclude methods from type
type NonMethodKeys<T> = {
  [K in keyof T]: T[K] extends (...args: any[]) => any ? never : K;
}[keyof T];

type NonMethodProperties<T> = Pick<T, NonMethodKeys<T>>;

class Utils {
  static helper(): void {}
  value: string = "value";
  compute(x: number): number { return x * 2; }
}

type PropsOnly = NonMethodProperties<Utils>; // { value: string }

// Example 6.2: Conditional mapped type
type ConditionalMap<T, Condition> = {
  [K in keyof T as T[K] extends Condition ? K : never]: T[K];
};

interface Data {
  id: number;
  name: string;
  active: boolean;
  count: number;
}

type StringAndBooleanKeys = ConditionalMap<Data, string | boolean>;
// { name: string; active: boolean }

// ============================================================================
// SECTION 7: PRACTICAL APPLICATIONS
// ============================================================================

// Example 7.1: API form validation
type ValidationRules<T> = {
  [K in keyof T]: {
    required: boolean;
    type: "string" | "number" | "boolean";
    min?: T[K] extends number ? number : never;
    max?: T[K] extends number ? number : never;
  };
};

interface LoginForm {
  username: string;
  password: string;
}

type LoginValidation = ValidationRules<LoginForm>;
// {
//   username: { required: true; type: "string" };
//   password: { required: true; type: "string" };
// }

// Example 7.2: Event handler types
type EventHandlers<T extends string> = {
  [K in T as `on${Capitalize<K>}`]: (event: { type: K }) => void;
};

type AppEvents = "click" | "hover" | "focus";
type AppEventHandlers = EventHandlers<AppEvents>;
// { onClick: (event: { type: "click" }) => void; ... }

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Complex mapped types with key remapping increase compilation
 * time. Use caching for frequently used complex types. Key remapping
 * operations are O(n) where n is the number of keys.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Key remapping requires TypeScript 4.1+. Conditional
 * mapping in mapped types requires TypeScript 4.6+. Earlier versions
 * support basic mapped types without key filtering.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Mapped type constraints enable type-safe transformations that
 * prevent runtime errors. Use ReadonlyOptional for immutable configurations.
 * Avoid exposing sensitive data through transformed types.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test each transformation with various input types. Verify
 * key filtering works correctly. Test edge cases with empty types and
 * never-typed keys.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use IDE tooltips to see transformed types. Break complex
 * transformations into intermediate steps. Use Pick/Omit to isolate
 * problematic key transformations.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Manual type definitions: More verbose but explicit
 * - Utility libraries: ts-toolbelt provides optimized versions
 * - Helper functions: Runtime transformations with type inference
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 07_Conditional_Type_Chaining.ts: Conditional types with mapped types
 * - 10_Infer_Type_Patterns.ts: Inference in mapped types
 * - 10_Type_Utilities: Built-in utility types
 */

console.log("=== Mapped Type Constraints Complete ===");
