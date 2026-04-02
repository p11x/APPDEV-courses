# HTTP/2, gRPC, and Advanced Network Streaming

## What You'll Learn

- HTTP/2 server push streams
- HTTP/2 multiplexed stream handling
- gRPC streaming (server, client, bidirectional)
- WebSocket stream adapter patterns
- Server-Sent Events as Readable stream
- `fetch()` with streaming response body
- Real-world: real-time data feed using HTTP/2 + SSE

---

## 1. HTTP/2 Server Push

Push resources before the client requests them.

```js
import { createSecureServer } from 'node:http2';
import { readFileSync } from 'node:fs';

createSecureServer({
  key: readFileSync('./certs/server.key'),
  cert: readFileSync('./certs/server.crt'),
}).on('stream', (stream, headers) => {
  if (headers[':path'] === '/') {
    stream.pushStream({ ':path': '/style.css' }, (err, push) => {
      if (!err) { push.respond({ ':status': 200, 'content-type': 'text/css' }); push.end('body{}'); }
    });
    stream.pushStream({ ':path': '/app.js' }, (err, push) => {
      if (!err) { push.respond({ ':status': 200, 'content-type': 'text/javascript' }); push.end('console.log("ok")'); }
    });
    stream.respond({ ':status': 200, 'content-type': 'text/html' });
    stream.end('<link rel="stylesheet" href="/style.css"><script src="/app.js"></script>');
  }
}).listen(8443, () => console.log('HTTP/2 on :8443'));
```

---

## 2. HTTP/2 Multiplexed Streams

Each stream is independent over a single TCP connection.

```js
import { createSecureServer } from 'node:http2';
import { readFileSync } from 'node:fs';

createSecureServer({
  key: readFileSync('./certs/server.key'),
  cert: readFileSync('./certs/server.crt'),
}).on('stream', (stream, headers) => {
  stream.on('error', (err) => console.error(`Stream ${stream.id}: ${err.message}`));
  if (headers[':path'] === '/api/data') {
    stream.respond({ ':status': 200, 'content-type': 'application/json' });
    stream.end(JSON.stringify({ id: stream.id, ts: Date.now() }));
  } else {
    const chunks = [];
    stream.on('data', (c) => chunks.push(c));
    stream.on('end', () => { stream.respond({ ':status': 200 }); stream.end(`${Buffer.concat(chunks).length} bytes`); });
  }
}).listen(8443);
```

### Multiplexed Client

```js
import { connect } from 'node:http2';
import { once } from 'node:events';

const session = connect('https://localhost:8443', { rejectUnauthorized: false });
async function request(path) {
  const req = session.request({ ':path': path });
  const [headers] = await once(req, 'response');
  const chunks = [];
  req.on('data', (c) => chunks.push(c));
  await once(req, 'end');
  req.close();
  return { status: headers[':status'], body: Buffer.concat(chunks).toString() };
}

// 3 concurrent requests over one connection
const [r1, r2, r3] = await Promise.all([request('/api/data'), request('/api/data'), request('/api/data')]);
session.close();
```

---

## 3. gRPC Streaming

```bash
npm install @grpc/grpc-js @grpc/proto-loader
```

### Proto (`streaming.proto`)

```protobuf
syntax = "proto3";
service DataService {
  rpc Subscribe(SubscribeRequest) returns (stream DataEvent);
  rpc Upload(stream Chunk) returns (UploadStatus);
  rpc Chat(stream ChatMessage) returns (stream ChatMessage);
}
message SubscribeRequest { string topic = 1; }
message DataEvent { string topic = 1; string payload = 2; int64 timestamp = 3; }
message Chunk { bytes data = 1; int32 sequence = 2; }
message UploadStatus { int32 total_bytes = 1; string message = 2; }
message ChatMessage { string user = 1; string text = 2; int64 timestamp = 3; }
```

### Server

```js
import grpc from '@grpc/grpc-js';
import protoLoader from '@grpc/proto-loader';
import { join, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const pkg = grpc.loadPackageDefinition(
  protoLoader.loadSync(join(dirname(fileURLToPath(import.meta.url)), 'streaming.proto'), { keepCase: false }),
).dataservice;

function subscribe(call) {
  let n = 0;
  const iv = setInterval(() => {
    call.write({ topic: call.request.topic, payload: `Event ${++n}`, timestamp: Date.now() });
    if (n >= 10) { clearInterval(iv); call.end(); }
  }, 500);
}

function upload(call) {
  let bytes = 0, count = 0;
  call.on('data', (c) => { bytes += c.data?.length ?? 0; count++; });
  call.on('end', () => call.end({ total_bytes: bytes, message: `${count} chunks` }));
}

function chat(call) {
  call.on('data', (msg) => call.write({ ...msg, timestamp: Date.now() }));
  call.on('end', () => call.end());
}

const server = new grpc.Server();
server.addService(pkg.DataService.service, { subscribe, upload, chat });
server.bindAsync('0.0.0.0:50051', grpc.ServerCredentials.createInsecure(), () => console.log('gRPC on :50051'));
```

