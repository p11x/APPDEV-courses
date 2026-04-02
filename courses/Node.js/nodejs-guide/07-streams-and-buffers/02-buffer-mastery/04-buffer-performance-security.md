# Buffer Performance Optimization and Security

## What You'll Learn

- Buffer vs TypedArray vs String performance benchmarks
- Buffer pooling strategies and memory management
- `Buffer.allocUnsafe()` vs `Buffer.alloc()` trade-offs
- `SharedArrayBuffer` for cross-worker buffer sharing
- `Atomics` for concurrent buffer access
- Buffer memory leak patterns and prevention
- Timing-safe buffer comparison and secure wiping
- `FinalizationRegistry` for buffer cleanup tracking
- Performance profiling of buffer operations

## Buffer vs TypedArray vs String Benchmarks

```javascript
import { performance } from 'node:perf_hooks';

const SIZE = 1_000_000;

function benchmark(label, fn, iterations = 1000) {
    for (let i = 0; i < 10; i++) fn(); // warmup
    const start = performance.now();
    for (let i = 0; i < iterations; i++) fn();
    console.log(`${label}: ${(performance.now() - start).toFixed(1)}ms`);
}

// Allocation
benchmark('Buffer.alloc()         ', () => Buffer.alloc(SIZE));
benchmark('Buffer.allocUnsafe()   ', () => Buffer.allocUnsafe(SIZE));
benchmark('Uint8Array             ', () => new Uint8Array(SIZE));
benchmark('Buffer.from(string)    ', () => Buffer.from('A'.repeat(SIZE)));

// Sequential read
const buf = Buffer.alloc(SIZE).fill(65);
const typed = new Uint8Array(SIZE).fill(65);

benchmark('Buffer readUInt8()     ', () => { let s = 0; for (let i = 0; i < SIZE; i++) s += buf[i]; });
benchmark('Uint8Array direct      ', () => { let s = 0; for (let i = 0; i < SIZE; i++) s += typed[i]; });

// Copy
benchmark('Buffer.copy()          ', () => { const d = Buffer.alloc(SIZE); buf.copy(d); });
benchmark('Uint8Array.set()       ', () => { const d = new Uint8Array(SIZE); d.set(typed); });
```

```
Typical Results (1MB × 1000 iterations):
──────────────────────────────────────────────────
Operation              Buffer     Uint8Array
──────────────────────────────────────────────────
Allocation (zeroed)    3ms        2ms
Allocation (unsafe)    0.2ms      N/A
Sequential read        12ms       4ms
Copy                   5ms        3ms
──────────────────────────────────────────────────
Uint8Array fastest for compute. Buffer best for Node.js I/O.
```

## Buffer.allocUnsafe() vs Buffer.alloc()

```javascript
// Buffer.alloc() — zeroes memory, safe for all use cases
const safe = Buffer.alloc(1024);
console.log(safe.every(b => b === 0)); // true

// Buffer.allocUnsafe() — skips zeroing, ~10-15x faster
const unsafe = Buffer.allocUnsafe(1024);
console.log(unsafe.some(b => b !== 0)); // Often true — residual data

// Safe pattern: allocUnsafe + immediate overwrite
function allocForEncoding(data, encoding = 'utf8') {
    const buf = Buffer.allocUnsafe(Buffer.byteLength(data, encoding));
    buf.write(data, encoding);
    return buf;
}
```

```
When to use each:
──────────────────────────────────────────────────────────
Scenario                          Use
──────────────────────────────────────────────────────────
User-facing data / secrets        Buffer.alloc()
High-throughput stream buffers    allocUnsafe()
Immediately overwritten buffers   allocUnsafe()
File I/O with sensitive content   Buffer.alloc()
Protocol framing (write-only)     allocUnsafe()
──────────────────────────────────────────────────────────
```

## Buffer Pooling Strategies

