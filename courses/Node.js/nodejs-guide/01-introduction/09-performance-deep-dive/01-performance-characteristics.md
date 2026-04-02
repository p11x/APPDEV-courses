# Performance Characteristics Deep Dive

## What You'll Learn

- Memory usage patterns and optimization strategies
- CPU efficiency and single-threaded nature
- Scalability and concurrency models
- Benchmarks and performance comparisons

## Memory Usage Patterns

### Memory Structure

```
Node.js Memory Layout
─────────────────────────────────────────
┌─────────────────────────────────────┐
│           Resident Set Size         │
│  ┌─────────────────────────────┐   │
│  │        Heap Total           │   │
│  │  ┌─────────────────────┐   │   │
│  │  │     Heap Used       │   │   │
│  │  │  (Your objects)     │   │   │
│  │  └─────────────────────┘   │   │
│  │  ┌─────────────────────┐   │   │
│  │  │    Heap Free        │   │   │
│  │  └─────────────────────┘   │   │
│  └─────────────────────────────┘   │
│  ┌─────────────────────────────┐   │
│  │       External              │   │
│  │  (Buffers, native memory)   │   │
│  └─────────────────────────────┘   │
│  ┌─────────────────────────────┐   │
│  │       Stack                 │   │
│  │  (Function calls, locals)   │   │
│  └─────────────────────────────┘   │
└─────────────────────────────────────┘
```

### Memory Monitoring

```javascript
// Comprehensive memory monitoring
function getMemoryUsage() {
    const used = process.memoryUsage();
    
    return {
        rss: {
            bytes: used.rss,
            mb: Math.round(used.rss / 1024 / 1024),
            description: 'Total memory allocated'
        },
        heapTotal: {
            bytes: used.heapTotal,
            mb: Math.round(used.heapTotal / 1024 / 1024),
            description: 'Total V8 heap size'
        },
        heapUsed: {
            bytes: used.heapUsed,
            mb: Math.round(used.heapUsed / 1024 / 1024),
            description: 'Actual memory used'
        },
        external: {
            bytes: used.external,
            mb: Math.round(used.external / 1024 / 1024),
            description: 'C++ objects (Buffers, etc.)'
        },
        arrayBuffers: {
            bytes: used.arrayBuffers,
            mb: Math.round(used.arrayBuffers / 1024 / 1024),
            description: 'ArrayBuffer memory'
        }
    };
}

// Usage
setInterval(() => {
    const memory = getMemoryUsage();
    console.log(`Heap: ${memory.heapUsed.mb}MB / ${memory.heapTotal.mb}MB`);
}, 1000);
```

### Memory Leak Detection

```javascript
// Detect memory leaks with heap snapshots
const v8 = require('v8');
const fs = require('fs');

function takeHeapSnapshot(filename) {
    const snapshotStream = v8.writeHeapSnapshot();
    console.log(`Heap snapshot written to: ${snapshotStream}`);
}

// Or programmatic
const { HeapSnapshot } = require('v8');

// Monitor heap growth
let lastHeapUsed = 0;
setInterval(() => {
    const current = process.memoryUsage().heapUsed;
    const growth = current - lastHeapUsed;
    
    if (growth > 10 * 1024 * 1024) { // 10MB growth
        console.warn(`Memory growth detected: ${growth / 1024 / 1024}MB`);
        takeHeapSnapshot('leak.heapsnapshot');
    }
    
    lastHeapUsed = current;
}, 10000);
```

### Memory Optimization Strategies

```javascript
// 1. Object pooling
class ObjectPool {
    constructor(createFn, resetFn, initialSize = 100) {
        this.createFn = createFn;
        this.resetFn = resetFn;
        this.pool = [];
        
        for (let i = 0; i < initialSize; i++) {
            this.pool.push(createFn());
        }
    }
    
    acquire() {
        return this.pool.pop() || this.createFn();
    }
    
    release(obj) {
        this.resetFn(obj);
        this.pool.push(obj);
    }
}

// Usage
const bufferPool = new ObjectPool(
    () => Buffer.alloc(1024),
    (buf) => buf.fill(0)
);

// 2. Use WeakRef for caches
const cache = new Map();

function getCachedData(key) {
    const ref = cache.get(key);
    if (ref) {
        const data = ref.deref();
        if (data) return data;
    }
    
    const data = expensiveComputation(key);
    cache.set(key, new WeakRef(data));
    return data;
}

// 3. Use FinalizationRegistry for cleanup
const registry = new FinalizationRegistry((heldValue) => {
    console.log(`Object was garbage collected: ${heldValue}`);
});

function createObject() {
    const obj = { data: 'large data' };
    registry.register(obj, 'some identifier');
    return obj;
}
```

## CPU Efficiency

### Single-Threaded Nature

```javascript
// Node.js main thread handles all JavaScript
// CPU-intensive work blocks everything

// BAD: Blocks event loop
function calculatePrimes(max) {
    const primes = [];
    for (let i = 2; i <= max; i++) {
        let isPrime = true;
        for (let j = 2; j < i; j++) {
            if (i % j === 0) {
                isPrime = false;
                break;
            }
        }
        if (isPrime) primes.push(i);
    }
    return primes;
}

// This blocks ALL requests
app.get('/primes', (req, res) => {
    const primes = calculatePrimes(1000000);
    res.json(primes);
});
```

