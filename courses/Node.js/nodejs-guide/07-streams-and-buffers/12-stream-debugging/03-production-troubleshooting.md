# Production Stream Troubleshooting Guide

## What You'll Learn

- Stream state inspection and diagnostics at runtime
- Common stream errors and their solutions (reference table)
- Performance bottleneck identification in stream pipelines
- Debug logging with structured context
- Stream metrics collection for production debugging
- Real-world: troubleshooting a stuck stream pipeline in production

## Stream State Inspection

```js
// stream-inspector.mjs
import { inspect } from 'node:util';

function inspectStream(stream, label = 'stream') {
  return {
    label,
    readable: stream.readable,
    writable: stream.writable,
    readableEnded: stream.readableEnded,
    writableEnded: stream.writableEnded,
    readableLength: stream.readableLength,
    writableLength: stream.writableLength,
    destroyed: stream.destroyed,
    errored: stream.errored?.message ?? null,
    closed: stream.closed,
    readableFlowing: stream.readableFlowing,
    writableNeedDrain: stream.writableNeedDrain,
  };
}

function logStreamStates(streams, logger = console) {
  const states = streams.map((s, i) => inspectStream(s, s.constructor.name || `stream-${i}`));
  const stuck = states.filter((s) => s.readableLength > 0 && !s.readableFlowing);

  for (const s of states) {
    logger.info(`[${s.label}] readable:${s.readable} rBuf:${s.readableLength} writable:${s.writable} wBuf:${s.writableLength} destroyed:${s.destroyed} flowing:${s.readableFlowing}`);
  }

  if (stuck.length) logger.warn('POTENTIAL STALL:', stuck.map((s) => `${s.label} (buffered: ${s.readableLength})`));
  return states;
}

export { inspectStream, logStreamStates };
```

## Common Stream Errors and Solutions

| Error | Cause | Solution |
|-------|-------|----------|
| `ERR_STREAM_PREMATURE_CLOSE` | Stream closed before pipeline finished | Ensure all streams end properly; check for early `.destroy()` |
| `ERR_STREAM_WRITE_AFTER_END` | Writing after `.end()` called | Check `writableEnded` before writing |
| `ERR_STREAM_DESTROYED` | Operating on a destroyed stream | Check `destroyed` property; handle `'close'` events |
| `ERR_STREAM_NULL_VALUES` | Pushing `null` in object mode | Use `push(null)` only in `_flush` |
| `ERR_STREAM_PUSH_AFTER_EOF` | Pushing data after `null` pushed | Do not call `push()` after `push(null)` |
| `ERR_STREAM_CANNOT_PIPE` | Piping from non-readable stream | Verify source has `readable: true` |
| `ERR_STREAM_WRITE_NOT_IMPLEMENTED` | `_write` not implemented | Implement `_write` method on custom Writable |
| `EPIPE` | Writing to closed readable | Handle `EPIPE` gracefully on writable side |

### Error Diagnostic Helper

```js
// stream-error-diagnostic.mjs

class StreamErrorDiagnostic {
  static analyze(err, stream) {
    const diagnosis = {
      error: { name: err.name, code: err.code, message: err.message },
      streamState: stream ? {
        readable: stream.readable, writable: stream.writable,
        destroyed: stream.destroyed, readableEnded: stream.readableEnded,
        writableEnded: stream.writableEnded, readableLength: stream.readableLength,
      } : null,
    };

    const suggestions = {
      'ERR_STREAM_PREMATURE_CLOSE': 'A stream closed before others finished. Check for early destroy() calls.',
      'ERR_STREAM_WRITE_AFTER_END': 'Data written after end(). Add writableEnded checks before write().',
      'ERR_STREAM_DESTROYED': 'Operations on destroyed stream. Check destroyed property or listen for close.',
      'EPIPE': 'Readable end of pipe closed. Handle EPIPE on writable side.',
    };

    diagnosis.suggestion = suggestions[err.code] ?? 'Check stack trace and stream state.';
    return diagnosis;
  }

  static format(d) {
    return `Stream Error: ${d.error.code || d.error.name}\n  Message: ${d.error.message}\n  Suggestion: ${d.suggestion}`;
  }
}

export { StreamErrorDiagnostic };
```

## Performance Bottleneck Identification

