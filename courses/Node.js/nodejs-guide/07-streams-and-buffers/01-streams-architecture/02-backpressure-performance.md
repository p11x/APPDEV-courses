# Backpressure and Stream Performance

## What You'll Learn

- Backpressure internals and flow control
- highWaterMark tuning strategies
- Stream performance optimization techniques
- Buffering strategies for streams
- Stream throughput measurement

## Backpressure Deep Dive

```javascript
import { Readable, Writable } from 'node:stream';

// Producer: fast data source
class FastProducer extends Readable {
    constructor(options) {
        super(options);
        this.counter = 0;
        this.max = options.max || 1000000;
    }

    _read(size) {
        // Push data as fast as possible
        const chunkSize = size || 1024;
        const chunk = Buffer.alloc(chunkSize, 'A');
        
        if (this.counter >= this.max) {
            this.push(null);
            return;
        }

        const shouldContinue = this.push(chunk);
        this.counter += chunkSize;

        if (!shouldContinue) {
            // Backpressure signal — stop pushing until _read is called again
            console.log(`Backpressure at ${this.counter} bytes`);
        }
    }
}

// Consumer: slow data sink
class SlowConsumer extends Writable {
    constructor(options) {
        super(options);
        this.totalBytes = 0;
        this.delay = options.delay || 10; // ms delay per chunk
    }

    _write(chunk, encoding, callback) {
        // Simulate slow processing
        setTimeout(() => {
            this.totalBytes += chunk.length;
            callback();
        }, this.delay);
    }
}

// Observe backpressure in action
const producer = new FastProducer({ max: 10 * 1024 * 1024 }); // 10MB
const consumer = new SlowConsumer({ delay: 5 });

const start = Date.now();

producer.pipe(consumer);

consumer.on('finish', () => {
    const elapsed = Date.now() - start;
    console.log(`Transferred ${consumer.totalBytes} bytes in ${elapsed}ms`);
    console.log(`Throughput: ${(consumer.totalBytes / elapsed / 1024).toFixed(1)} KB/s`);
});
```

## highWaterMark Tuning

The `highWaterMark` controls internal buffer size. Larger values reduce backpressure frequency but increase memory usage.

```javascript
import { createReadStream, createWriteStream } from 'node:fs';

// Small buffer — more backpressure events, less memory
const smallReader = createReadStream('large-file.txt', {
    highWaterMark: 4 * 1024, // 4KB chunks
});

// Large buffer — fewer backpressure events, more memory
const largeReader = createReadStream('large-file.txt', {
    highWaterMark: 256 * 1024, // 256KB chunks
});

// Optimal for SSD: 64KB-256KB
// Optimal for HDD: 16KB-64KB
// Optimal for network: 16KB-64KB
// Optimal for memory-constrained: 4KB-16KB

// Writable watermarks
const writer = createWriteStream('output.txt', {
    highWaterMark: 16 * 1024, // 16KB write buffer
});
```

### highWaterMark Comparison Benchmark

```javascript
import { performance } from 'node:perf_hooks';
import { createReadStream, createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';

async function benchmarkWaterMark(filePath, waterMarkKB) {
    const start = performance.now();
    
    await pipeline(
        createReadStream(filePath, { highWaterMark: waterMarkKB * 1024 }),
        createWriteStream('/dev/null', { highWaterMark: waterMarkKB * 1024 })
    );
    
    return performance.now() - start;
}

// Benchmark different watermarks
const sizes = [4, 16, 64, 128, 256, 512, 1024];
const results = [];

for (const size of sizes) {
    const time = await benchmarkWaterMark('test-1gb.dat', size);
    results.push({ waterMarkKB: size, timeMs: Math.round(time) });
}

console.table(results);
```

```
Typical Results (1GB file, NVMe SSD):
─────────────────────────────────────────────
waterMarkKB    timeMs     memoryPeak
─────────────────────────────────────────────
4              4200       12MB
16             2800       20MB
64             2100       45MB
128            1900       75MB
256            1850       130MB
512            1830       250MB
1024           1820       490MB
```

