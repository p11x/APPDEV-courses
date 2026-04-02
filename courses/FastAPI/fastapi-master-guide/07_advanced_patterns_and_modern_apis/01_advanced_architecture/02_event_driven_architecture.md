# Event-Driven Architecture

## Overview

Event-driven architecture enables loose coupling between services through asynchronous event communication.

## Implementation

### Event Publisher

```python
# Example 1: Event publishing
from fastapi import FastAPI
from pydantic import BaseModel
from datetime import datetime
import asyncio
import json

app = FastAPI()

class Event(BaseModel):
    event_type: str
    payload: dict
    timestamp: datetime = datetime.utcnow()

class EventBus:
    """Simple in-memory event bus"""

    def __init__(self):
        self.handlers = {}

    def subscribe(self, event_type: str, handler):
        if event_type not in self.handlers:
            self.handlers[event_type] = []
        self.handlers[event_type].append(handler)

    async def publish(self, event: Event):
        handlers = self.handlers.get(event.event_type, [])
        for handler in handlers:
            asyncio.create_task(handler(event))

event_bus = EventBus()

@app.post("/orders/")
async def create_order(order: dict):
    # Create order logic
    order_id = 1

    # Publish event
    await event_bus.publish(Event(
        event_type="order.created",
        payload={"order_id": order_id, "items": order["items"]}
    ))

    return {"order_id": order_id}
```

### Event Handler

```python
# Example 2: Event handlers
async def handle_order_created(event: Event):
    """Handle order created event"""
    order_id = event.payload["order_id"]
    # Send confirmation email
    # Update inventory
    print(f"Processing order {order_id}")

event_bus.subscribe("order.created", handle_order_created)
```

## Message Queue Integration

```python
# Example 3: RabbitMQ integration
import aio_pika

class RabbitMQEventBus:
    """RabbitMQ event bus"""

    def __init__(self, url: str):
        self.url = url
        self.connection = None
        self.channel = None

    async def connect(self):
        self.connection = await aio_pika.connect_robust(self.url)
        self.channel = await self.connection.channel()

    async def publish(self, routing_key: str, message: dict):
        await self.channel.default_exchange.publish(
            aio_pika.Message(body=json.dumps(message).encode()),
            routing_key=routing_key
        )
```

## Summary

Event-driven architecture enables scalable, loosely-coupled systems.

## Next Steps

Continue learning about:
- [CQRS Pattern](./03_cqrs_pattern.md)
- [Saga Pattern](./04_saga_pattern.md)
