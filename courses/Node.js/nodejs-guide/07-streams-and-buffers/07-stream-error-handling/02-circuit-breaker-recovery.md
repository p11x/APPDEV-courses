# Circuit Breaker and Advanced Error Recovery Patterns

## What You'll Learn

- Circuit breaker pattern for stream pipelines (closed/open/half-open states)
- Error classification: transient vs permanent errors
- Dead letter queue stream for failed records with replay capability
- Checkpoint/resume pattern for long-running stream processing
- Retry with jitter and exponential backoff
- Error telemetry and reporting integration
- Real-world: resilient data pipeline with circuit breaker and checkpointing

## Circuit Breaker for Streams

Prevents cascading failures by stopping requests to a failing downstream after a failure threshold.

```
CLOSED ──(failures exceed threshold)──> OPEN ──(timeout expires)──> HALF-OPEN
   ^                                                                      |
   └──────────────(success in HALF-OPEN)──────────────────────────────────┘
   ^                                                                      |
   └──────────(failure in HALF-OPEN)──────> OPEN ─────────────────────────┘
```

```js
// circuit-breaker-transform.mjs
import { Transform } from 'node:stream';

const STATES = { CLOSED: 'CLOSED', OPEN: 'OPEN', HALF_OPEN: 'HALF_OPEN' };

class CircuitBreakerTransform extends Transform {
  #state = STATES.CLOSED;
  #failureCount = 0;
  #successCount = 0;
  #openedAt = null;
  #transformFn;
  #failureThreshold;
  #resetTimeoutMs;
  #halfOpenSuccesses;
  #onStateChange;

  constructor(transformFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#transformFn = transformFn;
    this.#failureThreshold = options.failureThreshold ?? 5;
    this.#resetTimeoutMs = options.resetTimeoutMs ?? 30000;
    this.#halfOpenSuccesses = options.halfOpenSuccesses ?? 3;
    this.#onStateChange = options.onStateChange ?? (() => {});
  }

  get state() { return this.#state; }

  #transition(newState) {
    const old = this.#state;
    this.#state = newState;
    this.#onStateChange({ from: old, to: newState, timestamp: Date.now() });
    if (newState === STATES.OPEN) { this.#openedAt = Date.now(); this.#failureCount = 0; }
    else if (newState === STATES.HALF_OPEN) { this.#successCount = 0; this.#failureCount = 0; }
    else { this.#failureCount = 0; this.#openedAt = null; }
  }

  async _transform(chunk, encoding, callback) {
    if (this.#state === STATES.OPEN) {
      if (Date.now() - this.#openedAt >= this.#resetTimeoutMs) this.#transition(STATES.HALF_OPEN);
      else return callback(new CircuitOpenError('Circuit breaker is open'));
    }

    try {
      const result = await this.#transformFn(chunk);
      if (this.#state === STATES.HALF_OPEN) {
        this.#successCount++;
        if (this.#successCount >= this.#halfOpenSuccesses) this.#transition(STATES.CLOSED);
      } else { this.#failureCount = 0; }
      this.push(result);
      callback();
    } catch (err) {
      this.#failureCount++;
      if (this.#state === STATES.HALF_OPEN) {
        this.#transition(STATES.OPEN);
        return callback(new CircuitOpenError('Failure during half-open probe'));
      }
      if (this.#failureCount >= this.#failureThreshold) this.#transition(STATES.OPEN);
      callback(err);
    }
  }

  _flush(callback) { callback(); }
}

class CircuitOpenError extends Error {
  constructor(msg) { super(msg); this.name = 'CircuitOpenError'; this.code = 'CIRCUIT_OPEN'; }
}

export { CircuitBreakerTransform, CircuitOpenError, STATES };
```

## Error Classification: Transient vs Permanent

```js
// error-classifier.mjs

class TransientError extends Error {
  constructor(msg, { statusCode } = {}) { super(msg); this.name = 'TransientError'; this.retryable = true; this.statusCode = statusCode; }
}

class PermanentError extends Error {
  constructor(msg, { statusCode, reason } = {}) { super(msg); this.name = 'PermanentError'; this.retryable = false; this.statusCode = statusCode; }
}

function classifyError(err) {
  if (['ECONNRESET', 'ETIMEDOUT', 'ECONNREFUSED', 'ENOTFOUND'].includes(err.code)) {
    return new TransientError(err.message, { statusCode: err.statusCode });
  }
  if (err.statusCode) {
    if (err.statusCode === 429 || err.statusCode >= 500) return new TransientError(err.message, { statusCode: err.statusCode });
    if (err.statusCode >= 400 && err.statusCode < 500) return new PermanentError(err.message, { statusCode: err.statusCode });
  }
  return new TransientError(err.message);
}

export { TransientError, PermanentError, classifyError };
```

