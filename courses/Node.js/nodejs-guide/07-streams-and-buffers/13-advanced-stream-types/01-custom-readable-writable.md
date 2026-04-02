# Custom Readable and Writable Stream Creation

## What You'll Learn

- Building custom Readable streams from async data sources
- Database cursor reader, API pagination reader, log file tail reader
- Custom Writable streams with batching, rate-limiting, and retry logic
- `_construct()`, `_destroy()`, `_final()` lifecycle hooks
- Signal-based destruction with `AbortController`
- `objectMode` for non-buffer data

## Custom Readable Streams

### Database Cursor Reader

Fetches documents from MongoDB in batches using a cursor, applying backpressure naturally.

```javascript
import { Readable } from 'node:stream';
import { MongoClient } from 'mongodb';

class MongoCursorReader extends Readable {
  constructor(uri, dbName, collectionName, query = {}, options = {}) {
    super({ objectMode: true, highWaterMark: options.batchSize || 100 });
    this.uri = uri;
    this.dbName = dbName;
    this.collectionName = collectionName;
    this.query = query;
    this.batchSize = options.batchSize || 100;
    this.client = null;
    this.cursor = null;
  }

  async _construct(callback) {
    try {
      this.client = new MongoClient(this.uri);
      await this.client.connect();
      const collection = this.client.db(this.dbName).collection(this.collectionName);
      this.cursor = collection.find(this.query).batchSize(this.batchSize);
      callback();
    } catch (err) { callback(err); }
  }

  async _read() {
    if (!this.cursor) return this.push(null);
    try {
      const doc = await this.cursor.next();
      this.push(doc || null);
    } catch (err) { this.destroy(err); }
  }

  async _destroy(err, callback) {
    try {
      if (this.cursor) await this.cursor.close();
      if (this.client) await this.client.close();
    } catch (closeErr) { err = err || closeErr; }
    callback(err);
  }
}

const reader = new MongoCursorReader(
  'mongodb://localhost:27017', 'analytics', 'events',
  { status: 'pending' }, { batchSize: 200 }
);
reader.on('data', (doc) => console.log(doc._id));
reader.on('end', () => console.log('Cursor exhausted'));
```

### API Pagination Reader

Reads all pages of a paginated REST API as a continuous stream of objects.

```javascript
import { Readable } from 'node:stream';

class ApiPaginationReader extends Readable {
  constructor(baseUrl, options = {}) {
    super({ objectMode: true });
    this.baseUrl = baseUrl;
    this.page = options.startPage || 1;
    this.pageSize = options.pageSize || 100;
    this.headers = options.headers || {};
    this.hasMore = true;
  }

  async _read() {
    if (!this.hasMore) return this.push(null);
    try {
      const res = await fetch(
        `${this.baseUrl}?page=${this.page}&per_page=${this.pageSize}`,
        { headers: this.headers }
      );
      if (!res.ok) throw new Error(`API error: ${res.status}`);

      const data = await res.json();
      const items = data.items || data.data || data.results || [];
      if (items.length === 0) { this.push(null); return; }
      if (items.length < this.pageSize) this.hasMore = false;
      this.page++;

      for (const item of items) {
        if (!this.push(item)) return; // Backpressure
      }
    } catch (err) { this.destroy(err); }
  }
}

const users = new ApiPaginationReader('https://api.example.com/users', {
  headers: { Authorization: 'Bearer token123' }, pageSize: 50,
});
for await (const user of users) { console.log(user.name); }
```

### Log File Tail Reader

Continuously reads new lines appended to a file, similar to `tail -f`.

```javascript
import { Readable } from 'node:stream';
import { watch, open } from 'node:fs/promises';

class LogTailReader extends Readable {
  constructor(filePath) {
    super({ encoding: 'utf-8' });
    this.filePath = filePath;
    this.position = 0;
    this.fileHandle = null;
    this.watcher = null;
  }

  async _construct(callback) {
    try {
      this.fileHandle = await open(this.filePath, 'r');
      this.position = (await this.fileHandle.stat()).size;
      callback();
    } catch (err) { callback(err); }
  }

  async _read() {
    try {
      const stat = await this.fileHandle.stat();
      if (stat.size <= this.position) {
        this.watcher = watch(this.filePath);
        for await (const event of this.watcher) {
          if (event.eventType === 'change') break;
        }
      }

      const { bytesRead, buffer } = await this.fileHandle.read(
        Buffer.alloc(65536), 0, 65536, this.position
      );
      if (bytesRead > 0) {
        this.position += bytesRead;
        for (const line of buffer.toString('utf-8', 0, bytesRead).split('\n').filter(Boolean)) {
          if (!this.push(line)) return;
        }
      }
    } catch (err) { this.destroy(err); }
  }

  async _destroy(err, callback) {
    if (this.watcher) this.watcher.close();
    if (this.fileHandle) await this.fileHandle.close();
    callback(err);
  }
}
```

## Custom Writable Streams

### Batch Database Writer

Collects records into batches and writes them in bulk to reduce round-trips.

```javascript
import { Writable } from 'node:stream';

class BatchDbWriter extends Writable {
  constructor(executeBatch, options = {}) {
    super({ objectMode: true, highWaterMark: options.batchSize || 500 });
    this.batchSize = options.batchSize || 500;
    this.executeBatch = executeBatch;
    this.buffer = [];
    this.timer = null;
  }

  _construct(callback) {
    this.timer = setInterval(() => this.flushBatch(), options.flushInterval || 2000);
    callback();
  }

  async _write(chunk, encoding, callback) {
    this.buffer.push(chunk);
    if (this.buffer.length >= this.batchSize) await this.flushBatch();
    callback();
  }

  async _final(callback) {
    clearInterval(this.timer);
    await this.flushBatch();
    callback();
  }

  async flushBatch() {
    if (this.buffer.length === 0) return;
    const batch = this.buffer.splice(0);
    await this.executeBatch(batch);
    this.emit('batch-written', { count: batch.length });
  }
}

const writer = new BatchDbWriter(
  (records) => db.collection('events').insertMany(records),
  { batchSize: 1000, flushInterval: 5000 }
);
sourceStream.pipe(writer);
```

