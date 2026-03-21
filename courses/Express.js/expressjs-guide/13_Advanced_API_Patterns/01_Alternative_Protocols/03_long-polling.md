# Long Polling

## 📌 What You'll Learn

- What long polling is
- Implementing long polling in Express
- When to use long polling vs SSE vs WebSockets

## 🧠 Concept Explained (Plain English)

**Long polling** is a technique where the client makes a request, and the server holds it open until new data is available, then responds. The client immediately makes another request, creating near-real-time updates.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();

// Pending responses for polling
let pendingClients = [];
const updates = [];

function sendUpdate(data) {
  updates.push(data);
  pendingClients.forEach(res => res.json(data));
  pendingClients = [];
}

app.get('/health', (req, res) => res.json({ status: 'ok' }));

// Long poll endpoint
app.get('/api/updates', (req, res) => {
  if (updates.length > 0) {
    return res.json(updates.splice(0));
  }
  pendingClients.push(res);
  req.on('close', () => {
    pendingClients = pendingClients.filter(c => c !== res);
  });
});

// Trigger updates
app.post('/api/updates', express.json(), (req, res) => {
  sendUpdate(req.body);
  res.json({ ok: true });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Port ${PORT}`));
```

## ✅ Quick Recap

- Long polling holds requests until data available
- Works with regular HTTP
- Simpler than WebSockets

## 🔗 What's Next

Learn about [gRPC with Express](./04_grpc-with-express.md).
