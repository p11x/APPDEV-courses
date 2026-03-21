# API Gateway Pattern

## 📌 What You'll Learn

- What an API gateway does
- Building a lightweight gateway with Express
- Routing to backend services

## 💻 Code Example

```js
import express from 'express';

const app = express();

// Service URLs
const services = {
  users: 'http://localhost:3001',
  orders: 'http://localhost:3002',
  products: 'http://localhost:3003'
};

// Route to appropriate service
app.use((req, res) => {
  const path = req.path;
  
  if (path.startsWith('/api/users')) {
    return proxy(req, res, services.users);
  }
  if (path.startsWith('/api/orders')) {
    return proxy(req, res, services.orders);
  }
  if (path.startsWith('/api/products')) {
    return proxy(req, res, services.products);
  }
  
  res.status(404).json({ error: 'Not found' });
});

async function proxy(req, res, target) {
  // Simplified proxy - use http-proxy-middleware in production
  res.json({ proxied: target + req.path });
}

app.get('/health', (req, res) => res.json({ status: 'ok' }));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Gateway: ${PORT}`));
```

## ✅ Quick Recap

- API gateway routes requests to backend services
- Handles cross-cutting concerns
- Can aggregate multiple services

## 🔗 What's Next

Moving to [Security Deep Dive - Section 14](./../../14_Security_Deep_Dive/01_OWASP/01_owasp-top-10-nodejs.md).
