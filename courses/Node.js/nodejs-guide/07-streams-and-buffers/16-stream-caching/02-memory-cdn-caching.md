# Memory and CDN Stream Caching

## What You'll Learn

- Building an in-memory LRU cache integrated with Node.js streams
- Creating a cache-layer Transform stream that intercepts and serves cached data
- Populating a CDN from a stream pipeline
- ETag and Last-Modified validation using streaming responses
- Partial content caching with byte-range Readable streams
- Propagating cache invalidation events through a stream bus
- Real-world: CDN-backed static file serving with stream caching

---

## 1. In-Memory LRU Cache with Stream Integration

A simple LRU cache that stores stream results and replays them on subsequent reads.

```js
// lru-stream-cache.mjs
import { Readable, Transform } from 'node:stream';

class LRUCache {
  #map = new Map();
  #maxSize;

  constructor(maxSize = 100) {
    this.#maxSize = maxSize;
  }

  get(key) {
    if (!this.#map.has(key)) return undefined;
    const value = this.#map.get(key);
    // Move to end (most recently used)
    this.#map.delete(key);
    this.#map.set(key, value);
    return value;
  }

  set(key, value) {
    if (this.#map.has(key)) this.#map.delete(key);
    this.#map.set(key, value);
    if (this.#map.size > this.#maxSize) {
      const oldest = this.#map.keys().next().value;
      this.#map.delete(oldest);
    }
  }

  has(key) {
    return this.#map.has(key);
  }

  delete(key) {
    return this.#map.delete(key);
  }

  get size() {
    return this.#map.size;
  }
}

// Transform that buffers chunks and caches the full result
class LRUStreamCache extends Transform {
  #cache;
  #cacheKey;

  constructor(cache, cacheKey) {
    super({ objectMode: true });
    this.#cache = cache;
    this.#cacheKey = cacheKey;
    this.#chunks = [];
  }

  _transform(chunk, encoding, callback) {
    this.#chunks.push(chunk);
    this.push(chunk);
    callback();
  }

  _flush(callback) {
    this.#cache.set(this.#cacheKey, [...this.#chunks]);
    callback();
  }
}

// Usage: cache-on-first-read, serve-from-cache on second read
const lru = new LRUCache(50);

async function getData(cacheKey) {
  if (lru.has(cacheKey)) {
    console.log(`[LRU] Cache HIT for ${cacheKey}`);
    return Readable.from(lru.get(cacheKey));
  }

  console.log(`[LRU] Cache MISS for ${cacheKey}`);

  // Simulate a slow data source
  const source = Readable.from([
    { id: 1, data: 'row-1' },
    { id: 2, data: 'row-2' },
    { id: 3, data: 'row-3' },
  ]);

  // Create a passthrough that also caches
  const chunks = [];
  const cacheTransform = new Transform({
    objectMode: true,
    transform(chunk, enc, cb) {
      chunks.push(chunk);
      this.push(chunk);
      cb();
    },
    flush(cb) {
      lru.set(cacheKey, chunks);
      cb();
    },
  });

  source.pipe(cacheTransform);
  return cacheTransform;
}

// First call: cache miss
const stream1 = await getData('users:list');
stream1.on('data', (d) => console.log('  ->', d));
await new Promise((r) => stream1.on('end', r));

// Second call: cache hit
const stream2 = await getData('users:list');
stream2.on('data', (d) => console.log('  ->', d));
await new Promise((r) => stream2.on('end', r));

console.log(`LRU cache size: ${lru.size}`);
```

---

## 2. Cache-Layer Transform Stream

A generic Transform stream that wraps any data source and serves from a pluggable cache backend.

