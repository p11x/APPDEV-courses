# Advanced Stream Testing Patterns

## What You'll Learn

- Mock and stub streams for isolated testing
- Property-based testing with fast-check for streams
- Snapshot testing for stream output
- Chaos testing: injecting failures into stream pipelines
- Error path testing and recovery validation
- Stream testing with fake timers
- TestStreamFactory utility class
- Testing stream error boundaries
- Real-world example: testing a resilient data pipeline with fault injection

## Mock and Stub Streams for Testing

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class MockReadable extends Readable {
    constructor(items, options = {}) {
        super({ objectMode: options.objectMode ?? true });
        this._items = [...items];
        this._delayMs = options.delayMs ?? 0;
    }

    _read() {
        if (this._items.length === 0) return this.push(null);
        const item = this._items.shift();
        if (this._delayMs > 0) setTimeout(() => this.push(item), this._delayMs);
        else this.push(item);
    }
}

class StubWritable extends Writable {
    constructor(options = {}) {
        super({ objectMode: options.objectMode ?? true });
        this.chunks = [];
        this.writeCount = 0;
        this.flushed = false;
    }

    _write(chunk, enc, cb) { this.writeCount++; this.chunks.push(chunk); cb(); }
    _final(cb) { this.flushed = true; cb(); }
}

class SpyTransform extends Transform {
    constructor(fn, options = {}) {
        super({ objectMode: options.objectMode ?? true });
        this._fn = fn;
        this.transformCalls = [];
        this.flushCalled = false;
    }

    _transform(chunk, enc, cb) {
        this.transformCalls.push({ chunk, encoding: enc });
        const result = this._fn(chunk);
        if (Array.isArray(result)) { result.forEach(r => this.push(r)); cb(); }
        else cb(null, result);
    }

    _flush(cb) { this.flushCalled = true; cb(); }
}

describe('Mock and Stub Streams', () => {
    it('MockReadable emits provided items', async () => {
        const source = new MockReadable(['a', 'b', 'c']);
        const sink = new StubWritable();
        await pipeline(source, sink);
        assert.deepEqual(sink.chunks, ['a', 'b', 'c']);
        assert.equal(sink.writeCount, 3);
    });

    it('SpyTransform tracks invocations', async () => {
        const doubler = new SpyTransform(c => ({ ...c, v: c.v * 2 }));
        const source = new MockReadable([{ v: 1 }, { v: 2 }]);
        const sink = new StubWritable();
        await pipeline(source, doubler, sink);
        assert.equal(doubler.transformCalls.length, 2);
        assert.ok(doubler.flushCalled);
        assert.deepEqual(sink.chunks, [{ v: 2 }, { v: 4 }]);
    });

    it('StubWritable records all writes', async () => {
        const source = new MockReadable(['x', 'y']);
        const upper = new Transform({
            objectMode: true,
            transform(chunk, enc, cb) { cb(null, chunk.toUpperCase()); }
        });
        const sink = new StubWritable();
        await pipeline(source, upper, sink);
        assert.deepEqual(sink.chunks, ['X', 'Y']);
    });
});
```

## Property-Based Testing with fast-check

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import fc from 'fast-check';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

async function collect(readable) {
    const items = [];
    for await (const chunk of readable) items.push(chunk);
    return items;
}

function fromArray(arr) {
    let i = 0;
    return new Readable({ objectMode: true, read() { this.push(i < arr.length ? arr[i++] : null); } });
}

describe('Property-Based Stream Testing', () => {
    it('identity transform preserves all items', async () => {
        await fc.assert(
            fc.asyncProperty(fc.array(fc.integer(), { maxLength: 200 }), async (items) => {
                const identity = new Transform({
                    objectMode: true,
                    transform(chunk, enc, cb) { cb(null, chunk); }
                });
                const result = await collect(fromArray(items).pipe(identity));
                assert.deepEqual(result, items);
            }),
            { numRuns: 50 }
        );
    });

    it('sorting transform produces ordered output', async () => {
        await fc.assert(
            fc.asyncProperty(
                fc.array(fc.integer(), { minLength: 1, maxLength: 100 }),
                async (items) => {
                    const sorter = (() => {
                        const buf = [];
                        return new Transform({
                            objectMode: true,
                            transform(chunk, enc, cb) { buf.push(chunk); cb(); },
                            flush(cb) { buf.sort((a, b) => a - b); buf.forEach(x => this.push(x)); cb(); }
                        });
                    })();
                    const result = await collect(fromArray(items).pipe(sorter));
                    for (let i = 1; i < result.length; i++) assert.ok(result[i] >= result[i - 1]);
                    assert.equal(result.length, items.length);
                }
            ),
            { numRuns: 50 }
        );
    });

    it('filter transform never produces excluded items', async () => {
        await fc.assert(
            fc.asyncProperty(
                fc.array(fc.integer({ min: 0, max: 100 }), { maxLength: 200 }),
                async (items) => {
                    const filter = new Transform({
                        objectMode: true,
                        transform(chunk, enc, cb) { if (chunk >= 50) this.push(chunk); cb(); }
                    });
                    const result = await collect(fromArray(items).pipe(filter));
                    assert.ok(result.every(v => v >= 50));
                    assert.deepEqual(result, items.filter(v => v >= 50));
                }
            ),
            { numRuns: 50 }
        );
    });

    it('chained transforms preserve item count', async () => {
        await fc.assert(
            fc.asyncProperty(
                fc.array(fc.string({ maxLength: 10 }), { maxLength: 100 }),
                async (items) => {
                    const upper = new Transform({ objectMode: true, transform(c, e, cb) { cb(null, c.toUpperCase()); } });
                    const trim = new Transform({ objectMode: true, transform(c, e, cb) { cb(null, c.trim()); } });
                    const result = await collect(fromArray(items).pipe(upper).pipe(trim));
                    assert.equal(result.length, items.length);
                    assert.ok(result.every(v => v === v.toUpperCase()));
                }
            ),
            { numRuns: 30 }
        );
    });
});
```

