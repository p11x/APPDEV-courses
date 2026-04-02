# Enterprise Architecture

## Overview

Enterprise patterns provide scalable, maintainable architectures for large-scale FastAPI applications.

## Layered Architecture

### Clean Architecture Implementation

```python
# Example 1: Clean Architecture layers
"""
Layers:
1. Presentation (API Routes)
2. Application (Use Cases)
3. Domain (Business Logic)
4. Infrastructure (Database, External Services)
"""

# app/domain/entities/user.py
from dataclasses import dataclass
from datetime import datetime

@dataclass
class User:
    """Domain entity"""
    id: int
    username: str
    email: str
    created_at: datetime
    is_active: bool = True

    def deactivate(self):
        """Business logic in domain"""
        if not self.is_active:
            raise ValueError("User already deactivated")
        self.is_active = False

# app/domain/repositories/user_repository.py
from abc import ABC, abstractmethod
from typing import Optional

class UserRepository(ABC):
    """Repository interface (domain layer)"""

    @abstractmethod
    async def get_by_id(self, user_id: int) -> Optional[User]:
        pass

    @abstractmethod
    async def save(self, user: User) -> User:
        pass

# app/infrastructure/repositories/sqlalchemy_user_repository.py
from app.domain.repositories import UserRepository
from app.domain.entities import User

class SqlAlchemyUserRepository(UserRepository):
    """Repository implementation (infrastructure layer)"""

    def __init__(self, session: AsyncSession):
        self.session = session

    async def get_by_id(self, user_id: int) -> Optional[User]:
        result = await self.session.execute(
            select(UserModel).where(UserModel.id == user_id)
        )
        model = result.scalar_one_or_none()
        return self._to_entity(model) if model else None

    async def save(self, user: User) -> User:
        model = self._to_model(user)
        self.session.add(model)
        await self.session.commit()
        return self._to_entity(model)

# app/application/use_cases/create_user.py
from app.domain.entities import User
from app.domain.repositories import UserRepository

class CreateUserUseCase:
    """Application use case"""

    def __init__(self, user_repo: UserRepository):
        self.user_repo = user_repo

    async def execute(self, username: str, email: str) -> User:
        # Business rules
        if len(username) < 3:
            raise ValueError("Username too short")

        user = User(
            id=0,
            username=username,
            email=email,
            created_at=datetime.utcnow()
        )

        return await self.user_repo.save(user)

# app/presentation/api/routes/users.py
from fastapi import APIRouter, Depends
from app.application.use_cases import CreateUserUseCase

router = APIRouter(prefix="/users")

@router.post("/")
async def create_user(
    data: UserCreate,
    use_case: CreateUserUseCase = Depends()
):
    """Presentation layer - thin controller"""
    user = await use_case.execute(data.username, data.email)
    return UserResponse.from_entity(user)
```

## Domain-Driven Design

### DDD Implementation

```python
# Example 2: DDD with FastAPI
from dataclasses import dataclass, field
from typing import List
from datetime import datetime

# Value Objects
@dataclass(frozen=True)
class Email:
    value: str

    def __post_init__(self):
        if "@" not in self.value:
            raise ValueError("Invalid email")

@dataclass(frozen=True)
class Money:
    amount: float
    currency: str = "USD"

    def __post_init__(self):
        if self.amount < 0:
            raise ValueError("Amount cannot be negative")

    def add(self, other: 'Money') -> 'Money':
        if self.currency != other.currency:
            raise ValueError("Currency mismatch")
        return Money(self.amount + other.amount, self.currency)

# Aggregate Root
@dataclass
class Order:
    id: int
    customer_email: Email
    items: List['OrderItem'] = field(default_factory=list)
    status: str = "pending"
    created_at: datetime = field(default_factory=datetime.utcnow)

    @property
    def total(self) -> Money:
        """Calculate order total"""
        total = Money(0)
        for item in self.items:
            total = total.add(item.subtotal)
        return total

    def add_item(self, product_id: int, quantity: int, price: Money):
        """Business logic for adding items"""
        if self.status != "pending":
            raise ValueError("Cannot modify confirmed order")

        item = OrderItem(product_id=product_id, quantity=quantity, price=price)
        self.items.append(item)

    def confirm(self):
        """Confirm order"""
        if not self.items:
            raise ValueError("Cannot confirm empty order")
        self.status = "confirmed"

# Domain Events
@dataclass
class OrderConfirmed:
    order_id: int
    total: Money
    occurred_at: datetime = field(default_factory=datetime.utcnow)

# Repository with events
class OrderRepository:
    def __init__(self, session, event_bus):
        self.session = session
        self.event_bus = event_bus

    async def save(self, order: Order):
        # Save to database
        # Publish domain events
        await self.event_bus.publish(OrderConfirmed(
            order_id=order.id,
            total=order.total
        ))
```

## Summary

Enterprise patterns provide structure for complex applications.

## Next Steps

Continue learning about:
- [Microservices Communication](./02_microservices_communication.md)
- [API Gateway Patterns](./03_api_gateway_patterns.md)
