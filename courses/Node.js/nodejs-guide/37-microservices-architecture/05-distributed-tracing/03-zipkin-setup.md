# Zipkin Setup

## What You'll Learn

- How to set up Zipkin for distributed tracing
- How Zipkin compares to Jaeger
- How to use Zipkin UI

## Setup

```yaml
# docker-compose.yml
services:
  zipkin:
    image: openzipkin/zipkin:latest
    ports:
      - "9411:9411"
```

## Configuration

```ts
import { ZipkinExporter } from '@opentelemetry/exporter-zipkin';

const exporter = new ZipkinExporter({
  url: 'http://localhost:9411/api/v2/spans',
  serviceName: 'my-service',
});
```

## Comparison

| Feature | Jaeger | Zipkin |
|---------|--------|--------|
| UI | Rich | Simple |
| Storage | Cassandra, ES | ES, MySQL, memory |
| Performance | Higher | Lower |
| Best for | Production | Quick setup |

## Next Steps

For tracing patterns, continue to [Tracing Patterns](./04-tracing-patterns.md).
