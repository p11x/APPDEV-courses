"""
CQRS Command/Query Handlers Implementation

This module provides CQRS (Command Query Responsibility Segregation) pattern:
- Command handlers for write operations
- Query handlers for read operations
- Event sourcing support
- Separate read/write models

Usage:
    # Commands
    command_bus = CommandBus()
    command_bus.register(CreateOrderCommand, CreateOrderHandler())
    
    await command_bus.execute(CreateOrderCommand(order_id="123", amount=100))
    
    # Queries
    query_bus = QueryBus()
    query_bus.register(GetOrderQuery, GetOrderHandler())
    
    order = await query_bus.execute(GetOrderQuery(order_id="123"))
"""

import asyncio
import logging
from typing import Any, Dict, Generic, List, Optional, Type, TypeVar, Callable
from dataclasses import dataclass, field
from abc import ABC, abstractmethod
from datetime import datetime
import uuid


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


# Type definitions
TCommand = TypeVar("TCommand")
TQuery = TypeVar("TQuery")
TResult = TypeVar("TResult")


# Base classes
class Command(ABC):
    """Base class for all commands."""
    pass


class Query(ABC):
    """Base class for all queries."""
    pass


class Result(ABC):
    """Base class for command/query results."""
    pass


# Command Handlers
class CommandHandler(ABC, Generic[TCommand, TResult]):
    """
    Abstract command handler interface.
    
    Commands modify state and should return a result.
    """
    
    @abstractmethod
    async def handle(self, command: TCommand) -> TResult:
        """Handle a command and return result."""
        pass


class CommandBus:
    """
    Command bus for dispatching commands to handlers.
    
    The command bus is responsible for:
    - Registering command handlers
    - Validating commands
    - Dispatching commands to appropriate handlers
    
    Usage:
        bus = CommandBus()
        bus.register(CreateOrderCommand, CreateOrderHandler())
        
        result = await bus.execute(CreateOrderCommand(...))
    """
    
    def __init__(self):
        self._handlers: Dict[Type[Command], CommandHandler] = {}
        self._middleware: List[Callable] = []
    
    def register(self, command_type: Type[TCommand], handler: CommandHandler):
        """Register a handler for a command type."""
        self._handlers[command_type] = handler
        logger.info(f"Registered handler for {command_type.__name__}")
    
    def add_middleware(self, middleware: Callable):
        """Add middleware to the command bus."""
        self._middleware.append(middleware)
    
    async def execute(self, command: Command) -> Any:
        """Execute a command through the bus."""
        command_type = type(command)
        
        if command_type not in self._handlers:
            raise ValueError(f"No handler registered for {command_type.__name__}")
        
        # Apply middleware
        handler = self._handlers[command_type]
        
        for middleware in self._middleware:
            command = await middleware(command)
        
        return await handler.handle(command)


# Query Handlers
class QueryHandler(ABC, Generic[TQuery, TResult]):
    """
    Abstract query handler interface.
    
    Queries read data and should not modify state.
    """
    
    @abstractmethod
    async def handle(self, query: TQuery) -> TResult:
        """Handle a query and return result."""
        pass


class QueryBus:
    """
    Query bus for dispatching queries to handlers.
    
    The query bus separates read operations from write operations,
    allowing for optimized read models.
    
    Usage:
        bus = QueryBus()
        bus.register(GetOrderQuery, GetOrderHandler())
        
        order = await bus.execute(GetOrderQuery(order_id="123"))
    """
    
    def __init__(self):
        self._handlers: Dict[Type[Query], QueryHandler] = {}
    
    def register(self, query_type: Type[TQuery], handler: QueryHandler):
        """Register a handler for a query type."""
        self._handlers[query_type] = handler
        logger.info(f"Registered handler for {query_type.__name__}")
    
    async def execute(self, query: Query) -> Any:
        """Execute a query through the bus."""
        query_type = type(query)
        
        if query_type not in self._handlers:
            raise ValueError(f"No handler registered for {query_type.__name__}")
        
        handler = self._handlers[query_type]
        return await handler.handle(query)


# Example Command definitions
@dataclass
class CreateOrderCommand(Command):
    """Command to create a new order."""
    order_id: str
    customer_id: str
    amount: float
    items: List[Dict[str, Any]]


@dataclass
class UpdateOrderStatusCommand(Command):
    """Command to update order status."""
    order_id: str
    new_status: str


@dataclass
class CancelOrderCommand(Command):
    """Command to cancel an order."""
    order_id: str
    reason: str


# Example Query definitions
@dataclass
class GetOrderQuery(Query):
    """Query to get order details."""
    order_id: str


@dataclass
class GetCustomerOrdersQuery(Query):
    """Query to get all orders for a customer."""
    customer_id: str
    limit: int = 10


@dataclass
class GetOrderListQuery(Query):
    """Query to get a list of orders with filters."""
    status: Optional[str] = None
    customer_id: Optional[str] = None
    limit: int = 100
    offset: int = 0


