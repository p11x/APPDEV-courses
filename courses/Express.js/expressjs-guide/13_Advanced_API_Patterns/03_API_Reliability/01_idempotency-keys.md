# Idempotency Keys

## 📌 What You'll Learn

- What idempotency means in APIs
- Implementing client-generated idempotency keys
- Storing results for safe retries
- TTL considerations

## 💻 Code Example

```js
import express from 'express';
import crypto from 'crypto';

const app = express();

// In production, use Redis
const idempotencyStore = new Map();

app.use(express.json());

app.post('/api/payments', (req, res) => {
  const key = req.headers['idempotency-key'];
  
  if (!key) {
    return res.status(400).json({ error: 'Idempotency key required' });
  }
  
  // Check if already processed
  if (idempotencyStore.has(key)) {
    return res.json(idempotencyStore.get(key));
  }
  
  // Process payment
  const result = { paymentId: 'pay-' + Date.now(), status: 'success' };
  
  // Store result (with 24h TTL in production)
  idempotencyStore.set(key, result);
  
  res.status(201).json(result);
});

app.get('/health', (req, res) => res.json({ status: 'ok' }));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Port ${PORT}`));
```

## ✅ Quick Recap

- Use idempotency keys for POST requests
- Store results with key for retries
- Use TTL to prevent unbounded storage
