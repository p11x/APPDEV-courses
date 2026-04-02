# Buffer Pooling and Reuse Patterns

## What You'll Learn

- Custom buffer pool implementation
- Buffer reuse for high-throughput
- Memory-efficient buffer management
- Connection-level buffer strategies

## Custom Buffer Pool

```javascript
class BufferPool {
    constructor(bufferSize, poolSize) {
        this.bufferSize = bufferSize;
        this.pool = [];
        this.inUse = new Set();

        // Pre-allocate buffers
        for (let i = 0; i < poolSize; i++) {
            this.pool.push(Buffer.alloc(bufferSize));
        }
    }

    acquire() {
        if (this.pool.length > 0) {
            const buf = this.pool.pop();
            this.inUse.add(buf);
            return buf;
        }

        // Pool exhausted — allocate new
        const buf = Buffer.alloc(this.bufferSize);
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
            available: this.pool.length,
            inUse: this.inUse.size,
            total: this.pool.length + this.inUse.size,
        };
    }
}

// Usage
const pool = new BufferPool(4096, 100); // 100 × 4KB buffers

const buf = pool.acquire();
// ... use buffer ...
pool.release(buf);

console.log(pool.stats); // { available: 100, inUse: 0, total: 100 }
```

## Ring Buffer for Streaming

```javascript
class RingBuffer {
    constructor(capacity) {
        this.buffer = Buffer.alloc(capacity);
        this.capacity = capacity;
        this.head = 0;
        this.tail = 0;
        this.size = 0;
    }

    write(data) {
        const src = Buffer.isBuffer(data) ? data : Buffer.from(data);

        for (let i = 0; i < src.length; i++) {
            if (this.size === this.capacity) {
                // Overwrite oldest data
                this.head = (this.head + 1) % this.capacity;
            } else {
                this.size++;
            }
            this.buffer[this.tail] = src[i];
            this.tail = (this.tail + 1) % this.capacity;
        }
    }

    read(length) {
        const toRead = Math.min(length, this.size);
        const result = Buffer.alloc(toRead);

        for (let i = 0; i < toRead; i++) {
            result[i] = this.buffer[this.head];
            this.head = (this.head + 1) % this.capacity;
            this.size--;
        }

        return result;
    }

    get available() { return this.size; }
}

// Usage: Keep last 1MB of log data
const ring = new RingBuffer(1024 * 1024);
ring.write('Log entry 1\n');
ring.write('Log entry 2\n');
const recent = ring.read(100); // Read last 100 bytes
```

## Best Practices Checklist

- [ ] Pre-allocate buffer pools for high-throughput servers
- [ ] Clear buffers with `fill(0)` before releasing (security)
- [ ] Use ring buffers for sliding window data
- [ ] Monitor pool statistics for tuning
- [ ] Size pools based on expected concurrency

## Cross-References

- See [Creation and Encoding](./01-buffer-creation-encoding.md) for buffer basics
- See [Binary Optimization](./02-binary-data-optimization.md) for performance
- See [Memory Architecture](../03-memory-architecture/01-heap-stack-allocation.md) for memory

## Next Steps

Continue to [Child Processes](../07-child-processes/01-spawn-exec-fork.md) for process management.
