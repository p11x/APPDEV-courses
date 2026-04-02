# Buffer and Stream Integration Patterns

## What You'll Learn

- Buffer to stream conversion patterns
- Stream to buffer accumulation with size limits
- `TextEncoder`/`TextDecoder` for encoding in streams
- `Buffer.from()` on stream chunk patterns
- Large buffer chunked processing with streams
- `Buffer.concat()` optimization for stream chunks
- Binary protocol stream framing (length-prefixed messages)
- Blob and File API with streams
- Real-world binary message protocol over TCP

## Buffer to Stream Conversion

```javascript
import { Readable } from 'node:stream';

// Chunked buffer → readable stream
function bufferToStream(buffer, chunkSize = 64 * 1024) {
    let offset = 0;
    return new Readable({
        read() {
            if (offset >= buffer.length) { this.push(null); return; }
            const end = Math.min(offset + chunkSize, buffer.length);
            this.push(buffer.subarray(offset, end));
            offset = end;
        },
    });
}

const data = Buffer.alloc(256 * 1024, 'A');
bufferToStream(data, 64 * 1024).on('data', (chunk) => {
    console.log(`Chunk: ${chunk.length} bytes`);
});
// Chunk: 65536 bytes (×4)

// Async iterator version
async function* chunkBuffer(buffer, size) {
    for (let i = 0; i < buffer.length; i += size) {
        yield buffer.subarray(i, Math.min(i + size, buffer.length));
    }
}
```

## Stream to Buffer Accumulation

```javascript
// Safe accumulation with size limit
async function streamToBuffer(stream, maxSize = 10 * 1024 * 1024) {
    const chunks = [];
    let totalSize = 0;
    for await (const chunk of stream) {
        totalSize += chunk.length;
        if (totalSize > maxSize) throw new Error(`Stream exceeded max: ${totalSize}`);
        chunks.push(chunk);
    }
    return Buffer.concat(chunks);
}

import { Readable } from 'node:stream';
const source = Readable.from([Buffer.from('Hello '), Buffer.from('World!')]);
const result = await streamToBuffer(source, 1024);
console.log(result.toString()); // 'Hello World!'
```

## TextEncoder/TextDecoder in Streams

```javascript
import { Transform } from 'node:stream';

class EncodingTransform extends Transform {
    #encoder = new TextEncoder();
    #decoder = new TextDecoder('utf-8', { fatal: true });

    _transform(chunk, encoding, callback) {
        const text = this.#decoder.decode(chunk, { stream: true });
        const processed = text.toUpperCase();
        this.push(Buffer.from(this.#encoder.encode(processed).buffer));
        callback();
    }

    _flush(callback) {
        const remaining = this.#decoder.decode();
        if (remaining) {
            this.push(Buffer.from(this.#encoder.encode(remaining).buffer));
        }
        callback();
    }
}

import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

await pipeline(
    createReadStream('input.txt'),
    new EncodingTransform(),
    createWriteStream('output.txt'),
);
```

## Buffer.from() on Stream Chunks

```javascript
import { Transform } from 'node:stream';

// Normalize any chunk type to Buffer
class BufferNormalizeTransform extends Transform {
    _transform(chunk, encoding, callback) {
        if (Buffer.isBuffer(chunk)) {
            this.push(chunk);
        } else if (chunk instanceof ArrayBuffer) {
            this.push(Buffer.from(chunk));
        } else if (ArrayBuffer.isView(chunk)) {
            this.push(Buffer.from(chunk.buffer, chunk.byteOffset, chunk.byteLength));
        } else {
            this.push(Buffer.from(String(chunk), encoding));
        }
        callback();
    }
}

import { Readable } from 'node:stream';
const mixed = Readable.from([
    Buffer.from('buffer'),
    new Uint8Array([72, 101, 108, 108, 111]),
    'string-chunk',
]);
for await (const chunk of mixed.pipe(new BufferNormalizeTransform())) {
    console.log(Buffer.isBuffer(chunk), chunk.length);
}
// true 6, true 5, true 11
```

## Large Buffer Chunked Processing

