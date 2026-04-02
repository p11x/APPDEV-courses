# Server-Sent Events Basics

## What You'll Learn

- What Server-Sent Events (SSE) are and when to use them
- How the `text/event-stream` content type works
- How to create an SSE endpoint with `res.write()`
- How to handle reconnection with the `retry` field
- How to consume SSE in the browser with `EventSource`

## What Are Server-Sent Events?

Server-Sent Events (SSE) let a server push data to a browser over a standard HTTP connection. Unlike WebSockets, SSE is **one-directional**: the server sends, the client receives. There is no way for the client to send data through the SSE connection itself.

SSE is built into the HTTP protocol. No special libraries are needed on the server, and browsers have a native `EventSource` API.

```
WebSocket: Client ↔ Server (bidirectional)
SSE:       Server → Client (server push only)
HTTP Poll: Client → Server → Client (repeated requests)
```

## When to Use SSE

- Live news feeds or stock tickers
- Progress updates for long-running tasks
- Notification systems
- Log streaming

Do **not** use SSE when the client needs to send frequent messages back — use WebSockets instead.

> See: ../websockets/01-ws-basics.md for the bidirectional alternative.

## SSE Server

```js
// server.js — SSE endpoint using plain Node.js HTTP

import { createServer } from 'node:http';

// Track connected SSE clients
const clients = new Set();

const server = createServer((req, res) => {
  // SSE endpoint
  if (req.url === '/events') {
    // Set the required headers for SSE
    res.writeHead(200, {
      'Content-Type': 'text/event-stream',  // Tells the browser this is SSE
      'Cache-Control': 'no-cache',           // Do not cache — data is live
      'Connection': 'keep-alive',            // Keep the TCP connection open
    });

    // Send a retry directive — tells the browser to reconnect after 3 seconds
    // if the connection drops (this is a built-in SSE feature)
    res.write('retry: 3000\n\n');

    // Add this client to our tracking set
    clients.add(res);
    console.log(`Client connected (${clients.size} total)`);

    // Send an initial connected event
    // SSE format: "event: <type>\ndata: <json>\n\n"
    sendEvent(res, 'connected', { message: 'Subscribed to events' });

    // Remove client when they disconnect
    req.on('close', () => {
      clients.delete(res);
      console.log(`Client disconnected (${clients.size} remaining)`);
    });

    return;
  }

  // Regular HTTP endpoint to trigger an event
  if (req.url?.startsWith('/notify') && req.method === 'POST') {
    let body = '';
    req.on('data', (chunk) => { body += chunk; });
    req.on('end', () => {
      const { message } = JSON.parse(body);

      // Broadcast to all connected SSE clients
      for (const client of clients) {
        sendEvent(client, 'notification', {
          message,
          timestamp: new Date().toISOString(),
        });
      }

      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ sent: clients.size }));
    });
    return;
  }

  // Serve a basic HTML page for testing
  if (req.url === '/') {
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end(getClientHtml());
    return;
  }

  res.writeHead(404);
  res.end('Not found');
});

// Helper: send a named event with JSON data
// SSE message format:
//   event: <event-name>\n
//   data: <json-string>\n
//   id: <optional-id>\n\n
function sendEvent(res, eventName, data) {
  // Each event is terminated by a double newline (\n\n)
  res.write(`event: ${eventName}\n`);
  res.write(`data: ${JSON.stringify(data)}\n`);
  res.write(`id: ${Date.now()}\n\n`);  // id lets the browser track last-received event
}

// Generate test HTML page
function getClientHtml() {
  return `
<!DOCTYPE html>
<html lang="en">
<head><meta charset="UTF-8"><title>SSE Demo</title></head>
<body>
  <h1>Server-Sent Events Demo</h1>
  <div id="log"></div>
  <button onclick="notify()">Send Notification</button>
  <script>
    const log = document.getElementById('log');

    // EventSource is a browser built-in for consuming SSE
    const es = new EventSource('/events');

    // Listen for the 'connected' event (matches the event name from the server)
    es.addEventListener('connected', (e) => {
      const data = JSON.parse(e.data);
      log.innerHTML += '<p>[connected] ' + data.message + '</p>';
    });

    // Listen for 'notification' events
    es.addEventListener('notification', (e) => {
      const data = JSON.parse(e.data);
      log.innerHTML += '<p>[notification] ' + data.message + ' at ' + data.timestamp + '</p>';
    });

    // 'message' event fires for events without a named event type
    es.onmessage = (e) => {
      log.innerHTML += '<p>[message] ' + e.data + '</p>';
    };

    // 'error' fires when the connection drops
    // EventSource automatically reconnects using the retry interval
    es.onerror = () => {
      log.innerHTML += '<p>[error] Connection lost — reconnecting...</p>';
    };

    function notify() {
      fetch('/notify', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ message: 'Hello from SSE!' }),
      });
    }
  </script>
