# Resource Quotas

## Overview

ResourceQuota limits the total resources that can be consumed in a namespace. It prevents tenants from monopolizing cluster resources.

## Prerequisites

- Namespace created
- Understanding of CPU/memory concepts

## Core Concepts

### Quota Types

| Type | Description |
|------|-------------|
| Compute | CPU and memory limits |
| Storage | PersistentVolume claims |
| Object count | Number of resources |
| Pods | Total pod count |

### How Quotas Work

- **Hard limits**: Requests exceeding quota are rejected
- **Per-pod defaults**: Can set default requests/limits
- **Scope**: Can limit to specific pod states

## Step-by-Step Examples

### Basic ResourceQuota

```yaml
apiVersion: v1
kind: ResourceQuota
metadata:
  name: tenant-quota
  namespace: tenant-alpha
spec:
  hard:
    # Compute resources
    requests.cpu: "4"
    limits.cpu: "8"
    requests.memory: 8Gi
    limits.memory: 16Gi
    # Pod count
    pods: "20"
    # Storage
    persistentvolumeclaims: "10"
    requests.storage: 100Gi
```

### Quota with Defaults

```yaml
apiVersion: v1
kind: LimitRange
metadata:
  name: tenant-limits
  namespace: tenant-alpha
spec:
  limits:
  - default:
      memory: 512Mi
      cpu: "500m"
    defaultRequest:
      memory: 256Mi
      cpu: "250m"
    type: Container
```

### Object Count Quota

```yaml
apiVersion: v1
kind: ResourceQuota
metadata:
  name: object-quota
  namespace: tenant-alpha
spec:
  hard:
    configmaps: "10"
    secrets: "10"
    services: "5"
    services.loadbalancers: "1"
    replicationcontrollers: "5"
```

## Gotchas for Docker Users

- **No Docker equivalent**: ResourceQuota is Kubernetes-specific
- **Namespace scope**: Only applies within one namespace
- **Not retroactive**: Existing pods unaffected

## Common Mistakes

- **Too restrictive**: Pods can't be scheduled
- **No LimitRange**: Individual pods may fail
- **Missing requests**: Pods may not start

## Quick Reference

| Resource | Description |
|----------|-------------|
| requests.cpu | CPU request limit |
| limits.cpu | CPU max limit |
| requests.memory | Memory request |
| limits.memory | Memory max |
| pods | Max pods |
| persistentvolumeclaims | Max PVCs |

## What's Next

Continue to [Network Policies](./03-network-policies.md) for network isolation.
