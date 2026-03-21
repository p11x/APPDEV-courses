# Dockerfile Advanced

## Overview

Advanced Dockerfile techniques enable you to build more efficient, secure, and maintainable images. This guide covers multi-stage builds, build arguments, health checks, security best practices, and optimization strategies that professional Docker users employ in production environments.

## Prerequisites

- Understanding of basic Dockerfile instructions (FROM, RUN, COPY, CMD)
- Knowledge of Docker build process
- Familiarity with Linux commands and shell scripting

## Core Concepts

### Build Arguments (ARG)

Build arguments are variables defined in the Dockerfile that can be passed at build time. They're useful for customizing builds without modifying the Dockerfile:

```dockerfile
ARG VERSION=latest
FROM nginx:${VERSION}
```

### Environment Variables (ENV)

Environment variables set persistent variables available to the container at runtime:

```dockerfile
ENV NODE_ENV=production
ENV APP_PORT=3000
```

### Health Checks (HEALTHCHECK)

Health checks tell Docker how to test if a container is working correctly:

```dockerfile
HEALTHCHECK --interval=30s --timeout=3s \
  CMD curl -f http://localhost:3000/health || exit 1
```

### Labels (LABEL)

Labels add metadata to images without affecting functionality:

```dockerfile
LABEL maintainer="you@example.com"
LABEL version="1.0"
LABEL description="My application"
```

### Shell Form vs Exec Form

Commands can use shell or exec form:

- **Shell form**: `RUN npm install` - runs in /bin/sh -c
- **Exec form**: `RUN ["npm", "install"]` - runs directly without shell

Always prefer exec form for CMD and ENTRYPOINT to avoid shell interpretation issues:

```dockerfile
# Good - exec form
CMD ["node", "server.js"]

# Problematic - shell form
CMD node server.js
```

### Signal Handling

Containers receive SIGTERM for graceful shutdown. Ensure your application handles this:

```dockerfile
# Use exec form so Node receives signals directly
CMD ["node", "server.js"]

# For shell scripts, use trap:
# trap "exit" SIGTERM
```

## Step-by-Step Examples

### Using Build Arguments

```dockerfile
# Dockerfile
FROM node:20-alpine

# ARG must come before FROM if you want to use it in FROM
ARG VERSION=latest
ARG NODE_ENV=production

# Define build argument (available during build)
ARG BUILD_DATE
# Use build argument in labels
LABEL build-date=${BUILD_DATE}

# Set environment variable from ARG
ENV NODE_ENV=${NODE_ENV}

WORKDIR /app

COPY package*.json ./
RUN npm ci --only=production

COPY . .

EXPOSE 3000

CMD ["node", "server.js"]
```

Build with arguments:
```bash
# Build with custom version and build date
docker build \
  --build-arg VERSION=1.0 \
  --build-arg BUILD_DATE=$(date -u +"%Y-%m-%dT%H:%M:%SZ") \
  -t myapp:1.0 .
```

### Multi-Environment Configuration

```dockerfile
# Support both development and production
ARG NODE_ENV=production

FROM node:20-alpine

WORKDIR /app

# Copy dependency files
COPY package*.json ./

# Install dependencies based on environment
RUN if [ "$NODE_ENV" = "production" ]; then \
      npm ci --only=production; \
    else \
      npm install; \
    fi

# Copy source
COPY . .

# Expose port
EXPOSE 3000

# Set environment
ENV NODE_ENV=${NODE_ENV}

CMD ["node", "server.js"]
```

Build for development:
```bash
docker build --build-arg NODE_ENV=development -t myapp:dev .
```

### Health Checks

```dockerfile
FROM nginx:1.25-alpine

# Copy custom config
COPY nginx.conf /etc/nginx/nginx.conf

# Health check - test every 30 seconds, timeout after 3 seconds
# Docker considers container unhealthy if check fails 3 times
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost/ || exit 1

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

Test health check:
```bash
# Build and run
docker build -t nginx-health .
docker run -d --name nginx-test nginx-health

# Check health status
docker inspect nginx-test --format='{{.State.Health.Status}}'

# View health check logs
docker inspect nginx-test --format='{{json .State.Health.Log}}'
```

### Best Practices Implementation

```dockerfile
# Use specific version tags - never latest
FROM node:20.11.1-alpine3.19

# Labels for metadata
LABEL maintainer="devteam@example.com" \
      version="1.0.0" \
      description="Backend API service"

# Set working directory
WORKDIR /app

# Copy package files first for better caching
COPY package*.json ./

# Install production dependencies only
RUN npm ci --only=production

# Copy source code
COPY . .

# Create non-root user
RUN addgroup -g 1001 -S nodejs && \
    adduser -S nodejs -u 1001

# Set ownership
RUN chown -R nodejs:nodejs /app

# Switch to non-root user
USER nodejs

# Expose application port
EXPOSE 3000

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s \
  CMD node -e "require('http').get('http://localhost:3000/health', (r) => process.exit(r.statusCode === 200 ? 0 : 1))"

# Use exec form
CMD ["node", "server.js"]
```

### Using BuildKit Features

Enable BuildKit for advanced features:

```bash
# Enable BuildKit
export DOCKER_BUILDKIT=1

# Or enable in Docker Desktop settings
```

```dockerfile
# syntax=docker/dockerfile:1
# Enable syntax directive for experimental features

# Import cache from registry
# syntax=docker/dockerfile:1
FROM docker.io/library/alpine:latest AS base
RUN echo "base"

# Use cache from for layer caching
FROM base AS builder
RUN echo "builder"

# Use cache-from
# docker build --cache-from=myapp:build-cache -t myapp .
```

## Common Mistakes

- **Not using version tags**: Using `FROM node:latest` makes builds non-reproducible. Always use specific versions.
- **Running as root**: This is a security risk. Always create and use a non-root user.
- **Not cleaning up in the same layer**: Files deleted in a later RUN still exist in the image layer. Clean up in the same command.
- **Ignoring shell form issues**: Shell form doesn't handle signals properly and can cause issues with container management.
- **Not using .dockerignore**: Unnecessary files bloat the build context and can expose secrets.
- **COPY vs ADD confusion**: Use COPY for simple file copies. ADD handles URLs and tar extraction but is less explicit.
- **Forgetting health checks**: Without health checks, Docker can't detect if your application is actually working.

## Quick Reference

| Instruction | Purpose |
|-------------|---------|
| ARG | Build-time variables |
| ENV | Runtime environment variables |
| HEALTHCHECK | Container health test |
| LABEL | Image metadata |
| USER | Switch to non-root user |
| SHELL | Change shell for RUN commands |

| Best Practice | Implementation |
|--------------|----------------|
| Use specific versions | FROM node:20.11.1-alpine |
| Non-root user | USER nodejs |
| Minimal layers | Combine RUN commands |
| Cache optimization | Copy deps first |
| Health checks | HEALTHCHECK CMD |

## What's Next

Now that you understand advanced Dockerfile techniques, continue to [Multi-Stage Builds](./03-multi-stage-builds.md) to learn how to create smaller, more efficient production images.