```js
// cache-layer-transform.mjs
import { Transform } from 'node:stream';

class CacheLayer extends Transform {
  #backend;
  #keyFn;
  #ttl;

  constructor({ backend, keyFn = (chunk) => chunk.id, ttl = 300 } = {}) {
    super({ objectMode: true });
    this.#backend = backend;
    this.#keyFn = keyFn;
    this.#ttl = ttl;
  }

  async _transform(chunk, encoding, callback) {
    try {
      const key = this.#keyFn(chunk);

      // Check cache
      const cached = await this.#backend.get(key);
      if (cached) {
        this.push({ ...cached, _source: 'cache' });
        return callback();
      }

      // Cache miss: pass through and store
      await this.#backend.set(key, chunk, this.#ttl);
      this.push({ ...chunk, _source: 'origin' });
      callback();
    } catch (err) {
      callback(err);
    }
  }
}

// Simple in-memory backend adapter
const memoryBackend = {
  store: new Map(),
  async get(key) {
    const entry = this.store.get(key);
    if (!entry) return null;
    if (entry.expiresAt < Date.now()) {
      this.store.delete(key);
      return null;
    }
    return entry.value;
  },
  async set(key, value, ttl) {
    this.store.set(key, { value, expiresAt: Date.now() + ttl * 1000 });
  },
};

// Usage
import { Readable } from 'node:stream';

const source = Readable.from([
  { id: 'a', payload: 'first' },
  { id: 'b', payload: 'second' },
  { id: 'a', payload: 'first-duplicate' },
]);

const layer = new CacheLayer({
  backend: memoryBackend,
  keyFn: (c) => `item:${c.id}`,
  ttl: 60,
});

source.pipe(layer).on('data', (row) => {
  console.log(`${row._source} -> ${row.id}: ${row.payload}`);
});
```

---

## 3. CDN Cache Population via Streams

Stream local files to a CDN-like endpoint using multipart upload simulation.

```js
// cdn-stream-upload.mjs
import { createReadStream } from 'node:fs';
import { Transform, pipeline } from 'node:stream';
import { promisify } from 'node:util';

const pipelineAsync = promisify(pipeline);

class CdnUploadStream extends Transform {
  #cdnEndpoint;
  #uploadId;
  #partNumber = 0;
  #parts = [];

  constructor(cdnEndpoint) {
    super({ highWaterMark: 64 * 1024 }); // 64KB chunks
    this.#cdnEndpoint = cdnEndpoint;
    this.#uploadId = `upload-${Date.now()}`;
  }

  async _transform(chunk, encoding, callback) {
    try {
      this.#partNumber++;
      // Simulate CDN multipart upload
      const etag = `"part-${this.#partNumber}-${Date.now()}"`;
      this.#parts.push({ PartNumber: this.#partNumber, ETag: etag, Size: chunk.length });

      console.log(
        `[CDN] Uploaded part ${this.#partNumber} (${chunk.length} bytes)`
      );

      this.push(chunk); // Pass through for local verification
      callback();
    } catch (err) {
      callback(err);
    }
  }

  async _flush(callback) {
    // Simulate completing multipart upload
    console.log(
      `[CDN] Complete upload ${this.#uploadId}: ${this.#parts.length} parts, ` +
        `${this.#parts.reduce((s, p) => s + p.Size, 0)} total bytes`
    );
    callback();
  }
}

// Usage: stream a local file to CDN
const fileStream = createReadStream('./large-file.bin'); // replace with real file
const cdnStream = new CdnUploadStream('https://cdn.example.com');

// In production you would pipe to an HTTP request:
// fileStream.pipe(cdnStream).pipe(http.request(cdnEndpoint))

console.log('CDN stream population example ready');
// await pipelineAsync(fileStream, cdnStream);
```

---

## 4. ETag / Last-Modified Streaming Cache Validation

Generate ETags from streamed content and support conditional responses.

```js
// etag-stream-validation.mjs
import { createHash } from 'node:crypto';
import { Transform, Readable } from 'node:stream';

class ETagStream extends Transform {
  #hash = createHash('md5');
  #size = 0;

  _transform(chunk, encoding, callback) {
    this.#hash.update(chunk);
    this.#size += chunk.length;
    this.push(chunk);
    callback();
  }

  _flush(callback) {
    this.etag = `"${this.#hash.digest('hex')}"`;
    this.size = this.#size;
    callback();
  }
}

// HTTP server with conditional caching
import { createServer } from 'node:http';

const lastModified = new Date().toUTCString();

createServer((req, res) => {
  const ifNoneMatch = req.headers['if-none-match'];
  const ifModifiedSince = req.headers['if-modified-since'];

  const dataSource = Readable.from([
    JSON.stringify({ items: Array.from({ length: 100 }, (_, i) => i) }),
  ]);

  const etagStream = new ETagStream();

  // Collect body while computing ETag
  const chunks = [];
  etagStream.on('data', (chunk) => chunks.push(chunk));
  etagStream.on('end', () => {
    const etag = etagStream.etag;

    // Conditional GET: 304 Not Modified
    if (ifNoneMatch === etag) {
      res.writeHead(304, { ETag: etag, 'Last-Modified': lastModified });
      res.end();
      return;
    }

    res.writeHead(200, {
      'Content-Type': 'application/json',
      ETag: etag,
      'Last-Modified': lastModified,
      'Cache-Control': 'public, max-age=300',
    });
    res.end(Buffer.concat(chunks));
  });

  dataSource.pipe(etagStream);
}).listen(3001, () => console.log('ETag server on :3001'));
```

---

## 5. Partial Content Caching with Byte-Range Streams

Serve partial content from a cached buffer using byte-range requests.

```js
// byte-range-cache.mjs
import { Readable } from 'node:stream';

