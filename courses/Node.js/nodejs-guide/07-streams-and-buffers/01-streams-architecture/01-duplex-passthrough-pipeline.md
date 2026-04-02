# Duplex, PassThrough, and Pipeline Streams

## What You'll Learn

- Duplex streams for bidirectional communication
- PassThrough streams for data interception
- Stream pipeline creation with `pipeline()`
- Backpressure management and optimization
- Stream event system deep dive

## Duplex Streams

Duplex streams implement both Readable and Writable interfaces. Unlike Transform, they don't necessarily transform data — they can send and receive independently.

```javascript
import { Duplex } from 'node:stream';
import { createConnection } from 'node:net';

// Custom Duplex: wraps a TCP socket-like interface
class MemoryDuplex extends Duplex {
    constructor(options) {
        super(options);
        this.buffer = [];
    }

    _write(chunk, encoding, callback) {
        // Data written to this stream is stored
        this.buffer.push(chunk);
        console.log(`Written: ${chunk.toString().trim()}`);
        callback();
    }

    _read(size) {
        // Push buffered data when read is requested
        if (this.buffer.length > 0) {
            this.push(this.buffer.shift());
        } else {
            this.push(null); // Signal end
        }
    }
}

const duplex = new MemoryDuplex();

duplex.on('data', (chunk) => {
    console.log(`Read back: ${chunk.toString().trim()}`);
});

duplex.write('First message\n');
duplex.write('Second message\n');
duplex.end();
```

### Real-World Duplex: Crypto Stream

```javascript
import { createCipheriv, createDecipheriv, randomBytes } from 'node:crypto';
import { Duplex } from 'node:stream';

class EncryptedDuplex extends Duplex {
    constructor(key, options) {
        super(options);
        this.key = key;
        this.iv = randomBytes(16);
        this.cipher = createCipheriv('aes-256-cbc', this.key, this.iv);
        this.decipher = createDecipheriv('aes-256-cbc', this.key, this.iv);
    }

    _write(chunk, encoding, callback) {
        // Encrypt and push to readable side
        const encrypted = this.cipher.update(chunk);
        this.push(encrypted);
        callback();
    }

    _read() {
        // Read from internal buffer or wait
    }

    decrypt(chunk) {
        const decrypted = this.decipher.update(chunk);
        return decrypted;
    }

    _final(callback) {
        this.push(this.cipher.final());
        callback();
    }
}
```

## PassThrough Streams

PassThrough streams don't transform data — they simply pass it through. They're useful for logging, tee-ing, and intercepting.

```javascript
import { PassThrough, pipeline } from 'node:stream';
import { createReadStream, createWriteStream } from 'node:fs';

// Logging PassThrough
const logger = new PassThrough();
let totalBytes = 0;

logger.on('data', (chunk) => {
    totalBytes += chunk.length;
    process.stdout.write(`\rTransferred: ${totalBytes} bytes`);
});

logger.on('end', () => {
    console.log(`\nTotal: ${totalBytes} bytes`);
});

// Tee stream — split data to multiple destinations
const tee = new PassThrough();
tee.pipe(createWriteStream('copy1.txt'));
tee.pipe(createWriteStream('copy2.txt'));

// Use in pipeline
await pipeline(
    createReadStream('source.txt'),
    logger,
    tee,
    (err) => {
        if (err) console.error('Pipeline failed:', err);
    }
);
```

### PassThrough for Response Duplication

```javascript
import { PassThrough } from 'node:stream';
import { createReadStream } from 'node:fs';

function teeStream(source) {
    const stream1 = new PassThrough();
    const stream2 = new PassThrough();

    source.pipe(stream1);
    source.pipe(stream2);

    return [stream1, stream2];
}

// Split a file stream to two outputs
const source = createReadStream('data.csv');
const [forProcessing, forBackup] = teeStream(source);

forProcessing.on('data', (chunk) => {
    console.log('Processing chunk:', chunk.length);
});

forBackup.on('data', (chunk) => {
    console.log('Backing up chunk:', chunk.length);
});
```

## Stream Pipeline

The `pipeline()` function from `stream/promises` connects multiple streams with automatic error handling and cleanup.

