# Modern Stream APIs and Web Interop

## What You'll Learn

- `stream.compose()` (Node 16+) for stream construction
- `Readable.from()` creating readable from iterators/async generators
- `Readable.fromWeb()` and `Writable.toWeb()` for WHATWG stream interop
- `stream/promises` API (`pipeline`, `finished`)
- `stream/consumers` (`arrayBuffer`, `blob`, `text`, `json`, `buffer`)
- Web Streams API comparison (`ReadableStream`, `WritableStream`, `TransformStream`)
- Migration patterns between Node.js streams and Web Streams
- Real-world examples: async generator as readable stream, web stream adapter

## stream.compose()

`stream.compose()` (Node 16+) constructs a Duplex stream from readable and writable pairs, or from a list of streams.

```javascript
import { compose, Duplex, Readable, Writable } from 'node:stream';

// Compose a Duplex from separate read and write sides
const input = Readable.from(['chunk1', 'chunk2', 'chunk3']);
const output = new Writable({
    write(chunk, encoding, callback) {
        console.log('Received:', chunk.toString());
        callback();
    }
});

const duplex = compose({ readable: input, writable: output });
duplex.write('hello');
duplex.end();

// Compose from a list of streams
const composite = compose(
    Readable.from(['a', 'b', 'c']),
    new Duplex({
        read() {},
        write(chunk, encoding, callback) {
            this.push(chunk.toString().toUpperCase());
            callback();
        }
    })
);

for await (const chunk of composite) {
    console.log(chunk); // 'A', 'B', 'C'
}
```

## Readable.from() — Iterators and Async Generators

`Readable.from()` creates a Readable stream from any iterable or async iterable.

```javascript
import { Readable } from 'node:stream';

// From synchronous iterables (arrays, Sets, Maps, generators)
const fromArray = Readable.from(['alpha', 'beta', 'gamma']);
for await (const chunk of fromArray) {
    console.log(chunk); // alpha, beta, gamma
}

// With AbortController
const controller = new AbortController();
const stream = Readable.from(generateNumbers(), { signal: controller.signal });
stream.on('data', (n) => { if (n >= 5) controller.abort(); });
```

### Async Generator as Readable Stream

```javascript
import { Readable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';
import { createWriteStream } from 'node:fs';

async function* fetchAllUsers(baseUrl, pageSize = 100) {
    let page = 0;
    let hasMore = true;

    while (hasMore) {
        const res = await fetch(`${baseUrl}/users?page=${page}&size=${pageSize}`);
        const data = await res.json();
        yield* data.users;
        hasMore = data.users.length === pageSize;
        page++;
    }
}

const userStream = Readable.from(
    fetchAllUsers('https://api.example.com', 50),
    { objectMode: true }
);

const formatUser = new Transform({
    objectMode: true,
    transform(user, encoding, callback) {
        callback(null, `${user.id}\t${user.name}\t${user.email}\n`);
    }
});

await pipeline(userStream, formatUser, createWriteStream('users.tsv'));
```

### Database Cursor as Readable Stream

```javascript
import { Readable } from 'node:stream';

async function* cursorGenerator(cursor, batchSize = 100) {
    try {
        while (true) {
            const batch = await cursor.next(batchSize);
            if (!batch || batch.length === 0) break;
            yield* batch;
        }
    } finally { if (cursor?.close) await cursor.close(); }
}

for await (const doc of Readable.from(cursorGenerator(myCursor))) {
    await processDocument(doc);
}
```

## WHATWG Web Streams Interop

Node.js provides bridge methods to convert between Node.js streams and the Web Streams API.

### Readable.fromWeb()

```javascript
import { Readable } from 'node:stream';

// Convert Web ReadableStream (e.g., fetch response) to Node.js Readable
async function convertWebReadable() {
    const response = await fetch('https://api.example.com/data');
    for await (const chunk of Readable.fromWeb(response.body)) {
        process.stdout.write(chunk);
    }
}
```

