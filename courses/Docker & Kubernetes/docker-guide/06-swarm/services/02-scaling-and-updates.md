# Scaling and Updates

## Overview

Managing service lifecycles in Docker Swarm involves scaling replica counts and performing rolling updates. This guide covers both operations with their configuration options.

## Prerequisites

- Active Docker Swarm cluster
- Existing service created

## Core Concepts

### Scaling Services

Scaling adjusts the number of replica containers running your service:

- **Scale up**: Add more replicas for capacity
- **Scale down**: Reduce replicas to save resources
- Swarm automatically redistributes replicas across nodes

### Rolling Updates

Swarm supports zero-downtime updates:

- Updates happen incrementally
- Old containers are replaced one at a time
- Health checks can gate the update process
- Rollback is available if things go wrong

## Step-by-Step Examples

### Scaling a Service

```bash
# Scale service to 5 replicas
# Simple command to adjust replica count
docker service scale my-nginx=5

# Or using the explicit flag
docker service update --replicas 5 my-nginx

# Scale multiple services at once
docker service scale web=5 api=3 worker=10

# Example output:
# web scaled to 5
# api scaled to 3
# worker scaled to 10
```

### Viewing Scaling Progress

```bash
# Check current replica status
docker service ls

# Watch replica count change
docker service ps my-nginx

# Example output:
# ID             NAME           IMAGE          NODE   DESIRED STATE  CURRENT STATE
# abc123         my-nginx.1     nginx:1.25     node1  Running        Running
# def456         my-nginx.2     nginx:1.25     node2  Running        Running
# ghi789         my-nginx.3     nginx:1.25     node1  Running        Running
```

### Performing Rolling Updates

```bash
# Update a service's image
# --update-delay specifies time between updates
docker service update \
  --image nginx:1.26 \
  --update-delay 10s \
  my-nginx

# Update with parallelism
# --update-parallelism controls simultaneous updates
docker service update \
  --image nginx:1.26 \
  --update-parallelism 2 \
  --update-delay 5s \
  my-nginx
```

### Configuring Update Behavior

```bash
# Complete update configuration
docker service update \
  --image myapp:v2.0 \
  --update-delay 15s \         # 15 seconds between updates
  --update-parallelism 1 \    # Update 1 at a time
  --update-failure-action continue \  # Continue on failure
  --rollback-monitor 20s \    # Monitor rollback for 20s
  --rollback-parallelism 0 \  # Rollback all at once
  my-app
```

### Update Failure Actions

```bash
# On update failure, pause the update
docker service update \
  --image myapp:broken \
  --update-failure-action pause \
  my-app

# On update failure, rollback automatically
docker service update \
  --image myapp:v2.0 \
  --update-failure-action rollback \
  my-app
```

### Rolling Back

```bash
# Rollback to previous version
docker service rollback my-nginx

# Rollback with custom settings
docker service rollback \
  --update-delay 5s \
  --update-parallelism 1 \
  my-nginx

# Check rollback status
docker service ps my-nginx

# Example output shows ROLLBACK:
# ID             NAME            IMAGE       NODE   DESIRED STATE  CURRENT STATE
# xyz789         my-nginx.1      nginx:1.25  node1  Running        Running
# ... (shows rollback tasks)
```

### Health-Aware Updates

```bash
# Create service with health check
docker service create \
  --name web-with-health \
  --replicas 3 \
  --health-cmd "curl -f http://localhost/ || exit 1" \
  --health-interval 10s \
  --health-timeout 5s \
  --health-retries 3 \
  nginx:1.25

# Update uses health checks by default
# Service waits for container to be healthy before proceeding
docker service update --image nginx:1.26 web-with-health
```

## Update Configuration Parameters

| Flag | Description | Default |
|------|-------------|---------|
| `--update-delay` | Delay between updates | 0s |
| `--update-parallelism` | Concurrent updates | 1 |
| `--update-failure-action` | Action on failure | pause |
| `--rollback-monitor` | Monitor after rollback | 20s |
| `--rollback-parallelism` | Parallelism during rollback | 1 |

### Failure Actions

- **continue**: Continue updating other replicas
- **pause**: Pause the update
- **rollback**: Automatically rollback

## Gotchas for Docker Users

- **No auto-scaling**: Swarm doesn't auto-scale based on metrics (unlike Kubernetes HPA)
- **Rolling back vs updating**: Rollback returns to previous, update changes forward
- **Update order**: Updates start from replica 1, not randomly

## Common Mistakes

- **Parallelism too high**: Updating too many replicas causes downtime
- **No delay**: Zero delay can cause issues with stateful services
- **Forgetting health checks**: Without health checks, Swarm trusts container is running
- **Rollback confusion**: Rollback goes to previous state, not a specific version

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker service scale NAME=N` | Scale to N replicas |
| `docker service update --replicas N` | Update replica count |
| `docker service update --image TAG` | Update container image |
| `docker service rollback NAME` | Rollback to previous |
| `docker service ps NAME` | View task status |

| Update Flag | Purpose |
|-------------|---------|
| `--update-delay` | Time between updates |
| `--update-parallelism` | Simultaneous updates |
| `--update-failure-action` | What to do on failure |

## What's Next

Continue to [Service Constraints](./03-service-constraints.md) to learn about placement control.