```javascript
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';
import { createGzip, createGunzip } from 'node:zlib';
import { Transform } from 'node:stream';

// Simple pipeline
try {
    await pipeline(
        createReadStream('input.txt'),
        createGzip(),
        createWriteStream('output.txt.gz')
    );
    console.log('Compression complete');
} catch (err) {
    console.error('Pipeline failed:', err);
}

// Multi-stage pipeline with transforms
const toUpperCase = new Transform({
    transform(chunk, encoding, callback) {
        callback(null, chunk.toString().toUpperCase());
    }
});

const addLineNumbers = new Transform({
    transform(chunk, encoding, callback) {
        const lines = chunk.toString().split('\n');
        const numbered = lines.map((line, i) => `${i + 1}: ${line}`).join('\n');
        callback(null, numbered + '\n');
    }
});

await pipeline(
    createReadStream('input.txt'),
    toUpperCase,
    addLineNumbers,
    createGzip(),
    createWriteStream('output.txt.gz'),
    { signal: AbortSignal.timeout(30000) } // 30s timeout
);
```

### Pipeline with AbortController

```javascript
import { pipeline } from 'node:stream/promises';

const controller = new AbortController();

// Cancel after 5 seconds
setTimeout(() => controller.abort(), 5000);

try {
    await pipeline(
        createReadStream('large-file.txt'),
        createGzip(),
        createWriteStream('output.gz'),
        { signal: controller.signal }
    );
} catch (err) {
    if (err.name === 'AbortError') {
        console.log('Pipeline cancelled');
    }
}
```

## Backpressure Management

Backpressure occurs when a writable stream can't keep up with a readable stream. Node.js handles this automatically via return values.

```
Backpressure Flow:
─────────────────────────────────────────────
Readable                 Writable
  │                         │
  │── chunk ───────────────►│
  │                         │ write() returns false
  │◄── pause ──────────────│ (buffer full)
  │                         │
  │    (waiting...)         │
  │                         │
  │◄── drain event ─────────│ (buffer emptied)
  │── resume ──────────────►│
  │── next chunk ──────────►│
```

```javascript
import { createReadStream, createWriteStream } from 'node:fs';

const reader = createReadStream('huge-file.txt', { highWaterMark: 64 * 1024 });
const writer = createWriteStream('output.txt', { highWaterMark: 16 * 1024 });

reader.on('data', (chunk) => {
    const canWrite = writer.write(chunk);

    if (!canWrite) {
        console.log('Backpressure detected — pausing reader');
        reader.pause();

        writer.once('drain', () => {
            console.log('Drained — resuming reader');
            reader.resume();
        });
    }
});

reader.on('end', () => {
    writer.end();
    console.log('Done');
});
```

### Automatic Backpressure with pipeline

```javascript
// pipeline() handles backpressure automatically — recommended approach
import { pipeline } from 'node:stream/promises';

await pipeline(
    createReadStream('huge-file.txt', { highWaterMark: 64 * 1024 }),
    new Transform({
        highWaterMark: 16 * 1024,
        transform(chunk, encoding, callback) {
            // Simulate slow processing
            setTimeout(() => callback(null, chunk), 10);
        }
    }),
    createWriteStream('output.txt', { highWaterMark: 16 * 1024 })
);
```

## Stream Events Reference

```
Readable Stream Events:
─────────────────────────────────────────────
Event         Description
─────────────────────────────────────────────
data          Chunk available
end           No more data
error         An error occurred
close         Stream closed
readable      Data available to read
pause         Stream paused
resume        Stream resumed
pipe          Readable piped to writable
unpipe        Readable unpiped from writable

Writable Stream Events:
─────────────────────────────────────────────
Event         Description
─────────────────────────────────────────────
drain         Buffer emptied, safe to write
finish        All data written, end() called
error         An error occurred
close         Stream closed
pipe          Readable piping to this writable
unpipe        Readable unpiped

Transform/Duplex Events:
─────────────────────────────────────────────
All readable + writable events plus:
prefinish     All data written (before finish)
pipe          Source piping to this transform
unpipe        Source unpiped
```

## Best Practices Checklist

- [ ] Use `pipeline()` instead of `.pipe()` for error handling
- [ ] Always handle `error` events on every stream
- [ ] Set appropriate `highWaterMark` for your workload
- [ ] Use `AbortController` for cancellable pipelines
- [ ] Monitor backpressure with writable return values
- [ ] Use PassThrough for logging and tee-ing
- [ ] Prefer Duplex over Transform when no transformation needed

## Cross-References

- See [Readable Streams](../streams/01-readable-streams.md) for readable basics
- See [Writable Streams](../streams/02-writable-streams.md) for writable basics
- See [Transform Streams](../streams/04-transform-streams.md) for transformation
- See [Stream Error Handling](./02-stream-error-handling.md) for error patterns
- See [Backpressure Optimization](../08-stream-performance-optimization/01-profiling-memory.md) for tuning

## Next Steps

Continue to [Stream Error Handling](./02-stream-error-handling.md) for error patterns.
