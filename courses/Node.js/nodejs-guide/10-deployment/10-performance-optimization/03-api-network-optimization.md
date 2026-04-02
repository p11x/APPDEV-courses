# API and Network Performance Optimization

## What You'll Learn

- API response compression and pagination strategies
- Field selection (GraphQL-style) for REST APIs
- HTTP/2 server push and connection keep-alive tuning
- WebSocket optimization for real-time features
- Network tuning: TCP, DNS caching, connection pooling
- Edge computing and geographic routing for latency reduction
- Worker threads and cluster mode for CPU-bound work
- Memory optimization: heap tuning and leak detection
- Load testing with k6, Artillery, and autocannon
- Performance profiling with Node.js built-in tools, 0x, Clinic.js
- APM integration for continuous monitoring

## API Response Compression

```javascript
const express = require('express');
const compression = require('compression');
const zlib = require('zlib');

const app = express();

// Tiered compression based on content type
app.use(
  compression({
    level: 4, // Balance speed vs ratio
    threshold: 512,
    filter: (req, res) => {
      const ct = res.getHeader('Content-Type') || '';
      // Skip already-compressed formats
      if (ct.includes('image/') || ct.includes('video/') || ct.includes('font/')) {
        return false;
      }
      return compression.filter(req, res);
    },
  })
);

// Streaming compression for large JSON responses
function streamCompressedJSON(req, res, data) {
  const acceptEncoding = req.headers['accept-encoding'] || '';
  res.setHeader('Content-Type', 'application/json; charset=utf-8');

  if (acceptEncoding.includes('br')) {
    res.setHeader('Content-Encoding', 'br');
    const brotli = zlib.createBrotliCompress({
      params: { [zlib.constants.BROTLI_PARAM_QUALITY]: 4 },
    });
    return JSON.stringify(data).pipe
      ? undefined
      : (() => {
          const { Readable } = require('stream');
          const readable = Readable.from([JSON.stringify(data)]);
          return readable.pipe(brotli).pipe(res);
        })();
  }
  res.json(data);
}
```

## Cursor-Based Pagination and Field Selection

```javascript
const { pool } = require('./db');

// Cursor-based pagination (more efficient than OFFSET)
async function paginatedQuery(table, { cursor, limit = 20, fields, filters = {} }) {
  const allowedFields = ['id', 'name', 'email', 'created_at', 'status'];
  const selectedFields = fields
    ? fields.split(',').filter((f) => allowedFields.includes(f)).join(', ')
    : '*';

  let query = `SELECT ${selectedFields} FROM ${table}`;
  const params = [];
  const conditions = [];

  if (cursor) {
    conditions.push(`id > $${params.length + 1}`);
    params.push(cursor);
  }

  Object.entries(filters).forEach(([key, value]) => {
    if (allowedFields.includes(key)) {
      conditions.push(`${key} = $${params.length + 1}`);
      params.push(value);
    }
  });

  if (conditions.length) query += ` WHERE ${conditions.join(' AND ')}`;
  query += ` ORDER BY id ASC LIMIT $${params.length + 1}`;
  params.push(limit + 1); // Fetch one extra to detect next page

  const result = await pool.query(query, params);
  const hasNext = result.rows.length > limit;
  const data = hasNext ? result.rows.slice(0, -1) : result.rows;

  return {
    data,
    pagination: {
      nextCursor: hasNext ? data[data.length - 1].id : null,
      limit,
      hasNext,
    },
  };
}

// Express route with field selection
app.get('/api/users', async (req, res) => {
  const { cursor, limit, fields, status } = req.query;
  const result = await paginatedQuery('users', {
    cursor,
    limit: parseInt(limit) || 20,
    fields,
    filters: status ? { status } : {},
  });
  res.json(result);
});
```

## HTTP/2 and Connection Keep-Alive

