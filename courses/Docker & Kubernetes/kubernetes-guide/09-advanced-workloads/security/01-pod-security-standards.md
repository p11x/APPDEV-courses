# Pod Security Standards

## Overview

Pod Security Standards (PSS) define security requirements for pod specs. They protect workloads from security threats and ensure consistent security posture.

## Prerequisites

- Kubernetes 1.19+
- Understanding of pods and namespaces

## Core Concepts

### Three Security Levels

| Level | Description | Use Case |
|-------|-------------|----------|
| Privileged | Unrestricted access | Trusted workloads, infrastructure |
| Baseline | Minimal restrictions | Most workloads |
| Restricted | Strong restrictions | Sensitive workloads |

### Namespaced Enforcement

- **Enforce**: Policy is enforced at pod creation
- **Audit**: Policy violations are logged but allowed
- **Warn**: Warnings shown but pod is created

## Step-by-Step Examples

### Enable Built-in PSS

```yaml
apiVersion: v1
kind: Namespace
metadata:
  name: production
  labels:
    # Enforce baseline policy
    pod-security.kubernetes.io/enforce: baseline
    # Audit violations
    pod-security.kubernetes.io/audit: restricted
    # Warn users
    pod-security.kubernetes.io/warn: restricted
```

### Check Namespace Status

```bash
# View namespace security labels
kubectl get namespace default -o jsonpath='{.metadata.labels}'

# See security warnings on pods
kubectl get pods --all-namespaces -l pod-security.kubernetes.io/audit-violations
```

### Privileged Namespace Example

```yaml
# For infrastructure components
apiVersion: v1
kind: Namespace
metadata:
  name: monitoring
  labels:
    pod-security.kubernetes.io/enforce: privileged
    pod-security.kubernetes.io/audit: privileged
    pod-security.kubernetes.io/warn: privileged
```

## Gotchas for Docker Users

- **No Docker equivalent**: PSS is Kubernetes-specific
- **Namespaced**: Applies at namespace level
- **Not retroactive**: Only affects new pods

## Common Mistakes

- **Wrong level**: Using privileged when not needed
- **Namespace labels**: Must set all three (enforce, audit, warn)
- **Migration**: Moving to restricted requires pod updates

## Quick Reference

| Level | Restrictions |
|-------|-------------|
| Privileged | None |
| Baseline | No hostPath, no hostNetwork, no hostPID |
| Restricted | Run as non-root, drop all capabilities |

## What's Next

Continue to [Security Contexts](./02-security-contexts.md) for fine-grained pod security.
