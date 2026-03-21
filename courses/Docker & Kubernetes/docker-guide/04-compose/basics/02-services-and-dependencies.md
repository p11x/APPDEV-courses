# Services and Dependencies

## Overview

Docker Compose allows you to define complex multi-container applications with explicit dependencies between services. Understanding how to configure service dependencies is essential for ensuring your application starts in the correct order, with services that depend on others being available when needed.

## Prerequisites

- Understanding of Docker Compose file structure
- Basic container networking knowledge
- Familiarity with Docker commands

## Core Concepts

### depends_on

The depends_on directive defines startup order:

- Services start in dependency order
- Services stop in reverse order
- Only affects startup order, not health checks

### service_name

Services are identified by their keys in the compose file:

```yaml
services:
  web:       # Service name
    ...
  api:       # Another service
    ...
```

### Service Communication

Services communicate via:
- Service name as hostname
- Automatic DNS resolution
- Shared network

## Step-by-Step Examples

### Basic Dependencies

```yaml
version: "3.8"

services:
  web:
    image: nginx
    depends_on:
      - api
    ports:
      - "8080:80"
  
  api:
    image: myapi
    ports:
      - "3000:3000"
```

### Advanced Dependencies

```yaml
version: "3.8"

services:
  web:
    image: nginx
    depends_on:
      api:
        condition: service_healthy
      db:
        condition: service_started
  
  api:
    image: myapi
    depends_on:
      db:
        condition: service_healthy
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:3000/health"]
      interval: 10s
      timeout: 5s
      retries: 3
  
  db:
    image: postgres:15
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 5s
      timeout: 5s
      retries: 5
```

### Full Application Example

```yaml
version: "3.8"

services:
  # Reverse proxy
  proxy:
    image: nginx:alpine
    ports:
      - "80:80"
    depends_on:
      - webapp
  
  # Main application
  webapp:
    image: myapp/web
    depends_on:
      - api
      - cache
    environment:
      - API_URL=http://api:8000
  
  # Backend API
  api:
    image: myapp/api
    depends_on:
      - db
    environment:
      - DATABASE_URL=postgres://user:pass@db:5432/myapp
  
  # Cache
  cache:
    image: redis:alpine
    depends_on:
      - db
  
  # Database
  db:
    image: postgres:15-alpine
    volumes:
      - pgdata:/var/lib/postgresql/data
    environment:
      - POSTGRES_USER=user
      - POSTGRES_PASSWORD=pass
      - POSTGRES_DB=myapp

volumes:
  pgdata:
```

### Networking

```yaml
version: "3.8"

services:
  frontend:
    image: nginx
    networks:
      - frontend-net
  
  backend:
    image: myapi
    networks:
      - frontend-net
      - backend-net

networks:
  frontend-net:
  backend-net:
```

## Common Mistakes

- **Assuming depends_on waits for readiness**: It only waits for container to start.
- **Not using health checks**: Add health checks for true readiness.
- **Circular dependencies**: Not allowed in Compose.
- **Forgetting dependencies**: Services may fail if dependencies aren't ready.

## Quick Reference

| Option | Description |
|--------|-------------|
| service_Started | Wait for container to start |
| service_healthy | Wait for health check to pass |

## What's Next

Now that you understand service dependencies, continue to [Environment Variables](./03-environment-variables.md) to learn how to configure environment variables in Compose.
