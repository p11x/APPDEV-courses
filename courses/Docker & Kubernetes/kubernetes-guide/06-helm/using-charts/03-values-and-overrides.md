# Values and Overrides

## Overview

Helm values allow you to customize chart configurations. This guide covers different ways to override default values.

## Prerequisites

- Helm installed

## Step-by-Step Examples

### Using --set

```bash
# Set single value
helm install my-release chart --set image.tag=v1.0

# Multiple values
helm install my-release chart --set image.tag=v1.0,replicas=3
```

### Using Values Files

```bash
# Custom values file
helm install my-release chart -f myvalues.yaml
```

### Values Hierarchy

```bash
# Values applied in order (later overrides):
# 1. chart's values.yaml
# 2. parent chart's values.yaml
# 3. -f values files (in order)
# 4. --set values
```

## Quick Reference

| Method | Description |
|--------|-------------|
| --set | Command line |
| -f file | Values file |
| --set-string | Force string |

## What's Next

Continue to [Chart Structure](../writing-charts/01-chart-structure.md) for creating charts.
