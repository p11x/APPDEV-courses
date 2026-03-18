# Prometheus Metrics

## What You'll Learn
- Prometheus basics
- Custom metrics
- Grafana integration

## Prerequisites
- Completed centralized logging

## Installation

```bash
pip install prometheus-client
```

## Basic Metrics

```python
from prometheus_client import Counter, Histogram, Gauge, generate_latest
from fastapi import FastAPI, Response

app = FastAPI()

# Counter - counts occurrences
requests_total = Counter(
    'http_requests_total',
    'Total HTTP requests',
    ['method', 'endpoint', 'status']
)

# Histogram - measures duration
request_duration = Histogram(
    'http_request_duration_seconds',
    'HTTP request duration',
    ['method', 'endpoint']
)

# Gauge - current value
active_users = Gauge('active_users', 'Number of active users')

@app.middleware("http")
async def track_metrics(request, call_next):
    method = request.method
    path = request.url.path
    
    # Track request
    requests_total.labels(method=method, endpoint=path, status=200).inc()
    
    return await call_next(request)

@app.get("/metrics")
async def metrics():
    return Response(generate_latest(), media_type="text/plain")
```

## Custom Metrics

```python
# Track database queries
db_query_duration = Histogram(
    'db_query_duration_seconds',
    'Database query duration',
    ['query_type']
)

# Track business metrics
orders_placed = Counter('orders_placed_total', 'Total orders')

@app.post("/orders")
async def create_order():
    orders_placed.inc()
    return {"status": "created"}
```

## Summary
- Use Prometheus for metrics
- Track requests, duration, errors
- Visualize with Grafana

## Next Steps
→ Continue to `04-health-checks.md`
