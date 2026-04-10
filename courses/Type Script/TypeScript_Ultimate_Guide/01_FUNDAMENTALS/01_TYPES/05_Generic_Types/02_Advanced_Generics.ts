/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Generic_Types
 * Purpose: Advanced generic patterns and techniques
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Advanced Generics - Deep Dive
 * =============================
 * 
 * 📚 WHAT: Advanced generic patterns including conditional types, inference
 * 💡 WHY: Essential for creating sophisticated type-level abstractions
 * 🔧 HOW: infer, conditional types, mapped types, template literals
 */

// ============================================================================
// SECTION 1: GENERIC CONSTRAINT PATTERNS
// ============================================================================

// Example 1.1: Multiple Constraints
// ---------------------------

interface HasId {
  id: number;
}

interface HasName {
  name: string;
}

function compareByIdAndName<T extends HasId & HasName>(
  items: T[], 
  id: number, 
  name: string
): T | undefined {
  return items.find(item => item.id === id && item.name === name);
}

// Example 1.2: Key Constraint
// -----------------------

function getProperty<T, K extends keyof T>(obj: T, key: K): T[K] {
  return obj[key];
}

const user = { name: "John", age: 30 };
const name = getProperty(user, "name");
const age = getProperty(user, "age");

// Example 1.3: new() Constraint
// -------------------------

class Factory<T> {
  create(c: new () => T): T {
    return new c();
  }
}

// ============================================================================
// SECTION 2: CONDITIONAL TYPES
// ============================================================================

// Example 2.1: Basic Conditional Type
// ------------------------------

type IsString<T> = T extends string ? true : false;

type TestString = IsString<string>;   // true
type TestNumber = IsString<number>;   // false

// Example 2.2: Extract and Exclude
// -----------------------------

type Colors = "red" | "green" | "blue" | "yellow";

type PrimaryColors = Extract<Colors, "red" | "green" | "blue">;
type NonPrimary = Exclude<Colors, "red" | "green" | "blue">;

// Example 2.3: NonNullable
// ---------------------

type Maybe<T> = T | null | undefined;

type Cleaned = NonNullable<Maybe<string>>; // string
type NotNull<T> = T extends null | undefined ? never : T;

// ============================================================================
// SECTION 3: INFER KEYWORD
// ============================================================================

// Example 3.1: infer in Conditional Types
// ----------------------------------

type ReturnType<T> = T extends (...args: any[]) => infer R ? R : never;

type Func = (x: number, y: number) => string;
type Return = ReturnType<Func>; // string

// Example 3.2: infer in Parameter Types
// ---------------------------------

type Parameters<T> = T extends (...args: infer P) => any ? P : never;

type Params = Parameters<Func>; // [number, number]

// Example 3.3: infer in Array Elements
// ---------------------------------

type ArrayElement<T> = T extends (infer U)[] ? U : never;

type Element = ArrayElement<string[]>; // string

// ============================================================================
// SECTION 4: MAPPED TYPES WITH GENERICS
// ============================================================================

// Example 4.1: Basic Mapped Type
// ---------------------------

type KeysToUppercase<T> = {
  [K in keyof T]: string;
};

interface User {
  name: string;
  age: number;
}

type UppercaseUser = KeysToUppercase<User>;
// { name: string; age: string }

// Example 4.2: Mapped Type with Key Remapping
// ----------------------------------------

interface ApiResponse {
  user_id: number;
  user_name: string;
  user_email: string;
}

type CamelCase<T> = {
  [K in keyof T as K extends `user_${string}` ? K extends `user_${infer R}` ? R : never : K]: T[K];
};

type Mapped = CamelCase<ApiResponse>;
// { id: number; name: string; email: string }

// Example 4.3: Conditional Mapped Types
// ---------------------------------

type MakeOptional<T> = {
  [K in keyof T]?: T[K];
};

type MakeReadonly<T> = {
  readonly [K in keyof T]: T[K];
};

interface Config {
  debug: boolean;
  version: string;
}

type OptionalConfig = MakeOptional<Config>;
type ReadonlyConfig = MakeReadonly<Config>;

// ============================================================================
// SECTION 5: TEMPLATE LITERAL TYPES
// ============================================================================

// Example 5.1: Basic Template Literals
// ---------------------------------

type Greeting = `Hello, ${string}!`;
const greeting: Greeting = "Hello, World!";

