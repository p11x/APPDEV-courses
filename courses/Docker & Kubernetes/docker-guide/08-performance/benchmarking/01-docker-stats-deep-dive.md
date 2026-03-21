# Docker Stats Deep Dive

## Overview

`docker stats` provides real-time resource usage metrics for running containers. Understanding all its output options helps you monitor container performance effectively.

## Prerequisites

- Docker Engine 20.10+
- Running containers to monitor

## Core Concepts

### Metrics Available

- CPU percentage
- Memory usage and limits
- Network I/O
- Block I/O
- Process count

## Step-by-Step Examples

### Basic Usage

```bash
# View stats for all running containers
docker stats

# Stream stats (updates every second)
docker stats --no-stream

# One-time snapshot
docker stats --no-stream
```

### Output Format

```bash
# Default output:
# CONTAINER ID   NAME                CPU %   MEM USAGE / LIMIT     MEM %   NET I/O           BLOCK I/O           PIDS
# abc123         nginx               1.25%   50MiB / 256MiB       19.53%  1MB / 500KB       10MB / 0B          10
```

### Custom Format

```bash
# Format output using Go templates
docker stats --format "table {{.Name}}\t{{.CPUPerc}}\t{{.MemUsage}}"

# Show only specific columns
docker stats --format "{{.Name}}: {{.CPUPerc}}"

# JSON output
docker stats --format "{{json .}}"
```

### Filtered Stats

```bash
# Stats for specific container
docker stats my-container

# Multiple containers
docker stats container1 container2

# Filter by name pattern
docker stats --filter "name=nginx"
```

### Using with Other Tools

```bash
# Get stats as JSON for processing
docker stats --no-stream --format "{{json .}}" | jq

# Monitor specific metrics
docker stats --no-stream --format "{{.Name}} {{.CPUPerc}} {{.MemUsage}}"
```

## Column Reference

| Column | Description |
|--------|-------------|
| CONTAINER ID | Unique container identifier |
| NAME | Container name |
| CPU % | CPU usage percentage |
| MEM USAGE | Current memory used / limit |
| MEM % | Memory usage percentage |
| NET I/O | Network RX / TX |
| BLOCK I/O | Disk read / write |
| PIDS | Number of processes |

## Gotchas for Docker Users

- **Updates**: Default updates every second
- **No historical**: Only shows current state
- **All containers**: Shows all by default

## Common Mistakes

- **Not using --no-stream**: Unnecessary for scripting
- **Forgetting limits**: MEM USAGE shows limit too

## Quick Reference

| Flag | Description |
|------|-------------|
| --no-stream | Single output |
| --format | Custom format |
| --filter | Filter containers |

## What's Next

Continue to [cAdvisor Setup](./02-cadvisor-setup.md) for advanced metrics.
