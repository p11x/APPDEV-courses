/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 08
 * Topic: Class_Decorators
 * Purpose: Class decorator patterns and uses
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TS 5.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Class Decorators - Comprehensive Guide
 * ======================================
 * 
 * 📚 WHAT: Decorators for classes
 * 💡 WHY: Enable class modification and metadata
 * 🔧 HOW: Constructor transformation, sealed patterns
 */

// ============================================================================
// SECTION 1: CLASS MODIFICATION
// ============================================================================

// Example 1.1: Add Methods to Class
// -----------------------------

function addMethod(methodName: string, value: string) {
  return function<T extends Function>(constructor: T): T {
    return class extends constructor {
      [methodName]() {
        return value;
      }
    };
  };
}

@addMethod("getValue", "Hello")
class WithMethod { }

// ============================================================================
// SECTION 2: SIGNATURE DECORATORS
// ============================================================================

// Example 2.1: Singleton Decorator
// ------------------------------

function singleton<T extends Function>(constructor: T): T {
  let instance: unknown;
  return class extends constructor {
    constructor() {
      super();
      if (!instance) {
        instance = this;
      }
      return instance;
    }
  } as T;
}

@singleton
class Singleton { }

// ============================================================================
// SECTION 3: CACHE DECORATORS
// ============================================================================

// Example 3.1: Memoization Decorator
// -------------------------------

function memoize<T extends Function>(target: T, context: ClassMethodDecoratorContext) {
  const cache = new Map<string, unknown>();
  return function(this: unknown, ...args: unknown[]) {
    const key = JSON.stringify(args);
    if (cache.has(key)) return cache.get(key);
    const result = target.apply(this, args);
    cache.set(key, result);
    return result;
  } as T;
}

// ============================================================================
// SECTION 4: AUTO-Bind DECORATORS
// ============================================================================

// Example 4.1: Auto-Bind Methods
// ---------------------------

function bound<T extends Function>(target: T, context: ClassMethodDecoratorContext) {
  return function(this: unknown, ...args: unknown[]) {
    return target.apply(this, args);
  };
}

// ============================================================================
// PERFORMANCE
// ============================================================================

// Class decorators execute once
// Minimal runtime overhead

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 5.0+
// experimentalDecorators

// ============================================================================
// SECURITY
// ============================================================================

// Be careful with prototype modifications
// Avoid memory leaks in caching

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 01_Decorator_Basics, 03_Method_Decorators

console.log("\n=== Class Decorators Complete ===");
console.log("Next: 08_Metaprogramming_Syntax/03_Method_Decorators");