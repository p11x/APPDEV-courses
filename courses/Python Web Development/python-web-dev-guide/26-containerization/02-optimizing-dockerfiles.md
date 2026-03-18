# Optimizing Dockerfiles

## What You'll Learn
- Multi-stage builds
- Layer caching
- Security best practices

## Prerequisites
- Completed Docker basics

## Multi-Stage Builds

```dockerfile
# Build stage
FROM python:3.11-slim as builder
WORKDIR /app
RUN pip install --user --no-cache-dir -r requirements.txt

# Production stage
FROM python:3.11-slim
WORKDIR /app
COPY --from=builder /root/.local /root/.local
COPY . .
ENV PATH=/root/.local/bin:$PATH
EXPOSE 8000
CMD ["uvicorn", "main:app"]
```

## Layer Caching

```dockerfile
# BAD - dependencies change every code change
COPY . .
RUN pip install -r requirements.txt

# GOOD - dependencies first, then code
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
COPY . .
```

## Security

```dockerfile
# Create non-root user
FROM python:3.11-slim
RUN useradd -m appuser
USER appuser
```

## Summary
- Use multi-stage builds
- Order layers for caching
- Run as non-root

## Next Steps
→ Continue to `03-kubernetes-basics.md`
