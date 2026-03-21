# Nodes and Roles

## Overview

Understanding node roles and management is crucial for operating a Docker Swarm effectively. This guide covers manager and worker responsibilities, node promotion, demotion, and maintenance procedures.

## Prerequisites

- Active Docker Swarm cluster
- Manager node access

## Core Concepts

### Manager Node Responsibilities

Manager nodes handle critical cluster functions:

- **Cluster state**: Maintaining the Raft consensus database
- **Scheduling**: Deciding where to place containers
- **Orchestration**: Handling service creation, scaling, and updates
- **Health monitoring**: Detecting and responding to node failures

### Worker Node Responsibilities

Worker nodes execute container workloads:

- **Container runtime**: Running containers assigned by managers
- **Task execution**: Executing services as directed by managers
- **Reporting**: Reporting container status back to managers

## Step-by-Step Examples

### Viewing Node Details

```bash
# List all nodes in the swarm
# Shows: ID, Hostname, Status, Availability, Manager Status
docker node ls

# Example output:
# ID                            HOSTNAME   STATUS    AVAILABILITY   MANAGER STATUS
# abc123 *                     manager1   Ready     Active         Leader
# def456                        worker1    Ready     Active         
# ghi789                        worker2    Ready     Active         

# Inspect a specific node
# Shows full node details in JSON format
docker node inspect worker1

# Pretty-printed output
docker node inspect worker1 --pretty

# Example partial output:
# ID:                     def456
# Hostname:               worker1
# Status:
#   State:                Ready
#   Availability:         Active
# Manager Status:
#   Reachability:         Reachable
#   Leader:               False
```

### Promoting a Node to Manager

```bash
# Promote worker to manager
# This gives the node voting rights in Raft consensus
docker node promote worker1

# Output:
# Node worker1 promoted to a manager.

# Verify the change
docker node ls

# Example output after promotion:
# ID                            HOSTNAME   STATUS    AVAILABILITY   MANAGER STATUS
# abc123 *                     manager1   Ready     Active         Leader
# def456                        worker1   Ready     Active         Reachable
```

### Demoting a Manager to Worker

```bash
# Demote manager to worker
# Never demote the last manager!
docker node demote manager2

# Output:
# Node manager2 demoted to a worker.

# Verify
docker node ls

# CAUTION: If you demote the last manager incorrectly
# You may need to reinitialize the swarm
```

### Draining a Node for Maintenance

```bash
# Drain a node (mark as unavailable for new tasks)
# Running containers are gracefully moved to other nodes
docker node update --availability drain worker1

# Output:
# worker1

# Verify availability changed
docker node ls

# Example output:
# ID                            HOSTNAME   STATUS    AVAILABILITY   MANAGER STATUS
# def456                        worker1   Ready     Draining       

# Bring node back online
docker node update --availability active worker1

# Output:
# worker1
```

### Adding Node Labels

```bash
# Add labels to nodes for placement constraints
# Useful for pinning specific workloads
docker node update --label-add storage=ssd worker1

# Add multiple labels
docker node update \
  --label-add environment=production \
  --label-add tier=database \
  worker1

# View labels
docker node inspect worker1 --pretty | grep -A 10 Labels

# Example output:
# Labels:
#  environment=production
#  storage=ssd
#  tier=database
```

### Viewing Node Resource Usage

```bash
# Check node resources
docker node inspect worker1 --pretty

# Shows:
# Containers: 5
# CPUs: 4
# Memory: 8GiB
```

## Node Availability States

| State | Description |
|-------|-------------|
| Active | Accepts new tasks |
| Pause | Pauses running tasks, accepts no new tasks |
| Draining | Moves tasks to other nodes |

## Gotchas for Docker Users

- **Manager count**: Keep 3 or 5 managers for HA
- **Drain before maintenance**: Always drain before node maintenance
- **Demotion safety**: Never demote the last manager
- **Labels vs constraints**: Labels on nodes, constraints in service create

## Common Mistakes

- **Single manager**: Creating a single manager creates a single point of failure
- **Forgetting to drain**: Shutting down a node without draining causes service interruptions
- **Too many managers**: More than 5 managers impacts performance
- **Label typos**: Incorrect label names cause service deployment failures

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker node ls` | List all nodes |
| `docker node inspect NAME` | Show node details |
| `docker node promote NAME` | Promote to manager |
| `docker node demote NAME` | Demote to worker |
| `docker node update --availability drain NAME` | Drain node |
| `docker node update --availability active NAME` | Reactivate node |
| `docker node update --label-add KEY=VALUE NAME` | Add label |

## What's Next

Continue to [Creating Swarm Services](../services/01-creating-swarm-services.md) to learn about deploying services.
