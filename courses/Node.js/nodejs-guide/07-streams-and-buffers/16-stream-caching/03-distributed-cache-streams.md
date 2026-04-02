# Distributed Cache Synchronization with Streams

## What You'll Learn

- Synchronizing cache state across multiple nodes using Node.js streams
- Eventual vs. strong consistency patterns implemented with stream-based replication
- Building a stream-based cache replication pipeline
- Sharding cache data with stream partitioning strategies
- Monitoring cache performance via stream metrics collectors
- Cache warming strategies for scaled, multi-node deployments
- Real-world: distributed cache cluster with stream-based synchronization

---

## 1. Multi-Node Cache Synchronization via Streams

Each cache node exposes a Readable stream of changes; peers consume and apply them.

```js
// cache-node.mjs
import { Readable, Writable, Transform, PassThrough } from 'node:stream';
import { createServer, request } from 'node:http';
import { createClient } from 'redis';

class CacheNode {
  #name;
  #port;
  #cache = new Map();
  #changeLog = [];
  #peers = [];
  #redis;

  constructor(name, port) {
    this.#name = name;
    this.#port = port;
    this.#redis = createClient({ url: 'redis://localhost:6379' });
  }

  async start() {
    await this.#redis.connect();

    // HTTP endpoint that streams change log as NDJSON
    const server = createServer((req, res) => {
      if (req.url === '/sync') {
        res.writeHead(200, { 'Content-Type': 'application/x-ndjson' });
        const stream = Readable.from(this.#changeLog.map(JSON.stringify));
        stream.pipe(res);
        return;
      }
      res.writeHead(404);
      res.end();
    });

    server.listen(this.#port, () => {
      console.log(`[${this.#name}] Listening on :${this.#port}`);
    });
  }

  set(key, value) {
    this.#cache.set(key, value);
    const change = { op: 'set', key, value, node: this.#name, ts: Date.now() };
    this.#changeLog.push(change);
    this.#broadcast(change);
  }

  get(key) {
    return this.#cache.get(key);
  }

  addPeer(host, port) {
    this.#peers.push({ host, port });
  }

  async syncFromPeers() {
    for (const peer of this.#peers) {
      await this.#pullFromPeer(peer);
    }
  }

  async #pullFromPeer(peer) {
    return new Promise((resolve, reject) => {
      const req = request(
        { hostname: peer.host, port: peer.port, path: '/sync' },
        (res) => {
          let buffer = '';
          res.on('data', (chunk) => {
            buffer += chunk.toString();
            const lines = buffer.split('\n');
            buffer = lines.pop();
            for (const line of lines) {
              if (!line.trim()) continue;
              const change = JSON.parse(line);
              if (change.op === 'set') {
                this.#cache.set(change.key, change.value);
                console.log(
                  `[${this.#name}] Synced ${change.key} from ${change.node}`
                );
              }
            }
          });
          res.on('end', resolve);
        }
      );
      req.on('error', reject);
      req.end();
    });
  }

  #broadcast(change) {
    // Publish to Redis pub/sub for real-time sync
    this.#redis.publish('cache:sync', JSON.stringify(change)).catch(() => {});
  }

  get size() {
    return this.#cache.size;
  }

  async shutdown() {
    await this.#redis.quit();
  }
}

// Usage
const nodeA = new CacheNode('Node-A', 4001);
const nodeB = new CacheNode('Node-B', 4002);

await nodeA.start();
await nodeB.start();

nodeB.addPeer('localhost', 4001);
nodeA.set('user:1', { name: 'Alice' });
nodeA.set('user:2', { name: 'Bob' });

// Allow server to start, then sync
setTimeout(async () => {
  await nodeB.syncFromPeers();
  console.log(`Node-B cache size: ${nodeB.size}`);
  console.log(`Node-B user:1 ->`, nodeB.get('user:1'));
  await nodeA.shutdown();
  await nodeB.shutdown();
}, 500);
```

---

## 2. Eventual Consistency with Stream Replication

Changes are replicated asynchronously; nodes converge over time.

```js
// eventual-consistency.mjs
import { Transform, PassThrough } from 'node:stream';

class ReplicationStream extends Transform {
  #nodeName;
  #lagMs;

  constructor(nodeName, lagMs = 100) {
    super({ objectMode: true });
    this.#nodeName = nodeName;
    this.#lagMs = lagMs;
  }

