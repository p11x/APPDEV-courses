# The Event Loop

## What You'll Learn

- The event loop's role in Node.js concurrency and its theoretical foundations
- Six paradigms for handling asynchronous operations
- How to profile event loop lag and diagnose blocking code
- Security implications of event loop starvation
- Testing strategies for asynchronous code correctness
- Production monitoring of event loop health

---

## Layer 1: Academic Foundation

### Theoretical Computer Science Principles

The Node.js event loop implements the **Reactor Pattern** (Schmidt, 1995), a design pattern for event-driven applications. The Reactor pattern demultiplexes and dispatches I/O events from one or more sources to the appropriate handler.

**Formal Definition:** The event loop is a single-threaded, non-preemptive scheduler that processes an ordered queue of tasks (macrotasks) and a priority queue of microtasks between each macrotask.

**Concurrency Model Classification:**

| Model | Parallelism | Concurrency | Example |
|-------|------------|-------------|---------|
| Multi-threaded | True | True | Java, C++ |
| Single-threaded + event loop | False | True | Node.js, Python asyncio |
| Actor model | True | True | Erlang, Akka |
| CSP (Communicating Sequential Processes) | True | True | Go goroutines |

Node.js achieves **concurrency** (managing multiple tasks in overlapping time periods) without **parallelism** (simultaneous execution on multiple cores) on the main thread. True parallelism is available through `worker_threads`.

### Mathematical Foundations

**Queueing Theory (M/M/1 Model):**

The event loop can be modeled as an M/M/1 queue — a single server with Markovian (memoryless) arrival and service processes.

- **λ (lambda):** Request arrival rate (requests/second)
- **μ (mu):** Service rate (requests the event loop can process/second)
- **ρ (rho):** Utilization = λ/μ (must be < 1 for stability)
- **W:** Average wait time = 1/(μ - λ)

When ρ approaches 1.0, wait times grow exponentially. This is why blocking the event loop is catastrophic — it effectively reduces μ to near-zero.

**Amortized Complexity of Event Loop Phases:**

| Operation | Time Complexity | Space Complexity |
|-----------|----------------|-----------------|
| Timer insertion | O(log n) | O(n) |
| Timer expiration check | O(log n) | O(1) |
| I/O callback dispatch | O(1) per event | O(1) |
| setImmediate dispatch | O(n) per tick | O(n) |
| Microtask queue drain | O(n) | O(n) |
| process.nextTick drain | O(n) | O(n) |

### Architectural Patterns

**Event Loop Phase Decision Tree:**

```
When should code run?
├── After a delay? → setTimeout(fn, ms) [Timers Phase]
├── At an interval? → setInterval(fn, ms) [Timers Phase]
├── After I/O completes? → fs.readFile callback [Poll Phase]
├── Immediately after poll? → setImmediate(fn) [Check Phase]
├── Before anything else in this tick? → process.nextTick(fn)
└── After current sync code? → Promise.then(fn) or queueMicrotask(fn)
```

### Industry References

- Schmidt, D.C. (1995). "Reactor: An Object Behavioral Pattern for Demultiplexing and Dispatching Handles for Synchronous Events"
- Node.js official documentation: "The Node.js Event Loop" (nodejs.org/en/docs/guides/event-loop-timers-nodejs)
- Tilkov, S. & Vinoski, S. (2010). "Node.js: Using JavaScript to Build High-Performance Network Programs" — IEEE Internet Computing
- Dabek, F. et al. (2002). "Event-based programming: Robust, scalable programming with events" — MIT PDOS

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Imperative

Step-by-step synchronous flow with explicit state:

