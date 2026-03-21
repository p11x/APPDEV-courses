# Custom Breadcrumbs

## What You'll Learn
- Add custom breadcrumbs for better debugging
- Track user actions
- Create useful context for errors

## Prerequisites
- Sentry configured

## Do I Need This Right Now?
Breadcrumbs create a timeline of events leading up to an error. They help you understand what the user was doing when something went wrong.

## Concept Explained Simply

Breadcrumbs are like the black box in an airplane — they record everything that happened before a crash. When an error occurs, you see all the actions leading up to it.

## Basic Breadcrumbs

```typescript
import * as Sentry from '@sentry/nextjs';

// Add a breadcrumb
Sentry.addBreadcrumb({
  message: 'User clicked submit button',
  category: 'ui.click',
  level: 'info',
});
```

## Complete Examples

### Tracking Form Submissions

```typescript
// components/CheckoutForm.tsx
'use client';

import * as Sentry from '@sentry/nextjs';

function trackFormSubmission(formData: FormData) {
  // Track each step
  Sentry.addBreadcrumb({
    message: 'Form submission started',
    category: 'form',
    level: 'info',
    data: {
      fields: Array.from(formData.keys()),
    },
  });
  
  // Track validation
  const email = formData.get('email');
  Sentry.addBreadcrumb({
    message: 'Email field captured',
    category: 'form.validation',
    level: 'info',
    data: { hasEmail: !!email },
  });
  
  // Track submission
  Sentry.addBreadcrumb({
    message: 'Form submitting',
    category: 'form',
    level: 'info',
  });
}

export function CheckoutForm() {
  async function handleSubmit(formData: FormData) {
    trackFormSubmission(formData);
    
    const result = await submitOrder(formData);
    
    if (result.success) {
      Sentry.addBreadcrumb({
        message: 'Order submitted successfully',
        category: 'form',
        level: 'info',
      });
    }
  }
  
  return <form action={handleSubmit}>{/* ... */}</form>;
}
```

### Tracking Navigation

```typescript
// components/NavigationTracker.tsx
'use client';

import { useEffect } from 'react';
import { usePathname, useSearchParams } from 'next/navigation';
import * as Sentry from '@sentry/nextjs';

export function NavigationTracker() {
  const pathname = usePathname();
  const searchParams = useSearchParams();
  
  useEffect(() => {
    // Track page views
    Sentry.addBreadcrumb({
      message: `Page view: ${pathname}`,
      category: 'navigation',
      level: 'info',
      data: {
        pathname,
        query: Object.fromEntries(searchParams.entries()),
      },
    });
  }, [pathname, searchParams]);
  
  return null;
}
```

### Tracking API Calls

```typescript
// lib/api-client.ts
import * as Sentry from '@sentry/nextjs';

export async function apiRequest<T>(
  endpoint: string,
  options?: RequestInit
): Promise<T> {
  const url = `${process.env.NEXT_PUBLIC_API_URL}${endpoint}`;
  
  Sentry.addBreadcrumb({
    message: `API request: ${options?.method || 'GET'} ${endpoint}`,
    category: 'api',
    level: 'info',
    data: {
      url,
      method: options?.method || 'GET',
    },
  });
  
  try {
    const response = await fetch(url, {
      ...options,
      headers: {
        'Content-Type': 'application/json',
        ...options?.headers,
      },
    });
    
    Sentry.addBreadcrumb({
      message: `API response: ${response.status} ${endpoint}`,
      category: 'api',
      level: 'info',
      data: {
        status: response.status,
        ok: response.ok,
      },
    });
    
    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }
    
    return response.json();
  } catch (error) {
    Sentry.addBreadcrumb({
      message: `API error: ${endpoint}`,
      category: 'api',
      level: 'error',
      data: {
        error: error instanceof Error ? error.message : 'Unknown error',
      },
    });
    
    throw error;
  }
}
```

## Automatic Breadcrumbs

Sentry automatically captures:

- Console logs (in browser)
- Fetch/XHR requests
- User interactions
- Page navigation

### Configuring Auto Breadcrumbs

```typescript
// sentry.client.config.ts
import * as Sentry from '@sentry/nextjs';

Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
  
  // Configure breadcrumbs
  defaultIntegrations: false, // Disable defaults
  
  integrations: [
    new Sentry.BrowserTracing({
      // Which breadcrumbs to capture
      tracePropagationTargets: ['localhost', /^https:\/\/yourdomain\.com/],
    }),
    new Sentry.Replay({
      // Replay errors with breadcrumbs
    }),
  ],
  
  // Or configure in init
  beforeBreadcrumb(breadcrumb) {
    // Filter out noisy breadcrumbs
    if (breadcrumb.category === 'ui.click') {
      // Keep these
    }
    
    return breadcrumb;
  },
});
```

## Common Mistakes

### Mistake #1: Too Many Breadcrumbs
```typescript
// Wrong: Every tiny action
Sentry.addBreadcrumb({ message: 'User hovered button' });
Sentry.addBreadcrumb({ message: 'User moved mouse' });
Sentry.addBreadcrumb({ message: 'User scrolled 1px' });
```

```typescript
// Correct: Meaningful actions only
Sentry.addBreadcrumb({ message: 'User clicked checkout' });
Sentry.addBreadcrumb({ message: 'Payment processed' });
```

### Mistake #2: Missing Context
```typescript
// Wrong: Not enough info
Sentry.addBreadcrumb({ message: 'API call failed' });
```

```typescript
// Correct: Add useful context
Sentry.addBreadcrumb({
  message: 'API call failed',
  category: 'api',
  level: 'error',
  data: {
    endpoint: '/users',
    status: 500,
    responseTime: '2.5s',
  },
});
```

### Mistake #3: Not Using Categories
```typescript
// Wrong: All in same category
Sentry.addBreadcrumb({ message: 'click', category: 'action' });
Sentry.addBreadcrumb({ message: 'fetch', category: 'action' });
Sentry.addBreadcrumb({ message: 'error', category: 'action' });
```

```typescript
// Correct: Use categories for filtering
Sentry.addBreadcrumb({ message: 'click', category: 'ui.click' });
Sentry.addBreadcrumb({ message: 'fetch', category: 'api.request' });
Sentry.addBreadcrumb({ message: 'error', category: 'error' });
```

## Summary
- Breadcrumbs show the path leading to an error
- Add meaningful events: form submissions, navigation, API calls
- Use categories to organize: 'ui.click', 'api', 'navigation'
- Include useful data in breadcrumb.data
- Don't add too many — focus on important actions
- View in Sentry under "Breadcrumbs" tab on error details

## Next Steps
- [alerts-and-notifications.md](./alerts-and-notifications.md) — Setting up alerts
