# Performance Profiling with Chrome DevTools and CLI Tools

## What You'll Learn

- CPU profiling techniques and interpretation
- Heap snapshot analysis for memory leaks
- Flame graph generation and reading
- Production profiling strategies

## CPU Profiling

### Method 1: Chrome DevTools

```bash
# Start your app with inspector
node --inspect your-app.js
# Output: Debugger listening on ws://127.0.0.1:9229/...
```

```
Chrome DevTools Profiling Steps:
─────────────────────────────────────────────
1. Open chrome://inspect in Chrome
2. Click "inspect" under your Node.js target
3. Go to "Profiler" tab (or "Performance" tab)
4. Click the record button (●)
5. Trigger the operations you want to profile
6. Click stop
7. Analyze the flame chart:
   - Wider bars = more time spent
   - Taller stacks = deeper call chains
   - Yellow = JavaScript execution
   - Blue = system calls
   - Purple = rendering
```

### Method 2: Command-Line Profiling

```bash
# Generate V8 profiling log
node --prof your-app.js

# Process into human-readable format
node --prof-process isolate-*.log

# Save to file for analysis
node --prof-process isolate-*.log > cpu-profile.txt
```

### Creating a Profiling Script

Create `profile-app.js`:
```javascript
// profile-app.js — Application with identifiable bottlenecks

const http = require('node:http');

// Intentionally slow function (for profiling demonstration)
function fibonacci(n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

// Database simulation
async function queryDatabase(id) {
    // Simulate query latency
    await new Promise(resolve => setTimeout(resolve, Math.random() * 50));
    return { id, name: `User ${id}`, timestamp: Date.now() };
}

// Cache implementation
const cache = new Map();
function getCached(key, computeFn) {
    if (cache.has(key)) return cache.get(key);
    const value = computeFn();
    cache.set(key, value);
    return value;
}

const server = http.createServer(async (req, res) => {
    if (req.url === '/cpu') {
        // CPU-bound: fibonacci computation
        const n = parseInt(new URL(req.url, 'http://localhost').searchParams.get('n') || '35');
        const result = fibonacci(n);
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({ n, result }));
        
    } else if (req.url?.startsWith('/io')) {
        // I/O-bound: database queries
        const results = await Promise.all([
            queryDatabase(1),
            queryDatabase(2),
            queryDatabase(3),
        ]);
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify(results));
        
    } else if (req.url?.startsWith('/mixed')) {
        // Mixed: cache + compute
        const key = 'computation';
        const result = getCached(key, () => fibonacci(30));
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({ result, cached: cache.has(key) }));
        
    } else {
        res.writeHead(200);
        res.end('OK');
    }
});

server.listen(3000, () => {
    console.log('Server running on http://localhost:3000');
    console.log('Profile with: node --prof profile-app.js');
    console.log('  Then hit: curl http://localhost:3000/cpu');
});
```

```bash
# Profile it
node --prof profile-app.js &
PID=$!

# Generate load
for i in $(seq 1 100); do
    curl -s http://localhost:3000/cpu > /dev/null &
done
wait

kill $PID

# Process profile
node --prof-process isolate-*.log
```

## Heap Snapshot Analysis

### Taking Heap Snapshots

```bash
# Method 1: Signal-based (production)
node --heapsnapshot-signal=SIGUSR2 your-app.js &
kill -USR2 $!
# Creates *.heapsnapshot file

# Method 2: Chrome DevTools
node --inspect your-app.js
# Open chrome://inspect → Memory tab → Take snapshot
```

### Programmatic Heap Snapshots