### Clients

```js
const client = new pkg.DataService('localhost:50051', grpc.credentials.createInsecure());

// Server streaming
const stream = client.subscribe({ topic: 'metrics' });
stream.on('data', (e) => console.log(`[${e.topic}] ${e.payload}`));
stream.on('end', () => console.log('Done'));

// Bidirectional
const chat = client.chat();
chat.on('data', (msg) => console.log(`[${msg.user}]: ${msg.text}`));
chat.write({ user: 'Alice', text: 'Hello!' });
setTimeout(() => chat.end(), 3000);
```

---

## 4. WebSocket Stream Adapter

```bash
npm install ws
```

```js
import { WebSocketServer } from 'ws';
import { Duplex, pipeline, Transform } from 'node:stream';

class WebSocketStream extends Duplex {
  constructor(ws) {
    super();
    this._ws = ws;
    ws.on('message', (d) => { if (!this.push(d)) ws.pause(); });
    ws.on('close', () => this.push(null));
    ws.on('error', (err) => this.destroy(err));
  }
  _read() { this._ws.resume(); }
  _write(c, _, cb) { this._ws.send(c, cb); }
  _final(cb) { this._ws.close(); cb(); }
}

const wss = new WebSocketServer({ port: 8080 });
wss.on('connection', (ws) => {
  const stream = new WebSocketStream(ws);
  const echo = new Transform({ transform(c, _, cb) { cb(null, Buffer.from(`Echo: ${c}`)); } });
  pipeline(stream, echo, stream, () => {});
});
```

---

## 5. Server-Sent Events as Readable Stream

### Server

```js
import { createServer } from 'node:http';

createServer((req, res) => {
  if (req.url === '/events') {
    res.writeHead(200, { 'Content-Type': 'text/event-stream', 'Cache-Control': 'no-cache' });
    let id = 0;
    const iv = setInterval(() => {
      res.write(`id: ${++id}\nevent: update\ndata: ${JSON.stringify({ ts: Date.now(), v: Math.random() })}\n\n`);
    }, 1000);
    req.on('close', () => { clearInterval(iv); res.end(); });
  } else {
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end('<script>new EventSource("/events").addEventListener("update",d=>document.body.innerHTML+="<p>"+d.data)</script>');
  }
}).listen(3000, () => console.log('SSE on :3000'));
```

### Client as Readable

---

## 6. `fetch()` with Streaming Response Body

Convert Web `ReadableStream` to Node.js `Readable` with `Readable.fromWeb()`.

```js
import { Readable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createWriteStream } from 'node:fs';

// Streaming download
const res = await fetch('https://example.com/large.zip');
await pipeline(Readable.fromWeb(res.body), createWriteStream('./large.zip'));
```

### Streaming NDJSON API

```js
import { Transform } from 'node:stream';

const source = Readable.fromWeb((await fetch('https://api.example.com/events')).body);

const parse = new Transform({
  objectMode: true,
  transform(c, _, cb) {
    for (const line of c.toString().split('\n'))
      if (line.trim()) { try { cb(null, JSON.parse(line)); } catch {} return; }
    cb();
  },
});

const filter = new Transform({ objectMode: true, transform(r, _, cb) { cb(null, r.type === 'error' ? r : undefined); } });
const serialize = new Transform({ objectMode: true, transform(r, _, cb) { cb(null, JSON.stringify(r) + '\n'); } });

await pipeline(source, parse, filter, serialize, createWriteStream('./errors.jsonl'));
```

---

## 7. Real-World: Real-Time Metrics Feed (HTTP/2 + SSE)

### Server

