/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Generic_Types
 * Purpose: Introduction to generics in TypeScript
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Generic Types - Introduction
 * ============================
 * 
 * 📚 WHAT: Generics provide a way to create reusable, type-safe components
 * 💡 WHY: Write code once, use with many types
 * 🔧 HOW: Type parameters, generic constraints, generic defaults
 */

// ============================================================================
// SECTION 1: WHY GENERICS?
// ============================================================================

/**
 * Without generics, we'd either use 'any' (losing type safety) or create 
 * multiple versions of the same function for different types.
 */

// Example 1.1: The Problem Without Generics
// ---------------------------------------

// Without generics - lose type safety
function identityAny(value: any): any {
  return value;
}

const anyResult = identityAny("hello"); // Returns any, no type info

// With generics - preserve type safety
function identity<T>(value: T): T {
  return value;
}

const stringResult = identity("hello"); // Type is string
const numberResult = identity(42);       // Type is number

console.log("Generic identity:", stringResult, numberResult);

// ============================================================================
// SECTION 2: GENERIC FUNCTIONS
// ============================================================================

// Example 2.1: Basic Generic Function
// -------------------------------

function firstElement<T>(array: T[]): T | undefined {
  return array[0];
}

const strings = ["a", "b", "c"];
const numbers = [1, 2, 3];
const firstStr = firstElement(strings);  // string | undefined
const firstNum = firstElement(numbers);  // number | undefined

console.log("First element:", firstStr, firstNum);

// Example 2.2: Multiple Type Parameters
// ----------------------------------

function pair<K, V>(key: K, value: V): { key: K; value: V } {
  return { key, value };
}

const keyValue = pair("id", 123);
const mixed = pair<string, number[]>("items", [1, 2, 3]);

console.log("Pair:", keyValue, mixed);

// Example 2.3: Generic Arrow Functions
// ---------------------------------

const wrap = <T>(value: T): T => value;
const combine = <T, U>(a: T, b: U): [T, U] => [a, b];

console.log("Arrow generics:", wrap(42), combine("a", 1));

// ============================================================================
// SECTION 3: GENERIC INTERFACES
// ============================================================================

// Example 3.1: Generic Interface
// --------------------------

interface Container<T> {
  value: T;
  getValue(): T;
  setValue(value: T): void;
}

class StringContainer implements Container<string> {
  value: string = "";
  
  getValue(): string {
    return this.value;
  }
  
  setValue(value: string): void {
    this.value = value;
  }
}

class NumberContainer implements Container<number> {
  value: number = 0;
  
  getValue(): number {
    return this.value;
  }
  
  setValue(value: number): void {
    this.value = value;
  }
}

// Example 3.2: Generic Interface with Multiple Types
// -----------------------------------------------

interface KeyValuePair<K, V> {
  key: K;
  value: V;
}

const pair1: KeyValuePair<string, number> = { key: "age", value: 30 };
const pair2: KeyValuePair<number, string> = { key: 1, value: "one" };

console.log("Generic interface:", pair1, pair2);

// ============================================================================
// SECTION 4: GENERIC CLASSES
// ============================================================================

// Example 4.1: Basic Generic Class
// ---------------------------

class Box<T> {
  private content: T;
  
  constructor(content: T) {
    this.content = content;
  }
  
  getContent(): T {
    return this.content;
  }
  
  setContent(content: T): void {
    this.content = content;
  }
}

const stringBox = new Box("Hello");
const numberBox = new Box(42);
const objectBox = new Box({ name: "John" });

console.log("Generic class:", stringBox.getContent());

// Example 4.2: Generic Class with Methods
// ----------------------------------

class Stack<T> {
  private items: T[] = [];
  
  push(item: T): void {
    this.items.push(item);
  }
  
  pop(): T | undefined {
    return this.items.pop();
  }
  
  peek(): T | undefined {
    return this.items[this.items.length - 1];
  }
  
  isEmpty(): boolean {
    return this.items.length === 0;
  }
}

const numberStack = new Stack<number>();
numberStack.push(1);
numberStack.push(2);
numberStack.push(3);
console.log("Stack pop:", numberStack.pop());
console.log("Stack peek:", numberStack.peek());

// ============================================================================
// SECTION 5: GENERIC CONSTRAINTS
// ============================================================================

