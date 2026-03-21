# Server-Sent Events

## 📌 What You'll Learn

- What SSE is and when to use it
- Implementing SSE in Express
- Streaming updates to clients

## 🧠 Concept Explained (Plain English)

**Server-Sent Events (SSE)** is a server push technology where the server sends data to the client over a single, long-lived HTTP connection. Unlike WebSockets, SSE is one-way — only the server can send messages.

**When to use:**
- Live notifications
- Real-time updates (stock prices, news feeds)
- Progress updates
- Single-direction streaming

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();

app.get('/health', (req, res) => res.json({ status: 'ok' }));

// SSE endpoint
app.get('/api/events/stream', (req, res) => {
  res.setHeader('Content-Type', 'text/event-stream');
  res.setHeader('Cache-Control', 'no-cache');
  res.setHeader('Connection', 'keep-alive');
  
  // Send initial message
  res.write('data: {"message": "Connected"}\n\n');
  
  // Send updates periodically
  const interval = setInterval(() => {
    const data = { time: new Date().toISOString(), count: Math.random() };
    res.write(`data: ${JSON.stringify(data)}\n\n`);
  }, 2000);
  
  // Clean up on close
  req.on('close', () => clearInterval(interval));
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Port ${PORT}`));
```

## ✅ Quick Recap

- SSE is one-way server-to-client streaming
- Uses long-lived HTTP connections
- Simpler than WebSockets for one-way communication

## 🔗 What's Next

Learn about [Long Polling](./03_long-polling.md).