# Example Read Models (Projections)
@dataclass
class OrderReadModel:
    """Read model for order details."""
    order_id: str
    customer_id: str
    amount: float
    status: str
    items: List[Dict[str, Any]]
    created_at: datetime
    updated_at: datetime


@dataclass
class OrderSummary:
    """Summary view of an order."""
    order_id: str
    customer_id: str
    amount: float
    status: str
    created_at: datetime


# Example Data Store (simulated)
class DataStore:
    """
    Simulated data store for demonstration.
    In production, this would be connected to a database.
    """
    
    def __init__(self):
        # Write model (command side)
        self._orders: Dict[str, Dict[str, Any]] = {}
        
        # Read models (query side) - denormalized for fast reads
        self._orders_by_id: Dict[str, OrderReadModel] = {}
        self._orders_by_customer: Dict[str, List[str]] = {}
        self._orders_by_status: Dict[str, List[str]] = {}
    
    # Command side methods
    async def save_order(self, order_data: Dict[str, Any]):
        """Save order to write store."""
        self._orders[order_data["order_id"]] = order_data
    
    async def update_order(self, order_id: str, updates: Dict[str, Any]):
        """Update order in write store."""
        if order_id in self._orders:
            self._orders[order_id].update(updates)
    
    async def get_order(self, order_id: str) -> Optional[Dict[str, Any]]:
        """Get order from write store."""
        return self._orders.get(order_id)
    
    # Query side methods (projections)
    async def save_order_projection(self, order: OrderReadModel):
        """Save order to read store (projection)."""
        self._orders_by_id[order.order_id] = order
        
        # Update indexes
        if order.customer_id not in self._orders_by_customer:
            self._orders_by_customer[order.customer_id] = []
        self._orders_by_customer[order.customer_id].append(order.order_id)
        
        if order.status not in self._orders_by_status:
            self._orders_by_status[order.status] = []
        self._orders_by_status[order.status].append(order.order_id)
    
    async def get_order_projection(self, order_id: str) -> Optional[OrderReadModel]:
        """Get order from read store."""
        return self._orders_by_id.get(order_id)
    
    async def get_customer_order_ids(self, customer_id: str) -> List[str]:
        """Get order IDs for a customer."""
        return self._orders_by_customer.get(customer_id, [])
    
    async def get_orders_by_status(self, status: str) -> List[str]:
        """Get order IDs by status."""
        return self._orders_by_status.get(status, [])
    
    async def list_orders(
        self,
        status: Optional[str] = None,
        customer_id: Optional[str] = None,
        limit: int = 100,
        offset: int = 0,
    ) -> List[OrderReadModel]:
        """List orders with filters."""
        # Start with all orders
        order_ids = set(self._orders_by_id.keys())
        
        # Apply filters
        if status:
            status_orders = set(self._orders_by_status.get(status, []))
            order_ids &= status_orders
        
        if customer_id:
            customer_orders = set(self._orders_by_customer.get(customer_id, []))
            order_ids &= customer_orders
        
        # Get order objects
        orders = [
            self._orders_by_id[oid]
            for oid in list(order_ids)[offset:offset + limit]
        ]
        
        return sorted(orders, key=lambda x: x.created_at, reverse=True)


# Create global data store
data_store = DataStore()


# Command Handlers
class CreateOrderHandler(CommandHandler[CreateOrderCommand, OrderReadModel]):
    """Handler for creating orders."""
    
    async def handle(self, command: CreateOrderCommand) -> OrderReadModel:
        """Handle create order command."""
        logger.info(f"Creating order: {command.order_id}")
        
        # Create order data
        now = datetime.utcnow()
        order_data = {
            "order_id": command.order_id,
            "customer_id": command.customer_id,
            "amount": command.amount,
            "status": "pending",
            "items": command.items,
            "created_at": now,
            "updated_at": now,
        }
        
        # Save to write store
        await data_store.save_order(order_data)
        
        # Create read model
        order = OrderReadModel(
            order_id=command.order_id,
            customer_id=command.customer_id,
            amount=command.amount,
            status="pending",
            items=command.items,
            created_at=now,
            updated_at=now,
        )
        
        # Save projection
        await data_store.save_order_projection(order)
        
        logger.info(f"Order created: {command.order_id}")
        return order


class UpdateOrderStatusHandler(CommandHandler[UpdateOrderStatusCommand, OrderReadModel]):
    """Handler for updating order status."""
    
    async def handle(self, command: UpdateOrderStatusCommand) -> OrderReadModel:
        """Handle update order status command."""
        logger.info(f"Updating order status: {command.order_id} -> {command.new_status}")
        
        # Update write store
        await data_store.update_order(
            command.order_id,
            {"status": command.new_status, "updated_at": datetime.utcnow()}
        )
        
        # Get updated order
        order = await data_store.get_order_projection(command.order_id)
        
        if not order:
            raise ValueError(f"Order not found: {command.order_id}")
        
        # Update read model
        order.status = command.new_status
        order.updated_at = datetime.utcnow()
        
        await data_store.save_order_projection(order)
        
        return order