```javascript
import { Readable, Transform, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';

async function processLargeBuffer(buffer, processor, chunkSize = 64 * 1024) {
    let offset = 0;
    const source = new Readable({
        read() {
            if (offset >= buffer.length) { this.push(null); return; }
            const end = Math.min(offset + chunkSize, buffer.length);
            this.push(buffer.subarray(offset, end));
            offset = end;
        },
    });

    const chunks = [];
    await pipeline(
        source,
        new Transform({
            transform(chunk, enc, cb) { this.push(processor(chunk)); cb(); },
        }),
        new Writable({
            write(chunk, enc, cb) { chunks.push(chunk); cb(); },
        }),
    );
    return Buffer.concat(chunks);
}

const largeBuf = Buffer.alloc(10 * 1024 * 1024, 'a');
const result = await processLargeBuffer(
    largeBuf,
    (chunk) => Buffer.from(chunk.toString('utf8').toUpperCase()),
);
console.log(result.length, String.fromCharCode(result[0])); // 10485760 A
```

## Buffer.concat() Optimization

```javascript
import { performance } from 'node:perf_hooks';

// Naive: O(n²) due to repeated copies
function naiveConcat(chunks) {
    let result = Buffer.alloc(0);
    for (const chunk of chunks) result = Buffer.concat([result, chunk]);
    return result;
}

// Optimized: single allocation, single copy
function optimizedConcat(chunks) {
    const totalLength = chunks.reduce((sum, c) => sum + c.length, 0);
    const result = Buffer.alloc(totalLength);
    let offset = 0;
    for (const chunk of chunks) {
        chunk.copy(result, offset);
        offset += chunk.length;
    }
    return result;
}

const chunks = Array.from({ length: 1000 }, () => Buffer.alloc(1024));

let start = performance.now();
naiveConcat(chunks);
console.log(`Naive: ${(performance.now() - start).toFixed(1)}ms`);

start = performance.now();
optimizedConcat(chunks);
console.log(`Optimized: ${(performance.now() - start).toFixed(1)}ms`);
// Typical: Naive ~50ms, Optimized ~2ms
```

## Binary Protocol: Length-Prefixed Framing

```javascript
import { Transform } from 'node:stream';

// Frame: [length:4][type:1][payload:length-1]
class LengthPrefixedDecoder extends Transform {
    #buffer = Buffer.alloc(0);

    _transform(chunk, encoding, callback) {
        this.#buffer = Buffer.concat([this.#buffer, chunk]);

        while (this.#buffer.length >= 5) {
            const length = this.#buffer.readUInt32BE(0);
            if (this.#buffer.length < 4 + length) break;

            const type = this.#buffer.readUInt8(4);
            const payload = this.#buffer.subarray(5, 4 + length);
            this.#buffer = this.#buffer.subarray(4 + length);
            this.push({ type, payload });
        }
        callback();
    }
}

class LengthPrefixedEncoder extends Transform {
    _transform({ type, payload }, encoding, callback) {
        const frame = Buffer.alloc(4 + 1 + payload.length);
        frame.writeUInt32BE(1 + payload.length, 0);
        frame.writeUInt8(type, 4);
        payload.copy(frame, 5);
        this.push(frame);
        callback();
    }
}

// Encode a message
function encodeMessage(type, data) {
    const payload = Buffer.isBuffer(data) ? data : Buffer.from(data);
    const frame = Buffer.alloc(4 + 1 + payload.length);
    frame.writeUInt32BE(1 + payload.length, 0);
    frame.writeUInt8(type, 4);
    payload.copy(frame, 5);
    return frame;
}
```

## Blob and File API with Streams

```javascript
import { Blob, File } from 'node:buffer';

// Create Blob from buffers
const blob = new Blob([
    Buffer.from('{"name":"data",'),
    Buffer.from('"value":42}'),
], { type: 'application/json' });

console.log(blob.size); // 29

// Read Blob as stream
const chunks = [];
for await (const chunk of blob.stream()) {
    chunks.push(Buffer.from(chunk));
}
console.log(Buffer.concat(chunks).toString());
// {"name":"data","value":42}

// Blob → Buffer conversion
async function blobToBuffer(blob) {
    return Buffer.from(await blob.arrayBuffer());
}

// File API for named blobs
const file = new File(
    [Buffer.from('file content')],
    'data.bin',
    { type: 'application/octet-stream' },
);
console.log(file.name, file.size); // data.bin 12
```

