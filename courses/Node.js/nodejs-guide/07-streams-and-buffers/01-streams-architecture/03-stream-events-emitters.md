# Stream Events and Event Emitter Integration

## What You'll Learn

- Complete stream event lifecycle
- Custom stream event emission
- Integrating streams with EventEmitter
- Async iteration over streams
- Stream state management

## Stream Lifecycle Events

```javascript
import { createReadStream } from 'node:fs';

const stream = createReadStream('data.txt', { encoding: 'utf8' });

// Lifecycle in order:
stream.on('open', (fd) => {
    console.log('Stream opened, file descriptor:', fd);
});

stream.on('data', (chunk) => {
    console.log('Data received:', chunk.length, 'bytes');
});

stream.on('end', () => {
    console.log('No more data');
});

stream.on('close', () => {
    console.log('Stream closed, resources released');
});

// Error can happen at any point
stream.on('error', (err) => {
    console.error('Stream error:', err.message);
});
```

## Async Iteration over Streams

```javascript
import { createReadStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';

// for-await-of loop (recommended for simple cases)
async function processFile(filePath) {
    const stream = createReadStream(filePath, {
        encoding: 'utf8',
        highWaterMark: 64 * 1024,
    });

    let lineCount = 0;
    let buffer = '';

    for await (const chunk of stream) {
        buffer += chunk;
        const lines = buffer.split('\n');
        buffer = lines.pop(); // Keep incomplete line

        for (const line of lines) {
            lineCount++;
            await processLine(line, lineCount);
        }
    }

    // Process remaining buffer
    if (buffer) {
        lineCount++;
        await processLine(buffer, lineCount);
    }

    return lineCount;
}

// Async iteration with Transform stream
async function transformWithAsyncIterator() {
    const parser = new Transform({
        objectMode: true,
        transform(chunk, encoding, callback) {
            const lines = chunk.toString().split('\n');
            for (const line of lines) {
                if (line.trim()) {
                    try {
                        this.push(JSON.parse(line));
                    } catch {
                        // Skip invalid lines
                    }
                }
            }
            callback();
        }
    });

    const input = createReadStream('data.jsonl');
    input.pipe(parser);

    const records = [];
    for await (const record of parser) {
        records.push(record);
    }

    return records;
}
```

## Custom Readable with Events

```javascript
import { Readable } from 'node:stream';
import { EventEmitter } from 'node:events';

class DataGenerator extends Readable {
    constructor(options) {
        super({ ...options, objectMode: true });
        this.interval = options.interval || 100;
        this.maxRecords = options.maxRecords || 1000;
        this.count = 0;
        this.timer = null;
    }

    _read() {
        if (this.timer) return; // Already generating

        this.timer = setInterval(() => {
            if (this.count >= this.maxRecords) {
                clearInterval(this.timer);
                this.push(null);
                return;
            }

            const record = {
                id: this.count++,
                timestamp: Date.now(),
                value: Math.random() * 100,
            };

            const canContinue = this.push(record);

            if (!canContinue) {
                clearInterval(this.timer);
                this.timer = null;
            }
        }, this.interval);
    }

    _destroy(err, callback) {
        if (this.timer) {
            clearInterval(this.timer);
            this.timer = null;
        }
        callback(err);
    }
}

// Usage with events
const generator = new DataGenerator({ interval: 10, maxRecords: 100 });

generator.on('data', (record) => {
    if (record.id % 25 === 0) {
        console.log(`Record ${record.id}: ${record.value.toFixed(2)}`);
    }
});

generator.on('end', () => {
    console.log('Generator finished');
});
```

## Stream-to-EventBridge Pattern

```javascript
import { EventEmitter } from 'node:events';
import { Transform } from 'node:stream';

class StreamEventBridge extends EventEmitter {
    constructor() {
        super();
    }

    createTransform(eventName) {
        return new Transform({
            objectMode: true,
            transform: (chunk, encoding, callback) => {
                this.emit(eventName, chunk);
                callback(null, chunk); // Pass through
            }
        });
    }

    createFilter(predicate) {
        return new Transform({
            objectMode: true,
            transform(chunk, encoding, callback) {
                if (predicate(chunk)) {
                    callback(null, chunk);
                } else {
                    callback(); // Drop
                }
            }
        });
    }
}

// Usage
import { pipeline } from 'node:stream/promises';
import { createReadStream } from 'node:fs';

const bridge = new StreamEventBridge();

bridge.on('record', (record) => {
    // React to each record as an event
    if (record.error) {
        bridge.emit('alert', record);
    }
});

bridge.on('alert', (record) => {
    console.error('ALERT:', record);
});

await pipeline(
    createReadStream('logs.jsonl'),
    bridge.createTransform('record'),
    bridge.createFilter(r => r.level !== 'debug'),
    process.stdout
);
```

## Paused vs Flowing Mode

```
Readable Stream Modes:
─────────────────────────────────────────────
Flowing Mode:
├── Data events emitted automatically
├── Triggered by: .on('data'), .pipe(), .resume()
├── Fast, continuous data delivery
└── Use when: consuming all data immediately

Paused Mode:
├── Must call .read() to get data
├── Triggered by: .pause(), no 'data' listener
├── Controlled data consumption
└── Use when: processing at your own pace

Mode transitions:
  .on('data')   → flowing
  .pause()      → paused
  .resume()     → flowing
  .pipe(dest)   → flowing
  .unpipe()     → paused (if no 'data' listener)
```

```javascript
// Flowing mode (automatic)
stream.on('data', (chunk) => {
    console.log('Auto-received:', chunk.length);
});

// Paused mode (manual)
stream.pause();
stream.on('readable', () => {
    let chunk;
    while ((chunk = stream.read()) !== null) {
        console.log('Manual-read:', chunk.length);
    }
});

// Switch modes
stream.pause();  // → paused
stream.resume(); // → flowing
```

## Best Practices Checklist

- [ ] Use `for await...of` for simple stream consumption
- [ ] Always handle `error` and `close` events
- [ ] Clean up timers/resources in `_destroy` method
- [ ] Use objectMode for structured data streams
- [ ] Prefer async iteration over manual event handling
- [ ] Use AbortController for cancellation

## Cross-References

- See [Duplex and Pipeline](./01-duplex-passthrough-pipeline.md) for pipeline patterns
- See [Stream Error Handling](../07-stream-error-handling/01-error-patterns.md) for error events
- See [Readable Streams](../streams/01-readable-streams.md) for readable basics

## Next Steps

Continue to [Buffer Mastery](../02-buffer-mastery/01-buffer-creation-memory.md) for buffer fundamentals.
