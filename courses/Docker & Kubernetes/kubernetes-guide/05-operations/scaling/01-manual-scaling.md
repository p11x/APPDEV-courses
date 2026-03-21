# Manual Scaling

## Overview

Manual scaling is the simplest way to adjust the number of pod replicas in a Deployment or StatefulSet. While not suitable for production workloads with variable traffic, it's useful for predictable scaling needs, testing, and understanding how Kubernetes scaling works.

## Prerequisites

- Understanding of Deployments and ReplicaSets
- Basic kubectl knowledge

## Core Concepts

### How Manual Scaling Works

The ReplicaSet controller ensures the desired number of pods are always running:
- If too few → creates new pods
- If too many → terminates excess pods
- If pod fails → creates replacement

### Why Manual Scaling Is Insufficient

- No response to traffic changes
- No cost optimization
- No automated handling of failures
- Manual intervention required

## Step-by-Step Examples

### Scaling a Deployment

```bash
# Scale to 3 replicas
# replicas=3 ensures exactly 3 pods running
kubectl scale deployment web-app --replicas=3

# Scale in namespace
# -n specifies namespace for non-default
kubectl scale deployment web-app --replicas=5 -n production

# Scale to 0 (stop application)
# Useful for batch jobs or scheduled workloads
kubectl scale deployment web-app --replicas=0
```

### Scaling via YAML

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-app
spec:
  replicas: 5    # Change this value
  selector:
    matchLabels:
      app: web
  template:
    metadata:
      labels:
        app: web
    spec:
      containers:
      - name: web
        image: nginx:1.25
```

```bash
# Apply updated YAML
kubectl apply -f deployment.yaml

# Or edit directly
kubectl edit deployment web-app
```

### Scaling StatefulSet

```bash
# Scale StatefulSet
# Maintains stable pod identity (pod-0, pod-1, etc.)
kubectl scale statefulset postgres --replicas=3 -n production
```

### Watching Scaling

```bash
# Watch pods scale up
# -w watches for changes in real-time
kubectl get pods -l app=web -w

# Check deployment status
# Shows: DESIRED, CURRENT, UP-TO-DATE, AVAILABLE
kubectl get deployment web-app

# Describe for details
# Includes: ReplicaSet info, events
kubectl describe deployment web-app
```

## Scaling Commands Reference

```bash
# Scale by name
kubectl scale deployment/DEPLOYMENT --replicas=N

# Scale by file
kubectl scale -f deployment.yaml --replicas=N

# Scale all deployments in namespace
kubectl scale deployment --all --replicas=1 -n production

# Scale based on current state
kubectl scale deployment/web-app --current-replicas=3 --replicas=5
```

## Gotchas for Docker Users

- **Declarative intent**: Unlike docker-compose up --scale, this sets desired state
- **Controller-managed**: Kubernetes maintains replica count automatically
- **Rolling updates**: Scaling during updates affects rollout behavior

## Common Mistakes

- **Forgetting namespace**: Scaling wrong deployment in wrong namespace
- **Scaling to 0**: Application becomes unavailable
- **Not checking resources**: Pods may fail if insufficient cluster resources

## Quick Reference

| Command | Description |
|---------|-------------|
| `kubectl scale deployment NAME --replicas=N` | Scale deployment |
| `kubectl scale statefulset NAME --replicas=N` | Scale StatefulSet |
| `kubectl get deployment NAME -o wide` | Check status |

## What's Next

Continue to [HPA](./02-hpa.md) for automatic scaling based on metrics.
