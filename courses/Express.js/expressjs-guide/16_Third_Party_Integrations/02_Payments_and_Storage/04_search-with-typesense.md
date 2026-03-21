# Search with Typesense

## 📌 What You'll Learn

- Typesense client for search
- Indexing and querying

## 💻 Code Example

```js
import { Typesense } from 'typesense';

const client = new Typesense.Client({
  nodes: [{ host: 'localhost', port: 8108, protocol: 'http' }],
  apiKey: process.env.TYPESENSE_API_KEY
});

app.post('/index', async (req, res) => {
  await client.collections('products').documents().create(req.body);
  res.json({ indexed: true });
});

app.get('/search', async (req, res) => {
  const { q } = req.query;
  const results = await client.collections('products').documents().search({ q });
  res.json(results);
});
```
