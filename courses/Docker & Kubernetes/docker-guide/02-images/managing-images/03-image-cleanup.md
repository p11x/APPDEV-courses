# Image Cleanup

## Overview

Over time, Docker accumulates unused images, containers, volumes, and build cache, consuming significant disk space. Regular cleanup is essential for maintaining a healthy Docker environment, especially on development machines and CI/CD servers with limited storage. This guide covers safe and effective cleanup strategies.

## Prerequisites

- Docker installed and running
- Understanding of Docker images and containers
- Familiarity with command-line interface

## Core Concepts

### What Accumulates

Docker stores several types of data:

- **Images**: Pulled and built images
- **Containers**: Stopped and running containers
- **Volumes**: Persistent data
- **Networks**: User-defined networks
- **Build Cache**: Cached build layers

### Disk Space Usage

Check what's using space:

```bash
# Overall Docker disk usage
docker system df

# Detailed breakdown
docker system df -v
```

### Cleanup Commands

Docker provides convenient cleanup commands:

```bash
# Remove unused containers
docker container prune

# Remove unused images
docker image prune

# Remove unused volumes
docker volume prune

# Remove unused networks
docker network prune

# Remove all unused data
docker system prune

# Remove all unused data including volumes
docker system prune -a --volumes
```

## Step-by-Step Examples

### Basic Cleanup

```bash
# Check current disk usage
docker system df

# Clean up stopped containers
docker container prune -f

# Clean up dangling images (untagged)
# Dangling images have <none> as tag
docker image prune -f

# Clean up all unused images (not used by any container)
docker image prune -a -f

# Clean up build cache
docker builder prune -f

# Full system cleanup
docker system prune -f
```

### Selective Cleanup

```bash
# Remove specific image by ID
docker rmi image_id

# Remove specific image by tag
docker rmi myapp:old-version

# Remove all stopped containers
docker rm $(docker ps -aq -f status=exited)

# Remove all dangling images
docker rmi $(docker images -f "dangling=true" -q)

# Remove images older than a certain time
docker image prune -f --filter "until=24h"

# Remove images with specific label
docker image prune -f --filter "label=maintainer=old@example.com"
```

### Removing Build Cache

```bash
# View build cache size
docker buildx ls

# Prune build cache
docker builder prune

# Prune build cache larger than a size
docker builder prune --filter "max-used-space=10GB"

# Remove all build cache
docker builder prune -af
```

### Volume Cleanup

```bash
# List volumes
docker volume ls

# Remove specific volume
docker volume rm myvolume

# Remove all unused volumes
docker volume prune -f

# Remove volumes with specific label
docker volume prune -f --filter "label=environment=dev"
```

### Automation Scripts

Create a cleanup script:

```bash
#!/bin/bash
# docker-cleanup.sh

# Exit on error
set -e

echo "=== Docker Cleanup Started ==="
date

# Show before disk usage
echo "Disk usage before:"
docker system df

# Prune containers
echo "Removing stopped containers..."
docker container prune -f

# Prune networks
echo "Removing unused networks..."
docker network prune -f

# Prune build cache
echo "Removing build cache..."
docker builder prune -f

# Prune images (not used by any container)
echo "Removing unused images..."
docker image prune -a -f

# Optional: Prune volumes (be careful!)
# docker volume prune -f

# Show after disk usage
echo "Disk usage after:"
docker system df

echo "=== Docker Cleanup Complete ==="
```

Run it:
```bash
chmod +x docker-cleanup.sh
./docker-cleanup.sh
```

### Scheduled Cleanup in CI/CD

```yaml
# GitHub Actions example - cleanup after build
name: Build and Push

on:
  push:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v3
      
      - name: Build and push
        uses: docker/build-push-action@v5
        with:
          context: .
          push: true
          tags: myapp:latest
      
      # Cleanup after build
      - name: Cleanup
        run: |
          docker system prune -af --volumes=false
```

### Docker Desktop Cleanup

```bash
# On macOS/Windows with Docker Desktop

# Open Docker Desktop
# Go to Settings > Resources > Disk

# Or use command line:
# Reset to factory defaults (removes all data)
osascript -e 'quit app "Docker"'
rm -rf ~/Library/Application\ Support/Docker
rm -rf ~/Library/Group\ Containers/group.com.docker
rm -rf ~/.docker
```

## Common Mistakes

- **Not cleaning regularly**: Disk fills up gradually until Docker fails.
- **Deleting running containers**: Always stop containers before removing.
- **Removing needed volumes**: Some volumes contain important data. Be careful.
- **Not using filters**: Unfiltered cleanup can remove images you need.
- **Ignoring build cache**: Build cache can grow very large.
- **Not checking space before**: Always check disk usage first.
- **Removing :latest when in use**: Ensure no running containers use the image.

## Quick Reference

| Command | What It Removes |
|---------|-----------------|
| `docker container prune` | Stopped containers |
| `docker image prune` | Dangling images |
| `docker image prune -a` | All unused images |
| `docker volume prune` | Unused volumes |
| `docker network prune` | Unused networks |
| `docker builder prune` | Build cache |
| `docker system prune` | All of the above |
| `docker system prune -a` | Everything including volumes |

| Option | Description |
|--------|-------------|
| `-f` | Force without confirmation |
| `-a` | Remove all unused (not just dangling) |
| `--volumes` | Include volumes |
| `--filter` | Filter what to remove |
| `--until` | Remove older than timestamp |

## What's Next

Now that you can manage images effectively, continue to [Docker Run Deep Dive](./../../containers/running-containers/01-docker-run-deep-dive.md) to learn how to run containers with the right configuration.
