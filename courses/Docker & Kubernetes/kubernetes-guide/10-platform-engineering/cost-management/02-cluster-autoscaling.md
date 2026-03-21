# Cluster Autoscaling

## Overview

Cluster autoscaler automatically adjusts the number of nodes in a cluster based on pending pods and resource utilization. It scales up when pods can't be scheduled and scales down when nodes are underutilized.

## Prerequisites

- Cloud provider cluster (GKE, EKS, AKS)
- Autoscaler enabled on node pools

## Core Concepts

### How It Works

| Phase | Trigger | Action |
|-------|---------|--------|
| Scale up | Pods pending | Add nodes |
| Scale down | Low utilization | Remove nodes |

### Scale-Up Conditions

- Pods unschedulable due to resource shortage
- New pods created that can't fit

### Scale-Down Conditions

- Node underutilized for extended time
- No disruptive pods affected

## Step-by-Step Examples

### GKE Cluster Autoscaler

```bash
# Enable autoscaler on node pool
gcloud container node-pools update pool-default \
  --cluster my-cluster \
  --enable-autoscaling \
  --min-nodes 1 \
  --max-nodes 5
```

### EKS Auto Scaling Group

```bash
# Update ASG for cluster autoscaler
aws autoscaling update-auto-scaling-group \
  --auto-scaling-group-name my-cluster-nodes \
  --min-size 1 \
  --max-size 5 \
  --desired-capacity 2
```

### Pod Resource Requests

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: scalable-app
spec:
  replicas: 3
  selector:
    matchLabels:
      app: myapp
  template:
    metadata:
      labels:
        app: myapp
    spec:
      containers:
      - name: app
        image: myapp:latest
        resources:
          requests:
            # Important: requests trigger scale-up
            memory: "512Mi"
            cpu: "500m"
          limits:
            memory: "1Gi"
            cpu: "1000m"
```

## Gotchas for Docker Users

- **No Docker equivalent**: Cluster-level autoscaling is Kubernetes-specific
- **Cloud-specific**: Different on each cloud provider
- **Scale-down delay**: Takes time to scale down

## Common Mistakes

- **No requests**: Autoscaler can't calculate needed resources
- **Too aggressive**: Constant scaling
- **Min=max**: No room to scale

## Quick Reference

| Provider | Tool |
|----------|------|
| GKE | Built-in |
| EKS | Cluster Autoscaler |
| AKS | Built-in |

| Scaling Type | Scope |
|--------------|-------|
| Horizontal Pod Autoscaler | Pods |
| Vertical Pod Autoscaler | Pod resources |
| Cluster Autoscaler | Nodes |

## What's Next

Continue to [Cost Optimization Strategies](./03-cost-optimization-strategies.md) for overall cost reduction.
