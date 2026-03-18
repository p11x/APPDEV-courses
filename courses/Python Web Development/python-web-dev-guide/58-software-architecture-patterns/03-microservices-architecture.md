# Microservices Architecture

## What You'll Learn

- What microservices are and how they differ from monoliths
- Service decomposition strategies
- Inter-service communication patterns
- Data management in microservices

## Prerequisites

- Understanding of Service-Oriented Architecture
- Knowledge of REST APIs and message queues

## Introduction

Microservices architecture is an approach where an application is built as a collection of small, independent services. Each service is self-contained, maintains its own data, and communicates with other services through well-defined APIs.

Think of a microservices architecture like a city with specialized shops instead of one giant department store. Each shop (service) operates independently, has its own inventory (database), and communicates with other shops through standardized channels. If the bakery needs flour, it doesn't own the warehouse—it just places an order with the warehouse service.

## Key Characteristics

### 1. Independent Deployment

Each microservice can be deployed, scaled, and updated independently. This means:

- A bug in the user service doesn't require redeploying the entire application
- Teams can work on different services simultaneously
- Different services can use different technology stacks

### 2. Own Data Store

Each service maintains its own database:

- Services are loosely coupled through APIs, not shared databases
- Each service can choose the best database for its needs
- Data consistency becomes a challenge requiring eventual consistency patterns

### 3. Business Capability Boundaries

Services are organized around business capabilities, not technical layers:

- User service handles all user-related functionality
- Order service manages orders
- Notification service handles emails, SMS, push notifications

## Service Communication

### Synchronous Communication (REST/gRPC)

```python
# Example: Calling another service via REST
import httpx
from typing import Any

class OrderService:
    def __init__(self, user_service_url: str):
        self.user_service_url = user_service_url
    
    async def create_order(self, user_id: int, items: list[dict[str, Any]]) -> dict[str, Any]:
        async with httpx.AsyncClient() as client:
            # Verify user exists before creating order
            response = await client.get(f"{self.user_service_url}/users/{user_id}")
            if response.status_code == 404:
                raise ValueError(f"User {user_id} not found")
            
            user_data = response.json()
            
            order = {
                "user_id": user_id,
                "user_name": user_data["name"],
                "items": items,
                "status": "pending"
            }
            
            return order
```

🔍 **Line-by-Line Breakdown:**

1. `import httpx` — HTTP client library for making async HTTP requests. Essential for service-to-service communication.
2. `class OrderService:` — Service class that handles order-related business logic.
3. `def __init__(self, user_service_url: str):` — Constructor takes the URL of the user service. This is typically configured via environment variables in production.
4. `async with httpx.AsyncClient() as client:` — Creates an async HTTP client. The `async with` ensures proper resource cleanup.
5. `await client.get(f"{self.user_service_url}/users/{user_id}")` — Makes an async GET request to verify the user exists. Uses await because HTTP calls are I/O-bound.
6. `if response.status_code == 404:` — Checks if user was not found. Different services communicate errors via HTTP status codes.
7. `raise ValueError(f"User {user_id} not found")` — Raises an exception if the user doesn't exist. This will be handled by the caller.
8. `user_data = response.json()` — Parses the JSON response from the user service.
9. `order = {...}` — Creates the order dictionary with user information from the verified user.
10. `return order` — Returns the created order (would typically save to database in a real implementation).

### Asynchronous Communication (Message Queues)

```python
# Example: Publishing events to a message queue
import asyncio
from dataclasses import dataclass
from datetime import datetime
from typing import Literal

@dataclass
class OrderCreatedEvent:
    order_id: int
    user_id: int
    total_amount: float
    timestamp: datetime
    event_type: Literal["order_created"] = "order_created"

class EventPublisher:
    def __init__(self, queue_url: str):
        self.queue_url = queue_url
        self._pending_events: list[dict] = []
    
    async def publish_order_created(self, event: OrderCreatedEvent) -> None:
        """Publish order created event to message queue."""
        event_data = {
            "order_id": event.order_id,
            "user_id": event.user_id,
            "total_amount": event.total_amount,
            "timestamp": event.timestamp.isoformat(),
            "event_type": event.event_type
        }
        
        # In production, this would publish to RabbitMQ, Kafka, etc.
        self._pending_events.append(event_data)
        print(f"Published event: {event_data}")
        
        # Simulate async publish
        await asyncio.sleep(0.1)

# Usage example
async def main():
    publisher = EventPublisher("amqp://localhost:5672")
    
    event = OrderCreatedEvent(
        order_id=12345,
        user_id=42,
        total_amount=99.99,
        timestamp=datetime.now()
    )
    
    await publisher.publish_order_created(event)

asyncio.run(main())
```

🔍 **Line-by-Line Breakdown:**

