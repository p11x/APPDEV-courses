# Vitest Performance

## What You'll Learn

- How to optimize Vitest test speed
- How to use parallel execution
- How to use caching
- How to profile slow tests

## Parallel Execution

```ts
// vitest.config.ts

export default defineConfig({
  test: {
    pool: 'threads',
    poolOptions: {
      threads: {
        maxThreads: 4,
        minThreads: 1,
        // Single thread for tests that share state
        singleThread: false,
      },
    },
  },
});
```

```ts
// Force single-threaded for specific tests
// @vitest-environment node
// vitest: { singleThread: true }
```

## File-Level Parallelism

```ts
// vitest.config.ts
export default defineConfig({
  test: {
    // Run test files in parallel
    fileParallelism: true,
    // Limit concurrent test files
    maxConcurrency: 5,
  },
});
```

## Profiling

```bash
# Run with profiling
npx vitest run --reporter=verbose

# Show slowest tests
npx vitest run --reporter=verbose 2>&1 | sort -t'|' -k3 -rn | head -20
```

## Benchmarking

```ts
// benchmarks/math.bench.ts

import { bench, describe } from 'vitest';

describe('Math operations', () => {
  bench('fibonacci(30)', () => {
    fibonacci(30);
  });

  bench('factorial(20)', () => {
    factorial(20);
  });
});
```

```bash
npx vitest bench
```

## Next Steps

For React testing, continue to [Vitest React](./05-vitest-react.md).
