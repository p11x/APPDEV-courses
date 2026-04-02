# Grafana Dashboards

## What You'll Learn

- How to create Grafana dashboards
- How to add panels for metrics and logs
- How to use variables and templates
- How to share and export dashboards

## Creating a Dashboard

1. Click "+" → Create → Dashboard
2. Add Panel → Select data source (Prometheus)
3. Write PromQL query
4. Configure visualization
5. Save

## Key Panels

### Request Rate

```promql
# Requests per second
rate(http_requests_total[1m])
```

### Error Rate

```promql
# Error percentage
rate(http_requests_total{status=~"5.."}[1m]) / rate(http_requests_total[1m]) * 100
```

### Latency

```promql
# P99 latency
histogram_quantile(0.99, rate(http_request_duration_seconds_bucket[5m]))
```

### Log Panel

Add a panel with Loki data source:

```logql
{app="my-api"} |= "error"
```

## Dashboard Variables

```json
// Add variable for environment filtering
{
  "name": "env",
  "type": "query",
  "query": "label_values(http_requests_total, env)"
}
```

Use in queries:

```promql
rate(http_requests_total{env="$env"}[1m])
```

## Export/Import

```bash
# Export dashboard as JSON
# Dashboard → Settings → JSON Model → Copy

# Import dashboard
# "+" → Import → Paste JSON
```

## Next Steps

For log analysis, continue to [Log Analysis](./04-log-analysis.md).
