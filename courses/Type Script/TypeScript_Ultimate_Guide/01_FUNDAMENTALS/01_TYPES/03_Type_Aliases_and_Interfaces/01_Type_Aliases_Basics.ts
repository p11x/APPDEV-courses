/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Type_Aliases_and_Interfaces
 * Purpose: Understanding type aliases and interfaces in TypeScript
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Type Aliases - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Creating custom type names for complex types
 * 💡 WHY: Improves code readability and reusability
 * 🔧 HOW: Type alias declarations, union types, object types
 */

// ============================================================================
// SECTION 1: BASIC TYPE ALIASES
// ============================================================================

/**
 * Type aliases create new names for existing types.
 * They don't create new types - just create aliases.
 */

// Example 1.1: Simple Type Aliases
// -------------------------------

// Primitive type aliases
type ID = number;
type Name = string;
type Age = number;
type IsActive = boolean;

// Using the aliases
let userId: ID = 1;
let userName: Name = "John Doe";
let userAge: Age = 30;
let active: IsActive = true;

console.log("Simple Type Aliases:");
console.log({ userId, userName, userAge, active });

// Example 1.2: Object Type Aliases
// -----------------------------

// Define object shape with type alias
type User = {
  id: number;
  name: string;
  email: string;
  age?: number;
  isActive: boolean;
};

// Using the object type alias
const user: User = {
  id: 1,
  name: "John Doe",
  email: "john@example.com",
  isActive: true
};

const userWithAge: User = {
  id: 2,
  name: "Jane Doe",
  email: "jane@example.com",
  age: 28,
  isActive: true
};

console.log("\nObject Type Aliases:");
console.log(JSON.stringify(user, null, 2));

// Example 1.3: Function Type Aliases
// -------------------------------

// Function type alias
type Callback = (result: string) => void;
type AsyncCallback<T> = (error: Error | null, result: T | null) => void;
type Predicate<T> = (value: T) => boolean;

// Using function type aliases
function fetchData(callback: Callback): void {
  setTimeout(() => {
    callback("Data fetched successfully");
  }, 1000);
}

console.log("\nFunction Type Aliases:");
fetchData((result) => console.log(result));

// ============================================================================
// SECTION 2: INTERFACES VS TYPE ALIASES
// ============================================================================

/**
 * Both interfaces and type aliases can define object shapes.
 * Use interface for objects that may be extended.
 * Use type aliases for unions, tuples, and functions.
 */

// Example 2.1: Interface for Object Shapes
// ---------------------------------------

interface IUser {
  id: number;
  name: string;
  email: string;
}

// Interface can be extended
interface IAdminUser extends IUser {
  role: "admin" | "superadmin";
  permissions: string[];
}

const admin: IAdminUser = {
  id: 1,
  name: "Admin User",
  email: "admin@example.com",
  role: "admin",
  permissions: ["read", "write", "delete"]
};

console.log("\nInterface Extended:");
console.log(JSON.stringify(admin, null, 2));

// Example 2.2: Type Aliases for Complex Types
// ----------------------------------------

// Union types
type Status = "pending" | "active" | "completed" | "failed";
type Priority = "low" | "medium" | "high" | "critical";

// Tuple type alias
type Point = [number, number];
type RGB = [number, number, number];
type RGBA = [number, number, number, number];

// Using union types
function setPriority(priority: Priority): void {
  console.log(`Priority set to: ${priority}`);
}

// Using tuple types
const color: RGB = [255, 0, 0];
const colorWithAlpha: RGBA = [255, 0, 0, 0.5];

console.log("\nComplex Type Aliases:");
console.log(`Color: ${color}`);
console.log(`Color with alpha: ${colorWithAlpha}`);

// Example 2.3: When to Use Which
// ---------------------------

// Use Interface for:
interface Animal {
  name: string;
  speak(): void;
}

interface Animal {
  age: number; // Declaration merging
}

// Use Type Alias for:
// - Unions
// - Tuples
// - Function types
// - Conditional types

type StringOrNumber = string | number;
type Point3D = [number, number, number];
type EventHandler = (event: Event) => void;

// ============================================================================
// SECTION 3: INTERFACE EXTENSION PATTERNS
// ============================================================================

// Example 3.1: Interface Extending Interface
// ----------------------------------------

interface Person {
  firstName: string;
  lastName: string;
}

interface Employee extends Person {
  employeeId: string;
  department: string;
  salary: number;
}

const employee: Employee = {
  firstName: "John",
  lastName: "Doe",
  employeeId: "EMP001",
  department: "Engineering",
  salary: 75000
};

// Example 3.2: Interface Extending Type Aliases
// -----------------------------------------

type Address = {
  street: string;
  city: string;
  country: string;
};

interface Contact extends Address {
  phone: string;
  email: string;
}

const contact: Contact = {
  street: "123 Main St",
  city: "New York",
  country: "USA",
  phone: "+1-555-0123",
  email: "contact@example.com"
};

// Example 3.3: Multiple Interface Extension
// -------------------------------------

interface Serializable {
  serialize(): string;
}

interface Validatable {
  isValid(): boolean;
}

interface DatabaseRecord extends Serializable, Validatable {
  id: number;
  createdAt: Date;
}

class UserRecord implements DatabaseRecord {
  id: number;
  createdAt: Date;
  name: string;

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
    this.createdAt = new Date();
  }

  serialize(): string {
    return JSON.stringify({ id: this.id, name: this.name });
  }

  isValid(): boolean {
    return this.id > 0 && this.name.length > 0;
  }
}

