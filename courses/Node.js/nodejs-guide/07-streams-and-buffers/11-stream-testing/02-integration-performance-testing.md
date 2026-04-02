# Integration and Performance Testing for Streams

## What You'll Learn

- Pipeline integration testing with real file I/O
- End-to-end stream pipeline testing
- Performance testing with throughput assertions
- Load testing stream pipelines
- Stream throughput benchmarking framework
- Memory usage testing during stream operations
- Backpressure testing and validation
- CI/CD integration with GitHub Actions for stream tests
- Real-world example: testing a CSV-to-JSON pipeline end-to-end

## Pipeline Integration Testing with Real File I/O

```javascript
import { describe, it, before, after } from 'node:test';
import assert from 'node:assert/strict';
import { createReadStream, createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';
import { readFile, writeFile, mkdtemp, rm } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { Transform } from 'node:stream';

describe('File Pipeline Integration', () => {
    let tempDir;

    before(async () => { tempDir = await mkdtemp(join(tmpdir(), 'stream-test-')); });
    after(async () => { await rm(tempDir, { recursive: true, force: true }); });

    it('should transform file contents through pipeline', async () => {
        const inputPath = join(tempDir, 'input.txt');
        const outputPath = join(tempDir, 'output.txt');
        await writeFile(inputPath, 'hello world\nfoo bar\n');

        const upperCase = new Transform({
            transform(chunk, encoding, callback) { callback(null, chunk.toString().toUpperCase()); }
        });

        await pipeline(createReadStream(inputPath), upperCase, createWriteStream(outputPath));
        assert.equal(await readFile(outputPath, 'utf-8'), 'HELLO WORLD\nFOO BAR\n');
    });

    it('should handle binary file transformation', async () => {
        const inputPath = join(tempDir, 'input.bin');
        const outputPath = join(tempDir, 'output.bin');
        await writeFile(inputPath, Buffer.from([0x01, 0x02, 0x03]));

        const invert = new Transform({
            transform(chunk, encoding, callback) {
                const buf = Buffer.alloc(chunk.length);
                for (let i = 0; i < chunk.length; i++) buf[i] = chunk[i] ^ 0xFF;
                callback(null, buf);
            }
        });

        await pipeline(createReadStream(inputPath), invert, createWriteStream(outputPath));
        assert.deepEqual(await readFile(outputPath), Buffer.from([0xFE, 0xFD, 0xFC]));
    });
});
```

## End-to-End Stream Pipeline Testing

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class PipelineTestHarness {
    static createSource(data) {
        let i = 0;
        return new Readable({ objectMode: true, read() { this.push(i < data.length ? data[i++] : null); } });
    }

    static createSink() {
        const results = [];
        return {
            stream: new Writable({ objectMode: true, write(chunk, enc, cb) { results.push(chunk); cb(); } }),
            results
        };
    }

    static async runPipeline(source, ...transforms) {
        const { stream: sink, results } = this.createSink();
        await pipeline(source, ...transforms, sink);
        return results;
    }
}

