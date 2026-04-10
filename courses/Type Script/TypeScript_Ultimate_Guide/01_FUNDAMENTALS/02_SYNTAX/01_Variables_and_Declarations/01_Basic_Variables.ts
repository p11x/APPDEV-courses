/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: Variables_and_Declarations
 * Purpose: Understanding TypeScript variable declarations and types
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Variables and Declarations - Comprehensive Guide
 * ==================================================
 * 
 * 📚 WHAT: Variable declarations, scoping, and type annotations
 * 💡 WHY: Foundation for all TypeScript code
 * 🔧 HOW: let, const, var, type annotations, type inference
 */

// ============================================================================
// SECTION 1: VARIABLE DECLARATIONS
// ============================================================================

// Example 1.1: let Declarations
// ------------------------

// Basic let with type annotation
let count: number = 0;
let name: string = "John";
let isActive: boolean = true;

// Type annotation can be inferred
let inferred = "hello"; // TypeScript infers string

// Multiple declarations
let x: number = 1, y: number = 2, z: number = 3;

// Example 1.2: const Declarations
// ---------------------------

// const for values that won't be reassigned
const PI: number = 3.14159;
const APP_NAME: string = "MyApp";

// const with objects - reference is constant, not properties
const user = { name: "John", age: 30 };
user.age = 31; // OK - modifying property
// user = { name: "Jane" }; // Error - can't reassign

// const with arrays
const numbers: readonly number[] = [1, 2, 3];
// numbers.push(4); // Error - array is readonly

// Example 1.3: var Declarations (Avoid)
// --------------------------------

// var has function scope, not block scope - avoid in modern TypeScript
var oldStyle = "legacy";
var issues = "hoisting and function scope problems";

// Use let/const instead

// ============================================================================
// SECTION 2: TYPE ANNOTATIONS
// ============================================================================

// Example 2.1: Primitive Type Annotations
// ------------------------------------

let stringVar: string = "hello";
let numberVar: number = 42;
let booleanVar: boolean = true;
let nullVar: null = null;
let undefinedVar: undefined = undefined;

// Example 2.2: Array Type Annotations
// -------------------------------

let numberArray: number[] = [1, 2, 3];
let stringArray: Array<string> = ["a", "b", "c"];
let mixedArray: (string | number)[] = [1, "two", 3];

// Example 2.3: Object Type Annotations
// -------------------------------

// Inline object type
let point: { x: number; y: number } = { x: 1, y: 2 };

// Optional properties
let user: { name: string; age?: number } = { name: "John" };

// Readonly properties
let config: { readonly id: string; value: number } = { id: "1", value: 10 };
// config.id = "2"; // Error - readonly

// Example 2.4: Function Type Annotations
// ---------------------------------

let callback: (result: string) => void = (result) => console.log(result);
let mapper: (item: number) => number = (item) => item * 2;

// ============================================================================
// SECTION 3: SCOPING
// ============================================================================

// Example 3.1: Block Scope
// --------------------

function blockScope(): void {
  if (true) {
    let blockVar = "inside block";
    const blockConst = "also inside";
    console.log(blockVar);
  }
  // console.log(blockVar); // Error - not in scope

// Example 3.2: Function Scope
// -----------------------

function functionScope(): void {
  var functionVar = "function scope"; // Avoid
  let letVar = "block scope";
  const constVar = "block scope";
}

function nestedScopes(): void {
  let x = 1;
  {
    let x = 2; // Different variable - shadowing
    console.log(x); // 2
  }
  console.log(x); // 1

// ============================================================================
// SECTION 4: TYPE INFERENCE IN VARIABLES
// ============================================================================

// Example 4.1: Inference from Value
// -----------------------------

let inferredString = "hello"; // string
let inferredNumber = 42;      // number
let inferredBoolean = true;   // boolean
let inferredArray = [1, 2, 3]; // number[]
let inferredObject = { a: 1 }; // { a: number }

// Example 4.2: No Inference (implicit any)
// -----------------------------------

// In strict mode, these cause errors
// let noInference; // Error: implicit any

// Explicit any bypasses checks (avoid)
let explicitlyAny: any = "can be anything";

// Unknown requires type narrowing
let unknownValue: unknown = "hello";
if (typeof unknownValue === "string") {
  console.log(unknownValue.toUpperCase());
}

// ============================================================================
// SECTION 5: CONST ASSERTIONS
// ============================================================================

// Example 5.1: as const
// -----------------

// Makes object/array literal readonly with literal types
const colors = ["red", "green", "blue"] as const;
// Type: readonly ["red", "green", "blue"]

const directions = {
  up: "UP",
  down: "DOWN"
} as const;
// Type: { readonly up: "UP"; readonly down: "DOWN" }

// Example 5.2: Literal Type Inference
// ------------------------------

// Without as const - inferred as string
const color1 = "red";

// With as const - inferred as "red"
const color2 = "red" as const;

// Useful for function parameters
function setDirection(dir: "UP" | "DOWN"): void {}
setDirection("UP"); // Only accepts these literals

// ============================================================================
// SECTION 6: DECLARATION PATTERNS
// ============================================================================

// Example 6.1: Destructuring with Types
// ---------------------------------

const { name, age }: { name: string; age: number } = { name: "John", age: 30 };
const [first, second]: [number, number] = [1, 2];

// Example 6.2: Spread with Types
// --------------------------

const base = { x: 1, y: 2 };
const extended = { ...base, z: 3 };
// Type: { x: number; y: number; z: number }

// Example 6.3: Module-Scoped Variables
// -------------------------------

// At top level, declare exports for module
export const exportedVar = "can be imported";
export type ExportedType = string | number;

// ============================================================================
// SECTION 7: COMMON PITFALLS
// ============================================================================

/**
 * ⚠️ ISSUE 1: Mutable let with inferred type
 * -------------------------------------------
 */

function fixMutableInference(): void {
  let inferred = "hello";
  // inferred = 42; // Error - type is string
  
  // Use explicit type for flexibility
  let flexible: string | number = "hello";
  flexible = 42; // OK
}

/**
 * ⚠️ ISSUE 2: Reassigning const objects
 * ------------------------------------
 */

function fixConstReassignment(): void {
  const obj = { a: 1 };
  obj.a = 2; // OK - modifying property
  
  // If you need reassignment, use let
}

/**
 * ⚠️ ISSUE 3: Array type inference
 * -----------------------------
 */

function fixArrayInference(): void {
  const arr = []; // Warning: implicit any[]
  const typedArr: number[] = []; // Explicit type
}

console.log("\n=== Variables and Declarations Complete ===");
console.log("Next: FUNDAMENTALS/SYNTAX/02_Functions_and_Methods");