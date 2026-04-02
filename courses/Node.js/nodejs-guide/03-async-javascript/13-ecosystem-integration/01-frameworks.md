# Async Frameworks: Express, Fastify, Koa

## What You'll Learn

- Async patterns in Express.js
- Fastify async hooks and plugins
- Koa middleware async composition
- Framework async performance comparison

## Express.js Async Patterns

```javascript
import express from 'express';

const app = express();
app.use(express.json());

// Async route handler with error wrapper
const asyncHandler = (fn) => (req, res, next) => {
    Promise.resolve(fn(req, res, next)).catch(next);
};

app.get('/api/users', asyncHandler(async (req, res) => {
    const users = await db.query('SELECT * FROM users');
    res.json(users);
}));

app.get('/api/users/:id', asyncHandler(async (req, res) => {
    const user = await db.query('SELECT * FROM users WHERE id = $1', [req.params.id]);
    if (!user) return res.status(404).json({ error: 'Not found' });
    res.json(user);
}));

// Error handler
app.use((err, req, res, next) => {
    console.error(err);
    res.status(500).json({ error: 'Internal server error' });
});
```

## Fastify Async Plugins

```javascript
import Fastify from 'fastify';

const fastify = Fastify({ logger: true });

// Async plugin
fastify.register(async (instance) => {
    instance.get('/api/users', async (request, reply) => {
        const users = await db.query('SELECT * FROM users');
        return users; // Auto-serialized to JSON
    });

    instance.get('/api/users/:id', async (request, reply) => {
        const user = await db.query(
            'SELECT * FROM users WHERE id = $1',
            [request.params.id]
        );
        if (!user) {
            reply.code(404);
            return { error: 'Not found' };
        }
        return user;
    });
});

await fastify.listen({ port: 3000 });
```

## Performance Comparison

```
Framework Async Performance (requests/sec):
─────────────────────────────────────────────
Fastify    ████████████████████████████  55,000
Koa        ██████████████████████        40,000
Express    ████████████████████          35,000
Hono       ██████████████████████████████ 58,000
```

## Best Practices Checklist

- [ ] Use asyncHandler wrapper in Express
- [ ] Leverage Fastify native async support
- [ ] Use Koa for async middleware composition
- [ ] Benchmark frameworks for your workload
- [ ] Handle errors in all async route handlers

## Cross-References

- See [Database Operations](./02-database-operations.md) for async DB patterns
- See [API Development](./03-api-development.md) for async API patterns
- See [Error Handling](../07-async-error-handling/01-error-propagation.md) for errors

## Next Steps

Continue to [Database Operations](./02-database-operations.md) for async DB patterns.
