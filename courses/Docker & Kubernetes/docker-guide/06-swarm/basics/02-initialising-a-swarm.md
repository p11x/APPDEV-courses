# Initialising a Swarm

## Overview

Initialising a Docker Swarm transforms your Docker host into a manager node that can orchestrate containers across multiple hosts. This guide covers the complete setup process, from creating an initial manager to joining additional nodes.

## Prerequisites

- Docker Engine 20.10+ installed on all nodes
- Network connectivity between all hosts
- Root or sudo access on all machines

## Core Concepts

### What Happens During Init

When you run `docker swarm init`, Docker:

1. Creates a Swarm cluster with the current node as the first manager
2. Generates a manager join token and a worker join token
3. Establishes the Raft consensus database for state
4. Configures the overlay networking for inter-container communication

### Manager vs Worker Nodes

- **Manager nodes**: Handle cluster management, scheduling, and Raft consensus
- **Worker nodes**: Run container workloads
- You need an odd number of managers for quorum (1, 3, 5, etc.)

## Step-by-Step Examples

### Step 1: Initialise the Swarm

```bash
# Initialize Docker Swarm on the first manager node
# --advertise-addr specifies the IP other nodes will use to join
# This should be a routable IP on your network
docker swarm init \
  --advertise-addr 192.168.1.10

# Example output:
# Swarm initialized: current node (abc123def456) is now a manager.
# 
# To add a worker to this swarm, run the following command:
#     docker swarm join --token SWMTKN-1-xxxxx 192.168.1.10:2377
# 
# To add a manager to this swarm, run:
#     docker swarm join --token SWMTKN-1-yyyyy 192.168.1.10:2377
```

### Step 2: View Swarm Status

```bash
# Check current node role
# Shows: NODE ID, HOSTNAME, AVAILABILITY, MANAGER STATUS
docker node ls

# Example output:
# ID                            HOSTNAME   STATUS    AVAILABILITY   MANAGER STATUS
# abc123def456 *               manager1   Ready     Active         Leader
```

### Step 3: Generate Join Tokens

```bash
# Get worker join token (for adding worker nodes)
# Use this to add more worker nodes to the swarm
docker swarm join-token -q worker

# Get manager join token (for adding manager nodes)
# Use this to add more manager nodes for HA
docker swarm join-token -q manager

# Full command with token visible (for copying)
docker swarm join-token worker

# Example output:
# To add a worker to this swarm, run:
#     docker swarm join --token SWMTKN-1-2j3k4l5m6n7o8p9 192.168.1.10:2377
```

### Step 4: Join Worker Nodes

```bash
# On each worker node, run:
# This joins the node as a worker in the swarm
docker swarm join \
  --token SWMTKN-1-2j3k4l5m6n7o8p9 \
  192.168.1.10:2377

# Output on worker:
# This node joined a swarm as a worker.
```

### Step 5: Join Additional Managers

```bash
# On each additional manager node, run:
# Managers participate in Raft consensus
docker swarm join \
  --token SWMTKN-1-yyyyy \
  192.168.1.10:2377

# Note: For HA, use 3 or 5 managers
# Never use 2 managers (no majority if one fails)
```

### Step 6: Verify Cluster Health

```bash
# View all nodes in the swarm
# Check that all nodes show Ready status
docker node ls

# Example with 3 nodes:
# ID                            HOSTNAME   STATUS    AVAILABILITY   MANAGER STATUS
# abc123def456 *               manager1   Ready     Active         Leader
# def456ghi789                  worker1    Ready     Active         
# ghi789jkl012                  manager2   Ready     Active         Reachable

# Inspect a specific node
# Shows detailed node information
docker node inspect self

# Or by hostname:
docker node inspect manager2 --pretty
```

## Raft Consensus Basics

### Why Odd Numbers

| Manager Count | Failure Tolerance |
|--------------|------------------|
| 1 | 0 failures |
| 3 | 1 failure |
| 5 | 2 failures |
| 7 | 3 failures |

### Quorum

- A majority of managers must agree on decisions
- With 3 managers, you can lose 1 and still function
- With 5 managers, you can lose 2 and still function

## Common Mistakes

- **Using 2 managers**: If one fails, you lose quorum and cluster is unavailable
- **Wrong advertise address**: Nodes can't join if they can't reach the advertise IP
- **Not using --listen-addr**: Default port 2377 might conflict
- **Firewall blocking**: Ensure ports 2377 and 2378 are open

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker swarm init` | Initialize swarm on this node |
| `docker swarm init --advertise-addr IP` | Initialize with specific IP |
| `docker node ls` | List all nodes |
| `docker swarm join-token -q worker` | Get worker join token |
| `docker swarm join --token TOKEN IP:PORT` | Join swarm as worker/manager |
| `docker swarm leave` | Leave the swarm |

| Port | Purpose |
|------|---------|
| 2377 | Cluster management |
| 2376 | Docker daemon TLS |
| 2375 | Unencrypted Docker daemon |

## What's Next

Continue to [Nodes and Roles](./03-nodes-and-roles.md) to learn about managing node states and roles.
