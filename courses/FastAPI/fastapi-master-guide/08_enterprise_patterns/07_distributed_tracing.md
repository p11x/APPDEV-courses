# Distributed Tracing

## Overview

Distributed tracing tracks requests across microservices for debugging and performance analysis.

## OpenTelemetry Integration

### Tracing Setup

```python
# Example 1: OpenTelemetry with FastAPI
from opentelemetry import trace
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.trace.export import BatchSpanProcessor
from opentelemetry.exporter.jaeger.thrift import JaegerExporter
from opentelemetry.instrumentation.fastapi import FastAPIInstrumentor
from opentelemetry.instrumentation.httpx import HTTPXClientInstrumentor
from fastapi import FastAPI

app = FastAPI()

# Setup tracing
trace.set_tracer_provider(TracerProvider())
tracer = trace.get_tracer(__name__)

# Configure Jaeger exporter
jaeger_exporter = JaegerExporter(
    agent_host_name="jaeger",
    agent_port=6831,
)

trace.get_tracer_provider().add_span_processor(
    BatchSpanProcessor(jaeger_exporter)
)

# Instrument FastAPI
FastAPIInstrumentor.instrument_app(app)

# Instrument HTTPX for service calls
HTTPXClientInstrumentor().instrument()

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """Endpoint with automatic tracing"""
    with tracer.start_as_current_span("get_user") as span:
        span.set_attribute("user_id", user_id)

        # Database call will be traced automatically
        user = await get_user_from_db(user_id)

        # Service call will be traced
        orders = await get_user_orders(user_id)

        return {"user": user, "orders": orders}
```

### Custom Spans

```python
# Example 2: Custom span creation
@app.post("/orders/")
async def create_order(order: OrderCreate):
    """Create order with custom tracing"""
    with tracer.start_as_current_span("create_order") as span:
        span.set_attribute("order.items_count", len(order.items))

        # Validate order
        with tracer.start_as_current_span("validate_order"):
            await validate_order(order)

        # Check inventory
        with tracer.start_as_current_span("check_inventory") as span:
            available = await check_inventory(order.items)
            span.set_attribute("inventory.available", available)

        # Process payment
        with tracer.start_as_current_span("process_payment") as span:
            payment = await process_payment(order.total)
            span.set_attribute("payment.id", payment.id)

        # Create order
        with tracer.start_as_current_span("save_order"):
            new_order = await save_order(order)

        return new_order
```

## Summary

Distributed tracing provides visibility across microservices.

## Next Steps

Continue learning about:
- [Event Sourcing Patterns](./05_event_sourcing_patterns.md)
- [CQRS Patterns](./06_cqrs_patterns.md)
