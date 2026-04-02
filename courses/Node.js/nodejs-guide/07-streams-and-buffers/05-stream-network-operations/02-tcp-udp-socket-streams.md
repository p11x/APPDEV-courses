# TCP and UDP Socket Stream Communication

## What You'll Learn

- TCP server with stream-based client handling
- TCP client as Duplex stream
- Line-delimited protocol over TCP streams
- Binary protocol framing over TCP (length-prefixed messages)
- UDP datagram to stream adapter pattern
- TCP connection pooling with streams
- Real-world: Redis-like protocol implementation over TCP streams

---

## 1. TCP Server with Stream Client Handling

Each TCP connection is a `Duplex` stream:

```js
import { createServer } from 'node:net';
import { Transform, pipeline } from 'node:stream';

createServer((socket) => {
  socket.setEncoding('utf8');
  const addr = `${socket.remoteAddress}:${socket.remotePort}`;
  socket.write(`Welcome ${addr}\n`);
  const upper = new Transform({ transform(c, _, cb) { cb(null, c.toString().toUpperCase()); } });
  pipeline(socket, upper, socket, (err) => { if (err) console.error(err.message); });
}).listen(3000, () => console.log('TCP echo on :3000'));
```
---

## 2. TCP Client as Duplex Stream

```js
import { Socket } from 'node:net';
import { pipeline } from 'node:stream';
import { stdin, stdout } from 'node:process';
import { on } from 'node:events';

// Pipe stdin → server → stdout
const client = new Socket();
client.connect(3000, '127.0.0.1');
client.setEncoding('utf8');
client.pipe(stdout);
pipeline(stdin, client, (err) => { if (err) console.error(err.message); });

// Async iterable variant
async function tcpConnect(host, port) {
  const s = new Socket();
  s.connect(port, host);
  await new Promise((r) => s.on('connect', r));
  return { socket: s, async *[Symbol.asyncIterator]() { for await (const [d] of on(s, 'data')) yield d; }, write(d) { return s.write(d); }, end() { return s.end(); } };
}
```

---

## 3. Line-Delimited Protocol over TCP

```js
import { createServer } from 'node:net';
import { createInterface } from 'node:readline';

createServer((socket) => {
  socket.setEncoding('utf8');
  const rl = createInterface({ input: socket, crlfDelay: Infinity });
  rl.on('line', (line) => {
    const [cmd, ...args] = line.trim().split(' ');
    switch (cmd?.toUpperCase()) {
      case 'PING': socket.write('PONG\n'); break;
      case 'ECHO': socket.write(args.join(' ') + '\n'); break;
      case 'TIME': socket.write(new Date().toISOString() + '\n'); break;
      case 'QUIT': socket.write('BYE\n'); socket.end(); break;
      default: socket.write(`ERR unknown: ${cmd}\n`);
    }
  });
}).listen(3002, () => console.log('Line protocol on :3002'));
```

### Client

```js
import { createInterface } from 'node:readline';

class LineClient {
  constructor(host, port) {
    this.socket = new Socket();
    this.socket.connect(port, host);
    this.socket.setEncoding('utf8');
    this.rl = createInterface({ input: this.socket });
  }
  async command(cmd) {
    return new Promise((r) => { this.socket.write(cmd + '\n'); this.rl.once('line', r); });
  }
  close() { this.socket.end(); }
}

const c = new LineClient('127.0.0.1', 3002);
console.log(await c.command('PING'));  // PONG
c.close();
```

---

## 4. Binary Protocol Framing (Length-Prefixed)

Prefix each message with a 4-byte big-endian length header.

```js
import { Transform } from 'node:stream';

class LengthPrefixedEncoder extends Transform {
  _transform(chunk, _, cb) {
    const body = Buffer.isBuffer(chunk) ? chunk : Buffer.from(chunk);
    const h = Buffer.alloc(4);
    h.writeUInt32BE(body.length, 0);
    this.push(Buffer.concat([h, body]));
    cb();
  }
}

class LengthPrefixedDecoder extends Transform {
  constructor() { super(); this._buf = Buffer.alloc(0); }
  _transform(chunk, _, cb) {
    this._buf = Buffer.concat([this._buf, chunk]);
    while (this._buf.length >= 4) {
      const len = this._buf.readUInt32BE(0);
      if (this._buf.length < 4 + len) break;
      this.push(this._buf.subarray(4, 4 + len));
      this._buf = this._buf.subarray(4 + len);
    }
    cb();
  }
}
```

