# Observer Pattern with Events

## Overview
The Observer Pattern allows components to subscribe to and receive events from other components without tight coupling. This pattern is useful for cross-feature communication in React applications.

## Prerequisites
- React hooks
- Event handling

## Core Concepts

### Event Bus Hook

```typescript
// [File: src/hooks/useEventBus.ts]
type EventCallback<T = any> = (data: T) => void;

class EventBus {
  private events: Map<string, EventCallback[]> = new Map();
  
  subscribe<T>(event: string, callback: EventCallback<T>): () => void {
    if (!this.events.has(event)) {
      this.events.set(event, []);
    }
    this.events.get(event)!.push(callback);
    
    return () => {
      const callbacks = this.events.get(event)!;
      const index = callbacks.indexOf(callback);
      if (index > -1) callbacks.splice(index, 1);
    };
  }
  
  publish<T>(event: string, data: T): void {
    const callbacks = this.events.get(event);
    if (callbacks) {
      callbacks.forEach(cb => cb(data));
    }
  }
}

export const eventBus = new EventBus();

export function useEventBus() {
  return eventBus;
}
```

### Using Event Bus

```tsx
// [File: src/components/NotificationList.tsx]
'use client';

import { useEventBus } from '@/hooks/useEventBus';
import { useEffect, useState } from 'react';

interface Notification {
  id: string;
  message: string;
}

export function NotificationList() {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const eventBus = useEventBus();
  
  useEffect(() => {
    return eventBus.subscribe<Notification>('notification', (notification) => {
      setNotifications(prev => [...prev, notification]);
    });
  }, []);
  
  return (
    <ul>
      {notifications.map(n => (
        <li key={n.id}>{n.message}</li>
      ))}
    </ul>
  );
}
```

## Key Takeaways
- Use event bus for cross-component communication
- Always unsubscribe in cleanup function
- Consider using context instead for simpler cases

## What's Next
This completes the Architecture module. Continue to [React Reconciliation Deep Dive](17-interview-prep/01-core-concepts/01-react-reconciliation-deep-dive.md) to learn about React internals.