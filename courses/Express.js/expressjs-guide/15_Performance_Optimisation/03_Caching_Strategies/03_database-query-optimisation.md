# Database Query Optimisation

## 📌 What You'll Learn

- N+1 problem
- Query batching with DataLoader
- Index usage and EXPLAIN ANALYSE

## 💻 Code Example

```js
// N+1 problem - BAD
const users = await db.query('SELECT * FROM users');
for (const user of users) {
  const posts = await db.query('SELECT * FROM posts WHERE user_id = $1', [user.id]);
  user.posts = posts;
}

// DataLoader - GOOD
import DataLoader from 'dataloader';

const postLoader = new DataLoader(async (ids) => {
  const posts = await db.query(
    'SELECT * FROM posts WHERE user_id = ANY($1)',
    [ids]
  );
  
  return ids.map(id => posts.filter(p => p.user_id === id));
});
```
