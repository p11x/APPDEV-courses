# Express.js Docker Containerization

## What You'll Learn

- Dockerfile best practices
- Multi-stage builds
- Health checks in containers
- Environment configuration

## Production Dockerfile

```dockerfile
FROM node:22-alpine AS base
WORKDIR /app

# Dependencies stage
FROM base AS deps
COPY package*.json ./
RUN npm ci --only=production && npm cache clean --force

# Build stage
FROM base AS build
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Production stage
FROM base AS runtime
ENV NODE_ENV=production
COPY --from=deps /app/node_modules ./node_modules
COPY --from=build /app/dist ./dist
COPY package*.json ./

USER node
EXPOSE 3000

HEALTHCHECK --interval=30s --timeout=3s \
    CMD node healthcheck.js || exit 1

CMD ["node", "dist/server.js"]
```

## Docker Compose

```yaml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
      - DATABASE_URL=postgresql://postgres:password@db:5432/mydb
      - REDIS_URL=redis://cache:6379
    depends_on:
      db:
        condition: service_healthy
      cache:
        condition: service_started
    healthcheck:
      test: ["CMD", "node", "healthcheck.js"]
      interval: 30s
      timeout: 3s
      retries: 3

  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_PASSWORD: password
      POSTGRES_DB: mydb
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 10s

  cache:
    image: redis:7-alpine
```

## Best Practices Checklist

- [ ] Use multi-stage builds for smaller images
- [ ] Run as non-root user
- [ ] Implement health checks
- [ ] Use .dockerignore to exclude unnecessary files
- [ ] Pin base image versions

## Cross-References

- See [Monitoring](../14-monitoring-observability/01-apm-setup.md) for observability
- See [Deployment](../16-deployment-operations/01-production-deployment.md) for deployment
- See [Security](../05-security-implementation/01-helmet-cors.md) for security

## Next Steps

Continue to [Monitoring](../14-monitoring-observability/01-apm-setup.md) for observability.
