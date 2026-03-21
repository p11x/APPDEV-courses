# Local Persist Plugin

## Overview

The local-persist plugin allows Docker volumes to persist data on the host filesystem, surviving `docker volume prune`. This is useful for development and data that should survive cleanup.

## Prerequisites

- Docker Engine 20.10+
- Root access

## Core Concepts

### How It Works

- Creates volumes in specified host directory
- Data persists even after container removal
- Survives docker volume prune

## Step-by-Step Examples

### Installation

```bash
# Install local-persist plugin
docker plugin install \
  vieux/local-persist \
  --grant-all-permissions
```

### Creating Persistent Volumes

```bash
# Create volume with specific host path
docker volume create \
  --name my-persistent-data \
  -d vieux/local-persist \
  -o mountpoint=/data/myapp

# Run container with persistent volume
docker run -v my-persistent-data:/app/data myapp
```

### Docker Compose Usage

```yaml
version: "3.8"

services:
  app:
    image: myapp
    volumes:
      - mydata:/app/data

volumes:
  mydata:
    driver: vieux/local-persist
    driver_opts:
      mountpoint: /data/myapp
```

## Gotchas for Docker Users

- **Host path**: Must exist on host
- **Permissions**: Ensure correct UID/GID

## Quick Reference

| Command | Description |
|---------|-------------|
| docker plugin install vieux/local-persist | Install plugin |
| -o mountpoint=PATH | Specify host path |

## What's Next

Continue to [Writing a Volume Plugin](./03-writing-a-volume-plugin.md) for custom plugins.
