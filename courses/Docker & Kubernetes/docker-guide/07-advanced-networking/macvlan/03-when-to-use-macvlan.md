# When to Use MACVLAN

## Overview

Choosing the right Docker network driver is crucial for application performance and functionality. This guide provides a decision framework for selecting between bridge, overlay, MACVLAN, and IPvLAN networks.

## Prerequisites

- Understanding of Docker networking basics
- Knowledge of your network infrastructure

## Core Concepts

### Network Driver Overview

| Driver | Layer | Use Case | Multi-Host | NAT |
|--------|-------|----------|------------|-----|
| bridge | L2 | Default | No | Yes |
| overlay | L2/L3 | Swarm | Yes | Yes |
| macvlan | L2 | LAN access | Yes | No |
| ipvlan | L2/L3 | MAC restrictions | Yes | No |
| host | L1 | Max performance | No | No |

### Decision Factors

Consider these when choosing:

- **Network requirements**: Does it need LAN presence?
- **Multi-host**: Will it span multiple machines?
- **Performance**: Is NAT overhead a concern?
- **Network restrictions**: Are there MAC limitations?

## When to Use Bridge

### Default Choice

Use bridge networks when:

- Single host deployments
- Simple applications
- Development environments
- No special networking needs

```bash
# Default bridge network
docker network create my-bridge

# Use default bridge
docker run --network my-bridge nginx
```

### Limitations

- Not suitable for multi-host
- NAT adds latency
- Limited isolation options

## When to Use Overlay

### Swarm and Multi-Host

Use overlay networks when:

- Docker Swarm cluster
- Multi-host container communication
- Need built-in service discovery

```bash
# Overlay requires Swarm
docker swarm init

# Create overlay network
docker network create \
  --driver overlay \
  --attachable \
  my-overlay
```

## When to Use MACVLAN

### Direct LAN Access

Use MACVLAN when:

- Containers need to appear as physical devices
- Hardware integration required
- Zero-NAT performance needed
- Legacy network integration

### Requirements

- Network supporting promiscuous mode
- Separate IP subnet or alias
- Control over network infrastructure

## When to Use IPvLAN

### MAC Restrictions

Use IPvLAN when:

- Network blocks unknown MACs
- Need scalability of L3
- MAC address limits reached

## Decision Flowchart

```
Need multi-host?
├─ No → Need LAN presence?
│      ├─ No → Use bridge
│      └─ Yes → MAC restrictions?
│              ├─ No → Use MACVLAN
│              └─ Yes → Use IPvLAN
└─ Yes → Using Swarm?
         ├─ No → Need LAN presence?
         │        ├─ No → Use overlay
         │        └─ Yes → Use MACVLAN/IPVLAN
         └─ Yes → Service mesh?
                  ├─ Yes → Use overlay with encryption
                  └─ No → Use overlay
```

## Performance Comparison

| Driver | Latency | Throughput | Complexity |
|--------|---------|------------|------------|
| bridge | Medium | High | Low |
| overlay | Medium-High | Medium | Medium |
| macvlan | Low | Highest | Medium |
| ipvlan | Low | Highest | Medium |

## Gotchas for Docker Users

- **Promiscuous mode**: MACVLAN requires network support
- **Subnet planning**: MACVLAN/IPVLAN need IP subnet
- **Host access**: Special setup needed for host-to-container

## Common Mistakes

- **Overengineering**: Using MACVLAN when bridge is fine
- **Wrong subnet**: Subnet conflicts cause network issues
- **Ignoring constraints**: MACVLAN not available in all environments

## Quick Reference

| Scenario | Recommended Driver |
|----------|---------------------|
| Single host | bridge |
| Swarm multi-host | overlay |
| LAN presence | macvlan |
| MAC restrictions | ipvlan |
| Max performance | host |

## What's Next

Continue to [Network Plugins](./../custom-drivers/01-network-plugins.md) for third-party networking solutions.
