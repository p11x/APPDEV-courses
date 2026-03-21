# MongoDB Replica Set

## Overview

MongoDB replica sets provide high availability and data redundancy. This guide covers deploying MongoDB replica sets in Docker.

## Prerequisites

- Docker basics
- MongoDB concepts
- Understanding of replica sets

## Step-by-Step Examples

### Replica Set Setup

```yaml
# docker-compose.yml for MongoDB replica set
version: "3.8"

services:
  mongo1:
    image: mongo:7
    command: ["--replSet", "rs0", "--bind_ip_all"]
    ports:
      - "30001:27017"
    volumes:
      - mongo1-data:/data/db

  mongo2:
    image: mongo:7
    command: ["--replSet", "rs0", "--bind_ip_all"]
    ports:
      - "30002:27017"
    volumes:
      - mongo2-data:/data/db

  mongo3:
    image: mongo:7
    command: ["--replSet", "rs0", "--bind_ip_all"]
    ports:
      - "30003:27017"
    volumes:
      - mongo3-data:/data/db

volumes:
  mongo1-data:
  mongo2-data:
  mongo3-data:
```

### Initialize Replica Set

```bash
# Connect to primary and initialize
docker exec -it mongodb-mongo1-1 mongosh

# Run in mongosh:
rs.initiate({
  _id: "rs0",
  members: [
    { _id: 0, host: "mongo1:27017" },
    { _id: 1, host: "mongo2:27017" },
    { _id: 2, host: "mongo3:27017" }
  ]
})

# Check status
rs.status()
```

## Quick Reference

| Component | Purpose |
|-----------|---------|
| Primary | Write operations |
| Secondary | Read operations, backup |
| Arbiter | Voting only |

## What's Next

Continue to [Prometheus and Grafana](../observability/01-prometheus-and-grafana.md) for monitoring.
