# Service Constraints

## Overview

Service constraints control where Swarm schedules your containers. This guide covers using constraints for node-specific placement, resource reservation, and affinity rules.

## Prerequisites

- Active Docker Swarm cluster
- Understanding of node labeling
- Service creation basics

## Core Concepts

### Constraint Types

- **Node attributes**: Hostname, OS, architecture
- **Labels**: Custom labels added to nodes
- **Engine labels**: Docker Engine labels automatically applied

### Spread Strategies

Swarm uses spread scheduling by default:

- Distributes replicas evenly across nodes
- Minimizes the impact of node failure
- Can be customized with preferences

## Step-by-Step Examples

### Using Node Hostname Constraints

```bash
# Run service only on specific node
# Constrains to exact hostname
docker service create \
  --name database \
  --constraint 'node.hostname==db-node-1' \
  postgres:16-alpine

# Avoid running on specific node
docker service create \
  --name web \
  --constraint 'node.hostname!=worker-3' \
  nginx:1.25-alpine
```

### Using Node Labels

```bash
# First, add labels to nodes
# Add SSD storage label
docker node update --label-add storage=ssd worker-1

# Add environment label
docker node update --label-add environment=production worker-1

# Now constrain service to SSD nodes
docker service create \
  --name fast-api \
  --constraint 'node.labels.storage==ssd' \
  myapi:latest

# Multiple constraints (AND logic)
docker service create \
  --name prod-app \
  --constraint 'node.labels.environment==production' \
  --constraint 'node.labels.storage==ssd' \
  myapp:latest
```

### Using Role Constraints

```bash
# Run only on manager nodes
# Useful for control plane services
docker service create \
  --name registry \
  --constraint 'node.role==manager' \
  --publish 5000:5000 \
  registry:2

# Run only on worker nodes
docker service create \
  --name worker \
  --constraint 'node.role==worker' \
  myworker:latest
```

### Resource Reservation

```bash
# Reserve CPU and memory
# Guarantees resources are available
docker service create \
  --name api \
  --reserve-cpu 0.5 \
  --reserve-memory 512m \
  myapi:latest

# Set both reservation and limit
docker service create \
  --name web \
  --reserve-cpu 0.25 \
  --reserve-memory 256m \
  --limit-cpu 1.0 \
  --limit-memory 1g \
  nginx:1.25
```

### Spread Preferences

```bash
# Prefer running on nodes with more memory
# --prefer distributes across nodes preferring the constraint
docker service create \
  --name worker \
  --replicas 3 \
  --reserve-memory 512m \
  --placement-pref 'spread=node.labels.memory' \
  myworker:latest
```

### Constraint Examples for Databases

```bash
# Database requiring SSD storage
docker service create \
  --name postgres-db \
  --replicas 1 \
  --constraint 'node.labels.storage==ssd' \
  --constraint 'node.labels.tier==database' \
  --reserve-memory 2g \
  --reserve-cpu 1.0 \
  postgres:16-alpine

# Redis cache with memory requirements
docker service create \
  --name redis-cache \
  --replicas 2 \
  --constraint 'node.labels.type==memory' \
  --reserve-memory 4g \
  redis:7-alpine
```

## Constraint Syntax

| Constraint | Description |
|-----------|-------------|
| `node.hostname==value` | Match hostname |
| `node.hostname!=value` | Exclude hostname |
| `node.labels.key==value` | Match node label |
| `node.labels.key!=value` | Exclude node label |
| `node.role==manager` | Manager nodes only |
| `node.role==worker` | Worker nodes only |
| `engine.labels.label` | Docker Engine labels |

## Gotchas for Docker Users

- **Scheduling delays**: Impossible constraints cause pending tasks
- **Node failure**: If constrained node fails, service may go down
- **Resource conflicts**: Multiple constrained services compete for same nodes

## Common Mistakes

- **Too restrictive**: All replicas on one node defeats HA
- **Forgetting constraints**: Service might land on wrong nodes
- **Resource reservation too high**: Nodes may not have required resources

## Quick Reference

| Command | Description |
|---------|-------------|
| `--constraint 'node.hostname==NAME'` | Match hostname |
| `--constraint 'node.labels.KEY==VALUE'` | Match label |
| `--constraint 'node.role==manager'` | Manager only |
| `--reserve-cpu N` | Reserve CPU |
| `--reserve-memory SIZE` | Reserve memory |
| `--placement-pref` | Spread preference |

## What's Next

Continue to [Deploy with Stacks](../stacks/01-deploy-with-stacks.md) to learn about deploying multi-service applications.