  async _transform(change, encoding, callback) {
    // Simulate network latency
    await new Promise((r) => setTimeout(r, this.#lagMs));
    console.log(
      `[${this.#nodeName}] Applied: ${change.op} ${change.key} (v${change.version})`
    );
    this.push(change);
    callback();
  }
}

class EventuallyConsistentCache {
  #store = new Map();
  #version = 0;
  #replicationBus = new PassThrough({ objectMode: true });
  #replicas = [];

  constructor() {
    // Fan-out replication stream to all replica transforms
    this.#replicationBus.on('data', (change) => {
      for (const replica of this.#replicas) {
        replica.write(change);
      }
    });
  }

  set(key, value) {
    this.#version++;
    this.#store.set(key, { value, version: this.#version });
    this.#replicationBus.write({
      op: 'set',
      key,
      value,
      version: this.#version,
      ts: Date.now(),
    });
  }

  get(key) {
    return this.#store.get(key);
  }

  addReplica(name, lagMs = 50) {
    const replica = new ReplicationStream(name, lagMs);
    replica.on('data', (change) => {
      if (change.op === 'set') {
        this.#store.set(change.key, {
          value: change.value,
          version: change.version,
        });
      }
    });
    this.#replicas.push(replica);
    return replica;
  }
}

// Usage
const cache = new EventuallyConsistentCache();
cache.addReplica('Replica-East', 30);
cache.addReplica('Replica-West', 80);

cache.set('config:theme', 'dark');
cache.set('config:lang', 'en');
cache.set('feature:flag', true);

// Replicas apply changes asynchronously with simulated lag
setTimeout(() => {
  console.log('Primary store:', cache.get('config:theme'));
}, 500);
```

---

## 3. Strong Consistency with Stream-Based Two-Phase Commit

Writes block until all replicas acknowledge via a writable stream.

```js
// strong-consistency.mjs
import { Writable, pipeline } from 'node:stream';
import { promisify } from 'node:util';

const pipelineAsync = promisify(pipeline);

class ConsensusWriter extends Writable {
  #replicas;
  #quorum;

  constructor(replicas, quorum) {
    super({ objectMode: true });
    this.#replicas = replicas;
    this.#quorum = quorum;
  }

  async _write(chunk, encoding, callback) {
    try {
      // Phase 1: Prepare (parallel writes to all replicas)
      const results = await Promise.allSettled(
        this.#replicas.map((r) => r.prepare(chunk))
      );

      const prepared = results.filter((r) => r.status === 'fulfilled').length;
      if (prepared < this.#quorum) {
        return callback(
          new Error(`Quorum not met: ${prepared}/${this.#quorum}`)
        );
      }

      // Phase 2: Commit
      await Promise.allSettled(this.#replicas.map((r) => r.commit(chunk)));

      console.log(
        `Committed ${chunk.key} to ${prepared}/${this.#replicas.length} replicas`
      );
      callback();
    } catch (err) {
      callback(err);
    }
  }
}

// Simulated replica
class ReplicaNode {
  #name;
  #store = new Map();
  #prepared = new Map();

  constructor(name) {
    this.#name = name;
  }

  async prepare(change) {
    // Simulate random failure (10% chance)
    if (Math.random() < 0.1) {
      throw new Error(`${this.#name} prepare failed`);
    }
    this.#prepared.set(change.key, change.value);
    return true;
  }

  async commit(change) {
    this.#store.set(change.key, change.value);
    this.#prepared.delete(change.key);
    return true;
  }

  get size() {
    return this.#store.size;
  }
}

// Usage
import { Readable } from 'node:stream';

const replicas = [
  new ReplicaNode('R1'),
  new ReplicaNode('R2'),
  new ReplicaNode('R3'),
  new ReplicaNode('R4'),
  new ReplicaNode('R5'),
];

const writer = new ConsensusWriter(replicas, 3); // quorum = 3 of 5

const changes = Readable.from([
  { key: 'account:1', value: { balance: 1000 } },
  { key: 'account:2', value: { balance: 500 } },
  { key: 'account:3', value: { balance: 250 } },
]);

try {
  await pipelineAsync(changes, writer);
  console.log('Replica sizes:', replicas.map((r) => `${r.#name}:${r.size}`).join(', '));
} catch (err) {
  console.error('Consensus failed:', err.message);
}
```

---

## 4. Cache Sharding with Stream Partitioning

Distribute cache keys across shards using a Transform stream that routes by hash.

```js
// cache-sharding.mjs
import { Transform, Writable } from 'node:stream';
import { createHash } from 'node:crypto';

class ShardRouter extends Transform {
  #shards;

