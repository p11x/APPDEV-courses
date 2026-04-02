# API Versioning, Rate Limiting, and Caching

## What You'll Build In This File

API versioning strategy, rate limiting middleware, and response caching for the NodeMark API.

## API Versioning

```javascript
// src/routes/v1/bookmarks.js — Versioned API routes
import { Router } from 'express';
import { authenticate } from '../../middleware/auth.js';

const router = Router();
router.use(authenticate);

// V1: Simple bookmark response
router.get('/', async (req, res) => {
    const { rows } = await query(
        'SELECT id, title, url, description, created_at FROM bookmarks WHERE user_id = $1',
        [req.user.userId]
    );
    res.json(rows);
});

// src/routes/v2/bookmarks.js — V2 adds tags and pagination
router.get('/', async (req, res) => {
    const { limit = 20, offset = 0 } = req.query;
    const { rows } = await query(
        `SELECT b.*, json_agg(t.name) as tags
         FROM bookmarks b
         LEFT JOIN bookmark_tags bt ON bt.bookmark_id = b.id
         LEFT JOIN tags t ON t.id = bt.tag_id
         WHERE b.user_id = $1
         GROUP BY b.id
         ORDER BY b.created_at DESC
         LIMIT $2 OFFSET $3`,
        [req.user.userId, limit, offset]
    );
    res.json({
        data: rows,
        pagination: { limit, offset },
    });
});

// Version router
// src/routes/index.js
import { Router } from 'express';
import { router as v1Bookmarks } from './v1/bookmarks.js';
import { router as v2Bookmarks } from './v2/bookmarks.js';

const apiRouter = Router();

// Header-based versioning
apiRouter.use('/bookmarks', (req, res, next) => {
    const version = req.headers['api-version'] || '1';
    if (version === '2') return v2Bookmarks(req, res, next);
    return v1Bookmarks(req, res, next);
});

// Or path-based: /api/v1/bookmarks, /api/v2/bookmarks
```

## Rate Limiting

```javascript
// src/middleware/rate-limit.js — Redis-backed rate limiting
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
await redis.connect();

export function rateLimit(options = {}) {
    const {
        windowMs = 60 * 1000,  // 1 minute
        max = 100,              // 100 requests per window
        keyGenerator = (req) => req.user?.userId || req.ip,
        message = 'Too many requests',
    } = options;

    return async (req, res, next) => {
        const key = `ratelimit:${keyGenerator(req)}`;
        const windowSec = Math.ceil(windowMs / 1000);

        const current = await redis.incr(key);
        if (current === 1) await redis.expire(key, windowSec);

        res.setHeader('X-RateLimit-Limit', max);
        res.setHeader('X-RateLimit-Remaining', Math.max(0, max - current));
        res.setHeader('X-RateLimit-Reset', Math.ceil(Date.now() / 1000) + windowSec);

        if (current > max) {
            return res.status(429).json({
                error: 'Too Many Requests',
                message,
                retryAfter: windowSec,
            });
        }

        next();
    };
}

// Different limits for different endpoints
app.use('/auth/login', rateLimit({ windowMs: 15 * 60 * 1000, max: 5 }));
app.use('/auth/register', rateLimit({ windowMs: 60 * 60 * 1000, max: 3 }));
app.use('/api/', rateLimit({ windowMs: 60 * 1000, max: 100 }));
app.use('/api/bookmarks', rateLimit({ windowMs: 60 * 1000, max: 60 }));
```

## Response Caching

```javascript
// src/middleware/cache.js — HTTP response caching
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
await redis.connect();

export function cacheResponse(ttl = 60) {
    return async (req, res, next) => {
        if (req.method !== 'GET') return next();

        const key = `cache:${req.originalUrl}:${req.user?.userId || 'anon'}`;
        const cached = await redis.get(key);

        if (cached) {
            res.set('X-Cache', 'HIT');
            return res.json(JSON.parse(cached));
        }

        // Intercept response
        const originalJson = res.json.bind(res);
        res.json = async (data) => {
            await redis.set(key, JSON.stringify(data), { EX: ttl });
            res.set('X-Cache', 'MISS');
            return originalJson(data);
        };

        next();
    };
}

// Cache invalidation helper
export async function invalidateUserCache(userId) {
    let cursor = 0;
    do {
        const result = await redis.scan(cursor, {
            MATCH: `cache:*:${userId}`,
            COUNT: 100,
        });
        cursor = result.cursor;
        if (result.keys.length) await redis.del(result.keys);
    } while (cursor !== 0);
}

// Usage
router.get('/bookmarks', authenticate, cacheResponse(60), listHandler);
router.post('/bookmarks', authenticate, async (req, res, next) => {
    // ... create bookmark
    await invalidateUserCache(req.user.userId);
    res.status(201).json(bookmark);
});
```

## How It Connects

- Rate limiting follows [19-security-rate-limiting](../../../19-security-rate-limiting/)
- Caching follows [16-caching-redis](../../../16-caching-redis/) patterns
- Versioning follows REST API best practices

## Common Mistakes

- Not setting rate limit headers for clients
- Cache invalidation not working correctly
- Versioning without deprecation strategy
- Not differentiating rate limits by endpoint sensitivity

## Try It Yourself

### Exercise 1: Test Rate Limiting
Make 110 requests in 1 minute and verify the 429 response.

### Exercise 2: Cache Hit Ratio
Add logging to measure cache hit/miss ratio.

### Exercise 3: Version Migration
Create a V2 response with additional fields.

## Next Steps

Continue to [16-advanced-testing/01-e2e-integration.md](../16-advanced-testing/01-e2e-integration.md).
