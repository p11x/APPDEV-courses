# Real-time Data with WebSockets

## Overview

WebSockets provide full-duplex communication between client and server, enabling real-time updates. This guide covers implementing WebSocket connections in React and handling reconnection.

## Prerequisites

- React hooks knowledge
- Understanding of WebSocket API

## Core Concepts

### WebSocket Hook

```tsx
// File: src/hooks/useWebSocket.ts

import { useEffect, useRef, useState, useCallback } from 'react';

interface UseWebSocketOptions {
  url: string;
  onMessage?: (data: any) => void;
  reconnectAttempts?: number;
  reconnectInterval?: number;
}

export function useWebSocket({ 
  url, 
  onMessage,
  reconnectAttempts = 5,
  reconnectInterval = 3000 
}: UseWebSocketOptions) {
  const [isConnected, setIsConnected] = useState(false);
  const [lastMessage, setLastMessage] = useState<any>(null);
  const wsRef = useRef<WebSocket | null>(null);
  const attemptsRef = useRef(0);

  const connect = useCallback(() => {
    const ws = new WebSocket(url);
    
    ws.onopen = () => {
      setIsConnected(true);
      attemptsRef.current = 0;
    };
    
    ws.onmessage = (event) => {
      const data = JSON.parse(event.data);
      setLastMessage(data);
      onMessage?.(data);
    };
    
    ws.onclose = () => {
      setIsConnected(false);
      
      // Attempt reconnection
      if (attemptsRef.current < reconnectAttempts) {
        attemptsRef.current++;
        setTimeout(connect, reconnectInterval);
      }
    };
    
    ws.onerror = () => {
      ws.close();
    };
    
    wsRef.current = ws;
  }, [url, onMessage, reconnectAttempts, reconnectInterval]);

  useEffect(() => {
    connect();
    
    return () => {
      wsRef.current?.close();
    };
  }, [connect]);

  const send = useCallback((data: any) => {
    wsRef.current?.send(JSON.stringify(data));
  }, []);

  return { isConnected, lastMessage, send };
}
```

### Usage in Component

```tsx
// File: src/components/Ticker.tsx

import { useWebSocket } from '../hooks/useWebSocket';

export function Ticker() {
  const { isConnected, lastMessage } = useWebSocket({
    url: 'wss://api.example.com/ticker',
    onMessage: (data) => {
      console.log('Received:', data);
    }
  });

  return (
    <div>
      <p>Status: {isConnected ? 'Connected' : 'Disconnected'}</p>
      {lastMessage && (
        <p>Price: ${lastMessage.price}</p>
      )}
    </div>
  );
}
```

## Key Takeaways

- Use useEffect for WebSocket lifecycle
- Implement reconnection logic
- Clean up on unmount
- Handle message parsing

## What's Next

Continue to [React with Node.js API](/11-real-world-projects/03-full-stack-integration/01-react-with-nodejs-api.md) to learn about full-stack integration.