  constructor(shards) {
    super({ objectMode: true });
    this.#shards = shards;
  }

  _transform(chunk, encoding, callback) {
    const hash = createHash('md5').update(chunk.key).digest();
    const shardIndex = hash[0] % this.#shards.length;
    const shard = this.#shards[shardIndex];

    this.push({ shardIndex, shard: shard.name, ...chunk });
    shard.write(chunk);
    callback();
  }
}

class ShardNode extends Writable {
  #name;
  #store = new Map();
  #count = 0;

  constructor(name) {
    super({ objectMode: true });
    this.#name = name;
  }

  _write(chunk, encoding, callback) {
    this.#store.set(chunk.key, chunk.value);
    this.#count++;
    callback();
  }

  get name() {
    return this.#name;
  }

  get count() {
    return this.#count;
  }

  get size() {
    return this.#store.size;
  }
}

// Usage
import { Readable } from 'node:stream';

const shards = [
  new ShardNode('shard-0'),
  new ShardNode('shard-1'),
  new ShardNode('shard-2'),
  new ShardNode('shard-3'),
];

const router = new ShardRouter(shards);

// Monitor routing
router.on('data', (routed) => {
  // silent; uncomment for verbose
  // console.log(`${routed.key} -> ${routed.shard}`);
});

const keys = Array.from({ length: 1000 }, (_, i) => ({
  key: `item:${i}`,
  value: { index: i, data: `payload-${i}` },
}));

const source = Readable.from(keys);

source.pipe(router).on('finish', () => {
  console.log('Shard distribution:');
  for (const shard of shards) {
    console.log(`  ${shard.name}: ${shard.count} keys`);
  }
});
```

---

## 5. Cache Performance Monitoring via Stream Metrics

Collect cache hit/miss rates, latency, and throughput using a metrics stream.

```js
// cache-metrics.mjs
import { Transform, Readable } from 'node:stream';

class CacheMetricsCollector extends Transform {
  #hits = 0;
  #misses = 0;
  #totalLatency = 0;
  #count = 0;
  #windowStart = Date.now();

  constructor() {
    super({ objectMode: true });
  }

  _transform(event, encoding, callback) {
    this.#count++;

    if (event.type === 'hit') {
      this.#hits++;
    } else if (event.type === 'miss') {
      this.#misses++;
    }

    if (event.latencyMs) {
      this.#totalLatency += event.latencyMs;
    }

    // Emit periodic snapshots
    if (this.#count % 100 === 0) {
      this.push(this.#snapshot());
    }

    callback();
  }

  _flush(callback) {
    this.push(this.#snapshot());
    callback();
  }

  #snapshot() {
    const elapsed = (Date.now() - this.#windowStart) / 1000;
    return {
      timestamp: Date.now(),
      hits: this.#hits,
      misses: this.#misses,
      total: this.#count,
      hitRate: this.#count > 0 ? ((this.#hits / this.#count) * 100).toFixed(2) + '%' : '0%',
      avgLatencyMs: this.#count > 0 ? (this.#totalLatency / this.#count).toFixed(2) : '0',
      throughput: (this.#count / elapsed).toFixed(0) + ' ops/s',
    };
  }
}

// Instrumented cache wrapper
class MonitoredCache {
  #cache = new Map();
  #metricsStream;

  constructor(metricsStream) {
    this.#metricsStream = metricsStream;
  }

  get(key) {
    const start = Date.now();
    const value = this.#cache.get(key);
    const latencyMs = Date.now() - start;

    this.#metricsStream.write({
      type: value !== undefined ? 'hit' : 'miss',
      key,
      latencyMs,
    });

    return value;
  }

  set(key, value) {
    this.#cache.set(key, value);
    this.#metricsStream.write({ type: 'set', key, latencyMs: 0 });
  }
}

// Usage
const metrics = new CacheMetricsCollector();
const cache = new MonitoredCache(metrics);

// Feed synthetic workload
for (let i = 0; i < 500; i++) {
  cache.set(`key:${i % 50}`, `value-${i}`);
}

for (let i = 0; i < 500; i++) {
  cache.get(`key:${Math.floor(Math.random() * 50)}`);
}

metrics.resume(); // consume stream
metrics.end();

metrics.on('data', (snapshot) => {
  console.log('Metrics:', snapshot);
});

metrics.on('end', () => {
  console.log('Monitoring complete');
});
```

---

## 6. Cache Warming for Scaled Deployments

Distribute warm-up work across nodes using a partitioned stream.

```js
// distributed-warming.mjs
import { Readable, Transform, Writable } from 'node:stream';
import { createClient } from 'redis';

class WorkPartitioner extends Transform {
  #nodeCount;
  #nodeIndex;

