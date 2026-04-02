# Debugging Event Loop Issues

## What You'll Learn

- Identifying event loop blocking
- Debugging event loop lag in production
- Common event loop anti-patterns
- Tools and techniques for event loop analysis

## Detecting Event Loop Blocking

### Manual Detection

Create `event-loop-block-detector.js`:
```javascript
// Detect if the event loop is being blocked

const { monitorEventLoopDelay } = require('node:perf_hooks');

const histogram = monitorEventLoopDelay({ resolution: 20 });
histogram.enable();

setInterval(() => {
    const meanMs = histogram.mean / 1e6;
    const p99Ms = histogram.percentile(99) / 1e6;
    
    if (p99Ms > 100) {
        console.error(`[BLOCKED] Event loop p99 lag: ${p99Ms.toFixed(1)}ms`);
        console.trace('Stack trace at detection time:');
    } else if (meanMs > 50) {
        console.warn(`[WARNING] Event loop mean lag: ${meanMs.toFixed(1)}ms`);
    }
    
    histogram.reset();
}, 1000);
```

### Automated Detection with Diagnostics

```javascript
// diagnostics.js — Comprehensive event loop diagnostics

const { monitorEventLoopDelay, eventLoopUtilization } = require('node:perf_hooks');

class EventLoopDiagnostics {
    constructor(options = {}) {
        this.thresholds = {
            lagWarningMs: options.lagWarningMs || 50,
            lagCriticalMs: options.lagCriticalMs || 100,
            utilizationWarning: options.utilizationWarning || 0.8,
        };
        this.reports = [];
    }

    start() {
        this.histogram = monitorEventLoopDelay({ resolution: 20 });
        this.histogram.enable();
        this.lastELU = eventLoopUtilization();

        this.timer = setInterval(() => this.analyze(), 5000);
        console.log('Event loop diagnostics started');
    }

    analyze() {
        const elu = eventLoopUtilization(this.lastELU);
        this.lastELU = eventLoopUtilization();

        const lagMs = this.histogram.mean / 1e6;
        const p99Ms = this.histogram.percentile(99) / 1e6;

        const report = {
            timestamp: new Date().toISOString(),
            lagMeanMs: +lagMs.toFixed(2),
            lagP99Ms: +p99Ms.toFixed(2),
            utilization: +(elu.utilization * 100).toFixed(1) + '%',
            status: 'healthy',
            issues: [],
        };

        // Check lag
        if (p99Ms > this.thresholds.lagCriticalMs) {
            report.status = 'critical';
            report.issues.push(`P99 lag ${p99Ms.toFixed(1)}ms exceeds ${this.thresholds.lagCriticalMs}ms`);
        } else if (lagMs > this.thresholds.lagWarningMs) {
            report.status = 'warning';
            report.issues.push(`Mean lag ${lagMs.toFixed(1)}ms exceeds ${this.thresholds.lagWarningMs}ms`);
        }

        // Check utilization
        if (elu.utilization > this.thresholds.utilizationWarning) {
            report.status = report.status === 'critical' ? 'critical' : 'warning';
            report.issues.push(`Utilization ${(elu.utilization * 100).toFixed(1)}% exceeds threshold`);
        }

        this.reports.push(report);
        if (this.reports.length > 100) this.reports.shift();

        // Log issues
        if (report.issues.length > 0) {
            console.warn(`[EventLoop][${report.status.toUpperCase()}]`, report.issues.join('; '));
        }

        this.histogram.reset();
        return report;
    }

    getSummary() {
        if (this.reports.length === 0) return { status: 'no data' };

        const criticalCount = this.reports.filter(r => r.status === 'critical').length;
        const warningCount = this.reports.filter(r => r.status === 'warning').length;

        return {
            totalSamples: this.reports.length,
            criticalEvents: criticalCount,
            warningEvents: warningCount,
            status: criticalCount > 0 ? 'critical' : warningCount > 0 ? 'warning' : 'healthy',
        };
    }

    stop() {
        clearInterval(this.timer);
        this.histogram.disable();
    }
}

module.exports = { EventLoopDiagnostics };
```

