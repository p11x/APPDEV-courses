"""
Message Queue Producer/Consumer using aio-pika

This module provides:
- Async message producer for publishing to RabbitMQ
- Async consumer for processing messages
- Queue management utilities
- Dead letter queue handling
- Message acknowledgment patterns

Usage:
    # Producer
    async with MessageProducer("amqp://guest:guest@localhost/") as producer:
        await producer.publish("orders", {"order_id": "123", "amount": 100})
    
    # Consumer
    async with MessageConsumer("amqp://guest:guest@localhost/") as consumer:
        await consumer.consume("orders", process_order)
"""

import asyncio
import json
import logging
from typing import Any, Callable, Dict, Optional, Awaitable
from dataclasses import dataclass, field
from enum import Enum

import aio_pika
from aio_pika import (
    connect_robust,
    Message,
    ExchangeType,
    DeliveryMode,
)
from aio_pika.abc import (
    AbstractRobustConnection,
    AbstractChannel,
    AbstractExchange,
    AbstractQueue,
    AbstractIncomingMessage,
)

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


class MessagePriority(Enum):
    """Message priority levels."""
    LOW = 0
    NORMAL = 5
    HIGH = 10


@dataclass
class QueueConfig:
    """Configuration for a message queue."""
    name: str
    durable: bool = True
    exclusive: bool = False
    auto_delete: bool = False
    arguments: Dict[str, Any] = field(default_factory=dict)
    dead_letter_exchange: Optional[str] = None
    dead_letter_routing_key: Optional[str] = None


@dataclass
class ExchangeConfig:
    """Configuration for an exchange."""
    name: str
    exchange_type: ExchangeType = ExchangeType.DIRECT
    durable: bool = True
    auto_delete: bool = False


