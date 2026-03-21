# Prometheus and Grafana

## Overview

Prometheus collects metrics and Grafana visualizes them. Together they provide powerful observability for containerized applications.

## Prerequisites

- Docker and Compose basics
- Understanding of metrics

## Step-by-Step Examples

### Prometheus Configuration

```yaml
# prometheus.yml
global:
  scrape_interval: 15s

scrape_configs:
  - job_name: 'prometheus'
    static_configs:
      - targets: ['localhost:9090']

  - job_name: 'myapp'
    static_configs:
      - targets: ['myapp:8080']
```

### Docker Compose Stack

```yaml
# docker-compose.yml
version: "3.8"

services:
  prometheus:
    image: prom/prometheus:latest
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml
      - prometheus-data:/prometheus
    ports:
      - "9090:9090"
    command:
      - '--config.file=/etc/prometheus/prometheus.yml'
      - '--storage.tsdb.path=/prometheus'

  grafana:
    image: grafana/grafana:latest
    environment:
      - GF_SECURITY_ADMIN_PASSWORD=admin
    ports:
      - "3000:3000"
    volumes:
      - grafana-data:/var/lib/grafana

volumes:
  prometheus-data:
  grafana-data:
```

## Quick Reference

| Port | Service |
|------|---------|
| 9090 | Prometheus |
| 3000 | Grafana |

## What's Next

Continue to [Loki Log Aggregation](./02-loki-log-aggregation.md) for logging.