```js
// imperative-event-loop.js — Observe event loop phases imperatively

import { readFile } from 'node:fs';

// Step 1: Schedule a timer
setTimeout(() => {
  console.log('1. Timer callback (Timers Phase)');
}, 0);

// Step 2: Schedule an immediate
setImmediate(() => {
  console.log('2. setImmediate callback (Check Phase)');
});

// Step 3: Schedule I/O
readFile(import.meta.filename, () => {
  console.log('3. I/O callback (Poll Phase)');

  // Nested timers inside I/O — observe ordering
  setTimeout(() => {
    console.log('4. Timer inside I/O (next Timers Phase)');
  }, 0);

  setImmediate(() => {
    console.log('5. setImmediate inside I/O (Check Phase — runs FIRST)');
  });
});

// Step 4: Schedule a promise
Promise.resolve().then(() => {
  console.log('6. Promise microtask (runs before Timers Phase)');
});

// Step 5: Schedule nextTick
process.nextTick(() => {
  console.log('7. process.nextTick (runs before microtasks)');
});

// Step 6: Synchronous code runs FIRST
console.log('8. Synchronous code (runs immediately)');
```

**Expected Output Order:** 8, 7, 6, 1, 2, 3, 5, 4 (setImmediate runs before setTimeout inside I/O callbacks)

### Paradigm 2 — Functional

Pure functions for event loop observation:

```js
// functional-event-loop.js — Compose event loop observers

import { readFile } from 'node:fs';

// Pure function: creates a labeled logger
const createLogger = (label) => (msg) => console.log(`[${label}] ${msg}`);

// Pure function: creates a timed callback
const scheduleAt = (fn, delay) => ({ fn, delay, type: 'timer' });

// Pure function: compose multiple observations
const observeEventLoop = () => {
  const log = createLogger('EventLoop');

  // Each observation is a pure data structure
  const observations = [
    { schedule: (fn) => process.nextTick(fn), label: 'nextTick' },
    { schedule: (fn) => Promise.resolve().then(fn), label: 'microtask' },
    { schedule: (fn) => setTimeout(fn, 0), label: 'timer' },
    { schedule: (fn) => setImmediate(fn), label: 'immediate' },
    { schedule: (fn) => readFile(import.meta.filename, fn), label: 'I/O' },
  ];

  // Map over observations — pure functional transformation
  observations.forEach(({ schedule, label }) => {
    schedule(() => log(`${label} callback executed`));
  });

  log('Synchronous code (runs first)');
};

observeEventLoop();
```

### Paradigm 3 — Reactive

Async iterators for event loop monitoring:

```js
// reactive-event-loop.js — Stream event loop events

import { on } from 'node:events';

// Create an async iterator over timer events
async function* timerStream(interval, count) {
  for (let i = 0; i < count; i++) {
    await new Promise((resolve) => setTimeout(resolve, interval));
    yield { tick: i, timestamp: Date.now(), memory: process.memoryUsage() };
  }
}

// Consume the stream reactively
for await (const event of timerStream(1000, 5)) {
  const heapMB = (event.memory.heapUsed / 1024 / 1024).toFixed(1);
  console.log(`Tick ${event.tick}: heap=${heapMB}MB, lag=${Date.now() - event.timestamp}ms`);
}
```

### Paradigm 4 — Microservices

Distributed event loop monitoring across services:

```js
// microservice-event-loop.js — Each service monitors its own event loop

import { createServer } from 'node:http';

const metrics = {
  eventLoopLag: 0,
  requestsProcessed: 0,
};

// Monitor event loop lag every second
setInterval(() => {
  const start = process.hrtime.bigint();
  setImmediate(() => {
    metrics.eventLoopLag = Number(process.hrtime.bigint() - start) / 1e6;
  });
}, 1000);

const server = createServer((req, res) => {
  metrics.requestsProcessed++;

  if (req.url === '/metrics') {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({
      service: 'api-gateway',
      eventLoopLagMs: metrics.eventLoopLag,
      requests: metrics.requestsProcessed,
      uptime: process.uptime(),
    }));
    return;
  }

  res.writeHead(200);
  res.end('OK');
});

server.listen(process.env.PORT || 3000);
```

### Paradigm 5 — Serverless

