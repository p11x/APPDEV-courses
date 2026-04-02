# GraphQL Subscriptions, WebSocket, and SSE Streaming

## What You'll Learn

- GraphQL subscription implementation with `AsyncIterator`
- GraphQL file upload and download with streams
- WebSocket stream bidirectional communication adapter
- WebSocket binary stream transfer (files, images)
- Server-Sent Events (SSE) stream server implementation
- SSE client as a `Readable` stream
- HTTP/2 stream multiplexing with Node.js
- Real-world: real-time dashboard with GraphQL subscriptions + SSE fallback

## GraphQL Subscriptions with AsyncIterator

GraphQL subscriptions push events to clients over a persistent WebSocket connection via `graphql-ws`. The `PubSub` engine produces `AsyncIterator` instances per subscriber.

```javascript
import { createServer } from 'node:http';
import { makeExecutableSchema } from '@graphql-tools/schema';
import { createServer as createYoga } from 'graphql-yoga';
import { WebSocketServer } from 'ws';
import { useServer } from 'graphql-ws/lib/use/ws';
import { PubSub } from 'graphql-subscriptions';

const pubsub = new PubSub();

const typeDefs = /* GraphQL */ `
    type Query { _empty: String }
    type Subscription {
        messageAdded(channelId: ID!): Message!
        orderStatusUpdated(orderId: ID!): OrderStatus!
    }
    type Message { id: ID!, channelId: ID!, author: String!, text: String!, timestamp: String! }
    type OrderStatus { orderId: ID!, status: String!, updatedAt: String! }
`;

const resolvers = {
    Subscription: {
        messageAdded: {
            subscribe: (_, { channelId }) => pubsub.asyncIterator([`MSG_${channelId}`]),
        },
        orderStatusUpdated: {
            subscribe: (_, { orderId }) => pubsub.asyncIterator([`ORDER_${orderId}`]),
        },
    },
    Query: { _empty: () => null },
};

function publishMessage(channelId, message) {
    pubsub.publish(`MSG_${channelId}`, { messageAdded: message });
}

const schema = makeExecutableSchema({ typeDefs, resolvers });
const yoga = createYoga({ schema });
const httpServer = createServer(yoga);
useServer({ schema }, new WebSocketServer({ server: httpServer, path: '/graphql' }));
httpServer.listen(4000);
```

## GraphQL File Upload and Download with Streams

Yoga handles multipart uploads natively. Stream uploaded files to disk and serve downloads via piping.

```javascript
import { createServer } from 'node:http';
import { createServer as createYoga } from 'graphql-yoga';
import { makeExecutableSchema } from '@graphql-tools/schema';
import { createWriteStream, createReadStream } from 'node:fs';
import { mkdir, stat } from 'node:fs/promises';
import { join } from 'node:path';
import { pipeline } from 'node:stream/promises';
import { randomUUID } from 'node:crypto';

await mkdir('./uploads', { recursive: true });

const typeDefs = /* GraphQL */ `
    scalar Upload
    type Query { downloadFile(id: ID!): FileMeta }
    type Mutation { uploadFile(file: Upload!): FileMeta! }
    type FileMeta { id: ID!, filename: String!, size: Int!, mimetype: String }
`;

const store = new Map();

const resolvers = {
    Mutation: {
        uploadFile: async (_, { file }) => {
            const { createReadStream: factory, filename, mimetype } = await file;
            const id = randomUUID(), dest = join('./uploads', `${id}-${filename}`);
            await pipeline(factory(), createWriteStream(dest));
            const s = await stat(dest);
            const meta = { id, filename, size: s.size, mimetype };
            store.set(id, { ...meta, path: dest });
            return meta;
        },
    },
    Query: { downloadFile: (_, { id }) => store.get(id) || null },
};

const schema = makeExecutableSchema({ typeDefs, resolvers });
const yoga = createYoga({ schema });

createServer((req, res) => {
    if (req.url?.startsWith('/download/')) {
        const meta = store.get(req.url.split('/download/')[1]);
        if (!meta) { res.writeHead(404); res.end('Not found'); return; }
        res.writeHead(200, { 'Content-Type': meta.mimetype || 'application/octet-stream', 'Content-Length': meta.size });
        createReadStream(meta.path).pipe(res);
        return;
    }
    yoga(req, res);
}).listen(4000);
```

