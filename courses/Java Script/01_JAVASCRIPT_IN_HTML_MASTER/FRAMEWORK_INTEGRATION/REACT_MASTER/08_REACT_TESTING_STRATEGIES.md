# 🧪 React Testing Complete Guide

## Testing React Applications Professionally

---

## Table of Contents

1. [Testing Fundamentals](#testing-fundamentals)
2. [Jest Setup](#jest-setup)
3. [React Testing Library](#react-testing-library)
4. [Testing Components](#testing-components)
5. [Testing Hooks](#testing-hooks)
6. [Testing Context](#testing-context)
7. [Testing Async Code](#testing-async-code)
8. [Mocking](#mocking)
9. [Integration Testing](#integration-testing)
10. [E2E Testing](#e2e-testing)

---

## Testing Fundamentals

### Why Test?

```
TESTING PYRAMID
═══════════════════════════════════════════════
                    ▲
                   /│\
                  / │ \
                 /  │  \
                /   │   \
               /────│────\
              │          │
              │   E2E   │
              │  Tests  │
             /──────────\
            │ Integration│
           /  Tests      \
          /────────────────\
         │   Unit Tests    │
        └─────────────────┘

Unit Tests: 70% - Fast, focused
Integration Tests: 20% - Test interactions
E2E Tests: 10% - Slow, comprehensive
```

### Testing Philosophy

```javascript
// Focus on user behavior, not implementation
// ✅ Good: Tests user interactions
test('should show error when invalid email', () => {
  render(<LoginForm />);
  userEvent.type(screen.getByLabelText(/email/), 'invalid');
  userEvent.click(screen.getByRole('button', { name: /submit/i }));
  expect(screen.getByText(/invalid email/i)).toBeInTheDocument();
});

// ❌ Bad: Tests implementation details
test('should call handleEmailChange', () => {
  render(<LoginForm />);
  const handleEmailChange = jest.fn();
  // This tests how it works, not what it does
});
```

---

## Jest Setup

### Installation

```bash
npm install --save-dev jest @testing-library/react @testing-library/jest-dom jest-environment-jsdom
```

### Configuration

```javascript
// jest.config.js
module.exports = {
  testEnvironment: 'jsdom',
  setupFilesAfterEnv: ['<rootDir>/setupTests.js'],
  moduleNameMapper: {
    '\\.(css|less|scss|sass)$': 'identity-obj-proxy'
  },
  transform: {
    '^.+\\.(js|jsx)$': 'babel-jest'
  },
  testMatch: [
    '<rootDir>/src/**/*.test.{js,jsx}'
  ]
};
```

### Setup File

```javascript
// setupTests.js
import '@testing-library/jest-dom';

// Mock localStorage
const localStorageMock = {
  getItem: jest.fn(),
  setItem: jest.fn(),
  removeItem: jest.fn(),
  clear: jest.fn()
};
global.localStorage = localStorageMock;

// Mock window.matchMedia
Object.defineProperty(window, 'matchMedia', {
  writable: true,
  value: jest.fn().mockImplementation(query => ({
    matches: false,
    media: query,
    onchange: null,
    addListener: jest.fn(),
    removeListener: jest.fn(),
    addEventListener: jest.fn(),
    removeEventListener: jest.fn(),
    dispatchEvent: jest.fn()
  }))
});
```

---

## React Testing Library

### Basic Render

```javascript
import { render, screen } from '@testing-library/react';
import MyComponent from './MyComponent';

test('renders component', () => {
  render(<MyComponent />);
  expect(screen.getByText('Hello')).toBeInTheDocument();
});
```

### Query Methods

```javascript
// Priority order (best to worst):
// 1. getByRole - Most inclusive, matches accessibility
// 2. getByLabelText - For form elements
// 3. getByPlaceholderText - When no label
// 4. getByText - For non-interactive elements
// 5. getByTestId - Last resort

test('uses queries correctly', () => {
  render(<MyForm />);
  
  // Best: getByRole
  expect(screen.getByRole('button', { name: /submit/i })).toBeInTheDocument();
  
  // Good: getByLabelText
  expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
  
  // Fallback: getByText
  expect(screen.getByText('Welcome')).toBeInTheDocument();
});
```

### Debugging

```javascript
import { render, screen, prettyDOM } from '@testing-library/react';

test('debug output', () => {
  render(<MyComponent />);
  
  // Print entire DOM
  screen.debug();
  
  // Print specific element
  console.log(prettyDOM(screen.getByText('Hello')));
});
```

---

## Testing Components

### Simple Component

```jsx
// Button.jsx
export function Button({ children, onClick, disabled }) {
  return (
    <button onClick={onClick} disabled={disabled}>
      {children}
    </button>
  );
}
```

```javascript
// Button.test.jsx
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Button } from './Button';

test('renders button with text', () => {
  render(<Button>Click me</Button>);
  expect(screen.getByText('Click me')).toBeInTheDocument();
});

test('calls onClick when clicked', async () => {
  const handleClick = jest.fn();
  render(<Button onClick={handleClick}>Click me</Button>);
  
  await userEvent.click(screen.getByText('Click me'));
  expect(handleClick).toHaveBeenCalledTimes(1);
});

test('is disabled when disabled prop is true', () => {
  render(<Button disabled>Click me</Button>);
  expect(screen.getByText('Click me')).toBeDisabled();
});
```

### Form Component

```jsx
// LoginForm.jsx
export function LoginForm({ onSubmit }) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  
  const handleSubmit = (e) => {
    e.preventDefault();
    if (!email || !password) {
      setError('All fields are required');
      return;
    }
    onSubmit({ email, password });
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <label>Email
        <input value={email} onChange={e => setEmail(e.target.value)} />
      </label>
      <label>Password
        <input value={password} onChange={e => setPassword(e.target.value)} />
      </label>
      {error && <p role="alert">{error}</p>}
      <button type="submit">Login</button>
    </form>
  );
}
```

```javascript
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { LoginForm } from './LoginForm';

test('shows error when fields are empty', async () => {
  render(<LoginForm onSubmit={jest.fn()} />);
  
  await userEvent.click(screen.getByRole('button', { name: /login/i }));
  expect(screen.getByRole('alert')).toHaveTextContent('All fields are required');
});

test('calls onSubmit with form data', async () => {
  const onSubmit = jest.fn();
  render(<LoginForm onSubmit={onSubmit} />);
  
  await userEvent.type(screen.getByLabelText(/email/i), 'test@example.com');
  await userEvent.type(screen.getByLabelText(/password/i), 'password123');
  await userEvent.click(screen.getByRole('button', { name: /login/i }));
  
  expect(onSubmit).toHaveBeenCalledWith({
    email: 'test@example.com',
    password: 'password123'
  });
});
```

---

## Testing Hooks

### Testing Custom Hooks

```javascript
import { renderHook, act } from '@testing-library/react';
import { useCounter } from './useCounter';

test('increments counter', () => {
  const { result } = renderHook(() => useCounter());
  
  act(() => {
    result.current.increment();
  });
  
  expect(result.current.count).toBe(1);
});

test('decrements counter', () => {
  const { result } = renderHook(() => useCounter({ initial: 5 }));
  
  act(() => {
    result.current.decrement();
  });
  
  expect(result.current.count).toBe(4);
});
```

### Testing useEffect

```javascript
import { renderHook, act, waitFor } from '@testing-library/react';
import { use fetchUser } from './useFetchUser';

test('fetches user data', async () => {
  global.fetch = jest.fn(() => 
    Promise.resolve({
      json: () => Promise.resolve({ name: 'John' })
    })
  );
  
  const { result, waitForLoadingToFinish } = renderHook(() => 
    useFetchUser(1)
  );
  
  await waitForLoadingToFinish();
  
  expect(result.current.user).toEqual({ name: 'John' });
});
```

---

## Testing Context

### Context Provider

```jsx
// ThemeContext.jsx
export const ThemeContext = createContext();

export function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  
  return (
    <ThemeContext.Provider value={{ theme, setTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}
```

```javascript
import { render, screen } from '@testing-library/react';
import { ThemeProvider } from './ThemeContext';
import { ThemedButton } from './ThemedButton';

test('uses theme from context', () => {
  render(
    <ThemeProvider>
      <ThemedButton />
    </ThemeProvider>
  );
  
  expect(screen.getByRole('button')).toHaveTextContent('light');
});
```

### Overriding Context

```javascript
import { render, screen } from '@testing-library/react';
import { ThemeProvider } from './ThemeContext';
import { ThemedButton } from './ThemedButton';

test('overrides theme in tests', () => {
  render(
    <ThemeProvider>
      <ThemedButton />
    </ThemeProvider>
  );
  
  // Use custom render to override context
});
```

---

## Testing Async Code

### Waiting for Elements

```javascript
import { render, screen, waitFor } from '@testing-library/react';

test('loads user after delay', async () => {
  render(<UserList />);
  
  // Initially shows loading
  expect(screen.getByText(/loading/i)).toBeInTheDocument();
  
  // Wait for content
  await waitFor(() => {
    expect(screen.getByText('John')).toBeInTheDocument();
  });
});
```

### Async Queries

```javascript
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

test('search filters results', async () => {
  render(<SearchableList items={['apple', 'banana', 'cherry']} />);
  
  await userEvent.type(screen.getByRole('searchbox'), 'ban');
  
  expect(screen.getByText('banana')).toBeInTheDocument();
  expect(screen.queryByText('apple')).not.toBeInTheDocument();
});
```

---

## Mocking

### Mocking Modules

```javascript
// api.js
export const fetchUser = (id) => 
  fetch(`/api/users/${id}`).then(r => r.json());

// api.test.js
import { fetchUser } from './api';

test('fetches user', async () => {
  global.fetch = jest.fn(() =>
    Promise.resolve({
      json: () => Promise.resolve({ id: 1, name: 'John' })
    })
  );
  
  const user = await fetchUser(1);
  expect(user.name).toBe('John');
});
```

### Mocking Timers

```javascript
import { render, screen, act } from '@testing-library/react';

test('counts after delay', async () => {
  jest.useFakeTimers();
  
  render(<DelayedCounter />);
  
  // Fast-forward time
  act(() => {
    jest.advanceTimersByTime(1000);
  });
  
  expect(screen.getByText('Count: 1')).toBeInTheDocument();
  
  jest.useRealTimers();
});
```

### Mocking Components

```javascript
import { render, screen } from '@testing-library/react';

jest.mock('./HeavyComponent', () => ({
  __esModule: true,
  default: () => <div>Mocked Component</div>
}));

test('renders mocked component', () => {
  render(<Parent />);
  expect(screen.getByText('Mocked Component')).toBeInTheDocument();
});
```

---

## Integration Testing

### Testing Multiple Components

```javascript
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { App } from './App';

test('full user flow', async () => {
  render(<App />);
  
  // Add todo
  await userEvent.type(screen.getByPlaceholderText(/add todo/i), 'Buy milk');
  await userEvent.click(screen.getByRole('button', { name: /add/i }));
  
  // Verify todo added
  expect(screen.getByText('Buy milk')).toBeInTheDocument();
  
  // Complete todo
  await userEvent.click(screen.getByRole('checkbox'));
  
  // Verify completed
  expect(screen.getByRole('checkbox')).toBeChecked();
  
  // Delete todo
  await userEvent.click(screen.getByRole('button', { name: /delete/i }));
  
  // Verify deleted
  expect(screen.queryByText('Buy milk')).not.toBeInTheDocument();
});
```

---

## E2E Testing

### Cypress Setup

```bash
npm install --save-dev cypress
npx cypress open
```

### Cypress Tests

```javascript
// cypress/integration/login.spec.js
describe('Login', () => {
  beforeEach(() => {
    cy.visit('/login');
  });
  
  it('should login successfully', () => {
    cy.get('[data-testid="email"]').type('test@example.com');
    cy.get('[data-testid="password"]').type('password123');
    cy.get('[data-testid="submit"]').click();
    
    cy.url().should('include', '/dashboard');
    cy.contains('Welcome').should('be.visible');
  });
  
  it('should show error for invalid credentials', () => {
    cy.get('[data-testid="email"]').type('invalid');
    cy.get('[data-testid="password"]').type('wrong');
    cy.get('[data-testid="submit"]').click();
    
    cy.contains('Invalid credentials').should('be.visible');
  });
});
```

### Playwright Setup

```bash
npm install --save-dev @playwright/test
npx playwright install
```

### Playwright Tests

```javascript
// tests/login.spec.js
import { test, expect } from '@playwright/test';

test('login flow', async ({ page }) => {
  await page.goto('/login');
  
  await page.fill('[data-testid="email"]', 'test@example.com');
  await page.fill('[data-testid="password"]', 'password123');
  await page.click('[data-testid="submit"]');
  
  await expect(page).toHaveURL('/dashboard');
  await expect(page.locator('text=Welcome')).toBeVisible();
});
```

---

## Summary

### Key Takeaways

1. **RTL**: Test user interactions, not implementation
2. **Jest**: Test runner with mocking support
3. **async**: Use waitFor for async operations
4. **Integration**: Test component interactions
5. **E2E**: Use Cypress/Playwright for full tests

### Next Steps

- Continue with: [09_REACT_DEPLOYMENT_GUIDE.md](09_REACT_DEPLOYMENT_GUIDE.md)
- Set up CI/CD for tests
- Implement test coverage reporting

---

## Cross-References

- **Previous**: [07_REACT_PERFORMANCE_OPTIMIZATION.md](07_REACT_PERFORMANCE_OPTIMIZATION.md)
- **Next**: [09_REACT_DEPLOYMENT_GUIDE.md](09_REACT_DEPLOYMENT_GUIDE.md)

---

*Last updated: 2024*