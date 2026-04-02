# Docker Security

## Overview

Container security is critical for production deployments. This guide covers securing FastAPI Docker containers.

## Security Practices

### Non-Root User

```dockerfile
# Example 1: Running as non-root
FROM python:3.11-slim

WORKDIR /app

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

COPY --chown=appuser:appuser . .

# Switch to non-root user
USER appuser

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

### Read-Only Filesystem

```dockerfile
# Example 2: Read-only container
FROM python:3.11-slim

WORKDIR /app
COPY . .

# Run with read-only filesystem
# docker run --read-only --tmpfs /tmp myapp
```

### Security Scanning

```bash
# Example 3: Scan for vulnerabilities
# Using Trivy
trivy image my-fastapi-app:latest

# Using Snyk
snyk container test my-fastapi-app:latest
```

## Docker Compose Security

```yaml
# Example 4: Secure Docker Compose
version: '3.8'

services:
  app:
    build: .
    read_only: true
    tmpfs:
      - /tmp
    security_opt:
      - no-new-privileges:true
    cap_drop:
      - ALL
    cap_add:
      - NET_BIND_SERVICE
```

## Best Practices

1. Use official base images
2. Keep images updated
3. Scan for vulnerabilities
4. Use secrets management
5. Limit container capabilities

## Summary

Docker security requires multiple layers of protection.

## Next Steps

Continue learning about:
- [Kubernetes Security](./15_kubernetes_deployment.md)
- [Container Scanning](./07_container_scanning.md)
