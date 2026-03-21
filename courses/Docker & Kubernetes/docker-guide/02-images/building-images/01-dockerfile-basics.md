# Dockerfile Basics

## Overview

A Dockerfile is a text file containing instructions for building a Docker image. It's the blueprint that defines what goes into your container: the base operating system, application code, dependencies, configuration, and runtime commands. Writing efficient Dockerfiles is a core skill for containerizing applications.

## Prerequisites

- Understanding of Docker images and layers
- Basic text editor knowledge
- Familiarity with command-line interface
- Understanding of your application's dependencies

## Core Concepts

### Dockerfile Structure

A Dockerfile consists of instructions that are executed sequentially. Each instruction creates a new layer in the image. The most common instructions are:

- **FROM**: Sets the base image (required as the first instruction)
- **RUN**: Executes commands during build (creates layers)
- **COPY**: Copies files from build context to image
- **ADD**: Similar to COPY but can handle URLs and tar extraction
- **WORKDIR**: Sets the working directory
- **ENV**: Sets environment variables
- **EXPOSE**: Documents which ports the container listens on
- **CMD**: Default command to run when container starts
- **ENTRYPOINT**: Configures the container as an executable

### Build Context

The build context is the directory containing files referenced in the Dockerfile. When you run `docker build`, Docker sends everything in the context to the daemon. Exclude unnecessary files using .dockerignore to speed up builds and reduce image size.

### Image Naming

After building, you can tag your image with:
```bash
docker build -t myapp:1.0 .
docker build -t myregistry.com/myapp:latest .
```

## Step-by-Step Examples

### Your First Dockerfile

Create a simple Dockerfile for a Node.js application:

```dockerfile
# Use an official Node.js runtime as a parent image
# FROM sets the base image - always start with this
FROM node:20-alpine

# Set the working directory in the container
# WORKDIR creates the directory if it doesn't exist
WORKDIR /app

# Copy package files first (for better caching)
# This uses the Docker build cache efficiently
COPY package*.json ./

# Install dependencies
# RUN executes a command in a new layer
RUN npm install

# Copy application code
COPY . .

# Expose the port the app runs on
# This is documentation - doesn't actually publish the port
EXPOSE 3000

# Define the command to run your app
# CMD can be overridden at runtime
CMD ["node", "server.js"]
```

### Building and Running

```bash
# Build the image from Dockerfile in current directory
# -t tags the image with name:tag
# . specifies the build context (current directory)
docker build -t mynodeapp:1.0 .

# List the built images
docker image ls | grep mynodeapp

# Run a container from the image
# -p 3000:3000 maps host port 3000 to container port 3000
# -d runs in detached mode (background)
docker run -d -p 3000:3000 --name myapp mynodeapp:1.0

# View running containers
docker ps

# Stop and remove the container
docker stop myapp
docker rm myapp
```

### A Python Dockerfile

For Python applications:

```dockerfile
# Use Python Alpine image for small size
FROM python:3.12-slim

# Set working directory
WORKDIR /app

# Copy dependency file first
COPY requirements.txt .

# Install dependencies
# Use --no-cache-dir to reduce image size
RUN pip install --no-cache-dir -r requirements.txt

# Copy application code
COPY . .

# Set environment variables
# These are available when the container runs
ENV PYTHONUNBUFFERED=1

# Expose port
EXPOSE 8000

# Run the application
CMD ["python", "main.py"]
```

### A Go Dockerfile

For Go applications:

```dockerfile
# Build stage
FROM golang:1.21-alpine AS builder

# Install build dependencies
RUN apk add --no-cache git

WORKDIR /build

# Copy and download dependencies
COPY go.mod go.sum ./
RUN go mod download

# Copy source and build
COPY . .
RUN CGO_ENABLED=0 GOOS=linux go build -o myapp

# Runtime stage - smaller final image
FROM alpine:3.19

WORKDIR /app

# Copy binary from builder
COPY --from=builder /build/myapp .

# Create non-root user for security
RUN adduser -D -g '' appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8080

CMD ["./myapp"]
```

### Using .dockerignore

Create a .dockerignore file in your project root:

```
# Ignore node_modules from host
node_modules

# Ignore git
.git
.gitignore

# Ignore build artifacts
dist
build

# Ignore IDE files
.vscode
.idea
*.swp
*.swo

# Ignore documentation
README.md
*.md

# Ignore secrets
.env
.env.*
```

## Common Mistakes

- **Not using .dockerignore**: Including unnecessary files slows builds and increases image size.
- **Copying everything before dependencies**: Dependencies should be copied and installed first to leverage build cache.
- **Running as root**: Always create and use a non-root user for security.
- **Not setting WORKDIR**: Relative paths may not work as expected without a working directory.
- **Using ADD for simple copies**: COPY is clearer and more explicit than ADD.
- **Forgetting to expose ports**: EXPOSE documents the port but doesn't actually publish it (use -p at runtime).
- **CMD vs ENTRYPOINT confusion**: CMD can be overridden, ENTRYPOINT defines the executable.

## Quick Reference

| Instruction | Purpose |
|-------------|---------|
| FROM | Base image |
| RUN | Execute command |
| COPY | Copy files |
| WORKDIR | Set working directory |
| ENV | Environment variable |
| EXPOSE | Document port |
| CMD | Default command |
| USER | Set user |

| Command | Description |
|---------|-------------|
| `docker build -t name:tag .` | Build image from Dockerfile |
| `docker build -f Dockerfile.dev` | Use alternate Dockerfile |
| `docker build --no-cache` | Build without cache |
| `docker image ls` | List built images |
| `docker image prune` | Clean up unused images |

## What's Next

Now that you understand Dockerfile basics, continue to [Dockerfile Advanced](./02-dockerfile-advanced.md) to learn advanced Dockerfile instructions and best practices.
