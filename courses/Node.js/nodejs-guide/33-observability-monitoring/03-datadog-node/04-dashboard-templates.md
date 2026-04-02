# Datadog Dashboard Templates

## What You'll Learn

- Pre-built dashboard templates for Node.js
- How to import and customize templates
- Key metrics to monitor

## Node.js Dashboard

Import the official Node.js dashboard from Datadog:

1. Go to Dashboards → Browse
2. Search "Node.js"
3. Import "Node.js Overview"
4. Configure your service name

## Key Metrics to Monitor

| Metric | Description | Alert Threshold |
|--------|-------------|----------------|
| Request rate | Requests/sec | Sudden drop |
| Error rate | 5xx / total | > 1% |
| Latency p99 | 99th percentile response time | > 500ms |
| Event loop lag | Delay in event loop | > 100ms |
| Heap used | Memory consumption | > 80% of max |
| Active handles | Open connections | Growing unboundedly |
| GC pause time | Garbage collection duration | > 100ms |

## Custom Dashboard JSON

```json
{
  "title": "My API Dashboard",
  "widgets": [
    {
      "definition": {
        "type": "timeseries",
        "title": "Request Rate",
        "requests": [{
          "q": "sum:trace.http.request.hits{service:my-api}.as_rate()"
        }]
      }
    },
    {
      "definition": {
        "type": "timeseries",
        "title": "Error Rate",
        "requests": [{
          "q": "sum:trace.http.request.errors{service:my-api}.as_rate() / sum:trace.http.request.hits{service:my-api}.as_rate() * 100"
        }]
      }
    }
  ]
}
```

## Next Steps

For New Relic, continue to [New Relic Setup](../04-new-relic-node/01-newrelic-setup.md).
