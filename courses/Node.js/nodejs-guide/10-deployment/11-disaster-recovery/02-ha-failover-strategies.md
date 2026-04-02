# High Availability & Failover Strategies for Node.js

> **Previous**: [01-backup-recovery.md](./01-backup-recovery.md) | **Next**: [03-dr-testing-continuity.md](./03-dr-testing-continuity.md)

## What You'll Learn

- HA architecture patterns (active-active, active-passive, multi-region)
- Load balancer health checks and failover
- Database and Redis high availability
- DNS failover and auto-scaling
- Circuit breaker, retry, and bulkhead patterns
- Chaos engineering fundamentals

---

## HA Architecture Patterns

### Active-Active vs Active-Passive

```
ACTIVE-ACTIVE                         ACTIVE-PASSIVE
┌─────────┐ ┌─────────┐              ┌─────────┐ ┌─────────┐
│ Region A│ │ Region B│              │ Primary │ │ Standby │
│ (read/  │ │ (read/  │              │ (active)│ │(passive)│
│  write) │ │  write) │              └────┬────┘ └────┬────┘
└────┬────┘ └────┬────┘                   │           │
     │  sync     │                    async repl      │
     └─────┼─────┘                                   │
     ┌─────▼─────┐                        failover───┘
     │  Shared   │
     │   State   │
     └───────────┘
```

**Active-Active**: Both regions serve traffic with bidirectional replication. Lower latency, higher complexity.

**Active-Passive**: Standby activates on failure. Lower cost, higher failover latency (1-5 min).

### Multi-Region Router

```javascript
// multi-region-router.js
class MultiRegionRouter {
    constructor(regions) {
        this.regions = regions;
        this.healthyRegions = new Set(regions.map(r => r.id));
    }

    getRegionForRequest(request) {
        const eligible = this.regions.filter(r => this.healthyRegions.has(r.id));
        if (!eligible.length) throw new Error('No healthy regions');

        // Geographic routing first
        const geoMatch = eligible.find(r => r.id.startsWith(request.headers['x-client-region']));
        if (geoMatch) return geoMatch;

        // Weighted fallback
        const total = eligible.reduce((sum, r) => sum + r.weight, 0);
        let random = Math.random() * total;
        for (const region of eligible) {
            random -= region.weight;
            if (random <= 0) return region;
        }
    }

    markUnhealthy(regionId) { this.healthyRegions.delete(regionId); }
    markHealthy(regionId) { this.healthyRegions.add(regionId); }
}

export default MultiRegionRouter;
```

---

## Load Balancer Health Checks

### Nginx Configuration

```nginx
# nginx-lb.conf
upstream nodejs_backend {
    least_conn;
    server app1.internal:3000 max_fails=3 fail_timeout=30s;
    server app2.internal:3000 max_fails=3 fail_timeout=30s;
    server app3.internal:3000 backup;
}

server {
    listen 443 ssl http2;
    location / {
        proxy_pass http://nodejs_backend;
        proxy_next_upstream error timeout http_502 http_503;
        proxy_next_upstream_tries 3;
    }
    location /health { proxy_pass http://nodejs_backend/healthz; access_log off; }
}
```

### Health Check Endpoint

```javascript
// health-check-server.js
import { createServer } from 'node:http';

class HealthCheckServer {
    constructor(deps) { this.db = deps.db; this.redis = deps.redis; this.startupTime = Date.now(); }

    start(port = 3001) {
        const server = createServer(async (req, res) => {
            if (req.url === '/healthz') { res.writeHead(200); return res.end('ok'); }

            if (req.url === '/ready') {
                if (Date.now() - this.startupTime < 10000) { res.writeHead(503); return res.end('starting'); }
                try {
                    await this.db.query('SELECT 1');
                    await this.redis.ping();
                    res.writeHead(200); res.end('ready');
                } catch (err) {
                    res.writeHead(503); res.end(JSON.stringify({ error: err.message }));
                }
                return;
            }
            res.writeHead(404); res.end();
        });
        server.listen(port);
        return server;
    }
}
```

### ALB Health Check (Terraform)

```hcl
resource "aws_lb_target_group" "app" {
    name     = "nodejs-app-tg"
    port     = 3000
    protocol = "HTTP"

    health_check {
        enabled             = true
        path                = "/health"
        healthy_threshold   = 2
        unhealthy_threshold = 3
        timeout             = 5
        interval            = 10
        matcher             = "200"
    }
}
```

---

## Database High Availability

### PostgreSQL Streaming Replication

