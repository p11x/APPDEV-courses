# Custom Transform and Duplex Stream Patterns

## What You'll Learn

- Custom Transform stream patterns for data conversion
- CSV-to-JSON parser, data enrichment transform
- Flush logic for buffered data in Transform streams
- Parallel Transform with async concurrency
- Custom Duplex: TCP protocol handler, WebSocket adapter
- Transform vs Duplex decision guide
- `compose()` for declarative pipeline construction (Node 16+)
- Middleware-style transform chain pattern

## Transform vs Duplex Decision Guide

| Use Case | Type | Why |
|----------|------|-----|
| Modify data flowing through | Transform | Output derived from input |
| Bidirectional with independent read/write | Duplex | Separate channels |
| Compression / decompression | Transform | Input bytes → compressed output |
| TCP socket | Duplex | Reads and writes independent |
| Data parsing (CSV→JSON) | Transform | Each chunk produces transformed output |

## Custom Transform Streams

### CSV-to-JSON Parser

Parses CSV input into JSON objects, handling quoted fields and streaming row-by-row.

```javascript
import { Transform } from 'node:stream';

class CsvToJsonTransform extends Transform {
  constructor(options = {}) {
    super({ objectMode: true });
    this.delimiter = options.delimiter || ',';
    this.headers = null;
    this.buffer = '';
  }

  _transform(chunk, encoding, callback) {
    this.buffer += chunk.toString();
    const lines = this.buffer.split('\n');
    this.buffer = lines.pop();

    for (const line of lines) {
      if (!line.trim()) continue;
      if (!this.headers) { this.headers = this.parseLine(line); continue; }
      const values = this.parseLine(line);
      const obj = {};
      this.headers.forEach((h, i) => { obj[h] = values[i] || ''; });
      this.push(obj);
    }
    callback();
  }

  _flush(callback) {
    if (this.buffer.trim() && this.headers) {
      const values = this.parseLine(this.buffer);
      const obj = {};
      this.headers.forEach((h, i) => { obj[h] = values[i] || ''; });
      this.push(obj);
    }
    callback();
  }

  parseLine(line) {
    const result = [];
    let current = '', inQuotes = false;
    for (const char of line) {
      if (char === '"') inQuotes = !inQuotes;
      else if (char === this.delimiter && !inQuotes) { result.push(current.trim()); current = ''; }
      else current += char;
    }
    result.push(current.trim());
    return result;
  }
}
```

### Data Enrichment Transform

Looks up and attaches additional data to each record passing through the stream.

```javascript
import { Transform } from 'node:stream';

class EnrichmentTransform extends Transform {
  constructor(lookupFn, options = {}) {
    super({ objectMode: true });
    this.lookupFn = lookupFn;
    this.concurrency = options.concurrency || 10;
    this.running = 0;
    this.pending = [];
  }

  async _transform(chunk, encoding, callback) {
    this.running++;
    try {
      this.push(await this.lookupFn(chunk));
    } catch (err) {
      this.push({ ...chunk, _enrichmentError: err.message });
    } finally {
      this.running--;
      this.drainPending();
    }
    if (this.running >= this.concurrency) this.pending.push(callback);
    else callback();
  }

  drainPending() {
    while (this.pending.length > 0 && this.running < this.concurrency) {
      this.pending.shift()();
    }
  }

  _flush(callback) {
    const check = () => {
      if (this.running === 0 && this.pending.length === 0) callback();
      else setImmediate(check);
    };
    check();
  }
}

const enricher = new EnrichmentTransform(
  async (order) => {
    const user = await db.collection('users').findOne({ _id: order.userId });
    return { ...order, userName: user.name };
  },
  { concurrency: 20 }
);
```

### Parallel Transform with Ordered Output

Processes chunks in parallel, preserving output order.

