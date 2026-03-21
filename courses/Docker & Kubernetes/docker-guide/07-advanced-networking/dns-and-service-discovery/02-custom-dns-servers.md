# Custom DNS Servers

## Overview

Beyond Docker's embedded DNS, you can configure custom DNS servers for your containers. This is essential for corporate networks, split-horizon DNS, and integration with existing infrastructure.

## Prerequisites

- Docker Engine 20.10+
- Understanding of DNS concepts

## Core Concepts

### DNS Configuration Options

- **--dns**: Custom DNS server IP addresses
- **--dns-search**: Search domains for unqualified names
- **--dns-opt**: DNS options (like ndots)
- **Daemon-wide**: Configure in daemon.json

## Step-by-Step Examples

### Container-Level DNS

```bash
# Use specific DNS server
docker run --dns 10.0.0.1 nginx

# Multiple DNS servers (tried in order)
docker run --dns 10.0.0.1 --dns 10.0.0.2 nginx

# Google DNS as fallback
docker run --dns 8.8.8.8 --dns 8.8.4.4 nginx
```

### DNS Search Domains

```bash
# Add search domain for unqualified names
# "app" becomes "app.internal"
docker run --dns-search internal nginx

# Multiple search domains
docker run --dns-search internal --dns-search corp nginx
```

### DNS Options

```bash
# Set ndots (number of dots before search)
# Lower ndots = more external DNS queries
docker run --dns-opt ndots:2 nginx
```

### Daemon-Wide Configuration

```bash
# Configure DNS for all containers
# Edit /etc/docker/daemon.json
{
  "dns": ["10.0.0.1", "8.8.8.8"],
  "dns-search": ["internal", "corp.local"]
}

# Restart Docker to apply
sudo systemctl restart docker

# All new containers now use these DNS settings
```

### Split-Hizon DNS

```bash
# Corporate network with internal DNS
# Internal: 10.0.0.1 (resolves internal.example.com)
# External: 8.8.8.8 (resolves public domains)

docker run \
  --dns 10.0.0.1 \
  --dns 8.8.8.8 \
  --dns-search internal \
  nginx
```

### Testing DNS

```bash
# Test DNS resolution
docker run --dns 8.8.8.8 alpine nslookup google.com

# Test with dig if available
docker run --dns 8.8.8.8 alpine sh -c "apk add bind-tools && dig A google.com"

# Check which DNS is being used
docker exec container cat /etc/resolv.conf
```

## DNS Resolution Flow

```
Container queries "web"
    ↓
/etc/hosts (container hostname)
    ↓
127.0.0.11 (Docker embedded DNS)
    ↓
--dns servers (in order)
    ↓
Upstream DNS (Internet)
```

## Gotchas for Docker Users

- **Order matters**: DNS servers tried in order listed
- **Timeout**: Each DNS server has timeout
- **Local resolution**: Embedded DNS must be reachable first

## Common Mistakes

- **Wrong DNS IP**: Unreachable DNS causes timeouts
- **Missing fallback**: Should include public DNS
- **daemon.json restart**: Changes require Docker restart

## Quick Reference

| Flag | Description |
|------|-------------|
| `--dns IP` | DNS server |
| `--dns-search DOMAIN` | Search domain |
| `--dns-opt OPT` | DNS option |

| Common Options | Description |
|----------------|-------------|
| ndots | Dots before search |
| timeout | Query timeout |
| attempts | Retry attempts |

## What's Next

Continue to [Service Discovery Patterns](./03-service-discovery-patterns.md) for production patterns.
