# Next.js Real-Time Limitations

## What You'll Learn
- Understand what Next.js doesn't support natively for real-time
- Learn the architectural constraints of the App Router
- Discover workarounds for common real-time scenarios

## Prerequisites
- Understanding of WebSockets and SSE concepts
- Knowledge of Next.js App Router basics

## Do I Need This Right Now?
This is crucial information before implementing any real-time feature. Many developers waste hours trying to force Next.js to do something it wasn't designed for. Understanding these limitations will save you significant debugging time and help you choose the right architecture from the start.

## Concept Explained Simply

Next.js is primarily a server-side rendering framework, not a real-time framework. Think of Next.js as a highly optimized delivery truck that makes regular deliveries (page loads), but it doesn't have a live radio (WebSocket server) built in. You can add a radio, but it requires extra equipment and doesn't come with the truck by default.

### What Next.js Doesn't Support Natively:

1. **No Built-in WebSocket Server**
   - Next.js API routes are serverless functions that don't maintain persistent connections
   - They respond to requests and then terminate
   - WebSockets require a long-running server process

2. **No Native Pub/Sub**
   - You can't easily broadcast events between different API routes or Server Components
   - No built-in mechanism for server-to-server communication

3. **Serverless Function Timeouts**
   - Vercel serverless functions have execution limits (10-60 seconds)
   - Long-running connections will be terminated

4. **No Persistent Server Memory**
   - Server Components are recreated on each request
   - No way to store connection state on the server between requests

### What Works Well:

1. **Server-Sent Events (SSE)**
   - Works with Next.js Route Handlers
   - Supported on Vercel with some configuration
   - One-way communication (server to client)

2. **Third-Party Services**
   - PartyKit (built on Cloudflare Workers)
   - Socket.io with a separate server
   - Pusher / Ably (managed services)
   - Supabase Realtime

3. **Client-Side Polling**
   - Works with any API route
   - Simple to implement
   - No server infrastructure changes needed

## Complete Code Example

Here's how to work around the limitations using SSE with a Route Handler:

```typescript
// app/api/events/route.ts
import { NextRequest } from 'next/server';

export const dynamic = 'force-dynamic';

export async function GET(request: NextRequest) {
  const encoder = new TextEncoder();
  
  const stream = new ReadableStream({
    async start(controller) {
      // Simulate sending real-time updates
      let counter = 0;
      
      const interval = setInterval(() => {
        counter++;
        const data = JSON.stringify({ 
          timestamp: Date.now(), 
          counter 
        });
        
        controller.enqueue(
          encoder.encode(`data: ${data}\n\n`)
        );
        
        // After 30 seconds, close the connection
        // This prevents serverless timeout issues
        if (counter >= 30) {
          clearInterval(interval);
          controller.close();
        }
      }, 1000);
      
      // Clean up when client disconnects
      request.signal.addEventListener('abort', () => {
        clearInterval(interval);
        controller.close();
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

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `export const dynamic = 'force-dynamic'` | Forces this route to be dynamic | Ensures the route isn't pre-rendered at build time |
| `new ReadableStream()` | Creates a stream for streaming responses | Allows sending data incrementally instead of all at once |
| `controller.enqueue()` | Adds data to the stream | This is how we send SSE events to the client |
| `request.signal.addEventListener('abort')` | Detects when client disconnects | Prevents memory leaks from abandoned connections |
| `'Content-Type': 'text/event-stream'` | Tells browser this is an SSE stream | Required format for Server-Sent Events |

## Common Mistakes

### Mistake #1: Trying to Use WebSockets in API Routes
```typescript
// Wrong: This won't work in Next.js API routes
// Serverless functions can't maintain persistent connections
export async function GET() {
  const wss = new WebSocketServer(this.httpServer);
  wss.on('connection', (ws) => {
    ws.send('Hello from Next.js!');
  });
}
```

### Mistake #2: Not Handling Connection Timeouts
```typescript
// Wrong: Without timeout handling, the connection
// will be killed by the serverless platform
export async function GET() {
  const stream = new ReadableStream({
    start(controller) {
      // Infinite loop - will eventually fail
      setInterval(() => {
        controller.enqueue(encoder.encode('data: ping\n\n'));
      }, 1000);
    },
  });
  // Missing: timeout handling, cleanup logic
}
```

### Mistake #3: Using Serverless for Long-Running Connections
```typescript
// Wrong: Vercel serverless functions timeout after 10-60 seconds
// Long-running SSE connections will be terminated
export async function GET() {
  // This will be killed by the platform
  const stream = new ReadableStream({
    start(controller) {
      setInterval(() => {
        controller.enqueue(encoder.encode('data: hello\n\n'));
      }, 1000);
    },
  });
}
```

## Summary
- Next.js doesn't have built-in WebSocket support in API routes
- Serverless functions have timeout limits that affect long connections
- SSE works but requires careful handling of timeouts
- Use third-party services (PartyKit, Socket.io, Pusher) for true WebSockets
- Consider client-side polling for simple use cases
- Always plan for connection cleanup to prevent memory leaks

## Next Steps
- [sse-with-route-handlers.md](../../18-edge-runtime/02-edge-functions/edge-route-handlers.md) — Using Edge Runtime for better SSE support