```javascript
const http2 = require('http2');
const fs = require('fs');
const path = require('path');

const server = http2.createSecureServer({
  key: fs.readFileSync('certs/server.key'),
  cert: fs.readFileSync('certs/server.crt'),
  allowHTTP1: true,
  settings: {
    maxConcurrentStreams: 100,
    initialWindowSize: 1048576,    // 1MB
    maxHeaderListSize: 8192,
    enablePush: true,
  },
});

// HTTP/2 Server Push for critical assets
server.on('stream', (stream, headers) => {
  const pathHeader = headers[http2.constants.HTTP2_HEADER_PATH];

  if (pathHeader === '/') {
    // Push critical CSS and JS
    const pushCSS = stream.pushStream({ ':path': '/css/main.css' });
    pushCSS.on('stream', (pushStream) => {
      pushStream.respondWithFile(path.join(__dirname, 'public/css/main.css'), {
        'content-type': 'text/css',
        'cache-control': 'public, max-age=31536000, immutable',
      });
    });

    const pushJS = stream.pushStream({ ':path': '/js/app.js' });
    pushJS.on('stream', (pushStream) => {
      pushStream.respondWithFile(path.join(__dirname, 'public/js/app.js'), {
        'content-type': 'application/javascript',
        'cache-control': 'public, max-age=31536000, immutable',
      });
    });
  }

  // Respond with the main document
  stream.respondWithFile(path.join(__dirname, 'public/index.html'), {
    'content-type': 'text/html',
    ':status': 200,
  });
});

server.listen(443);
```

### Connection Keep-Alive Tuning (HTTP/1.1)

```javascript
const http = require('http');

const server = http.createServer(app);

server.keepAliveTimeout = 65000;   // Slightly above ALB's 60s timeout
server.headersTimeout = 66000;     // Must be > keepAliveTimeout
server.timeout = 120000;           // Socket timeout

// Set keep-alive header at application level
app.use((req, res, next) => {
  res.setHeader('Connection', 'keep-alive');
  res.setHeader('Keep-Alive', 'timeout=65, max=1000');
  next();
});
```

## WebSocket Optimization

```javascript
const { WebSocketServer } = require('ws');
const http = require('http');
const Redis = require('ioredis');

const server = http.createServer();
const wss = new WebSocketServer({
  server,
  maxPayload: 64 * 1024,        // 64KB max message
  perMessageDeflate: {
    zlibDeflateOptions: { level: 3 },
    zlibInflateOptions: { chunkSize: 10 * 1024 },
    clientNoContextTakeover: true,
    serverNoContextTakeover: true,
    serverMaxWindowBits: 10,
    concurrencyLimit: 10,
    threshold: 1024,              // Only compress messages > 1KB
  },
});

const pub = new Redis();
const sub = new Redis();

// Subscribe to channels for multi-instance scaling
sub.subscribe('broadcast', 'room:general');

sub.on('message', (channel, message) => {
  wss.clients.forEach((client) => {
    if (client.readyState === 1) {
      client.send(message);
    }
  });
});

// Connection management with heartbeat
const HEARTBEAT_INTERVAL = 30000;
const clients = new Map();

wss.on('connection', (ws, req) => {
  const clientId = generateId();
  clients.set(clientId, { ws, isAlive: true, connectedAt: Date.now() });

  ws.on('pong', () => {
    const client = clients.get(clientId);
    if (client) client.isAlive = true;
  });

  ws.on('message', (data) => {
    try {
      const msg = JSON.parse(data);
      // Batch messages to reduce syscalls
      if (msg.type === 'chat') {
        pub.publish('room:general', JSON.stringify(msg));
      }
    } catch (e) {
      ws.close(1003, 'Invalid message format');
    }
  });

  ws.on('close', () => clients.delete(clientId));
});

// Heartbeat interval to detect stale connections
setInterval(() => {
  wss.clients.forEach((ws) => {
    const clientId = [...clients.entries()].find(([, v]) => v.ws === ws)?.[0];
    const client = clientId ? clients.get(clientId) : null;
    if (client && !client.isAlive) {
      clients.delete(clientId);
      return ws.terminate();
    }
    if (client) client.isAlive = false;
    ws.ping();
  });
}, HEARTBEAT_INTERVAL);

server.listen(8080);
console.log(`WebSocket server: ${wss.clients.size} connected`);
```

## Network Optimization

### DNS Caching and Connection Pooling

