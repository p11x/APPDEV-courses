# Vitest Plugins

## What You'll Learn

- How to use Vitest plugins
- How to set up UI mode
- How to use Vitest with React
- How to create custom plugins

## Vitest UI

```bash
npm install -D @vitest/ui
```

```ts
// vitest.config.ts

export default defineConfig({
  test: {
    // Enable UI mode
    ui: true,
  },
});
```

```bash
# Open Vitest UI
npx vitest --ui
# Opens at http://localhost:5120/__vitest__/
```

## React Testing

```bash
npm install -D @testing-library/react @testing-library/jest-dom jsdom
```

```ts
// vitest.config.ts

export default defineConfig({
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: ['./tests/setup.ts'],
  },
});
```

```ts
// tests/setup.ts

import '@testing-library/jest-dom/vitest';
```

## Snapshot Plugin

```ts
// Tests use the same snapshot format as Jest
describe('Component', () => {
  it('matches snapshot', () => {
    const html = render('<div>Hello</div>');
    expect(html).toMatchSnapshot();
  });
});
```

## Next Steps

For performance, continue to [Vitest Performance](./04-vitest-performance.md).
