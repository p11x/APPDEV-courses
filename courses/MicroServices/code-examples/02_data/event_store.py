"""
Event Store Implementation

This module provides an event store for storing and retrieving domain events:
- Event persistence (PostgreSQL, MySQL, or in-memory)
- Event versioning and upcasting
- Event playback for projections
- Snapshotting for aggregate roots

Usage:
    event_store = EventStore()
    
    # Append events
    await event_store.append_events("order-1", [
        OrderCreatedEvent(order_id="order-1", amount=100),
        PaymentProcessedEvent(payment_id="pay-1", order_id="order-1"),
    ])
    
    # Read stream
    events = await event_store.read_stream("order-1")
    for event in events:
        print(event)
"""

import asyncio
import json
import logging
from typing import Any, Dict, List, Optional, Sequence
from dataclasses import dataclass, field
from datetime import datetime
from enum import Enum
from abc import ABC, abstractmethod
import uuid


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


class EventStoreError(Exception):
    """Base exception for event store errors."""
    pass


@dataclass
class StoredEvent:
    """
    A stored event with metadata.
    
    Attributes:
        event_id: Unique event identifier
        aggregate_id: ID of the aggregate this event belongs to
        aggregate_type: Type of aggregate
        event_type: Type of event
        event_data: Event payload
        metadata: Additional event metadata
        version: Aggregate version after this event
        timestamp: When the event was stored
    """
    event_id: str
    aggregate_id: str
    aggregate_type: str
    event_type: str
    event_data: Dict[str, Any]
    metadata: Dict[str, Any]
    version: int
    timestamp: datetime
    
    def to_dict(self) -> Dict[str, Any]:
        """Convert to dictionary."""
        return {
            "event_id": self.event_id,
            "aggregate_id": self.aggregate_id,
            "aggregate_type": self.aggregate_type,
            "event_type": self.event_type,
            "event_data": self.event_data,
            "metadata": self.metadata,
            "version": self.version,
            "timestamp": self.timestamp.isoformat(),
        }
    
    @classmethod
    def from_dict(cls, data: Dict[str, Any]) -> "StoredEvent":
        """Create from dictionary."""
        return cls(
            event_id=data["event_id"],
            aggregate_id=data["aggregate_id"],
            aggregate_type=data["aggregate_type"],
            event_type=data["event_type"],
            event_data=data["event_data"],
            metadata=data["metadata"],
            version=data["version"],
            timestamp=datetime.fromisoformat(data["timestamp"]),
        )


class AbstractEventStore(ABC):
    """
    Abstract event store interface.
    Implement this to add support for different storage backends.
    """
    
    @abstractmethod
    async def append_events(
        self,
        aggregate_id: str,
        events: List[StoredEvent],
        expected_version: int,
    ) -> None:
        """
        Append events to an aggregate stream.
        
        Args:
            aggregate_id: ID of the aggregate
            events: Events to append
            expected_version: Expected current version (for optimistic concurrency)
            
        Raises:
            EventStoreError: If version mismatch
        """
        pass
    
    @abstractmethod
    async def read_stream(
        self,
        aggregate_id: str,
        from_version: int = 0,
    ) -> List[StoredEvent]:
        """
        Read all events for an aggregate.
        
        Args:
            aggregate_id: ID of the aggregate
            from_version: Read from this version
            
        Returns:
            List of stored events
        """
        pass
    
    @abstractmethod
    async def get_all_events(
        self,
        aggregate_type: Optional[str] = None,
        limit: int = 100,
    ) -> List[StoredEvent]:
        """
        Get all events, optionally filtered by aggregate type.
        
        Args:
            aggregate_type: Filter by aggregate type
            limit: Maximum number of events
            
        Returns:
            List of stored events
        """
        pass


