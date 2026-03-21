# SSE Client Setup

## What You'll Learn
- Properly configure EventSource on the client
- Handle reconnection and error states
- Manage connection lifecycle in React components

## Prerequisites
- Understanding of SSE basics from previous files
- Knowledge of React hooks (useEffect, useState)

## Do I Need This Right Now?
This is essential when implementing any SSE-based feature in your frontend. The browser's EventSource API is simple but has nuances that, if not handled correctly, will lead to poor user experience during network issues.

## Concept Explained Simply

The browser provides a built-in way to receive Server-Sent Events called `EventSource`. Think of it like subscribing to a newsletter — you open the connection (subscribe), and then articles (events) are delivered to you automatically. If the connection drops, the browser automatically tries to reconnect, but you need to handle this gracefully in your UI.

## Complete Code Example

Here's a complete, production-ready SSE hook and component:

```typescript
// app/hooks/useSSE.ts
'use client';

import { useEffect, useState, useCallback, useRef } from 'react';

interface UseSSEOptions<T> {
  url: string;
  onMessage?: (data: T) => void;
  onError?: (error: Event) => void;
  onOpen?: () => void;
  enabled?: boolean;
}

interface UseSSEReturn<T> {
  data: T | null;
  isConnected: boolean;
  error: string | null;
  reconnect: () => void;
}

export function useSSE<T>({
  url,
  onMessage,
  onError,
  onOpen,
  enabled = true,
}: UseSSEOptions<T>): UseSSEReturn<T> {
  const [data, setData] = useState<T | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const eventSourceRef = useRef<EventSource | null>(null);

  const connect = useCallback(() => {
    // Clean up existing connection first
    if (eventSourceRef.current) {
      eventSourceRef.current.close();
    }

    if (!enabled) return;

    // Create new EventSource connection
    const eventSource = new EventSource(url);
    eventSourceRef.current = eventSource;

    eventSource.onopen = () => {
      setIsConnected(true);
      setError(null);
      onOpen?.();
    };

    eventSource.onmessage = (event) => {
      try {
        const parsedData = JSON.parse(event.data) as T;
        setData(parsedData);
        onMessage?.(parsedData);
      } catch (err) {
        console.error('Failed to parse SSE data:', err);
      }
    };

    eventSource.onerror = (err) => {
      console.error('SSE error:', err);
      setIsConnected(false);
      setError('Connection lost. Reconnecting...');
      onError?.(err);
      
      // EventSource automatically tries to reconnect
      // but we update UI to show disconnected state
    };
  }, [url, enabled, onMessage, onError, onOpen]);

  // Connect on mount or when URL/enabled changes
  useEffect(() => {
    connect();

    // Cleanup on unmount
    return () => {
      eventSourceRef.current?.close();
    };
  }, [connect]);

  const reconnect = useCallback(() => {
    setError(null);
    connect();
  }, [connect]);

  return { data, isConnected, error, reconnect };
}
```

Now let's use this hook in a notification component:

```typescript
// app/components/NotificationStream.tsx
'use client';

import { useSSE } from '@/hooks/useSSE';

interface Notification {
  id: string;
  message: string;
  type: 'info' | 'success' | 'warning' | 'error';
  timestamp: number;
}

export default function NotificationStream() {
  const [notifications, setNotifications] = useState<Notification[]>([]);

  const { isConnected, error, reconnect } = useSSE<Notification[]>({
    url: '/api/notifications/stream',
    enabled: true,
    onMessage: (newNotifications) => {
      // Prepend new notifications to show newest first
      setNotifications(prev => [...newNotifications, ...prev].slice(0, 50));
    },
    onOpen: () => console.log('Connected to notification stream'),
    onError: (err) => console.error('Notification stream error:', err),
  });

  return (
    <div className="max-w-md mx-auto p-4">
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-lg font-bold">Notifications</h2>
        <div className="flex items-center gap-2">
          <span className={`w-2 h-2 rounded-full ${
            isConnected ? 'bg-green-500' : 'bg-red-500'
          }`} />
          <span className="text-sm text-gray-600">
            {isConnected ? 'Live' : 'Disconnected'}
          </span>
        </div>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded flex items-center justify-between">
          <span className="text-red-700 text-sm">{error}</span>
          <button
            onClick={reconnect}
            className="px-3 py-1 bg-red-600 text-white text-sm rounded hover:bg-red-700"
          >
            Retry
          </button>
        </div>
      )}

      <div className="space-y-2 max-h-96 overflow-y-auto">
        {notifications.length === 0 ? (
          <p className="text-gray-500 text-center py-8">
            No notifications yet
          </p>
        ) : (
          notifications.map((notification) => (
            <div
              key={notification.id}
              className={`p-3 rounded border-l-4 ${
                notification.type === 'success' ? 'bg-green-50 border-green-500' :
                notification.type === 'warning' ? 'bg-yellow-50 border-yellow-500' :
                notification.type === 'error' ? 'bg-red-50 border-red-500' :
                'bg-blue-50 border-blue-500'
              }`}
            >
              <p className="text-sm">{notification.message}</p>
              <p className="text-xs text-gray-500 mt-1">
                {new Date(notification.timestamp).toLocaleTimeString()}
              </p>
            </div>
          ))
        )}
      </div>
    </div>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `new EventSource(url)` | Opens SSE connection to URL | Creates persistent connection to server |
| `eventSource.onopen` | Fires when connection established | Use to update UI to show connected state |
| `eventSource.onmessage` | Fires for each SSE message | Parse and handle incoming data here |
| `eventSource.onerror` | Fires on connection errors | Show error UI and trigger reconnection |
| `eventSource.close()` | Closes the SSE connection | Clean up on component unmount |
| `useRef` | Stores EventSource across renders | Prevents creating multiple connections |
| `.slice(0, 50)` | Limits stored notifications | Prevents memory issues with many notifications |

## Common Mistakes

### Mistake #1: Creating Multiple EventSource Instances
```typescript
// Wrong: Creates new connection on every render
useEffect(() => {
  const eventSource = new EventSource('/api/stream');
  // Missing cleanup!
}, []);
```

```typescript
// Correct: Use ref to store and clean up properly
const eventSourceRef = useRef<EventSource | null>(null);

useEffect(() => {
  eventSourceRef.current = new EventSource('/api/stream');
  
  return () => {
    eventSourceRef.current?.close();
  };
}, []);
```

### Mistake #2: Not Handling Error State
```typescript
// Wrong: User has no idea if connection failed
eventSource.onmessage = (event) => {
  setData(JSON.parse(event.data));
};
```

```typescript
// Correct: Show error state to user
eventSource.onerror = (err) => {
  setError('Connection lost');
  setIsConnected(false);
};
```

### Mistake #3: Memory Leak from Accumulating Data
```typescript
// Wrong: Notifications keep accumulating forever
setNotifications(prev => [...prev, newNotification]);
```

```typescript
// Correct: Limit stored notifications
setNotifications(prev => [...prev, newNotification].slice(0, 50));
```

## Summary
- Use EventSource to receive SSE from server
- Always close connections in cleanup to prevent leaks
- Handle onopen, onmessage, and onerror events
- Show connection state in UI for better UX
- Consider using a custom hook to encapsulate SSE logic
- Limit stored data to prevent memory issues
- Browser auto-reconnects but you should show error state

## Next Steps
- [websockets-with-partykit.md](../03-websockets/websockets-with-partykit.md) — True bidirectional communication
