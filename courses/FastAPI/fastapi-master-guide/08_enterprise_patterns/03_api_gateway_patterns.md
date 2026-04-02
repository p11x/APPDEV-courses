# API Gateway Patterns

## Overview

API gateways provide centralized request routing, authentication, and cross-cutting concerns.

## Gateway Implementation

### FastAPI as API Gateway

```python
# Example 1: API Gateway with FastAPI
from fastapi import FastAPI, Request, HTTPException
import httpx
from typing import Dict

app = FastAPI(title="API Gateway")

# Service registry
SERVICES: Dict[str, str] = {
    "users": "http://user-service:8001",
    "orders": "http://order-service:8002",
    "products": "http://product-service:8003"
}

@app.api_route("/{service}/{path:path}", methods=["GET", "POST", "PUT", "DELETE"])
async def gateway(service: str, path: str, request: Request):
    """Route requests to appropriate service"""
    if service not in SERVICES:
        raise HTTPException(404, f"Service {service} not found")

    target_url = f"{SERVICES[service]}/{path}"

    async with httpx.AsyncClient() as client:
        response = await client.request(
            method=request.method,
            url=target_url,
            headers=dict(request.headers),
            content=await request.body()
        )

    return Response(
        content=response.content,
        status_code=response.status_code,
        headers=dict(response.headers)
    )
```

### Request Transformation

```python
# Example 2: Request/response transformation
from fastapi import Request
import json

class GatewayTransformer:
    """Transform requests and responses"""

    @staticmethod
    async def transform_request(request: Request) -> dict:
        """Add gateway headers"""
        headers = dict(request.headers)
        headers["X-Gateway-Timestamp"] = datetime.utcnow().isoformat()
        headers["X-Request-ID"] = str(uuid.uuid4())
        return headers

    @staticmethod
    def transform_response(response_data: dict) -> dict:
        """Transform response format"""
        return {
            "data": response_data,
            "meta": {
                "timestamp": datetime.utcnow().isoformat(),
                "version": "1.0"
            }
        }

@app.middleware("http")
async def gateway_middleware(request: Request, call_next):
    """Gateway middleware for transformation"""
    # Transform request
    transformed_headers = await GatewayTransformer.transform_request(request)

    response = await call_next(request)

    # Transform response could be added here
    return response
```

## Rate Limiting at Gateway

```python
# Example 3: Gateway rate limiting
from collections import defaultdict
import time

class GatewayRateLimiter:
    """Rate limiter for API gateway"""

    def __init__(self, requests_per_minute: int = 100):
        self.limit = requests_per_minute
        self.requests: Dict[str, list] = defaultdict(list)

    def check(self, client_id: str) -> bool:
        """Check if request is allowed"""
        now = time.time()
        window_start = now - 60

        # Clean old requests
        self.requests[client_id] = [
            t for t in self.requests[client_id] if t > window_start
        ]

        if len(self.requests[client_id]) >= self.limit:
            return False

        self.requests[client_id].append(now)
        return True

rate_limiter = GatewayRateLimiter()

@app.middleware("http")
async def rate_limit_middleware(request: Request, call_next):
    """Rate limiting at gateway level"""
    client_id = request.client.host

    if not rate_limiter.check(client_id):
        raise HTTPException(429, "Rate limit exceeded")

    return await call_next(request)
```

## Summary

API gateways provide centralized concerns for microservices.

## Next Steps

Continue learning about:
- [Service Mesh Patterns](./04_service_mesh_patterns.md)
- [Event Sourcing Patterns](./05_event_sourcing_patterns.md)
