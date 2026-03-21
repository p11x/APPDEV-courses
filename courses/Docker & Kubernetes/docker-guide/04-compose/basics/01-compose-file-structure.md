# Compose File Structure

## Overview

Docker Compose is a tool for defining and running multi-container Docker applications. With Compose, you use a YAML file to configure your application's services, networks, and volumes, then create and start everything with a single command. Understanding the Compose file structure is fundamental to using Compose effectively for both development and production environments.

## Prerequisites

- Docker installed and running
- Understanding of Docker containers
- Basic YAML knowledge
- Familiarity with command-line

## Core Concepts

### What is Docker Compose?

Compose is a tool for managing multi-container applications:

- Defines services in a single YAML file
- Manages networking between containers
- Handles volume mounts
- Enables easy scaling
- Simplifies local development

### Version Compatibility

Compose files have versions that map to Docker versions:

```yaml
version: "3.8"  # Docker 19.03+
version: "3.9"  # Docker 20.10+
version: "3.10" # Docker 22.0+
```

### Basic Structure

```yaml
version: "3.8"

services:
  web:
    image: nginx
    ports:
      - "80:80"
  
  db:
    image: postgres
    volumes:
      - db-data:/var/lib/postgresql/data

volumes:
  db-data:
```

## Step-by-Step Examples

### Simple Compose File

```yaml
# docker-compose.yml
version: "3.8"

services:
  web:
    image: nginx:1.25-alpine
    ports:
      - "8080:80"
    volumes:
      - ./html:/usr/share/nginx/html:ro
```

Run it:

```bash
# Start all services
docker compose up -d

# View logs
docker compose logs -f

# Stop all services
docker compose down
```

### Full Example with Multiple Services

```yaml
version: "3.8"

services:
  # Frontend service
  frontend:
    image: myapp/web:latest
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
      - API_URL=http://api:8000
    depends_on:
      - api
      - db
    networks:
      - frontend-network
      - backend-network

  # Backend API service
  api:
    image: myapp/api:latest
    environment:
      - DATABASE_URL=postgres://db:5432/myapp
      - REDIS_URL=redis://cache:6379
    depends_on:
      - db
      - cache
    networks:
      - backend-network

  # Database service
  db:
    image: postgres:15-alpine
    volumes:
      - postgres-data:/var/lib/postgresql/data
    environment:
      - POSTGRES_USER=appuser
      - POSTGRES_PASSWORD=secret
      - POSTGRES_DB=myapp
    networks:
      - backend-network

  # Cache service
  cache:
    image: redis:7-alpine
    networks:
      - backend-network

networks:
  frontend-network:
  backend-network:

volumes:
  postgres-data:
```

### Environment Variables

```yaml
services:
  web:
    image: nginx
    environment:
      - VAR1=value1
      - VAR2=value2
      # Or use:
    env_file:
      - .env
```

Create .env file:
```
VAR1=value1
VAR2=value2
```

### Volume Definitions

```yaml
services:
  app:
    image: myapp
    volumes:
      # Named volume
      - app-data:/app/data
      
      # Bind mount
      - ./config:/app/config
      
      # Read-only
      - ./static:/app/static:ro

volumes:
  app-data:
```

## Common Mistakes

- **Wrong indentation**: YAML is sensitive to indentation.
- **Missing required fields**: version, services are required.
- **Using old syntax**: version must match your Docker version.
- **Forgetting depends_on**: Services may start before dependencies.
- **Not defining networks**: Containers can't communicate without networks.

## Quick Reference

| Top-level Key | Description |
|--------------|-------------|
| version | Compose file version |
| services | Container definitions |
| networks | Network definitions |
| volumes | Volume definitions |

| Common Service Options | Description |
|------------------------|-------------|
| image | Base image |
| build | Build from Dockerfile |
| ports | Port mappings |
| volumes | Volume mounts |
| environment | Environment variables |
| depends_on | Service dependencies |
| networks | Network membership |

## What's Next

Now that you understand Compose file structure, continue to [Services and Dependencies](./02-services-and-dependencies.md) to learn how to configure service relationships.
