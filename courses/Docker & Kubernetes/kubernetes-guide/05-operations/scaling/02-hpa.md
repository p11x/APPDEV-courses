# Horizontal Pod Autoscaler

## Overview

The Horizontal Pod Autoscaler (HPA) automatically adjusts the number of pod replicas based on observed CPU utilization, memory usage, or custom metrics. This enables applications to handle variable traffic loads efficiently without manual intervention.

## Prerequisites

- Metrics Server installed on cluster
- Understanding of Deployments
- Resource requests defined on containers

## Core Concepts

### How HPA Works

1. Metrics Server collects resource metrics from kubelet
2. HPA controller calculates desired replica count
3. Deployment/ReplicaSet controller scales pods
4. Process repeats every 15 seconds (default)

### Metrics Collection

```bash
# Check if metrics server is running
# metrics-server collects pod/container resource usage
kubectl get pods -n kube-system -l k8s-app=metrics-server
```

### Scaling Behavior

- Scale up: Quick response to traffic spikes
- Scale down: Slower (default 5 minutes) to prevent flapping
- Don't scale below 1 for most deployments
- Don't scale above cluster capacity

## Step-by-Step Examples

### Creating HPA for CPU

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: web-app-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1    # Target resource type
    kind: Deployment       # Scale the deployment
    name: web-app          # Name of deployment to scale
  minReplicas: 2           # Minimum pods - never scale below
  maxReplicas: 10          # Maximum pods - cost protection
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70    # Scale when avg CPU > 70%
```

```bash
# Apply HPA
kubectl apply -f hpa-cpu.yaml

# Watch HPA status
kubectl get hpa web-app-hpa -w

# Example output:
# NAME         REFERENCE          TARGETS   MINPODS   MAXPODS   REPLICAS   AGE
# web-app-hpa  Deployment/web-app  45%/70%   2         10        3          5m
```

### Creating HPA for Memory

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: web-app-hpa-memory
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: web-app
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80    # Scale when avg memory > 80%
```

### Creating HPA with Both CPU and Memory

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: web-app-hpa-both
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: web-app
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80
  behavior:
    scaleDown:
      stabilizationWindowSeconds: 300    # Wait 5 min before scaling down
      policies:
      - type: Percent
        value: 10                         # Max 10% decrease at once
        periodSeconds: 60
    scaleUp:
      stabilizationWindowSeconds: 0      # Immediate scale up
      policies:
      - type: Percent
        value: 100                       # Can double pods
        periodSeconds: 15
```

### Creating via kubectl

```bash
# Create HPA using kubectl autoscale
# --cpu-percent=70 sets target CPU utilization
# --min=2 and --max=10 set replica bounds
kubectl autoscale deployment web-app \
  --cpu-percent=70 \
  --min=2 \
  --max=10

# Check created HPA
kubectl get hpa web-app

# Same with custom name
kubectl autoscale deployment web-app \
  --name=web-app-hpa \
  --cpu-percent=80 \
  --min=3 \
  --max=15
```

### Important: Resource Requests Required

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-app
spec:
  replicas: 2
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
            memory: "128Mi"    # Required for HPA
            cpu: "100m"        # Required for CPU-based HPA
          limits:
            memory: "256Mi"
            cpu: "500m"
```

### Checking HPA Status

```bash
# Get HPA with details
kubectl get hpa

# Full description with events
kubectl describe hpa web-app-hpa

# See current metrics
kubectl get hpa web-app-hpa -o yaml
```

### Testing HPA

```bash
# Create load generator
kubectl run -it --rm load-generator \
  --image=busybox:1.36 \
  -- /bin/sh

# Inside container, run load:
# wget -q -O- http://web-app-service/ &
# Run multiple times to increase load
while true; do wget -q -O- http://web-app-service/; done
```

## Common Mistakes

- **Missing resource requests**: HPA cannot calculate utilization without requests
- **Too aggressive scaling**: Causes pod churn and instability
- **Not setting min replicas**: Can scale to 0 and break application
- **Ignoring scale-down delay**: Causes flapping between replica counts

## Quick Reference

| Field | Description | Example |
|-------|-------------|---------|
| minReplicas | Minimum pods | 2 |
| maxReplicas | Maximum pods | 10 |
| averageUtilization | Target % | 70 |

| Command | Description |
|---------|-------------|
| `kubectl get hpa` | List HPAs |
| `kubectl describe hpa NAME` | Detailed info |
| `kubectl autoscale deployment NAME --cpu-percent=70 --min=2 --max=10` | Quick create |

## What's Next

Continue to [VPA and KEDA](./03-vpa-and-keda.md) for vertical scaling and event-driven autoscaling.
