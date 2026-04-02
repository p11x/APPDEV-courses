# Production Deployment for NodeMark

## What You'll Build In This File

Production deployment configuration, CI/CD pipeline, environment management, and operational procedures.

## Production Dockerfile

```dockerfile
# Dockerfile — Production optimized
FROM node:20-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM node:20-alpine AS production
WORKDIR /app

RUN addgroup -g 1001 -S app && adduser -S app -u 1001 -G app

COPY package*.json ./
RUN npm ci --only=production && npm cache clean --force
COPY --from=builder --chown=app:app /app/dist ./dist
COPY --chown=app:app migrations/ ./migrations/

USER app
ENV NODE_ENV=production
EXPOSE 3000

HEALTHCHECK --interval=30s --timeout=5s \
    CMD wget -qO- http://localhost:3000/health || exit 1

CMD ["node", "dist/index.js"]
```

## Docker Compose (Production)

```yaml
# docker-compose.prod.yml
version: '3.8'

services:
  app:
    image: nodemark:${TAG:-latest}
    restart: unless-stopped
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
      - DATABASE_URL=${DATABASE_URL}
      - REDIS_URL=${REDIS_URL}
      - JWT_ACCESS_SECRET=${JWT_ACCESS_SECRET}
      - JWT_REFRESH_SECRET=${JWT_REFRESH_SECRET}
    depends_on:
      postgres:
        condition: service_healthy
      redis:
        condition: service_healthy
    deploy:
      replicas: 3
      resources:
        limits:
          memory: 256M
          cpus: '0.5'

  postgres:
    image: postgres:15-alpine
    restart: unless-stopped
    environment:
      POSTGRES_DB: ${DB_NAME}
      POSTGRES_USER: ${DB_USER}
      POSTGRES_PASSWORD: ${DB_PASSWORD}
    volumes:
      - pgdata:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${DB_USER}"]
      interval: 10s
      timeout: 5s
      retries: 5

  redis:
    image: redis:7-alpine
    restart: unless-stopped
    command: redis-server --appendonly yes
    volumes:
      - redis-data:/data
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 10s

  nginx:
    image: nginx:alpine
    restart: unless-stopped
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx.conf:/etc/nginx/conf.d/default.conf
      - ./certs:/etc/nginx/certs
    depends_on:
      - app

volumes:
  pgdata:
  redis-data:
```

## Environment Configuration

```javascript
// src/config/index.js — Environment validation
import { z } from 'zod';

const configSchema = z.object({
    port: z.coerce.number().default(3000),
    nodeEnv: z.enum(['development', 'test', 'production']).default('development'),

    db: z.object({
        host: z.string(),
        port: z.coerce.number().default(5432),
        name: z.string(),
        user: z.string(),
        password: z.string(),
        poolMax: z.coerce.number().default(20),
    }),

    redis: z.object({
        url: z.string().default('redis://localhost:6379'),
    }),

    jwt: z.object({
        accessSecret: z.string().min(32),
        refreshSecret: z.string().min(32),
    }),

    email: z.object({
        host: z.string(),
        port: z.coerce.number(),
        user: z.string(),
        pass: z.string(),
        from: z.string(),
    }).optional(),

    cors: z.object({
        origin: z.string().default('*'),
    }),
});

export const config = configSchema.parse({
    port: process.env.PORT,
    nodeEnv: process.env.NODE_ENV,
    db: {
        host: process.env.DB_HOST,
        port: process.env.DB_PORT,
        name: process.env.DB_NAME,
        user: process.env.DB_USER,
        password: process.env.DB_PASSWORD,
        poolMax: process.env.DB_POOL_MAX,
    },
    redis: { url: process.env.REDIS_URL },
    jwt: {
        accessSecret: process.env.JWT_ACCESS_SECRET,
        refreshSecret: process.env.JWT_REFRESH_SECRET,
    },
    email: process.env.EMAIL_HOST ? {
        host: process.env.EMAIL_HOST,
        port: process.env.EMAIL_PORT,
        user: process.env.EMAIL_USER,
        pass: process.env.EMAIL_PASS,
        from: process.env.EMAIL_FROM,
    } : undefined,
    cors: { origin: process.env.CORS_ORIGIN },
});
```

## Deployment Script

```bash
#!/bin/bash
# scripts/deploy.sh — Production deployment script
set -e

TAG=${1:-$(git rev-parse --short HEAD)}
ENV=${2:-production}

echo "Deploying NodeMark $TAG to $ENV"

# 1. Build and push image
docker build -t nodemark:$TAG .
docker tag nodemark:$TAG registry.example.com/nodemark:$TAG
docker push registry.example.com/nodemark:$TAG

# 2. Run migrations
docker run --rm \
    --env-file .env.$ENV \
    nodemark:$TAG \
    node scripts/migrate.js up

# 3. Deploy with rolling update
TAG=$TAG docker compose -f docker-compose.prod.yml up -d --no-deps app

# 4. Wait for health check
echo "Waiting for health check..."
for i in $(seq 1 30); do
    if curl -sf http://localhost:3000/health > /dev/null; then
        echo "Deployment successful!"
        exit 0
    fi
    sleep 2
done

echo "Health check failed, rolling back..."
docker compose -f docker-compose.prod.yml rollback app
exit 1
```

## Health Check Endpoints

```javascript
// src/routes/health.js — Health check routes
import { Router } from 'express';
import { pool } from '../db/index.js';

const router = Router();

router.get('/health', (req, res) => {
    res.json({ status: 'ok', uptime: process.uptime() });
});

router.get('/health/ready', async (req, res) => {
    const checks = {};

    try {
        await pool.query('SELECT 1');
        checks.database = 'ok';
    } catch {
        checks.database = 'error';
    }

    const healthy = Object.values(checks).every(v => v === 'ok');
    res.status(healthy ? 200 : 503).json({ status: healthy ? 'ready' : 'not ready', checks });
});

export { router as healthRouter };
```

## How It Connects

- Deployment follows [10-deployment/01-deployment-architecture/](../../../10-deployment/01-deployment-architecture/)
- Docker follows [10-deployment/03-container-orchestration/](../../../10-deployment/03-container-orchestration/)
- CI/CD follows [26-cicd-github-actions](../../../26-cicd-github-actions/)

## Common Mistakes

- Not validating environment variables on startup
- Not implementing graceful shutdown
- Not running migrations before deployment
- Missing health check endpoints

## Try It Yourself

### Exercise 1: Deploy Locally
Run the full stack with docker-compose.

### Exercise 2: Add Health Checks
Test health endpoints with curl.

### Exercise 3: Simulate Failure
Kill the database and verify health check returns 503.

## Next Steps

Continue to [20-monitoring-observability/01-logging-metrics.md](../20-monitoring-observability/01-logging-metrics.md).
