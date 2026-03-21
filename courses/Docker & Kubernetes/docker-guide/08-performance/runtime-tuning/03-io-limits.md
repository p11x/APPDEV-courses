# IO Limits

## Overview

Block I/O limits control how much storage throughput containers can use. This prevents noisy neighbor problems where one container's I/O affects others.

## Prerequisites

- Docker Engine 20.10+
- Understanding of storage performance
- Knowledge of I/O operations (IOPS)

## Core Concepts

### I/O Control

- **IOPS**: Input/Output Operations Per Second
- **Throughput**: MB/s data transfer
- **Block I/O**: Disk read/write operations

### Weight vs Limit

- **Weight**: Relative priority (0-1000)
- **Limit**: Absolute maximum

## Step-by-Step Examples

### Setting I/O Limits

```bash
# Limit read IOPS to 1000
docker run --device-read-iops=/dev/sda:1000 nginx

# Limit write IOPS to 500
docker run --device-write-iops=/dev/sda:500 nginx

# Limit read MB/s to 10MB
docker run --device-read-bps=/dev/sda:10mb nginx

# Limit write MB/s to 5MB
docker run --device-write-bps=/dev/sda:5mb nginx
```

### Combined Limits

```bash
# Multiple limits at once
docker run \
  --device-read-bps=/dev/sda:50mb \
  --device-write-bps=/dev/sda:25mb \
  --device-read-iops=/dev/sda:1000 \
  --device-write-iops=/dev/sda:500 \
  nginx
```

### Block I/O Weight

```bash
# Set relative weight (default 500)
docker run --blkio-weight=1000 nginx

# Weight range: 10-1000
# Higher = more priority
```

### Limiting All Devices

```bash
# Using blockio weight for all devices
docker run --blkio-weight=100 nginx
```

## Use Cases

### Backup Containers

```bash
# Throttle backup to not affect production
docker run \
  --device-write-bps=/dev/sda:10mb \
  --device-read-bps=/dev/sda:10mb \
  backup-container
```

### Database Containers

```bash
# Ensure database gets I/O priority
docker run --blkio-weight=800 database
```

## Gotchas for Docker Users

- **Device path**: Must specify actual device
- **cgroup v1**: Requires cgroup v1 for full support
- **Weight only**: Not a hard limit

## Common Mistakes

- **Wrong device**: Using wrong block device path
- **Too aggressive**: Very low limits cause timeouts
- **Weight vs limit**: Weight is relative, not absolute

## Quick Reference

| Flag | Description |
|------|-------------|
| --device-read-bps | Read bytes/sec |
| --device-write-bps | Write bytes/sec |
| --device-read-iops | Read ops/sec |
| --device-write-iops | Write ops/sec |
| --blkio-weight | Relative weight |

## What's Next

Continue to [Docker Stats Deep Dive](../../09-plugins-and-extensions/benchmarking/01-docker-stats-deep-dive.md) for monitoring.
