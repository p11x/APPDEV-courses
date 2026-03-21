# Container Lifecycle

## Overview

Understanding the container lifecycle is fundamental to managing Docker containers effectively. Containers go through distinct states from creation to deletion, and knowing how to control these transitions is essential for debugging, deployment, and automation. This guide covers the complete lifecycle and how to manage containers at each stage.

## Prerequisites

- Understanding of docker run command
- Basic command-line knowledge
- Familiarity with process management concepts

## Core Concepts

### Container States

A Docker container can be in one of these states:

- **Created**: Container is created but hasn't started yet
- **Running**: Container is executing with its main process
- **Paused**: Container processes are suspended
- **Stopped**: Container has exited (the main process terminated)
- **Restarting**: Container is in the process of restarting
- **Dead**: Container exists but couldn't be removed (rare)

### Container ID vs Name

Every container has:
- **Container ID**: 64-character hexadecimal string (first 12 characters often used)
- **Name**: User-friendly string (auto-generated if not specified)

### Process in Container

Containers run a primary process (PID 1). When this process exits, the container stops:

```bash
# Container runs until nginx exits
docker run -d nginx

# Container runs until bash exits (then container stops)
docker run -it alpine /bin/sh
# If you type "exit", container stops
```

## Step-by-Step Examples

### Creating and Starting Containers

```bash
# Create container without starting
# docker create creates but doesn't start
docker create --name my-container nginx

# Check status
docker ps -a | grep my-container

# Start the created container
docker start my-container

# Or run directly (create + start)
docker run --name my-container nginx
```

### Starting and Stopping

```bash
# Start a stopped container
docker start my-container

# Stop a running container gracefully
# Sends SIGTERM, waits for graceful shutdown
docker stop my-container

# Stop with timeout (default 10 seconds)
docker stop -t 30 my-container

# Force stop (sends SIGKILL immediately)
docker kill my-container

# Restart a container (stop + start)
docker restart my-container

# Pause container (suspend processes)
docker pause my-container

# Unpause container (resume processes)
docker unpause my-container
```

### Viewing Container Status

```bash
# List running containers
docker ps

# List all containers (including stopped)
docker ps -a

# List containers with size
docker ps -as

# Filter containers
docker ps -f "status=exited"
docker ps -f "status=running"
docker ps -f "name=nginx"

# Inspect container details
docker inspect my-container

# Get specific field
docker inspect --format='{{.State.Status}}' my-container
docker inspect --format='{{.State.Pid}}' my-container

# View container logs
docker logs my-container

# Follow logs in real-time
docker logs -f my-container

# View logs with timestamps
docker logs -t my-container
```

### Removing Containers

```bash
# Remove stopped container
docker rm my-container

# Force remove running container
docker rm -f my-container

# Remove multiple containers
docker rm container1 container2 container3

# Remove all stopped containers
docker container prune -f

# Remove containers by filter
docker rm $(docker ps -aq -f status=exited)

# Remove all containers (including running)
docker rm -f $(docker ps -aq)
```

### Executing Commands in Containers

```bash
# Run command in running container
# docker exec runs additional process in container
docker exec my-container ls /

# Interactive shell
docker exec -it my-container /bin/sh

# Run as specific user
docker exec -u 1000 my-container whoami

# Run in specific working directory
docker exec -w /app my-container pwd
```

### Container Events and Stats

```bash
# Real-time container events
docker events

# Filter events by container
docker events -f container=my-container

# Resource usage stats
docker stats my-container

# Stats for all containers
docker stats

# Stats with no streaming (one-time)
docker stats --no-stream my-container
```

### Complete Lifecycle Example

```bash
# Start a container
docker run -d --name web nginx

# Check it's running
docker ps

# Pause it (maintains state)
docker pause web

# Check status
docker inspect --format='{{.State.Status}}' web

# Unpause to resume
docker unpause web

# Stop gracefully
docker stop web

# Check it's stopped
docker ps -a

# Start again
docker start web

# Remove when done
docker stop web
docker rm web
```

## Common Mistakes

- **Not stopping before removing**: Always stop containers before rm unless using -f.
- **Confusing pause with stop**: Pause suspends processes; stop terminates them.
- **Forgetting container names**: Use --name to make containers easier to manage.
- **Not cleaning up**: Stopped containers consume disk space.
- **Kill vs stop**: Kill is immediate (SIGKILL); stop is graceful (SIGTERM).
- **Not using exec**: Can't run commands in stopped containers.

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker create` | Create container |
| `docker start` | Start container |
| `docker stop` | Stop gracefully |
| `docker kill` | Stop forcefully |
| `docker restart` | Stop and start |
| `docker pause` | Suspend processes |
| `docker unpause` | Resume processes |
| `docker rm` | Remove container |
| `docker exec` | Run command in container |
| `docker logs` | View logs |
| `docker stats` | Resource usage |
| `docker inspect` | Detailed info |

| State | Meaning |
|-------|----------|
| created | Created but not started |
| running | Executing |
| paused | Suspended |
| exited | Terminated |
| restarting | Restarting |
| dead | Failed to remove |

## What's Next

Now that you understand container lifecycle, continue to [Resource Limits](./03-resource-limits.md) to learn how to constrain CPU, memory, and other resources for containers.
