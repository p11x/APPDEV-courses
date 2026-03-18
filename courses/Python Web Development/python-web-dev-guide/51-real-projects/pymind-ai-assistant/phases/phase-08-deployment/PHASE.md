# Phase 8 — Deployment

## Goal

By the end of this phase, you will have:
- Production-ready Dockerfile
- Docker Compose for production
- Environment variable configuration
- Health check endpoints
- Production database setup

## Prerequisites

- Completed Phase 7 (testing)
- Docker knowledge

## Step-by-Step Implementation

### Step 8.1 — Update Dockerfile for Production

```dockerfile
# Dockerfile
# Build stage
FROM python:3.11-slim AS builder

WORKDIR /app

# Install system dependencies
RUN apt-get update && apt-get install -y --no-install-recommends \
    gcc \
    libpq-dev \
    && rm -rf /var/lib/apt/lists/*

# Install Python dependencies
COPY pyproject.toml .
RUN pip install --no-cache-dir --prefix=/install -e .

# Production stage
FROM python:3.11-slim

WORKDIR /app

# Copy installed packages
COPY --from=builder /install /usr/local

# Create non-root user
RUN useradd --create-home appuser && chown -R appuser:appuser /app

# Copy application
COPY --chown=appuser:appuser . .

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 8000

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD python -c "import httpx; httpx.get('http://localhost:8000/health')"

# Run with uvicorn
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

### Step 8.2 — Create Production Docker Compose

```yaml
# docker-compose.prod.yml
services:
  app:
    build: .
    ports:
      - "8000:8000"
    environment:
      - DATABASE_URL=${DATABASE_URL}
      - REDIS_URL=${REDIS_URL}
      - OPENAI_API_KEY=${OPENAI_API_KEY}
      - JWT_SECRET_KEY=${JWT_SECRET_KEY}
      - APP_ENV=production
      - LOG_LEVEL=INFO
    depends_on:
      postgres:
        condition: service_healthy
      redis:
        condition: service_healthy
    restart: unless-stopped
    deploy:
      resources:
        limits:
          cpus: '1'
          memory: 2G

  postgres:
    image: pgvector/pgvector:pg16
    environment:
      POSTGRES_USER: ${DB_USER}
      POSTGRES_PASSWORD: ${DB_PASSWORD}
      POSTGRES_DB: ${DB_NAME}
    volumes:
      - postgres_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${DB_USER}"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped
    deploy:
      resources:
        limits:
          cpus: '1'
          memory: 1G

  redis:
    image: redis:7-alpine
    command: redis-server --appendonly yes
    volumes:
      - redis_data:/data
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 10s
      timeout: 3s
      retries: 5
    restart: unless-stopped
    deploy:
      resources:
        limits:
          cpus: '0.5'
          memory: 512M

volumes:
  postgres_data:
  redis_data:
```

### Step 8.3 — Create Production .env

```bash
# .env.production
# Database
DATABASE_URL=postgresql+asyncpg://user:password@postgres:5432/pymind
DB_USER=pymind
DB_PASSWORD=secure-password-here
DB_NAME=pymind

# Redis
REDIS_URL=redis://redis:6379/0

# OpenAI
OPENAI_API_KEY=sk-your-openai-key

# JWT
JWT_SECRET_KEY=generate-with-openssl-rand-hex-32

# App
APP_ENV=production
LOG_LEVEL=INFO
```

### Step 8.4 — Create Nginx Config (Optional)

```nginx
# nginx.conf
upstream pymind {
    server app:8000;
}

server {
    listen 80;
    server_name your-domain.com;

    location / {
        proxy_pass http://pymind;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    location /docs {
        proxy_pass http://pymind;
    }
}
```

### Step 8.5 — Deploy to Render/Railway

#### Render.com

1. Create account at render.com
2. Connect GitHub repository
3. Create new Web Service
4. Configure:
   - Build Command: `pip install -e .`
   - Start Command: `uvicorn app.main:app --host 0.0.0.0 --port $PORT`
5. Add environment variables
6. Deploy

#### Railway

1. Create account at railway.app
2. Create new project
3. Add PostgreSQL plugin
4. Add Redis plugin
5. Deploy from GitHub
6. Set environment variables

### Step 8.6 — Production Health Checks

```python
# app/main.py additions

@app.get("/health")
async def health_check() -> JSONResponse:
    """Basic health check."""
    return JSONResponse(
        status_code=200,
        content={"status": "healthy"},
    )


@app.get("/health/ready")
async def readiness_check() -> JSONResponse:
    """Readiness check - includes dependencies."""
    checks = {"database": False, "redis": False}
    
    # Check database
    try:
        from app.core.database import engine
        async with engine.connect() as conn:
            await conn.execute(text("SELECT 1"))
        checks["database"] = True
    except Exception:
        pass
    
    # Check Redis
    try:
        from app.core.redis import redis_client
        if redis_client:
            await redis_client.ping()
        checks["redis"] = True
    except Exception:
        pass
    
    all_healthy = all(checks.values())
    
    return JSONResponse(
        status_code=200 if all_healthy else 503,
        content={"status": "healthy" if all_healthy else "unhealthy", "checks": checks},
    )
```

## Deployment Commands

```bash
# Build and run locally
docker-compose -f docker-compose.prod.yml up -d --build

# View logs
docker-compose -f docker-compose.prod.yml logs -f

# Scale services
docker-compose -f docker-compose.prod.yml up -d --scale app=3
```

## Phase Summary

**What was built:**
- Production Dockerfile
- Production Docker Compose
- Health check endpoints
- Nginx configuration (optional)
- Deployment guides

**What was learned:**
- Docker multi-stage builds
- Production configuration
- Health checks
- Deployment platforms
