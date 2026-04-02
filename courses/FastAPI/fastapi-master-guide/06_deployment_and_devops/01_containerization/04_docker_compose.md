# Docker Compose

## Overview

Docker Compose orchestrates multi-container FastAPI applications with databases, caches, and other services.

## Complete Setup

### docker-compose.yml

```yaml
# Example 1: Production-ready Docker Compose
version: '3.8'

services:
  app:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "8000:8000"
    environment:
      - DATABASE_URL=postgresql://user:password@db:5432/fastapi_db
      - REDIS_URL=redis://redis:6379
      - ENVIRONMENT=production
    depends_on:
      db:
        condition: service_healthy
      redis:
        condition: service_started
    restart: unless-stopped
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8000/health"]
      interval: 30s
      timeout: 10s
      retries: 3

  db:
    image: postgres:15-alpine
    environment:
      POSTGRES_USER: user
      POSTGRES_PASSWORD: password
      POSTGRES_DB: fastapi_db
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U user -d fastapi_db"]
      interval: 5s
      timeout: 5s
      retries: 5

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data

  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf
      - ./certs:/etc/nginx/certs
    depends_on:
      - app

volumes:
  postgres_data:
  redis_data:
```

### Development Override

```yaml
# Example 2: docker-compose.override.yml (development)
version: '3.8'

services:
  app:
    volumes:
      - .:/app
    environment:
      - DEBUG=true
      - RELOAD=true
    command: uvicorn app.main:app --host 0.0.0.0 --port 8000 --reload
```

## Commands

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f app

# Rebuild and restart
docker-compose up -d --build

# Stop all services
docker-compose down

# Remove volumes
docker-compose down -v
```

## Summary

Docker Compose simplifies multi-container deployment for FastAPI applications.

## Next Steps

Continue learning about:
- [Docker Security](./05_docker_security.md)
- [Kubernetes Deployment](./15_kubernetes_deployment.md)
