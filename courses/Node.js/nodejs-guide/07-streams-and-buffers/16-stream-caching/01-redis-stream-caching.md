# Redis Stream Caching Patterns

## What You'll Learn

- Write-through and write-behind caching with Node.js streams
- Streaming cache population from a database source into Redis
- Cache-aside (lazy-loading) pattern using readable and writable streams
- Redis Stream data type (`XADD`/`XREAD`) for real-time cached data
- TTL-based cache invalidation driven by stream events
- Cache warming on application startup via pipeline streams
- Streaming API response cache backed by Redis

---

## 1. Project Setup

```bash
npm init -y
npm install redis
```

```js
// redis-client.mjs
import { createClient } from 'redis';

const client = createClient({ url: 'redis://localhost:6379' });
client.on('error', (err) => console.error('Redis Client Error', err));
await client.connect();

export default client;
```

---

## 2. Write-Through Cache with Streams

Every write persists to both the database and Redis synchronously.

```js
// write-through-stream.mjs
import { Writable, Readable } from 'node:stream';
import client from './redis-client.mjs';

class WriteThroughCache extends Writable {
  #dbWriter;
  #ttl;

  constructor(dbWriter, ttl = 3600) {
    super({ objectMode: true });
    this.#dbWriter = dbWriter;
    this.#ttl = ttl;
  }

  async _write(chunk, encoding, callback) {
    try {
      await this.#dbWriter.write(chunk);
      await client.set(`entity:${chunk.id}`, JSON.stringify(chunk), { EX: this.#ttl });
      callback();
    } catch (err) {
      callback(err);
    }
  }
}

const dataStream = Readable.from([
  { id: 1, name: 'Alice', score: 95 },
  { id: 2, name: 'Bob', score: 87 },
  { id: 3, name: 'Charlie', score: 92 },
]);

const dbSink = new Writable({
  objectMode: true,
  write(chunk, enc, cb) { console.log(`[DB] Persisted ${chunk.id}`); cb(); },
});

dataStream
  .pipe(new WriteThroughCache(dbSink, 600))
  .on('finish', async () => {
    console.log('Cached entity:2 ->', await client.get('entity:2'));
    await client.quit();
  });
```

---

## 3. Write-Behind (Write-Back) Cache

Writes go to Redis immediately; a background batch flush pushes to the database periodically.

```js
// write-behind-stream.mjs
import { Writable, Readable } from 'node:stream';
import client from './redis-client.mjs';

class WriteBehindCache extends Writable {
  #buffer = [];
  #flushInterval;
  #batchSize;
  #ttl;

  constructor({ batchSize = 50, flushMs = 5000, ttl = 3600 } = {}) {
    super({ objectMode: true });
    this.#batchSize = batchSize;
    this.#ttl = ttl;
    this.#flushInterval = setInterval(() => this.#flush(), flushMs);
  }

  async _write(chunk, encoding, callback) {
    try {
      await client.set(`entity:${chunk.id}`, JSON.stringify(chunk), { EX: this.#ttl });
      this.#buffer.push(chunk);
      if (this.#buffer.length >= this.#batchSize) await this.#flush();
      callback();
    } catch (err) { callback(err); }
  }

  async _final(callback) {
    clearInterval(this.#flushInterval);
    await this.#flush();
    callback();
  }

  async #flush() {
    if (!this.#buffer.length) return;
    const batch = this.#buffer.splice(0);
    console.log(`[Write-Behind] Flushing ${batch.length} records to DB`);
  }
}

Readable.from(Array.from({ length: 120 }, (_, i) => ({ id: i + 1, value: `r${i}` })))
  .pipe(new WriteBehindCache({ batchSize: 30, flushMs: 2000 }))
  .on('finish', () => console.log('All records buffered; DB flush runs in background'));
```

---

## 4. Cache-Aside (Lazy Loading) with Streams

Check Redis first; on miss, fetch from source and populate the cache.

