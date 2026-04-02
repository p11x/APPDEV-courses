# API Aggregation Pattern

## What You'll Learn

- How to aggregate responses from multiple services
- How to implement BFF (Backend for Frontend)
- How to handle partial failures

## Aggregation Gateway

```ts
// aggregator.ts

import express from 'express';

const app = express();

// Aggregate data from multiple services
app.get('/api/dashboard/:userId', async (req, res) => {
  const { userId } = req.params;

  try {
    // Parallel requests with timeout
    const [user, orders, notifications] = await Promise.all([
      fetchWithTimeout(`http://user-service/users/${userId}`, 3000),
      fetchWithTimeout(`http://order-service/users/${userId}/orders`, 5000),
      fetchWithTimeout(`http://notification-service/users/${userId}`, 3000),
    ]);

    res.json({
      user: await user.json(),
      recentOrders: (await orders.json()).slice(0, 5),
      unreadNotifications: (await notifications.json()).filter((n) => !n.read).length,
    });
  } catch (err) {
    // Return partial data on failure
    res.status(207).json({
      error: 'Partial data available',
      message: err.message,
    });
  }
});

async function fetchWithTimeout(url: string, timeout: number) {
  const controller = new AbortController();
  setTimeout(() => controller.abort(), timeout);
  return fetch(url, { signal: controller.signal });
}
```

## Next Steps

For antipatterns, continue to [Antipatterns](./06-antipatterns.md).
