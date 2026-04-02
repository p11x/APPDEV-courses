# Advanced Buffer Patterns: Leak Prevention, Performance, and Security

## What You'll Learn

- Buffer memory leak detection and prevention
- Buffer performance profiling
- Buffer security best practices
- Real-world buffer use cases
- Buffer testing strategies

## Memory Leak Prevention

```javascript
// Common leak: holding references to sliced buffers
function leakingPattern() {
    const largeBuffer = Buffer.alloc(10 * 1024 * 1024); // 10MB
    
    // Slice keeps reference to parent buffer
    const header = largeBuffer.slice(0, 100);
    
    // largeBuffer can't be GC'd while header exists
    return header;
}

// Safe pattern: copy only what you need
function safePattern() {
    const largeBuffer = Buffer.alloc(10 * 1024 * 1024);
    
    // Copy — allows parent to be GC'd
    const header = Buffer.alloc(100);
    largeBuffer.copy(header, 0, 0, 100);
    
    return header;
}

// Leak detection with --inspect
// Run: node --inspect --max-old-space-size=512 app.js
// Chrome DevTools → Memory → Take heap snapshot

class BufferLeakDetector {
    constructor() {
        this.allocations = new Map();
        this.totalAllocated = 0;
    }

    track(buf, label = 'unknown') {
        this.allocations.set(buf, { size: buf.length, label, time: Date.now() });
        this.totalAllocated += buf.length;
    }

    untrack(buf) {
        const info = this.allocations.get(buf);
        if (info) {
            this.totalAllocated -= info.size;
            this.allocations.delete(buf);
        }
    }

    report() {
        const byLabel = {};
        for (const [, info] of this.allocations) {
            byLabel[info.label] = (byLabel[info.label] || 0) + info.size;
        }

        return {
            totalTracked: this.allocations.size,
            totalBytes: this.totalAllocated,
            byLabel,
        };
    }
}

const detector = new BufferLeakDetector();

const buf = Buffer.alloc(1024);
detector.track(buf, 'temp-buffer');

console.log(detector.report());
// { totalTracked: 1, totalBytes: 1024, byLabel: { 'temp-buffer': 1024 } }

detector.untrack(buf);
```

## Buffer Performance Profiling

```javascript
import { performance } from 'node:perf_hooks';

function profileBufferOps() {
    const SIZE = 1024 * 1024; // 1MB
    const ops = {};

    // Allocation
    let start = performance.now();
    for (let i = 0; i < 1000; i++) Buffer.alloc(SIZE);
    ops.alloc = performance.now() - start;

    start = performance.now();
    for (let i = 0; i < 1000; i++) Buffer.allocUnsafe(SIZE);
    ops.allocUnsafe = performance.now() - start;

    // Copy
    const src = Buffer.alloc(SIZE, 0xAA);
    const dst = Buffer.alloc(SIZE);
    
    start = performance.now();
    for (let i = 0; i < 1000; i++) src.copy(dst);
    ops.copy = performance.now() - start;

    // Concat
    const bufs = Array.from({ length: 100 }, () => Buffer.alloc(1024));
    start = performance.now();
    for (let i = 0; i < 1000; i++) Buffer.concat(bufs);
    ops.concat = performance.now() - start;

    // Slice
    start = performance.now();
    for (let i = 0; i < 100000; i++) src.slice(0, 1024);
    ops.slice = performance.now() - start;

    // String conversion
    start = performance.now();
    for (let i = 0; i < 1000; i++) src.toString('base64');
    ops.toBase64 = performance.now() - start;

    start = performance.now();
    for (let i = 0; i < 1000; i++) src.toString('hex');
    ops.toHex = performance.now() - start;

    return ops;
}

console.table(profileBufferOps());
```

## Security Patterns

```javascript
import { timingSafeEqual, randomBytes } from 'node:crypto';

// Timing-safe comparison (prevents timing attacks)
function secureCompare(a, b) {
    const bufA = Buffer.from(a);
    const bufB = Buffer.from(b);
    
    if (bufA.length !== bufB.length) return false;
    return timingSafeEqual(bufA, bufB);
}

// Secure random buffer generation
function generateToken(bytes = 32) {
    return randomBytes(bytes).toString('hex');
}

// Secure buffer cleanup
class SecureBuffer {
    constructor(size) {
        this.buffer = Buffer.alloc(size);
    }

    write(data, encoding = 'utf8') {
        this.buffer.write(data, encoding);
    }

    read(encoding = 'utf8') {
        return this.buffer.toString(encoding);
    }

    destroy() {
        // Overwrite with random data multiple times
        for (let i = 0; i < 3; i++) {
            randomBytes(this.buffer.length).copy(this.buffer);
        }
        this.buffer.fill(0);
    }
}

// Prevent Buffer.from() injection
function safeBufferFrom(input, encoding) {
    if (typeof input !== 'string' && !Buffer.isBuffer(input) && !Array.isArray(input)) {
        throw new TypeError('Invalid buffer input');
    }
    return Buffer.from(input, encoding);
}
```