describe('End-to-End Pipeline', () => {
    it('should process data through multiple transforms', async () => {
        const trim = new Transform({ objectMode: true, transform(c, e, cb) { cb(null, c.trim()); } });
        const split = new Transform({
            objectMode: true,
            transform(c, e, cb) { for (const w of c.split(/\s+/)) if (w) this.push(w); cb(); }
        });
        const dedupe = new Transform({
            objectMode: true,
            transform(c, e, cb) {
                if (!this._seen) this._seen = new Set();
                if (!this._seen.has(c)) { this._seen.add(c); this.push(c); }
                cb();
            }
        });

        const source = PipelineTestHarness.createSource(['hello world hello', 'foo bar foo', 'hello baz']);
        const results = await PipelineTestHarness.runPipeline(source, trim, split, dedupe);
        assert.deepEqual(results, ['hello', 'world', 'foo', 'bar', 'baz']);
    });

    it('should enrich data through transforms', async () => {
        const enricher = new Transform({
            objectMode: true,
            transform(c, e, cb) { cb(null, { ...c, enriched: true, ts: Date.now() }); }
        });

        const source = PipelineTestHarness.createSource([{ v: 1 }, { v: 2 }]);
        const results = await PipelineTestHarness.runPipeline(source, enricher);
        assert.equal(results.length, 2);
        assert.ok(results.every(r => r.enriched));
    });
});
```

## Performance Testing with Throughput Assertions

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { performance } from 'node:perf_hooks';
import { Readable, Transform, PassThrough } from 'node:stream';
import { pipeline } from 'node:stream/promises';

async function measureThroughput(factory, options = {}) {
    const chunkSize = options.chunkSize ?? 64 * 1024;
    const totalBytes = options.totalBytes ?? 50 * 1024 * 1024;
    const iterations = options.iterations ?? 3;
    const samples = [];

    for (let i = 0; i < iterations; i++) {
        const memBefore = process.memoryUsage();
        const start = performance.now();
        let bytes = 0;
        const source = new Readable({
            read() { this.push(Buffer.alloc(chunkSize, 0xAA)); bytes += chunkSize; if (bytes >= totalBytes) this.push(null); }
        });
        await pipeline(source, factory(), new PassThrough());
        const elapsed = performance.now() - start;
        samples.push({
            throughputMBs: (totalBytes / (1024 * 1024)) / (elapsed / 1000),
            heapDeltaMB: (process.memoryUsage().heapUsed - memBefore.heapUsed) / (1024 * 1024)
        });
    }

    return { samples, avgThroughput: +(samples.reduce((s, r) => s + r.throughputMBs, 0) / samples.length).toFixed(2) };
}

describe('Throughput Assertions', () => {
    it('pass-through should exceed 500 MB/s', async () => {
        const { avgThroughput } = await measureThroughput(() => new PassThrough(), {
            totalBytes: 100 * 1024 * 1024, iterations: 3
        });
        assert.ok(avgThroughput > 500, `Expected >500 MB/s, got ${avgThroughput}`);
    });

    it('chunk transform should exceed 100 MB/s', async () => {
        const { avgThroughput } = await measureThroughput(
            () => new Transform({ transform(c, e, cb) { const b = Buffer.from(c); b.reverse(); cb(null, b); } }),
            { totalBytes: 50 * 1024 * 1024 }
        );
        assert.ok(avgThroughput > 100, `Expected >100 MB/s, got ${avgThroughput}`);
    });
});
```

## Load Testing Stream Pipelines

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { performance } from 'node:perf_hooks';

class LoadTester {
    constructor(options = {}) {
        this.concurrency = options.concurrency ?? 10;
        this.durationMs = options.durationMs ?? 5000;
        this.chunkSize = options.chunkSize ?? 1024;
    }

    async run(factory) {
        const start = performance.now();
        const outcomes = await Promise.allSettled(
            Array.from({ length: this.concurrency }, () => this._run(factory, start))
        );
        return {
            concurrency: this.concurrency,
            succeeded: outcomes.filter(o => o.status === 'fulfilled').length,
            failed: outcomes.filter(o => o.status === 'rejected').length,
            errors: outcomes.filter(o => o.status === 'rejected').map(o => o.reason.message)
        };
    }

    async _run(factory, globalStart) {
        const src = new Readable({
            read() {
                if (performance.now() - globalStart > this.durationMs) return this.push(null);
                this.push(Buffer.alloc(this.chunkSize, 0x42));
            }
        });
        await pipeline(src, factory(), new Writable({ write(c, e, cb) { cb(); } }));
    }
}

describe('Load Testing', () => {
    it('should handle 20 concurrent pipelines', async () => {
        const t = new LoadTester({ concurrency: 20, durationMs: 2000 });
        const r = await t.run(() => new Transform({ transform(c, e, cb) { cb(null, c); } }));
        assert.equal(r.failed, 0, `Failures: ${r.errors.join(', ')}`);
    });

    it('should degrade gracefully under pressure', async () => {
        const t = new LoadTester({ concurrency: 50, durationMs: 2000, chunkSize: 64 * 1024 });
        const r = await t.run(() => new Transform({
            highWaterMark: 16, transform(c, e, cb) { setTimeout(() => cb(null, c), 1); }
        }));
        assert.ok(r.succeeded / r.concurrency >= 0.8);
    });
});
```

## Stream Throughput Benchmarking Framework

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { performance, monitorEventLoopDelay } from 'node:perf_hooks';
import { Readable, PassThrough } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class StreamBenchmark {
    static async run({ name = 'unnamed', factory, chunkSizes = [4096, 65536], totalBytes = 25 * 1024 * 1024, iterations = 3 }) {
        const reports = [];
        for (const chunkSize of chunkSizes) {
            const samples = [];
            for (let i = 0; i < iterations; i++) {
                let bytes = 0;
                const src = new Readable({
                    read() { this.push(Buffer.alloc(chunkSize)); bytes += chunkSize; if (bytes >= totalBytes) this.push(null); }
                });
                const mon = monitorEventLoopDelay({ resolution: 10 }); mon.enable();
                const start = performance.now();
                await pipeline(src, factory(), new PassThrough());
                samples.push({ throughputMBs: (totalBytes / (1024 * 1024)) / ((performance.now() - start) / 1000), elMean: mon.mean });
                mon.disable();
            }
            reports.push({ name, chunkSize, avg: +(samples.reduce((s, r) => s + r.throughputMBs, 0) / samples.length).toFixed(2) });
        }
        return reports;
    }
}

describe('Benchmark Framework', () => {
    it('should produce report across chunk sizes', async () => {
        const reports = await StreamBenchmark.run({
            factory: () => new PassThrough(), chunkSizes: [4096, 65536], totalBytes: 10 * 1024 * 1024, iterations: 2
        });
        assert.equal(reports.length, 2);
        for (const r of reports) assert.ok(r.avg > 0);
    });
});
```