## WebSocket Bidirectional Stream Adapter

Wrap a WebSocket as a Node.js `Duplex` stream so `pipe()` and `pipeline()` work directly.

```javascript
import { createServer } from 'node:http';
import { WebSocketServer, WebSocket } from 'ws';
import { Duplex } from 'node:stream';

class WebSocketStream extends Duplex {
    constructor(ws) {
        super();
        this.ws = ws;
        this._queue = [];
        ws.on('message', (data) => {
            const buf = Buffer.isBuffer(data) ? data : Buffer.from(data);
            if (!this.push(buf)) this._queue.push(buf);
        });
        ws.on('close', () => this.push(null));
        ws.on('error', (err) => this.destroy(err));
    }
    _read() { while (this._queue.length) if (!this.push(this._queue.shift())) break; }
    _write(chunk, enc, cb) {
        if (this.ws.readyState === WebSocket.OPEN) this.ws.send(chunk, cb);
        else cb(new Error('WebSocket not open'));
    }
    _final(cb) { this.ws.close(); cb(); }
}

const wss = new WebSocketServer({ server: createServer().listen(8080) });
wss.on('connection', (ws) => {
    const stream = new WebSocketStream(ws);
    stream.pipe(stream); // Echo server
});
```

## WebSocket Binary Stream Transfer

Transfer large files over WebSocket in chunks with progress and backpressure handling.

```javascript
import { WebSocketServer, WebSocket } from 'ws';
import { createReadStream, createWriteStream } from 'node:fs';
import { mkdir } from 'node:fs/promises';
import { createServer } from 'node:http';

await mkdir('./ws-uploads', { recursive: true });

const wss = new WebSocketServer({ server: createServer().listen(8080), maxPayload: 200 * 1024 * 1024 });

wss.on('connection', (ws) => {
    let writer = null;
    ws.on('message', (data, isBinary) => {
        if (!isBinary) {
            const msg = JSON.parse(data.toString());
            if (msg.type === 'start') { writer = createWriteStream(`./ws-uploads/${msg.filename}`); ws.send(JSON.stringify({ type: 'ready' })); }
            return;
        }
        if (writer) {
            const ok = writer.write(Buffer.from(data));
            ws.send(JSON.stringify({ type: 'progress' }));
            if (!ok) { ws.pause(); writer.once('drain', () => ws.resume()); }
        }
    });
    ws.on('close', () => writer?.end());
});

// Client: stream a file with backpressure
async function uploadFile(filePath) {
    const ws = new WebSocket('ws://localhost:8080');
    await new Promise((r) => ws.on('open', r));
    ws.send(JSON.stringify({ type: 'start', filename: filePath.split(/[\\/]/).pop() }));
    await new Promise((r) => ws.on('message', (m) => { if (JSON.parse(m).type === 'ready') r(); }));
    for await (const chunk of createReadStream(filePath, { highWaterMark: 64 * 1024 })) {
        if (!ws.send(chunk)) await new Promise((r) => ws.once('drain', r));
    }
    ws.close();
}
```

## Server-Sent Events (SSE) Stream Server

A pure Node.js SSE server using `EventEmitter` as the event bus.

