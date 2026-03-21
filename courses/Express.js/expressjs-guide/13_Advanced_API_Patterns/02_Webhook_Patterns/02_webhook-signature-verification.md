# Webhook Signature Verification

## 📌 What You'll Learn

- Verifying webhook signatures
- HMAC-SHA256 implementation
- Timing-safe comparisons

## 💻 Code Example

```js
import crypto from 'crypto';

const WEBHOOK_SECRET = process.env.WEBHOOK_SECRET;

function verifySignature(payload, signature) {
  const expected = crypto
    .createHmac('sha256', WEBHOOK_SECRET)
    .update(JSON.stringify(payload))
    .digest('hex');
  
  return crypto.timingSafeEqual(
    Buffer.from(signature),
    Buffer.from(expected)
  );
}
```

## ✅ Quick Recap

- Always verify webhook signatures
- Use HMAC-SHA256
- Use timingSafeEqual to prevent timing attacks

## 🔗 What's Next

Learn about [Webhook Delivery Guarantees](./03_webhook-delivery-guarantees.md).