## Real-World Use Case: Log Parser

```javascript
import { createReadStream } from 'node:fs';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

// Parse binary log format: [timestamp:8][level:1][length:2][message:length]
class LogParser extends Transform {
    constructor() {
        super({ objectMode: true });
        this.buffer = Buffer.alloc(0);
    }

    _transform(chunk, encoding, callback) {
        this.buffer = Buffer.concat([this.buffer, chunk]);

        while (this.buffer.length >= 11) { // Minimum: 8 + 1 + 2
            const timestamp = Number(this.buffer.readBigUInt64BE(0));
            const level = this.buffer.readUInt8(8);
            const length = this.buffer.readUInt16BE(9);

            if (this.buffer.length < 11 + length) break;

            const message = this.buffer.slice(11, 11 + length).toString('utf8');
            this.buffer = this.buffer.slice(11 + length);

            this.push({
                timestamp: new Date(timestamp),
                level: ['', 'DEBUG', 'INFO', 'WARN', 'ERROR'][level] || 'UNKNOWN',
                message,
            });
        }

        callback();
    }
}

// Usage
await pipeline(
    createReadStream('app.log'),
    new LogParser(),
    async function* (source) {
        for await (const log of source) {
            if (log.level === 'ERROR') {
                console.error(`[${log.timestamp.toISOString()}] ${log.message}`);
            }
        }
    }
);
```

## Buffer Testing Strategies

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';

describe('Buffer Operations', () => {
    it('should create buffer from string', () => {
        const buf = Buffer.from('Hello');
        assert.equal(buf.length, 5);
        assert.equal(buf.toString(), 'Hello');
    });

    it('should handle hex encoding', () => {
        const buf = Buffer.from('deadbeef', 'hex');
        assert.equal(buf.length, 4);
        assert.equal(buf.toString('hex'), 'deadbeef');
    });

    it('should compare buffers correctly', () => {
        const a = Buffer.from('test');
        const b = Buffer.from('test');
        const c = Buffer.from('different');
        
        assert.ok(a.equals(b));
        assert.ok(!a.equals(c));
    });

    it('should handle concat correctly', () => {
        const result = Buffer.concat([
            Buffer.from('Hello '),
            Buffer.from('World'),
        ]);
        assert.equal(result.toString(), 'Hello World');
    });

    it('should slice without copying', () => {
        const original = Buffer.from('Hello World');
        const slice = original.slice(0, 5);
        
        slice.fill('X');
        assert.equal(original.toString(), 'XXXXX World');
    });

    it('should copy independently', () => {
        const original = Buffer.from('Hello World');
        const copy = Buffer.alloc(original.length);
        original.copy(copy);
        
        copy.fill('X');
        assert.equal(original.toString(), 'Hello World');
    });
});
```

## Best Practices Checklist

- [ ] Copy buffer slices when retaining long-term references
- [ ] Use `Buffer.alloc()` for zero-initialized security-sensitive data
- [ ] Use `timingSafeEqual` for comparing secrets
- [ ] Zero-fill buffers containing sensitive data before release
- [ ] Profile buffer operations in performance-critical paths
- [ ] Track buffer allocations in long-running processes
- [ ] Validate input types before creating buffers
- [ ] Test buffer operations with edge cases (empty, large, encoding)

## Cross-References

- See [Buffer Creation](./01-buffer-creation-memory.md) for allocation methods
- See [Buffer Manipulation](./02-buffer-manipulation-encoding.md) for encoding patterns
- See [Stream Security](../09-stream-security/01-encryption-decryption.md) for encrypted streams
- See [Buffer Basics](../buffers/01-buffer-basics.md) for fundamentals

## Next Steps

Continue to [Stream Processing Patterns](../03-stream-processing-patterns/01-transformation-pipelines.md).
