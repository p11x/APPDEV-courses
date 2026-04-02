# Advanced Stream Patterns and Error Recovery

## What You'll Learn

- PassThrough streams for tee/splitting
- Duplex streams for bidirectional communication
- Stream error recovery strategies
- High-throughput stream processing

## PassThrough Streams

```javascript
import { PassThrough, pipeline } from 'node:stream';
import { createReadStream } from 'node:fs';

// Tee: split stream to multiple destinations
const tee = new PassThrough();

// Log while processing
tee.on('data', (chunk) => {
    console.log(`Processing chunk: ${chunk.length} bytes`);
});

// Pipe to multiple outputs
tee.pipe(createWriteStream('copy1.txt'));
tee.pipe(createWriteStream('copy2.txt'));

createReadStream('source.txt').pipe(tee);
```

## Duplex Streams

```javascript
import { Duplex } from 'node:stream';

// Bidirectional stream (read and write independent)
class TCPWrapper extends Duplex {
    constructor(socket) {
        super();
        this.socket = socket;

        socket.on('data', (data) => this.push(data));
        socket.on('end', () => this.push(null));
    }

    _write(chunk, encoding, callback) {
        this.socket.write(chunk, encoding, callback);
    }

    _read() {
        // Reading handled by socket events
    }
}
```

## Error Recovery

```javascript
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';

// Transform with retry logic
class RetryTransform extends Transform {
    constructor(fn, retries = 3) {
        super({ objectMode: true });
        this.fn = fn;
        this.retries = retries;
    }

    async _transform(chunk, encoding, callback) {
        for (let attempt = 1; attempt <= this.retries; attempt++) {
            try {
                const result = await this.fn(chunk);
                callback(null, result);
                return;
            } catch (err) {
                if (attempt === this.retries) {
                    callback(err);
                    return;
                }
                console.warn(`Retry ${attempt}/${this.retries}: ${err.message}`);
                await new Promise(r => setTimeout(r, 100 * attempt));
            }
        }
    }
}

// Dead letter queue for failed items
class DeadLetterTransform extends Transform {
    constructor(fn) {
        super({ objectMode: true });
        this.fn = fn;
        this.deadLetters = [];
    }

    async _transform(chunk, encoding, callback) {
        try {
            callback(null, await this.fn(chunk));
        } catch (err) {
            this.deadLetters.push({ item: chunk, error: err.message });
            callback(); // Skip failed item
        }
    }

    _flush(callback) {
        if (this.deadLetters.length > 0) {
            console.warn(`${this.deadLetters.length} items in dead letter queue`);
        }
        callback();
    }
}
```

## Best Practices Checklist

- [ ] Use PassThrough for stream splitting/logging
- [ ] Implement retry logic for transient failures
- [ ] Use dead letter queues for failed items
- [ ] Monitor stream throughput in production
- [ ] Set appropriate timeouts for long-running streams

## Cross-References

- See [Readable/Writable](./01-readable-writable-streams.md) for stream basics
- See [Pipeline and Transform](./02-pipeline-transform.md) for chaining
- See [Error Recovery](../11-error-handling/02-recovery-strategies.md) for error patterns

## Next Steps

Continue to [Buffers Deep Dive](../06-buffers-deep-dive/01-buffer-creation-encoding.md) for binary data.
