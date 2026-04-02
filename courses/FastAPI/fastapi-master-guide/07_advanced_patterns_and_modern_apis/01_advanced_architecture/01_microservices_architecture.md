# Microservices Architecture

## Overview

Microservices architecture decomposes applications into small, independent services. FastAPI is ideal for building microservices due to its performance and async capabilities.

## Service Structure

### Basic Microservice

```python
# Example 1: User microservice structure
# user-service/app/main.py
from fastapi import FastAPI, Depends
from contextlib import asynccontextmanager

@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup: Initialize connections
    await init_database()
    await init_cache()
    yield
    # Shutdown: Cleanup
    await close_database()
    await close_cache()

app = FastAPI(
    title="User Service",
    version="1.0.0",
    lifespan=lifespan
)

# Health check for service discovery
@app.get("/health")
async def health():
    return {"status": "healthy", "service": "user-service"}

# Service endpoints
@app.get("/users/{user_id}")
async def get_user(user_id: int):
    user = await get_user_from_db(user_id)
    return user

@app.post("/users/")
async def create_user(user: UserCreate):
    return await create_user_in_db(user)

# Service discovery registration
@app.on_event("startup")
async def register_service():
    await register_with_consul(
        service_name="user-service",
        service_id=f"user-service-{uuid.uuid4()}",
        address="localhost",
        port=8001
    )
```

## Inter-Service Communication

### HTTP Client for Service Calls

```python
# Example 2: Service-to-service communication
from fastapi import FastAPI, HTTPException
import httpx
from functools import lru_cache

app = FastAPI()

class ServiceClient:
    """Client for inter-service communication"""

    def __init__(self):
        self.client = httpx.AsyncClient(timeout=30.0)
        self.services = {
            "user": "http://user-service:8001",
            "order": "http://order-service:8002",
            "inventory": "http://inventory-service:8003"
        }

    async def call_service(
        self,
        service: str,
        method: str,
        path: str,
        **kwargs
    ):
        """Make HTTP call to another service"""
        url = f"{self.services[service]}{path}"

        try:
            response = await getattr(self.client, method)(url, **kwargs)
            response.raise_for_status()
            return response.json()
        except httpx.HTTPError as e:
            raise HTTPException(
                status_code=503,
                detail=f"Service {service} unavailable: {str(e)}"
            )

    async def get_user(self, user_id: int):
        """Get user from user service"""
        return await self.call_service("user", "get", f"/users/{user_id}")

    async def get_order(self, order_id: int):
        """Get order from order service"""
        return await self.call_service("order", "get", f"/orders/{order_id}")

@lru_cache()
def get_service_client():
    return ServiceClient()

# Usage in endpoints
@app.get("/orders/{order_id}/details")
async def get_order_details(order_id: int):
    """Aggregate data from multiple services"""
    client = get_service_client()

    # Get order from order service
    order = await client.get_order(order_id)

    # Get user details from user service
    user = await client.get_user(order["user_id"])

    return {
        "order": order,
        "user": user
    }
```

## Event-Driven Communication

### Message Publishing

```python
# Example 3: Event-driven with RabbitMQ/Redis
from fastapi import FastAPI
import aio_pika
import json
from pydantic import BaseModel

app = FastAPI()

class Event(BaseModel):
    event_type: str
    payload: dict
    timestamp: str

class EventBus:
    """Event bus for publishing/subscribing"""

    def __init__(self, connection_url: str):
        self.connection_url = connection_url
        self.connection = None
        self.channel = None

    async def connect(self):
        self.connection = await aio_pika.connect_robust(self.connection_url)
        self.channel = await self.connection.channel()

    async def publish(self, exchange: str, routing_key: str, event: Event):
        """Publish event to exchange"""
        await self.channel.default_exchange.publish(
            aio_pika.Message(
                body=json.dumps(event.dict()).encode(),
                content_type="application/json"
            ),
            routing_key=f"{exchange}.{routing_key}"
        )

    async def subscribe(self, queue: str, callback):
        """Subscribe to queue"""
        q = await self.channel.declare_queue(queue, durable=True)
        await q.consume(callback)

event_bus = EventBus("amqp://guest:guest@localhost/")

@app.on_event("startup")
async def startup():
    await event_bus.connect()

# Publishing events
@app.post("/orders/")
async def create_order(order: OrderCreate):
    # Create order
    new_order = await save_order(order)

    # Publish event
    await event_bus.publish(
        exchange="orders",
        routing_key="created",
        event=Event(
            event_type="order.created",
            payload={"order_id": new_order.id, "user_id": new_order.user_id},
            timestamp=datetime.utcnow().isoformat()
        )
    )

    return new_order
```

## Service Discovery

### Consul Integration

