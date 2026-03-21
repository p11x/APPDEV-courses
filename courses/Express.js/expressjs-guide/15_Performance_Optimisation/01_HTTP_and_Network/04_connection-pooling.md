# Connection Pooling

## 📌 What You'll Learn

- Why connection pools matter
- Configuring pool size for pg/mongoose
- Pool monitoring

## 💻 Code Example

```js
import { Pool } from 'pg';

const pool = new Pool({
  max: 20,
  idleTimeoutMillis: 30000,
  connectionTimeoutMillis: 2000
});

app.get('/users', async (req, res) => {
  const client = await pool.connect();
  try {
    const result = await client.query('SELECT * FROM users');
    res.json(result.rows);
  } finally {
    client.release();
  }
});

// Monitor pool
pool.on('error', (err) => console.error('Pool error:', err));
```
