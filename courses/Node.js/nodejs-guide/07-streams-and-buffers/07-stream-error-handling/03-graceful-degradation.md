# Graceful Degradation and Fault Tolerance in Streams

## What You'll Learn

- Graceful shutdown of stream pipelines (SIGTERM/SIGINT handling)
- Partial failure handling in fan-out/fan-in pipelines
- Fallback stream sources when primary fails
- Stream health checks and liveness probes
- Timeout patterns for stream operations
- Real-world: gracefully shutting down a data processing service

## Graceful Shutdown

Production stream services must drain in-flight work before exiting on `SIGTERM` or `SIGINT`.

```js
// graceful-shutdown.mjs
import { Transform } from 'node:stream';

class GracefulShutdownManager {
  #shuttingDown = false;
  #activeStreams = new Set();
  #drainTimeoutMs;
  #onShutdown;

  constructor(options = {}) {
    this.#drainTimeoutMs = options.drainTimeoutMs ?? 30000;
    this.#onShutdown = options.onShutdown ?? (() => {});
    process.on('SIGTERM', () => this.shutdown('SIGTERM'));
    process.on('SIGINT', () => this.shutdown('SIGINT'));
  }

  get isShuttingDown() { return this.#shuttingDown; }

  track(stream) {
    this.#activeStreams.add(stream);
    stream.on('close', () => this.#activeStreams.delete(stream));
    return stream;
  }

  async shutdown(signal) {
    if (this.#shuttingDown) return;
    this.#shuttingDown = true;
    console.log(`[${signal}] Graceful shutdown initiated`);
    this.#onShutdown({ signal, activeStreams: this.#activeStreams.size });

    for (const stream of this.#activeStreams) {
      if (typeof stream.end === 'function' && !stream.writableEnded) stream.end();
    }

    const forceTimer = setTimeout(() => {
      console.error(`[${signal}] Drain timeout — forcing exit`);
      process.exit(1);
    }, this.#drainTimeoutMs);
    forceTimer.unref();

    await Promise.allSettled(
      [...this.#activeStreams].map((s) => new Promise((r) => s.on('close', r)))
    );
    console.log(`[${signal}] All streams drained`);
    process.exit(0);
  }
}

class ShutdownAwareTransform extends Transform {
  #manager;
  #transformFn;

  constructor(manager, transformFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#manager = manager;
    this.#transformFn = transformFn;
  }

  async _transform(chunk, enc, callback) {
    if (this.#manager.isShuttingDown) { this.push(chunk); return callback(); }
    try { this.push(await this.#transformFn(chunk)); callback(); }
    catch (err) { callback(err); }
  }

  _flush(cb) { cb(); }
}

export { GracefulShutdownManager, ShutdownAwareTransform };
```

## Partial Failure in Fan-Out/Fan-In

```js
// fan-out-resilient.mjs
import { Transform } from 'node:stream';

class ResilientFanOut extends Transform {
  #destinations;
  #errorStrategy;

  constructor(destinations, options = {}) {
    super({ objectMode: true, ...options });
    this.#destinations = destinations; // [{ name, transformFn }]
    this.#errorStrategy = options.errorStrategy ?? 'continue';
  }

  _transform(chunk, enc, callback) {
    Promise.all(this.#destinations.map(async ({ name, transformFn }) => {
      try { return { name, success: true, result: await transformFn(chunk) }; }
      catch (error) { return { name, success: false, error: error.message }; }
    })).then((outcomes) => {
      const failures = outcomes.filter((o) => !o.success);
      if (failures.length && this.#errorStrategy === 'fail-fast') {
        return callback(new Error(`Fan-out failures: ${failures.map((f) => `${f.name}: ${f.error}`).join(', ')}`));
      }
      this.push({
        input: chunk,
        successes: outcomes.filter((o) => o.success).map((s) => ({ name: s.name, result: s.result })),
        failures: failures.map((f) => ({ name: f.name, error: f.error })),
      });
      callback();
    }).catch(callback);
  }

  _flush(cb) { cb(); }
}

export { ResilientFanOut };
```

## Fallback Stream Sources

