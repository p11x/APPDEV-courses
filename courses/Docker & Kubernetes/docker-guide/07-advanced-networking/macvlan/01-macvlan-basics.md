# MACVLAN Basics

## Overview

MACVLAN networking allows containers to appear as physical devices on your network with their own MAC addresses. This advanced networking mode is useful when containers need direct Layer 2 access to the physical network.

## Prerequisites

- Docker Engine 20.10+
- Network interface with promiscuous mode allowed
- Understanding of Docker networking basics

## Core Concepts

### How MACVLAN Works

MACVLAN creates a virtual network interface inside containers:

- Container gets a real MAC address from the host's network
- Appears as a physical device on your LAN
- Can have its own IP address from your LAN subnet
- Bypasses Docker's NAT for direct network access

### Use Cases

- Containers that must appear as physical devices
- Legacy applications expecting physical network
- Hardware passthrough requirements
- Zero-NAT performance optimization

## Step-by-Step Examples

### Creating a MACVLAN Network

```bash
# Create MACVLAN network
# --driver macvlan specifies the driver
# --subnet is your physical network subnet
# --gateway is your network gateway
# --opt parent=eth0 attaches to host interface
docker network create \
  --driver macvlan \
  --subnet 192.168.1.0/24 \
  --gateway 192.168.1.1 \
  --opt parent=eth0 \
  macvlan-net

# Verify network created
docker network ls

# Example output:
# NETWORK ID   NAME         DRIVER      SCOPE
# abc123       macvlan-net  macvlan     local
```

### Running a Container with MACVLAN

```bash
# Run container on MACVLAN network
# Container gets IP from subnet
docker run -dit \
  --network macvlan-net \
  --name macvlan-container \
  alpine:latest

# Check container has MACVLAN IP
docker inspect macvlan-container --format '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}'

# Example output: 192.168.1.50
```

### MACVLAN with Specific IP

```bash
# Assign specific IP to container
# Use --ip flag to specify exact IP
docker run -dit \
  --network macvlan-net \
  --ip 192.168.1.100 \
  --name fixed-ip-container \
  nginx:1.25-alpine
```

### Connecting to Existing Networks

```bash
# Create MACVLAN alongside existing bridge
# Useful for host communication
docker network create \
  --driver macvlan \
  --subnet 192.168.1.0/24 \
  --gateway 192.168.1.1 \
  --opt parent=eth0 \
  --opt macvlan_mode=bridge \
  macvlan-net

# Bridge mode (default): containers can communicate with each other
# 802.1q trunk mode: for VLAN tagging
docker network create \
  --driver macvlan \
  --subnet 10.0.1.0/24 \
  --gateway 10.0.1.1 \
  --opt parent=eth0.10 \
  macvlan-vlan10
```

## Limitations

### Network Requirements

- **Promiscuous mode**: Host interface must allow promiscuous mode
- **MAC address limits**: Network equipment may limit MAC addresses
- **No host communication**: By default, can't reach host directly

### Addressing Host Communication

```bash
# Create a secondary interface on host
# Allows host to reach containers
ip link add macvlan0 link eth0 type macvlan mode bridge
ip addr add 192.168.1.254/32 dev macvlan0
ip link set macvlan0 up

# Now host can reach containers at 192.168.1.x
```

## Comparison with Other Drivers

| Driver | Use Case | NAT | MAC Address |
|--------|----------|-----|-------------|
| bridge | Default | Yes | Docker-assigned |
| overlay | Multi-host | Yes | Docker-assigned |
| host | Max performance | No | Host's |
| macvlan | LAN presence | No | Real |

## Gotchas for Docker Users

- **Promiscuous mode required**: Network switch must allow this
- **No default route**: Container can't reach host by default
- **Subnet choice**: Use unused subnet or alias interface

## Common Mistakes

- **Subnet conflict**: Using existing subnet causes IP conflicts
- **Gateway wrong**: Wrong gateway makes containers unreachable
- **Promiscuous blocked**: Most cloud networks don't support this

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker network create --driver macvlan` | Create MACVLAN |
| `--opt parent=eth0` | Parent interface |
| `--subnet` | Network subnet |
| `--gateway` | Gateway IP |
| `--ip` | Specific container IP |

| Mode | Description |
|------|-------------|
| bridge | Default, containers on same VLAN |
| 802.1q | VLAN tagging via parent.10 |
| passthrough | Pass through host MAC |

## What's Next

Continue to [IPvLAN](./02-ipvlan.md) for another Layer 2 networking option.
