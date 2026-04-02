# Stream Performance Profiling and Memory Optimization

## What You'll Learn

- Stream performance profiling techniques
- Memory usage optimization
- CPU optimization for transforms
- I/O optimization strategies
- Stream benchmarking

## Stream Profiler

```javascript
import { Transform, PassThrough } from 'node:stream';
import { performance, monitorEventLoopDelay } from 'node:perf_hooks';

class StreamProfiler extends PassThrough {
    constructor(options = {}) {
        super(options);
        this.label = options.label || 'stream';
        this.startTime = performance.now();
        this.bytesProcessed = 0;
        this.chunkCount = 0;
        this.interval = options.interval || 1000;

        this.histogram = monitorEventLoopDelay({ resolution: 20 });
        this.histogram.enable();

        this.timer = setInterval(() => this.report(), this.interval);
    }

    _transform(chunk, encoding, callback) {
        this.bytesProcessed += chunk.length;
        this.chunkCount++;
        callback(null, chunk);
    }

    report() {
        const elapsed = (performance.now() - this.startTime) / 1000;
        const throughput = this.bytesProcessed / elapsed;

        console.log(`[${this.label}] ` +
            `${(throughput / 1024 / 1024).toFixed(2)} MB/s | ` +
            `${this.chunkCount} chunks | ` +
            `${(this.bytesProcessed / 1024 / 1024).toFixed(1)} MB total | ` +
            `EL lag: ${(this.histogram.mean / 1e6).toFixed(1)}ms`
        );
    }

    _flush(callback) {
        clearInterval(this.timer);
        this.histogram.disable();
        this.report();
        callback();
    }
}

// Usage
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';
import { createGzip } from 'node:zlib';

await pipeline(
    createReadStream('large-file.txt'),
    new StreamProfiler({ label: 'read' }),
    createGzip(),
    new StreamProfiler({ label: 'gzip' }),
    createWriteStream('output.gz'),
    new StreamProfiler({ label: 'write' })
);
```

## Memory Optimization

```javascript
// Problem: accumulating chunks in memory
class BadTransform extends Transform {
    _transform(chunk, encoding, callback) {
        this.buffer = (this.buffer || '') + chunk.toString(); // Memory leak!
        callback();
    }
}

// Solution: process incrementally
class GoodTransform extends Transform {
    constructor() {
        super();
        this.buffer = '';
    }

    _transform(chunk, encoding, callback) {
        this.buffer += chunk.toString();
        const lines = this.buffer.split('\n');
        this.buffer = lines.pop(); // Keep only incomplete line

        for (const line of lines) {
            this.push(line.toUpperCase() + '\n');
        }
        callback();
    }

    _flush(callback) {
        if (this.buffer) {
            this.push(this.buffer.toUpperCase());
        }
        callback();
    }
}

// Use objectMode for structured data (avoids repeated string conversion)
const objectTransform = new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        // Record is already an object, no string parsing
        callback(null, { ...record, processed: true });
    }
});
```

## CPU Optimization

```javascript
// Avoid synchronous operations in transforms
class SlowTransform extends Transform {
    _transform(chunk, encoding, callback) {
        // BAD: blocking event loop
        const result = crypto.pbkdf2Sync('password', 'salt', 100000, 64, 'sha512');
        callback(null, result);
    }
}

class FastTransform extends Transform {
    async _transform(chunk, encoding, callback) {
        // GOOD: async operation
        const result = await crypto.pbkdf2('password', 'salt', 100000, 64, 'sha512');
        callback(null, result);
    }
}

// Use TypedArrays for numerical processing
class NumericTransform extends Transform {
    _transform(chunk, encoding, callback) {
        // Convert to typed array for faster processing
        const floats = new Float64Array(
            chunk.buffer,
            chunk.byteOffset,
            chunk.length / 8
        );

        // Fast numerical operations
        let sum = 0;
        for (let i = 0; i < floats.length; i++) {
            sum += floats[i];
        }

        const result = Buffer.alloc(8);
        result.writeDoubleLE(sum, 0);
        callback(null, result);
    }
}
```

## Benchmark Framework

```javascript
import { performance } from 'node:perf_hooks';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class NullReadable extends Readable {
    constructor(chunkSize, totalBytes) {
        super();
        this.chunkSize = chunkSize;
        this.totalBytes = totalBytes;
        this.produced = 0;
        this.chunk = Buffer.alloc(chunkSize, 0xAA);
    }

    _read() {
        if (this.produced >= this.totalBytes) {
            this.push(null);
            return;
        }
        this.push(this.chunk);
        this.produced += this.chunkSize;
    }
}

class NullWritable extends Writable {
    constructor() {
        super();
        this.totalBytes = 0;
    }

    _write(chunk, encoding, callback) {
        this.totalBytes += chunk.length;
        callback();
    }
}

async function benchmarkTransform(transformFn, options = {}) {
    const chunkSize = options.chunkSize || 64 * 1024;
    const totalBytes = options.totalBytes || 100 * 1024 * 1024; // 100MB

    const start = performance.now();

    const writer = new NullWritable();
    await pipeline(
        new NullReadable(chunkSize, totalBytes),
        transformFn(),
        writer
    );

    const elapsed = performance.now() - start;
    const throughput = (totalBytes / elapsed * 1000) / (1024 * 1024);

    return {
        totalMB: totalBytes / (1024 * 1024),
        elapsedMs: Math.round(elapsed),
        throughputMBs: +throughput.toFixed(2),
    };
}

// Benchmark different transform configurations
const results = await Promise.all([
    benchmarkTransform(() => new Transform({
        transform(c, e, cb) { cb(null, c); } // Pass-through
    })),
    benchmarkTransform(() => new Transform({
        transform(c, e, cb) { cb(null, Buffer.from(c.toString('utf8'), 'utf8')); } // String round-trip
    })),
    benchmarkTransform(() => new Transform({
        transform(c, e, cb) {
            const upper = Buffer.from(c.toString().toUpperCase());
            cb(null, upper);
        }
    })),
]);

console.table(results);
```

## Best Practices Checklist

- [ ] Profile streams with throughput monitoring
- [ ] Avoid string concatenation in transforms
- [ ] Use `objectMode` for structured data
- [ ] Use TypedArrays for numerical computation
- [ ] Avoid synchronous operations in transforms
- [ ] Set appropriate `highWaterMark` for workload
- [ ] Benchmark with production-like data volumes

## Cross-References

- See [Backpressure](../01-streams-architecture/02-backpressure-performance.md) for flow control
- See [Buffer Performance](../02-buffer-mastery/01-buffer-creation-memory.md) for buffer optimization
- See [Concurrency](../06-stream-concurrency-parallelism/01-parallel-processing.md) for parallel processing

## Next Steps

Continue to [Stream Security](../09-stream-security/01-encryption-decryption.md).