## Common Anti-Patterns

### Anti-Pattern 1: Synchronous File Operations

```javascript
// BAD: Blocks event loop
const fs = require('node:fs');

app.get('/file', (req, res) => {
    const data = fs.readFileSync('/large-file.txt'); // BLOCKS!
    res.send(data);
});

// GOOD: Asynchronous
app.get('/file', (req, res) => {
    fs.readFile('/large-file.txt', (err, data) => {
        if (err) return res.status(500).send('Error');
        res.send(data);
    });
});

// BETTER: Streams
app.get('/file', (req, res) => {
    fs.createReadStream('/large-file.txt').pipe(res);
});
```

### Anti-Pattern 2: CPU-Intensive Synchronous Code

```javascript
// BAD: Blocks event loop
app.get('/primes/:max', (req, res) => {
    const primes = [];
    for (let i = 2; i <= req.params.max; i++) {
        let isPrime = true;
        for (let j = 2; j < i; j++) {
            if (i % j === 0) { isPrime = false; break; }
        }
        if (isPrime) primes.push(i);
    }
    res.json(primes); // Other requests are blocked!
});

// GOOD: Worker threads
const { Worker } = require('node:workers');

app.get('/primes/:max', (req, res) => {
    const worker = new Worker('./prime-worker.js', {
        workerData: +req.params.max,
    });
    worker.on('message', primes => res.json(primes));
    worker.on('error', err => res.status(500).json({ error: err.message }));
});
```

### Anti-Pattern 3: JSON Parsing Large Payloads

```javascript
// BAD: Synchronous JSON parsing of large data
app.post('/import', (req, res) => {
    const data = JSON.parse(req.body); // Blocks if body is large!
    // process data...
});

// GOOD: Stream-based JSON parsing
const { JSONParser } = require('stream-json');
const { chain } = require('stream-chain');

app.post('/import', (req, res) => {
    const pipeline = chain([
        req,
        JSONParser(),
        // Process each object as it streams in
        async function* (data) {
            for await (const item of data) {
                yield processItem(item);
            }
        },
    ]);
    
    pipeline.on('end', () => res.json({ success: true }));
});
```

### Anti-Pattern 4: Regex on Large Strings

```javascript
// BAD: Catastrophic backtracking blocks event loop
app.get('/search', (req, res) => {
    const text = largeDocument; // 10MB text
    const pattern = /^(a+)+$/; // Catastrophic regex
    const match = text.match(pattern); // BLOCKS for seconds!
    res.json({ match });
});

// GOOD: Use safer regex patterns
// Avoid: nested quantifiers, backreferences in loops
// Use: atomic groups, possessive quantifiers where possible
const safePattern = /^a+$/; // Linear time

// BETTER: Process in chunks for large text
async function searchLarge(text, pattern) {
    const chunkSize = 10000;
    for (let i = 0; i < text.length; i += chunkSize) {
        const chunk = text.slice(i, i + chunkSize);
        if (pattern.test(chunk)) return true;
        await new Promise(r => setImmediate(r)); // Yield
    }
    return false;
}
```

## Profiling Event Loop Issues

### CPU Profile During Block

```bash
# Take a CPU profile to find blocking code
node --prof app.js

# In another terminal, trigger the blocking operation
curl http://localhost:3000/blocking-endpoint

# Stop the server, process the profile
node --prof-process isolate-*.log | head -50
```

### Chrome DevTools Timeline

```bash
# Start with inspector
node --inspect app.js

# In Chrome DevTools:
# 1. Go to Performance tab
# 2. Click Record
# 3. Trigger the blocking operation
# 4. Click Stop
# 5. Look for:
#    - Long tasks (red triangles)
#    - Long JavaScript execution bars
#    - Event loop delay markers
```

### Using Clinic.js Doctor

```bash
# Clinic Doctor detects event loop issues
npm install -g clinic

clinic doctor -- node app.js
# Triggers load, generates report showing:
# - Event loop delays
# - CPU usage
# - Memory usage
# - Active handles
# - Recommendations
```

