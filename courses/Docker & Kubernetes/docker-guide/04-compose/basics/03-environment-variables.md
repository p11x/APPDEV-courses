# Environment Variables

## Overview

Environment variables are essential for configuring containerized applications. Docker Compose provides multiple ways to set environment variables, from inline values to files, enabling flexible configuration across different environments from development to production.

## Prerequisites

- Understanding of Docker Compose basics
- Familiarity with environment variables
- Basic shell knowledge

## Core Concepts

### Setting Environment Variables

Three main ways to set environment variables in Compose:

1. **Inline**: Direct values in compose file
2. **env_file**: External file with variables
3. **Host variables**: From host environment

### Variable Substitution

Compose supports variable substitution:

```yaml
services:
  web:
    image: nginx:${NGINX_VERSION:-latest}
```

## Step-by-Step Examples

### Inline Environment Variables

```yaml
version: "3.8"

services:
  web:
    image: nginx
    environment:
      - NODE_ENV=production
      - DEBUG=false
      - API_URL=http://api:8000
```

### Using env_file

```yaml
version: "3.8"

services:
  web:
    image: myapp
    env_file:
      - .env
      - ./config/prod.env
```

.env file:
```
DATABASE_URL=postgres://db:5432/myapp
SECRET_KEY=mysecretkey
API_KEY=myapikey
```

### Variable Substitution

```yaml
version: "3.8"

services:
  web:
    image: nginx:${NGINX_VERSION:-alpine}
    environment:
      - APP_VERSION=${APP_VERSION:-1.0.0}
```

### Environment-Specific Files

```yaml
# docker-compose.yml (base)
version: "3.8"
services:
  app:
    image: myapp
    env_file:
      - .env.common
      - .env.${ENV:-development}
```

```bash
# Use production
ENV=production docker compose up -d
```

## Common Mistakes

- **Secrets in env files**: Don't store secrets in plain text files.
- **Not using defaults**: Provide defaults for optional variables.
- **Confusing .env with env_file**: .env is for Compose variables, env_file for container.

## Quick Reference

| Syntax | Description |
|--------|-------------|
| ${VAR} | Use variable value |
| ${VAR:-default} | Default if unset |
| ${VAR:=default} | Set default if unset |

## What's Next

Now continue to [Profiles](./../../advanced/01-profiles.md) to learn about Compose profiles for managing different configurations.