## Snapshot Testing for Stream Output

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { readFile, writeFile, mkdir } from 'node:fs/promises';
import { join } from 'node:path';
import { Readable, Transform, PassThrough } from 'node:stream';

const SNAPSHOT_DIR = join(import.meta.dirname, '__snapshots__');

async function streamToBuffer(readable) {
    const chunks = [];
    for await (const chunk of readable) chunks.push(Buffer.isBuffer(chunk) ? chunk : Buffer.from(chunk));
    return Buffer.concat(chunks);
}

async function assertStreamSnapshot(stream, name) {
    await mkdir(SNAPSHOT_DIR, { recursive: true });
    const path = join(SNAPSHOT_DIR, `${name}.snap`);
    const tee = new PassThrough();
    stream.pipe(tee);
    const actual = await streamToBuffer(tee);

    try {
        const expected = await readFile(path);
        assert.bufferEqual(actual, expected, `Snapshot mismatch: ${name}`);
    } catch (err) {
        if (err.code === 'ENOENT') { await writeFile(path, actual); console.log(`Created snapshot: ${name}`); }
        else throw err;
    }
}

describe('Snapshot Testing', () => {
    it('should match snapshot for CSV header generation', async () => {
        const gen = new Transform({
            transform(chunk, enc, cb) {
                cb(null, chunk.toString().split(',').map(f => f.toUpperCase().trim()).join(' | ') + '\n');
            }
        });
        const src = new Readable({ read() { this.push('name,age,city'); this.push(null); } });
        await assertStreamSnapshot(src.pipe(gen), 'csv-header');
    });

    it('should match snapshot for formatted JSON', async () => {
        const pretty = new Transform({
            objectMode: true,
            transform(chunk, enc, cb) { cb(null, JSON.stringify(chunk, null, 2) + '\n---\n'); }
        });
        const src = new Readable({ objectMode: true, read() { this.push({ users: 42 }); this.push(null); } });
        await assertStreamSnapshot(src.pipe(pretty), 'json-output');
    });
});
```

## Chaos Testing: Injecting Failures into Stream Pipelines

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class ChaosTransform extends Transform {
    constructor(options = {}) {
        super({ objectMode: options.objectMode ?? true });
        this._failureRate = options.failureRate ?? 0.1;
        this._delayMs = options.delayRangeMs ?? [0, 100];
        this._dropRate = options.dropRate ?? 0;
        this._corruptRate = options.corruptRate ?? 0;
        this._errors = options.errorMessages ?? ['Chaos: random failure'];
    }

    _transform(chunk, enc, cb) {
        if (Math.random() < this._dropRate) return cb();
        if (Math.random() < this._failureRate) {
            return cb(new Error(this._errors[Math.floor(Math.random() * this._errors.length)]));
        }
        let result = chunk;
        if (Math.random() < this._corruptRate && typeof chunk === 'object') result = { ...chunk, _corrupted: true };
        const delay = this._delayMs[0] + Math.random() * (this._delayMs[1] - this._delayMs[0]);
        if (delay > 0) setTimeout(() => cb(null, result), delay);
        else cb(null, result);
    }
}

function createSource(items) { let i = 0; return new Readable({ objectMode: true, read() { this.push(i < items.length ? items[i++] : null); } }); }
function createCollector() { const c = []; return { stream: new Writable({ objectMode: true, write(chunk, enc, cb) { c.push(chunk); cb(); } }), collected: c }; }

describe('Chaos Testing', () => {
    it('zero-failure chaos preserves all data', async () => {
        const items = Array.from({ length: 100 }, (_, i) => ({ id: i }));
        const chaos = new ChaosTransform({ failureRate: 0, dropRate: 0, corruptRate: 0 });
        const { stream: sink, collected } = createCollector();
        await pipeline(createSource(items), chaos, sink);
        assert.equal(collected.length, 100);
    });

    it('deterministic failure propagates correctly', async () => {
        const chaos = new ChaosTransform({ failureRate: 1.0, errorMessages: ['Deterministic failure'] });
        await assert.rejects(
            () => pipeline(createSource([{ id: 1 }]), chaos, createCollector().stream),
            { message: 'Deterministic failure' }
        );
    });

    it('retry recovers from intermittent failures', async () => {
        async function resilient(items, maxRetries = 5) {
            for (let retry = 0; retry <= maxRetries; retry++) {
                const chaos = new ChaosTransform({ failureRate: retry < 2 ? 0.5 : 0 });
                const { stream: sink, collected } = createCollector();
                try {
                    await pipeline(createSource(items), chaos, sink);
                    return { collected, attempts: retry + 1 };
                } catch { if (retry === maxRetries) throw new Error('Max retries exceeded'); }
            }
        }
        const items = Array.from({ length: 10 }, (_, i) => ({ id: i }));
        const result = await resilient(items);
        assert.ok(result.attempts <= 5);
    });

    it('detects data corruption via chaos injection', async () => {
        const items = Array.from({ length: 50 }, (_, i) => ({ id: i }));
        const chaos = new ChaosTransform({ failureRate: 0, dropRate: 0, corruptRate: 0.3 });
        const corrupted = [];
        const checker = new Writable({ objectMode: true, write(chunk, enc, cb) { if (chunk._corrupted) corrupted.push(chunk); cb(); } });
        await pipeline(createSource(items), chaos, checker);
        assert.ok(corrupted.length > 0);
        assert.ok(corrupted.length <= items.length * 0.5);
    });
});
```

