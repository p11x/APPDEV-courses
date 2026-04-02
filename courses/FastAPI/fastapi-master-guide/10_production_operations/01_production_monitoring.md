# Production Monitoring

## Overview

Production monitoring ensures FastAPI applications remain healthy and performant.

## Monitoring Stack

### Prometheus + Grafana

```python
# Example 1: Prometheus metrics
from prometheus_client import (
    Counter, Histogram, Gauge, generate_latest,
    REGISTRY
)
from fastapi import FastAPI, Request, Response
import time

app = FastAPI()

# Metrics
REQUEST_COUNT = Counter(
    'http_requests_total',
    'Total HTTP requests',
    ['method', 'endpoint', 'status']
)

REQUEST_DURATION = Histogram(
    'http_request_duration_seconds',
    'HTTP request duration',
    ['method', 'endpoint']
)

ACTIVE_REQUESTS = Gauge(
    'http_requests_active',
    'Active HTTP requests'
)

DB_CONNECTIONS = Gauge(
    'database_connections_active',
    'Active database connections'
)

@app.middleware("http")
async def metrics_middleware(request: Request, call_next):
    """Collect request metrics"""
    ACTIVE_REQUESTS.inc()
    start = time.time()

    try:
        response = await call_next(request)

        REQUEST_COUNT.labels(
            method=request.method,
            endpoint=request.url.path,
            status=response.status_code
        ).inc()

        return response
    finally:
        duration = time.time() - start
        REQUEST_DURATION.labels(
            method=request.method,
            endpoint=request.url.path
        ).observe(duration)
        ACTIVE_REQUESTS.dec()

@app.get("/metrics")
async def metrics():
    """Prometheus metrics endpoint"""
    return Response(
        content=generate_latest(REGISTRY),
        media_type="text/plain"
    )
```

### Health Checks

```python
# Example 2: Comprehensive health checks
from fastapi import FastAPI, Depends
from sqlalchemy import text
from datetime import datetime
import redis.asyncio as redis

app = FastAPI()

@app.get("/health")
async def health():
    """Basic health check"""
    return {"status": "healthy", "timestamp": datetime.utcnow()}

@app.get("/health/detailed")
async def detailed_health():
    """Detailed health check"""
    checks = {}

    # Database check
    try:
        await db.execute(text("SELECT 1"))
        checks["database"] = {"status": "healthy"}
    except Exception as e:
        checks["database"] = {"status": "unhealthy", "error": str(e)}

    # Redis check
    try:
        await redis_client.ping()
        checks["redis"] = {"status": "healthy"}
    except Exception as e:
        checks["redis"] = {"status": "unhealthy", "error": str(e)}

    # External service check
    try:
        async with httpx.AsyncClient() as client:
            response = await client.get("http://external-service/health")
            checks["external"] = {"status": "healthy" if response.status_code == 200 else "unhealthy"}
    except Exception:
        checks["external"] = {"status": "unreachable"}

    overall = "healthy" if all(
        c["status"] == "healthy" for c in checks.values()
    ) else "degraded"

    return {
        "status": overall,
        "checks": checks,
        "timestamp": datetime.utcnow()
    }
```

## Alerting

### Alert Configuration

```yaml
# Example 3: Prometheus alerting rules
groups:
  - name: fastapi
    rules:
      - alert: HighErrorRate
        expr: |
          rate(http_requests_total{status=~"5.."}[5m])
          / rate(http_requests_total[5m]) > 0.05
        for: 5m
        labels:
          severity: critical
        annotations:
          summary: "High error rate detected"

      - alert: HighLatency
        expr: |
          histogram_quantile(0.95, rate(http_request_duration_seconds_bucket[5m])) > 1
        for: 5m
        labels:
          severity: warning

      - alert: ServiceDown
        expr: up{job="fastapi"} == 0
        for: 1m
        labels:
          severity: critical
```

## Summary

Production monitoring is essential for reliability.

## Next Steps

Continue learning about:
- [Incident Response](./02_incident_response.md)
- [Disaster Recovery](./03_disaster_recovery.md)
