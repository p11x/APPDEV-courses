# Stream Debugging, Profiling, and Troubleshooting

## What You'll Learn

- Stream debugging techniques
- Memory leak detection
- Performance bottleneck identification
- Production stream monitoring
- Common issues and solutions

## Stream Debugger

```javascript
import { Transform, PassThrough } from 'node:stream';

class StreamDebugger extends PassThrough {
    constructor(label, options = {}) {
        super(options);
        this.label = label;
        this.verbose = options.verbose || false;
        this.chunkCount = 0;
        this.totalBytes = 0;
        this.startTime = Date.now();

        this.on('data', (chunk) => {
            this.chunkCount++;
            this.totalBytes += chunk.length;
            if (this.verbose) {
                console.log(`[${this.label}] chunk #${this.chunkCount}: ${chunk.length} bytes`);
            }
        });

        this.on('end', () => {
            const elapsed = Date.now() - this.startTime;
            console.log(`[${this.label}] END: ${this.chunkCount} chunks, ${this.totalBytes} bytes, ${elapsed}ms`);
        });

        this.on('error', (err) => {
            console.error(`[${this.label}] ERROR:`, err.message);
        });

        this.on('close', () => {
            console.log(`[${this.label}] CLOSED`);
        });

        this.on('pause', () => console.log(`[${this.label}] PAUSED`));
        this.on('resume', () => console.log(`[${this.label}] RESUMED`));
    }
}

// Usage: wrap any stream with debugger
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';
import { createGzip } from 'node:zlib';

await pipeline(
    createReadStream('input.txt'),
    new StreamDebugger('read'),
    createGzip(),
    new StreamDebugger('gzip'),
    createWriteStream('output.gz'),
    new StreamDebugger('write')
);
```

## Memory Leak Detection

```javascript
import { Transform } from 'node:stream';

// Common leak: accumulating data without draining
class LeakyTransform extends Transform {
    _transform(chunk, encoding, callback) {
        // LEAK: grows indefinitely
        this.buffer = Buffer.concat([this.buffer || Buffer.alloc(0), chunk]);
        callback();
    }
}

// Detection: monitor heap during stream processing
function detectStreamMemoryLeak() {
    const startHeap = process.memoryUsage().heapUsed;
    let peakHeap = startHeap;

    const monitor = setInterval(() => {
        const current = process.memoryUsage().heapUsed;
        if (current > peakHeap) peakHeap = current;
    }, 100);

    return {
        stop() {
            clearInterval(monitor);
            const endHeap = process.memoryUsage().heapUsed;
            return {
                startMB: +(startHeap / 1024 / 1024).toFixed(1),
                endMB: +(endHeap / 1024 / 1024).toFixed(1),
                peakMB: +(peakHeap / 1024 / 1024).toFixed(1),
                leakedMB: +((endHeap - startHeap) / 1024 / 1024).toFixed(1),
            };
        }
    };
}

// Usage
const leak = detectStreamMemoryLeak();

await pipeline(
    createReadStream('large-file.txt'),
    new GoodTransform(),
    createWriteStream('output.txt')
);

const report = leak.stop();
console.log('Memory report:', report);
if (report.leakedMB > 10) {
    console.warn('Potential memory leak detected!');
}
```

## Common Issues and Solutions

```
Stream Troubleshooting Guide:
─────────────────────────────────────────────

Issue: "write after end" error
Cause: Writing to a stream after end() is called
Fix: Check stream.writable before writing

Issue: Stream hangs / never ends
Cause: Missing push(null) in _read, or _flush not calling callback
Fix: Ensure all code paths call push(null) or callback()

Issue: High memory usage
Cause: Chunks accumulating without processing
Fix: Process chunks incrementally, don't concatenate

Issue: Backpressure ignored
Cause: Not checking write() return value
Fix: Check return value, wait for 'drain' event

Issue: "premature close" error
Cause: Dest stream closed before source finished
Fix: Use pipeline() for automatic cleanup

Issue: EPIPE errors
Cause: Writing to closed stream (e.g., broken pipe)
Fix: Handle 'error' and 'close' events

Issue: Data corruption
Cause: Race conditions in concurrent transforms
Fix: Ensure ordering or use objectMode

Issue: Slow stream processing
Cause: Synchronous operations in transform
Fix: Use async operations, tune highWaterMark
```

## Production Stream Monitor

```javascript
import { PassThrough } from 'node:stream';

class ProductionStreamMonitor extends PassThrough {
    constructor(label, options = {}) {
        super(options);
        this.label = label;
        this.metrics = {
            chunks: 0,
            bytes: 0,
            errors: 0,
            startTime: Date.now(),
        };

        this.reportInterval = options.reportInterval || 30000;
        this.alertThreshold = options.alertThreshold || { bytesPerSec: 1024 };

        this.timer = setInterval(() => this.report(), this.reportInterval);
    }

    _transform(chunk, encoding, callback) {
        this.metrics.chunks++;
        this.metrics.bytes += chunk.length;
        callback(null, chunk);
    }

    report() {
        const elapsed = (Date.now() - this.metrics.startTime) / 1000;
        const bytesPerSec = this.metrics.bytes / elapsed;

        const status = {
            label: this.label,
            chunks: this.metrics.chunks,
            totalMB: +(this.metrics.bytes / 1024 / 1024).toFixed(2),
            throughputMBs: +(bytesPerSec / 1024 / 1024).toFixed(2),
            errors: this.metrics.errors,
            elapsed: +elapsed.toFixed(1),
        };

        if (bytesPerSec < this.alertThreshold.bytesPerSec) {
            console.warn(`[ALERT] Low throughput: ${status.throughputMBs} MB/s`);
        }

        this.emit('metrics', status);
    }

    _flush(callback) {
        clearInterval(this.timer);
        this.report();
        callback();
    }
}
```

## Best Practices Checklist

- [ ] Add debug logging to custom streams during development
- [ ] Monitor heap usage during stream processing
- [ ] Use `StreamDebugger` for flow analysis
- [ ] Check for data accumulation in transforms
- [ ] Monitor throughput in production
- [ ] Set up alerts for stream errors and low throughput
- [ ] Test with `--inspect` flag for detailed debugging

## Cross-References

- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for error patterns
- See [Performance](../08-stream-performance-optimization/01-profiling-memory.md) for profiling
- See [Testing](../11-stream-testing/01-unit-testing.md) for testing strategies

## Next Steps

Review all sections and apply patterns relevant to your application. Cross-reference [Buffer Basics](../buffers/01-buffer-basics.md) and [Readable Streams](../streams/01-readable-streams.md) for foundational concepts.
