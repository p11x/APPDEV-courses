# Redis Caching Setup

## Overview

Redis provides in-memory caching for applications. This guide covers deploying Redis in Docker with persistence and authentication.

## Prerequisites

- Docker basics
- Redis concepts

## Step-by-Step Examples

### Basic Redis

```bash
# Run Redis with persistence
docker run -d \
  --name redis-cache \
  -p 6379:6379 \
  redis:7-alpine \
  redis-server --appendonly yes
```

### With Password

```bash
# Run with password authentication
docker run -d \
  --name redis-cache \
  -p 6379:6379 \
  redis:7-alpine \
  redis-server --requirepass mypassword
```

### Docker Compose

```yaml
# docker-compose.yml
version: "3.8"

services:
  redis:
    image: redis:7-alpine
    command: redis-server --appendonly yes --requirepass mypassword
    volumes:
      - redis-data:/data
    ports:
      - "6379:6379"

volumes:
  redis-data:
```

## Quick Reference

| Flag | Description |
|------|-------------|
| --appendonly yes | Enable AOF persistence |
| --requirepass | Set password |

## What's Next

Continue to [MongoDB Replica Set](./03-mongodb-replica-set.md) for document databases.