### Writable.toWeb()

```javascript
import { Writable } from 'node:stream';

const nodeWritable = new Writable({
    write(chunk, encoding, callback) {
        console.log('Written:', chunk.toString());
        callback();
    }
});

// Convert to Web WritableStream
const webWritable = Writable.toWeb(nodeWritable);
const writer = webWritable.getWriter();
await writer.write(new TextEncoder().encode('Hello from Web Streams'));
await writer.close();
```

## stream/promises API

```javascript
import { pipeline, finished } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';
import { createGzip } from 'node:zlib';

// Promise-based pipeline with timeout
async function compressFile(input, output) {
    const ac = new AbortController();
    const timeout = setTimeout(() => ac.abort(), 30_000);

    try {
        await pipeline(
            createReadStream(input),
            createGzip(),
            createWriteStream(output),
            { signal: ac.signal }
        );
    } catch (err) {
        if (err.name === 'AbortError') console.error('Timed out');
        else throw err;
    } finally {
        clearTimeout(timeout);
    }
}

// Promise-based finished() for cleanup
async function readAndCleanup(filePath) {
    const stream = createReadStream(filePath, { encoding: 'utf8' });
    const chunks = [];
    stream.on('data', (chunk) => chunks.push(chunk));
    await finished(stream); // Guaranteed closed
    return chunks.join('');
}

// Parallel pipelines: await Promise.all(inputs.map(i => pipeline(...)))
```

## stream/consumers

Node 16+ provides consumers for consuming entire streams into memory as specific types.

```javascript
import { arrayBuffer, blob, text, json, buffer } from 'node:stream/consumers';
import { createReadStream } from 'node:fs';
import { Readable } from 'node:stream';

const content = await text(createReadStream('data.txt'));     // String
const config = await json(createReadStream('config.json'));   // Parsed JSON
const data = await buffer(createReadStream('binary.dat'));    // Buffer
const arrayBuf = await arrayBuffer(createReadStream('data.bin')); // ArrayBuffer
const fileBlob = await blob(createReadStream('image.png'));   // Blob

// From fetch response
async function fetchAndParse(url) {
    const response = await fetch(url);
    const body = Readable.fromWeb(response.body);
    return await json(body);
}
```

## Web Streams API Comparison

```
Node.js Streams vs Web Streams API:
──────────────────────────────────────────────────────────────
Feature              Node.js Stream           Web Stream (WHATWG)
Readable             stream.Readable          ReadableStream
Writable             stream.Writable          WritableStream
Transform            stream.Transform         TransformStream
Duplex               stream.Duplex            (no equivalent)
Pipe                 .pipe(dest)              readable.pipeTo(writable)
Cancel               .destroy()               reader.cancel()
Locking              N/A                      reader (exclusive)
Backpressure         push() return value      controller.desiredSize
──────────────────────────────────────────────────────────────
Web Streams: browser compatible, built-in locking, WHATWG standard
Node.js Streams: auto backpressure, Duplex support, native C++ I/O
──────────────────────────────────────────────────────────────
```

### Web Streams Example

```javascript
const webReadable = new ReadableStream({
    start(controller) {
        controller.enqueue(new TextEncoder().encode('chunk 1'));
        controller.enqueue(new TextEncoder().encode('chunk 2'));
        controller.close();
    },
});

const webTransform = new TransformStream({
    transform(chunk, controller) {
        const text = new TextDecoder().decode(chunk);
        controller.enqueue(new TextEncoder().encode(text.toUpperCase()));
    }
});

const webWritable = new WritableStream({
    write(chunk) {
        console.log(new TextDecoder().decode(chunk));
    }
});

await webReadable.pipeThrough(webTransform).pipeTo(webWritable);
// Output: CHUNK 1, CHUNK 2
```

## Migration Patterns

### Web Stream → Node.js Pipeline

