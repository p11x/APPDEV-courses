# Resource Limits Basics

## Overview

Resource limits control how much CPU and memory containers can use. Proper limits prevent cost overruns and ensure fair resource distribution across workloads.

## Prerequisites

- Basic Kubernetes knowledge
- Understanding of CPU/memory units

## Core Concepts

### Resource Types

| Resource | Unit | Description |
|----------|------|-------------|
| CPU | cores | Compute processing power |
| Memory | bytes | RAM usage (Mi, Gi) |

### Request vs Limit

| Type | Purpose | Behavior |
|------|---------|----------|
| Request | Minimum guaranteed | Scheduler uses this |
| Limit | Maximum allowed | Throttled if exceeded |

## Step-by-Step Examples

### Set Container Limits

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: limited-pod
spec:
  containers:
  - name: app
    image: nginx:1.25
    resources:
      requests:
        memory: "128Mi"
        cpu: "250m"
      limits:
        memory: "256Mi"
        cpu: "500m"
```

### Deployment with Limits

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-deployment
spec:
  replicas: 3
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
        resources:
          requests:
            memory: "256Mi"
            cpu: "100m"
          limits:
            memory: "512Mi"
            cpu: "500m"
```

### Best Practice Template

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: production-app
spec:
  replicas: 3
  template:
    spec:
      containers:
      - name: app
        image: myapp:latest
        resources:
          # Start conservative, adjust based on metrics
          requests:
            memory: "128Mi"
            cpu: "100m"
          limits:
            memory: "256Mi"
            cpu: "200m"
```

## Gotchas for Docker Users

- **Similar to Docker**: `docker run --memory` and `--cpus`
- **OOMKilled**: Container killed if memory exceeded
- **CPU throttling**: Container throttled at limit

## Common Mistakes

- **No limits**: Unbounded resource usage
- **Too high**: Wasted resources
- **Too low**: Application crashes

## Quick Reference

| Suffix | Value |
|--------|-------|
| m | milli (1/1000) |
| Mi | mebibyte (1024^2) |
| Gi | gibibyte (1024^3) |
| M | megabyte (1000^2) |
| G | gigabyte (1000^3) |

## What's Next

Continue to [Cluster Autoscaling](./02-cluster-autoscaling.md) for dynamic scaling.
