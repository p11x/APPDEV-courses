# Vitest Setup with React

## Overview

Vitest is a blazing fast unit test runner built on Vite. It's compatible with Jest APIs, making it easy to migrate existing tests. This guide covers setting up Vitest with React Testing Library for comprehensive testing.

## Prerequisites

- Node.js 18+
- Basic JavaScript/TypeScript knowledge

## Core Concepts

### Installing Vitest

```bash
# File: terminal

npm install -D vitest @testing-library/react @testing-library/jest-dom jsdom @vitejs/plugin-react
```

### Configuring Vitest

```javascript
// File: vitest.config.ts

import { defineConfig } from 'vitest/config';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: ['./src/test/setup.ts'],
  },
});
```

### Setup File

```typescript
// File: src/test/setup.ts

import '@testing-library/jest-dom';
```

### First Test

```tsx
// File: src/components/Button.test.tsx

import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import Button from './Button';

describe('Button', () => {
  it('renders correctly', () => {
    render(<Button>Click me</Button>);
    expect(screen.getByRole('button')).toHaveTextContent('Click me');
  });

  it('handles clicks', () => {
    const handleClick = vi.fn();
    render(<Button onClick={handleClick}>Click me</Button>);
    screen.getByRole('button').click();
    expect(handleClick).toHaveBeenCalled();
  });
});
```

## Key Takeaways

- Vitest provides fast test execution
- Use React Testing Library for component tests
- Run tests with `npx vitest`

## What's Next

Continue to [Testing Components with RTL](/10-testing/01-unit-testing/02-testing-components-with-rtl.md) to learn about component testing patterns.