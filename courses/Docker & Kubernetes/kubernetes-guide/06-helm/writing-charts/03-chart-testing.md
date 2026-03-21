# Chart Testing

## Overview

Testing Helm charts ensures quality and reliability. This guide covers various testing approaches.

## Prerequisites

- Chart creation knowledge

## Step-by-Step Examples

### Lint Chart

```bash
# Check for issues
helm lint mychart
```

### Dry Run

```bash
# Render templates without installing
helm template mychart

# Dry-run with cluster
helm install --dry-run mychart mychart
```

### Test Hooks

```yaml
# In templates
apiVersion: v1
kind: Pod
metadata:
  annotations:
    "helm.sh/hook": test
```

## Quick Reference

| Command | Description |
|---------|-------------|
| helm lint | Lint chart |
| helm template | Render templates |
| helm test | Run tests |

## What's Next

Continue to [What is Kustomize](../../07-kustomize/basics/01-what-is-kustomize.md) for alternative templating.
