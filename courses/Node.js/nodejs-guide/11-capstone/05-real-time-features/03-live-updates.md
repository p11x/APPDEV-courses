# Live Updates

## What You'll Learn

- Implementing real-time data updates
- Server-Sent Events as an alternative
- Optimistic UI updates

## SSE Implementation

```js
// routes/events.js
app.get('/api/events', (req, res) => {
  res.writeHead(200, {
    'Content-Type': 'text/event-stream',
    'Cache-Control': 'no-cache',
    'Connection': 'keep-alive',
  });

  const sendEvent = (data) => {
    res.write(`data: ${JSON.stringify(data)}\n\n`);
  };

  // Add to active connections
  connections.add(sendEvent);

  req.on('close', () => connections.delete(sendEvent));
});

// Broadcast updates
function broadcastUpdate(data) {
  connections.forEach((send) => send(data));
}
```

## Next Steps

For API documentation, continue to [Swagger Setup](../07-api-documentation/01-swagger-setup.md).
