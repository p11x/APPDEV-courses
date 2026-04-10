# Local Development Environment

## Overview

Local development environments enable developers to run microservices locally for faster iteration and debugging. This guide covers setup strategies, tools, and best practices for local microservice development.

## Setup Approaches

### 1. Docker Compose

```yaml
# docker-compose.yml
version: '3.8'
services:
  user-service:
    build: ./user-service
    ports:
      - "8081:8080"
    environment:
      - DATABASE_URL=postgres://db:5432/users
      
  order-service:
    build: ./order-service
    ports:
      - "8082:8080"
    depends_on:
      - user-service
      
  db:
    image: postgres:14
    ports:
      - "5432:5432"
```

### 2. Skaffold

```yaml
# skaffold.yaml
apiVersion: skaffold/v2beta26
kind: Config
build:
  artifacts:
    - image: user-service
      context: ./user-service
    - image: order-service
      context: ./order-service
deploy:
  kubectl:
    manifests:
      - ./k8s/*.yaml
portForward:
  - resourceType: deployment
    resourceName: user-service
    port: 8080
```

## Best Practices

- Use same Docker images as production
- Include all dependencies (databases, message queues)
- Make setup one command (`make up` or similar)
- Document system requirements

## Output

```
Local Environment Status:
✓ All services running
✓ Database connected
✓ Health checks passing

Services:
- user-service: http://localhost:8081
- order-service: http://localhost:8082
- catalog-service: http://localhost:8083
```
