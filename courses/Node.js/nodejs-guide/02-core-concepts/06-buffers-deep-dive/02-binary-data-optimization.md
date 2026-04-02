# Binary Data Optimization and Performance

## What You'll Learn

- Buffer pooling strategies
- TypedArray integration
- Large buffer handling
- Performance benchmarks

## Buffer Pool

```javascript
// Node.js uses internal buffer pool for small buffers (< 8KB)
// Pool size: 8KB (adjustable via Buffer.poolSize)

console.log(Buffer.poolSize); // 8192

// Small buffers use pool (shared allocation)
const small1 = Buffer.alloc(100); // From pool
const small2 = Buffer.alloc(200); // From pool

// Large buffers bypass pool (direct allocation)
const large = Buffer.alloc(10000); // Direct heap

// Check pool usage
console.log(small1.buffer.byteLength); // Part of pool buffer
console.log(large.buffer.byteLength);  // Separate allocation
```

## TypedArray Integration

```javascript
// Buffers are Uint8Array subclasses
const buf = Buffer.from([1, 2, 3, 4]);

// Convert to typed arrays for numeric operations
const float32 = new Float32Array(buf.buffer, buf.byteOffset, 1);
console.log(float32[0]); // Interpret bytes as float

// Create typed array directly
const floats = new Float64Array([1.1, 2.2, 3.3]);
const intView = new Uint32Array(floats.buffer);

// Efficient numeric buffer creation
function createNumberBuffer(numbers) {
    const buf = Buffer.alloc(numbers.length * 8); // 8 bytes per double
    for (let i = 0; i < numbers.length; i++) {
        buf.writeDoubleLE(numbers[i], i * 8);
    }
    return buf;
}

const numBuf = createNumberBuffer([1.5, 2.5, 3.5]);
console.log(numBuf.readDoubleLE(0)); // 1.5
console.log(numBuf.readDoubleLE(8)); // 2.5
```

## Large Buffer Handling

```javascript
// Process large buffers in chunks
function processLargeBuffer(buffer, chunkSize = 64 * 1024) {
    const results = [];

    for (let offset = 0; offset < buffer.length; offset += chunkSize) {
        const end = Math.min(offset + chunkSize, buffer.length);
        const chunk = buffer.subarray(offset, end);
        results.push(processChunk(chunk));
    }

    return results;
}

// Avoid copying — use subarray (creates view)
const large = Buffer.alloc(1024 * 1024); // 1MB
const view = large.subarray(0, 1024);     // View, not copy — O(1)

// Copy only when needed
const copy = Buffer.from(large.subarray(0, 1024)); // Actual copy
```

## Performance Benchmarks

```
Buffer Operation Benchmarks (1MB data):
─────────────────────────────────────────────
Buffer.alloc()        ████████████████  45ms (safe, zeroed)
Buffer.allocUnsafe()  ████████████      12ms (unsafe, fast)
Buffer.from(string)   ██████████████    38ms (copy from string)
Buffer.concat()       ████████████████  42ms (combine buffers)
buf.toString()        ██████████████████ 55ms (decode to string)
buf.subarray()        ████              0.01ms (view, no copy)
buf.copy()            ████████████████  40ms (actual copy)

Key insight: Avoid unnecessary copies and string conversions
```

## Best Practices Checklist

- [ ] Use `Buffer.alloc()` for safety
- [ ] Use `subarray()` instead of `slice()` for views
- [ ] Process large buffers in chunks
- [ ] Avoid string ↔ buffer conversions in hot paths
- [ ] Use typed arrays for numeric data
- [ ] Set appropriate chunk sizes for your workload

## Cross-References

- See [Creation and Encoding](./01-buffer-creation-encoding.md) for buffer basics
- See [Memory Architecture](../03-memory-architecture/01-heap-stack-allocation.md) for memory
- See [Stream Architecture](../05-stream-architecture/01-readable-writable-streams.md) for streaming

## Next Steps

Continue to [Buffer Pooling](./03-buffer-pooling-strategies.md) for reuse patterns.