</body>
</html>`;
}

const PORT = 3000;
server.listen(PORT, () => {
  console.log(`SSE server on http://localhost:${PORT}`);
});
```

## SSE with Express

```js
// express-sse.js — SSE endpoint using Express

import express from 'express';

const app = express();

// Middleware to parse JSON bodies
app.use(express.json());

// Store connected clients
const clients = new Set();

// SSE endpoint
app.get('/events', (req, res) => {
  // Set SSE headers
  res.writeHead(200, {
    'Content-Type': 'text/event-stream',
    'Cache-Control': 'no-cache',
    'Connection': 'keep-alive',
  });

  res.write('retry: 3000\n\n');
  clients.add(res);

  req.on('close', () => {
    clients.delete(res);
  });
});

// Broadcast endpoint
app.post('/broadcast', (req, res) => {
  const { event, data } = req.body;

  for (const client of clients) {
    client.write(`event: ${event}\n`);
    client.write(`data: ${JSON.stringify(data)}\n\n`);
  }

  res.json({ sent: clients.size });
});

app.listen(3000, () => {
  console.log('Express SSE server on http://localhost:3000');
});
```

## How It Works

### The SSE Wire Format

Each SSE message is a block of text with this format:

```
event: notification
data: {"message":"hello","timestamp":"2024-01-15T10:30:00Z"}
id: 1705312200000

```

- `event:` — the event name (optional; defaults to `message`)
- `data:` — the payload (typically JSON, but can be any string)
- `id:` — an identifier the browser sends back as `Last-Event-ID` on reconnect
- `retry:` — milliseconds to wait before reconnecting (sent once, not per message)
- Each message ends with **two newlines** (`\n\n`)

### Reconnection

When the connection drops, the browser automatically reconnects after the `retry` interval. On reconnect, it sends the `Last-Event-ID` header so the server can resume from where it left off.

```js
// Server-side: check Last-Event-ID to resume
app.get('/events', (req, res) => {
  const lastId = req.headers['last-event-id'];
  if (lastId) {
    // Send only events that occurred after lastId
  }
});
```

## Common Mistakes

### Mistake 1: Missing Double Newline

```js
// WRONG — browser never receives the event (no message terminator)
res.write(`event: chat\ndata: "hello"\n`);

// CORRECT — double newline terminates each event
res.write(`event: chat\ndata: "hello"\n\n`);
```

### Mistake 2: Forgetting Cache-Control Header

```js
// WRONG — proxies may cache the response, delivering stale data
res.writeHead(200, { 'Content-Type': 'text/event-stream' });

// CORRECT — disable caching for live streams
res.writeHead(200, {
  'Content-Type': 'text/event-stream',
  'Cache-Control': 'no-cache',
  'Connection': 'keep-alive',
});
```

### Mistake 3: Not Cleaning Up Disconnected Clients

```js
// WRONG — disconnected clients remain in the set, send() throws errors
const clients = new Set();
// No 'close' handler — Set grows forever with dead sockets

// CORRECT — remove clients on disconnect
req.on('close', () => clients.delete(res));
```

## Try It Yourself

### Exercise 1: Live Clock

Create an SSE endpoint that sends the current server time every second. Display it in the browser in real time.

### Exercise 2: Progress Bar

Create an endpoint that simulates a long task (e.g., file processing). Send progress percentage events (10%, 20%, ..., 100%) every 500ms. Display a progress bar in the browser.

### Exercise 3: Last-Event-ID Support

Modify the server to store the last 100 events. When a client reconnects with a `Last-Event-ID` header, send only the events it missed.

## Next Steps

You understand SSE. Let's compare it with WebSockets to know when to use each. Continue to [SSE vs WebSockets](./02-sse-vs-ws.md).
