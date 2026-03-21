# Service Discovery Patterns

## Overview

Service discovery enables containers to find each other dynamically. This guide covers the various approaches available in Docker, from simple DNS to external service registries.

## Prerequisites

- Understanding of Docker networking
- Basic knowledge of service-oriented architecture

## Core Concepts

### Service Discovery Methods

| Method | Complexity | Features | Use Case |
|--------|------------|----------|----------|
| Docker DNS | Low | Name resolution | Simple apps |
| Environment variables | Low | Legacy support | Compose |
| Consul/etcd | Medium | Dynamic registration | Complex |
| Swarm services | Low | Built-in | Swarm deployments |

## Step-by-Step Examples

### Docker DNS (Recommended)

```bash
# Create network
docker network create app-net

# Start services on same network
docker run -d --network app-net --name api myapi
docker run -d --network app-net --name web myweb

# DNS automatically resolves names
# web can reach api at http://api:8080
```

### Docker Compose Service Discovery

```yaml
# docker-compose.yml
version: "3.8"

services:
  web:
    image: myweb
    ports:
      - "8080:80"
    depends_on:
      - api
    environment:
      # Legacy: Hostnames injected as env vars
      - API_URL=http://api:8080

  api:
    image: myapi
    ports:
      - "8081:8080"
```

### Environment Variables

```bash
# Docker Compose injects environment variables
# Format: <NAME>_SERVICE_HOST=<IP>
# Example: API_SERVICE_HOST=172.18.0.2

# Legacy links (deprecated)
docker run --link db:mydb myapp
```

### Using Consul for Service Discovery

```bash
# Start Consul for service registry
docker run -d \
  --name consul \
  -p 8500:8500 \
  consul agent -server -ui -bootstrap-expect=1 \
  -client=0.0.0.0

# Register service usingregistrator
docker run -d \
  --name registrator \
  --network host \
  -v /var/run/docker.sock:/tmp/docker.sock \
  gliderlabs/registrator:latest \
  consul://localhost:8500

# Now services auto-register when started
```

### Swarm Service Discovery

```bash
# In Swarm mode, DNS is built-in
docker service create --name api myapi
docker service create --name web myweb

# Web can reach api via DNS: http://api:8080
```

## Comparison

| Method | Pros | Cons |
|--------|------|------|
| Docker DNS | Simple, built-in | Single host/network |
| Compose env | Easy | Container IP changes break |
| Consul/etcd | Dynamic, scalable | Complex setup |
| Swarm | Native, scalable | Requires Swarm |

## Gotchas for Docker Users

- **Links deprecated**: Don't use --link in modern Docker
- **Network isolation**: Must be on same network
- **Container restarts**: IPs change, use DNS

## Common Mistakes

- **Default bridge**: Doesn't support DNS discovery
- **Network mismatch**: Containers on different networks can't communicate
- **Hardcoded IPs**: Never hardcode container IPs

## Quick Reference

| Method | Setup | Best For |
|--------|-------|----------|
| DNS | --network | Simple apps |
| Compose | docker-compose.yml | Development |
| Consul | External service | Production |
| Swarm | docker service create | Orchestration |

## What's Next

Continue to [Reducing Image Size](../../08-performance/image-optimisation/01-reducing-image-size.md) for performance optimization.
