# Buffer Creation, Memory Analysis, and Pooling

## What You'll Learn

- Buffer creation methods and performance comparison
- Buffer memory model and allocation
- Buffer pooling and reuse strategies
- Buffer vs TypedArray vs string memory usage
- Buffer performance optimization

## Buffer Creation Methods

```javascript
// Method 1: Buffer.from() — from existing data
const fromString = Buffer.from('Hello, World!');
const fromArray = Buffer.from([72, 101, 108, 108, 111]);
const fromHex = Buffer.from('48656c6c6f', 'hex');
const fromBase64 = Buffer.from('SGVsbG8=', 'base64');
const fromBuffer = Buffer.from(fromString); // Copy

// Method 2: Buffer.alloc() — zero-filled (safe)
const safe = Buffer.alloc(1024); // 1KB, all zeros
const safeFilled = Buffer.alloc(1024, 0xFF); // 1KB, all 0xFF

// Method 3: Buffer.allocUnsafe() — uninitialized (fast)
const fast = Buffer.allocUnsafe(1024); // May contain old data

// Method 4: Buffer.allocUnsafeSlow() — no pool allocation
const noPool = Buffer.allocUnsafeSlow(1024);
```

### Performance Comparison

```javascript
import { performance } from 'node:perf_hooks';

function benchmark(label, fn, iterations = 100000) {
    const start = performance.now();
    for (let i = 0; i < iterations; i++) fn();
    const elapsed = performance.now() - start;
    console.log(`${label}: ${elapsed.toFixed(1)}ms (${iterations} ops)`);
    return elapsed;
}

const SIZE = 1024; // 1KB

benchmark('Buffer.alloc()        ', () => Buffer.alloc(SIZE));
benchmark('Buffer.allocUnsafe()  ', () => Buffer.allocUnsafe(SIZE));
benchmark('Buffer.from(string)   ', () => Buffer.from('x'.repeat(SIZE)));
benchmark('Buffer.from(array)    ', () => Buffer.from(new Array(SIZE).fill(0)));
benchmark('Buffer.allocUnsafeSlow()', () => Buffer.allocUnsafeSlow(SIZE));
```

```
Typical Results (1KB × 100,000 iterations):
─────────────────────────────────────────────
Method                  Time       Notes
─────────────────────────────────────────────
Buffer.alloc()          45ms       Safe, zero-filled
Buffer.allocUnsafe()     3ms       ~15x faster, may leak data
Buffer.from(string)    120ms       Copies and converts
Buffer.from(array)     380ms       Iterates array
Buffer.allocUnsafeSlow() 3ms       No pool, more GC pressure
```

## Memory Model

```
Buffer Memory Layout:
─────────────────────────────────────────────
Node.js Buffer uses a shared memory pool:

Pool (default: 8KB slabs)
┌─────────────────────────────────────────────┐
│ [Buffer1: 1KB][Buffer2: 2KB][free: 5KB]     │
└─────────────────────────────────────────────┘

When pool exhausted → new slab allocated

Buffer.alloc()    → zeroes the memory (slower)
Buffer.allocUnsafe() → reuses pool space (faster)
Buffer.allocUnsafeSlow() → bypasses pool entirely

V8 heap overhead per Buffer object: ~48 bytes
External memory (the actual data) is NOT on V8 heap
```

```javascript
// Inspect pool usage
import v8 from 'node:v8';

console.log('Heap statistics:', v8.getHeapStatistics());
console.log('Buffer pool size:', Buffer.poolSize); // Default: 8192
console.log('Pool offset:', Buffer.poolOffset);
```

## Buffer Pooling