class InMemoryEventStore(AbstractEventStore):
    """
    In-memory event store implementation.
    Useful for development and testing.
    """
    
    def __init__(self):
        self._events: List[StoredEvent] = []
        self._streams: Dict[str, List[StoredEvent]] = {}
    
    async def append_events(
        self,
        aggregate_id: str,
        events: List[StoredEvent],
        expected_version: int,
    ) -> None:
        # Check version
        current_version = len(self._streams.get(aggregate_id, []))
        
        if current_version != expected_version:
            raise EventStoreError(
                f"Version mismatch: expected {expected_version}, got {current_version}"
            )
        
        # Append events
        if aggregate_id not in self._streams:
            self._streams[aggregate_id] = []
        
        self._streams[aggregate_id].extend(events)
        self._events.extend(events)
        
        logger.info(
            f"Appended {len(events)} events to {aggregate_id} "
            f"(version {expected_version} -> {expected_version + len(events)})"
        )
    
    async def read_stream(
        self,
        aggregate_id: str,
        from_version: int = 0,
    ) -> List[StoredEvent]:
        events = self._streams.get(aggregate_id, [])
        return events[from_version:]
    
    async def get_all_events(
        self,
        aggregate_type: Optional[str] = None,
        limit: int = 100,
    ) -> List[StoredEvent]:
        events = self._events
        if aggregate_type:
            events = [e for e in events if e.aggregate_type == aggregate_type]
        return events[:limit]


class EventStore:
    """
    Event store with support for snapshots and event handlers.
    
    Features:
    - Event persistence
    - Snapshotting for aggregate roots
    - Event versioning
    - Projections via event handlers
    
    Usage:
        event_store = EventStore(InMemoryEventStore())
        
        # Record events for an order
        order_events = [
            create_event("OrderCreated", {"order_id": "order-1", "amount": 100}),
            create_event("OrderShipped", {"order_id": "order-1"}),
        ]
        
        await event_store.record_events("order-1", "Order", order_events)
        
        # Reconstruct aggregate
        events = await event_store.get_events("order-1")
        order = OrderAggregate.reconstruct(events)
    """
    
    def __init__(self, store: Optional[AbstractEventStore] = None):
        self.store = store or InMemoryEventStore()
        self._snapshots: Dict[str, Dict[str, Any]] = {}
        self._event_handlers: Dict[str, List[callable]] = {}
    
    def register_handler(self, event_type: str, handler: callable):
        """Register an event handler for projections."""
        if event_type not in self._event_handlers:
            self._event_handlers[event_type] = []
        self._event_handlers[event_type].append(handler)
    
    async def record_events(
        self,
        aggregate_id: str,
        aggregate_type: str,
        events: Sequence[Dict[str, Any]],
        metadata: Optional[Dict[str, Any]] = None,
    ) -> List[StoredEvent]:
        """
        Record new events for an aggregate.
        
        Args:
            aggregate_id: ID of the aggregate
            aggregate_type: Type of the aggregate
            events: List of event dictionaries
            metadata: Optional metadata for all events
            
        Returns:
            List of stored events
        """
        # Get current version
        existing_events = await self.store.read_stream(aggregate_id)
        current_version = existing_events[-1].version if existing_events else 0
        
        # Create stored events
        stored_events = []
        for i, event in enumerate(events):
            stored_event = StoredEvent(
                event_id=str(uuid.uuid4()),
                aggregate_id=aggregate_id,
                aggregate_type=aggregate_type,
                event_type=event.get("type", "Unknown"),
                event_data=event.get("data", event),
                metadata=metadata or {},
                version=current_version + i + 1,
                timestamp=datetime.utcnow(),
            )
            stored_events.append(stored_event)
        
        # Append to store
        await self.store.append_events(
            aggregate_id,
            stored_events,
            current_version,
        )
        
        # Notify handlers
        for event in stored_events:
            await self._notify_handlers(event)
        
        return stored_events
    
    async def _notify_handlers(self, event: StoredEvent):
        """Notify registered handlers of new events."""
        handlers = self._event_handlers.get(event.event_type, [])
        
        for handler in handlers:
            try:
                await handler(event)
            except Exception as e:
                logger.error(f"Handler error for {event.event_type}: {e}")
    
    async def get_events(
        self,
        aggregate_id: str,
        from_version: int = 0,
    ) -> List[StoredEvent]:
        """Get events for an aggregate."""
        return await self.store.read_stream(aggregate_id, from_version)
    
    async def get_all_events(
        self,
        aggregate_type: Optional[str] = None,
        limit: int = 100,
    ) -> List[StoredEvent]:
        """Get all events, optionally filtered by type."""
        return await self.store.get_all_events(aggregate_type, limit)
    
    async def save_snapshot(
        self,
        aggregate_id: str,
        aggregate_type: str,
        state: Dict[str, Any],
        version: int,
    ):
        """Save a snapshot of aggregate state."""
        self._snapshots[aggregate_id] = {
            "aggregate_type": aggregate_type,
            "state": state,
            "version": version,
            "timestamp": datetime.utcnow(),
        }
        logger.info(f"Saved snapshot for {aggregate_id} at version {version}")
    
    async def load_snapshot(
        self,
        aggregate_id: str,
    ) -> Optional[Dict[str, Any]]:
        """Load the most recent snapshot for an aggregate."""
        return self._snapshots.get(aggregate_id)