```js
// cache-aside-stream.mjs
import { Readable } from 'node:stream';
import client from './redis-client.mjs';

class CacheAsideReader extends Readable {
  #keys;
  #index = 0;
  #ttl;

  constructor(keys, ttl = 3600) {
    super({ objectMode: true });
    this.#keys = keys;
    this.#ttl = ttl;
  }

  async _read() {
    if (this.#index >= this.#keys.length) { this.push(null); return; }

    const key = this.#keys[this.#index++];
    const cacheKey = `entity:${key}`;

    try {
      const cached = await client.get(cacheKey);
      if (cached) { this.push({ ...JSON.parse(cached), _src: 'cache' }); return; }

      const record = await this.#fetchFromDB(key);
      await client.set(cacheKey, JSON.stringify(record), { EX: this.#ttl });
      this.push({ ...record, _src: 'db' });
    } catch (err) { this.destroy(err); }
  }

  async #fetchFromDB(id) {
    return new Promise((r) => setTimeout(() => r({ id, name: `User-${id}` }), 50));
  }
}

new CacheAsideReader([1, 2, 3, 1, 2], 120)
  .on('data', (row) => console.log(`ID ${row.id} -> ${row._src}`))
  .on('end', async () => { console.log('Done'); await client.quit(); });
```

---

## 5. Redis Streams for Real-Time Cached Data

Use the Redis Stream data type to cache a real-time event feed.

```js
// redis-stream-cache.mjs
import { Readable } from 'node:stream';
import client from './redis-client.mjs';

const STREAM_KEY = 'events:realtime';

async function produceEvents() {
  for (let i = 0; i < 10; i++) {
    await client.xAdd(STREAM_KEY, '*', {
      type: 'metric',
      payload: JSON.stringify({ cpu: Math.random() * 100 }),
    });
    await new Promise((r) => setTimeout(r, 100));
  }
}

class RedisStreamConsumer extends Readable {
  #lastId = '0';

  async _read() {
    const results = await client.xRead(
      { key: STREAM_KEY, id: this.#lastId },
      { COUNT: 10, BLOCK: 1000 }
    );

    if (!results?.length) { this.push(null); return; }

    for (const stream of results) {
      for (const msg of stream.messages) {
        this.#lastId = msg.id;
        this.push({ id: msg.id, ...msg.message });
      }
    }
  }
}

await produceEvents();
new RedisStreamConsumer(STREAM_KEY)
  .on('data', (msg) => console.log(`Stream ${msg.id}:`, msg.payload))
  .on('end', async () => { await client.del(STREAM_KEY); await client.quit(); });
```

---

## 6. TTL-Based Cache Invalidation with Streams

Tag each chunk with a TTL; Redis auto-evicts expired keys. Subscribe to keyspace notifications to observe eviction.

```js
// ttl-invalidation.mjs
import { Transform, pipeline } from 'node:stream';
import { promisify } from 'node:util';
import client from './redis-client.mjs';

const pipelineAsync = promisify(pipeline);

class TTLCacheWriter extends Transform {
  #ttl;
  constructor(ttl = 60) { super({ objectMode: true }); this.#ttl = ttl; }

  async _transform(chunk, encoding, callback) {
    try {
      const key = `ttl:${chunk.id}`;
      await client.set(key, JSON.stringify(chunk), { EX: this.#ttl });
      this.push({ ...chunk, _cachedKey: key });
      callback();
    } catch (err) { callback(err); }
  }
}

// Monitor expirations
const sub = client.duplicate();
await sub.connect();
await sub.configSet('notify-keyspace-events', 'Ex');
await sub.subscribe('__keyevent@0__:expired', (key) => console.log(`[Evicted] ${key}`));

await pipelineAsync(
  Readable.from([{ id: 'a' }, { id: 'b' }]),
  new TTLCacheWriter(5)
);
console.log('Entries cached. Waiting for TTL...');

setTimeout(async () => { await sub.disconnect(); await client.quit(); }, 8000);
```

---

## 7. Cache Warming on Startup

Populate Redis from a database snapshot using a batched pipeline.

