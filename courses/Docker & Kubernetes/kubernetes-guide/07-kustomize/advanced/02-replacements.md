# Replacements

## Overview

Replacements allow copying values between resources. This guide covers the replacements transformer.

## Prerequisites

- Kustomize basics

## Core Concepts

### Replacement Example

```yaml
replacements:
  - source:
      kind: ConfigMap
      name: my-config
      fieldPath: data.port
    targets:
      - fieldPath: spec.template.spec.containers[0].env[0].value
        select:
          kind: Deployment
          name: my-app
```

## Quick Reference

| Field | Description |
|-------|-------------|
| source | Value to copy from |
| targets | Where to apply value |

## What's Next

Continue to [Kustomize with Helm](./03-kustomize-with-helm.md) for combined usage.