```javascript
class SlabPool {
    #slabSize;
    #slab;
    #offset = 0;
    #slabs = [];

    constructor(slabSize = 8192) {
        this.#slabSize = slabSize;
        this.#slab = Buffer.alloc(slabSize);
        this.#slabs.push(this.#slab);
    }

    alloc(size) {
        if (size > this.#slabSize) return Buffer.alloc(size);
        if (this.#offset + size > this.#slabSize) {
            this.#slab = Buffer.alloc(this.#slabSize);
            this.#slabs.push(this.#slab);
            this.#offset = 0;
        }
        const buf = this.#slab.subarray(this.#offset, this.#offset + size);
        this.#offset += size;
        return buf;
    }

    get stats() {
        return { slabs: this.#slabs.length, totalMemory: this.#slabs.length * this.#slabSize };
    }
}

const pool = new SlabPool(8192);
const header = pool.alloc(64);
const payload = pool.alloc(1024);
console.log(pool.stats); // { slabs: 1, totalMemory: 8192 }
```

## SharedArrayBuffer and Atomics for Concurrent Access

```javascript
// main.mjs
import { Worker } from 'node:worker_threads';

const shared = new SharedArrayBuffer(1024);
const flag = new Int32Array(shared, 0, 1);
const data = new Uint8Array(shared, 4, 1020);

// Acquire lock, write data
while (Atomics.compareExchange(flag, 0, 0, 1) !== 0) {
    Atomics.wait(flag, 0, 1);
}
Buffer.from('MAIN-DATA').copy(data);
Atomics.store(flag, 0, 0);
Atomics.notify(flag, 0);

const worker = new Worker('./worker.mjs', { workerData: { shared } });
```

```javascript
// worker.mjs
import { workerData, parentPort } from 'node:worker_threads';

const flag = new Int32Array(workerData.shared, 0, 1);
const data = new Uint8Array(workerData.shared, 4, 1020);

while (Atomics.compareExchange(flag, 0, 0, 1) !== 0) {
    Atomics.wait(flag, 0, 1);
}
const msg = Buffer.from(data).toString('utf8').replace(/\0+$/, '');
Atomics.store(flag, 0, 0);
Atomics.notify(flag, 0);

parentPort.postMessage({ read: msg });
```

## Buffer Memory Leak Patterns

```javascript
// LEAK: Slice retains parent reference
function leakingSlice() {
    const large = Buffer.alloc(10 * 1024 * 1024); // 10MB
    return large.slice(0, 100); // Parent stays alive
}

// FIX: Copy to independent buffer
function safeSlice() {
    const large = Buffer.alloc(10 * 1024 * 1024);
    const small = Buffer.alloc(100);
    large.copy(small, 0, 0, 100);
    return small;
}

// LEAK: Unbounded accumulator
const chunks = [];
function leakyAccumulator(chunk) { chunks.push(chunk); }

// FIX: Ring buffer accumulator
class RingAccumulator {
    #buffers;
    #head = 0;
    constructor(maxChunks = 100) {
        this.#buffers = new Array(maxChunks);
    }
    push(buf) {
        this.#buffers[this.#head] = buf;
        this.#head = (this.#head + 1) % this.#buffers.length;
    }
    concat() { return Buffer.concat(this.#buffers.filter(Boolean)); }
}
```

## FinalizationRegistry for Buffer Cleanup

```javascript
const cleanupRegistry = new FinalizationRegistry(({ label, size }) => {
    console.log(`Buffer GC'd: ${label}, was ${size} bytes`);
});

