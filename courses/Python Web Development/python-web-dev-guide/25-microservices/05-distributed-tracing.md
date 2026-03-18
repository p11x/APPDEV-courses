# Distributed Tracing

## What You'll Learn
- Trace requests across services
- OpenTelemetry
- Jaeger

## Prerequisites
- Completed event-driven architecture

## OpenTelemetry

```bash
pip install opentelemetry-api opentelemetry-sdk
```

```python
from opentelemetry import trace
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.trace.export import BatchSpanProcessor
from opentelemetry.exporter.jaeger.thrift import JaegerExporter
from opentelemetry.sdk.resources import Resource

# Setup tracing
provider = TracerProvider()
jaeger_exporter = JaegerExporter(
    agent_host_name="localhost",
    agent_port=6831,
)
provider.add_span_processor(BatchSpanProcessor(jaeger_exporter))
trace.set_tracer_provider(provider)

tracer = trace.get_tracer(__name__)

@app.get("/api/users/{user_id}")
async def get_user(user_id: int):
    with tracer.start_as_current_span("get_user") as span:
        span.set_attribute("user_id", user_id)
        
        # Call another service
        user = await call_user_service(user_id)
        
        return user
```

## Summary
- Use distributed tracing for debugging
- OpenTelemetry is the standard
- Jaeger for visualization

## Next Steps
→ Move to `26-containerization/`
