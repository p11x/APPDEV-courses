# API Gateway Pattern

## Overview

An API gateway provides a single entry point for microservices. It handles routing, authentication, rate limiting, and more.

## Prerequisites

- Docker networking
- Understanding of reverse proxies

## Core Concepts

### Gateway Responsibilities

- Request routing
- Authentication
- Rate limiting
- Logging
- SSL termination

## Step-by-Step Examples

### Using Traefik

```yaml
# docker-compose.yml with Traefik
version: "3.8"

services:
  traefik:
    image: traefik:v3.0
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock:ro
    networks:
      - myapp-net

  web:
    image: myweb
    labels:
      - "traefik.http.routers.web.rule=PathPrefix(/web)"
    networks:
      - myapp-net

  api:
    image: myapi
    labels:
      - "traefik.http.routers.api.rule=PathPrefix(/api)"
    networks:
      - myapp-net

networks:
  myapp-net:
```

## Quick Reference

| Gateway | Pros |
|---------|------|
| Traefik | Docker-native labels |
| Nginx | Performance |
| Kong | Enterprise features |

## What's Next

Continue to [Postgres with Docker](../../databases/01-postgres-with-docker.md) for database setup.
