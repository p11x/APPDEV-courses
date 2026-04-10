/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 08_Advanced_Type_System
 * Topic: 08_Type_Predicates_Advanced
 * Purpose: Custom type predicates for complex type narrowing
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe type guards
 */

/**
 * Advanced Type Predicates - Custom Type Guards
 * ==============================================
 * 
 * 📚 WHAT: Functions that return type predicates for runtime type checking
 * 💡 WHY: Enables precise type narrowing beyond built-in mechanisms
 * 🔧 HOW: Return type `x is T` for type guard functions
 */

// ============================================================================
// SECTION 1: BASIC TYPE PREDICATES
// ============================================================================

// Example 1.1: Simple type predicate
function isString(value: unknown): value is string {
  return typeof value === "string";
}

function process(value: unknown): void {
  if (isString(value)) {
    console.log(value.toUpperCase());
  }
}

// Example 1.2: Type predicate with custom type
interface Admin {
  role: "admin";
  permissions: string[];
}

interface User {
  role: "user";
  username: string;
}

type Person = Admin | User;

function isAdmin(person: Person): person is Admin {
  return person.role === "admin";
}

function processPerson(person: Person): void {
  if (isAdmin(person)) {
    console.log(person.permissions.join(", "));
  } else {
    console.log(person.username);
  }
}

// ============================================================================
// SECTION 2: GENERIC TYPE PREDICATES
// ============================================================================

// Example 2.1: Generic type guard
function isArrayOf<T>(value: unknown, isType: (item: unknown) => item is T): value is T[] {
  return Array.isArray(value) && value.every(isType);
}

function isString2(item: unknown): item is string {
  return typeof item === "string";
}

const result = isArrayOf<string>(["a", "b", "c"], isString2);

// Example 2.2: Generic predicate with constructor
function isInstanceOf<T>(constructor: new (...args: unknown[]) => T) {
  return (value: unknown): value is T => value instanceof constructor;
}

const isError = isInstanceOf(Error);
const isDate = isInstanceOf(Date);

const maybeError: unknown = new Error();
const maybeDate: unknown = new Date();

if (isError(maybeError)) {
  console.log(maybeError.message);
}

if (isDate(maybeDate)) {
  console.log(maybeDate.toISOString());
}

// ============================================================================
// SECTION 3: COMPOUND TYPE PREDICATES
// ============================================================================

// Example 3.1: Chaining predicates
function isNonNull<T>(value: T | null | undefined): value is T {
  return value !== null && value !== undefined;
}

function isString3(value: unknown): value is string {
  return typeof value === "string";
}

function isNonEmptyString(value: unknown): value is string {
  return isNonNull(value) && isString3(value) && value.length > 0;
}

// Example 3.2: Combining multiple type checks
interface Validatable {
  validate(): boolean;
}

function isValidatable(value: unknown): value is Validatable {
  return typeof value === "object" && value !== null && "validate" in value;
}

function processValidatable(value: unknown): void {
  if (isValidatable(value)) {
    console.log(value.validate());
  }
}

// ============================================================================
// SECTION 4: DISCRIMINANT PREDICATES
// ============================================================================

// Example 4.1: Discriminant property checking
type Response<T, E = Error> = 
  | { ok: true; data: T }
  | { ok: false; error: E };

function isSuccessful<T, E>(response: Response<T, E>): response is { ok: true; data: T } {
  return response.ok;
}

function handleResponse<T, E>(response: Response<T, E>): void {
  if (isSuccessful(response)) {
    console.log(response.data);
  } else {
    console.error(response.error);
  }
}

// Example 4.2: Complex discriminant
type Event = 
  | { type: "click"; x: number; y: number }
  | { type: "keypress"; key: string }
  | { type: "scroll"; position: number };

function isClickEvent(event: Event): event is { type: "click"; x: number; y: number } {
  return event.type === "click";
}

function handleEvent(event: Event): void {
  if (isClickEvent(event)) {
    console.log(`Clicked at ${event.x}, ${event.y}`);
  }
}

