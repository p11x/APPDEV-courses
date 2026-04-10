/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Type_Aliases_and_Interfaces
 * Purpose: Advanced interface and type alias patterns
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Advanced Interface Patterns
 * ============================
 * 
 * 📚 WHAT: Complex interface patterns and advanced type techniques
 * 💡 WHY: Essential for enterprise-level TypeScript development
 * 🔧 HOW: Advanced generics, conditional types, pattern matching
 */

// ============================================================================
// SECTION 1: INTERFACE INDEX SIGNATURES
// ============================================================================

/**
 * Index signatures allow dynamic property access.
 */

// Example 1.1: Basic Index Signature
// -------------------------------

interface StringDictionary {
  [key: string]: string;
}

const dict: StringDictionary = {
  name: "John",
  city: "New York",
  country: "USA"
};

console.log("Index Signature:", dict["name"]);

// Example 1.2: Multiple Index Types
// ------------------------------

interface FlexibleObject {
  [key: string]: string | number | boolean;
}

const flexible: FlexibleObject = {
  name: "John",
  age: 30,
  active: true
};

// Example 1.3: Numeric Index Signatures
// ---------------------------------

interface StringArray {
  [index: number]: string;
}

const array: StringArray = ["a", "b", "c"];
console.log("Numeric Index:", array[0]);

// ============================================================================
// SECTION 2: INTERFACE EXTENSIBILITY PATTERNS
// ============================================================================

// Example 2.1: Extending Multiple Interfaces
// -----------------------------------------

interface Base {
  id: number;
  createdAt: Date;
}

interface Timestamped extends Base {
  updatedAt: Date;
}

interface Authored extends Timestamped {
  author: string;
}

const entity: Authored = {
  id: 1,
  createdAt: new Date(),
  updatedAt: new Date(),
  author: "John"
};

// Example 2.2: Interface Composition
// -------------------------------

interface Named {
  name: string;
}

interface Dated {
  created: Date;
}

interface Described {
  description: string;
}

// Compose interfaces using intersection
type Entity = Named & Dated & Described;

const composed: Entity = {
  name: "My Entity",
  created: new Date(),
  description: "A sample entity"
};

// ============================================================================
// SECTION 3: INTERFACE CALL SIGNATURES
// ============================================================================

// Example 3.1: Callable Interfaces
// -------------------------------

interface Counter {
  (start: number): string;
  increment(): void;
  reset(): void;
}

function createCounter(start: number): Counter {
  let count = start;
  
  const counter = ((initial: number) => {
    return `Count: ${count}`;
  }) as Counter;
  
  counter.increment = () => count++;
  counter.reset = () => count = start;
  
  return counter;
}

const counter = createCounter(10);
console.log(counter(0));
counter.increment();
console.log(counter(0));

// Example 3.2: Constructor Signatures
// -------------------------------

interface Constructor<T> {
  new (...args: any[]): T;
}

class Person {
  constructor(public name: string) {}
}

const PersonConstructor: Constructor<Person> = Person;
const person = new PersonConstructor("John");

// ============================================================================
// SECTION 4: INTERFACE INDEX TYPES
// ============================================================================

// Example 4.1: keyof Operator
// -----------------------

interface User {
  id: number;
  name: string;
  email: string;
}

type UserKeys = keyof User; // "id" | "name" | "email"

function getProperty<T, K extends keyof T>(obj: T, key: K): T[K] {
  return obj[key];
}

const user: User = { id: 1, name: "John", email: "john@test.com" };
console.log("keyof Usage:", getProperty(user, "name"));

// Example 4.2: Indexed Access Types
// -----------------------------

interface Product {
  category: {
    id: number;
    name: string;
  };
}

type ProductCategory = Product["category"]; // { id: number; name: string }
type CategoryName = Product["category"]["name"]; // string

// ============================================================================
// SECTION 5: MAPPED TYPES WITH INTERFACES
// ============================================================================

// Example 5.1: Basic Mapped Types
// ---------------------------

interface Settings {
  theme: string;
  notifications: boolean;
  language: string;
}

type ReadonlySettings = {
  readonly [K in keyof Settings]: Settings[K];
};

type OptionalSettings = {
  [K in keyof Settings]?: Settings[K];
};

// Example 5.2: Mapped Types with Modifiers
// ------------------------------------

type Nullable<T> = {
  [K in keyof T]?: T[K] | null;
};

type PropertyHandlers<T> = {
  [K in keyof T]?: (value: T[K]) => void;
};

const handlers: PropertyHandlers<User> = {
  name: (value) => console.log(`Name changed to ${value}`),
  email: (value) => console.log(`Email changed to ${value}`)
};

