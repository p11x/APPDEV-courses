# Bun Test Setup

## What You'll Learn

- How to set up Bun's test runner
- How Bun test compares to Jest
- How to configure Bun testing
- How to run tests with Bun

## Setup

No installation needed — `bun:test` is built into Bun.

```bash
# Run all tests
bun test

# Run specific file
bun test math.test.ts

# Watch mode
bun test --watch
```

## Basic Test

```ts
// math.test.ts

import { describe, it, expect } from 'bun:test';

function add(a: number, b: number): number {
  return a + b;
}

describe('add', () => {
  it('adds two numbers', () => {
    expect(add(2, 3)).toBe(5);
  });

  it('handles negatives', () => {
    expect(add(-1, 1)).toBe(0);
  });
});
```

## Next Steps

For features, continue to [Bun Test Features](./02-bun-test-features.md).
