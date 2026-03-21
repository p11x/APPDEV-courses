# Testing Setup Template

## Overview
A comprehensive testing infrastructure template for React applications, including Vitest for unit/integration tests, React Testing Library for component tests, and Playwright for E2E tests.

## Project Structure

```
my-app/
├── tests/
│   ├── setup/                     # Test setup and utilities
│   │   ├── vitest-setup.ts       # Vitest configuration
│   │   ├── global.ts              # Global test utilities
│   │   └── mocks/                # Mock implementations
│   │       ├── next-auth.ts       # NextAuth mock
│   │       ├── intersection-observer.ts
│   │       └── match-media.ts
│   │
│   ├── utils/                    # Test utilities
│   │   ├── render.tsx            # Custom render with providers
│   │   ├── fire-event.ts         # Custom fire event helpers
│   │   └── assertions.ts         # Custom assertions
│   │
│   └── fixtures/                 # Test fixtures
│       ├── users.ts              # User fixtures
│       └── data.ts               # Mock data
│
├── src/
│   └── ...                       # Source files
│
├── e2e/                          # Playwright E2E tests
│   ├── auth.spec.ts
│   ├── dashboard.spec.ts
│   └── utils/
│       ├── page-objects.ts
│       └── test-data.ts
│
├── playwright.config.ts          # Playwright configuration
├── vitest.config.ts              # Vitest configuration
├── package.json
└── tsconfig.json
```

## Required Packages

```bash
# Unit & Integration Testing
npm install -D vitest @testing-library/react @testing-library/jest-dom @testing-library/user-event jsdom
npm install -D @testing-library/dom @vitest/coverage-v8

# E2E Testing
npm install -D @playwright/test
npx playwright install --with-deps chromium

# Mocking
npm install -D msw
npm install -D axios-mock-adapter
```

## Configuration Files

### vitest.config.ts

```typescript
/// <reference types="vitest" />
import { defineConfig } from 'vitest/config';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  
  test: {
    globals: true,  // Use global test APIs (describe, it, expect)
    environment: 'jsdom',  // Simulate browser environment
    setupFiles: ['./tests/setup/vitest-setup.ts'],
    
    // Coverage configuration
    coverage: {
      provider: 'v8',
      reporter: ['text', 'json', 'html'],
      include: ['src/**/*.{ts,tsx}'],
      exclude: ['src/**/*.d.ts', 'src/**/*.stories.tsx'],
    },
    
    // Test file patterns
    include: [
      'src/**/*.test.{ts,tsx}',
      'tests/**/*.test.{ts,tsx}',
    ],
    
    // Aliases for imports
    alias: {
      '@': path.resolve(__dirname, './src'),
      '@components': path.resolve(__dirname, './src/components'),
      '@hooks': path.resolve(__dirname, './src/hooks'),
    },
    
    // Timeout for async tests
    testTimeout: 10000,
    hookTimeout: 10000,
  },
  
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
});
```

### playwright.config.ts

```typescript
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './e2e',
  
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : undefined,
  
  reporter: [
    ['html', { outputFolder: 'playwright-report' }],
    ['list'],
  ],
  
  use: {
    baseURL: 'http://localhost:3000',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
  },
  
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
  
  webServer: {
    command: 'npm run dev',
    url: 'http://localhost:3000',
    reuseExistingServer: !process.env.CI,
    timeout: 120000,
  },
});
```

## Test Setup Files

### vitest-setup.ts

```typescript
// [File: tests/setup/vitest-setup.ts]
import '@testing-library/jest-dom';

// Mock Intersection Observer
class MockIntersectionObserver {
  observe = vi.fn();
  unobserve = vi.fn();
  disconnect = vi.fn();
}
window.IntersectionObserver = MockIntersectionObserver;

// Mock matchMedia
Object.defineProperty(window, 'matchMedia', {
  writable: true,
  value: vi.fn().mockImplementation((query) => ({
    matches: false,
    media: query,
    onchange: null,
    addListener: vi.fn(),
    removeListener: vi.fn(),
    addEventListener: vi.fn(),
    removeEventListener: vi.fn(),
    dispatchEvent: vi.fn(),
  })),
});

// Mock localStorage
const localStorageMock = {
  getItem: vi.fn(),
  setItem: vi.fn(),
  removeItem: vi.fn(),
  clear: vi.fn(),
};
Object.defineProperty(window, 'localStorage', { value: localStorageMock });
```

### Custom Render

