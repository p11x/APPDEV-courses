# Flux Bootstrap

## Overview

This guide covers bootstrapping Flux in a Kubernetes cluster.

## Prerequisites

- kubectl access
- Git repository

## Step-by-Step Examples

### Bootstrap Flux

```bash
# Install Flux CLI
# brew install fluxcd/tap/flux

# Bootstrap to cluster
flux bootstrap github \
  --owner=myorg \
  --repository=myrepo \
  --branch=main \
  --path=clusters/prod \
  --personal
```

### Verify Installation

```bash
# Check Flux components
flux check

# List sources
flux get sources all

# List kustomizations
flux get kustomizations
```

## Quick Reference

| Command | Description |
|---------|-------------|
| flux bootstrap | Initialize Flux |
| flux check | Verify setup |
| flux get | List resources |

## What's Next

Continue to [GitRepository and Kustomization](./03-gitrepository-and-kustomization.md) for configuration.
