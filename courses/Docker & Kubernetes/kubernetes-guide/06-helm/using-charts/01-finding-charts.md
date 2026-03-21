# Finding Charts

## Overview

This guide covers finding and evaluating Helm charts from various sources.

## Prerequisites

- Helm installed

## Step-by-Step Examples

### Search Repositories

```bash
# Search in configured repos
helm search repo nginx

# Search Artifact Hub
helm search hub nginx

# View chart info
helm show chart bitnami/nginx
helm show values bitnami/nginx
```

### Evaluate Chart Quality

```bash
# Check chart README
helm show readme bitnami/nginx

# View default values
helm show values bitnami/nginx

# Inspect chart
helm inspect all bitnami/nginx
```

## Quick Reference

| Command | Description |
|---------|-------------|
| helm search repo | Search local repos |
| helm search hub | Search Artifact Hub |
| helm show | View chart info |

## What's Next

Continue to [Installing and Upgrading](./02-installing-and-upgrading.md) for deployment.
