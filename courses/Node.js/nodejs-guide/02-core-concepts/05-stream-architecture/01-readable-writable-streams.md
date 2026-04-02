# Readable and Writable Streams

## What You'll Learn

- Readable stream patterns and events
- Writable stream implementation
- Backpressure handling
- Stream options and configuration

## Readable Streams

```javascript
import { createReadStream } from 'node:fs';
import { Readable } from 'node:stream';

// File readable stream
const readStream = createReadStream('./large-file.txt', {
    encoding: 'utf-8',
    highWaterMark: 64 * 1024, // 64KB chunks (default)
});

// Event-based reading
readStream.on('data', (chunk) => {
    console.log(`Received ${chunk.length} bytes`);
});

readStream.on('end', () => console.log('Done reading'));
readStream.on('error', (err) => console.error('Error:', err));

// Async iterator reading
import { createReadStream } from 'node:fs';

const stream = createReadStream('./data.txt', { encoding: 'utf-8' });
for await (const chunk of stream) {
    console.log(chunk);
}

// Custom readable stream
class CounterStream extends Readable {
    constructor(limit) {
        super({ objectMode: true });
        this.limit = limit;
        this.current = 0;
    }

    _read() {
        if (this.current >= this.limit) {
            this.push(null); // Signal end
            return;
        }
        this.current++;
        this.push({ count: this.current });
    }
}

const counter = new CounterStream(5);
for await (const data of counter) {
    console.log(data); // { count: 1 }, { count: 2 }, ...
}
```

## Writable Streams

```javascript
import { createWriteStream } from 'node:fs';
import { Writable } from 'node:stream';

// File writable stream
const writeStream = createWriteStream('./output.txt', {
    encoding: 'utf-8',
    highWaterMark: 16 * 1024, // 16KB buffer
});

writeStream.write('Hello, ');
writeStream.write('World!\n');
writeStream.end('Final line');

writeStream.on('finish', () => console.log('Write complete'));
writeStream.on('error', (err) => console.error('Write error:', err));

// Custom writable stream
class LogStream extends Writable {
    constructor(options) {
        super({ objectMode: true });
        this.logs = [];
    }

    _write(chunk, encoding, callback) {
        this.logs.push({
            timestamp: new Date().toISOString(),
            message: chunk.toString(),
        });
        callback(); // Signal write complete
    }

    _final(callback) {
        console.log(`Logged ${this.logs.length} entries`);
        callback();
    }
}

const logger = new LogStream();
logger.write('Server started');
logger.write('Request received');
logger.end();
```

## Backpressure

```javascript
// Backpressure: writable signals readable to slow down
import { createReadStream, createWriteStream } from 'node:fs';

const readable = createReadStream('./huge-file.txt');
const writable = createWriteStream('./output.txt');

readable.on('data', (chunk) => {
    const canWrite = writable.write(chunk);
    if (!canWrite) {
        // Buffer is full — pause readable
        console.log('Backpressure: pausing readable');
        readable.pause();

        writable.once('drain', () => {
            console.log('Drain: resuming readable');
            readable.resume();
        });
    }
});

// pipe() handles backpressure automatically
readable.pipe(writable);
```

## Best Practices Checklist

- [ ] Use `pipe()` or `pipeline()` for automatic backpressure
- [ ] Handle 'error' events on all streams
- [ ] Set appropriate `highWaterMark` for your workload
- [ ] Use `objectMode: true` for non-binary data
- [ ] Always call `end()` on writable streams

## Cross-References

- See [Pipeline and Transform](./02-pipeline-transform.md) for stream chaining
- See [Advanced Patterns](./03-stream-advanced-patterns.md) for complex streams
- See [Buffers](../06-buffers-deep-dive/01-buffer-creation-encoding.md) for binary data

## Next Steps

Continue to [Pipeline and Transform](./02-pipeline-transform.md) for stream chaining.
