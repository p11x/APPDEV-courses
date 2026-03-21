# Multi-Stage Builds

## Overview
Multi-stage builds use multiple FROM statements in a Dockerfile to create optimized production images with only the necessary artifacts.

## Prerequisites
- Docker basics
- Dockerfile syntax

## Core Concepts

### Understanding Multi-Stage Builds

Multi-stage builds have multiple "stages" where each starts with a FROM instruction. You can selectively copy artifacts from one stage to another.

```dockerfile
# [File: Dockerfile.multi-stage]
# Stage 1: Build
FROM node:20-alpine AS builder

WORKDIR /app

# Copy dependency files
COPY package*.json ./

# Install all dependencies (including dev)
RUN npm ci

# Copy source
COPY . .

# Build the app
RUN npm run build

# Stage 2: Production
FROM node:20-alpine AS production

# Create non-root user
RUN addgroup -g 1001 -S nodejs
RUN adduser -S nextjs -u 1001

WORKDIR /app

# Copy only production dependencies
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package*.json ./
COPY --from=builder /app/.next ./.next

# Switch to non-root user
USER nextjs

EXPOSE 3000

CMD ["node", "server.js"]
```

### Nginx Multi-Stage

```dockerfile
# [File: Dockerfile.nginx]
# Build stage
FROM node:20-alpine AS builder

WORKDIR /app
COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

# Production stage with nginx
FROM nginx:alpine

# Copy build output
COPY --from=builder /app/build /usr/share/nginx/html

# Copy custom nginx config
COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

### Custom Nginx Config

```nginx
# [File: nginx/nginx.conf]
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    # Gzip compression
    gzip on;
    gzip_types text/plain text/css application/json application/javascript;

    # Cache static assets
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }

    # SPA routing fallback
    location / {
        try_files $uri $uri/ /index.html;
    }
}
```

## Key Takeaways
- Multi-stage builds create smaller images
- Separate build and runtime environments
- Use nginx for optimal static serving

## What's Next
This completes all modules in the React development guide!