## Memory Usage and Backpressure Testing

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { performance } from 'node:perf_hooks';

describe('Memory Usage Testing', () => {
    it('should not leak memory during large stream processing', async () => {
        const samples = [];
        const iv = setInterval(() => samples.push(process.memoryUsage().heapUsed), 200);
        let bytes = 0;
        const src = new Readable({
            read() { this.push(Buffer.alloc(64 * 1024)); bytes += 64 * 1024; if (bytes >= 100 * 1024 * 1024) this.push(null); }
        });
        await pipeline(src, new Transform({ objectMode: true, transform(c, e, cb) { cb(null, { s: c.length }); } }),
            new Writable({ objectMode: true, write(c, e, cb) { cb(); } }));
        clearInterval(iv);
        assert.ok((samples.at(-1) - samples[0]) / (1024 * 1024) < 50, 'Memory leak detected');
    });
});

describe('Backpressure Validation', () => {
    it('should slow producer when consumer is slow', async () => {
        const times = [];
        let n = 0;
        await pipeline(
            new Readable({ highWaterMark: 4, read() { n++; if (n > 20) this.push(null); else this.push(Buffer.alloc(1024)); } }),
            new Writable({ highWaterMark: 4, write(c, e, cb) { times.push(performance.now()); setTimeout(cb, 50); } })
        );
        for (let i = 1; i < times.length; i++) assert.ok(times[i] - times[i - 1] >= 30, 'Backpressure not applied');
    });

    it('should respect highWaterMark on transform', async () => {
        let n = 0;
        await pipeline(
            new Readable({ read() { n++; if (n > 50) this.push(null); else this.push(Buffer.alloc(4096)); } }),
            new Transform({ highWaterMark: 8, transform(c, e, cb) { setTimeout(() => cb(null, c), 10); } }),
            new Writable({ write(c, e, cb) { cb(); } })
        );
        assert.ok(n <= 55, `Source read ${n} times — backpressure not respected`);
    });
});
```

## CI/CD Integration with GitHub Actions

```yaml
# .github/workflows/stream-tests.yml
name: Stream Tests
on:
  push: { branches: [main] }
  pull_request: { branches: [main] }

jobs:
  stream-tests:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        node-version: [20.x, 22.x]
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '${{ matrix.node-version }}' }
      - run: npm ci
      - name: Run stream tests
        run: node --test --test-name-pattern="Stream" tests/
      - name: Run performance benchmarks
        run: node --test --test-name-pattern="Benchmark|Throughput" tests/performance/
      - name: Upload results
        if: github.ref == 'refs/heads/main'
        uses: actions/upload-artifact@v4
        with: { name: 'benchmark-${{ matrix.node-version }}', path: benchmark-results/ }
```

```javascript
// tests/performance/stream-bench-ci.test.js
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { mkdir, writeFile } from 'node:fs/promises';
import { join } from 'node:path';
import { pipeline } from 'node:stream/promises';
import { Readable, PassThrough, Transform } from 'node:stream';

async function ciBench(name, factory) {
    let bytes = 0;
    const src = new Readable({
        read() { this.push(Buffer.alloc(16384)); bytes += 16384; if (bytes >= 10 * 1024 * 1024) this.push(null); }
    });
    const start = performance.now();
    await pipeline(src, factory(), new PassThrough());
    const tps = (10 / ((performance.now() - start) / 1000));
    const dir = join(process.cwd(), 'benchmark-results');
    await mkdir(dir, { recursive: true });
    await writeFile(join(dir, `${name}.json`), JSON.stringify({ name, throughputMBs: +tps.toFixed(2) }, null, 2));
    return tps;
}