  constructor(nodeIndex, nodeCount) {
    super({ objectMode: true });
    this.#nodeIndex = nodeIndex;
    this.#nodeCount = nodeCount;
  }

  _transform(key, encoding, callback) {
    // Consistent hashing: only process keys assigned to this node
    const hash = [...key].reduce((acc, c) => acc + c.charCodeAt(0), 0);
    if (hash % this.#nodeCount === this.#nodeIndex) {
      this.push(key);
    }
    callback();
  }
}

class CacheWarmer extends Writable {
  #redis;
  #count = 0;
  #dbFetch;

  constructor(redis, dbFetch) {
    super({ objectMode: true });
    this.#redis = redis;
    this.#dbFetch = dbFetch;
  }

  async _write(key, encoding, callback) {
    try {
      const value = await this.#dbFetch(key);
      await this.#redis.set(`warm:${key}`, JSON.stringify(value), { EX: 3600 });
      this.#count++;
      if (this.#count % 100 === 0) {
        console.log(`[Warmer] ${this.#count} keys warmed`);
      }
      callback();
    } catch (err) {
      callback(err);
    }
  }

  get count() {
    return this.#count;
  }
}

// Simulated database fetch
async function dbFetch(key) {
  return new Promise((resolve) =>
    setTimeout(() => resolve({ key, data: `db-value-${key}`, ts: Date.now() }), 5)
  );
}

// Usage
import { pipeline } from 'node:stream';
import { promisify } from 'node:util';

const pipelineAsync = promisify(pipeline);

const redis = createClient({ url: 'redis://localhost:6379' });
await redis.connect();

const TOTAL_KEYS = 5000;
const NODE_INDEX = 0; // change per node
const NODE_COUNT = 4;

const allKeys = Array.from({ length: TOTAL_KEYS }, (_, i) => `item:${i}`);
const source = Readable.from(allKeys);

const start = Date.now();

await pipelineAsync(
  source,
  new WorkPartitioner(NODE_INDEX, NODE_COUNT),
  new CacheWarmer(redis, dbFetch)
);

console.log(
  `Node ${NODE_INDEX} warmed in ${Date.now() - start}ms`
);

await redis.quit();
```

---

## 7. Real-World: Distributed Cache Cluster with Stream-Based Synchronization

A full example combining sharding, replication, metrics, and HTTP serving.

```js
// distributed-cluster.mjs
import { createServer } from 'node:http';
import { PassThrough, Transform } from 'node:stream';
import { createClient } from 'redis';

class ClusterCoordinator {
  #shards = [];
  #replicationBus = new PassThrough({ objectMode: true });
  #metrics = { hits: 0, misses: 0, sets: 0 };

  constructor(shardCount = 4) {
    for (let i = 0; i < shardCount; i++) {
      this.#shards.push({ id: i, store: new Map(), name: `shard-${i}` });
    }

    // Log replication events
    this.#replicationBus.on('data', (event) => {
      // Fan-out to all shards for cross-shard awareness
      if (event.type === 'invalidate') {
        for (const shard of this.#shards) {
          shard.store.delete(event.key);
        }
      }
    });
  }

  #getShard(key) {
    let hash = 0;
    for (let i = 0; i < key.length; i++) {
      hash = (hash * 31 + key.charCodeAt(i)) >>> 0;
    }
    return this.#shards[hash % this.#shards.length];
  }

  get(key) {
    const shard = this.#getShard(key);
    const value = shard.store.get(key);
    if (value !== undefined) {
      this.#metrics.hits++;
      return { hit: true, value, shard: shard.name };
    }
    this.#metrics.misses++;
    return { hit: false, shard: shard.name };
  }

  set(key, value, ttlMs = 60000) {
    const shard = this.#getShard(key);
    const expiresAt = Date.now() + ttlMs;
    shard.store.set(key, { value, expiresAt });
    this.#metrics.sets++;

    // Publish replication event
    this.#replicationBus.write({
      type: 'set',
      key,
      shard: shard.name,
      ts: Date.now(),
    });

    return { shard: shard.name };
  }

  invalidate(key) {
    this.#replicationBus.write({ type: 'invalidate', key, ts: Date.now() });
  }

  getMetrics() {
    const total = this.#metrics.hits + this.#metrics.misses;
    return {
      ...this.#metrics,
      hitRate: total > 0 ? ((this.#metrics.hits / total) * 100).toFixed(1) + '%' : 'N/A',
      shardDistribution: this.#shards.map((s) => ({
        name: s.name,
        keys: s.store.size,
      })),
    };
  }
}

