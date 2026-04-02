# New Relic Setup

## What You'll Learn

- How to set up New Relic for Node.js
- How New Relic's APM agent works
- How to configure distributed tracing
- How New Relic compares to Datadog

## Setup

```bash
npm install newrelic
```

```js
// newrelic.js — Configuration file

'use strict';
exports.config = {
  app_name: ['my-api'],
  license_key: process.env.NEW_RELIC_LICENSE_KEY,
  distributed_tracing: { enabled: true },
  allow_all_headers: true,
  attributes: {
    exclude: [
      'request.headers.cookie',
      'request.headers.authorization',
      'request.headers.proxyAuthorization',
      'request.headers.setCookie*',
      'request.headers.x*',
      'response.headers.cookie',
      'response.headers.setCookie*',
    ],
  },
};
```

```ts
// server.ts — Import newrelic BEFORE everything else

import 'newrelic';  // Must be first import!
import express from 'express';

const app = express();

app.get('/api/users', async (req, res) => {
  const users = await db.query('SELECT * FROM users');
  res.json(users);
});

app.listen(3000);
```

## Environment Variables

```bash
# .env
NEW_RELIC_LICENSE_KEY=your-license-key
NEW_RELIC_APP_NAME=my-api
NEW_RELIC_DISTRIBUTED_TRACING_ENABLED=true
NEW_RELIC_LOG_ENABLED=false
```

## Comparison with Datadog

| Feature | New Relic | Datadog |
|---------|-----------|---------|
| Pricing | Per-seat + ingest | Per-host + ingest |
| Free tier | 100GB/month | 14-day trial |
| APM | Full | Full |
| Dashboard | NRQL (SQL-like) | PromQL + custom |
| Best for | All-in-one observability | Infrastructure-heavy |

## Next Steps

For APM features, continue to [APM Features](./02-apm-features.md).