```yaml
# docker-compose-pg-ha.yml
version: '3.8'
services:
    primary:
        image: postgres:16
        command: postgres -c wal_level=replica -c max_wal_senders=5 -c synchronous_commit=on
    standby:
        image: postgres:16
        depends_on: [primary]
        entrypoint: |
            bash -c "
            [ ! -s /var/lib/postgresql/data/PG_VERSION ] &&
                pg_basebackup -h primary -D /var/lib/postgresql/data -U replicator -Fp -Xs -P -R
            postgres
            "
```

### Node.js Failover Pool

```javascript
// db-failover-pool.js
import pg from 'pg';
const { Pool } = pg;

class FailoverPool {
    constructor(config) {
        this.primary = new Pool({ ...config.primary, max: 20 });
        this.replica = new Pool({ ...(config.replica || config.primary), max: 20 });
        this.isPrimaryHealthy = true;
    }

    async startHealthChecks() {
        setInterval(async () => {
            try { await this.primary.query('SELECT 1'); this.isPrimaryHealthy = true; }
            catch {
                if (this.isPrimaryHealthy) { console.error('[DB] Primary down, failing over'); this.isPrimaryHealthy = false; }
            }
        }, 5000);
    }

    async query(text, params, { preferReplica = false } = {}) {
        const pool = preferReplica && this.isPrimaryHealthy ? this.replica : this.primary;
        try { return await pool.query(text, params); }
        catch (err) { if (preferReplica) return this.primary.query(text, params); throw err; }
    }

    async transaction(callback) {
        const client = await this.primary.connect();
        try { await client.query('BEGIN'); const r = await callback(client); await client.query('COMMIT'); return r; }
        catch (err) { await client.query('ROLLBACK'); throw err; }
        finally { client.release(); }
    }
}
```

### Patroni Configuration (Key Settings)

```yaml
# patroni.yml
scope: nodejs-cluster
etcd3: { hosts: etcd-0:2379,etcd-1:2379,etcd-2:2379 }
bootstrap:
    dcs:
        synchronous_mode: true
        postgresql:
            use_pg_rewind: true
            parameters: { wal_level: replica, max_wal_senders: 10, hot_standby: "on" }
```

---

## Redis High Availability

### Redis Sentinel Client

```javascript
// redis-sentinel-client.js
import { createClient } from 'redis';

class ResilientRedisClient {
    constructor(sentinels, serviceName = 'mymaster') {
        this.sentinels = sentinels;
        this.serviceName = serviceName;
    }

    async connect() {
        this.client = createClient({
            sentinels: this.sentinels,
            name: this.serviceName,
            socket: {
                reconnectStrategy: (retries) => retries > 20 ? new Error('Exhausted') : Math.min(retries * 200, 5000),
                connectTimeout: 5000,
            },
        });
        this.client.on('error', (err) => console.error('[Redis]', err.message));
        await this.client.connect();
        return this;
    }

    async get(key) { try { return await this.client.get(key); } catch { return null; } }
    async set(key, value, opts) { try { return await this.client.set(key, value, opts); } catch { return false; } }
    async shutdown() { await this.client?.quit(); }
}
```

---

## DNS Failover

### Route53 Failover (Terraform)

```hcl
resource "aws_route53_health_check" "primary" {
    fqdn              = "api-primary.example.com"
    port              = 443
    type              = "HTTPS"
    resource_path     = "/health"
    failure_threshold = 3
}

resource "aws_route53_record" "primary" {
    zone_id = var.zone_id
    name    = "api.example.com"
    type    = "A"
    health_check_id    = aws_route53_health_check.primary.id
    failover_routing_policy { type = "PRIMARY" }
    set_identifier = "primary"
    alias {
        name                   = aws_lb.primary.dns_name
        zone_id                = aws_lb.primary.zone_id
        evaluate_target_health = true
    }
}

resource "aws_route53_record" "standby" {
    zone_id = var.zone_id
    name    = "api.example.com"
    type    = "A"
    failover_routing_policy { type = "SECONDARY" }
    set_identifier = "standby"
    alias {
        name                   = aws_lb.standby.dns_name
        zone_id                = aws_lb.standby.zone_id
        evaluate_target_health = true
    }
}
```

---

## Auto-Scaling for Resilience

### Kubernetes HPA

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata: { name: nodejs-app-hpa }
spec:
    scaleTargetRef: { apiVersion: apps/v1, kind: Deployment, name: nodejs-app }
    minReplicas: 3
    maxReplicas: 20
    behavior:
        scaleUp:  { stabilizationWindowSeconds: 30, policies: [{ type: Percent, value: 100, periodSeconds: 60 }] }
        scaleDown: { stabilizationWindowSeconds: 300, policies: [{ type: Percent, value: 10, periodSeconds: 60 }] }
    metrics:
        - type: Resource
          resource: { name: cpu, target: { type: Utilization, averageUtilization: 70 } }