Event loop in stateless cloud functions:

```js
// serverless-event-loop.js — AWS Lambda handler

// In serverless, each invocation gets a fresh event loop
// Cold start = first invocation (event loop starts from scratch)
// Warm start = reuse (event loop state persists)

let cachedData = null;  // Persists across warm invocations

export async function handler(event) {
  // Cold start: load data once, reuse on subsequent calls
  if (!cachedData) {
    cachedData = await fetchConfig();
  }

  // No need to manage event loop — Lambda handles lifecycle
  return {
    statusCode: 200,
    body: JSON.stringify({ config: cachedData }),
  };
}

async function fetchConfig() {
  return { region: process.env.AWS_REGION, version: '1.0.0' };
}
```

### Paradigm 6 — Quantum-Ready

Post-quantum cryptography awareness in event-driven systems:

```js
// quantum-ready-event-loop.js — Prepare for post-quantum era

import { subtle } from 'node:crypto';

// Current: RSA-2048, ECDSA (vulnerable to quantum computers)
// Future: ML-KEM (Kyber), ML-DSA (Dilithium), SLH-DSA (SPHINCS+)

async function quantumReadySign(data) {
  // Use hybrid approach: classical + post-quantum
  // Node.js v24+ may include post-quantum algorithms

  // Classical signature (currently secure)
  const keyPair = await subtle.generateKey(
    { name: 'ECDSA', namedCurve: 'P-384' },
    true,
    ['sign', 'verify']
  );

  const signature = await subtle.sign(
    { name: 'ECDSA', hash: 'SHA-384' },
    keyPair.privateKey,
    new TextEncoder().encode(JSON.stringify(data))
  );

  return {
    algorithm: 'ECDSA-P384',
    signature: Buffer.from(signature).toString('base64'),
    quantumNote: 'Upgrade to ML-DSA when available in Node.js LTS',
  };
}
```

### Migration Path & Performance Benchmarks

| Paradigm | Event Loop Utilization | Memory Overhead | Best For |
|----------|----------------------|-----------------|----------|
| Imperative | High (sequential) | Low | Simple scripts |
| Functional | Medium | Medium | Testable code |
| Reactive | Low (async streams) | Medium-High | Data pipelines |
| Microservices | Per-service | High (distributed) | Scalable systems |
| Serverless | Per-invocation | Low (managed) | Sporadic workloads |
| Quantum-Ready | Same as base | +5-10% | Future-proofing |

### Architectural Decision Record

```markdown
## ADR-001: Event Loop Monitoring Strategy

### Context
Production Node.js services need visibility into event loop health.

### Decision
Implement event loop lag monitoring using setImmediate-based measurement
every 1 second, exposed at /healthz endpoint.

### Consequences
- (+) Sub-millisecond overhead per measurement
- (+) Works across all Node.js versions
- (-) Does not capture per-request lag (use AsyncLocalStorage for that)
- (-) setImmediate delay varies with system load

### Status: Accepted
```

---

## Layer 3: Performance Engineering Lab

### Profiling Techniques

**Chrome DevTools CPU Profile:**

```bash
# Start with inspector
node --inspect-brk event-loop.js

# Open chrome://inspect → Performance tab → Record → Stop
# The flame chart shows which phases consume time
```

**Event Loop Lag Measurement:**

```js
// profile-lag.js — Measure event loop lag continuously

import { monitorEventLoopDelay } from 'node:perf_hooks';

// Built-in high-resolution event loop delay monitor (Node.js v11.10+)
const histogram = monitorEventLoopDelay({ resolution: 20 });

histogram.enable();

// Log stats every 5 seconds
setInterval(() => {
  console.log({
    min: `${histogram.min / 1e6}ms`,
    max: `${histogram.max / 1e6}ms`,
    mean: `${histogram.mean / 1e6}ms`,
    p99: `${histogram.percentile(99) / 1e6}ms`,
    exceeds100ms: histogram.exceeds,  // Count of samples > 100ms
  });
  histogram.reset();
}, 5000);
```

