# Postgres with Docker

## Overview

Running PostgreSQL in Docker provides consistency across environments. This guide covers deploying and managing PostgreSQL containers.

## Prerequisites

- Docker Engine 20.10+
- PostgreSQL basics

## Step-by-Step Examples

### Basic PostgreSQL

```bash
# Run PostgreSQL with volume for persistence
docker run -d \
  --name postgres-db \
  -e POSTGRES_USER=myuser \
  -e POSTGRES_PASSWORD=mypassword \
  -e POSTGRES_DB=mydb \
  -v postgres-data:/var/lib/postgresql/data \
  postgres:16-alpine
```

### Docker Compose Setup

```yaml
# docker-compose.yml
version: "3.8"

services:
  postgres:
    image: postgres:16-alpine
    environment:
      POSTGRES_USER: myuser
      POSTGRES_PASSWORD: mypassword
      POSTGRES_DB: mydb
    volumes:
      - postgres-data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U myuser"]
      interval: 10s
      timeout: 5s
      retries: 5

volumes:
  postgres-data:
```

### Connecting to PostgreSQL

```bash
# Connect using psql
docker exec -it postgres-db psql -U myuser -d mydb

# Run SQL commands
# CREATE TABLE users (id SERIAL PRIMARY KEY, name TEXT);
# \q
```

## Quick Reference

| Environment Variable | Description |
|---------------------|-------------|
| POSTGRES_USER | Database user |
| POSTGRES_PASSWORD | User password |
| POSTGRES_DB | Initial database |

## What's Next

Continue to [Redis Caching Setup](./02-redis-caching-setup.md) for caching.
