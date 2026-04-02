# API Gateway Patterns

## What You'll Learn

- Common API gateway patterns
- How to implement request aggregation
- How to handle protocol translation
- How to version APIs at the gateway

## Pattern: API Aggregation

```ts
// Gateway aggregates responses from multiple services

app.get('/api/dashboard', async (req, res) => {
  const [users, orders, stats] = await Promise.all([
    fetch('http://user-service/users'),
    fetch('http://order-service/orders/recent'),
    fetch('http://stats-service/overview'),
  ]);

  res.json({
    users: await users.json(),
    recentOrders: await orders.json(),
    stats: await stats.json(),
  });
});
```

## Pattern: Protocol Translation

```
Client (REST) → Gateway → Service A (gRPC)
                       → Service B (GraphQL)
                       → Service C (REST)
```

## Pattern: API Versioning

```nginx
# Route by version header
location /api/v1/users {
    proxy_pass http://user-service-v1;
}

location /api/v2/users {
    proxy_pass http://user-service-v2;
}
```

## Next Steps

For security, continue to [Gateway Security](./05-gateway-security.md).
