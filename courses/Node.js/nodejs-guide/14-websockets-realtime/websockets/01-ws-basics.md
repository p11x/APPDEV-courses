# WebSocket Basics

## What You'll Learn

- What WebSockets are and how they differ from HTTP
- How to install and use the `ws` package
- How to create a WebSocket server and handle connections
- How to send and receive messages between client and server
- How to handle connection lifecycle events (open, message, close, error)

## What Are WebSockets?

HTTP follows a **request-response** model: the client sends a request, the server responds, and the connection closes. This works for loading pages but is terrible for real-time features like chat, live scores, or notifications.

**WebSockets** create a persistent, full-duplex connection. Once established, both client and server can send messages at any time without re-establishing the connection.

```
HTTP:     Client → Request → Server → Response → [connection closed]
WebSocket: Client ↔ Messages ↔ Server ↔ Messages ↔ [connection stays open]
```

## Project Setup

```bash
mkdir ws-demo && cd ws-demo
npm init -y
npm install ws
```

Add `"type": "module"` to `package.json`.

## WebSocket Server

```js
// server.js — Basic WebSocket server using the ws package

import { WebSocketServer } from 'ws';

// Create a WebSocket server on port 8080
// The WebSocket server runs alongside (or separately from) an HTTP server
const wss = new WebSocketServer({ port: 8080 });

console.log('WebSocket server listening on ws://localhost:8080');

// 'connection' fires when a client connects
wss.on('connection', (ws, req) => {
  // ws is the WebSocket instance for this specific client
  // req is the original HTTP upgrade request (contains headers, IP, etc.)
  const clientIp = req.socket.remoteAddress;
  console.log(`Client connected from ${clientIp}`);

  // 'message' fires when this client sends data
  ws.on('message', (data, isBinary) => {
    // data is a Buffer by default — convert to string for text messages
    const message = isBinary ? data : data.toString();
    console.log(`Received: ${message}`);

    // Echo the message back to the client
    ws.send(`Echo: ${message}`);

    // Broadcast to all OTHER connected clients
    wss.clients.forEach((client) => {
      // client.readyState === 1 means the connection is open (WebSocket.OPEN)
      if (client !== ws && client.readyState === 1) {
        client.send(`Broadcast: ${message}`);
      }
    });
  });

  // 'close' fires when the client disconnects
  ws.on('close', (code, reason) => {
    // code: numeric close code (1000 = normal)
    // reason: Buffer with the reason string sent by the client
    console.log(`Client disconnected: code=${code}, reason=${reason.toString()}`);
  });

  // 'error' fires on connection errors
  ws.on('error', (err) => {
    console.error('WebSocket error:', err.message);
  });

  // Send a welcome message to the connecting client
  ws.send('Welcome to the WebSocket server!');
});

// Server-level error handling
wss.on('error', (err) => {
  console.error('Server error:', err.message);
});
```

## WebSocket Client (Browser)

```html
<!-- client.html — Browser WebSocket client -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>WebSocket Client</title>
</head>
<body>
  <h1>WebSocket Demo</h1>
  <input type="text" id="message" placeholder="Type a message...">
  <button id="send">Send</button>
  <div id="log"></div>

  <script>
    // WebSocket is a browser built-in — no install needed
    const ws = new WebSocket('ws://localhost:8080');
    const log = document.getElementById('log');

    function appendLog(text) {
      const p = document.createElement('p');
      p.textContent = text;
      log.appendChild(p);
    }

    // 'open' fires when the connection is established
    ws.addEventListener('open', () => {
      appendLog('Connected to server');
    });

    // 'message' fires when the server sends data
    ws.addEventListener('message', (event) => {
      appendLog(`Server: ${event.data}`);
    });

    // 'close' fires when the connection is closed
    ws.addEventListener('close', (event) => {
      appendLog(`Disconnected: code=${event.code}`);
    });

    // 'error' fires on connection errors
    ws.addEventListener('error', (err) => {
      appendLog(`Error: ${err.message}`);
    });

    // Send messages on button click
    document.getElementById('send').addEventListener('click', () => {
      const input = document.getElementById('message');
      if (input.value && ws.readyState === WebSocket.OPEN) {
        ws.send(input.value);
        appendLog(`You: ${input.value}`);
        input.value = '';
      }
    });
  </script>
</body>
</html>
```

