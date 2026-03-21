# WebSockets with Socket.io

## What You'll Learn
- Set up Socket.io with a separate Node.js server
- Connect Next.js client to Socket.io
- Handle rooms and broadcasting

## Prerequisites
- Understanding of WebSockets basics
- Knowledge of running separate Node.js servers

## Do I Need This Right Now?
Socket.io is a popular choice if you already have a Node.js backend or need specific Socket.io features (rooms, automatic reconnection). However, for new projects with Next.js, PartyKit is usually simpler. Choose this if you need more control or have an existing Node.js infrastructure.

## Concept Explained Simply

Socket.io is like having a sophisticated intercom system. It automatically handles things that raw WebSockets don't — like finding the best connection method (WebSocket, polling, or even long-polling as fallback), automatically reconnecting if the connection drops, and organizing users into "rooms" for group chats.

## Complete Code Example

First, let's set up a separate Socket.io server:

```typescript
// server/index.ts
import { Server } from 'socket.io';
import { createServer } from 'http';

interface User {
  id: string;
  name: string;
}

interface ChatMessage {
  id: string;
  text: string;
  senderId: string;
  senderName: string;
  roomId: string;
  timestamp: number;
}

const httpServer = createServer();
const io = new Server(httpServer, {
  cors: {
    origin: '*', // In production, restrict this
    methods: ['GET', 'POST'],
  },
});

// Track connected users
const users = new Map<string, User>();

io.on('connection', (socket) => {
  console.log(`User connected: ${socket.id}`);

  // User joins a room
  socket.on('join-room', ({ roomId, userName }: { roomId: string; userName: string }) => {
    socket.join(roomId);
    
    users.set(socket.id, {
      id: socket.id,
      name: userName,
    });

    // Notify others in the room
    socket.to(roomId).emit('user-joined', {
      id: socket.id,
      name: userName,
    });

    // Send room history (in real app, fetch from database)
    socket.emit('room-joined', {
      roomId,
      users: Array.from(users.values()).filter(u => u.id !== socket.id),
    });
  });

  // User sends a message
  socket.on('send-message', ({ roomId, text }: { roomId: string; text: string }) => {
    const user = users.get(socket.id);
    if (!user) return;

    const message: ChatMessage = {
      id: crypto.randomUUID(),
      text,
      senderId: socket.id,
      senderName: user.name,
      roomId,
      timestamp: Date.now(),
    };

    // Broadcast to everyone in the room (including sender)
    io.to(roomId).emit('new-message', message);
  });

  // User disconnects
  socket.on('disconnect', () => {
    const user = users.get(socket.id);
    if (user) {
      // Notify others that user left
      io.emit('user-left', { id: socket.id, name: user.name });
      users.delete(socket.id);
    }
    console.log(`User disconnected: ${socket.id}`);
  });

  // Typing indicator
  socket.on('typing', ({ roomId }: { roomId: string }) => {
    const user = users.get(socket.id);
    if (user) {
      socket.to(roomId).emit('user-typing', { id: socket.id, name: user.name });
    }
  });
});

const PORT = process.env.SOCKET_PORT || 3001;
httpServer.listen(PORT, () => {
  console.log(`Socket.io server running on port ${PORT}`);
});
```

Now the Next.js client component:

