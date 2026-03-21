# Taints and Tolerations

## Overview

Taints and tolerations work together to ensure that pods are scheduled to appropriate nodes. A taint marks a node to repel pods, and a toleration allows a pod to be scheduled on nodes with matching taints.

## Prerequisites

- Kubernetes cluster access
- kubectl basics
- Understanding of pods and scheduling

## Core Concepts

### How They Work

- **Taint**: Applied to a node to repel pods
- **Toleration**: Applied to a pod to allow scheduling on tainted nodes

### Taint Effects

| Effect | Description |
|--------|-------------|
| NoSchedule | Pods without matching toleration won't be scheduled |
| PreferNoSchedule | Scheduler tries to avoid placing pods, but not forced |
| NoExecute | Evicts existing pods without matching toleration |

## Step-by-Step Examples

### Adding a Taint to a Node

```bash
# Taint a node with key=value and NoSchedule effect
# key=value:taint-effect — colon separates value from effect
kubectl taint nodes node1 \
  gpu=true:NoSchedule \
  --overwrite

# Taint with NoExecute effect (evicts existing pods)
kubectl taint nodes node1 \
  disk=ssd:NoExecute \
  --overwrite

# Remove a taint (trailing minus removes it)
kubectl taint nodes node1 gpu=true:NoSchedule-
```

### Adding Tolerations to a Pod

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: gpu-workload
spec:
  tolerations:
  # Match the taint exactly
  - key: gpu
    operator: Equal
    value: "true"
    effect: NoSchedule
  containers:
  - name: cuda
    image: nvidia/cuda:11.0-base
```

### Toleration with Time Delay

```yaml
# tolerationSeconds controls how long before eviction
apiVersion: v1
kind: Pod
metadata:
  name: tolerates-taint
spec:
  tolerations:
  - key: disk
    operator: Equal
    value: ssd
    effect: NoExecute
    tolerationSeconds: 300  # Pod can stay 5 minutes before eviction
```

### Complete Example: GPU Node

```bash
# Step 1: Taint the GPU node
kubectl taint nodes gpu-node-1 \
  nvidia.com/gpu=present:NoSchedule

# Step 2: Deploy GPU workload with toleration
kubectl apply -f - <<EOF
apiVersion: v1
kind: Pod
metadata:
  name: ml-training
spec:
  tolerations:
  - key: nvidia.com/gpu
    operator: Exists
    effect: NoSchedule
  containers:
  - name: train
    image: tensorflow/tensorflow:latest-gpu
EOF
```

## Gotchas for Docker Users

- **No Docker equivalent**: Taints and tolerations are Kubernetes-specific
- **One-way repulsion**: Taints repel pods, not the other way around
- **Eviction**: NoExecute effect can evict running pods

## Common Mistakes

- **Forgetting tolerations**: Pod won't schedule without matching toleration
- **Wrong effect**: Using NoSchedule instead of PreferNoSchedule
- **Key mismatch**: Toleration key must match exactly

## Quick Reference

| Taint Command | Description |
|--------------|-------------|
| kubectl taint nodes NODE KEY=VALUE:EFFECT | Add taint |
| kubectl taint nodes NODE KEY- | Remove taint |

| Effect | Behavior |
|--------|----------|
| NoSchedule | No new pods scheduled |
| PreferNoSchedule | Avoids scheduling |
| NoExecute | Evicts existing pods |

| Toleration Field | Purpose |
|-----------------|---------|
| key | Match taint key |
| operator | Equal or Exists |
| value | Match taint value |
| effect | Match taint effect |
| tolerationSeconds | Delay before eviction |

## What's Next

Continue to [Node Affinity](./02-node-affinity.md) for more advanced scheduling options.
