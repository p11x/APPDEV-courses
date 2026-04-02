# Multi-Stage Builds

## Overview

Multi-stage Docker builds separate build and runtime stages, creating smaller, more secure production images.

## Implementation

### Python Multi-Stage Build

```dockerfile
# Example 1: Multi-stage FastAPI Dockerfile
# Stage 1: Build dependencies
FROM python:3.11-slim as builder

WORKDIR /app

# Install build dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends gcc libpq-dev && \
    rm -rf /var/lib/apt/lists/*

# Copy requirements and install
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
RUN pip install --no-cache-dir /wheels/* && rm -rf /wheels

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

## Benefits

| Aspect | Single Stage | Multi-Stage |
|--------|-------------|-------------|
| Image Size | ~1GB | ~150MB |
| Security | Build tools included | Runtime only |
| Layers | Many | Optimized |

## Summary

Multi-stage builds create smaller, more secure production images.

## Next Steps

Continue learning about:
- [Docker Security](./05_docker_security.md)
- [Docker Compose](./04_docker_compose.md)
