# Webhook Delivery Guarantees

## 📌 What You'll Learn

- At-least-once delivery patterns
- Idempotency keys
- Deduplication strategies

## 💻 Code Example

```js
// Idempotency key verification
const processedKeys = new Set();

app.post('/webhooks/stripe', express.json(), (req, res) => {
  const idempotencyKey = req.headers['idempotency-key'];
  
  if (processedKeys.has(idempotencyKey)) {
    return res.status(200).json({ alreadyProcessed: true });
  }
  
  // Process webhook
  processEvent(req.body);
  processedKeys.add(idempotencyKey);
  
  res.json({ ok: true });
});
```

## ✅ Quick Recap

- Use idempotency keys to handle duplicate deliveries
- Store processed keys with TTL
- Always respond quickly to prevent retries