```typescript
// app/components/SocketChat.tsx
'use client';

import { useEffect, useState, useRef, useCallback } from 'react';
import { io, Socket } from 'socket.io-client';

interface User {
  id: string;
  name: string;
}

interface Message {
  id: string;
  text: string;
  senderId: string;
  senderName: string;
  timestamp: number;
}

export default function SocketChat() {
  const [socket, setSocket] = useState<Socket | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [roomId, setRoomId] = useState('general');
  const [userName, setUserName] = useState('');
  const [joined, setJoined] = useState(false);
  const [messages, setMessages] = useState<Message[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [input, setInput] = useState('');
  const [typingUsers, setTypingUsers] = useState<string[]>([]);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  // Initialize socket connection
  useEffect(() => {
    const socketInstance = io(process.env.NEXT_PUBLIC_SOCKET_URL || 'http://localhost:3001');
    setSocket(socketInstance);

    socketInstance.on('connect', () => {
      setIsConnected(true);
      console.log('Connected to socket server');
    });

    socketInstance.on('disconnect', () => {
      setIsConnected(false);
    });

    return () => {
      socketInstance.disconnect();
    };
  }, []);

  // Set up event listeners
  useEffect(() => {
    if (!socket) return;

    socket.on('room-joined', ({ users: roomUsers }: { users: User[] }) => {
      setUsers(roomUsers);
    });

    socket.on('user-joined', (user: User) => {
      setUsers(prev => [...prev, user]);
    });

    socket.on('user-left', ({ id }: { id: string }) => {
      setUsers(prev => prev.filter(u => u.id !== id));
    });

    socket.on('new-message', (message: Message) => {
      setMessages(prev => [...prev, message]);
    });

    socket.on('user-typing', ({ name }: { name: string }) => {
      setTypingUsers(prev => [...new Set([...prev, name])]);
    });

    // Clear typing indicator after delay
    socket.on('stop-typing', ({ id }: { id: string }) => {
      const user = users.find(u => u.id === id);
      if (user) {
        setTypingUsers(prev => prev.filter(n => n !== user.name));
      }
    });
  }, [socket, users]);

  // Auto-scroll to bottom
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  const joinRoom = useCallback(() => {
    if (!userName.trim() || !socket) return;
    socket.emit('join-room', { roomId, userName: userName.trim() });
    setJoined(true);
  }, [userName, roomId, socket]);

  const sendMessage = useCallback((e: React.FormEvent) => {
    e.preventDefault();
    if (!input.trim() || !socket) return;

    socket.emit('send-message', { roomId, text: input.trim() });
    setInput('');
  }, [input, roomId, socket]);

  const handleTyping = useCallback(() => {
    if (!socket || !joined) return;
    socket.emit('typing', { roomId });
  }, [socket, roomId, joined]);

  if (!joined) {
    return (
      <div className="max-w-md mx-auto p-6 bg-white rounded-lg border">
        <h2 className="text-xl font-bold mb-4">Join Chat</h2>
        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-1">Your Name</label>
            <input
              type="text"
              value={userName}
              onChange={(e) => setUserName(e.target.value)}
              placeholder="Enter your name"
              className="w-full px-3 py-2 border rounded-lg"
            />
          </div>
          <div>
            <label className="block text-sm font-medium mb-1">Room</label>
            <input
              type="text"
              value={roomId}
              onChange={(e) => setRoomId(e.target.value)}
              className="w-full px-3 py-2 border rounded-lg"
            />
          </div>
          <button
            onClick={joinRoom}
            disabled={!userName.trim()}
            className="w-full py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 disabled:opacity-50"
          >
            Join Room
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto p-4">
      <div className="grid grid-cols-4 gap-4">
        <div className="col-span-1 bg-white border rounded-lg p-4">
          <h3 className="font-bold mb-2">Room: {roomId}</h3>
          <p className="text-sm text-gray-500 mb-4">
            {isConnected ? '● Connected' : '○ Disconnected'}
          </p>
          <h4 className="font-medium text-sm mb-2">Users ({users.length})</h4>
          <ul className="space-y-1">
            {users.map((user) => (
              <li key={user.id} className="text-sm">• {user.name}</li>
            ))}
          </ul>
        </div>

        <div className="col-span-3 bg-white border rounded-lg overflow-hidden">
          <div className="h-96 overflow-y-auto p-4 space-y-3">
            {messages.map((msg) => (
              <div
                key={msg.id}
                className={`flex ${msg.senderId === socket?.id ? 'justify-end' : 'justify-start'}`}
              >
                <div className={`max-w-[70%] px-4 py-2 rounded-lg ${
                  msg.senderId === socket?.id
                    ? 'bg-blue-500 text-white'
                    : 'bg-gray-100'
                }`}>
                  <p className="text-xs opacity-70">{msg.senderName}</p>
                  <p>{msg.text}</p>
                </div>
              </div>
            ))}
            {typingUsers.length > 0 && (
              <p className="text-sm text-gray-500 italic">
                {typingUsers.join(', ')} {typingUsers.length === 1 ? 'is' : 'are'} typing...
              </p>
            )}
            <div ref={messagesEndRef} />
          </div>

          <form onSubmit={sendMessage} className="border-t p-4 flex gap-2">
            <input
              type="text"
              value={input}
              onChange={(e) => {
                setInput(e.target.value);
                handleTyping();
              }}
              placeholder="Type a message..."
              className="flex-1 px-3 py-2 border rounded-lg"
            />
            <button
              type="submit"
              className="px-4 py-2 bg-blue-500 text-white rounded-lg"
            >
              Send
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `new Server(httpServer)` | Creates Socket.io server | Handles WebSocket connections |
| `socket.join(roomId)` | Adds socket to a room | Groups users for targeted messaging |
| `socket.to(roomId).emit()` | Sends to specific room | Private messaging without sender |
| `io.to(roomId).emit()` | Sends to everyone in room | Broadcasting within a room |
| `socket.on('typing')` | Handles typing events | Shows "user is typing" indicator |
| `io()` | Creates client socket | Connects to Socket.io server |

## Common Mistakes

### Mistake #1: Not Separating the Server
```typescript
// Wrong: Can't run Socket.io inside Next.js API routes
// Serverless functions don't maintain persistent connections
export async function GET() {
  const io = new Server(httpServer);
  // This won't work!
}
```

```typescript
// Correct: Run Socket.io as a separate Node.js process
// server/index.ts runs independently from Next.js
```

### Mistake #2: CORS Issues
```typescript
// Wrong: Default CORS might block requests
const io = new Server(httpServer);
```

```typescript
// Correct: Configure CORS properly
const io = new Server(httpServer, {
  cors: {
    origin: 'https://your-nextjs-app.com',
    methods: ['GET', 'POST'],
  },
});
```

### Mistake #3: Not Handling Reconnection
```typescript
// Wrong: No strategy for connection drops
const socket = io(url);
```

```typescript
// Correct: Handle reconnection
const socket = io(url, {
  reconnection: true,
  reconnectionAttempts: 5,
  reconnectionDelay: 1000,
});
```

## Summary
- Socket.io requires a separate Node.js server
- Provides rooms for grouping users
- Has built-in reconnection and fallback mechanisms
- Client and server use similar event-based API
- Must configure CORS for cross-origin connections
- Works well with existing Node.js infrastructure

## Next Steps
- [realtime-database-sync.md](./realtime-database-sync.md) — Real-time database sync patterns
