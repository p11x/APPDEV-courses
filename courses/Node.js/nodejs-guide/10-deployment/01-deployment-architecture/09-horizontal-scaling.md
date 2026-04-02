# Horizontal Scaling

## What You'll Learn

- Scaling Node.js across multiple instances
- Stateless application design
- Shared session storage
- Database connection pooling at scale

## Stateless Design

```js
// WRONG — state stored in process memory
let requestCount = 0;
app.get('/stats', (req, res) => {
  res.json({ requests: ++requestCount });  // Different on each instance
});

// CORRECT — state in shared storage (Redis)
app.get('/stats', async (req, res) => {
  const count = await redis.incr('request-count');
  res.json({ requests: count });
});
```

## Next Steps

For microservices, continue to [Microservices](./03-microservices.md).
