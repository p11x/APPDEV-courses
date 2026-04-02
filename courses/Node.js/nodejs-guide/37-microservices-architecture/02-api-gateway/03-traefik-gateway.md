# Traefik Gateway

## What You'll Learn

- What Traefik is and how it works
- How to set up Traefik with Docker
- How Traefik auto-discovers services
- How Traefik compares to Nginx and Kong

## Setup

```yaml
# docker-compose.yml

services:
  traefik:
    image: traefik:v3.0
    command:
      - "--api.insecure=true"
      - "--providers.docker=true"
      - "--providers.docker.exposedbydefault=false"
      - "--entrypoints.web.address=:80"
      - "--entrypoints.websecure.address=:443"
      - "--certificatesresolvers.letsencrypt.acme.httpchallenge.entrypoint=web"
      - "--certificatesresolvers.letsencrypt.acme.email=admin@example.com"
      - "--certificatesresolvers.letsencrypt.acme.storage=/letsencrypt/acme.json"
    ports:
      - "80:80"
      - "443:443"
      - "8080:8080"
    volumes:
      - "/var/run/docker.sock:/var/run/docker.sock:ro"
      - "./letsencrypt:/letsencrypt"

  user-service:
    image: myapp/user-service
    labels:
      - "traefik.enable=true"
      - "traefik.http.routers.user.rule=PathPrefix(`/api/users`)"
      - "traefik.http.routers.user.entrypoints=websecure"
      - "traefik.http.routers.user.tls.certresolver=letsencrypt"
      - "traefik.http.services.user.loadbalancer.server.port=3000"

  order-service:
    image: myapp/order-service
    labels:
      - "traefik.enable=true"
      - "traefik.http.routers.order.rule=PathPrefix(`/api/orders`)"
      - "traefik.http.routers.order.entrypoints=websecure"
      - "traefik.http.routers.order.tls.certresolver=letsencrypt"
```

## Comparison

| Feature | Nginx | Kong | Traefik |
|---------|-------|------|---------|
| Config | File-based | Declarative/DB | Labels (Docker) |
| Auto-discovery | No | Plugins | Native (Docker/K8s) |
| SSL | Manual | Plugins | Auto (Let's Encrypt) |
| Dashboard | No | Yes (Konga) | Built-in |
| Best for | Simple setups | Enterprise APIs | Docker/K8s |

## Next Steps

For gateway patterns, continue to [Gateway Patterns](./04-gateway-patterns.md).
