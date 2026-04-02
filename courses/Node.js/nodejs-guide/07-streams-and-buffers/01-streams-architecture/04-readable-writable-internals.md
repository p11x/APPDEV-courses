# Readable and Writable Streams Internal Implementation

## What You'll Learn

- Readable stream internal state machine (paused/flowing modes)
- Custom Readable implementation with `_read()`, `_construct()`, `_destroy()`
- Readable stream options: highWaterMark, encoding, objectMode, signal
- Custom Writable implementation with `_write()`, `_writev()`, `_construct()`, `_destroy()`
- Writable stream `cork()`/`uncork()` for batch writes
- Internal buffer management and data flow
- `stream.finished()` utility for cleanup
- Real-world examples: custom file reader, custom log writer

## Readable Stream State Machine

Readable streams operate in two internal modes that determine how data reaches consumers.

```
Readable Stream State Machine:
──────────────────────────────────────────────────────────────
                    ┌──────────────┐
                    │  Constructed │
                    └──────┬───────┘
                           │
                    ┌──────▼───────┐
              ┌─────┤   Paused     │◄──────────┐
              │     │  (pull mode) │           │
              │     └──────┬───────┘           │
              │      .resume() / .on('data')   │
              │            │                   │
              │     ┌──────▼───────┐           │
              │     │   Flowing    │  .pause() / .unpipe()
              │     │  (push mode) ├───────────┘
              │     └──────┬───────┘
              └────────────┘

Paused Mode:
  - Must call read() or listen for 'readable' to consume
  - Data accumulates in internal buffer up to highWaterMark
  - _read() called only when buffer dips below highWaterMark

Flowing Mode:
  - Data events emitted as soon as data is available
  - _read() called continuously to fill the buffer
  - Backpressure communicated via return value of push()
──────────────────────────────────────────────────────────────
```

## Custom Readable Implementation

`_read(size)` is the core method you implement. It's called whenever the stream needs more data.

```javascript
import { Readable } from 'node:stream';
import { open } from 'node:fs/promises';

class ChunkedFileReader extends Readable {
    constructor(filePath, options = {}) {
        super({
            highWaterMark: options.highWaterMark || 64 * 1024,
            encoding: options.encoding,
            objectMode: false,
            autoDestroy: true,
            signal: options.signal,
        });
        this.filePath = filePath;
        this.fd = null;
        this.position = 0;
        this.chunkSize = options.chunkSize || 16 * 1024;
    }

    async _construct(callback) {
        try {
            this.fd = await open(this.filePath, 'r');
            callback();
        } catch (err) { callback(err); }
    }

    async _read(size) {
        if (!this.fd) { this.push(null); return; }

        const bufSize = Math.max(size, this.chunkSize);
        const buffer = Buffer.alloc(bufSize);

        try {
            const { bytesRead } = await this.fd.read({
                buffer, offset: 0, length: bufSize, position: this.position,
            });

            if (bytesRead === 0) this.push(null);
            else {
                this.position += bytesRead;
                this.push(buffer.subarray(0, bytesRead));
            }
        } catch (err) { this.destroy(err); }
    }

    async _destroy(err, callback) {
        if (this.fd) {
            try { await this.fd.close(); } catch { /* ignore */ }
            this.fd = null;
        }
        callback(err);
    }
}

const reader = new ChunkedFileReader('large-log.txt', { highWaterMark: 32 * 1024 });
reader.on('data', (chunk) => console.log(`Read ${chunk.length} bytes`));
reader.on('end', () => console.log('Done'));
```

### Readable Stream Options

```javascript
const readable = new Readable({
    highWaterMark: 64 * 1024,             // Buffer size before _read() pauses
    encoding: 'utf8',                     // Decode chunks to string
    objectMode: false,                    // true = JS objects
    signal: AbortSignal.timeout(5000),    // Cancellation
    autoDestroy: true,
    _read(size) { this.push(someData); },
});
```

### Object Mode Readable