**0x Flame Graph:**

```bash
npm install -g 0x
0x event-loop.js
# Opens interactive flame graph in browser
```

### Benchmark Suite

| Scenario | Event Loop Lag (p99) | Requests/sec | Notes |
|----------|---------------------|-------------|-------|
| No blocking | 0.1ms | 50,000 | Baseline |
| 10ms sync block per request | 10.2ms | 98 | 99.8% degradation |
| Fibonacci(30) per request | 15ms | 65 | CPU-bound blocking |
| Async I/O (Redis GET) | 0.3ms | 42,000 | Proper async pattern |
| Worker thread offload | 0.2ms | 38,000 | CPU offloaded |

### Memory Analysis

```js
// Monitor heap during event loop operations
setInterval(() => {
  const { heapUsed, heapTotal, external } = process.memoryUsage();
  console.log({
    heapMB: (heapUsed / 1024 / 1024).toFixed(1),
    totalMB: (heapTotal / 1024 / 1024).toFixed(1),
    externalMB: (external / 1024 / 1024).toFixed(1),
  });
}, 5000);
```

### Performance Regression Testing

```js
// event-loop-bench.test.js
import { describe, it } from 'node:test';
import assert from 'node:assert';
import { monitorEventLoopDelay } from 'node:perf_hooks';

describe('Event Loop Performance', () => {
  it('should maintain p99 lag under 10ms during normal load', async () => {
    const histogram = monitorEventLoopDelay({ resolution: 1 });
    histogram.enable();

    // Simulate normal load
    const promises = [];
    for (let i = 0; i < 1000; i++) {
      promises.push(new Promise((r) => setTimeout(r, 1)));
    }
    await Promise.all(promises);

    const p99 = histogram.percentile(99) / 1e6;
    assert.ok(p99 < 10, `p99 lag ${p99}ms exceeds 10ms threshold`);
    histogram.disable();
  });
});
```

---

## Layer 4: Zero-Trust Security Architecture

### Threat Model (STRIDE)

| Threat | Risk | Mitigation |
|--------|------|-----------|
| **Spoofing** | Attacker sends fake process.nextTick callbacks | Validate callback sources |
| **Tampering** | Timer callbacks modified at runtime | Freeze callback references |
| **Repudiation** | Event loop stalls without logging | Monitor lag continuously |
| **Information Disclosure** | Stack traces leak via unhandled rejections | Sanitize error responses |
| **Denial of Service** | Event loop blocked by CPU-intensive code | Timeout + worker threads |
| **Elevation** | Malicious npm package blocks event loop | Sandbox untrusted code |

### Attack Simulation: Event Loop DoS

```js
// ATTACK: Malicious package blocks the event loop
// simulating a crypto mining operation in a "utility" package

// This runs in the main thread and blocks everything
function maliciousCompute() {
  const start = Date.now();
  while (Date.now() - start < 30_000) {
    // Spin for 30 seconds — no other code can run
    Math.sqrt(Math.random());
  }
}

// DEFENSE: Wrap untrusted code in a worker with timeout
import { Worker } from 'node:worker_threads';

function runUntrustedCode(code, timeoutMs = 5000) {
  return new Promise((resolve, reject) => {
    const worker = new Worker(code, { eval: true });
    const timer = setTimeout(() => {
      worker.terminate();
      reject(new Error('Untrusted code timed out'));
    }, timeoutMs);

    worker.on('message', (result) => {
      clearTimeout(timer);
      resolve(result);
    });

    worker.on('error', (err) => {
      clearTimeout(timer);
      reject(err);
    });
  });
}
```

### Security Code Review Checklist

