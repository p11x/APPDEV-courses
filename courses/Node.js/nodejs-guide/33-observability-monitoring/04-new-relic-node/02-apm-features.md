# New Relic APM Features

## What You'll Learn

- How to use New Relic's transaction tracing
- How to analyze slow queries
- How to use service maps
- How to set up alerts

## Transaction Traces

New Relic automatically captures traces for HTTP transactions:

1. Go to APM → select service → Transactions
2. Click on a transaction to see the trace
3. View waterfall chart showing all sub-operations

## NRQL Queries

```sql
-- Average response time by endpoint
SELECT average(duration) FROM Transaction 
WHERE appName = 'my-api' 
FACET name 
SINCE 1 hour ago

-- Error rate
SELECT percentage(count(*), WHERE error IS true) 
FROM Transaction 
WHERE appName = 'my-api' 
SINCE 1 hour ago

-- Slowest transactions
SELECT name, duration 
FROM Transaction 
WHERE appName = 'my-api' 
ORDER BY duration DESC 
LIMIT 10

-- Throughput
SELECT rate(count(*), 1 minute) 
FROM Transaction 
WHERE appName = 'my-api' 
SINCE 1 hour ago
```

## Alerts

```sql
-- Alert: high error rate
SELECT percentage(count(*), WHERE error IS true) 
FROM Transaction 
WHERE appName = 'my-api' 
SINCE 5 minutes ago 
ABOVE 5
```

## Next Steps

For error monitoring, continue to [Error Monitoring](./03-error-monitoring.md).