```javascript
import { createServer } from 'node:http';
import { EventEmitter } from 'node:events';

const bus = new EventEmitter();

createServer((req, res) => {
    if (req.url === '/events') {
        res.writeHead(200, { 'Content-Type': 'text/event-stream', 'Cache-Control': 'no-cache', 'Connection': 'keep-alive' });
        res.write('event: connected\ndata: {}\n\n');
        const hb = setInterval(() => res.write(': ping\n\n'), 15000);
        const handler = ({ type, data }) => res.write(`event: ${type}\ndata: ${JSON.stringify(data)}\n\n`);
        bus.on('broadcast', handler);
        req.on('close', () => { clearInterval(hb); bus.off('broadcast', handler); });
        return;
    }
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end('<pre id="log"></pre><script>const es=new EventSource("/events");es.addEventListener("metrics",e=>log.textContent+=e.data+"\\n");</script>');
}).listen(3000);

setInterval(() => bus.emit('broadcast', { type: 'metrics', data: { cpu: Math.random().toFixed(2), ts: Date.now() } }), 2000);
```

## SSE Client as a Readable Stream

Convert an SSE HTTP endpoint into a Node.js `Readable` stream for integration with `pipeline()`.

```javascript
import { Readable } from 'node:stream';

class SSEClient extends Readable {
    constructor(url) {
        super({ objectMode: true });
        this.url = url;
        this.controller = null;
    }

    async _read() {
        if (this.controller) return;
        this.controller = new AbortController();
        try {
            const res = await fetch(this.url, { signal: this.controller.signal });
            const reader = res.body.getReader(), decoder = new TextDecoder();
            let buffer = '';
            while (true) {
                const { done, value } = await reader.read();
                if (done) { this.push(null); break; }
                buffer += decoder.decode(value, { stream: true });
                const lines = buffer.split('\n');
                buffer = lines.pop();
                let eventType = 'message', eventData = '';
                for (const line of lines) {
                    if (line.startsWith('event: ')) eventType = line.slice(7);
                    else if (line.startsWith('data: ')) eventData = line.slice(6);
                    else if (line === '' && eventData) {
                        try { this.push({ type: eventType, data: JSON.parse(eventData) }); }
                        catch { this.push({ type: eventType, data: eventData }); }
                        eventType = 'message'; eventData = '';
                    }
                }
            }
        } catch (err) { if (err.name !== 'AbortError') this.destroy(err); }
    }
    _destroy() { this.controller?.abort(); }
}

// for await (const event of new SSEClient('http://localhost:3000/events')) {
//     console.log(`[${event.type}]`, event.data);
// }
```

## HTTP/2 Stream Multiplexing

Each HTTP/2 stream is an independent duplex stream. Multiplex many requests over a single TCP connection.

```javascript
import { createSecureServer } from 'node:http2';
import { readFileSync } from 'node:fs';
import { createReadStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';
import { createGzip } from 'node:zlib';

createSecureServer({
    key: readFileSync('localhost-key.pem'),
    cert: readFileSync('localhost-cert.pem'),
}).on('stream', (stream, headers) => {
    const path = headers[':path'];
    stream.respond({ ':status': 200, 'content-type': 'application/json' });

    if (path === '/api/stream') {
        let i = 0;
        const iv = setInterval(() => {
            stream.write(JSON.stringify({ tick: i++ }) + '\n');
            if (i >= 100) { stream.end(); clearInterval(iv); }
        }, 50);
        stream.on('close', () => clearInterval(iv));
        return;
    }
    if (path?.startsWith('/files/')) {
        pipeline(createReadStream(`./uploads/${path.split('/files/')[1]}`), createGzip(), stream)
            .catch((e) => { if (!stream.destroyed) stream.destroy(e); });
    }
}).listen(8443);
```

## Real-World: Real-Time Dashboard with GraphQL Subscriptions + SSE Fallback

A dashboard that provides metrics via GraphQL subscriptions over WebSocket, with an SSE fallback for legacy clients.

