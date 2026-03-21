# Build Cache Optimization

## Overview

Docker's build cache is one of its most powerful features, enabling fast incremental builds by reusing previously built layers. Understanding how to optimize cache usage is essential for efficient development workflows, especially in CI/CD pipelines where build times directly impact productivity. Proper cache optimization can reduce build times from minutes to seconds.

## Prerequisites

- Understanding of Docker layers and build process
- Familiarity with Dockerfile instructions
- Knowledge of your application's dependency management

## Core Concepts

### How Build Cache Works

When Docker builds an image, it executes each instruction in order. For each instruction, Docker checks if it can use a cached layer from a previous build:

1. **Instruction match**: Does the instruction match exactly?
2. **Context match**: Are the files being copied the same?
3. **Argument match**: Are build arguments the same?

If all match, Docker reuses the cached layer. If any differ, Docker invalidates that layer and all subsequent layers.

### Layer Ordering Strategy

The key to cache optimization is ordering instructions from least frequently changed to most frequently changed:

1. Base image (rarely changes)
2. System dependencies (rarely changes)
3. Application dependencies (changes occasionally)
4. Source code (changes frequently)

This way, changing source code doesn't invalidate the dependency installation layers.

### Copy Dependencies First

Copying dependency files before source code is the most important optimization:

```dockerfile
# Good - dependencies are cached unless package*.json changes
COPY package*.json ./
RUN npm ci
COPY . .
```

```dockerfile
# Bad - any source change invalidates dependency installation
COPY . .
RUN npm ci
```

## Step-by-Step Examples

### Optimizing Node.js Dockerfile

```dockerfile
# Always use specific versions
FROM node:20.11.1-alpine3.19

# Labels for tracking
LABEL maintainer="devteam@example.com"

# Set working directory
WORKDIR /app

# Step 1: Copy only dependency files first
# This layer is cached unless package files change
COPY package*.json ./

# Step 2: Install dependencies
# This layer is cached unless packages change
RUN npm ci --only=production

# Step 3: Copy source code
# This layer changes frequently during development
COPY . .

# Step 4: Build (if needed)
# Only rebuilds when source changes
RUN npm run build

# Step 5: Expose and run
EXPOSE 3000
USER nodejs
CMD ["node", "server.js"]
```

### Optimizing Python Dockerfile

```dockerfile
FROM python:3.12-slim

WORKDIR /app

# Copy dependency file first
COPY requirements.txt .

# Install dependencies to a virtual environment
# This layer is cached unless requirements.txt changes
RUN python -m venv /app/venv && \
    /app/venv/bin/pip install --no-cache-dir -r requirements.txt

# Copy source code
COPY . .

# Activate virtual environment in the container
ENV PATH="/app/venv/bin:$PATH"

EXPOSE 8000
CMD ["python", "main.py"]
```

### Using .dockerignore Effectively

Create a comprehensive .dockerignore:

```
# Version control
.git
.gitignore
.gitattributes

# IDE
.vscode
.idea
*.swp
*.swo
*.sublime-*

# Build artifacts
dist
build
target
*.egg-info
__pycache__
*.pyc

# Dependencies (install in container, not copy)
node_modules
venv
.env
.env.local

# Documentation
README.md
*.md
docs/

# Testing
coverage
.pytest_cache
.mypy_cache

# CI/CD
.github
.gitlab-ci.yml
.jenkinsfile

# Secrets
*.pem
*.key
.env.*
```

### Leveraging BuildKit for Better Caching

BuildKit provides advanced caching features:

```bash
# Enable BuildKit
export DOCKER_BUILDKIT=1

# Or use docker buildx for more features
docker buildx create --use
```

```dockerfile
# syntax=docker/dockerfile:1
# Enable syntax for advanced features

FROM node:20-alpine AS base

# Cache npm packages between builds
# This requires BuildKit and a registry
# docker build --push with cache-from
# See: https://docs.docker.com/build/cache/from/
```

### Rebuilding and Cache Invalidation

```bash
# View build cache usage
docker build --progress=plain -t myapp .
# Look for "CACHED" in output

# Force no cache for rebuild
docker build --no-cache -t myapp .

# Build with cache from a specific image
docker build --cache-from myapp:previous -t myapp .

# Prune build cache
docker builder prune

# Prune all cache
docker system prune -a
```

### Comparing Build Times

```bash
# First build (no cache)
time docker build -t test:no-cache --no-cache .
# Takes: ~60 seconds

# Second build (with cache)
time docker build -t test:with-cache .
# Takes: ~5 seconds

# After only changing source code
time docker build -t test:source-change .
# Takes: ~10 seconds

# After changing dependencies
time docker build -t test:dep-change . 
# Takes: ~55 seconds (rebuilds deps)
```

## Common Mistakes

- **Copying entire project before dependencies**: Any source change invalidates dependency installation.
- **Not using .dockerignore**: Unnecessary files bloat context and may invalidate cache.
- **Combining too many commands**: Breaking RUN into separate commands improves cache granularity.
- **Not understanding when cache invalidates**: Even whitespace changes can invalidate layers.
- **Using COPY over ADD unnecessarily**: COPY is explicit and clear; ADD has side effects.
- **Ignoring build context size**: Large contexts take time to send to Docker daemon.
- **Not using specific tags**: Base image changes invalidate all dependent layers.

## Quick Reference

| Optimization | Technique |
|-------------|-----------|
| Dependency caching | COPY package*.json first, then RUN install |
| Layer ordering | Least to most frequently changed |
| .dockerignore | Exclude unnecessary files |
| BuildKit | Advanced cache features |
| Specific versions | Never use latest |
| Context size | Minimize files sent to daemon |

| Cache Status | Meaning |
|--------------|---------|
| CACHED | Layer reused from previous build |
| #1 | Running instruction for first time |
| Removing cache | Cache was invalidated |

## What's Next

Now that you understand build cache optimization, continue to [Pulling and Pushing](./../../managing-images/01-pulling-and-pushing.md) to learn how to manage images in registries.
