# Deployment Performance Optimization

## What You'll Learn

- Application performance in production
- Caching strategies for deployment
- CDN configuration
- Database query optimization
- Static asset optimization

## Production Caching

```javascript
// src/cache.js
import { createClient } from 'redis';
import NodeCache from 'node-cache';

// Multi-level cache
class ProductionCache {
    constructor() {
        // L1: In-memory (fastest)
        this.memoryCache = new NodeCache({
            stdTTL: 60,
            checkperiod: 120,
            maxKeys: 1000,
        });

        // L2: Redis (shared across instances)
        this.redis = createClient({ url: process.env.REDIS_URL });
        this.redis.connect();
    }

    async get(key) {
        // L1 check
        const memValue = this.memoryCache.get(key);
        if (memValue !== undefined) return memValue;

        // L2 check
        const redisValue = await this.redis.get(key);
        if (redisValue) {
            const parsed = JSON.parse(redisValue);
            this.memoryCache.set(key, parsed); // Populate L1
            return parsed;
        }

        return null;
    }

    async set(key, value, ttl = 300) {
        this.memoryCache.set(key, value, ttl);
        await this.redis.set(key, JSON.stringify(value), { EX: ttl });
    }

    async invalidate(pattern) {
        this.memoryCache.flushAll();

        let cursor = 0;
        do {
            const result = await this.redis.scan(cursor, {
                MATCH: pattern,
                COUNT: 100,
            });
            cursor = result.cursor;
            if (result.keys.length > 0) {
                await this.redis.del(result.keys);
            }
        } while (cursor !== 0);
    }
}

// Cache middleware
function cacheMiddleware(cache, ttl = 300) {
    return async (req, res, next) => {
        if (req.method !== 'GET') return next();

        const key = `route:${req.originalUrl}`;
        const cached = await cache.get(key);

        if (cached) {
            res.set('X-Cache', 'HIT');
            return res.json(cached);
        }

        const originalJson = res.json.bind(res);
        res.json = async (data) => {
            await cache.set(key, data, ttl);
            res.set('X-Cache', 'MISS');
            return originalJson(data);
        };

        next();
    };
}
```

## CDN Configuration

```javascript
// Static asset headers for CDN
app.use('/static', express.static('dist', {
    maxAge: '1y',
    immutable: true,
    setHeaders: (res, path) => {
        if (path.endsWith('.html')) {
            res.setHeader('Cache-Control', 'no-cache');
        } else if (path.match(/\.[a-f0-9]{8}\./)) {
            // Hashed assets — cache forever
            res.setHeader('Cache-Control', 'public, max-age=31536000, immutable');
        }
    },
}));

// API response caching
app.get('/api/products', cacheMiddleware(cache, 60), async (req, res) => {
    const products = await db.query('SELECT * FROM products WHERE active = true');
    res.json(products);
});
```

## Performance Benchmarks

```
Production Performance Targets:
─────────────────────────────────────────────
Metric               Target        Critical
─────────────────────────────────────────────
Response time (p50)  < 100ms       < 200ms
Response time (p95)  < 250ms       < 500ms
Response time (p99)  < 500ms       < 1000ms
Error rate           < 0.1%        < 1%
Availability         > 99.9%       > 99.5%
Throughput           > 1000 rps    > 500 rps

Optimization Impact:
├── Redis cache:     50-100x faster responses
├── CDN:             10-50x faster static assets
├── Gzip:            70-90% smaller responses
├── Connection pool: 5-10x better DB throughput
└── HTTP/2:          20-30% faster page loads
```

## Best Practices Checklist

- [ ] Implement multi-level caching (memory + Redis)
- [ ] Use CDN for static assets
- [ ] Set proper Cache-Control headers
- [ ] Use connection pooling for databases
- [ ] Enable gzip/brotli compression
- [ ] Minify and bundle JavaScript
- [ ] Optimize database queries
- [ ] Monitor response time percentiles

## Cross-References

- See [Caching](../../06-databases-performance/04-caching-strategies-implementation/) for caching strategies
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for performance monitoring
- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for scaling patterns

## Next Steps

Continue to [Disaster Recovery](../11-disaster-recovery/01-backup-recovery.md).