```js
// cache-warming.mjs
import { Readable, Writable, pipeline } from 'node:stream';
import { promisify } from 'node:util';
import client from './redis-client.mjs';

const pipelineAsync = promisify(pipeline);

class BatchCacheWriter extends Writable {
  #batch = [];
  #batchSize;
  #ttl;

  constructor({ batchSize = 100, ttl = 3600 } = {}) {
    super({ objectMode: true });
    this.#batchSize = batchSize;
    this.#ttl = ttl;
  }

  async _write(chunk, enc, cb) {
    this.#batch.push(chunk);
    if (this.#batch.length >= this.#batchSize) await this.#flush();
    cb();
  }

  async _final(cb) { await this.#flush(); cb(); }

  async #flush() {
    if (!this.#batch.length) return;
    const pipe = client.multi();
    for (const item of this.#batch) {
      pipe.set(`warm:${item.id}`, JSON.stringify(item), { EX: this.#ttl });
    }
    await pipe.exec();
    console.log(`[Warm] Cached ${this.#batch.length} items`);
    this.#batch = [];
  }
}

const start = Date.now();
const source = Readable.from(
  Array.from({ length: 5000 }, (_, i) => ({ id: i + 1, data: `payload-${i}` }))
);

await pipelineAsync(source, new BatchCacheWriter({ batchSize: 200, ttl: 7200 }));
console.log(`Cache warming complete in ${Date.now() - start}ms`);
await client.quit();
```

---

## 8. Real-World: Streaming API Response Cache

Cache HTTP API responses chunk-by-chunk in Redis while streaming to the client.

```js
// api-response-cache.mjs
import { Transform } from 'node:stream';
import { Readable } from 'node:stream';
import { createServer } from 'node:http';
import client from './redis-client.mjs';

class ApiResponseCache extends Transform {
  #cacheKey;
  #chunks = [];
  #ttl;

  constructor(cacheKey, ttl = 300) {
    super();
    this.#cacheKey = cacheKey;
    this.#ttl = ttl;
  }

  _transform(chunk, enc, cb) { this.#chunks.push(chunk); this.push(chunk); cb(); }

  async _flush(cb) {
    const body = Buffer.concat(this.#chunks).toString();
    await client.set(this.#cacheKey, JSON.stringify({ body, cachedAt: Date.now() }), { EX: this.#ttl });
    console.log(`[API Cache] Stored ${this.#cacheKey} (${body.length} bytes)`);
    cb();
  }
}

createServer(async (req, res) => {
  const cached = await client.get(`api:${req.url}`);
  if (cached) {
    res.writeHead(200, { 'Content-Type': 'application/json', 'X-Cache': 'HIT' });
    return res.end(JSON.parse(cached).body);
  }

  res.writeHead(200, { 'Content-Type': 'application/json', 'X-Cache': 'MISS' });
  Readable.from([JSON.stringify({ data: 'upstream', ts: Date.now() })])
    .pipe(new ApiResponseCache(`api:${req.url}`, 300))
    .pipe(res);
}).listen(3000, () => console.log('API cache server on :3000'));
```

---

## Best Practices Checklist

- [ ] Choose write-through for strong consistency; write-behind for high throughput
- [ ] Set appropriate TTLs — too short causes excessive DB hits; too long serves stale data
- [ ] Use Redis `MULTI`/`EXEC` (pipelines) for batch-writing cache entries
- [ ] Monitor Redis memory; set `maxmemory-policy` to `allkeys-lru` or `volatile-lru`
- [ ] Handle Redis connection failures with retry and circuit-breaker logic
- [ ] Use `XACK` when consuming Redis Streams to prevent reprocessing
- [ ] Avoid caching objects >1 MB — compress or split into smaller entries
- [ ] Use `pipeline()` from `node:stream` for error propagation
- [ ] Namespace cache keys (`entity:`, `warm:`, `api:`) for clarity and eviction control
- [ ] Test cache behavior under concurrent writes and connection drops

---

## Cross-References

- [01-streams-architecture](../01-streams-architecture/) — foundational stream concepts
- [03-stream-processing-patterns](../03-stream-processing-patterns/) — Transform and pipeline patterns
- [14-stream-databases-storage](../14-stream-databases-storage/) — streaming from databases
- [15-stream-message-queues](../15-stream-message-queues/) — integrating streams with message brokers

---

## Next Steps

- **02-memory-cdn-caching.md** — extend caching to in-memory LRU and CDN layers
- **03-distributed-cache-streams.md** — synchronize caches across multiple nodes with streams
- Explore Redis Cluster for horizontally-scaled cache partitions
- Investigate RedisGears for server-side stream processing