### Framed Server and Client

```js
import { createServer } from 'node:net';
import { pipeline } from 'node:stream';

createServer((socket) => {
  const handler = new Transform({
    transform(c, _, cb) { cb(null, Buffer.from(JSON.stringify({ echo: JSON.parse(c), ts: Date.now() }))); },
  });
  pipeline(socket, new LengthPrefixedDecoder(), handler, new LengthPrefixedEncoder(), socket, () => {});
}).listen(3003);

// Client
const s = new Socket();
s.connect(3003, '127.0.0.1', () => {
  const enc = new LengthPrefixedEncoder(), dec = new LengthPrefixedDecoder();
  enc.pipe(s).pipe(dec);
  dec.on('data', (m) => { console.log('Response:', JSON.parse(m)); s.end(); });
  enc.write(Buffer.from(JSON.stringify({ action: 'get', key: 'foo' })));
});
```

---

## 5. UDP Datagram to Stream Adapter

```js
import { createSocket } from 'node:dgram';
import { Readable, Writable, Transform, pipeline } from 'node:stream';

class UdpReadable extends Readable {
  constructor(type, port, host) {
    super({ objectMode: true });
    this._s = createSocket(type);
    this._port = port; this._host = host;
  }
  _construct(cb) {
    this._s.bind(this._port, this._host, cb);
    this._s.on('message', (msg, ri) => this.push({ data: msg, remote: { address: ri.address, port: ri.port } }));
    this._s.on('error', (err) => this.destroy(err));
  }
  _read() {}
  _destroy(err, cb) { this._s.close(); cb(err); }
}

class UdpWritable extends Writable {
  constructor(type, host, port) {
    super({ objectMode: true });
    this._s = createSocket(type);
    this._host = host; this._port = port;
  }
  _construct(cb) { this._s.bind(cb); }
  _write({ data, remote }, _, cb) {
    this._s.send(data, remote?.port ?? this._port, remote?.address ?? this._host, cb);
  }
  _destroy(err, cb) { this._s.close(); cb(err); }
}

const handler = new Transform({
  objectMode: true,
  transform({ data, remote }, _, cb) {
    console.log(`${remote.address}:${remote.port}: ${data}`);
    cb(null, { data: Buffer.from(`Echo: ${data}`), remote });
  },
});
pipeline(new UdpReadable('udp4', 41234, '0.0.0.0'), handler, new UdpWritable('udp4'), () => {});
```

---

## 6. TCP Connection Pooling

```js
import { Socket } from 'node:net';

class TcpPool {
  constructor(host, port, { max = 10 } = {}) {
    this.host = host; this.port = port; this.max = max;
    this._avail = []; this._all = []; this._waiting = [];
  }
  async acquire() {
    if (this._avail.length) return this._avail.shift();
    if (this._all.length < this.max) return this._create();
    return new Promise((r) => this._waiting.push(r));
  }
  release(c) {
    if (this._waiting.length) this._waiting.shift()(c);
    else this._avail.push(c);
  }
  _create() {
    return new Promise((r, j) => {
      const s = new Socket();
      s.connect(this.port, this.host, () => { this._all.push(s); r(s); });
      s.on('error', j);
    });
  }
  destroy() { for (const c of this._all) c.destroy(); }
}

const pool = new TcpPool('127.0.0.1', 3000, { max: 5 });
async function send(data) {
  const c = await pool.acquire();
  try {
    return await new Promise((r, j) => { c.write(data); c.once('data', r); c.once('error', j); });
  } finally { pool.release(c); }
}
```

---

## 7. Real-World: Redis-Like Protocol (RESP) over TCP

### RESP Parser and Response Encoder

```js
import { Transform } from 'node:stream';

class RespParser extends Transform {
  constructor() { super({ objectMode: true }); this._buf = ''; }
  _transform(c, _, cb) {
    this._buf += c.toString();
    while (true) { const r = this._parse(); if (!r) break; this.push(r); }
    cb();
  }
  _parse() {
    if (!this._buf.length) return null;
    if (this._buf[0] === '*') {
      const e = this._buf.indexOf('\r\n');
      if (e === -1) return null;
      const n = parseInt(this._buf.slice(1, e));
      this._buf = this._buf.slice(e + 2);
      const args = [];
      for (let i = 0; i < n; i++) {
        const le = this._buf.indexOf('\r\n');
        if (le === -1) return null;
        const len = parseInt(this._buf.slice(1, le));
        if (this._buf.length < le + 2 + len + 2) return null;
        args.push(this._buf.slice(le + 2, le + 2 + len));
        this._buf = this._buf.slice(le + 2 + len + 2);
      }
      return args;
    }
    const e = this._buf.indexOf('\r\n');
    if (e === -1) return null;
    const cmd = this._buf.slice(0, e).split(' ');
    this._buf = this._buf.slice(e + 2);
    return cmd;
  }
}

function resp(v) {
  if (v === null) return '$-1\r\n';
  if (typeof v === 'string') return `+${v}\r\n`;
  if (typeof v === 'number') return `:${v}\r\n`;
  if (Array.isArray(v)) return `*${v.length}\r\n${v.map(resp).join('')}`;
  return `+${String(v)}\r\n`;
}
```

