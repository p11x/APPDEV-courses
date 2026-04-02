# Express.js Performance Optimization

## What You'll Learn

- Caching strategies for Express
- Compression middleware
- Static file optimization
- Connection pooling

## Caching Strategies

```javascript
// In-memory cache middleware
function cacheMiddleware(ttl = 60000) {
    const cache = new Map();

    return (req, res, next) => {
        if (req.method !== 'GET') return next();

        const key = req.originalUrl;
        const cached = cache.get(key);

        if (cached && Date.now() < cached.expires) {
            return res.json(cached.data);
        }

        const originalJson = res.json.bind(res);
        res.json = (data) => {
            cache.set(key, { data, expires: Date.now() + ttl });
            return originalJson(data);
        };

        next();
    };
}

app.use('/api/', cacheMiddleware(30000));
```

## Compression

```bash
npm install compression
```

```javascript
import compression from 'compression';

app.use(compression({
    threshold: 1024, // Only compress > 1KB
    level: 6,        // Compression level (1-9)
}));
```

## Static File Optimization

```javascript
import express from 'express';

app.use(express.static('public', {
    maxAge: '1d',           // Cache for 1 day
    etag: true,             // Enable ETag
    lastModified: true,     // Enable Last-Modified
    immutable: true,        // Immutable for versioned files
    index: false,           // Don't serve index.html
}));
```

## Best Practices Checklist

- [ ] Implement response caching
- [ ] Enable gzip compression
- [ ] Optimize static file serving
- [ ] Use CDN for static assets
- [ ] Monitor response times

## Cross-References

- See [Security](../05-security-implementation/01-helmet-cors.md) for security
- See [Monitoring](../14-monitoring-observability/01-apm-setup.md) for observability
- See [Architecture](../01-express-architecture/03-performance-characteristics.md) for benchmarks

## Next Steps

Continue to [Testing Strategies](../07-testing-strategies/01-unit-testing.md) for testing.