```javascript
import { Transform } from 'node:stream';

class ParallelTransform extends Transform {
  constructor(fn, options = {}) {
    super({ objectMode: options.objectMode !== false });
    this.fn = fn;
    this.concurrency = options.concurrency || 5;
    this.running = 0;
    this.queue = [];
    this.pending = new Map();
    this.nextSeq = 0;
    this.flushSeq = 0;
    this.flushCallback = null;
  }

  async _transform(chunk, encoding, callback) {
    this.queue.push({ seq: this.nextSeq++, callback, chunk });
    if (this.running < this.concurrency) this.processNext();
  }

  async processNext() {
    if (this.queue.length === 0) return;
    const { seq, callback, chunk } = this.queue.shift();
    this.running++;
    try {
      this.pending.set(seq, await this.fn(chunk, seq));
      callback();
      while (this.pending.has(this.flushSeq)) {
        this.push(this.pending.get(this.flushSeq));
        this.pending.delete(this.flushSeq++);
      }
      if (this.running === 0 && this.queue.length === 0 && this.flushCallback) {
        this.flushCallback();
      }
    } catch (err) { callback(err); }
    finally {
      this.running--;
      if (this.queue.length > 0) this.processNext();
    }
  }

  _flush(callback) {
    if (this.running === 0 && this.queue.length === 0) callback();
    else this.flushCallback = callback;
  }
}

const fetcher = new ParallelTransform(
  async (url) => {
    const res = await fetch(url);
    return { url, status: res.status, body: await res.text() };
  },
  { concurrency: 10 }
);
```

## Custom Duplex Streams

### TCP Protocol Handler

A Duplex wrapping a TCP socket with JSON protocol framing.

```javascript
import { Duplex } from 'node:stream';
import { connect } from 'node:net';

class ProtocolHandler extends Duplex {
  constructor(host, port, options = {}) {
    super({ objectMode: true });
    this.host = host;
    this.port = port;
    this.delimiter = options.delimiter || '\n';
    this.buffer = '';
    this.socket = null;
  }

  _construct(callback) {
    this.socket = connect({ host: this.host, port: this.port }, callback);
    this.socket.on('data', (data) => {
      this.buffer += data.toString();
      const lines = this.buffer.split(this.delimiter);
      this.buffer = lines.pop();
      for (const line of lines) {
        if (line.trim()) {
          try { this.push(JSON.parse(line)); }
          catch { this.push({ raw: line }); }
        }
      }
    });
    this.socket.on('error', (err) => this.destroy(err));
    this.socket.on('close', () => this.push(null));
  }

  _read() {}
  _write(chunk, encoding, callback) {
    const payload = (typeof chunk === 'string' ? chunk : JSON.stringify(chunk)) + this.delimiter;
    this.socket.write(payload, callback);
  }
  _destroy(err, callback) {
    if (this.socket) this.socket.destroy();
    callback(err);
  }
}

const handler = new ProtocolHandler('localhost', 9000);
handler.write({ type: 'subscribe', channel: 'orders' });
handler.on('data', (msg) => console.log('Received:', msg));
```

### WebSocket Duplex Adapter

Adapts a WebSocket into a Node.js Duplex stream.

```javascript
import { Duplex } from 'node:stream';
import { WebSocket } from 'ws';

class WebSocketDuplex extends Duplex {
  constructor(ws, options = {}) {
    super({ objectMode: options.objectMode || false });
    this.ws = ws;
    this.readBuffer = [];
    this.readCallback = null;

    ws.on('message', (data) => {
      const chunk = typeof data === 'string' ? data : Buffer.from(data);
      if (this.readCallback) {
        const cb = this.readCallback;
        this.readCallback = null;
        cb(null, chunk);
      } else { this.readBuffer.push(chunk); }
    });
    ws.on('close', () => this.push(null));
    ws.on('error', (err) => this.destroy(err));
  }

  _read() {
    if (this.readBuffer.length > 0) {
      while (this.readBuffer.length > 0) {
        if (!this.push(this.readBuffer.shift())) return;
      }
    } else {
      this.readCallback = (err, chunk) => {
        if (err) return this.destroy(err);
        this.push(chunk);
      };
    }
  }
  _write(chunk, encoding, callback) { this.ws.send(chunk, callback); }
  _final(callback) { this.ws.close(); callback(); }

  static create(url, protocols, options) {
    const ws = new WebSocket(url, protocols);
    return new Promise((resolve, reject) => {
      ws.on('open', () => resolve(new WebSocketDuplex(ws, options)));
      ws.on('error', reject);
    });
  }
}

const wsStream = await WebSocketDuplex.create('ws://localhost:8080/events');
wsStream.write(JSON.stringify({ subscribe: 'orders' }));
```

