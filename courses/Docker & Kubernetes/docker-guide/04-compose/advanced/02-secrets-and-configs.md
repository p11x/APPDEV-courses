# Secrets and Configs

## Overview

Docker Compose supports storing sensitive data (secrets) and configuration data (configs) separately from your compose file. This provides a secure way to manage sensitive information like passwords and API keys, separating them from your application code.

## Prerequisites

- Understanding of Docker Compose
- Basic security practices knowledge

## Core Concepts

### Secrets vs Configs

- **Secrets**: Sensitive data (passwords, tokens, keys)
- **Configs**: Non-sensitive configuration (configs, settings)

Both are mounted as files in containers but secrets are encrypted at rest in Swarm mode.

## Step-by-Step Examples

```yaml
version: "3.8"

services:
  web:
    image: nginx
    secrets:
      - db_password
      - api_key
    configs:
      - nginx_config

configs:
  nginx_config:
    file: ./nginx.conf

secrets:
  db_password:
    file: ./secrets/db_password.txt
  api_key:
    file: ./secrets/api_key.txt
```

## Common Mistakes

- **Using secrets in environment variables**: Use file-based secrets instead.
- **Not restricting secret access**: Only mount secrets where needed.

## Quick Reference

| Feature | Secret | Config |
|---------|--------|--------|
| Sensitive | Yes | No |
| Encrypted | Yes (Swarm) | No |
| Mount as | File | File |

## What's Next

Continue to [Health Checks](./03-health-checks.md)
