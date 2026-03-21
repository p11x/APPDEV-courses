# Helm Concepts

## Overview

Understanding Helm's core concepts helps you use it effectively. This guide covers Charts, Releases, and Repositories.

## Prerequisites

- Helm installed

## Core Concepts

### Chart

A chart is a collection of files:

```
mychart/
├── Chart.yaml        # Chart metadata
├── values.yaml       # Default values
├── templates/        # Kubernetes manifests
└── charts/           # Dependencies
```

### Release

A release is a deployed chart instance:

```bash
# Install creates a release
helm install my-release mychart

# Upgrade modifies the release
helm upgrade my-release mychart

# Rollback reverts changes
helm rollback my-release 1
```

### Repository

Chart repositories store and share charts:

```bash
# Add repository
helm repo add stable https://charts.helm.sh/stable

# Search charts
helm search repo nginx

# List installed releases
helm list
```

## Quick Reference

| Object | Description |
|--------|-------------|
| Chart | Package template |
| Release | Deployed instance |
| Repository | Chart storage |

## What's Next

Continue to [Finding Charts](../using-charts/01-finding-charts.md) for using charts.
