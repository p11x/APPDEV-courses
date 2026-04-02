# Bun Testing Framework

## What You'll Learn

- How to use Bun's built-in test runner
- Bun-specific test APIs
- Snapshot testing
- Mocking and spying with Bun

## Setup

No installation needed — `bun:test` is built into Bun.

```json
// package.json
{
  "scripts": {
    "test": "bun test",
    "test:watch": "bun test --watch"
  }
}
```

## Basic Tests

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

  it('handles negative numbers', () => {
    expect(add(-1, 1)).toBe(0);
  });

  it('handles decimals', () => {
    expect(add(0.1, 0.2)).toBeCloseTo(0.3);
  });
});
```

```bash
# Run tests
bun test

# Run specific file
bun test math.test.ts

# Watch mode
bun test --watch
```

## Matchers

```ts
import { expect } from 'bun:test';

// Equality
expect(value).toBe(expected);           // Strict equality
expect(value).toEqual(expected);        // Deep equality
expect(value).not.toBe(unexpected);     // Negation

// Truthiness
expect(value).toBeTruthy();
expect(value).toBeFalsy();
expect(value).toBeNull();
expect(value).toBeUndefined();
expect(value).toBeDefined();

// Numbers
expect(value).toBeGreaterThan(5);
expect(value).toBeGreaterThanOrEqual(5);
expect(value).toBeLessThan(10);
expect(value).toBeCloseTo(0.1 + 0.2);  // Floating point

// Strings
expect(str).toContain('hello');
expect(str).toMatch(/hel+o/);
expect(str).startsWith('hel');
expect(str).endsWith('llo');

// Arrays / Objects
expect(arr).toHaveLength(3);
expect(arr).toContain('item');
expect(arr).toContainEqual({ id: 1 });
expect(obj).toHaveProperty('name');
expect(obj).toMatchObject({ name: 'Alice' });

// Throws
expect(() => throw new Error()).toThrow();
expect(() => throw new Error('msg')).toThrow('msg');
```

## Mocking

```ts
import { describe, it, expect, mock, spyOn } from 'bun:test';

// Mock a function
const mockFn = mock(() => 'mocked');

describe('mocking', () => {
  it('calls mock function', () => {
    mockFn();
    expect(mockFn).toHaveBeenCalled();
    expect(mockFn).toHaveBeenCalledTimes(1);
    expect(mockFn).toHaveReturnedWith('mocked');
  });

  it('spies on a method', () => {
    const obj = { greet: (name: string) => `Hello, ${name}` };
    const spy = spyOn(obj, 'greet');

    obj.greet('Alice');

    expect(spy).toHaveBeenCalledWith('Alice');
    spy.mockRestore();  // Clean up
  });
});
```

## Async Tests

```ts
import { it, expect } from 'bun:test';

async function fetchData(url: string): Promise<{ data: string }> {
  const res = await fetch(url);
  return res.json();
}

it('fetches data', async () => {
  const result = await fetchData('https://api.example.com/data');
  expect(result.data).toBeDefined();
});

it('handles fetch errors', async () => {
  expect(fetchData('https://invalid.url')).rejects.toThrow();
});
```

## Testing HTTP Servers

```ts
import { describe, it, expect, beforeAll, afterAll } from 'bun:test';

let server: ReturnType<typeof Bun.serve>;

beforeAll(() => {
  server = Bun.serve({
    port: 0,  // Random available port
    fetch(req) {
      if (req.url.endsWith('/health')) {
        return new Response(JSON.stringify({ status: 'ok' }));
      }
      return new Response('Not Found', { status: 404 });
    },
  });
});

afterAll(() => {
  server.stop();
});

describe('HTTP Server', () => {
  it('returns health status', async () => {
    const res = await fetch(`http://localhost:${server.port}/health`);
    const data = await res.json();

    expect(res.status).toBe(200);
    expect(data.status).toBe('ok');
  });

  it('returns 404 for unknown routes', async () => {
    const res = await fetch(`http://localhost:${server.port}/unknown`);
    expect(res.status).toBe(404);
  });
});
```

## Bun vs Node.js Test Runner

| Feature | node:test | bun:test |
|---------|-----------|---------|
| API style | node:test + node:assert | Jest-compatible |
| Mocking | `mock.fn()` | `mock()` + `spyOn()` |
| Watch mode | `--watch` | `--watch` |
| Snapshot | Manual | Built-in |
| Speed | Good | Faster (native) |

## Next Steps

For advanced Fastify patterns, continue to [Fastify Setup](../02-fastify-advanced/01-fastify-setup.md).