```javascript
class BufferPool {
    constructor(chunkSize, poolSize) {
        this.chunkSize = chunkSize;
        this.pool = [];
        this.inUse = new Set();

        // Pre-allocate pool
        for (let i = 0; i < poolSize; i++) {
            this.pool.push(Buffer.alloc(chunkSize));
        }
    }

    acquire() {
        if (this.pool.length > 0) {
            const buf = this.pool.pop();
            this.inUse.add(buf);
            return buf;
        }
        // Pool exhausted — allocate new
        const buf = Buffer.alloc(this.chunkSize);
        this.inUse.add(buf);
        return buf;
    }

    release(buf) {
        if (this.inUse.has(buf)) {
            this.inUse.delete(buf);
            buf.fill(0); // Clear sensitive data
            this.pool.push(buf);
        }
    }

    get stats() {
        return {
            poolAvailable: this.pool.length,
            inUse: this.inUse.size,
            chunkSize: this.chunkSize,
        };
    }
}

// Usage with streams
import { Transform } from 'node:stream';

const pool = new BufferPool(64 * 1024, 10); // 64KB chunks, pool of 10

const transform = new Transform({
    transform(chunk, encoding, callback) {
        const buf = pool.acquire();
        const written = buf.write(chunk.toString().toUpperCase(), 'utf8');
        this.push(buf.slice(0, written));
        pool.release(buf);
        callback();
    }
});
```

## Buffer vs String vs TypedArray

```
Memory Comparison (1 million characters):
─────────────────────────────────────────────
Type              Memory Usage    Random Access   Encoding
─────────────────────────────────────────────
string (UTF-16)   ~2 MB           O(1)            UTF-16
Buffer (UTF-8)    ~1 MB           O(1)            Configurable
Uint8Array        ~1 MB           O(1)            Binary only
ArrayBuffer       ~1 MB           O(1)            Binary only
```

```javascript
// String: UTF-16 encoding (2 bytes per character for most)
const str = 'A'.repeat(1_000_000);
console.log('String length:', str.length); // 1,000,000 chars

// Buffer: UTF-8 encoding (1 byte for ASCII, variable for others)
const buf = Buffer.from('A'.repeat(1_000_000));
console.log('Buffer length:', buf.length); // 1,000,000 bytes

// Binary data: TypedArray (efficient)
const typed = new Uint8Array(1_000_000);
typed.fill(65); // 'A'
console.log('TypedArray length:', typed.length); // 1,000,000 elements

// When to use what:
// - Buffer: Node.js I/O, binary protocols, file operations
// - string: Text processing, user-facing data
// - TypedArray: Numerical computation, WebGL, binary manipulation
// - ArrayBuffer: Raw binary data sharing between contexts
```

## Buffer Security

```javascript
// Buffer.allocUnsafe() may contain sensitive data from memory
const unsafe = Buffer.allocUnsafe(32);
console.log(unsafe); // May print old passwords, keys, etc.

// Safe pattern: always use Buffer.alloc() for sensitive data
const safe = Buffer.alloc(32);

// Pattern: zero-fill on release
function secureAlloc(size) {
    const buf = Buffer.alloc(size);
    // Register cleanup
    process.on('exit', () => buf.fill(0));
    return buf;
}

// Secure comparison (timing-safe)
import { timingSafeEqual } from 'node:crypto';

const token1 = Buffer.from('secret-token-123');
const token2 = Buffer.from('secret-token-123');

if (token1.length === token2.length && timingSafeEqual(token1, token2)) {
    console.log('Tokens match');
}
```

## Best Practices Checklist

- [ ] Use `Buffer.alloc()` for security-sensitive data
- [ ] Use `Buffer.allocUnsafe()` only for performance-critical, non-sensitive data
- [ ] Pool buffers for high-throughput streams
- [ ] Zero-fill buffers before releasing to pool
- [ ] Use `timingSafeEqual` for secret comparison
- [ ] Monitor `Buffer.poolOffset` for pool exhaustion
- [ ] Prefer Buffer over string for binary data

## Cross-References

- See [Binary Data](../buffers/02-binary-data.md) for binary protocols
- See [Backpressure](../01-streams-architecture/02-backpressure-performance.md) for stream performance
- See [Buffer Manipulation](./02-buffer-manipulation-encoding.md) for encoding patterns

## Next Steps

Continue to [Buffer Manipulation and Encoding](./02-buffer-manipulation-encoding.md).
