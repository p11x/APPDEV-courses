# Streaming APIs

## Overview

Streaming APIs handle real-time data flows using Server-Sent Events, WebSockets, and async generators.

## Streaming Implementation

### Async Generator Streaming

```python
# Example 1: Streaming responses
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
import asyncio
import json

app = FastAPI()

async def generate_data():
    """Generate streaming data"""
    for i in range(100):
        data = {
            "id": i,
            "timestamp": datetime.utcnow().isoformat(),
            "value": random.random()
        }
        yield f"data: {json.dumps(data)}\n\n"
        await asyncio.sleep(0.1)

@app.get("/stream/data")
async def stream_data():
    """Stream data to client"""
    return StreamingResponse(
        generate_data(),
        media_type="text/event-stream"
    )

# Kafka streaming
from aiokafka import AIOKafkaConsumer

async def consume_kafka(topic: str):
    """Consume from Kafka topic"""
    consumer = AIOKafkaConsumer(
        topic,
        bootstrap_servers="localhost:9092"
    )
    await consumer.start()

    try:
        async for msg in consumer:
            yield f"data: {msg.value.decode()}\n\n"
    finally:
        await consumer.stop()

@app.get("/stream/kafka/{topic}")
async def stream_kafka(topic: str):
    """Stream from Kafka"""
    return StreamingResponse(
        consume_kafka(topic),
        media_type="text/event-stream"
    )
```

## Summary

Streaming APIs enable real-time data delivery.

## Next Steps

Continue learning about:
- [WebSocket Implementation](../../07_advanced_patterns_and_modern_apis/05_real_time_apis/01_websocket_implementation.md)
- [Server-Sent Events](../../07_advanced_patterns_and_modern_apis/05_real_time_apis/02_server_sent_events.md)
