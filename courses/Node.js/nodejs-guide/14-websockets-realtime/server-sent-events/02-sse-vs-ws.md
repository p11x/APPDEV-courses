# SSE vs WebSockets

## What You'll Learn

- The key differences between SSE and WebSockets
- When to choose SSE over WebSockets (and vice versa)
- How HTTP/2 improves SSE performance
- How to implement both protocols for the same feature and compare
- How proxy and browser support differ between the two

## Side-by-Side Comparison

| Feature | Server-Sent Events | WebSockets |
|---------|-------------------|------------|
| Direction | Server → Client (one-way) | Server ↔ Client (two-way) |
| Protocol | HTTP/1.1 or HTTP/2 | WebSocket (ws:// or wss://) |
| Library needed | None (browser built-in + plain HTTP) | `ws` or `socket.io` |
| Auto-reconnect | Built-in via `retry` field | Must implement manually |
| Text/Binary | Text only | Text and binary |
| Max connections | 6 per domain (HTTP/1.1), unlimited (HTTP/2) | Unlimited |
| Proxy/CDN friendly | Yes (standard HTTP) | Sometimes problematic |
| CORS | Standard HTTP CORS | Separate handshake |
| Browser support | All modern browsers | All modern browsers |

## When to Use SSE

Use SSE when:

- The server needs to **push updates** to the client (news feed, notifications, live scores)
- The client **rarely sends data** back (or sends it via separate HTTP requests)
- You want **automatic reconnection** without extra code
- You need it to work through **proxies, CDNs, and load balancers** without configuration
- You want the **simplest possible implementation** (no library needed)

## When to Use WebSockets

Use WebSockets when:

- You need **bidirectional communication** (chat, collaborative editing, gaming)
- You send **binary data** (audio, video, file chunks)
- You need **very low latency** with frequent message exchange
- You are building a **real-time multiplayer game** or **live collaboration tool**

## Implementing the Same Feature Both Ways

### Notification System — SSE Version

```js
// notify-sse.js — Notifications via SSE

import { createServer } from 'node:http';

const clients = new Set();

createServer((req, res) => {
  if (req.url === '/notifications') {
    res.writeHead(200, {
      'Content-Type': 'text/event-stream',
      'Cache-Control': 'no-cache',
      'Connection': 'keep-alive',
    });
    res.write('retry: 3000\n\n');
    clients.add(res);
    req.on('close', () => clients.delete(res));
    return;
  }

  if (req.url === '/send' && req.method === 'POST') {
    let body = '';
    req.on('data', (chunk) => { body += chunk; });
    req.on('end', () => {
      const { message } = JSON.parse(body);
      for (const client of clients) {
        client.write(`event: notification\n`);
        client.write(`data: ${JSON.stringify({ message, ts: Date.now() })}\n\n`);
      }
      res.writeHead(200);
      res.end('OK');
    });
    return;
  }

  res.writeHead(200, { 'Content-Type': 'text/html' });
  res.end(`
    <h1>SSE Notifications</h1>
    <div id="log"></div>
    <script>
      const es = new EventSource('/notifications');
      es.addEventListener('notification', (e) => {
        const d = JSON.parse(e.data);
        log.innerHTML += '<p>' + d.message + '</p>';
      });
    </script>
  `);
}).listen(3000);
```

### Notification System — WebSocket Version

```js
// notify-ws.js — Notifications via WebSocket

import { createServer } from 'node:http';
import { WebSocketServer } from 'ws';

const httpServer = createServer((req, res) => {
  if (req.url === '/send' && req.method === 'POST') {
    let body = '';
    req.on('data', (chunk) => { body += chunk; });
    req.on('end', () => {
      const { message } = JSON.parse(body);
      const data = JSON.stringify({ type: 'notification', message, ts: Date.now() });
      wss.clients.forEach((c) => {
        if (c.readyState === 1) c.send(data);
      });
      res.writeHead(200);
      res.end('OK');
    });
    return;
  }

  res.writeHead(200, { 'Content-Type': 'text/html' });
  res.end(`
    <h1>WebSocket Notifications</h1>
    <div id="log"></div>
    <script>
      const ws = new WebSocket('ws://localhost:3000');
      ws.onmessage = (e) => {
        const d = JSON.parse(e.data);
        if (d.type === 'notification') log.innerHTML += '<p>' + d.message + '</p>';
      };
    </script>
  `);
});

const wss = new WebSocketServer({ server: httpServer });
httpServer.listen(3000);
```

### Comparing the Two

For this notification use case, the SSE version is simpler because:
- No library needed on the server
- No reconnection code needed on the client
- Standard HTTP headers handle CORS and caching
- The client never sends data through the connection (uses `/send` endpoint instead)

## HTTP/2 and SSE

SSE over HTTP/1.1 is limited to **6 concurrent connections per domain** (a browser-imposed limit). HTTP/2 removes this limit because it multiplexes streams over a single TCP connection.

```
HTTP/1.1: 6 tabs × 1 SSE connection = 6 max (7th tab is blocked)
HTTP/2:   Unlimited tabs × 1 SSE connection = all work simultaneously
```

To enable HTTP/2 with SSE:

```js
// http2-sse.js — SSE over HTTP/2

import { createSecureServer } from 'node:http2';
import { readFileSync } from 'node:fs';

const server = createSecureServer({
  key: readFileSync('server-key.pem'),   // Self-signed or Let's Encrypt
  cert: readFileSync('server-cert.pem'),
});

server.on('stream', (stream, headers) => {
  const path = headers[':path'];

  if (path === '/events') {
    // HTTP/2 uses stream.respond() instead of res.writeHead()
    stream.respond({
      'content-type': 'text/event-stream',
      'cache-control': 'no-cache',
      ':status': 200,
    });

    // stream.write() works the same as res.write()
    const interval = setInterval(() => {
      stream.write(`data: ${JSON.stringify({ time: Date.now() })}\n\n`);
    }, 1000);

    stream.on('close', () => clearInterval(interval));
  }
});

server.listen(3001, () => {
  console.log('HTTP/2 SSE server on https://localhost:3001');
});
```

## Common Mistakes

### Mistake 1: Using SSE for Chat Applications

```js
// WRONG — chat requires bidirectional communication
// SSE can only push from server to client
// Client must use a separate HTTP request for each message
// This adds latency and complexity

// CORRECT — use WebSockets for chat (see ../websockets/01-ws-basics.md)
```

### Mistake 2: Opening Multiple SSE Connections

```js
// WRONG — each new EventSource opens a new TCP connection
// On HTTP/1.1, you hit the 6-connection limit quickly
const es1 = new EventSource('/news');
const es2 = new EventSource('/stocks');
const es3 = new EventSource('/chat');  // May be blocked

// CORRECT — use one EventSource and multiplex event types
const es = new EventSource('/events');  // Single connection
es.addEventListener('news', handleNews);
es.addEventListener('stocks', handleStocks);
es.addEventListener('chat', handleChat);
```

### Mistake 3: Not Handling Event ID for Recovery

```js
// WRONG — on reconnect, client gets duplicate or missing events
// No id field sent — browser cannot tell the server where it left off

// CORRECT — send an id with each event
res.write(`id: ${lastEventId}\n`);
res.write(`data: ${JSON.stringify(data)}\n\n`);

// And check Last-Event-ID on reconnect
const lastId = req.headers['last-event-id'];
if (lastId) {
  // Send events that occurred after lastId
}
```

## Try It Yourself

### Exercise 1: Build Both Protocols

Build the same "live dashboard" feature twice: once with SSE and once with WebSockets. The dashboard shows server uptime and memory usage, updated every second. Compare the code complexity.

### Exercise 2: Connection Count Test

Open 10 browser tabs with SSE connections on HTTP/1.1. Observe what happens. Then try the same with WebSockets. Document the difference.

### Exercise 3: Reconnection Test

Start the SSE server, connect a client, then stop and restart the server. Verify that the client automatically reconnects and resumes receiving events.

## Next Steps

You understand when to use SSE and when to use WebSockets. For a completely different API paradigm, continue to [Chapter 15: GraphQL](../../15-graphql/schema-first/01-graphql-setup.md).