## Stream Composition with `compose()` (Node 16+)

`compose()` returns a single Duplex from multiple streams, making pipelines reusable.

```javascript
import { compose, Transform } from 'node:stream';
import { createReadStream, createWriteStream } from 'node:fs';
import { createGzip } from 'node:zlib';

const compressPipeline = compose(
  new Transform({
    transform(chunk, enc, cb) {
      cb(null, Buffer.concat([Buffer.from('MAGIC_HEADER'), chunk]));
    },
  }),
  createGzip()
);

createReadStream('data.bin').pipe(compressPipeline).pipe(createWriteStream('data.bin.gz'));
```

## Middleware-Style Transform Chain

Express-like middleware for stream processing with pluggable transformations.

```javascript
import { Transform } from 'node:stream';

class StreamMiddleware {
  constructor() { this.middlewares = []; }
  use(fn) { this.middlewares.push(fn); return this; }

  toTransform() {
    const middlewares = this.middlewares;
    return new Transform({
      objectMode: true,
      async transform(chunk, encoding, callback) {
        let index = 0, result = chunk;
        const next = async (err, data) => {
          if (err) return callback(err);
          if (data !== undefined) result = data;
          if (index >= middlewares.length) { this.push(result); return callback(); }
          try { await middlewares[index++](result, next); }
          catch (e) { callback(e); }
        };
        await next(null, chunk);
      },
    });
  }
}

const pipeline = new StreamMiddleware()
  .use(async (record, next) => {
    if (!record.email) return next(new Error('Missing email'));
    next(null, record);
  })
  .use(async (record, next) => {
    record.email = record.email.trim().toLowerCase();
    next(null, record);
  })
  .use(async (record, next) => {
    record.domain = record.email.split('@')[1];
    next(null, record);
  })
  .toTransform();
```

## Best Practices Checklist

- [ ] Choose Transform when input and output are the same logical data stream
- [ ] Choose Duplex when read and write are independent channels
- [ ] Always implement `_flush()` if you buffer data internally
- [ ] Use `compose()` (Node 16+) to build reusable multi-stream pipelines
- [ ] Apply concurrency limits in parallel transforms to control memory usage
- [ ] Emit custom events (`'batch-written'`, `'enrichment-error'`) for observability
- [ ] Handle errors in transforms gracefully — log and skip vs. fail the pipeline
- [ ] Use `objectMode: true` when transforming between different data representations
- [ ] Keep transforms stateless when possible; use `_flush()` for stateful cleanup
- [ ] Prefer `compose()` over manual `pipe()` chains for cleaner composition

## Cross-References

- [Custom Readable and Writable Streams](./01-custom-readable-writable.md) for data sources and sinks
- [Stream Piping and Composition](./03-stream-piping-composition.md) for connecting streams
- [Transform Streams](../streams/04-transform-streams.md) for Transform fundamentals
- [Stream Error Handling](../07-stream-error-handling/01-error-patterns.md) for error propagation

## Next Steps

- Master `pipeline()` and `compose()` for production stream architectures ([Stream Piping and Composition](./03-stream-piping-composition.md))
- Explore HTTP and network streaming with custom Duplex wrappers
- Apply concurrency patterns to parallelize transform operations
