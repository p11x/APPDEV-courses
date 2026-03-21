# Bind Mounts

## Overview

Bind mounts map a specific host file or directory into a container. Unlike volumes, bind mounts use the host's filesystem directly, making them perfect for development workflows where you want to edit code on your host and see changes immediately in the container. Bind mounts are commonly used for development environments, configuration files, and log access.

## Prerequisites

- Understanding of Docker basics
- Knowledge of filesystem paths
- Familiarity with container storage

## Core Concepts

### How Bind Mounts Work

Bind mounts map an absolute path from the host into the container:

- Path must be absolute (not relative)
- File or directory is mounted directly
- Changes are immediate (no copy)
- Host filesystem is authoritative

### Bind Mounts vs Volumes

| Feature | Bind Mount | Volume |
|---------|-----------|--------|
| Location | Host filesystem | Docker-managed |
| Management | External | Docker-managed |
| Performance | Direct I/O | Slightly slower |
| Portability | Host-dependent | Portable |
| Use case | Development | Production data |

## Step-by-Step Examples

### Basic Bind Mount

```bash
# Mount host directory into container
# -v /absolute/path:/container/path
docker run -v /my/project:/app myimage

# Mount current directory
docker run -v $(pwd):/app myimage
```

### Development Workflow

```bash
# Mount source code for live editing
docker run -d \
  -p 3000:3000 \
  -v $(pwd)/src:/app/src \
  -v $(pwd)/package.json:/app/package.json \
  node:20-alpine

# Changes in ./src appear immediately in container
# No rebuild needed for code changes
```

### Configuration Files

```bash
# Mount nginx config
docker run -d \
  -v $(pwd)/nginx.conf:/etc/nginx/nginx.conf:ro \
  -p 80:80 \
  nginx

# Mount SSL certificates
docker run -d \
  -v $(pwd)/certs:/etc/nginx/certs:ro \
  -p 443:443 \
  nginx
```

### Log Access

```bash
# Access container logs from host
docker run -d \
  -v /var/log/myapp:/var/log \
  myapp

# View logs from host
tail -f /var/log/myapp/application.log
```

### Read-Only Mounts

```bash
# Prevent container from modifying host files
# :ro makes mount read-only
docker run -v $(pwd)/config:/app/config:ro myapp
```

### Docker Compose with Bind Mounts

```yaml
# docker-compose.yml for development
version: "3.8"

services:
  web:
    image: node:20-alpine
    volumes:
      - ./src:/app/src
      - ./package.json:/app/package.json
    working_dir: /app
    command: npm start
    ports:
      - "3000:3000"
```

### Windows Paths

```bash
# Windows paths use forward slashes or double backslashes
docker run -v C:/Users/me/project:/app myimage

# Or with PowerShell variable
docker run -v ${PWD}:/app myimage
```

## Common Mistakes

- **Relative paths**: Always use absolute paths or $(pwd) for current directory.
- **Permission issues**: Container may not have permission to read/write host files.
- **Path not existing**: Host directory must exist before mounting.
- **Security risks**: Container has full access to mounted host path.
- **Forgetting :ro**: Bind mounts are writable by default.

## Quick Reference

| Syntax | Description |
|--------|-------------|
| `-v /host:/container` | Full access |
| `-v /host:/container:ro` | Read-only |
| `-v /host:/container:rw` | Read-write (default) |

| Use Case | Mount Type |
|----------|-----------|
| Development | Bind mount |
| Production data | Volume |
| Config files | Bind mount |
| Databases | Volume |

## What's Next

Now that you understand bind mounts, continue to [tmpfs](./03-tmpfs.md) to learn about temporary storage in memory.
