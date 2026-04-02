# Docker Basics

## Overview

Docker containerizes FastAPI applications for consistent deployment across environments. This guide covers essential Docker concepts for FastAPI.

## Basic Dockerfile

### Simple FastAPI Dockerfile

```dockerfile
# Example 1: Basic FastAPI Dockerfile
FROM python:3.11-slim

WORKDIR /app

# Install dependencies
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copy application code
COPY . .

# Expose port
EXPOSE 8000

# Run application
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

### Building and Running

```bash
# Build the image
docker build -t my-fastapi-app .

# Run the container
docker run -d -p 8000:8000 --name my-app my-fastapi-app

# View logs
docker logs my-app

# Stop container
docker stop my-app
```

## Production Dockerfile

### Optimized Dockerfile

```dockerfile
# Example 2: Production-ready Dockerfile
FROM python:3.11-slim as builder

WORKDIR /app

# Install build dependencies
RUN apt-get update && apt-get install -y --no-install-recommends \
    build-essential \
    && rm -rf /var/lib/apt/lists/*

# Install Python dependencies
COPY requirements.txt .
RUN pip wheel --no-cache-dir --no-deps --wheel-dir /app/wheels -r requirements.txt

# Production stage
FROM python:3.11-slim

WORKDIR /app

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy wheels and install
COPY --from=builder /app/wheels /wheels
RUN pip install --no-cache-dir /wheels/* && rm -rf /wheels

# Copy application
COPY --chown=appuser:appuser . .

# Switch to non-root user
USER appuser

EXPOSE 8000

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8000/health || exit 1

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000", "--workers", "4"]
```

## Docker Compose

### Multi-Service Setup

```yaml
# Example 3: docker-compose.yml
version: '3.8'

services:
  app:
    build: .
    ports:
      - "8000:8000"
    environment:
      - DATABASE_URL=postgresql://user:password@db:5432/fastapi_db
      - REDIS_URL=redis://redis:6379
    depends_on:
      db:
        condition: service_healthy
      redis:
        condition: service_started
    volumes:
      - ./app:/app/app
    restart: unless-stopped

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

volumes:
  postgres_data:
  redis_data:
```

### Running with Compose

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f app

# Stop services
docker-compose down

# Rebuild and start
docker-compose up -d --build
```

## Environment Configuration

### Using .env Files

```bash
# Example 4: .env file
# .env
DATABASE_URL=postgresql://user:password@localhost:5432/mydb
REDIS_URL=redis://localhost:6379
SECRET_KEY=your-secret-key-here
DEBUG=false
```

```yaml
# docker-compose.yml with env file
version: '3.8'

services:
  app:
    build: .
    env_file:
      - .env
    ports:
      - "8000:8000"
```

## Docker Commands Reference

### Essential Commands

```bash
# Example 5: Common Docker commands

# Image management
docker images                    # List images
docker rmi <image>              # Remove image
docker tag <image> <new-tag>    # Tag image

# Container management
docker ps                       # List running containers
docker ps -a                    # List all containers
docker stop <container>         # Stop container
docker rm <container>           # Remove container
docker exec -it <container> sh  # Shell into container

# Volume management
docker volume ls                # List volumes
docker volume rm <volume>       # Remove volume

# Network management
docker network ls               # List networks
docker network create <name>    # Create network

# Cleanup
docker system prune             # Remove unused data
docker image prune              # Remove unused images
docker volume prune             # Remove unused volumes
```

## Best Practices

### Docker Guidelines

```dockerfile
# Example 6: Dockerfile best practices
FROM python:3.11-slim

WORKDIR /app

# 1. Use .dockerignore
# Create .dockerignore to exclude:
# __pycache__
# *.pyc
# .git
# .env
# venv/

# 2. Order instructions for caching
# Dependencies first (changes less often)
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Application code last (changes more often)
COPY . .

# 3. Use specific versions
# Don't use: pip install fastapi
# Use: pip install fastapi==0.100.0

# 4. Minimize layers
# Combine RUN commands
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/*

# 5. Don't run as root
RUN useradd -m appuser
USER appuser

# 6. Use HEALTHCHECK
HEALTHCHECK CMD curl -f http://localhost:8000/health || exit 1
```

## Summary

| Concept | Command | Purpose |
|---------|---------|---------|
| Build | `docker build -t name .` | Create image |
| Run | `docker run -p 8000:8000 name` | Start container |
| Compose | `docker-compose up -d` | Multi-service |
| Logs | `docker logs container` | View output |
| Shell | `docker exec -it container sh` | Debug |

## Next Steps

Continue learning about:
- [Dockerfile Best Practices](./02_dockerfile_best_practices.md) - Optimization
- [Docker Compose](./04_docker_compose.md) - Multi-service setup
- [Kubernetes Deployment](./15_kubernetes_deployment.md) - Orchestration
