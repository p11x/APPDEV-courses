# Pulling and Pushing Images

## Overview

Pulling and pushing images is how you distribute Docker images between your local machine, registries, and production environments. Mastering these operations is essential for deploying applications and collaborating with teams. This guide covers the mechanics of image transfer, including authentication, handling multiple architectures, and best practices for production deployments.

## Prerequisites

- Docker installed and running
- Understanding of Docker registries
- Basic command-line knowledge

## Core Concepts

### Pulling Images

Pulling downloads an image from a registry to your local Docker host:

```bash
# Pull from Docker Hub
docker pull nginx:latest

# Pull from specific registry
docker pull myregistry.com/myapp:1.0

# Pull specific architecture
docker pull --platform linux/arm64 nginx:alpine
```

### Pushing Images

Pushing uploads your local image to a registry:

```bash
# Tag first
docker tag myapp:latest myregistry.com/myapp:1.0

# Push to registry
docker push myregistry.com/myapp:1.0
```

### Authentication

Most registries require authentication:

```bash
# Login to Docker Hub
docker login -u username

# Login to private registry
docker login myregistry.com -u username

# Logout
docker logout
```

### Image Identifiers

Images can be referenced by:
- **Tag**: nginx:1.25
- **Digest**: nginx@sha256:abc123...
- **Image ID**: 4e38e38c8ce0...

## Step-by-Step Examples

### Pulling Images

```bash
# Pull latest nginx from Docker Hub
docker pull nginx:latest

# Pull specific version
docker pull nginx:1.25.0

# Pull Alpine variant (smaller)
docker pull nginx:1.25-alpine

# Pull multiple tags
docker pull -a nginx

# Pull with digest (pin to exact version)
docker pull nginx@sha256:4e38e38c8ce0b8d9045a3a0b049e3b9f3f8d5b8b8a8a8a8a8a8a8a8a8a8a8a

# Pull and show progress
docker pull -q nginx:1.25

# Pull all architectures for multi-platform image
docker pull --platform linux/amd64,linux/arm64 nginx:latest
```

### Pushing Images

```bash
# Tag for Docker Hub
docker tag myapp:latest myusername/myapp:latest

# Push to Docker Hub
docker push myusername/myapp:latest

# Tag for private registry
docker tag myapp:latest private-registry.com:5000/myapp:1.0

# Push to private registry
docker push private-registry.com:5000/myapp:1.0

# Push all tags for an image
docker push myusername/myapp --all

# Push with no cache (force rebuild on registry)
docker push --no-cache myusername/myapp:latest
```

### Working with Digests

```bash
# Pull image and save digest
docker pull nginx:1.25-alpine

# Get the digest
docker inspect --format='{{index .RepoDigests 0}}' nginx:1.25-alpine
# Output: nginx:1.25-alpine@sha256:abc123...

# Pull by digest (exact version)
docker pull nginx@sha256:abc123...

# Verify digest matches
docker pull nginx:1.25-alpine
docker images --no-trunc
# Compare digest values
```

### Working with Private Registries

```bash
# Login to private registry
docker login myregistry.example.com -u admin

# Tag image for registry
docker tag myapp:latest myregistry.example.com/myapp:v1.0

# Push to private registry
docker push myregistry.example.com/myapp:v1.0

# Pull from private registry
docker pull myregistry.example.com/myapp:v1.0

# Logout when done
docker logout myregistry.example.com
```

### Using Amazon ECR

```bash
# Get ECR login password
aws ecr get-login-password --region us-east-1 | \
  docker login --username AWS --password-stdin \
  123456789012.dkr.ecr.us-east-1.amazonaws.com

# Pull image
docker pull 123456789012.dkr.ecr.us-east-1.amazonaws.com/myapp:latest

# Tag for ECR
docker tag myapp:latest 123456789012.dkr.ecr.us-east-1.amazonaws.com/myapp:latest

# Push to ECR
docker push 123456789012.dkr.ecr.us-east-1.amazonaws.com/myapp:latest
```

### Automating Push with CI/CD

```bash
# Example: Tag and push on build
#!/bin/bash
set -e

VERSION=$(node -p "require('./package.json').version")
IMAGE="myregistry.com/myapp"

# Tag with version
docker build -t ${IMAGE}:${VERSION} .
docker build -t ${IMAGE}:latest .

# Push version tag
docker push ${IMAGE}:${VERSION}

# Push latest if on main branch
if [ "$BRANCH" = "main" ]; then
  docker push ${IMAGE}:latest
fi
```

## Common Mistakes

- **Pushing without tagging**: Always tag images before pushing to avoid confusion.
- **Using :latest in production**: Tags can change. Use specific versions or digests for reproducibility.
- **Forgetting to login**: Private registries require authentication.
- **Pushing to wrong registry**: Double-check the registry URL before pushing.
- **Not understanding multi-arch**: Images may not support all architectures. Always verify.
- **Ignoring rate limits**: Docker Hub has pull rate limits for anonymous and free accounts.
- **Not cleaning up before push**: Large images take time to push; optimize first.

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker pull image:tag` | Download image from registry |
| `docker push image:tag` | Upload image to registry |
| `docker tag src dest` | Create tag pointing to source image |
| `docker login` | Authenticate to registry |
| `docker logout` | Remove authentication |
| `--platform` | Pull specific architecture |
| `@digest` | Reference by exact hash |

| Best Practice | Implementation |
|--------------|----------------|
| Use specific tags | Never use :latest in production |
| Pin with digest | Use @sha256:... for exact versions |
| Scan before push | Check for vulnerabilities |
| Minimize size | Use Alpine, multi-stage builds |

## What's Next

Now that you can pull and push images, continue to [Tagging Strategy](./02-tagging-strategy.md) to learn how to organize and version your images effectively.
