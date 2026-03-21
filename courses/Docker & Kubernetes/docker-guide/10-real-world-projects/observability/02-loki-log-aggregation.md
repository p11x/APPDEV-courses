# Loki Log Aggregation

## Overview

Loki is a horizontally-scalable log aggregation system from Grafana Labs. It integrates well with Grafana for log visualization.

## Prerequisites

- Docker basics
- Understanding of logging

## Step-by-Step Examples

### Loki with Docker

```yaml
# docker-compose.yml
version: "3.8"

services:
  loki:
    image: grafana/loki:latest
    ports:
      - "3100:3100"
    volumes:
      - ./loki-config.yml:/etc/loki/local-config.yaml

  promtail:
    image: grafana/promtail:latest
    volumes:
      - /var/lib/docker/containers:/var/lib/docker/containers
      - ./promtail-config.yml:/etc/promtail/config.yml

  grafana:
    image: grafana/grafana:latest
    ports:
      - "3000:3000"
```

### Loki Config

```yaml
# loki-config.yml
auth_enabled: false

server:
  http_listen_port: 3100

schema_config:
  configs:
    - from: 2024-01-01
      store: boltdb-shipper
      object_store: filesystem
      schema: v11
      index:
        prefix: loki_index_

storage_config:
  boltdb:
    directory: /loki/index
  filesystem:
    directory: /loki/chunks

limits_config:
  reject_old_samples: true
  reject_old_samples_max_age: 168h
```

## Quick Reference

| Port | Service |
|------|---------|
| 3100 | Loki |
| 3000 | Grafana |

## What's Next

Continue to [OpenTelemetry Tracing](./03-opentelemetry-tracing.md) for distributed tracing.