class CancelOrderHandler(CommandHandler[CancelOrderCommand, OrderReadModel]):
    """Handler for cancelling orders."""
    
    async def handle(self, command: CancelOrderCommand) -> OrderReadModel:
        """Handle cancel order command."""
        logger.info(f"Cancelling order: {command.order_id}")
        
        # Get current order
        order = await data_store.get_order_projection(command.order_id)
        
        if not order:
            raise ValueError(f"Order not found: {command.order_id}")
        
        if order.status in ["shipped", "delivered"]:
            raise ValueError(f"Cannot cancel order in status: {order.status}")
        
        # Update status
        order.status = "cancelled"
        order.updated_at = datetime.utcnow()
        
        # Update both stores
        await data_store.update_order(
            command.order_id,
            {"status": "cancelled", "cancel_reason": command.reason, "updated_at": datetime.utcnow()}
        )
        
        await data_store.save_order_projection(order)
        
        logger.info(f"Order cancelled: {command.order_id}")
        return order


# Query Handlers
class GetOrderHandler(QueryHandler[GetOrderQuery, Optional[OrderReadModel]]):
    """Handler for getting order by ID."""
    
    async def handle(self, query: GetOrderQuery) -> Optional[OrderReadModel]:
        """Handle get order query."""
        return await data_store.get_order_projection(query.order_id)


class GetCustomerOrdersHandler(QueryHandler[GetCustomerOrdersQuery, List[OrderSummary]]):
    """Handler for getting customer orders."""
    
    async def handle(self, query: GetCustomerOrdersQuery) -> List[OrderSummary]:
        """Handle get customer orders query."""
        order_ids = await data_store.get_customer_order_ids(query.customer_id)
        
        orders = []
        for order_id in order_ids[:query.limit]:
            order = await data_store.get_order_projection(order_id)
            if order:
                orders.append(OrderSummary(
                    order_id=order.order_id,
                    customer_id=order.customer_id,
                    amount=order.amount,
                    status=order.status,
                    created_at=order.created_at,
                ))
        
        return orders


class GetOrderListHandler(QueryHandler[GetOrderListQuery, List[OrderSummary]]):
    """Handler for getting order list with filters."""
    
    async def handle(self, query: GetOrderListQuery) -> List[OrderSummary]:
        """Handle get order list query."""
        orders = await data_store.list_orders(
            status=query.status,
            customer_id=query.customer_id,
            limit=query.limit,
            offset=query.offset,
        )
        
        return [
            OrderSummary(
                order_id=order.order_id,
                customer_id=order.customer_id,
                amount=order.amount,
                status=order.status,
                created_at=order.created_at,
            )
            for order in orders
        ]


# Example CQRS implementation setup
def setup_cqrs() -> tuple:
    """
    Set up CQRS with command and query buses.
    
    Returns:
        Tuple of (command_bus, query_bus)
    """
    command_bus = CommandBus()
    query_bus = QueryBus()
    
    # Register command handlers
    command_bus.register(CreateOrderCommand, CreateOrderHandler())
    command_bus.register(UpdateOrderStatusCommand, UpdateOrderStatusHandler())
    command_bus.register(CancelOrderCommand, CancelOrderHandler())
    
    # Register query handlers
    query_bus.register(GetOrderQuery, GetOrderHandler())
    query_bus.register(GetCustomerOrdersQuery, GetCustomerOrdersHandler())
    query_bus.register(GetOrderListQuery, GetOrderListHandler())
    
    return command_bus, query_bus


# Example usage
async def main():
    """Demonstrate CQRS pattern."""
    
    command_bus, query_bus = setup_cqrs()
    
    # Create an order via command
    order = await command_bus.execute(CreateOrderCommand(
        order_id="order-001",
        customer_id="customer-123",
        amount=99.99,
        items=[{"product_id": "prod-1", "quantity": 2}],
    ))
    print(f"Created order: {order.order_id}, Status: {order.status}")
    
    # Update order status via command
    order = await command_bus.execute(UpdateOrderStatusCommand(
        order_id="order-001",
        new_status="processing",
    ))
    print(f"Updated order status: {order.status}")
    
    # Query order
    fetched_order = await query_bus.execute(GetOrderQuery(order_id="order-001"))
    print(f"Queried order: {fetched_order.order_id}, Amount: {fetched_order.amount}")
    
    # Query customer orders
    orders = await query_bus.execute(GetCustomerOrdersQuery(
        customer_id="customer-123",
        limit=10,
    ))
    print(f"Customer orders: {len(orders)}")
    
    # Query order list
    all_orders = await query_bus.execute(GetOrderListQuery(
        status="processing",
        limit=100,
    ))
    print(f"Processing orders: {len(all_orders)}")


if __name__ == "__main__":
    asyncio.run(main())