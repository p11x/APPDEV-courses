# Socket.IO Rooms & Namespaces

## What You'll Learn

- What Socket.IO provides on top of raw WebSockets
- How to set up a Socket.IO server and client
- How rooms work and how to join/leave them
- What namespaces are and when to use them
- How to use the Redis adapter for multi-server scaling

## What Is Socket.IO?

Socket.IO is a library that adds features on top of WebSockets:

- **Automatic reconnection** — client reconnects if the connection drops
- **Rooms** — group sockets and broadcast to a subset
- **Namespaces** — separate communication channels on one connection
- **Fallback transport** — falls back to HTTP long-polling if WebSockets are blocked
- **Binary support** — send Buffers and ArrayBuffers seamlessly

## Project Setup

```bash
mkdir socketio-demo && cd socketio-demo
npm init -y
npm install socket.io
```

## Socket.IO Server

```js
// server.js — Socket.IO server with rooms and namespaces

import { createServer } from 'node:http';
import { Server } from 'socket.io';

const httpServer = createServer();

// Create Socket.IO server attached to the HTTP server
const io = new Server(httpServer, {
  cors: {
    origin: '*',  // Allow all origins (restrict in production)
    methods: ['GET', 'POST'],
  },
  connectionStateRecovery: {
    maxDisconnectionDuration: 2 * 60 * 1000,  // Recover connections within 2 minutes
  },
});

// === Default Namespace (/) ===

// 'connection' fires when a client connects to the default namespace
io.on('connection', (socket) => {
  console.log(`Client connected: ${socket.id}`);

  // Each socket has a unique id — use it to identify clients
  // Store user data on the socket object
  socket.data.username = `user-${socket.id.slice(0, 4)}`;

  // Send a welcome event to just this client
  socket.emit('welcome', {
    id: socket.id,
    username: socket.data.username,
    message: 'Connected to chat server',
  });

  // Notify all OTHER clients
  socket.broadcast.emit('system', `${socket.data.username} joined`);

  // === Rooms ===

  // 'join-room' — client requests to join a room
  socket.on('join-room', (roomName) => {
    socket.join(roomName);  // Add this socket to the room
    console.log(`${socket.id} joined room: ${roomName}`);

    // Notify only clients in this room
    io.to(roomName).emit('system', `${socket.data.username} joined ${roomName}`);
  });

  // 'leave-room' — client requests to leave a room
  socket.on('leave-room', (roomName) => {
    socket.leave(roomName);  // Remove this socket from the room
    io.to(roomName).emit('system', `${socket.data.username} left ${roomName}`);
  });

  // 'room-message' — send a message to a specific room
  socket.on('room-message', ({ room, message }) => {
    io.to(room).emit('chat', {
      username: socket.data.username,
      message,
      room,
      timestamp: new Date().toISOString(),
    });
  });

  // 'dm' — direct message to a specific socket
  socket.on('dm', ({ to, message }) => {
    // io.to(socketId) sends to a specific connected socket
    io.to(to).emit('dm', {
      from: socket.data.username,
      fromId: socket.id,
      message,
      timestamp: new Date().toISOString(),
    });
  });

  // List rooms this socket is in
  socket.on('my-rooms', (callback) => {
    // socket.rooms is a Set of room names this socket belongs to
    // Every socket is automatically in a room named after its own id
    callback([...socket.rooms]);
  });

  // Disconnect
  socket.on('disconnect', (reason) => {
    console.log(`Client disconnected: ${socket.id} (${reason})`);
    socket.broadcast.emit('system', `${socket.data.username} disconnected`);
  });

  // Error handling
  socket.on('error', (err) => {
    console.error(`Socket error for ${socket.id}:`, err.message);
  });
});

// === Namespace: /admin ===

// Namespaces separate communication channels — like virtual servers on one connection
const adminNamespace = io.of('/admin');

adminNamespace.on('connection', (socket) => {
  console.log(`Admin connected: ${socket.id}`);

  // This namespace could require authentication
  socket.on('kick-user', (userId) => {
    // Disconnect a user from the default namespace
    io.sockets.sockets.get(userId)?.disconnect(true);
    socket.emit('user-kicked', { userId });
  });

  socket.on('broadcast-all', (message) => {
    // Send to ALL connected clients across all namespaces
    io.emit('system', `[Admin] ${message}`);
  });
});

// Start the server
const PORT = 3000;
httpServer.listen(PORT, () => {
  console.log(`Socket.IO server on http://localhost:${PORT}`);
});
```

## Socket.IO Client (Node.js)

```js
// client.js — Socket.IO client

import { io } from 'socket.io-client';

// Connect to the default namespace
const socket = io('http://localhost:3000');

socket.on('connect', () => {
  console.log('Connected with id:', socket.id);
});

socket.on('welcome', (data) => {
  console.log('Welcome:', data);

  // Join a room after connecting
  socket.emit('join-room', 'general');

  // Send a message to the room
  setTimeout(() => {
    socket.emit('room-message', { room: 'general', message: 'Hello, room!' });
  }, 1000);
});

