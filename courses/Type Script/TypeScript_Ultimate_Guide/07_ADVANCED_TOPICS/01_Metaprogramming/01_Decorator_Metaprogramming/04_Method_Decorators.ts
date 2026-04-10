/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 01_Decorator_Metaprogramming
 * Topic: 04_Method_Decorators
 * Purpose: Master method-level decorators for runtime behavior modification
 * Difficulty: advanced
 * UseCase: aspect-oriented-programming
 * Version: TS 5.0+
 * Compatibility: Node.js 12+, Browsers (ES2020+)
 * Performance: ~0.5-2ms overhead per method call
 * Security: Validate decorator inputs to prevent injection
 */

/**
 * WHAT: Method decorators allow you to intercept, modify, or extend method behavior.
 * They receive the target, property key, and property descriptor.
 */

// ============================================
// SECTION 1: PROPERTY DESCRIPTOR BASICS
// ============================================

function trace<T>(target: T, propertyKey: string, descriptor: PropertyDescriptor) {
  const methodName = propertyKey.toString();
  const original = descriptor.value;
  
  descriptor.value = function (...args: any[]) {
    console.log(`[TRACE] Entering ${methodName}`);
    console.log(`[TRACE] Args:`, JSON.stringify(args));
    const result = original.apply(this, args);
    console.log(`[TRACE] Exiting ${methodName}`);
    return result;
  };
  
  return descriptor;
}

class BankAccount {
  @trace
  transfer(from: string, to: string, amount: number) {
    return `Transferred $${amount} from ${from} to ${to}`;
  }
}

// ============================================
// SECTION 2: ASYNC METHOD DECORATORS
// ============================================

function asyncRetry(maxRetries: number, delayMs: number = 1000) {
  return function (target: any, propertyKey: string, descriptor: PropertyDescriptor) {
    const method = descriptor.value;
    
    descriptor.value = async function (...args: any[]) {
      let lastError: Error | null = null;
      
      for (let attempt = 1; attempt <= maxRetries; attempt++) {
        try {
          return await method.apply(this, args);
        } catch (error) {
          lastError = error as Error;
          console.log(`[RETRY] Attempt ${attempt} failed: ${lastError.message}`);
          if (attempt < maxRetries) {
            await new Promise(r => setTimeout(r, delayMs));
          }
        }
      }
      
      throw lastError;
    };
    
    return descriptor;
  };
}

class ApiClient {
  @asyncRetry(3, 500)
  async fetchData(endpoint: string): Promise<any> {
    if (Math.random() > 0.7) throw new Error("Network error");
    return { data: endpoint };
  }
}

// ============================================
// SECTION 3: METHOD VALIDATION DECORATORS
// ============================================

function validateArgs(...validators: ((arg: any) => boolean)[]) {
  return function (target: any, propertyKey: string, descriptor: PropertyDescriptor) {
    const method = descriptor.value;
    
    descriptor.value = function (...args: any[]) {
      for (let i = 0; i < validators.length; i++) {
        if (i >= args.length || !validators[i](args[i])) {
          throw new Error(`Validation failed for argument ${i} in ${propertyKey}`);
        }
      }
      return method.apply(this, args);
    };
    
    return descriptor;
  };
}

class Calculator {
  @validateArgs(
    (n: any) => typeof n === "number" && !isNaN(n),
    (n: any) => typeof n === "number" && !isNaN(n)
  )
  divide(a: number, b: number): number {
    if (b === 0) throw new Error("Division by zero");
    return a / b;
  }
}

// ============================================
// SECTION 4: GETTER AND SETTER DECORATORS
// ============================================

function lazy<T>(target: T, propertyKey: string, descriptor: PropertyDescriptor) {
  const getter = descriptor.get;
  
  descriptor.get = function () {
    const cached = `_lazy_${propertyKey}`;
    if (!(this as any)[cached]) {
      (this as any)[cached] = getter!.call(this);
    }
    return (this as any)[cached];
  };
  
  return descriptor;
}

