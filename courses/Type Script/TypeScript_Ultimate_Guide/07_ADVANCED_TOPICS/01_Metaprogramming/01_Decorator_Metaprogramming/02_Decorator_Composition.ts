/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 01_Decorator_Metaprogramming
 * Topic: 02_Decorator_Composition
 * Purpose: Learn how to compose multiple decorators for complex behavior
 * Difficulty: advanced
 * UseCase: framework-development
 * Version: TS 5.0+
 * Compatibility: Node.js 12+, Browsers (ES2020+)
 * Performance: Decorator chain overhead ~1-5ms per invocation
 * Security: Avoid storing sensitive data in decorator metadata
 */

/**
 * WHAT: Decorator composition is the technique of combining multiple decorators
 * to create complex behaviors. TypeScript supports composing decorators by
 * applying them in sequence or by creating higher-order decorators.
 */

// ============================================
// SECTION 1: BASIC DECORATOR COMPOSITION
// ============================================

function Log(target: any, propertyKey: string, descriptor: PropertyDescriptor) {
  const original = descriptor.value;
  descriptor.value = function (...args: any[]) {
    console.log(`[LOG] Calling ${propertyKey} with:`, args);
    return original.apply(this, args);
  };
  return descriptor;
}

function Validate(target: any, propertyKey: string, descriptor: PropertyDescriptor) {
  const original = descriptor.value;
  descriptor.value = function (...args: any[]) {
    if (args.some((arg: any) => arg === undefined || arg === null)) {
      throw new Error(`[VALIDATE] ${propertyKey} received invalid arguments`);
    }
    return original.apply(this, args);
  };
  return descriptor;
}

// Composition: Applying multiple decorators
class Calculator {
  @Log
  @Validate
  add(a: number, b: number): number {
    return a + b;
  }
}

// ============================================
// SECTION 2: DECORATOR FACTORY COMPOSITION
// ============================================

type DecoratorFactory = (config: any) => MethodDecorator;

const withTiming = (): MethodDecorator => (target, key, descriptor) => {
  const original = descriptor.value;
  descriptor.value = function (...args: any[]) {
    const start = performance.now();
    const result = original.apply(this, args);
    const duration = performance.now() - start;
    console.log(`[TIMING] ${String(key)} took ${duration.toFixed(2)}ms`);
    return result;
  };
  return descriptor;
};

const withRetry = (maxAttempts: number = 3): MethodDecorator => (target, key, descriptor) => {
  const original = descriptor.value;
  descriptor.value = async function (...args: any[]) {
    let lastError: Error | null = null;
    for (let i = 0; i < maxAttempts; i++) {
      try {
        return await original.apply(this, args);
      } catch (e) {
        lastError = e as Error;
        console.log(`[RETRY] Attempt ${i + 1} failed for ${String(key)}`);
      }
    }
    throw lastError;
  };
  return descriptor;
};

class DataService {
  @withTiming()
  @withRetry(3)
  async fetchData(url: string): Promise<string> {
    // Simulated fetch
    return `Data from ${url}`;
  }
}

// ============================================
// SECTION 3: SEQUENTIAL VS PARALLEL COMPOSITION
// ============================================

// Sequential: Decorators execute in order (bottom to top)
function First() {
  return function (target: any, key: string, descriptor: PropertyDescriptor) {
    console.log("First - Before");
    const original = descriptor.value;
    descriptor.value = function (...args: any[]) {
      console.log("First - During");
      const result = original.apply(this, args);
      console.log("First - After");
      return result;
    };
    return descriptor;
  };
}

function Second() {
  return function (target: any, key: string, descriptor: PropertyDescriptor) {
    console.log("Second - Before");
    const original = descriptor.value;
    descriptor.value = function (...args: any[]) {
      console.log("Second - During");
      const result = original.apply(this, args);
      console.log("Second - After");
      return result;
    };
    return descriptor;
  };
}

class Example {
  @First()
  @Second()
  method() { console.log("Original method"); }
}

// ============================================
// SECTION 4: CONDITIONAL DECORATORS
// ============================================

const conditional = (condition: boolean, decorator: MethodDecorator): MethodDecorator => {
  return condition ? decorator : (target, key, descriptor) => descriptor;
};

class Configurable {
  @conditional(true, withTiming())
  enabledMethod() { return "enabled"; }
  
  @conditional(false, withTiming())
  disabledMethod() { return "disabled"; }
}

// ============================================
// SECTION 5: COMPOSITION WITH PARAMETER DECORATORS
// ============================================

function TrackCalls(counterName: string) {
  const counters = new Map<string, number>();
  return function (target: any, propertyKey: string, descriptor: PropertyDescriptor) {
    const original = descriptor.value;
    descriptor.value = function (...args: any[]) {
      counters.set(counterName, (counters.get(counterName) || 0) + 1);
      console.log(`[TRACK] ${counterName}: ${counters.get(counterName)} calls`);
      return original.apply(this, args);
    };
    return descriptor;
  };
}

class TrackedService {
  @TrackCalls("userFetch")
  fetchUsers() { return []; }
  
  @TrackCalls("userFetch")
  fetchUserById(id: number) { return {}; }
}

// ============================================
// PERFORMANCE CONSIDERATIONS
// ============================================

/**
 * PERFORMANCE:
 * - Each decorator adds overhead to method invocation
 * - Stack depth increases with decorator count
 * - Use `experimentalDecorators` flag for legacy mode
 * - Consider lazy evaluation for expensive operations
 * 
 * COMPATIBILITY:
 * - TypeScript 5.0+ offers full decorator support
 * - Legacy mode requires `experimentalDecorators`
 * - Works in Node.js and modern browsers
 * 
 * SECURITY:
 * - Don't store sensitive data in decorator closures
 * - Avoid capturing secrets in decorator factories
 * - Be careful with prototype chain manipulation
 * 
 * TESTING:
 * - Test each decorator in isolation
 * - Mock dependencies injected via decorators
 * - Verify composition order doesn't break logic
 * 
 * DEBUGGING:
 * - Use console.log within decorators for debugging
 * - Inspect descriptor properties in dev tools
 * - Check decorator execution order with traces
 * 
 * ALTERNATIVE:
 * - Higher-order functions for composition
 * - Proxy objects for runtime decoration
 * - Mixins for class composition
 * 
 * CROSS-REFERENCE:
 * - 01_Advanced_Decorators.ts - Basic decorator patterns
 * - 03_Class_Decorators.ts - Class-level decorators
 * - 04_Method_Decorators.ts - Method-specific decorators
 */

console.log("\n=== Decorator Composition Demo ===");
const calc = new Calculator();
console.log(calc.add(5, 3));

const service = new DataService();
service.fetchData("https://api.example.com");

console.log("\n=== Execution Order Demo ===");
const ex = new Example();
ex.method();

console.log("\n=== Configuration Demo ===");
const cfg = new Configurable();
console.log(cfg.enabledMethod());
console.log(cfg.disabledMethod());

console.log("\n=== Tracking Demo ===");
const tracked = new TrackedService();
tracked.fetchUsers();
tracked.fetchUserById(1);