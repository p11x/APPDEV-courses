# Stream Partitioning, Batching, and Windowing

## What You'll Learn

- Partitioning streams by key (like Kafka partitioning)
- Time-based windowing transforms (tumbling, sliding, session windows)
- Count-based batching transforms
- Rate-limiting transforms (tokens per second)
- Debouncing and throttling stream patterns
- Real-world: log aggregation with time-windowed stream processing

## Stream Partitioning by Key

Routes records to separate sub-streams based on a key function. Each partition can be processed independently.

```js
// partition-transform.mjs
import { Transform, PassThrough } from 'node:stream';

class PartitionTransform extends Transform {
  #keyFn;
  #partitions = new Map();
  #handlers = new Map();

  constructor(keyFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#keyFn = keyFn;
  }

  getPartition(key) {
    if (!this.#partitions.has(key)) {
      const passthrough = new PassThrough({ objectMode: true });
      this.#partitions.set(key, passthrough);
      const handler = this.#handlers.get(key);
      if (handler) handler(passthrough, key);
    }
    return this.#partitions.get(key);
  }

  onPartition(key, handler) { this.#handlers.set(key, handler); }

  _transform(chunk, encoding, callback) {
    this.getPartition(this.#keyFn(chunk)).write(chunk);
    this.push(chunk);
    callback();
  }

  _flush(callback) {
    for (const [, stream] of this.#partitions) stream.end();
    callback();
  }
}

export { PartitionTransform };
```

```js
// partition-example.mjs
import { Readable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { PartitionTransform } from './partition-transform.mjs';

const events = Readable.from([
  { service: 'auth', level: 'info', msg: 'login' },
  { service: 'payment', level: 'error', msg: 'charge failed' },
  { service: 'auth', level: 'warn', msg: 'rate limit' },
  { service: 'payment', level: 'info', msg: 'refund' },
]);

const p = new PartitionTransform((r) => r.service);
p.onPartition('auth', (s) => { (async () => { for await (const r of s) console.log(`[AUTH] ${r.msg}`); })(); });
p.onPartition('payment', (s) => { (async () => { for await (const r of s) console.log(`[PAY] ${r.msg}`); })(); });

await pipeline(events, p, async function* (src) { for await (const _ of src) {} });
```

## Time-Based Windowing

### Tumbling Windows (Fixed, Non-Overlapping)

```js
// tumbling-window.mjs
import { Transform } from 'node:stream';

class TumblingWindowTransform extends Transform {
  #durationMs;
  #window = [];
  #windowStart = Date.now();
  #timer;

  constructor(durationMs, options = {}) {
    super({ objectMode: true, ...options });
    this.#durationMs = durationMs;
    this.#timer = setInterval(() => this.#emit(), durationMs);
    this.#timer.unref();
  }

  _transform(chunk, enc, cb) {
    if (Date.now() - this.#windowStart >= this.#durationMs) { this.#emit(); this.#windowStart = Date.now(); }
    this.#window.push(chunk);
    cb();
  }

  #emit() {
    if (!this.#window.length) return;
    this.push({ windowStart: this.#windowStart, windowEnd: this.#windowStart + this.#durationMs,
      count: this.#window.length, records: this.#window.splice(0) });
  }

  _flush(cb) { clearInterval(this.#timer); this.#emit(); cb(); }
}

export { TumblingWindowTransform };
```

### Sliding Windows (Overlapping)

```js
class SlidingWindowTransform extends Transform {
  #sizeMs;
  #records = [];
  #timer;

  constructor(sizeMs, slideMs, options = {}) {
    super({ objectMode: true, ...options });
    this.#sizeMs = sizeMs;
    this.#timer = setInterval(() => this.#slide(), slideMs);
    this.#timer.unref();
  }

  _transform(chunk, enc, cb) { this.#records.push({ t: Date.now(), data: chunk }); cb(); }

  #slide() {
    const now = Date.now(), cutoff = now - this.#sizeMs;
    while (this.#records.length && this.#records[0].t < cutoff) this.#records.shift();
    this.push({ windowStart: cutoff, windowEnd: now, count: this.#records.length,
      records: this.#records.map((r) => r.data) });
  }

  _flush(cb) { clearInterval(this.#timer); this.#slide(); cb(); }
}
```

