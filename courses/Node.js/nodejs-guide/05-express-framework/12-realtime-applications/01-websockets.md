# Real-time Applications with Express and WebSockets

## What You'll Learn

- WebSocket integration with Socket.io
- Server-sent events implementation
- Real-time data synchronization
- Connection management

## Socket.io Integration

```bash
npm install socket.io
```

```javascript
import express from 'express';
import { createServer } from 'node:http';
import { Server } from 'socket.io';

const app = express();
const server = createServer(app);
const io = new Server(server, {
    cors: { origin: 'https://example.com' }
});

io.on('connection', (socket) => {
    console.log('User connected:', socket.id);

    socket.on('join-room', (room) => {
        socket.join(room);
        io.to(room).emit('user-joined', socket.id);
    });

    socket.on('message', (data) => {
        io.to(data.room).emit('message', {
            userId: socket.id,
            text: data.text,
            timestamp: Date.now(),
        });
    });

    socket.on('disconnect', () => {
        console.log('User disconnected:', socket.id);
    });
});

server.listen(3000);
```

## Server-Sent Events

```javascript
app.get('/events', (req, res) => {
    res.writeHead(200, {
        'Content-Type': 'text/event-stream',
        'Cache-Control': 'no-cache',
        'Connection': 'keep-alive',
    });

    const sendEvent = (data) => {
        res.write(`data: ${JSON.stringify(data)}\n\n`);
    };

    // Send events periodically
    const interval = setInterval(() => {
        sendEvent({ time: Date.now(), message: 'heartbeat' });
    }, 1000);

    req.on('close', () => {
        clearInterval(interval);
    });
});
```

## Best Practices Checklist

- [ ] Implement connection authentication
- [ ] Handle reconnection gracefully
- [ ] Use rooms for targeted broadcasting
- [ ] Monitor connection counts
- [ ] Implement rate limiting for messages

## Cross-References

- See [Architecture](../01-express-architecture/01-lifecycle-deep-dive.md) for request flow
- See [Security](../05-security-implementation/01-helmet-cors.md) for security
- See [Monitoring](../14-monitoring-observability/01-apm-setup.md) for observability

## Next Steps

Continue to [Container Orchestration](../13-container-orchestration/01-docker-setup.md) for deployment.
