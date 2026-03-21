# Dockerfile

## What You'll Build In This File

A multi-stage Dockerfile for the NodeMark API optimized for production.

## Complete Dockerfile

Create `Dockerfile`:

```dockerfile
# Stage 1: Builder
# Build the application
FROM node:20-alpine AS builder

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install all dependencies (including dev for building)
RUN npm ci

# Copy source code
COPY . .

# Stage 2: Production
# Minimal image for running the app
FROM node:20-alpine

WORKDIR /app

# Set production environment
ENV NODE_ENV=production

# Copy only production dependencies
COPY package*.json ./
RUN npm ci --omit=dev && npm cache clean --force

# Copy built application from builder
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/src ./src
COPY --from=builder /app/.env.example ./.env

# Create non-root user for security
RUN addgroup -g 1001 -S nodejs && \
    adduser -S nodejs -u 1001

# Use non-root user
USER nodejs

# Expose application port
EXPOSE 3000

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD node -e "require('http').get('http://localhost:3000/health', (r) => process.exit(r.statusCode === 200 ? 0 : 1))"

# Start the application
CMD ["node", "src/index.js"]
```

## Dockerfile Breakdown

| Stage | Purpose |
|-------|---------|
| **builder** | Installs all deps, builds app |
| **production** | Minimal image with only what's needed |

Key optimizations:
- Multi-stage reduces final image size
- Non-root user improves security
- .dockerignore excludes unnecessary files

## .dockerignore File

Create `.dockerignore`:

```
node_modules
npm-debug.log
.env
.env.*
!.env.example
dist
coverage
*.md
tests
.git
.gitignore
Dockerfile
docker-compose*
```

## How It Connects

This connects to concepts from:
- [10-deployment/docker/01-dockerfile.md](../../../10-deployment/docker/01-dockerfile.md)

## Common Mistakes

- Including dev dependencies in production image
- Not using multi-stage builds
- Running as root user
- Not setting NODE_ENV

## Try It Yourself

### Exercise 1: Build Docker Image
Build the Docker image and verify it works.

### Exercise 2: Check Image Size
Compare image size with and without multi-stage.

### Exercise 3: Add Health Check
Verify the health check endpoint works.

## Next Steps

Continue to [02-docker-compose.md](./02-docker-compose.md) to add Docker Compose.
