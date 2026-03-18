# WebSockets with Django

## What You'll Learn
- Django Channels for WebSockets

## Prerequisites
- Completed WebSockets with FastAPI

## Installing Channels

```bash
pip install channels
```

## Setup

```python
# settings.py
ASGI_APPLICATION = 'myproject.asgi.application'
CHANNEL_LAYERS = {
    'default': {
        'BACKEND': 'channels.layers.InMemoryChannelLayer'
    }
}
```

## Consumer

```python
# consumers.py
import json
from channels.generic.websocket import AsyncWebsocketConsumer

class ChatConsumer(AsyncWebsocketConsumer):
    async def connect(self):
        self.room_group_name = 'chat_room'
        await self.channel_layer.group_add(self.room_group_name, self.channel_name)
        await self.accept()

    async def disconnect(self, close_code):
        await self.channel_layer.group_discard(self.room_group_name, self.channel_name)

    async def receive(self, text_data):
        await self.channel_layer.group_send(self.room_group_name, {'type': 'chat_message', 'message': text_data})

    async def chat_message(self, event):
        await self.send(text_data=event['message'])
```

## Summary
- Django Channels adds async support
- Consumers handle WebSocket connections
