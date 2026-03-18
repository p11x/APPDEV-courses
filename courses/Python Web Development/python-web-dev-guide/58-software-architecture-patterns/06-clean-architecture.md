# Clean Architecture

## What You'll Learn

- Clean architecture principles
- Domain-driven design basics
- Building testable, maintainable systems

## Prerequisites

- Understanding of layered architecture

## Introduction

Clean architecture is a software design philosophy that emphasizes separation of concerns and dependency direction. The key insight is that your business logic should not depend on external frameworks, databases, or user interfaces.

Think of clean architecture like the foundation of a building. The core structure (domain) should be solid and independent, while the outer layers (infrastructure, UI) can change without affecting the foundation.

## The Clean Architecture Layers

```
┌─────────────────────────────────────────────────────┐
│                  Presentation                        │
│              (Controllers, Routes)                  │
├─────────────────────────────────────────────────────┤
│                  Use Cases                          │
│            (Application Services)                   │
├─────────────────────────────────────────────────────┤
│                    Domain                            │
│         (Entities, Value Objects, Rules)            │
├─────────────────────────────────────────────────────┤
│                 Infrastructure                       │
│         (Database, External Services)               │
└─────────────────────────────────────────────────────┘
         ↑ Dependency direction (pointing inward)
```

The crucial rule: dependencies only point inward. Inner layers know nothing about outer layers.

## Domain Layer (Core)

```python
from dataclasses import dataclass
from datetime import datetime
from typing import Optional
from abc import ABC, abstractmethod

# ============== Entities ==============
@dataclass
class Order:
    id: int
    customer_id: int
    items: list["OrderItem"]
    status: "OrderStatus"
    created_at: datetime
    updated_at: datetime
    
    @property
    def total(self) -> float:
        return sum(item.price * item.quantity for item in self.items)
    
    def can_cancel(self) -> bool:
        return self.status in [OrderStatus.PENDING, OrderStatus.CONFIRMED]

@dataclass
class OrderItem:
    product_id: int
    product_name: str
    quantity: int
    price: float

class OrderStatus:
    PENDING = "pending"
    CONFIRMED = "confirmed"
    SHIPPED = "shipped"
    DELIVERED = "delivered"
    CANCELLED = "cancelled"

# ============== Value Objects ==============
@dataclass(frozen=True)
class Email:
    """Value object representing an email address."""
    value: str
    
    def __post_init__(self):
        if "@" not in self.value:
            raise ValueError("Invalid email address")

@dataclass(frozen=True)
class Money:
    """Value object representing monetary value."""
    amount: float
    currency: str = "USD"
    
    def __post_init__(self):
        if self.amount < 0:
            raise ValueError("Amount cannot be negative")
    
    def __add__(self, other: "Money") -> "Money":
        if self.currency != other.currency:
            raise ValueError("Cannot add different currencies")
        return Money(self.amount + other.amount, self.currency)
    
    def __mul__(self, quantity: int) -> "Money":
        return Money(self.amount * quantity, self.currency)

# ============== Domain Services ==============
class OrderDomainService:
    """Domain logic that belongs to Order but doesn't fit in the entity."""
    
    def calculate_discount(self, order: Order, discount_rate: float) -> float:
        """Calculate discount based on order total."""
        if discount_rate < 0 or discount_rate > 1:
            raise ValueError("Discount rate must be between 0 and 1")
        return order.total * discount_rate
    
    def apply_bulk_discount(self, order: Order, min_quantity: int, discount: float) -> Money:
        """Apply discount if bulk quantity threshold is met."""
        has_bulk = any(item.quantity >= min_quantity for item in order.items)
        if has_bulk:
            discount_amount = order.total * discount
            return Money(order.total - discount_amount)
        return Money(order.total)
```

🔍 **Line-by-Line Breakdown:**

1. `@dataclass` — Creates automatic `__init__`, `__repr__`, `__eq__` methods.
2. `class Order:` — Core domain entity representing an order.
3. `@property` — Computed property for derived data.
4. `def can_cancel(self) -> bool:` — Domain logic determining if order can be cancelled.
5. `@dataclass(frozen=True)` — Frozen dataclass makes object immutable (like a value).
6. `def __post_init__(self):` — Validation after object creation.
7. `raise ValueError` — Domain validation that prevents invalid state.
8. `class OrderDomainService:` — Domain service for logic that doesn't fit in entities.

## Use Cases (Application Layer)