### Rate-Limited Writer

Throttles write throughput to avoid overwhelming downstream services.

```javascript
import { Writable } from 'node:stream';

class RateLimitedWriter extends Writable {
  constructor(writeFn, options = {}) {
    super({ objectMode: true });
    this.writeFn = writeFn;
    this.maxPerSecond = options.maxPerSecond || 100;
    this.tokens = this.maxPerSecond;
    this.interval = null;
  }

  _construct(callback) {
    this.interval = setInterval(() => {
      this.tokens = this.maxPerSecond;
      this.emit('refill');
    }, 1000);
    callback();
  }

  async _write(chunk, encoding, callback) {
    if (this.tokens <= 0) {
      await new Promise((resolve) => this.once('refill', resolve));
    }
    this.tokens--;
    try { await this.writeFn(chunk); callback(); }
    catch (err) { callback(err); }
  }

  _destroy(err, callback) { clearInterval(this.interval); callback(err); }
}

const apiWriter = new RateLimitedWriter(
  (record) => fetch('https://api.example.com/events', {
    method: 'POST', body: JSON.stringify(record),
  }),
  { maxPerSecond: 50 }
);
```

### Retry-Enabled Writer

Automatically retries failed writes with exponential backoff.

```javascript
import { Writable } from 'node:stream';

class RetryWriter extends Writable {
  constructor(writeFn, options = {}) {
    super({ objectMode: true });
    this.writeFn = writeFn;
    this.maxRetries = options.maxRetries || 5;
    this.baseDelay = options.baseDelay || 100;
    this.deadLetterQueue = [];
  }

  async _write(chunk, encoding, callback) {
    let lastError;
    for (let attempt = 0; attempt <= this.maxRetries; attempt++) {
      try { await this.writeFn(chunk); return callback(); }
      catch (err) {
        lastError = err;
        if (attempt < this.maxRetries) {
          await new Promise((r) => setTimeout(r, this.baseDelay * 2 ** attempt));
        }
      }
    }
    this.deadLetterQueue.push({ record: chunk, error: lastError.message });
    this.emit('dead-letter', { record: chunk, error: lastError });
    callback();
  }
}
```

## Signal-Based Destruction with AbortController

```javascript
import { Readable } from 'node:stream';

class PollingReader extends Readable {
  constructor(fetchFn, intervalMs, signal) {
    super({ objectMode: true });
    this.fetchFn = fetchFn;
    this.intervalMs = intervalMs;
    this.timer = null;

    if (signal) {
      signal.addEventListener('abort', () => {
        this.destroy(new Error('Aborted'));
      }, { once: true });
    }
  }

  async _read() {
    if (this.timer) return;
    const poll = async () => {
      try {
        this.push(await this.fetchFn());
        this.timer = setTimeout(poll, this.intervalMs);
      } catch (err) { this.destroy(err); }
    };
    await poll();
  }

  _destroy(err, callback) { clearTimeout(this.timer); callback(err); }
}

const controller = new AbortController();
const reader = new PollingReader(
  () => fetch('/api/metrics').then((r) => r.json()), 1000, controller.signal
);
setTimeout(() => controller.abort(), 30000); // Cancel after 30s
```

## objectMode vs Buffer Mode

```javascript
import { Readable, Writable } from 'node:stream';

// Buffer mode (default): chunks must be strings or Buffers
const bufferReader = Readable.from(['hello', ' ', 'world']);

// objectMode: chunks can be any JavaScript value
const objectReader = new Readable({
  objectMode: true,
  read() { this.push({ id: 1, data: 'any value' }); this.push(null); },
});

const objectWriter = new Writable({
  objectMode: true,
  write(chunk, encoding, callback) {
    console.log(chunk.id, chunk.data);
    callback();
  },
});
```

## Best Practices Checklist

- [ ] Always implement `_construct()` for async setup (DB connections, file handles)
- [ ] Always implement `_destroy()` to release resources on error or early close
- [ ] Use `_final()` to flush buffered data before the stream ends
- [ ] Use `objectMode: true` when streaming JavaScript objects, not buffers
- [ ] Respect backpressure — check the return value of `push()` and `write()`
- [ ] Use `AbortController` for cancellation instead of manual flags
- [ ] Handle errors in `_construct()`, `_read()`, `_write()`, and `_destroy()` via `callback(err)`
- [ ] Never throw synchronously from stream internals — always use callbacks or `destroy(err)`
- [ ] Use `highWaterMark` tuning for batch sizes to balance memory and throughput
- [ ] Emit custom events (`'batch-written'`, `'dead-letter'`) for observability

## Cross-References

- [Stream Architecture and Backpressure](../01-streams-architecture/02-backpressure-performance.md)
- [Custom Transform and Duplex](./02-custom-transform-duplex.md) for transformation patterns
- [Stream Error Handling](../07-stream-error-handling/01-error-patterns.md) for error propagation
- [Stream Testing](../11-stream-testing/01-unit-testing.md) for testing custom streams

## Next Steps

- Build custom Transform streams for data conversion pipelines ([Custom Transform and Duplex](./02-custom-transform-duplex.md))
- Learn stream composition with `pipeline()` and `compose()` ([Stream Piping and Composition](./03-stream-piping-composition.md))
- Apply concurrency patterns to process multiple custom streams in parallel
