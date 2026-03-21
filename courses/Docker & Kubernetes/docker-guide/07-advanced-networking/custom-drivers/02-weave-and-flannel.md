# Weave and Flannel

## Overview

Weave Net and Flannel are popular third-party networking solutions for Docker. Both provide overlay networking for multi-host container communication. This guide compares and shows how to use each.

## Prerequisites

- Multi-host Docker setup
- Understanding of overlay networking
- Root access for installation

## Core Concepts

### Weave Net

- Creates a mesh network between hosts
- Provides encryption
- Simple to set up
- Includes DNS for service discovery

### Flannel

- Designed for Kubernetes but works with Docker
- VXLAN backend by default
- Lightweight and simple
- Stores network config in etcd

## Weave Net Setup

### Installation

```bash
# Install Weave Net as a Docker plugin
# Downloads and installs the plugin
docker plugin install \
  weaveworks/net-plugin:1.8 \
  --grant-all-permissions

# Verify installation
docker plugin ls
```

### Creating Weave Network

```bash
# Create overlay network using Weave
docker network create \
  --driver weaveworks/net-plugin:1.8 \
  weave-net

# Run containers on Weave network
docker run -dit \
  --network weave-net \
  --name container1 \
  alpine:latest

docker run -dit \
  --network weave-net \
  --name container2 \
  alpine:latest

# Containers can now communicate across hosts
docker exec container1 ping -c container2
```

### Weave with Encryption

```bash
# Weave encrypts traffic by default
# Each host runs a Weave router
# Traffic between hosts is encrypted
```

## Flannel Setup

### Prerequisites

```bash
# Flannel needs etcd for storing network config
# Start etcd container
docker run -dit \
  --name etcd \
  --network host \
  --volume etcd-data:/etcd \
  quay.io/coreos/etcd:latest \
  etcd \
  --name etcd0 \
  --advertise-client-urls http://127.0.0.1:2379 \
  --listen-client-urls http://0.0.0.0:2379 \
  --initial-cluster etcd0=http://127.0.0.1:2380
```

### Installing Flannel

```bash
# Install Flannel plugin
docker plugin install \
  rancher/flannelcni-flannel:v0.19.0 \
  --grant-all-permissions

# Create Flannel network
docker network create \
  --driver rancher/flannelcni-flannel:v0.19.0 \
  --subnet 10.42.0.0/16 \
  flannel-net
```

## Comparison

| Feature | Weave | Flannel |
|---------|-------|---------|
| Setup complexity | Simple | Medium |
| Encryption | Built-in | Optional |
| Service discovery | Yes | No |
| Storage backend | None | etcd |
| Kubernetes support | Yes | Primary |

## Gotchas for Docker Users

- **Plugin versions**: Check compatibility with Docker version
- **etcd requirement**: Flannel needs etcd
- **Network ranges**: Use non-conflicting subnets

## Common Mistakes

- **Port conflicts**: Both use specific ports
- **etcd issues**: Flannel fails without working etcd
- **Subnet conflicts**: Must not overlap with existing networks

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker plugin install weaveworks/net-plugin` | Install Weave |
| `docker network create --driver weaveworks/net-plugin` | Create Weave network |
| `docker plugin install rancher/flannelcni-flannel` | Install Flannel |
| `docker network create --driver rancher/flannel` | Create Flannel network |

## What's Next

Continue to [Network Troubleshooting](./03-network-troubleshooting.md) for debugging tips.