```

---

## Circuit Breaker Pattern

### Implementation with `opossum`

```javascript
// circuit-breaker.js
import CircuitBreaker from 'opossum';

class ResilientService {
    constructor() { this.breakers = new Map(); }

    createBreaker(name, fn, options = {}) {
        const breaker = new CircuitBreaker(fn, {
            timeout: 10000, errorThresholdPercentage: 50, resetTimeout: 30000, volumeThreshold: 10, ...options,
        });
        breaker.on('open', () => console.warn(`[CB] ${name} OPENED`));
        breaker.on('halfOpen', () => console.log(`[CB] ${name} HALF-OPEN`));
        breaker.on('close', () => console.log(`[CB] ${name} CLOSED`));
        breaker.fallback(() => options.fallbackValue ?? null);
        this.breakers.set(name, breaker);
        return breaker;
    }

    async call(name, ...args) {
        const breaker = this.breakers.get(name);
        if (!breaker) throw new Error(`No breaker: ${name}`);
        try { return await breaker.fire(...args); }
        catch (err) { if (breaker.opened) err.circuitOpen = true; throw err; }
    }
}

// Express middleware
function withCircuitBreaker(breakerName, service) {
    return async (req, res, next) => {
        try {
            req.circuitResult = await service.call(breakerName, req.body);
            next();
        } catch (err) {
            if (err.circuitOpen) return res.status(503).json({ error: 'Unavailable', retryAfter: 30 });
            next(err);
        }
    };
}

// Usage
const service = new ResilientService();
service.createBreaker('payment-api', async (payment) => {
    const res = await fetch('https://payment-api.example.com/charge', {
        method: 'POST', body: JSON.stringify(payment), headers: { 'Content-Type': 'application/json' },
    });
    if (!res.ok) throw new Error(`Payment failed: ${res.status}`);
    return res.json();
}, { fallbackValue: { status: 'pending', queued: true } });
```

---

## Retry Patterns with Exponential Backoff

```javascript
// retry-with-backoff.js
class RetryError extends Error {
    constructor(message, lastError, attempts) {
        super(message);
        this.name = 'RetryError';
        this.lastError = lastError;
        this.attempts = attempts;
    }
}

async function retryWithBackoff(fn, options = {}) {
    const { maxRetries = 3, baseDelay = 1000, maxDelay = 30000, factor = 2,
            jitter = true, retryIf = () => true, onRetry = () => {} } = options;

    let lastError;
    for (let attempt = 0; attempt <= maxRetries; attempt++) {
        try { return await fn(attempt); }
        catch (err) {
            lastError = err;
            if (attempt === maxRetries || !retryIf(err))
                throw new RetryError(`Failed after ${attempt + 1} attempts`, err, attempt + 1);

            const delay = Math.min(baseDelay * factor ** attempt, maxDelay);
            const jitterMs = jitter ? Math.random() * delay * 0.5 : 0;
            onRetry({ attempt: attempt + 1, error: err.message, nextRetryIn: delay + jitterMs });
            await new Promise(r => setTimeout(r, delay + jitterMs));
        }
    }
    throw lastError;
}

function isRetryableError(err) {
    if (['ECONNRESET', 'ETIMEDOUT', 'ECONNREFUSED'].includes(err.code)) return true;
    if (err.status >= 500 || err.status === 429) return true;
    return false;
}

// Usage
async function callExternalAPI(url, body) {
    return retryWithBackoff(async (attempt) => {
        const res = await fetch(url, {
            method: 'POST',
            body: JSON.stringify(body),
            headers: { 'Content-Type': 'application/json', 'Idempotency-Key': `req-${Date.now()}-${attempt}` },
            signal: AbortSignal.timeout(10000),
        });
        if (!res.ok) { const err = new Error(`HTTP ${res.status}`); err.status = res.status; throw err; }
        return res.json();
    }, { maxRetries: 4, baseDelay: 500, retryIf: isRetryableError });
}

export { retryWithBackoff, RetryError, isRetryableError };
```

---

## Bulkhead Pattern

```javascript
// bulkhead.js
class Bulkhead {
    constructor(name, maxConcurrent, maxQueue = 0) {
        this.name = name;
        this.maxConcurrent = maxConcurrent;
        this.maxQueue = maxQueue;
        this.active = 0;
        this.queue = [];
    }

    async execute(fn) {
        if (this.active >= this.maxConcurrent) {
            if (this.queue.length >= this.maxQueue)
                throw new Error(`Bulkhead "${this.name}" full (${this.active}/${this.maxConcurrent})`);
            return new Promise((resolve, reject) => this.queue.push({ fn, resolve, reject }));
        }
        return this.run(fn);
    }