### Worker Threads for CPU-Bound Tasks

```javascript
// worker.js
const { parentPort, workerData } = require('worker_threads');

function calculatePrimes(max) {
    const primes = [];
    for (let i = 2; i <= max; i++) {
        let isPrime = true;
        for (let j = 2; j < i; j++) {
            if (i % j === 0) {
                isPrime = false;
                break;
            }
        }
        if (isPrime) primes.push(i);
    }
    return primes;
}

const result = calculatePrimes(workerData);
parentPort.postMessage(result);

// main.js
const { Worker } = require('worker_threads');

app.get('/primes', (req, res) => {
    const worker = new Worker('./worker.js', {
        workerData: 1000000
    });
    
    worker.on('message', (primes) => {
        res.json(primes);
    });
    
    worker.on('error', (err) => {
        res.status(500).json({ error: err.message });
    });
});
```

### Cluster Module for Multi-Core

```javascript
const cluster = require('cluster');
const os = require('os');

if (cluster.isMaster) {
    const numCPUs = os.cpus().length;
    
    console.log(`Master ${process.pid} starting ${numCPUs} workers`);
    
    for (let i = 0; i < numCPUs; i++) {
        cluster.fork();
    }
    
    cluster.on('exit', (worker, code, signal) => {
        console.log(`Worker ${worker.process.pid} died`);
        cluster.fork(); // Restart
    });
} else {
    const express = require('express');
    const app = express();
    
    app.get('/', (req, res) => {
        res.send(`Hello from worker ${process.pid}`);
    });
    
    app.listen(3000);
    console.log(`Worker ${process.pid} started`);
}
```

## Scalability Models

### Horizontal Scaling

```
Horizontal Scaling (Multiple Instances)
─────────────────────────────────────────
                    Load Balancer
                         │
          ┌──────────────┼──────────────┐
          │              │              │
     ┌────┴────┐    ┌────┴────┐    ┌────┴────┐
     │ Node.js │    │ Node.js │    │ Node.js │
     │  App    │    │  App    │    │  App    │
     └─────────┘    └─────────┘    └─────────┘
```

### Vertical Scaling

```
Vertical Scaling (More Resources)
─────────────────────────────────────────
     ┌─────────────────────────────────┐
     │        Node.js Application     │
     │  ┌─────────┐  ┌─────────────┐  │
     │  │  CPU    │  │   Memory    │  │
     │  │ 4 Cores │  │   16 GB     │  │
     │  └─────────┘  └─────────────┘  │
     └─────────────────────────────────┘
```

### Event Loop Scaling

```javascript
// Non-blocking I/O allows high concurrency
// Single thread can handle thousands of connections

const http = require('http');

let requestCount = 0;

const server = http.createServer(async (req, res) => {
    requestCount++;
    
    // Simulate database query (non-blocking)
    await new Promise(resolve => setTimeout(resolve, 100));
    
    res.writeHead(200);
    res.end(`Request #${requestCount}`);
});

server.listen(3000);

// Can handle 10,000+ concurrent connections
// Each uses minimal memory (~1KB vs ~2MB for threads)
```

## Benchmark Comparisons

### HTTP Server Throughput

```javascript
// Benchmark: Requests per second
const http = require('http');
const { performance } = require('perf_hooks');

let requests = 0;
let startTime = performance.now();

const server = http.createServer((req, res) => {
    requests++;
    res.writeHead(200);
    res.end('OK');
});

server.listen(3000);

// Monitor throughput
setInterval(() => {
    const elapsed = (performance.now() - startTime) / 1000;
    const rps = Math.round(requests / elapsed);
    console.log(`Requests/sec: ${rps}`);
}, 1000);

// Typical results:
// Node.js: 30,000-50,000 req/sec
// Python Flask: 2,000-5,000 req/sec
// Ruby on Rails: 1,000-3,000 req/sec
// Go: 100,000-200,000 req/sec
```

### Database Query Performance

```javascript
// Benchmark: Database operations
const { performance } = require('perf_hooks');

async function benchmarkDatabase(db, queries = 1000) {
    const start = performance.now();
    
    for (let i = 0; i < queries; i++) {
        await db.query('SELECT * FROM users LIMIT 10');
    }
    
    const elapsed = performance.now() - start;
    const qps = Math.round(queries / (elapsed / 1000));
    
    console.log(`Queries/sec: ${qps}`);
    console.log(`Average latency: ${(elapsed / queries).toFixed(2)}ms`);
}

// Results vary by database:
// PostgreSQL: 5,000-10,000 qps
// MongoDB: 10,000-20,000 qps
// Redis: 100,000+ qps
```

### JSON Parsing Performance

```javascript
// Benchmark: JSON operations
const { performance } = require('perf_hooks');

const data = JSON.stringify({
    users: Array(1000).fill(null).map((_, i) => ({
        id: i,
        name: `User ${i}`,
        email: `user${i}@example.com`,
        age: 20 + (i % 50)
    }))
});