## Real-World: Binary Message Protocol over TCP

```javascript
import { createServer, connect } from 'node:net';
import { Transform } from 'node:stream';

// Frame: [type:1][id:4][length:4][checksum:4][payload:length]
const HEADER_SIZE = 13;

class ProtocolCodec extends Transform {
    #buffer = Buffer.alloc(0);

    constructor(options) { super({ ...options, objectMode: true }); }

    _transform(chunk, encoding, callback) {
        this.#buffer = Buffer.concat([this.#buffer, chunk]);

        while (this.#buffer.length >= HEADER_SIZE) {
            const type = this.#buffer.readUInt8(0);
            const id = this.#buffer.readUInt32BE(1);
            const length = this.#buffer.readUInt32BE(5);
            const checksum = this.#buffer.readUInt32BE(9);

            if (this.#buffer.length < HEADER_SIZE + length) break;

            const payload = this.#buffer.subarray(HEADER_SIZE, HEADER_SIZE + length);

            let computed = 0;
            for (let i = 0; i < payload.length; i++) computed = (computed + payload[i]) >>> 0;
            if (computed !== checksum) {
                this.emit('error', new Error(`Checksum mismatch`));
                return callback();
            }

            this.#buffer = this.#buffer.subarray(HEADER_SIZE + length);
            this.push({ type, id, payload });
        }
        callback();
    }

    encode(type, id, payload) {
        const frame = Buffer.alloc(HEADER_SIZE + payload.length);
        frame.writeUInt8(type, 0);
        frame.writeUInt32BE(id, 1);
        frame.writeUInt32BE(payload.length, 5);
        let checksum = 0;
        for (let i = 0; i < payload.length; i++) checksum = (checksum + payload[i]) >>> 0;
        frame.writeUInt32BE(checksum, 9);
        payload.copy(frame, HEADER_SIZE);
        return frame;
    }
}

// Server
const server = createServer((socket) => {
    const codec = new ProtocolCodec();
    socket.pipe(codec);
    codec.on('data', ({ type, id, payload }) => {
        console.log(`Msg type=${type} id=${id}: ${payload.length} bytes`);
        socket.write(codec.encode(0x03, id, Buffer.alloc(0))); // ACK
    });
});
server.listen(3001);

// Client
const codec = new ProtocolCodec();
const client = connect(3001, 'localhost', () => {
    client.pipe(codec);
    client.write(codec.encode(0x01, 1, Buffer.from('Hello')));
    client.write(codec.encode(0x02, 2, Buffer.alloc(1024, 0xAB)));
});
codec.on('data', ({ type, id }) => {
    if (type === 0x03) console.log(`Ack for message ${id}`);
});
```

## Best Practices Checklist

- [ ] Use chunked streaming for buffers over 1MB to avoid memory spikes
- [ ] Set size limits when accumulating stream data into buffers
- [ ] Normalize stream chunks with `Buffer.from()` when type is uncertain
- [ ] Use `TextEncoder`/`TextDecoder` for streaming text encoding
- [ ] Pre-calculate total length before `Buffer.concat()` to avoid repeated copies
- [ ] Use `Buffer.subarray()` instead of `Buffer.slice()` for zero-copy views
- [ ] Implement proper frame boundary handling in binary protocols
- [ ] Validate checksums on decoded binary messages
- [ ] Handle partial reads in stream decoders (buffer incomplete frames)
- [ ] Use `Blob.stream()` for lazy evaluation of large binary data

## Cross-References

- See [Buffer Creation](./01-buffer-creation-memory.md) for allocation patterns
- See [Buffer Performance](./04-buffer-performance-security.md) for optimization techniques
- See [Stream Architecture](../01-streams-architecture/01-duplex-passthrough-pipeline.md) for stream fundamentals
- See [Transform Streams](../streams/04-transform-streams.md) for transform patterns
- See [Backpressure](../01-streams-architecture/02-backpressure-performance.md) for flow control
- See [Stream Compression](../03-stream-processing-patterns/03-compression-encryption.md) for encoding pipelines

## Next Steps

Continue to [Stream Processing Patterns](../03-stream-processing-patterns/01-transformation-pipelines.md).
