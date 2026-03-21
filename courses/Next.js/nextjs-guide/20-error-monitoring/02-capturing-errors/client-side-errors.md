# Client-Side Errors

## What You'll Learn
- Capture React errors
- Handle client-side JavaScript errors
- Use error boundaries effectively

## Prerequisites
- Sentry installed with client config

## Do I Need This Right Now?
Client errors directly affect user experience. Understanding how to capture them helps you fix issues that users encounter in their browsers.

## Concept Explained Simply

Client errors are like problems customers see at the restaurant table. You need to know exactly what they saw to fix it. Sentry captures these errors even when users don't report them!

## Automatic Client Capture

Sentry automatically captures:

- React error boundaries
- Uncaught JavaScript errors
- Promise rejections (unhandled rejections)

### React Error Boundaries

```typescript
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
    // Report to Sentry
    Sentry.captureException(error);
  }, [error]);

  return (
    <div className="flex flex-col items-center justify-center min-h-screen">
      <h2 className="text-2xl font-bold mb-4">Something went wrong!</h2>
      <p className="text-gray-600 mb-4">
        {error.message || 'An unexpected error occurred'}
      </p>
      <button
        onClick={() => reset()}
        className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
      >
        Try again
      </button>
    </div>
  );
}
```

### Global Error Handler

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
    Sentry.captureException(error);
  }, [error]);

  return (
    <html>
      <body>
        <h1>Critical Error</h1>
        <p>{error.message}</p>
        <button onClick={() => reset()}>Reload</button>
      </body>
    </html>
  );
}
```

## Manual Error Capture

```typescript
// components/RiskyComponent.tsx
'use client';

import * as Sentry from '@sentry/nextjs';
import { useState } from 'react';

export function RiskyComponent() {
  const [data, setData] = useState<string | null>(null);

  const handleClick = async () => {
    try {
      const response = await fetch('/api/risky-operation');
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const result = await response.json();
      setData(result.message);
    } catch (error) {
      // Capture the error with context
      Sentry.captureException(error, {
        tags: {
          component: 'RiskyComponent',
          operation: 'handleClick',
        },
        extra: {
          state: 'button_clicked',
        },
      });
      
      // Still show error to user
      alert('Something went wrong. We\'ve been notified.');
    }
  };

  return (
    <div>
      <button onClick={handleClick}>Do Something Risky</button>
      {data && <p>{data}</p>}
    </div>
  );
}
```

## Capturing Component Errors

```typescript
// components/ErrorBoundary.tsx
'use client';

import { Component, ReactNode } from 'react';
import * as Sentry from '@sentry/nextjs';

interface Props {
  children: ReactNode;
  fallback?: ReactNode;
}

interface State {
  hasError: boolean;
  error?: Error;
}

export class ErrorBoundary extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error: Error): State {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, errorInfo: any) {
    // Capture the error
    Sentry.captureException(error, {
      extra: {
        componentStack: errorInfo.componentStack,
      },
    });
  }

  render() {
    if (this.state.hasError) {
      return this.props.fallback || (
        <div className="p-4 bg-red-50 border border-red-200 rounded">
          <h3 className="font-bold text-red-800">Something went wrong</h3>
          <p className="text-red-600">{this.state.error?.message}</p>
        </div>
      );
    }

    return this.props.children;
  }
}
```

## Tracking Non-Error Events

```typescript
// Log messages that aren't errors
Sentry.captureMessage('User clicked button', 'info');

Sentry.captureMessage('Slow page load', 'warning');
```

## Common Mistakes

### Mistake #1: Not Using Error Boundaries
```typescript
// Without error boundary - whole app crashes
export default function Page() {
  return <RiskyComponent />;
}
```

```typescript
// With error boundary - graceful handling
import { ErrorBoundary } from '@/components/ErrorBoundary';

export default function Page() {
  return (
    <ErrorBoundary>
      <RiskyComponent />
    </ErrorBoundary>
  );
}
```

### Mistake #2: Not Filtering Noisy Errors
```typescript
// Wrong: Everything gets captured
Sentry.captureException(error);
```

```typescript
// Correct: Filter common non-critical errors
// sentry.client.config.ts
beforeSend(event) {
  const error = event.exception?.values?.[0];
  
  // Ignore these common errors
  if (error?.value === 'ResizeObserver loop limit exceeded') {
    return null;
  }
  
  if (error?.value === 'Script error.') {
    return null;
  }
  
  return event;
}
```

### Mistake #3: Not Setting User Context
```typescript
// Create a provider to set user context
// components/SentryProvider.tsx
'use client';

import { useEffect } from 'react';
import * as Sentry from '@sentry/nextjs';
import { useUser } from '@/hooks/useUser';

export function SentryProvider({ children }: { children: React.ReactNode }) {
  const { user } = useUser();
  
  useEffect(() => {
    if (user) {
      Sentry.setUser({
        id: user.id,
        email: user.email,
        username: user.name,
      });
    } else {
      Sentry.setUser(null); // Clear on logout
    }
  }, [user]);
  
  return <>{children}</>;
}
```

## Summary
- Client errors captured through error.tsx and global-error.tsx
- Use ErrorBoundary component for component-level errors
- Add context with tags and extra data
- Filter common non-critical errors in beforeSend
- Set user context in a provider for better debugging
- Use captureMessage for non-error events you want to track

## Next Steps
- [server-action-errors.md](./server-action-errors.md) — Server Action error handling
