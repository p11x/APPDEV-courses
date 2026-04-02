# Azure Functions Performance

## What You'll Learn

- How to optimize function performance
- How to reduce cold start times
- How to implement caching strategies
- How to monitor performance metrics

---

## Layer 1: Optimization

### Connection Reuse

```typescript
let dbPool: Pool | null = null;

async function getPool(): Promise<Pool> {
  if (!dbPool) {
    dbPool = new Pool({
      host: process.env.DB_HOST,
      max: 20
    });
  }
  return dbPool;
}

const httpTrigger: AzureFunction = async function (context) {
  const pool = await getPool();
  const result = await pool.query('SELECT * FROM users');
  context.res = { json: { users: result.rows } };
};
```

---

## Next Steps

Continue to [Azure Functions Testing](./11-azure-functions-testing.md)