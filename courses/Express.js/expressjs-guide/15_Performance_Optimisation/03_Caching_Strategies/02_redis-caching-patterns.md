# Redis Caching Patterns

## 📌 What You'll Learn

- Cache-aside, write-through, read-through
- Cache stampede prevention

## 💻 Code Example

```js
import redis from 'redis';

const client = redis.createClient();

// Cache-aside pattern
async function getUser(id) {
  const cached = await client.get(`user:${id}`);
  if (cached) return JSON.parse(cached);
  
  const user = await db.query('SELECT * FROM users WHERE id = $1', [id]);
  await client.setex(`user:${id}`, 300, JSON.stringify(user));
  
  return user;
}
```
