# Compose Watch

## Overview

Compose Watch (docker compose watch) automatically updates running Compose services when you modify source files. This provides a streamlined development experience similar to nodemon or other file watchers.

## Prerequisites

- Docker Compose V2.23+
- Node.js or similar development setup

## Usage

```bash
# Enable watch mode
docker compose watch

# Run and watch
docker compose up -d --watch
```

```yaml
# docker-compose.yml with watch
services:
  web:
    build: .
    develop:
      watch:
        - path: ./src
          action: rebuild
        - path: ./package.json
          action: rebuild
```

## Common Mistakes

- **Not using .dockerignore**: Large files may slow down watch.
- **Forgetting action type**: Specify rebuild or sync.

## What's Next

Continue to [Production vs Dev Overrides](./03-production-vs-dev-overrides.md)
