# Exec and Inspect

## Overview

Docker provides powerful tools for inspecting and debugging running containers. The `docker exec` command lets you run commands inside containers, while `docker inspect` provides detailed information about containers, images, and other Docker objects.

## Prerequisites

- Basic Docker knowledge
- Container lifecycle understanding

## Core Concepts

### docker exec

Runs additional processes in running containers.

### docker inspect

Returns detailed information about Docker objects in JSON format.

## Step-by-Step Examples

### Exec

```bash
# Run command in container
docker exec container_name ls /

# Interactive shell
docker exec -it container_name /bin/sh

# Run as specific user
docker exec -u 1000 container_name whoami

# Execute in specific directory
docker exec -w /app container_name pwd
```

### Inspect

```bash
# Inspect container
docker inspect container_name

# Get specific field
docker inspect --format='{{.NetworkSettings.IPAddress}}' container_name

# Get all exposed ports
docker inspect --format='{{.NetworkSettings.Ports}}' container_name

# Get state
docker inspect --format='{{.State}}' container_name
```

### Practical Debugging

```bash
# Check what's running
docker top container_name

# View processes
docker exec container_name ps aux

# Check environment
docker exec container_name env

# Test network
docker exec container_name ping other-container

# Copy file from container
docker cp container_name:/path/to/file ./local/
```

## Common Mistakes

- **Exec on stopped containers**: Can't exec into stopped containers.
- **Not understanding JSON output**: Use --format to extract specific values.

## What's Next

Continue to [Docker Events](./03-docker-events.md)
