# Testing Components with React Testing Library

## Overview

React Testing Library (RTL) encourages testing components as users would interact with them. Rather than testing implementation details, you test what users see and how they use your components.

## Prerequisites

- Vitest setup completed
- Basic React knowledge

## Core Concepts

### Query Methods

```tsx
// File: src/components/LoginForm.test.tsx

import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import LoginForm from './LoginForm';

describe('LoginForm', () => {
  // getBy - throws if not found
  // queryBy - returns null if not found
  // findBy - returns promise, waits for element

  it('renders email and password inputs', () => {
    render(<LoginForm onSubmit={vi.fn()} />);
    
    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/password/i)).toBeInTheDocument();
  });

  it('shows validation errors on empty submit', async () => {
    render(<LoginForm onSubmit={vi.fn()} />);
    
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));
    
    await waitFor(() => {
      expect(screen.getByText(/email is required/i)).toBeInTheDocument();
    });
  });

  it('calls onSubmit with form data', async () => {
    const handleSubmit = vi.fn();
    render(<LoginForm onSubmit={handleSubmit} />);
    
    fireEvent.change(screen.getByLabelText(/email/i), {
      target: { value: 'test@example.com' }
    });
    fireEvent.change(screen.getByLabelText(/password/i), {
      target: { value: 'password123' }
    });
    
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));
    
    await waitFor(() => {
      expect(handleSubmit).toHaveBeenCalledWith({
        email: 'test@example.com',
        password: 'password123'
      });
    });
  });
});
```

### Using user-event Library

```tsx
import userEvent from '@testing-library/user-event';

it('fills form with user-event', async () => {
  const user = userEvent.setup();
  const handleSubmit = vi.fn();
  
  render(<LoginForm onSubmit={handleSubmit} />);
  
  await user.type(screen.getByLabelText(/email/i), 'test@example.com');
  await user.type(screen.getByLabelText(/password/i), 'password123');
  await user.click(screen.getByRole('button', { name: /submit/i }));
  
  expect(handleSubmit).toHaveBeenCalled();
});
```

## Key Takeaways

- Use getBy, queryBy, findBy appropriately
- Test accessible components using roles
- Prefer userEvent over fireEvent
- Test what users see, not implementation

## What's Next

Continue to [Testing Custom Hooks](/10-testing/01-unit-testing/03-testing-custom-hooks.md) to learn about testing React hooks.