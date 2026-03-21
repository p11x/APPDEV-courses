# Resource Metrics

## Overview

Resource metrics provide insight into CPU, memory, and network usage across pods and nodes. Kubernetes exposes these metrics through the Metrics Server, enabling autoscaling, capacity planning, and performance troubleshooting.

## Prerequisites

- Metrics Server installed
- kubectl basics

## Core Concepts

### Metrics Server

The Metrics Server collects resource metrics from kubelet on each node:
- CPU usage (millicores)
- Memory usage (bytes)
- Works with HPA for autoscaling
- Lightweight, no long-term storage

### Resource Requests and Limits

```yaml
resources:
  requests:
    memory: "128Mi"    # Guaranteed allocation
    cpu: "100m"        # 0.1 CPU cores
  limits:
    memory: "256Mi"   # Maximum allowed
    cpu: "500m"       # 0.5 CPU cores
```

- **Requests**: Guaranteed minimum allocation
- **Limits**: Maximum allowed (can cause OOMKilled if exceeded)

## Viewing Metrics

### Node Metrics

```bash
# Get node resource usage
# top nodes shows CPU/memory per node
kubectl top nodes

# Example output:
# NAME              CPU(cores)   CPU%   MEMORY(bytes)   MEMORY%
# minikube          250m         12%    1024Mi          45%
# worker-1          500m         25%    2048Mi          60%
```

### Pod Metrics

```bash
# Get pod resource usage
# top pods shows metrics for all pods
kubectl top pods

# Example output:
# NAME                        CPU(cores)   MEMORY(bytes)
# web-app-6d9f8b7c9-abcde     50m          128Mi
# web-app-6d9f8b7c9-fghij     75m          256Mi
# db-0                        100m         512Mi

# Show per-container metrics
kubectl top pods --containers=true

# Show pods in specific namespace
kubectl top pods -n production

# Show pods with labels
kubectl top pods -l app=web
```

### Container Metrics

```bash
# Get container-level metrics
# --containers shows individual container usage
kubectl top pod my-pod --containers

# Output:
# POD          NAME            CPU(cores)   MEMORY(bytes)
# my-pod       web             25m          64Mi
# my-pod       sidecar        10m          32Mi
```

### Metrics for Specific Resources

```bash
# Get metrics for a single pod
kubectl top pod my-pod

# Get metrics for deployment (sum of all pods)
kubectl top deployment web-app
```

## Checking Metrics Server

```bash
# Verify metrics server is running
# metrics-server runs in kube-system namespace
kubectl get pods -n kube-system -l k8s-app=metrics-server

# Get metrics server logs
kubectl logs -n kube-system -l k8s-app=metrics-server

# Describe metrics server
kubectl describe pod -n kube-system -l k8s-app=metrics-server
```

## Installation

### Install Metrics Server

```bash
# Install via manifest
kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml

# Verify installation
kubectl get apiservices | grep metrics

# Check metrics API
kubectl get --raw /apis/metrics.k8s.io/
```

### Custom Metrics (Advanced)

```bash
# Install Prometheus Adapter for custom metrics
# Required for HPA with custom metrics
kubectl apply -f https://github.com/DaoCloud/dce-charts-repackage/releases/download/prometheus-adapter-0.12.0/prometheus-adapter.yaml

# Get custom metrics
kubectl get --raw "/apis/custom.metrics.k8s.io/v1beta1"
```

## Resource Usage Analysis

### Identify High-Usage Pods

```bash
# Sort pods by CPU usage
kubectl top pods --sort-by=cpu

# Sort by memory
kubectl top pods --sort-by=memory

# Combined with grep for analysis
kubectl top pods -n production | grep -E "NAME|200m|500Mi"
```

### Check Node Capacity

```bash
# Show node allocatable resources
kubectl describe nodes | grep -A 5 "Allocated resources"

# Output shows:
# Allocated resources:
#   (Total limits may be over 100% because nodes have unschedulable resources)
#   CPU Requests  CPU Limits  Memory Requests  Memory Limits
#   500m (25%)    1000m       512Mi (15%)      1024Mi (30%)
```

