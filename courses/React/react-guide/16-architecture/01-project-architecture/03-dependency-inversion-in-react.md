# Dependency Inversion in React

## Overview
Dependency Inversion is a principle that states high-level modules should not depend on low-level modules. Both should depend on abstractions. In React, this means depending on interfaces/contracts rather than concrete implementations, making code more testable and maintainable.

## Prerequisites
- TypeScript basics
- React patterns

## Core Concepts

### Using Context for Dependency Injection

```typescript
// [File: src/context/AnalyticsContext.tsx]
import { createContext, useContext } from 'react';

// Define analytics interface
interface Analytics {
  track(event: string, properties?: Record<string, any>): void;
  identify(userId: string, traits?: Record<string, any>): void;
}

// Default no-op implementation
const noopAnalytics: Analytics = {
  track: () => {},
  identify: () => {},
};

const AnalyticsContext = createContext<Analytics>(noopAnalytics);

export function useAnalytics(): Analytics {
  return useContext(AnalyticsContext);
}

export function AnalyticsProvider({ 
  analytics, 
  children 
}: { 
  analytics: Analytics; 
  children: React.ReactNode; 
}) {
  return (
    <AnalyticsContext.Provider value={analytics}>
      {children}
    </AnalyticsContext.Provider>
  );
}
```

### Swappable Implementations

```typescript
// [File: src/lib/analytics/Analytics.ts]
// Production implementation
export class ProductionAnalytics implements Analytics {
  track(event: string, properties?: Record<string, any>) {
    // Send to analytics service
    console.log('Track:', event, properties);
  }
  
  identify(userId: string, traits?: Record<string, any>) {
    console.log('Identify:', userId, traits);
  }
}

// Mock implementation for testing
export class MockAnalytics implements Analytics {
  track(event: string, properties?: Record<string, any>) {
    // Do nothing
  }
  
  identify(userId: string, traits?: Record<string, any>) {
    // Do nothing
  }
}
```

## Key Takeaways
- Use context for dependency injection
- Define interfaces for services
- Create swappable implementations

## What's Next
This completes the Architecture module. Continue to [React Reconciliation Deep Dive](17-interview-prep/01-core-concepts/01-react-reconciliation-deep-dive.md) to learn about React internals.