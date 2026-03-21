# Fullstack App Example

## Overview

This example demonstrates a complete production-ready Docker Compose setup with a frontend, backend API, database, and caching layer. It showcases real-world patterns including health checks, dependency management, and proper networking.

## Prerequisites

- Docker Compose installed
- Basic understanding of web applications

## Complete Example

```yaml
version: "3.8"

services:
  # Frontend
  frontend:
    build:
      context: ./frontend
      dockerfile: Dockerfile
    ports:
      - "3000:3000"
    environment:
      - REACT_APP_API_URL=http://localhost:4000
    depends_on:
      api:
        condition: service_healthy
    networks:
      - app-network

  # Backend API
  api:
    build:
      context: ./backend
      dockerfile: Dockerfile
    ports:
      - "4000:4000"
    environment:
      - DATABASE_URL=postgres://user:pass@db:5432/myapp
      - REDIS_URL=redis://cache:6379
      - NODE_ENV=production
    depends_on:
      db:
        condition: service_healthy
      cache:
        condition: service_started
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:4000/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
    networks:
      - app-network

  # Database
  db:
    image: postgres:15-alpine
    volumes:
      - postgres-data:/var/lib/postgresql/data
    environment:
      - POSTGRES_USER=user
      - POSTGRES_PASSWORD=pass
      - POSTGRES_DB=myapp
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U user -d myapp"]
      interval: 10s
      timeout: 5s
      retries: 5
    networks:
      - app-network

  # Cache
  cache:
    image: redis:7-alpine
    volumes:
      - redis-data:/data
    networks:
      - app-network

networks:
  app-network:

volumes:
  postgres-data:
  redis-data:
```

## Running the Application

```bash
# Build and start all services
docker compose up -d --build

# View logs
docker compose logs -f

# Scale services
docker compose up -d --scale api=3

# Stop and remove
docker compose down
```

## Common Mistakes

- **Not setting health checks**: Add health checks for proper dependency handling.
- **Missing environment variables**: Ensure all required variables are set.

## What's Next

Continue to [Compose Watch](./02-compose-watch.md)
