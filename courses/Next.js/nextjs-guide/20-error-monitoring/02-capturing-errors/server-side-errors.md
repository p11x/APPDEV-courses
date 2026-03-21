# Server-Side Errors

## What You'll Learn
- Capture server-side errors in Next.js
- Configure error boundaries
- Handle API route errors

## Prerequisites
- Sentry installed in your project

## Do I Need This Right Now?
Server errors can happen in API routes, Server Components, and Server Actions. Knowing how to capture them helps you fix issues before users complain.

## Concept Explained Simply

Server errors are like problems in the kitchen that customers never see — but the chef (you) needs to know about them! Sentry catches these errors automatically and alerts you.

## Automatic Error Capture

Sentry for Next.js automatically captures errors in:

- API Routes
- Server Components
- Server Actions
- Middleware

### Example: API Route Error

```typescript
// app/api/users/route.ts
import { NextResponse } from 'next/server';

export async function GET() {
  // This will be automatically captured by Sentry
  const response = await fetch('https://api.example.com/users');
  
  if (!response.ok) {
    throw new Error(`Failed to fetch users: ${response.status}`);
  }
  
  const users = await response.json();
  return NextResponse.json(users);
}
```

### Example: Server Component Error

```typescript
// app/users/page.tsx
async function getUsers() {
  const response = await fetch('https://api.example.com/users');
  
  if (!response.ok) {
    throw new Error('Failed to fetch users');
  }
  
  return response.json();
}

export default async function UsersPage() {
  const users = await getUsers();
  
  return (
    <div>
      {users.map((user: any) => (
        <div key={user.id}>{user.name}</div>
      ))}
    </div>
  );
}
```

## Manual Error Capture

You can also manually capture errors:

```typescript
// app/api/manual-error/route.ts
import * as Sentry from '@sentry/nextjs';

export async function GET() {
  try {
    // Risky operation
    const result = await riskyOperation();
    return Response.json({ result });
  } catch (error) {
    // Capture manually with context
    Sentry.captureException(error, {
      extra: {
        operation: 'riskyOperation',
        timestamp: new Date().toISOString(),
      },
    });
    
    return Response.json(
      { error: 'Something went wrong' },
      { status: 500 }
    );
  }
}

async function riskyOperation() {
  throw new Error('Something went wrong!');
}
```

## Adding Context

```typescript
// Add user information
Sentry.setUser({
  id: 'user-123',
  email: 'user@example.com',
  username: 'johndoe',
});

// Add extra context
Sentry.setExtra('key', 'value');

// Add tags (for filtering)
Sentry.setTag('feature', 'checkout');
```

## Error Handling in Middleware

```typescript
// middleware.ts
import * as Sentry from '@sentry/nextjs';

export function middleware(request: Request) {
  try {
    // Something that might fail
    const response = await fetch('https://api.example.com/validate');
    
    if (!response.ok) {
      throw new Error('Validation failed');
    }
    
    return NextResponse.next();
  } catch (error) {
    // Capture the error
    Sentry.captureException(error);
    
    return NextResponse.json(
      { error: 'Middleware error' },
      { status: 500 }
    );
  }
}
```

## Custom Error Pages

```typescript
// app/global-error.tsx
'use client';

import * as Sentry from '@sentry/nextjs';
import { useEffect } from 'react';

export default function GlobalError({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    // Report to Sentry
    Sentry.captureException(error);
  }, [error]);

  return (
    <html>
      <body>
        <h2>Something went wrong!</h2>
        <p>{error.message}</p>
        <button onClick={() => reset()}>Try again</button>
      </body>
    </html>
  );
}
```

## Common Mistakes

### Mistake #1: Not Rethrowing in API Routes
```typescript
// Wrong: Catching error without rethrowing
export async function GET() {
  try {
    // risky operation
  } catch (error) {
    // Just log - Sentry won't capture!
    console.log(error);
    return Response.json({ error: 'Failed' });
  }
}
```

```typescript
// Correct: Rethrow so Sentry captures it
export async function GET() {
  try {
    // risky operation
  } catch (error) {
    Sentry.captureException(error);
    throw error; // Re-throw!
  }
}
```

### Mistake #2: Missing Error Boundaries
```typescript
// Without error boundary - whole page crashes
// app/page.tsx
export default function Page() {
  throw new Error('Crash!');
  // No way to recover gracefully!
}
```

```typescript
// With error boundary - graceful fallback
// app/error.tsx
'use client';

import { useEffect } from 'react';
import * as Sentry from '@sentry/nextjs';

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    Sentry.captureException(error);
  }, [error]);

  return (
    <div>
      <h2>Something went wrong</h2>
      <button onClick={() => reset()}>Try again</button>
    </div>
  );
}
```

### Mistake #3: Not Setting User Context
```typescript
// Wrong: Anonymous errors hard to debug
Sentry.captureException(error);
// No way to know which user had this issue!
```

```typescript
// Correct: Always include user info when available
Sentry.setUser({
  id: session.user.id,
  email: session.user.email,
});

Sentry.captureException(error);
```

## Summary
- Sentry automatically captures errors in API routes and Server Components
- Use `Sentry.captureException()` for manual capture
- Add context with `setUser()`, `setTag()`, `setExtra()`
- Create error.tsx for graceful error handling
- Always re-throw errors in try/catch if you want Sentry to capture them
- Middleware errors are captured but need special handling

## Next Steps
- [client-side-errors.md](./client-side-errors.md) — Capturing client errors
