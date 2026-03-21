# Profiles

## Overview

Compose profiles allow you to define optional services that can be enabled selectively. This enables a single compose file to support multiple use cases, from basic development to full production deployments, without maintaining separate files.

## Prerequisites

- Understanding of Docker Compose basics
- Familiarity with compose commands

## Core Concepts

### What are Profiles?

Profiles are named groups of services:

- Services can belong to multiple profiles
- Default profile is always active (no profile specified)
- Activate profiles with --profile flag

## Step-by-Step Examples

```yaml
version: "3.8"

services:
  web:
    image: nginx
  
  # Development tools
  debug:
    image: adminer
    profiles:
      - debug
  
  # Production monitoring
  prometheus:
    image: prometheus
    profiles:
      - monitoring
    depends_on:
      - web
```

Run with profiles:

```bash
# Run only default services
docker compose up -d

# Enable debug profile
docker compose --profile debug up -d

# Enable multiple profiles
docker compose --profile debug --profile monitoring up -d
```

## Common Mistakes

- **Forgetting to specify profile**: Service won't start without --profile.
- **Overlapping profiles**: Can cause conflicts.

## What's Next

Continue to [Secrets and Configs](./02-secrets-and-configs.md)
