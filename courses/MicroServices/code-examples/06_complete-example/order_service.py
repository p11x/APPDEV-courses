"""
Order Service - FastAPI Microservice

This service provides order management functionality:
- Create and manage orders
- Message queue integration for async processing
- Order status tracking

Usage:
    uvicorn order_service:app --host 0.0.0.0 --port 8002
"""

import logging
import os
import json
from typing import List, Optional
from dataclasses import dataclass
from datetime import datetime
from enum import Enum

from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import aio_pika


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(title="Order Service", version="1.0.0")


# =============================================================================
# MODELS
# =============================================================================

class OrderStatus(str, Enum):
    """Order status enum."""
    PENDING = "pending"
    CONFIRMED = "confirmed"
    PROCESSING = "processing"
    SHIPPED = "shipped"
    DELIVERED = "delivered"
    CANCELLED = "cancelled"


class OrderItemCreate(BaseModel):
    """Item in an order."""
    product_id: str
    quantity: int
    price: float


class OrderCreate(BaseModel):
    """Request model for creating an order."""
    customer_id: str
    items: List[OrderItemCreate]
    shipping_address: str


class OrderItemResponse(BaseModel):
    """Response for an order item."""
    product_id: str
    quantity: int
    price: float


class OrderResponse(BaseModel):
    """Response model for order."""
    id: str
    customer_id: str
    items: List[OrderItemResponse]
    total: float
    status: OrderStatus
    shipping_address: str
    created_at: datetime
    updated_at: datetime


# =============================================================================
# DATABASE (In-memory for demo)
# =============================================================================

class OrderDatabase:
    """In-memory order database."""
    
    def __init__(self):
        self._orders = {}
        self._id_counter = 0
    
    def _generate_id(self) -> str:
        self._id_counter += 1
        return f"order-{self._id_counter:05d}"
    
    def create_order(
        self,
        customer_id: str,
        items: List[OrderItemCreate],
        shipping_address: str,
    ) -> OrderResponse:
        """Create a new order."""
        order_id = self._generate_id()
        now = datetime.utcnow()
        
        # Calculate total
        total = sum(item.price * item.quantity for item in items)
        
        order = OrderResponse(
            id=order_id,
            customer_id=customer_id,
            items=[OrderItemResponse(
                product_id=item.product_id,
                quantity=item.quantity,
                price=item.price,
            ) for item in items],
            total=total,
            status=OrderStatus.PENDING,
            shipping_address=shipping_address,
            created_at=now,
            updated_at=now,
        )
        
        self._orders[order_id] = order
        return order
    
    def get_order(self, order_id: str) -> Optional[OrderResponse]:
        """Get an order by ID."""
        return self._orders.get(order_id)
    
    def list_orders(
        self,
        customer_id: Optional[str] = None,
        status: Optional[OrderStatus] = None,
        limit: int = 100,
        offset: int = 0,
    ) -> List[OrderResponse]:
        """List orders with optional filtering."""
        orders = list(self._orders.values())
        
        if customer_id:
            orders = [o for o in orders if o.customer_id == customer_id]
        
        if status:
            orders = [o for o in orders if o.status == status]
        
        # Sort by created_at descending
        orders.sort(key=lambda x: x.created_at, reverse=True)
        
        return orders[offset:offset + limit]
    
    def update_status(
        self,
        order_id: str,
        status: OrderStatus,
    ) -> Optional[OrderResponse]:
        """Update order status."""
        order = self._orders.get(order_id)
        
        if not order:
            return None
        
        order.status = status
        order.updated_at = datetime.utcnow()
        
        return order


# Initialize database
order_db = OrderDatabase()


# =============================================================================
# MESSAGE QUEUE
# =============================================================================

rabbitmq_connection = None
rabbitmq_channel = None


async def get_rabbitmq_channel():
    """Get RabbitMQ channel."""
    global rabbitmq_connection, rabbitmq_channel
    
    if rabbitmq_channel is None:
        rabbitmq_url = os.getenv("RABBITMQ_URL", "amqp://guest:guest@localhost:5672")
        
        try:
            rabbitmq_connection = await aio_pika.connect_robust(rabbitmq_url)
            rabbitmq_channel = await rabbitmq_connection.channel()
            
            # Declare exchange
            exchange = await rabbitmq_channel.declare_exchange(
                "orders",
                aio_pika.ExchangeType.DIRECT,
                durable=True,
            )
            
            logger.info("Connected to RabbitMQ")
            
        except Exception as e:
            logger.warning(f"RabbitMQ connection failed: {e}")
            return None
    
    return rabbitmq_channel


async def publish_order_event(order_id: str, event_type: str):
    """Publish order event to message queue."""
    try:
        channel = await get_rabbitmq_channel()
        
        if channel is None:
            logger.warning("Cannot publish event - no RabbitMQ channel")
            return
        
        # Create message
        message = aio_pika.Message(
            body=json.dumps({
                "order_id": order_id,
                "event_type": event_type,
                "timestamp": datetime.utcnow().isoformat(),
            }).encode(),
            content_type="application/json",
        )
        
        # Publish
        await channel.default_exchange.publish(
            message,
            routing_key="order_events",
        )
        
        logger.info(f"Published {event_type} event for order {order_id}")
        
    except Exception as e:
        logger.error(f"Failed to publish event: {e}")


# =============================================================================
# ROUTES
# =============================================================================

@app.get("/health")
async def health_check():
    """Health check endpoint."""
    return {"status": "healthy", "service": "order-service"}


@app.get("/")
async def root():
    """Root endpoint."""
    return {"service": "Order Service", "version": "1.0.0"}


@app.post("/orders", response_model=OrderResponse, status_code=201)
async def create_order(order: OrderCreate):
    """Create a new order."""
    # Create order
    new_order = order_db.create_order(
        customer_id=order.customer_id,
        items=order.items,
        shipping_address=order.shipping_address,
    )
    
    # Publish event
    await publish_order_event(new_order.id, "order_created")
    
    return new_order


@app.get("/orders", response_model=List[OrderResponse])
async def list_orders(
    customer_id: Optional[str] = None,
    status: Optional[OrderStatus] = None,
    limit: int = 100,
    offset: int = 0,
):
    """List all orders."""
    return order_db.list_orders(
        customer_id=customer_id,
        status=status,
        limit=limit,
        offset=offset,
    )


@app.get("/orders/{order_id}", response_model=OrderResponse)
async def get_order(order_id: str):
    """Get an order by ID."""
    order = order_db.get_order(order_id)
    
    if not order:
        raise HTTPException(status_code=404, detail="Order not found")
    
    return order


@app.patch("/orders/{order_id}/status", response_model=OrderResponse)
async def update_order_status(order_id: str, status: OrderStatus):
    """Update order status."""
    order = order_db.update_status(order_id, status)
    
    if not order:
        raise HTTPException(status_code=404, detail="Order not found")
    
    # Publish status update event
    await publish_order_event(order_id, f"status_changed_to_{status.value}")
    
    return order


# =============================================================================
# MAIN
# =============================================================================

if __name__ == "__main__":
    import uvicorn
    port = int(os.getenv("PORT", "8002"))
    uvicorn.run(app, host="0.0.0.0", port=port)