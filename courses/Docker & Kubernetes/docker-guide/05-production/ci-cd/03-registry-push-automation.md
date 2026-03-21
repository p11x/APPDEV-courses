# Registry Push Automation

## Overview

Automating image pushes to container registries is essential for continuous deployment. This guide covers automated pushing to Docker Hub, GitHub Container Registry, Amazon ECR, and other registries with proper tagging and security practices.

## Prerequisites

- Docker knowledge
- Registry account access

## Core Concepts

### Registry Types

- **Docker Hub**: docker.io
- **GitHub Container Registry**: ghcr.io
- **Amazon ECR**: account.dkr.ecr.region.amazonaws.com
- **Google Container Registry**: gcr.io

### Tagging Strategies

- Semantic versioning
- Git SHA
- Branch-based

## Step-by-Step Examples

### Docker Hub Push

```bash
# Tag for Docker Hub
docker build -t myuser/myapp:latest .
docker build -t myuser/myapp:1.0.0 .

# Push
docker push myuser/myapp:latest
docker push myuser/myapp:1.0.0
```

### GitHub Container Registry

```bash
# Login
echo $GITHUB_TOKEN | docker login ghcr.io -u $GITHUB_ACTOR --password-stdin

# Tag
docker build -t ghcr.io/myuser/myapp:latest .

# Push
docker push ghcr.io/myuser/myapp:latest
```

### Amazon ECR

```bash
# Login
aws ecr get-login-password --region us-east-1 | \
  docker login --username AWS --password-stdin \
  123456789012.dkr.ecr.us-east-1.amazonaws.com

# Build and tag
docker build -t 123456789012.dkr.ecr.us-east-1.amazonaws.com/myapp:latest .

# Push
docker push 123456789012.dkr.ecr.us-east-1.amazonaws.com/myapp:latest
```

### Automated Script

```bash
#!/bin/bash
set -e

REGISTRY=$1
IMAGE_NAME=$2
VERSION=$3

docker build -t ${REGISTRY}/${IMAGE_NAME}:${VERSION} .
docker push ${REGISTRY}/${IMAGE_NAME}:${VERSION}
```

## Common Mistakes

- **Forgetting to login**: Always authenticate before pushing.
- **Rate limiting**: Docker Hub has limits; use authenticated access.
- **Wrong registry**: Double-check registry URL.

## What's Next

The Docker guide is now complete. Now you can proceed to create the Kubernetes guide, or the task is complete for Docker. Let me now move to Step 5 - create kubernetes-guide root directory.
