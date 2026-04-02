# CQRS Pattern

## Overview

Command Query Responsibility Segregation (CQRS) separates read and write operations for better scalability and performance.

## Implementation

### Basic CQRS

```python
# Example 1: CQRS with separate models
from fastapi import FastAPI, Depends
from sqlalchemy.orm import Session

app = FastAPI()

# Command side (writes)
class CreateOrderCommand:
    def __init__(self, user_id: int, items: list):
        self.user_id = user_id
        self.items = items

class OrderCommandHandler:
    """Handle order commands (writes)"""

    def __init__(self, db: Session):
        self.db = db

    async def handle_create(self, command: CreateOrderCommand):
        """Process order creation"""
        order = Order(
            user_id=command.user_id,
            items=command.items,
            status="pending"
        )
        self.db.add(order)
        self.db.commit()

        # Publish event for read side
        await publish_event("order.created", {"order_id": order.id})

        return order

# Query side (reads)
class OrderQueryHandler:
    """Handle order queries (reads)"""

    def __init__(self, db: Session):
        self.db = db

    async def get_order(self, order_id: int):
        """Get order from read model"""
        return self.db.query(OrderReadModel).filter_by(id=order_id).first()

    async def get_user_orders(self, user_id: int):
        """Get all orders for user"""
        return self.db.query(OrderReadModel).filter_by(user_id=user_id).all()

# API endpoints
@app.post("/orders/")
async def create_order(
    command: CreateOrderCommand,
    db: Session = Depends(get_db)
):
    handler = OrderCommandHandler(db)
    return await handler.handle_create(command)

@app.get("/orders/{order_id}")
async def get_order(order_id: int, db: Session = Depends(get_db)):
    handler = OrderQueryHandler(db)
    return await handler.get_order(order_id)
```

## Benefits

1. Independent scaling of reads and writes
2. Optimized read models
3. Better separation of concerns
4. Easier to optimize each side

## Summary

CQRS is powerful for applications with complex read/write patterns.

## Next Steps

Continue learning about:
- [Event-Driven Architecture](./02_event_driven_architecture.md)
- [Saga Pattern](./04_saga_pattern.md)
