# Broadcast & Heartbeat

## What You'll Learn

- How to broadcast messages to all connected clients
- How to build a heartbeat/ping-pong mechanism to detect dead connections
- How to manage client metadata (usernames, rooms)
- How to gracefully shut down a WebSocket server
- How to implement connection throttling

## Broadcasting to All Clients

A **broadcast** sends a message to every connected client. The `ws` library provides `wss.clients` — a `Set` of all active WebSocket connections.

```js
// broadcast-server.js — Chat server with broadcast and heartbeat

import { createServer } from 'node:http';
import { WebSocketServer } from 'ws';

const httpServer = createServer();
const wss = new WebSocketServer({ server: httpServer });

// Store metadata for each client
// Map keys are WebSocket instances, values are metadata objects
const clients = new Map();

// Heartbeat interval in milliseconds
const HEARTBEAT_INTERVAL = 30_000;  // Send ping every 30 seconds
const PONG_TIMEOUT = 10_000;        // Wait 10 seconds for pong response

wss.on('connection', (ws, req) => {
  // Assign each new connection a unique ID and default metadata
  const clientId = generateId();
  clients.set(ws, {
    id: clientId,
    username: `user-${clientId.slice(0, 4)}`,
    isAlive: true,      // Set to true when we receive a pong
    connectedAt: Date.now(),
  });

  console.log(`Client ${clientId} connected (${clients.size} total)`);

  // Respond to pong frames — ws handles ping/pong automatically
  // We just track that the client responded
  ws.on('pong', () => {
    const client = clients.get(ws);
    if (client) client.isAlive = true;
  });

  ws.on('message', (data) => {
    const client = clients.get(ws);
    if (!client) return;  // Client was removed during message processing

    let parsed;
    try {
      // Parse incoming JSON messages
      parsed = JSON.parse(data.toString());
    } catch {
      // Invalid JSON — send error to this client only
      ws.send(JSON.stringify({ type: 'error', message: 'Invalid JSON' }));
      return;
    }

    // Handle different message types
    switch (parsed.type) {
      case 'chat':
        // Broadcast chat message to ALL clients (including sender)
        broadcast({
          type: 'chat',
          username: client.username,
          message: parsed.message,
          timestamp: new Date().toISOString(),
        });
        break;

      case 'set-username':
        // Update this client's username
        const oldName = client.username;
        client.username = parsed.username;
        // Notify everyone about the name change
        broadcast({
          type: 'system',
          message: `${oldName} is now ${parsed.username}`,
        });
        break;

      default:
        ws.send(JSON.stringify({ type: 'error', message: 'Unknown message type' }));
    }
  });

  ws.on('close', () => {
    const client = clients.get(ws);
    clients.delete(ws);

    if (client) {
      console.log(`Client ${client.id} disconnected (${clients.size} remaining)`);
      // Notify remaining clients
      broadcast({
        type: 'system',
        message: `${client.username} disconnected`,
      });
    }
  });

  ws.on('error', (err) => {
    console.error(`Client error: ${err.message}`);
    clients.delete(ws);
  });

  // Send welcome message with assigned username
  ws.send(JSON.stringify({
    type: 'welcome',
    id: clientId,
    username: clients.get(ws).username,
  }));
});

// Broadcast a message to ALL connected clients
function broadcast(message) {
  const data = JSON.stringify(message);

  wss.clients.forEach((client) => {
    // Only send to clients that are currently open (readyState 1)
    if (client.readyState === 1) {
      client.send(data);
    }
  });
}

// Broadcast to all clients EXCEPT one
function broadcastExcept(excludeWs, message) {
  const data = JSON.stringify(message);

  wss.clients.forEach((client) => {
    if (client !== excludeWs && client.readyState === 1) {
      client.send(data);
    }
  });
}

// Generate a short random ID
function generateId() {
  return Math.random().toString(36).slice(2, 10);
}

// === Heartbeat (Ping/Pong) ===

// The heartbeat detects dead connections — clients that disconnected
// without sending a close frame (e.g., network failure, crashed tab)
const heartbeatInterval = setInterval(() => {
  wss.clients.forEach((ws) => {
    const client = clients.get(ws);

    if (!client) return;

    if (client.isAlive === false) {
      // Client did not respond to the last ping — it is dead
      console.log(`Terminating unresponsive client ${client.id}`);
      clients.delete(ws);
      return ws.terminate();  // Force-close the TCP connection
    }

    // Mark as not alive — if the client responds with pong, it is set back to true
    client.isAlive = false;
    ws.ping();  // Send a ping frame (the ws library auto-responds with pong)
  });
}, HEARTBEAT_INTERVAL);

// Clean up the interval when the server closes
wss.on('close', () => {
  clearInterval(heartbeatInterval);
});

// Start the server
const PORT = 3000;
httpServer.listen(PORT, () => {
  console.log(`Server running on http://localhost:${PORT}`);
  console.log(`WebSocket on ws://localhost:${PORT}`);
});

