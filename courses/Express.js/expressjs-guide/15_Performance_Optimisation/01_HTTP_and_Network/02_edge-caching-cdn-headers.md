# Edge Caching & CDN Headers

## 📌 What You'll Learn

- Cache-Control headers
- ETag and Last-Modified
- Vary header for CDN

## 💻 Code Example

```js
import express from 'express';

const app = express();

// Static files with caching
app.use(express.static('public', {
  maxAge: '1d',
  etag: true,
  lastModified: true
}));

// API response with caching
app.get('/api/products', (req, res) => {
  res.set('Cache-Control', 'public, max-age=300');
  res.set('ETag', '"abc123"');
  res.json({ products: [] });
});

// Vary for content negotiation
app.get('/api/data', (req, res) => {
  res.set('Vary', 'Accept-Encoding, Accept');
  res.json({ data: 'value' });
});
