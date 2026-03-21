# Docker for Node.js

## What You'll Learn

- Creating a Dockerfile
- Multi-stage builds
- Best practices

## Dockerfile

```dockerfile
# Dockerfile - Node.js application

# Use official Node.js image
FROM node:20-alpine

# Set working directory
WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy source code
COPY . .

# Expose port
EXPOSE 3000

# Start the app
CMD ["node", "index.js"]
```

## Multi-Stage Build

```dockerfile
# Multi-stage build for smaller image

# Build stage
FROM node:20-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Production stage
FROM node:20-alpine
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY --from=builder /app/dist ./dist
CMD ["node", "dist/index.js"]
```

## Code Example

```dockerfile
# Complete Dockerfile

FROM node:20-alpine

WORKDIR /app

# Install dependencies first (better caching)
COPY package*.json ./
RUN npm ci

# Copy source
COPY . .

# Environment variables
ENV NODE_ENV=production

# Expose port
EXPOSE 3000

# Run as non-root user
RUN addgroup -g 1001 -S nodejs
RUN adduser -S nodejs -u 1001
USER nodejs

CMD ["node", "index.js"]
```

## Building and Running

```bash
# Build
docker build -t my-app .

# Run
docker run -p 3000:3000 my-app
```

## Try It Yourself

### Exercise 1: Create Dockerfile
Create a Dockerfile for your Node.js app.

### Exercise 2: Multi-Stage Build
Create a multi-stage Dockerfile.
