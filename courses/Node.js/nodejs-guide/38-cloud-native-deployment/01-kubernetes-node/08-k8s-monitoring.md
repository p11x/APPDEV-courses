# Kubernetes Monitoring

## What You'll Learn

- How to set up Prometheus and Grafana
- How to configure Kubernetes metrics
- How to create alerts and dashboards
- How to implement log aggregation

---

## Layer 1: Academic Foundation

### Observability Stack

The three pillars of observability:

1. **Metrics**: Quantitative measurements over time (Prometheus)
2. **Logs**: Discrete events with timestamps (ELK/Loki)
3. **Traces**: Distributed request paths (Jaeger/Zipkin)

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Prometheus Operator

```yaml
# prometheus-operator.yaml
apiVersion: monitoring.coreos.com/v1
kind: Prometheus
metadata:
  name: prometheus
spec:
  serviceAccountName: prometheus
  serviceMonitorSelector:
    matchLabels:
      team: backend
  resources:
    requests:
      memory: 400Mi
  retention: 15d
```

### Paradigm 2 — ServiceMonitor

```yaml
# service-monitor.yaml
apiVersion: monitoring.coreos.com/v1
kind: ServiceMonitor
metadata:
  name: node-api-monitor
  labels:
    team: backend
spec:
  selector:
    matchLabels:
      app: node-api
  endpoints:
    - port: metrics
      path: /metrics
      interval: 15s
```

### Paradigm 3 — Grafana Dashboard

```yaml
# grafana-dashboard.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: node-api-dashboard
  labels:
    grafana_dashboard: "1"
data:
  dashboard.json: |
    {
      "title": "Node.js API Dashboard",
      "panels": [
        {
          "title": "CPU Usage",
          "type": "graph",
          "targets": [
            {
              "expr": "rate(process_cpu_seconds_total{job=\"node-api\"}[5m])"
            }
          ]
        },
        {
          "title": "Memory Usage",
          "type": "graph",
          "targets": [
            {
              "expr": "process_resident_memory_bytes{job=\"node-api\"}"
            }
          ]
        },
        {
          "title": "Request Rate",
          "type": "graph",
          "targets": [
            {
              "expr": "rate(http_requests_total[5m])"
            }
          ]
        }
      ]
    }
```

### Paradigm 4 — Alert Rules

```yaml
# alert-rules.yaml
apiVersion: monitoring.coreos.com/v1
kind: PrometheusRule
metadata:
  name: node-api-alerts
spec:
  groups:
    - name: node-api
      rules:
        - alert: HighErrorRate
          expr: rate(http_requests_total{status=~"5.."}[5m]) > 0.05
          for: 5m
          labels:
            severity: critical
          annotations:
            summary: High error rate detected
        - alert: HighLatency
          expr: histogram_quantile(0.99, rate(http_request_duration_seconds_bucket[5m])) > 2
          for: 5m
          labels:
            severity: warning
```

---

## Layer 3: Performance Engineering

### Key Metrics

| Metric | Description | Target |
|--------|-------------|--------|
| Request rate | Requests per second | > 1000 |
| Error rate | 5xx errors | < 1% |
| Latency p99 | 99th percentile | < 500ms |
| Memory usage | RSS memory | < 80% |

---

## Layer 4: Security

### Security Configuration

- Enable TLS for Prometheus
- Configure RBAC for metrics access
- Restrict metric endpoints

---

## Layer 5: Testing

### Metrics Validation

```typescript
// metrics-test.ts
import { register, httpRequestDuration } from './metrics';

describe('Metrics', () => {
  it('should expose metrics endpoint', async () => {
    const response = await fetch('http://localhost:3000/metrics');
    expect(response.ok).toBe(true);
    const text = await response.text();
    expect(text).toContain('http_request_duration_seconds');
  });
});
```

---

## Next Steps

Continue to [Kubernetes Best Practices](./09-k8s-best-practices.md) for production guidelines.