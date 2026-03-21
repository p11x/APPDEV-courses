# Deploy to VPS

## What You'll Learn
- Deploy to VPS/Docker
- Docker setup
- Production configuration

## Prerequisites
- VPS or Docker host

## Do I Need This Right Now?
Self-hosting Next.js on your own server.

## Docker Setup

```dockerfile
# Dockerfile
FROM node:20-alpine

WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

EXPOSE 3000
CMD ["npm", "start"]
```

```yaml
# docker-compose.yml
version: '3'
services:
  web:
    build: .
    ports:
      - "3000:3000"
    environment:
      - DATABASE_URL=postgres://...
```

## Summary
- Use Docker for consistent deployments
- Build and run on your server
- Good alternative to Vercel

## Next Steps
- [rollback-strategies.md](./rollback-strategies.md) — Rollback strategies
