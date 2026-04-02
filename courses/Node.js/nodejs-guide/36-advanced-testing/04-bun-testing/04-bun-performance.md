# Bun Test Performance

## What You'll Learn

- Why Bun tests are faster
- How to optimize test speed
- How to benchmark tests

## Speed Comparison

| Tests | Jest | Vitest | Bun |
|-------|------|--------|-----|
| 100 unit | 2.5s | 0.8s | 0.3s |
| 500 unit | 12s | 3s | 1.2s |
| 1000 unit | 25s | 6s | 2.5s |

## Optimization Tips

```ts
// Use bun:test instead of jest
// Bun's runtime is 10x faster than Node.js for most operations

// Use --bail to stop on first failure
bun test --bail

// Run specific tests
bun test --test-name-pattern="user"
```

## Next Steps

For mocking, continue to [Bun Mocking](./05-bun-mocking.md).
