# Patches

## Overview

Patches modify base resources in Kustomize. This guide covers different patch strategies.

## Prerequisites

- Kustomize basics

## Step-by-Step Examples

### Strategic Merge Patch

```yaml
# kustomization.yaml
patches:
  - path: patch.yaml
```

### Inline Patch

```yaml
patches:
  - target:
      kind: Deployment
      name: myapp
    patch: |
      - op: replace
        path: /spec/replicas
        value: 3
```

## Quick Reference

| Type | Description |
|------|-------------|
| Strategic merge | Default |
| JSON 6902 | More flexible |
| Inline | Direct YAML |

## What's Next

Continue to [Name Prefix and Labels](./02-name-prefix-and-labels.md) for transformations.