## Error Path Testing and Recovery Validation

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

function createFailingTransform(failAfterN) {
    let count = 0;
    return new Transform({
        objectMode: true,
        transform(chunk, enc, cb) {
            count++;
            if (count > failAfterN) cb(new Error(`Failed after ${failAfterN}`));
            else cb(null, chunk);
        }
    });
}

describe('Error Path Testing', () => {
    it('should capture errors mid-pipeline', async () => {
        let i = 0;
        const src = new Readable({ objectMode: true, read() { if (i < 10) this.push({ id: ++i }); else this.push(null); } });
        const collected = [];
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { collected.push(chunk); cb(); } });

        await assert.rejects(() => pipeline(src, createFailingTransform(5), sink), { message: 'Failed after 5' });
        assert.equal(collected.length, 5);
    });

    it('should clean up resources on error', async () => {
        let allocated = 0, freed = 0;
        const tracker = new Transform({
            objectMode: true,
            construct(cb) { allocated++; cb(); },
            transform(chunk, enc, cb) { if (chunk.id === 3) cb(new Error('fail')); else cb(null, chunk); },
            destroy(err, cb) { freed++; cb(err); }
        });

        let i = 0;
        const src = new Readable({ objectMode: true, read() { if (i < 5) this.push({ id: ++i }); else this.push(null); } });
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { cb(); } });

        try { await pipeline(src, tracker, sink); } catch { /* expected */ }
        assert.equal(allocated, 1);
        assert.equal(freed, 1);
    });

    it('should collect partial results before boundary failure', async () => {
        let count = 0;
        const flaky = new Transform({
            objectMode: true,
            transform(chunk, enc, cb) {
                count++;
                if (count > 5) cb(new Error('Circuit breaker'));
                else cb(null, chunk);
            }
        });

        let i = 0;
        const src = new Readable({ objectMode: true, read() { if (i < 10) this.push({ id: ++i }); else this.push(null); } });
        const collected = [];
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { collected.push(chunk); cb(); } });

        try { await pipeline(src, flaky, sink); } catch { /* expected */ }
        assert.ok(collected.length >= 5);
    });
});
```

## Stream Testing with Fake Timers

```javascript
import { describe, it, beforeEach } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class FakeClock {
    constructor() { this._now = 0; this._timers = []; this._id = 1; }
    now() { return this._now; }
    setTimeout(fn, delay) {
        const id = this._id++;
        this._timers.push({ id, fn, at: this._now + delay });
        this._timers.sort((a, b) => a.at - b.at);
        return id;
    }
    clearTimeout(id) { this._timers = this._timers.filter(t => t.id !== id); }
    advance(ms) {
        this._now += ms;
        while (this._timers.length && this._timers[0].at <= this._now) {
            const t = this._timers.shift();
            t.fn();
        }
    }
    flush() { while (this._timers.length) { this._now = this._timers[0].at; this._timers.shift().fn(); } }
}