```js
// throughput-monitor.mjs
import { Transform } from 'node:stream';

class ThroughputMonitor extends Transform {
  #label;
  #count = 0;
  #bytes = 0;
  #startTime = null;
  #samples = [];
  #sampleIntervalMs;
  #timer;
  #lastSampleTime;
  #lastSampleCount = 0;

  constructor(label, options = {}) {
    super({ objectMode: options.objectMode ?? true, ...options });
    this.#label = label;
    this.#sampleIntervalMs = options.sampleIntervalMs ?? 1000;
  }

  _construct(callback) {
    this.#startTime = Date.now();
    this.#lastSampleTime = this.#startTime;
    this.#timer = setInterval(() => this.#sample(), this.#sampleIntervalMs);
    this.#timer.unref();
    callback();
  }

  #sample() {
    const now = Date.now(), elapsed = now - this.#lastSampleTime;
    const delta = this.#count - this.#lastSampleCount;
    this.#samples.push({ ts: now, rps: Math.round((delta / elapsed) * 1000) });
    this.#lastSampleTime = now;
    this.#lastSampleCount = this.#count;
  }

  _transform(chunk, enc, cb) { this.#count++; this.#bytes += Buffer.byteLength(chunk) || 0; this.push(chunk); cb(); }

  _flush(callback) {
    clearInterval(this.#timer);
    const elapsed = Date.now() - this.#startTime;
    this.emit('throughput-report', {
      label: this.#label, totalRecords: this.#count,
      durationMs: elapsed, avgRps: Math.round((this.#count / elapsed) * 1000),
      peakRps: Math.max(...this.#samples.map((s) => s.rps), 0),
    });
    callback();
  }

  getSamples() { return [...this.#samples]; }
}

export { ThroughputMonitor };
```

## Debug Logging with Structured Context

```js
// stream-logger.mjs

class StreamLogger {
  #context;
  #logFn;
  #level;

  constructor(context = {}, options = {}) {
    this.#context = context;
    this.#logFn = options.logFn ?? console.log;
    this.#level = options.level ?? 'info';
  }

  child(ctx) { return new StreamLogger({ ...this.#context, ...ctx }, { logFn: this.#logFn, level: this.#level }); }

  #log(level, message, data = {}) {
    const levels = { debug: 0, info: 1, warn: 2, error: 3 };
    if ((levels[level] ?? 0) < (levels[this.#level] ?? 0)) return;
    this.#logFn(JSON.stringify({ timestamp: new Date().toISOString(), level, message, ...this.#context, ...data }));
  }

  debug(msg, data) { this.#log('debug', msg, data); }
  info(msg, data) { this.#log('info', msg, data); }
  warn(msg, data) { this.#log('warn', msg, data); }
  error(msg, data) { this.#log('error', msg, data); }
}

class LoggingTransform extends Transform {
  #transformFn;
  #logger;
  #count = 0;

  constructor(transformFn, logger, options = {}) {
    super({ objectMode: true, ...options });
    this.#transformFn = transformFn;
    this.#logger = logger;
  }

  async _transform(chunk, enc, cb) {
    this.#count++;
    const start = performance.now();
    try {
      this.push(await this.#transformFn(chunk));
      const ms = performance.now() - start;
      if (ms > 1000) this.#logger.warn('Slow transform', { record: this.#count, durationMs: Math.round(ms) });
      cb();
    } catch (err) {
      this.#logger.error('Transform failed', { record: this.#count, error: err.message });
      cb(err);
    }
  }

  _flush(cb) { this.#logger.info('Transform completed', { totalRecords: this.#count }); cb(); }
}

export { StreamLogger, LoggingTransform };
```

## Stream Metrics Collection

```js
// stream-metrics.mjs
import { Transform } from 'node:stream';
import { EventEmitter } from 'node:events';

class StreamMetricsCollector extends EventEmitter {
  #metrics = new Map();
  #timer;

  constructor(options = {}) {
    super();
    this.#timer = setInterval(() => this.#emit(), options.emitIntervalMs ?? 5000);
    this.#timer.unref();
  }

  register(name, stream) {
    const m = { recordsIn: 0, recordsOut: 0, errors: 0, startTime: Date.now(), lastActivity: Date.now() };
    this.#metrics.set(name, m);

    const origWrite = stream.write?.bind(stream);
    const origPush = stream.push?.bind(stream);

    if (origWrite) stream.write = (chunk, ...args) => { m.recordsIn++; m.lastActivity = Date.now(); return origWrite(chunk, ...args); };
    if (origPush) stream.push = (chunk, ...args) => { if (chunk !== null) { m.recordsOut++; m.lastActivity = Date.now(); } return origPush(chunk, ...args); };
    stream.on('error', () => { m.errors++; m.lastActivity = Date.now(); });

    return stream;
  }

  getMetrics(name) {
    if (name) { const m = this.#metrics.get(name); return m ? { ...m, ageMs: Date.now() - m.startTime } : null; }
    const result = {};
    for (const [n, m] of this.#metrics) result[n] = { ...m, ageMs: Date.now() - m.startTime, idleMs: Date.now() - m.lastActivity };
    return result;
  }

  #emit() { this.emit('metrics', this.getMetrics()); }

  destroy() { clearInterval(this.#timer); this.#metrics.clear(); }
}

export { StreamMetricsCollector };
```

## Real-World: Troubleshooting a Stuck Stream Pipeline

