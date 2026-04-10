/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: Control_Flow
 * Purpose: Control flow statements with TypeScript types
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Control Flow - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Control flow statements and type narrowing
 * 💡 WHY: Essential for building conditional and iterative logic
 * 🔧 HOW: if/else, switch, loops, type guards in flow analysis
 */

// ============================================================================
// SECTION 1: CONDITIONAL STATEMENTS
// ============================================================================

// Example 1.1: if/else with Type Narrowing
// -----------------------------------

function processValue(value: string | number): string {
  if (typeof value === "string") {
    return value.toUpperCase(); // TypeScript knows string
  } else {
    return value.toFixed(2); // TypeScript knows number
  }
}

// Example 1.2: Optional Chaining in Conditionals
// -----------------------------------------

interface User {
  name: string;
  address?: {
    city: string;
    country?: { name: string };
  };
}

function getCityName(user: User): string {
  if (user.address?.country?.name === "USA") {
    return user.address.city;
  }
  return "Unknown";
}

// Example 1.3: Nullish Coalescing
// ---------------------------

function getValue(): string | null {
  return null;
}

const result = getValue() ?? "default";

// ============================================================================
// SECTION 2: SWITCH STATEMENTS
// ============================================================================

// Example 2.1: Discriminated Union Switch
// ------------------------------------

type Shape = 
  | { type: "circle"; radius: number }
  | { type: "rectangle"; width: number; height: number }
  | { type: "triangle"; base: number; height: number };

function getArea(shape: Shape): number {
  switch (shape.type) {
    case "circle":
      return Math.PI * shape.radius ** 2;
    case "rectangle":
      return shape.width * shape.height;
    case "triangle":
      return 0.5 * shape.base * shape.height;
    default:
      // Exhaustiveness check
      const _exhaustive: never = shape;
      return _exhaustive;
  }
}

// Example 2.2: Enum Switch
// ------------------

enum Direction {
  Up, Down, Left, Right
}

function move(dir: Direction): string {
  switch (dir) {
    case Direction.Up: return "Moving up";
    case Direction.Down: return "Moving down";
    case Direction.Left: return "Moving left";
    case Direction.Right: return "Moving right";
  }
}

// ============================================================================
// SECTION 3: LOOPS
// ============================================================================

// Example 3.1: for Loops
// -----------------

function iterateArray<T>(arr: T[]): void {
  for (let i = 0; i < arr.length; i++) {
    console.log(arr[i]);
  }
}

// Example 3.2: for...of with Type Inference
// ------------------------------------

function processItems(items: string[]): void {
  for (const item of items) {
    console.log(item.toUpperCase());
  }
}

// Example 3.3: for...in for Object Keys
// ---------------------------------

function iterateObject(obj: Record<string, number>): void {
  for (const key in obj) {
    console.log(`${key}: ${obj[key]}`);
  }
}

// Example 3.4: forEach
// ----------------

const numbers = [1, 2, 3];
numbers.forEach((n, i) => console.log(i, n));

// ============================================================================
// SECTION 4: TYPE GUARDS IN FLOW
// ============================================================================

// Example 4.1: in Operator Guard
// -------------------------

interface Fish { swim(): void; }
interface Bird { fly(): void; }

function moveAnimal(animal: Fish | Bird): void {
  if ("swim" in animal) {
    animal.swim();
  } else {
    animal.fly();
  }
}

// Example 4.2: Array.isArray Guard
// ---------------------------

function handleArrayOrValue(value: string | string[]): string {
  if (Array.isArray(value)) {
    return value.join(", ");
  }
  return value;
}

// Example 4.3: Custom Type Guard
// --------------------------

interface Dog { bark(): void; }
interface Cat { meow(): void; }

function isDog(pet: Dog | Cat): pet is Dog {
  return "bark" in pet;
}

function sound(pet: Dog | Cat): string {
  if (isDog(pet)) {
    return pet.bark();
  }
  return pet.meow();
}

// ============================================================================
// SECTION 5: EXHAUSTIVENESS CHECKING
// ============================================================================

// Example 5.1: Never Type for Exhaustiveness
// -------------------------------------

type Color = "red" | "green" | "blue";

function getColorName(color: Color): string {
  switch (color) {
    case "red": return "Red";
    case "green": return "Green";
    case "blue": return "Blue";
    default:
      const _exhaustive: never = color;
      return _exhaustive;
  }
}

console.log("\n=== Control Flow Complete ===");
console.log("Next: FUNDAMENTALS/SYNTAX/06_Operators_and_Expressions");