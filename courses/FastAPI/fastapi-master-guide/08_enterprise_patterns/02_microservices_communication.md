# Microservices Communication

## Overview

Microservices communicate through synchronous and asynchronous patterns for scalable distributed systems.

## Communication Patterns

### Synchronous Communication

```python
# Example 1: HTTP-based service communication
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

    async def call(
        self,
        service: str,
        method: str,
        path: str,
        **kwargs
    ):
        """Make service call with error handling"""
        url = f"{self.services[service]}{path}"

        try:
            response = await getattr(self.client, method)(url, **kwargs)
            response.raise_for_status()
            return response.json()
        except httpx.HTTPStatusError as e:
            raise HTTPException(
                status_code=e.response.status_code,
                detail=f"Service {service} error"
            )
        except httpx.RequestError:
            raise HTTPException(
                status_code=503,
                detail=f"Service {service} unavailable"
            )

@lru_cache()
def get_service_client():
    return ServiceClient()

# API Composition
@app.get("/orders/{order_id}/details")
async def get_order_details(order_id: int):
    """Aggregate data from multiple services"""
    client = get_service_client()

    # Parallel service calls
    order_task = client.call("order", "get", f"/orders/{order_id}")
    user_task = client.call("user", "get", f"/users/{order['user_id']}")

    order, user = await asyncio.gather(order_task, user_task)

    return {
        "order": order,
        "user": user
    }
```

### Asynchronous Communication

```python
# Example 2: Event-driven communication
import aio_pika
import json
from datetime import datetime

class EventBus:
    """RabbitMQ event bus"""

    def __init__(self, connection_url: str):
        self.connection_url = connection_url
        self.connection = None
        self.channel = None

    async def connect(self):
        self.connection = await aio_pika.connect_robust(self.connection_url)
        self.channel = await self.connection.channel()

    async def publish(self, exchange: str, routing_key: str, event: dict):
        """Publish event to exchange"""
        message = aio_pika.Message(
            body=json.dumps(event).encode(),
            content_type="application/json",
            delivery_mode=aio_pika.DeliveryMode.PERSISTENT
        )

        await self.channel.default_exchange.publish(
            message,
            routing_key=f"{exchange}.{routing_key}"
        )

    async def subscribe(self, queue: str, callback):
        """Subscribe to queue"""
        q = await self.channel.declare_queue(queue, durable=True)
        await q.consume(callback)

event_bus = EventBus("amqp://guest:guest@rabbitmq/")

@app.post("/orders/")
async def create_order(order: OrderCreate):
    # Create order
    new_order = await save_order(order)

    # Publish event for other services
    await event_bus.publish(
        exchange="orders",
        routing_key="created",
        event={
            "event_type": "order.created",
            "order_id": new_order.id,
            "user_id": new_order.user_id,
            "timestamp": datetime.utcnow().isoformat()
        }
    )

    return new_order
```

## Circuit Breaker Pattern

### Resilience Implementation

```python
# Example 3: Circuit breaker for service calls
from enum import Enum
from datetime import datetime, timedelta
from functools import wraps

class CircuitState(Enum):
    CLOSED = "closed"
    OPEN = "open"
    HALF_OPEN = "half_open"

class CircuitBreaker:
    """Circuit breaker for fault tolerance"""

    def __init__(
        self,
        failure_threshold: int = 5,
        recovery_timeout: int = 30
    ):
        self.failure_threshold = failure_threshold
        self.recovery_timeout = recovery_timeout
        self.state = CircuitState.CLOSED
        self.failure_count = 0
        self.last_failure_time = None

    def __call__(self, func):
        @wraps(func)
        async def wrapper(*args, **kwargs):
            if self.state == CircuitState.OPEN:
                if self._should_attempt_reset():
                    self.state = CircuitState.HALF_OPEN
                else:
                    raise Exception("Circuit breaker is OPEN")

            try:
                result = await func(*args, **kwargs)
                self._on_success()
                return result
            except Exception as e:
                self._on_failure()
                raise e

        return wrapper

    def _on_success(self):
        if self.state == CircuitState.HALF_OPEN:
            self.state = CircuitState.CLOSED
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
@circuit_breaker(failure_threshold=5, recovery_timeout=30)
async def call_user_service(user_id: int):
    async with httpx.AsyncClient() as client:
        response = await client.get(f"http://user-service:8001/users/{user_id}")
        response.raise_for_status()
        return response.json()
```

## Summary

Microservices require careful communication patterns for reliability.

## Next Steps

Continue learning about:
- [API Gateway Patterns](./03_api_gateway_patterns.md)
- [Service Mesh Patterns](./04_service_mesh_patterns.md)
