/**
 * Category: PRACTICAL
 * Subcategory: DATA_PROCESSING
 * Concept: Streaming_and_Processing
 * Purpose: Stream processing in TypeScript
 * Difficulty: intermediate
 * UseCase: backend
 */

/**
 * Stream Processing - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Processing data streams in TypeScript
 * 💡 WHERE: Real-time data processing
 * 🔧 HOW: Async iterators, streams
 */

// ============================================================================
// SECTION 1: ASYNC ITERATORS
// ============================================================================

// Example 1.1: Async Iterator
// ---------------------------------

async function* asyncGenerator(): AsyncGenerator<number> {
  for (let i = 0; i < 10; i++) {
    await new Promise(resolve => setTimeout(resolve, 100));
    yield i;
  }
}

// Example 1.2: Consume Async Iterator
// ---------------------------------

async function consumeAsyncIterator(): Promise<void> {
  for await (const value of asyncGenerator()) {
    console.log(value);
  }
}

// ============================================================================
// SECTION 2: TRANSFORM STREAMS
// ============================================================================

// Example 2.1: Transform Stream
// ---------------------------------

interface TransformStream<T, U> {
  transform(chunk: T): U;
  flush(): U;
}

function createTransformStream<T, U>(
  transformer: (item: T) => U
): TransformStream<T, U> {
  return {
    transform: transformer,
    flush: () => null as unknown as U
  };
}

// ============================================================================
// SECTION 3: PIPELINE
// ============================================================================

// Example 3.1: Pipeline Composition
// ---------------------------------

function pipe<T>(...fns: ((input: T) => T)[]): (input: T) => T {
  return (input: T) => fns.reduce((acc, fn) => fn(acc), input);
}

const pipeline = pipe(
  (x: number) => x * 2,
  (x: number) => x + 1,
  (x: number) => String(x)
);

console.log("\n=== Stream Processing Complete ===");