// ============================================================================
// SECTION 5: NESTED TYPE PREDICATES
// ============================================================================

// Example 5.1: Nested object checking
interface DeepObject {
  [key: string]: unknown;
}

function hasProperty<T extends DeepObject, K extends string>(
  obj: T, 
  key: K
): obj is T & { [P in K]: unknown } {
  return key in obj;
}

function deepGet<T extends DeepObject, K extends string>(
  obj: T, 
  key: K
): unknown {
  if (hasProperty(obj, key)) {
    return obj[key];
  }
  return undefined;
}

// Example 5.2: Nested type guard
type TreeNode<T> = {
  value: T;
  children: TreeNode<T>[];
};

function isLeafNode<T>(node: TreeNode<T>): node is { value: T; children: [] } {
  return node.children.length === 0;
}

function processTreeNode<T>(node: TreeNode<T>): void {
  if (isLeafNode(node)) {
    console.log(`Leaf: ${node.value}`);
  } else {
    node.children.forEach(processTreeNode);
  }
}

// ============================================================================
// SECTION 6: TYPE PREDICATES IN CLASSES
// ============================================================================

// Example 6.1: Class method as type guard
class TypeGuard {
  static isString(value: unknown): value is string {
    return typeof value === "number";
  }
}

if (TypeGuard.isString("test")) {
  console.log("is string");
}

// Example 6.6.2: Instance method type guard
class Validator {
  private data: unknown;

  constructor(data: unknown) {
    this.data = data;
  }

  isString(): this is { data: string } {
    return typeof this.data === "string";
  }

  isNumber(): this is { data: number } {
    return typeof this.data === "number";
  }
}

const validator = new Validator("hello");
if (validator.isString()) {
  console.log(validator.data.toUpperCase());
}

// ============================================================================
// SECTION 7: ADVANCED PATTERNS
// ============================================================================

// Example 7.1: Predicate with brand types
type Brand<T, B extends string> = T & { __brand: B };

type UserId = Brand<string, "UserId">;
type PostId = Brand<string, "PostId">;

function isUserId(value: string): value is UserId {
  return value.startsWith("user_");
}

function isPostId(value: string): value is PostId {
  return value.startsWith("post_");
}

function processId(id: string): void {
  if (isUserId(id)) {
    console.log(`User: ${id}`);
  } else if (isPostId(id)) {
    console.log(`Post: ${id}`);
  }
}

// Example 7.2: Predicate factory
function createTypeGuard<T>(validator: (value: unknown) => boolean) {
  return (value: unknown): value is T => validator(value);
}

const isEmail = createTypeGuard<string>(
  (v): v is string => typeof v === "string" && v.includes("@")
);

const isPositiveNumber = createTypeGuard<number>(
  (v): v is number => typeof v === "number" && v > 0
);

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Type predicates are compile-time constructs. They compile
 * to regular functions that return booleans. No runtime overhead beyond
 * the predicate function itself.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Type predicates (`value is T`) require TypeScript 1.6+.
 * Improved type narrowing with predicates was enhanced in TypeScript 2.7+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Type predicates help prevent runtime errors. Use branded types
 * to prevent type confusion attacks. Validate all external input with
 * type guards before processing.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test each branch of the type predicate. Verify that predicates
 * correctly identify all expected types. Test edge cases with null, undefined,
 * and unexpected types.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use console.log in predicate functions for debugging. Check
 * TypeScript's type inference by hovering over variables after the guard.
 * Verify narrowing by intentionally breaking the predicate.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Discriminated unions: More explicit but requires common property
 * - Type assertions: Less safe, more verbose
 * - Runtime validation libraries: More flexible validation
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 04_Type_Narrowing.ts: Built-in narrowing techniques
 * - 03_Discriminated_Unions.ts: Discriminant pattern matching
 * - 02_Advanced_Type_Guards.ts: Basic type guards
 */

console.log("=== Advanced Type Predicates Complete ===");
