/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Union_and_Intersection_Types
 * Purpose: Understanding intersection types in TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Intersection Types - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Intersection types combine multiple types into one
 * 💡 WHERE: Used for mixing capabilities from different types
 * 🔧 HOW: & operator, type composition, interface merging
 */

// ============================================================================
// SECTION 1: BASIC INTERSECTION TYPES
// ============================================================================

/**
 * Intersection types (&) combine all properties of multiple types.
 */

// Example 1.1: Basic Intersection
// ---------------------------

type Person = {
  name: string;
  age: number;
};

type Employee = {
  employeeId: string;
  department: string;
};

// Intersection: both Person AND Employee
type EmployeePerson = Person & Employee;

const employee: EmployeePerson = {
  name: "John Doe",
  age: 30,
  employeeId: "EMP001",
  department: "Engineering"
};

console.log("Basic Intersection:", JSON.stringify(employee, null, 2));

// Example 1.2: Interface Intersection
// ------------------------------

interface Named {
  name: string;
}

interface Dated {
  createdAt: Date;
}

interface Timestamped extends Named, Dated {
  updatedAt: Date;
}

const timestamped: Timestamped = {
  name: "Item",
  createdAt: new Date(),
  updatedAt: new Date()
};

// ============================================================================
// SECTION 2: INTERSECTION PATTERNS
// ============================================================================

// Example 2.1: Mixin Pattern
// ----------------------

interface Serializable {
  serialize(): string;
}

interface Validatable {
  isValid(): boolean;
}

interface Deserializable {
  deserialize(data: string): void;
}

// Combine capabilities
type Storable = Serializable & Deserializable;
type ValidatedStorable = Serializable & Validatable & Deserializable;

class DataStore implements ValidatedStorable {
  private data: string = "";
  
  serialize(): string {
    return JSON.stringify({ data: this.data });
  }
  
  deserialize(data: string): void {
    this.data = JSON.parse(data).data;
  }
  
  isValid(): boolean {
    return this.data.length > 0;
  }
}

// Example 2.2: Configuration Intersection
// ----------------------------------

type BaseConfig = {
  debug: boolean;
  environment: string;
};

type ApiConfig = {
  baseUrl: string;
  timeout: number;
};

type DatabaseConfig = {
  host: string;
  port: number;
  database: string;
};

// Combine all configs
type AppConfig = BaseConfig & ApiConfig & DatabaseConfig;

const config: AppConfig = {
  debug: true,
  environment: "development",
  baseUrl: "https://api.example.com",
  timeout: 5000,
  host: "localhost",
  port: 5432,
  database: "myapp"
};

// ============================================================================
// SECTION 3: INTERSECTION WITH PRIMITIVES
// ============================================================================

// Example 3.1: Intersection with Literal Types
// -----------------------------------------

type Brand<T, B> = T & { __brand: B };

type UserId = Brand<string, "UserId">;
type ProductId = Brand<string, "ProductId">;

function getUserId(id: string): UserId {
  return id as UserId;
}

function getProductId(id: string): ProductId {
  return id as ProductId;
}

const userId: UserId = getUserId("123");
const productId: ProductId = getProductId("456");

// Example 3.2: Tagged Types
// ---------------------

type Tagged<T, Tag extends string> = T & { __tag: Tag };

type AdminToken = Tagged<string, "admin">;
type UserToken = Tagged<string, "user">;

function validateToken(token: AdminToken | UserToken): boolean {
  if ("__tag" in token) {
    return token.__tag === "admin";
  }
  return false;
}

// ============================================================================
// SECTION 4: INTERSECTION VS UNION
// ============================================================================

// Example 4.1: Understanding the Difference
// -----------------------------------------

// UNION: Can be one OR the other
type Union = { a: string } | { b: number };

// INTERSECTION: Must be both
type Intersection = { a: string } & { b: number };

// Union usage
const union1: Union = { a: "hello" }; // OK - has 'a'
const union2: Union = { b: 42 };      // OK - has 'b'
// const union3: Union = { a: "hello", b: 42 }; // Error - too many properties

// Intersection usage
const intersection1: Intersection = { a: "hello", b: 42 }; // OK - has both
// const intersection2: Intersection = { a: "hello" }; // Error - missing 'b'

console.log("Union vs Intersection:");
console.log("Union can be one of several types");
console.log("Intersection must have all properties from all types");

// Example 4.2: When to Use Which
// ---------------------------

// Use UNION for: different shapes for same concept
type ApiError = { status: number; error: string };
type ApiSuccess = { status: number; data: unknown };
type ApiResponse = ApiError | ApiSuccess;

