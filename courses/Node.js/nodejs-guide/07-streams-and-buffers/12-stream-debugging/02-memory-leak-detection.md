# Stream Memory Leak Detection and Prevention

## What You'll Learn

- Common memory leak patterns in streams (unclosed streams, accumulating transforms)
- Heap snapshot comparison for leak detection
- WeakRef patterns for stream resource tracking
- FinalizationRegistry for stream cleanup
- Memory usage monitoring during stream operations
- Real-world: diagnosing and fixing a memory leak in a long-running stream service

## Common Memory Leak Patterns

### Pattern 1: Unclosed Streams

Streams that are never ended or destroyed hold references to internal buffers and listeners.

```js
// LEAK: stream never closed
function createLeakyStream() {
  const stream = new Transform({
    transform(chunk, enc, cb) { this.push(chunk); cb(); },
  });
  return stream; // Reference escapes but is never ended
}

// FIXED: always clean up
function createSafeStream() {
  const stream = new Transform({
    transform(chunk, enc, cb) { this.push(chunk); cb(); },
  });
  stream.on('error', () => stream.destroy());
  return stream;
}
```

### Pattern 2: Accumulating Transforms

Transforms that accumulate data without emitting create unbounded memory growth.

```js
// LEAK: buffer grows forever
class LeakyAccumulator extends Transform {
  #records = [];
  constructor() { super({ objectMode: true }); }
  _transform(chunk, enc, cb) { this.#records.push(chunk); cb(); } // Never flushed
}

// FIXED: flush when threshold reached
class SafeAccumulator extends Transform {
  #records = [];
  #maxSize;
  constructor(maxSize = 1000) { super({ objectMode: true }); this.#maxSize = maxSize; }
  _transform(chunk, enc, cb) {
    this.#records.push(chunk);
    if (this.#records.length >= this.#maxSize) { this.push([...this.#records]); this.#records = []; }
    cb();
  }
  _flush(cb) { if (this.#records.length) this.push([...this.#records]); cb(); }
}
```

### Pattern 3: Event Listener Leaks

```js
// LEAK: handler never removed
function attachLeakyListener(stream) {
  stream.on('data', (data) => console.log(data));
}

// FIXED: remove on close
function attachSafeListener(stream) {
  const handler = (data) => console.log(data);
  stream.on('data', handler);
  stream.once('close', () => stream.removeListener('data', handler));
}
```

## Heap Snapshot Comparison

Take snapshots before and after stream operations to identify growing object counts.

```js
// heap-diff.mjs
import { writeHeapSnapshot } from 'node:v8';
import { Readable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

function takeHeapSnapshot(label) {
  const filename = writeHeapSnapshot();
  console.log(`Heap snapshot (${label}): ${filename}`);
  return filename;
}

async function detectLeaks(iterations = 5) {
  if (global.gc) global.gc();
  takeHeapSnapshot('baseline');

  for (let i = 0; i < iterations; i++) {
    const source = Readable.from(
      Array.from({ length: 10000 }, (_, j) => ({ id: j, data: `record-${j}` }))
    );
    const transform = new Transform({
      objectMode: true,
      transform(chunk, enc, cb) { this.push({ ...chunk, processed: true }); cb(); },
    });
    let count = 0;
    const sink = new Transform({
      objectMode: true, transform(chunk, enc, cb) { count++; cb(); },
    });
    await pipeline(source, transform, sink);
    console.log(`Iteration ${i + 1}: ${count} records`);
  }

  if (global.gc) global.gc();
  takeHeapSnapshot('after-iterations');

  console.log('Compare snapshots in Chrome DevTools (chrome://inspect) → "Comparison" view');
}

detectLeaks().catch(console.error);
```

## WeakRef Patterns for Stream Tracking

Use `WeakRef` to track streams without preventing garbage collection.

