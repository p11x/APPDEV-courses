# Swarm Monitoring

## Overview

Monitoring Docker Swarm is essential for production deployments. This guide covers built-in monitoring tools, service logs aggregation, and integrating with Prometheus for metrics collection.

## Prerequisites

- Active Docker Swarm cluster
- Understanding of Docker services

## Core Concepts

### Monitoring Options

- **docker stats**: Real-time container metrics
- **docker service logs**: Aggregated logs across replicas
- **Visualizer**: Web-based Swarm dashboard
- **Prometheus**: Industry-standard metrics collection

## Step-by-Step Examples

### Viewing Service Logs

```bash
# View logs for a service
# Aggregates logs from all replicas
docker service logs my-nginx

# Follow logs in real-time
docker service logs -f my-nginx

# Show timestamps
docker service logs -t my-nginx

# Show recent logs only
docker service logs --tail 100 my-nginx

# Example output:
# my-nginx.1 | 192.168.1.1 - - [15/Jan/2024:10:00:00] "GET / HTTP/1.1" 200
# my-nginx.2 | 192.168.1.2 - - [15/Jan/2024:10:00:01] "GET / HTTP/1.1" 200
```

### Service Logs for Troubleshooting

```bash
# Find errors in logs
docker service logs my-api 2>&1 | grep -i error

# Get last 50 error lines
docker service logs --tail 50 my-api | grep ERROR

# Filter by task ID
docker service logs my-api --task my-api.1
```

### Docker Stats

```bash
# View resource usage for all containers
docker stats

# Stream stats
docker stats --no-stream

# Format output
docker stats --format "table {{.Name}}\t{{.CPUPerc}}\t{{.MemUsage}}"

# Stats for specific service
docker stats $(docker ps -q --filter name=my-nginx)

# Example output:
# CONTAINER ID   NAME                CPU %   MEM USAGE / LIMIT     MEM %   NET I/O           BLOCK I/O
# abc123         my-nginx.1.abc123   1.25%   50MiB / 256MiB       19.53%  1MB / 500KB       10MB / 0B
# def456         my-nginx.2.def456   1.10%   48MiB / 256MiB       18.75%  1MB / 480KB       8MB / 0B
```

### Visualizer Deployment

```bash
# Deploy Swarm Visualizer
# Shows cluster state in web UI
docker service create \
  --name=viz \
  --publish=8080:8080 \
  --constraint=node.role==manager \
  --mount=type=bind,src=/var/run/docker.sock,dst=/var/run/docker.sock \
  dockersamples/visualizer

# Access at http://your-manager:8080
```

### Prometheus with Swarm

```yaml
# docker-compose.yml for Prometheus on Swarm
version: "3.8"

services:
  prometheus:
    image: prom/prometheus:latest
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml
      - prometheus-data:/prometheus
    command:
      - '--config.file=/etc/prometheus/prometheus.yml'
      - '--storage.tsdb.path=/prometheus'
    deploy:
      replicas: 1
      placement:
        constraints:
          - node.role == manager
    ports:
      - "9090:9090"
    networks:
      - monitoring

networks:
  monitoring:
    driver: overlay

volumes:
  prometheus-data:
```

```yaml
# prometheus.yml for Swarm service discovery
global:
  scrape_interval: 15s
  evaluation_interval: 15s

scrape_configs:
  - job_name: 'docker-swarm-nodes'
    dockerswarm_sd_configs:
      - host: unix:///var/run/docker.sock
        role: nodes
    relabel_configs:
      - source_labels: [__meta_dockerswarm_node_hostname]
        target_label: instance

  - job_name: 'docker-swarm-services'
    dockerswarm_sd_configs:
      - host: unix:///var/run/docker.sock
        role: services
    relabel_configs:
      - source_labels: [__meta_dockerswarm_service_name]
        target_label: service

  - job_name: 'docker-swarm-tasks'
    dockerswarm_sd_configs:
      - host: unix:///var/run/docker.sock
        role: tasks
    relabel_configs:
      - source_labels: [__meta_dockerswarm_service_name]
        target_label: service
      - source_labels: [__meta_dockerswarm_task_name]
        target_label: task
```

### Health Checks in Services

```bash
# Create service with health check
docker service create \
  --name api \
  --health-cmd "curl -f http://localhost/health || exit 1" \
  --health-interval 30s \
  --health-timeout 10s \
  --health-retries 3 \
  --health-start-period 30s \
  myapi:latest

# Check health status
docker service ps api

# Example shows HEALTH status:
# ID             NAME      IMAGE      NODE   DESIRED STATE  CURRENT STATE           HEALTH
# abc123         api.1     myapi      node1  Running        Running 2 minutes ago   Healthy
```

## Gotchas for Docker Users

- **Log driver**: Default json-file driver may not be ideal for production
- **Stats refresh**: docker stats refreshes every second by default
- **Prometheus access**: Needs Docker socket access on manager

## Common Mistakes

- **No log aggregation**: Losing logs when containers restart
- **Metrics gaps**: Not using external Prometheus for history
- **Resource limits**: Forgetting to set memory/CPU limits

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker service logs NAME` | View service logs |
| `docker service logs -f NAME` | Follow logs |
| `docker stats` | Container stats |
| `docker stats --no-stream` | One-time stats |

| Metric | Description |
|--------|-------------|
| CPU % | CPU usage percentage |
| MEM USAGE | Memory usage/limit |
| NET I/O | Network I/O |
| BLOCK I/O | Block I/O |
| PIDS | Process count |

## What's Next

Continue to [MACVLAN Basics](../07-advanced-networking/macvlan/01-macvlan-basics.md) for advanced networking.
