/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Advanced_Type_Operations
 * Purpose: Advanced type system operations and patterns
 * Difficulty: advanced
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Advanced Type Operations - Comprehensive Guide
 * ================================================
 * 
 * 📚 WHAT: Type-level programming, conditional types, type manipulation
 * 💡 WHY: Build sophisticated type-safe abstractions and libraries
 * 🔧 HOW: Template literals, mapped types, conditional inference
 */

// ============================================================================
// SECTION 1: TYPE-LEVEL PROGRAMMING BASICS
// ============================================================================

// Example 1.1: Basic Type Manipulation
// ----------------------------------

// Type aliases can reference themselves for recursion
type DeepReadonly<T> = {
  readonly [K in keyof T]: T[K] extends object ? DeepReadonly<T[K]> : T[K];
};

interface User {
  name: string;
  address: {
    city: string;
    coords: { lat: number; lng: number };
  };
}

type DeepReadonlyUser = DeepReadonly<User>;
// { readonly name: string; readonly address: { readonly city: string; ... } }

// Example 1.2: Type Recursion
// ----------------------

type DeepPartial<T> = {
  [K in keyof T]?: T[K] extends object ? DeepPartial<T[K]> : T[K];
};

// Usage
type PartialUser = DeepPartial<User>;
// { name?: string; address?: { city?: string; coords?: ... } }

// ============================================================================
// SECTION 2: CONDITIONAL TYPE PATTERNS
// ============================================================================

// Example 2.1: Distributive Conditional Types
// ---------------------------------------

// Union distributes over conditional
type ToArray<T> = T extends any ? T[] : never;

type StrOrNumArr = ToArray<string | number>;
// string[] | number[]

// Example 2.2: Filter Types
// ----------------------

type Filter<T, U> = T extends U ? never : T;

type ExcludeNumbers = Filter<string | number | boolean, number>;
// string | boolean

// Example 2.3: Partition Types
// -----------------------

type Partition<T, U> = {
  matching: T extends U ? T : never;
  nonMatching: T extends U ? never : T;
};

type Numbers = string | number | boolean;
type Part = Partition<Numbers, number>;
// { matching: number; nonMatching: string | boolean }

// ============================================================================
// SECTION 3: INFER PATTERNS
// ============================================================================

// Example 3.1: Return Type Inference
// -------------------------------

type MyReturnType<T> = T extends (...args: any[]) => infer R ? R : never;

type F1 = () => string;
type F2 = (x: number) => boolean;

type R1 = MyReturnType<F1>; // string
type R2 = MyReturnType<F2>; // boolean

// Example 3.2: Parameter Inference
// -----------------------------

type MyParameters<T> = T extends (...args: infer P) => any ? P : never;

type P1 = MyParameters<F1>; // []
type P2 = MyParameters<F2>; // [number]

// Example 3.3: Nested Inference
// -----------------------

type GetReturn<T> = T extends { get(): infer R } ? R : never;

interface WithGetter {
  get(): string;
}

type Got = GetReturn<WithGetter>; // string

// ============================================================================
// SECTION 4: MAPPED TYPES ADVANCED
// ============================================================================

// Example 4.1: Key Remapping
// ----------------------

interface ApiResponse {
  user_id: number;
  user_name: string;
  created_at: string;
}

type CamelCase<T> = {
  [K in keyof T as K extends `${string}_${string}` 
    ? K extends `${infer P}_${infer S}`
      ? `${P}${Capitalize<S>}`
      : K
    : K]: T[K];
};

type Converted = CamelCase<ApiResponse>;
// { userId: number; userName: string; createdAt: string }

// Example 4.2: Filter Keys in Mapped Types
// ----------------------------------

type KeysOfType<T, U> = {
  [K in keyof T]: T[K] extends U ? K : never;
}[keyof T];

interface Obj {
  a: string;
  b: number;
  c: boolean;
  d: string;
}

type StringKeys = KeysOfType<Obj, string>; // "a" | "d"

// Example 4.3: Add Prefix to Keys
// ---------------------------