class MessageProducer:
    """
    Async message producer for RabbitMQ.
    
    Provides robust connection handling, message publishing,
    and exchange/queue management.
    
    Attributes:
        connection_url: RabbitMQ connection URL
    """
    
    def __init__(
        self,
        connection_url: str = "amqp://guest:guest@localhost/",
        priority: MessagePriority = MessagePriority.NORMAL,
    ):
        """
        Initialize the message producer.
        
        Args:
            connection_url: RabbitMQ connection URL
            priority: Default message priority
        """
        self.connection_url = connection_url
        self.default_priority = priority
        self._connection: Optional[AbstractRobustConnection] = None
        self._channel: Optional[AbstractChannel] = None
        self._exchanges: Dict[str, AbstractExchange] = {}
    
    async def __aenter__(self):
        """Async context manager entry."""
        await self.connect()
        return self
    
    async def __aexit__(self, exc_type, exc_val, exc_tb):
        """Async context manager exit."""
        await self.close()
    
    async def connect(self):
        """Establish connection to RabbitMQ."""
        self._connection = await connect_robust(self.connection_url)
        self._channel = await self._connection.channel()
        logger.info("Connected to RabbitMQ")
    
    async def close(self):
        """Close the connection."""
        if self._connection:
            await self._connection.close()
            logger.info("Disconnected from RabbitMQ")
    
    async def declare_exchange(
        self,
        name: str,
        exchange_type: ExchangeType = ExchangeType.DIRECT,
        durable: bool = True,
        auto_delete: bool = False,
    ) -> AbstractExchange:
        """
        Declare an exchange.
        
        Args:
            name: Exchange name
            exchange_type: Type of exchange
            durable: Whether exchange survives broker restart
            auto_delete: Delete when no queues bound
            
        Returns:
            The declared exchange
        """
        exchange = await self._channel.declare_exchange(
            name=name,
            type=exchange_type,
            durable=durable,
            auto_delete=auto_delete,
        )
        self._exchanges[name] = exchange
        logger.info(f"Declared exchange: {name}")
        return exchange
    
    async def declare_queue(
        self,
        config: QueueConfig,
    ) -> AbstractQueue:
        """
        Declare a queue with optional dead letter configuration.
        
        Args:
            config: Queue configuration
            
        Returns:
            The declared queue
        """
        arguments = config.arguments.copy()
        
        # Configure dead letter exchange if specified
        if config.dead_letter_exchange:
            arguments["x-dead-letter-exchange"] = config.dead_letter_exchange
        if config.dead_letter_routing_key:
            arguments["x-dead-letter-routing-key"] = config.dead_letter_routing_key
        
        queue = await self._channel.declare_queue(
            name=config.name,
            durable=config.durable,
            exclusive=config.exclusive,
            auto_delete=config.auto_delete,
            arguments=arguments,
        )
        logger.info(f"Declared queue: {config.name}")
        return queue
    
    async def publish(
        self,
        routing_key: str,
        body: Dict[str, Any],
        exchange: str = "",
        headers: Optional[Dict[str, Any]] = None,
        delivery_mode: DeliveryMode = DeliveryMode.PERSISTENT,
        priority: Optional[int] = None,
    ):
        """
        Publish a message to a queue.
        
        Args:
            routing_key: Routing key for message
            body: Message body (will be JSON serialized)
            exchange: Exchange name (empty for default)
            headers: Message headers
            delivery_mode: Message delivery mode
            priority: Message priority
        """
        # Get or create exchange
        if exchange and exchange not in self._exchanges:
            await self.declare_exchange(exchange)
        
        exchange_obj = self._exchanges.get(exchange) or await self._channel.get_default_exchange()
        
        # Serialize message body
        message_body = json.dumps(body).encode()
        
        # Create message with properties
        message = Message(
            body=message_body,
            headers=headers or {},
            delivery_mode=delivery_mode,
            priority=priority or self.default_priority.value,
            content_type="application/json",
        )
        
        # Publish
        await exchange_obj.publish(message, routing_key=routing_key)
        logger.debug(f"Published message to {routing_key}: {body}")
    
    async def publish_batch(
        self,
        messages: list,
        routing_key: str,
        exchange: str = "",
    ):
        """
        Publish multiple messages efficiently.
        
        Args:
            messages: List of message bodies
            routing_key: Routing key
            exchange: Exchange name
        """
        for body in messages:
            await self.publish(routing_key, body, exchange)
            # Small delay to prevent overwhelming the channel
            await asyncio.sleep(0.001)