## Retry with Jitter and Exponential Backoff

```js
// retry-transform.mjs
import { Transform } from 'node:stream';
import { classifyError, TransientError } from './error-classifier.mjs';

class RetryTransform extends Transform {
  #transformFn;
  #maxRetries;
  #baseDelayMs;
  #maxDelayMs;

  constructor(transformFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#transformFn = transformFn;
    this.#maxRetries = options.maxRetries ?? 5;
    this.#baseDelayMs = options.baseDelayMs ?? 100;
    this.#maxDelayMs = options.maxDelayMs ?? 30000;
  }

  #delay(attempt) {
    const exp = Math.min(this.#baseDelayMs * Math.pow(2, attempt), this.#maxDelayMs);
    return exp * (0.5 + Math.random() * 0.5);
  }

  async _transform(chunk, enc, callback) {
    let lastErr;
    for (let i = 0; i <= this.#maxRetries; i++) {
      try { this.push(await this.#transformFn(chunk)); return callback(); }
      catch (err) {
        lastErr = classifyError(err);
        if (!lastErr.retryable) return callback(lastErr);
        if (i < this.#maxRetries) await new Promise((r) => setTimeout(r, this.#delay(i)));
      }
    }
    callback(lastErr);
  }

  _flush(cb) { cb(); }
}

export { RetryTransform };
```

## Dead Letter Queue Stream

Failed records that exhaust retries go to a DLQ for inspection or replay.

```js
// dead-letter-queue.mjs
import { Transform } from 'node:stream';
import { appendFile, readFile, access } from 'node:fs/promises';

class DeadLetterQueue extends Transform {
  #dlqPath;
  #transformFn;
  #stats = { total: 0, failed: 0, deadLettered: 0 };

  constructor(transformFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#transformFn = transformFn;
    this.#dlqPath = options.dlqPath ?? './dead-letters.jsonl';
  }

  get stats() { return { ...this.#stats }; }

  async _transform(chunk, enc, callback) {
    this.#stats.total++;
    try { this.push(await this.#transformFn(chunk)); callback(); }
    catch (err) {
      this.#stats.failed++;
      this.#stats.deadLettered++;
      const entry = { timestamp: new Date().toISOString(), error: err.message, record: chunk };
      await appendFile(this.#dlqPath, JSON.stringify(entry) + '\n', 'utf-8').catch(() => {});
      callback(); // Continue processing
    }
  }

  _flush(cb) { cb(); }
}

async function* replayDeadLetters(dlqPath) {
  try { await access(dlqPath); } catch { return; }
  const content = await readFile(dlqPath, 'utf-8');
  for (const line of content.split('\n').filter(Boolean)) yield JSON.parse(line).record;
}

export { DeadLetterQueue, replayDeadLetters };
```

## Checkpoint/Resume Pattern

```js
// checkpoint-transform.mjs
import { Transform } from 'node:stream';
import { readFile, writeFile } from 'node:fs/promises';

class CheckpointTransform extends Transform {
  #checkpointPath;
  #transformFn;
  #processedCount = 0;
  #skipUntil = 0;
  #saveInterval;

  constructor(transformFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#transformFn = transformFn;
    this.#checkpointPath = options.checkpointPath ?? './checkpoint.json';
    this.#saveInterval = options.saveInterval ?? 100;
  }

  async initialize() {
    try {
      const data = JSON.parse(await readFile(this.#checkpointPath, 'utf-8'));
      this.#skipUntil = data.processedCount ?? 0;
      this.#processedCount = this.#skipUntil;
      this.emit('resume', { skipUntil: this.#skipUntil });
    } catch { /* no checkpoint */ }
  }

  async _transform(chunk, enc, callback) {
    if (this.#processedCount < this.#skipUntil) { this.#processedCount++; return callback(); }
    try {
      this.push(await this.#transformFn(chunk));
      this.#processedCount++;
      if (this.#processedCount % this.#saveInterval === 0) await this.#save();
      callback();
    } catch (err) { await this.#save(); callback(err); }
  }

  async #save() {
    await writeFile(this.#checkpointPath,
      JSON.stringify({ processedCount: this.#processedCount, savedAt: new Date().toISOString() }), 'utf-8');
  }

  async _flush(cb) { await this.#save(); cb(); }
}

export { CheckpointTransform };
```

