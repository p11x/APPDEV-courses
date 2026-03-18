# Microservices Basics

## What You'll Learn
- Monolith vs microservices
- Service communication
- API Gateway

## Prerequisites
- Completed performance folder

## Monolith vs Microservices

| Aspect | Monolith | Microservices |
|--------|----------|---------------|
| Deploy | Single unit | Independent |
| Scale | Entire app | Individual services |
| Tech | Single stack | Polyglot |
| Complexity | Low | High |

## Service Communication

```python
# Direct HTTP calls
import httpx

async def call_user_service(user_id: int):
    async with httpx.AsyncClient() as client:
        response = await client.get(f"http://user-service:8000/users/{user_id}")
        return response.json()

# Service discovery (Consul, etcd)
```

## API Gateway

```yaml
# docker-compose.yml
services:
  api-gateway:
    image: nginx
    ports:
      - "80:80"
```

```nginx
# nginx.conf
server {
    location /api/users/ {
        proxy_pass http://user-service:8000;
    }
    location /api/products/ {
        proxy_pass http://product-service:8001;
    }
}
```

## Summary
- Microservices add complexity
- Use when needed
- Start with monolith

## Next Steps
→ Continue to `02-service-discovery.md`
