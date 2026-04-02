# IoT API Patterns

## Overview

IoT APIs handle device management, telemetry, and real-time communication.

## Device Management

### Device Registry

```python
# Example 1: IoT device management
from fastapi import FastAPI, HTTPException, Depends
from pydantic import BaseModel
from typing import Dict, List, Optional
from datetime import datetime

app = FastAPI()

class Device(BaseModel):
    device_id: str
    device_type: str
    firmware_version: str
    last_seen: Optional[datetime] = None
    status: str = "offline"
    metadata: Dict = {}

class Telemetry(BaseModel):
    device_id: str
    timestamp: datetime
    data: Dict

# Device registry
devices_db: Dict[str, Device] = {}
telemetry_buffer: List[Telemetry] = []

@app.post("/devices/register")
async def register_device(device: Device):
    """Register new IoT device"""
    devices_db[device.device_id] = device
    return {"registered": device.device_id}

@app.put("/devices/{device_id}/heartbeat")
async def device_heartbeat(device_id: str):
    """Device heartbeat"""
    if device_id not in devices_db:
        raise HTTPException(404, "Device not found")

    devices_db[device_id].last_seen = datetime.utcnow()
    devices_db[device_id].status = "online"

    return {"status": "acknowledged"}

@app.post("/devices/{device_id}/telemetry")
async def receive_telemetry(device_id: str, telemetry: Telemetry):
    """Receive device telemetry"""
    if device_id not in devices_db:
        raise HTTPException(404, "Device not found")

    telemetry.device_id = device_id
    telemetry_buffer.append(telemetry)

    # Process telemetry
    await process_telemetry(telemetry)

    return {"received": True}
```

## MQTT Integration

```python
# Example 2: MQTT for IoT
import asyncio_mqtt as mqtt
import json

class MQTTBridge:
    """MQTT bridge for IoT devices"""

    def __init__(self, broker: str):
        self.broker = broker
        self.client = None

    async def connect(self):
        self.client = mqtt.Client(self.broker)
        await self.client.connect()

    async def subscribe(self, topic: str, callback):
        await self.client.subscribe(topic)
        async for message in self.client.messages:
            await callback(message.topic, json.loads(message.payload))

    async def publish(self, topic: str, data: dict):
        await self.client.publish(topic, json.dumps(data))

mqtt_bridge = MQTTBridge("mqtt://localhost:1883")

async def handle_device_message(topic: str, data: dict):
    """Handle MQTT messages from devices"""
    device_id = topic.split("/")[1]

    if "telemetry" in topic:
        await receive_telemetry(device_id, Telemetry(**data))
    elif "status" in topic:
        await update_device_status(device_id, data["status"])

@app.on_event("startup")
async def startup():
    await mqtt_bridge.connect()
    asyncio.create_task(
        mqtt_bridge.subscribe("devices/+/telemetry", handle_device_message)
    )
```

## Summary

IoT APIs handle device communication and telemetry processing.

## Next Steps

Continue learning about:
- [AI/ML APIs](./08_ai_ml_apis.md)
- [Real-Time Analytics](./09_real_time_analytics.md)
