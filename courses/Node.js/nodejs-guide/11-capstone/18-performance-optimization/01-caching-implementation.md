# Performance Optimization for NodeMark

## What You'll Build In This File

Application-level caching, database query optimization, CDN integration, and performance monitoring setup.

## Application Caching Layer

```javascript
// src/services/cache.js — Multi-level cache implementation
import { createClient } from 'redis';
import NodeCache from 'node-cache';
import { config } from '../config/index.js';

// L1: In-memory cache (fastest)
const memoryCache = new NodeCache({ stdTTL: 60, maxKeys: 500 });

// L2: Redis cache (shared across instances)
const redis = createClient({ url: config.redis.url });
await redis.connect();

export class CacheService {
    static async get(key) {
        // L1 check
        const memValue = memoryCache.get(key);
        if (memValue !== undefined) return memValue;

        // L2 check
        const redisValue = await redis.get(key);
        if (redisValue) {
            const parsed = JSON.parse(redisValue);
            memoryCache.set(key, parsed); // Populate L1
            return parsed;
        }

        return null;
    }

    static async set(key, value, ttl = 300) {
        memoryCache.set(key, value, Math.min(ttl, 300));
        await redis.set(key, JSON.stringify(value), { EX: ttl });
    }

    static async invalidate(pattern) {
        memoryCache.flushAll(); // Simple: flush all L1

        let cursor = 0;
        do {
            const result = await redis.scan(cursor, {
                MATCH: pattern,
                COUNT: 100,
            });
            cursor = result.cursor;
            if (result.keys.length) await redis.del(result.keys);
        } while (cursor !== 0);
    }

    static async getOrSet(key, fetcher, ttl = 300) {
        const cached = await this.get(key);
        if (cached) return cached;

        const data = await fetcher();
        await this.set(key, data, ttl);
        return data;
    }
}

// Cache middleware
export function cache(ttl = 60) {
    return async (req, res, next) => {
        if (req.method !== 'GET') return next();

        const key = `http:${req.originalUrl}:${req.user?.userId || 'anon'}`;
        const cached = await CacheService.get(key);

        if (cached) {
            res.set('X-Cache', 'HIT');
            return res.json(cached);
        }

        const originalJson = res.json.bind(res);
        res.json = async (data) => {
            await CacheService.set(key, data, ttl);
            res.set('X-Cache', 'MISS');
            return originalJson(data);
        };

        next();
    };
}
```

## Database Query Optimization

```javascript
// src/db/optimized-queries.js — Optimized query patterns

// BAD: N+1 query
async function getBookmarksWithTagsBad(userId) {
    const bookmarks = await query('SELECT * FROM bookmarks WHERE user_id = $1', [userId]);
    for (const bookmark of bookmarks.rows) {
        const tags = await query(
            `SELECT t.name FROM tags t
             JOIN bookmark_tags bt ON bt.tag_id = t.id
             WHERE bt.bookmark_id = $1`,
            [bookmark.id]
        );
        bookmark.tags = tags.rows.map(t => t.name);
    }
    return bookmarks.rows;
}

// GOOD: Single query with JOIN
async function getBookmarksWithTagsGood(userId) {
    const { rows } = await query(
        `SELECT b.*, COALESCE(json_agg(t.name) FILTER (WHERE t.name IS NOT NULL), '[]') as tags
         FROM bookmarks b
         LEFT JOIN bookmark_tags bt ON bt.bookmark_id = b.id
         LEFT JOIN tags t ON t.id = bt.tag_id
         WHERE b.user_id = $1
         GROUP BY b.id
         ORDER BY b.created_at DESC`,
        [userId]
    );
    return rows;
}

// GOOD: Cursor-based pagination (better than OFFSET for large datasets)
async function getBookmarksCursor(userId, cursor, limit = 20) {
    let sql, params;
    if (cursor) {
        sql = `SELECT * FROM bookmarks
               WHERE user_id = $1 AND id < $2
               ORDER BY id DESC LIMIT $3`;
        params = [userId, cursor, limit];
    } else {
        sql = `SELECT * FROM bookmarks
               WHERE user_id = $1
               ORDER BY id DESC LIMIT $2`;
        params = [userId, limit];
    }
    const { rows } = await query(sql, params);
    return {
        data: rows,
        nextCursor: rows.length ? rows[rows.length - 1].id : null,
        hasMore: rows.length === limit,
    };
}

// GOOD: Use covering index
// CREATE INDEX idx_bookmarks_user_created ON bookmarks(user_id, created_at DESC) INCLUDE (title, url);
```

## Static Asset Optimization

```javascript
// src/middleware/static.js — Optimized static file serving
import express from 'express';

// Serve static assets with long cache for hashed files
app.use('/static', express.static('dist', {
    maxAge: '1y',
    immutable: true,
    setHeaders: (res, filePath) => {
        if (filePath.endsWith('.html')) {
            res.setHeader('Cache-Control', 'no-cache');
        } else if (filePath.match(/\.[a-f0-9]{8}\./)) {
            // Hashed files: cache forever
            res.setHeader('Cache-Control', 'public, max-age=31536000, immutable');
        }
    },
}));

// Compression middleware
import compression from 'compression';
app.use(compression({
    filter: (req, res) => {
        if (req.headers['x-no-compression']) return false;
        return compression.filter(req, res);
    },
    threshold: 1024, // Only compress responses > 1KB
}));
```

## Performance Monitoring

```javascript
// src/middleware/performance.js — Performance tracking
import { performance } from 'node:perf_hooks';

export function performanceMiddleware() {
    return (req, res, next) => {
        const start = performance.now();

        res.on('finish', () => {
            const duration = performance.now() - start;

            // Log slow requests
            if (duration > 1000) {
                console.warn(`SLOW: ${req.method} ${req.path} ${duration.toFixed(0)}ms`);
            }

            // Track metrics
            if (global.metrics) {
                global.metrics.requestDuration.observe(
                    { method: req.method, path: req.route?.path || req.path, status: res.statusCode },
                    duration / 1000
                );
            }
        });

        next();
    };
}

// Performance benchmarks
```
Typical NodeMark Performance Targets:
─────────────────────────────────────────────
Endpoint                p50      p95      p99
─────────────────────────────────────────────
GET  /bookmarks         15ms     45ms     120ms
POST /bookmarks         25ms     80ms     200ms
POST /auth/login        150ms    250ms    400ms
GET  /bookmarks/:id     10ms     30ms     80ms
GET  /bookmarks (cached) 3ms     8ms      15ms

Improvement with caching:
├── Without cache: 45ms p95
├── With L1 cache: 8ms p95 (5.6x faster)
├── With L2 cache: 12ms p95 (3.7x faster)
└── With both:     5ms p95  (9x faster)
```

## How It Connects

- Caching follows [16-caching-redis](../../../16-caching-redis/) patterns
- DB optimization follows [06-databases-performance/02-database-performance-optimization/](../../../06-databases-performance/02-database-performance-optimization/)
- Monitoring follows [21-logging-monitoring](../../../21-logging-monitoring/)

## Common Mistakes

- Not measuring before optimizing
- Caching without invalidation
- Using OFFSET for pagination on large tables
- Not compressing API responses

## Try It Yourself

### Exercise 1: Benchmark
Measure p95 latency before and after caching.

### Exercise 2: Query Optimization
Find and fix an N+1 query in the bookmarks list.

### Exercise 3: Cache Hit Ratio
Implement cache hit/miss logging and measure the ratio.

## Next Steps

Continue to [19-deployment-operations/01-production-deployment.md](../19-deployment-operations/01-production-deployment.md).