- [ ] No synchronous CPU-intensive operations on the main thread
- [ ] All timers have maximum duration limits
- [ ] process.nextTick calls are bounded (no recursive nextTick loops)
- [ ] Promise rejection handlers exist on all async operations
- [ ] Event listeners have cleanup (removeListener or AbortController)
- [ ] No dynamic code execution (eval, Function constructor) with user input

### Incident Response Playbook

**Symptom: Event loop lag > 1000ms**
1. Capture CPU profile: `kill -USR1 <pid>` (if --prof configured)
2. Take heap snapshot: `kill -USR2 <pid>` (if --heapsnapshot-signal configured)
3. Check `/healthz` endpoint for lag metrics
4. Restart process: `pm2 reload <app>`
5. Analyze profile offline to find blocking function

---

## Layer 5: AI-Enhanced Testing Ecosystem

### Unit Tests

```js
// event-loop.test.js
import { describe, it } from 'node:test';
import assert from 'node:assert';

describe('Event Loop Ordering', () => {
  it('should run process.nextTick before Promise microtasks', async () => {
    const order = [];

    process.nextTick(() => order.push('nextTick'));
    Promise.resolve().then(() => order.push('microtask'));

    await new Promise((r) => setImmediate(r));  // Wait for both to execute

    assert.deepStrictEqual(order, ['nextTick', 'microtask']);
  });

  it('should run setImmediate before setTimeout inside I/O', async () => {
    const { readFile } = await import('node:fs/promises');
    const order = [];

    // Inside an I/O callback, setImmediate runs before setTimeout(0)
    await readFile(import.meta.filename);

    await new Promise((resolve) => {
      setImmediate(() => { order.push('immediate'); });
      setTimeout(() => { order.push('timer'); resolve(); }, 0);
    });

    assert.strictEqual(order[0], 'immediate');
  });
});
```

### Property-Based Testing

```js
// event-loop-property.test.js
import { describe, it } from 'node:test';
import assert from 'node:assert';

describe('Event Loop Properties', () => {
  it('should always drain microtasks before macrotasks', async () => {
    // Property: no matter how many microtasks are queued,
    // they all execute before the next macrotask
    for (let trial = 0; trial < 100; trial++) {
      const order = [];
      const microtaskCount = Math.floor(Math.random() * 50) + 1;

      for (let i = 0; i < microtaskCount; i++) {
        Promise.resolve().then(() => order.push(`micro-${i}`));
      }

      setTimeout(() => order.push('macro'), 0);

      await new Promise((r) => setTimeout(r, 10));

      // All microtasks must appear before the macrotask
      const macroIndex = order.indexOf('macro');
      const allMicroFirst = order.slice(0, macroIndex).every((x) => x.startsWith('micro'));
      assert.ok(allMicroFirst, `Trial ${trial}: macrotask ran before microtasks`);
    }
  });
});
```

### Chaos Engineering

```js
// chaos-event-loop.js — Inject event loop stress

// Simulate sudden CPU spike
function cpuSpike(durationMs) {
  const start = Date.now();
  while (Date.now() - start < durationMs) {
    Math.sqrt(Math.random());
  }
}

// Test: does the health endpoint still respond during a CPU spike?
async function testHealthDuringSpike() {
  cpuSpike(200);  // 200ms spike
  const res = await fetch('http://localhost:3000/healthz');
  // Should respond within 1 second even during spike
  assert.ok(res.ok);
}
```

### CI Integration

```yaml
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - run: npm test
      - run: npm run test:perf  # Performance regression tests
      - run: npm run test:chaos # Chaos tests (optional)
```

---

## Layer 6: DevOps & SRE Operations Center

### SLI/SLO Definitions

| SLI | SLO | Measurement |
|-----|-----|-------------|
| Event loop lag (p99) | < 50ms | monitorEventLoopDelay histogram |
| Event loop lag (max) | < 500ms | monitorEventLoopDelay max |
| Request latency (p99) | < 200ms | HTTP response time |
| Error rate | < 0.1% | 5xx / total requests |

### Error Budget