type PrefixKeys<T, P extends string> = {
  [K in keyof T as `${P}${K & string}`]: T[K];
};

type Prefixed = PrefixKeys<{ a: 1; b: 2 }, "get">;
// { geta: 1; getb: 2 }

// ============================================================================
// SECTION 5: BUILT-IN UTILITY TYPES
// ============================================================================

// Example 5.1: Awaited Type
// ---------------------

type PromiseValue<T> = T extends Promise<infer U> ? U : T;

type P1 = PromiseValue<Promise<string>>; // string
type P2 = PromiseValue<number>; // number

// Example 5.2: InstanceType
// ---------------------

class MyClass {
  constructor(public x: number) {}
}

type Instance = InstanceType<typeof MyClass>; // MyClass

// Example 5.3: ThisType
// -----------------

interface WithThis {
  increment(this: WithThis): void;
}

// ============================================================================
// SECTION 6: TEMPLATE LITERAL TYPES
// ============================================================================

// Example 6.1: Pattern Matching Types
// -------------------------------

type RemovePrefix<T extends string> = 
  T extends `${infer P}_${string}` ? P : T;

type Removed = RemovePrefix<"user_id">; // "user"

// Example 6.2: String Manipulation Types
// ----------------------------------

type UppercaseKeys<T> = {
  [K in keyof T as Uppercase<K & string>]: T[K];
};

type Split<S extends string, D extends string> = 
  S extends `${infer T}${D}${infer U}` ? [T, ...Split<U, D>] : [S];

type Parts = Split<"a,b,c", ",">; // ["a", "b", "c"]

// Example 6.3: Union Template Literals
// -------------------------------

type HTTPMethod = "get" | "post" | "put" | "delete";
type Route = "/users" | "/posts" | "/comments";

type Endpoint = `${HTTPMethod} ${Route}`;

const endpoint: Endpoint = "get /users";

// ============================================================================
// SECTION 7: TYPE COMPOSITION PATTERNS
// ============================================================================

// Example 7.1: Compose Types
// ---------------------

type Compose<A, B> = A extends (x: infer X) => infer R 
  ? B extends (x: infer Y) => any 
    ? (x: Y) => R 
    : never 
  : never;

// Example 7.2: Pipe Types
// -------------------

type Pipe<T, Fns> = 
  Fns extends [(arg: infer A) => infer B, ...infer Rest]
    ? Pipe<B, Rest>
    : T;

// Example 7.3: Type Intersection
// --------------------------

type Intersect<T, U> = T & U;

// ============================================================================
// SECTION 8: REAL-WORLD APPLICATIONS
// ============================================================================

// Example 8.1: Type-Safe Event System
// --------------------------------

type EventMap = {
  click: { x: number; y: number };
  keydown: { key: string };
  focus: { target: HTMLElement };
};

type EventName = keyof EventMap;
type EventHandler<T> = (payload: T) => void;

class EventEmitter<T extends Record<string, any>> {
  handlers: { [K in keyof T]?: EventHandler<T[K]>[] } = {};
  
  on<K extends keyof T>(event: K, handler: EventHandler<T[K]>): void {
    this.handlers[event] = (this.handlers[event] || []).concat(handler);
  }
  
  emit<K extends keyof T>(event: K, payload: T[K]): void {
    this.handlers[event]?.forEach(h => h(payload));
  }
}

// Example 8.2: Type-Safe API Builder
// ------------------------------

type HttpMethod = "GET" | "POST" | "PUT" | "DELETE";
type Route = string;

type RouteHandler<M extends HttpMethod, R extends Route> = (config: {
  method: M;
  route: R;
}) => any;

class ApiBuilder {
  get<R extends Route>(route: R): RouteHandler<"GET", R> {
    return (() => {}) as any;
  }
  
  post<R extends Route>(route: R): RouteHandler<"POST", R> {
    return (() => {}) as any;
  }
}

console.log("\n=== Advanced Type Operations Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/07_Type_Inference_and_Guards");