Create `heap-analysis.js`:
```javascript
// heap-analysis.js — Programmatic heap analysis

const v8 = require('node:v8');
const fs = require('node:fs');

function takeHeapSnapshot(filename) {
    const snapshotStream = v8.writeHeapSnapshot(filename);
    console.log(`Heap snapshot written: ${snapshotStream}`);
    return snapshotStream;
}

function getHeapStatistics() {
    const stats = v8.getHeapStatistics();
    return {
        totalHeapSize: `${(stats.total_heap_size / 1024 / 1024).toFixed(1)} MB`,
        usedHeapSize: `${(stats.used_heap_size / 1024 / 1024).toFixed(1)} MB`,
        heapSizeLimit: `${(stats.heap_size_limit / 1024 / 1024).toFixed(1)} MB`,
        totalAvailableSize: `${(stats.total_available_size / 1024 / 1024).toFixed(1)} MB`,
        mallocedMemory: `${(stats.malloced_memory / 1024 / 1024).toFixed(1)} MB`,
    };
}

// Monitor heap over time
let snapshots = 0;
function monitorHeap() {
    const stats = getHeapStatistics();
    const usedMB = parseFloat(stats.usedHeapSize);
    
    console.log(`[Heap] ${stats.usedHeapSize} / ${stats.totalHeapSize} (limit: ${stats.heapSizeLimit})`);
    
    // Auto-snapshot on high usage
    if (usedMB > 500) {
        snapshots++;
        takeHeapSnapshot(`heap-${snapshots}-${Date.now()}.heapsnapshot`);
    }
}

// Start monitoring
setInterval(monitorHeap, 5000);

// Simulate memory allocation
const data = [];
function allocateMemory() {
    for (let i = 0; i < 10000; i++) {
        data.push({
            id: i,
            name: `item-${i}`,
            data: Array.from({ length: 100 }, (_, j) => j * i),
            timestamp: Date.now(),
        });
    }
}

// Allocate every second
setInterval(allocateMemory, 1000);
console.log('Memory monitoring started. Press Ctrl+C to take final snapshot.');

process.on('SIGINT', () => {
    takeHeapSnapshot('final.heapsnapshot');
    console.log('Final heap snapshot taken. Analyze in Chrome DevTools.');
    process.exit(0);
});
```

### Analyzing Heap Snapshots in Chrome DevTools

```
Heap Snapshot Analysis Steps:
─────────────────────────────────────────────
1. Open chrome://inspect → Memory tab
2. Load .heapsnapshot file (or take live snapshot)
3. Switch between views:
   
   Summary view:
   - Grouped by constructor name
   - Shows object count and size
   - Look for large Retained Size values
   
   Comparison view:
   - Compare two snapshots
   - Shows objects allocated between snapshots
   - Growing objects indicate leaks
   
   Containment view:
   - Shows object hierarchy
   - Find what's keeping objects alive
   
4. Sort by "Retained Size" to find biggest memory consumers
5. Expand objects to see reference chains
6. Detached DOM nodes = memory leak indicators
```

## Flame Graphs

### Generating Flame Graphs

```bash
# Method 1: 0x (recommended)
npm install -g 0x
0x your-app.js
# Opens interactive flame graph in browser

# Method 2: Clinic.js
npm install -g clinic
clinic doctor -- node your-app.js
clinic flame -- node your-app.js

# Method 3: perf (Linux only)
perf record -F 99 -p $(pgrep -n node) -g -- sleep 30
perf script > perf.out
# Use flamegraph tools to visualize
```

### Using Clinic.js

```bash
# Install clinic
npm install -g clinic

# Doctor: Detects performance issues
clinic doctor -- node your-app.js

# Flame: CPU flame graph
clinic flame -- node your-app.js

# Bubbleprof: Async operations visualization
clinic bubbleprof -- node your-app.js
```

## Event Loop Profiling

### Measuring Event Loop Lag

Create `event-loop-monitor.js`:
```javascript
// event-loop-monitor.js — Monitor event loop health

const { monitorEventLoopDelay } = require('node:perf_hooks');

const histogram = monitorEventLoopDelay({ resolution: 20 });
histogram.enable();

setInterval(() => {
    const stats = {
        min: `${(histogram.min / 1e6).toFixed(2)}ms`,
        max: `${(histogram.max / 1e6).toFixed(2)}ms`,
        mean: `${(histogram.mean / 1e6).toFixed(2)}ms`,
        p50: `${(histogram.percentile(50) / 1e6).toFixed(2)}ms`,
        p90: `${(histogram.percentile(90) / 1e6).toFixed(2)}ms`,
        p99: `${(histogram.percentile(99) / 1e6).toFixed(2)}ms`,
        exceeds: histogram.exceeds,
    };
    
    console.log('[Event Loop]', stats);
    
    // Alert on high lag
    if (histogram.mean > 100e6) { // > 100ms
        console.warn('WARNING: Event loop lag exceeds 100ms');
    }
    
    histogram.reset();
}, 5000);

// Simulate blocking operation periodically
setInterval(() => {
    const start = Date.now();
    while (Date.now() - start < 200) {
        // Block for 200ms
    }
}, 15000);
```