// Example 5.1: extends Keyword for Constraints
// -----------------------------------------

interface HasLength {
  length: number;
}

function logLength<T extends HasLength>(item: T): void {
  console.log(`Length: ${item.length}`);
}

logLength("hello");        // string has length
logLength([1, 2, 3]);      // array has length
logLength({ length: 10 }); // object with length property

// Example 5.2: Constraint to Specific Type
// ------------------------------------

function findById<T extends { id: number }>(items: T[], id: number): T | undefined {
  return items.find(item => item.id === id);
}

interface User { id: number; name: string; }
interface Product { id: number; title: string; price: number; }

const users: User[] = [{ id: 1, name: "John" }, { id: 2, name: "Jane" }];
const products: Product[] = [{ id: 1, title: "Apple", price: 1 }];

console.log("Find by id:", findById(users, 1));
console.log("Find by id:", findById(products, 1));

// ============================================================================
// SECTION 6: DEFAULT TYPE PARAMETERS
// ============================================================================

// Example 6.1: Generic Defaults
// --------------------------

interface ApiResponse<T = unknown> {
  data: T;
  status: number;
  message: string;
}

// Using default
const basicResponse: ApiResponse = {
  data: null,
  status: 200,
  message: "OK"
};

// Overriding default
const userResponse: ApiResponse<User> = {
  data: { id: 1, name: "John" },
  status: 200,
  message: "OK"
};

// Example 6.2: Multiple Defaults
// ---------------------------

function createPair<K = string, V = string>(key?: K, value?: V): { key: K; value: V } {
  return { key: key as K, value: value as V };
}

const defaultPair = createPair();           // { key: string, value: string }
const customPair = createPair<number, boolean>(1, true);

// ============================================================================
// SECTION 7: GENERIC TYPE ALIASES
// ============================================================================

// Example 7.1: Generic Type Alias
// ---------------------------

type Result<T> = 
  | { success: true; data: T }
  | { success: false; error: string };

function handleResult<T>(result: Result<T>): T {
  if (result.success) {
    return result.data;
  }
  throw new Error(result.error);
}

// Example 7.2: Generic Array Types
// ---------------------------

type MaybeArray<T> = T | T[];
type ReadonlyArray<T> = readonly T[];

function processItems<T>(items: MaybeArray<T>): T[] {
  return Array.isArray(items) ? items : [items];
}

// ============================================================================
// SECTION 8: COMMON PITFALLS
// ============================================================================

/**
 * ⚠️ ISSUE 1: Type inference failure
 * ---------------------------------
 */

function fixTypeInference(): void {
  // Sometimes TypeScript can't infer - help with explicit type
  function wrap<T>(value: T): T { return value; }
  
  // Explicit may be needed in complex scenarios
  const explicit: number = wrap<number>(42);
}

// /**
//  * ⚠️ ISSUE 2: Too many type parameters
//  * ----------------------------------
//  */
// 
// function fixTooManyParams(): void {
//   // Instead of many params, group related types
//   // Bad: function f<A, B, C, D, E>(...)
//   // Good: use interface or type alias
// }

// ============================================================================
// SECTION 9: PRACTICAL EXAMPLES
// ============================================================================

// Example 9.1: Generic Repository Pattern
// -----------------------------------

interface Repository<T> {
  findAll(): Promise<T[]>;
  findById(id: number): Promise<T | null>;
  save(entity: T): Promise<T>;
  delete(id: number): Promise<boolean>;
}

// Example 9.2: Generic Event System
// ---------------------------

type EventHandler<T = unknown> = (data: T) => void;

class EventEmitter<T> {
  private handlers: Map<string, EventHandler<T>[]> = new Map();
  
  on(event: string, handler: EventHandler<T>): void {
    const handlers = this.handlers.get(event) || [];
    handlers.push(handler);
    this.handlers.set(event, handlers);
  }
  
  emit(event: string, data: T): void {
    const handlers = this.handlers.get(event) || [];
    handlers.forEach(handler => handler(data));
  }
}

const emitter = new EventEmitter<string>();
emitter.on("message", (msg) => console.log(msg));
emitter.emit("message", "Hello!");

console.log("\n=== Generic Types Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/06_Advanced_Type_Operations");