```javascript
import { Readable } from 'node:stream';

class RecordReader extends Readable {
    constructor(records) {
        super({ objectMode: true, highWaterMark: 16 });
        this.records = records;
        this.index = 0;
    }

    _read() {
        if (this.index >= this.records.length) { this.push(null); return; }
        const canContinue = this.push(this.records[this.index++]);
        if (!canContinue) return; // Buffer full
        this._read();
    }
}

const reader = new RecordReader([{ id: 1, name: 'Alice' }, { id: 2, name: 'Bob' }]);
for await (const record of reader) {
    console.log(record); // { id: 1, name: 'Alice' }
}
```

## Custom Writable Implementation

`_write(chunk, encoding, callback)` processes each chunk written to the stream.

```javascript
import { Writable } from 'node:stream';
import { open } from 'node:fs/promises';

class BatchFileWriter extends Writable {
    constructor(filePath, options = {}) {
        super({
            highWaterMark: options.highWaterMark || 16 * 1024,
            decodeStrings: true,
            objectMode: false,
            autoDestroy: true,
        });
        this.filePath = filePath;
        this.fd = null;
        this.position = 0;
    }

    async _construct(callback) {
        try {
            this.fd = await open(this.filePath, 'w');
            callback();
        } catch (err) { callback(err); }
    }

    async _write(chunk, encoding, callback) {
        try {
            const { bytesWritten } = await this.fd.write(chunk, 0, chunk.length, this.position);
            this.position += bytesWritten;
            callback();
        } catch (err) { callback(err); }
    }

    async _writev(chunks, callback) {
        try {
            for (const { chunk } of chunks) {
                const { bytesWritten } = await this.fd.write(chunk, 0, chunk.length, this.position);
                this.position += bytesWritten;
            }
            callback();
        } catch (err) { callback(err); }
    }

    async _destroy(err, callback) {
        if (this.fd) {
            try { await this.fd.close(); } catch { /* ignore */ }
            this.fd = null;
        }
        callback(err);
    }
}

const writer = new BatchFileWriter('output.txt');
writer.write('Line 1\n');
writer.end();
writer.on('finish', () => console.log('Write complete'));
```

### Writable Stream Options

```javascript
const writable = new Writable({
    highWaterMark: 32 * 1024,      // Internal buffer size
    decodeStrings: true,           // Decode strings to Buffer before _write()
    objectMode: false,             // true = JS objects
    signal: AbortSignal.timeout(10000),
    autoDestroy: true,

    writev(chunks, callback) {     // Batch writes
        const combined = Buffer.concat(chunks.map(c => c.chunk));
        this._write(combined, 'buffer', callback);
    },

    _write(chunk, encoding, callback) {
        process.stdout.write(chunk);
        callback();
    },
});
```

## Writable cork() and uncork()

`cork()` buffers writes internally. `uncork()` flushes them as a batch, reducing I/O operations.

```javascript
import { Writable } from 'node:stream';

class BatchedSocketWriter extends Writable {
    constructor() { super({ highWaterMark: 16 * 1024 }); }

    _writev(chunks, callback) {
        const combined = Buffer.concat(chunks.map(c => c.chunk));
        console.log(`Flushing ${combined.length} bytes in single batch`);
        callback();
    }

    _write(chunk, encoding, callback) {
        console.log(`Single write: ${chunk.length} bytes`);
        callback();
    }
}

const writer = new BatchedSocketWriter();
writer.cork();
writer.write(Buffer.from('Header: '));
writer.write(Buffer.from('Content-Type: text/plain\r\n'));
writer.write(Buffer.from('\r\nBody'));
writer.uncork(); // Flush all at once → single _writev call
writer.end();
```

## Internal Buffer Management

```
Internal Buffer Flow:
──────────────────────────────────────────────────────────────
  READABLE                      WRITABLE
  push(data)                    write(data)
       │                            │
       ▼                            ▼
  ┌─────────────┐             ┌─────────────┐
  │ Internal Buf │             │ Internal Buf │
  │ [chunk]      │             │ [chunk]      │
  │ Size: 8/64KB │             │ Size: 12/16KB│
  └──────┬──────┘             └──────┬──────┘
         │                            │
    ┌────▼────┐                  ┌────▼────┐
    │ _read()  │                  │ _write() │
    │ below HWM│                  │ per chunk│
    └──────────┘                  └──────────┘

  push() returns false → buffer full
  write() returns false → buffer full
  'drain' event → safe to write again
──────────────────────────────────────────────────────────────
```