### Event Loop Utilization

```javascript
// event-loop-utilization.js — Measure event loop efficiency

const { performance } = require('node:perf_hooks');

let lastUtilization = performance.eventLoopUtilization();

setInterval(() => {
    const current = performance.eventLoopUtilization(lastUtilization);
    
    console.log({
        utilization: `${(current.utilization * 100).toFixed(1)}%`,
        idle: `${(current.idle / 1000).toFixed(1)}s`,
        active: `${(current.active / 1000).toFixed(1)}s`,
    });
    
    // High utilization = event loop is busy
    // Low utilization = mostly waiting for I/O
    if (current.utilization > 0.9) {
        console.warn('Event loop is >90% utilized — consider scaling');
    }
    
    lastUtilization = performance.eventLoopUtilization();
}, 5000);
```

## Production Profiling

### Always-On Monitoring

```javascript
// production-monitor.js — Production-ready monitoring

const { monitorEventLoopDelay, eventLoopUtilization } = require('node:perf_hooks');

class ProductionMonitor {
    constructor(options = {}) {
        this.interval = options.interval || 30000;
        this.alertThreshold = options.alertThreshold || 100; // ms
        this.metrics = [];
    }
    
    start() {
        // Event loop delay monitoring
        this.histogram = monitorEventLoopDelay({ resolution: 20 });
        this.histogram.enable();
        this.lastELU = eventLoopUtilization();
        
        this.timer = setInterval(() => this.collect(), this.interval);
        console.log('Production monitor started');
    }
    
    collect() {
        const elu = eventLoopUtilization(this.lastELU);
        this.lastELU = eventLoopUtilization();
        
        const snapshot = {
            timestamp: Date.now(),
            memory: {
                rss: process.memoryUsage().rss,
                heapUsed: process.memoryUsage().heapUsed,
                heapTotal: process.memoryUsage().heapTotal,
            },
            eventLoop: {
                lagMs: this.histogram.mean / 1e6,
                p99LagMs: this.histogram.percentile(99) / 1e6,
                utilization: elu.utilization,
            },
            uptime: process.uptime(),
        };
        
        this.metrics.push(snapshot);
        
        // Keep last 100 snapshots
        if (this.metrics.length > 100) {
            this.metrics.shift();
        }
        
        // Alert on high lag
        if (snapshot.eventLoop.p99LagMs > this.alertThreshold) {
            console.error(`ALERT: Event loop p99 lag: ${snapshot.eventLoop.p99LagMs.toFixed(1)}ms`);
        }
        
        this.histogram.reset();
    }
    
    getMetrics() {
        return this.metrics;
    }
    
    stop() {
        clearInterval(this.timer);
        this.histogram.disable();
    }
}

// Usage
const monitor = new ProductionMonitor({ interval: 10000 });
monitor.start();

// Expose metrics via HTTP
const http = require('node:http');
http.createServer((req, res) => {
    if (req.url === '/metrics') {
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify(monitor.getMetrics()));
    } else {
        res.end('OK');
    }
}).listen(9090);
```

## Profiling Checklist

```
Performance Profiling Checklist:
─────────────────────────────────────────────
□ CPU Profile
  □ Run --prof on production-like workload
  □ Identify top 5 hot functions
  □ Check for unexpected deoptimizations
  
□ Memory Profile
  □ Take heap snapshot at startup
  □ Take heap snapshot under load
  □ Compare snapshots for growing objects
  □ Check for detached DOM nodes
  
□ Event Loop Profile
  □ Monitor event loop delay (p99)
  □ Measure event loop utilization
  □ Identify blocking operations
  
□ I/O Profile
  □ Check file descriptor usage
  □ Monitor network connections
  □ Verify connection pool sizing
  
□ Flame Graph
  □ Generate flame graph under load
  □ Identify wide (time-consuming) frames
  □ Look for optimization opportunities
```

## Cross-References

- See [Compilation Demonstration](./01-compilation-demonstration.md) for V8 internals
- See [Memory Optimization](./03-memory-optimization.md) for memory management
- See [Performance Deep Dive](../09-performance-deep-dive/01-performance-characteristics.md) for optimization
- See [Event Loop Debugging](../06-event-loop-mechanics/03-event-loop-debugging.md) for async profiling

## Next Steps

Continue to [Memory Optimization](./03-memory-optimization.md) for memory management techniques.
