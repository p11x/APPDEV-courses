# Tagging Strategy

## Overview

A consistent tagging strategy is crucial for managing Docker images throughout the software development lifecycle. Proper tagging enables repeatable deployments, easy rollbacks, and clear communication between development teams. Without a strategy, you risk confusion, failed deployments, and difficulty troubleshooting production issues.

## Prerequisites

- Understanding of Docker images
- Familiarity with CI/CD pipelines
- Knowledge of version control

## Core Concepts

### Tag Types

Tags serve different purposes in the image lifecycle:

- **Version tags**: Specific releases (1.0.0, 2.1.3)
- **Latest tag**: Most recent stable release
- **Branch tags**: Development versions (main, develop)
- **Environment tags**: Deployment targets (dev, staging, prod)
- **Architecture tags**: Platform-specific (amd64, arm64)

### Semantic Versioning

Follow Semantic Versioning (SemVer) for version tags:

```
MAJOR.MINOR.PATCH
1.0.0
 ^ ^ ^
 | | |
 | | +-- Bug fixes, backward compatible
 | +---- New features, backward compatible
 +------ Breaking changes
```

- **1.0.0** - Initial release
- **1.0.1** - Bug fix
- **1.1.0** - New feature
- **2.0.0** - Breaking change

### Tagging Patterns

Common tagging patterns:

```bash
# Semantic version
myapp:1.2.3
myapp:1.2
myapp:1

# With variant
myapp:1.2.3-alpine
myapp:1.2.3-debian
myapp:1.2.3-arm64

# Date-based (for frequent builds)
myapp:20240315.1234

# Git-based (for CI/CD)
myapp:git-abcd123
myapp:commit-abcd123

# Environment-specific
myapp:staging
myapp:production
```

## Step-by-Step Examples

### Implementing Semantic Versioning

```bash
# Build with semantic version tags
docker build -t myapp:1.0.0 .
docker build -t myapp:1.0 .
docker build -t myapp:1 .
docker build -t myapp:latest .

# Push all tags
docker push myapp:1.0.0
docker push myapp:1.0
docker push myapp:1
docker push myapp:latest
```

### Git-Based Tagging

In your CI/CD pipeline:

```bash
#!/bin/bash
# Get Git information
GIT_SHA=$(git rev-parse --short HEAD)
GIT_TAG=$(git describe --tags --always)
GIT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
BUILD_NUMBER=${BUILD_NUMBER:-1}

# Get version from package.json or similar
VERSION=$(node -p "require('./package.json').version")

# Tag with multiple identifiers
docker build -t myapp:${VERSION} .
docker build -t myapp:${GIT_SHA} .
docker build -t myapp:${GIT_BRANCH} .

# Tag as latest for main branch
if [ "$GIT_BRANCH" = "main" ]; then
    docker build -t myapp:latest .
fi

# Push all tags
docker push myapp:${VERSION}
docker push myapp:${GIT_SHA}
docker push myapp:${GIT_BRANCH}
docker push myapp:latest
```

### Multi-Architecture Tagging

```bash
# Build for multiple architectures
docker buildx build \
  --platform linux/amd64,linux/arm64,linux/arm/v7 \
  -t myapp:latest \
  --push .

# Tag architecture-specific images
docker buildx build \
  --platform linux/amd64 \
  -t myapp:1.0.0-amd64 \
  --push .

docker buildx build \
  --platform linux/arm64 \
  -t myapp:1.0.0-arm64 \
  --push .
```

### Environment-Specific Tags

```bash
# Development - auto-build on every commit
docker build -t myapp:dev-${GIT_SHA} .
docker push myapp:dev-${GIT_SHA}

# Staging - deploy tagged versions
docker build -t myapp:staging .
docker tag myapp:staging myapp:staging-1.0.0
docker push myapp:staging-1.0.0

# Production - only tagged releases
docker build -t myapp:production .
docker tag myapp:production myapp:1.0.0
docker tag myapp:production myapp:latest
docker push myapp:1.0.0
docker push myapp:latest
```

### Docker Hub Auto-Builds

Configure automatic builds on Docker Hub:

1. Create repository on Docker Hub
2. Link to GitHub/Bitbucket repository
3. Configure build rules:

```yaml
# dockerhub.yml example (if using automated builds)
dockerfile: Dockerfile
build:
  context: .
  args:
    VERSION: master
tags:
  - "{{.Branch}}"
  - "{{.SHA}}"
```

### Version Matrix in CI/CD

```yaml
# GitHub Actions example
jobs:
  build:
    strategy:
      matrix:
        version: ['1.19', '1.20', '1.21', '1.22']
    steps:
      - uses: actions/checkout@v4
      - name: Build
        run: |
          docker build -t myapp:${{ matrix.version }} .
          docker push myapp:${{ matrix.version }}
```

## Common Mistakes

- **Using :latest everywhere**: This tag changes and breaks reproducibility.
- **Inconsistent tagging**: Mix of version schemes makes tracking difficult.
- **Not tagging before push**: Pushing without tags makes images hard to reference.
- **Deleting version tags**: Once used in deployment, tags should remain.
- **Too many tags**: Excessive tags create confusion. Be strategic.
- **Not documenting**: Document your tagging strategy so the team follows it.
- **Ignoring rollbacks**: Ensure you can roll back using previous tags.

## Quick Reference

| Tag Pattern | Use Case |
|------------|----------|
| `1.0.0` | Exact version |
| `1.0` | Minor version line |
| `1` | Major version line |
| `latest` | Current stable |
| `edge` | Latest development |
| `main` | Main branch |
| `sha-abcd123` | Specific commit |
| `20240315` | Date-based |

| Strategy | When to Use |
|----------|-------------|
| SemVer | Production, releases |
| Git-based | CI/CD integration |
| Date-based | Frequent builds |
| Environment | Multi-env deploys |

## What's Next

Now that you have a tagging strategy, continue to [Image Cleanup](./03-image-cleanup.md) to learn how to manage disk space and keep your Docker environment clean.