```js
// fallback-source.mjs
import { Readable } from 'node:stream';

class FallbackReadable extends Readable {
  #primaryFn;
  #fallbackFn;
  #source = null;
  #usingFallback = false;

  constructor(primaryFn, fallbackFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#primaryFn = primaryFn;
    this.#fallbackFn = fallbackFn;
  }

  async _read() {
    if (!this.#source) {
      try { this.#source = this.#primaryFn(); console.log('Using primary source'); }
      catch (err) { console.warn(`Primary failed: ${err.message}, using fallback`); this.#source = this.#fallbackFn(); this.#usingFallback = true; }
    }
    try {
      const { value, done } = await this.#source.next();
      if (done) this.push(null); else this.push(value);
    } catch (err) {
      if (!this.#usingFallback) {
        console.warn(`Primary error: ${err.message}, switching to fallback`);
        this.#source = this.#fallbackFn();
        this.#usingFallback = true;
        return this._read();
      }
      this.destroy(err);
    }
  }
}

// Usage
const stream = new FallbackReadable(
  async function* () {
    const res = await fetch('https://api.example.com/events');
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    for (const e of (await res.json()).events) yield e;
  },
  async function* () {
    const data = JSON.parse(await import('node:fs/promises')
      .then((fs) => fs.readFile('./cache/events.json', 'utf-8')));
    console.log('Serving from cache fallback');
    for (const e of data) yield e;
  }
);

export { FallbackReadable };
```

## Stream Health Checks and Liveness Probes

```js
// stream-health.mjs
import { createServer } from 'node:http';
import { Transform } from 'node:stream';
import { EventEmitter } from 'node:events';

class StreamHealthMonitor extends EventEmitter {
  #metrics = { recordsProcessed: 0, recordsFailed: 0, lastRecordAt: null, startedAt: Date.now() };
  #stalenessMs;

  constructor(options = {}) { super(); this.#stalenessMs = options.stalenessMs ?? 60000; }

  recordSuccess() { this.#metrics.recordsProcessed++; this.#metrics.lastRecordAt = Date.now(); }
  recordFailure(err) { this.#metrics.recordsFailed++; this.emit('failure', err); }

  get isHealthy() {
    if (!this.#metrics.lastRecordAt) return true;
    return Date.now() - this.#metrics.lastRecordAt < this.#stalenessMs;
  }

  getMetrics() {
    return { ...this.#metrics, uptimeMs: Date.now() - this.#metrics.startedAt, healthy: this.isHealthy };
  }

  startProbeServer(port = 3000) {
    const server = createServer((req, res) => {
      if (req.url === '/healthz') {
        res.writeHead(this.isHealthy ? 200 : 503, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({ status: this.isHealthy ? 'ok' : 'unhealthy' }));
      } else if (req.url === '/metrics') {
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify(this.getMetrics()));
      } else { res.writeHead(404); res.end(); }
    });
    server.listen(port);
    return server;
  }
}

class HealthReportingTransform extends Transform {
  #monitor; #transformFn;
  constructor(monitor, transformFn, options = {}) {
    super({ objectMode: true, ...options }); this.#monitor = monitor; this.#transformFn = transformFn;
  }
  async _transform(chunk, enc, cb) {
    try { this.push(await this.#transformFn(chunk)); this.#monitor.recordSuccess(); cb(); }
    catch (err) { this.#monitor.recordFailure(err); cb(err); }
  }
  _flush(cb) { cb(); }
}

export { StreamHealthMonitor, HealthReportingTransform };
```

## Timeout Patterns for Stream Operations

```js
// timeout-transform.mjs
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class TimeoutTransform extends Transform {
  #transformFn;
  #timeoutMs;

  constructor(transformFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#transformFn = transformFn;
    this.#timeoutMs = options.timeoutMs ?? 5000;
  }

  async _transform(chunk, enc, callback) {
    const timer = setTimeout(() => {}, this.#timeoutMs);
    try {
      const result = await Promise.race([
        this.#transformFn(chunk),
        new Promise((_, reject) => {
          const t = setTimeout(() => reject(new Error('Record processing timeout')), this.#timeoutMs);
          t.unref();
        }),
      ]);
      this.push(result);
      callback();
    } catch (err) { callback(err); }
  }

  _flush(cb) { cb(); }
}

async function pipelineWithTimeout(streams, timeoutMs, label = 'pipeline') {
  const controller = new AbortController();
  const timer = setTimeout(() => { controller.abort(); console.error(`[${label}] Timeout after ${timeoutMs}ms`); }, timeoutMs);
  timer.unref();
  try { await pipeline(...streams, { signal: controller.signal }); }
  finally { clearTimeout(timer); }
}

export { TimeoutTransform, pipelineWithTimeout };
```

