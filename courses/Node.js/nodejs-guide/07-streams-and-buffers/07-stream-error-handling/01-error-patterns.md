# Stream Error Handling Patterns and Recovery

## What You'll Learn

- Stream error handling patterns
- Error propagation through pipelines
- Error recovery and retry strategies
- Error boundaries for streams
- Production error handling

## Error Handling Patterns

```javascript
import { createReadStream } from 'node:fs';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

// Pattern 1: Try-catch with pipeline (recommended)
async function safePipeline(input, output, transforms) {
    try {
        await pipeline(input, ...transforms, output);
        return { success: true };
    } catch (err) {
        console.error('Pipeline error:', err.message);
        return { success: false, error: err.message };
    }
}

// Pattern 2: Error events on individual streams
const stream = createReadStream('data.txt');
stream.on('error', (err) => {
    if (err.code === 'ENOENT') {
        console.error('File not found');
    } else if (err.code === 'EACCES') {
        console.error('Permission denied');
    } else {
        console.error('Stream error:', err);
    }
});

// Pattern 3: Error recovery with retry
class RetryTransform extends Transform {
    constructor(fn, options = {}) {
        super({ objectMode: true });
        this.fn = fn;
        this.maxRetries = options.maxRetries || 3;
        this.retryDelay = options.retryDelay || 1000;
    }

    async _transform(chunk, encoding, callback) {
        for (let attempt = 1; attempt <= this.maxRetries; attempt++) {
            try {
                const result = await this.fn(chunk);
                callback(null, result);
                return;
            } catch (err) {
                if (attempt === this.maxRetries) {
                    this.emit('retry-exhausted', { chunk, error: err });
                    callback(); // Skip failed record
                    return;
                }
                await new Promise(r => setTimeout(r, this.retryDelay * attempt));
            }
        }
    }
}
```

## Error Boundary for Streams

```javascript
import { Transform, PassThrough } from 'node:stream';

class ErrorBoundary extends Transform {
    constructor(options = {}) {
        super({ objectMode: options.objectMode });
        this.errorHandler = options.onError || console.error;
        this.maxErrors = options.maxErrors || 100;
        this.errorCount = 0;
        this.droppedCount = 0;
    }

    _transform(chunk, encoding, callback) {
        try {
            callback(null, chunk);
        } catch (err) {
            this.handleError(err, chunk);
            callback(); // Continue processing
        }
    }

    handleError(err, chunk) {
        this.errorCount++;
        this.droppedCount++;

        this.errorHandler({
            error: err.message,
            chunk,
            errorCount: this.errorCount,
        });

        if (this.errorCount >= this.maxErrors) {
            this.destroy(new Error(`Max errors (${this.maxErrors}) exceeded`));
        }
    }

    getStats() {
        return { errorCount: this.errorCount, droppedCount: this.droppedCount };
    }
}

// Usage
import { pipeline } from 'node:stream/promises';

const boundary = new ErrorBoundary({
    onError: (info) => {
        console.error(`Error #${info.errorCount}: ${info.error}`);
    },
    maxErrors: 50,
});

boundary.on('close', () => {
    console.log('Boundary stats:', boundary.getStats());
});

await pipeline(
    createReadStream('data.jsonl'),
    boundary,
    new Transform({
        objectMode: true,
        transform(chunk, encoding, callback) {
            const record = JSON.parse(chunk.toString());
            if (!record.id) throw new Error('Missing ID');
            callback(null, record);
        }
    }),
    process.stdout
);
```

## Error Propagation in Pipelines

```javascript
// pipeline() automatically handles error propagation:
// 1. Errors in any stream propagate to pipeline callback
// 2. All streams are cleaned up (closed/destroyed)
// 3. Partial writes may be left (use atomic writes for critical data)

await pipeline(
    source,
    transform1,    // If this errors...
    transform2,    // ...this is destroyed
    destination,   // ...and this is destroyed
    (err) => {
        if (err) {
            console.error('Pipeline failed:', err);
            // All streams have been cleaned up
        }
    }
);

// Manual error propagation (less reliable)
source.pipe(transform1).pipe(transform2).pipe(destination);

source.on('error', (err) => {
    transform1.destroy(err);
    transform2.destroy(err);
    destination.destroy(err);
});

transform1.on('error', (err) => {
    source.destroy(err);
    transform2.destroy(err);
    destination.destroy(err);
});
```

## Dead Letter Queue for Streams

```javascript
import { Transform } from 'node:stream';

class DeadLetterTransform extends Transform {
    constructor(fn, options = {}) {
        super({ objectMode: true });
        this.fn = fn;
        this.deadLetterQueue = options.deadLetterQueue || [];
        this.maxQueueSize = options.maxQueueSize || 1000;
    }

    async _transform(chunk, encoding, callback) {
        try {
            const result = await this.fn(chunk);
            callback(null, result);
        } catch (err) {
            this.deadLetterQueue.push({
                chunk,
                error: err.message,
                timestamp: Date.now(),
            });

            if (this.deadLetterQueue.length > this.maxQueueSize) {
                this.deadLetterQueue.shift();
            }

            this.emit('dead-letter', { chunk, error: err });
            callback(); // Skip failed record
        }
    }

    getDeadLetters() {
        return this.deadLetterQueue;
    }

    async retryDeadLetters(retryFn) {
        const results = [];
        for (const item of this.deadLetterQueue) {
            try {
                await retryFn(item.chunk);
                results.push({ success: true, chunk: item.chunk });
            } catch (err) {
                results.push({ success: false, chunk: item.chunk, error: err.message });
            }
        }
        return results;
    }
}
```

## Best Practices Checklist

- [ ] Always use `pipeline()` for automatic error handling
- [ ] Handle `ENOENT`, `EACCES`, `EPIPE` specifically
- [ ] Implement retry with backoff for transient errors
- [ ] Use error boundaries for resilient pipelines
- [ ] Log errors with context for debugging
- [ ] Set `maxErrors` limits to prevent infinite error loops
- [ ] Use dead letter queues for failed records

## Cross-References

- See [Pipeline](../01-streams-architecture/01-duplex-passthrough-pipeline.md) for pipeline basics
- See [Error Recovery](./02-error-recovery-retry.md) for retry patterns
- See [Testing](../11-stream-testing/01-unit-testing.md) for error testing

## Next Steps

Continue to [Performance Optimization](../08-stream-performance-optimization/01-profiling-memory.md).
