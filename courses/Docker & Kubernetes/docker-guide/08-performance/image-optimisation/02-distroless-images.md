# Distroless Images

## Overview

Google's distroless images provide the minimal necessary runtime for applications, dramatically reducing attack surface and image size. This guide covers using and debugging distroless containers.

## Prerequisites

- Docker Engine 20.10+
- Understanding of multi-stage builds

## Core Concepts

### What Are Distroless Images

- Minimal base images from Google
- No package manager, shell, or unnecessary tools
- Only the application and required runtime
- Smaller attack surface than Alpine

### Available Images

| Image | Size | Use Case |
|-------|------|----------|
| gcr.io/distroless/base | ~17 MB | Static binaries |
| gcr.io/distroless/cc | ~15 MB | C/C++ compiled |
| gcr.io/distroless/java | ~150 MB | Java JRE |
| gcr.io/distroless/python3 | ~60 MB | Python |
| gcr.io/distroless/nodejs | ~100 MB | Node.js |

## Step-by-Step Examples

### Using Distroless Base

```dockerfile
# Multi-stage build with distroless
FROM golang:1.21 AS builder
WORKDIR /app
COPY . .
RUN CGO_ENABLED=0 GOOS=linux go build -o myapp

# Final stage: distroless
FROM gcr.io/distroless/base:nonroot
WORKDIR /app
COPY --from=builder /app/myapp .
USER nonroot:nonroot
CMD ["./myapp"]
```

### Distroless with Static Binary

```dockerfile
# Build fully static Go binary
FROM golang:1.21 AS builder
WORKDIR /app
COPY . .
ENV CGO_ENABLED=0
ENV GOOS=linux
RUN go build -ldflags="-s -w" -o myapp

FROM gcr.io/distroless/base:nonroot
COPY --from=builder /app/myapp .
USER nonroot
CMD ["./myapp"]
```

### Adding CA Certificates

```dockerfile
# For HTTPS support, copy certs
FROM gcr.io/distroless/base:nonroot
COPY --from=builder /app/myapp .
COPY --from=builder /etc/ssl/certs/ca-certificates.crt /etc/ssl/certs/
USER nonroot
CMD ["./myapp"]
```

### Debug Image

```dockerfile
# Use debug variant for troubleshooting
FROM gcr.io/distroless/base:debug
# Includes busybox shell for debugging
```

### Java with Distroless

```dockerfile
# Multi-stage for Java
FROM maven:3.9-eclipse-temurin-21 AS builder
WORKDIR /app
COPY pom.xml .
COPY src ./src
RUN mvn package -DskipTests

FROM gcr.io/distroless/java:17
COPY --from=builder /app/target/myapp.jar /app/myapp.jar
WORKDIR /app
CMD ["myapp.jar"]
```

## Debugging Distroless

```bash
# Use debug image variant
docker run -it myimage:debug

# Copy files from container
docker cp container:/app .

# Use ephemeral debug container
docker run --rm -it gcr.io/distroless/base:debug \
  -- mounts
```

## Gotchas for Docker Users

- **No shell**: Can't exec into container
- **No package manager**: Can't install tools
- **Debug variant**: Use :debug tag for debugging
- **Static binaries**: Must be fully static

## Common Mistakes

- **No shell access**: Can't run commands interactively
- **Missing certs**: HTTPS may fail without CA certificates
- **Wrong user**: Distroless uses nonroot by default

## Quick Reference

| Tag | Description |
|-----|-------------|
| :latest | Non-root, latest |
| :nonroot | Non-root user |
| :root | Root user |
| :debug | Includes shell |

## What's Next

Continue to [Scratch Base Images](./03-scratch-base-images.md) for the smallest possible images.
