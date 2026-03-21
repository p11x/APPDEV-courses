# WebSockets with PartyKit

## What You'll Learn
- Set up PartyKit for real-time WebSocket communication
- Create a chat application using PartyKit
- Connect Next.js with PartyKit server

## Prerequisites
- Understanding of WebSockets vs SSE
- Basic knowledge of real-time applications

## Do I Need This Right Now?
PartyKit is the recommended approach for WebSockets in Next.js because it runs outside the serverless model. If you need bidirectional communication (chat, multiplayer games, collaborative editing), this is the best path. If you only need server-to-client updates, SSE is simpler.

## Concept Explained Simply

PartyKit is like hiring a professional event coordinator. Instead of trying to manage all the communication between guests yourself (which would be exhausting), the coordinator (PartyKit server) handles all the connections and message routing. You just tell it what messages to send and to whom.

PartyKit runs on Cloudflare Workers, which means:
- Extremely low latency worldwide
- No cold starts
- WebSocket connections stay open indefinitely
- Scales automatically

## Complete Code Example

First, let's set up the PartyKit server:

```typescript
// party/server.ts
import type * as Party from "partykit/server";

interface ChatMessage {
  id: string;
  text: string;
  sender: string;
  timestamp: number;
}

interface User {
  id: string;
  name: string;
  color: string;
}

export default class ChatServer implements Party.Server {
  constructor(readonly room: Party.Room) {}

  // When a client connects
  onConnect(conn: Party.Connection, ctx: Party.ConnectionContext) {
    console.log(`Connected: ${conn.id}`);
    
    // Send welcome message to new user
    conn.send(JSON.stringify({
      type: 'welcome',
      message: 'Welcome to the chat!',
      id: conn.id,
    }));
  }

  // When a client disconnects
  onClose(conn: Party.Connection) {
    console.log(`Disconnected: ${conn.id}`);
    
    // Notify others that user left
    this.room.broadcast(JSON.stringify({
      type: 'user-left',
      id: conn.id,
    }));
  }

  // When a client sends a message
  onMessage(message: string, sender: Party.Connection) {
    const data = JSON.parse(message);
    
    if (data.type === 'chat') {
      const chatMessage: ChatMessage = {
        id: crypto.randomUUID(),
        text: data.text,
        sender: sender.id,
        timestamp: Date.now(),
      };
      
      // Broadcast to ALL clients including sender
      this.room.broadcast(JSON.stringify({
        type: 'message',
        ...chatMessage,
      }));
    }
  }
}

ChatServer satisfies Party.Worker;
```

Now let's create the Next.js client component:

