/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 07
 * Topic: Async_Flow_Control
 * Purpose: Managing control flow in asynchronous TypeScript
 * Difficulty: advanced
 * UseCase: web, backend, mobile, enterprise
 * Version: TS 4.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Async Flow Control - Comprehensive Guide
 * ========================================
 * 
 * 📚 WHAT: Control flow patterns for async operations
 * 💡 WHY: Essential for handling promises and async/await safely
 * 🔧 HOW: Promise patterns, async/await, async iterators
 */

// ============================================================================
// SECTION 1: PROMISE FLOW CONTROL
// ============================================================================

// Example 1.1: Promise Chain Patterns
// -------------------------------

async function chainPromises<T>(values: T[]): Promise<T[]> {
  return values.reduce(async (acc, value) => {
    const result = await acc;
    result.push(value);
    return result;
  }, Promise.resolve<T[]>([]));
}

// ============================================================================
// SECTION 2: ASYNC/AWAIT PATTERNS
// ============================================================================

// Example 2.1: Sequential Async Flow
// ---------------------------------

async function sequentialFlow<T>(tasks: (() => Promise<T>)[]): Promise<T[]> {
  const results: T[] = [];
  for (const task of tasks) {
    results.push(await task());
  }
  return results;
}

// Example 2.2: Parallel Async Flow
// ------------------------------

async function parallelFlow<T>(tasks: (() => Promise<T>)[]): Promise<T[]> {
  return Promise.all(tasks.map(task => task()));
}

// ============================================================================
// SECTION 3: ASYNC ITERATORS
// ============================================================================

// Example 3.1: Async Iterator Pattern
// ---------------------------------

async function* asyncGenerator<T>(values: T[]): AsyncGenerator<T> {
  for (const value of values) {
    yield value;
  }
}

// ============================================================================
// SECTION 4: ERROR HANDLING IN ASYNC
// ============================================================================

// Example 4.1: Async Error Handling
// -------------------------------

async function safeAsync<T>(promise: Promise<T>): Promise<[T, null] | [null, Error]> {
  try {
    const value = await promise;
    return [value, null];
  } catch (error) {
    return [null, error as Error];
  }
}

// ============================================================================
// SECTION 5: RACE CONDITIONS
// ============================================================================

// Example 5.1: Abort Controller Pattern
// ----------------------------------

function withTimeout<T>(promise: Promise<T>, ms: number): Promise<T> {
  return Promise.race([
    promise,
    new Promise<T>((_, reject) => 
      setTimeout(() => reject(new Error("Timeout")), ms)
    )
  ]);
}

// ============================================================================
// SECTION 6: ADVANCED ASYNC PATTERNS
// ============================================================================

// Example 6.1: Async Retry Pattern
// ----------------------------

async function withRetry<T>(
  fn: () => Promise<T>,
  retries: number = 3
): Promise<T> {
  try {
    return await fn();
  } catch (error) {
    if (retries <= 0) throw error;
    await new Promise(r => setTimeout(r, 1000));
    return withRetry(fn, retries - 1);
  }
}

// ============================================================================
// PERFORMANCE
// ============================================================================

// Sequential: O(n) time, serial execution
// Parallel: O(1) time, but memory for all promises
// Use Promise.all for independent tasks

// ============================================================================
// COMPATIBILITY
// ============================================================================

// ES2017+ for async/await
// ES2018+ for async iterators
// Node 12+ full support

// ============================================================================
// SECURITY
// ============================================================================

// Handle rejection properly
// Use catch for all promises
// Timeout long-running operations

// ============================================================================
// TESTING
// ============================================================================

// Mock promises
// Test error paths
// Verify timeout behavior

// ============================================================================
// DEBUGGING
// ============================================================================

// Use async stack traces
// Check for unhandled rejections

// ============================================================================
// ALTERNATIVE
// ============================================================================

// Use callback patterns for Node.js style
// Use libraries like async.js

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 07_Type_Safe_Error_Handling

console.log("\n=== Async Flow Control Complete ===");
console.log("07_Advanced_Control_Flow Complete");
console.log("Next: 08_Metaprogramming_Syntax/01_Decorator_Basics");