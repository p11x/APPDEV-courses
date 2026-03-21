# Container DNS

## Overview

Docker provides built-in DNS for containers, allowing them to discover each other by name rather than IP address. Understanding how Docker's DNS works is essential for configuring container communication, especially in development and microservices architectures. Docker's embedded DNS server handles name resolution for containers on user-defined networks.

## Prerequisites

- Understanding of Docker networking basics
- Basic DNS knowledge
- Familiarity with bridge and custom networks

## Core Concepts

### How Docker DNS Works

Docker runs an embedded DNS server (127.0.0.11) in each container:

1. Containers on user-defined networks register with Docker's DNS
2. DNS queries for container names are resolved by embedded DNS
3. External DNS queries are forwarded to host's DNS

### DNS on Different Networks

**Default Bridge**: Only IP-based communication; no automatic DNS resolution.

**Custom Bridge/Overlay**: Full DNS support - containers can resolve each other by name.

### Embedded DNS

- Resolves container names on user-defined networks
- Forwards external queries to host DNS
- Handles A, AAAA, CNAME, and PTR records

## Step-by-Step Examples

### DNS on Custom Bridge

```bash
# Create a custom bridge network
docker network create my-network

# Start containers on the network
docker run -d --name web --network my-network nginx
docker run -d --name api --network my-network alpine

# Test DNS resolution
docker exec api ping -c 1 web
# Resolves to web's IP address

# Get web's IP
docker inspect --format='{{.NetworkSettings.Networks.my-network.IPAddress}}' web

# Test from web to api
docker exec web ping -c 1 api
```

### DNS Configuration

```bash
# Set custom DNS servers
docker run --dns=8.8.8.8 alpine nslookup google.com

# Set multiple DNS servers
docker run --dns=8.8.8.8 --dns=8.8.4.4 alpine

# Add search domain
docker run --dns=8.8.8.8 --search=example.com alpine

# View container's DNS configuration
docker exec container_name cat /etc/resolv.conf
```

### Container Aliases

```bash
# Add DNS alias to container
docker network connect --alias db my-network database-container

# Now 'db' resolves to this container
# Even with different container name
```

### Testing DNS

```bash
# Test DNS resolution
docker exec container_name nslookup other-container

# Use dig or host if available
docker exec container_name apt-get update && apt-get install -y dnsutils
docker exec container_name dig other-container

# Check embedded DNS
docker exec container_name cat /etc/hosts
# Shows: 127.0.0.11 (Docker's embedded DNS)
```

### External DNS

```bash
# Container can resolve external domains
docker run alpine ping -c 1 google.com

# DNS queries go through Docker's embedded DNS to host DNS
# No special configuration needed
```

## Common Mistakes

- **Expecting DNS on default bridge**: Can't resolve by name on default bridge.
- **Hardcoding IPs**: Use names instead of IPs for flexibility.
- **Forgetting network**: Containers must be on same network for DNS resolution.
- **DNS not updating**: Docker caches DNS; may need container restart.
- **Firewall blocking DNS**: DNS uses port 53; ensure it's allowed.

## Quick Reference

| Feature | Default Bridge | Custom Network |
|---------|--------------|----------------|
| Name resolution | No | Yes |
| By IP only | Yes | Yes |
| Automatic | No | Yes |

| Flag | Description |
|------|-------------|
| --dns | Custom DNS server |
| --search | Search domain |
| --network-alias | DNS alias |

## What's Next

Now that you understand container networking and DNS, continue to [Volumes](./../storage/01-volumes.md) to learn about persistent storage in containers.
