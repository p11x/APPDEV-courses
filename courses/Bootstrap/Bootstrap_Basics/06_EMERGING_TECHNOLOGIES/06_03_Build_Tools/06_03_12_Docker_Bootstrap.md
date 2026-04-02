---
title: "Docker + Bootstrap Development"
slug: "docker-bootstrap"
difficulty: 3
tags: ["bootstrap", "docker", "containers", "devops", "ci-cd"]
prerequisites:
  - "06_03_09_ESBuild_Bootstrap"
  - "06_03_02_Webpack_Setup"
related:
  - "06_03_10_Turbopack_Setup"
  - "06_03_09_ESBuild_Bootstrap"
duration: "40 minutes"
---

# Docker + Bootstrap Development

## Overview

Docker containerizes the Bootstrap development environment, ensuring consistent builds across all team members' machines. A Docker setup includes the Node.js runtime, SCSS compiler, and all dependencies pre-configured. Multi-stage builds optimize the production image by separating the build environment from the runtime environment. Docker Compose orchestrates the development stack with hot reload, while production images serve static assets via Nginx. This eliminates "works on my machine" problems and simplifies CI/CD pipelines.

## Basic Implementation

A Dockerfile that builds and serves a Bootstrap application with Nginx.

```dockerfile
# Dockerfile
# Build stage
FROM node:20-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Production stage
FROM nginx:alpine AS production
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

```nginx
# nginx.conf
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location ~* \.(css|js|woff2?|ttf|eot|svg)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }

    gzip on;
    gzip_types text/css application/javascript text/html;
}
```

```yaml
# docker-compose.yml
version: '3.8'
services:
  web:
    build: .
    ports:
      - "3000:80"
    restart: unless-stopped
```

```bash
docker build -t bootstrap-app .
docker run -p 3000:80 bootstrap-app
```

## Advanced Variations

### Development Container with Hot Reload

Docker Compose setup with volume mounts and live file watching.

```dockerfile
# Dockerfile.dev
FROM node:20-alpine
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
EXPOSE 3000
CMD ["npm", "run", "dev"]
```

```yaml
# docker-compose.dev.yml
version: '3.8'
services:
  dev:
    build:
      context: .
      dockerfile: Dockerfile.dev
    ports:
      - "3000:3000"
    volumes:
      - .:/app
      - /app/node_modules
    environment:
      - NODE_ENV=development
      - CHOKIDAR_USEPOLLING=true
    stdin_open: true
    tty: true
```

```bash
docker compose -f docker-compose.dev.yml up
```

### Multi-Stage Build with Testing

Include a test stage in the Docker build pipeline.

```dockerfile
# Dockerfile.ci
FROM node:20-alpine AS deps
WORKDIR /app
COPY package*.json ./
RUN npm ci

FROM deps AS test
COPY . .
RUN npm run lint
RUN npm run test
RUN npm run build

FROM node:20-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .
RUN npm run build

FROM nginx:alpine AS production
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
HEALTHCHECK --interval=30s --timeout=3s CMD wget -q --spider http://localhost/ || exit 1
```

### Docker Compose with Multiple Services

Full development stack with Bootstrap app, API backend, and database.

```yaml
# docker-compose.full.yml
version: '3.8'
services:
  frontend:
    build:
      context: .
      dockerfile: Dockerfile.dev
    ports:
      - "3000:3000"
    volumes:
      - .:/app
      - /app/node_modules
    environment:
      - API_URL=http://api:8080
    depends_on:
      - api

  api:
    image: node:20-alpine
    working_dir: /app
    volumes:
      - ./api:/app
    command: npm run dev
    ports:
      - "8080:8080"
    depends_on:
      - db

  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: bootstrap_app
      POSTGRES_PASSWORD: secret
    volumes:
      - pgdata:/var/lib/postgresql/data
    ports:
      - "5432:5432"

volumes:
  pgdata:
```

### Environment-Specific Builds

```dockerfile
# Multi-env Dockerfile
FROM node:20-alpine AS base
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .

FROM base AS dev
CMD ["npm", "run", "dev"]

FROM base AS build
RUN npm run build

FROM nginx:alpine AS prod
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.prod.conf /etc/nginx/conf.d/default.conf

FROM nginx:alpine AS staging
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.staging.conf /etc/nginx/conf.d/default.conf
```

```bash
docker build --target dev -t bootstrap-app:dev .
docker build --target prod -t bootstrap-app:prod .
docker build --target staging -t bootstrap-app:staging .
```

## Best Practices

1. Use `.dockerignore` to exclude `node_modules`, `.git`, and build artifacts from the build context
2. Use multi-stage builds to keep production images small (Nginx alpine is ~25MB)
3. Pin Node.js and dependency versions in Dockerfile for reproducible builds
4. Use `npm ci` instead of `npm install` in Docker for deterministic dependency installation
5. Mount `node_modules` as an anonymous volume in development to prevent host override
6. Enable `CHOKIDAR_USEPOLLING` for file watching inside containers on non-Linux hosts
7. Use Docker BuildKit for faster builds with `DOCKER_BUILDKIT=1`
8. Add a `HEALTHCHECK` instruction to production containers
9. Run containers as non-root users in production for security
10. Use multi-stage targets to build different environments from the same Dockerfile
11. Layer Docker instructions from least to most frequently changing for cache efficiency
12. Use `.env` files for environment-specific configuration
13. Set `NODE_ENV=production` in production builds for optimal npm behavior
14. Compress static assets in the build stage, not at runtime

## Common Pitfalls

1. **Copying node_modules**: Including host `node_modules` in the Docker build context
2. **No .dockerignore**: Sending gigabytes of files to the Docker daemon unnecessarily
3. **File watching broken**: Hot reload not working inside containers without polling
4. **Large images**: Not using multi-stage builds resulting in 1GB+ production images
5. **Cache invalidation**: Placing `COPY . .` before `RUN npm ci`, breaking Docker layer cache
6. **Port conflicts**: Not mapping container ports correctly in docker-compose
7. **Permission issues**: Files created inside containers owned by root on the host

## Accessibility Considerations

Docker containers do not affect Bootstrap's accessibility features. Ensure the Nginx configuration serves all CSS and font files needed for accessible rendering. Test the containerized application with screen readers on the host machine. Verify that the production container serves correct MIME types for assistive technology resource loading. Include accessibility testing in the CI stage of the Docker build pipeline. Ensure `aria-*` attributes survive the build and minification process.

## Responsive Behavior

Docker deployment does not alter Bootstrap's responsive behavior. Nginx serves the same CSS and HTML as any other hosting method. Test responsive layouts by accessing the containerized app on different devices via the exposed port. Ensure that gzip compression in Nginx correctly handles all CSS media queries. Verify that Bootstrap's responsive font sizes (`rfs`) compile correctly in the containerized build. Use Docker's port mapping to test on real mobile devices on the same network.
