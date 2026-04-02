# Node.js vs Compiled Languages (Go, Rust, Java)

## What You'll Learn

- Performance comparison with compiled languages
- When to choose Node.js over compiled alternatives
- Hybrid architecture patterns
- Migration considerations

## Performance Comparison

### HTTP Server Throughput

```
HTTP Server Requests/sec (simple JSON response):
─────────────────────────────────────────────
Rust (Actix)     ████████████████████████ 150,000
Go (net/http)    ██████████████████████   120,000
Java (Vert.x)    ████████████████████     100,000
Node.js (Fastify)████████████████         58,000
Node.js (Express)███████████              35,000
Python (FastAPI) ██████                   8,000
Ruby (Rails)     ██                       3,000

Note: These are approximate benchmarks.
Real-world performance depends on workload.
```

### When I/O-Bound: Node.js Wins

```
I/O-Bound Workload (API + Database + Cache):
─────────────────────────────────────────────
All runtimes perform similarly because they're
waiting on I/O, not computing.

Node.js:    15,000 req/sec (50ms p99)
Go:         16,000 req/sec (48ms p99)
Java:       14,000 req/sec (52ms p99)
Rust:       17,000 req/sec (45ms p99)

The bottleneck is the database, not the runtime.
```

### When CPU-Bound: Compiled Languages Win

```
CPU-Bound Workload (image processing):
─────────────────────────────────────────────
Rust:       500 images/sec
Go:         400 images/sec
Java:       350 images/sec
Node.js:    50 images/sec (single thread)
Node.js:    200 images/sec (4 worker threads)

Use worker threads to close the gap.
```

## Decision Framework

```
Node.js vs Compiled Languages:
─────────────────────────────────────────────
Choose Node.js when:
├── I/O-bound workload (APIs, real-time)
├── JavaScript/TypeScript team exists
├── Rapid development needed
├── Real-time features (WebSockets)
└── Full-stack JavaScript desired

Choose Go when:
├── Microservices infrastructure
├── CLI tools
├── Network services
├── High concurrency with low memory
└── Simple deployment (single binary)

Choose Rust when:
├── Maximum performance critical
├── Memory safety without GC
├── Systems programming
├── WebAssembly targets
└── Safety-critical applications

Choose Java when:
├── Enterprise environment
├── Existing Java ecosystem
├── Complex business logic
├── Team has Java expertise
└── Long-term maintenance
```

## Hybrid Architecture

```javascript
// Node.js API Gateway + Go microservices
// Node.js handles: routing, auth, rate limiting, WebSocket
// Go handles: computation, data processing

// Node.js gateway
import express from 'express';

const app = express();

// I/O-heavy routes → Node.js handles directly
app.get('/api/users', async (req, res) => {
    const users = await db.getUsers(); // Node.js excels here
    res.json(users);
});

// CPU-heavy routes → Delegate to Go service
app.post('/api/process-image', async (req, res) => {
    const result = await fetch('http://go-service:8081/process', {
        method: 'POST',
        body: req.body,
    });
    res.json(await result.json());
});
```

## Best Practices Checklist

- [ ] Benchmark for your specific workload
- [ ] Consider team expertise in decision
- [ ] Use hybrid architectures for mixed workloads
- [ ] Profile before assuming runtime is the bottleneck
- [ ] Consider maintenance cost, not just performance

## Cross-References

- See [Runtime Comparison Matrix](./01-runtime-matrix.md) for Deno/Bun comparison
- See [Decision Framework](./03-decision-framework.md) for selection guidance
- See [Performance Deep Dive](../09-performance-deep-dive/01-performance-characteristics.md) for optimization

## Next Steps

Continue to [Decision Framework](./03-decision-framework.md) for comprehensive runtime selection.