```typescript
// app/components/ChatRoom.tsx
'use client';

import { useEffect, useState, useRef, useCallback } from 'react';

interface Message {
  id: string;
  text: string;
  sender: string;
  timestamp: number;
}

export default function ChatRoom() {
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState('');
  const [isConnected, setIsConnected] = useState(false);
  const [myId, setMyId] = useState<string | null>(null);
  const wsRef = useRef<WebSocket | null>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  // Connect to PartyKit server
  useEffect(() => {
    // Replace with your PartyKit host
    const partyKitHost = process.env.NEXT_PUBLIC_PARTYKIT_HOST || 'localhost:1999';
    const ws = new WebSocket(`wss://${partyKitHost}/party/main-room`);
    wsRef.current = ws;

    ws.onopen = () => {
      setIsConnected(true);
      console.log('Connected to chat');
    };

    ws.onmessage = (event) => {
      const data = JSON.parse(event.data);
      
      switch (data.type) {
        case 'welcome':
          setMyId(data.id);
          break;
        case 'message':
          setMessages(prev => [...prev, {
            id: data.id,
            text: data.text,
            sender: data.sender,
            timestamp: data.timestamp,
          }]);
          break;
        case 'user-left':
          setMessages(prev => [...prev, {
            id: data.id,
            text: 'User left the chat',
            sender: 'system',
            timestamp: Date.now(),
          }]);
          break;
      }
    };

    ws.onclose = () => {
      setIsConnected(false);
    };

    ws.onerror = (error) => {
      console.error('WebSocket error:', error);
    };

    return () => {
      ws.close();
    };
  }, []);

  // Auto-scroll to bottom when new messages arrive
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  const sendMessage = useCallback((e: React.FormEvent) => {
    e.preventDefault();
    
    if (!input.trim() || !wsRef.current) return;

    wsRef.current.send(JSON.stringify({
      type: 'chat',
      text: input.trim(),
    }));

    setInput('');
  }, [input]);

  return (
    <div className="max-w-2xl mx-auto p-4">
      <div className="bg-white border rounded-lg overflow-hidden">
        <div className="bg-gray-50 px-4 py-3 border-b flex items-center justify-between">
          <h2 className="font-bold">Chat Room</h2>
          <span className={`px-2 py-1 rounded text-xs ${
            isConnected ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
          }`}>
            {isConnected ? '● Connected' : '○ Disconnected'}
          </span>
        </div>

        <div className="h-96 overflow-y-auto p-4 space-y-3">
          {messages.map((msg) => (
            <div
              key={msg.id}
              className={`flex ${msg.sender === myId ? 'justify-end' : 'justify-start'}`}
            >
              <div
                className={`max-w-[70%] px-4 py-2 rounded-lg ${
                  msg.sender === myId
                    ? 'bg-blue-500 text-white'
                    : msg.sender === 'system'
                    ? 'bg-gray-100 text-gray-600 text-center w-full'
                    : 'bg-gray-100 text-gray-900'
                }`}
              >
                {msg.sender !== myId && msg.sender !== 'system' && (
                  <p className="text-xs text-gray-500 mb-1">
                    User {msg.sender.slice(0, 6)}
                  </p>
                )}
                <p>{msg.text}</p>
                <p className={`text-xs mt-1 ${
                  msg.sender === myId ? 'text-blue-200' : 'text-gray-400'
                }`}>
                  {new Date(msg.timestamp).toLocaleTimeString()}
                </p>
              </div>
            </div>
          ))}
          <div ref={messagesEndRef} />
        </div>

        <form onSubmit={sendMessage} className="border-t p-4 flex gap-2">
          <input
            type="text"
            value={input}
            onChange={(e) => setInput(e.target.value)}
            placeholder="Type a message..."
            className="flex-1 px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
            disabled={!isConnected}
          />
          <button
            type="submit"
            disabled={!isConnected || !input.trim()}
            className="px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Send
          </button>
        </form>
      </div>
    </div>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `implements Party.Server` | Defines the PartyKit server class | Required for PartyKit server implementation |
| `onConnect(conn)` | Fires when client connects | Initialize connection, send welcome message |
| `onClose(conn)` | Fires when client disconnects | Clean up, notify others |
| `onMessage(message, sender)` | Fires when message received | Handle incoming messages, broadcast |
| `this.room.broadcast()` | Sends to all connected clients | Distributes messages to everyone |
| `new WebSocket()` | Creates client WebSocket connection | Connects to PartyKit server |
| `ws.onopen/onmessage/onclose` | WebSocket event handlers | Handle connection lifecycle |
| `ws.send(JSON.stringify(...))` | Sends message to server | Sends chat messages |

## Common Mistakes

### Mistake #1: Not Handling Connection State
```typescript
// Wrong: No feedback when disconnected
useEffect(() => {
  const ws = new WebSocket(url);
  ws.onmessage = (e) => setMessages(JSON.parse(e.data));
}, []);
```

```typescript
// Correct: Show connection state to user
const [isConnected, setIsConnected] = useState(false);

useEffect(() => {
  const ws = new WebSocket(url);
  ws.onopen = () => setIsConnected(true);
  ws.onclose = () => setIsConnected(false);
}, []);
```

### Mistake #2: Not Cleaning Up WebSocket
```typescript
// Wrong: Connection stays open after unmount
useEffect(() => {
  const ws = new WebSocket(url);
  // Missing cleanup!
}, []);
```

```typescript
// Correct: Close on unmount
useEffect(() => {
  const ws = new WebSocket(url);
  return () => ws.close();
}, []);
```

### Mistake #3: Sending Without Checking Connection
```typescript
// Wrong: May throw if disconnected
ws.send(JSON.stringify(data));
```

```typescript
// Correct: Check readyState before sending
if (ws.readyState === WebSocket.OPEN) {
  ws.send(JSON.stringify(data));
}
```

## Summary
- PartyKit provides WebSocket servers that run on Cloudflare Workers
- It's the recommended way to add WebSockets to Next.js apps
- Server handles connections, messages, and broadcasting
- Client connects via WebSocket and handles incoming messages
- Always handle connection state and clean up on unmount
- Use environment variables for the PartyKit host

## Next Steps
- [websockets-with-socketio.md](./websockets-with-socketio.md) — Alternative WebSocket implementation