## Stream Throughput Measurement

```javascript
import { Transform } from 'node:stream';

class ThroughputMonitor extends Transform {
    constructor(options = {}) {
        super(options);
        this.interval = options.interval || 1000;
        this.bytesProcessed = 0;
        this.startTime = Date.now();
        this.lastReport = this.startTime;
    }

    _transform(chunk, encoding, callback) {
        this.bytesProcessed += chunk.length;

        const now = Date.now();
        if (now - this.lastReport >= this.interval) {
            const elapsed = (now - this.startTime) / 1000;
            const rate = this.bytesProcessed / elapsed;
            
            this.emit('throughput', {
                bytes: this.bytesProcessed,
                elapsed: elapsed.toFixed(1),
                rateMBs: (rate / 1024 / 1024).toFixed(2),
            });

            this.lastReport = now;
        }

        callback(null, chunk);
    }

    _flush(callback) {
        const elapsed = (Date.now() - this.startTime) / 1000;
        const rate = this.bytesProcessed / elapsed;

        this.emit('throughput', {
            bytes: this.bytesProcessed,
            elapsed: elapsed.toFixed(1),
            rateMBs: (rate / 1024 / 1024).toFixed(2),
            final: true,
        });

        callback();
    }
}

// Usage
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

const monitor = new ThroughputMonitor({ interval: 500 });

monitor.on('throughput', (stats) => {
    const prefix = stats.final ? 'Final' : 'Current';
    console.log(`${prefix}: ${stats.rateMBs} MB/s (${stats.bytes} bytes in ${stats.elapsed}s)`);
});

await pipeline(
    createReadStream('large-file.dat'),
    monitor,
    createWriteStream('output.dat')
);
```

## Buffer Pooling for Streams

```javascript
import { Readable } from 'node:stream';

// Pool of reusable buffers to reduce GC pressure
class BufferPool {
    constructor(chunkSize, poolSize) {
        this.chunkSize = chunkSize;
        this.pool = Array.from({ length: poolSize }, () => Buffer.alloc(chunkSize));
        this.available = [...this.pool];
    }

    acquire() {
        if (this.available.length === 0) {
            return Buffer.alloc(this.chunkSize);
        }
        return this.available.pop();
    }

    release(buf) {
        if (this.available.length < this.pool.length) {
            buf.fill(0);
            this.available.push(buf);
        }
    }
}

class PooledReadable extends Readable {
    constructor(pool, options) {
        super(options);
        this.pool = pool;
        this.counter = 0;
    }

    _read() {
        const buf = this.pool.acquire();
        // Fill buffer with data
        const written = buf.write(`Data chunk ${this.counter++}\n`, 'utf8');
        
        const canContinue = this.push(buf.slice(0, written));
        
        if (!canContinue) {
            // Will be called again when _read is triggered
        }
    }

    _destroy(err, callback) {
        // Cleanup
        this.pool = null;
        callback(err);
    }
}
```

## Best Practices Checklist

- [ ] Use `pipeline()` for automatic backpressure management
- [ ] Tune `highWaterMark` based on I/O type (disk, network, memory)
- [ ] Monitor throughput during development
- [ ] Use buffer pooling for high-throughput streams
- [ ] Avoid synchronous operations in transform callbacks
- [ ] Test with large data volumes to validate backpressure handling
- [ ] Measure memory usage under sustained load

## Cross-References

- See [Duplex and Pipeline](./01-duplex-passthrough-pipeline.md) for pipeline patterns
- See [Stream Performance Optimization](../08-stream-performance-optimization/01-profiling-memory.md) for profiling
- See [Buffer Mastery](../02-buffer-mastery/01-buffer-creation-memory.md) for buffer optimization

## Next Steps

Continue to [Stream Events and Emitters](./03-stream-events-emitters.md) for event system.
