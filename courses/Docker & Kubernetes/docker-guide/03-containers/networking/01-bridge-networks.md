# Bridge Networks

## Overview

The bridge network is Docker's default networking mode. When you run a container without specifying a network, it connects to the bridge network. Understanding how bridge networking works is essential for container communication, especially for local development and single-host deployments. The bridge creates a virtual network that connects containers to each other and to the host system.

## Prerequisites

- Understanding of Docker basics
- Familiarity with IP addressing concepts
- Basic networking knowledge

## Core Concepts

### What is Bridge Networking?

Bridge networking creates a software bridge on the Docker host. Containers connected to the bridge network:

- Get their own IP address (typically 172.17.0.0/16)
- Can communicate with each other
- Can communicate with the host
- Can't be reached from outside without port mapping

### Default Bridge (docker0)

When Docker is installed, it creates a default bridge called "bridge":

```bash
# View bridge network
docker network ls

# Inspect bridge network
docker network inspect bridge
```

### How It Works

1. Docker creates a bridge interface (docker0) on the host
2. Each container gets a virtual Ethernet interface (veth) connected to the bridge
3. NAT (Network Address Translation) allows outbound connections
4. Port mapping allows inbound connections

### Container Communication

Containers on the same bridge network can communicate by name or IP:

```bash
# Start two containers on bridge network
docker run -d --name container1 nginx
docker run -d --name container2 alpine

# They can ping each other
docker exec container2 ping -c 1 container1
```

## Step-by-Step Examples

### Default Bridge Behavior

```bash
# Run a container (uses default bridge)
docker run -d --name web nginx

# Check container's IP address
docker inspect --format='{{.NetworkSettings.IPAddress}}' web

# List bridge network details
docker network inspect bridge
# Shows containers, IP addresses, gateway

# Test connectivity from host
curl http://$(docker inspect --format='{{.NetworkSettings.IPAddress}}' web)
```

### Custom Bridge Networks

```bash
# Create a custom bridge network
# --driver bridge specifies bridge driver
docker network create --driver bridge my-network

# Run containers on custom network
docker run -d --name container1 --network my-network nginx
docker run -d --name container2 --network my-network alpine

# Containers can communicate by name
docker exec container2 ping -c 1 container1

# Containers get custom subnet
docker network inspect my-network
```

### Port Mapping

```bash
# Map container port to host port
# -p hostPort:containerPort maps traffic to container
docker run -d --name web -p 8080:80 nginx
# Access at http://localhost:8080

# Map to specific IP
# Only listen on specific host interface
docker run -d --name web2 -p 127.0.0.1:8081:80 nginx

# Map to random port
# Let Docker choose an available port
docker run -d -P nginx

# Check port mappings
docker port web
# Output: 80/tcp -> 0.0.0.0:8080
```

### Advanced Bridge Options

```bash
# Create bridge with custom options
docker network create \
  --driver bridge \
  --subnet=192.168.100.0/24 \
  --gateway=192.168.100.1 \
  my-bridge-network

# Inspect custom network
docker network inspect my-bridge-network
```

### Container Network Isolation

```bash
# Create isolated networks for different projects
docker network create frontend-network
docker network create backend-network

# Containers on different networks can't communicate
docker run -d --name app1 --network frontend-network nginx
docker run -d --name app2 --network backend-network nginx
# These cannot ping each other

# Connect container to multiple networks
docker network connect backend-network app1
```

## Common Mistakes

- **Not understanding default bridge limitations**: Containers can't resolve each other by name on default bridge.
- **Confusing bridge with host networking**: Bridge uses NAT; host shares host's network namespace.
- **Port conflicts**: Two containers can't use the same host port.
- **Exposing ports unintentionally**: Remember -p publishes ports to the host.
- **Forgetting network cleanup**: Unused networks consume resources.

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker network ls` | List networks |
| `docker network create` | Create network |
| `docker network inspect` | View details |
| `docker network rm` | Remove network |
| `--network` | Attach container to network |
| `-p host:container` | Port mapping |

| Feature | Default Bridge | Custom Bridge |
|---------|---------------|---------------|
| DNS resolution | By IP only | By container name |
| Network isolation | All containers together | Can isolate |
| Subnet configuration | Automatic | Customizable |

## What's Next

Now that you understand bridge networks, continue to [Host and Overlay Networks](./02-host-and-overlay.md) to learn about alternative network drivers.
