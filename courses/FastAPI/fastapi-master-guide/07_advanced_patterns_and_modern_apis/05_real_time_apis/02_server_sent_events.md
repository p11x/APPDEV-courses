# Server-Sent Events

## Overview

Server-Sent Events (SSE) enable one-way real-time communication from server to client, ideal for live updates and notifications.

## Implementation

### Basic SSE

```python
# Example 1: Server-Sent Events in FastAPI
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
import asyncio
import json
from datetime import datetime

app = FastAPI()

async def event_generator():
    """Generate SSE events"""
    count = 0
    while True:
        count += 1
        data = {
            "count": count,
            "timestamp": datetime.utcnow().isoformat()
        }
        yield f"data: {json.dumps(data)}\n\n"
        await asyncio.sleep(1)

@app.get("/events")
async def get_events():
    """SSE endpoint"""
    return StreamingResponse(
        event_generator(),
        media_type="text/event-stream",
        headers={
            "Cache-Control": "no-cache",
            "Connection": "keep-alive"
        }
    )
```

### Real-Time Updates

```python
# Example 2: Real-time stock prices
import asyncio
import random

async def stock_price_generator(symbol: str):
    """Generate stock price updates"""
    price = 100.0

    while True:
        # Simulate price change
        change = random.uniform(-1, 1)
        price += change

        event_data = {
            "symbol": symbol,
            "price": round(price, 2),
            "change": round(change, 2),
            "timestamp": datetime.utcnow().isoformat()
        }

        yield f"data: {json.dumps(event_data)}\n\n"
        await asyncio.sleep(1)

@app.get("/stocks/{symbol}")
async def stock_stream(symbol: str):
    """Stream stock prices"""
    return StreamingResponse(
        stock_price_generator(symbol),
        media_type="text/event-stream"
    )
```

### Named Events

```python
# Example 3: Named event types
async def notification_generator(user_id: int):
    """Generate different event types"""
    while True:
        # Notification event
        yield f"event: notification\n"
        yield f"data: {json.dumps({'message': 'New notification'})}\n\n"
        await asyncio.sleep(5)

        # Status event
        yield f"event: status\n"
        yield f"data: {json.dumps({'status': 'online'})}\n\n"
        await asyncio.sleep(10)
```

## Summary

SSE provides efficient one-way real-time communication from server to client.

## Next Steps

Continue learning about:
- [Real-Time Notifications](./03_real_time_notifications.md)
- [Caching Strategies](../02_performance_optimization/01_caching_strategies.md)
