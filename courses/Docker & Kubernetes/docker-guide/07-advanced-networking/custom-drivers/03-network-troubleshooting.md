# Network Troubleshooting

## Overview

Network issues in Docker can be challenging to diagnose. This guide covers common networking problems and the tools and techniques to troubleshoot them effectively.

## Prerequisites

- Docker Engine 20.10+
- Understanding of Docker networking
- Basic networking knowledge

## Core Concepts

### Troubleshooting Tools

- **docker network inspect**: Detailed network configuration
- **docker exec**: Run commands in containers
- **iptables**: Kernel-level firewall rules
- **ip**: Network interface management

## Step-by-Step Examples

### Inspecting Networks

```bash
# List all networks
docker network ls

# Inspect specific network
docker network inspect bridge

# Inspect overlay network
docker network inspect my-overlay

# Shows: Subnet, Gateway, Drivers, Connected containers
```

### Debugging Container Connectivity

```bash
# Check container network settings
docker inspect container_name --format '{{json .NetworkSettings.Networks}}'

# Test connectivity from within container
docker exec container ping -c 3 8.8.8.8

# Test DNS resolution
docker exec container nslookup google.com

# Check container's /etc/resolv.conf
docker exec container cat /etc/resolv.conf
```

### Checking Network Routes

```bash
# View container's routing table
docker exec container ip route

# Example output:
# default via 172.17.0.1 dev eth0
# 172.17.0.0/16 dev eth0
```

### Inspecting iptables Rules

```bash
# List Docker's iptables rules
# -L lists rules, -n shows numbers, -v shows stats
sudo iptables -L -n -v

# Check DOCKER-USER chain
sudo iptables -L DOCKER-USER -n

# Check NAT table for port mappings
sudo iptables -t nat -L -n -v
```

### Common Issues and Fixes

### Port Conflicts

```bash
# Find what's using a port
# Linux: lsof or netstat
sudo netstat -tulpn | grep 8080

# Fix: Use different host port
docker run -p 8081:80 nginx
```

### DNS Resolution Failure

```bash
# Check container can resolve names
docker exec container ping -c 3 google.com

# If failing, check DNS settings
docker run --dns 8.8.8.8 nginx

# Or specify DNS in daemon.json
echo '{"dns":["8.8.8.8"]}' | sudo tee /etc/docker/daemon.json
sudo systemctl restart docker
```

### MTU Issues

```bash
# Check MTU on host
ip link show eth0

# Check if container MTU causes issues
# Packets getting fragmented or dropped

# Set container MTU
docker run --network my-net mtu=1400 nginx
```

### Overlay Network Issues

```bash
# Check Swarm initialization
docker node ls

# Verify overlay network
docker network inspect my-overlay

# Check for ingress network issues
docker network inspect ingress
```

### Network Plugin Issues

```bash
# Check plugin status
docker plugin ls

# Enable disabled plugin
docker plugin enable my-plugin

# Check plugin logs
docker plugin inspect my-plugin
```

## Gotchas for Docker Users

- **iptables ordering**: Docker rules may conflict with custom rules
- **Network mode**: Host network bypasses Docker networking
- **DNS limitations**: Default bridge doesn't support DNS

## Common Mistakes

- **Forgetting -p flag**: Container ports not accessible
- **Wrong network**: Container on wrong network
- **Firewall blocking**: Local firewall blocking Docker ports

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker network ls` | List networks |
| `docker network inspect NAME` | Inspect network |
| `docker exec CONTAINER ping` | Test connectivity |
| `docker exec CONTAINER nslookup` | Test DNS |
| `iptables -L -n -v` | Check firewall rules |

## What's Next

Continue to [Embedded DNS](./../dns-and-service-discovery/01-embedded-dns.md) for service discovery.
