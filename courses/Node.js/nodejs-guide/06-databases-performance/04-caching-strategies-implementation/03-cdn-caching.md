# CDN Caching for Static and Dynamic Content

## What You'll Learn

- CDN integration with Node.js
- Cache-Control headers optimization
- Static asset caching strategies
- Dynamic content caching with CDN
- Edge caching patterns

## Cache-Control Headers

```javascript
import express from 'express';

const app = express();

// Static assets - long cache, immutable
app.use('/static', express.static('public', {
    maxAge: '1y',
    immutable: true,
    etag: true,
    lastModified: true,
}));

// Static assets with content hash in filename
app.use('/assets', express.static('dist', {
    maxAge: '1y',
    immutable: true,
    setHeaders: (res, path) => {
        if (path.endsWith('.html')) {
            res.setHeader('Cache-Control', 'no-cache');
        }
    },
}));

// API responses - short cache or no cache
app.get('/api/users', (req, res) => {
    res.setHeader('Cache-Control', 'private, max-age=60');
    res.json(users);
});

app.get('/api/users/:id', (req, res) => {
    res.setHeader('Cache-Control', 'private, max-age=300');
    res.json(user);
});

// Public data - CDN-cacheable
app.get('/api/products', (req, res) => {
    res.setHeader('Cache-Control', 'public, max-age=300, s-maxage=600, stale-while-revalidate=86400');
    res.json(products);
});

// No cache for sensitive data
app.get('/api/account', (req, res) => {
    res.setHeader('Cache-Control', 'no-store, no-cache, must-revalidate, proxy-revalidate');
    res.json(account);
});
```

## ETag and Conditional Requests

```javascript
import crypto from 'node:crypto';

// ETag middleware
function etagMiddleware(options = {}) {
    const weak = options.weak !== false;

    return (req, res, next) => {
        const originalJson = res.json.bind(res);

        res.json = (body) => {
            const bodyStr = JSON.stringify(body);
            const hash = crypto.createHash('md5').update(bodyStr).digest('hex');
            const etag = weak ? `W/"${hash}"` : `"${hash}"`;

            res.setHeader('ETag', etag);

            // Check If-None-Match
            if (req.headers['if-none-match'] === etag) {
                return res.status(304).end();
            }

            return originalJson(body);
        };

        next();
    };
}

app.use(etagMiddleware());
```

## CDN Purge API

```javascript
class CDNManager {
    constructor(config) {
        this.provider = config.provider; // 'cloudflare', 'cloudfront', 'fastly'
        this.apiKey = config.apiKey;
        this.zoneId = config.zoneId;
    }

    async purgeUrls(urls) {
        switch (this.provider) {
            case 'cloudflare':
                return this.cloudflarePurge(urls);
            case 'cloudfront':
                return this.cloudfrontPurge(urls);
            default:
                throw new Error(`Unsupported CDN provider: ${this.provider}`);
        }
    }

    async cloudflarePurge(urls) {
        const response = await fetch(
            `https://api.cloudflare.com/client/v4/zones/${this.zoneId}/purge_cache`,
            {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${this.apiKey}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ files: urls }),
            }
        );
        return response.json();
    }

    async purgeAll() {
        const response = await fetch(
            `https://api.cloudflare.com/client/v4/zones/${this.zoneId}/purge_cache`,
            {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${this.apiKey}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ purge_everything: true }),
            }
        );
        return response.json();
    }

    async purgeByTag(tags) {
        const response = await fetch(
            `https://api.cloudflare.com/client/v4/zones/${this.zoneId}/purge_cache`,
            {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${this.apiKey}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ tags }),
            }
        );
        return response.json();
    }
}

// Purge on content update
app.put('/api/products/:id', async (req, res) => {
    const product = await updateProduct(req.params.id, req.body);

    // Purge CDN cache
    await cdn.purgeUrls([
        `https://cdn.example.com/api/products/${req.params.id}`,
        `https://cdn.example.com/api/products`,
    ]);

    res.json(product);
});
```

## Cache-Control Header Reference

```
Cache-Control Directives:
─────────────────────────────────────────────
public          Can be cached by CDN/proxy
private         Only browser can cache
no-cache        Must revalidate before using
no-store        Never cache
max-age=N       Cache for N seconds
s-maxage=N      CDN cache for N seconds
immutable       Asset won't change (skip revalidation)
stale-while-revalidate=N  Serve stale while fetching fresh

Recommended values:
├── Static assets (hashed):  public, max-age=31536000, immutable
├── HTML pages:              no-cache (or max-age=0, must-revalidate)
├── API public data:         public, max-age=60, s-maxage=300
├── API private data:        private, max-age=30
├── Authenticated responses: no-store
└── Images/media:            public, max-age=2592000, immutable
```

## Best Practices Checklist

- [ ] Use content hashing for static asset filenames
- [ ] Set appropriate Cache-Control headers per content type
- [ ] Implement ETag for conditional requests
- [ ] Use `s-maxage` for CDN-specific TTL
- [ ] Implement CDN purge on content updates
- [ ] Use `stale-while-revalidate` for non-critical data
- [ ] Set `Vary` header for content negotiation
- [ ] Monitor CDN cache hit rates

## Cross-References

- See [In-Memory Caching](./01-in-memory-caching.md) for application caching
- See [Redis Caching](./02-redis-caching.md) for server-side caching
- See [Cache Invalidation](./04-cache-invalidation.md) for invalidation patterns

## Next Steps

Continue to [Cache Invalidation](./04-cache-invalidation.md) for invalidation strategies.