1. `import asyncio` — Built-in async library for simulating async operations.
2. `@dataclass` — Python decorator that automatically generates `__init__`, `__repr__`, and other methods. Makes creating structured data easy.
3. `class OrderCreatedEvent:` — Data class representing an event that gets published when an order is created.
4. `order_id: int` — Unique identifier for the order.
5. `user_id: int` — ID of the user who placed the order.
6. `total_amount: float` — Total price of the order.
7. `timestamp: datetime` — When the order was created.
8. `event_type: Literal["order_created"] = "order_created"` — Type literal ensures only valid event types can be set.
9. `class EventPublisher:` — Handles publishing events to a message queue.
10. `def __init__(self, queue_url: str):` — Constructor takes the message queue connection URL.
11. `self._pending_events: list[dict] = []` — List to store events (in production, this would be a real message queue).
12. `async def publish_order_created(self, event: OrderCreatedEvent) -> None:` — Async method to publish an order created event.
13. `event_data = {...}` — Converts the event object to a dictionary for serialization.
14. `self._pending_events.append(event_data)` — Adds the event to the queue (simulated).
15. `await asyncio.sleep(0.1)` — Simulates the time it would take to publish to a real message queue.

## Service Discovery

In a microservices system, services need to find each other dynamically:

```python
# Example: Simple service discovery
from dataclasses import dataclass
from typing import Optional

@dataclass
class ServiceInstance:
    service_id: str
    host: str
    port: int
    healthy: bool = True

class SimpleServiceRegistry:
    """A simple in-memory service registry for demonstration."""
    
    def __init__(self):
        self._services: dict[str, list[ServiceInstance]] = {}
    
    def register(self, service_name: str, host: str, port: int) -> ServiceInstance:
        instance = ServiceInstance(
            service_id=f"{service_name}-{port}",
            host=host,
            port=port
        )
        
        if service_name not in self._services:
            self._services[service_name] = []
        
        self._services[service_name].append(instance)
        return instance
    
    def discover(self, service_name: str) -> Optional[ServiceInstance]:
        instances = self._services.get(service_name, [])
        # Return a healthy instance (simple round-robin could go here)
        healthy = [i for i in instances if i.healthy]
        return healthy[0] if healthy else None
    
    def deregister(self, service_name: str, instance_id: str) -> None:
        if service_name in self._services:
            self._services[service_name] = [
                i for i in self._services[service_name]
                if i.service_id != instance_id
            ]

# Usage
registry = SimpleServiceRegistry()

# Register services
registry.register("user-service", "user-service.internal", 8001)
registry.register("order-service", "order-service.internal", 8002)
registry.register("notification-service", "notification-service.internal", 8003)

# Discover a service
user_service = registry.discover("user-service")
if user_service:
    print(f"Found user service at {user_service.host}:{user_service.port}")
```

🔍 **Line-by-Line Breakdown:**

1. `@dataclass` — Creates a data class for service instance information.
2. `class SimpleServiceRegistry:` — A simple service registry for demo purposes. In production, you'd use Consul, Eureka, or Kubernetes DNS.
3. `def __init__(self):` — Initialize with empty services dictionary.
4. `self._services: dict[str, list[ServiceInstance]] = {}` — Dictionary mapping service names to lists of instances.
5. `def register(self, service_name: str, host: str, port: int) -> ServiceInstance:` — Method for services to register themselves.
6. `instance = ServiceInstance(...)` — Create a new service instance with unique ID.
7. `if service_name not in self._services:` — Add new service name to registry if it doesn't exist.
8. `def discover(self, service_name: str) -> Optional[ServiceInstance]:` — Find a healthy instance of a service.
9. `instances = self._services.get(service_name, [])` — Get all instances for the requested service.
10. `healthy = [i for i in instances if i.healthy]` — Filter to only healthy instances.
11. `return healthy[0] if healthy else None` — Return first healthy instance or None.
12. `def deregister(self, service_name: str, instance_id: str) -> None:` — Remove a service instance from the registry.

## When to Use Microservices

### Good Fit

- Large teams (50+ developers) working on the same application
- Complex business domains with clear bounded contexts
- Need for independent scaling of different components
- Different parts of the system have different technology requirements

### Not a Good Fit

- Small teams (less than 5 developers)
- Simple applications with tightly coupled business logic
- Startups needing to move fast with minimal infrastructure
- Applications with heavy transactional requirements

## Summary

- Microservices architecture breaks applications into small, independent services
- Each service can be deployed, scaled, and maintained independently
- Services communicate via REST APIs (synchronous) or message queues (asynchronous)
- Service discovery helps services find each other dynamically
- Trade-offs include increased complexity, network latency, and data consistency challenges

## Next Steps

Continue to `03-event-driven-architecture.md` to learn about building loosely coupled systems through events.
