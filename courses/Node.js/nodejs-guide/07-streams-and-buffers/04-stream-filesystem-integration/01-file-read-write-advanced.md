# Stream File System Integration

## What You'll Learn

- Advanced file reading/writing with streams
- File monitoring and watching
- Large file handling optimization
- File stream error recovery
- Directory streaming

## Advanced File Streams

```javascript
import { createReadStream, createWriteStream, watch } from 'node:fs';
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';
import { opendir } from 'node:fs/promises';

// Tail a file (like `tail -f`)
async function tailFile(filePath, onData) {
    const stream = createReadStream(filePath, {
        encoding: 'utf8',
        start: 0,
    });

    let buffer = '';

    stream.on('data', (chunk) => {
        buffer += chunk;
        const lines = buffer.split('\n');
        buffer = lines.pop(); // Keep incomplete line
        lines.forEach(onData);
    });

    // Watch for changes
    const watcher = watch(filePath, (eventType) => {
        if (eventType === 'change') {
            // Re-read from current position
            // (simplified — production needs offset tracking)
        }
    });

    return { stream, watcher };
}
```

## File Watching with Streams

```javascript
import { watch } from 'node:fs';
import { Readable } from 'node:stream';

function watchAsStream(path, options = {}) {
    const events = [];

    const stream = new Readable({
        objectMode: true,
        read() {
            // Events pushed by watcher
        }
    });

    const watcher = watch(path, { recursive: options.recursive }, (eventType, filename) => {
        const event = {
            type: eventType,
            filename,
            timestamp: Date.now(),
        };
        stream.push(event);
    });

    watcher.on('error', (err) => stream.destroy(err));

    // Cleanup on stream close
    stream.on('close', () => watcher.close());

    return stream;
}

// Usage
const changes = watchAsStream('./data', { recursive: true });
changes.on('data', (event) => {
    console.log(`${event.type}: ${event.filename}`);
});
```

## Directory Streaming

```javascript
import { opendir } from 'node:fs/promises';
import { Readable } from 'node:stream';

function directoryStream(dirPath, options = {}) {
    return new Readable({
        objectMode: true,
        async read() {
            if (!this.dir) {
                this.dir = await opendir(dirPath);
            }

            const entry = await this.dir.read();
            if (entry) {
                this.push({
                    name: entry.name,
                    isDirectory: entry.isDirectory(),
                    isFile: entry.isFile(),
                });
            } else {
                await this.dir.close();
                this.push(null);
            }
        }
    });
}

// List all JS files in a directory tree
async function* findFiles(dir, pattern) {
    const dirStream = directoryStream(dir);
    for await (const entry of dirStream) {
        if (entry.isFile && pattern.test(entry.name)) {
            yield `${dir}/${entry.name}`;
        } else if (entry.isDirectory) {
            yield* findFiles(`${dir}/${entry.name}`, pattern);
        }
    }
}

for await (const file of findFiles('./src', /\.js$/)) {
    console.log(file);
}
```

## File Stream Error Recovery

```javascript
import { createReadStream, createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';

class ResilientFileStream {
    constructor(path, options = {}) {
        this.path = path;
        this.maxRetries = options.maxRetries || 3;
        this.retryDelay = options.retryDelay || 1000;
    }

    async readWithRetry(processChunk) {
        for (let attempt = 1; attempt <= this.maxRetries; attempt++) {
            try {
                const stream = createReadStream(this.path, { highWaterMark: 64 * 1024 });
                
                for await (const chunk of stream) {
                    await processChunk(chunk);
                }
                
                return;
            } catch (err) {
                if (attempt === this.maxRetries) throw err;
                console.warn(`Read attempt ${attempt} failed: ${err.message}`);
                await new Promise(r => setTimeout(r, this.retryDelay));
            }
        }
    }

    async writeWithRetry(data) {
        for (let attempt = 1; attempt <= this.maxRetries; attempt++) {
            try {
                const stream = createWriteStream(this.path, { flags: 'a' });
                stream.write(data);
                stream.end();
                return;
            } catch (err) {
                if (attempt === this.maxRetries) throw err;
                console.warn(`Write attempt ${attempt} failed: ${err.message}`);
                await new Promise(r => setTimeout(r, this.retryDelay));
            }
        }
    }
}
```

## Best Practices Checklist

- [ ] Use `highWaterMark` tuned for your storage type (SSD vs HDD)
- [ ] Handle `ENOENT` and `EACCES` errors gracefully
- [ ] Use `watch()` for real-time file monitoring
- [ ] Close file handles in error paths
- [ ] Use `pipeline()` for automatic cleanup
- [ ] Test with large files to validate memory usage

## Cross-References

- See [Readable Streams](../streams/01-readable-streams.md) for readable basics
- See [Writable Streams](../streams/02-writable-streams.md) for writable basics
- See [File Processing](../03-stream-processing-patterns/02-file-processing-streams.md) for processing

## Next Steps

Continue to [Network Operations](../05-stream-network-operations/01-http-streaming.md).
