# GitRepository and Kustomization

## Overview

This guide covers GitRepository and Kustomization resources in Flux for managing cluster state.

## Prerequisites

- Flux installed

## Step-by-Step Examples

### Create GitRepository

```yaml
apiVersion: source.toolkit.fluxcd.io/v1
kind: GitRepository
metadata:
  name: myapp
  namespace: flux-system
spec:
  url: https://github.com/myorg/myapp
  ref:
    branch: main
  interval: 60s
```

### Create Kustomization

```yaml
apiVersion: kustomize.toolkit.fluxcd.io/v1
kind: Kustomization
metadata:
  name: myapp
  namespace: flux-system
spec:
  sourceRef:
    kind: GitRepository
    name: myapp
  path: ./k8s/prod
  prune: true
  interval: 60s
```

## Quick Reference

| Resource | Purpose |
|----------|---------|
| GitRepository | Git source |
| Kustomization | Apply manifests |

## What's Next

Continue to [GitOps Principles](../../10-platform-engineering/disaster-recovery/01-etcd-backup.md)
