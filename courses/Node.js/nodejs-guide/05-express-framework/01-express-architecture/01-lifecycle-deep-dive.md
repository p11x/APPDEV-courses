# Express.js Request/Response Lifecycle Deep Dive

## What You'll Learn

- Express.js internal request processing flow
- Middleware chain execution mechanism
- Response transformation pipeline
- Performance characteristics and bottlenecks

## Request Processing Lifecycle

```
Express.js Request Lifecycle:
─────────────────────────────────────────────
HTTP Request Arrives
    │
    ▼
┌─────────────────────────────────────┐
│  1. HTTP Parser (Node.js core)      │
│     Parse raw HTTP request          │
│     Create IncomingMessage object   │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│  2. Express App Initialization      │
│     Create req/res objects          │
│     Attach Express extensions       │
│     Initialize middleware index     │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│  3. Middleware Chain Execution      │
│     For each middleware:            │
│       ├─ Call middleware fn         │
│       ├─ Pass (req, res, next)     │
│       ├─ next() → continue         │
│       └─ No next() → end chain     │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│  4. Router Matching                 │
│     Match URL + HTTP method         │
│     Execute route-specific          │
│     middleware and handler          │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│  5. Response Generation             │
│     Handler calls res.json() etc.   │
│     Serialize response body         │
│     Set headers                     │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│  6. Response Send                   │
│     HTTP Response sent to client    │
│     'finish' event emitted          │
│     Connection cleanup              │
└─────────────────────────────────────┘
```

## Middleware Chain Implementation

```javascript
// Simplified Express middleware chain implementation
class ExpressApp {
    constructor() {
        this.stack = []; // Middleware stack
    }

    use(fn) {
        this.stack.push({ type: 'middleware', handle: fn });
    }

    get(path, ...handlers) {
        this.stack.push({ type: 'route', method: 'GET', path, handlers });
    }

    handle(req, res, done) {
        let index = 0;

        const next = (err) => {
            if (index >= this.stack.length) {
                return done(err);
            }

            const layer = this.stack[index++];

            if (err) {
                // Skip to error handler
                if (layer.handle.length === 4) {
                    return layer.handle(err, req, res, next);
                }
                return next(err);
            }

            try {
                if (layer.type === 'middleware') {
                    layer.handle(req, res, next);
                } else if (layer.type === 'route') {
                    if (this.matchRoute(layer, req)) {
                        this.executeRoute(layer, req, res, next);
                    } else {
                        next();
                    }
                }
            } catch (error) {
                next(error);
            }
        };

        next();
    }
}
```

## Request Object Extensions

```javascript
// Express extends Node.js IncomingMessage
app.use((req, res, next) => {
    // Express-added properties
    console.log(req.params);      // Route parameters
    console.log(req.query);       // Query string parsed
    console.log(req.body);        // Parsed body (with middleware)
    console.log(req.cookies);     // Parsed cookies
    console.log(req.path);        // URL path only
    console.log(req.hostname);    // Host header
    console.log(req.ip);          // Client IP
    console.log(req.protocol);    // http or https
    console.log(req.secure);      // true if HTTPS
    console.log(req.xhr);         // true if XHR request
    console.log(req.get('Content-Type')); // Header getter

    next();
});
```

## Response Object Extensions

```javascript
// Express extends Node.js ServerResponse
app.get('/api/data', (req, res) => {
    // Status methods
    res.status(200);                    // Set status code
    res.sendStatus(200);                // Set status + send OK

    // Header methods
    res.set('X-Custom', 'value');       // Set header
    res.get('X-Custom');                // Get header

    // Content methods
    res.json({ data: 'value' });        // JSON response
    res.send('Hello');                  // Send string/buffer
    res.render('template', { data });   // Template render
    res.redirect('/other');             // Redirect
    res.download('./file.pdf');         // File download
    res.sendFile('./page.html');        // Send file

    // Cookie methods
    res.cookie('name', 'value', options);
    res.clearCookie('name');

    // Chaining
    res.status(201).json({ created: true });
});
```

## Performance Characteristics

```
Express Performance Profile:
─────────────────────────────────────────────
Overhead per request:
├── Middleware stack traversal: ~0.01ms per middleware
├── Route matching: ~0.02ms (depends on route count)
├── req/res object creation: ~0.005ms
├── JSON serialization: ~0.01ms per KB
└── Total framework overhead: ~0.1-0.5ms

Bottlenecks:
├── Too many middleware (>50): Adds latency
├── Synchronous operations in handlers: Blocks event loop
├── Large JSON responses: CPU-bound serialization
├── Missing compression: Increases bandwidth
└── No caching: Repeated expensive operations
```

## Best Practices Checklist

- [ ] Keep middleware count under 20 for performance
- [ ] Use async middleware for I/O operations
- [ ] Implement early returns in middleware
- [ ] Monitor request processing time
- [ ] Use response compression for large payloads

## Cross-References

- See [Middleware Guide](../03-middleware-guide/01-custom-middleware.md) for middleware patterns
- See [Advanced Routing](../02-advanced-routing/01-route-parameters.md) for routing
- See [Performance](../06-performance-optimization/01-caching-strategies.md) for optimization

## Next Steps

Continue to [Router Internals](./02-router-internals.md) for router implementation details.
