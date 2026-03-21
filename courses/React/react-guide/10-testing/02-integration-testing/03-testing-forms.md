# Testing Forms

## Overview

Forms are critical parts of React applications. Testing forms involves verifying rendering, validation, submission, and error handling. This guide covers testing controlled forms, validation, and async submissions.

## Prerequisites

- RTL basics
- User event testing

## Core Concepts

### Testing React Hook Form

```tsx
// File: src/components/RegisterForm.test.tsx

import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { describe, it, expect, vi } from 'vitest';
import { rest } from 'msw';
import { server } from '../mocks/server';
import RegisterForm from './RegisterForm';

describe('RegisterForm', () => {
  it('renders all form fields', () => {
    render(<RegisterForm />);
    
    expect(screen.getByLabelText(/username/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/password/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/confirm password/i)).toBeInTheDocument();
  });

  it('shows validation errors', async () => {
    const user = userEvent.setup();
    render(<RegisterForm />);
    
    // Submit empty form
    await user.click(screen.getByRole('button', { name: /register/i }));
    
    // Check validation errors
    expect(await screen.findByText(/username is required/i)).toBeInTheDocument();
    expect(await screen.findByText(/email is required/i)).toBeInTheDocument();
  });

  it('validates password match', async () => {
    const user = userEvent.setup();
    render(<RegisterForm />);
    
    await user.type(screen.getByLabelText(/password/i), 'password123');
    await user.type(screen.getByLabelText(/confirm password/i), 'different');
    await user.click(screen.getByRole('button', { name: /register/i }));
    
    expect(await screen.findByText(/passwords do not match/i)).toBeInTheDocument();
  });

  it('submits form successfully', async () => {
    const user = userEvent.setup();
    const onSuccess = vi.fn();
    
    render(<RegisterForm onSuccess={onSuccess} />);
    
    await user.type(screen.getByLabelText(/username/i), 'john');
    await user.type(screen.getByLabelText(/email/i), 'john@example.com');
    await user.type(screen.getByLabelText(/password/i), 'password123');
    await user.type(screen.getByLabelText(/confirm password/i), 'password123');
    await user.click(screen.getByRole('button', { name: /register/i }));
    
    await waitFor(() => {
      expect(onSuccess).toHaveBeenCalledWith({
        username: 'john',
        email: 'john@example.com',
        password: 'password123'
      });
    });
  });

  it('handles API errors', async () => {
    // Override handler to return error
    server.use(
      rest.post('/api/register', (req, res, ctx) => {
        return res(ctx.status(400), ctx.json({ message: 'Email already exists' }));
      })
    );
    
    const user = userEvent.setup();
    render(<RegisterForm />);
    
    await user.type(screen.getByLabelText(/username/i), 'john');
    await user.type(screen.getByLabelText(/email/i), 'existing@example.com');
    await user.type(screen.getByLabelText(/password/i), 'password123');
    await user.type(screen.getByLabelText(/confirm password/i), 'password123');
    await user.click(screen.getByRole('button', { name: /register/i }));
    
    expect(await screen.findByText(/email already exists/i)).toBeInTheDocument();
  });
});
```

## Key Takeaways

- Test field rendering and labels
- Test validation messages
- Test password matching
- Test successful submission
- Test API error handling

## What's Next

Continue to [Playwright Setup](/10-testing/03-e2e-testing/01-playwright-setup.md) to learn about end-to-end testing.