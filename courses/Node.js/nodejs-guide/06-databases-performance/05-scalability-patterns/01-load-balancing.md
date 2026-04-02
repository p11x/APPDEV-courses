# Scalability Patterns with Node.js

## What You'll Learn

- Horizontal vs vertical scaling
- Load balancing strategies
- Database scaling patterns
- Connection scaling optimization

## Horizontal Scaling with Cluster

```javascript
import cluster from 'node:cluster';
import os from 'node:os';

if (cluster.isPrimary) {
    const numWorkers = os.cpus().length;
    console.log(`Primary ${process.pid} starting ${numWorkers} workers`);

    for (let i = 0; i < numWorkers; i++) {
        cluster.fork();
    }

    cluster.on('exit', (worker) => {
        console.log(`Worker ${worker.process.pid} died, restarting...`);
        cluster.fork();
    });
} else {
    import('./server.js');
}
```

## Load Balancing with Nginx

```nginx
# /etc/nginx/conf.d/node-app.conf
upstream node_app {
    least_conn;
    server 127.0.0.1:3000;
    server 127.0.0.1:3001;
    server 127.0.0.1:3002;
    server 127.0.0.1:3003;
}

server {
    listen 80;
    server_name example.com;

    location / {
        proxy_pass http://node_app;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_cache_bypass $http_upgrade;
    }

    location /health {
        proxy_pass http://node_app/health;
    }
}
```

## Database Scaling Patterns

```
Database Scaling Strategies:
─────────────────────────────────────────────
Vertical Scaling:
├── Increase CPU/RAM
├── Faster storage (SSD)
└── Limit: Hardware constraints

Horizontal Scaling:
├── Read replicas (read-heavy)
├── Sharding (write-heavy)
├── Connection pooling
└── Application-level caching

Read/Write Splitting:
├── Write → Primary
├── Read → Replica(s)
├── Application routes queries
└── Handle replication lag
```

## Connection Scaling

```javascript
// Multi-pool for read/write splitting
import { Pool } from 'pg';

const writePool = new Pool({
    host: process.env.PG_PRIMARY_HOST,
    max: 10,
});

const readPool = new Pool({
    host: process.env.PG_REPLICA_HOST,
    max: 20,
});

async function query(text, params, { useRead = false } = {}) {
    const pool = useRead ? readPool : writePool;
    return pool.query(text, params);
}

// Usage
async function getUsers() {
    return query('SELECT * FROM users', [], { useRead: true });
}

async function createUser(data) {
    return query('INSERT INTO users (name) VALUES ($1)', [data.name]);
}
```

## Best Practices Checklist

- [ ] Use cluster module for multi-core utilization
- [ ] Implement health checks for load balancer
- [ ] Use read replicas for read-heavy workloads
- [ ] Implement connection pooling
- [ ] Monitor connection pool usage

## Cross-References

- See [Caching](../04-caching-strategies-implementation/01-in-memory-caching.md) for caching
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for observability
- See [Database Performance](../02-database-performance-optimization/01-query-optimization.md) for queries

## Next Steps

Continue to [Data Processing](../06-data-processing-transformation/01-streaming-data.md).
