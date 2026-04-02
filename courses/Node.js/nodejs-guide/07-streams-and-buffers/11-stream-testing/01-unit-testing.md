# Stream Testing: Unit, Integration, and Performance

## What You'll Learn

- Unit testing stream implementations
- Integration testing stream pipelines
- Performance testing streams
- Stream test utilities
- CI/CD stream testing

## Unit Testing Streams

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform, PassThrough } from 'node:stream';
import { pipeline } from 'node:stream/promises';

// Test helper: collect stream output
async function collectStream(stream) {
    const chunks = [];
    for await (const chunk of stream) {
        chunks.push(chunk);
    }
    return chunks;
}

// Test helper: create readable from array
function arrayToReadable(arr, objectMode = false) {
    let index = 0;
    return new Readable({
        objectMode,
        read() {
            if (index < arr.length) {
                this.push(arr[index++]);
            } else {
                this.push(null);
            }
        }
    });
}

describe('UpperCase Transform', () => {
    it('should transform text to uppercase', async () => {
        const upperCase = new Transform({
            transform(chunk, encoding, callback) {
                callback(null, chunk.toString().toUpperCase());
            }
        });

        const input = arrayToReadable(['hello ', 'world']);
        const output = await collectStream(input.pipe(upperCase));

        assert.equal(output.join(''), 'HELLO WORLD');
    });

    it('should handle empty input', async () => {
        const upperCase = new Transform({
            transform(chunk, encoding, callback) {
                callback(null, chunk.toString().toUpperCase());
            }
        });

        const input = arrayToReadable([]);
        const output = await collectStream(input.pipe(upperCase));

        assert.equal(output.length, 0);
    });

    it('should propagate errors', async () => {
        const errorStream = new Transform({
            transform(chunk, encoding, callback) {
                callback(new Error('Transform failed'));
            }
        });

        const input = arrayToReadable(['data']);
        
        await assert.rejects(
            () => pipeline(input, errorStream, new PassThrough()),
            { message: 'Transform failed' }
        );
    });
});
```

## Pipeline Integration Testing

```javascript
describe('CSV to JSON Pipeline', () => {
    it('should parse CSV and convert to JSON', async () => {
        const csvParser = new Transform({
            objectMode: true,
            transform(chunk, encoding, callback) {
                const lines = chunk.toString().split('\n');
                const headers = this.headers || lines[0].split(',');
                if (!this.headers) {
                    this.headers = headers;
                    return callback();
                }

                for (const line of lines.slice(1)) {
                    if (!line.trim()) continue;
                    const values = line.split(',');
                    const record = {};
                    headers.forEach((h, i) => record[h.trim()] = values[i]?.trim());
                    this.push(record);
                }
                callback();
            }
        });

        const csvData = 'name,age\nAlice,30\nBob,25\n';
        const input = arrayToReadable([csvData]);

        const results = await collectStream(input.pipe(csvParser));

        assert.equal(results.length, 2);
        assert.deepEqual(results[0], { name: 'Alice', age: '30' });
        assert.deepEqual(results[1], { name: 'Bob', age: '25' });
    });
});
```

## Performance Testing

```javascript
import { performance } from 'node:perf_hooks';
import { NullWritable } from 'node:stream';

async function benchmarkStream(transformFactory, options = {}) {
    const chunkSize = options.chunkSize || 64 * 1024;
    const totalBytes = options.totalBytes || 50 * 1024 * 1024;
    const iterations = options.iterations || 3;

    const results = [];

    for (let i = 0; i < iterations; i++) {
        const start = performance.now();

        const source = new Readable({
            read() {
                this.push(Buffer.alloc(chunkSize, 0xAA));
                if (this._count === undefined) this._count = 0;
                this._count += chunkSize;
                if (this._count >= totalBytes) this.push(null);
            }
        });

        const sink = new NullWritable();

        await pipeline(source, transformFactory(), sink);

        const elapsed = performance.now() - start;
        results.push({
            iteration: i + 1,
            elapsedMs: Math.round(elapsed),
            throughputMBs: +((totalBytes / elapsed * 1000) / (1024 * 1024)).toFixed(2),
        });
    }

    const avgThroughput = results.reduce((s, r) => s + r.throughputMBs, 0) / results.length;

    return {
        results,
        average: {
            throughputMBs: +avgThroughput.toFixed(2),
            elapsedMs: Math.round(results.reduce((s, r) => s + r.elapsedMs, 0) / results.length),
        },
    };
}

describe('Stream Performance', () => {
    it('should maintain throughput above threshold', async () => {
        const result = await benchmarkStream(
            () => new Transform({
                transform(chunk, encoding, callback) {
                    callback(null, chunk);
                }
            }),
            { totalBytes: 10 * 1024 * 1024, iterations: 1 }
        );

        assert.ok(
            result.average.throughputMBs > 100,
            `Throughput ${result.average.throughputMBs} MB/s below threshold`
        );
    });
});
```

## Test Utilities

```javascript
import { Readable, Writable, PassThrough } from 'node:stream';

class TestStreamFactory {
    static readable(data, options = {}) {
        const items = Array.isArray(data) ? data : [data];
        let index = 0;

        return new Readable({
            objectMode: options.objectMode,
            read() {
                if (index < items.length) {
                    const chunk = options.objectMode
                        ? items[index++]
                        : Buffer.from(String(items[index++]));
                    this.push(chunk);
                } else {
                    this.push(null);
                }
            }
        });
    }

    static writable(collector = []) {
        return new Writable({
            objectMode: true,
            write(chunk, encoding, callback) {
                collector.push(chunk);
                callback();
            }
        });
    }

    static passThrough() {
        return new PassThrough();
    }

    static errorAfter(count) {
        let processed = 0;
        return new PassThrough({
            transform(chunk, encoding, callback) {
                if (++processed > count) {
                    callback(new Error(`Error after ${count} chunks`));
                } else {
                    callback(null, chunk);
                }
            }
        });
    }

    static slowStream(delayMs) {
        return new Transform({
            transform(chunk, encoding, callback) {
                setTimeout(() => callback(null, chunk), delayMs);
            }
        });
    }
}
```

## Best Practices Checklist

- [ ] Test transforms with various input sizes
- [ ] Test error propagation through pipelines
- [ ] Test backpressure handling
- [ ] Use `collectStream` helper for output verification
- [ ] Benchmark streams with production-like data
- [ ] Test edge cases: empty input, single chunk, huge chunks
- [ ] Run stream tests in CI/CD pipeline

## Cross-References

- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for error patterns
- See [Performance](../08-stream-performance-optimization/01-profiling-memory.md) for profiling
- See [Concurrency](../06-stream-concurrency-parallelism/01-parallel-processing.md) for parallel testing

## Next Steps

Continue to [Stream Debugging](../12-stream-debugging/01-debugging-tools.md).