class MessageConsumer:
    """
    Async message consumer for RabbitMQ.
    
    Provides robust message consumption with:
    - Automatic reconnection
    - Message acknowledgment
    - Prefetch control
    - Error handling
    
    Attributes:
        connection_url: RabbitMQ connection URL
    """
    
    def __init__(
        self,
        connection_url: str = "amqp://guest:guest@localhost/",
        prefetch_count: int = 10,
    ):
        """
        Initialize the message consumer.
        
        Args:
            connection_url: RabbitMQ connection URL
            prefetch_count: Number of unacknowledged messages
        """
        self.connection_url = connection_url
        self.prefetch_count = prefetch_count
        self._connection: Optional[AbstractRobustConnection] = None
        self._channel: Optional[AbstractChannel] = None
        self._queues: Dict[str, AbstractQueue] = {}
        self._running = False
    
    async def __aenter__(self):
        """Async context manager entry."""
        await self.connect()
        return self
    
    async def __aexit__(self, exc_type, exc_val, exc_tb):
        """Async context manager exit."""
        await self.close()
    
    async def connect(self):
        """Establish connection to RabbitMQ."""
        self._connection = await connect_robust(self.connection_url)
        self._channel = await self._connection.channel()
        
        # Set prefetch count for fair dispatch
        await self._channel.set_qos(prefetch_count=self.prefetch_count)
        
        logger.info("Consumer connected to RabbitMQ")
    
    async def close(self):
        """Close the connection."""
        self._running = False
        if self._connection:
            await self._connection.close()
            logger.info("Consumer disconnected from RabbitMQ")
    
    async def declare_queue(
        self,
        config: QueueConfig,
    ) -> AbstractQueue:
        """
        Declare a queue for consuming messages.
        
        Args:
            config: Queue configuration
            
        Returns:
            The declared queue
        """
        queue = await self._channel.declare_queue(
            name=config.name,
            durable=config.durable,
            exclusive=config.exclusive,
            auto_delete=config.auto_delete,
        )
        self._queues[config.name] = queue
        return queue
    
    async def bind_queue(
        self,
        queue_name: str,
        exchange_name: str,
        routing_key: str = "",
    ):
        """
        Bind a queue to an exchange.
        
        Args:
            queue_name: Queue name
            exchange_name: Exchange name
            routing_key: Routing key
        """
        queue = self._queues.get(queue_name)
        if not queue:
            raise ValueError(f"Queue {queue_name} not declared")
        
        await queue.bind(exchange_name, routing_key=routing_key)
        logger.info(f"Bound queue {queue_name} to exchange {exchange_name}")
    
    async def consume(
        self,
        queue_name: str,
        handler: Callable[[Dict[str, Any]], Awaitable[Any]],
        auto_ack: bool = False,
    ):
        """
        Start consuming messages from a queue.
        
        Args:
            queue_name: Queue to consume from
            handler: Async function to handle messages
            auto_ack: Auto acknowledge messages
        """
        queue = self._queues.get(queue_name)
        if not queue:
            raise ValueError(f"Queue {queue_name} not declared")
        
        self._running = True
        
        async def process_message(message: AbstractIncomingMessage):
            """Process incoming message."""
            async with message.process():
                try:
                    # Parse message body
                    body = json.loads(message.body.decode())
                    
                    # Call handler
                    await handler(body)
                    
                except json.JSONDecodeError as e:
                    logger.error(f"Failed to decode message: {e}")
                except Exception as e:
                    logger.error(f"Error processing message: {e}")
                    # Re-raise to trigger nack
                    raise
        
        await queue.consume(process_message, auto_ack=auto_ack)
        logger.info(f"Started consuming from {queue_name}")
        
        # Keep running
        while self._running:
            await asyncio.sleep(1)
    
    def stop(self):
        """Stop consuming messages."""
        self._running = False


# Example usage and handlers
async def order_handler(message: Dict[str, Any]):
    """Example message handler for orders."""
    order_id = message.get("order_id")
    amount = message.get("amount")
    logger.info(f"Processing order {order_id} with amount {amount}")
    
    # Simulate processing
    await asyncio.sleep(1)
    
    logger.info(f"Order {order_id} processed successfully")
    return {"status": "processed", "order_id": order_id}


async def main():
    """Demonstrate message queue producer and consumer."""
    
    # Producer example
    async with MessageProducer() as producer:
        # Declare exchange
        await producer.declare_exchange("orders_exchange", ExchangeType.DIRECT)
        
        # Declare queue with dead letter config
        queue_config = QueueConfig(
            name="orders",
            dead_letter_exchange="dlx",
            dead_letter_routing_key="dead_orders",
        )
        await producer.declare_queue(queue_config)
        
        # Publish messages
        await producer.publish(
            routing_key="orders",
            exchange="orders_exchange",
            body={"order_id": "123", "amount": 100, "customer": "John"},
        )
        
        # Batch publish
        await producer.publish_batch(
            messages=[
                {"order_id": f"ORD-{i}", "amount": i * 10}
                for i in range(1, 4)
            ],
            routing_key="orders",
            exchange="orders_exchange",
        )
    
    # Consumer example (would run in separate process)
    # async with MessageConsumer() as consumer:
    #     await consumer.declare_queue(QueueConfig(name="orders"))
    #     await consumer.bind_queue("orders", "orders_exchange", "orders")
    #     await consumer.consume("orders", order_handler)


if __name__ == "__main__":
    asyncio.run(main())
