# In-Memory Caching

## 📌 What You'll Learn

- Using node-cache/lru-cache
- TTL and cache invalidation
- When NOT to cache

## 💻 Code Example

```js
import NodeCache from 'node-cache';

const cache = new NodeCache({ stdTTL: 300 });

app.get('/users/:id', (req, res) => {
  const key = `user:${req.params.id}`;
  let user = cache.get(key);
  
  if (!user) {
    user = { id: req.params.id, name: 'John' };
    cache.set(key, user);
  }
  
  res.json(user);
});
```
