# Buffer Manipulation, Encoding, and Binary Processing

## What You'll Learn

- Buffer manipulation techniques
- Encoding/decoding patterns
- Binary protocol implementation
- Buffer comparison and search
- Large buffer handling strategies

## Buffer Manipulation

```javascript
const buf = Buffer.from('Hello, World!');

// Slicing (creates a view, not a copy)
const slice = buf.slice(0, 5); // 'Hello'

// Copying
const copy = Buffer.alloc(buf.length);
buf.copy(copy);

// Concatenation
const combined = Buffer.concat([Buffer.from('Hello '), Buffer.from('World')]);

// Fill
buf.fill(0); // Zero out

// Write at offset
const target = Buffer.alloc(20);
target.write('Hello', 0, 'utf8');
target.write(' World', 5, 'utf8');

// Read/write integers
const buf2 = Buffer.alloc(4);
buf2.writeUInt32BE(0xDEADBEEF, 0);
console.log(buf2.readUInt32BE(0)); // 3735928559

// Byte comparison
const a = Buffer.from('hello');
const b = Buffer.from('hello');
console.log(a.equals(b)); // true
console.log(a.compare(b)); // 0 (equal)

// Search
const data = Buffer.from('The quick brown fox jumps over the lazy dog');
console.log(data.indexOf('fox'));     // 16
console.log(data.includes('brown'));  // true
```

## Encoding Patterns

```javascript
// UTF-8 ↔ Buffer
const utf8Buf = Buffer.from('Hello 世界', 'utf8');
console.log(utf8Buf.toString('utf8')); // 'Hello 世界'

// Hex encoding
const hexBuf = Buffer.from('deadbeef', 'hex');
console.log(hexBuf.toString('hex')); // 'deadbeef'

// Base64 encoding
const b64Buf = Buffer.from('Hello World').toString('base64');
console.log(b64Buf); // 'SGVsbG8gV29ybGQ='
console.log(Buffer.from(b64Buf, 'base64').toString()); // 'Hello World'

// URL-safe Base64
function base64UrlEncode(buf) {
    return buf.toString('base64')
        .replace(/\+/g, '-')
        .replace(/\//g, '_')
        .replace(/=+$/, '');
}

function base64UrlDecode(str) {
    str = str.replace(/-/g, '+').replace(/_/g, '/');
    while (str.length % 4) str += '=';
    return Buffer.from(str, 'base64');
}

// Latin1 (ISO-8859-1)
const latin1 = Buffer.from('café', 'latin1');
console.log(latin1.toString('latin1')); // 'café'

// ASCII (strips high bits)
const ascii = Buffer.from('Hello\x80World', 'ascii');
console.log(ascii.toString()); // 'Hello?World'
```

## Binary Protocol Implementation

```javascript
// Message format: [type:1][length:2][payload:length][checksum:4]

class BinaryProtocol {
    static TYPE_STRING = 0x01;
    static TYPE_INT32 = 0x02;
    static TYPE_FLOAT = 0x03;

    static encode(type, payload) {
        let payloadBuf;

        switch (type) {
            case this.TYPE_STRING:
                payloadBuf = Buffer.from(String(payload), 'utf8');
                break;
            case this.TYPE_INT32:
                payloadBuf = Buffer.alloc(4);
                payloadBuf.writeInt32BE(payload, 0);
                break;
            case this.TYPE_FLOAT:
                payloadBuf = Buffer.alloc(4);
                payloadBuf.writeFloatBE(payload, 0);
                break;
            default:
                throw new Error(`Unknown type: ${type}`);
        }

        const checksum = this.computeChecksum(payloadBuf);
        const message = Buffer.alloc(1 + 2 + payloadBuf.length + 4);
        
        message.writeUInt8(type, 0);
        message.writeUInt16BE(payloadBuf.length, 1);
        payloadBuf.copy(message, 3);
        message.writeUInt32BE(checksum, 3 + payloadBuf.length);

        return message;
    }

    static decode(message) {
        const type = message.readUInt8(0);
        const length = message.readUInt16BE(1);
        const payload = message.slice(3, 3 + length);
        const checksum = message.readUInt32BE(3 + length);

        if (this.computeChecksum(payload) !== checksum) {
            throw new Error('Checksum mismatch');
        }

        let value;
        switch (type) {
            case this.TYPE_STRING:
                value = payload.toString('utf8');
                break;
            case this.TYPE_INT32:
                value = payload.readInt32BE(0);
                break;
            case this.TYPE_FLOAT:
                value = payload.readFloatBE(0);
                break;
        }

        return { type, value, length, checksum };
    }

    static computeChecksum(buf) {
        let sum = 0;
        for (let i = 0; i < buf.length; i++) {
            sum = (sum + buf[i]) >>> 0;
        }
        return sum;
    }
}

// Usage
const msg = BinaryProtocol.encode(BinaryProtocol.TYPE_STRING, 'Hello World');
console.log('Encoded:', msg.toString('hex'));
console.log('Decoded:', BinaryProtocol.decode(msg));
```

