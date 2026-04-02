# Loki Logs

## What You'll Learn

- What Loki is and how it differs from Elasticsearch
- How to send logs to Loki
- How to query logs with LogQL
- How to set up Loki with Node.js

## What Is Loki?

Loki is a **log aggregation system** by Grafana. Unlike Elasticsearch, it only indexes **labels** (metadata), not the full log content — making it cheaper and faster.

| Feature | Elasticsearch | Loki |
|---------|--------------|------|
| Indexes | Full text | Labels only |
| Storage | High | Low |
| Query | KQL/ES Query | LogQL (Prometheus-like) |
| Best for | Full-text search | Cost-effective log aggregation |

## Sending Logs to Loki

```bash
npm install pino pino-loki
```

```ts
// logger.ts — Send logs to Loki

import pino from 'pino';

const logger = pino({
  transport: {
    targets: [
      // Console output
      {
        target: 'pino-pretty',
        options: { colorize: true },
        level: 'info',
      },
      // Loki output
      {
        target: 'pino-loki',
        options: {
          batching: true,
          interval: 5,  // Batch and send every 5 seconds
          host: process.env.LOKI_URL || 'http://localhost:3100',
          labels: {
            app: 'my-api',
            env: process.env.NODE_ENV || 'development',
          },
        },
        level: 'info',
      },
    ],
  },
});

export default logger;
```

## LogQL Queries

```sql
-- All logs from my-api
{app="my-api"}

-- Error logs only
{app="my-api"} |= "error"

-- JSON parsing and filtering
{app="my-api"} | json | level="error"

-- Count errors per minute
count_over_time({app="my-api"} |= "error" [1m])

-- Rate of errors
rate({app="my-api"} |= "error" [5m])
```

## Next Steps

For dashboards, continue to [Grafana Dashboards](./03-grafana-dashboards.md).