## Debugging Production Issues

### Event Loop Lag Endpoint

```javascript
// Add to your Express/Fastify app for monitoring

const { monitorEventLoopDelay } = require('node:perf_hooks');
const histogram = monitorEventLoopDelay({ resolution: 20 });
histogram.enable();

// Health check with event loop metrics
app.get('/health', (req, res) => {
    const elu = require('node:perf_hooks').eventLoopUtilization();
    
    res.json({
        status: 'healthy',
        eventLoop: {
            lagMeanMs: +(histogram.mean / 1e6).toFixed(2),
            lagP99Ms: +(histogram.percentile(99) / 1e6).toFixed(2),
            utilization: +(elu.utilization * 100).toFixed(1) + '%',
        },
        memory: {
            rssMB: +(process.memoryUsage().rss / 1024 / 1024).toFixed(1),
            heapUsedMB: +(process.memoryUsage().heapUsed / 1024 / 1024).toFixed(1),
        },
        uptime: process.uptime(),
    });
    
    histogram.reset();
});
```

### Structured Logging

```javascript
// Log event loop metrics alongside application events

const { monitorEventLoopDelay } = require('node:perf_hooks');
const histogram = monitorEventLoopDelay({ resolution: 20 });
histogram.enable();

function logWithMetrics(level, message, extra = {}) {
    const metrics = {
        eventLoopLagMs: +(histogram.mean / 1e6).toFixed(2),
        heapUsedMB: +(process.memoryUsage().heapUsed / 1024 / 1024).toFixed(1),
    };
    
    console.log(JSON.stringify({
        timestamp: new Date().toISOString(),
        level,
        message,
        ...metrics,
        ...extra,
    }));
}

// Usage
app.get('/api/data', async (req, res) => {
    logWithMetrics('info', 'Request started', { path: req.path });
    
    const data = await fetchData();
    
    logWithMetrics('info', 'Request completed', { 
        path: req.path,
        durationMs: Date.now() - req.startTime 
    });
    
    res.json(data);
});
```

## Debugging Checklist

```
Event Loop Debugging Checklist:
─────────────────────────────────────────────
□ Baseline
  □ Measure event loop lag under normal load
  □ Record typical heap usage
  □ Document expected response times

□ When lag detected
  □ Take CPU profile (--prof or Chrome DevTools)
  □ Check for synchronous operations
  □ Review recent code changes
  □ Check npm audit for vulnerable deps

□ Common causes
  □ Synchronous fs operations (readFileSync, writeFileSync)
  □ CPU-intensive computation on main thread
  □ Large JSON.parse/stringify operations
  □ Catastrophic regex backtracking
  □ Large Array.sort operations
  □ Synchronous crypto operations

□ Resolution
  □ Convert sync to async
  □ Move CPU work to worker threads
  □ Use streams for large data
  □ Add rate limiting
  □ Implement caching

□ Prevention
  □ Add event loop monitoring to health checks
  □ Use linter rules against sync operations
  □ Set event loop lag alerts
  □ Run clinic doctor in CI
```

## Best Practices Checklist

- [ ] Monitor event loop lag in production (p99 < 100ms)
- [ ] Never use `fs.*Sync` in request handlers
- [ ] Use worker threads for CPU-intensive operations
- [ ] Implement graceful degradation when event loop is lagged
- [ ] Add `/health` endpoint with event loop metrics
- [ ] Use structured logging with event loop metrics
- [ ] Run `clinic doctor` periodically in staging
- [ ] Set up alerting for sustained event loop lag

## Cross-References

- See [Event Loop Deep Dive](./01-event-loop-deep-dive.md) for phase details
- See [Task Scheduling](./02-task-scheduling-prioritization.md) for prioritization
- See [Performance Profiling](../13-v8-engine-practice/02-performance-profiling.md) for profiling tools
- See [Performance Deep Dive](../09-performance-deep-dive/01-performance-characteristics.md) for optimization

## Next Steps

Continue to [Use Case Analysis](../07-use-case-analysis/02-application-scenarios.md) for application patterns.
