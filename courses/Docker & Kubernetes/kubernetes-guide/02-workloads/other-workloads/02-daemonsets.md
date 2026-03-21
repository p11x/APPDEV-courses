# DaemonSets

## Overview

DaemonSets ensure that a specific pod runs on every node (or a subset of nodes) in the cluster. Unlike Deployments which scale pods arbitrarily, DaemonSets guarantee one pod instance per node, making them perfect for infrastructure-level tasks like log collectors, monitoring agents, and network plugins.

## Prerequisites

- Understanding of Kubernetes Pods
- Basic knowledge of node scheduling concepts
- Familiarity with node labels and taints/tolerations

## Core Concepts

### How DaemonSets Work

DaemonSet controller:
- Detects new nodes added to cluster
- Creates pod instances on each matching node
- Ensures pod continues to run (reschedules if node fails)
- Removes pods when nodes are removed from cluster

### Why Not Just Use a Deployment?

Deployments aim for a specific replica count across the cluster, but:
- Don't guarantee one pod per node
- May schedule multiple pods on powerful nodes
- May not schedule on underutilized nodes

DaemonSets guarantee:
- Exactly one pod per matching node
- Automatic scheduling on new nodes
- Immediate rescheduling on node failure

### Use Cases

- **Log collectors**: Fluentd, Logstash, filebeat
- **Monitoring agents**: Prometheus node exporter, Datadog agent
- **Network plugins**: CNI plugins (Calico, Cilium)
- **Storage agents**: Ceph OSD, clusterfs
- **Cluster services**: DNS servers, ingress controllers

## Step-by-Step Examples

### Creating a DaemonSet

```yaml
apiVersion: apps/v1
kind: DaemonSet
metadata:
  name: node-exporter                      # Unique name within namespace
  labels:
    app: node-exporter
spec:
  selector:
    matchLabels:
      app: node-exporter                   # Must match template labels
  template:
    metadata:
      labels:
        app: node-exporter
    spec:
      containers:
      - name: node-exporter
        image: prom/node-exporter:v1.7.0
        ports:
        - containerPort: 9100
          name: metrics
      tolerations:
      - key: node-role.kubernetes.io/control-plane  # Tolerates control-plane taint
        operator: Exists
        effect: NoSchedule
      - key: node-role.kubernetes.io/master         # Legacy taint for older clusters
        operator: Exists
        effect: NoSchedule
```

### DaemonSet with Node Selector

```yaml
apiVersion: apps/v1
kind: DaemonSet
metadata:
  name: ssd-monitor
spec:
  selector:
    matchLabels:
      app: ssd-monitor
  template:
    metadata:
      labels:
        app: ssd-monitor
    spec:
      nodeSelector:                        # Only run on nodes with this label
        disktype: ssd
      containers:
      - name: monitor
        image: ssd-monitor:latest
        command: ["/app/monitor"]
```

### Managing DaemonSets

```bash
# Create DaemonSet
kubectl apply -f daemonset.yaml

# List DaemonSets
# Shows desired number (should equal node count)
kubectl get daemonset -o wide

# Get pod distribution across nodes
# Each node should have exactly one pod
kubectl get pods -l app=node-exporter -o wide

# Describe for detailed information
# Includes: node selectors, update strategy, tolerations
kubectl describe daemonset node-exporter

# Check DaemonSet status
# Shows number of nodes with/without scheduled pods
kubectl get daemonset node-exporter

# Rolling update DaemonSet
# Similar to Deployment but node-by-node
kubectl rollout status daemonset/node-exporter

# Delete DaemonSet (pods are cleaned up automatically)
kubectl delete daemonset node-exporter
```

### Update Strategies

```yaml
spec:
  updateStrategy:
    type: RollingUpdate                   # Default: delete and recreate node by node
    # or
    type: OnDelete                        # Pods only updated when manually deleted
```

## Gotchas for Docker Users

- **Guaranteed scheduling**: Unlike Docker Swarm or Docker Compose which spread arbitrarily, DaemonSets ensure every node runs the pod
- **One-per-node semantics**: Can't scale with replicas - you get one pod per matching node automatically
- **Infrastructure responsibility**: These are typically cluster-infrastructure pods, not application pods
- **Node-level scheduling**: Uses node taints/tolerations rather than pod scheduling constraints
- **Cleanup behavior**: Unlike Docker which leaves containers, DaemonSet pods are automatically removed when DaemonSet is deleted

## Common Mistakes

- **Missing tolerations**: Pods won't schedule on control-plane nodes without proper tolerations
- **Wrong node selector**: DaemonSet won't create pods if no nodes match the selector
- **Ignoring resource limits**: One pod per node can quickly consume resources if not limited
- **No update strategy**: Not planning for updates can cause service interruptions
- **Overuse**: Don't use DaemonSets for application pods - only for infrastructure

## Quick Reference

| Field | Description |
|-------|-------------|
| nodeSelector | Run only on nodes with these labels |
| tolerations | Allow running on tainted nodes |
| updateStrategy | RollingUpdate or OnDelete |
| minReadySeconds | Seconds before pod is ready |

| Command | Description |
|---------|-------------|
| `kubectl get daemonset` | List DaemonSets |
| `kubectl describe daemonset` | Detailed info |
| `kubectl rollout status daemonset/NAME` | Check update status |

| Toleration Syntax | Effect |
|-------------------|--------|
| `key: foo, operator: Exists` | Tolerant of any taint with key "foo" |
| `key: foo, effect: NoSchedule` | Tolerant of NoSchedule taints |
| `operator: Exists` (no key) | Tolerant of all taints |

## What's Next

Continue to [Jobs and CronJobs](./03-jobs-and-cronjobs.md) to learn about run-to-completion workloads.