### Server

```js
import { createServer } from 'node:net';

const store = new Map();
createServer((socket) => {
  const p = new RespParser();
  socket.pipe(p);
  p.on('data', (args) => {
    const cmd = args[0]?.toUpperCase();
    let r;
    switch (cmd) {
      case 'PING': r = resp('PONG'); break;
      case 'SET': store.set(args[1], args[2]); r = resp('OK'); break;
      case 'GET': r = resp(store.get(args[1]) ?? null); break;
      case 'DEL': r = resp(store.delete(args[1]) ? 1 : 0); break;
      case 'KEYS': r = resp([...store.keys()]); break;
      default: r = resp(`ERR unknown '${cmd}'`);
    }
    socket.write(r);
  });
}).listen(6380, () => console.log('Redis-like on :6380'));
```

### Client

```js
class RespClient {
  constructor(host = '127.0.0.1', port = 6380) {
    this.socket = new Socket();
    this.socket.connect(port, host);
    this.socket.setEncoding('utf8');
    this._buf = '';
  }
  _encode(args) {
    let c = `*${args.length}\r\n`;
    for (const a of args) { const s = String(a); c += `$${Buffer.byteLength(s)}\r\n${s}\r\n`; }
    return c;
  }
  async command(...args) {
    return new Promise((r) => {
      this.socket.write(this._encode(args));
      const onData = (d) => {
        this._buf += d;
        const v = this._parse();
        if (v !== undefined) { this.socket.removeListener('data', onData); r(v); }
      };
      this.socket.on('data', onData);
    });
  }
  _parse() {
    const t = this._buf[0], e = this._buf.indexOf('\r\n');
    if (e === -1) return undefined;
    if (t === '+') { this._buf = this._buf.slice(e + 2); return this._buf.slice(0, e); }
    if (t === ':') { this._buf = this._buf.slice(e + 2); return parseInt(this._buf.slice(1, e)); }
    if (t === '$') {
      const len = parseInt(this._buf.slice(1, e));
      if (len === -1) { this._buf = this._buf.slice(e + 2); return null; }
      if (this._buf.length < e + 2 + len + 2) return undefined;
      const v = this._buf.slice(e + 2, e + 2 + len);
      this._buf = this._buf.slice(e + 2 + len + 2);
      return v;
    }
    return undefined;
  }
  close() { this.socket.end(); }
}

const c = new RespClient();
console.log(await c.command('PING'));           // PONG
console.log(await c.command('GET', 'k'));        // v
c.close();
```

---

## Best Practices Checklist

- [ ] `pipeline()` for TCP — error propagation + cleanup on disconnect
- [ ] `socket.setEncoding('utf8')` for text; `Buffer` for binary
- [ ] Handle `ECONNRESET` — client disconnects are normal
- [ ] Length-prefix framing for binary protocols
- [ ] Connection pooling for high-throughput scenarios
- [ ] `readline` for line-delimited protocols
- [ ] Graceful shutdown: stop accepting, drain, then close

---

## Cross-References

- [02-directory-streaming-advanced.md](../04-stream-filesystem-integration/02-directory-streaming-advanced.md) — File system streams
- [03-file-format-streaming.md](../04-stream-filesystem-integration/03-file-format-streaming.md) — NDJSON and format parsing
- [03-http2-grpc-streaming.md](./03-http2-grpc-streaming.md) — HTTP/2 and gRPC

---

## Next Steps

- Explore [03-http2-grpc-streaming.md](./03-http2-grpc-streaming.md) for HTTP/2 and gRPC
- Implement TLS wrapping with `tls.createServer()` and `tls.connect()`
- Build a Redis command pipeline over one connection
- Benchmark connection pooling under concurrent load