## Node.js WebSocket Client

```js
// client.js — WebSocket client in Node.js

import { WebSocket } from 'ws';

// Connect to the server
const ws = new WebSocket('ws://localhost:8080');

ws.on('open', () => {
  console.log('Connected to server');

  // Send a message after connecting
  ws.send('Hello from Node.js client!');

  // Send periodic messages
  let count = 0;
  const interval = setInterval(() => {
    count++;
    ws.send(`Message ${count}`);

    if (count >= 5) {
      clearInterval(interval);
      ws.close(1000, 'Done');  // Close with normal code and reason
    }
  }, 1000);
});

ws.on('message', (data) => {
  console.log('Received:', data.toString());
});

ws.on('close', (code, reason) => {
  console.log(`Connection closed: code=${code}, reason=${reason.toString()}`);
});

ws.on('error', (err) => {
  console.error('Error:', err.message);
});
```

## How It Works

### The WebSocket Handshake

1. Client sends an HTTP request with `Upgrade: websocket` header
2. Server responds with `101 Switching Protocols`
3. The TCP connection is now a WebSocket connection
4. Both sides can send frames (text or binary) at any time

### Connection Lifecycle

```
Client                    Server
  │                          │
  │── HTTP GET / ──────────→│  (handshake request)
  │←── 101 Switching ───────│  (handshake response)
  │                          │
  │←── welcome message ─────│  (server sends first message)
  │                          │
  │── "Hello" ─────────────→│  (client message)
  │←── "Echo: Hello" ───────│  (server response)
  │                          │
  │── close frame ─────────→│  (client initiates close)
  │←── close frame ─────────│  (server acknowledges)
  │                          │
```

### readyState Values

| Value | Constant | Meaning |
|-------|----------|---------|
| 0 | `CONNECTING` | Connection in progress |
| 1 | `OPEN` | Connection active, can send |
| 2 | `CLOSING` | Close handshake in progress |
| 3 | `CLOSED` | Connection closed |

## Common Mistakes

### Mistake 1: Sending Before Connection Opens

```js
// WRONG — send() fails if connection is not yet open
const ws = new WebSocket('ws://localhost:8080');
ws.send('Hello');  // Throws: WebSocket is not open

// CORRECT — wait for the 'open' event
ws.on('open', () => {
  ws.send('Hello');  // Safe — connection is established
});
```

### Mistake 2: Not Handling Binary vs Text

```js
// WRONG — assuming data is always a string
ws.on('message', (data) => {
  console.log(data.toUpperCase());  // Fails if data is binary Buffer
});

// CORRECT — check isBinary flag or convert
ws.on('message', (data, isBinary) => {
  const text = isBinary ? data.toString('base64') : data.toString();
  console.log(text);
});
```

### Mistake 3: Forgetting to Handle Connection Loss

```js
// WRONG — no reconnection logic
const ws = new WebSocket('ws://localhost:8080');
ws.on('message', console.log);
// If server restarts, client stays disconnected silently

// CORRECT — implement reconnection (see broadcast example for patterns)
ws.on('close', () => {
  console.log('Disconnected — reconnecting in 3s...');
  setTimeout(() => connect(), 3000);
});
```

## Try It Yourself

### Exercise 1: Echo Server

Start the server and connect with the browser client. Send 5 messages and verify each one is echoed back.

### Exercise 2: Chat Room

Modify the server to assign each client a username. When a message arrives, broadcast it as "username: message" to all other clients.

### Exercise 3: JSON Protocol

Modify the server to only accept JSON messages. Parse incoming messages and reject anything that is not valid JSON with an error response.

## Next Steps

You have a working WebSocket connection. Let's add broadcast, heartbeat, and connection management. Continue to [Broadcast & Heartbeat](./02-broadcast.md).
