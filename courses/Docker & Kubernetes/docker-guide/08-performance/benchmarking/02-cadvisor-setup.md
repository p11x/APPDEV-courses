# cAdvisor Setup

## Overview

cAdvisor (Container Advisor) provides container metrics and performance analysis. It collects resource usage from all running containers and exposes metrics in Prometheus format.

## Prerequisites

- Docker Engine 20.10+
- Understanding of Prometheus

## Core Concepts

### What cAdvisor Provides

- CPU, memory, disk, network metrics
- Historical data (within container)
- Web UI for visualization
- Prometheus metrics endpoint

## Step-by-Step Examples

### Running cAdvisor

```bash
# Run cAdvisor with required volumes
# --privileged gives access to all devices
# Mounts: /var/run for runtime, /sys for system, /var/lib for data
docker run -d \
  --name cadvisor \
  --privileged \
  -p 8080:8080 \
  --volume=/var/run:/var/run:ro \
  --volume=/sys:/sys:ro \
  --volume=/var/lib/docker/:/var/lib/docker:ro \
  gcr.io/cadvisor/cadvisor:latest

# Access web UI
# Open http://localhost:8080
```

### Prometheus Integration

```yaml
# prometheus.yml scrape config
scrape_configs:
  - job_name: 'cadvisor'
    static_configs:
      - targets: ['cadvisor:8080']
```

### Key Metrics

| Metric | Description |
|--------|-------------|
| container_cpu_usage_seconds_total | Cumulative CPU time |
| container_memory_working_set_bytes | Memory working set |
| container_fs_reads_bytes_total | Disk reads |
| container_network_receive_bytes_total | Network RX |
| container_network_transmit_bytes_total | Network TX |

## Gotchas for Docker Users

- **Resource usage**: cAdvisor itself uses resources
- **Data persistence**: Metrics lost on container restart
- **Privileged mode**: Required for full metrics

## Quick Reference

| Port | Service |
|------|---------|
| 8080 | Web UI |
| 8080/metrics | Prometheus endpoint |

## What's Next

Continue to [Profiling Containers](./03-profiling-containers.md) for performance analysis.