function tracked<T>(target: T, propertyKey: string, descriptor: PropertyDescriptor) {
  const setter = descriptor.set;
  
  descriptor.set = function (value: any) {
    console.log(`[TRACK] Setting ${propertyKey} to:`, value);
    setter!.call(this, value);
  };
  
  return descriptor;
}

class LazyData {
  private _data: string[] = [];
  
  @lazy
  get data(): string[] {
    console.log("[LAZY] Computing expensive data...");
    return Array.from({ length: 1000 }, (_, i) => `item-${i}`);
  }
  
  @tracked
  set value(v: string) { this._data = [v]; }
  get value(): string { return this._data[0]; }
}

// ============================================
// SECTION 5: DECORATOR CHAINING FOR PROXY PATTERN
// ============================================

function memoize<T extends Function>(target: T, propertyKey: string, descriptor: PropertyDescriptor) {
  const cache = new Map<string, any>();
  const original = descriptor.value;
  
  descriptor.value = function (...args: any[]) {
    const key = JSON.stringify(args);
    if (cache.has(key)) {
      console.log(`[MEMO] Cache hit for ${propertyKey}`);
      return cache.get(key);
    }
    const result = original.apply(this, args);
    cache.set(key, result);
    return result;
  };
  
  return descriptor;
}

function debounce<T extends Function>(delay: number) {
  return function (target: T, propertyKey: string, descriptor: PropertyDescriptor) {
    let timeoutId: NodeJS.Timeout | null = null;
    const original = descriptor.value;
    
    descriptor.value = function (...args: any[]) {
      if (timeoutId) clearTimeout(timeoutId);
      timeoutId = setTimeout(() => original.apply(this, args), delay);
    };
    
    return descriptor;
  };
}

class SearchService {
  @memoize
  search(query: string): string[] {
    console.log(`[SEARCH] Executing search for: ${query}`);
    return [`Result 1 for ${query}`, `Result 2 for ${query}`];
  }
  
  @debounce(300)
  updateSuggestions(query: string) {
    console.log(`[DEBOUNCE] Updating suggestions: ${query}`);
  }
}

/**
 * PERFORMANCE:
 * - Each decorator adds wrapper overhead
 * - Memoization significantly speeds up repeated calls
 * - Debounce reduces unnecessary function calls
 * 
 * COMPATIBILITY:
 * - Works with TypeScript 5.0+ standard decorators
 * - Legacy mode requires experimentalDecorators
 * 
 * SECURITY:
 * - Validate cached data to prevent injection
 * - Be careful with closure over sensitive data
 * 
 * TESTING:
 * - Test each decorator independently
 * - Mock the original method in tests
 * 
 * DEBUGGING:
 * - Use unique prefixes for console output
 * - Inspect PropertyDescriptor in debugger
 * 
 * ALTERNATIVE:
 * - Proxy objects for runtime interception
 * - Higher-order functions for composition
 * 
 * CROSS-REFERENCE:
 * - 02_Decorator_Composition.ts - Composition patterns
 * - 03_Class_Decorators.ts - Class-level decorators
 * - 01_Advanced_Decorators.ts - Basic patterns
 */

console.log("\n=== Method Decorators Demo ===");
const account = new BankAccount();
account.transfer("Alice", "Bob", 100);

console.log("\n=== Async Retry ===");
async function testRetry() {
  const client = new ApiClient();
  try {
    const result = await client.fetchData("/users");
    console.log("Success:", result);
  } catch (e) {
    console.log("Failed after retries:", (e as Error).message);
  }
}
testRetry();

console.log("\n=== Validation ===");
const calc = new Calculator();
console.log(calc.divide(10, 2));

console.log("\n=== Lazy Evaluation ===");
const lazyData = new LazyData();
console.log("First access:", lazyData.data.length);
console.log("Second access:", lazyData.data.length);

console.log("\n=== Search Service ===");
const search = new SearchService();
console.log(search.search("typescript"));
console.log(search.search("typescript"));
search.updateSuggestions("ts");
search.updateSuggestions("ts2");