```python
# Example 4: Service discovery with Consul
import consul
from fastapi import FastAPI
import socket

app = FastAPI()

class ServiceRegistry:
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

registry = ServiceRegistry()

@app.on_event("startup")
async def startup():
    hostname = socket.gethostname()
    ip = socket.gethostbyname(hostname)

    registry.register(
        service_name="user-service",
        service_id=f"user-service-{hostname}",
        address=ip,
        port=8000,
        health_check_url=f"http://{ip}:8000/health"
    )

@app.on_event("shutdown")
async def shutdown():
    registry.deregister(f"user-service-{socket.gethostname()}")
```

## API Gateway Pattern

### Gateway Implementation

```python
# Example 5: API Gateway with FastAPI
from fastapi import FastAPI, Request, HTTPException
import httpx

app = FastAPI(title="API Gateway")

# Service routes configuration
SERVICE_ROUTES = {
    "/users": {"url": "http://user-service:8001", "strip_prefix": True},
    "/orders": {"url": "http://order-service:8002", "strip_prefix": True},
    "/products": {"url": "http://product-service:8003", "strip_prefix": True},
}

@app.api_route("/{service}/{path:path}", methods=["GET", "POST", "PUT", "DELETE", "PATCH"])
async def gateway(service: str, path: str, request: Request):
    """Route requests to appropriate service"""
    service_prefix = f"/{service}"

    if service_prefix not in SERVICE_ROUTES:
        raise HTTPException(status_code=404, detail="Service not found")

    config = SERVICE_ROUTES[service_prefix]
    target_url = f"{config['url']}/{path}"

    # Forward request
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

## Circuit Breaker

### Resilience Pattern

```python
# Example 6: Circuit breaker implementation
from enum import Enum
from datetime import datetime, timedelta
import asyncio
from functools import wraps

class CircuitState(Enum):
    CLOSED = "closed"
    OPEN = "open"
    HALF_OPEN = "half_open"

class CircuitBreaker:
    """Circuit breaker for service calls"""

    def __init__(
        self,
        failure_threshold: int = 5,
        recovery_timeout: int = 30,
        expected_exception: type = Exception
    ):
        self.failure_threshold = failure_threshold
        self.recovery_timeout = recovery_timeout
        self.expected_exception = expected_exception

        self.state = CircuitState.CLOSED
        self.failure_count = 0
        self.last_failure_time = None
        self.success_count = 0

    def __call__(self, func):
        @wraps(func)
        async def wrapper(*args, **kwargs):
            return await self.call(func, *args, **kwargs)
        return wrapper

    async def call(self, func, *args, **kwargs):
        if self.state == CircuitState.OPEN:
            if self._should_attempt_reset():
                self.state = CircuitState.HALF_OPEN
            else:
                raise Exception("Circuit breaker is OPEN")

        try:
            result = await func(*args, **kwargs)
            self._on_success()
            return result
        except self.expected_exception as e:
            self._on_failure()
            raise e

    def _on_success(self):
        if self.state == CircuitState.HALF_OPEN:
            self.success_count += 1
            if self.success_count >= 3:
                self.state = CircuitState.CLOSED
                self.failure_count = 0
                self.success_count = 0
        else:
            self.failure_count = 0

    def _on_failure(self):
        self.failure_count += 1
        self.last_failure_time = datetime.utcnow()

        if self.failure_count >= self.failure_threshold:
            self.state = CircuitState.OPEN

    def _should_attempt_reset(self):
        if self.last_failure_time is None:
            return True
        return datetime.utcnow() - self.last_failure_time > timedelta(
            seconds=self.recovery_timeout
        )

# Usage
user_service_breaker = CircuitBreaker(failure_threshold=5, recovery_timeout=30)

@user_service_breaker
async def call_user_service(user_id: int):
    async with httpx.AsyncClient() as client:
        response = await client.get(f"http://user-service:8001/users/{user_id}")
        response.raise_for_status()
        return response.json()
```

## Best Practices

### Microservices Guidelines

```python
# Example 7: Best practices checklist
"""
Microservices Best Practices:

1. Service Boundaries
   - Single responsibility
   - Own database
   - Independent deployment

2. Communication
   - Async where possible
   - Circuit breakers
   - Retry with backoff

3. Data Management
   - Event sourcing
   - CQRS pattern
   - Saga pattern

4. Observability
   - Distributed tracing
   - Centralized logging
   - Health checks

5. Resilience
   - Circuit breakers
   - Bulkheads
   - Timeouts

6. Deployment
   - Container orchestration
   - Service mesh
   - Blue-green/canary
"""
```

## Summary

| Pattern | Purpose | Implementation |
|---------|---------|----------------|
| Service Discovery | Find services | Consul, etcd |
| API Gateway | Route requests | FastAPI routing |
| Circuit Breaker | Fault tolerance | Custom decorator |
| Event Bus | Async communication | RabbitMQ, Redis |

## Next Steps

Continue learning about:
- [Event-Driven Architecture](./02_event_driven_architecture.md) - Event patterns
- [CQRS Pattern](./03_cqrs_pattern.md) - Command/Query separation
- [Caching Strategies](../02_performance_optimization/01_caching_strategies.md)
