# Bun vs Jest

## What You'll Learn

- Detailed comparison of Bun test and Jest
- When to choose each
- How to migrate from Jest

## Comparison

| Feature | Jest | Bun Test |
|---------|------|----------|
| Speed | ~2s/100 tests | ~0.2s/100 tests |
| TypeScript | Needs config | Built-in |
| ESM | Limited | Native |
| Mocking | jest.fn() | mock() |
| Snapshots | Yes | Yes |
| Watch | Yes | Yes |
| Coverage | Yes | Yes |

## Migration

```ts
// Jest
import { describe, it, expect } from '@jest/globals';
jest.mock('./module');

// Bun (same API)
import { describe, it, expect, mock } from 'bun:test';
mock.module('./module', () => ({ ... }));
```

## Next Steps

For performance, continue to [Bun Performance](./04-bun-performance.md).
