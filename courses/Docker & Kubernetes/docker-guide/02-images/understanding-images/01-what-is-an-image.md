# What is an Image

## Overview

A Docker image is a read-only template that provides instructions for creating a Docker container. Images are the building blocks of containerization, containing the application code, runtime, system tools, libraries, and settings needed to run your application. Understanding images is fundamental to working effectively with Docker, as containers are created from images.

## Prerequisites

- Docker installed on your system
- Basic understanding of containers (from previous sections)
- Familiarity with command-line interface

## Core Concepts

### Image Definition

A Docker image is an immutable snapshot of a filesystem. Think of it as a template or blueprint that describes how to create a container. Unlike a running container, an image doesn't change - it's a static artifact that can be stored, shared, and reused. When you run a container from an image, Docker creates a writable layer on top of the image's read-only layers.

Images are created using Dockerfiles, which are text files containing step-by-step instructions. Each instruction in a Dockerfile creates a new layer in the image. These layers are cached, making subsequent builds much faster because Docker only needs to rebuild changed layers.

### Image Naming and Tags

Docker images are identified by names that follow the format `registry/repository:tag`. If you don't specify a tag, Docker defaults to `latest`.

- `nginx` - Uses the "latest" tag implicitly
- `nginx:1.25` - Specific version 1.25
- `nginx:alpine` - Alpine variant (lightweight)
- `redis:7.2-alpine` - Specific version with Alpine base
- `myregistry.com:5000/myapp:v1.0` - Custom registry with port and tag

The tag is simply a label that helps you identify specific versions of an image. Tags don't have to be semantic versions - you can use any labels like `latest`, `development`, `production`, or dates.

### Image vs Container

Understanding the difference between images and containers is crucial:

- **Image** is the static template - like a class in object-oriented programming
- **Container** is the running instance - like an object created from a class

Multiple containers can run from the same image, and each container has its own writable layer while sharing the underlying image layers. This makes containers extremely efficient because running multiple containers doesn't duplicate the base image data.

### Base Images

Every Docker image is based on another image, forming a chain from a base image to your final application image. The base image is typically a minimal operating system like Alpine Linux, Ubuntu, or Debian. Popular minimal base images include:

- `alpine` (~5 MB) - Minimal Linux distribution, highly recommended for production
- `debian:bookworm-slim` (~80 MB) - Smaller Debian variant
- `ubuntu:22.04` - Full Ubuntu, more compatibility but larger
- `scratch` - Special empty image, used to create images from scratch

Alpine Linux is the most common base for production images because it's security-focused and extremely small, reducing the attack surface and download time.

## Step-by-Step Examples

### Pulling and Inspecting Images

Let's explore images on your system:

```bash
# Pull an image from Docker Hub
# docker pull downloads an image from a registry
docker pull alpine:latest

# List all images on your system
# This shows locally cached images
docker image ls

# Pull specific versions
docker pull python:3.12-slim
docker pull node:20-alpine
docker pull nginx:1.25-alpine

# Inspect an image to see its details
# This shows configuration including environment variables, ports, etc.
docker image inspect alpine:latest

# Show image layers
# Each line represents a layer in the image
docker history alpine:latest
```

### Running Containers from Images

Images become containers when you run them:

```bash
# Run a container from an image
# docker run creates and starts a container
docker run --rm alpine:latest echo "Hello from container"

# Run an interactive container
# -it combines -i (interactive) and -t (pseudo-TTY)
docker run -it alpine:latest /bin/sh

# Inside the container, explore:
# cat /etc/os-release   # Check OS
# whoami               # Check user
# pwd                  # Check directory
# exit                 # Exit container

# Run a container in detached mode
# -d runs in background
docker run -d --name my-nginx nginx:1.25-alpine

# Check running containers
docker ps

# Stop the container
docker stop my-nginx

# Remove the container
docker rm my-nginx
```

### Working with Image IDs

Every image has a unique identifier (SHA256 hash):

```bash
# See image ID in image list
docker image ls

# Pull by digest (exact version)
# Digest is the cryptographic hash of the image manifest
docker pull alpine@sha256:4e38e38c8ce0b8d9045a3a0b049e3b9f3f8d5b8b8a8a8a8a8a8a8a8a8a8a8a

# Reference image by ID (useful in scripts)
docker image ls --no-trunc
# Use the full ID to reference the image
docker run 4e38e38c8ce0b8d9045a3a0b049e3b9f3f8d5b8b8a8a8a8a8a8a8a8a8a8a8a8a8a8 /bin/sh
```

## Common Mistakes

- **Using :latest tag in production**: The :latest tag changes over time, causing inconsistent deployments. Always use specific version tags.
- **Not cleaning up images**: Old images consume disk space. Use `docker image prune` regularly.
- **Assuming images are secure**: Base images may have vulnerabilities. Always scan images for security issues.
- **Confusing image tags with Git tags**: Docker tags serve a similar purpose but are independent of Git tags.
- **Pulling images without verification**: For production, verify images come from trusted registries and maintainers.
- **Not understanding image layers**: Changes to intermediate layers don't affect running containers until a new image is built.

## Quick Reference

| Concept | Description |
|---------|-------------|
| Image | Read-only template for creating containers |
| Tag | Version label for an image (e.g., 1.25, latest) |
| Layer | Intermediate filesystem changes in an image |
| Base Image | The image an image is built upon |
| Registry | Storage location for images (Docker Hub, ECR, etc.) |
| Digest | Cryptographic hash identifying exact image content |

## What's Next

Now that you understand what images are, continue to [Image Layers](./02-image-layers.md) to learn how images are structured and how Docker efficiently stores and shares them.