function createTimedTransform(clock, delayMs) {
    return new Transform({
        objectMode: true,
        transform(chunk, enc, cb) {
            clock.setTimeout(() => cb(null, { ...chunk, ts: clock.now() }), delayMs);
        }
    });
}

describe('Fake Timer Stream Testing', () => {
    let clock;
    beforeEach(() => { clock = new FakeClock(); });

    it('should timestamp chunks with fake clock', async () => {
        const items = [{ id: 1 }, { id: 2 }, { id: 3 }];
        let i = 0;
        const src = new Readable({ objectMode: true, read() { this.push(i < items.length ? items[i++] : null); } });
        const collected = [];
        const sink = new Writable({
            objectMode: true,
            write(chunk, enc, cb) { collected.push(chunk); clock.advance(100); cb(); }
        });

        const p = pipeline(src, createTimedTransform(clock, 100), sink);
        clock.flush();
        await p;
        assert.equal(collected[0].ts, 100);
        assert.equal(collected[2].ts, 300);
    });

    it('should detect timeout with fake clock', async () => {
        class TimeoutDetector extends Transform {
            constructor(c, ms) {
                super({ objectMode: true });
                this._c = c; this._ms = ms; this._tid = null; this.timedOut = false;
            }
            _transform(chunk, enc, cb) {
                if (this._tid) this._c.clearTimeout(this._tid);
                this._tid = this._c.setTimeout(() => {
                    this.timedOut = true;
                    this.emit('error', new Error('timeout'));
                }, this._ms);
                cb(null, chunk);
            }
        }

        const det = new TimeoutDetector(clock, 1000);
        const src = new Readable({ objectMode: true, read() { this.push({ data: 1 }); this.push(null); } });
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { cb(); } });
        await pipeline(src, det, sink);
        clock.advance(1001);
        assert.ok(det.timedOut);
    });
});
```

## TestStreamFactory Utility Class

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform, PassThrough, Duplex } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class TestStreamFactory {
    static readable(data, options = {}) {
        const items = Array.isArray(data) ? data : [data];
        let i = 0;
        return new Readable({
            objectMode: options.objectMode ?? true,
            read() {
                if (i < items.length) this.push(items[i++]);
                else this.push(null);
            }
        });
    }

    static writable(onWrite) {
        const chunks = [];
        const stream = new Writable({
            objectMode: true,
            write(chunk, enc, cb) { chunks.push(chunk); if (onWrite) onWrite(chunk, chunks.length); cb(); }
        });
        stream.chunks = chunks;
        return stream;
    }

    static errorAfter(count, msg = `Error after ${count}`) {
        let n = 0;
        return new Transform({
            objectMode: true,
            transform(chunk, enc, cb) { n++; if (n > count) cb(new Error(msg)); else cb(null, chunk); }
        });
    }

    static slowStream(delayMs, options = {}) {
        return new Transform({
            objectMode: options.objectMode ?? true,
            transform(chunk, enc, cb) { setTimeout(() => cb(null, chunk), delayMs); }
        });
    }

    static generator(fn, total, options = {}) {
        let n = 0;
        return new Readable({
            objectMode: options.objectMode ?? true,
            read() { this.push(n < total ? fn(n++) : null); }
        });
    }

    static duplex(endOnEmpty = false) {
        const buf = [];
        return new Duplex({
            objectMode: true,
            read() { if (buf.length) this.push(buf.shift()); else if (endOnEmpty) this.push(null); },
            write(chunk, enc, cb) { buf.push(chunk); cb(); }
        });
    }

    static async collect(stream) {
        const items = [];
        for await (const chunk of stream) items.push(chunk);
        return items;
    }
}

describe('TestStreamFactory', () => {
    it('readable emits items then ends', async () => {
        assert.deepEqual(await TestStreamFactory.collect(TestStreamFactory.readable([1, 2, 3])), [1, 2, 3]);
    });

    it('writable collects chunks', async () => {
        const sink = TestStreamFactory.writable();
        await pipeline(TestStreamFactory.readable(['a', 'b']), sink);
        assert.deepEqual(sink.chunks, ['a', 'b']);
    });

    it('errorAfter fails at correct position', async () => {
        await assert.rejects(
            () => pipeline(TestStreamFactory.readable([1, 2, 3, 4, 5]), TestStreamFactory.errorAfter(3), TestStreamFactory.writable()),
            { message: 'Error after 3' }
        );
    });

    it('generator creates items on demand', async () => {
        const result = await TestStreamFactory.collect(TestStreamFactory.generator(i => ({ i, v: i * 10 }), 5));
        assert.deepEqual(result[4], { i: 4, v: 40 });
    });

    it('slowStream introduces delay', async () => {
        const start = performance.now();
        await pipeline(TestStreamFactory.readable([1, 2, 3]), TestStreamFactory.slowStream(20), TestStreamFactory.writable());
        assert.ok(performance.now() - start >= 60);
    });

    it('duplex supports read-your-writes', async () => {
        const d = TestStreamFactory.duplex(true);
        d.write('hello'); d.write('world'); d.end();
        assert.deepEqual(await TestStreamFactory.collect(d), ['hello', 'world']);
    });
});
```

