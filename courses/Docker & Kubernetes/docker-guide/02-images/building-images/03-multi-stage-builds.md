# Multi-Stage Builds

## Overview

Multi-stage builds are a powerful Docker feature that allows you to use multiple FROM statements in a single Dockerfile. This technique lets you compile your application in one stage with all build tools, then copy only the final artifacts to a minimal production image. The result is dramatically smaller images that contain only what's needed to run your application.

## Prerequisites

- Understanding of basic Dockerfile instructions
- Knowledge of build tools and compilation
- Familiarity with Docker layer concepts

## Core Concepts

### How Multi-Stage Builds Work

A multi-stage Dockerfile has multiple FROM statements, each starting a new build stage. You can name stages using AS and copy artifacts between stages:

```dockerfile
# Stage 1: Build
FROM node:20-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Stage 2: Production
FROM node:20-alpine
WORKDIR /app
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/node_modules ./node_modules
CMD ["node", "dist/server.js"]
```

### Benefits

1. **Smaller images**: Only runtime artifacts are included, not build dependencies
2. **Better security**: Build tools and source code aren't in the final image
3. **Faster deployments**: Smaller images mean faster pulls and deployments
4. **Clean separation**: Build and runtime environments are isolated
5. **Single Dockerfile**: No separate build scripts needed

### Stage Naming

Name stages using AS to reference them later:

```dockerfile
FROM node:20-alpine AS builder
# ... build steps ...

FROM node:20-alpine AS test
# ... test steps ...

FROM node:20-alpine AS production
COPY --from=builder /app/dist ./dist
```

## Step-by-Step Examples

### Go Application

Go is particularly well-suited for multi-stage builds because it produces a single static binary:

```dockerfile
# Stage 1: Build
FROM golang:1.21-alpine AS builder

# Install build dependencies
RUN apk add --no-cache git

WORKDIR /build

# Copy go mod files
COPY go.mod go.sum ./

# Download dependencies
RUN go mod download

# Copy source code
COPY . .

# Build with static linking
# CGO_ENABLED=0 creates a static binary
# -ldflags sets build variables
RUN CGO_ENABLED=0 GOOS=linux go build \
    -ldflags="-s -w" \
    -o myapp

# Stage 2: Production
FROM alpine:3.19

# Install CA certificates for HTTPS
RUN apk add --no-cache ca-certificates

WORKDIR /app

# Copy binary from builder stage
# --from=builder references the named stage
COPY --from=builder /build/myapp .

# Create non-root user
RUN adduser -D -g '' appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8080

CMD ["./myapp"]
```

### Node.js Application

```dockerfile
# Stage 1: Dependencies
FROM node:20-alpine AS deps

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install ALL dependencies (including dev)
RUN npm ci

# Stage 2: Builder
FROM node:20-alpine AS builder

WORKDIR /app

# Copy dependencies from deps stage
COPY --from=deps /app/node_modules ./node_modules
COPY package*.json ./
COPY . .

# Build the application
RUN npm run build

# Stage 3: Production
FROM node:20-alpine AS production

WORKDIR /app

# Copy production dependencies
COPY --from=deps /app/node_modules ./node_modules

# Copy built application
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/package.json ./

# Create non-root user
RUN adduser -D -g '' nodejs && \
    chown -R nodejs:nodejs /app

USER nodejs

EXPOSE 3000

CMD ["node", "dist/main.js"]
```

### Python Application

```dockerfile
# Stage 1: Builder
FROM python:3.12-slim AS builder

# Install build dependencies
RUN apt-get update && apt-get install -y --no-install-recommends \
    gcc \
    libpq-dev \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Install Python dependencies
COPY requirements.txt .
RUN pip install --no-cache-dir --user -r requirements.txt

# Stage 2: Production
FROM python:3.12-slim AS production

# Install runtime dependencies only
RUN apt-get update && apt-get install -y --no-install-recommends \
    libpq5 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy installed Python packages from builder
COPY --from=builder /root/.local /root/.local

# Copy application code
COPY . .

# Adjust PATH for user-installed packages
ENV PATH=/root/.local/bin:$PATH

# Create non-root user
RUN useradd -m -u 1000 appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8000

CMD ["python", "main.py"]
```

### Frontend Application with Nginx

```dockerfile
# Stage 1: Build
FROM node:20-alpine AS builder

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy source and build
COPY . .
RUN npm run build

# Stage 2: Serve with Nginx
FROM nginx:1.25-alpine

# Copy nginx config
COPY nginx.conf /etc/nginx/nginx.conf

# Copy built static files from builder
COPY --from=builder /app/dist /usr/share/nginx/html

# Create non-root user for nginx
RUN addgroup -g 101 -S nginx && \
    adduser -S nginx -u 101 -G nginx

# Set ownership
RUN chown -R nginx:nginx /usr/share/nginx/html && \
    chown -R nginx:nginx /var/cache/nginx && \
    chown -R nginx:nginx /var/log/nginx && \
    chown -R nginx:nginx /etc/nginx/conf.d

USER nginx

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

### Building Multi-Stage Images

```bash
# Build the image
docker build -t myapp:production .

# Build only up to a specific stage
docker build --target builder -t myapp:builder .

# Build with no cache (force rebuild)
docker build --no-cache -t myapp:production .

# Compare image sizes
docker image ls | grep myapp

# Notice the production image is much smaller
# because it doesn't include build tools
```

## Common Mistakes

- **Not cleaning up in the same layer**: Build artifacts left in intermediate stages still contribute to final image if copied.
- **Forgetting --from=builder**: Without this, you'll copy from the previous stage, not the builder stage.
- **Not using specific base image versions**: Multi-stage builds are most effective with specific, stable versions.
- **Copying too much**: Be precise about what you copy from each stage. Only copy what's needed.
- **Ignoring security**: Even with multi-stage builds, ensure the final image runs as non-root.
- **Not understanding what gets copied**: COPY --from copies from the filesystem of that stage, not from the build context.

## Quick Reference

| Stage | Purpose |
|-------|---------|
| builder | Compile/build application |
| development | Run tests, dev tools |
| test | Run test suites |
| production | Minimal runtime image |

| Feature | Benefit |
|---------|---------|
| AS name | Reference stages by name |
| --from=name | Copy from named stage |
| --target | Build specific stage only |
| Multiple FROM | Create multiple images |

## What's Next

Now that you understand multi-stage builds, continue to [Build Cache Optimization](./04-build-cache-optimization.md) to learn how to optimize your Docker builds for speed and efficiency.
