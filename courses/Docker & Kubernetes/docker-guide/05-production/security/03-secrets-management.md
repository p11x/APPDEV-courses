# Secrets Management

## Overview

Proper secrets management is critical for container security. Never store sensitive data like passwords, API keys, or certificates directly in images or environment variables. This guide covers best practices for managing secrets in Docker environments, including Docker Secrets, external secret managers, and secure handling practices.

## Prerequisites

- Understanding of Docker security
- Familiarity with environment variables

## Core Concepts

### Types of Secrets

- **Passwords**: Database passwords, API passwords
- **Keys**: API keys, encryption keys
- **Certificates**: TLS certificates
- **Tokens**: Authentication tokens

### Secrets Management Solutions

1. **Docker Secrets**: Built-in Swarm mode solution
2. **Environment variables**: With external injection
3. **External managers**: HashiCorp Vault, AWS Secrets Manager

## Step-by-Step Examples

### Docker Secrets (Swarm Mode)

```yaml
# docker-compose.yml
version: "3.8"

services:
  db:
    image: postgres
    secrets:
      - db_password

secrets:
  db_password:
    file: ./secrets/db_password.txt
```

### Using Environment Variables

```bash
# Pass secrets at runtime
docker run -e DB_PASSWORD=secret myapp

# Or from file
docker run --env-file secrets.env myapp
```

### External Secret Managers

```bash
# Using HashiCorp Vault
# Get secret at runtime
VAULT_TOKEN=$(vault token create -policy=myapp -period=24h)
docker run -e VAULT_TOKEN myapp

# Or use docker-compose with external secrets
```

## Best Practices

- Never commit secrets to version control
- Use secrets rotation
- Use short-lived tokens
- Audit secret access
- Use external secret managers in production

## Common Mistakes

- **Embedding secrets in images**: Never put secrets in Dockerfile or image layers.
- **Logging secrets**: Don't log environment variables.
- **Using default secrets**: Change all default passwords.

## What's Next

Continue to [Docker Logs](./../debugging/01-docker-logs.md)
