# Microservices Architecture

## Overview

Microservices architecture splits applications into small, independent services. This guide covers designing and implementing microservices with Docker.

## Prerequisites

- Docker and Compose knowledge
- API design basics

## Core Concepts

### Monolith vs Microservices

| Aspect | Monolith | Microservices |
|--------|----------|---------------|
| Deployment | Single unit | Independent |
| Scaling | Entire app | Individual services |
| Complexity | Lower | Higher |

### 12-Factor App Principles

1. **Codebase**: One repo per service
2. **Dependencies**: Explicitly declared
3. **Config**: Store in environment
4. **Backing services**: Treat as attached resources
5. **Processes**: Stateless
6. **Port binding**: Export via port

## Reference Architecture

```
┌─────────────────────────────────┐
│         API Gateway             │
│         (Traefik/Nginx)         │
└─────────────┬───────────────────┘
              │
    ┌─────────┼─────────┐
    ▼         ▼         ▼
┌───────┐ ┌───────┐ ┌───────┐
│  UI   │ │ Order │ │ User  │
│Service│ │Service│ │Service│
└───┬───┘ └───┬───┘ └───┬───┘
    │         │         │
    └─────────┼─────────┘
              ▼
        ┌─────────┐
        │ Database│
        │ (per    │
        │ service)│
        └─────────┘
```

## Gotchas for Docker Users

- **Network communication**: Services must communicate over network
- **Service discovery**: Need a way to find services
- **Distributed tracing**: Harder to debug than monolith

## Quick Reference

| Principle | Docker Equivalent |
|-----------|-------------------|
| Config | Environment variables |
| Backing services | Docker networks |
| Port binding | -p flag |

## What's Next

Continue to [Inter-Service Communication](./02-inter-service-communication.md) for service networking.