# Example aggregate
class OrderAggregate:
    """
    Example aggregate root with event sourcing.
    
    Usage:
        # Create new order
        order = OrderAggregate.create(amount=100)
        
        # Record events
        events = order.uncommitted_events
        
        await event_store.record_events(order.id, "Order", events)
        
        # Get existing order
        events = await event_store.get_events(order_id)
        order = OrderAggregate.reconstruct(events)
    """
    
    def __init__(self, order_id: str, amount: float, status: str = "pending"):
        self.id = order_id
        self.amount = amount
        self.status = status
        self._version = 0
        self._uncommitted_events: List[Dict[str, Any]] = []
    
    @classmethod
    def create(cls, order_id: str, amount: float) -> "OrderAggregate":
        """Create a new order."""
        order = cls(order_id, amount)
        order._apply_change(
            "OrderCreated",
            {"order_id": order_id, "amount": amount},
        )
        return order
    
    @classmethod
    def reconstruct(cls, events: List[StoredEvent]) -> "OrderAggregate":
        """Reconstruct aggregate from events."""
        if not events:
            raise ValueError("No events to reconstruct")
        
        aggregate_id = events[0].aggregate_id
        aggregate = cls.__new__(cls)
        aggregate.id = aggregate_id
        aggregate.amount = 0
        aggregate.status = "pending"
        aggregate._version = 0
        aggregate._uncommitted_events = []
        
        for event in events:
            aggregate._apply_event(event)
            aggregate._version = event.version
        
        return aggregate
    
    def _apply_change(self, event_type: str, data: Dict[str, Any]):
        """Record an uncommitted change."""
        self._uncommitted_events.append({
            "type": event_type,
            "data": data,
        })
    
    def _apply_event(self, event: StoredEvent):
        """Apply a stored event to this aggregate."""
        if event.event_type == "OrderCreated":
            self.amount = event.event_data["amount"]
            self.status = "pending"
        elif event.event_type == "PaymentProcessed":
            self.status = "paid"
        elif event.event_type == "OrderShipped":
            self.status = "shipped"
        elif event.event_type == "OrderCancelled":
            self.status = "cancelled"
    
    def process_payment(self, payment_id: str):
        """Process payment for the order."""
        if self.status != "pending":
            raise ValueError(f"Cannot process payment for order in status {self.status}")
        
        self._apply_change(
            "PaymentProcessed",
            {"payment_id": payment_id, "order_id": self.id, "amount": self.amount},
        )
    
    def ship(self):
        """Ship the order."""
        if self.status != "paid":
            raise ValueError(f"Cannot ship order in status {self.status}")
        
        self._apply_change(
            "OrderShipped",
            {"order_id": self.id},
        )
    
    def cancel(self):
        """Cancel the order."""
        if self.status == "shipped":
            raise ValueError("Cannot cancel shipped order")
        
        self._apply_change(
            "OrderCancelled",
            {"order_id": self.id, "reason": "cancelled by customer"},
        )
    
    @property
    def uncommitted_events(self) -> List[Dict[str, Any]]:
        """Get uncommitted events and clear them."""
        events = self._uncommitted_events
        self._uncommitted_events = []
        return events


# Example usage
async def main():
    """Demonstrate event store usage."""
    
    store = InMemoryEventStore()
    event_store = EventStore(store)
    
    # Create an order
    order = OrderAggregate.create("order-123", 99.99)
    order.process_payment("pay-001")
    
    # Record events
    events = order.uncommitted_events
    await event_store.record_events(order.id, "Order", events)
    
    # Get events
    stored_events = await event_store.get_events("order-123")
    print(f"Retrieved {len(stored_events)} events:")
    for event in stored_events:
        print(f"  - {event.event_type} v{event.version}")
    
    # Reconstruct aggregate
    reconstructed = OrderAggregate.reconstruct(stored_events)
    print(f"\nReconstructed order: ID={reconstructed.id}, Status={reconstructed.status}, Amount={reconstructed.amount}")


if __name__ == "__main__":
    asyncio.run(main())