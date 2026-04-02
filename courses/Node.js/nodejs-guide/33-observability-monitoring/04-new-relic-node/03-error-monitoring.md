# New Relic Error Monitoring

## What You'll Learn

- How New Relic captures and groups errors
- How to analyze error traces
- How to set up error alerts
- How to track error trends

## Error Tracking

New Relic automatically captures unhandled exceptions and groups them by error class and message.

1. Go to APM → Errors
2. View grouped errors with stack traces
3. Click to see affected transactions

## Error Analytics

```sql
-- Errors by type
SELECT count(*) 
FROM TransactionError 
WHERE appName = 'my-api' 
FACET error.class 
SINCE 1 day ago

-- Error rate trend
SELECT percentage(count(*), WHERE error IS true) 
FROM Transaction 
WHERE appName = 'my-api' 
TIMESERIES 
SINCE 1 day ago

-- Top error messages
SELECT count(*) 
FROM TransactionError 
WHERE appName = 'my-api' 
FACET error.message 
SINCE 1 day ago 
LIMIT 20
```

## Custom Error Tracking

```ts
import newrelic from 'newrelic';

try {
  await riskyOperation();
} catch (err) {
  // Report to New Relic
  newrelic.noticeError(err, {
    customAttribute: 'important-value',
    userId: req.user?.id,
  });
  throw err;
}
```

## Next Steps

For custom metrics, continue to [Custom Metrics](./04-custom-metrics.md).
