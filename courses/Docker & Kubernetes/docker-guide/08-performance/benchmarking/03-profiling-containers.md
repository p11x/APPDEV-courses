# Profiling Containers

## Overview

Profiling containerized applications helps identify performance bottlenecks. This guide covers techniques for analyzing CPU, memory, and I/O within containers.

## Prerequisites

- Running containers
- Profiling tools knowledge

## Core Concepts

### Profiling Methods

- **top/htop**: Basic CPU/memory
- **perf**: Hardware counters
- **strace**: System calls
- **pprof**: Go profiling

## Step-by-Step Examples

### Basic Profiling

```bash
# View processes in container
docker exec container top

# Interactive top
docker exec -it container htop

# View process tree
docker exec container ps aux
```

### Using perf (Privileged)

```bash
# Run with --privileged for perf
docker run --privileged myapp \
  sh -c "apt-get update && apt-get install -y linux-perf"

# CPU profiling
docker exec container perf record -g -p $(pgrep myapp)

# View report
docker exec container perf report
```

### System Call Tracing

```bash
# Trace system calls
docker exec container strace -c -p $(pgrep myapp)

# Trace specific calls
docker exec container strace -e openat,read,write -p $(pgrep myapp)

# Trace file access
docker exec container strace -f -e openat myapp
```

### Go pprof

```bash
# Enable pprof in Go app
# Add to code:
import _ "net/http/pprof"

# Start server:
go run -pprof :6060 main.go

# Access:
# http://localhost:6060/debug/pprof/
```

## Gotchas for Docker Users

- **Privileged needed**: Most profiling needs --privileged
- **Performance impact**: Profiling slows containers
- **Tool installation**: May need to install in container

## Quick Reference

| Tool | Purpose |
|------|---------|
| top | Basic CPU/memory |
| htop | Interactive view |
| perf | Hardware counters |
| strace | System calls |
| pprof | Go profiling |

## What's Next

Continue to [REX-Ray Volume Plugin](../../09-plugins-and-extensions/volume-plugins/01-rexray.md) for storage plugins.
