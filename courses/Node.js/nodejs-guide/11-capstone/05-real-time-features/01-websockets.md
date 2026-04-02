# Real-Time Features with WebSockets

## What You'll Learn

- Adding real-time updates to the capstone project
- WebSocket integration with Express
- Broadcasting updates to connected clients

## Integration

```js
// server.js — Add WebSocket to the capstone

import { WebSocketServer } from 'ws';
import { createServer } from 'node:http';

const server = createServer(app);
const wss = new WebSocketServer({ server });

wss.on('connection', (ws) => {
  ws.on('message', (data) => {
    const message = JSON.parse(data);
    if (message.type === 'subscribe') {
      ws.bookmarkId = message.bookmarkId;
    }
  });
});

// Broadcast bookmark updates
export function broadcastUpdate(bookmarkId, data) {
  wss.clients.forEach((client) => {
    if (client.bookmarkId === bookmarkId && client.readyState === 1) {
      client.send(JSON.stringify(data));
    }
  });
}
```

## Next Steps

Continue to [Notifications](./02-notifications.md).
