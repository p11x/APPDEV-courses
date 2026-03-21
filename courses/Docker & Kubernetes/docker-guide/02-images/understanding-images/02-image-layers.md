# Image Layers

## Overview

Docker images consist of multiple stacked layers, where each layer represents a set of filesystem changes from the layer below it. Understanding image layers is crucial for optimizing Docker builds, reducing image size, and troubleshooting build issues. Layers enable Docker's efficiency by allowing images to share common data and cache intermediate results.

## Prerequisites

- Understanding of what Docker images are (previous section)
- Basic Docker command-line knowledge
- Familiarity with filesystem concepts

## Core Concepts

### Layer Architecture

Docker images are built as a series of read-only layers. Each layer contains the changes from the previous layer, stored as a set of files. When you create a container from an image, Docker adds a writable container layer on top of all the image layers. Any changes made in the container (like writing files or modifying existing ones) are written to this writable layer.

This layered architecture provides several key benefits:

1. **Storage Efficiency**: Multiple images can share the same base layers. If you have 100 containers running the same base image, they all share the underlying layers.
2. **Build Speed**: Docker caches layers during builds. If a layer hasn't changed, Docker reuses the cached version instead of rebuilding it.
3. **Transfer Efficiency**: When pushing or pulling images, Docker only transfers layers that don't already exist on the destination.
4. **Immutability**: Image layers are read-only, ensuring consistency and reproducibility.

### Layer Storage

Each layer in a Docker image is stored as a compressed tar file in the Docker storage directory (`/var/lib/docker` on Linux). When you build an image, Docker calculates a unique content hash for each layer. If two layers have the same content, they share the same storage, regardless of what image they belong to.

The layer content-addressable storage uses SHA256 hashes, ensuring content integrity. Two layers with identical content will have identical hashes, regardless of where they come from.

### Union Filesystem

Docker uses union filesystems (like OverlayFS on modern Linux, or AUFS, Btrfs, ZFS on older systems) to combine multiple layers into a single unified view. To the container, it appears as though all the layers are merged into one filesystem, with later layers overwriting earlier ones.

Modern Docker uses OverlayFS by default on most Linux systems. It combines two directories - a lower directory (the image layers) and an upper directory (the container layer) - into a unified view.

## Step-by-Step Examples

### Inspecting Image Layers

Let's examine how layers work in practice:

```bash
# Pull an image and see its layers
docker pull nginx:1.25-alpine

# View layer history
# This shows each layer and its creation command
docker history nginx:1.25-alpine
# Output shows: CREATED BY, SIZE, CREATED AT for each layer

# Inspect image to see layer information
docker image inspect nginx:1.25-alpine
# Look for "RootFS" section showing layer digests

# Show only the layer digests
docker image inspect --format='{{.RootFS.Layers}}' nginx:1.25-alpine

# Build a simple image and see its layers
# Create a Dockerfile:
cat > /tmp/test-dockerfile << 'EOF'
FROM alpine:3.19
RUN echo "hello" > /hello.txt
RUN echo "world" > /world.txt
EOF

# Build and inspect
docker build -t test-layers /tmp/test-dockerfile
docker history test-layers
```

### Understanding Layer Caching

Layer caching is fundamental to Docker build performance:

```bash
# Create a Dockerfile demonstrating caching
mkdir -p /tmp/cache-demo
cat > /tmp/cache-demo/Dockerfile << 'EOF'
FROM alpine:3.19
RUN echo "Step 1 - this takes time"
RUN sleep 2 && echo "Step 1 done" > /step1.txt
RUN echo "Step 2 - this should be cached"
RUN echo "Step 2 done" > /step2.txt
EOF

# First build - takes time for both steps
docker build -t cache-test /tmp/cache-demo
# Step 1: runs and takes ~2 seconds
# Step 2: runs and takes ~2 seconds

# Modify only step 2
cat > /tmp/cache-demo/Dockerfile << 'EOF'
FROM alpine:3.19
RUN echo "Step 1 - this takes time"
RUN sleep 2 && echo "Step 1 done" > /step1.txt
RUN echo "Step 2 - modified!"
RUN echo "Step 2 done" > /step2.txt
EOF

# Rebuild - step 1 is cached, only step 2 runs
docker build -t cache-test /tmp/cache-demo
# Step 1: using cache (instant)
# Step 2: runs fresh (takes time)
```

### Sharing Layers Between Images

Let's see how layers are shared:

```bash
# Pull two related images
docker pull ubuntu:22.04
docker pull ubuntu:22.04.3

# Check disk usage
docker system df
# Shows space used by images, containers, build cache

# Pull a different image based on same parent
docker pull nginx:1.25-alpine

# Check storage again - alpine shares nothing with ubuntu
# But nginx:alpine variants share alpine base
docker pull nginx:1.24-alpine
docker system df
# Notice minimal increase - most layers were cached
```

## Common Mistakes

- **Not understanding build cache**: Beginners often don't realize that Docker caches build steps. This can lead to unexpected behavior when modifying Dockerfiles.
- **Changing layer order unnecessarily**: More frequently changed layers should be placed later in the Dockerfile to maximize cache hit rate.
- **Combining multiple commands**: Using && to chain commands creates a single layer, reducing caching effectiveness. Use separate RUN commands for better layer utilization.
- **Not cleaning up in the same layer**: Files created in one RUN command and deleted in another still exist in the image layer. Use multi-stage builds or clean up in the same command.
- **Assuming layers are always shared**: Only identical layer content is shared. Even a single character difference creates a new layer.
- **Ignoring layer count**: Too many layers can impact performance. While modern Docker handles hundreds of layers, extremely deep stacks can slow operations.

## Quick Reference

| Concept | Description |
|---------|-------------|
| Layer | Filesystem changes stored as a delta |
| Union filesystem | Merges multiple layers into one view |
| Build cache | Stored layers used to speed up rebuilds |
| Content-addressable | Layers identified by content hash |
| Shared layers | Same layers reused across images |
| Container layer | Writable layer on top of image layers |

## What's Next

Now that you understand image layers, continue to [Image Registries](./03-image-registries.md) to learn where to store and distribute your Docker images.