## Testing Stream Error Boundaries

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class ErrorBoundaryTransform extends Transform {
    constructor(inner, options = {}) {
        super({ objectMode: options.objectMode ?? true });
        this._inner = inner;
        this._fallback = options.fallbackValue;
        this._maxErrors = options.maxErrors ?? Infinity;
        this.errors = [];
        this._count = 0;
    }

    _transform(chunk, enc, cb) {
        this._count++;
        this._inner.write(chunk, enc, (err) => {
            if (err) {
                this.errors.push({ index: this._count, chunk, error: err.message });
                if (this.errors.length > this._maxErrors) return cb(new Error(`Exceeded max errors (${this._maxErrors})`));
                if (this._fallback !== undefined) {
                    return cb(null, typeof this._fallback === 'function' ? this._fallback(chunk) : this._fallback);
                }
                cb();
            } else { this.push(chunk); cb(); }
        });
    }
}

describe('Error Boundary Testing', () => {
    it('swallows errors and continues with fallback', async () => {
        const flaky = new Transform({
            objectMode: true,
            transform(chunk, enc, cb) { if (chunk.id === 3) cb(new Error('Flaky')); else cb(null, chunk); }
        });

        const bounded = new ErrorBoundaryTransform(flaky, {
            fallbackValue: c => ({ ...c, fallback: true })
        });

        let i = 0;
        const src = new Readable({ objectMode: true, read() { if (i < 5) this.push({ id: ++i }); else this.push(null); } });
        const collected = [];
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { collected.push(chunk); cb(); } });

        await pipeline(src, bounded, sink);
        assert.equal(collected.length, 5);
        assert.ok(collected[2].fallback);
        assert.equal(bounded.errors.length, 1);
    });

    it('enforces max error threshold', async () => {
        const alwaysFail = new Transform({
            objectMode: true, transform(chunk, enc, cb) { cb(new Error('Always fails')); }
        });

        const bounded = new ErrorBoundaryTransform(alwaysFail, { maxErrors: 3 });
        let i = 0;
        const src = new Readable({ objectMode: true, read() { if (i < 10) this.push({ id: ++i }); else this.push(null); } });
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { cb(); } });

        await assert.rejects(() => pipeline(src, bounded, sink), { message: 'Exceeded max errors (3)' });
    });

    it('collects partial results before boundary failure', async () => {
        let n = 0;
        const failOn5 = new Transform({
            objectMode: true,
            transform(chunk, enc, cb) { n++; if (n > 5) cb(new Error('Circuit breaker')); else cb(null, chunk); }
        });

        const bounded = new ErrorBoundaryTransform(failOn5, { maxErrors: 2 });
        let i = 0;
        const src = new Readable({ objectMode: true, read() { if (i < 10) this.push({ id: ++i }); else this.push(null); } });
        const collected = [];
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { collected.push(chunk); cb(); } });

        try { await pipeline(src, bounded, sink); } catch { /* expected */ }
        assert.ok(collected.length >= 5);
    });
});
```

## Real-World Example: Resilient Data Pipeline with Fault Injection

```javascript
import { describe, it, beforeEach } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class FaultInjector {
    constructor(options = {}) {
        this._dropRate = options.networkDropRate ?? 0;
        this._parseRate = options.parseErrorRate ?? 0;
        this._latency = options.latencyMs ?? 0;
        this._patterns = options.errorPatterns ?? ['ECONNRESET', 'ETIMEDOUT'];
    }

    createInterceptor(name) {
        const self = this;
        let count = 0;
        return new Transform({
            objectMode: true,
            transform(chunk, enc, cb) {
                count++;
                if (Math.random() < self._dropRate)
                    return cb(new Error(self._patterns[Math.floor(Math.random() * self._patterns.length)]));
                if (Math.random() < self._parseRate)
                    return cb(new Error(`Parse error in ${name} at item ${count}`));
                if (self._latency > 0) setTimeout(() => cb(null, chunk), self._latency);
                else cb(null, chunk);
            }
        });
    }
}

