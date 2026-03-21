# Calico with Docker

## Overview

Calico provides networking and network policy for containers at scale. This guide covers using Calico with standalone Docker (not Kubernetes).

## Prerequisites

- Docker Engine 20.10+
- Multi-host Docker setup
- etcd cluster (required)

## Core Concepts

### Calico Features

- Layer 3 networking with BGP
- Network policy enforcement
- Scalable to thousands of nodes
- No overlay required

## Step-by-Step Examples

### Prerequisites - etcd

```bash
# Start etcd for Calico backend
docker run -d \
  --name etcd \
  -p 2379:2379 \
  -p 2380:2380 \
  quay.io/coreos/etcd:latest \
  etcd \
    --name etcd0 \
    --advertise-client-urls=http://127.0.0.1:2379 \
    --listen-client-urls=http://0.0.0.0:2379
```

### Creating Calico Network

```bash
# Note: Calico Docker plugin setup requires Kubernetes
# For standalone Docker, consider Weave or Flannel instead

# Using Calico in Kubernetes mode is recommended
# See Kubernetes guide for Calico
```

## Gotchas for Docker Users

- **Kubernetes preferred**: Calico is designed for Kubernetes
- **Complex setup**: Requires etcd, Felix, Bird

## Quick Reference

| Component | Purpose |
|-----------|---------|
| Felix | Agent on each host |
| etcd | Data store |
| Bird | BGP route reflector |

## What's Next

Continue to [Cilium Basics](./02-cilium-basics.md) for eBPF-based networking.
