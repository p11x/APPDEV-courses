# Service Discovery

## Overview

Service discovery enables dynamic service location in microservices architectures.

## Consul Integration

### Service Registration

```python
# Example 1: Consul service discovery
import consul
from fastapi import FastAPI
import socket
import uuid

app = FastAPI()

class ConsulRegistry:
    """Consul service registry client"""

    def __init__(self, host: str = "localhost", port: int = 8500):
        self.client = consul.Consul(host=host, port=port)

    def register(
        self,
        service_name: str,
        service_id: str,
        address: str,
        port: int,
        health_check_url: str = None
    ):
        """Register service with Consul"""
        check = consul.Check.http(
            health_check_url,
            interval="10s",
            timeout="5s"
        ) if health_check_url else None

        self.client.agent.service.register(
            name=service_name,
            service_id=service_id,
            address=address,
            port=port,
            check=check
        )

    def deregister(self, service_id: str):
        """Deregister service"""
        self.client.agent.service.deregister(service_id)

    def discover(self, service_name: str) -> list:
        """Discover service instances"""
        _, services = self.client.health.service(service_name, passing=True)
        return [
            {
                "address": s["Service"]["Address"],
                "port": s["Service"]["Port"]
            }
            for s in services
        ]

registry = ConsulRegistry()

@app.on_event("startup")
async def startup():
    """Register with Consul on startup"""
    hostname = socket.gethostname()
    ip = socket.gethostbyname(hostname)

    registry.register(
        service_name="fastapi-app",
        service_id=f"fastapi-{uuid.uuid4().hex[:8]}",
        address=ip,
        port=8000,
        health_check_url=f"http://{ip}:8000/health"
    )

@app.on_event("shutdown")
async def shutdown():
    """Deregister on shutdown"""
    registry.deregister(f"fastapi-{socket.gethostname()}")
```

## Client-Side Discovery

### Load Balanced Discovery

```python
# Example 2: Client-side service discovery
import random
import httpx
from typing import List, Dict

class ServiceDiscoveryClient:
    """Client-side service discovery with load balancing"""

    def __init__(self, registry: ConsulRegistry):
        self.registry = registry
        self.cache: Dict[str, List[dict]] = {}
        self.cache_ttl = 30

    async def get_service_url(self, service_name: str) -> str:
        """Get service URL with load balancing"""
        instances = self.registry.discover(service_name)

        if not instances:
            raise Exception(f"No instances of {service_name} available")

        # Random load balancing
        instance = random.choice(instances)
        return f"http://{instance['address']}:{instance['port']}"

    async def call_service(
        self,
        service_name: str,
        method: str,
        path: str,
        **kwargs
    ):
        """Call service with discovery"""
        url = await self.get_service_url(service_name)
        full_url = f"{url}{path}"

        async with httpx.AsyncClient() as client:
            response = await getattr(client, method)(full_url, **kwargs)
            return response.json()

discovery_client = ServiceDiscoveryClient(registry)

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """Get user via service discovery"""
    return await discovery_client.call_service(
        "user-service",
        "get",
        f"/users/{user_id}"
    )
```

## Summary

Service discovery enables dynamic microservices architectures.

## Next Steps

Continue learning about:
- [Circuit Breaker Patterns](./10_circuit_breaker_patterns.md)
- [API Gateway Implementation](./08_api_gateway_implementation.md)
