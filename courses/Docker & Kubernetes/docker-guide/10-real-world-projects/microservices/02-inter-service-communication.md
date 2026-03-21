# Inter-Service Communication

## Overview

Services in a microservices architecture need to communicate. This guide covers synchronous and asynchronous communication patterns.

## Prerequisites

- Docker networking knowledge
- Understanding of REST/gRPC

## Core Concepts

### Communication Patterns

- **Synchronous**: REST, gRPC - request/response
- **Asynchronous**: Message queues - fire and forget

## Step-by-Step Examples

### Docker Network Setup

```bash
# Create network for services
docker network create myapp-net

# Services on same network can reach each other by name
docker run -d --network myapp-net --name api myapi
docker run -d --network myapp-net --name web myweb

# Web can call api as http://api:port
```

### REST Communication

```bash
# Using service names as hostnames
curl http://api:8080/api/users

# In code, use service name as hostname
# Python: requests.get('http://api:8080/users')
```

### Message Queue Setup

```yaml
# docker-compose.yml
version: "3.8"

services:
  api:
    image: myapi
    networks:
      - myapp-net

  worker:
    image: myworker
    networks:
      - myapp-net
    depends_on:
      - rabbitmq

  rabbitmq:
    image: rabbitmq:3-management
    ports:
      - "15672:15672"

networks:
  myapp-net:
```

## Quick Reference

| Pattern | Use Case |
|---------|----------|
| REST | Simple request/response |
| gRPC | High performance |
| RabbitMQ | Async processing |
| Kafka | High throughput |

## What's Next

Continue to [API Gateway Pattern](./03-api-gateway-pattern.md) for routing.
