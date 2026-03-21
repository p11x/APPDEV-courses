# Node Affinity

## Overview

Node affinity allows you to constrain which nodes your pods can be scheduled on based on node labels. It provides more flexible control than taints and tolerations.

## Prerequisites

- Kubernetes basics
- Understanding of node labels

## Core Concepts

### Types of Affinity

| Type | Description |
|------|-------------|
| requiredDuringSchedulingIgnoredDuringExecution | Hard requirement (must match) |
| preferredDuringSchedulingIgnoredDuringExecution | Soft preference (try to match) |

### Operators

| Operator | Description |
|----------|-------------|
| In | Label value must be in list |
| NotIn | Label value must not be in list |
| Exists | Label must exist |
| DoesNotExist | Label must not exist |
| Gt | Label value greater than |
| Lt | Label value less than |

## Step-by-Step Examples

### Required Node Affinity

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-app
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
      affinity:
        nodeAffinity:
          requiredDuringSchedulingIgnoredDuringExecution:
            nodeSelectorTerms:
            - matchExpressions:
              - key: disktype
                operator: In
                values:
                - ssd
              - key: zone
                operator: In
                values:
                - us-east-1a
      containers:
      - name: web
        image: nginx:1.25
```

### Preferred Node Affinity

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: app
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
      affinity:
        nodeAffinity:
          preferredDuringSchedulingIgnoredDuringExecution:
          - weight: 1
            preference:
              matchExpressions:
              - key: zone
                operator: In
                values:
                - us-east-1a
      containers:
      - name: app
        image: myapp:latest
```

### Inter-Pod Affinity

```yaml
# Co-locate web and redis pods
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-cache
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
      affinity:
        podAffinity:
          requiredDuringSchedulingIgnoredDuringExecution:
          - labelSelector:
              matchLabels:
                app: redis
            topologyKey: kubernetes.io/hostname
      containers:
      - name: web
        image: nginx
```

### Pod Anti-Affinity

```yaml
# Spread pods across zones
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-distributed
spec:
  replicas: 5
  selector:
    matchLabels:
      app: web
  template:
    metadata:
      labels:
        app: web
    spec:
      affinity:
        podAntiAffinity:
          requiredDuringSchedulingIgnoredDuringExecution:
          - labelSelector:
              matchLabels:
                app: web
            topologyKey: topology.kubernetes.io/zone
      containers:
      - name: web
        image: nginx
```

## Gotchas for Docker Users

- **No Docker equivalent**: Node affinity is Kubernetes-specific
- **Label requirement**: Nodes must have the specified labels
- **Affinity vs taints**: Affinity attracts, taints repel

## Common Mistakes

- **Missing labels**: Nodes don't have required labels
- **Too restrictive**: Pods can't be scheduled anywhere
- **Weight range**: Must be 1-100

## Quick Reference

| Affinity Type | Behavior |
|--------------|----------|
| required | Must match |
| preferred | Try to match |

| Operator | Matches |
|----------|----------|
| In | Value in list |
| NotIn | Value not in list |
| Exists | Key exists |
| DoesNotExist | Key missing |

## What's Next

Continue to [Pod Topology Spread](./03-pod-topology-spread.md) for spreading workloads.
