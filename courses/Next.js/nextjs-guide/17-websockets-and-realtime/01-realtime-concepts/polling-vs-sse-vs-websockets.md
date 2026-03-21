# Polling vs SSE vs WebSockets

## What You'll Learn
- Understanding different realtime methods
- When to use each approach
- Trade-offs of each method
- Choosing the right solution

## Prerequisites
- Basic understanding of HTTP
- Knowledge of client-server architecture
- Familiarity with async patterns

## Concept Explained Simply

When you need real-time updates in your app, you have three main options. Each works differently and is best for different situations. Think of them like different ways to get weather updates:

**Polling** — Like calling the weather station every 5 minutes to ask "Any updates?" Simple but not truly real-time.

**Server-Sent Events (SSE)** — Like subscribing to a weather alert service that calls YOU when there's a change. One-way communication, very efficient.

**WebSockets** — Like having a phone line that stays open. Both parties can send messages anytime. Two-way, most powerful but more complex.

## Complete Code Example

### Polling

```typescript
// Simple polling - checking every 10 seconds
"use client";

import { useEffect, useState } from "react";

export function PollingExample() {
  const [data, setData] = useState<any>(null);
  
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch("/api/status");
      const newData = await response.json();
      setData(newData);
    };
    
    // Fetch immediately
    fetchData();
    
    // Then poll every 10 seconds
    const interval = setInterval(fetchData, 10000);
    
    return () => clearInterval(interval);
  }, []);
  
  return <div>Status: {data?.status}</div>;
}
```

### Server-Sent Events (SSE)

```typescript
// Server-Sent Events - server pushes updates
"use client";

import { useEffect, useState } from "react";

export function SSEExample() {
  const [messages, setMessages] = useState<string[]>([]);
  
  useEffect(() => {
    const eventSource = new EventSource("/api/stream");
    
    eventSource.onmessage = (event) => {
      setMessages((prev) => [...prev, event.data]);
    };
    
    eventSource.onerror = () => {
      eventSource.close();
    };
    
    return () => eventSource.close();
  }, []);
  
  return (
    <ul>
      {messages.map((msg, i) => (
        <li key={i}>{msg}</li>
      ))}
    </ul>
  );
}
```

```typescript
// app/api/stream/route.ts - SSE endpoint
import { NextResponse } from "next/server";

export async function GET() {
  const stream = new ReadableStream({
    start(controller) {
      const encoder = new TextEncoder();
      
      // Send initial message
      controller.enqueue(encoder.encode("data: Hello\n\n"));
      
      // Send updates periodically
      const interval = setInterval(() => {
        const message = `data: Update at ${new Date().toISOString()}\n\n`;
        controller.enqueue(encoder.encode(message));
      }, 5000);
      
      // Clean up on close
      return () => clearInterval(interval);
    },
  });
  
  return new NextResponse(stream, {
    headers: {
      "Content-Type": "text/event-stream",
      "Cache-Control": "no-cache",
      "Connection": "keep-alive",
    },
  });
}
```

### WebSockets

```typescript
// WebSocket client
"use client";

import { useEffect, useState } from "react";

export function WebSocketExample() {
  const [messages, setMessages] = useState<string[]>([]);
  const [socket, setSocket] = useState<WebSocket | null>(null);
  
  useEffect(() => {
    const ws = new WebSocket("wss://your-server.com/ws");
    
    ws.onopen = () => {
      console.log("Connected!");
      ws.send(JSON.stringify({ type: "hello" }));
    };
    
    ws.onmessage = (event) => {
      setMessages((prev) => [...prev, event.data]);
    };
    
    ws.onclose = () => {
      console.log("Disconnected");
    };
    
    setSocket(ws);
    
    return () => ws.close();
  }, []);
  
  const sendMessage = () => {
    socket?.send(JSON.stringify({ type: "message", content: "Hello!" }));
  };
  
  return (
    <div>
      <button onClick={sendMessage}>Send</button>
      <ul>
        {messages.map((msg, i) => (
          <li key={i}>{msg}</li>
        ))}
      </ul>
    </div>
  );
}
```

## Comparison

| Feature | Polling | SSE | WebSockets |
|---------|---------|-----|------------|
| **Setup** | Very Easy | Easy | Complex |
| **Server → Client** | Request/Response | Server Push | Bidirectional |
| **Client → Server** | Request/Response | Via HTTP | Direct |
| **Browser Support** | All | Modern | All |
| **Connections** | Many (each poll) | One | One |
| **Best For** | Infrequent updates | One-way real-time | Chat, games |

## When to Use Each

### Use Polling When:
- Updates are infrequent (every few minutes)
- You need GET request caching
- Simple implementation is priority
- Server can't maintain connections

### Use SSE When:
- Only server needs to send updates
- You need automatic reconnection
- Simpler than WebSockets
- Works through firewalls easily

### Use WebSockets When:
- Both sides need to send frequently
- Ultra-low latency needed
- Building chat or game
- Complex bidirectional communication

## Common Mistakes

### Mistake 1: Using Polling for Real-Time

```typescript
// WRONG - Polling every second is basically WebSocket but worse
setInterval(async () => {
  const data = await fetchData();
  updateUI(data);
}, 1000);

// This creates too many requests!

// CORRECT - Use SSE or WebSockets
const source = new EventSource("/api/updates");
source.onmessage = (e) => updateUI(JSON.parse(e.data));
```

### Mistake 2: Not Handling Connection Failures

```typescript
// WRONG - No error handling
const source = new EventSource("/api/stream");
source.onmessage = (e) => handleData(e.data);

// CORRECT - Handle errors and reconnect
const source = new EventSource("/api/stream");
source.onerror = () => {
  console.log("Connection lost, retrying...");
  // SSE has built-in retry!
};
```

### Mistake 3: Using WebSockets for Simple Updates

```typescript
// WRONG - Overengineering simple needs
const ws = new WebSocket("wss://api.com/ws");
ws.onmessage = (e) => setData(e.data);
// When SSE would work fine!

// CORRECT - Use SSE for server→client only
const source = new EventSource("/api/updates");
source.onmessage = (e) => setData(e.data);
```

## Summary

- Polling is simple but inefficient for real-time
- SSE is perfect for server-to-client updates
- WebSockets are best for bidirectional communication
- Choose based on your actual needs
- Don't overengineer if polling works

## Next Steps

- [sse-with-route-handlers.md](../02-server-sent-events/sse-with-route-handlers.md) - Implementing SSE in Next.js
- [websockets-with-partykit.md](../03-websockets/websockets-with-partykit.md) - WebSocket solutions
