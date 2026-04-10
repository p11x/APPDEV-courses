/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 08_Advanced_Type_System
 * Topic: 04_Type_Narrowing
 * Purpose: Compile-time type refinement through control flow analysis
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Reduces runtime type errors
 */

/**
 * Type Narrowing - Compile-Time Type Refinement
 * =============================================
 * 
 * 📚 WHAT: Process of refining types from broad to specific within conditional branches
 * 💡 WHY: Enables type-safe handling of union types and complex types
 * 🔧 HOW: Control flow analysis, type guards, and assertions
 */

// ============================================================================
// SECTION 1: BASIC TYPE NARROWING
// ============================================================================

// Example 1.1: typeof narrowing
function processValue(value: string | number): string {
  if (typeof value === "string") {
    return value.toUpperCase();
  }
  return value.toFixed(2);
}

// Example 1.2: instanceof narrowing
class Dog {
  bark(): void { console.log("Woof!"); }
}

class Cat {
  meow(): void { console.log("Meow!"); }
}

function makeSound(animal: Dog | Cat): void {
  if (animal instanceof Dog) {
    animal.bark();
  } else {
    animal.meow();
  }
}

// Example 1.3: Truthiness narrowing
function printLength(str: string | null | undefined): void {
  if (str) {
    console.log(str.length);
  } else {
    console.log("No string provided");
  }
}

// ============================================================================
// SECTION 2: DISCRIMINANT PROPERTY NARROWING
// ============================================================================

// Example 2.1: Union with discriminant property
interface Bird {
  kind: "bird";
  canFly: boolean;
}

interface Fish {
  kind: "fish";
  canSwim: boolean;
}

type Animal = Bird | Fish;

function describeAnimal(animal: Animal): void {
  if (animal.kind === "bird") {
    console.log(`Bird that can fly: ${animal.canFly}`);
  } else {
    console.log(`Fish that can swim: ${animal.canSwim}`);
  }
}

// Example 2.2: Multiple discriminant properties
type Shape = 
  | { type: "circle"; radius: number }
  | { type: "rectangle"; width: number; height: number };

function getArea(shape: Shape): number {
  if (shape.type === "circle") {
    return Math.PI * shape.radius ** 2;
  }
  return shape.width * shape.height;
}

// ============================================================================
// SECTION 3: IN CLAUSE NARROWING
// ============================================================================

// Example 3.1: Using 'in' operator
interface A {
  a: string;
}

interface B {
  b: number;
}

function processUnion(value: A | B): void {
  if ("a" in value) {
    console.log(value.a);
  } else {
    console.log(value.b);
  }
}

// Example 3.2: Checking optional properties
interface Config {
  required: string;
  optional?: number;
}

function processConfig(config: Config): void {
  if ("optional" in config) {
    console.log(config.optional * 2);
  } else {
    console.log("No optional value");
  }
}

// ============================================================================
// SECTION 4: CUSTOM TYPE GUARDS
// ============================================================================

// Example 4.1: Simple type predicate
function isString(value: unknown): value is string {
  return typeof value === "string";
}

function process(value: unknown): void {
  if (isString(value)) {
    console.log(value.toUpperCase());
  }
}

// Example 4.2: Class type guard
class Animal {
  constructor(public name: string) {}
}

class Dog extends Animal {
  breed: string = "Unknown";
}

class Cat extends Animal {
  color: string = "Unknown";
}

function isDog(animal: Animal): animal is Dog {
  return animal instanceof Dog;
}

function processAnimal(animal: Animal): void {
  if (isDog(animal)) {
    console.log(animal.breed);
  }
}

// Example 4.3: Interface type guard
interface Admin {
  role: "admin";
  permissions: string[];
}

interface User {
  role: "user";
  username: string;
}

function isAdmin(person: Admin | User): person is Admin {
  return person.role === "admin";
}

// ============================================================================
// SECTION 5: NARROWING WITH PREDICATES
// ============================================================================

// Example 5.1: Array type narrowing
function processArray(arr: string[] | number[]): void {
  if (arr.length > 0) {
    const first = arr[0];
    if (typeof first === "string") {
      console.log((arr as string[]).join(", "));
    } else {
      console.log((arr as number[]).reduce((a, b) => a + b, 0));
    }
  }
}

// Example 5.2: Map type narrowing
type StringMap = Map<string, string>;
type NumberMap = Map<string, number>;

function processMap(map: StringMap | NumberMap): void {
  for (const [key, value] of map.entries()) {
    if (typeof value === "string") {
      console.log(`${key}: ${value}`);
    } else {
      console.log(`${key}: ${value * 2}`);
    }
  }
}

// ============================================================================
// SECTION 6: NEVER TYPE AND EXHAUSTIVE CHECKING
// ============================================================================

// Example 6.1: Exhaustive switch with never
type Color = "red" | "green" | "blue";

function getColorCode(color: Color): string {
  switch (color) {
    case "red":
      return "#FF0000";
    case "green":
      return "#00FF00";
    case "blue":
      return "#0000FF";
    default:
      const exhaustive: never = color;
      return exhaustive;
  }
}

// Example 6.2: Assertion function for exhaustive checking
function assertExhaustive(value: never): never {
  throw new Error(`Unhandled case: ${value}`);
}

function handleStatus(status: "pending" | "active" | "completed"): void {
  switch (status) {
    case "pending":
      console.log("Pending");
      break;
    case "active":
      console.log("Active");
      break;
    case "completed":
      console.log("Completed");
      break;
    default:
      assertExhaustive(status);
  }
}

// ============================================================================
// SECTION 7: ADVANCED NARROWING PATTERNS
// ============================================================================

// Example 7.1: Keyof narrowing
function getProperty<T, K extends keyof T>(obj: T, key: K): T[K] {
  return obj[key];
}

// Example 7.2: Conditional type narrowing
type NonNull<T> = T extends null | undefined ? never : T;

function processNonNull<T>(value: T | null | undefined): void {
  const nonNull = value as NonNull<typeof value>;
  if (nonNull !== undefined) {
    console.log(nonNull);
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Type narrowing is entirely compile-time. There is no runtime
 * performance impact. The compiled JavaScript will contain the full union
 * handling code but type guards are not executed at runtime.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: All narrowing techniques work with TypeScript 2.0+.
 * Control flow analysis for const was improved in TypeScript 4.6.
 * Custom type guards work in all versions.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Proper type narrowing prevents runtime errors that could lead
 * to security vulnerabilities. Exhaustive checking ensures all cases are
 * handled, preventing undefined behavior.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test each code path for each union member. Verify type guards
 * work correctly with edge cases. Check that exhaustive checking catches
 * missing cases.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use 'typeof' to inspect values at runtime. Enable strictNullChecks
 * to catch potential null/undefined issues. Use TypeScript's hover to see
 * narrowed types in IDE.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Discriminated unions: More explicit narrowing
 * - Type assertions: Less safe, more verbose
 * - Custom guard functions: More flexible
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 03_Discriminated_Unions.ts: Pattern matching with unions
 * - 08_Type_Predicates_Advanced.ts: Advanced type predicates
 * - 06_Mapped_Type_Constraints.ts: Constraining mapped types
 */

console.log("=== Type Narrowing Examples Complete ===");
