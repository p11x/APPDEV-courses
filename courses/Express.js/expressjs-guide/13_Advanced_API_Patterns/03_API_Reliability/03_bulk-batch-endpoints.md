# Bulk Batch Endpoints

## 📌 What You'll Learn

- Designing efficient batch endpoints
- Processing arrays of operations
- Partial success handling

## 💻 Code Example

```js
import express from 'express';

const app = express();

app.use(express.json());

app.post('/api/users/bulk', async (req, res) => {
  const { users } = req.body || [];
  
  const results = await Promise.allSettled(
    users.map(user => createUser(user))
  );
  
  const succeeded = results
    .filter(r => r.status === 'fulfilled')
    .map(r => r.value);
    
  const failed = results
    .filter(r => r.status === 'rejected')
    .map((r, i) => ({ index: i, error: r.reason.message }));
  
  res.json({
    total: users.length,
    succeeded: succeeded.length,
    failed: failed.length,
    results: succeeded,
    errors: failed
  });
});

async function createUser(data) {
  return { id: Date.now(), ...data };
}

app.get('/health', (req, res) => res.json({ status: 'ok' }));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Port ${PORT}`));
```
