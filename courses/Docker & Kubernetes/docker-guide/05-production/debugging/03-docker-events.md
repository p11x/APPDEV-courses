# Docker Events

## Overview

Docker events provide a real-time stream of information about what's happening in your Docker environment. Events are useful for debugging, monitoring, and automation, allowing you to see every action taken by the Docker daemon.

## Prerequisites

- Basic Docker knowledge
- Understanding of container lifecycle

## Core Concepts

### Event Types

- **container events**: create, start, stop, destroy, die, etc.
- **image events**: pull, tag, untag
- **volume events**: create, mount, unmount
- **network events**: create, connect, disconnect

## Step-by-Step Examples

### Viewing Events

```bash
# Stream events in real-time
docker events

# Filter by container
docker events --filter 'container=mycontainer'

# Filter by event type
docker events --filter 'event=start'
docker events --filter 'event=die'

# Filter by image
docker events --filter 'image=nginx'

# Show timestamps
docker events --since 2024-01-01
```

### Practical Usage

```bash
# Monitor specific container
docker events --filter 'container=web' --filter 'event=start'

# See all container actions
docker events --filter 'type=container'

# Use in scripts
docker events --since '1h' --format '{{json .}}' | jq
```

## Common Mistakes

- **Too much output**: Use filters to narrow down events.
- **Not understanding event timing**: Events show when Docker took action, not always when application is ready.

## What's Next

Continue to [Building in CI](./../ci-cd/01-building-in-ci.md)