```js
import { createSecureServer } from 'node:http2';
import { readFileSync } from 'node:fs';

const metrics = [];
createSecureServer({
  key: readFileSync('./certs/server.key'),
  cert: readFileSync('./certs/server.crt'),
}).on('stream', (stream, headers) => {
  if (headers[':path'] === '/api/metrics/live') {
    stream.respond({ ':status': 200, 'content-type': 'text/event-stream', 'cache-control': 'no-cache' });
    const iv = setInterval(() => {
      const d = { ts: Date.now(), cpu: (Math.random() * 100).toFixed(1), rps: Math.floor(Math.random() * 10000) };
      metrics.push(d);
      if (metrics.length > 1000) metrics.shift();
      stream.write(`event: metrics\ndata: ${JSON.stringify(d)}\n\n`);
    }, 1000);
    stream.on('close', () => clearInterval(iv));
  } else {
    stream.respond({ ':status': 200, 'content-type': 'text/html' });
    stream.end(readFileSync('./dashboard.html'));
  }
}).listen(8443);
```

### Client Aggregator

```js
import { Readable, Transform, pipeline } from 'node:stream';

class SseMetrics extends Readable {
  constructor(url) {
    super({ objectMode: true });
    this._url = url;
    this._abort = new AbortController();
  }

  async _construct(cb) {
    try {
      const res = await fetch(this._url, { headers: { Accept: 'text/event-stream' }, signal: this._abort.signal });
      const reader = res.body.getReader();
      const dec = new TextDecoder();
      let buf = '';
      const pump = async () => {
        try {
          while (true) {
            const { done, value } = await reader.read();
            if (done) { this.push(null); return; }
            buf += dec.decode(value, { stream: true });
            const lines = buf.split('\n');
            buf = lines.pop();
            let data = '';
            for (const line of lines) {
              if (line.startsWith('data:')) data += line.slice(5).trim();
              else if (line === '' && data) { this.push(JSON.parse(data)); data = ''; }
            }
          }
        } catch (e) { if (e.name !== 'AbortError') this.destroy(e); }
      };
      pump();
      cb();
    } catch (e) { cb(e); }
  }
  _read() {}
  _destroy(_, cb) { this._abort.abort(); cb(); }
}

class Aggregator extends Transform {
  constructor(windowMs = 10000) {
    super({ objectMode: true });
    this.windowMs = windowMs;
    this.buf = [];
  }
  _transform(m, _, cb) {
    this.buf.push(m);
    this.buf = this.buf.filter((x) => Date.now() - x.ts < this.windowMs);
    if (this.buf.length) {
      this.push({ avgCpu: (this.buf.reduce((s, x) => s + parseFloat(x.cpu), 0) / this.buf.length).toFixed(1), samples: this.buf.length });
    }
    cb();
  }
}

await pipeline(
  new SseMetrics('https://localhost:8443/api/metrics/live'),
  new Aggregator(),
  async function* (src) {
    for await (const a of src) console.log(`Avg CPU: ${a.avgCpu}% (${a.samples} samples)`);
  },
);
```

---

## Best Practices Checklist

- [ ] HTTP/2 for multiplexed requests — avoids head-of-line blocking
- [ ] Handle `stream.on('error')` and `session.on('error')` in HTTP/2
- [ ] `pipeline()` for WebSocket stream adapters — backpressure + cleanup
- [ ] Correct `Content-Type` for SSE (`text/event-stream`) and NDJSON (`application/x-ndjson`)
- [ ] `AbortController` to cancel `fetch()` streams and clean up SSE
- [ ] `Readable.fromWeb()` to convert Web `ReadableStream` to Node.js `Readable`
- [ ] SSE reconnection in Node.js clients (browsers handle automatically)
- [ ] `@grpc/grpc-js` (pure JS) over native bindings for compatibility
- [ ] Buffer SSE events and replay via `Last-Event-ID` on reconnect
- [ ] HTTP/2 server push only for critical above-the-fold resources

---

## Cross-References

- [02-tcp-udp-socket-streams.md](./02-tcp-udp-socket-streams.md) — Low-level TCP patterns used by HTTP/2
- [02-directory-streaming-advanced.md](../04-stream-filesystem-integration/02-directory-streaming-advanced.md) — Streaming file reads for static assets
- [03-file-format-streaming.md](../04-stream-filesystem-integration/03-file-format-streaming.md) — NDJSON parsing for API responses

---

## Next Steps

- Implement TLS with `createSecureServer()` and proper certificate chains
- Build a gRPC gateway translating REST endpoints to gRPC calls
- Benchmark HTTP/2 multiplexing vs HTTP/1.1 connection pooling
- Explore `fetch()` with `duplex: 'half'` for streaming request bodies
- Implement WebSocket compression (`permessage-deflate`) for binary streams