## Error Telemetry Integration

```js
// error-telemetry.mjs
import { Transform } from 'node:stream';
import { EventEmitter } from 'node:events';

class ErrorTelemetryTransform extends Transform {
  #emitter;
  #transformFn;
  #pipelineId;

  constructor(transformFn, options = {}) {
    super({ objectMode: true, ...options });
    this.#transformFn = transformFn;
    this.#pipelineId = options.pipelineId ?? 'default';
    this.#emitter = options.emitter ?? new EventEmitter();
  }

  async _transform(chunk, enc, callback) {
    try { this.push(await this.#transformFn(chunk)); callback(); }
    catch (err) {
      this.#emitter.emit('stream:error', {
        pipelineId: this.#pipelineId, timestamp: Date.now(),
        error: { name: err.name, message: err.message, code: err.code },
        record: chunk, recoverable: err.retryable !== false,
      });
      callback(err);
    }
  }

  _flush(cb) { cb(); }
}

export { ErrorTelemetryTransform };
```

## Real-World: Resilient Data Pipeline

```js
// resilient-pipeline.mjs
import { Readable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { CircuitBreakerTransform } from './circuit-breaker-transform.mjs';
import { RetryTransform } from './retry-transform.mjs';
import { DeadLetterQueue } from './dead-letter-queue.mjs';
import { CheckpointTransform } from './checkpoint-transform.mjs';
import { classifyError } from './error-classifier.mjs';

async function processRecord(record) {
  const res = await fetch(`https://api.example.com/records/${record.id}`, {
    method: 'POST', headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(record),
  });
  if (!res.ok) { const e = new Error(`HTTP ${res.status}`); e.statusCode = res.status; throw e; }
  return { ...record, processed: true, at: Date.now() };
}

const source = Readable.from(Array.from({ length: 1000 }, (_, i) => ({ id: i, data: `record-${i}` })));

const cb = new CircuitBreakerTransform(processRecord, {
  failureThreshold: 3, resetTimeoutMs: 10000, halfOpenSuccesses: 2,
  onStateChange: ({ from, to }) => console.log(`Circuit: ${from} -> ${to}`),
});

const dlq = new DeadLetterQueue(async (r) => {
  try { return await cb._transformPromise?.(r) ?? r; }
  catch { throw classifyError(new Error('Processing failed')); }
}, { dlqPath: './pipeline-dlq.jsonl' });

const checkpoint = new CheckpointTransform(async (r) => { dlq.write(r); return r; },
  { checkpointPath: './checkpoint.json', saveInterval: 50 });

await checkpoint.initialize();
checkpoint.on('resume', ({ skipUntil }) => console.log(`Resuming from record ${skipUntil}`));

await pipeline(source, checkpoint, async function* (src) {
  let n = 0;
  for await (const _ of src) { n++; if (n % 100 === 0) console.log(`Progress: ${n}`); }
  console.log(`Done: ${n} records`);
});

console.log('DLQ stats:', dlq.stats);
```

## Best Practices Checklist

- [ ] Classify errors before deciding on retry vs dead-letter vs fail
- [ ] Use exponential backoff with jitter to prevent thundering herd
- [ ] Set circuit breaker thresholds based on your SLO error budgets
- [ ] Persist checkpoints to durable storage (filesystem, database, S3)
- [ ] Size dead letter queues to capture all failure modes without losing data
- [ ] Emit telemetry events for every circuit state transition
- [ ] Implement a `half-open` probe limit to avoid overwhelming recovering services
- [ ] Log circuit breaker state changes with timestamps for post-mortem analysis
- [ ] Test circuit breakers with fault injection (Chaos Engineering)
- [ ] Ensure `CheckpointTransform` skips already-processed records on resume

## Cross-References

- [Stream Error Handling Patterns](./01-error-patterns.md) — foundational error patterns
- [Graceful Degradation](./03-graceful-degradation.md) — graceful shutdown and fallback strategies
- [Worker Thread Streams](../06-stream-concurrency-parallelism/02-worker-thread-streams.md) — worker-based parallelism with error handling
- [Stream Debugging](../12-stream-debugging/01-debugging-tools.md) — debugging circuit breaker behavior

## Next Steps

- Implement adaptive circuit breaker thresholds based on error rate
- Build a replay tool that reads the DLQ and re-injects records into the pipeline
- Add metrics export (Prometheus, StatsD) for circuit breaker state and error rates
