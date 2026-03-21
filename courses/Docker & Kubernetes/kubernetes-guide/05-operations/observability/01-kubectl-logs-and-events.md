# kubectl logs and Events

## Overview

Logging and event monitoring are essential for debugging applications in Kubernetes. kubectl provides powerful commands to view container logs and cluster events, helping diagnose issues with pods, deployments, and the cluster itself.

## Prerequisites

- Basic kubectl knowledge
- Understanding of pods and containers

## Viewing Logs

### Basic Log Commands

```bash
# View logs from a single container
# -f streams logs in real-time (like tail -f)
kubectl logs my-pod

# Stream logs continuously
kubectl logs -f my-pod

# Get previous container logs (after crash)
# --previous shows logs from before last restart
kubectl logs my-pod --previous

# View with timestamps
# --timestamps adds timestamp to each line
kubectl logs my-pod --timestamps
```

### Multi-Container Pods

```bash
# Specify container name when pod has multiple containers
# -c identifies which container to get logs from
kubectl logs my-pod -c web-container

# Stream logs from specific container
kubectl logs -f my-pod -c web-container

# Get logs from all containers
# --all-containers=true includes all containers in pod
kubectl logs my-pod --all-containers=true
```

### Labels and Selectors

```bash
# Get logs for all pods matching label
# -l selects pods with app=web label
kubectl logs -l app=web

# Stream logs from all matching pods
kubectl logs -f -l app=web --prefix

# All containers in namespace
kubectl logs --all-containers=true -n production
```

### Log Filtering

```bash
# Get last 100 lines
# --tail=100 shows only last 100 lines
kubectl logs my-pod --tail=100

# Get last 1 hour of logs
# Since shows logs from last hour
kubectl logs my-pod --since=1h

# Get logs since timestamp
kubectl logs my-pod --since-time=2024-01-15T10:00:00Z
```

### Saving Logs

```bash
# Save logs to file
# > redirects output to file
kubectl logs my-pod > pod.log

# Append to existing file
kubectl logs my-pod -c sidecar >> sidecar.log

# Save all pod logs in namespace
kubectl logs -l app=web -n production > web-logs.txt
```

### Interactive Log Viewer

```bash
# Use stern for advanced log tailing
# stern supports multiple pods, regex, color coding

# Install stern
# brew install stern (macOS)
# Download from https://github.com/stern/stern

# Tail logs from multiple pods
# --selector selects by label
stern --selector app=web

# Tail with regex filter
stern my-pod --include="ERROR|WARN"

# Since specific time
stern my-pod --since=15m
```

## Viewing Events

### Basic Event Commands

```bash
# Get all events in namespace
# Events sorted by time by default
kubectl get events

# Get events for specific resource
# --field-selector filters by field
kubectl get events --field-selector involvedObject.name=my-pod

# Watch events in real-time
kubectl get events -w

# Get events for specific namespace
kubectl get events -n production
```

### Event Filtering

```bash
# Filter by type: Normal, Warning
kubectl get events --field-selector type=Warning

# Filter by reason
kubectl get events --field-selector reason=Failed

# Filter by involved object
kubectl get events --field-selector involvedObject.kind=Pod

# Complex filtering
kubectl get events -n production --field-selector \
  involvedObject.name=my-pod,type=Warning
```

### Event Details

```bash
# Describe specific event
# Shows: Type, Reason, Message, Source, Age
kubectl describe events my-pod

# Full event output with YAML
kubectl get event my-pod-xyz -o yaml
```

### Useful Event Queries

```bash
# Recent events (last hour)
kubectl get events --sort-by='.lastTimestamp'

# All Warning events
kubectl get events --field-selector type=Warning

# Pod failures
kubectl get events --field-selector reason=Failed

# Image pull failures
kubectl get events --field-selector reason=FailedPullImage

# Scheduling failures
kubectl get events --field-selector reason=FailedScheduling

# Evictions
kubectl get events --field-selector reason=Evicted
```

## JSONPath Examples

```bash
# Get event messages only
kubectl get events -o jsonpath='{.items[*].message}'

# Get warning events with messages
kubectl get events -o jsonpath='{range .items[?(@.type=="Warning")]}{.message}{"\n"}{end}'
```

## Debugging with Logs and Events

### Common Patterns

```bash
# Debug pod not starting
# 1. Check pod events
kubectl get events --field-selector involvedObject.name=my-pod

# 2. Check pod logs
kubectl logs my-pod

# 3. Check previous logs if crashed
kubectl logs my-pod --previous

# Debug deployment issues
# 1. Check deployment events
kubectl describe deployment my-app

# 2. Check replica set events
kubectl get events --field-selector \
  involvedObject.name=my-app-xyz

# Debug service issues
kubectl describe service my-service
kubectl get events -l app=my-service
```

### Log Aggregation Tools

```bash
# Common log aggregation patterns:

# 1. ELK/EFK Stack
# Elasticsearch, Fluentd, Kibana
# - Centralized log storage and visualization

# 2. Loki
# Grafana Loki - horizontally scalable
# kubectl apply -f https://raw.githubusercontent.com/grafana/loki/main/ractices/helm/loki-stack.yaml

# 3. Datadog/Dynatrace
# Commercial solutions with auto-discovery
```

## Log Best Practices

```bash
# Structure logs as JSON for parsing
# In your application:
# {"level": "info", "message": "Request processed", "duration": "ms"}

# Use stdout and stderr correctly
# stdout (1) = normal logs
# stderr (2) = errors/warnings

# Avoid logging sensitive data
# Passwords, tokens, PII

# Set appropriate log levels
# Debug in development
# Info/Warning in production
```

## Gotchas for Docker Users

- **Container logs**: kubectl logs equivalent to docker logs
- **JSON logs**: Kubernetes logs are JSON-structured by default
- **Events**: Kubernetes-specific - no Docker equivalent
- **Aggregation**: Requires external tools like Loki or ELK

## Common Mistakes

- **Not checking events**: Always check events first when debugging
- **Missing --previous**: Logs lost after container restart
- **Wrong namespace**: Events are namespace-scoped
- **Not using labels**: Missing labels makes filtering impossible

## Quick Reference

| Command | Description |
|---------|-------------|
| `kubectl logs POD` | Get pod logs |
| `kubectl logs -f POD` | Stream logs |
| `kubectl logs POD --previous` | Previous container logs |
| `kubectl logs -l LABEL` | Logs for label selector |
| `kubectl get events` | List events |
| `kubectl get events -w` | Watch events |
| `kubectl describe RESOURCE` | Detailed resource info |

| Event Type | Meaning |
|------------|---------|
| Normal | Informational |
| Warning | Needs attention |
| Failed | Action failed |
| Evicted | Pod evicted |
| OOMKilled | Out of memory |

## What's Next

Continue to [Resource Metrics](./02-resource-metrics.md) for cluster monitoring.
