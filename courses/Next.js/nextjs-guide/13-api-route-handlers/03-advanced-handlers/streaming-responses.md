# Streaming Responses

## What You'll Learn
- Using streaming in route handlers
- When to use streaming vs regular responses
- Implementing real-time data streaming
- Using TextEncoder for streaming text

## Prerequisites
- Understanding of route handlers
- Basic knowledge of streaming concepts
- Familiarity with async iterables

## Concept Explained Simply

Normally, when your API sends a response, it waits until it has everything ready and then sends it all at once — like a restaurant waiting for your entire order to be cooked before bringing anything out.

Streaming is more like a buffet line — the kitchen starts sending out dishes as soon as they're ready, while you're already eating the appetizers. This is perfect for:
- Very large responses (like exporting a huge dataset)
- AI chatbot responses that come in word-by-word
- Real-time updates

## Complete Code Example

### Basic Streaming Response

```typescript
// app/api/stream/route.ts
import { NextResponse } from "next/server";

export async function GET() {
  // Create a ReadableStream
  const stream = new ReadableStream({
    start(controller) {
      // Send data in chunks
      const encoder = new TextEncoder();
      
      const data = ["Hello", " ", "World", "!"];
      
      data.forEach((chunk, i) => {
        controller.enqueue(encoder.encode(chunk));
      });
      
      // Close the stream
      controller.close();
    },
  });
  
  return new NextResponse(stream, {
    headers: {
      "Content-Type": "text/plain",
      "Transfer-Encoding": "chunked",
    },
  });
}
```

### Streaming AI/Chat Responses

```typescript
// app/api/chat/route.ts
import { NextResponse } from "next/server";

// Mock AI response generator
async function* generateAIResponse(prompt: string) {
  const words = prompt.split(" ");
  
  for (const word of words) {
    // Simulate processing time
    await new Promise((resolve) => setTimeout(resolve, 100));
    yield word + " ";
  }
  
  yield "\n\n(This was streamed!)";
}

export async function POST(request: Request) {
  const { prompt } = await request.json();
  
  const stream = new ReadableStream({
    async start(controller) {
      const encoder = new TextEncoder();
      
      for await (const chunk of generateAIResponse(prompt)) {
        controller.enqueue(encoder.encode(chunk));
      }
      
      controller.close();
    },
  });
  
  return new NextResponse(stream, {
    headers: {
      "Content-Type": "text/plain; charset=utf-8",
      "X-Content-Type-Options": "nosniff",
    },
  });
}
```

### Streaming Large Data Export

```typescript
// app/api/export/users/route.ts
import { NextResponse } from "next/server";
import { db } from "@/lib/db";

export async function GET() {
  const stream = new ReadableStream({
    async start(controller) {
      const encoder = new TextEncoder();
      
      // Write CSV header
      controller.enqueue(encoder.encode("id,name,email,createdAt\n"));
      
      // Stream users in batches
      let cursor: string | undefined;
      
      while (true) {
        const batch = await db.user.findMany({
          take: 1000,
          cursor,
          skip: cursor ? 1 : 0,
        });
        
        for (const user of batch) {
          const row = `${user.id},${user.name},${user.email},${user.createdAt.toISOString()}\n`;
          controller.enqueue(encoder.encode(row));
        }
        
        if (batch.length < 1000) break;
        cursor = batch[batch.length - 1].id;
      }
      
      controller.close();
    },
  });
  
  return new NextResponse(stream, {
    headers: {
      "Content-Type": "text/csv",
      "Content-Disposition": "attachment; filename=users.csv",
    },
  });
}
```

### Using Web Streams with Async Iterator

```typescript
// app/api/notifications/route.ts
import { NextResponse } from "next/server";

// Simulate real-time notifications
async function* notificationsGenerator() {
  for (let i = 1; i <= 10; i++) {
    await new Promise((resolve) => setTimeout(resolve, 1000));
    yield JSON.stringify({ 
      id: i, 
      message: `Notification ${i}`,
      timestamp: new Date().toISOString() 
    }) + "\n";
  }
}

export async function GET() {
  const encoder = new TextEncoder();
  
  const stream = new ReadableStream({
    async start(controller) {
      for await (const notification of notificationsGenerator()) {
        controller.enqueue(encoder.encode(notification));
      }
      controller.close();
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

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `new ReadableStream({...})` | Creates a stream | Core Web Streams API |
| `start(controller)` | Initializes stream | Called when reading begins |
| `controller.enqueue(data)` | Adds data to stream | Sends chunk to client |
| `controller.close()` | Ends the stream | Signals no more data |
| `new TextEncoder()` | Converts text to bytes | Needed for string → stream data |
| `async generator*` | Async iterator | Yields values over time |

## Common Mistakes

### Mistake 1: Forgetting to Close Stream

```typescript
// WRONG - Stream never closes, client hangs forever!
const stream = new ReadableStream({
  start(controller) {
    controller.enqueue(encoder.encode("Hello"));
    // Missing: controller.close();
  },
});

// CORRECT - Always close when done
const stream = new ReadableStream({
  start(controller) {
    controller.enqueue(encoder.encode("Hello"));
    controller.close();
  },
});
```

### Mistake 2: Not Handling Errors

```typescript
// WRONG - No error handling in stream
const stream = new ReadableStream({
  start(controller) {
    try {
      // Risky operation
    } catch (e) {
      // What happens to the stream?
    }
  },
});

// CORRECT - Use cancel for error handling
const stream = new ReadableStream({
  start(controller) {
    // Main logic
  },
  cancel(error) {
    // Called if client disconnects or error occurs
    console.log("Stream cancelled:", error);
  },
});
```

### Mistake 3: Wrong Content-Type

```typescript
// WRONG - Sending stream but claiming it's not streaming
return new NextResponse(stream, {
  headers: { "Content-Type": "application/json" },
  // Missing: "Transfer-Encoding": "chunked"
});

// CORRECT - Use appropriate headers
return new NextResponse(stream, {
  headers: { 
    "Content-Type": "text/plain",
    "Transfer-Encoding": "chunked",
  },
});
```

## Summary

- Use `ReadableStream` for streaming responses
- Always close the stream when done (`controller.close()`)
- Use `TextEncoder` to convert strings to bytes
- Stream is perfect for AI chat, large exports, and real-time updates
- The client receives chunks as they become available
- Add error handling with the `cancel` option

## Next Steps

- [sse-with-route-handlers.md](../17-websockets-and-realtime/02-server-sent-events/sse-with-route-handlers.md) - Server-Sent Events with route handlers
- [webhooks.md](./webhooks.md) - Handling incoming webhook requests
