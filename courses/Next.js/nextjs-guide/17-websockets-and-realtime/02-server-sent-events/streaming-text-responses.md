# Streaming Text Responses

## What You'll Learn
- Stream text responses character by character
- Create typewriter-style text effects
- Stream AI/LLM responses in real-time

## Prerequisites
- Understanding of SSE with Route Handlers
- Knowledge of ReadableStream in JavaScript

## Do I Need This Right Now?
This is particularly useful if you're building AI-powered features, chatbots, or any application where text appears gradually. However, if you just need simple updates (like notifications), the previous SSE example is sufficient.

## Concept Explained Simply

Streaming text responses work like a live captioning service. Instead of waiting for the entire sentence to be written before showing it (which feels slow), the server sends each word or character as soon as it's ready. This makes the app feel faster and more responsive — like watching someone type in real-time.

This is exactly how ChatGPT and other AI chatbots work. They don't wait to generate the entire response before showing it to you.

## Complete Code Example

Here's a route handler that streams text with a typewriter effect:

```typescript
// app/api/stream-text/route.ts
import { NextRequest } from 'next/server';

export const dynamic = 'force-dynamic';

// The text to stream - could come from an AI API
const fullResponse = `Welcome to our platform! We're excited to have you here.

Our mission is to build the best tools for developers. Whether you're 
building a simple blog or a complex web application, we have you covered.

Here are some features you might enjoy:

1. Lightning-fast performance
2. Beautiful, responsive design
3. Built-in security
4. Easy deployment

Get started today and build something amazing!`;

export async function GET(request: NextRequest) {
  const encoder = new TextEncoder();
  
  const stream = new ReadableStream({
    async start(controller) {
      // Split into words for realistic streaming
      const words = fullResponse.split(' ');
      
      for (let i = 0; i < words.length; i++) {
        // Add small delay between words for typewriter effect
        await new Promise(resolve => setTimeout(resolve, 50));
        
        // Send each word (with space, except for last word)
        const word = i === words.length - 1 
          ? words[i] 
          : words[i] + ' ';
        
        controller.enqueue(encoder.encode(word));
        
        // Check if client disconnected
        if (request.signal.aborted) {
          controller.close();
          break;
        }
      }
      
      // Close the stream when done
      controller.close();
    },
  });

  return new Response(stream, {
    headers: {
      'Content-Type': 'text/plain',
      'Transfer-Encoding': 'chunked',
    },
  });
}
```

Now let's create a client component that displays this streaming text:

```typescript
// app/components/TypewriterText.tsx
'use client';

import { useEffect, useState, useRef } from 'react';

export default function TypewriterText() {
  const [text, setText] = useState('');
  const [isComplete, setIsComplete] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const abortControllerRef = useRef<AbortController | null>(null);

  useEffect(() => {
    // Create abort controller for cleanup
    abortControllerRef.current = new AbortController();
    
    async function fetchStream() {
      try {
        const response = await fetch('/api/stream-text', {
          signal: abortControllerRef.current!.signal,
        });

        if (!response.body) {
          throw new Error('No response body');
        }

        const reader = response.body.getReader();
        const decoder = new TextDecoder();

        while (true) {
          const { done, value } = await reader.read();
          
          if (done) {
            setIsComplete(true);
            break;
          }
          
          const chunk = decoder.decode(value, { stream: true });
          setText(prev => prev + chunk);
        }
      } catch (err: any) {
        if (err.name !== 'AbortError') {
          setError('Failed to load text');
          console.error('Stream error:', err);
        }
      }
    }

    fetchStream();

    // Cleanup: abort fetch when component unmounts
    return () => {
      abortControllerRef.current?.abort();
    };
  }, []);

  return (
    <div className="max-w-2xl mx-auto p-6">
      <div className="bg-white border rounded-lg p-6 min-h-[300px]">
        <h2 className="text-xl font-bold mb-4">Streaming Text Demo</h2>
        
        {error && (
          <p className="text-red-500 mb-4">{error}</p>
        )}
        
        <div className="whitespace-pre-wrap leading-relaxed">
          {text}
          {!isComplete && <span className="animate-pulse">▊</span>}
        </div>
        
        {isComplete && (
          <div className="mt-4 text-green-600">
            ✓ Streaming complete!
          </div>
        )}
      </div>
    </div>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `export const dynamic = 'force-dynamic'` | Makes route dynamic | Required for streaming responses |
| `fullResponse.split(' ')` | Splits text into words | Enables word-by-word streaming |
| `await new Promise(resolve => setTimeout(resolve, 50))` | Adds delay between words | Creates realistic typewriter effect |
| `controller.enqueue(encoder.encode(word))` | Sends each word to client | Streams content incrementally |
| `request.signal.aborted` | Checks if client disconnected | Prevents unnecessary work |
| `'Transfer-Encoding': 'chunked'` | Enables chunked transfer | Allows sending data in chunks |
| `reader.read()` | Reads from the stream | Gets next chunk of data |
| `decoder.decode(value, { stream: true })` | Decodes chunk with streaming | Handles partial characters correctly |

## Common Mistakes

### Mistake #1: Not Using AbortController
```typescript
// Wrong: Can't cancel the fetch request
useEffect(() => {
  fetch('/api/stream-text').then(/* ... */);
}, []);
```

```typescript
// Correct: Can cancel when component unmounts
useEffect(() => {
  const controller = new AbortController();
  fetch('/api/stream-text', { signal: controller.signal });
  
  return () => controller.abort();
}, []);
```

### Mistake #2: Forgetting Stream Flag in Decoder
```typescript
// Wrong: May break on multi-byte characters
const chunk = decoder.decode(value);
```

```typescript
// Correct: Handles partial characters correctly
const chunk = decoder.decode(value, { stream: true });
```

### Mistake #3: Blocking the Event Loop
```typescript
// Wrong: Using synchronous loop blocks everything
for (let i = 0; i < 10000; i++) {
  controller.enqueue(encoder.encode('x'));
  // No await - blocks the server!
}
```

```typescript
// Correct: Async delay allows other work to happen
for (let i = 0; i < 10000; i++) {
  controller.enqueue(encoder.encode('x'));
  await new Promise(resolve => setTimeout(resolve, 10));
}
```

## Summary
- Streaming text responses create a typewriter effect
- Use `ReadableStream` on the server and `fetch` with `ReadableStream` reader on client
- Always handle client disconnection properly
- Use `Transfer-Encoding: chunked` for streaming
- The `stream: true` option in TextDecoder handles partial characters correctly
- Add delays between chunks for realistic effects

## Next Steps
- [sse-client-setup.md](./sse-client-setup.md) — Complete SSE client implementation patterns