type EventName = `on${string}`;
type Handler = (event: any) => void;

type EventHandlers = {
  [K in EventName]?: Handler;
};

// Example 5.2: Union Template Literals
// -------------------------------

type Method = "get" | "post" | "put" | "delete";
type Endpoint = `/${string}`;

type ApiPath = `${Method} ${Endpoint}`;

const path: ApiPath = "get /users";

// Example 5.3: Template Literal with Transformations
// ----------------------------------------------

type SnakeToCamel<S extends string> = 
  S extends `${infer T}_${infer U}` 
    ? `${T}${Capitalize<SnakeToCamel<U>>}` 
    : S;

type Converted = SnakeToCamel<"user_name">; // "userName"

// ============================================================================
// SECTION 6: GENERIC INFERENCE PATTERNS
// ============================================================================

// Example 6.1: Inference with Defaults
// ---------------------------------

interface Container<T = string> {
  value: T;
}

function wrap<T = string>(value: T): Container<T> {
  return { value };
}

const stringContainer = wrap("hello");
const numberContainer = wrap(42);
const explicitContainer = wrap<boolean>(true);

// Example 6.2: Inference from Return Type
// ----------------------------------

function createState<T>(initial: T): {
  value: T;
  setValue: (value: T) => void;
} {
  return {
    value: initial,
    setValue: (value) => { /* ... */ }
  };
}

const state = createState(0);

// ============================================================================
// SECTION 7: CONSTRAINTS WITH 'IS'
// ============================================================================

// Example 7.1: Type Guard Functions
// ---------------------------

interface Fish { swim(): void; }
interface Bird { fly(): void; }

function isFish(pet: Fish | Bird): pet is Fish {
  return "swim" in pet;
}

function move(pet: Fish | Bird): void {
  if (isFish(pet)) {
    pet.swim();
  } else {
    pet.fly();
  }
}

// Example 7.2: Type Predicate with Generics
// -------------------------------------

function isArrayOf<T>(value: unknown): value is T[] {
  return Array.isArray(value) && value.every(item => typeof item === "object");
}

function processData(data: unknown): void {
  if (isArrayOf<object>(data)) {
    console.log(data.length);
  }
}

// ============================================================================
// SECTION 8: ADVANCED PATTERNS
// ============================================================================

// Example 8.1: Generic Mixer
// ----------------------

type Merge<T, U> = {
  [K in keyof T | keyof U]: K extends keyof T ? T[K] : K extends keyof U ? U[K] : never;
};

type Merged = Merge<{ a: 1 }, { b: 2 }>;
// { a: 1; b: 2 }

// Example 8.2: Generic Diff
// ---------------------

type Diff<T, U> = {
  [K in keyof T as K extends keyof U ? never : K]: T[K];
};

type Original = { a: 1; b: 2; c: 3 };
type Changed = { a: 1; b: 10 };

type Differences = Diff<Original, Changed>;
// { c: 3 }

// Example 8.3: Generic Overwrite
// ---------------------------

type Overwrite<T, U> = {
  [K in keyof T]: K extends keyof U ? U[K] : T[K];
};

type Overwritten = Overwrite<Original, Changed>;
// { a: 1; b: 10; c: 3 }

// ============================================================================
// SECTION 9: REAL-WORLD GENERICS
// ============================================================================

// Example 9.1: Generic API Client
// ---------------------------

interface RequestConfig {
  method: string;
  url: string;
  headers?: Record<string, string>;
}

interface Response<T> {
  data: T;
  status: number;
  headers: Record<string, string>;
}

class ApiClient {
  async request<T>(config: RequestConfig): Promise<Response<T>> {
    // Implementation
    return { data: {} as T, status: 200, headers: {} };
  }
}

const client = new ApiClient();
interface User { id: number; name: string; }
const response = await client.request<User>({ 
  method: "GET", 
  url: "/users" 
});

// Example 9.2: Generic Redux-like Store
// ---------------------------------

type Reducer<S, A> = (state: S, action: A) => S;

function createStore<S, A>(reducer: Reducer<S, A>, initialState: S) {
  let state = initialState;
  
  return {
    getState: () => state,
    dispatch: (action: A) => {
      state = reducer(state, action);
    }
  };
}

console.log("\n=== Advanced Generics Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/06_Advanced_Type_Operations");