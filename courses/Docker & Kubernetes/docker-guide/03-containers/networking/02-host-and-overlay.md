# Host and Overlay Networks

## Overview

Beyond the bridge network, Docker provides host and overlay network drivers for different use cases. Host networking removes network isolation between container and host, useful for specific performance scenarios. Overlay networks enable container communication across multiple Docker hosts, essential for swarm mode and multi-host deployments.

## Prerequisites

- Understanding of bridge networking
- Basic networking knowledge (IP, ports, DNS)
- Familiarity with Docker concepts

## Core Concepts

### Host Network

When using host network mode, the container shares the host's network namespace:

- No network isolation between container and host
- Container uses host's IP directly
- No NAT or port mapping needed
- Better performance (no overhead)

Use cases:
- Network performance-critical applications
- Containers that need full host network access
- Kubernetes pods (uses host networking concept)

### Overlay Network

Overlay networks enable containers on different hosts to communicate:

- Works with Docker Swarm or external key-value store
- Creates a distributed network across multiple hosts
- Each container gets an IP on the overlay network
- Built-in service discovery

Use cases:
- Multi-host Docker deployments
- Docker Swarm services
- Microservices spanning multiple machines

## Step-by-Step Examples

### Host Network

```bash
# Run container on host network
# --network host removes network isolation
docker run -d --network host nginx

# Check container's network (same as host)
docker exec container_name ip addr

# No port mapping needed - nginx uses port 80 directly
# Access at http://localhost:80

# Note: Can't run multiple containers on host network
# with same port (would conflict)
```

### Overlay Network (Swarm Mode)

```bash
# Initialize Docker Swarm (if not already done)
docker swarm init

# Create an overlay network
# --driver overlay enables multi-host networking
docker network create \
  --driver overlay \
  --attachable \
  my-overlay-network

# Run containers on overlay network
docker run -d --name service1 --network my-overlay-network nginx
docker run -d --name service2 --network my-overlay-network alpine

# Containers on different hosts can communicate
# (if joined to the same overlay network)

# Create a service in swarm mode
docker service create \
  --name my-service \
  --network my-overlay-network \
  --replicas 3 \
  nginx

# Inspect overlay network
docker network inspect my-overlay-network
```

### Using with Docker Compose

```yaml
# docker-compose.yml
version: "3.8"

services:
  web:
    image: nginx
    networks:
      - frontend-network
      - backend-network
  api:
    image: myapi
    networks:
      - backend-network

networks:
  frontend-network:
    driver: bridge
  backend-network:
    driver: bridge
```

For overlay (Swarm):

```yaml
version: "3.8"

services:
  web:
    image: nginx
    deploy:
      replicas: 3
    networks:
      - my-overlay

networks:
  my-overlay:
    driver: overlay
```

### Advanced: macvlan Network

macvlan gives containers their own MAC address:

```bash
# Create macvlan network
docker network create \
  --driver macvlan \
  --subnet=192.168.1.0/24 \
  --gateway=192.168.1.1 \
  -o parent=eth0 \
  my-macvlan

# Run container with macvlan
docker run -d --network my-macvlan nginx

# Container appears as real device on network
```

## Common Mistakes

- **Using host network in production**: Loses container isolation, security risk.
- **Port conflicts on host network**: Can't run multiple containers on same port.
- **Overlay without Swarm**: Overlay requires Swarm mode or external kv-store.
- **Network driver confusion**: Choose right driver for your use case.
- **Forgetting network cleanup**: Remove unused networks to avoid confusion.

## Quick Reference

| Driver | Use Case |
|--------|----------|
| bridge | Single host, default |
| host | Performance-critical, no isolation |
| overlay | Multi-host, Swarm |
| macvlan | Containers as physical devices |
| none | No networking |

| Command | Description |
|---------|-------------|
| `--network host` | Use host network |
| `--driver overlay` | Overlay network |
| `--driver macvlan` | macvlan network |
| `--attachable` | Allow non-swarm containers |

## What's Next

Now that you understand host and overlay networks, continue to [Container DNS](./03-container-dns.md) to learn how DNS resolution works in Docker containers.