class TrackedBuffer {
    #buffer;
    constructor(size, label = 'anonymous') {
        this.#buffer = Buffer.alloc(size);
        cleanupRegistry.register(this, { label, size }, this);
    }
    get buffer() { return this.#buffer; }
    wipe() {
        this.#buffer.fill(0);
        cleanupRegistry.unregister(this);
    }
}

let tb = new TrackedBuffer(1024 * 1024, 'crypto-key');
// ... use buffer ...
tb = null; // Eligible for GC — registry logs cleanup
```

## Timing-Safe Comparison and Secure Wiping

```javascript
import { timingSafeEqual, randomBytes } from 'node:crypto';

// VULNERABLE: leaks timing info
function vulnerableCompare(a, b) { return a === b; }

// SECURE: constant-time comparison
function secureCompare(a, b) {
    const bufA = Buffer.from(a);
    const bufB = Buffer.from(b);
    if (bufA.length !== bufB.length) {
        return timingSafeEqual(randomBytes(1), randomBytes(1)) && false;
    }
    return timingSafeEqual(bufA, bufB);
}

// DoD-style secure wiping
function secureWipe(buf) {
    buf.fill(0x00);       // Pass 1: zeros
    buf.fill(0xFF);       // Pass 2: ones
    randomBytes(buf.length).copy(buf); // Pass 3: random
}

// Auto-wipe on process exit
const sensitiveBuffers = [];
function allocSecure(size) {
    const buf = Buffer.alloc(size);
    sensitiveBuffers.push(buf);
    return buf;
}
process.on('exit', () => { for (const buf of sensitiveBuffers) buf.fill(0); });
```

## Buffer.from(arrayBuffer) Shared Memory

```javascript
// Share underlying memory between Buffer and ArrayBuffer
const ab = new ArrayBuffer(16);
const view = new Uint8Array(ab);
const buf = Buffer.from(ab); // Same memory, zero-copy

view[0] = 0x41; // 'A'
console.log(buf[0]); // 65 — same memory

buf[1] = 0x42; // 'B'
console.log(view[1]); // 66 — same memory

// Subarray also shares memory
const sub = buf.subarray(0, 4);
sub.fill(0x58);
console.log(Buffer.from(ab).toString()); // 'XXXX...'
```

## Performance Profiling

```javascript
import { performance, PerformanceObserver } from 'node:perf_hooks';

const obs = new PerformanceObserver((items) => {
    for (const entry of items.getEntries()) {
        console.log(`${entry.name}: ${entry.duration.toFixed(3)}ms`);
    }
});
obs.observe({ entryTypes: ['measure'] });

const SIZE = 64 * 1024;

performance.mark('alloc-start');
const buf = Buffer.alloc(SIZE);
performance.mark('alloc-end');
performance.measure('Buffer.alloc(64KB)', 'alloc-start', 'alloc-end');

performance.mark('write-start');
for (let i = 0; i < SIZE; i++) buf.writeUInt8(i & 0xFF, i);
performance.mark('write-end');
performance.measure('Sequential write', 'write-start', 'write-end');

performance.mark('concat-start');
Buffer.concat(Array.from({ length: 100 }, () => Buffer.alloc(1024)));
performance.mark('concat-end');
performance.measure('Buffer.concat(100x1KB)', 'concat-start', 'concat-end');
```

## Best Practices Checklist

- [ ] Use `Buffer.alloc()` for any buffer that may hold sensitive data
- [ ] Use `Buffer.allocUnsafe()` only when you immediately overwrite contents
- [ ] Implement slab pooling for high-frequency small buffer allocation
- [ ] Use `SharedArrayBuffer` with `Atomics` for multi-threaded buffer access
- [ ] Always use `timingSafeEqual` for comparing tokens and secrets
- [ ] Wipe sensitive buffers with multiple overwrite passes before release
- [ ] Copy buffer slices instead of retaining parent references
- [ ] Use `FinalizationRegistry` to detect buffer lifecycle issues
- [ ] Profile buffer operations with `node:perf_hooks` in production paths
- [ ] Use `Buffer.from(arrayBuffer)` for zero-copy memory sharing

## Cross-References

- See [Buffer Creation](./01-buffer-creation-memory.md) for allocation methods and pooling
- See [Buffer Manipulation](./02-buffer-manipulation-encoding.md) for encoding operations
- See [Advanced Patterns](./03-buffer-advanced-patterns.md) for leak detection
- See [Stream Security](../09-stream-security/01-encryption-validation.md) for encrypted buffers
- See [Parallel Processing](../06-stream-concurrency-parallelism/01-parallel-processing.md) for worker patterns

## Next Steps

Continue to [Buffer and Stream Integration](./05-buffer-stream-integration.md).