socket.on('chat', (data) => {
  console.log(`[${data.room}] ${data.username}: ${data.message}`);
});

socket.on('system', (message) => {
  console.log(`[system] ${message}`);
});

socket.on('dm', (data) => {
  console.log(`[DM from ${data.from}] ${data.message}`);
});

socket.on('disconnect', (reason) => {
  console.log('Disconnected:', reason);
});

// Connect to the admin namespace
const adminSocket = io('http://localhost:3000/admin');

adminSocket.on('connect', () => {
  console.log('Admin connected:', adminSocket.id);
});
```

## Socket.IO Client (Browser)

```html
<!-- browser-client.html -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>Socket.IO Client</title>
  <script src="https://cdn.socket.io/4.7.4/socket.io.min.js"></script>
</head>
<body>
  <h1>Socket.IO Chat</h1>
  <div id="log"></div>
  <input id="msg" placeholder="Message...">
  <button id="send">Send</button>

  <script>
    const socket = io('http://localhost:3000');
    const log = document.getElementById('log');

    function append(text) {
      log.innerHTML += `<p>${text}</p>`;
    }

    socket.on('connect', () => append('Connected: ' + socket.id));
    socket.on('welcome', (data) => {
      append('Welcome: ' + data.username);
      socket.emit('join-room', 'general');
    });
    socket.on('chat', (d) => append(`[${d.room}] ${d.username}: ${d.message}`));
    socket.on('system', (msg) => append('[system] ' + msg));

    document.getElementById('send').addEventListener('click', () => {
      const msg = document.getElementById('msg').value;
      socket.emit('room-message', { room: 'general', message: msg });
      document.getElementById('msg').value = '';
    });
  </script>
</body>
</html>
```

## How It Works

### Rooms

Rooms are server-side groupings. A socket can join multiple rooms, and you can broadcast to all sockets in a room:

```js
socket.join('room-a');       // Add socket to room
socket.leave('room-a');      // Remove socket
io.to('room-a').emit(...);  // Send to everyone in room-a
socket.to('room-b').emit(...); // Send to everyone in room-b except sender
```

### Namespaces

Namespaces are separate communication channels. Each namespace has its own event handlers and rooms:

```
Default namespace (/)    Admin namespace (/admin)
├── room: general        ├── event handlers for admin
├── room: support        └── admin-only features
└── room: gaming
```

Clients connect to specific namespaces. Events on one namespace do not reach clients on another.

### The Adapter

Socket.IO uses an **adapter** to track which sockets are in which rooms. The default in-memory adapter works for single-server setups. For multiple servers, use the **Redis adapter**:

```bash
npm install @socket.io/redis-adapter redis
```

```js
import { createClient } from 'redis';
import { createAdapter } from '@socket.io/redis-adapter';

const pubClient = createClient({ url: 'redis://localhost:6379' });
const subClient = pubClient.duplicate();

await Promise.all([pubClient.connect(), subClient.connect()]);

io.adapter(createAdapter(pubClient, subClient));
// Now rooms and broadcasts work across multiple server instances
```

## Common Mistakes

### Mistake 1: Forgetting socket.rooms Includes the Socket ID

```js
// WRONG — socket.rooms always includes the socket's own ID
socket.on('my-rooms', (cb) => {
  cb([...socket.rooms]);  // Returns ['abc123', 'general'] — the first is the socket ID
});

// CORRECT — filter out the socket's own ID
socket.on('my-rooms', (cb) => {
  const rooms = [...socket.rooms].filter((r) => r !== socket.id);
  cb(rooms);  // Returns ['general']
});
```

### Mistake 2: Using broadcast with io.to()

```js
// io.to() sends to everyone in the room INCLUDING the sender
io.to('general').emit('chat', msg);  // Sender also receives this

// socket.to() sends to everyone in the room EXCLUDING the sender
socket.to('general').emit('chat', msg);  // Sender does NOT receive this
```

### Mistake 3: Not Handling Reconnection Events

```js
// WRONG — assume connection fires once
socket.on('connect', () => {
  socket.emit('join-room', 'general');  // Room is lost after reconnect
});

// CORRECT — rejoin rooms on reconnect
socket.on('connect', () => {
  socket.emit('join-room', 'general');
});

socket.on('disconnect', () => {
  console.log('Disconnected — will auto-reconnect');
  // Socket.IO reconnects automatically
});
```

## Try It Yourself

### Exercise 1: Multi-Room Chat

Build a chat where users can join multiple rooms and switch between them. Display which room each message came from.

### Exercise 2: User List per Room

Add an event that returns all usernames currently in a specific room. Use `io.in(room).fetchSockets()` to get socket metadata.

### Exercise 3: Admin Namespace

Create an `/admin` namespace. Implement a `kick` event that disconnects a user by their socket ID. Only clients connected to `/admin` should have this power.

## Next Steps

Socket.IO gives you rooms and namespaces. For one-way server-to-client streaming without WebSockets, continue to [SSE Basics](../server-sent-events/01-sse-basics.md).
