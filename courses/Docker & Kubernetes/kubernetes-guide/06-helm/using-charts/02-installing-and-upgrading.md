# Installing and Upgrading

## Overview

This guide covers installing charts, upgrading releases, and managing the Helm release lifecycle.

## Prerequisites

- Helm installed

## Step-by-Step Examples

### Install Chart

```bash
# Simple install
helm install my-release bitnami/nginx

# With namespace
kubectl create namespace myns
helm install my-release bitnami/nginx -n myns

# Wait for deployment
helm install my-release bitnami/nginx --wait --timeout 5m
```

### Upgrade Release

```bash
# Upgrade to new version
helm upgrade my-release bitnami/nginx

# Upgrade with values
helm upgrade my-release bitnami/nginx --set service.port=8080
```

### Install or Upgrade

```bash
# Install or upgrade (idempotent)
helm upgrade --install my-release bitnami/nginx
```

### Rollback

```bash
# List revisions
helm history my-release

# Rollback to previous
helm rollback my-release

# Rollback to specific revision
helm rollback my-release 1
```

## Quick Reference

| Command | Description |
|---------|-------------|
| helm install | Deploy chart |
| helm upgrade | Update release |
| helm rollback | Revert changes |
| helm list | List releases |

## What's Next

Continue to [Values and Overrides](./03-values-and-overrides.md) for configuration.
