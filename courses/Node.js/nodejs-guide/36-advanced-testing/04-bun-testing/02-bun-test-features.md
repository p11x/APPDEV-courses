# Bun Test Features

## What You'll Learn

- Bun test matchers
- How to use Bun's built-in mocking
- How to test async code
- How to use snapshots

## Matchers

```ts
import { expect } from 'bun:test';

// Equality
expect(value).toBe(expected);
expect(value).toEqual(expected);
expect(value).not.toBe(unexpected);

// Truthiness
expect(value).toBeTruthy();
expect(value).toBeFalsy();
expect(value).toBeNull();
expect(value).toBeUndefined();

// Numbers
expect(value).toBeGreaterThan(5);
expect(value).toBeLessThan(10);
expect(value).toBeCloseTo(0.1 + 0.2);

// Strings
expect(str).toContain('hello');
expect(str).toMatch(/hel+o/);

// Arrays
expect(arr).toHaveLength(3);
expect(arr).toContain('item');

// Objects
expect(obj).toHaveProperty('name');
expect(obj).toMatchObject({ name: 'Alice' });
```

## Mocking

```ts
import { mock, describe, it, expect } from 'bun:test';

describe('Mocking', () => {
  it('mocks a function', () => {
    const fn = mock(() => 'mocked');
    expect(fn()).toBe('mocked');
    expect(fn).toHaveBeenCalled();
  });
});
```

## Next Steps

For comparison, continue to [Bun vs Jest](./03-bun-vs-jest.md).
