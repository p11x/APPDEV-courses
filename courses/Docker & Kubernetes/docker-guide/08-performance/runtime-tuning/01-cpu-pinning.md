# CPU Pinning

## Overview

CPU pinning (CPU affinity) allows you to bind containers to specific CPU cores on the host. This can improve performance for latency-sensitive workloads and enable NUMA-aware deployments.

## Prerequisites

- Docker Engine 20.10+
- Multi-core host system
- Understanding of CPU architecture

## Core Concepts

### CPU Scheduling

- By default, containers can use any CPU
- Pinning restricts to specific cores
- Useful for real-time applications

### CPU Share vs Limit vs Pinning

- **--cpu-shares**: Relative weight (0-1024)
- **--cpus**: Hard limit (fractional CPUs)
- **--cpuset-cpus**: Pin to specific cores

## Step-by-Step Examples

### Pinning to Specific Cores

```bash
# Pin container to CPU 0 only
docker run --cpuset-cpus=0 nginx

# Pin to CPUs 0 and 1 (dual-core)
docker run --cpuset-cpus=0,1 nginx

# Pin to CPUs 0-3 (quad-core)
docker run --cpuset-cpus=0-3 nginx
```

### Setting CPU Limits

```bash
# Limit to 0.5 CPU cores
docker run --cpus=0.5 nginx

# Limit to 2 CPU cores
docker run --cpus=2 nginx
```

### CPU Shares (Relative Weight)

```bash
# Set relative CPU shares
# Default is 1024
docker run --cpu-shares=512 nginx
# Container gets half the CPU of a default container
```

### Combining Options

```bash
# Pin to specific CPUs with limit
docker run \
  --cpuset-cpus=0,1 \
  --cpus=1.5 \
  --cpu-shares=1024 \
  nginx
```

### NUMA Awareness

```bash
# Check NUMA topology
numactl --hardware

# Pin to NUMA node
docker run --cpuset-mems=0 nginx
# Only use memory from NUMA node 0
```

## Use Cases

### High-Performance Computing

```bash
# Scientific computing
docker run --cpuset-cpus=0-7 --memory=16g compute-app
```

### Real-Time Systems

```bash
# Low-latency requirements
docker run --cpuset-cpus=0 --cpus=1 realtime-app
```

## Gotchas for Docker Users

- **Available cores**: Pinning to non-existent CPU fails
- **Overcommit**: Can pin more containers than cores
- **Host impact**: Pinned containers affect host scheduling

## Common Mistakes

- **Wrong CPU number**: CPUs are 0-indexed
- **Over-subscription**: Too many containers on few cores
- **Ignoring NUMA**: Performance impact on multi-socket systems

## Quick Reference

| Flag | Description |
|------|-------------|
| --cpuset-cpus | Pin to specific CPUs |
| --cpuset-cpus=0-3 | Pin to range |
| --cpus | Hard limit |
| --cpu-shares | Relative weight |
| --cpuset-mems | NUMA node |

## What's Next

Continue to [Memory Tuning](./02-memory-tuning.md) for memory optimization.
