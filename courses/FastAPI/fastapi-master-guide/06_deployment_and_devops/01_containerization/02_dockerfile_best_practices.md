# Dockerfile Best Practices

## Overview

Well-optimized Dockerfiles improve build speed, reduce image size, and enhance security for FastAPI applications.

## Multi-Stage Builds

### Optimized Multi-Stage Dockerfile

```dockerfile
# Example 1: Multi-stage build for FastAPI
# Stage 1: Build dependencies
FROM python:3.11-slim as builder

WORKDIR /app

# Install build dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends gcc libpq-dev && \
    rm -rf /var/lib/apt/lists/*

# Copy and install Python dependencies
COPY requirements.txt .
RUN pip wheel --no-cache-dir --no-deps --wheel-dir /wheels -r requirements.txt

# Stage 2: Production image
FROM python:3.11-slim

# Install runtime dependencies only
RUN apt-get update && \
    apt-get install -y --no-install-recommends libpq5 curl && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy wheels from builder
COPY --from=builder /wheels /wheels
RUN pip install --no-cache-dir /wheels/*

# Copy application
COPY app/ app/

# Create non-root user
RUN useradd -m -r appuser && chown -R appuser:appuser /app
USER appuser

EXPOSE 8000

HEALTHCHECK --interval=30s --timeout=10s \
    CMD curl -f http://localhost:8000/health || exit 1

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

## Layer Optimization

### Caching Strategy

```dockerfile
# Example 2: Optimized layer caching
FROM python:3.11-slim

WORKDIR /app

# Layer 1: System dependencies (rarely changes)
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    curl \
    && rm -rf /var/lib/apt/lists/*

# Layer 2: Python dependencies (changes occasionally)
# Copy only requirements first for better caching
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Layer 3: Application code (changes frequently)
COPY app/ app/
COPY alembic/ alembic/
COPY alembic.ini .

EXPOSE 8000

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

## Security Practices

### Secure Dockerfile

```dockerfile
# Example 3: Security-hardened Dockerfile
FROM python:3.11-slim as builder

WORKDIR /app

# Don't run as root during build
COPY --chown=1000:1000 requirements.txt .
RUN pip install --no-cache-dir --user -r requirements.txt

FROM python:3.11-slim

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser -d /app -s /sbin/nologin appuser

WORKDIR /app

# Copy dependencies from builder
COPY --from=builder --chown=appuser:appuser /root/.local /home/appuser/.local

# Copy application
COPY --chown=appuser:appuser app/ app/

# Set PATH for user-installed packages
ENV PATH=/home/appuser/.local/bin:$PATH

# Switch to non-root user
USER appuser

# Don't expose unnecessary ports
EXPOSE 8000

# Use exec form for proper signal handling
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

## .dockerignore

### Excluding Files

```bash
# Example 4: .dockerignore file
# Version control
.git
.gitignore

# Python
__pycache__
*.py[cod]
*.pyo
*.egg-info
dist/
build/
.eggs/

# Virtual environments
venv/
env/
.venv/

# IDE
.vscode/
.idea/
*.swp
*.swo

# Environment files
.env
.env.*

# Documentation
*.md
docs/

# Tests
tests/
.pytest_cache/
.coverage
htmlcov/

# Docker
Dockerfile
docker-compose*.yml
.dockerignore

# OS files
.DS_Store
Thumbs.db
```

## Size Optimization

### Reducing Image Size

```dockerfile
# Example 5: Minimal FastAPI image
# Use slim base image
FROM python:3.11-slim as builder

WORKDIR /app

# Install only what's needed
COPY requirements.txt .
RUN pip install --no-cache-dir --user -r requirements.txt

# Minimal runtime image
FROM python:3.11-slim

# Copy only runtime dependencies
COPY --from=builder /root/.local /root/.local
ENV PATH=/root/.local/bin:$PATH

WORKDIR /app
COPY app/ app/

# Remove unnecessary files in same layer
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

EXPOSE 8000

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

## Health Checks

### Container Health

```dockerfile
# Example 6: Health check configuration
FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY app/ app/

# Basic health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8000/health || exit 1

# Alternative: Python-based health check
# HEALTHCHECK --interval=30s --timeout=10s \
#     CMD python -c "import urllib.request; urllib.request.urlopen('http://localhost:8000/health')"

EXPOSE 8000

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

## Build Arguments

### Parameterized Builds

```dockerfile
# Example 7: Build arguments
ARG PYTHON_VERSION=3.11
FROM python:${PYTHON_VERSION}-slim

ARG APP_ENV=production
ARG BUILD_DATE
ARG VERSION=1.0.0

LABEL maintainer="team@example.com"
LABEL version="${VERSION}"
LABEL build_date="${BUILD_DATE}"

ENV APP_ENV=${APP_ENV}

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY app/ app/

# Conditional configuration based on environment
RUN if [ "$APP_ENV" = "development" ]; then \
    pip install --no-cache-dir debugpy pytest; \
    fi

EXPOSE 8000

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

```bash
# Build with arguments
docker build \
    --build-arg PYTHON_VERSION=3.11 \
    --build-arg APP_ENV=production \
    --build-arg VERSION=1.2.0 \
    -t my-app:1.2.0 .
```

## Best Practices Summary

```dockerfile
# Example 8: Complete best practices Dockerfile
FROM python:3.11-slim as builder

WORKDIR /app

# Install build dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends gcc && \
    rm -rf /var/lib/apt/lists/*

# Cache pip packages
COPY requirements.txt .
RUN pip wheel --no-cache-dir --no-deps --wheel-dir /wheels -r requirements.txt

# Production stage
FROM python:3.11-slim

# Install runtime dependencies only
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Install from wheels
COPY --from=builder /wheels /wheels
RUN pip install --no-cache-dir /wheels/* && rm -rf /wheels

# Create non-root user
RUN useradd -m -r appuser
USER appuser

# Copy application
COPY --chown=appuser:appuser app/ app/

EXPOSE 8000

HEALTHCHECK --interval=30s --timeout=10s \
    CMD curl -f http://localhost:8000/health || exit 1

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000", "--workers", "4"]
```

## Summary

| Practice | Benefit |
|----------|---------|
| Multi-stage builds | Smaller images |
| Layer caching | Faster builds |
| Non-root user | Security |
| .dockerignore | Faster builds |
| Health checks | Reliability |
| Build args | Flexibility |

## Next Steps

Continue learning about:
- [Multi-Stage Builds](./03_multi_stage_builds.md) - Advanced patterns
- [Docker Compose](./04_docker_compose.md) - Multi-service
- [Docker Security](./05_docker_security.md) - Hardening
