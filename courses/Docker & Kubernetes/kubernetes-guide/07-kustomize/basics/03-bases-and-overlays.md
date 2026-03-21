# Bases and Overlays

## Overview

Kustomize uses a base + overlay model for managing configurations across environments. This guide covers creating and using overlays.

## Prerequisites

- Kustomization file knowledge

## Core Concepts

### Directory Structure

```
myapp/
├── base/
│   ├── kustomization.yaml
│   ├── deployment.yaml
│   └── service.yaml
└── overlays/
    ├── dev/
    │   └── kustomization.yaml
    └── prod/
        └── kustomization.yaml
```

### Base kustomization.yaml

```yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization
resources:
  - deployment.yaml
  - service.yaml
```

### Overlay kustomization.yaml

```yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization
resources:
  - ../../base
namePrefix: dev-
commonLabels:
  env: dev
```

## Quick Reference

| Command | Description |
|---------|-------------|
| kubectl kustomize base | Build base |
| kubectl kustomize overlays/prod | Build overlay |
| kubectl apply -k overlays/prod | Apply overlay |

## What's Next

Continue to [Patches](../transformers/01-patches.md) for modifications.
