# Mocking API Calls

## Overview

When testing components that make API calls, you need to mock the network requests. MSW (Mock Service Worker) provides a powerful way to intercept HTTP requests at the network level, making tests more realistic.

## Prerequisites

- RTL testing knowledge
- Understanding of async testing

## Core Concepts

### Setting Up MSW

```bash
npm install msw --save-dev
```

### Creating Handlers

```typescript
// File: src/mocks/handlers.ts

import { http, HttpResponse } from 'msw';

export const handlers = [
  // GET request handler
  http.get('/api/users', () => {
    return HttpResponse.json([
      { id: 1, name: 'John Doe' },
      { id: 2, name: 'Jane Doe' },
    ]);
  }),

  // POST request handler
  http.post('/api/login', async ({ request }) => {
    const body = await request.json() as { email: string; password: string };
    
    if (body.email === 'test@example.com' && body.password === 'password') {
      return HttpResponse.json({ token: 'fake-jwt-token' });
    }
    
    return HttpResponse.json(
      { message: 'Invalid credentials' },
      { status: 401 }
    );
  }),
];
```

### Setting Up Server in Tests

```tsx
// File: src/mocks/server.ts

import { setupServer } from 'msw/node';
import { handlers } from './handlers';

export const server = setupServer(...handlers);
```

### Using in Tests

```tsx
// File: src/components/UserList.test.tsx

import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { describe, it, expect, beforeAll, afterAll } from 'vitest';
import { server } from '../mocks/server';
import UserList from './UserList';

beforeAll(() => server.listen());
afterAll(() => server.close());

describe('UserList', () => {
  it('loads and displays users', async () => {
    render(<UserList />);
    
    // Should show loading state
    expect(screen.getByText(/loading/i)).toBeInTheDocument();
    
    // Wait for users to load
    await waitFor(() => {
      expect(screen.getByText('John Doe')).toBeInTheDocument();
    });
  });

  it('handles API errors', async () => {
    // Override handler for this test
    server.use(
      http.get('/api/users', () => {
        return HttpResponse.json(
          { message: 'Failed to fetch' },
          { status: 500 }
        );
      })
    );
    
    render(<UserList />);
    
    await waitFor(() => {
      expect(screen.getByText(/error/i)).toBeInTheDocument();
    });
  });
});
```

## Key Takeaways

- MSW intercepts requests at network level
- Define handlers for each endpoint
- Use beforeAll/afterAll to set up server
- Override handlers for specific test cases

## What's Next

Continue to [Testing Forms](/10-testing/02-integration-testing/03-testing-forms.md) to learn about testing form components.