## stream.finished() Utility

Monitors a stream and invokes a callback when closed, errored, or destroyed.

```javascript
import { finished } from 'node:stream';
import { finished as finishedPromise } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

// Callback style
finished(createReadStream('data.txt'), (err) => {
    if (err) console.error('Closed with error:', err.message);
    else console.log('Closed successfully');
});

// Promise style (recommended)
async function readAndCleanup(filePath) {
    const stream = createReadStream(filePath, { encoding: 'utf8' });
    const chunks = [];
    stream.on('data', (chunk) => chunks.push(chunk));
    await finishedPromise(stream); // Guaranteed closed
    return chunks.join('');
}

// With AbortController
const controller = new AbortController();
finishedPromise(createWriteStream('out.txt'), { signal: controller.signal })
    .catch((err) => { if (err.name === 'AbortError') console.log('Aborted'); });
```

## Real-World: Custom Log Writer

```javascript
import { Writable } from 'node:stream';
import { open } from 'node:fs/promises';
import { EOL } from 'node:os';

class RotatingLogWriter extends Writable {
    constructor(basePath, options = {}) {
        super({ objectMode: true, highWaterMark: 64 });
        this.basePath = basePath;
        this.maxSize = options.maxSize || 10 * 1024 * 1024;
        this.currentSize = 0;
        this.fileIndex = 0;
        this.fd = null;
    }

    async _construct(callback) {
        try {
            this.fd = await open(`${this.basePath}.${this.fileIndex}.log`, 'a');
            this.currentSize = (await this.fd.stat()).size;
            callback();
        } catch (err) { callback(err); }
    }

    async _write(entry, encoding, callback) {
        try {
            const line = `${entry.timestamp} [${entry.level}] ${entry.message}${EOL}`;
            const buffer = Buffer.from(line, 'utf8');
            if (this.currentSize + buffer.length > this.maxSize) await this._rotate();
            await this.fd.appendFile(buffer);
            this.currentSize += buffer.length;
            callback();
        } catch (err) { callback(err); }
    }

    async _rotate() {
        if (this.fd) await this.fd.close();
        this.fileIndex++;
        this.currentSize = 0;
        this.fd = await open(`${this.basePath}.${this.fileIndex}.log`, 'a');
        this.emit('rotate', this.fileIndex);
    }

    async _destroy(err, callback) {
        if (this.fd) { await this.fd.close(); this.fd = null; }
        callback(err);
    }
}

const logger = new RotatingLogWriter('./logs/app', { maxSize: 5 * 1024 * 1024 });
logger.on('rotate', (i) => console.log(`Rotated to index ${i}`));
logger.write({ timestamp: new Date().toISOString(), level: 'INFO', message: 'Started' });
logger.end();
```

## Best Practices Checklist

- [ ] Implement `_construct()` for async resource initialization
- [ ] Implement `_destroy()` for cleanup — close file handles, clear timers
- [ ] Set appropriate `highWaterMark` based on data size and processing speed
- [ ] Use `objectMode: true` when streaming JavaScript objects
- [ ] Return `false` from `push()` to handle backpressure internally
- [ ] Use `cork()`/`uncork()` when batching small writes
- [ ] Call `callback(err)` in `_write()` on errors to propagate them
- [ ] Use `stream.finished()` for reliable cleanup on stream close/error
- [ ] Pass `signal` option for AbortController-based cancellation
- [ ] Implement `_writev()` for efficient batch writes when using cork()

## Cross-References

- See [Duplex and Pipeline](./01-duplex-passthrough-pipeline.md) for combining readable and writable
- See [Stream Events and Emitters](./03-stream-events-emitters.md) for event lifecycle
- See [Backpressure Performance](./02-backpressure-performance.md) for tuning buffer sizes
- See [Stream Error Handling](../07-stream-error-handling/01-error-patterns.md) for error propagation
- See [File System Streams](../04-stream-filesystem-integration/01-file-read-write-advanced.md) for file-based streams

## Next Steps

Continue to [Stream Compose and Web Interop](./05-stream-compose-web-interop.md) for modern stream APIs and Web Streams integration.
