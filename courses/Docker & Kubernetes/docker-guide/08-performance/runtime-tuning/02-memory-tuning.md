# Memory Tuning

## Overview

Proper memory configuration prevents containers from consuming too much host memory while ensuring applications have enough to run efficiently. This guide covers Docker's memory management options.

## Prerequisites

- Docker Engine 20.10+
- Understanding of memory concepts
- Application memory requirements knowledge

## Core Concepts

### Memory Limits

- **Hard limit**: Container cannot exceed this
- **Soft limit**: Container can use more if available
- **Swap**: Memory + swap combined

### OOM (Out of Memory)

- Exit code 137 indicates OOM kill
- Host kernel terminates container
- Container can't recover

## Step-by-Step Examples

### Setting Memory Limits

```bash
# Hard memory limit
docker run --memory=512m nginx

# Memory with swap limit
docker run --memory=512m --memory-swap=1g nginx
# 512m RAM + 512m swap

# Disable swap entirely
docker run --memory=1g --memory-swap=-1 nginx
```

### Soft Limits

```bash
# Soft limit (guaranteed minimum)
docker run --memory-reservation=256m nginx

# Combined hard and soft limits
docker run \
  --memory=1g \
  --memory-reservation=512m \
  nginx
```

### JVM Memory in Containers

```bash
# JVM heap should be less than container limit
docker run \
  --memory=1g \
  -e JAVA_OPTS="-XX:MaxRAMFraction=2 -Xmx512m" \
  myjavaapp

# Better: Set exact heap
docker run \
  --memory=1g \
  -e JAVA_OPTS="-Xms256m -Xmx512m" \
  myjavaapp
```

### Detecting OOM

```bash
# Check if container was OOM killed
docker inspect container --format '{{.State.OOMKilled}}'

# Check exit code
docker inspect container --format '{{.State.ExitCode}}'
# 137 = 128 + 9 (SIGKILL)

# View OOM events
docker events --filter 'event=oom'
```

### Preventing OOM Kill

```bash
# Disable OOM killer for container
docker run --memory=512m --oom-kill-disable nginx

# DANGEROUS: Can cause host OOM instead
# Only use with proper memory limits
```

## Memory Metric Monitoring

```bash
# View memory usage
docker stats container

# Memory fields:
# MEM USAGE / LIMIT - Current / Maximum
# MEM % - Usage percentage
```

## Gotchas for Docker Users

- **Swap behavior**: Default --memory-swap equals memory limit
- **JVM tuning**: JVM doesn't see container limits by default
- **OOM kill**: Happens at kernel level, Docker can't prevent

## Common Mistakes

- **No limits**: Container can consume all host memory
- **JVM ignoring limits**: Needs explicit heap configuration
- **Swap confusion**: Not understanding memory-swap relationship

## Quick Reference

| Flag | Description |
|------|-------------|
| --memory | Hard limit |
| --memory-reservation | Soft limit |
| --memory-swap | Swap limit |
| --oom-kill-disable | Disable OOM kill |

| Exit Code | Meaning |
|-----------|---------|
| 0 | Success |
| 137 | OOM killed (128+9) |

## What's Next

Continue to [IO Limits](./03-io-limits.md) for storage performance.
