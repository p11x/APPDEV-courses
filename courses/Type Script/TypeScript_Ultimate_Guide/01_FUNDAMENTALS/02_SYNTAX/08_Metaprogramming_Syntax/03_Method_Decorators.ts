/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 08
 * Topic: Method_Decorators
 * Purpose: Method decorator patterns
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TS 5.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Method Decorators - Comprehensive Guide
 * =====================================
 * 
 * 📚 WHAT: Decorators for methods
 * 💡 WHY: Enable method modification, logging, timing
 * 🔧 HOW: Method surrounding, parameter access
 */

// ============================================================================
// SECTION 1: METHOD WRAPPING
// ============================================================================

// Example 1.1: Timing Decorator
// ---------------------------

function timed<T extends Function>(target: T, context: ClassMethodDecoratorContext) {
  return function(this: unknown, ...args: unknown[]) {
    console.time(target.name);
    const result = target.apply(this, args);
    console.timeEnd(target.name);
    return result;
  } as T;
}

class TimerExample {
  @timed
  heavy() { /* expensive operation */ }
}

// ============================================================================
// SECTION 2: LOGGING DECORATOR
// ============================================================================

// Example 2.1: Method Logging
// --------------------------

function log<T extends Function>(target: T, context: ClassMethodDecoratorContext) {
  return function(this: unknown, ...args: unknown[]) {
    console.log(`Calling ${target.name}`, args);
    const result = target.apply(this, args);
    console.log(`Result`, result);
    return result;
  } as T;
}

// ============================================================================
// SECTION 3: VALIDATION DECORATOR
// ============================================================================

// Example 3.1: Method Validation
// ---------------------------

function validate(target: Function, context: ClassMethodDecoratorContext) {
  return function(this: unknown, ...args: unknown[]) {
    if (args.length === 0) {
      throw new Error("Arguments required");
    }
    return target.apply(this, args);
  };
}

// ============================================================================
// SECTION 4: CACHING DECORATOR
// ============================================================================

// Example 4.1: Result Caching
// -------------------------

function cached<T extends Function>(target: T, context: ClassMethodDecoratorContext) {
  const cache = new Map<string, unknown>();
  const key = context.kind === "method" ? context.name : String(context.name);
  
  return function(this: unknown, ...args: unknown[]) {
    const cacheKey = `${key}:${JSON.stringify(args)}`;
    if (cache.has(cacheKey)) return cache.get(cacheKey);
    const result = target.apply(this, args);
    cache.set(cacheKey, result);
    return result;
  } as T;
}

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 5.0+
// experimentalDecorators

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 02_Class_Decorators, 04_Parameter_Decorators

console.log("\n=== Method Decorators Complete ===");
console.log("Next: 08_Metaprogramming_Syntax/04_Parameter_Decorators");