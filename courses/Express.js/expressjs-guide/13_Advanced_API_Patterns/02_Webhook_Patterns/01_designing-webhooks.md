# Designing Webhooks

## 📌 What You'll Learn

- What webhooks are and how they work
- Designing webhook payloads
- Implementing webhook endpoints in Express
- Handling retries and reliability

## 🧠 Concept Explained (Plain English)

**Webhooks** are HTTP callbacks — when something happens in your system, you make an HTTP POST request to notify another system. They're the reverse of traditional APIs: instead of polling for data, you get pushed updates.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();

app.use(express.json());

// Webhook endpoint
app.post('/webhooks/events', (req, res) => {
  const event = req.body;
  
  // Verify webhook signature
  const signature = req.headers['x-webhook-signature'];
  if (!verifySignature(signature, event)) {
    return res.status(401).json({ error: 'Invalid signature' });
  }
  
  // Process event
  console.log('Received webhook:', event.type, event.data);
  
  // Respond quickly (within 30 seconds)
  res.status(200).json({ received: true });
});

function verifySignature(signature, payload) {
  // Implement signature verification
  return true; // Simplified
}

app.get('/health', (req, res) => res.json({ status: 'ok' }));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Port ${PORT}`));
```

## ✅ Quick Recap

- Webhooks are HTTP POST callbacks
- Send payloads when events occur
- Verify signatures for security

## 🔗 What's Next

Learn about [Webhook Signature Verification](./02_webhook-signature-verification.md).