class ByteRangeStream extends Readable {
  #buffer;
  #start;
  #end;
  #pos;

  constructor(buffer, start, end) {
    super({ highWaterMark: 16 * 1024 });
    this.#buffer = buffer;
    this.#start = start;
    this.#end = Math.min(end, buffer.length - 1);
    this.#pos = start;
  }

  _read(size) {
    if (this.#pos > this.#end) {
      this.push(null);
      return;
    }

    const end = Math.min(this.#pos + size - 1, this.#end);
    const chunk = this.#buffer.subarray(this.#pos, end + 1);
    this.#pos = end + 1;
    this.push(chunk);
  }
}

// Simulate a cached full response in memory
const fullContent = Buffer.from(
  'A'.repeat(1000) + 'B'.repeat(1000) + 'C'.repeat(1000)
);

// Serve bytes 500-1499 (range request)
const rangeStart = 500;
const rangeEnd = 1499;

const rangeStream = new ByteRangeStream(fullContent, rangeStart, rangeEnd);

const chunks = [];
rangeStream.on('data', (chunk) => chunks.push(chunk));
rangeStream.on('end', () => {
  const result = Buffer.concat(chunks);
  console.log(
    `Served ${result.length} bytes (range ${rangeStart}-${rangeEnd})`
  );
  console.log(
    `Content starts with: ${result.subarray(0, 20).toString()}`
  );
});
```

---

## 6. Cache Invalidation Propagation via Streams

Broadcast invalidation events to all cache nodes using a readable stream bus.

```js
// invalidation-bus.mjs
import { PassThrough, Transform } from 'node:stream';

class InvalidationBus extends PassThrough {
  constructor() {
    super({ objectMode: true });
  }

  invalidate(key) {
    this.write({ type: 'invalidate', key, timestamp: Date.now() });
  }

  purge(prefix) {
    this.write({ type: 'purge', prefix, timestamp: Date.now() });
  }
}

// Cache node that subscribes to invalidation events
class CacheNode extends Transform {
  #name;
  #cache = new Map();

  constructor(name) {
    super({ objectMode: true });
    this.#name = name;
  }

  _transform(event, encoding, callback) {
    if (event.type === 'invalidate') {
      this.#cache.delete(event.key);
      console.log(`[${this.#name}] Invalidated key: ${event.key}`);
    } else if (event.type === 'purge') {
      let count = 0;
      for (const key of this.#cache.keys()) {
        if (key.startsWith(event.prefix)) {
          this.#cache.delete(key);
          count++;
        }
      }
      console.log(`[${this.#name}] Purged ${count} keys with prefix: ${event.prefix}`);
    }
    this.push(event);
    callback();
  }

  set(key, value) {
    this.#cache.set(key, value);
  }

  get size() {
    return this.#cache.size;
  }
}

// Usage
const bus = new InvalidationBus();

const node1 = new CacheNode('Node-A');
const node2 = new CacheNode('Node-B');

// Fan-out: bus -> node1, bus -> node2
bus.pipe(node1);
bus.pipe(node2);

// Populate caches
node1.set('user:1', { name: 'Alice' });
node1.set('user:2', { name: 'Bob' });
node2.set('user:1', { name: 'Alice' });
node2.set('product:x', { price: 10 });

console.log('Before invalidation:', node1.size, node2.size);

// Invalidate across all nodes
bus.invalidate('user:1');
bus.purge('user:');

// Drain events
node1.resume();
node2.resume();

setTimeout(() => {
  console.log('After invalidation:', node1.size, node2.size);
}, 100);
```

---

## 7. Real-World: CDN-Backed Static File Serving

An HTTP server that streams static files with in-memory caching and CDN header support.

```js
// static-cdn-server.mjs
import { createServer } from 'node:http';
import { createReadStream, statSync } from 'node:fs';
import { join, extname } from 'node:path';
import { createHash } from 'node:crypto';
import { Transform } from 'node:stream';

const STATIC_DIR = './public';
const CACHE = new Map(); // key -> { body, etag, lastModified, mimeType }
const MAX_CACHE_SIZE = 50 * 1024 * 1024; // 50 MB
let currentCacheSize = 0;

const MIME_TYPES = {
  '.html': 'text/html',
  '.css': 'text/css',
  '.js': 'application/javascript',
  '.json': 'application/json',
  '.png': 'image/png',
  '.jpg': 'image/jpeg',
  '.svg': 'image/svg+xml',
};

function getMimeType(ext) {
  return MIME_TYPES[ext] || 'application/octet-stream';
}

class ETagCalculator extends Transform {
  #hash = createHash('sha256');
  #chunks = [];

