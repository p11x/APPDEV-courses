# OpenTelemetry Tracing

## Overview

OpenTelemetry (OTel) provides vendor-neutral APIs for collecting telemetry data. This guide covers distributed tracing with OpenTelemetry and Jaeger.

## Prerequisites

- Docker basics
- Understanding of distributed systems

## Step-by-Step Examples

### OpenTelemetry Collector

```yaml
# otel-collector-config.yaml
receivers:
  otlp:
    protocols:
      grpc:
        endpoint: 0.0.0.0:4317
      http:
        endpoint: 0.0.0.0:4318

processors:
  batch:
    timeout: 10s

exporters:
  jaeger:
    endpoint: jaeger:14250
    tls:
      insecure: true

service:
  pipelines:
    traces:
      receivers: [otlp]
      processors: [batch]
      exporters: [jaeger]
```

### Docker Compose Stack

```yaml
# docker-compose.yml
version: "3.8"

services:
  otel-collector:
    image: otel/opentelemetry-collector:latest
    volumes:
      - ./otel-collector-config.yaml:/etc/otelcol-contrib/config.yaml
    ports:
      - "4317:4317"
      - "4318:4318"

  jaeger:
    image: jaegertracing/all-in-one:latest
    ports:
      - "16686:16686"
      - "14250:14250"
```

## Quick Reference

| Port | Service |
|------|---------|
| 4317 | OTel gRPC |
| 4318 | OTel HTTP |
| 16686 | Jaeger UI |

## Docker Expansion Complete: 45/45 files ✅

Now I need to create the Kubernetes expansion files.
