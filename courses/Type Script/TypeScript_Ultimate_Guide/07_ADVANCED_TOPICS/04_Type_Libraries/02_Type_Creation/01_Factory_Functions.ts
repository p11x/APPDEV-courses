/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 04_Type_Libraries
 * Concept: 02_Type_Creation
 * Topic: 01_Factory_Functions
 * Purpose: Learn type factory functions
 * Difficulty: intermediate
 * UseCase: type-construction
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Factory functions create types programmatically, enabling
 * reusable type construction patterns.
 */

type Factory<T> = () => T;

type TypeFactory<T, Args extends any[] = []> = (...args: Args) => T;

type Builder<T> = {
  build(): T;
  with<K extends keyof T>(key: K, value: T[K]): Builder<T>;
};

function createBuilder<T>(initial: Partial<T>): Builder<T> {
  let state = { ...initial };
  return {
    build(): T {
      return state as T;
    },
    with<K extends keyof T>(key: K, value: T[K]): Builder<T> {
      state = { ...state, [key]: value };
      return this;
    }
  };
}

interface User {
  id: number;
  name: string;
  email: string;
  age: number;
}

const userBuilder = createBuilder<User>({ id: 0, name: "", email: "", age: 0 });
const user = userBuilder.with("id", 1).with("name", "Alice").with("email", "alice@example.com").with("age", 30).build();

type FactoryMap<T> = {
  [K in keyof T]: Factory<T[K]>;
};

type TypeMap<T> = {
  [K in keyof T]: T[K];
};

function createTypeFactory<T>(defaults: T): TypeFactory<T, Partial<keyof T>> {
  return (overrides?: Partial<T>) => ({ ...defaults, ...overrides });
}

type BuilderFactory<T> = {
  new(): Builder<T>;
  prototype: { build(): T };
};

function createBuilderFactory<T>(defaultProps: T): BuilderFactory<T> {
  return class implements Builder<T> {
    private _state: T = { ...defaultProps };
    with<K extends keyof T>(key: K, value: T[K]): Builder<T> {
      this._state[key] = value;
      return this;
    }
    build(): T {
      return { ...this._state };
    }
  } as any;
}

class UserBuilder extends createBuilderFactory<User>({ id: 0, name: "", email: "", age: 0 }) {}

const builtUser = new UserBuilder()
  .with("id", 2)
  .with("name", "Bob")
  .build();

console.log("\n=== Factory Functions Demo ===");
console.log("User from builder:", user);
console.log("User from class builder:", builtUser);

/**
 * PERFORMANCE:
 * - Factory functions are compile-time patterns
 * - Runtime overhead depends on implementation
 * 
 * CROSS-REFERENCE:
 * - 02_Type_Builders.ts - Type builders
 * - 03_Type_Guards.ts - Type guards
 */