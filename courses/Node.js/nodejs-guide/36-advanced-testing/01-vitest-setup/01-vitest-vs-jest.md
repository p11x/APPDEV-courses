# Vitest vs Jest

## What You'll Learn

- How Vitest compares to Jest
- Why Vitest is faster
- When to choose each
- How to migrate from Jest to Vitest

## Comparison

| Feature | Jest | Vitest |
|---------|------|--------|
| Speed | Good | 10-50x faster |
| ESM support | Limited | Native |
| TypeScript | Needs config | Built-in |
| Vite integration | No | Native |
| Watch mode | Basic | Smart (reruns only changed) |
| Browser testing | No | Yes (via @vitest/browser) |
| Snapshot testing | Yes | Yes (compatible) |
| Mocking | Manual | Manual (compatible API) |
| Config | jest.config.js | vitest.config.ts |

## Why Vitest Is Faster

1. **Vite's dev server** — no bundling, instant transforms
2. **Smart test runner** — only reruns affected tests
3. **Native ESM** — no CommonJS transformation overhead
4. **Thread pooling** — parallel test execution

## Migration from Jest

```bash
# Install Vitest
npm install -D vitest

# Rename jest.config.js → vitest.config.ts
# Most Jest APIs work in Vitest with minimal changes
```

```ts
// vitest.config.ts
import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    globals: true,          // Use describe, it, expect without imports
    environment: 'node',    // or 'jsdom' for React
    include: ['**/*.test.ts'],
  },
});
```

## Next Steps

For configuration, continue to [Vitest Configuration](./02-vitest-configuration.md).
