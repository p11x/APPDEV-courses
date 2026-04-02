# Kong Gateway

## What You'll Learn

- What Kong is and how it works
- How to set up Kong as an API gateway
- How to add plugins for rate limiting, auth, logging
- How Kong compares to Nginx and Traefik

## Setup

```bash
# Docker Compose
docker compose up -d
```

```yaml
# docker-compose.yml
services:
  kong:
    image: kong/kong-gateway:latest
    environment:
      KONG_DATABASE: "off"
      KONG_DECLARATIVE_CONFIG: /etc/kong/kong.yml
      KONG_PROXY_LISTEN: "0.0.0.0:8000"
      KONG_ADMIN_LISTEN: "0.0.0.0:8001"
    ports:
      - "8000:8000"
      - "8001:8001"
    volumes:
      - ./kong.yml:/etc/kong/kong.yml
```

## Declarative Configuration

```yaml
# kong.yml

_format_version: "3.0"

services:
  - name: user-service
    url: http://user-service:3000
    routes:
      - name: user-routes
        paths:
          - /api/users

  - name: order-service
    url: http://order-service:3000
    routes:
      - name: order-routes
        paths:
          - /api/orders

plugins:
  - name: rate-limiting
    config:
      minute: 100
      policy: local

  - name: jwt
    config:
      uri_param_names:
        - jwt

  - name: cors
    config:
      origins:
        - "*"
      methods:
        - GET
        - POST
        - PUT
        - DELETE
```

## Next Steps

For Traefik, continue to [Traefik Gateway](./03-traefik-gateway.md).
