# React Testing Library

## What You'll Learn
- Setting up React Testing Library
- Writing component tests
- Finding elements by different queries
- Testing user interactions

## Prerequisites
- Understanding of Jest
- Knowledge of React components
- Familiarity with Client Components

## Concept Explained Simply

React Testing Library (RTL) is like having a robot user test your app — it actually interacts with your components the way a real user would. Instead of checking internal implementation details, you test what the user sees and can do: clicking buttons, filling forms, seeing text on screen.

The philosophy is: "If a user can't see or use it, it doesn't matter." This leads to better tests that catch real bugs.

## Complete Code Example

### Component to Test

```typescript
// components/Counter.tsx
"use client";

import { useState } from "react";

export default function Counter() {
  const [count, setCount] = useState(0);
  
  return (
    <div>
      <h1 data-testid="count-display">Count: {count}</h1>
      <button 
        onClick={() => setCount(count + 1)}
        data-testid="increment-button"
      >
        Increment
      </button>
      <button 
        onClick={() => setCount(0)}
        data-testid="reset-button"
      >
        Reset
      </button>
    </div>
  );
}
```

### Writing Tests

```typescript
// __tests__/components/Counter.test.tsx
import { render, screen, fireEvent } from "@testing-library/react";
import "@testing-library/jest-dom";
import Counter from "../../components/Counter";

describe("Counter Component", () => {
  it("renders initial count of 0", () => {
    render(<Counter />);
    
    // Find element by text
    expect(screen.getByText("Count: 0")).toBeInTheDocument();
  });
  
  it("increments count when button is clicked", () => {
    render(<Counter />);
    
    // Get button and click it
    const button = screen.getByText("Increment");
    fireEvent.click(button);
    
    // Check updated text
    expect(screen.getByText("Count: 1")).toBeInTheDocument();
  });
  
  it("resets count when reset button is clicked", () => {
    render(<Counter />);
    
    // Increment twice
    fireEvent.click(screen.getByText("Increment"));
    fireEvent.click(screen.getByText("Increment"));
    
    // Reset
    fireEvent.click(screen.getByText("Reset"));
    
    // Should be back to 0
    expect(screen.getByText("Count: 0")).toBeInTheDocument();
  });
});
```

### More Complex Component

```typescript
// components/LoginForm.tsx
"use client";

import { useState } from "react";

interface LoginFormProps {
  onSubmit: (email: string, password: string) => Promise<void>;
}

export default function LoginForm({ onSubmit }: LoginFormProps) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setIsLoading(true);
    
    try {
      await onSubmit(email, password);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Login failed");
    } finally {
      setIsLoading(false);
    }
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label htmlFor="email">Email</label>
        <input
          id="email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          data-testid="email-input"
        />
      </div>
      
      <div>
        <label htmlFor="password">Password</label>
        <input
          id="password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          data-testid="password-input"
        />
      </div>
      
      {error && (
        <p role="alert" data-testid="error-message">
          {error}
        }
      </p>
      
      <button 
        type="submit" 
        disabled={isLoading}
        data-testid="submit-button"
      >
        {isLoading ? "Logging in..." : "Login"}
      </button>
    </form>
  );
}
```

```typescript
// __tests__/components/LoginForm.test.tsx
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import "@testing-library/jest-dom";
import LoginForm from "../../components/LoginForm";

describe("LoginForm Component", () => {
  it("renders email and password inputs", () => {
    render(<LoginForm onSubmit={jest.fn()} />);
    
    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/password/i)).toBeInTheDocument();
  });
  
  it("calls onSubmit with email and password", async () => {
    const mockSubmit = jest.fn().mockResolvedValue(undefined);
    render(<LoginForm onSubmit={mockSubmit} />);
    
    // Fill in form
    fireEvent.change(screen.getByTestId("email-input"), {
      target: { value: "test@example.com" },
    });
    fireEvent.change(screen.getByTestId("password-input"), {
      target: { value: "password123" },
    });
    
    // Submit form
    fireEvent.click(screen.getByTestId("submit-button"));
    
    // Wait for async submission
    await waitFor(() => {
      expect(mockSubmit).toHaveBeenCalledWith(
        "test@example.com",
        "password123"
      );
    });
  });
  
  it("shows error message on failed login", async () => {
    const mockSubmit = jest.fn().mockRejectedValue(new Error("Invalid credentials"));
    render(<LoginForm onSubmit={mockSubmit} />);
    
    fireEvent.change(screen.getByTestId("email-input"), {
      target: { value: "test@example.com" },
    });
    fireEvent.change(screen.getByTestId("password-input"), {
      target: { value: "wrongpassword" },
    });
    
    fireEvent.click(screen.getByTestId("submit-button"));
    
    await waitFor(() => {
      expect(screen.getByTestId("error-message")).toHaveTextContent("Invalid credentials");
    });
  });
});
```

## Query Priority

Use queries in this order (most preferred to least):

1. **`getByRole`** - Most accessible (screen readers use this)
2. **`getByLabelText`** - For form fields
3. **`getByPlaceholderText`** - If no label
4. **`getByText`** - For non-interactive elements
5. **`getByTestId`** - Last resort

```typescript
// Best: Query by role
const button = screen.getByRole("button", { name: /submit/i });

// Good: Query by label
const input = screen.getByLabelText(/email/i);

// Okay: Query by placeholder
const input = screen.getByPlaceholderText("Enter your email");

// Last resort: Test ID
const element = screen.getByTestId("unique-id");
```

## Common Mistakes

### Mistake 1: Using getByText for Interactive Elements

```typescript
// WRONG - Using text that might change
const button = screen.getByText("Login"); // Text could change!

// CORRECT - Use role
const button = screen.getByRole("button", { name: /login/i });
```

### Mistake 2: Not Waiting for Async Operations

```typescript
// WRONG - Not waiting for async render
render(<AsyncComponent />);
expect(screen.getByText("Loaded!")).toBeInTheDocument(); // Might fail!

// CORRECT - Wait for async
render(<AsyncComponent />);
await waitFor(() => {
  expect(screen.getByText("Loaded!")).toBeInTheDocument();
});
```

### Mistake 3: Testing Implementation Details

```typescript
// WRONG - Testing internal state
const { container } = render(<Counter />);
expect(container.querySelector(".count")).toHaveTextContent("0");

// CORRECT - Test what user sees/interacts with
expect(screen.getByText("Count: 0")).toBeInTheDocument();
```

## Summary

- Use React Testing Library to test components the user would use
- Query by role first, then label, then text, then test ID
- Use fireEvent to simulate user interactions
- Use waitFor to handle async operations
- Test what the user sees, not implementation details
- Import jest-dom matchers for better assertions

## Next Steps

- [mocking-next-navigation.md](./mocking-next-navigation.md) - Mocking Next.js navigation in tests
- [testing-client-components.md](./testing-client-components.md) - More client component testing patterns
