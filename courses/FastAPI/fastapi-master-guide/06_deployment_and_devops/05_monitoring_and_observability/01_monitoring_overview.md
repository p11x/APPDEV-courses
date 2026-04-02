# Monitoring Overview

## Overview

Monitoring FastAPI applications ensures reliability, performance, and quick incident response. This guide covers essential monitoring concepts and tools.

## Health Checks

### Basic Health Endpoint

```python
# Example 1: Health check endpoint
from fastapi import FastAPI, Depends
from sqlalchemy import text
from datetime import datetime

app = FastAPI()

@app.get("/health")
async def health_check():
    """Basic health check"""
    return {
        "status": "healthy",
        "timestamp": datetime.utcnow().isoformat(),
        "version": "1.0.0"
    }

@app.get("/health/detailed")
async def detailed_health(db: Session = Depends(get_db)):
    """Detailed health check with dependencies"""
    checks = {}

    # Database check
    try:
        db.execute(text("SELECT 1"))
        checks["database"] = {"status": "healthy"}
    except Exception as e:
        checks["database"] = {"status": "unhealthy", "error": str(e)}

    # Redis check
    try:
        redis.ping()
        checks["redis"] = {"status": "healthy"}
    except Exception as e:
        checks["redis"] = {"status": "unhealthy", "error": str(e)}

    # Overall status
    overall = "healthy" if all(
        c["status"] == "healthy" for c in checks.values()
    ) else "degraded"

    return {
        "status": overall,
        "timestamp": datetime.utcnow().isoformat(),
        "checks": checks
    }
```

## Prometheus Metrics

### Metrics Setup

```python
# Example 2: Prometheus metrics integration
from prometheus_client import Counter, Histogram, Gauge, generate_latest
from fastapi import FastAPI, Request
from starlette.responses import Response
import time

app = FastAPI()

# Define metrics
REQUEST_COUNT = Counter(
    'http_requests_total',
    'Total HTTP requests',
    ['method', 'endpoint', 'status']
)

REQUEST_DURATION = Histogram(
    'http_request_duration_seconds',
    'HTTP request duration in seconds',
    ['method', 'endpoint']
)

ACTIVE_REQUESTS = Gauge(
    'http_requests_active',
    'Active HTTP requests'
)

@app.middleware("http")
async def metrics_middleware(request: Request, call_next):
    """Middleware to collect metrics"""
    ACTIVE_REQUESTS.inc()
    start_time = time.time()

    response = await call_next(request)

    duration = time.time() - start_time
    REQUEST_COUNT.labels(
        method=request.method,
        endpoint=request.url.path,
        status=response.status_code
    ).inc()
    REQUEST_DURATION.labels(
        method=request.method,
        endpoint=request.url.path
    ).observe(duration)
    ACTIVE_REQUESTS.dec()

    return response

@app.get("/metrics")
async def metrics():
    """Expose Prometheus metrics"""
    return Response(
        content=generate_latest(),
        media_type="text/plain"
    )
```

### Custom Metrics

```python
# Example 3: Custom application metrics
from prometheus_client import Counter, Histogram

# Business metrics
ORDERS_CREATED = Counter(
    'orders_created_total',
    'Total orders created'
)

ORDER_VALUE = Histogram(
    'order_value_dollars',
    'Order value in dollars',
    buckets=[10, 50, 100, 250, 500, 1000]
)

PAYMENT_DURATION = Histogram(
    'payment_processing_seconds',
    'Payment processing duration',
    buckets=[0.1, 0.5, 1.0, 2.0, 5.0]
)

@app.post("/orders/")
async def create_order(order: OrderCreate):
    """Create order with metrics"""
    start = time.time()

    # Process order
    result = await process_order(order)

    # Record metrics
    ORDERS_CREATED.inc()
    ORDER_VALUE.observe(result.total)

    return result
```

## Structured Logging

### JSON Logging

```python
# Example 4: Structured logging
import logging
import json
from datetime import datetime
from fastapi import Request
import uuid

class JSONFormatter(logging.Formatter):
    """JSON log formatter"""
    def format(self, record):
        log_data = {
            "timestamp": datetime.utcnow().isoformat(),
            "level": record.levelname,
            "message": record.getMessage(),
            "module": record.module,
            "function": record.funcName,
            "line": record.lineno
        }

        if record.exc_info:
            log_data["exception"] = self.formatException(record.exc_info)

        return json.dumps(log_data)

# Configure logging
logger = logging.getLogger("app")
handler = logging.StreamHandler()
handler.setFormatter(JSONFormatter())
logger.addHandler(handler)
logger.setLevel(logging.INFO)

@app.middleware("http")
async def request_logging(request: Request, call_next):
    """Log all requests"""
    request_id = str(uuid.uuid4())
    start = time.time()

    # Add request ID to context
    request.state.request_id = request_id

    response = await call_next(request)

    duration = time.time() - start
    logger.info(
        "Request completed",
        extra={
            "request_id": request_id,
            "method": request.method,
            "path": request.url.path,
            "status": response.status_code,
            "duration": duration
        }
    )

    response.headers["X-Request-ID"] = request_id
    return response
```