## Large Buffer Handling

```javascript
import { Transform } from 'node:stream';

// Chunk-based processing for large buffers
class LargeBufferProcessor extends Transform {
    constructor(chunkSize = 64 * 1024) {
        super();
        this.chunkSize = chunkSize;
        this.buffer = Buffer.alloc(0);
    }

    _transform(chunk, encoding, callback) {
        this.buffer = Buffer.concat([this.buffer, chunk]);

        while (this.buffer.length >= this.chunkSize) {
            const processChunk = this.buffer.slice(0, this.chunkSize);
            this.buffer = this.buffer.slice(this.chunkSize);
            
            // Process chunk
            const result = this.processChunk(processChunk);
            this.push(result);
        }

        callback();
    }

    _flush(callback) {
        if (this.buffer.length > 0) {
            this.push(this.processChunk(this.buffer));
        }
        callback();
    }

    processChunk(chunk) {
        // Example: reverse bytes
        return Buffer.from(chunk).reverse();
    }
}
```

## Buffer Search and Pattern Matching

```javascript
// Binary pattern search
function findPattern(buffer, pattern) {
    const positions = [];
    let pos = 0;

    while ((pos = buffer.indexOf(pattern, pos)) !== -1) {
        positions.push(pos);
        pos += 1;
    }

    return positions;
}

// Find all occurrences of a byte sequence
const data = Buffer.from([0x01, 0x02, 0x03, 0x01, 0x02, 0x04, 0x01, 0x02, 0x03]);
const pattern = Buffer.from([0x01, 0x02, 0x03]);
console.log(findPattern(data, pattern)); // [0, 6]

// Regex-like byte pattern matching
function matchBytePattern(buffer, pattern) {
    // pattern: array of byte values or null (wildcard)
    // e.g., [0x89, 0x50, null, 0x47] matches PNG header
    const matches = [];

    for (let i = 0; i <= buffer.length - pattern.length; i++) {
        let match = true;
        for (let j = 0; j < pattern.length; j++) {
            if (pattern[j] !== null && buffer[i + j] !== pattern[j]) {
                match = false;
                break;
            }
        }
        if (match) matches.push(i);
    }

    return matches;
}
```

## Best Practices Checklist

- [ ] Use `Buffer.concat()` for combining buffers
- [ ] Slice creates views (zero-copy) — be aware of memory retention
- [ ] Use `Buffer.alloc()` for security-sensitive data
- [ ] Implement checksums for binary protocols
- [ ] Process large buffers in chunks to avoid memory exhaustion
- [ ] Use correct encoding for your data type
- [ ] Validate buffer lengths before reading

## Cross-References

- See [Buffer Creation](./01-buffer-creation-memory.md) for creation methods
- See [Binary Data](../buffers/02-binary-data.md) for binary fundamentals
- See [Stream Processing](../03-stream-processing-patterns/01-transformation-pipelines.md) for stream patterns

## Next Steps

Continue to [Buffer Advanced Patterns](./03-buffer-advanced-patterns.md).
