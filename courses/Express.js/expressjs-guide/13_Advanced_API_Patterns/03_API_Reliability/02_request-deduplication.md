# Request Deduplication

## 📌 What You'll Learn

- Detecting duplicate requests
- Caching responses by key
- Returning cached results

## 💻 Code Example

```js
import express from 'express';

const app = express();

const responseCache = new Map();

app.use(express.json());

function getCacheKey(req) {
  return `${req.method}:${req.path}:${JSON.stringify(req.body)}`;
}

app.post('/api/search', (req, res) => {
  const key = getCacheKey(req);
  
  if (responseCache.has(key)) {
    return res.json({ ...responseCache.get(key), cached: true });
  }
  
  const result = { data: 'search results', timestamp: Date.now() };
  responseCache.set(key, result);
  
  res.json(result);
});

app.get('/health', (req, res) => res.json({ status: 'ok' }));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Port ${PORT}`));
```