## Real-World: Gracefully Shutting Down a Data Processing Service

```js
// data-processing-service.mjs
import { Readable, Transform, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { GracefulShutdownManager, ShutdownAwareTransform } from './graceful-shutdown.mjs';
import { StreamHealthMonitor, HealthReportingTransform } from './stream-health.mjs';
import { FallbackReadable } from './fallback-source.mjs';
import { CheckpointTransform } from '../02-circuit-breaker-recovery/checkpoint-transform.mjs';
import { createHash } from 'node:crypto';

function databaseSource() {
  let offset = 0;
  return Readable.from((async function* () {
    while (true) {
      for (let i = 0; i < 50; i++) yield { id: offset + i, payload: `record-${offset + i}` };
      offset += 50;
      await new Promise((r) => setTimeout(r, 100));
    }
  })());
}

async function processRecord(record) {
  const hash = createHash('sha256').update(JSON.stringify(record)).digest('hex');
  return { ...record, hash, processedAt: Date.now() };
}

const shutdown = new GracefulShutdownManager({
  drainTimeoutMs: 10000,
  onShutdown: ({ signal }) => console.log(`[${signal}] Shutting down data processing service`),
});

const health = new StreamHealthMonitor({ stalenessMs: 30000 });
health.on('failure', (err) => console.error(`Health failure: ${err.message}`));
const probeServer = health.startProbeServer(8080);

const source = shutdown.track(new FallbackReadable(
  databaseSource, () => Readable.from([{ id: -1, payload: 'fallback' }])
));
const processor = shutdown.track(new HealthReportingTransform(health, processRecord));
const checkpointed = shutdown.track(new CheckpointTransform(
  async (r) => { processor.write(r); return r; },
  { checkpointPath: './service-checkpoint.json', saveInterval: 100 }
));
const sink = shutdown.track(new Writable({
  objectMode: true,
  write(chunk, enc, cb) { cb(); },
}));

try {
  await pipeline(source, checkpointed, sink);
} catch (err) {
  if (err.code === 'ABORT_ERR') console.log('Pipeline aborted — shutdown in progress');
  else console.error('Pipeline error:', err);
} finally {
  probeServer.close();
  console.log('Final metrics:', health.getMetrics());
}
```

## Best Practices Checklist

- [ ] Always handle `SIGTERM` and `SIGINT` for graceful shutdown in production
- [ ] Set a drain timeout to prevent infinite hangs during shutdown
- [ ] Use `AbortController` with `pipeline` for cancellable stream operations
- [ ] Implement health probes (`/healthz`, `/metrics`) for orchestration systems
- [ ] Track record staleness to detect stalled pipelines
- [ ] Provide fallback sources for critical data paths
- [ ] Use checkpointing to resume after crashes without reprocessing
- [ ] Log shutdown events with active stream counts for debugging
- [ ] Test shutdown behavior with fault injection (kill -TERM, kill -INT)
- [ ] Never use `process.exit()` without draining in-flight work first

## Cross-References

- [Circuit Breaker and Error Recovery](./02-circuit-breaker-recovery.md) — circuit breaker and retry patterns
- [Stream Error Handling Patterns](./01-error-patterns.md) — foundational error handling
- [Stream Debugging](../12-stream-debugging/01-debugging-tools.md) — debugging shutdown and health issues
- [Worker Thread Streams](../06-stream-concurrency-parallelism/02-worker-thread-streams.md) — graceful shutdown with worker pools

## Next Steps

- Implement automatic scaling based on health metrics
- Build a dashboard that visualizes pipeline health and throughput
- Add dead letter queue integration for records that fail during shutdown drain
