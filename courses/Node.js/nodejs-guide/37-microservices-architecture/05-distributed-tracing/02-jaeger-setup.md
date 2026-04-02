# Jaeger Setup

## What You'll Learn

- How to set up Jaeger for distributed tracing
- How to use Jaeger UI
- How to query traces
- How Jaeger compares to Zipkin

## Setup

```yaml
# docker-compose.yml

services:
  jaeger:
    image: jaegertracing/all-in-one:latest
    ports:
      - "16686:16686"   # UI
      - "4318:4318"     # OTLP HTTP
      - "6831:6831"     # Jaeger Thrift
    environment:
      COLLECTOR_OTLP_ENABLED: "true"
```

## Configuration

```ts
// Connect to Jaeger via OTLP
const exporter = new OTLPTraceExporter({
  url: 'http://localhost:4318/v1/traces',
});
```

## Using Jaeger UI

1. Open http://localhost:16686
2. Select your service from the dropdown
3. Click "Find Traces"
4. Click on a trace to see the waterfall chart
5. Click on spans to see attributes and events

## Next Steps

For Zipkin, continue to [Zipkin Setup](./03-zipkin-setup.md).
