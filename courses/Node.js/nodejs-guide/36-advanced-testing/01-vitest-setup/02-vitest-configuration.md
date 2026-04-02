# Vitest Configuration

## What You'll Learn

- How to configure Vitest
- How to set up test environments
- How to configure coverage
- How to set up path aliases

## Basic Configuration

```ts
// vitest.config.ts

import { defineConfig } from 'vitest/config';
import { resolve } from 'node:path';

export default defineConfig({
  test: {
    // Test environment
    environment: 'node',

    // Global APIs (no imports needed)
    globals: true,

    // Test file patterns
    include: ['src/**/*.test.ts', 'tests/**/*.test.ts'],
    exclude: ['node_modules', 'dist'],

    // Coverage
    coverage: {
      provider: 'v8',  // or 'istanbul'
      reporter: ['text', 'html', 'json'],
      include: ['src/**/*.ts'],
      exclude: ['src/**/*.test.ts', 'src/types/**'],
      thresholds: {
        lines: 80,
        functions: 80,
        branches: 80,
        statements: 80,
      },
    },

    // Setup files (run before each test file)
    setupFiles: ['./tests/setup.ts'],

    // Timeout
    testTimeout: 10_000,

    // Parallel execution
    pool: 'threads',  // or 'forks' or 'vmThreads'
    poolOptions: {
      threads: {
        maxThreads: 4,
        minThreads: 1,
      },
    },
  },

  // Path aliases (match your tsconfig)
  resolve: {
    alias: {
      '@': resolve(__dirname, './src'),
    },
  },
});
```

## Environment per File

```ts
// @vitest-environment jsdom
// ↑ Put this at the top of a test file to use jsdom for that file only

import { describe, it, expect } from 'vitest';

describe('DOM test', () => {
  it('should create an element', () => {
    const div = document.createElement('div');
    div.textContent = 'Hello';
    expect(div.textContent).toBe('Hello');
  });
});
```

## Scripts

```json
{
  "scripts": {
    "test": "vitest",
    "test:run": "vitest run",
    "test:watch": "vitest --watch",
    "test:coverage": "vitest run --coverage",
    "test:ui": "vitest --ui"
  }
}
```

## Next Steps

For plugins, continue to [Vitest Plugins](./03-vitest-plugins.md).
