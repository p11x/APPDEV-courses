# Docker Publish

## What You'll Learn

- How to build and push Docker images in GitHub Actions
- How to use docker/build-push-action
- How to authenticate with GitHub Container Registry (GHCR)
- How to tag images with version and latest
- How to use build cache for faster builds

## Workflow

```yaml
# .github/workflows/docker-publish.yml

name: Docker Publish

on:
  push:
    branches: [main]
    tags: ['v*']                    # Trigger on version tags (v1.0.0)
  pull_request:
    branches: [main]

env:
  REGISTRY: ghcr.io
  IMAGE_NAME: ${{ github.repository }}

jobs:
  build-and-push:
    runs-on: ubuntu-latest

    # Permissions for pushing to GHCR
    permissions:
      contents: read
      packages: write

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      # Log in to GitHub Container Registry
      - name: Log in to GHCR
        uses: docker/login-action@v3
        with:
          registry: ${{ env.REGISTRY }}
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}  # Auto-provided by GitHub

      # Extract metadata for tagging
      - name: Extract metadata
        id: meta
        uses: docker/metadata-action@v5
        with:
          images: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}
          tags: |
            # Tag with git SHA
            type=sha
            # Tag with branch name
            type=ref,event=branch
            # Tag with version from git tag (v1.0.0 → 1.0.0)
            type=semver,pattern={{version}}
            # Tag with major.minor (v1.0.0 → 1.0)
            type=semver,pattern={{major}}.{{minor}}
            # Tag as latest on default branch
            type=raw,value=latest,enable={{is_default_branch}}

      # Build and push the Docker image
      - name: Build and push
        uses: docker/build-push-action@v5
        with:
          context: .
          # Only push on main branch or tags (not on PRs)
          push: ${{ github.event_name != 'pull_request' }}
          # Use tags from the metadata step
          tags: ${{ steps.meta.outputs.tags }}
          # Use labels from the metadata step
          labels: ${{ steps.meta.outputs.labels }}
          # Enable build cache
          cache-from: type=gha        # GitHub Actions cache
          cache-to: type=gha,mode=max
```

## Dockerfile

```dockerfile
# Dockerfile — Multi-stage build for Node.js

# Stage 1: Install dependencies
FROM node:20-alpine AS deps
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production

# Stage 2: Build (if needed)
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Stage 3: Production
FROM node:20-alpine AS production
WORKDIR /app

# Create non-root user for security
RUN addgroup -g 1001 -S appgroup && \
    adduser -S appuser -u 1001 -G appgroup

# Copy only production dependencies
COPY --from=deps /app/node_modules ./node_modules
# Copy build output
COPY --from=build /app/dist ./dist
COPY --from=build /app/package.json ./

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 3000

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:3000/healthz || exit 1

# Start the app
CMD ["node", "dist/index.js"]
```

## Using the Published Image

```bash
# Pull the image
docker pull ghcr.io/yourname/yourrepo:latest

# Run it
docker run -p 3000:3000 ghcr.io/yourname/yourrepo:latest

# Run a specific version
docker run -p 3000:3000 ghcr.io/yourname/yourrepo:1.0.0
```

## How It Works

### Image Tags

| Tag | When Applied | Example |
|-----|-------------|---------|
| `latest` | Push to main | `ghcr.io/user/repo:latest` |
| `sha-abc1234` | Every push | `ghcr.io/user/repo:sha-abc1234` |
| `1.0.0` | Git tag `v1.0.0` | `ghcr.io/user/repo:1.0.0` |
| `1.0` | Git tag `v1.0.0` | `ghcr.io/user/repo:1.0` |
| `main` | Push to main | `ghcr.io/user/repo:main` |

### Build Cache

```yaml
cache-from: type=gha   # Read cache from GitHub Actions cache
cache-to: type=gha,mode=max  # Write all layers to cache
```

This caches Docker layers between builds, making subsequent builds much faster.

## Common Mistakes

### Mistake 1: Not Using Multi-Stage Builds

```dockerfile
# WRONG — final image includes dev dependencies and source code (500MB+)
FROM node:20
COPY . .
RUN npm ci
CMD ["node", "index.js"]

# CORRECT — multi-stage build (50MB)
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM node:20-alpine
COPY --from=build /app/dist ./dist
COPY --from=build /app/node_modules ./node_modules
CMD ["node", "dist/index.js"]
```

### Mistake 2: Running as Root

```dockerfile
# WRONG — container runs as root (security risk)
FROM node:20
CMD ["node", "index.js"]

# CORRECT — create and use a non-root user
FROM node:20
RUN addgroup -g 1001 -S app && adduser -S app -u 1001
USER app
CMD ["node", "index.js"]
```

### Mistake 3: No .dockerignore

```bash
# WRONG — node_modules, .git, etc. are copied into the build context (slow)
# .dockerignore does not exist

# CORRECT — create .dockerignore
node_modules
.git
*.md
.env
coverage
```

## Try It Yourself

### Exercise 1: Build and Push

Create a workflow that builds and pushes your Docker image to GHCR on every push to main.

### Exercise 2: Version Tags

Create a git tag `v1.0.0`. Verify the image is tagged as both `1.0.0` and `1.0`.

### Exercise 3: Multi-Stage Dockerfile

Create a multi-stage Dockerfile that reduces your image size by 80%.

## Next Steps

You can publish Docker images. For deploying to a VPS, continue to [Deploy to VPS](../deployment/01-deploy-to-vps.md).
