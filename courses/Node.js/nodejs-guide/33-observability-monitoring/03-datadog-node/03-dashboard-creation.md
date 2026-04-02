# Datadog Dashboard Creation

## What You'll Learn

- How to create Datadog dashboards
- How to add widgets for metrics, traces, and logs
- How to use template variables

## Creating a Dashboard

1. Go to Dashboards → New Dashboard
2. Add widgets (timeseries, toplist, query value, etc.)
3. Configure data sources
4. Save and share

## Key Widget Queries

```
# Request rate
avg:trace.http.request.hits{service:my-api}.as_rate()

# Error rate
(sum:trace.http.request.errors{service:my-api}.as_rate() / sum:trace.http.request.hits{service:my-api}.as_rate()) * 100

# P99 latency
p99:trace.http.request.duration{service:my-api}

# Active connections
avg:nodejs.eventloop.lag.avg{service:my-api}
```

## Template Variables

```json
{
  "variables": [
    {
      "name": "env",
      "type": "query",
      "query": "tag:env"
    },
    {
      "name": "service",
      "type": "query",
      "query": "tag:service"
    }
  ]
}
```

Use in queries: `avg:trace.http.request.hits{service:$service, env:$env}`

## Next Steps

For dashboard templates, continue to [Dashboard Templates](./04-dashboard-templates.md).
