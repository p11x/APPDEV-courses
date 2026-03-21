# Pod Topology Spread

## Overview

Pod topology spread ensures that pods are evenly distributed across failure domains (nodes, zones, regions). This improves availability and resource utilization.

## Prerequisites

- Kubernetes 1.19+
- Understanding of pods and deployments

## Core Concepts

### Why Topology Spread

- **High availability**: Spreads pods across failure domains
- **Zone redundancy**: Survives zone failures
- **Resource balance**: Evenly distributes load

### Key Fields

| Field | Description |
|-------|-------------|
| maxSkew | Maximum allowed imbalance |
| topologyKey | Label key defining the topology |
| whenUnsatisfiable | Action when constraint can't be met |
| labelSelector | Which pods to count |

## Step-by-Step Examples

### Spread Across Zones

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-zone-spread
spec:
  replicas: 6
  selector:
    matchLabels:
      app: web
  template:
    metadata:
      labels:
        app: web
    spec:
      topologySpreadConstraints:
      - maxSkew: 1
        topologyKey: topology.kubernetes.io/zone
        whenUnsatisfiable: DoNotSchedule
        labelSelector:
          matchLabels:
            app: web
      containers:
      - name: web
        image: nginx:1.25
```

### Multiple Constraints

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-multi-spread
spec:
  replicas: 10
  selector:
    matchLabels:
      app: web
  template:
    metadata:
      labels:
        app: web
    spec:
      topologySpreadConstraints:
      # Spread across zones
      - maxSkew: 1
        topologyKey: topology.kubernetes.io/zone
        whenUnsatisfiable: DoNotSchedule
        labelSelector:
          matchLabels:
            app: web
      # Also spread across nodes
      - maxSkew: 2
        topologyKey: kubernetes.io/hostname
        whenUnsatisfiable: ScheduleAnyway
        labelSelector:
          matchLabels:
            app: web
      containers:
      - name: web
        image: nginx
```

### Using matchLabelKeys (K8s 1.27+)

```yaml
# matchLabelKeys automatically uses pod's own labels
apiVersion: apps/v1
kind: Deployment
metadata:
  name: app-with-keys
spec:
  replicas: 5
  selector:
    matchLabels:
      app: myapp
  template:
    metadata:
      labels:
        app: myapp
        version: v1
    spec:
      topologySpreadConstraints:
      - maxSkew: 1
        topologyKey: topology.kubernetes.io/zone
        whenUnsatisfiable: DoNotSchedule
        labelSelector:
          matchLabels:
            app: myapp
        matchLabelKeys:
          - version
      containers:
      - name: app
        image: myapp:v1
```

## Gotchas for Docker Users

- **No Docker equivalent**: This is Kubernetes-specific scheduling
- **K8s version**: Some features require 1.19+
- **minDomains**: Requires K8s 1.25+

## Common Mistakes

- **Too high skew**: Pods may not be schedulable
- **Conflicting constraints**: Multiple constraints can conflict
- **Missing labels**: Selector must match pod labels

## Quick Reference

| Field | Purpose |
|-------|---------|
| maxSkew | Maximum imbalance allowed |
| topologyKey | Label defining topology domain |
| whenUnsatisfiable | DoNotSchedule or ScheduleAnyway |
| labelSelector | Which pods to consider |

| whenUnsatisfiable | Behavior |
|-------------------|-----------|
| DoNotSchedule | Don't schedule if can't meet |
| ScheduleAnyway | Schedule even if can't meet |

## What's Next

Continue to [Pod Security Standards](./../security/01-pod-security-standards.md) for security.
