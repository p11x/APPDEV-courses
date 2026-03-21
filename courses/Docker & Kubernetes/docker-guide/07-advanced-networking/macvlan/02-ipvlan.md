# IPvlan

## Overview

IPvLAN is similar to MACVLAN but operates at Layer 3 instead of Layer 2. It allows containers to have unique IP addresses while sharing the host's MAC address, making it ideal for environments with strict MAC address filtering.

## Prerequisites

- Docker Engine 20.10+
- Understanding of basic networking
- Knowledge of MACVLAN concepts

## Core Concepts

### How IPvLAN Differs from MACVLAN

- **Shared MAC**: All containers share the host's MAC address
- **Unique IPs**: Each container gets its own IP on the network
- **Layer 3**: Routing happens at Layer 3 (IP), not Layer 2 (Ethernet)
- **MAC filtering**: Works in environments that block unknown MACs

### IPvLAN Modes

- **L2 mode**: Like MACVLAN, operates at Ethernet level
- **L3 mode**: Router-based, more scalable, better for large deployments

## Step-by-Step Examples

### Creating IPvLAN Network

```bash
# Create IPvLAN in L2 mode
# Similar to MACVLAN but shares host MAC
docker network create \
  --driver ipvlan \
  --subnet 192.168.1.0/24 \
  --gateway 192.168.1.1 \
  --opt parent=eth0 \
  --opt ipvlan_mode=l2 \
  ipvlan-net

# Create IPvLAN in L3 mode
# Better for routing-based networks
docker network create \
  --driver ipvlan \
  --subnet 192.168.1.0/24 \
  --gateway 192.168.1.1 \
  --opt parent=eth0 \
  --opt ipvlan_mode=l3 \
  ipvlan-l3
```

### Running Containers with IPvLAN

```bash
# Run container on IPvLAN
docker run -dit \
  --network ipvlan-net \
  --name ipvlan-container \
  alpine:latest

# Check IP address
docker inspect ipvlan-container --format '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}'
```

### IPvLAN L3 Mode

```bash
# L3 mode requires specific subnet configuration
# Host acts as router for container traffic

# Create network with L3 mode
docker network create \
  --driver ipvlan \
  --subnet 172.16.10.0/24 \
  --gateway 172.16.10.1 \
  --opt parent=eth0 \
  --opt ipvlan_mode=l3 \
  ipvlan-l3

# Run container
docker run -dit \
  --network ipvlan-l3 \
  --ip 172.16.10.10 \
  --name l3-container \
  nginx:1.25-alpine
```

## MACVLAN vs IPvLAN Comparison

| Feature | MACVLAN | IPvLAN |
|---------|---------|--------|
| MAC Address | Unique per container | Host's MAC |
| Layer | L2 | L2 or L3 |
| MAC Filtering | No | Works |
| Scalability | Limited | Better |
| Host Communication | Difficult | Easier in L3 |

## Use Cases for IPvLAN

### MAC Address Restrictions

Use IPvLAN when:

- Network equipment blocks unknown MACs
- MAC address limit reached
- Security policies restrict MAC addresses

### Scalability

IPvLAN L3 mode is better for:

- Large numbers of containers
- Environments requiring routing
- Multi-tenant setups

## Gotchas for Docker Users

- **L3 mode routing**: Container subnets need routing on physical network
- **MAC spoofing**: Some networks detect container MAC as spoofed
- **Mode matters**: L2 vs L3 behave very differently

## Common Mistakes

- **Wrong mode**: Using L2 when L3 is needed
- **Subnet routing**: Forgetting to route container subnet
- **Gateway**: IPvLAN needs correct gateway for external access

## Quick Reference

| Command | Description |
|---------|-------------|
| `--opt ipvlan_mode=l2` | L2 mode |
| `--opt ipvlan_mode=l3` | L3 mode |
| `--opt parent=eth0` | Parent interface |

| Mode | Description |
|------|-------------|
| l2 | Layer 2 bridging |
| l3 | Layer 3 routing |

## What's Next

Continue to [When to Use MACVLAN](./03-when-to-use-macvlan.md) for decision guidance.
