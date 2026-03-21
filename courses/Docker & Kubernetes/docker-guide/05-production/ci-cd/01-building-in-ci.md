# Building in CI

## Overview

Building Docker images in CI/CD pipelines is a fundamental practice for modern software development. Automated builds ensure consistent, reproducible deployments and enable proper testing of containerized applications before production releases.

## Prerequisites

- Docker knowledge
- CI/CD system basics

## Core Concepts

### CI/CD Integration

Build images in CI to:
- Test Dockerfile builds
- Scan for vulnerabilities
- Tag with version info
- Push to registry

### Build Strategies

- **Multi-stage builds**: Smaller final images
- **Build cache**: Faster builds
- **Build arguments**: Dynamic configuration

## Step-by-Step Examples

### GitHub Actions

```yaml
name: Build and Push

on:
  push:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v3
      
      - name: Login to Docker Hub
        uses: docker/login-action@v3
        with:
          username: ${{ secrets.DOCKERHUB_USERNAME }}
          password: ${{ secrets.DOCKERHUB_TOKEN }}
      
      - name: Build and push
        uses: docker/build-push-action@v5
        with:
          context: .
          push: true
          tags: myuser/myapp:${{ github.sha }}
```

### GitLab CI

```yaml
build:
  image: docker:latest
  services:
    - docker:dind
  script:
    - docker build -t myapp:$CI_COMMIT_SHA .
    - docker push myapp:$CI_COMMIT_SHA
```

### Jenkins

```groovy
pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                sh 'docker build -t myapp:${env.BUILD_ID} .'
            }
        }
        stage('Push') {
            steps {
                sh 'docker push myapp:${env.BUILD_ID}'
            }
        }
    }
}
```

## Common Mistakes

- **Not using build cache**: Use layer caching for faster builds.
- **Not scanning images**: Always scan in CI before pushing.
- **Pushing without testing**: Test the built image first.

## What's Next

Continue to [GitHub Actions Example](./02-github-actions-example.md)
