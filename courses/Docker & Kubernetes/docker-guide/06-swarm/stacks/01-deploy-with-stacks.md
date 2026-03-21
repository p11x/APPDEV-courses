# Deploy with Stacks

## Overview

Docker Stacks provide a way to deploy and manage multi-service applications in Docker Swarm. Using Compose files with the `deploy` key, you can define entire applications and deploy them with a single command.

## Prerequisites

- Active Docker Swarm cluster
- Understanding of Docker Compose files

## Core Concepts

### What Are Stacks

A stack is a collection of services that make up an application:

- Defined in a Compose file with `deploy:` section
- Deployed atomically as a single unit
- Support scaling, updates, and rollback
- Services share the same overlay network

### Compose File with Deploy

Unlike regular Compose files, Swarm stacks use the `deploy` key:

```yaml
version: "3.8"  # Or later
services:
  web:
    image: nginx
    deploy:
      replicas: 2
      resources:
        limits:
          cpus: "0.5"
          memory: 512M
```

## Step-by-Step Examples

### Creating a Stack File

```yaml
# docker-compose.yml for a stack
version: "3.8"

services:
  web:
    image: nginx:1.25-alpine
    ports:
      - "8080:80"
    deploy:
      replicas: 3
      restart_policy:
        condition: on-failure
        delay: 5s
        max_attempts: 3
      update_config:
        parallelism: 1
        delay: 10s
        failure_action: rollback
      resources:
        limits:
          cpus: "0.5"
          memory: 256M
        reservations:
          cpus: "0.25"
          memory: 128M

  api:
    image: myapi:latest
    deploy:
      replicas: 2
      restart_policy:
        condition: on-failure
      resources:
        limits:
          cpus: "1.0"
          memory: 1G
```

### Deploying a Stack

```bash
# Deploy a stack from compose file
# -n specifies stack name (instead of default directory name)
docker stack deploy -c docker-compose.yml myapp

# Or use default (filename)
docker stack deploy myapp
```

### Managing Stacks

```bash
# List all stacks
docker stack ls

# Example output:
# NAME                SERVICES   ORCHESTRATOR
# myapp               3          Swarm

# List services in a stack
docker stack services myapp

# Example output:
# ID             NAME          MODE         REPLICAS   IMAGE
# abc123         myapp_web      replicated   3/3        nginx:1.25
# def456         myapp_api      replicated   2/2        myapi:latest

# List tasks for a stack
docker stack ps myapp

# Remove a stack
docker stack rm myapp
```

### Viewing Stack Details

```bash
# Inspect a stack
docker stack inspect myapp

# Watch stack deployment
docker stack ps myapp --no-trunc
```

### Complete Example with Networks

```yaml
# complete-stack.yml
version: "3.8"

networks:
  frontend:
    driver: overlay
  backend:
    driver: overlay

services:
  web:
    image: nginx:1.25-alpine
    networks:
      - frontend
    ports:
      - "80:80"
    deploy:
      replicas: 3
      placement:
        constraints:
          - node.role == worker
      restart_policy:
        condition: on-failure

  api:
    image: myapi:latest
    networks:
      - frontend
      - backend
    deploy:
      replicas: 2
      resources:
        limits:
          memory: 1G
      restart_policy:
        condition: on-failure

  db:
    image: postgres:16-alpine
    networks:
      - backend
    volumes:
      - db-data:/var/lib/postgresql/data
    deploy:
      replicas: 1
      placement:
        constraints:
          - node.labels.storage == ssd
      restart_policy:
        condition: on-failure

volumes:
  db-data:
```

## Deploy Key Reference

| Key | Description |
|-----|-------------|
| `replicas` | Number of instances |
| `restart_policy` | Container restart rules |
| `update_config` | Rolling update settings |
| `resources` | CPU and memory settings |
| `placement` | Node constraints |
| `mode` | replicated or global |

### Restart Policy Options

| Option | Description |
|--------|-------------|
| `condition` | none, on-failure, any |
| `delay` | Delay between restarts |
| `max_attempts` | Restart attempts before giving up |
| `window` | Time to evaluate restart |

## Gotchas for Docker Users

- **Build not supported**: Stacks don't support `build:` - use pre-built images
- **Different compose version**: Use version 3+ for stacks
- **Network sharing**: Services in stack share overlay network automatically
- **Secrets work differently**: Use Swarm secrets, not compose secrets

## Common Mistakes

- **Using build:**: Build directive is ignored in stacks
- **Version too old**: Use 3.8 or later for all features
- **Forgetting restart policy**: Without it, containers don't restart on failure

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker stack deploy -c FILE NAME` | Deploy stack |
| `docker stack ls` | List stacks |
| `docker stack services NAME` | List stack services |
| `docker stack ps NAME` | List stack tasks |
| `docker stack rm NAME` | Remove stack |
| `docker stack inspect NAME` | Inspect stack |

## What's Next

Continue to [Secrets in Swarm](./02-secrets-in-swarm.md) to learn about secure configuration.