// Use INTERSECTION for: combining capabilities
interface CanSerialize { serialize(): string; }
interface CanValidate { isValid(): boolean; }
type SerializableAndValidatable = CanSerialize & CanValidate;

// ============================================================================
// SECTION 5: INTERSECTION IN GENERICS
// ============================================================================

// Example 5.1: Constraining Multiple Types
// ------------------------------------

function merge<T extends object, U extends object>(a: T, b: U): T & U {
  return { ...a, ...b };
}

const obj1 = { name: "John" };
const obj2 = { age: 30 };
const merged = merge(obj1, obj2);

console.log("\nGeneric Intersection:", merged);

// Example 5.2: Generic Intersection Constraints
// -----------------------------------------

interface WithId { id: number; }
interface WithName { name: string; }

type Entity = WithId & WithName;

function createEntity<T extends WithId & WithName>(entity: T): T {
  return entity;
}

const entity = createEntity({ id: 1, name: "Test" });

// ============================================================================
// SECTION 6: INTERSECTION PITFALLS
// ============================================================================

/**
 * ⚠️ ISSUE 1: Impossible Intersections
 * -----------------------------------
 */

function fixImpossibleIntersections(): void {
  // This is problematic
  // type Impossible = string & number; // Never - can't be both
  
  // Solution: Use discriminated unions instead
  type Result = { kind: "string"; value: string } | { kind: "number"; value: number };
}

/**
 * ⚠️ ISSUE 2: Intersection with any
 * -------------------------------
 */

function fixAnyIntersection(): void {
  // any & T = T (any absorbs everything)
  // This is dangerous - avoid if possible
  type Dangerous = any & string; // string (basically any)
}

/**
 * ⚠️ ISSUE 3: Intersecting functions
 * -------------------------------
 */

function fixFunctionIntersection(): void {
  // Intersecting function types has interesting behavior
  type F1 = (a: string) => void;
  type F2 = (a: string, b: number) => void;
  
  // Intersection requires BOTH signatures
  type Combined = F1 & F2;
  
  const fn: Combined = (a, b) => {
    console.log(a, b);
  };
  
  fn("test", 42);
}

// ============================================================================
// SECTION 7: PRACTICAL APPLICATIONS
// ============================================================================

// Example 7.1: Extended Error Types
// ------------------------------

interface Error {
  message: string;
}

interface HttpError extends Error {
  statusCode: number;
}

interface ValidationError extends Error {
  field: string;
}

interface NetworkError extends Error {
  retryCount: number;
}

type AppError = Error & (HttpError | ValidationError | NetworkError);

// Example 7.2: Extended User Types
// ---------------------------

interface BaseUser {
  id: number;
  name: string;
  email: string;
}

interface WithTimestamp {
  createdAt: Date;
  updatedAt: Date;
}

interface WithPermissions {
  permissions: string[];
}

type FullUser = BaseUser & WithTimestamp & WithPermissions;

const fullUser: FullUser = {
  id: 1,
  name: "John",
  email: "john@example.com",
  createdAt: new Date(),
  updatedAt: new Date(),
  permissions: ["read", "write"]
};

// Example 7.3: API Response Types
// ---------------------------

interface Pagination {
  page: number;
  pageSize: number;
  total: number;
}

interface ApiResponse<T> {
  data: T;
  status: number;
}

// Combined type
type PaginatedResponse<T> = ApiResponse<T> & Pagination;

const response: PaginatedResponse<User[]> = {
  data: [{ id: 1, name: "John" }],
  status: 200,
  page: 1,
  pageSize: 10,
  total: 100
};

// ============================================================================
// SECTION 8: INTERSECTION WITH MAPPED TYPES
// ============================================================================

// Example 8.1: Combining Mapped Types
// ---------------------------------

type Readonly<T> = {
  readonly [K in keyof T]: T[K];
};

type Optional<T> = {
  [K in keyof T]?: T[K];
};

// Apply both
type ReadonlyOptional<T> = Readonly<Optional<T>>;

interface User {
  name: string;
  age: number;
}

type FrozenOptionalUser = ReadonlyOptional<User>;
// { readonly name?: string; readonly age?: number }

// Example 8.2: Type Composition Helpers
// ---------------------------------

type Getters<T> = {
  [K in keyof T]: () => T[K];
};

type Setters<T> = {
  [K in keyof T]: (value: T[K]) => void;
};

type Accessors<T> = Getters<T> & Setters<T>;

interface UserAccessor extends Accessors<User> {}

console.log("\n=== Intersection Types Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/05_Generic_Types");