```js
// troubleshoot-stuck-pipeline.mjs
import { Readable, Transform, Writable, pipeline } from 'node:stream';
import { promisify } from 'node:util';
import { logStreamStates } from './stream-inspector.mjs';
import { StreamErrorDiagnostic } from './stream-error-diagnostic.mjs';
import { StreamLogger, LoggingTransform } from './stream-logger.mjs';
import { StreamMetricsCollector } from './stream-metrics.mjs';
import { ThroughputMonitor } from './throughput-monitor.mjs';

const pipelineAsync = promisify(pipeline);

function createProductionPipeline() {
  const logger = new StreamLogger({ pipeline: 'order-processing', env: 'production' });
  const metrics = new StreamMetricsCollector({ emitIntervalMs: 2000 });

  metrics.on('metrics', (all) => {
    for (const [name, m] of Object.entries(all)) {
      if (m.idleMs > 10000 && m.recordsIn > 0) logger.warn(`Stream "${name}" stalled`, { idleMs: m.idleMs, in: m.recordsIn, out: m.recordsOut });
    }
  });

  const source = metrics.register('source', Readable.from(
    Array.from({ length: 1000 }, (_, i) => ({ orderId: `ord-${i}`, items: [{ sku: `SKU-${i % 50}`, qty: 1 + (i % 5) }], total: Math.round(Math.random() * 50000) / 100 }))
  ));

  const throughput = new ThroughputMonitor('order-processing', { objectMode: true });
  throughput.on('throughput-report', (r) => logger.info('Throughput', r));

  const enricher = metrics.register('enricher', new LoggingTransform(
    async (order) => {
      if (Math.random() < 0.01) await new Promise((r) => setTimeout(r, 30000)); // Simulate hang
      await new Promise((r) => setTimeout(r, 50));
      return { ...order, enriched: true };
    },
    logger.child({ stage: 'enricher' }),
    { objectMode: true }
  ));

  const validator = metrics.register('validator', new LoggingTransform(
    async (order) => {
      if (!order.items?.length) throw Object.assign(new Error('Order has no items'), { code: 'INVALID_ORDER' });
      return { ...order, validated: true };
    },
    logger.child({ stage: 'validator' }),
    { objectMode: true }
  ));

  const sink = metrics.register('sink', new Writable({
    objectMode: true, write(chunk, enc, cb) { setTimeout(cb, 10); },
  }));

  // Diagnostic interval
  const diagTimer = setInterval(() => {
    const states = logStreamStates([source, enricher, validator, sink], logger);
    const stuck = states.filter((s) => s.readable && !s.readableFlowing && s.readableLength > 0);
    if (stuck.length) {
      logger.error('DIAGNOSTIC: Pipeline stuck', {
        stuckStages: stuck.map((s) => s.label),
        buffers: states.map((s) => ({ label: s.label, rBuf: s.readableLength, wBuf: s.writableLength })),
        suggestions: ['Check enricher external dependency', 'Verify DB connection for sink', 'Check backpressure in validator'],
      });
    }
  }, 5000);
  diagTimer.unref();

  return { source, enricher, throughput, validator, sink, metrics, logger, diagTimer };
}

async function main() {
  const { source, enricher, throughput, validator, sink, metrics, logger, diagTimer } = createProductionPipeline();
  logger.info('Starting production pipeline');

  try {
    await pipelineAsync(source, enricher, throughput, validator, sink);
    logger.info('Pipeline completed');
  } catch (err) {
    const d = StreamErrorDiagnostic.analyze(err, enricher);
    logger.error('Pipeline failed', { diagnosis: StreamErrorDiagnostic.format(d) });
  } finally {
    clearInterval(diagTimer);
    logger.info('Final metrics', metrics.getMetrics());
    metrics.destroy();
  }
}

main().catch(console.error);
```

## Best Practices Checklist

- [ ] Log structured JSON with pipeline context (stage, record count, timestamps)
- [ ] Monitor throughput at each pipeline stage to identify bottlenecks
- [ ] Set idle timeout alerts for stages that go silent unexpectedly
- [ ] Use `inspectStream()` to snapshot stream state during incidents
- [ ] Classify errors with codes for automated response routing
- [ ] Collect metrics without impacting throughput (sample, don't instrument every record)
- [ ] Maintain a runbook mapping error codes to remediation steps
- [ ] Profile pipeline stages individually before deploying the full pipeline
- [ ] Set `highWaterMark` values appropriate to your memory budget
- [ ] Use `performance.now()` for sub-millisecond timing of transform operations

## Cross-References

- [Memory Leak Detection](./02-memory-leak-detection.md) — detecting memory-related pipeline issues
- [Circuit Breaker and Error Recovery](../07-stream-error-handling/02-circuit-breaker-recovery.md) — automated error recovery
- [Graceful Degradation](../07-stream-error-handling/03-graceful-degradation.md) — health probes and shutdown handling
- [Stream Partitioning and Batching](../06-stream-concurrency-parallelism/03-stream-partitioning-batching.md) — batch-level performance analysis

## Next Steps

- Integrate stream metrics with Prometheus/Grafana for real-time dashboards
- Build an interactive CLI that connects to a running pipeline via IPC for live inspection
- Implement automated anomaly detection on throughput metrics to trigger alerts
