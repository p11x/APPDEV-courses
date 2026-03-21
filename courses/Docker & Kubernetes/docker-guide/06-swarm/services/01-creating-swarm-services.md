# Creating Swarm Services

## Overview

Docker Swarm services allow you to deploy and manage containerized applications across your cluster. This guide covers creating services, understanding replication modes, and publishing ports.

## Prerequisites

- Active Docker Swarm cluster with at least one manager
- Basic understanding of Docker containers

## Core Concepts

### Service vs Container

- **docker run**: Creates a single container on one host
- **docker service create**: Creates a service that can span multiple hosts with replication

### Service Components

A service definition includes:

- **Image**: The container image to run
- **Replicas**: Number of container instances
- **Ports**: Network ports to expose
- **Resources**: CPU and memory limits
- **Update policy**: How rolling updates work

## Step-by-Step Examples

### Creating a Basic Service

```bash
# Create a simple nginx service
# --name gives the service a name
# --replicas specifies number of instances
docker service create \
  --name my-nginx \
  --replicas 2 \
  nginx:1.25-alpine

# Example output:
# s123abc456
# overall progress: 2 running
```

### Creating a Service with Published Ports

```bash
# Create a service with port publishing
# --publish maps host port to container port
# mode=ingress (default) uses Swarm's routing mesh
docker service create \
  --name web-app \
  --replicas 3 \
  --publish published=8080,target=80 \
  nginx:1.25-alpine

# Another way (shorthand):
docker service create \
  --name web-app \
  --replicas 3 \
  -p 8080:80 \
  nginx:1.25-alpine
```

### Global Services

```bash
# Create a global service (one instance per node)
# Useful for logging, monitoring agents
docker service create \
  --name logging-agent \
  --mode global \
  --mount type=bind,source=/var/lib/docker/containers,target=/containers \
  fluentd:latest
```

### Viewing Services

```bash
# List all services
# Shows: ID, Name, Mode, Replicas, Image, Ports
docker service ls

# Example output:
# ID             NAME       MODE         REPLICAS   IMAGE          PORTS
# abc123         web-app    replicated   3/3        nginx:1.25     *:8080->80/tcp

# Inspect a service (detailed JSON)
docker service inspect my-nginx

# Pretty-printed format
docker service inspect my-nginx --pretty

# View service tasks (actual containers)
docker service ps my-nginx

# Example output:
# ID             NAME         IMAGE          NODE    DESIRED STATE  CURRENT STATE
# abc123def456   web-app.1    nginx:1.25     node1   Running         Running
# def456ghi789   web-app.2    nginx:1.25     node2   Running         Running
# ghi789jkl012   web-app.3    nginx:1.25     node3   Running         Running
```

### Service with Environment Variables

```bash
# Create service with environment variables
docker service create \
  --name api-service \
  --replicas 2 \
  -e NODE_ENV=production \
  -e DATABASE_URL=postgres://db:5432 \
  myapi:latest
```

### Service with Volume Mounts

```bash
# Create service with volume mount
# type=bind mounts host directory into container
docker service create \
  --name data-service \
  --replicas 1 \
  --mount type=bind,source=/data,target=/app/data \
  myapp:latest
```

### Service with Constraints

```bash
# Constrain service to specific nodes
# node==worker1 runs only on worker1
docker service create \
  --name database \
  --replicas 1 \
  --constraint 'node.hostname==worker1' \
  postgres:16-alpine
```

## The Routing Mesh

### How It Works

Swarm's routing mesh provides:

- **Port-based routing**: Incoming traffic on published ports is routed to any container
- **Load balancing**: Requests are distributed across replicas
- **Service discovery**: Containers can reach each other by service name

### Accessing Services

```bash
# Access from any swarm node
# Routing mesh forwards to any replica
curl http://localhost:8080

# Access from within the swarm
# Use service name for DNS resolution
docker exec -it container curl http://web-app:80
```

## Common Mistakes

- **Port conflicts**: Two services can't publish the same host port
- **Wrong constraint syntax**: Must use 'node.hostname' not just 'hostname'
- **Missing replicas**: Default is 1, might need more for HA
- **Network issues**: Make sure services are on same overlay network

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker service create` | Create a new service |
| `docker service ls` | List services |
| `docker service ps NAME` | List service tasks |
| `docker service inspect NAME` | Show service details |
| `docker service rm NAME` | Remove a service |

| Flag | Description |
|------|-------------|
| `--name` | Service name |
| `--replicas` | Number of replicas |
| `-p, --publish` | Port mapping |
| `-e, --env` | Environment variable |
| `--constraint` | Node constraints |
| `--mode global` | One per node |

## What's Next

Continue to [Scaling and Updates](./02-scaling-and-updates.md) to learn about managing service lifecycles.
