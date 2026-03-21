# Production vs Dev Overrides

## Overview

Docker Compose supports override files that modify the base configuration for specific environments. This allows you to maintain a single base compose file while having different configurations for development, testing, and production.

## Prerequisites

- Docker Compose basics
- Understanding of environment configuration

## Core Concepts

Override files:
- Named `docker-compose.override.yml`
- Automatically merged with base file
- Can override any service configuration

## Examples

Base configuration (docker-compose.yml):
```yaml
version: "3.8"
services:
  web:
    image: myapp
    ports:
      - "3000:3000"
```

Development override (docker-compose.override.yml):
```yaml
version: "3.8"
services:
  web:
    build: .
    volumes:
      - ./src:/app/src
    environment:
      - DEBUG=true
```

Production override (docker-compose.prod.yml):
```yaml
version: "3.8"
services:
  web:
    image: myapp:latest
    restart: unless-stopped
    logging:
      driver: json-file
      options:
        max-size: "10m"
        max-file: "3"
```

## Running

```bash
# Development (auto-loads override)
docker compose up -d

# Production
docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

## What's Next

Now proceed to the [Production Section](./../../production/security/01-rootless-docker.md)
