# Resource Limits

## Overview

Resource limits control how much CPU, memory, disk, and other system resources a container can use. Setting appropriate limits is crucial for production deployments, preventing any single container from consuming all available resources and ensuring fair resource allocation across multiple containers.

## Prerequisites

- Understanding of container lifecycle
- Basic knowledge of system resources (CPU, memory)
- Familiarity with docker run command

## Core Concepts

### Why Set Resource Limits?

Without limits, containers can:
- Consume all available RAM, causing other containers to fail
- Use all CPU cycles, slowing down other processes
- Fill up disk space with logs
- Cause system instability

### Types of Limits

**Memory Limits**:
- Hard limit: Maximum memory container can use
- Soft limit: Warning threshold (for cgroups v2)

**CPU Limits**:
- Absolute: Number of CPU cores
- Relative: Share of CPU time

**Block I/O Limits**:
- Read/Write bandwidth limits
- Read/Write IOPS limits

**PIDs Limit**:
- Maximum number of processes

## Step-by-Step Examples

### Memory Limits

```bash
# Set memory limit (most common)
# -m or --memory sets the maximum RAM
docker run -m 512m nginx

# Set memory limit with swap
# container can use 512m RAM + 512m swap
docker run -m 512m --memory-swap=1g nginx

# Set memory reservation (minimum guaranteed)
# container gets at least 256m, can use up to 512m
docker run -m 512m --memory-reservation=256m nginx

# Set kernel memory limit (advanced)
docker run -m 512m --kernel-memory=256m nginx

# Disable OOM killer (dangerous!)
# container won't be killed even if it exceeds limit
docker run -m 512m --oom-kill-disable nginx

# Set OOM score adjustment
docker run -m 512m --oom-score-adj=500 nginx
```

### CPU Limits

```bash
# Limit to 1 CPU core
# --cpus specifies number of CPU cores
docker run --cpus=1 nginx

# Limit to 1.5 CPU cores
docker run --cpus=1.5 nginx

# Limit to specific CPUs (CPU affinity)
# --cpuset-cpus binds container to specific cores
docker run --cpuset-cpus=0,1 nginx
# This binds to first two cores

# Limit to specific NUMA node
docker run --cpuset-mems=0 nginx

# Set CPU shares (relative priority)
# default is 1024, higher = more CPU time
docker run --cpu-shares=2048 nginx

# Set CPU period and quota (cgroups v1)
# container can use 50% of CPU every 100ms
docker run --cpu-period=100000 --cpu-quota=50000 nginx
```

### Block I/O Limits

```bash
# Limit read rate
# --device-read-bps limits bytes per second
docker run --device-read-bps /dev/sda:1mb nginx

# Limit write rate
docker run --device-write-bps /dev/sda:1mb nginx

# Limit read IOPS
docker run --device-read-iops /dev/sda:100 nginx

# Limit write IOPS
docker run --device-write-iops /dev/sda:100 nginx
```

### Process Limits

```bash
# Limit number of processes
# -p or --pids-limit sets max processes
docker run --pids-limit=100 nginx

# Disable process limit
docker run --pids-limit=-1 nginx
```

### Network Limits

```bash
# Note: Docker doesn't have built-in network rate limiting
# Usetc or third-party tools for this
# Example using tc in a container:
docker run --cap-add=NET_ADMIN alpine \
  tc qdisc add dev eth0 root tbf rate 100mbit burst 50kb latency 50ms
```

### Setting Multiple Limits

```bash
# Combine multiple limits
docker run -d \
  --name production-app \
  -m 1g \
  --cpus=2 \
  --cpuset-cpus=0,1 \
  --pids-limit=200 \
  myapp:latest
```

### Updating Limits on Running Containers

```bash
# Update limits on running container
docker update --memory=512m --cpus=1 my-container

# Update multiple containers
docker update -m 512m container1 container2 container3

# Update all containers to have limits
docker update --cpus=1 $(docker ps -q)
```

### Monitoring Resource Usage

```bash
# View real-time stats
docker stats

# View specific container stats
docker stats my-container

# Stats without streaming (one-time)
docker stats --no-stream my-container

# Format stats output
docker stats --format "table {{.Name}}\t{{.CPUPerc}}\t{{.MemUsage}}"

# Check current limits
docker inspect --format='{{.HostConfig.Memory}}' my-container
docker inspect --format='{{.HostConfig.CpusetCpus}}' my-container
```

## Common Mistakes

- **Setting memory too low**: Applications may crash with OOM errors. Monitor actual usage.
- **Not setting any limits**: A runaway container can crash the entire system.
- **Confusing CPU shares with limits**: Shares are relative; limits are absolute.
- **Setting swap to 0**: Disabling swap can cause unexpected OOM kills.
- **Not monitoring**: Always monitor actual resource usage vs limits.
- **Ignoring I/O limits**: Disk I/O can also cause performance issues.

## Quick Reference

| Flag | Description | Example |
|------|-------------|---------|
| -m, --memory | Memory limit | 512m, 1g |
| --memory-swap | Swap limit | 1g |
| --memory-reservation | Soft limit | 256m |
| --cpus | CPU limit | 1.5, 2 |
| --cpuset-cpus | CPU affinity | 0,1 |
| --cpu-shares | CPU priority | 1024 |
| --pids-limit | Process limit | 100 |
| --device-read-bps | Read rate | /dev/sda:1mb |

| Resource | Default | Recommended |
|----------|---------|-------------|
| Memory | Unlimited | 256m-2g |
| CPU | Unlimited | 0.5-2 cores |
| Processes | Unlimited | 100-1000 |

## What's Next

Now that you understand resource limits, continue to [Bridge Networks](./../networking/01-bridge-networks.md) to learn about Docker networking fundamentals.
