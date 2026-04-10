/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 04_Type_Libraries
 * Concept: 02_Type_Creation
 * Topic: 02_Type_Builders
 * Purpose: Learn type builder patterns
 * Difficulty: intermediate
 * UseCase: type-construction
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Type builders provide fluent APIs for constructing complex types
 * through method chaining.
 */

interface TypeBuilder<T> {
  build(): T;
}

class ObjectBuilder<T extends object> implements TypeBuilder<T> {
  private props: Partial<T> = {};
  
  with<K extends keyof T>(key: K, value: T[K]): this {
    this.props[key] = value;
    return this;
  }
  
  build(): T {
    return this.props as T;
  }
}

interface User {
  id: number;
  name: string;
  email: string;
  age?: number;
}

const userBuilder = new ObjectBuilder<User>()
  .with("id", 1)
  .with("name", "Alice")
  .with("email", "alice@example.com");

class BuilderImpl<T> {
  protected state: T;
  
  constructor(initial: T) {
    this.state = initial;
  }
  
  set<K extends keyof T>(key: K, value: T[K]): this {
    this.state[key] = value;
    return this;
  }
  
  build(): T {
    return { ...this.state };
  }
}

class ConfigBuilder extends BuilderImpl<{ url: string; timeout: number; retries: number }> {
  constructor() {
    super({ url: "", timeout: 3000, retries: 3 });
  }
  
  url(url: string): this {
    this.state.url = url;
    return this;
  }
  
  timeout(timeout: number): this {
    this.state.timeout = timeout;
    return this;
  }
  
  retries(retries: number): this {
    this.state.retries = retries;
    return this;
  }
}

const config = new ConfigBuilder()
  .url("https://api.example.com")
  .timeout(5000)
  .retries(5)
  .build();

type FluentBuilder<T> = T & {
  with<K extends keyof T>(key: K, value: T[K]): FluentBuilder<T>;
  build(): T;
};

function fluent<T>(initial: T): FluentBuilder<T> {
  const builder: any = { ...initial };
  builder.with = (key: string, value: any) => {
    (builder as any)[key] = value;
    return builder;
  };
  builder.build = () => initial;
  return builder;
}

const fluentUser = fluent<User>({ id: 0, name: "", email: "" })
  .with("id", 1)
  .with("name", "Bob")
  .with("email", "bob@example.com");

console.log("\n=== Type Builders Demo ===");
console.log("Config:", config);
console.log("Fluent user:", fluentUser);

/**
 * CROSS-REFERENCE:
 * - 01_Factory_Functions.ts - Factory functions
 * - 03_Type_Guards.ts - Type guards
 */