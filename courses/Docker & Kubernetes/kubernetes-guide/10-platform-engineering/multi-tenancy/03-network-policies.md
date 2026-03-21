# Multi-Tenant Network Policies

## Overview

Network policies control traffic flow between pods and external endpoints. In multi-tenant clusters, they enforce tenant isolation at the network level.

## Prerequisites

- CNI plugin that supports NetworkPolicy (Calico, Cilium, etc.)
- Understanding of pod networking

## Core Concepts

### Policy Types

| Type | Description |
|------|-------------|
| Ingress | Controls incoming traffic |
| Egress | Controls outgoing traffic |
| Both | Combined ingress/egress |

### Key Fields

| Field | Purpose |
|-------|---------|
| podSelector | Which pods to apply policy to |
| namespaceSelector | Filter by namespace |
| policyTypes | Ingress, Egress, or both |

## Step-by-Step Examples

### Deny All Ingress

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: deny-ingress
  namespace: tenant-alpha
spec:
  podSelector: {}
  policyTypes:
  - Ingress
```

### Allow Specific Traffic

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: allow-web
  namespace: tenant-alpha
spec:
  podSelector:
    matchLabels:
      app: web
  policyTypes:
  - Ingress
  ingress:
  # Allow from frontend pods
  - from:
    - podSelector:
        matchLabels:
          app: frontend
    ports:
    - protocol: TCP
      port: 80
```

### Egress Control

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: restrict-egress
  namespace: tenant-alpha
spec:
  podSelector: {}
  policyTypes:
  - Egress
  egress:
  # Allow DNS
  - to:
    - namespaceSelector:
        matchLabels:
          kubernetes.io/metadata.name: kube-system
      podSelector:
        matchLabels:
          k8s-app: kube-dns
    ports:
    - protocol: UDP
      port: 53
  # Allow API server
  - to:
    - ipBlock:
        cidr: 0.0.0.0/0
        except:
        - 10.0.0.0/8
        - 172.16.0.0/12
        - 192.168.0.0/16
```

## Gotchas for Docker Users

- **No Docker equivalent**: NetworkPolicy is Kubernetes-specific
- **CNI requirement**: Not all CNI plugins support it
- **Default deny**: Must explicitly allow traffic

## Common Mistakes

- **No default deny**: Traffic not blocked
- **Missing egress**: Pods can't reach external services
- **DNS blocked**: Cannot resolve names

## Quick Reference

| Selector | Matches |
|----------|---------|
| podSelector | Pods in same namespace |
| namespaceSelector | Pods in other namespaces |
| ipBlock | External IP ranges |

## What's Next

Continue to [Resource Limits Basics](./../cost-management/01-resource-limits-basics.md) for cost management.
