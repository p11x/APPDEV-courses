# Embedded DNS

## Overview

Docker's embedded DNS server provides automatic service discovery for containers on custom networks. This guide explains how Docker's DNS works and how containers can find each other by name.

## Prerequisites

- Docker Engine 20.10+
- Understanding of Docker networking

## Core Concepts

### Embedded DNS Server

Docker runs a DNS server at 127.0.0.11:

- Only available on custom networks (not default bridge)
- Automatically resolves container names
- Supports container aliases
- Handles DNS queries for container names

### DNS Resolution Order

1. /etc/hosts (container's own hostname)
2. Embedded DNS server (other containers)
3. External DNS (from --dns flag)

## Step-by-Step Examples

### Container Name Resolution

```bash
# Create a custom network
docker network create my-net

# Run first container
docker run -dit \
  --network my-net \
  --name container1 \
  alpine:latest

# Run second container
docker run -dit \
  --network my-net \
  --name container2 \
  alpine:latest

# Test DNS resolution
# container2 can reach container1 by name
docker exec container2 ping -c 3 container1

# Test reverse DNS
docker exec container2 nslookup container1
```

### Container Aliases

```bash
# Create container with network alias
docker run -dit \
  --network my-net \
  --network-alias web \
  --name web1 \
  nginx

# Can also reach as 'web'
docker run -dit \
  --network my-net \
  --name client \
  alpine:latest

docker exec client ping -c web
```

### DNS Server Options

```bash
# Specify custom DNS server
docker run --dns 8.8.8.8 nginx

# Multiple DNS servers
docker run --dns 8.8.8.8 --dns 8.8.4.4 nginx

# DNS search domain
docker run --dns-search example.com nginx
```

### Checking DNS Configuration

```bash
# View container's DNS config
docker exec container cat /etc/resolv.conf

# Example output:
# nameserver 127.0.0.11
# options ndots:0

# After adding custom DNS:
# nameserver 8.8.8.8
# nameserver 8.8.4.4
# search example.com
```

## Default Bridge vs Custom Networks

| Feature | Default Bridge | Custom Network |
|---------|---------------|---------------|
| DNS by name | No | Yes |
| DNS by alias | No | Yes |
| Automatic discovery | No | Yes |
| Network isolation | Limited | Complete |

## Gotchas for Docker Users

- **Default bridge limitation**: Container names don't resolve on default bridge
- **127.0.0.11**: This magic IP is only reachable from within containers
- **External DNS**: Must use --dns flag to query external DNS

## Common Mistakes

- **Default bridge**: Expecting DNS to work on default bridge
- **Spelling**: Container names are case-sensitive
- **Network isolation**: Containers on different networks can't communicate

## Quick Reference

| Command | Description |
|---------|-------------|
| `--network NAME` | Connect to network |
| `--network-alias ALIAS` | Add DNS alias |
| `--dns IP` | Custom DNS server |
| `--dns-search DOMAIN` | Search domain |

## What's Next

Continue to [Custom DNS Servers](./02-custom-dns-servers.md) for advanced DNS configuration.