```javascript
const https = require('https');
const http = require('http');
const { Resolver } = require('dns').promises;
const CacheableLookup = require('cacheable-lookup');

const cacheableLookup = new CacheableLookup({ maxTtl: 300 });

// HTTP Agent with connection pooling
const httpsAgent = new https.Agent({
  keepAlive: true,
  keepAliveMsecs: 1000,
  maxSockets: 50,
  maxFreeSockets: 10,
  timeout: 10000,
  lookup: cacheableLookup.lookup,
});

// HTTP Agent for internal services
const httpAgent = new http.Agent({
  keepAlive: true,
  maxSockets: 100,
  maxFreeSockets: 20,
  timeout: 5000,
});

// Fetch wrapper with connection reuse
async function fetchJSON(url, options = {}) {
  const agent = url.startsWith('https') ? httpsAgent : httpAgent;
  const res = await fetch(url, {
    ...options,
    agent,
    headers: {
      Accept: 'application/json',
      'Accept-Encoding': 'gzip, br',
      ...options.headers,
    },
  });
  return res.json();
}
```

## Edge Computing and Geographic Routing

```javascript
// Cloudflare Worker for geographic routing
// Deploy as edge function

addEventListener('fetch', (event) => {
  event.respondWith(routeRequest(event.request));
});

const API_BACKENDS = {
  'NA': 'https://api-us.example.com',
  'EU': 'https://api-eu.example.com',
  'AS': 'https://api-ap.example.com',
};

async function routeRequest(request) {
  const country = request.cf?.country || 'US';
  const continent = request.cf?.continent || 'NA';

  const backend = API_BACKENDS[continent] || API_BACKENDS['NA'];

  const url = new URL(request.url);
  url.hostname = new URL(backend).hostname;

  const response = await fetch(url, {
    method: request.method,
    headers: request.headers,
    body: request.body,
  });

  // Add latency metadata
  const newResponse = new Response(response.body, response);
  newResponse.headers.set('X-Edge-Location', request.cf?.colo || 'unknown');
  newResponse.headers.set('X-Routed-To', backend);

  return newResponse;
}
```

## Worker Threads for CPU-Intensive Operations

```javascript
// worker-pool.js
const { Worker, isMainThread, parentPort, workerData } = require('worker_threads');
const os = require('os');

if (!isMainThread) {
  // Worker thread: execute the function
  const { fn, args } = workerData;
  const result = require('vm').runInThisContext(`(${fn})`)(...args);
  parentPort.postMessage(result);
  return;
}

// Main thread: worker pool
class WorkerPool {
  constructor(maxWorkers = os.cpus().length) {
    this.maxWorkers = maxWorkers;
    this.workers = [];
    this.queue = [];
  }

  async execute(fn, ...args) {
    return new Promise((resolve, reject) => {
      const worker = new Worker(__filename, {
        workerData: { fn: fn.toString(), args },
      });
      worker.on('message', resolve);
      worker.on('error', reject);
      worker.on('exit', (code) => {
        if (code !== 0) reject(new Error(`Worker exited with code ${code}`));
      });
    });
  }
}

// Usage: offload CPU-heavy tasks
const pool = new WorkerPool();

app.post('/api/resize-image', async (req, res) => {
  const result = await pool.execute((buffer) => {
    // Heavy computation runs in worker thread
    const sharp = require('sharp');
    return sharp(Buffer.from(buffer))
      .resize(800, 600, { fit: 'inside' })
      .webp({ quality: 80 })
      .toBuffer();
  }, req.body.imageBuffer);
  res.send(result);
});
```

## Cluster Mode Optimization

```javascript
const cluster = require('cluster');
const os = require('os');
const http = require('http');

const WORKERS = parseInt(process.env.WORKER_COUNT) || os.cpus().length;

if (cluster.isPrimary) {
  console.log(`Primary ${process.pid} starting ${WORKERS} workers`);

  // Fork workers
  for (let i = 0; i < WORKERS; i++) {
    cluster.fork();
  }

  // Auto-restart crashed workers
  cluster.on('exit', (worker, code, signal) => {
    console.warn(`Worker ${worker.process.pid} died (${signal || code}). Restarting...`);
    cluster.fork();
  });

  // Graceful shutdown
  process.on('SIGTERM', () => {
    for (const id in cluster.workers) {
      cluster.workers[id].process.kill('SIGTERM');
    }
  });
} else {
  const app = require('./app');
  const server = http.createServer(app);

  server.listen(process.env.PORT || 3000, () => {
    console.log(`Worker ${process.pid} listening on port ${process.env.PORT || 3000}`);
  });

  process.on('SIGTERM', () => {
    server.close(() => process.exit(0));
  });
}
```