// Parse benchmark
const parseStart = performance.now();
for (let i = 0; i < 1000; i++) {
    JSON.parse(data);
}
const parseTime = performance.now() - parseStart;

console.log(`JSON.parse: ${(parseTime / 1000).toFixed(3)}ms per operation`);

// Stringify benchmark
const obj = JSON.parse(data);
const stringifyStart = performance.now();
for (let i = 0; i < 1000; i++) {
    JSON.stringify(obj);
}
const stringifyTime = performance.now() - stringifyStart;

console.log(`JSON.stringify: ${(stringifyTime / 1000).toFixed(3)}ms per operation`);
```

## Performance Profiling

### CPU Profiling

```bash
# Generate CPU profile
node --prof app.js

# Process profile
node --prof-process isolate-*.log

# Or use Chrome DevTools
node --inspect app.js
# Open chrome://inspect
```

### Memory Profiling

```javascript
// Programmatic heap profiling
const v8 = require('v8');

// Take heap snapshot
const heapSnapshot = v8.writeHeapSnapshot();

// Get heap statistics
const heapStats = v8.getHeapStatistics();
console.log({
    totalHeapSize: heapStats.total_heap_size,
    usedHeapSize: heapStats.used_heap_size,
    heapSizeLimit: heapStats.heap_size_limit
});

// Force garbage collection (with --expose-gc flag)
if (global.gc) {
    global.gc();
}
```

### Event Loop Profiling

```javascript
const { monitorEventLoopDelay } = require('perf_hooks');

const histogram = monitorEventLoopDelay({ resolution: 20 });
histogram.enable();

setInterval(() => {
    console.log({
        min: histogram.min,
        max: histogram.max,
        mean: histogram.mean,
        p50: histogram.percentile(50),
        p99: histogram.percentile(99)
    });
    histogram.reset();
}, 5000);
```

## Optimization Techniques

### Code-Level Optimizations

```javascript
// 1. Avoid repeated computations
const cache = new Map();

function expensiveComputation(input) {
    if (cache.has(input)) {
        return cache.get(input);
    }
    
    const result = /* expensive work */;
    cache.set(input, result);
    return result;
}

// 2. Use typed arrays for numeric data
const floatArray = new Float64Array(1000000);
// Faster than regular arrays for numbers

// 3. Batch I/O operations
async function batchWrite(items) {
    const batch = items.slice(0, 100);
    await db.insertMany(batch);
    
    if (items.length > 100) {
        await batchWrite(items.slice(100));
    }
}

// 4. Use streams for large data
const fs = require('fs');
const { Transform } = require('stream');

fs.createReadStream('large.txt')
    .pipe(new Transform({
        transform(chunk, encoding, callback) {
            callback(null, chunk.toString().toUpperCase());
        }
    }))
    .pipe(fs.createWriteStream('output.txt'));
```

### System-Level Optimizations

```bash
# Increase memory limit
node --max-old-space-size=4096 app.js

# Enable V8 optimizations
node --turbo-fast-api-calls app.js

# Use production environment
NODE_ENV=production node app.js

# Enable keep-alive
node --keep-alive-timeout=65000 app.js
```

## Common Misconceptions

### Myth: Node.js is always slow
**Reality**: For I/O-bound tasks, Node.js outperforms many alternatives due to non-blocking I/O.

### Myth: More CPU cores mean better Node.js performance
**Reality**: Single-threaded nature means one core per process. Use clustering or worker threads.

### Myth: Memory usage is always high
**Reality**: Node.js can handle 10,000+ connections with minimal memory compared to thread-based servers.

### Myth: Garbage collection always causes pauses
**Reality**: V8 uses incremental and concurrent GC to minimize pause times.

## Best Practices Checklist

- [ ] Monitor memory usage in production
- [ ] Profile CPU-intensive operations
- [ ] Use worker threads for CPU-bound tasks
- [ ] Implement clustering for multi-core utilization
- [ ] Use streams for large data processing
- [ ] Cache expensive computations
- [ ] Set appropriate heap size limits
- [ ] Monitor event loop delay
- [ ] Use production environment settings
- [ ] Implement proper error handling

## Performance Optimization Tips

- Use `NODE_ENV=production` for production
- Enable gzip compression
- Implement caching strategies
- Use connection pooling
- Optimize database queries
- Use CDN for static assets
- Implement rate limiting
- Monitor and alert on performance metrics

## Cross-References

- See [V8 Internals](./05-runtime-architecture/01-v8-internals.md) for engine details
- See [Event Loop Mechanics](./06-event-loop-mechanics/01-event-loop-deep-dive.md) for async model
- See [Use Case Analysis](./07-use-case-analysis.md) for application patterns
- See [Runtime Comparison](./10-runtime-comparison.md) for alternative runtimes
- See [Real-world Cases](./11-real-world-cases.md) for production examples

## Next Steps

Now that you understand performance characteristics, let's compare Node.js with other runtimes. Continue to [Runtime Comparison Matrix](./10-runtime-comparison.md).