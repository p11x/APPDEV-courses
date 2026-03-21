# SSE with Route Handlers

## What You'll Learn
- Create Server-Sent Events (SSE) endpoints in Next.js
- Stream data from server to client in real-time
- Handle client disconnection properly

## Prerequisites
- Understanding of Next.js API Route Handlers
- Basic knowledge of streams in JavaScript

## Do I Need This Right Now?
If you're building any feature that needs the server to push updates to the client — like a live notification system, real-time dashboard, or live feed — this is essential. However, if your app only needs client-to-server communication, you can use regular Server Actions instead.

## Concept Explained Simply

Server-Sent Events are like having an open telephone line where the server can call you whenever there's new information. Unlike a phone call where you have to ask and wait for an answer (regular HTTP requests), SSE lets the server say "hey, there's something new!" without you asking. It's a one-way street — the server talks, the client listens.

In Next.js, we create SSE endpoints using Route Handlers that return a streaming response. The connection stays open, and we can send data whenever we want.

## Complete Code Example

Here's a complete SSE endpoint that streams stock price updates:

```typescript
// app/api/stock-prices/route.ts
import { NextRequest } from 'next/server';

// Force this route to be dynamic - required for streaming
export const dynamic = 'force-dynamic';

export async function GET(request: NextRequest) {
  const encoder = new TextEncoder();
  
  // Create a readable stream to send events
  const stream = new ReadableStream({
    async start(controller) {
      // Simulate stock price updates
      const symbols = ['AAPL', 'GOOGL', 'MSFT', 'AMZN'];
      
      // Send initial data
      const initialData = symbols.map(symbol => ({
        symbol,
        price: (Math.random() * 1000 + 100).toFixed(2),
        timestamp: Date.now(),
      }));
      
      controller.enqueue(
        encoder.encode(`data: ${JSON.stringify(initialData)}\n\n`)
      );

      // Update prices every 2 seconds
      const interval = setInterval(() => {
        const updates = symbols.map(symbol => ({
          symbol,
          price: (Math.random() * 1000 + 100).toFixed(2),
          timestamp: Date.now(),
        }));
        
        controller.enqueue(
          encoder.encode(`data: ${JSON.stringify(updates)}\n\n`)
        );
      }, 2000);

      // Clean up when client disconnects
      request.signal.addEventListener('abort', () => {
        clearInterval(interval);
        controller.close();
        console.log('Client disconnected, cleaning up...');
      });
    },
  });

  return new Response(stream, {
    headers: {
      'Content-Type': 'text/event-stream',
      'Cache-Control': 'no-cache',
      'Connection': 'keep-alive',
    },
  });
}
```

Now let's create a client component that listens to these updates:

```typescript
// app/components/StockTicker.tsx
'use client';

import { useEffect, useState } from 'react';

interface StockPrice {
  symbol: string;
  price: string;
  timestamp: number;
}

export default function StockTicker() {
  const [stocks, setStocks] = useState<StockPrice[]>([]);
  const [isConnected, setIsConnected] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    // Create EventSource for SSE connection
    const eventSource = new EventSource('/api/stock-prices');

    eventSource.onopen = () => {
      setIsConnected(true);
      setError(null);
    };

    eventSource.onmessage = (event) => {
      const data = JSON.parse(event.data);
      setStocks(data);
    };

    eventSource.onerror = (err) => {
      console.error('SSE error:', err);
      setError('Connection lost. Reconnecting...');
      setIsConnected(false);
    };

    // Clean up on unmount
    return () => {
      eventSource.close();
    };
  }, []);

  return (
    <div className="p-4 border rounded-lg">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-xl font-bold">Live Stock Prices</h2>
        <span className={`px-2 py-1 rounded text-sm ${
          isConnected 
            ? 'bg-green-100 text-green-800' 
            : 'bg-red-100 text-red-800'
        }`}>
          {isConnected ? '● Live' : '○ Disconnected'}
        </span>
      </div>

      {error && (
        <p className="text-red-500 mb-2">{error}</p>
      )}

      <div className="space-y-2">
        {stocks.map((stock) => (
          <div 
            key={stock.symbol}
            className="flex justify-between items-center p-2 bg-gray-50 rounded"
          >
            <span className="font-medium">{stock.symbol}</span>
            <span className="text-lg">${stock.price}</span>
          </div>
        ))}
      </div>
    </div>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `export const dynamic = 'force-dynamic'` | Makes this route always run at request time | Required because we're streaming, not returning a static response |
| `new ReadableStream()` | Creates a stream for sending data incrementally | Allows real-time updates without creating new connections |
| `controller.enqueue()` | Adds data to the stream | The way we send SSE events to the client |
| `'Content-Type': 'text/event-stream'` | Tells browser this is an SSE stream | Required header for SSE to work |
| `'Cache-Control': 'no-cache'` | Prevents caching of the stream | Ensures the connection isn't cached |
| `'Connection': 'keep-alive'` | Keeps the connection open | Essential for long-lived SSE connections |
| `new EventSource('/api/stock-prices')` | Creates client-side SSE connection | Built-in browser API for receiving SSE |
| `eventSource.onmessage` | Handles incoming messages | Where we process the streamed data |

## Common Mistakes

### Mistake #1: Forgetting to Handle Client Disconnection
```typescript
// Wrong: Memory leak - interval keeps running after disconnect
const stream = new ReadableStream({
  start(controller) {
    const interval = setInterval(() => {
      controller.enqueue(encoder.encode('data: hi\n\n'));
    }, 1000);
    // Missing cleanup!
  },
});
```

```typescript
// Correct: Always clean up when client disconnects
const stream = new ReadableStream({
  start(controller) {
    const interval = setInterval(() => {
      controller.enqueue(encoder.encode('data: hi\n\n'));
    }, 1000);
    
    request.signal.addEventListener('abort', () => {
      clearInterval(interval);
      controller.close();
    });
  },
});
```

### Mistake #2: Using Wrong Content-Type
```typescript
// Wrong: Will not work as SSE
return new Response(stream, {
  headers: {
    'Content-Type': 'application/json', // Wrong!
  },
});
```

```typescript
// Correct: Must use text/event-stream for SSE
return new Response(stream, {
  headers: {
    'Content-Type': 'text/event-stream',
  },
});
```

### Mistake #3: Not Making Route Dynamic
```typescript
// Wrong: Static routes can't stream responses
export async function GET() {
  // This will fail - static routes don't support streaming
}
```

```typescript
// Correct: Force dynamic to allow streaming
export const dynamic = 'force-dynamic';

export async function GET() {
  // Now streaming will work
}
```

## Summary
- SSE lets the server push updates to the client in real-time
- Use `EventSource` in browsers to receive SSE
- Always handle client disconnection to prevent memory leaks
- Set proper headers: `text/event-stream`, `no-cache`, `keep-alive`
- Use `dynamic = 'force-dynamic'` on the route
- SSE works great for one-way server-to-client communication

## Next Steps
- [streaming-text-responses.md](./streaming-text-responses.md) — Advanced streaming techniques
