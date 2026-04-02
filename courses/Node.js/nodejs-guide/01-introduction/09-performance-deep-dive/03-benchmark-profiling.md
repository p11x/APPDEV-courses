# Benchmarks and Performance Profiling

## What You'll Learn

- Creating reliable benchmarks
- Interpreting performance results
- Comparing Node.js with other runtimes
- Production performance monitoring

## HTTP Server Benchmark

```javascript
// benchmark-server.js — Benchmarkable HTTP server

import { createServer } from 'node:http';

const server = createServer((req, res) => {
    if (req.url === '/json') {
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({ hello: 'world', timestamp: Date.now() }));
    } else if (req.url === '/compute') {
        const result = fibonacci(30);
        res.writeHead(200);
        res.end(JSON.stringify({ result }));
    } else {
        res.writeHead(200);
        res.end('OK');
    }
});

function fibonacci(n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

server.listen(3000, () => console.log('Benchmark server on :3000'));
```

```bash
# Install autocannon (HTTP benchmarking tool)
npm install -g autocannon

# Benchmark JSON endpoint
autocannon -c 100 -d 10 http://localhost:3000/json

# Benchmark with pipelining
autocannon -c 100 -d 10 -p 10 http://localhost:3000/json

# Results interpretation:
# Req/Sec:  Requests per second (higher is better)
# Latency:  Response time (lower is better)
# Throughput: Data transferred per second
```

## Comprehensive Benchmark Suite

```javascript
// benchmark-suite.js — Multi-operation benchmark

import { performance, PerformanceObserver } from 'node:perf_hooks';
import { createHash } from 'node:crypto';

const obs = new PerformanceObserver((items) => {
    for (const entry of items.getEntries()) {
        console.log(`${entry.name}: ${entry.duration.toFixed(2)}ms`);
    }
});
obs.observe({ entryTypes: ['measure'] });

function benchmark(name, fn, iterations = 1000) {
    // Warmup
    for (let i = 0; i < Math.min(iterations, 100); i++) fn();
    
    // Measure
    performance.mark('start');
    for (let i = 0; i < iterations; i++) fn();
    performance.mark('end');
    performance.measure(name, 'start', 'end');
}

// Benchmarks
benchmark('JSON.stringify', () => {
    JSON.stringify({ a: 1, b: 'hello', c: [1, 2, 3] });
}, 100000);

benchmark('JSON.parse', () => {
    JSON.parse('{"a":1,"b":"hello","c":[1,2,3]}');
}, 100000);

benchmark('crypto.sha256', () => {
    createHash('sha256').update('hello world').digest('hex');
}, 10000);

benchmark('Array.map', () => {
    [1, 2, 3, 4, 5].map(x => x * 2);
}, 100000);

benchmark('Array.filter', () => {
    [1, 2, 3, 4, 5].filter(x => x > 2);
}, 100000);

benchmark('Promise.resolve', async () => {
    await Promise.resolve(42);
}, 10000);
```

## Runtime Comparison Benchmark

```javascript
// runtime-compare.js — Compare Node.js versions/engines

const { performance } = require('perf_hooks');
const os = require('os');

function runBenchmarks() {
    const results = {
        runtime: `Node.js ${process.version}`,
        platform: `${os.type()} ${os.release()}`,
        arch: os.arch(),
        benchmarks: {},
    };
    
    // CPU benchmark
    const cpuStart = performance.now();
    let sum = 0;
    for (let i = 0; i < 10_000_000; i++) {
        sum += Math.sqrt(i) * Math.sin(i);
    }
    results.benchmarks.cpu = `${(performance.now() - cpuStart).toFixed(2)}ms`;
    
    // Memory allocation benchmark
    const memStart = performance.now();
    const arrays = [];
    for (let i = 0; i < 1000; i++) {
        arrays.push(new Array(10000).fill(0));
    }
    results.benchmarks.memory = `${(performance.now() - memStart).toFixed(2)}ms`;
    arrays.length = 0; // Free memory
    
    // JSON benchmark
    const jsonStart = performance.now();
    const obj = { users: Array.from({ length: 1000 }, (_, i) => ({ id: i, name: `user${i}` })) };
    for (let i = 0; i < 100; i++) {
        JSON.parse(JSON.stringify(obj));
    }
    results.benchmarks.json = `${(performance.now() - jsonStart).toFixed(2)}ms`;
    
    return results;
}

console.log(JSON.stringify(runBenchmarks(), null, 2));
```

## Performance Monitoring in Production

```javascript
// production-metrics.js — Always-on performance monitoring

import { monitorEventLoopDelay, eventLoopUtilization } from 'node:perf_hooks';

class PerformanceMonitor {
    constructor() {
        this.histogram = monitorEventLoopDelay({ resolution: 20 });
        this.histogram.enable();
        this.lastELU = eventLoopUtilization();
        this.requestTimes = [];
    }
    
    middleware() {
        return (req, res, next) => {
            const start = performance.now();
            res.on('finish', () => {
                const duration = performance.now() - start;
                this.requestTimes.push(duration);
                if (this.requestTimes.length > 1000) this.requestTimes.shift();
            });
            next();
        };
    }
    
    getMetrics() {
        const elu = eventLoopUtilization(this.lastELU);
        this.lastELU = eventLoopUtilization();
        
        const sorted = [...this.requestTimes].sort((a, b) => a - b);
        
        return {
            eventLoop: {
                lagMeanMs: +(this.histogram.mean / 1e6).toFixed(2),
                lagP99Ms: +(this.histogram.percentile(99) / 1e6).toFixed(2),
                utilization: +(elu.utilization * 100).toFixed(1) + '%',
            },
            requests: {
                count: sorted.length,
                p50ms: sorted.length > 0 ? +sorted[Math.floor(sorted.length * 0.5)]?.toFixed(2) : 0,
                p95ms: sorted.length > 0 ? +sorted[Math.floor(sorted.length * 0.95)]?.toFixed(2) : 0,
                p99ms: sorted.length > 0 ? +sorted[Math.floor(sorted.length * 0.99)]?.toFixed(2) : 0,
            },
            memory: {
                rssMB: +(process.memoryUsage().rss / 1024 / 1024).toFixed(1),
                heapUsedMB: +(process.memoryUsage().heapUsed / 1024 / 1024).toFixed(1),
            },
        };
    }
}

export const monitor = new PerformanceMonitor();
```

## Best Practices Checklist

- [ ] Warm up before benchmarking (JIT compilation)
- [ ] Run benchmarks multiple times and take median
- [ ] Benchmark in production-like environment
- [ ] Monitor event loop lag in production
- [ ] Track p50, p95, p99 response times
- [ ] Set up alerts for performance regressions

## Cross-References

- See [CPU Scalability](./02-cpu-scalability.md) for scaling strategies
- See [Performance Profiling](../13-v8-engine-practice/02-performance-profiling.md) for profiling tools
- See [Event Loop Debugging](../06-event-loop-mechanics/03-event-loop-debugging.md) for async issues

## Next Steps

Continue to [Runtime Comparison](../10-runtime-comparison/02-nodejs-vs-compiled-languages.md) for language comparisons.
