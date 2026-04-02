# Chaos Testing

## What You'll Learn

- What chaos engineering is
- How to inject failures in tests
- How to test resilience
- How to test recovery

## Failure Injection

```ts
// tests/chaos/resilience.test.ts

import { describe, it, expect } from 'vitest';

describe('Resilience', () => {
  it('handles database connection loss', async () => {
    // Simulate DB going down
    mock.module('./db', () => ({
      query: () => Promise.reject(new Error('Connection refused')),
    }));

    const response = await fetch('http://localhost:3000/api/users');
    expect(response.status).toBe(503);
  });

  it('handles slow responses', async () => {
    // Simulate slow DB
    mock.module('./db', () => ({
      query: () => new Promise((r) => setTimeout(r, 10_000)),
    }));

    const response = await fetch('http://localhost:3000/api/users');
    expect(response.status).toBe(504);  // Gateway timeout
  });

  it('handles memory pressure', () => {
    // Simulate high memory usage
    const largeArrays = [];
    for (let i = 0; i < 100; i++) {
      largeArrays.push(new Array(1_000_000).fill(0));
    }

    // App should still respond (with degraded performance)
    const response = await fetch('http://localhost:3000/health');
    expect(response.ok).toBeTruthy();
  });
});
```

## Next Steps

For contract testing, continue to [Pact Setup](../06-contract-testing/01-pact-setup.md).