class ResilientPipeline {
    constructor(options = {}) {
        this._maxRetries = options.maxRetries ?? 3;
        this._retryDelay = options.retryDelayMs ?? 10;
        this._deadLetter = [];
        this._metrics = { processed: 0, retried: 0, deadLettered: 0, errors: [] };
    }
    get deadLetter() { return this._deadLetter; }
    get metrics() { return this._metrics; }

    async createPipeline(source, factories, sink) {
        const transforms = factories.map(f => f());
        for (let retry = 0; retry <= this._maxRetries; retry++) {
            try {
                await pipeline(source, ...transforms, sink);
                this._metrics.processed++;
                return this._metrics;
            } catch (err) {
                this._metrics.errors.push(err.message);
                if (retry < this._maxRetries) {
                    this._metrics.retried++;
                    await new Promise(r => setTimeout(r, this._retryDelay));
                } else {
                    this._metrics.deadLettered++;
                    this._deadLetter.push({ error: err.message });
                }
            }
        }
        return this._metrics;
    }
}

describe('Resilient Data Pipeline E2E', () => {
    it('should process clean data without errors', async () => {
        const data = Array.from({ length: 20 }, (_, i) => ({ id: i, payload: `rec-${i}` }));
        let i = 0;
        const src = new Readable({ objectMode: true, read() { this.push(i < data.length ? data[i++] : null); } });
        const enricher = () => new Transform({
            objectMode: true,
            transform(chunk, enc, cb) { cb(null, { ...chunk, enriched: true }); }
        });
        const collected = [];
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { collected.push(chunk); cb(); } });

        const rp = new ResilientPipeline();
        const injector = new FaultInjector();
        await rp.createPipeline(src, [() => injector.createInterceptor('ingest'), enricher], sink);

        assert.equal(collected.length, 20);
        assert.ok(collected.every(r => r.enriched));
        assert.equal(rp.metrics.errors.length, 0);
    });

    it('should route parse errors to dead letter queue', async () => {
        const injector = new FaultInjector({ parseErrorRate: 1.0 });
        const src = new Readable({ objectMode: true, read() { this.push({ id: 1 }); this.push(null); } });
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { cb(); } });

        const rp = new ResilientPipeline({ maxRetries: 2, retryDelayMs: 1 });
        await rp.createPipeline(src, [() => injector.createInterceptor('parser')], sink);

        assert.ok(rp.metrics.deadLettered > 0);
        assert.ok(rp.deadLetter.length > 0);
    });

    it('should validate pipeline metrics under mixed faults', async () => {
        const injector = new FaultInjector({ networkDropRate: 0.3, latencyMs: 5 });
        const items = Array.from({ length: 10 }, (_, i) => ({ id: i }));
        let i = 0;
        const src = new Readable({ objectMode: true, read() { this.push(i < items.length ? items[i++] : null); } });
        const xform = () => new Transform({ objectMode: true, transform(chunk, enc, cb) { cb(null, { ...chunk, t: true }); } });
        const collected = [];
        const sink = new Writable({ objectMode: true, write(chunk, enc, cb) { collected.push(chunk); cb(); } });

        const rp = new ResilientPipeline({ maxRetries: 3, retryDelayMs: 2 });
        const m = await rp.createPipeline(src, [() => injector.createInterceptor('net'), xform], sink);

        assert.ok(m.processed + m.deadLettered > 0, 'Pipeline should process or dead-letter items');
    });
});
```

## Best Practices Checklist

- [ ] Use `MockReadable` and `StubWritable` instead of real I/O for unit-level stream tests
- [ ] Apply property-based testing with fast-check to verify invariants across arbitrary inputs
- [ ] Snapshot test stream output formats (JSON, CSV, headers) for regression detection
- [ ] Inject chaos (failures, delays, corruption) to validate pipeline resilience
- [ ] Test error boundaries to ensure partial failures don't crash entire pipelines
- [ ] Use fake timers to test timeout and latency-sensitive stream logic deterministically
- [ ] Centralize stream construction with `TestStreamFactory` for consistent test setup
- [ ] Verify dead-letter queues capture unprocessable items after retry exhaustion
- [ ] Assert resource cleanup (destroy hooks) fires on both success and failure paths
- [ ] Test that pipeline metrics accurately reflect processed, retried, and failed counts

## Cross-References

- See [Unit Testing](./01-unit-testing.md) for foundational stream test patterns and `TestStreamFactory` basics
- See [Integration & Performance Testing](./02-integration-performance-testing.md) for file I/O and throughput tests
- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for pipeline error propagation patterns
- See [Backpressure](../01-streams-architecture/02-backpressure-performance.md) for backpressure internals
- See [Concurrency](../06-stream-concurrency-parallelism/01-parallel-processing.md) for parallel stream testing
- See [Stream Debugging](../12-stream-debugging/01-debugging-tools.md) for debugging failing stream tests

## Next Steps

Continue to [Stream Debugging](../12-stream-debugging/01-debugging-tools.md).
