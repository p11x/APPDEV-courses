# Next.js Chat UI

## What You'll Learn

- How to build a complete chat UI
- How to handle streaming responses
- How to implement chat history
- How to add markdown rendering

## Full Chat Component

```tsx
// components/ChatUI.tsx

'use client';

import { useChat } from 'ai/react';
import { useRef, useEffect } from 'react';

export function ChatUI() {
  const { messages, input, handleInputChange, handleSubmit, isLoading, error } = useChat({
    api: '/api/chat',
    onError: (err) => console.error('Chat error:', err),
  });

  const messagesEndRef = useRef<HTMLDivElement>(null);

  // Auto-scroll to bottom
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  return (
    <div className="flex flex-col h-screen max-w-2xl mx-auto p-4">
      <div className="flex-1 overflow-y-auto space-y-4 mb-4">
        {messages.map((m) => (
          <div
            key={m.id}
            className={`p-4 rounded-lg ${
              m.role === 'user' ? 'bg-blue-100 ml-auto' : 'bg-gray-100'
            } max-w-[80%]`}
          >
            <div className="font-bold mb-1">
              {m.role === 'user' ? 'You' : 'Assistant'}
            </div>
            <div className="whitespace-pre-wrap">{m.content}</div>
          </div>
        ))}

        {isLoading && (
          <div className="bg-gray-100 p-4 rounded-lg animate-pulse">
            Thinking...
          </div>
        )}

        {error && (
          <div className="bg-red-100 p-4 rounded-lg text-red-700">
            Error: {error.message}
          </div>
        )}

        <div ref={messagesEndRef} />
      </div>

      <form onSubmit={handleSubmit} className="flex gap-2">
        <input
          value={input}
          onChange={handleInputChange}
          placeholder="Type a message..."
          className="flex-1 p-3 border rounded-lg"
          disabled={isLoading}
        />
        <button
          type="submit"
          disabled={isLoading || !input.trim()}
          className="px-6 py-3 bg-blue-500 text-white rounded-lg disabled:opacity-50"
        >
          Send
        </button>
      </form>
    </div>
  );
}
```

## Next Steps

For markdown rendering, continue to [Next.js Markdown](./03-nextjs-markdown.md).
