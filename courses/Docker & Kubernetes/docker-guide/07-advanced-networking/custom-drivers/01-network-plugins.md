# Network Plugins

## Overview

Docker's network plugin system allows third-party solutions to provide advanced networking capabilities beyond built-in drivers. This guide covers the CNM plugin interface and how to use network plugins.

## Prerequisites

- Docker Engine 20.10+
- Understanding of Docker networking
- Root or sudo access

## Core Concepts

### Container Network Model (CNM)

CNM is Docker's network plugin architecture:

- **Network Sandbox**: Isolated network stack
- **Endpoint**: Connection point to a network
- **Network**: Collection of endpoints

### Plugin Types

- **Network plugins**: Create custom network drivers
- **IPAM plugins**: Custom IP address management
- **Volume plugins**: (covered elsewhere)

## Step-by-Step Examples

### Installing a Network Plugin

```bash
# List available plugins
docker plugin ls

# Install a network plugin
# Example: weave scope (for visualization)
docker plugin install store/weaveworks/net-plugin:1.8

# Enable plugin if disabled
docker plugin enable net-plugin

# Disable plugin
docker plugin disable net-plugin
```

### Using a Network Plugin

```bash
# Create network using plugin driver
# After installing a plugin, use --driver with its name
docker network create \
  --driver store/weaveworks/net-plugin:1.8 \
  --opt some-option=value \
  weave-net

# List networks to verify
docker network ls

# Example output:
# NETWORK ID   NAME         DRIVER                           SCOPE
# abc123       bridge       bridge                           local
# def456       weave-net    store/weaveworks/net-plugin:1.8  global
```

### Custom IPAM Configuration

```bash
# Some plugins provide custom IPAM
docker network create \
  --driver my-custom-driver \
  --ipam-driver my-ipam-driver \
  --ipam-opt subnet=172.20.0.0/16 \
  custom-net
```

### Removing Plugins

```bash
# Remove a plugin
# Must disable first if enabled
docker plugin disable net-plugin
docker plugin rm net-plugin
```

## Common Network Plugins

| Plugin | Purpose | Use Case |
|--------|---------|----------|
| Weave Net | Overlay networking | Multi-host with encryption |
| Flannel | Overlay networking | Simple multi-host |
| Calico | Network policy | Enterprise with policies |
| Cilium | eBPF networking | High performance |
| Romana | Network policy | L3 networking |

## Gotchas for Docker Users

- **Plugin compatibility**: Ensure plugin matches Docker version
- **Plugin state**: May need enabling after install
- **Cleanup**: Uninstall plugins before Docker upgrade sometimes needed

## Common Mistakes

- **Forgetting to enable**: Plugin installed but not enabled
- **Wrong driver name**: Must use exact plugin reference
- **Missing dependencies**: Some plugins need etcd or consul

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker plugin ls` | List plugins |
| `docker plugin install NAME` | Install plugin |
| `docker plugin rm NAME` | Remove plugin |
| `docker network create --driver PLUGIN` | Use plugin |

## What's Next

Continue to [Weave and Flannel](./02-weave-and-flannel.md) for specific plugin details.