```python
from abc import ABC, abstractmethod
from dataclasses import dataclass
from datetime import datetime
from typing import Optional

# ============== Repository Interfaces ==============
class OrderRepository(ABC):
    """Repository interface - defined in domain but implemented in infrastructure."""
    
    @abstractmethod
    async def save(self, order: Order) -> Order:
        pass
    
    @abstractmethod
    async def find_by_id(self, order_id: int) -> Optional[Order]:
        pass
    
    @abstractmethod
    async def find_by_customer(self, customer_id: int) -> list[Order]:
        pass

class CustomerRepository(ABC):
    @abstractmethod
    async def find_by_id(self, customer_id: int) -> Optional["Customer"]:
        pass

# ============== Use Cases ==============
@dataclass
class CreateOrderInput:
    customer_id: int
    items: list[dict]  # {product_id, quantity, price, name}

class CreateOrderUseCase:
    """Use case for creating orders."""
    
    def __init__(
        self,
        order_repository: OrderRepository,
        customer_repository: CustomerRepository
    ):
        self._order_repo = order_repository
        self._customer_repo = customer_repository
    
    async def execute(self, input_data: CreateOrderInput) -> Order:
        # Validate customer exists
        customer = await self._customer_repo.find_by_id(input_data.customer_id)
        if not customer:
            raise ValueError(f"Customer {input_data.customer_id} not found")
        
        # Create order items
        items = [
            OrderItem(
                product_id=item["product_id"],
                product_name=item["name"],
                quantity=item["quantity"],
                price=item["price"]
            )
            for item in input_data.items
        ]
        
        # Create order
        order = Order(
            id=0,  # Will be assigned by repository
            customer_id=input_data.customer_id,
            items=items,
            status=OrderStatus.PENDING,
            created_at=datetime.now(),
            updated_at=datetime.now()
        )
        
        # Validate order
        if not order.items:
            raise ValueError("Order must have at least one item")
        
        # Save order
        return await self._order_repo.save(order)

@dataclass
class CancelOrderInput:
    order_id: int
    reason: str

class CancelOrderUseCase:
    """Use case for cancelling orders."""
    
    def __init__(self, order_repository: OrderRepository):
        self._order_repo = order_repository
    
    async def execute(self, input_data: CancelOrderInput) -> Order:
        order = await self._order_repo.find_by_id(input_data.order_id)
        if not order:
            raise ValueError(f"Order {input_data.order_id} not found")
        
        if not order.can_cancel():
            raise ValueError(f"Cannot cancel order in status: {order.status}")
        
        order.status = OrderStatus.CANCELLED
        order.updated_at = datetime.now()
        
        return await self._order_repo.save(order)
```

🔍 **Line-by-Line Breakdown:**

1. `class OrderRepository(ABC):` — Abstract repository interface. Implementation is in infrastructure layer.
2. `@abstractmethod` — Methods that must be implemented by concrete classes.
3. `@dataclass` — Input data class for use case.
4. `class CreateOrderUseCase:` — Application service implementing business workflow.
5. `def __init__(self, order_repository: OrderRepository, ...):` — Dependencies injected via constructor.
6. `async def execute(self, input_data: CreateOrderInput) -> Order:` — Entry point for the use case.
7. `customer = await self._customer_repo.find_by_id(...)` — Fetch data through repository interface.
8. `raise ValueError(...)` — Domain validation returning errors.
9. `order = Order(...)` — Create domain entity from input data.
10. `return await self._order_repo.save(order)` — Persist through repository.

## Infrastructure Layer

```python
import asyncio
from typing import Optional
from datetime import datetime

# ============== Repository Implementations ==============
class InMemoryOrderRepository(OrderRepository):
    """In-memory implementation for testing/development."""
    
    def __init__(self):
        self._orders: dict[int, Order] = {}
        self._next_id = 1
    
    async def save(self, order: Order) -> Order:
        if order.id == 0:
            order.id = self._next_id
            self._next_id += 1
        self._orders[order.id] = order
        return order
    
    async def find_by_id(self, order_id: int) -> Optional[Order]:
        return self._orders.get(order_id)
    
    async def find_by_customer(self, customer_id: int) -> list[Order]:
        return [o for o in self._orders.values() if o.customer_id == customer_id]

@dataclass
class Customer:
    id: int
    name: str
    email: str

class InMemoryCustomerRepository(CustomerRepository):
    def __init__(self):
        self._customers: dict[int, Customer] = {}
    
    async def find_by_id(self, customer_id: int) -> Optional[Customer]:
        return self._customers.get(customer_id)
```

## Presentation Layer (FastAPI)

```python
from fastapi import FastAPI, HTTPException, Depends
from pydantic import BaseModel
from typing import List

app = FastAPI()

# ============== Dependency Injection ==============
async def get_order_repository() -> OrderRepository:
    return InMemoryOrderRepository()

async def get_customer_repository() -> CustomerRepository:
    return InMemoryCustomerRepository()

# ============== Request/Response Models ==============
class OrderItemRequest(BaseModel):
    product_id: int
    product_name: str
    quantity: int
    price: float

class CreateOrderRequest(BaseModel):
    customer_id: int
    items: List[OrderItemRequest]

class OrderResponse(BaseModel):
    id: int
    customer_id: int
    total: float
    status: str
    created_at: datetime

# ============== Routes ==============
@app.post("/orders", response_model=OrderResponse, status_code=201)
async def create_order(
    request: CreateOrderRequest,
    order_repo: OrderRepository = Depends(get_order_repository),
    customer_repo: CustomerRepository = Depends(get_customer_repository)
):
    use_case = CreateOrderOrderUseCase(order_repo, customer_repo)
    
    input_data = CreateOrderInput(
        customer_id=request.customer_id,
        items=[item.model_dump() for item in request.items]
    )
    
    try:
        order = await use_case.execute(input_data)
        return OrderResponse(
            id=order.id,
            customer_id=order.customer_id,
            total=order.total,
            status=order.status,
            created_at=order.created_at
        )
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))

@app.post("/orders/{order_id}/cancel")
async def cancel_order(
    order_id: int,
    reason: str,
    order_repo: OrderRepository = Depends(get_order_repository)
):
    use_case = CancelOrderUseCase(order_repo)
    
    try:
        order = await use_case.execute(CancelOrderInput(order_id, reason))
        return {"status": "cancelled", "order_id": order.id}
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
```

## Summary

- Clean architecture separates concerns with dependency pointing inward
- Domain layer contains pure business logic with no external dependencies
- Use cases orchestrate the flow of data to and from entities
- Repository interfaces are defined in domain, implemented in infrastructure
- This pattern makes your code testable, maintainable, and framework-independent

## Next Steps

Continue to `07-hexagonal-architecture.md` to learn about the ports and adapters pattern.
