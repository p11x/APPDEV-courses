# Docker Run Deep Dive

## Overview

The `docker run` command is the primary way to create and start containers. Understanding its many options is essential for running containers correctly in development and production. This guide covers the most important flags, common patterns, and best practices for running containers effectively.

## Prerequisites

- Docker installed and running
- Understanding of Docker images
- Basic command-line knowledge

## Core Concepts

### Basic Syntax

```bash
docker run [OPTIONS] IMAGE [COMMAND] [ARG...]
```

The command creates a new container from an image and starts it. If the image isn't available locally, Docker pulls it automatically.

### Container Lifecycle States

A container goes through these states:

- **Created**: Container exists but hasn't started
- **Running**: Container is executing
- **Paused**: Container processes are suspended
- **Stopped**: Container has exited
- **Deleted**: Container is removed

### Detached vs Interactive

```bash
# Detached mode - runs in background
docker run -d nginx

# Interactive mode - keeps stdin open
docker run -it alpine /bin/sh

# Combined
docker run -dit alpine /bin/sh
```

## Step-by-Step Examples

### Basic Container Running

```bash
# Run a container in detached mode
# -d = detached, runs in background
docker run -d nginx

# Run with custom name
# --name assigns a friendly name for easier management
docker run -d --name my-nginx nginx

# Run and remove after exit
# --rm automatically removes container when it stops
docker run --rm alpine echo "Hello"

# Run with restart policy
# --restart=unless-stopped restarts unless explicitly stopped
docker run -d --restart=unless-stopped nginx
```

### Port Mapping

```bash
# Map container port to host port
# -p hostPort:containerPort maps traffic
docker run -d -p 8080:80 nginx
# Access nginx at http://localhost:8080

# Map to specific host interface
# Only listen on localhost
docker run -d -p 127.0.0.1:8080:80 nginx

# Map to random available port
# Let Docker choose an available host port
docker run -d -P nginx
# Check with: docker port container_name

# Multiple ports
docker run -d -p 8080:80 -p 8443:443 nginx
```

### Environment Variables

```bash
# Set environment variable
# -e sets an environment variable in the container
docker run -e "ENVIRONMENT=production" alpine env

# Multiple environment variables
docker run -e "ENVIRONMENT=prod" -e "DEBUG=false" alpine env

# Read from host environment variable
# Host vars are passed into container if not explicitly set
docker run -e HOST_VAR alpine env

# Read from file
# --env-file reads variables from a file
docker run --env-file .env alpine env
```

### Volume Mounting

```bash
# Mount a volume
# -v hostPath:containerPath mounts directory
docker run -v /my/data:/data alpine ls /data

# Read-only mount
# :ro makes the mount read-only
docker run -v /my/data:/data:ro alpine ls /data

# Named volume
# Creates volume if it doesn't exist
docker run -v myvolume:/data alpine ls /data

# Mount single file
docker run -v ~/.bashrc:/root/.bashrc:ro alpine cat /root/.bashrc
```

### Resource Limits

```bash
# Limit memory
# --memory or -m limits RAM usage
docker run -m 512m nginx

# Limit CPU
# --cpus limits CPU cores available
docker run --cpus="1.5" nginx

# Limit CPU to specific cores
# --cpuset-cpus binds to specific CPU
docker run --cpuset-cpus="0,1" nginx

# Set memory and CPU together
docker run -m 1g --cpus=2 nginx
```

### User and Security

```bash
# Run as non-root user
# --user specifies UID:GID
docker run --user 1000:1000 alpine id

# Read-only root filesystem
# --read-only makes root filesystem immutable
docker run --read-only alpine touch /test
# This would fail: touch: /test: Read-only file system

# Add capabilities
# --cap-add adds Linux capabilities
docker run --cap-add NET_ADMIN alpine ip link

# Drop capabilities
# --cap-drop removes capabilities
docker run --cap-drop ALL alpine

# Run in privileged mode (dangerous!)
# --privileged gives all capabilities
docker run --privileged alpine
```

### Health Checks

```bash
# Run with health check
docker run \
  --health-cmd="curl -f http://localhost/ || exit 1" \
  --health-interval=30s \
  --health-timeout=3s \
  --health-retries=3 \
  --health-start-period=5s \
  nginx

# Check health status
docker inspect --format='{{.State.Health.Status}}' container_name
```

### Complete Production Example

```bash
# Production-ready container run
docker run -d \
  --name production-nginx \
  --restart unless-stopped \
  -p 80:80 \
  -p 443:443 \
  -v /etc/nginx:/etc/nginx:ro \
  -v /var/log/nginx:/var/log/nginx \
  -v /var/www/html:/usr/share/nginx/html:ro \
  -e NGINX_HOST=example.com \
  -e NGINX_PORT=80 \
  -m 256m \
  --cpus=1.0 \
  --health-cmd="wget --no-verbose --tries=1 --spider http://localhost/ || exit 1" \
  --health-interval=30s \
  --health-timeout=3s \
  --health-retries=3 \
  nginx:1.25-alpine
```

## Common Mistakes

- **Running without -d in terminal**: Foreground mode blocks the terminal. Use -d for daemon mode.
- **Not mapping ports correctly**: Remember the format is hostPort:containerPort.
- **Forgetting -p flag**: Containers can't be accessed without port mapping.
- **Setting memory too low**: Applications may crash with OOM (Out of Memory) errors.
- **Using --privileged unnecessarily**: This is a security risk; avoid unless absolutely necessary.
- **Not cleaning up containers**: Use --rm for temporary containers or prune regularly.
- **Confusing -v with -p**: -v mounts volumes, -p publishes ports.

## Quick Reference

| Flag | Short | Description |
|------|-------|-------------|
| --detach | -d | Run in background |
| --name | -n | Container name |
| --publish | -p | Port mapping |
| --env | -e | Environment variable |
| --volume | -v | Mount volume |
| --memory | -m | Memory limit |
| --cpus | - | CPU limit |
| --rm | - | Auto-remove on stop |
| --restart | - | Restart policy |
| --user | -u | Run as user |
| --read-only | - | Read-only rootfs |
| --health-cmd | - | Health check command |

## What's Next

Now that you understand docker run, continue to [Container Lifecycle](./02-container-lifecycle.md) to learn how to manage containers throughout their lifecycle.
