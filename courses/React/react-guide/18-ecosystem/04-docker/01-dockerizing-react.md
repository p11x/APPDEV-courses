# Dockerizing React

## Overview
Docker allows you to containerize your React application, making it easy to deploy and run consistently across different environments.

## Prerequisites
- Docker installed
- React project

## Core Concepts

### Basic Dockerfile

```dockerfile
# [File: Dockerfile]
# Use Node.js as base image
FROM node:20-alpine

# Set working directory
WORKDIR /app

# Copy package files first (for better caching)
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy source code
COPY . .

# Build the application
RUN npm run build

# Expose port
EXPOSE 3000

# Start the app
CMD ["npm", "start"]
```

### Optimized Dockerfile

```dockerfile
# [File: Dockerfile.optimized]
# Build stage
FROM node:20-alpine AS builder

WORKDIR /app

# Copy and install dependencies first (better layer caching)
COPY package*.json ./
RUN npm ci

# Copy source
COPY . .

# Build with environment variables
ENV REACT_APP_API_URL=https://api.example.com
RUN npm run build

# Production stage
FROM nginx:alpine

# Copy built assets from builder
COPY --from=builder /app/build /usr/share/nginx/html

# Custom nginx config for SPA routing
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

### Docker Compose

```yaml
# [File: docker-compose.yml]
version: '3.8'

services:
  react-app:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "3000:3000"
    environment:
      - REACT_APP_API_URL=http://localhost:8080
    volumes:
      - ./src:/app/src
    depends_on:
      - api

  api:
    build: ./api
    ports:
      - "8080:8080"
    environment:
      - DATABASE_URL=postgres://db:5432/app
    depends_on:
      - db

  db:
    image: postgres:15
    environment:
      - POSTGRES_DB=app
      - POSTGRES_PASSWORD=secret
    volumes:
      - pgdata:/var/lib/postgresql/data

volumes:
  pgdata:
```

## Key Takeaways
- Multi-stage builds reduce image size
- Layer caching speeds up builds
- Docker Compose orchestrates multi-container apps

## What's Next
Continue to [Multi-Stage Builds](02-multi-stage-builds.md) for advanced optimization.