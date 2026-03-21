# Timing Attack Prevention

## 📌 What You'll Learn

- What timing attacks are
- Safe string comparison

## 💻 Code Example

```js
import crypto from 'crypto';

function safeCompare(a, b) {
  if (typeof a !== 'string' || typeof b !== 'string') {
    return false;
  }
  
  if (a.length !== b.length) {
    return false;
  }
  
  return crypto.timingSafeEqual(Buffer.from(a), Buffer.from(b));
}

// Use for API key comparison
app.get('/api/verify', (req, res) => {
  const apiKey = req.headers['x-api-key'];
  
  if (!safeCompare(apiKey, process.env.API_KEY)) {
    return res.status(401).json({ error: 'Invalid API key' });
  }
  
  res.json({ valid: true });
});