```typescript
// [File: tests/utils/render.tsx]
import { ReactElement, ReactNode } from 'react';
import { render, RenderOptions } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from '@/providers/auth-provider';

/**
 * Custom render function that wraps components with all necessary providers.
 * Use this instead of render() in your tests.
 */
interface WrapperProps {
  children: ReactNode;
}

function AllProviders({ children }: WrapperProps) {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
        gcTime: 0,
      },
    },
  });

  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <AuthProvider>
          {children}
        </AuthProvider>
      </QueryClientProvider>
    </BrowserRouter>
  );
}

function customRender(
  ui: ReactElement,
  options?: Omit<RenderOptions, 'wrapper'>
) {
  return render(ui, { wrapper: AllProviders, ...options });
}

export * from '@testing-library/react';
export { customRender as render };
```

## Test Fixtures

### users.ts

```typescript
// [File: tests/fixtures/users.ts]
export const mockUsers = {
  admin: {
    id: '1',
    name: 'Admin User',
    email: 'admin@example.com',
    role: 'admin',
  },
  
  regular: {
    id: '2',
    name: 'Regular User',
    email: 'user@example.com',
    role: 'user',
  },
  
  unauthenticated: null,
} as const;

export type MockUser = typeof mockUsers.admin | typeof mockUsers.regular;
```

## Example Tests

### Component Test

```tsx
// [File: src/components/Button/Button.test.tsx]
import { describe, it, expect } from 'vitest';
import { render, screen } from '@/tests/utils/render';
import { Button } from './Button';

describe('Button', () => {
  it('renders with text', () => {
    render(<Button>Click me</Button>);
    expect(screen.getByRole('button', { name: /click me/i })).toBeInTheDocument();
  });

  it('handles click events', async () => {
    const handleClick = vi.fn();
    const user = userEvent.setup();
    
    render(<Button onClick={handleClick}>Click me</Button>);
    
    await user.click(screen.getByRole('button'));
    
    expect(handleClick).toHaveBeenCalledTimes(1);
  });

  it('shows loading state', () => {
    render(<Button isLoading>Click me</Button>);
    
    expect(screen.getByRole('button')).toBeDisabled();
    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });
});
```

### Hook Test

```tsx
// [File: src/hooks/useCounter.test.ts]
import { renderHook, act } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import { useCounter } from './useCounter';

describe('useCounter', () => {
  it('initializes with default value', () => {
    const { result } = renderHook(() => useCounter());
    expect(result.current.count).toBe(0);
  });

  it('initializes with custom value', () => {
    const { result } = renderHook(() => useCounter(10));
    expect(result.current.count).toBe(10);
  });

  it('increments count', () => {
    const { result } = renderHook(() => useCounter());
    
    act(() => {
      result.current.increment();
    });
    
    expect(result.current.count).toBe(1);
  });

  it('decrements count', () => {
    const { result } = renderHook(() => useCounter(5));
    
    act(() => {
      result.current.decrement();
    });
    
    expect(result.current.count).toBe(4);
  });
});
```

### E2E Test

```typescript
// [File: e2e/auth.spec.ts]
import { test, expect } from '@playwright/test';

test.describe('Authentication', () => {
  test('user can login with valid credentials', async ({ page }) => {
    await page.goto('/login');
    
    await page.fill('[name="email"]', 'user@example.com');
    await page.fill('[name="password"]', 'password123');
    await page.click('button[type="submit"]');
    
    await expect(page).toHaveURL('/dashboard');
    await expect(page.getByText('Welcome')).toBeVisible();
  });

  test('user sees error with invalid credentials', async ({ page }) => {
    await page.goto('/login');
    
    await page.fill('[name="email"]', 'wrong@example.com');
    await page.fill('[name="password"]', 'wrongpassword');
    await page.click('button[type="submit"]');
    
    await expect(page.getByText('Invalid credentials')).toBeVisible();
  });
});
```

## Running Tests

```bash
# Unit and Integration Tests
npm run test              # Run tests once
npm run test:watch        # Watch mode
npm run test:coverage     # With coverage report

# E2E Tests
npx playwright test       # Run all E2E tests
npx playwright test auth  # Run auth tests only
npx playwright test --ui # Interactive UI

# All Tests
npm run test:all         # Run unit + E2E tests
```

## Key Takeaways

1. **Custom render** — Wrap components with providers in tests
2. **Setup files** — Configure global mocks in vitest-setup.ts
3. **Fixtures** — Reusable test data for consistent tests
4. **E2E isolation** — Each test runs in fresh browser context

## Next Steps

1. Copy the configuration files to your project
2. Customize mocks for your specific needs
3. Add tests to your CI/CD pipeline

For more details, see:
- [Vitest Setup with React](../../10-testing/01-unit-testing/01-vitest-setup-with-react.md)
- [Testing Components with RTL](../../10-testing/01-unit-testing/02-testing-components-with-rtl.md)
- [Playwright Setup](../../10-testing/03-e2e-testing/01-playwright-setup.md)