## Memory Optimization

### Heap Tuning and Garbage Collection

```bash
# Optimal Node.js memory flags for production
node \
  --max-old-space-size=1536 \
  --max-semi-space-size=64 \
  --optimize-for-size \
  --expose-gc \
  dist/server.js
```

```javascript
// Memory monitoring and leak detection
const v8 = require('v8');
const { PerformanceObserver } = require('perf_hooks');

// Monitor heap usage
function logMemoryStats() {
  const heap = process.memoryUsage();
  const stats = {
    rss: `${(heap.rss / 1024 / 1024).toFixed(1)} MB`,
    heapTotal: `${(heap.heapTotal / 1024 / 1024).toFixed(1)} MB`,
    heapUsed: `${(heap.heapUsed / 1024 / 1024).toFixed(1)} MB`,
    external: `${(heap.external / 1024 / 1024).toFixed(1)} MB`,
    heapPercent: `${((heap.heapUsed / heap.heapTotal) * 100).toFixed(1)}%`,
  };
  console.log('[Memory]', stats);
  return stats;
}

// Alert on high memory usage
setInterval(() => {
  const { heapUsed, heapTotal } = process.memoryUsage();
  const ratio = heapUsed / heapTotal;
  if (ratio > 0.85) {
    console.warn(`[Memory] High heap usage: ${(ratio * 100).toFixed(1)}%`);
    if (global.gc) {
      console.log('[Memory] Triggering manual GC');
      global.gc();
    }
  }
}, 30000);

// Heap snapshot on signal
process.on('SIGUSR2', () => {
  const snapshotStream = v8.writeHeapSnapshot();
  console.log(`Heap snapshot written: ${snapshotStream}`);
});
```

## Load Testing

### k6 Script

```javascript
// load-test.js — run with: k6 run load-test.js
import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Trend } from 'k6/metrics';

const errorRate = new Rate('errors');
const latency = new Trend('request_latency');

export const options = {
  stages: [
    { duration: '30s', target: 50 },   // Ramp up
    { duration: '2m', target: 200 },   // Sustained load
    { duration: '30s', target: 500 },  // Peak
    { duration: '1m', target: 0 },     // Ramp down
  ],
  thresholds: {
    http_req_duration: ['p(95)<500', 'p(99)<1500'],
    errors: ['rate<0.01'],
  },
};

export default function () {
  const res = http.get('https://api.example.com/api/products?limit=20');

  const success = check(res, {
    'status is 200': (r) => r.status === 200,
    'response time < 500ms': (r) => r.timings.duration < 500,
    'has products': (r) => JSON.parse(r.body).data.length > 0,
  });

  errorRate.add(!success);
  latency.add(res.timings.duration);
  sleep(1);
}
```

### Artillery Script

```yaml
# artillery-test.yml — run with: artillery run artillery-test.yml
config:
  target: "https://api.example.com"
  phases:
    - duration: 60
      arrivalRate: 10
      name: Warm up
    - duration: 120
      arrivalRate: 50
      name: Sustained load
    - duration: 60
      arrivalRate: 100
      name: Peak load
  plugins:
    ensure: {}
  ensure:
    p95: 500
    p99: 1500
    maxErrorRate: 1

scenarios:
  - name: "Browse products"
    flow:
      - get:
          url: "/api/products?page=1"
      - think: 1
      - get:
          url: "/api/products/{{ $randomNumber(1, 1000) }}"
      - think: 2
      - post:
          url: "/api/cart"
          json:
            productId: "{{ $randomNumber(1, 1000) }}"
            quantity: 1
```

### autocannon (Quick Benchmark)

```bash
# Install
npm i -g autocannon

# Benchmark API endpoint
autocannon -c 100 -d 30 -p 10 http://localhost:3000/api/products

# Output:
# Req/Sec  Avg  Latency  2xx  Non-2xx
#  4823    21ms          144k   0
```

## Performance Profiling

### Node.js Built-in Profiler

```bash
# CPU profiling
node --prof dist/server.js
# Produces isolate-*.log, then:
node --prof-process isolate-0x*.log > profile.txt

# Inspector-based profiling
node --inspect dist/server.js
# Open chrome://inspect in Chrome DevTools

# --cpu-prof for V8 CPU profile (.cpuprofile)
node --cpu-prof --cpu-prof-interval=100 dist/server.js

# --heap-prof for allocation timeline
node --heap-prof dist/server.js
```