```javascript
import { createServer } from 'node:http';
import { makeExecutableSchema } from '@graphql-tools/schema';
import { createServer as createYoga } from 'graphql-yoga';
import { WebSocketServer } from 'ws';
import { useServer } from 'graphql-ws/lib/use/ws';
import { PubSub } from 'graphql-subscriptions';
import { EventEmitter } from 'node:events';

const pubsub = new PubSub();
const sseBus = new EventEmitter();

// Metrics simulation
setInterval(() => {
    const metrics = {
        cpu: (Math.random() * 100).toFixed(1),
        mem: (Math.random() * 100).toFixed(1),
        rps: Math.floor(Math.random() * 2000),
        ts: new Date().toISOString(),
    };
    pubsub.publish('METRICS', { metricsUpdated: metrics });
    sseBus.emit('metrics', metrics);
}, 1000);

// GraphQL schema
const typeDefs = /* GraphQL */ `
    type Query { currentMetrics: Metrics }
    type Subscription { metricsUpdated: Metrics! }
    type Metrics { cpu: String!, mem: String!, rps: Int!, ts: String! }
`;

const resolvers = {
    Query: { currentMetrics: () => ({ cpu: '0', mem: '0', rps: 0, ts: new Date().toISOString() }) },
    Subscription: { metricsUpdated: { subscribe: () => pubsub.asyncIterator(['METRICS']) } },
};

const schema = makeExecutableSchema({ typeDefs, resolvers });
const yoga = createYoga({ schema });
const httpServer = createServer(yoga);

// GraphQL subscriptions over WebSocket
useServer({ schema }, new WebSocketServer({ server: httpServer, path: '/graphql' }));

// SSE fallback route
yoga.use((req, res, next) => {
    if (req.url === '/sse/metrics') {
        res.writeHead(200, { 'Content-Type': 'text/event-stream', 'Cache-Control': 'no-cache', 'Connection': 'keep-alive' });
        res.write('event: connected\ndata: {}\n\n');
        const hb = setInterval(() => res.write(': ping\n\n'), 15000);
        const handler = (d) => res.write(`event: metrics\ndata: ${JSON.stringify(d)}\n\n`);
        sseBus.on('metrics', handler);
        req.on('close', () => { clearInterval(hb); sseBus.off('metrics', handler); });
        return;
    }
    if (req.url === '/sse/dashboard') {
        res.writeHead(200, { 'Content-Type': 'text/html' });
        res.end('<h1>Dashboard (SSE)</h1><pre id="log"></pre><script>const es=new EventSource("/sse/metrics");es.addEventListener("metrics",e=>log.textContent=e.data);</script>');
        return;
    }
    next();
});

httpServer.listen(4000, () => {
    console.log('GraphQL: http://localhost:4000/graphql');
    console.log('SSE:     http://localhost:4000/sse/metrics');
});
```

## Best Practices Checklist

- [ ] Use `pubsub.asyncIterator()` for GraphQL subscription resolvers
- [ ] Set `Cache-Control: no-cache` and `Connection: keep-alive` for SSE responses
- [ ] Send periodic heartbeat comments (`: ping\n\n`) to prevent SSE proxy timeouts
- [ ] Use binary WebSocket messages for file transfers, not base64
- [ ] Check `ws.send()` return value for backpressure
- [ ] Wrap WebSocket as `Duplex` to reuse `pipeline()` semantics
- [ ] Provide SSE fallback for environments where WebSocket is blocked
- [ ] Use HTTP/2 multiplexing for many concurrent streams over one connection
- [ ] Clean up event listeners and intervals on stream close
- [ ] Set `maxPayload` on `WebSocketServer` to prevent memory exhaustion

## Cross-References

- See [Koa & NestJS Streaming](./02-koa-nestjs-streaming.md) for framework-specific patterns
- See [HTTP Streaming](../05-stream-network-operations/01-http-streaming.md) for HTTP chunked transfer
- See [Transform Streams](../03-transform-streams/01-custom-transforms.md) for custom transforms
- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for stream error patterns

## Next Steps

Continue to [Stream Testing](../11-stream-testing/01-unit-testing.md).
