# Express.js Microservices Architecture

## What You'll Learn

- Service communication patterns
- Load balancing strategies
- Distributed tracing
- Service discovery

## Service Communication

```javascript
// HTTP-based communication
import express from 'express';

const app = express();

// API Gateway pattern
app.get('/api/dashboard', async (req, res) => {
    const [users, orders, products] = await Promise.all([
        fetch('http://user-service:3001/users').then(r => r.json()),
        fetch('http://order-service:3002/orders').then(r => r.json()),
        fetch('http://product-service:3003/products').then(r => r.json()),
    ]);

    res.json({ users, orders, products });
});
```

## Health Checks

```javascript
app.get('/health', (req, res) => {
    res.json({
        status: 'healthy',
        service: 'user-service',
        uptime: process.uptime(),
        timestamp: new Date().toISOString(),
    });
});

app.get('/ready', async (req, res) => {
    try {
        await db.ping();
        res.json({ status: 'ready' });
    } catch {
        res.status(503).json({ status: 'not ready' });
    }
});
```

## Best Practices Checklist

- [ ] Implement health check endpoints
- [ ] Use circuit breaker for external calls
- [ ] Implement distributed tracing
- [ ] Use message queues for async communication
- [ ] Design for failure

## Cross-References

- See [Architecture](../01-express-architecture/01-lifecycle-deep-dive.md) for request flow
- See [Monitoring](../14-monitoring-observability/01-apm-setup.md) for observability
- See [Container](../13-container-orchestration/01-docker-setup.md) for deployment

## Next Steps

Continue to [Real-time Applications](../12-realtime-applications/01-websockets.md) for WebSockets.