// Graceful shutdown
process.on('SIGINT', () => {
  console.log('\nShutting down...');

  // Notify all clients
  broadcast({ type: 'system', message: 'Server shutting down' });

  // Close all connections
  wss.clients.forEach((client) => {
    client.close(1001, 'Server shutting down');  // 1001 = Going Away
  });

  wss.close(() => {
    httpServer.close(() => {
      process.exit(0);
    });
  });
});
```

## How It Works

### Heartbeat Mechanism

```
Server                              Client
  │                                    │
  │──── ping frame ──────────────────→│  (every 30 seconds)
  │←── pong frame ────────────────────│  (automatic response)
  │                                    │
  │  [isAlive = true → client OK]     │
  │                                    │
  │──── ping frame ──────────────────→│  (next cycle)
  │     [no pong received...]         │  (client crashed or network died)
  │                                    │
  │  [isAlive = false → terminate]    │
  │──── TCP RST ──────────────────────│
```

1. Server sends a **ping** frame every 30 seconds
2. The `ws` library automatically responds with a **pong** for each client
3. Server marks `isAlive = true` when it receives the pong
4. On the next cycle, server sets `isAlive = false` before pinging
5. If no pong arrives, the client is unresponsive and gets terminated

### The Clients Map

```js
const clients = new Map();
// ws instance → { id, username, isAlive, connectedAt }
```

Using a `Map` with WebSocket instances as keys lets you attach metadata to each connection. When a connection closes, delete its entry.

### Broadcast Patterns

| Pattern | Function | Use Case |
|---------|----------|----------|
| Broadcast all | `broadcast(msg)` | System announcements |
| Broadcast except one | `broadcastExcept(ws, msg)` | Do not echo sender's message |
| Send to one | `ws.send(msg)` | Private messages, errors |

## Common Mistakes

### Mistake 1: Not Checking readyState

```js
// WRONG — sending to a closing or closed connection throws
wss.clients.forEach((client) => {
  client.send(data);  // Throws if client is closing
});

// CORRECT — check readyState first
wss.clients.forEach((client) => {
  if (client.readyState === 1) {
    client.send(data);
  }
});
```

### Mistake 2: No Heartbeat Detection

```js
// WRONG — dead connections stay in wss.clients forever
// If a user's WiFi drops, the server never learns they left

// CORRECT — implement ping/pong heartbeat (see code above)
// Dead connections are terminated within one heartbeat cycle
```

### Mistake 3: Broadcast Loop Blocking

```js
// WRONG — JSON.stringify inside the loop for every client
wss.clients.forEach((client) => {
  client.send(JSON.stringify(message));  // Re-serializes N times
});

// CORRECT — serialize once, send the same string to all
const data = JSON.stringify(message);
wss.clients.forEach((client) => {
  if (client.readyState === 1) client.send(data);
});
```

## Try It Yourself

### Exercise 1: Private Messages

Add a `whisper` message type. When a client sends `{ type: "whisper", to: "user-ab12", message: "secret" }`, deliver the message only to the target client.

### Exercise 2: User List

Add a `users` message type. When a client sends `{ type: "users" }`, respond with a list of all connected usernames and IDs.

### Exercise 3: Connection Limit

Limit the server to 10 concurrent connections. When an 11th client connects, reject it with a "server full" message and close the connection.

## Next Steps

You can broadcast and detect dead connections. For a higher-level framework with rooms and namespaces, continue to [Socket.IO Rooms](./03-socketio-rooms.md).
