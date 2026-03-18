# Event-Driven Architecture

## What You'll Learn

- Event-driven principles
- Event sourcing and CQRS
- Message brokers
- Building reactive systems

## Prerequisites

- Understanding of microservices communication

## Introduction

Event-driven architecture (EDA) is a pattern where services communicate by producing and consuming events rather than making direct synchronous calls. An event is a record of something that happened—like "order placed," "user registered," or "payment processed."

Imagine a newspaper delivery system. Instead of each person calling the newspaper office to ask "any news today?", the newspaper delivers papers to everyone who's interested. If you want news, you subscribe. This is exactly how event-driven architecture works: components announce events, and interested parties react.

## Core Concepts

### Events vs Commands

- **Events** are notifications about something that already happened. They're immutable and typically named in past tense (OrderShipped, UserCreated).
- **Commands** are requests for something to happen. They're imperative and expect a response (CreateOrder, UpdateUser).

```python
from dataclasses import dataclass
from datetime import datetime
from typing import Literal

# Events - something that happened
@dataclass
class OrderPlaced:
    order_id: int
    user_id: int
    items: list[dict]
    total: float
    timestamp: datetime

@dataclass
class PaymentProcessed:
    order_id: int
    amount: float
    status: Literal["success", "failed"]
    timestamp: datetime

# Commands - something to do
@dataclass
class CreateOrder:
    user_id: int
    items: list[dict]

@dataclass
class ProcessPayment:
    order_id: int
    amount: float
    payment_method: str
```

🔍 **Line-by-Line Breakdown:**

1. `from dataclasses import dataclass` — Import dataclass decorator for creating data carrier classes.
2. `from datetime import datetime` — Import datetime for timestamp tracking.
3. `from typing import Literal` — Import Literal type hint for strict string values.
4. `@dataclass` — Decorator that generates `__init__`, `__repr__`, `__eq__` methods automatically.
5. `class OrderPlaced:` — Event class representing an order that was placed.
6. `order_id: int` — Unique identifier for the order.
7. `user_id: int` — ID of the user who placed the order.
8. `items: list[dict]` — List of items in the order.
9. `total: float` — Total order amount.
10. `timestamp: datetime` — When the event occurred.
11. `Literal["success", "failed"]` — Type hint restricting status to these exact values.

## Event Consumers

```python
import asyncio
from abc import ABC, abstractmethod
from dataclasses import dataclass
from datetime import datetime
from typing import Any, Callable

@dataclass
class Event:
    event_type: str
    data: dict[str, Any]
    timestamp: datetime

class EventConsumer(ABC):
    """Abstract base class for event consumers."""
    
    @abstractmethod
    async def handle(self, event: Event) -> None:
        """Handle an incoming event."""
        pass

class NotificationConsumer(EventConsumer):
    """Consumer that sends notifications based on events."""
    
    async def handle(self, event: Event) -> None:
        if event.event_type == "order_placed":
            await self._send_order_confirmation(event.data)
        elif event.event_type == "payment_failed":
            await self._send_payment_alert(event.data)
    
    async def _send_order_confirmation(self, data: dict) -> None:
        print(f"Sending order confirmation for order {data['order_id']}")
        await asyncio.sleep(0.1)  # Simulate sending email
    
    async def _send_payment_alert(self, data: dict) -> None:
        print(f"ALERT: Payment failed for order {data['order_id']}")
        await asyncio.sleep(0.1)  # Simulate sending alert

class InventoryConsumer(EventConsumer):
    """Consumer that manages inventory based on orders."""
    
    async def handle(self, event: Event) -> None:
        if event.event_type == "order_placed":
            await self._reserve_inventory(event.data)
    
    async def _reserve_inventory(self, data: dict) -> None:
        print(f"Reserving inventory for order {data['order_id']}")
        await asyncio.sleep(0.1)  # Simulate inventory reservation

# Simple event bus
class EventBus:
    def __init__(self):
        self._consumers: list[EventConsumer] = []
    
    def subscribe(self, consumer: EventConsumer) -> None:
        self._consumers.append(consumer)
    
    async def publish(self, event: Event) -> None:
        # Fan out to all consumers
        await asyncio.gather(
            *[consumer.handle(event) for consumer in self._consumers],
            return_exceptions=True
        )

# Example usage
async def main():
    event_bus = EventBus()
    event_bus.subscribe(NotificationConsumer())
    event_bus.subscribe(InventoryConsumer())
    
    # Publish an order placed event
    event = Event(
        event_type="order_placed",
        data={
            "order_id": 12345,
            "user_id": 42,
            "items": [{"product_id": 1, "quantity": 2}],
            "total": 99.98
        },
        timestamp=datetime.now()
    )
    
    await event_bus.publish(event)

asyncio.run(main())
```

🔍 **Line-by-Line Breakdown:**

