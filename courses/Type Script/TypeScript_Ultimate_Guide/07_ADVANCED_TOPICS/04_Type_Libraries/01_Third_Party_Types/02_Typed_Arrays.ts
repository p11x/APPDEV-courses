/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 04_Type_Libraries
 * Concept: 01_Third_Party_Types
 * Topic: 02_Typed_Arrays
 * Purpose: Explore typed array implementations
 * Difficulty: intermediate
 * UseCase: data-processing
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: N/A
 * Security: N/A
 */

/**
 * WHAT: Typed arrays in TypeScript provide type-safe collection operations
 * with compile-time type checking for element types.
 */

type TypedArray<T> = T[];

type ReadonlyTypedArray<T> = readonly T[];

type ImmutableArray<T> = ReadonlyArray<T>;

type List<T> = T[];

type Set<T> = T[];

type Dictionary<T> = Record<string, T>;

type Tuple<T extends any[]> = T;

type Vector<T extends number> = T[];

type Matrix<T extends number, N extends number> = T[][];

type Option<T> = T | null;

type Result<T, E = Error> = { ok: true; value: T } | { ok: false; error: E };

type Maybe<T> = T | undefined | null;

type Either<L, R> = { type: "left"; value: L } | { type: "right"; value: R };

type AsyncResult<T> = Promise<Result<T>>;

interface TypedList<T> {
  add(item: T): this;
  remove(predicate: (item: T) => boolean): this;
  map<U>(fn: (item: T) => U): TypedList<U>;
  filter(predicate: (item: T) => boolean): TypedList<T>;
  find(predicate: (item: T) => boolean): T | undefined;
  reduce<U>(fn: (acc: U, item: T) => U, initial: U): U;
}

class ListImpl<T> implements TypedList<T> {
  private items: T[] = [];
  
  add(item: T): this {
    this.items.push(item);
    return this;
  }
  
  remove(predicate: (item: T) => boolean): this {
    this.items = this.items.filter(item => !predicate(item));
    return this;
  }
  
  map<U>(fn: (item: T) => U): TypedList<U> {
    return new ListImpl<U>().add(...this.items.map(fn));
  }
  
  filter(predicate: (item: T) => boolean): TypedList<T> {
    return new ListImpl<T>().add(...this.items.filter(predicate));
  }
  
  find(predicate: (item: T) => boolean): T | undefined {
    return this.items.find(predicate);
  }
  
  reduce<U>(fn: (acc: U, item: T) => U, initial: U): U {
    return this.items.reduce(fn, initial);
  }
}

type SafeArray<T> = {
  get(index: number): T | undefined;
  set(index: number, value: T): boolean;
  size(): number;
};

function createSafeArray<T>(): SafeArray<T> {
  const items: T[] = [];
  return {
    get: (index) => items[index],
    set: (index, value) => {
      if (index < items.length) {
        items[index] = value;
        return true;
      }
      return false;
    },
    size: () => items.length
  };
}

console.log("\n=== Typed Arrays Demo ===");
const list = new ListImpl<number>().add(1).add(2).add(3);
console.log("List:", list.reduce((a, b) => a + b, 0));

const safe = createSafeArray<string>();
safe.set(0, "hello");
console.log("Safe array get:", safe.get(0));

/**
 * PERFORMANCE:
 * - Type checking is compile-time only
 * - Runtime overhead depends on implementation
 * 
 * CROSS-REFERENCE:
 * - 01_Community_Types.ts - Type definitions
 * - 02_Type_Creation/02_Type_Builders.ts - Type builders
 */