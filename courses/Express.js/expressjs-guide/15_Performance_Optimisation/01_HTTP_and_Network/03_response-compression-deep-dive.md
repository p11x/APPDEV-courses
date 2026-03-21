# Response Compression Deep Dive

## 📌 What You'll Learn

- Brotli vs gzip
- Compression middleware
- Per-route control

## 💻 Code Example

```js
import express from 'express';
import compression from 'compression';

const app = express();

// Global compression
app.use(compression({
  threshold: 1024,
  level: 6,
  filter: (req) => {
    if (req.headers['x-no-compression']) return false;
    return compression.filter(req);
  }
}));

// Per-route control
app.get('/api/large', compression({ threshold: 0 }), (req, res) => {
  res.json({ data: 'x'.repeat(10000) });
});