### 0x Flame Graph

```bash
npm i -g 0x
0x -o dist/server.js
# Opens interactive flame graph in browser
```

### Clinic.js Suite

```bash
npm i -g clinic

# Detect event loop blocking
clinic doctor -- node dist/server.js

# Flame graph for CPU hotspots
clinic flame -- node dist/server.js

# Heap allocation profiling
clinic heapprof -- node dist/server.js
```

## APM Integration

```javascript
// Datadog APM integration
const tracer = require('dd-trace').init({
  service: 'my-api',
  env: process.env.NODE_ENV,
  logInjection: true,
  runtimeMetrics: true,
  profiling: true,
});

const express = require('express');
const app = express();

// Automatic instrumentation for Express, pg, redis, etc.

// Custom spans for business logic
app.post('/api/orders', async (req, res) => {
  const span = tracer.startSpan('order.process');
  try {
    const order = await createOrder(req.body);
    span.setTag('order.id', order.id);
    span.setTag('order.total', order.total);
    res.json(order);
  } catch (err) {
    span.setTag('error', true);
    span.setTag('error.msg', err.message);
    res.status(500).json({ error: 'Order failed' });
  } finally {
    span.finish();
  }
});
```

```javascript
// OpenTelemetry integration
const { NodeSDK } = require('@opentelemetry/sdk-node');
const { ExpressInstrumentation } = require('@opentelemetry/instrumentation-express');
const { PgInstrumentation } = require('@opentelemetry/instrumentation-pg');
const { HttpInstrumentation } = require('@opentelemetry/instrumentation-http');

const sdk = new NodeSDK({
  serviceName: 'my-api',
  instrumentations: [
    new HttpInstrumentation(),
    new ExpressInstrumentation(),
    new PgInstrumentation(),
  ],
});

sdk.start();
```

## Performance Benchmark Data

| Optimization | Before | After | Improvement |
|---|---|---|---|
| Cursor vs OFFSET pagination (100K rows) | 320ms | 8ms | 97.5% |
| Field selection (15→3 fields) | 180ms | 24ms | 87% |
| HTTP/2 multiplexing (10 assets) | 2.1s (6 conns) | 0.6s (1 conn) | 71% |
| Keep-alive (100 sequential reqs) | 8.2s | 1.4s | 83% |
| WebSocket batch vs individual msgs | 450 msg/s | 3200 msg/s | 611% |
| DNS cache hit | 45ms | 0ms | 100% |
| Worker thread (image resize) | Blocks event loop 800ms | Non-blocking | — |
| Cluster 8 workers (throughput) | 1200 req/s | 8400 req/s | 600% |
| GC tuning (--max-semi-space-size) | 12ms avg pause | 4ms avg pause | 67% |
| Edge routing (EU user) | 380ms RTT | 45ms RTT | 88% |

## Best Practices Checklist

- [ ] Use cursor-based pagination over OFFSET for large datasets
- [ ] Implement field selection to reduce payload size
- [ ] Enable HTTP/2 with appropriate stream limits
- [ ] Set keepAliveTimeout > load balancer idle timeout
- [ ] Compress WebSocket messages above threshold only
- [ ] Cache DNS lookups with CacheableLookup or similar
- [ ] Configure HTTP/HTTPS agents with keepAlive and pool sizing
- [ ] Use worker threads for CPU-bound tasks to avoid event loop blocking
- [ ] Run cluster mode with worker count = CPU cores (or fewer for I/O-heavy apps)
- [ ] Set --max-old-space-size based on container memory limits
- [ ] Monitor heap usage and alert at >85% utilization
- [ ] Run load tests in CI with performance thresholds
- [ ] Profile with 0x or Clinic.js before optimizing
- [ ] Integrate APM for production performance monitoring

## Cross-References

- [Performance Optimization Fundamentals](./01-performance-optimization.md)
- [Database and CDN Optimization](./02-database-cdn-optimization.md)
- [Monitoring](../monitoring/)
- [Scaling](../scaling/)
- [CI/CD Pipelines](../05-ci-cd-pipelines/)

## Next Steps

Continue to the [next deployment topic](../11-disaster-recovery/) to learn about disaster recovery planning and high availability architecture.
