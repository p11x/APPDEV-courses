# Next.js AI Setup

## What You'll Learn

- How to set up the Vercel AI SDK with Next.js
- How to create streaming chat endpoints
- How to use the useChat hook
- How to handle AI responses in React

## Setup

```bash
npm install ai @ai-sdk/openai
```

## API Route

```ts
// app/api/chat/route.ts

import { openai } from '@ai-sdk/openai';
import { streamText } from 'ai';

export const maxDuration = 30;  // Max function duration (seconds)

export async function POST(req: Request) {
  const { messages } = await req.json();

  const result = streamText({
    model: openai('gpt-4o'),
    messages,
    system: 'You are a helpful Node.js assistant.',
  });

  return result.toDataStreamResponse();
}
```

## React Component

```tsx
// components/Chat.tsx

'use client';

import { useChat } from 'ai/react';

export function Chat() {
  const { messages, input, handleInputChange, handleSubmit, isLoading } = useChat();

  return (
    <div className="chat">
      <div className="messages">
        {messages.map((m) => (
          <div key={m.id} className={`message ${m.role}`}>
            <strong>{m.role}:</strong> {m.content}
          </div>
        ))}
      </div>

      <form onSubmit={handleSubmit}>
        <input
          value={input}
          onChange={handleInputChange}
          placeholder="Ask a question..."
          disabled={isLoading}
        />
        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Thinking...' : 'Send'}
        </button>
      </form>
    </div>
  );
}
```

## Next Steps

For chat UI, continue to [Next.js Chat](./02-nextjs-chat.md).
