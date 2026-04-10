/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 04_Type_Libraries
 * Concept: 02_Type_Creation
 * Topic: 03_Type_Guards
 * Purpose: Learn type guard functions for runtime type checking
 * Difficulty: intermediate
 * UseCase: type-safe-code
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Runtime type checking overhead
 * Security: Validate guard inputs
 */

/**
 * WHAT: Type guards are functions that narrow types at runtime, enabling
 * TypeScript to make more accurate type inferences.
 */

type TypeGuard<T> = (value: unknown) => value is T;

function isString(value: unknown): value is string {
  return typeof value === "string";
}

function isNumber(value: unknown): value is number {
  return typeof value === "number" && !isNaN(value);
}

function isBoolean(value: unknown): value is boolean {
  return typeof value === "boolean";
}

function isArray<T>(value: unknown): value is T[] {
  return Array.isArray(value);
}

function isObject(value: unknown): value is Record<string, any> {
  return typeof value === "object" && value !== null && !Array.isArray(value);
}

function isNull(value: unknown): value is null {
  return value === null;
}

function isUndefined(value: unknown): value is undefined {
  return value === undefined;
}

interface User {
  id: number;
  name: string;
  email: string;
}

function isUser(value: unknown): value is User {
  return isObject(value) && "id" in value && "name" in value && "email" in value;
}

interface Admin extends User {
  role: "admin";
}

function isAdmin(value: unknown): value is Admin {
  return isUser(value) && value.role === "admin";
}

type Shape = { kind: "circle"; radius: number } | { kind: "square"; side: number };

function isCircle(shape: Shape): shape is { kind: "circle"; radius: number } {
  return shape.kind === "circle";
}

function isSquare(shape: Shape): shape is { kind: "square"; side: number } {
  return shape.kind === "square";
}

function processShape(shape: Shape): number {
  if (isCircle(shape)) {
    return Math.PI * shape.radius ** 2;
  }
  if (isSquare(shape)) {
    return shape.side ** 2;
  }
  return 0;
}

type DiscriminatedUnion<T, K extends keyof T> = T[K] extends string ? { type: T[K]; value: T } : never;

function isOfType<T extends Record<string, unknown>, K extends keyof T>(
  value: T, 
  key: K, 
  type: string
): value is T {
  return typeof value[key] === type;
}

function assertString(value: unknown, name: string): asserts value is string {
  if (typeof value !== "string") {
    throw new Error(`${name} must be a string`);
  }
}

function assertNotNull<T>(value: T, message = "Value cannot be null"): asserts value is NonNullable<T> {
  if (value === null || value === undefined) {
    throw new Error(message);
  }
}

function processData(data: unknown): string {
  if (isString(data)) {
    return data.toUpperCase();
  }
  if (isArray(data)) {
    return data.join(", ");
  }
  return "unknown";
}

const shapes: Shape[] = [
  { kind: "circle", radius: 5 },
  { kind: "square", side: 4 },
  { kind: "circle", radius: 10 }
];

console.log("\n=== Type Guards Demo ===");
console.log("Circle area:", processShape(shapes[0]));
console.log("Square area:", processShape(shapes[1]));
console.log("Process data:", processData("hello"));
console.log("Process array:", processData([1, 2, 3]));

/**
 * PERFORMANCE:
 * - Type guards add runtime overhead
 * - Use specific guards to avoid unnecessary checks
 * 
 * COMPATIBILITY:
 * - Works with all TypeScript targets
 * - Can be used with type predicates
 * 
 * SECURITY:
 * - Validate inputs before narrowing
 * - Be careful with any type assertions
 * 
 * CROSS-REFERENCE:
 * - 01_Factory_Functions.ts - Factory functions
 * - 02_Type_Builders.ts - Type builders
 */