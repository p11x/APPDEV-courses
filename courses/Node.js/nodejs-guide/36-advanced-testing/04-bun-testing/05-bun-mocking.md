# Bun Mocking

## What You'll Learn

- How to mock functions in Bun
- How to mock modules
- How to spy on methods

## Function Mocking

```ts
import { mock, describe, it, expect } from 'bun:test';

describe('Mocking', () => {
  it('mocks a function', () => {
    const fn = mock((x: number) => x * 2);
    expect(fn(5)).toBe(10);
    expect(fn).toHaveBeenCalledTimes(1);
    expect(fn).toHaveBeenCalledWith(5);
  });

  it('mocks return values', () => {
    const fn = mock(() => 'default');
    fn.mockReturnValueOnce('first');
    fn.mockReturnValueOnce('second');

    expect(fn()).toBe('first');
    expect(fn()).toBe('second');
    expect(fn()).toBe('default');
  });
});
```

## Module Mocking

```ts
import { mock, describe, it, expect } from 'bun:test';

// Mock an entire module
mock.module('./db', () => ({
  query: mock(() => Promise.resolve({ rows: [{ id: 1, name: 'Alice' }] })),
}));

// Use in test
import { query } from './db';

it('queries mock database', async () => {
  const result = await query('SELECT * FROM users');
  expect(result.rows).toHaveLength(1);
});
```

## Next Steps

For testing strategies, continue to [Testing Pyramid](../05-testing-strategies/01-testing-pyramid.md).
