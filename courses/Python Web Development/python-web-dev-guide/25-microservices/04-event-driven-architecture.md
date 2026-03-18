# Event-Driven Architecture

## What You'll Learn
- Message queues
- Event publishing
- Event handling

## Prerequisites
- Completed inter-service communication

## Using RabbitMQ

```bash
pip install aio-pika
```

```python
import aio_pika
import json

async def publish_event(exchange_name: str, event: dict):
    """Publish event to message queue"""
    connection = await aio_pika.connect_robust("amqp://guest:guest@localhost/")
    async with connection:
        channel = await connection.channel()
        exchange = await channel.declare_exchange(
            exchange_name, aio_pika.ExchangeType.TOPIC, durable=True
        )
        await exchange.publish(
            aio_pika.Message(
                body=json.dumps(event).encode(),
                content_type="application/json"
            ),
            routing_key="order.created"
        )

# Publish
await publish_event("orders", {"type": "order_created", "order_id": 123})
```

## Event Handler

```python
async def on_order_created(message: aio_pika.IncomingMessage):
    async with message.process():
        event = json.loads(message.body.decode())
        print(f"Processing order: {event['order_id']}")
        # Do something with the order
```

## Summary
- Use events for loose coupling
- RabbitMQ, Kafka are popular
- Handle events asynchronously

## Next Steps
→ Continue to `05-distributed-tracing.md`
