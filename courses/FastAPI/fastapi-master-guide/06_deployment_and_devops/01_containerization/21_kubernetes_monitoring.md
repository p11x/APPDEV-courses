# Kubernetes Monitoring

## Overview

Monitoring Kubernetes deployments ensures application health and performance.

## Prometheus Integration

### Metrics Collection

```yaml
# Example 1: Prometheus configuration
apiVersion: v1
kind: ConfigMap
metadata:
  name: prometheus-config
data:
  prometheus.yml: |
    global:
      scrape_interval: 15s

    scrape_configs:
      - job_name: 'fastapi'
        kubernetes_sd_configs:
          - role: pod
        relabel_configs:
          - source_labels: [__meta_kubernetes_pod_annotation_prometheus_io_scrape]
            action: keep
            regex: true
```

### FastAPI Metrics

```python
# Example 2: FastAPI metrics endpoint
from prometheus_client import Counter, Histogram, generate_latest
from fastapi import FastAPI, Response

app = FastAPI()

REQUEST_COUNT = Counter('http_requests_total', 'Total requests', ['method', 'endpoint'])
REQUEST_DURATION = Histogram('http_request_duration_seconds', 'Request duration')

@app.middleware("http")
async def metrics_middleware(request, call_next):
    with REQUEST_DURATION.time():
        response = await call_next(request)
    REQUEST_COUNT.labels(method=request.method, endpoint=request.url.path).inc()
    return response

@app.get("/metrics")
async def metrics():
    return Response(generate_latest(), media_type="text/plain")
```

## Summary

Monitoring is essential for production Kubernetes deployments.

## Next Steps

Continue learning about:
- [Kubernetes Logging](./22_kubernetes_logging.md)
- [Kubernetes Troubleshooting](./23_kubernetes_troubleshooting.md)