  _transform(chunk, encoding, cb) {
    this.#hash.update(chunk);
    this.#chunks.push(chunk);
    this.push(chunk);
    cb();
  }

  _flush(cb) {
    this.etag = `"${this.#hash.digest('hex').slice(0, 32)}"`;
    this.body = Buffer.concat(this.#chunks);
    cb();
  }
}

const server = createServer(async (req, res) => {
  const filePath = join(STATIC_DIR, req.url);
  const ext = extname(filePath);
  const mimeType = getMimeType(ext);

  // Check in-memory cache
  const cached = CACHE.get(req.url);
  if (cached) {
    if (req.headers['if-none-match'] === cached.etag) {
      res.writeHead(304, {
        ETag: cached.etag,
        'Cache-Control': 'public, max-age=86400',
        'X-Cache': 'HIT-304',
      });
      return res.end();
    }

    res.writeHead(200, {
      'Content-Type': mimeType,
      'Content-Length': cached.body.length,
      ETag: cached.etag,
      'Last-Modified': cached.lastModified,
      'Cache-Control': 'public, max-age=86400',
      'X-Cache': 'HIT',
    });
    return res.end(cached.body);
  }

  // Stream file from disk, compute ETag, cache result
  let stat;
  try {
    stat = statSync(filePath);
  } catch {
    res.writeHead(404);
    return res.end('Not Found');
  }

  const etagCalc = new ETagCalculator();
  const fileStream = createReadStream(filePath);

  res.writeHead(200, {
    'Content-Type': mimeType,
    'Content-Length': stat.size,
    'Last-Modified': stat.mtime.toUTCString(),
    'Cache-Control': 'public, max-age=86400',
    'X-Cache': 'MISS',
  });

  etagCalc.on('flush', () => {
    // Store in cache if within budget
    if (currentCacheSize + stat.size <= MAX_CACHE_SIZE) {
      CACHE.set(req.url, {
        body: etagCalc.body,
        etag: etagCalc.etag,
        lastModified: stat.mtime.toUTCString(),
      });
      currentCacheSize += stat.size;
    }
  });

  fileStream.pipe(etagCalc).pipe(res);
});

server.listen(3002, () => console.log('Static CDN server on :3002'));
```

---

## Best Practices Checklist

- [ ] Set a maximum cache size (bytes or item count) to prevent unbounded memory growth
- [ ] Use LRU eviction policy — evict least-recently-used entries when the cache is full
- [ ] Compute ETags via streaming hash (`crypto.createHash`) to avoid buffering entire responses
- [ ] Support `304 Not Modified` responses to save bandwidth for conditional requests
- [ ] Use `Cache-Control` headers with appropriate `max-age` for CDN and browser caching
- [ ] Implement byte-range support (`Range` header) for large cached assets
- [ ] Propagate cache invalidation events via a stream bus so all nodes stay consistent
- [ ] Separate cache layers (L1 in-memory, L2 Redis/CDN) and define fallback order
- [ ] Monitor cache hit/miss rates via Transform stream counters
- [ ] Avoid caching user-specific (authenticated) content in shared CDN caches
- [ ] Use `pipeline()` from `node:stream` for robust error handling across cache layers

---

## Cross-References

- [01-redis-stream-caching.md](./01-redis-stream-caching.md) — Redis-based stream caching (L2 cache)
- [03-stream-processing-patterns](../03-stream-processing-patterns/) — Transform and pipeline patterns
- [08-stream-performance-optimization](../08-stream-performance-optimization/) — backpressure and buffering
- [09-stream-security](../09-stream-security/) — securing cached content and headers

---

## Next Steps

- **03-distributed-cache-streams.md** — synchronize caches across multiple nodes with streams
- Explore `lru-cache` npm package for production-grade LRU implementation
- Investigate CloudFront, Fastly, or Varnish APIs for programmatic CDN cache purging
- Benchmark stream-based caching vs. buffer-based approaches for your workload
