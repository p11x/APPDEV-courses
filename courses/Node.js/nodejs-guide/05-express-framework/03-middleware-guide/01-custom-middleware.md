# Custom Middleware Creation and Patterns

## What You'll Learn

- Creating custom middleware functions
- Middleware patterns and best practices
- Error handling middleware
- Middleware debugging techniques

## Middleware Fundamentals

```javascript
// Basic middleware structure
function myMiddleware(req, res, next) {
    // Pre-processing
    req.startTime = Date.now();
    console.log(`${req.method} ${req.path}`);

    // Call next to continue chain
    next();
}

// Async middleware
async function asyncMiddleware(req, res, next) {
    try {
        req.userData = await fetchUser(req.params.id);
        next();
    } catch (err) {
        next(err); // Pass error to error handler
    }
}

// Middleware with options
function rateLimit(options = {}) {
    const { windowMs = 60000, max = 100 } = options;
    const requests = new Map();

    return (req, res, next) => {
        const key = req.ip;
        const now = Date.now();

        if (!requests.has(key)) {
            requests.set(key, []);
        }

        const timestamps = requests.get(key);
        while (timestamps.length > 0 && timestamps[0] < now - windowMs) {
            timestamps.shift();
        }

        if (timestamps.length >= max) {
            return res.status(429).json({ error: 'Too many requests' });
        }

        timestamps.push(now);
        next();
    };
}

app.use(rateLimit({ windowMs: 15 * 60 * 1000, max: 100 }));
```

## Error Handling Middleware

```javascript
// Error handler (4 parameters)
app.use((err, req, res, next) => {
    console.error(`[${req.method}] ${req.path}:`, err.message);

    if (err.isOperational) {
        return res.status(err.statusCode).json({
            error: { message: err.message, code: err.code }
        });
    }

    console.error('Unexpected:', err.stack);
    res.status(500).json({
        error: { message: 'Internal server error' }
    });
});
```

## Middleware Patterns

```javascript
// Response time middleware
function responseTime(req, res, next) {
    const start = process.hrtime.bigint();

    res.on('finish', () => {
        const duration = Number(process.hrtime.bigint() - start) / 1e6;
        console.log(`${req.method} ${req.path} ${res.statusCode} ${duration.toFixed(2)}ms`);
    });

    next();
}

// Request ID middleware
import { randomUUID } from 'node:crypto';

function requestId(req, res, next) {
    req.id = req.headers['x-request-id'] || randomUUID();
    res.set('X-Request-Id', req.id);
    next();
}

// CORS middleware
function cors(options = {}) {
    const { origin = '*', methods = 'GET,POST,PUT,DELETE' } = options;

    return (req, res, next) => {
        res.set('Access-Control-Allow-Origin', origin);
        res.set('Access-Control-Allow-Methods', methods);
        res.set('Access-Control-Allow-Headers', 'Content-Type, Authorization');

        if (req.method === 'OPTIONS') {
            return res.sendStatus(204);
        }

        next();
    };
}
```

## Best Practices Checklist

- [ ] Always call next() or send a response
- [ ] Use async middleware for I/O operations
- [ ] Pass errors to next(err)
- [ ] Keep middleware focused on single responsibility
- [ ] Document middleware behavior

## Cross-References

- See [Lifecycle](../01-express-architecture/01-lifecycle-deep-dive.md) for request flow
- See [Error Handling](../08-error-handling/01-centralized-errors.md) for error patterns
- See [Security](../05-security-implementation/01-helmet-cors.md) for security middleware

## Next Steps

Continue to [Middleware Ordering](./02-middleware-ordering.md) for execution optimization.
