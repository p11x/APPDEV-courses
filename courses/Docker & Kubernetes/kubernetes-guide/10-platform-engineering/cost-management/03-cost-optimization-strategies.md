# Cost Optimization Strategies

## Overview

Kubernetes cost optimization involves right-sizing resources, using spot instances, implementing autoscaling, and leveraging cloud provider cost management tools.

## Prerequisites

- Running Kubernetes cluster
- Cloud provider access

## Core Concepts

### Cost Factors

| Factor | Impact |
|--------|--------|
| Node compute | Primary cost driver |
| Storage | PVCs and snapshots |
| Network | Data transfer |
| Load balancers | Per-instance cost |

### Optimization Levers

- Right-sizing resources
- Autoscaling
- Spot/preemptible instances
- Reserved capacity

## Step-by-Step Examples

### Use Spot Nodes

```yaml
apiVersion:eks.amazonaws.com/v1alpha1
kind: NodeGroup
metadata:
  name: spot-nodes
spec:
  instanceTypes:
  - m5.large
  - m5.xlarge
  capacityType: SPOT
  scalingConfig:
    minSize: 1
    maxSize: 10
    desiredSize: 3
```

### Priority Classes for Spot

```yaml
apiVersion: scheduling.k8s.io/v1
kind: PriorityClass
metadata:
  name: spot-workload
value: -1
globalDefault: false
description: "For spot interruptible workloads"
```

### Resource Quota for Cost Control

```yaml
apiVersion: v1
kind: ResourceQuota
metadata:
  name: cost-control
  namespace: dev
spec:
  hard:
    requests.cpu: "10"
    limits.cpu: "20"
    requests.memory: 20Gi
    limits.memory: 40Gi
```

### Vertical Pod Autoscaler

```yaml
apiVersion: autoscaling.k8s.io/v1
kind: VerticalPodAutoscaler
metadata:
  name: myapp-vpa
spec:
  targetRef:
    apiVersion: "apps/v1"
    kind: Deployment
    name: myapp
  updatePolicy:
    updateMode: "Auto"
```

## Gotchas for Docker Users

- **No Docker equivalent**: Cost optimization is cloud/K8s-specific
- **Spot tolerance**: Workloads must handle interruptions
- **Metrics needed**: VPA requires metrics-server

## Common Mistakes

- **No monitoring**: Can't identify waste
- **Too aggressive**: Application instability
- **Reserved忽略**: Not using reserved instances

## Quick Reference

| Strategy | Savings Potential |
|----------|-------------------|
| Right-sizing | 20-40% |
| Spot instances | 60-90% |
| Reserved capacity | 30-60% |
| Autoscaling | 30-50% |

## What's Next

Continue to [Backup Strategies](./../disaster-recovery/01-backup-strategies.md) for disaster recovery.