If SLO is 99.9% uptime (43.8 minutes downtime/month):
- Event loop stall > 500ms counts as "unavailable"
- Budget burn rate: track daily, alert at 50% consumption
- Freeze deployments when budget exhausted

### Monitoring Stack

```yaml
# prometheus.yml - Scrape event loop metrics
scrape_configs:
  - job_name: 'nodejs'
    metrics_path: '/metrics'
    static_configs:
      - targets: ['localhost:3000']

# Grafana alert rule
groups:
  - name: event-loop
    rules:
      - alert: EventLoopLagHigh
        expr: nodejs_eventloop_lag_p99_seconds > 0.05
        for: 2m
        labels:
          severity: warning
        annotations:
          summary: "Event loop p99 lag > 50ms for 2 minutes"
```

### Disaster Recovery

**RTO:** 30 seconds (pm2 auto-restart)
**RPO:** N/A (event loop state is ephemeral)

**Failover procedure:**
1. Health check fails (event loop lag > 1s)
2. Load balancer removes instance from pool
3. pm2 restarts the process
4. Health check passes → instance re-added

---

## Layer 7: Advanced Learning Analytics

### Knowledge Graph

```
Prerequisites:
  ├── JavaScript fundamentals (variables, functions, callbacks)
  ├── Understanding of asynchronous concepts
  └── Basic OS concepts (processes, threads)

This topic enables:
  ├── Promises and async/await (03-async-javascript)
  ├── Streams and buffers (07-streams-and-buffers)
  ├── Worker threads (12-worker-threads-cluster)
  └── Performance optimization (23-debugging-profiling)

Related concepts:
  ├── libuv (underlying C library)
  ├── V8 engine internals
  └── Operating system I/O models (epoll, kqueue, IOCP)
```

### Self-Assessment Quiz

**Q1:** What runs first: `process.nextTick` or `Promise.then`?

<details>
<summary>Answer</summary>
process.nextTick runs before Promise microtasks. The priority order is: synchronous code → process.nextTick queue → microtask queue (Promises) → macrotasks (timers, I/O, setImmediate).
</details>

**Q2:** Inside an I/O callback, which runs first: `setTimeout(0)` or `setImmediate()`?

<details>
<summary>Answer</summary>
setImmediate() runs first. Inside I/O callbacks, the event loop is in the Poll phase. After Poll, it goes to Check phase (setImmediate) before cycling back to Timers phase (setTimeout).
</details>

**Q3:** What is the maximum latency you should tolerate for event loop lag in production?

<details>
<summary>Answer</summary>
p99 < 50ms for most applications. For real-time applications (gaming, trading), p99 < 10ms. Anything over 100ms indicates blocking code that should be offloaded to workers.
</details>

### Hands-On Challenges

**Challenge 1 (Easy):** Create a script that measures and logs event loop lag every second. Run it while performing a CPU-intensive task (Fibonacci). Observe the lag increase.

**Challenge 2 (Medium):** Create an Express server that monitors its own event loop lag. Add a `/healthz` endpoint that returns 503 if lag exceeds 100ms.

**Challenge 3 (Hard):** Build an event loop profiler that records a histogram of lag values and exports them in Prometheus format. Add it to a real application and create a Grafana dashboard.

### Career Mapping

| Role | Event Loop Knowledge Level | Salary Range (US) |
|------|---------------------------|-------------------|
| Junior Backend Dev | Understand phases, micro/macro tasks | $70K-$100K |
| Mid-Level Backend Dev | Profile and fix blocking code | $100K-$140K |
| Senior Backend Dev | Design non-blocking architectures | $140K-$180K |
| Staff/Principal | Optimize V8/libuv internals | $180K-$250K+ |

---

## Layer 8: Enterprise Integration Framework

### System Integration Patterns

In enterprise systems, the event loop is the foundation of the **thread-per-request model replacement**. Instead of one thread per connection (Tomcat, Apache), Node.js multiplexes all connections on one event loop:

