# New Relic Custom Metrics

## What You'll Learn

- How to record custom metrics in New Relic
- How to create custom dashboards
- How to use custom events
- How to track business metrics

## Custom Metrics

```ts
import newrelic from 'newrelic';

// Record a custom metric
newrelic.recordMetric('Custom/Orders/Value', orderTotal);
newrelic.recordMetric('Custom/Queue/Size', queueLength);

// Increment a counter
newrelic.incrementMetric('Custom/Orders/Count');
```

## Custom Events

```ts
// Record a custom event (queryable in NRQL)
newrelic.recordCustomEvent('OrderCompleted', {
  orderId: 'order-123',
  userId: 'user-456',
  total: 99.99,
  itemCount: 3,
  paymentMethod: 'credit_card',
});
```

```sql
-- Query custom events in NRQL
SELECT count(*) 
FROM OrderCompleted 
FACET paymentMethod 
SINCE 1 day ago

SELECT average(total) 
FROM OrderCompleted 
TIMESERIES 
SINCE 1 week ago
```

## Business Metrics Dashboard

```sql
-- Revenue by hour
SELECT sum(total) 
FROM OrderCompleted 
TIMESERIES 1 hour 
SINCE 1 day ago

-- Conversion funnel
SELECT count(*) 
FROM PageView 
WHERE pageUrl LIKE '%/checkout%' 
FACET pageUrl

-- Top customers by spend
SELECT sum(total) 
FROM OrderCompleted 
FACET userId 
LIMIT 10
```

## Next Steps

For Honeycomb, continue to [Honeycomb Setup](../05-honeycomb-nodejs/01-honeycomb-setup.md).