```javascript
import { Readable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createWriteStream } from 'node:fs';

async function downloadToFile(url, outputPath) {
    const response = await fetch(url);
    if (!response.ok) throw new Error(`HTTP ${response.status}`);
    await pipeline(Readable.fromWeb(response.body), createWriteStream(outputPath));
}
```

### Node.js Stream → Web Response

```javascript
import { Readable } from 'node:stream';

async function streamFileAsResponse(filePath) {
    const { createReadStream } = await import('node:fs');
    const webReadable = Readable.toWeb(createReadStream(filePath));

    return new Response(webReadable, {
        headers: { 'Content-Type': 'text/plain', 'Transfer-Encoding': 'chunked' },
    });
}
```

### Web WritableStream → Node.js Writable

```javascript
import { Writable } from 'node:stream';

function webWritableToNode(webWritableStream) {
    const writer = webWritableStream.getWriter();

    return new Writable({
        async write(chunk, encoding, callback) {
            try { await writer.ready; await writer.write(chunk); callback(); }
            catch (err) { callback(err); }
        },
        async final(callback) {
            try { await writer.close(); callback(); }
            catch (err) { callback(err); }
        },
        async destroy(err, callback) {
            try { await writer.abort(err); } catch { /* ignore */ }
            callback(err);
        }
    });
}
```

## Real-World: Async Generator Stream Adapter

```javascript
import { Readable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

async function* fetchFromSource(sourceUrl) {
    let cursor = 0;
    while (true) {
        const res = await fetch(`${sourceUrl}?offset=${cursor}&limit=50`);
        const batch = await res.json();
        if (batch.length === 0) return;
        yield* batch;
        cursor += 50;
    }
}

async function* mergeSources(...sources) {
    for (const source of sources) yield* source;
}

const deduplicate = new Transform({
    objectMode: true,
    construct(callback) { this.seen = new Set(); callback(); },
    transform(chunk, encoding, callback) {
        const key = chunk.id ?? JSON.stringify(chunk);
        if (!this.seen.has(key)) { this.seen.add(key); callback(null, chunk); }
        else callback();
    },
    flush(callback) { this.seen.clear(); callback(); }
});

await pipeline(
    Readable.from(
        mergeSources(
            fetchFromSource('https://api.source-a.com/items'),
            fetchFromSource('https://api.source-b.com/items'),
        ),
        { objectMode: true }
    ),
    deduplicate,
    new Transform({
        objectMode: true,
        transform(item, encoding, callback) {
            callback(null, JSON.stringify(item) + '\n');
        }
    }),
    process.stdout
);
```

## Best Practices Checklist

- [ ] Use `Readable.from()` with async generators for data source streams
- [ ] Use `stream/promises` (`pipeline`, `finished`) instead of callback versions
- [ ] Use `stream/consumers` when you need entire stream content in memory
- [ ] Use `Readable.fromWeb()` / `Writable.toWeb()` for WHATWG stream interop
- [ ] Use `stream.compose()` to create composite Duplex streams
- [ ] Prefer Web Streams for cross-runtime compatibility (Node, Deno, Bun, browsers)
- [ ] Use Node.js native streams for file and network I/O (better performance)
- [ ] Pass `signal` option for AbortController integration
- [ ] Wrap async generators in `Readable.from()` instead of manual subclasses

## Cross-References

- See [Readable and Writable Internals](./04-readable-writable-internals.md) for custom stream implementation
- See [Stream Events and Emitters](./03-stream-events-emitters.md) for async iteration patterns
- See [Pipeline and Backpressure](./01-duplex-passthrough-pipeline.md) for pipeline composition
- See [Compression and Encryption](../03-stream-processing-patterns/03-compression-encryption.md) for transform pipelines
- See [HTTP Streaming](../05-stream-network-operations/01-http-streaming.md) for web server stream patterns

## Next Steps

Continue to [Buffer Mastery](../02-buffer-mastery/01-buffer-creation-memory.md) for binary data fundamentals that underpin stream operations.
