# Namespace Isolation

## Overview

Namespace isolation is the foundation of multi-tenancy in Kubernetes. Each tenant gets their own namespace with logical separation of resources, permissions, and policies.

## Prerequisites

- Kubernetes cluster admin access
- Understanding of RBAC

## Core Concepts

### Namespace Isolation Layers

| Layer | Purpose |
|-------|---------|
| Network | NetworkPolicy restricts traffic |
| RBAC | Role-based access control |
| Resource | ResourceQuota limits usage |
| Storage | PersistentVolume claims |

### Why Multi-Tenancy

- **Cost efficiency**: Share cluster resources
- **Operational simplicity**: Single cluster to manage
- **Isolation**: Tenants separated logically

## Step-by-Step Examples

### Create Tenant Namespace

```yaml
apiVersion: v1
kind: Namespace
metadata:
  name: tenant-alpha
  labels:
    # For PSS enforcement
    pod-security.kubernetes.io/enforce: restricted
    pod-security.kubernetes.io/audit: restricted
    pod-security.kubernetes.io/warn: restricted
    # Tenant ownership
    tenant: alpha
```

### Network Isolation

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: deny-all
  namespace: tenant-alpha
spec:
  # Deny all ingress
  podSelector: {}
  policyTypes:
  - Ingress
```

### RBAC for Tenant

```yaml
# Role for tenant admin
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  namespace: tenant-alpha
  name: tenant-admin
rules:
- apiGroups: [""]
  resources: ["*"]
  verbs: ["*"]
- apiGroups: ["apps"]
  resources: ["*"]
  verbs: ["*"]
```

## Gotchas for Docker Users

- **No Docker equivalent**: Namespaces are Kubernetes concepts
- **Not security boundary**: Namespaces alone don't isolate
- **Shared nodes**: All tenants share underlying nodes

## Common Mistakes

- **Missing network policy**: Traffic not isolated
- **Overly permissive RBAC**: Too many permissions
- **No resource limits**: Tenant can consume all resources

## Quick Reference

| Isolation Type | Tool |
|----------------|------|
| Network | NetworkPolicy |
| Identity | RBAC |
| Compute | ResourceQuota |
| Storage | PersistentVolumeClaim |

## What's Next

Continue to [Resource Quotas](./02-resource-quotas.md) for resource management.
