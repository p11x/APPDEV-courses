# Log Analysis

## What You'll Learn

- How to analyze logs effectively
- How to set up log-based alerts
- How to create log aggregation dashboards
- How to troubleshoot with logs

## Log Analysis Queries

```logql
-- Top error messages
{app="my-api"} | json | level="error" | line_format "{{.message}}" | topk(10, count_over_time({app="my-api"} |= "error" [1h]))

-- Slow requests (>1s)
{app="my-api"} | json | duration > 1000ms

-- Errors by endpoint
{app="my-api"} | json | level="error" | line_format "{{.route}}" | topk(10, count_over_time({app="my-api"} |= "error" [1h]))

-- Request volume by status code
{app="my-api"} | json | status != "" | count_over_time({app="my-api"} [1m]) by (status)
```

## Log-Based Alerts

```yaml
# alert-rules.yml
groups:
  - name: log-alerts
    rules:
      - alert: HighErrorRate
        expr: |
          count_over_time({app="my-api"} |= "error" [5m]) > 100
        for: 2m
        labels:
          severity: critical
        annotations:
          summary: "High error rate detected"

      - alert: SlowRequests
        expr: |
          count_over_time({app="my-api"} | json | duration > 5000 [5m]) > 10
        for: 1m
        labels:
          severity: warning
```

## Next Steps

For Datadog, continue to [Datadog Setup](../03-datadog-node/01-datadog-setup.md).
