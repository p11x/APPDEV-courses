# tmpfs

## Overview

tmpfs mounts store data in memory rather than on disk. This makes tmpfs ideal for sensitive data that shouldn't be written to disk, temporary files that don't need persistence, or performance-critical cache data. Since data is stored in RAM, tmpfs is extremely fast but limited by available memory and data is lost when the container stops.

## Prerequisites

- Understanding of container storage basics
- Knowledge of memory vs disk storage
- Familiarity with Docker volumes

## Core Concepts

### What is tmpfs?

tmpfs is a temporary file storage backed by memory:

- Data stored in RAM, not disk
- Extremely fast read/write
- Limited by available memory
- Data lost when container stops
- Linux-only feature

### When to Use tmpfs

- Sensitive data (passwords, keys)
- Temporary caches
- Session data
- Performance-critical buffers

## Step-by-Step Examples

### Basic tmpfs Usage

```bash
# Create tmpfs mount
# --tmpfs mounts a tmpfs filesystem
docker run -d \
  --tmpfs /app/cache \
  nginx

# Can't specify size directly with --tmpfs
# Use --mount for more options
docker run -d \
  --mount type=tmpfs,target=/app/cache,tmpfs-size=1000000000 \
  nginx
# tmpfs-size in bytes
```

### Sensitive Data

```bash
# Store secrets in memory
docker run -d \
  --tmpfs /run/secrets \
  myapp

# Secrets never written to disk
# Warning: visible in process list
```

### Performance-Critical Cache

```bash
# tmpfs for cache
docker run -d \
  --tmpfs /tmp/cache:rw,noexec,size=512m \
  myapp

# Options explained:
# rw = read-write
# noexec = no executable files
# size = maximum size (512 MB)
```

### tmpfs Options

```bash
# Full options syntax
docker run -d \
  --mount type=tmpfs,target=/app/data,tmpfs-mode=1770,tmpfs-size=1000000000 \
  myapp

# tmpfs-mode: permission mode (octal)
# tmpfs-size: size in bytes
```

### Comparing Storage Types

```bash
# Volume (persistent)
docker run -v myvolume:/data myapp

# Bind mount (host filesystem)
docker run -v /host/path:/data myapp

# tmpfs (memory)
docker run --tmpfs /data myapp
```

## Common Mistakes

- **Forgetting data is lost**: tmpfs data disappears when container stops.
- **Memory pressure**: tmpfs consumes RAM; too many can cause OOM.
- **Security concerns**: Sensitive data may be visible in process lists.
- **Swapping**: Linux may swap tmpfs to disk under memory pressure.
- **Not sizing properly**: Large tmpfs can exhaust memory.

## Quick Reference

| Feature | Volume | Bind Mount | tmpfs |
|---------|--------|------------|-------|
| Persistence | Yes | Yes | No |
| Storage | Disk | Disk | RAM |
| Performance | Good | Good | Best |
| Portability | High | Low | N/A |

| Flag | Description |
|------|-------------|
| --tmpfs | Simple tmpfs mount |
| --mount | Advanced options |
| tmpfs-size | Size limit in bytes |
| tmpfs-mode | Permissions |

## What's Next

Now that you understand all storage types, continue to [Compose File Structure](./../../compose/basics/01-compose-file-structure.md) to learn about Docker Compose for orchestrating multi-container applications.