// HTTP API
const cluster = new ClusterCoordinator(4);

const server = createServer(async (req, res) => {
  const url = new URL(req.url, `http://${req.headers.host}`);
  res.setHeader('Content-Type', 'application/json');

  if (req.method === 'GET' && url.pathname === '/cache') {
    const key = url.searchParams.get('key');
    if (!key) {
      res.writeHead(400);
      return res.end(JSON.stringify({ error: 'key required' }));
    }

    const result = cluster.get(key);
    res.writeHead(result.hit ? 200 : 404);
    return res.end(JSON.stringify(result));
  }

  if (req.method === 'POST' && url.pathname === '/cache') {
    const body = await readBody(req);
    const { key, value, ttl } = JSON.parse(body);
    const result = cluster.set(key, value, ttl);
    res.writeHead(201);
    return res.end(JSON.stringify(result));
  }

  if (req.method === 'DELETE' && url.pathname === '/cache') {
    const key = url.searchParams.get('key');
    cluster.invalidate(key);
    res.writeHead(200);
    return res.end(JSON.stringify({ invalidated: key }));
  }

  if (req.method === 'GET' && url.pathname === '/metrics') {
    res.writeHead(200);
    return res.end(JSON.stringify(cluster.getMetrics(), null, 2));
  }

  res.writeHead(404);
  res.end(JSON.stringify({ error: 'not found' }));
});

function readBody(req) {
  return new Promise((resolve, reject) => {
    const chunks = [];
    req.on('data', (c) => chunks.push(c));
    req.on('end', () => resolve(Buffer.concat(chunks).toString()));
    req.on('error', reject);
  });
}

server.listen(3003, () => {
  console.log('Distributed cache cluster API on :3003');
  console.log('Endpoints:');
  console.log('  GET    /cache?key=...');
  console.log('  POST   /cache  { key, value, ttl }');
  console.log('  DELETE /cache?key=...');
  console.log('  GET    /metrics');
});
```

---

## Best Practices Checklist

- [ ] Choose consistency model (eventual vs. strong) based on application requirements
- [ ] Use quorum-based writes (e.g., 3 of 5 replicas) for strong consistency with fault tolerance
- [ ] Implement consistent hashing for shard routing to minimize key redistribution when nodes change
- [ ] Use Redis pub/sub or Kafka as a replication bus for real-time change propagation
- [ ] Monitor cache hit rate, replication lag, and shard distribution via stream metrics
- [ ] Partition warming work across nodes to parallelize cache population at startup
- [ ] Set TTLs on replicated entries to prevent stale data from persisting after network partitions
- [ ] Implement retry and backoff for failed replication writes
- [ ] Use stream backpressure to prevent overwhelming slow replica nodes
- [ ] Version cache entries (vector clocks or Lamport timestamps) to detect and resolve conflicts
- [ ] Log replication events to a durable stream (file or Kafka) for replay after node recovery

---

## Cross-References

- [01-redis-stream-caching.md](./01-redis-stream-caching.md) — Redis stream caching fundamentals
- [02-memory-cdn-caching.md](./02-memory-cdn-caching.md) — In-memory LRU and CDN caching
- [06-stream-concurrency-parallelism](../06-stream-concurrency-parallelism/) — parallel stream processing
- [15-stream-message-queues](../15-stream-message-queues/) — Kafka/RabbitMQ as replication buses
- [08-stream-performance-optimization](../08-stream-performance-optimization/) — backpressure and throughput tuning

---

## Next Steps

- Implement Raft or Paxos consensus over streams for strongly-consistent distributed caches
- Explore Redis Cluster with automatic sharding and failover
- Investigate Hazelcast or Apache Ignite for JVM-based distributed cache with stream APIs
- Build a cache proxy layer that transparently shards and replicates via Transform streams
- Benchmark eventual vs. strong consistency trade-offs under realistic workloads
