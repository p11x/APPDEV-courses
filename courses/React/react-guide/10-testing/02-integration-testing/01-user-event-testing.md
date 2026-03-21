# User Event Testing

## Overview

User Event Testing simulates actual user interactions more accurately than fireEvent. It handles complex interactions like typing, clicking, and keyboard navigation in ways that mirror real browser behavior.

## Prerequisites

- RTL testing basics
- Understanding of component testing

## Core Concepts

### Setting Up User Event

```bash
npm install @testing-library/user-event
```

### Testing User Interactions

```tsx
// File: src/components/LoginForm.test.tsx

import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { describe, it, expect, vi } from 'vitest';
import LoginForm from './LoginForm';

describe('LoginForm', () => {
  it('fills form and submits', async () => {
    const user = userEvent.setup();
    const handleSubmit = vi.fn();
    
    render(<LoginForm onSubmit={handleSubmit} />);
    
    // Type into inputs
    await user.type(screen.getByLabelText(/email/i), 'test@example.com');
    await user.type(screen.getByLabelText(/password/i), 'password123');
    
    // Click submit button
    await user.click(screen.getByRole('button', { name: /submit/i }));
    
    // Verify submission
    expect(handleSubmit).toHaveBeenCalledWith({
      email: 'test@example.com',
      password: 'password123'
    });
  });

  it('shows validation errors', async () => {
    const user = userEvent.setup();
    const handleSubmit = vi.fn();
    
    render(<LoginForm onSubmit={handleSubmit} />);
    
    // Try to submit without filling
    await user.click(screen.getByRole('button', { name: /submit/i }));
    
    // Verify error shown
    expect(await screen.findByText(/email is required/i)).toBeInTheDocument();
    expect(handleSubmit).not.toHaveBeenCalled();
  });

  it('clears form after submission', async () => {
    const user = userEvent.setup();
    const handleSubmit = vi.fn();
    
    render(<LoginForm onSubmit={handleSubmit} />);
    
    await user.type(screen.getByLabelText(/email/i), 'test@example.com');
    await user.click(screen.getByRole('button', { name: /submit/i }));
    
    // Form should be cleared
    expect(screen.getByLabelText(/email/i)).toHaveValue('');
  });
});
```

### Keyboard Navigation

```tsx
it('navigates form with keyboard', async () => {
  const user = userEvent.setup();
  
  render(<Form />);
  
  // Tab through form
  await user.tab();
  expect(screen.getByLabelText(/email/i)).toHaveFocus();
  
  await user.keyboard('test@example.com');
  await user.tab();
  expect(screen.getByLabelText(/password/i)).toHaveFocus();
});
```

## Key Takeaways

- userEvent simulates real user behavior
- Use userEvent.setup() for proper async handling
- Test keyboard navigation and tab order
- More accurate than fireEvent

## What's Next

Continue to [Mocking API Calls](/10-testing/02-integration-testing/02-mocking-api-calls.md) to learn about mocking HTTP requests.