### Pod Resource Analysis

```yaml
# Get resource usage percentage
# Compare usage vs requests
apiVersion: v1
kind: Pod
metadata:
  name: my-pod
status:
  containerStatuses:
  - name: web
    resources:
      requests:
        memory: "128Mi"
      usage:
        memory: "64000000"  # ~61Mi in bytes
```

```bash
# Calculate usage percentage
# Usage / Request * 100 = Percentage
# 64Mi / 128Mi * 100 = 50%
```

## Resource Quotas

### Check ResourceQuota

```bash
# Get resource quota
kubectl get resourcequota -n production

# Describe for details
kubectl describe resourcequota -n production

# Output:
# Name:            production-quota
# Resource         Used    Hard
# --------         ----    ----
# pods             10      20
# secrets          5       10
# services         3       5
# requests.cpu     2       8
# requests.memory  1Gi     4Gi
```

### Check LimitRange

```bash
# Get limit ranges (default limits)
kubectl get limitrange -n production

# Show applied limits
kubectl describe limitrange -n production
```

## Monitoring Tools

### Prometheus and Grafana

```bash
# Install Prometheus Operator
kubectl apply -f https://raw.githubusercontent.com/prometheus-operator/prometheus-operator/main/bundle.yaml

# Install Grafana
helm install grafana grafana/grafana

# Access Grafana
kubectl port-forward svc/grafana 3000:3000
```

### Metrics Dashboard

```bash
# Common dashboard metrics:
# - Pod CPU usage (rate)
# - Pod memory working set
# - Network I/O
# - Storage usage
# - Node exporter metrics
```

## Common Metrics Patterns

```bash
# Monitor deployment rollout
# Track new pods resource usage
kubectl top pods -l app=web,version=v2

# Monitor problematic pods
kubectl top pods --sort-by=cpu | head -10

# Find pods at memory limit
# Look for pods approaching limits
kubectl top pods -n production | awk '{print $2,$3,$4}' | \
  while read name cpu mem; do
    echo "$name $cpu $mem"
  done
```

## Troubleshooting

### Metrics Not Available

```bash
# Check kubelet is running
kubectl get nodes

# Check kubelet metrics port
kubectl get --raw /api/v1/nodes/minikube/proxy/stats/summary

# Restart metrics server
kubectl rollout restart deployment metrics-server -n kube-system
```

### High Resource Usage

```bash
# Identify cause
# 1. Check pod logs
kubectl logs high-cpu-pod

# 2. Check pod events
kubectl describe pod high-memory-pod

# 3. Check application metrics
kubectl exec -it app-pod -- curl localhost:9090/metrics
```

## Gotchas for Docker Users

- **Aggregated metrics**: Unlike docker stats, metrics are aggregated by Kubernetes
- **Cumulative values**: Some metrics are cumulative (need rate calculation)
- **Time windows**: Metrics are point-in-time, not historical
- **Custom metrics**: Require additional setup beyond metrics-server

## Common Mistakes

- **Forgetting requests**: Pods scheduled based on requests, not limits
- **Limits too high**: Can cause resource starvation for other pods
- **Not monitoring**: Reactive instead of proactive
- **Metrics delays**: May be 1-2 minutes delayed

## Quick Reference

| Command | Description |
|---------|-------------|
| `kubectl top nodes` | Node resource usage |
| `kubectl top pods` | Pod resource usage |
| `kubectl top pods --sort-by=cpu` | Sort by CPU |
| `kubectl get --raw /apis/metrics.k8s.io/` | Raw metrics API |

| Metric | Description |
|--------|-------------|
| CPU (m) | Millicores (1000m = 1 core) |
| Memory (Mi/Gi) | Mebibytes/Gibibytes |
| Ephemeral storage | Temporary storage |
| Pods | Pod count |

## What's Next

Continue to [Liveness and Readiness Probes](./03-liveness-and-readiness-probes.md) for health checks.
