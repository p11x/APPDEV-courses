# Config Generators

## Overview

Kustomize can generate ConfigMaps and Secrets automatically. This guide covers configMapGenerator and secretGenerator.

## Prerequisites

- Kustomize basics

## Core Concepts

### configMapGenerator

```yaml
configMapGenerator:
  - name: my-config
    literals:
      - KEY=value
      - ANOTHER=123
    files:
      - config.ini=config.ini
```

### secretGenerator

```yaml
secretGenerator:
  - name: my-secret
    literals:
      - username=admin
      - password=secret
```

## Quick Reference

| Generator | Purpose |
|-----------|---------|
| configMapGenerator | Generate ConfigMaps |
| secretGenerator | Generate Secrets |

## What's Next

Continue to [Components](../advanced/01-components.md) for reusable configurations.