```js
// weakref-stream-tracker.mjs

class StreamResourceTracker {
  #refs = new Map();
  #timer;

  constructor(options = {}) {
    this.#timer = setInterval(() => this.#cleanup(), options.checkIntervalMs ?? 10000);
    this.#timer.unref();
  }

  track(stream, label) {
    const ref = new WeakRef(stream);
    this.#refs.set(label, { ref, createdAt: Date.now() });
    stream.once('close', () => this.#refs.delete(label));
    return stream;
  }

  #cleanup() {
    for (const [label, { ref }] of this.#refs) {
      if (!ref.deref()) {
        this.#refs.delete(label);
        console.warn(`Stream "${label}" GC'd without closing`);
      }
    }
  }

  report() {
    let alive = 0, dead = 0;
    for (const [, { ref }] of this.#refs) ref.deref() ? alive++ : dead++;
    return { alive, dead, tracked: this.#refs.size };
  }

  destroy() { clearInterval(this.#timer); this.#refs.clear(); }
}

export { StreamResourceTracker };
```

## FinalizationRegistry for Stream Cleanup

Executes cleanup callbacks when objects are garbage collected.

```js
// finalization-stream-cleanup.mjs

const streamCleanupRegistry = new FinalizationRegistry((heldValue) => {
  const { label, resources } = heldValue;
  console.warn(`Stream "${label}" GC'd — cleaning up resources`);
  for (const r of resources) {
    if (r.type === 'file-handle') r.handle?.close?.().catch(() => {});
    if (r.type === 'worker') r.handle?.terminate?.().catch(() => {});
    if (r.type === 'timer') { clearInterval(r.handle); clearTimeout(r.handle); }
  }
});

function createManagedStream(stream, { label = 'unnamed', resources = [] } = {}) {
  streamCleanupRegistry.register(stream, { label, resources }, stream);

  stream.once('close', () => {
    streamCleanupRegistry.unregister(stream);
    for (const r of resources) {
      if (r.type === 'file-handle') r.handle?.close?.().catch(() => {});
      if (r.type === 'worker') r.handle?.terminate?.().catch(() => {});
      if (r.type === 'timer') clearInterval(r.handle);
    }
  });

  return stream;
}

export { createManagedStream, streamCleanupRegistry };
```

## Memory Usage Monitoring

```js
// memory-monitor.mjs
import { Transform } from 'node:stream';

class MemoryMonitorTransform extends Transform {
  #transformFn;
  #samples = [];
  #timer;
  #maxHeapMB;
  #sampleIntervalMs;

  constructor(transformFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#transformFn = transformFn;
    this.#maxHeapMB = options.maxHeapMB ?? 512;
    this.#sampleIntervalMs = options.sampleIntervalMs ?? 1000;
  }

  _construct(callback) {
    this.#timer = setInterval(() => {
      const u = process.memoryUsage();
      const sample = { ts: Date.now(), heapMB: Math.round(u.heapUsed / 1048576), rssMB: Math.round(u.rss / 1048576) };
      this.#samples.push(sample);
      this.emit('memory-sample', sample);
      if (sample.heapMB > this.#maxHeapMB) this.emit('memory-warning', { current: sample.heapMB, limit: this.#maxHeapMB });
    }, this.#sampleIntervalMs);
    this.#timer.unref();
    callback();
  }

  async _transform(chunk, enc, cb) {
    try { this.push(await this.#transformFn(chunk)); cb(); }
    catch (err) { cb(err); }
  }

  _flush(cb) {
    clearInterval(this.#timer);
    if (this.#samples.length > 1) {
      const first = this.#samples[0], last = this.#samples.at(-1);
      this.emit('memory-report', {
        durationMs: last.ts - first.ts,
        heapGrowthMB: last.heapMB - first.heapMB,
        peakHeapMB: Math.max(...this.#samples.map((s) => s.heapMB)),
      });
    }
    cb();
  }

  getSamples() { return [...this.#samples]; }
}

export { MemoryMonitorTransform };
```

## Real-World: Diagnosing a Memory Leak in a Long-Running Service

```js
// leak-diagnosis.mjs
import { Readable, Transform, Writable, pipeline } from 'node:stream';
import { promisify } from 'node:util';
import { MemoryMonitorTransform } from './memory-monitor.mjs';
import { StreamResourceTracker } from './weakref-stream-tracker.mjs';

const pipelineAsync = promisify(pipeline);

// LEAKY: buffer never cleared after flush
class LeakyLogAggregator extends Transform {
  #buffer = [];
  constructor() { super({ objectMode: true }); }
  _transform(logEntry, enc, cb) {
    this.#buffer.push(logEntry);
    if (this.#buffer.length % 1000 === 0) this.push({ type: 'batch', entries: [...this.#buffer] });
    cb();
  }
}

// FIXED: clear buffer after flush
class FixedLogAggregator extends Transform {
  #buffer = [];
  #threshold;
  constructor(threshold = 1000) { super({ objectMode: true }); this.#threshold = threshold; }
  _transform(logEntry, enc, cb) {
    this.#buffer.push(logEntry);
    if (this.#buffer.length >= this.#threshold) this.#flushBatch();
    cb();
  }
  #flushBatch() {
    if (!this.#buffer.length) return;
    this.push({ type: 'batch', count: this.#buffer.length, entries: this.#buffer.splice(0) });
  }
  _flush(cb) { this.#flushBatch(); cb(); }
}

async function runDiagnosis() {
  const tracker = new StreamResourceTracker();
  const monitor = new MemoryMonitorTransform(async (entry) => entry, { sampleIntervalMs: 500, maxHeapMB: 256 });

  monitor.on('memory-warning', ({ current, limit }) => console.error(`WARNING: heap ${current}MB exceeds ${limit}MB`));
  monitor.on('memory-report', ({ heapGrowthMB, peakHeapMB }) => {
    console.log(`\n--- Memory Report ---`);
    console.log(`Heap growth: ${heapGrowthMB}MB | Peak: ${peakHeapMB}MB`);
    if (heapGrowthMB > 50) console.error('DETECTED: Significant heap growth — possible leak');
    else console.log('OK: Acceptable growth');
  });

  const source = Readable.from(
    Array.from({ length: 50000 }, (_, i) => ({
      ts: Date.now(), level: ['info', 'warn', 'error'][i % 3],
      service: `svc-${i % 10}`, message: `Log ${i}`,
    }))
  );

  tracker.track(source, 'source');
  tracker.track(monitor, 'monitor');

  // Swap LeakyLogAggregator here to see the leak in action
  const aggregator = new FixedLogAggregator(500);
  tracker.track(aggregator, 'aggregator');

  const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { cb(); } });

  await pipelineAsync(source, aggregator, monitor, sink);
  console.log('\nResource tracker:', tracker.report());
  tracker.destroy();
}

runDiagnosis().catch(console.error);
```

## Best Practices Checklist

- [ ] Always end streams with `pipeline()` or explicit `.end()` calls
- [ ] Set `highWaterMark` to bound internal buffer sizes
- [ ] Periodically flush accumulating transforms to prevent unbounded growth
- [ ] Use `WeakRef` to track streams without preventing garbage collection
- [ ] Register resources with `FinalizationRegistry` for cleanup on GC
- [ ] Monitor heap growth during long-running stream operations
- [ ] Compare heap snapshots before and after suspected leaky operations
- [ ] Use `stream.on('close', ...)` to clean up listeners and timers
- [ ] Avoid closures that capture large objects in stream event handlers
- [ ] Test stream services under sustained load to detect gradual leaks

## Cross-References

- [Stream Debugging Tools](./01-debugging-tools.md) — Node.js inspector and stream diagnostics
- [Production Troubleshooting](./03-production-troubleshooting.md) — diagnosing leaks in production
- [Circuit Breaker and Error Recovery](../07-stream-error-handling/02-circuit-breaker-recovery.md) — preventing leak-causing error accumulation
- [Graceful Degradation](../07-stream-error-handling/03-graceful-degradation.md) — clean shutdown to prevent leaks on exit

## Next Steps

- Set up automated heap snapshot collection in CI/CD for leak regression testing
- Build a stream resource dashboard using `StreamResourceTracker` metrics
- Implement `perf_hooks` `PerformanceObserver` for GC event correlation with stream operations
