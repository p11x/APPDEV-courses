# Components

## Overview

Kustomize components allow reusable configurations that can be optionally included. This guide covers creating and using components.

## Prerequisites

- Kustomize overlays knowledge

## Core Concepts

### What Are Components

Components are reusable bundles of Kustomize configurations:

```yaml
# components/monitoring/kustomization.yaml
apiVersion: kustomize.config.k8s.io/v1alpha1
kind: Component

patches:
  - path: prometheus-patch.yaml
```

### Using Components

```yaml
# overlays/prod/kustomization.yaml
resources:
  - ../../base

components:
  - ../../components/monitoring
```

## Quick Reference

| Feature | Description |
|---------|-------------|
| Optional | Can be conditionally applied |
| Composable | Multiple components |
| Reusable | Shared across environments |

## What's Next

Continue to [Replacements](./02-replacements.md) for value transformation.
