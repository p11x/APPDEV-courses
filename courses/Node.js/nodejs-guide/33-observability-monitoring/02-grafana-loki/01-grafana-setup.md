# Grafana Setup

## What You'll Learn

- What Grafana is and how it works
- How to install and configure Grafana
- How to add data sources
- How Grafana fits into the observability stack

## What Is Grafana?

Grafana is an **open-source visualization platform** for metrics, logs, and traces. It connects to data sources (Prometheus, Loki, Elasticsearch, Tempo) and creates dashboards.

## Docker Setup

```yaml
# docker-compose.yml

services:
  grafana:
    image: grafana/grafana:latest
    ports:
      - '3000:3000'
    environment:
      - GF_SECURITY_ADMIN_PASSWORD=admin
    volumes:
      - grafana-data:/var/lib/grafana

  prometheus:
    image: prom/prometheus:latest
    ports:
      - '9090:9090'
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml

  loki:
    image: grafana/loki:latest
    ports:
      - '3100:3100'

volumes:
  grafana-data:
```

```yaml
# prometheus.yml
global:
  scrape_interval: 15s

scrape_configs:
  - job_name: 'nodejs'
    static_configs:
      - targets: ['host.docker.internal:3000']
```

## Accessing Grafana

```bash
# Start
docker compose up -d

# Open http://localhost:3000
# Login: admin / admin
```

## Adding Data Sources

1. Go to Configuration → Data Sources
2. Add Prometheus: URL = `http://prometheus:9090`
3. Add Loki: URL = `http://loki:3100`
4. Click "Save & Test"

## Next Steps

For Loki logs, continue to [Loki Logs](./02-loki-logs.md).