### Session Windows (Gap-Based)

```js
class SessionWindowTransform extends Transform {
  #gapMs;
  #session = [];
  #sessionStart = null;
  #timeout = null;

  constructor(gapMs, options = {}) { super({ objectMode: true, ...options }); this.#gapMs = gapMs; }

  _transform(chunk, enc, cb) {
    const now = Date.now();
    if (this.#session.length && now - this.#session.at(-1).t > this.#gapMs) this.#emit();
    if (!this.#session.length) this.#sessionStart = now;
    this.#session.push({ t: now, data: chunk });
    clearTimeout(this.#timeout);
    this.#timeout = setTimeout(() => { if (this.#session.length) this.#emit(); }, this.#gapMs);
    this.#timeout.unref();
    cb();
  }

  #emit() {
    if (!this.#session.length) return;
    this.push({ sessionStart: this.#sessionStart, sessionEnd: Date.now(),
      count: this.#session.length, records: this.#session.splice(0).map((r) => r.data) });
    this.#sessionStart = null;
  }

  _flush(cb) { clearTimeout(this.#timeout); this.#emit(); cb(); }
}
```

## Count-Based Batching

```js
// batch-transform.mjs
import { Transform } from 'node:stream';

class BatchTransform extends Transform {
  #batchSize;
  #batch = [];
  #timer;

  constructor(batchSize, { flushIntervalMs, ...options } = {}) {
    super({ objectMode: true, ...options });
    this.#batchSize = batchSize;
    if (flushIntervalMs) { this.#timer = setInterval(() => this.#flushBatch(), flushIntervalMs); this.#timer.unref(); }
  }

  _transform(chunk, enc, cb) {
    this.#batch.push(chunk);
    if (this.#batch.length >= this.#batchSize) this.#flushBatch();
    cb();
  }

  #flushBatch() { if (this.#batch.length) this.push(this.#batch.splice(0)); }

  _flush(cb) { clearInterval(this.#timer); this.#flushBatch(); cb(); }
}

export { BatchTransform };
```

## Rate-Limiting (Token Bucket)

```js
// rate-limit-transform.mjs
import { Transform } from 'node:stream';

class RateLimitTransform extends Transform {
  #tps;
  #tokens;
  #lastRefill;
  #pending = [];
  #timer;

  constructor(tokensPerSecond, options = {}) {
    super({ objectMode: true, ...options });
    this.#tps = tokensPerSecond;
    this.#tokens = tokensPerSecond;
    this.#lastRefill = Date.now();
    this.#timer = setInterval(() => this.#refill(), 100);
    this.#timer.unref();
  }

  #refill() {
    const now = Date.now();
    this.#tokens = Math.min(this.#tps, this.#tokens + ((now - this.#lastRefill) / 1000) * this.#tps);
    this.#lastRefill = now;
    while (this.#pending.length && this.#tokens >= 1) {
      this.#tokens--;
      const { chunk, cb } = this.#pending.shift();
      this.push(chunk); cb();
    }
  }

  _transform(chunk, enc, cb) {
    if (this.#tokens >= 1) { this.#tokens--; this.push(chunk); cb(); }
    else this.#pending.push({ chunk, cb });
  }

  _flush(cb) {
    clearInterval(this.#timer);
    for (const { chunk, cb } of this.#pending) { this.push(chunk); cb(); }
    cb();
  }
}

export { RateLimitTransform };
```

## Debouncing and Throttling

