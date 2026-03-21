# Secrets in Swarm

## Overview

Docker Swarm secrets provide a secure way to manage sensitive data like passwords, API keys, and certificates. Secrets are encrypted at rest and in transit, accessible only to authorized services.

## Prerequisites

- Active Docker Swarm cluster
- Understanding of Docker services and stacks

## Core Concepts

### What Are Secrets

Secrets are sensitive data items that:

- Are encrypted using AES-256
- Stored in the Raft consensus database
- Mounted into containers at `/run/secrets/`
- Can be updated without rebuilding images
- Are automatically rotated on manager failover

### Secret Storage

- Secrets are distributed across manager nodes
- Stored encrypted in Raft log
- Only accessible to services granted access
- Not visible in `docker inspect` output

## Step-by-Step Examples

### Creating a Secret from Command Line

```bash
# Create secret from a file
# Useful for certificates, keys
echo "my-secret-password" | docker secret create db-password -

# Or use a file directly
docker secret create my-key ./keyfile.pem

# Create secret from literal value
docker secret create db-password "SecretP@ssw0rd!"

# List secrets
docker secret ls

# Example output:
# ID                          NAME          DRIVER    CREATED          UPDATED
# abc123456789                db-password   2 minutes ago      2 minutes ago

# Inspect a secret (without seeing the value)
docker secret inspect db-password

# Example output:
# {
#     "ID": "abc123456789",
#     "CreatedAt": "2024-01-15T10:00:00Z",
#     "UpdatedAt": "2024-01-15T10:00:00Z",
#     "Spec": {
#         "Name": "db-password"
#     }
# }
```

### Using Secrets in Services

```bash
# Create service with secret
# Secret is mounted at /run/secrets/db_password
docker service create \
  --name db \
  --secret db-password \
  postgres:16-alpine

# Create service with secret and custom target name
docker service create \
  --name api \
  --secret source=db-password,target=api_password \
  myapi:latest
```

### Using Secrets in Stacks

```yaml
# docker-compose.yml with secrets
version: "3.8"

secrets:
  db_password:
    external: true  # Created separately
  api_key:
    file: ./api-key.txt  # Create from file

services:
  web:
    image: nginx:1.25-alpine
    secrets:
      - db_password
    environment:
      - DB_PASSWORD_FILE=/run/secrets/db_password
    ports:
      - "8080:80"

  api:
    image: myapi:latest
    secrets:
      - api_key
    environment:
      - API_KEY_FILE=/run/secrets/api_key

# Create secret for stack
echo "secretvalue" | docker secret create api_key -
docker stack deploy -c docker-compose.yml myapp
```

### Accessing Secrets in Containers

```bash
# Secrets are available as files
# Check the secret is mounted
docker exec <container-id> ls -la /run/secrets/

# Example output:
# drwxr-xr-x  1 root root  0 Jan 15 10:00 /run/secrets/db_password

# Read the secret
docker exec <container-id> cat /run/secrets/db_password

# Environment variable approach (common pattern)
# Set environment variable pointing to secret file
docker service create \
  --name web \
  --env POSTGRES_PASSWORD_FILE=/run/secrets/db_password \
  --secret db_password \
  postgres:16-alpine
```

### Secret Rotation

```bash
# Update a secret
# Create new version with same name
echo "newpassword" | docker secret create db-password -

# Swarm automatically updates containers
# Service needs update to pick up new secret
docker service update --secret-rm db-password --secret-add db-password web

# Or just recreate service
docker service rm web
docker service create --secret db-password ...
```

### External Secrets

```bash
# Using externally created secrets
# Create secret first
docker secret create my_secret ./secret.txt

# Reference in compose
version: "3.8"

secrets:
  my_secret:
    external: true

services:
  app:
    image: myapp
    secrets:
      - my_secret
```

## Secret Security

### Encryption

- Secrets encrypted at rest in Raft
- Encrypted in transit between managers
- Each secret has unique encryption key
- Manager quorum required for access

### Best Practices

- Use secrets for all sensitive data
- Never commit secrets to version control
- Rotate secrets regularly
- Use external secrets management for production

## Gotchas for Docker Users

- **No base64 encoding needed**: Docker handles encoding automatically
- **File-based only**: Secrets are files, not environment variables
- **External secrets**: Must create before stack deploy if external
- **Max size**: Secrets limited to 500KB

## Common Mistakes

- **Using environment variables**: Secrets should be files, not env vars
- **Not rotating**: Regular rotation improves security
- **Committing secrets**: Never commit secrets to git

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker secret create NAME FILE` | Create secret |
| `docker secret ls` | List secrets |
| `docker secret rm NAME` | Remove secret |
| `docker secret inspect NAME` | Inspect secret |
| `--secret` flag | Add secret to service |

| Compose Key | Description |
|-------------|-------------|
| `secrets:` | Define secrets |
| `external: true` | Use existing secret |
| `file: ./path` | Create from file |

## What's Next

Continue to [Swarm Monitoring](./03-swarm-monitoring.md) to learn about observability.
