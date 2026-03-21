# Reducing Image Size

## Overview

Smaller Docker images mean faster deployments, reduced storage costs, and smaller attack surfaces. This guide covers practical techniques for minimizing image sizes without sacrificing functionality.

## Prerequisites

- Docker Engine 20.10+
- Understanding of Dockerfile basics
- Familiarity with layers concept

## Core Concepts

### Why Image Size Matters

- **Pull time**: Smaller images deploy faster
- **Storage**: Less disk space needed
- **Attack surface**: Fewer packages = fewer vulnerabilities
- **Memory**: Smaller memory footprint

### Size Comparison

| Base Image | Size |
|------------|------|
| ubuntu:latest | ~77 MB |
| ubuntu:22.04 | ~77 MB |
| ubuntu:22.04-slim | ~29 MB |
| alpine:latest | ~7 MB |
| distroless | ~2 MB |

## Step-by-Step Examples

### Using Smaller Base Images

```dockerfile
# Bad: Full Ubuntu image
FROM ubuntu:22.04
RUN apt-get update && apt-get install -y nginx

# Good: Alpine-based image (~7 MB)
FROM alpine:3.19
RUN apk add nginx

# Better: Official slim variant
FROM nginx:1.25-slim
```

### Combining RUN Commands

```dockerfile
# Bad: Multiple RUN = multiple layers
FROM alpine:3.19
RUN apk add nginx
RUN apk add curl
RUN rm -rf /var/cache/apk/*

# Good: Single RUN, chained commands
FROM alpine:3.19
RUN apk add --no-cache nginx curl
# --no-cache removes apk cache in same layer
```

### Using Multi-Stage Builds

```dockerfile
# Build stage
FROM golang:1.21 AS builder
WORKDIR /app
COPY . .
RUN CGO_ENABLED=0 GOOS=linux go build -o myapp

# Runtime stage - tiny final image
FROM alpine:3.19
WORKDIR /app
COPY --from=builder /app/myapp .
CMD ["./myapp"]
```

### .dockerignore

```dockerfile
# .dockerignore file
# Exclude unnecessary files from build context
.git
.gitignore
node_modules
*.md
README.md
docs/
tests/
*.log
.env
.vscode/
.idea/
```

### Order of Instructions

```dockerfile
# Bad: Items that change often first
FROM alpine:3.19
COPY . .              # Changes every build
RUN apk add nginx     # Rarely changes
CMD ["nginx"]

# Good: Least-changing items first
FROM alpine:3.19
RUN apk add nginx     # Rarely changes
COPY . .              # Changes every build
CMD ["nginx"]
```

### Removing Unnecessary Files

```dockerfile
# Remove documentation, man pages
RUN apk add --no-cache nginx \
    && rm -rf /usr/share/man/* \
    && rm -rf /var/cache/apk/* \
    && rm -rf /var/log/*.log

# Remove temporary files
RUN rm -rf /tmp/*
```

## Image Optimization Checklist

1. Use Alpine or Slim base images
2. Combine RUN commands to reduce layers
3. Use multi-stage builds for compiled languages
4. Create proper .dockerignore
5. Order instructions from least to most volatile
6. Remove unnecessary files in same RUN layer

## Gotchas for Docker Users

- **Slim vs Alpine**: Alpine uses musl libc, may have compatibility issues
- **Cache invalidation**: Order matters for build cache
- **--no-cache**: Alpine's apk needs this flag

## Common Mistakes

- **Not using --no-cache**: Leaves apk cache in image
- **Too many layers**: Each RUN creates a layer
- **Not using .dockerignore**: Including unnecessary files slows build

## Quick Reference

| Technique | Savings |
|-----------|---------|
| Alpine over Ubuntu | ~70 MB |
| Multi-stage build | Variable |
| .dockerignore | Depends |
| Combined RUN | Per layer saved |

## What's Next

Continue to [Distroless Images](./02-distroless-images.md) for minimal images.