1. `from abc import ABC, abstractmethod` — Import abstract base class for defining interfaces.
2. `class EventConsumer(ABC):` — Abstract base class defining the consumer interface.
3. `@abstractmethod` — Decorator marking handle() as must-be-implemented by subclasses.
4. `async def handle(self, event: Event) -> None:` — Async method all consumers must implement.
5. `class NotificationConsumer(EventConsumer):` — Concrete consumer for sending notifications.
6. `if event.event_type == "order_placed":` — Check the event type to determine action.
7. `await self._send_order_confirmation(event.data)` — Call internal method to send confirmation.
8. `class InventoryConsumer(EventConsumer):` — Another consumer for inventory management.
9. `class EventBus:` — Simple event distribution system.
10. `self._consumers: list[EventConsumer] = []` — List to hold registered consumers.
11. `def subscribe(self, consumer: EventConsumer) -> None:` — Method to register a consumer.
12. `async def publish(self, event: Event) -> None:` — Publish event to all consumers.
13. `await asyncio.gather(...)` — Run all consumer handlers concurrently.

## Event Sourcing

Event sourcing stores the complete history of state changes as a sequence of events instead of storing just the current state:

```python
from dataclasses import dataclass, field
from datetime import datetime
from typing import Any, Callable
from enum import Enum

class EventType(Enum):
    ACCOUNT_CREATED = "account_created"
    DEPOSIT = "deposit"
    WITHDRAWAL = "withdrawal"
    TRANSFER = "transfer"

@dataclass
class AccountEvent:
    event_type: EventType
    timestamp: datetime
    data: dict[str, Any]

class BankAccount:
    """Bank account using event sourcing pattern."""
    
    def __init__(self, account_id: int, initial_balance: float = 0.0):
        self.account_id = account_id
        self._balance = initial_balance
        self._event_history: list[AccountEvent] = []
    
    @property
    def balance(self) -> float:
        return self._balance
    
    def get_history(self) -> list[AccountEvent]:
        return self._event_history.copy()
    
    def deposit(self, amount: float) -> None:
        if amount <= 0:
            raise ValueError("Deposit amount must be positive")
        
        self._balance += amount
        self._event_history.append(AccountEvent(
            event_type=EventType.DEPOSIT,
            timestamp=datetime.now(),
            data={"amount": amount, "new_balance": self._balance}
        ))
    
    def withdraw(self, amount: float) -> None:
        if amount <= 0:
            raise ValueError("Withdrawal amount must be positive")
        if amount > self._balance:
            raise ValueError("Insufficient funds")
        
        self._balance -= amount
        self._event_history.append(AccountEvent(
            event_type=EventType.WITHDRAWAL,
            timestamp=datetime.now(),
            data={"amount": amount, "new_balance": self._balance}
        ))
    
    @classmethod
    def replay_events(cls, account_id: int, events: list[AccountEvent]) -> "BankAccount":
        """Reconstruct account state from event history."""
        account = cls(account_id)
        
        for event in events:
            match event.event_type:
                case EventType.ACCOUNT_CREATED:
                    account._balance = event.data.get("initial_balance", 0.0)
                case EventType.DEPOSIT:
                    account._balance += event.data["amount"]
                case EventType.WITHDRAWAL:
                    account._balance -= event.data["amount"]
        
        account._event_history = events
        return account

# Example usage
async def main():
    # Create account and perform operations
    account = BankAccount(account_id=1, initial_balance=100.0)
    account.deposit(50.0)
    account.withdraw(25.0)
    account.deposit(100.0)
    
    print(f"Current balance: ${account.balance}")
    print(f"Event history length: {len(account.get_history())}")
    
    # Reconstruct from history (e.g., after system restart)
    reconstructed = BankAccount.replay_events(
        account_id=1,
        events=account.get_history()
    )
    print(f"Reconstructed balance: ${reconstructed.balance}")

asyncio.run(main())
```

🔍 **Line-by-Line Breakdown:**

1. `from dataclasses import dataclass, field` — Import dataclass and field for data classes.
2. `from enum import Enum` — Import Enum for type-safe constants.
3. `class EventType(Enum):` — Enum defining valid event types for accounts.
4. `@dataclass` — Dataclass for representing events.
5. `class BankAccount:` — Bank account using event sourcing.
6. `self._event_history: list[AccountEvent] = []` — Store complete history of all events.
7. `def deposit(self, amount: float) -> None:` — Deposit method that creates an event.
8. `self._balance += amount` — Update current state.
9. `self._event_history.append(AccountEvent(...))` — Append event to history.
10. `@classmethod` — Class method that can be called on the class itself.
11. `def replay_events(cls, account_id: int, events: list[AccountEvent])` — Reconstruct state from events.
12. `match event.event_type:` — Python 3.10+ pattern matching for event types.

## Summary

- Event-driven architecture decouples producers from consumers through events
- Events represent things that happened; commands represent things to do
- Event sourcing stores complete history instead of just current state
- Benefits include loose coupling, scalability, and auditability
- Challenges include eventual consistency and debugging complexity

## Next Steps

Continue to `05-layered-architecture.md` to learn about traditional layered architecture patterns.
