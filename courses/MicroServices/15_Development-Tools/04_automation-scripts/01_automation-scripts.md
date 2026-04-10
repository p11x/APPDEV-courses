# Automation Scripts for Development

## Overview

Automation scripts streamline repetitive development tasks, reducing errors and saving time. This guide covers essential automation for building, testing, and deploying microservices.

## Common Scripts

### Build Scripts

```bash
#!/bin/bash
# build.sh
set -e

echo "Building microservices..."
for service in user order catalog payment; do
  echo "Building $service-service..."
  docker build -t $service-service:latest ./$service-service
done

echo "All services built successfully"
```

### Test Scripts

```bash
#!/bin/bash
# test.sh
set -e

echo "Running tests..."
docker-compose run --rm test-service npm test
echo "Tests complete"
```

### Deployment Scripts

```bash
#!/bin/bash
# deploy.sh
kubectl apply -f k8s/
kubectl set image deployment/order-service order-service=order-service:v$1
kubectl rollout status deployment/order-service
```

## Output

```
Automation Scripts Available:
- build.sh: Build all services
- test.sh: Run test suite
- deploy.sh: Deploy to environment
- rollback.sh: Rollback deployment
- health-check.sh: Check service health
```