describe('CI Benchmark Suite', () => {
    it('passthrough baseline', async () => {
        assert.ok(await ciBench('passthrough', () => new PassThrough()) > 100);
    });

    it('transform overhead', async () => {
        assert.ok(await ciBench('identity', () => new Transform({ transform(c, e, cb) { cb(null, c); } })) > 50);
    });
});
```

## Real-World Example: CSV-to-JSON Pipeline E2E

```javascript
import { describe, it, before, after } from 'node:test';
import assert from 'node:assert/strict';
import { createReadStream, createWriteStream } from 'node:fs';
import { writeFile, readFile, mkdtemp, rm } from 'node:fs/promises';
import { join } from 'node:path';
import { tmpdir } from 'node:os';
import { Transform, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createInterface } from 'node:readline';

function csvToJson() {
    let headers = null;
    return new Transform({
        objectMode: true,
        transform(chunk, enc, cb) {
            const line = chunk.toString().trim();
            if (!line) return cb();
            if (!headers) { headers = line.split(',').map(h => h.trim()); return cb(); }
            const vals = line.split(',').map(v => v.trim());
            const rec = {};
            headers.forEach((h, i) => { const v = vals[i] ?? ''; rec[h] = isNaN(v) ? v : Number(v); });
            this.push(rec);
            cb();
        }
    });
}

describe('CSV-to-JSON Pipeline E2E', () => {
    let tempDir;
    const csv = ['id,name,dept,salary', '1,Alice,Eng,95000', '2,Bob,Mkt,72000', '3,Carol,Eng,105000'].join('\n');

    before(async () => { tempDir = await mkdtemp(join(tmpdir(), 'csv-')); await writeFile(join(tempDir, 'data.csv'), csv); });
    after(async () => { await rm(tempDir, { recursive: true, force: true }); });

    it('should convert CSV to JSON objects', async () => {
        const recs = [];
        await pipeline(createInterface({ input: createReadStream(join(tempDir, 'data.csv')) }),
            csvToJson(), new Writable({ objectMode: true, write(c, e, cb) { recs.push(c); cb(); } }));
        assert.equal(recs.length, 3);
        assert.deepEqual(recs[0], { id: 1, name: 'Alice', dept: 'Eng', salary: 95000 });
        assert.equal(typeof recs[0].salary, 'number');
    });

    it('should aggregate by department and write output', async () => {
        const agg = new Transform({
            objectMode: true,
            transform(c, e, cb) {
                if (!this._a) this._a = {};
                if (!this._a[c.dept]) this._a[c.dept] = { n: 0, s: 0 };
                this._a[c.dept].n++; this._a[c.dept].s += c.salary; cb();
            },
            flush(cb) {
                for (const [d, v] of Object.entries(this._a))
                    this.push({ dept: d, count: v.n, avgSalary: Math.round(v.s / v.n) });
                cb();
            }
        });

        const stringify = new Transform({ objectMode: true, transform(c, e, cb) { cb(null, JSON.stringify(c) + '\n'); } });
        const out = join(tempDir, 'summary.jsonl');
        await pipeline(createInterface({ input: createReadStream(join(tempDir, 'data.csv')) }),
            csvToJson(), agg, stringify, createWriteStream(out));

        const lines = (await readFile(out, 'utf-8')).trim().split('\n');
        const eng = JSON.parse(lines.find(l => l.includes('"Eng"')));
        assert.equal(eng.count, 2);
        assert.equal(eng.avgSalary, 100000);
    });
});
```

## Best Practices Checklist

- [ ] Test pipelines with real filesystem operations using temp directories
- [ ] Assert throughput meets minimum thresholds for each transform
- [ ] Validate memory stays bounded during long-running stream operations
- [ ] Test backpressure by using slow consumers with fast producers
- [ ] Include load tests with concurrent pipeline execution
- [ ] Run benchmarks across multiple chunk sizes
- [ ] Persist benchmark results for regression detection in CI
- [ ] Clean up temp files in `after` hooks to prevent test pollution
- [ ] Test both object-mode and binary-mode stream pipelines
- [ ] Include event-loop delay monitoring in performance tests

## Cross-References

- See [Unit Testing](./01-unit-testing.md) for foundational stream test patterns
- See [Performance Optimization](../08-stream-performance-optimization/01-profiling-memory.md) for profiling techniques
- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for pipeline error patterns
- See [Backpressure](../01-streams-architecture/02-backpressure-performance.md) for backpressure internals

## Next Steps

Continue to [Advanced Testing Patterns](./03-advanced-testing-patterns.md).
