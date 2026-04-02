# Event Sourcing Patterns

## Overview

Event sourcing stores state changes as events, enabling audit trails and temporal queries.

## Event Store Implementation

### Basic Event Sourcing

```python
# Example 1: Event sourcing with FastAPI
from fastapi import FastAPI
from pydantic import BaseModel
from typing import List, Dict
from datetime import datetime
import json

app = FastAPI()

class Event(BaseModel):
    aggregate_id: str
    event_type: str
    data: Dict
    timestamp: datetime = datetime.utcnow()
    version: int

# Event store (use database in production)
event_store: List[Event] = []

class Aggregate:
    """Base aggregate for event sourcing"""

    def __init__(self, aggregate_id: str):
        self.aggregate_id = aggregate_id
        self.version = 0
        self.changes: List[Event] = []

    def apply(self, event: Event):
        """Apply event to aggregate"""
        self._apply_event(event)
        self.version = event.version

    def _apply_event(self, event: Event):
        """Override in subclasses"""
        raise NotImplementedError

    def raise_event(self, event_type: str, data: Dict):
        """Raise new event"""
        event = Event(
            aggregate_id=self.aggregate_id,
            event_type=event_type,
            data=data,
            version=self.version + 1
        )
        self.apply(event)
        self.changes.append(event)

class Order(Aggregate):
    """Order aggregate with event sourcing"""

    def __init__(self, order_id: str):
        super().__init__(order_id)
        self.status = "pending"
        self.items: List[Dict] = []
        self.total = 0.0

    def _apply_event(self, event: Event):
        if event.event_type == "OrderCreated":
            self.status = "pending"
        elif event.event_type == "ItemAdded":
            self.items.append(event.data)
            self.total += event.data["price"]
        elif event.event_type == "OrderConfirmed":
            self.status = "confirmed"

    def create(self):
        self.raise_event("OrderCreated", {})

    def add_item(self, item: Dict):
        self.raise_event("ItemAdded", item)

    def confirm(self):
        self.raise_event("OrderConfirmed", {})

def load_aggregate(aggregate_id: str, aggregate_class) -> Aggregate:
    """Load aggregate from events"""
    aggregate = aggregate_class(aggregate_id)

    events = [e for e in event_store if e.aggregate_id == aggregate_id]
    events.sort(key=lambda e: e.version)

    for event in events:
        aggregate.apply(event)

    return aggregate

def save_aggregate(aggregate: Aggregate):
    """Save aggregate changes to event store"""
    event_store.extend(aggregate.changes)
    aggregate.changes.clear()

@app.post("/orders/")
async def create_order():
    """Create order using event sourcing"""
    order = Order("order-1")
    order.create()
    save_aggregate(order)

    return {"order_id": order.aggregate_id, "status": order.status}

@app.post("/orders/{order_id}/items")
async def add_item(order_id: str, item: Dict):
    """Add item to order"""
    order = load_aggregate(order_id, Order)
    order.add_item(item)
    save_aggregate(order)

    return {"order_id": order_id, "items": order.items}
```

## Summary

Event sourcing provides complete audit trails and temporal queries.

## Next Steps

Continue learning about:
- [CQRS Patterns](./06_cqrs_patterns.md)
- [Distributed Tracing](./07_distributed_tracing.md)