// ============================================================================
// SECTION 6: TEMPLATE LITERAL TYPES
// ============================================================================

// Example 6.1: Basic Template Literal Types
// -------------------------------------

type Name = `Mr. ${string}` | `Mrs. ${string}` | `Ms. ${string}`;

const prefixedName: Name = "Mr. Smith";
// const invalid: Name = "Dr. Smith"; // Error

// Example 6.2: Event Handler Types
// ----------------------------

type EventName = `on${string}`;
type Handler = (event: any) => void;

type EventHandlers = {
  [K in EventName]?: Handler;
};

// Example 6.3: CSS Property Types
// ---------------------------

type CssProperty = `--${string}`;
const customProp: CssProperty = "--primary-color";

// ============================================================================
// SECTION 7: CONDITIONAL TYPES WITH INTERFACES
// ============================================================================

// Example 7.1: Basic Conditional Types
// ---------------------------------

type IsString<T> = T extends string ? true : false;
type Test1 = IsString<string>; // true
type Test2 = IsString<number>; // false

// Example 7.2: Conditional Type Mappings
// ----------------------------------

type NonNullable<T> = T extends null | undefined ? never : T;
type Cleaned = NonNullable<string | null | undefined>; // string

// Example 7.3: Extract and Exclude
// -----------------------------

type Extract<T, U> = T extends U ? T : never;
type Exclude<T, U> = T extends U ? never : T;

type Colors = "red" | "green" | "blue" | "yellow";
type PrimaryColors = Extract<Colors, "red" | "blue">; // "red" | "blue"
type NonPrimary = Exclude<Colors, "red" | "blue">; // "green" | "yellow"

// ============================================================================
// SECTION 8: PATTERN MATCHING WITH TYPE GUARDS
// ============================================================================

// Example 8.1: Custom Type Guards
// ---------------------------

interface Fish {
  swim(): void;
}

interface Bird {
  fly(): void;
}

function isFish(pet: Fish | Bird): pet is Fish {
  return (pet as Fish).swim !== undefined;
}

function move(pet: Fish | Bird): void {
  if (isFish(pet)) {
    pet.swim();
  } else {
    pet.fly();
  }
}

// Example 8.2: in Operator Type Guard
// -------------------------------

interface Mouse {
  click(): void;
  move(): void;
}

interface Keyboard {
  type(): void;
}

function interact(device: Mouse | Keyboard): void {
  if ("click" in device) {
    device.click();
  } else {
    device.type();
  }
}

// ============================================================================
// SECTION 9: GENERIC INTERFACES
// ============================================================================

// Example 9.1: Generic Repository Interface
// -------------------------------------

interface Repository<T> {
  findById(id: number): Promise<T | null>;
  findAll(): Promise<T[]>;
  save(entity: T): Promise<T>;
  delete(id: number): Promise<boolean>;
}

interface UserEntity {
  id: number;
  name: string;
  email: string;
}

const userRepo: Repository<UserEntity> = {
  async findById(id) { return null; },
  async findAll() { return []; },
  async save(entity) { return entity; },
  async delete(id) { return true; }
};

// Example 9.2: Generic Factory Interface
// ---------------------------------

interface Factory<T> {
  create(): T;
  createMany(count: number): T[];
}

class UserFactory implements Factory<User> {
  create(): User {
    return { id: 0, name: "", email: "" };
  }
  
  createMany(count: number): User[] {
    return Array(count).fill(this.create());
  }
}

// ============================================================================
// SECTION 10: REAL-WORLD INTERFACE PATTERNS
// ============================================================================

// Example 10.1: API Client Interface
// --------------------------------

interface ApiClient {
  get<T>(url: string): Promise<T>;
  post<T>(url: string, data: any): Promise<T>;
  put<T>(url: string, data: any): Promise<T>;
  delete<T>(url: string): Promise<T>;
}

interface ApiClientConfig {
  baseUrl: string;
  timeout: number;
  headers: Record<string, string>;
}

interface ApiClientFactory {
  create(config: ApiClientConfig): ApiClient;
}

// Example 10.2: Component Props Interface
// -----------------------------------

interface BaseProps {
  className?: string;
  style?: React.CSSProperties;
  children?: React.ReactNode;
}

interface ButtonProps extends BaseProps {
  variant: "primary" | "secondary" | "danger";
  size: "small" | "medium" | "large";
  onClick: () => void;
  disabled?: boolean;
}

console.log("\n=== Advanced Interface Patterns Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/04_Union_and_Intersection_Types");