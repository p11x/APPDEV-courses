# Framework Testing Strategies

Comprehensive guide to testing JavaScript framework applications. Covers unit testing, integration testing, E2E testing, and testing libraries (Jest, Cypress).

## Table of Contents

1. [Testing Fundamentals](#testing-fundamentals)
2. [Unit Testing](#unit-testing)
3. [Integration Testing](#integration-testing)
4. [Component Testing](#component-testing)
5. [Hook Testing](#hook-testing)
6. [Context Testing](#context-testing)
7. [E2E Testing with Cypress](#e2e-testing-with-cypress)
8. [Testing Patterns](#testing-patterns)
9. [Key Takeaways](#key-takeaways)
10. [Common Pitfalls](#common-pitfalls)

---

## Testing Fundamentals

### Testing Pyramid

```
        /\
       /  \
      / E2E \
     /--------\
    /   Integration \
   /----------------\
  /      Unit       \
 /__________________\
```

### Testing Philosophy

- Fast feedback for developers
- Catch regressions early
- Document expected behavior
- Enable safe refactoring

---

## Unit Testing

### Basic Unit Tests

```javascript
// file: testing/UnitTests.test.js
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { describe, it, expect, beforeEach } from 'jest';

describe('Counter', () => {
  let component;

  beforeEach(() => {
    component = render(<Counter />);
  });

  it('renders initial count of 0', () => {
    expect(screen.getByText('Count: 0')).toBeInTheDocument();
  });

  it('increments count when + button is clicked', async () => {
    const user = userEvent.setup();
    const incrementButton = screen.getByText('+');

    await user.click(incrementButton);
    expect(screen.getByText('Count: 1')).toBeInTheDocument();
  });

  it('decrements count when - button is clicked', async () => {
    const user = userEvent.setup();
    const incrementButton = screen.getByText('+');
    const decrementButton = screen.getByText('-');

    await user.click(incrementButton);
    await user.click(incrementButton);
    await user.click(decrementButton);
    expect(screen.getByText('Count: 1')).toBeInTheDocument();
  });
});
```

### Testing Utilities

```javascript
// file: testing/helpers.js
export const createMockFn = (returnValue) => {
  const fn = jest.fn();
  fn.mockReturnValue(returnValue);
  return fn;
};

export const createAsyncMockFn = (returnValue, error = null) => {
  const fn = jest.fn();
  if (error) {
    fn.mockRejectedValue(error);
  } else {
    fn.mockResolvedValue(returnValue);
  }
  return fn;
};

export const mockConsole = () => {
  const originalConsole = { ...console };
  beforeEach(() => {
    jest.spyOn(console, 'log').mockImplementation(() => {});
    jest.spyOn(console, 'error').mockImplementation(() => {});
    jest.spyOn(console, 'warn').mockImplementation(() => {});
  });
  afterEach(() => {
    Object.assign(console, originalConsole);
  });
};
```

---

## Integration Testing

### Testing Connected Components

```javascript
// file: testing/ConnectedComponent.test.js
import { render, screen, waitFor } from '@testing-library/react';
import { Provider } from 'react-redux';
import { configureStore } from '@reduxjs/toolkit';
import userEvent from '@testing-library/user-event';
import { rest } from 'msw';
import { setupServer } from 'msw/node';
import App from './App';

const server = setupServer(
  rest.get('/api/users', (req, res, ctx) => {
    return res(
      ctx.json([
        { id: 1, name: 'John Doe', email: 'john@example.com' },
        { id: 2, name: 'Jane Doe', email: 'jane@example.com' },
      ])
    );
  })
);

beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());

const createTestStore = (preloadedState = {}) => {
  return configureStore({
    reducer: {
      users: userReducer,
    },
    preloadedState,
  });
};

describe('App Integration', () => {
  it('loads and displays users', async () => {
    const store = createTestStore();
    render(
      <Provider store={store}>
        <App />
      </Provider>
    );

    expect(screen.getByText('Loading...')).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.getByText('John Doe')).toBeInTheDocument();
      expect(screen.getByText('Jane Doe')).toBeInTheDocument();
    });
  });
});
```

---

## Component Testing

### Component Props Testing

```javascript
// file: testing/ComponentProps.test.js
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Button from './Button';

describe('Button', () => {
  it('renders with label', () => {
    render(<Button label="Click me" />);
    expect(screen.getByRole('button', { name: 'Click me' })).toBeInTheDocument();
  });

  it('calls onClick handler when clicked', async () => {
    const user = userEvent.setup();
    const onClick = jest.fn();

    render(<Button label="Click me" onClick={onClick} />);
    await user.click(screen.getByRole('button'));

    expect(onClick).toHaveBeenCalledTimes(1);
  });

  it('is disabled when disabled prop is true', () => {
    render(<Button label="Click me" disabled />);
    expect(screen.getByRole('button')).toBeDisabled();
  });

  it('shows loading state', () => {
    render(<Button label="Click me" isLoading />);
    expect(screen.getByText('Loading...')).toBeInTheDocument();
    expect(screen.getByRole('button')).toBeDisabled();
  });

  it('applies variant classes', () => {
    render(<Button label="Primary" variant="primary" />);
    expect(screen.getByRole('button')).toHaveClass('btn-primary');
  });
});
```

### Testing Complex Components

```javascript
// file: testing/ComplexComponent.test.js
import { render, screen, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import LoginForm from './LoginForm';

describe('LoginForm', () => {
  const mockOnSubmit = jest.fn();

  beforeEach(() => {
    mockOnSubmit.mockClear();
  });

  it('validates email format', async () => {
    const user = userEvent.setup();
    render(<LoginForm onSubmit={mockOnSubmit} />);

    const emailInput = screen.getByLabelText(/email/i);
    const submitButton = screen.getByRole('button', { name: /login/i });

    await user.type(emailInput, 'invalid-email');
    await user.click(submitButton);

    expect(screen.getByText(/invalid email/i)).toBeInTheDocument();
    expect(mockOnSubmit).not.toHaveBeenCalled();
  });

  it('validates password length', async () => {
    const user = userEvent.setup();
    render(<LoginForm onSubmit={mockOnSubmit} />);

    const passwordInput = screen.getByLabelText(/password/i);
    const submitButton = screen.getByRole('button', { name: /login/i });

    await user.type(passwordInput, 'short');
    await user.click(submitButton);

    expect(screen.getByText(/password must be/i)).toBeInTheDocument();
  });

  it('submits form with valid data', async () => {
    const user = userEvent.setup();
    render(<LoginForm onSubmit={mockOnSubmit} />);

    await user.type(screen.getByLabelText(/email/i), 'test@example.com');
    await user.type(screen.getByLabelText(/password/i), 'password123');
    await user.click(screen.getByRole('button', { name: /login/i }));

    await waitFor(() => {
      expect(mockOnSubmit).toHaveBeenCalledWith({
        email: 'test@example.com',
        password: 'password123',
      });
    });
  });
});
```

---

## Hook Testing

### Custom Hook Testing

```javascript
// file: testing/Hook.test.js
import { renderHook, act, waitFor } from '@testing-library/react';
import { useCounter } from './useCounter';

describe('useCounter', () => {
  it('initializes with default value', () => {
    const { result } = renderHook(() => useCounter());
    expect(result.current.count).toBe(0);
  });

  it('initializes with provided value', () => {
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

  it('resets count', () => {
    const { result } = renderHook(() => useCounter(5));

    act(() => {
      result.current.increment();
      result.current.reset();
    });

    expect(result.current.count).toBe(0);
  });
});
```

### Async Hook Testing

```javascript
// file: testing/AsyncHook.test.js
import { renderHook, waitFor, act } from '@testing-library/react';
import { useFetchUsers } from './useFetchUsers';

describe('useFetchUsers', () => {
  it('fetches users on mount', async () => {
    const { result } = renderHook(() => useFetchUsers());

    expect(result.current.loading).toBe(true);

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.users).toHaveLength(2);
  });

  it('refetches users', async () => {
    const { result } = renderHook(() => useFetchUsers());

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    act(() => {
      result.current.refetch();
    });

    expect(result.current.loading).toBe(true);

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });
  });

  it('handles errors', async () => {
    const { result } = renderHook(() => useFetchUsers({ shouldFail: true }));

    await waitFor(() => {
      expect(result.current.error).toBe('Failed to fetch');
    });
  });
});
```

---

## Context Testing

### Testing Context Providers

```javascript
// file: testing/Context.test.js
import { render, screen, waitFor } from '@testing-library/react';
import { ThemeProvider, useTheme } from './ThemeContext';

const ThemedComponent = () => {
  const { theme, toggleTheme } = useTheme();
  return (
    <div>
      <span data-testid="theme">{theme}</span>
      <button onClick={toggleTheme}>Toggle</button>
    </div>
  );
};

describe('ThemeContext', () => {
  it('provides default light theme', () => {
    render(
      <ThemeProvider>
        <ThemedComponent />
      </ThemeProvider>
    );

    expect(screen.getByTestId('theme')).toHaveTextContent('light');
  });

  it('toggles theme', async () => {
    const user = userEvent.setup();
    render(
      <ThemeProvider>
        <ThemedComponent />
      </ThemeProvider>
    );

    await user.click(screen.getByRole('button'));

    expect(screen.getByTestId('theme')).toHaveTextContent('dark');
  });

  it('throws error when used outside provider', () => {
    const consoleError = jest.spyOn(console, 'error').mockImplementation(() => {});

    expect(() => render(<ThemedComponent />)).toThrow(
      'useTheme must be used within ThemeProvider'
    );

    consoleError.mockRestore();
  });
});
```

---

## E2E Testing with Cypress

### Cypress Setup

```javascript
// file: cypress/integration/login.spec.js
describe('Login', () => {
  beforeEach(() => {
    cy.visit('/login');
  });

  it('displays login form', () => {
    cy.get('[data-testid="login-form"]').should('be.visible');
    cy.get('[data-testid="email-input"]').should('be.visible');
    cy.get('[data-testid="password-input"]').should('be.visible');
  });

  it('validates email is required', () => {
    cy.get('[data-testid="login-button"]').click();
    cy.contains('Email is required').should('be.visible');
  });

  it('validates password is required', () => {
    cy.get('[data-testid="email-input"]').type('test@example.com');
    cy.get('[data-testid="login-button"]').click();
    cy.contains('Password is required').should('be.visible');
  });

  it('logs in successfully', () => {
    cy.get('[data-testid="email-input"]').type('test@example.com');
    cy.get('[data-testid="password-input"]').type('password123');
    cy.get('[data-testid="login-button"]').click();

    cy.url().should('include', '/dashboard');
    cy.contains('Welcome').should('be.visible');
  });

  it('shows error on invalid credentials', () => {
    cy.get('[data-testid="email-input"]').type('test@example.com');
    cy.get('[data-testid="password-input"]').type('wrongpassword');
    cy.get('[data-testid="login-button"]').click();

    cy.contains('Invalid credentials').should('be.visible');
  });
});
```

### Advanced Cypress Tests

```javascript
// file: cypress/integration/dashboard.spec.js
describe('Dashboard', () => {
  beforeEach(() => {
    cy.login('test@example.com', 'password123');
    cy.visit('/dashboard');
  });

  it('loads user data', () => {
    cy.contains('John Doe').should('be.visible');
    cy.contains('john@example.com').should('be.visible');
  });

  it('filters users', () => {
    cy.get('[data-testid="search-input"]').type('Jane');

    cy.contains('Jane Doe').should('be.visible');
    cy.contains('John Doe').should('not.exist');
  });

  it('updates user', () => {
    cy.contains('John Doe').click();
    cy.get('[data-testid="edit-button"]').click();

    cy.get('[data-testid="name-input"]').clear().type('John Smith');
    cy.get('[data-testid="save-button"]').click();

    cy.contains('John Smith').should('be.visible');
  });

  it('deletes user', () => {
    cy.contains('John Doe').click();
    cy.get('[data-testid="delete-button"]').click();

    cy.on('window:confirm', () => true);
    cy.contains('John Doe').should('not.exist');
  });
});

describe('Navigation', () => {
  it('navigates between pages', () => {
    cy.visit('/dashboard');

    cy.contains('Dashboard').click();
    cy.url().should('include', '/');

    cy.contains('Users').click();
    cy.url().should('include', '/users');
  });
});
```

### Custom Cypress Commands

```javascript
// file: cypress/support/commands.js
import '@testing-library/cypress/add-commands';

Cypress.Commands.add('login', (email, password) => {
  cy.session([email, password], () => {
    cy.visit('/login');
    cy.get('[data-testid="email-input"]').type(email);
    cy.get('[data-testid="password-input"]').type(password);
    cy.get('[data-testid="login-button"]').click();
    cy.url().should('include', '/dashboard');
  });
});

Cypress.Commands.add('logout', () => {
  cy.get('[data-testid="user-menu"]').click();
  cy.get('[data-testid="logout-button"]').click();
});

Cypress.Commands.add('getByTestId', (testId) => {
  return cy.get(`[data-testid="${testId}"]`);
});
```

---

## Testing Patterns

### AAA Pattern

```javascript
// file: testing/patterns/AAA.test.js
describe('AAA Pattern', () => {
  it('example with Arrange, Act, Assert', () => {
    // Arrange
    const calculator = new Calculator();

    // Act
    const result = calculator.add(2, 3);

    // Assert
    expect(result).toBe(5);
  });
});
```

### Given-When-Then

```javascript
// file: testing/patterns/GWT.test.js
describe('User Management', () => {
  describe('given a user exists', () => {
    let user;

    beforeEach(() => {
      user = createTestUser();
    });

    describe('when updating the user', () => {
      let result;

      beforeEach(() => {
        result = updateUser(user, { name: 'New Name' });
      });

      it('then the user is updated', () => {
        expect(result.name).toBe('New Name');
      });
    });
  });
});
```

---

## Key Takeaways

1. **Unit tests** test isolated components
2. **Integration tests** test component interaction
3. **E2E tests** verify user workflows
4. **Mock external dependencies**
5. **Test edge cases and errors**

---

## Common Pitfalls

1. **Testing implementation details** instead of behavior
2. **Not testing error states**
3. **Flaky tests due to timing**
4. **Missing cleanup in beforeEach**
5. **Over-mocking** hides bugs

---

## Related Files

- [02_COMPONENT_ARCHITECTURE_PATTERNS](./02_COMPONENT_ARCHITECTURE_PATTERNS.md)
- [04_STATE_MANAGEMENT_PATTERNS](./04_STATE_MANAGEMENT_PATTERNS.md)
- [06_FRAMEWORK_PERFORMANCE_OPTIMIZATION](./06_FRAMEWORK_PERFORMANCE_OPTIMIZATION.md)
- [08_FRAMEWORK_DEPLOYMENT_AND_BUILDING](./08_FRAMEWORK_DEPLOYMENT_AND_BUILDING.md)