// ============================================================================
// SECTION 4: INTERFACE DECLARATION MERGING
// ============================================================================

/**
 * Interfaces support declaration merging - same name merges into one.
 * Type aliases do NOT support this.
 */

// Example 4.1: Basic Declaration Merging
// ---------------------------------

interface Logger {
  log(message: string): void;
}

interface Logger {
  warn(message: string): void;
}

interface Logger {
  error(message: string): void;
}

// Merged interface
const logger: Logger = {
  log: (msg) => console.log(`LOG: ${msg}`),
  warn: (msg) => console.warn(`WARN: ${msg}`),
  error: (msg) => console.error(`ERROR: ${msg}`)
};

console.log("\nDeclaration Merging:");
logger.log("Application started");
logger.warn("Low memory");
logger.error("Connection failed");

// Example 4.2: Namespace Merging with Interface
// -----------------------------------------

interface Config {
  mode: string;
}

interface Config {
  debug: boolean;
}

const config: Config = {
  mode: "production",
  debug: false
};

// ============================================================================
// SECTION 5: TYPE ALIASES WITH GENERICS
// ============================================================================

// Example 5.1: Generic Type Aliases
// ------------------------------

type Container<T> = {
  value: T;
  getValue(): T;
  setValue(value: T): void;
};

const stringContainer: Container<string> = {
  value: "Hello",
  getValue() { return this.value; },
  setValue(val) { this.value = val; }
};

const numberContainer: Container<number> = {
  value: 42,
  getValue() { return this.value; },
  setValue(val) { this.value = val; }
};

console.log("\nGeneric Type Aliases:");
console.log(stringContainer.getValue());
console.log(numberContainer.getValue());

// Example 5.2: Conditional Type Aliases
// -------------------------------

type NonNullable<T> = T extends null | undefined ? never : T;

// Usage
type CleanString = NonNullable<string>; // string
type CleanNull = NonNullable<null>; // never
type CleanUnion = NonNullable<string | null | undefined>; // string

// Example 5.3: Mapped Type Aliases
// ---------------------------

type Readonly<T> = {
  readonly [P in keyof T]: T[P];
};

type Nullable<T> = {
  [P in keyof T]?: T[P] | null;
};

interface UserProfile {
  name: string;
  age: number;
  email: string;
}

type ReadonlyUser = Readonly<UserProfile>;
type NullableUser = Nullable<UserProfile>;

const readonlyUser: ReadonlyUser = {
  name: "John",
  age: 30,
  email: "john@example.com"
};

// readonlyUser.name = "Jane"; // Error - readonly

// ============================================================================
// SECTION 6: PRACTICAL PATTERNS
// ============================================================================

// Example 6.1: Response Types
// -----------------------

interface ApiResponse<T> {
  data: T;
  status: number;
  message: string;
  timestamp: Date;
}

type UserResponse = ApiResponse<User>;
type ErrorResponse = ApiResponse<{ error: string }>;

function handleResponse<T>(response: ApiResponse<T>): void {
  console.log(`Status: ${response.status}`);
  console.log(`Message: ${response.message}`);
  if (response.status >= 200 && response.status < 300) {
    console.log("Success:", response.data);
  }
}

// Example 6.2: Event Types
// --------------------

type MouseEventHandler = (event: MouseEvent) => void;
type KeyboardEventHandler = (event: KeyboardEvent) => void;
type FocusEventHandler = (event: FocusEvent) => void;

interface EventMap {
  click: MouseEventHandler;
  keydown: KeyboardEventHandler;
  focus: FocusEventHandler;
}

// Example 6.3: State Machines
// ------------------------

type State = "idle" | "loading" | "success" | "error";

interface Transition {
  from: State;
  to: State;
  action: () => void;
}

const transitions: Transition[] = [
  { from: "idle", to: "loading", action: () => console.log("Loading...") },
  { from: "loading", to: "success", action: () => console.log("Success!") },
  { from: "loading", to: "error", action: () => console.log("Error!") },
  { from: "success", to: "idle", action: () => console.log("Reset") },
  { from: "error", to: "idle", action: () => console.log("Retry") }
];

// ============================================================================
// SECTION 7: COMMON PITFALLS
// ============================================================================

/**
 * ⚠️ COMMON ISSUE 1: Type alias circular reference
 * -----------------------------------------------
 */

function fixCircularReference(): void {
  // This causes an error
  // type Tree = Tree[]; // Error
  
  // Instead, use an interface
  interface TreeNode {
    value: string;
    children: TreeNode[];
  }
}

/**
 * ⚠️ COMMON ISSUE 2: Interface vs Type for function types
 * -----------------------------------------------------
 * Both work, but type alias is more common for functions
 */

type FunctionType = (a: number, b: number) => number;
interface FunctionInterface {
  (a: number, b: number): number;
}

/**
 * ⚠️ COMMON ISSUE 3: Not using readonly appropriately
 * ---------------------------------------------------
 */

function fixReadonly(): void {
  // Use readonly for parameters that shouldn't be modified
  function processUser(user: Readonly<User>): void {
    // user.name = "Jane"; // Error
    console.log(user.name); // OK - can read
  }
}

console.log("\n=== Type Aliases Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/04_Union_and_Intersection_Types");