```js
import { Transform } from 'node:stream';

class DebounceTransform extends Transform {
  #delayMs; #last = null; #timer = null;
  constructor(delayMs, opts = {}) { super({ objectMode: true, ...opts }); this.#delayMs = delayMs; }
  _transform(chunk, enc, cb) {
    this.#last = chunk;
    clearTimeout(this.#timer);
    this.#timer = setTimeout(() => { if (this.#last !== null) { this.push(this.#last); this.#last = null; } }, this.#delayMs);
    cb();
  }
  _flush(cb) { clearTimeout(this.#timer); if (this.#last !== null) this.push(this.#last); cb(); }
}

class ThrottleTransform extends Transform {
  #intervalMs; #lastEmit = 0;
  constructor(intervalMs, opts = {}) { super({ objectMode: true, ...opts }); this.#intervalMs = intervalMs; }
  _transform(chunk, enc, cb) {
    const now = Date.now();
    if (now - this.#lastEmit >= this.#intervalMs) { this.#lastEmit = now; this.push(chunk); }
    cb();
  }
}

export { DebounceTransform, ThrottleTransform };
```

## Real-World: Log Aggregation with Time-Windowed Processing

```js
// log-aggregator.mjs
import { createReadStream } from 'node:fs';
import { createInterface } from 'node:readline';
import { pipeline } from 'node:stream/promises';
import { Readable, Transform } from 'node:stream';
import { TumblingWindowTransform } from './tumbling-window.mjs';

class LogParserTransform extends Transform {
  _transform(line, enc, cb) {
    const m = line.match(/^(\d{4}-\d{2}-\d{2}T[\d:.Z]+)\s+(\w+)\s+\[(\w+)\]\s+(.*)$/);
    if (!m) return cb();
    this.push({ timestamp: new Date(m[1]).getTime(), level: m[2], service: m[3], message: m[4] });
    cb();
  }
}

class WindowAggregatorTransform extends Transform {
  _transform(window, enc, cb) {
    const byService = {}, byLevel = {};
    for (const r of window.records) {
      byService[r.service] = (byService[r.service] ?? 0) + 1;
      byLevel[r.level] = (byLevel[r.level] ?? 0) + 1;
    }
    this.push({
      window: { start: new Date(window.windowStart).toISOString(), end: new Date(window.windowEnd).toISOString() },
      totalEvents: window.count, byService, byLevel,
      errorCount: window.records.filter((r) => r.level === 'error').length,
    });
    cb();
  }
}

function logLineSource(filePath) {
  const rl = createInterface({ input: createReadStream(filePath, 'utf-8') });
  return Readable.from((async function* () { for await (const l of rl) if (l.trim()) yield l; })());
}

await pipeline(
  logLineSource('./app.log'),
  new LogParserTransform(),
  new TumblingWindowTransform(5000),
  new WindowAggregatorTransform(),
  async function* (src) {
    for await (const a of src) {
      console.log(`[${a.window.start} - ${a.window.end}] ${a.totalEvents} events | errors: ${a.errorCount} | ${JSON.stringify(a.byService)}`);
    }
  }
);
```

## Best Practices Checklist

- [ ] Choose partition keys that distribute load evenly
- [ ] Set window durations based on latency vs throughput trade-off
- [ ] Always flush remaining records in `_flush` for windowing transforms
- [ ] Use `timer.unref()` to prevent intervals from keeping the process alive
- [ ] Implement backpressure in partitioned streams to avoid memory growth
- [ ] Use count-based batching with a flush interval to prevent unbounded memory
- [ ] Set rate limits based on downstream capacity, not upstream desire
- [ ] Prefer tumbling windows when each record belongs to exactly one group
- [ ] Use session windows for user-activity or transaction-based grouping
- [ ] Monitor partition skew — some keys may generate disproportionate load

## Cross-References

- [Worker Thread Stream Processing](./02-worker-thread-streams.md) — parallel processing per partition
- [Stream Backpressure](../03-readable-streams/03-backpressure.md) — managing backpressure in batched streams
- [Transform Streams](../04-transform-streams/01-transform-basics.md) — transform stream fundamentals
- [Stream Error Handling](../07-stream-error-handling/01-error-patterns.md) — error handling in windowed pipelines

## Next Steps

- Combine partitioning with worker threads for per-partition parallel processing
- Implement watermark-based late data handling for windowed streams
- Build a stream processing DSL for declarative windowing and aggregation