## Tracing

### OpenTelemetry Setup

```python
# Example 5: Distributed tracing with OpenTelemetry
from opentelemetry import trace
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.trace.export import BatchSpanProcessor
from opentelemetry.exporter.jaeger.thrift import JaegerExporter
from opentelemetry.instrumentation.fastapi import FastAPIInstrumentor

# Setup tracing
trace.set_tracer_provider(TracerProvider())
tracer = trace.get_tracer(__name__)

# Configure Jaeger exporter
jaeger_exporter = JaegerExporter(
    agent_host_name="localhost",
    agent_port=6831,
)

trace.get_tracer_provider().add_span_processor(
    BatchSpanProcessor(jaeger_exporter)
)

# Instrument FastAPI
app = FastAPI()
FastAPIInstrumentor.instrument_app(app)

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """Endpoint with tracing"""
    with tracer.start_as_current_span("get_user") as span:
        span.set_attribute("user_id", user_id)

        with tracer.start_as_current_span("database_query"):
            user = await db.get_user(user_id)

        return user
```

## Alerting

### Alert Configuration

```yaml
# Example 6: Prometheus alerting rules
# prometheus/alerts.yml
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
          description: "Error rate is {{ $value | humanizePercentage }}"

      - alert: HighLatency
        expr: |
          histogram_quantile(0.95, rate(http_request_duration_seconds_bucket[5m])) > 1
        for: 5m
        labels:
          severity: warning
        annotations:
          summary: "High latency detected"
          description: "95th percentile latency is {{ $value }}s"

      - alert: ServiceDown
        expr: up{job="fastapi"} == 0
        for: 1m
        labels:
          severity: critical
        annotations:
          summary: "Service is down"
```

## Dashboards

### Grafana Dashboard

```json
// Example 7: Grafana dashboard configuration
{
  "dashboard": {
    "title": "FastAPI Monitoring",
    "panels": [
      {
        "title": "Request Rate",
        "targets": [
          {
            "expr": "rate(http_requests_total[5m])"
          }
        ]
      },
      {
        "title": "Error Rate",
        "targets": [
          {
            "expr": "rate(http_requests_total{status=~'5..'}[5m]) / rate(http_requests_total[5m])"
          }
        ]
      },
      {
        "title": "Response Time (P95)",
        "targets": [
          {
            "expr": "histogram_quantile(0.95, rate(http_request_duration_seconds_bucket[5m]))"
          }
        ]
      }
    ]
  }
}
```

## Uptime Monitoring

### External Monitoring

```python
# Example 8: Uptime monitoring endpoint
@app.get("/health/ready")
async def readiness_check():
    """
    Readiness probe for Kubernetes.
    Returns 200 when ready to serve traffic.
    """
    try:
        # Check database
        db.execute(text("SELECT 1"))

        # Check cache
        redis.ping()

        return {"status": "ready"}
    except Exception:
        return Response(status_code=503)

@app.get("/health/live")
async def liveness_check():
    """
    Liveness probe for Kubernetes.
    Returns 200 if application is alive.
    """
    return {"status": "alive"}
```

## Best Practices

### Monitoring Guidelines

```python
# Example 9: Monitoring best practices
"""
Monitoring Best Practices:

1. Health Checks
   - Implement /health endpoint
   - Check all dependencies
   - Use for load balancers

2. Metrics
   - Track request rate, errors, latency (RED)
   - Monitor resource usage
   - Create business metrics

3. Logging
   - Use structured JSON logging
   - Include request IDs
   - Log at appropriate levels

4. Tracing
   - Implement distributed tracing
   - Track cross-service calls
   - Add meaningful spans

5. Alerting
   - Alert on symptoms, not causes
   - Set appropriate thresholds
   - Avoid alert fatigue

6. Dashboards
   - Create operational dashboards
   - Show key metrics
   - Make them actionable
"""
```

## Summary

| Component | Tool | Purpose |
|-----------|------|---------|
| Metrics | Prometheus | Collect metrics |
| Dashboards | Grafana | Visualize metrics |
| Logging | ELK/Loki | Centralized logs |
| Tracing | Jaeger/Zipkin | Request tracing |
| Alerting | AlertManager | Notifications |

## Next Steps

Continue learning about:
- [Prometheus Setup](./02_prometheus_setup.md) - Detailed setup
- [Grafana Dashboards](./03_grafana_dashboards.md) - Dashboard creation
- [Jaeger Tracing](./05_jaeger_tracing.md) - Distributed tracing