    async run(fn) {
        this.active++;
        try { return await fn(); }
        finally {
            this.active--;
            if (this.queue.length > 0 && this.active < this.maxConcurrent) {
                const { fn, resolve, reject } = this.queue.shift();
                this.run(fn).then(resolve).catch(reject);
            }
        }
    }
}

// Resource-isolated bulkheads
const bulkheads = {
    payment: new Bulkhead('payment', 10, 20),
    email: new Bulkhead('email', 5, 50),
    analytics: new Bulkhead('analytics', 3, 100),
};

// Surge in analytics won't starve payment processing
async function processOrder(order) {
    const payment = await bulkheads.payment.execute(() => chargePayment(order));
    bulkheads.email.execute(() => sendConfirmation(order)).catch(() => {});
    return payment;
}
```

---

## Chaos Engineering

### Chaos Monkey Middleware

```javascript
// chaos-monkey.js
import { setTimeout as sleep } from 'node:timers/promises';

class ChaosMonkey {
    constructor(config = {}) {
        this.enabled = config.enabled ?? process.env.CHAOS_ENABLED === 'true';
        this.aggression = config.aggression ?? 0.05;
        this.excludedPaths = config.excludedPaths ?? ['/health', '/ready'];
    }

    middleware() {
        return async (req, res, next) => {
            if (!this.enabled || this.excludedPaths.some(p => req.path.startsWith(p))) return next();
            if (Math.random() >= this.aggression) return next();

            const types = [
                { type: 'latency', run: async () => { await sleep(Math.floor(Math.random() * 5000) + 500); next(); } },
                { type: 'error', run: () => res.status([500,502,503,504][Math.floor(Math.random() * 4)]).json({ error: 'Chaos' }) },
            ];
            const chaos = types[Math.floor(Math.random() * types.length)];
            console.log(`[Chaos] ${chaos.type} on ${req.path}`);
            await chaos.run();
        };
    }
}

export default ChaosMonkey;
```

---

## Architecture Diagram

```
┌──────────────────────────────────────────────────────────────┐
│                       CLIENTS                                │
└─────────────────────────┬────────────────────────────────────┘
┌─────────────────────────▼────────────────────────────────────┐
│                  DNS (Route53 / Cloudflare)                  │
└─────────────────────────┬────────────────────────────────────┘
              ┌───────────┴───────────┐
              ▼                       ▼
┌────────────────────┐    ┌────────────────────┐
│  REGION A (primary)│    │  REGION B (standby)│
│  ┌──────────────┐  │    │  ┌──────────────┐  │
│  │   ALB/NLB    │  │    │  │   ALB/NLB    │  │
│  └──────┬───────┘  │    │  └──────┬───────┘  │
│  ┌──────▼───────┐  │    │  ┌──────▼───────┐  │
│  │   Node.js    │  │    │  │   Node.js    │  │
│  │ ┌──────────┐ │  │    │  │ ┌──────────┐ │  │
│  │ │ Circuit  │ │  │    │  │ │ Circuit  │ │  │
│  │ │ Breaker  │ │  │    │  │ │ Breaker  │ │  │
│  │ │ Bulkhead │ │  │    │  │ │ Bulkhead │ │  │
│  │ │ Retry    │ │  │    │  │ │ Retry    │ │  │
│  │ └──────────┘ │  │    │  │ └──────────┘ │  │
│  └──────┬───────┘  │    │  └──────┬───────┘  │
│  ┌──────▼───────┐  │    │  ┌──────▼───────┐  │
│  │Redis Sentinel│◄─┼────┼─►│Redis Replica │  │
│  └──────────────┘  │    │  └──────────────┘  │
│  ┌──────────────┐  │    │  ┌──────────────┐  │
│  │ PostgreSQL   │──┼────┼─►│ PostgreSQL   │  │
│  │   Primary    │  │    │  │   Standby    │  │
│  └──────────────┘  │    │  └──────────────┘  │
└────────────────────┘    └────────────────────┘

Layers: DNS → LB → App (CB+Retry+Bulkhead) → Cache → DB
```

---

## Cross-References

| Topic | Reference |
|-------|-----------|
| Backup strategies | [01-backup-recovery.md](./01-backup-recovery.md) |
| DR testing & continuity | [03-dr-testing-continuity.md](./03-dr-testing-continuity.md) |
| Deployment architecture | [../01-deployment-architecture/](../01-deployment-architecture/) |
| Kubernetes HA | [../03-container-orchestration/](../03-container-orchestration/) |
| Monitoring & alerting | [../08-deployment-monitoring/](../08-deployment-monitoring/) |

> **Previous**: [01-backup-recovery.md](./01-backup-recovery.md) | **Next**: [03-dr-testing-continuity.md](./03-dr-testing-continuity.md)