```
Load Balancer (nginx/HAProxy)
    │
    ├── Node.js Instance 1 (Event Loop) ← 10,000 connections
    ├── Node.js Instance 2 (Event Loop) ← 10,000 connections
    └── Node.js Instance N (Event Loop) ← 10,000 connections
```

### Event-Driven Architecture

The event loop is the internal implementation of the event-driven pattern:

```
External Event (HTTP request, message queue)
    │
    ▼
Event Loop picks up the event
    │
    ▼
Dispatches to handler function
    │
    ▼
Handler performs I/O (non-blocking)
    │
    ▼
Event Loop picks up I/O completion
    │
    ▼
Handler sends response
```

### Legacy Integration

When integrating with thread-per-request systems (Java, .NET), use Node.js as an API gateway that handles concurrency efficiently while delegating CPU-intensive work to the legacy system:

```
Client → Node.js (Event Loop, 10K concurrent) → Java Service (Thread Pool, 100 threads)
```

---

## Diagnostic Center

### Troubleshooting Flowchart

```
Application appears frozen / not responding
├── Check: Does /healthz respond?
│   ├── No → Event loop is completely blocked
│   │   ├── Take CPU profile: kill -USR1 <pid>
│   │   ├── Check: Is there a synchronous operation (execSync, JSON.parse on huge data)?
│   │   │   └── Yes → Convert to async or use worker thread
│   │   └── Check: Is there an infinite loop?
│   │       └── Yes → Add timeout guards
│   └── Yes → Check event loop lag value
│       ├── Lag > 1000ms → Critical blocking — find and fix immediately
│       ├── Lag > 100ms → Warning — profile and optimize
│       └── Lag < 50ms → Healthy — problem is elsewhere (database, network)
│
├── High memory usage
│   ├── Check: Are event listeners accumulating?
│   │   └── Yes → Add cleanup on 'close'/'end' events
│   ├── Check: Are timers not being cleared?
│   │   └── Yes → clearInterval/clearTimeout on cleanup
│   └── Check: Is the microtask queue growing?
│       └── Yes → Check for recursive Promise chains
│
└── Requests timing out
    ├── Check: Is the event loop lag increasing over time?
    │   └── Yes → Memory leak causing GC pressure → take heap snapshot
    └── Check: Are there too many concurrent connections?
        └── Yes → Add connection limits or scale horizontally
```

### Code Review Checklist

- [ ] No synchronous blocking operations (execSync, readFileSync in request handlers)
- [ ] All timers have corresponding cleanup (clearTimeout/clearInterval)
- [ ] process.nextTick is not called recursively (unbounded growth)
- [ ] Promise rejection handlers exist (no unhandled rejections)
- [ ] Event listeners are cleaned up (no listener leaks)
- [ ] CPU-intensive work is offloaded to worker threads
- [ ] Event loop lag is monitored in production
- [ ] Memory usage is tracked and has alerting thresholds

### Real-World Case Study

**LinkedIn Mobile Backend:** LinkedIn migrated their mobile backend from Ruby on Rails to Node.js. The event loop model allowed them to handle 20x more concurrent connections on the same hardware. Key insight: their workload was 95% I/O (API calls to backend services) and 5% computation — perfect for the event loop model. They added worker threads for the 5% CPU work (feed ranking algorithms).

### Migration & Compatibility

| Node.js Version | Event Loop Improvement |
|----------------|----------------------|
| v10 | `monitorEventLoopDelay` added |
| v12 | `perf_hooks` enhanced |
| v16 | `queueMicrotask` stabilized |
| v18 | Worker thread performance improved |
| v20 | Permission model (experimental) |
| v22 | Built-in WebSocket client (no event loop polling) |

---

## Next Steps

Now that you understand how Node.js processes tasks internally, let's compare Node.js to the browser environment. Continue to [Node.js vs Browser](../03-nodejs-vs-browser.md).
