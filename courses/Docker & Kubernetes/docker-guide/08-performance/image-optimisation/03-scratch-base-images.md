# Scratch Base Images

## Overview

The `scratch` image is the most minimal base possible - literally nothing. It's perfect for statically compiled languages like Go and Rust, producing the smallest images possible.

## Prerequisites

- Docker Engine 20.10+
- Understanding of multi-stage builds
- Static binary compilation knowledge

## Core Concepts

### What is Scratch

- Empty base image
- Contains absolutely nothing
- No OS, no libraries, no shell
- Container starts with just your binary

### When to Use Scratch

- Compiled languages (Go, Rust, C)
- Fully static binaries
- Maximum size reduction needed
- Security-critical applications

## Step-by-Step Examples

### Go Binary on Scratch

```dockerfile
# Stage 1: Build
FROM golang:1.21 AS builder
WORKDIR /app

# Copy source
COPY . .

# Build static binary
# CGO_ENABLED=0 disables C bindings
# GOOS=linux targets Linux
# -s strips debug info
# -w removes symbol table
RUN CGO_ENABLED=0 GOOS=linux go build \
    -ldflags="-s -w" \
    -o myapp

# Stage 2: Minimal runtime
FROM scratch

# Copy binary only
COPY --from=builder /app/myapp .

# Run as non-root (requires binary to support it)
USER 1000:1000

CMD ["./myapp"]
```

### Adding TLS Certificates

```dockerfile
FROM scratch
COPY --from=builder /app/myapp .
# Add CA certificates for HTTPS
COPY --from=builder /etc/ssl/certs/ca-certificates.crt /etc/ssl/certs/
USER 1000
CMD ["./myapp"]
```

### Rust Binary on Scratch

```dockerfile
# Build stage
FROM rust:1.75 AS builder
WORKDIR /app
COPY . .
# Build release binary with static linking
RUN rustup target add x86_64-unknown-linux-musl && \
    RUSTFLAGS='-C target-feature=+crt-static' \
    cargo build --release --target x86_64-unknown-linux-musl

# Runtime
FROM scratch
COPY --from=builder /app/target/x86_64-unknown-linux-musl/release/myapp .
USER 1000
CMD ["./myapp"]
```

### C/C++ Static Binary

```dockerfile
# Build stage
FROM gcc:13 AS builder
WORKDIR /app
COPY main.c .
# Compile static binary
RUN gcc -static -o myapp main.c

# Runtime
FROM scratch
COPY --from=builder /app/myapp .
CMD ["./myapp"]
```

## Checking Binary for Scratch

```bash
# Verify binary is statically linked
file myapp
# Output: myapp: ELF 64-bit LSB executable, statically linked

# Check for dynamic libraries
ldd myapp
# Output: not a dynamic executable

# Check it's Linux
readelf -l myapp | grep -i elf
# Shows it's an ELF executable
```

## Security Benefits

| Aspect | Regular Image | Scratch |
|--------|--------------|---------|
| Packages | Hundreds | Zero |
| CVEs | Many | None |
| Attack surface | Large | Minimal |
| Size | 50MB+ | 5-10MB |

## Gotchas for Docker Users

- **No shell**: Can't exec or debug normally
- **No libraries**: All dependencies must be static
- **No user tools**: No cat, ls, etc.
- **Binary requirements**: Must support Linux capabilities

## Common Mistakes

- **Dynamic linking**: Forgetting CGO_ENABLED=0
- **Missing certs**: HTTPS fails without CA certs
- **Wrong architecture**: Building for wrong platform

## Quick Reference

| Build Flag | Purpose |
|------------|---------|
| CGO_ENABLED=0 | No C bindings |
| GOOS=linux | Target Linux |
| -s | Strip debug |
| -w | Strip symbols |

## What's Next

Continue to [CPU Pinning](../runtime-tuning/01-cpu-pinning.md) for runtime tuning.
